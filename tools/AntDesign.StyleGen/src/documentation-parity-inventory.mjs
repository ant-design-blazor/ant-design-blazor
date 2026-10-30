#!/usr/bin/env node

import { createRequire } from 'node:module';
import { readFile, writeFile } from 'node:fs/promises';
import { resolve } from 'node:path';
import process from 'node:process';
import { compareDomSnapshots } from './dom-style-parity.mjs';

const EXCLUDED_REACT_ROUTES = new Set(['_util']);

function parseArgs(argv) {
  const result = {};
  for (let i = 0; i < argv.length; i += 1) {
    if (!argv[i].startsWith('--')) continue;
    const key = argv[i].slice(2);
    const next = argv[i + 1];
    if (!next || next.startsWith('--')) {
      result[key] = true;
      continue;
    }
    result[key] = next;
    i += 1;
  }
  return result;
}

export function normalizeDemoId(id = '') {
  return id.replace(/^components-/i, '').replace(/[^a-z0-9]/gi, '').toLowerCase();
}

export function isElementRenderedInCurrentState(element) {
  const view = element?.ownerDocument?.defaultView;
  if (!view) return true;
  for (let current = element; current; current = current.parentElement) {
    const style = view.getComputedStyle(current);
    if (style.display === 'none' || style.visibility === 'hidden' || style.visibility === 'collapse'
      || style.contentVisibility === 'hidden') return false;
  }
  return true;
}

function withLightTheme(url) {
  const target = new URL(url);
  if (!target.searchParams.has('theme')) target.searchParams.set('theme', 'light');
  return target.toString();
}

/** Count missing examples from React's names; Blazor-only examples never increase the deficit. */
export function compareReactDemoNames(reactDemos, blazorDemos) {
  const getName = (demo) => demo.name ?? demo.key ?? demo.id ?? '';
  const blazorNames = new Set(blazorDemos.map((demo) => normalizeDemoId(getName(demo))));
  const reactNames = new Set(reactDemos.map((demo) => normalizeDemoId(getName(demo))));
  return {
    matched: reactDemos.filter((demo) => blazorNames.has(normalizeDemoId(getName(demo)))),
    missingReact: reactDemos.filter((demo) => !blazorNames.has(normalizeDemoId(getName(demo)))),
    extraBlazor: blazorDemos.filter((demo) => !reactNames.has(normalizeDemoId(getName(demo)))),
  };
}

export function matchComponentRoutes(reactRoutes, blazorRoutes) {
  const normalize = (route) => route.slug.replaceAll('-', '').toLowerCase();
  const blazorBySlug = new Map(blazorRoutes.map((route) => [normalize(route), route]));
  const matched = [];
  const missing = [];
  const excluded = [];
  for (const route of reactRoutes) {
    if (EXCLUDED_REACT_ROUTES.has(route.slug)) {
      excluded.push(route);
      continue;
    }
    const blazor = blazorBySlug.get(normalize(route));
    if (blazor) matched.push({ react: route, blazor });
    else missing.push(route);
  }
  const reactSlugs = new Set(reactRoutes.map(normalize));
  const extra = blazorRoutes.filter((route) => !reactSlugs.has(normalize(route)));
  return { matched, missing, excluded, extra };
}

const REFERENCED_OVERLAY_ROOTS = [
  '.ant-select-dropdown', '.ant-cascader-dropdown', '.ant-picker-dropdown',
  '.ant-dropdown', '.ant-tooltip', '.ant-popover', '.ant-menu-submenu-popup',
  '.ant-table-filter-dropdown', '.ant-color-picker', '.ant-modal-root',
  '.ant-drawer', '.ant-tour', '.ant-image-preview-root',
].join(', ');

/** Resolve portaled overlays through the ARIA references of controls in one demo. */
export function getReferencedOverlayRoots(scope, documentRef = globalThis.document) {
  const roots = new Set();
  for (const trigger of scope.querySelectorAll('[aria-controls], [aria-describedby], [aria-owns]')) {
    const ids = ['aria-controls', 'aria-describedby', 'aria-owns']
      .flatMap((attribute) => (trigger.getAttribute(attribute) ?? '').split(/\s+/))
      .filter(Boolean);
    for (const id of ids) {
      const target = documentRef.getElementById(id);
      if (!target || scope.contains(target)) continue;
      const root = target.closest(REFERENCED_OVERLAY_ROOTS);
      if (root && !scope.contains(root)) roots.add(root);
    }
  }
  return [...roots];
}

async function readRoutes(page, url) {
  await page.goto(url, { waitUntil: 'domcontentloaded', timeout: 30000 });
  await waitForSettledPage(page);
  await page.waitForSelector('a[href]');
  return page.evaluate(() => {
    const prefix = location.pathname.includes('/en-US/') ? '/en-US/components/' : '/components/';
    const routes = new Map();
    for (const anchor of document.querySelectorAll('a[href]')) {
      const pathname = new URL(anchor.href).pathname;
      if (!pathname.startsWith(prefix)) continue;
      const slug = pathname.slice(prefix.length).split('/')[0];
      if (!slug || ['overview', 'changelog'].includes(slug)) continue;
      routes.set(slug, { slug, name: anchor.textContent.trim(), path: pathname });
    }
    return [...routes.values()];
  });
}

export async function waitForSettledPage(page) {
  // Dumi and Blazor may append demos after the initial document load. Waiting for
  // full network idle can hang for minutes on pages with long-lived requests,
  // so use a short idle window followed by bounded DOM stability sampling.
  await page.waitForNetworkIdle({ idleTime: 500, timeout: 2500 }).catch(() => {});
  await page.waitForFunction(() => {
    const count = document.querySelectorAll('.code-box-demo').length;
    const state = window.__parityInventoryState ??= { count, stable: 0 };
    if (state.count === count) state.stable += 1;
    else {
      state.count = count;
      state.stable = 0;
    }
    return state.stable >= 5;
  }, { timeout: 8000, polling: 250 }).catch(() => {});
}

export async function scrollToLoadAllDemos(page) {
  await page.evaluate(async () => {
    const pause = (duration) => new Promise((resolve) => setTimeout(resolve, duration));
    let previousHeight = 0;
    let stableBottomPasses = 0;

    // Dumi defers demo mounts until their code boxes approach the viewport.
    // Walk the page in viewport-sized increments so inventory does not mistake
    // unvisited, lazy-mounted React demos for missing Blazor documentation.
    for (let pass = 0; pass < 120; pass += 1) {
      const height = Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);
      const step = Math.max(Math.floor(window.innerHeight * 0.8), 400);
      window.scrollTo(0, Math.min(window.scrollY + step, height));
      await new Promise((resolve) => requestAnimationFrame(() => requestAnimationFrame(resolve)));
      await pause(80);

      const nextHeight = Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);
      const atBottom = window.scrollY + window.innerHeight >= nextHeight - 2;
      if (atBottom && nextHeight === previousHeight) stableBottomPasses += 1;
      else stableBottomPasses = 0;
      if (stableBottomPasses >= 2) break;
      previousHeight = nextHeight;
    }

    window.scrollTo(0, 0);
  });
}

async function readPageDemos(page, url) {
  const errors = [];
  const onPageError = (error) => errors.push(error.message);
  page.on('pageerror', onPageError);
  try {
    await page.goto(withLightTheme(url), { waitUntil: 'domcontentloaded', timeout: 30000 });
    await page.waitForSelector('.code-box-demo', { timeout: 20000 });
    await waitForSettledPage(page);
    // Blazor's documentation keeps debug examples hidden behind the page-level
    // Debug toggle. React's inventory contains corresponding source examples
    // such as `_semantic`; reveal Blazor's hidden examples before comparing
    // names or the scanner incorrectly reports them as missing.
    const debugToggle = await page.$('.docs-debug-toggle');
    if (debugToggle) {
      // Blazor Server does not guarantee a global `window.Blazor` object; the
      // rendered toggle can be clicked directly without waiting for it.
      await debugToggle.click();
      await page.waitForFunction(
        () => document.querySelector('.docs-debug-toggle')?.getAttribute('aria-pressed') === 'true',
        { timeout: 15000 },
      );
      await waitForSettledPage(page);
    }
    await scrollToLoadAllDemos(page);
    await waitForSettledPage(page);
    const demos = await page.evaluate((overlaySelector) => {
      const isRenderedInCurrentState = (element) => {
        // A closed Blazor overlay may remain mounted with display:none while
        // React does not create its portal until it opens. Treat that as an
        // interaction-state difference, not as a default rendered-style
        // difference. Hidden nodes are counted separately for diagnostics.
        for (let current = element; current; current = current.parentElement) {
          const style = getComputedStyle(current);
          if (style.display === 'none' || style.visibility === 'hidden' || style.visibility === 'collapse'
            || style.contentVisibility === 'hidden') return false;
        }
        return true;
      };
      const getReferencedOverlayRoots = (scope) => {
        const roots = new Set();
        for (const trigger of scope.querySelectorAll('[aria-controls], [aria-describedby], [aria-owns]')) {
          const ids = ['aria-controls', 'aria-describedby', 'aria-owns']
            .flatMap((attribute) => (trigger.getAttribute(attribute) ?? '').split(/\s+/))
            .filter(Boolean);
          for (const id of ids) {
            const target = document.getElementById(id);
            if (!target || scope.contains(target)) continue;
            const root = target.closest(overlaySelector);
            if (root && !scope.contains(root) && isRenderedInCurrentState(root)) roots.add(root);
          }
        }
        return [...roots];
      };
      const serializeRenderedSubtree = (element) => {
        const clone = element.cloneNode(true);
        const removeHiddenChildren = (source, copy) => {
          const sourceChildren = [...source.children];
          const copyChildren = [...copy.children];
          sourceChildren.forEach((child, childIndex) => {
            const copyChild = copyChildren[childIndex];
            if (!isRenderedInCurrentState(child)) copyChild?.remove();
            else if (copyChild) removeHiddenChildren(child, copyChild);
          });
        };
        removeHiddenChildren(element, clone);
        return clone.outerHTML;
      };
      return [...document.querySelectorAll('.code-box-demo')].map((scope, index) => {
      const owner = scope.closest('[id]');
      const counts = new Map();
      const allElements = [...scope.querySelectorAll('[class]')];
      const elements = allElements.filter(isRenderedInCurrentState);
      for (const element of elements) {
        for (const name of element.classList) {
          if (name.startsWith('ant-')) counts.set(name, (counts.get(name) ?? 0) + 1);
        }
      }
      const rootElements = elements.filter((element) => {
        if (![...element.classList].some((name) => name.startsWith('ant-'))) return false;
        return !element.parentElement?.closest('[class*="ant-"]')
          || !scope.contains(element.parentElement.closest('[class*="ant-"]'));
      });
      const referencedOverlayRoots = getReferencedOverlayRoots(scope)
        .filter((overlay) => !rootElements.some((root) => root.contains(overlay)));
      const componentElements = [...new Set([
        ...elements,
        ...referencedOverlayRoots.flatMap((overlay) => [overlay, ...overlay.querySelectorAll('[class]')]
          .filter(isRenderedInCurrentState)),
      ])];
      const roots = rootElements.slice(0, 20).map((element) => ({
        tag: element.tagName.toLowerCase(),
        classes: [...element.classList].filter((name) => name.startsWith('ant-')),
      }));
      const scopeRect = scope.getBoundingClientRect();
      const componentNodes = componentElements.filter((element) => [...element.classList].some((name) => name.startsWith('ant-'))).map((element) => {
        const computed = getComputedStyle(element);
        const fixedProperties = [
          'display', 'position', 'box-sizing', 'width', 'height', 'min-width', 'max-width', 'min-height', 'max-height',
          'padding-block-start', 'padding-block-end', 'padding-inline-start', 'padding-inline-end',
          'margin-block-start', 'margin-block-end', 'margin-inline-start', 'margin-inline-end', 'gap',
          'align-items', 'justify-content', 'font-family', 'font-size', 'font-weight', 'line-height', 'color',
          'background-color', 'border-top-width', 'border-right-width', 'border-bottom-width', 'border-left-width',
          'border-top-style', 'border-right-style', 'border-bottom-style', 'border-left-style',
          'border-top-color', 'border-right-color', 'border-bottom-color', 'border-left-color',
          'border-radius', 'box-shadow', 'opacity', 'overflow', 'overflow-x', 'overflow-y', 'transform', 'z-index',
          'flex-direction', 'flex-wrap', 'flex-grow', 'flex-shrink', 'grid-template-columns', 'vertical-align'
        ];
        const styles = Object.fromEntries(fixedProperties.map((property) => [property, computed.getPropertyValue(property).trim()]));
        const rect = element.getBoundingClientRect();
        return {
          tag: element.tagName.toLowerCase(),
          classes: [...element.classList].filter((name) => name.startsWith('ant-')).sort(),
          primaryClass: [...element.classList].find((name) => name.startsWith('ant-')) ?? '',
          styles,
          geometry: {
            left: rect.left - scopeRect.left, top: rect.top - scopeRect.top,
            width: rect.width, height: rect.height,
          },
        };
      });
      return {
        index,
        id: owner?.id ?? '',
        key: owner?.id ? owner.id.split('-demo-').slice(1).join('-demo-') : `index-${index}`,
        name: owner?.id ? owner.id.split('-demo-').slice(1).join('-demo-') : `index-${index}`,
        elementCount: elements.length,
        hiddenComponentNodeCount: allElements.filter((element) =>
          [...element.classList].some((name) => name.startsWith('ant-')) && !isRenderedInCurrentState(element),
        ).length,
        rootCandidates: roots,
        componentClasses: [...counts.entries()].sort((a, b) => b[1] - a[1]).slice(0, 12).map(([name, count]) => ({ name, count })),
        html: [...rootElements, ...referencedOverlayRoots].map(serializeRenderedSubtree).join('\n'),
        componentNodes,
      };
      });
    }, REFERENCED_OVERLAY_ROOTS);
    return { demos, errors };
  } catch (error) {
    return { demos: [], errors: [...errors, String(error?.message ?? error).split('\n')[0]] };
  } finally {
    page.off('pageerror', onPageError);
  }
}

export async function withIsolatedPage(browser, action) {
  // Keep a hydration or navigation failure isolated to one route. Reusing a
  // page whose main frame has detached makes every later component look like
  // it has zero demos, obscuring the actual per-route blocker.
  const page = await browser.newPage();
  try {
    return await action(page);
  } finally {
    if (!page.isClosed()) await page.close();
  }
}

async function readRouteDemos(browser, url) {
  let result;
  for (let attempt = 0; attempt < 2; attempt += 1) {
    result = await withIsolatedPage(browser, async (page) => {
      await page.setViewport({ width: 1280, height: 900, deviceScaleFactor: 1 });
      return readPageDemos(page, url);
    });
    // React/Dumi's lazy route chunks sometimes leave a fresh page blank while
    // the dev server is busy. Retry only pages that yielded no demo DOM; keep
    // component page errors alongside valid captures as separate diagnostics.
    if (result.demos.length > 0 || attempt === 1) break;
  }
  return result;
}

const HTML_NORMALIZATION = {
  runtimeIdPattern: '^(?:ant-blazor-[a-zA-Z0-9_-]+|_[rR]_[a-zA-Z0-9_-]+)$',
  removeUnreferencedRuntimeIds: true,
  removeAttributes: ['_bl_*'],
  removeClassPatterns: ['^css-dev-only-do-not-override-', '^css-var-_[rR]_', '^css-[a-zA-Z0-9]+$', '^acss-[a-zA-Z0-9_-]+$'],
  removeCommentMarkers: ['<!--!-->'],
  sortClassTokens: true,
};

export function compareRuntimeDemo(reactDemo, blazorDemo) {
  const createNodeMap = (nodes) => {
    const occurrences = new Map();
    return new Map(nodes.map((node) => {
      // Match by the full Ant class set. The first class is not stable across
      // wrappers (for example css-var-root may be appended on one runtime),
      // and occurrence-based matching otherwise pairs unrelated nodes.
      const signature = `${node.tag}|${node.classes.join('.')}`;
      const occurrence = occurrences.get(signature) ?? 0;
      occurrences.set(signature, occurrence + 1);
      return [`${signature}[${occurrence}]`, node];
    }));
  };
  const reactNodes = createNodeMap(reactDemo.componentNodes);
  const blazorNodes = createNodeMap(blazorDemo.componentNodes);
  const nodeKeys = [...new Set([...reactNodes.keys(), ...blazorNodes.keys()])].sort();
  const commonNodeKeys = nodeKeys.filter((key) => reactNodes.has(key) && blazorNodes.has(key));
  const expectedStyles = Object.fromEntries(commonNodeKeys.map((key) => [key, reactNodes.get(key).styles]));
  const actualStyles = Object.fromEntries(commonNodeKeys.map((key) => [key, blazorNodes.get(key).styles]));
  const semanticNodes = {
    componentNodes: commonNodeKeys.map((key) => {
      const node = reactNodes.get(key);
      return { html: `<${node.tag} class=\"${node.classes.join(' ')}\"></${node.tag}>`, computedStyle: node.styles, relativeGeometry: node.geometry };
    }),
  };
  const actualSemanticNodes = {
    componentNodes: commonNodeKeys.map((key) => {
      const node = blazorNodes.get(key);
      return { html: `<${node.tag} class=\"${node.classes.join(' ')}\"></${node.tag}>`, computedStyle: node.styles, relativeGeometry: node.geometry };
    }),
  };
  const geometryDifferences = [];
  for (const key of commonNodeKeys) {
    const left = reactNodes.get(key).geometry;
    const right = blazorNodes.get(key).geometry;
    for (const property of ['left', 'top', 'width', 'height']) {
      if (Math.abs(left[property] - right[property]) > 1) {
        geometryDifferences.push({ key, property, react: left[property], blazor: right[property], tolerancePx: 1 });
      }
    }
  }
  const comparison = compareDomSnapshots(
    { html: reactDemo.html, semantic: semanticNodes, computedStyles: expectedStyles },
    { html: blazorDemo.html, semantic: actualSemanticNodes, computedStyles: actualStyles },
    { normalization: HTML_NORMALIZATION, geometryTolerancePx: 1 },
  );
  return {
    html: {
      equal: comparison.domTree.equal,
      differenceCount: comparison.domTree.differences.length,
      samples: comparison.domTree.differences.slice(0, 5),
    },
    computedStyles: {
      equal: comparison.computedStyles.equal,
      differenceCount: comparison.computedStyles.differences.length,
      samples: comparison.computedStyles.differences.slice(0, 12),
    },
    componentNodeCount: { react: reactDemo.componentNodes.length, blazor: blazorDemo.componentNodes.length },
    unmatchedComponentNodeKeys: {
      react: [...reactNodes.keys()].filter((key) => !blazorNodes.has(key)),
      blazor: [...blazorNodes.keys()].filter((key) => !reactNodes.has(key)),
    },
    geometry: {
      equal: geometryDifferences.length === 0,
      differenceCount: geometryDifferences.length,
      samples: geometryDifferences.slice(0, 8),
    },
  };
}

function createInventoryReport(reactRoutes, blazorRoutes, pairs, components, reactBaseUrl, blazorBaseUrl, missingReactRouteDemos = []) {
  return {
    generatedAt: new Date().toISOString(),
    bases: { react: reactBaseUrl, blazor: blazorBaseUrl },
    missingReactRouteDemos,
    summary: {
      reactRoutes: reactRoutes.length,
      blazorRoutes: blazorRoutes.length,
      matchedComponents: pairs.matched.length,
      missingBlazorRoutes: pairs.missing,
      excludedReactRoutes: pairs.excluded,
      extraBlazorRoutes: pairs.extra,
      completedComponents: components.length,
      reactDemos: components.reduce((total, item) => total + item.demos.react, 0),
      blazorDemos: components.reduce((total, item) => total + item.demos.blazor, 0),
      matchedDemos: components.reduce((total, item) => total + item.demos.matched, 0),
      missingReactDemos: components.reduce((total, item) => total + item.demos.missingReactNames.length, 0),
      extraBlazorDemos: components.reduce((total, item) => total + item.demos.extraBlazorNames.length, 0),
      blockedPages: components.filter((item) => item.blockers.react.length || item.blockers.blazor.length).length,
      runtimeHtmlDifferences: components.reduce((total, item) => total + item.demos.matchedDemos.filter((demo) => !demo.runtime.html.equal).length, 0),
      runtimeStyleDifferences: components.reduce((total, item) => total + item.demos.matchedDemos.filter((demo) => !demo.runtime.computedStyles.equal).length, 0),
      runtimeGeometryDifferences: components.reduce((total, item) => total + item.demos.matchedDemos.filter((demo) => !demo.runtime.geometry.equal).length, 0),
    },
    components,
  };
}

async function main() {
  const args = parseArgs(process.argv.slice(2));
  const reactRoot = resolve(args['antd-path'] ?? process.cwd());
  const requireFromReact = createRequire(resolve(reactRoot, 'package.json'));
  const puppeteer = requireFromReact('puppeteer');
  const reactBaseUrl = args['react-base-url'] ?? 'http://127.0.0.1:8000';
  const blazorBaseUrl = args['blazor-base-url'] ?? 'http://127.0.0.1:8186';
  const launchBrowser = () => puppeteer.launch({
    headless: true,
    executablePath: args.chrome,
    args: ['--no-sandbox', '--disable-dev-shm-usage', '--disable-gpu'],
  });
  let browser = await launchBrowser();
  try {
    const routePage = await browser.newPage();
    // `/components` is a 404 on the React Dumi site; use the actual overview
    // page so route discovery runs against a rendered component catalog.
    const reactRoutes = await readRoutes(routePage, withLightTheme(`${reactBaseUrl}/components/overview`));
    const blazorRoutes = await readRoutes(routePage, withLightTheme(`${blazorBaseUrl}/en-US/components/overview`));
    await routePage.close();
    const pairs = matchComponentRoutes(reactRoutes, blazorRoutes);
    const missingReactRouteDemos = [];
    for (const route of pairs.missing) {
      const react = await readRouteDemos(browser, `${reactBaseUrl}${route.path}`);
      missingReactRouteDemos.push({
        name: route.name,
        demos: react.demos.map((demo) => demo.name),
        blockers: react.errors,
      });
    }
    const onlySlugs = args.only ? new Set(args.only.split(',').map(normalizeDemoId)) : null;
    const selectedPairs = onlySlugs
      ? pairs.matched.filter((pair) => onlySlugs.has(normalizeDemoId(pair.react.slug)))
      : pairs.matched;
    let resumedComponents = [];
    if (args.resume && args.report) {
      try {
        const previous = JSON.parse(await readFile(resolve(args.report), 'utf8'));
        resumedComponents = previous.components ?? [];
      } catch {
        resumedComponents = [];
      }
    }
    const completedRoutes = new Set(resumedComponents.map((component) => component.reactRoute));
    const pendingPairs = selectedPairs.filter((pair) => !completedRoutes.has(pair.react.path));
    const components = [...resumedComponents];
    for (const [index, pair] of pendingPairs.entries()) {
      // Dumi pages are large; periodically reclaim Chromium memory while
      // retaining a JSON checkpoint after every completed component.
      if (index > 0 && index % 5 === 0) {
        await browser.close().catch(() => {});
        browser = await launchBrowser();
      }
      const react = await readRouteDemos(browser, `${reactBaseUrl}${pair.react.path}`);
      const blazor = await readRouteDemos(browser, `${blazorBaseUrl}${pair.blazor.path}`);
      const demoComparison = compareReactDemoNames(react.demos, blazor.demos);
      const matchedDemos = demoComparison.matched.map((reactDemo) => {
        const blazorDemo = blazor.demos.find((demo) => normalizeDemoId(demo.name) === normalizeDemoId(reactDemo.name));
        return {
          react: { id: reactDemo.id, name: reactDemo.name, elementCount: reactDemo.elementCount, hiddenComponentNodeCount: reactDemo.hiddenComponentNodeCount, rootCandidates: reactDemo.rootCandidates },
          blazor: { id: blazorDemo.id, name: blazorDemo.name, elementCount: blazorDemo.elementCount, hiddenComponentNodeCount: blazorDemo.hiddenComponentNodeCount, rootCandidates: blazorDemo.rootCandidates },
          runtime: compareRuntimeDemo(reactDemo, blazorDemo),
        };
      });
      const missingDemos = demoComparison.missingReact;
      components.push({
        name: pair.react.name,
        reactRoute: pair.react.path,
        blazorRoute: pair.blazor.path,
        demos: {
          react: react.demos.length,
          blazor: blazor.demos.length,
          matched: matchedDemos.length,
          reactNames: react.demos.map((demo) => demo.name),
          blazorNames: blazor.demos.map((demo) => demo.name),
          missingReactNames: missingDemos.map((demo) => demo.name),
          extraBlazorNames: demoComparison.extraBlazor.map((demo) => demo.name),
          matchedDemos,
        },
        blockers: { react: react.errors, blazor: blazor.errors },
      });
      process.stdout.write(`[${components.length}/${selectedPairs.length}] ${pair.react.slug}: React ${react.demos.length}, Blazor ${blazor.demos.length}, matched ${matchedDemos.length}\n`);
      if (args.report) {
        const checkpoint = createInventoryReport(reactRoutes, blazorRoutes, pairs, components, reactBaseUrl, blazorBaseUrl, missingReactRouteDemos);
        await writeFile(resolve(args.report), `${JSON.stringify(checkpoint, null, 2)}\n`);
      }
      globalThis.gc?.();
    }
    const report = createInventoryReport(reactRoutes, blazorRoutes, pairs, components, reactBaseUrl, blazorBaseUrl, missingReactRouteDemos);
    if (args.report) await writeFile(resolve(args.report), `${JSON.stringify(report, null, 2)}\n`);
    process.stdout.write(`${JSON.stringify({ report: args.report ?? null, ...report.summary }, null, 2)}\n`);
  } finally {
    await browser.close().catch(() => {});
  }
}

if (import.meta.url === `file://${process.argv[1]}`) {
  main().catch((error) => {
    console.error(error);
    process.exitCode = 1;
  });
}
