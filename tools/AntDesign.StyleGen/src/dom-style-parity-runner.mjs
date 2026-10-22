#!/usr/bin/env node

import { createRequire } from 'node:module';
import { readFile, writeFile, mkdir } from 'node:fs/promises';
import { dirname, resolve } from 'node:path';
import process from 'node:process';
import { compareDomSnapshots } from './dom-style-parity.mjs';

const DEFAULT_STYLE_PROPERTIES = [
  'display', 'position', 'boxSizing', 'width', 'height', 'minHeight', 'maxHeight',
  'paddingBlock', 'paddingInline', 'marginBlock', 'gap', 'alignItems', 'lineHeight',
  'fontSize', 'fontWeight', 'color', 'backgroundColor', 'borderRadius',
  'borderTopWidth', 'borderRightWidth', 'borderBottomWidth', 'borderLeftWidth',
  '--ant-select-height', '--ant-select-multi-item-height', '--ant-select-multi-padding-vertical',
];
const DEFAULT_PSEUDO_STYLE_PROPERTIES = [
  'content', 'display', 'position', 'width', 'height', 'inset', 'transform',
  'backgroundColor', 'borderTopWidth', 'borderRightWidth', 'borderBottomWidth', 'borderLeftWidth',
];

function parseArgs(argv) {
  const values = {};
  for (let index = 0; index < argv.length; index += 1) {
    if (!argv[index].startsWith('--')) continue;
    values[argv[index].slice(2)] = argv[index + 1];
    index += 1;
  }
  return values;
}

function usage() {
  console.error('Usage: node dom-style-parity-runner.mjs --cases <manifest.json> --antd-path <react-repo> [--chrome <executable>] [--report <report.json>]');
}

function puppeteerFor(reactRoot) {
  const requireFromReact = createRequire(resolve(reactRoot, 'package.json'));
  return requireFromReact('puppeteer');
}

function safeError(error) {
  return String(error?.message ?? error).split('\n')[0];
}

async function navigate(page, url, side, timeoutMs) {
  const errors = [];
  page.on('pageerror', (error) => errors.push(`pageerror: ${safeError(error)}`));
  page.on('console', (message) => {
    if (message.type() === 'error') errors.push(`console: ${message.text().slice(0, 240)}`);
  });
  try {
    await page.goto(url, { waitUntil: 'domcontentloaded', timeout: timeoutMs });
    return { side, url, errors, loaded: true };
  } catch (error) {
    return { side, url, errors, loaded: false, blocker: safeError(error) };
  }
}

async function waitForDemo(page, sideSpec, timeoutMs, minimumRoots) {
  const scopeSelector = sideSpec.scopeSelector;
  const rootSelector = sideSpec.rootSelector;
  // Responsive components often finish layout-dependent rendering from
  // requestAnimationFrame/ResizeObserver. Chrome can throttle those callbacks
  // while a Puppeteer page is in the background, so activate each side before
  // deciding that its DOM is ready to compare.
  await page.bringToFront();
  await page.waitForSelector(scopeSelector, { timeout: timeoutMs });
  try {
    await page.waitForFunction(({ scopeSelector, rootSelector, skeletonSelector, minimumRoots }) => {
      const scope = document.querySelector(scopeSelector);
      return Boolean(scope)
        && (!skeletonSelector || scope.querySelectorAll(skeletonSelector).length === 0)
        && scope.querySelectorAll(rootSelector).length >= minimumRoots;
    }, { timeout: timeoutMs, polling: 100 }, {
      scopeSelector,
      rootSelector,
      skeletonSelector: sideSpec.skeletonSelector ?? '.ant-skeleton',
      minimumRoots,
    });
  } catch (error) {
    const state = await page.evaluate(({ scopeSelector, rootSelector, skeletonSelector }) => {
      const scope = document.querySelector(scopeSelector);
      return {
        title: document.title,
        skeletons: scope && skeletonSelector ? scope.querySelectorAll(skeletonSelector).length : null,
        roots: scope ? scope.querySelectorAll(rootSelector).length : null,
        blazorStarted: Boolean(window.Blazor),
      };
    }, { scopeSelector, rootSelector, skeletonSelector: sideSpec.skeletonSelector ?? '.ant-skeleton' });
    throw new Error(`${safeError(error)} (scope=${scopeSelector}, roots=${state.roots}, skeletons=${state.skeletons}, title=${JSON.stringify(state.title)}, blazor=${state.blazorStarted})`);
  }
  // A body-wide scope is common for portals, but scrolling body does not align
  // the component demo. Center the actual trigger so page layout/scroll offsets
  // do not masquerade as component popup-positioning differences.
  await page.evaluate(({ rootSelector, scopeSelector }) => {
    (document.querySelector(rootSelector) ?? document.querySelector(scopeSelector))
      ?.scrollIntoView({ block: 'center', inline: 'nearest' });
  }, { rootSelector, scopeSelector });
}

export async function applyAction(page, side, action, timeoutMs) {
  // CSS transitions and ResizeObserver callbacks are throttled in background
  // tabs. Activate the page before dispatching input so the resulting DOM and
  // computed styles are sampled from the same settled state.
  await page.bringToFront();
  const selector = action[`${side}Selector`] ?? action.selector;
  switch (action.type) {
    case 'click':
      if (action.force) {
        // Native radio inputs are visually hidden; activate the enclosing label so
        // both the browser's checked state and framework change handler run.
        await page.$eval(selector, (element) => (element.closest('label') ?? element).click());
      } else {
        await page.locator(selector).click({ timeout: timeoutMs });
      }
      if (action[`${side}CheckedValue`] !== undefined) {
        const expectedValue = action[`${side}CheckedValue`];
        await page.waitForFunction(({ selector, expectedValue }) => {
          const element = document.querySelector(selector);
          return element instanceof HTMLInputElement && element.checked && element.value === expectedValue;
        }, { timeout: timeoutMs }, { selector, expectedValue });
      }
      break;
    case 'hover':
      await page.locator(selector).hover({ timeout: timeoutMs });
      break;
    case 'waitForSelector':
      await page.waitForSelector(selector, { visible: action.state !== 'hidden', timeout: timeoutMs });
      break;
    case 'focus':
      await page.locator(selector).focus({ timeout: timeoutMs });
      break;
    case 'fill':
      await page.locator(selector).fill(action.value ?? '', { timeout: timeoutMs });
      break;
    case 'press':
      await page.locator(selector).focus({ timeout: timeoutMs });
      await page.keyboard.press(action.key);
      break;
    case 'wait':
      await new Promise((resolvePromise) => setTimeout(resolvePromise, action.milliseconds ?? 0));
      break;
    case 'viewport':
      await page.setViewport({
        width: action.width,
        height: action.height,
        deviceScaleFactor: action.deviceScaleFactor ?? 1,
      });
      break;
    default:
      throw new Error(`unsupported action type: ${action.type}`);
  }
}

async function hoverAndVerify(page, selector, timeoutMs) {
  await page.locator(selector).hover({ timeout: timeoutMs });
  const hovered = () => page.waitForFunction((target) => document.querySelector(target)?.matches(':hover') === true,
    { timeout: Math.min(timeoutMs, 2000), polling: 50 }, selector);
  try {
    await hovered();
  } catch {
    // Some multi-page browser runs reactivate a page without restoring its
    // pointer state. Move to the live target rect explicitly, then verify the
    // actual CSS :hover state before waiting for its portal.
    const rect = await page.$eval(selector, (element) => {
      const { x, y, width, height } = element.getBoundingClientRect();
      return { x, y, width, height };
    });
    await page.mouse.move(rect.x + rect.width / 2, rect.y + rect.height / 2);
    await hovered();
  }
}

export function getFinalHoverAction(actions = []) {
  const finalStateAction = [...actions].reverse().find(({ type }) => !['wait', 'waitForSelector'].includes(type));
  return finalStateAction?.type === 'hover' ? finalStateAction : null;
}

export function getPortalWaitTargets(semanticNodes = [], side) {
  const content = semanticNodes.find(({ path, portal, portalVisible, portalText }) =>
    path === 'content' && portal && portalVisible && portalText);
  return content ? [{
    selector: content[`${side}Selector`] ?? content.selector,
    text: content.portalText,
  }] : [];
}

async function waitForVisiblePortals(page, semanticNodes, side, timeoutMs, triggerSelector) {
  const targets = getPortalWaitTargets(semanticNodes, side);
  if (targets.length === 0) return;

  try {
    await page.waitForFunction((portalTargets) => portalTargets.every(({ selector, text }) => {
      const nodes = [...document.querySelectorAll(selector)];
      return nodes.some((element) => {
        const tooltipRoot = element.matches('.ant-tooltip') ? element : element.closest('.ant-tooltip');
        const style = tooltipRoot && getComputedStyle(tooltipRoot);
        const content = element.matches('[role="tooltip"]')
          ? element
          : tooltipRoot?.querySelector('[role="tooltip"]');
        return Boolean(style && style.display !== 'none' && style.visibility !== 'hidden'
          && Number(style.opacity) !== 0 && !tooltipRoot.classList.contains('ant-tooltip-hidden'))
          && (content?.textContent.trim() ?? tooltipRoot?.textContent.trim()) === text;
      });
    }), { timeout: timeoutMs, polling: 100 }, targets);
  } catch (error) {
    const state = await page.evaluate(({ portalTargets, triggerSelector: selector }) => ({
      trigger: (() => {
        const element = document.querySelector(selector);
        return element && { html: element.outerHTML, hovered: element.matches(':hover') };
      })(),
      portals: portalTargets.map(({ selector: portalSelector }) => ({
        selector: portalSelector,
        matches: [...document.querySelectorAll(portalSelector)].map((element) => {
          const root = element.matches('.ant-tooltip') ? element : element.closest('.ant-tooltip');
          const style = root && getComputedStyle(root);
          return { className: root?.className, display: style?.display, visibility: style?.visibility,
            text: element.textContent.trim().slice(0, 80), tooltipText: root?.querySelector('[role="tooltip"]')?.textContent.trim() };
        }).slice(-5),
      })),
    }), { portalTargets: targets, triggerSelector });
    throw new Error(`${safeError(error)} (${JSON.stringify(state)})`);
  }
}

async function capture(page, side, sideSpec, semanticNodes, styleProperties, pseudoElements, pseudoStyleProperties) {
  return page.evaluate(({ side, scopeSelector, rootSelector, semanticNodes, styleProperties, pseudoElements, pseudoStyleProperties }) => {
    const scope = document.querySelector(scopeSelector);
    if (!scope) throw new Error(`capture scope not found: ${scopeSelector}`);
    const roots = [...scope.querySelectorAll(rootSelector)];
    const readStyle = (element) => {
      const computed = getComputedStyle(element);
      return Object.fromEntries(styleProperties.map((property) => [property, computed.getPropertyValue(property.startsWith('--') ? property : property.replace(/[A-Z]/g, (letter) => `-${letter.toLowerCase()}`)).trim()]));
    };
    const readComputedStyle = (element) => {
      const styles = readStyle(element);
      for (const pseudo of pseudoElements) {
        const computed = getComputedStyle(element, pseudo);
        for (const property of pseudoStyleProperties) {
          const cssName = property.startsWith('--') ? property : property.replace(/[A-Z]/g, (letter) => `-${letter.toLowerCase()}`);
          styles[`${pseudo}.${property}`] = computed.getPropertyValue(cssName).trim();
        }
      }
      return styles;
    };
    const semantic = {};
    for (const definition of semanticNodes) {
      const selector = definition[`${side}Selector`] ?? definition.selector;
      const nodes = definition.portal
        ? [...document.querySelectorAll(selector)].filter((element) => {
          const tooltipRoot = element.matches('.ant-tooltip') ? element : element.closest('.ant-tooltip');
          if (definition.portalVisible) {
            const style = tooltipRoot && getComputedStyle(tooltipRoot);
            if (!style || style.display === 'none' || style.visibility === 'hidden'
              || Number(style.opacity) === 0 || tooltipRoot.classList.contains('ant-tooltip-hidden')) return false;
          }
          if (!definition.portalText) return true;
          const textNode = element.matches('[role="tooltip"]')
            ? element
            : tooltipRoot?.querySelector('[role="tooltip"]');
          return textNode?.textContent.trim() === definition.portalText;
        })
        : roots.flatMap((root) => selector === '$root'
          ? [root]
          : [...root.querySelectorAll(selector)]);
      semantic[definition.path] = nodes.map((element) => ({
        html: element.outerHTML,
        tag: element.tagName.toLowerCase(),
        computedStyle: readComputedStyle(element),
        ...(definition.portal && roots[0] ? (() => {
          const rect = element.getBoundingClientRect();
          const triggerRect = roots[0].getBoundingClientRect();
          return { relativeGeometry: {
            left: rect.left - triggerRect.left,
            top: rect.top - triggerRect.top,
            width: rect.width,
            height: rect.height,
          } };
        })() : {}),
      }));
    }
    const computedStyles = Object.fromEntries(roots.map((element, index) => [`root[${index}]`, readComputedStyle(element)]));
    return {
      html: roots.map((root) => root.outerHTML).join('\n'),
      semantic,
      computedStyles,
      rootCount: roots.length,
      environment: { viewportWidth: innerWidth, viewportHeight: innerHeight, devicePixelRatio, zoom: getComputedStyle(document.documentElement).zoom },
    };
  }, {
    side,
    scopeSelector: sideSpec.scopeSelector,
    rootSelector: sideSpec.rootSelector,
    semanticNodes,
    styleProperties,
    pseudoElements,
    pseudoStyleProperties,
  });
}

async function runGroup(browser, group, config) {
  const reactPage = await browser.newPage();
  const blazorPage = await browser.newPage();
  await Promise.all([
    reactPage.setViewport(config.viewport),
    blazorPage.setViewport(config.viewport),
  ]);
  if (config.colorScheme) {
    await Promise.all([
      reactPage.emulateMediaFeatures([{ name: 'prefers-color-scheme', value: config.colorScheme }]),
      blazorPage.emulateMediaFeatures([{ name: 'prefers-color-scheme', value: config.colorScheme }]),
    ]);
  }
  const navigationTimeoutMs = config.navigationTimeoutMs ?? 60000;
  const reactNavigation = await navigate(reactPage, group.react.url, 'react', navigationTimeoutMs);
  const blazorNavigation = await navigate(blazorPage, group.blazor.url, 'blazor', navigationTimeoutMs);
  const result = { id: group.id, component: group.component, scenarios: [] };
  const minimumRoots = Math.max(1, ...group.scenarios.map((scenario) => scenario.minimumRoots ?? 1));
  let readinessBlocker = [reactNavigation, blazorNavigation]
    .filter((navigation) => !navigation.loaded)
    .map((navigation) => `${navigation.side}: ${navigation.blocker}`)
    .join('; ');
  if (!readinessBlocker) {
    try {
      await waitForDemo(reactPage, group.react, config.timeoutMs, minimumRoots);
    } catch (error) {
      readinessBlocker = `react: ${safeError(error)}`;
    }
  if (!readinessBlocker) {
    try {
      await waitForDemo(blazorPage, group.blazor, config.timeoutMs, minimumRoots);
      } catch (error) {
        readinessBlocker = `blazor: ${safeError(error)}`;
      }
    }
  }

  if (!readinessBlocker && group.matchScopeWidth) {
    const sourceSide = group.matchScopeWidth;
    const targetSide = sourceSide === 'react' ? 'blazor' : 'react';
    const sourcePage = sourceSide === 'react' ? reactPage : blazorPage;
    const targetPage = targetSide === 'react' ? reactPage : blazorPage;
    const sourceSpec = group[sourceSide];
    const targetSpec = group[targetSide];
    const sourceWidth = await sourcePage.$eval(sourceSpec.scopeSelector,
      (element) => element.getBoundingClientRect().width);
    await targetPage.$eval(targetSpec.scopeSelector, (element, width) => {
      element.style.width = `${width}px`;
    }, sourceWidth);
  }

  for (const scenario of group.scenarios) {
    const scenarioResult = { id: scenario.id, status: 'failed', actions: [] };
    let activeStage = 'scenario setup';
    if (readinessBlocker) {
      scenarioResult.status = 'blocked';
      scenarioResult.blocker = readinessBlocker;
      result.scenarios.push(scenarioResult);
      continue;
    }
    try {
      await Promise.all([
        reactPage.setViewport(config.viewport),
        blazorPage.setViewport(config.viewport),
      ]);
      for (const action of scenario.actions ?? []) {
        activeStage = `${action.type} action`;
        await applyAction(reactPage, 'react', action, config.timeoutMs);
        await applyAction(blazorPage, 'blazor', action, config.timeoutMs);
        if (action.stableForMs) {
          await new Promise((resolvePromise) => setTimeout(resolvePromise, action.stableForMs));
        }
        scenarioResult.actions.push(action.id ?? action.type);
      }
      // Capture each app while it is active. React's CSS-in-JS size changes
      // transition for 200ms; if captured in the background, the DOM class and
      // CSS variables can be updated while the computed box remains stale.
      const finalHover = getFinalHoverAction(scenario.actions);
      activeStage = 'React final hover and portal readiness';
      await reactPage.bringToFront();
      if (finalHover) {
        const selector = finalHover.reactSelector ?? finalHover.selector;
        // A page brought back to the foreground can retain Puppeteer's old
        // mouse coordinates without retaining :hover. Move away first so the
        // subsequent locator.hover always dispatches a fresh pointer move.
        await reactPage.mouse.move(0, 0);
        await new Promise((resolvePromise) => setTimeout(resolvePromise, 250));
        await hoverAndVerify(reactPage, selector, config.timeoutMs);
        await waitForVisiblePortals(reactPage, group.semanticNodes, 'react', config.timeoutMs, selector);
        scenarioResult.finalHoverState ??= {};
        scenarioResult.finalHoverState.react = await reactPage.$eval(selector, (element) => ({ html: element.outerHTML, hovered: element.matches(':hover') }));
      }
      await new Promise((resolvePromise) => setTimeout(resolvePromise, 800));
      activeStage = 'React DOM/CSS capture';
      const react = await capture(reactPage, 'react', group.react, group.semanticNodes,
        group.styleProperties ?? DEFAULT_STYLE_PROPERTIES,
        group.pseudoElements ?? ['::before', '::after'],
        group.pseudoStyleProperties ?? DEFAULT_PSEUDO_STYLE_PROPERTIES);
      await blazorPage.bringToFront();
      activeStage = 'Blazor final hover and portal readiness';
      if (finalHover) {
        const selector = finalHover.blazorSelector ?? finalHover.selector;
        await blazorPage.mouse.move(0, 0);
        await new Promise((resolvePromise) => setTimeout(resolvePromise, 250));
        await hoverAndVerify(blazorPage, selector, config.timeoutMs);
        await waitForVisiblePortals(blazorPage, group.semanticNodes, 'blazor', config.timeoutMs, selector);
        scenarioResult.finalHoverState ??= {};
        scenarioResult.finalHoverState.blazor = await blazorPage.$eval(selector, (element) => ({ html: element.outerHTML, hovered: element.matches(':hover') }));
      }
      await new Promise((resolvePromise) => setTimeout(resolvePromise, 800));
      activeStage = 'Blazor DOM/CSS capture';
      const blazor = await capture(blazorPage, 'blazor', group.blazor, group.semanticNodes,
        group.styleProperties ?? DEFAULT_STYLE_PROPERTIES,
        group.pseudoElements ?? ['::before', '::after'],
        group.pseudoStyleProperties ?? DEFAULT_PSEUDO_STYLE_PROPERTIES);
      const comparison = compareDomSnapshots(react, blazor, {
        ...config.comparisonPolicy,
        geometryTolerancePx: group.geometryTolerancePx ?? config.comparisonPolicy.geometryTolerancePx ?? 0,
        semanticMinimumCounts: Object.fromEntries((group.semanticNodes ?? []).map(({ path, minimumCount }) => [path, minimumCount ?? 0])),
      });
      scenarioResult.status = comparison.equal ? 'passed' : 'differences';
      scenarioResult.rootCount = { react: react.rootCount, blazor: blazor.rootCount };
      scenarioResult.comparison = comparison;
      scenarioResult.samples = { react, blazor };
    } catch (error) {
      scenarioResult.status = 'blocked';
      scenarioResult.blocker = `${activeStage}: ${safeError(error)}`;
    }
    result.scenarios.push(scenarioResult);
  }

  result.navigationErrors = { react: reactNavigation.errors, blazor: blazorNavigation.errors };
  await Promise.all([reactPage.close(), blazorPage.close()]);
  return result;
}

export async function runDomStyleParity(manifest, { puppeteer, executablePath } = {}) {
  validateParityManifest(manifest);
  const config = {
    ...manifest.config,
    timeoutMs: manifest.config?.timeoutMs ?? 20000,
    navigationTimeoutMs: manifest.config?.navigationTimeoutMs ?? 60000,
    viewport: manifest.config?.viewport ?? { width: 1280, height: 900, deviceScaleFactor: 1 },
    comparisonPolicy: manifest.config?.comparisonPolicy ?? {},
  };
  const browserType = puppeteer ?? puppeteerFor(manifest.reactRoot);
  const browser = await browserType.launch({
    ...(executablePath ? { executablePath } : {}),
    headless: true,
    args: ['--no-sandbox', '--disable-dev-shm-usage'],
  });
  try {
    const groups = [];
    for (const group of manifest.groups) groups.push(await runGroup(browser, group, config));
    const scenarios = groups.flatMap((group) => group.scenarios);
    return {
      schemaVersion: 1,
      generatedAt: new Date().toISOString(),
      upstream: manifest.upstream ?? null,
      summary: {
        groups: groups.length,
        scenarios: scenarios.length,
        passed: scenarios.filter(({ status }) => status === 'passed').length,
        differences: scenarios.filter(({ status }) => status === 'differences').length,
        blocked: scenarios.filter(({ status }) => status === 'blocked').length,
        visualStylePassed: scenarios.filter(({ comparison }) => comparison?.visualStyles?.equal).length,
        visualStyleDifferences: scenarios.filter(({ comparison }) => comparison && !comparison.visualStyles?.equal).length,
      },
      groups,
    };
  } finally {
    await browser.close();
  }
}

export function validateParityManifest(manifest) {
  if (manifest?.schemaVersion !== 1 || !Array.isArray(manifest.groups) || manifest.groups.length === 0) {
    throw new Error('scenario manifest must be schemaVersion 1 and contain at least one group');
  }
  for (const group of manifest.groups) {
    if (!group.id || !group.react?.url || !group.blazor?.url) {
      throw new Error('each group requires id and React/Blazor URLs');
    }
    for (const side of ['react', 'blazor']) {
      if (!group[side].scopeSelector || !group[side].rootSelector) {
        throw new Error(`${group.id}.${side} requires scopeSelector and rootSelector`);
      }
    }
    if (!Array.isArray(group.scenarios) || group.scenarios.length === 0) {
      throw new Error(`${group.id} must contain at least one scenario`);
    }
    if (group.matchScopeWidth !== undefined && !['react', 'blazor'].includes(group.matchScopeWidth)) {
      throw new Error(`${group.id}.matchScopeWidth must be react or blazor`);
    }
    const scenarioIds = new Set();
    for (const scenario of group.scenarios) {
      if (!scenario.id || scenarioIds.has(scenario.id)) throw new Error(`${group.id} has a missing or duplicate scenario id`);
      scenarioIds.add(scenario.id);
      for (const action of scenario.actions ?? []) {
        if (!['click', 'hover', 'focus', 'fill', 'press', 'wait', 'waitForSelector', 'viewport'].includes(action.type)) throw new Error(`${group.id}.${scenario.id} has unsupported action ${action.type}`);
        if (!['wait', 'viewport'].includes(action.type) && !(action.selector || action.reactSelector)) throw new Error(`${group.id}.${scenario.id} has an action without a React selector`);
        if (!['wait', 'viewport'].includes(action.type) && !(action.selector || action.blazorSelector)) throw new Error(`${group.id}.${scenario.id} has an action without a Blazor selector`);
      }
    }
    if (!Array.isArray(group.semanticNodes) || group.semanticNodes.length === 0) {
      throw new Error(`${group.id} must declare Semantic DOM paths`);
    }
    for (const node of group.semanticNodes) {
      if (!node.path) throw new Error(`${group.id} has a Semantic DOM node without a path`);
      for (const side of ['react', 'blazor']) {
        if (!(node[`${side}Selector`] ?? node.selector)) {
          throw new Error(`${group.id}.${node.path} requires a ${side} selector`);
        }
      }
      if (node.minimumCount !== undefined && (!Number.isInteger(node.minimumCount) || node.minimumCount < 0)) {
        throw new Error(`${group.id}.${node.path} has an invalid minimumCount`);
      }
    }
  }
  return manifest;
}

if (import.meta.url === `file://${process.argv[1]}`) {
  const args = parseArgs(process.argv.slice(2));
  if (!args.cases || !args['antd-path']) {
    usage();
    process.exit(2);
  }
  const manifestPath = resolve(args.cases);
  const manifest = JSON.parse(await readFile(manifestPath, 'utf8'));
  manifest.reactRoot = resolve(args['antd-path']);
  manifest.config ??= {};
  manifest.config.timeoutMs ??= 20000;
  if (args.timeout) manifest.config.timeoutMs = Number(args.timeout);
  manifest.config.viewport ??= { width: 1280, height: 900, deviceScaleFactor: 1 };
  const report = await runDomStyleParity(manifest, { executablePath: args.chrome ?? process.env.CHROME_BIN });
  const reportPath = args.report ? resolve(args.report) : null;
  if (reportPath) {
    await mkdir(dirname(reportPath), { recursive: true });
    await writeFile(reportPath, `${JSON.stringify(report, null, 2)}\n`, 'utf8');
  }
  console.log(JSON.stringify({ report: reportPath, ...report.summary }, null, 2));
  if (report.summary.differences || report.summary.blocked) process.exitCode = 1;
}
