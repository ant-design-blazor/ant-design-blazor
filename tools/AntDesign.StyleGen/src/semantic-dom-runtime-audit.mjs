#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { createRequire } from 'node:module';
import { pathToFileURL } from 'node:url';

const DEFAULT_TIMEOUT = 12000;
const DEFERRED_COMPONENTS = new Set(['float-button', 'masonry', 'tour']);
const PORTAL_TRIGGERS = {
  dropdown: { selector: '.ant-dropdown-trigger', event: 'hover' },
  message: { text: 'Display normal message', event: 'click' },
  modal: { text: 'Open Modal', event: 'click' },
  notification: { text: 'Open the notification box', event: 'click' },
  popconfirm: { text: 'Delete', event: 'click' },
  popover: { text: 'Hover me', event: 'hover' },
  tooltip: { text: 'Tooltip will show on mouse enter.', event: 'hover' },
};

function parseArgs(argv) {
  const args = {};
  for (let i = 0; i < argv.length; i += 1) {
    if (argv[i].startsWith('--')) args[argv[i].slice(2)] = argv[++i];
  }
  return args;
}

export function parseSemanticSelectorCatalog(source) {
  const catalogJson = typeof source === 'string' ? JSON.parse(source) : source;
  const catalog = new Map();
  for (const component of catalogJson.components ?? []) {
    catalog.set(component.name, (component.nodes ?? []).map(({ name, selector }) => ({ name, selector })));
  }
  return catalog;
}

export const getEffectiveSemanticCatalog = parseSemanticSelectorCatalog;

export function normalizeSemanticRouteKey(value) {
  return value.toLowerCase().replace(/[^a-z0-9]/g, '');
}

function flattenMenu(items) {
  return items.flatMap((item) => [item, ...flattenMenu(item.Children ?? [])]);
}

export function matchSemanticRoutes(catalog, menuItems) {
  const routes = flattenMenu(menuItems).filter((item) => item.Url?.startsWith('components/'));
  const routeByKey = new Map();
  for (const item of routes) {
    for (const key of [item.Url.split('/').at(-1), item.Title]) {
      const normalized = normalizeSemanticRouteKey(key);
      if (!routeByKey.has(normalized)) routeByKey.set(normalized, item);
    }
  }

  const aliases = { listy: ['list'] };
  const matched = [];
  const missing = [];
  const excluded = [];
  for (const [component, nodes] of catalog) {
    if (!nodes.length) continue;
    const candidates = [component, ...(aliases[component] ?? [])];
    const route = candidates.map(normalizeSemanticRouteKey).map((key) => routeByKey.get(key)).find(Boolean);
    if (route) matched.push({ component, nodes, route: route.Url, title: route.Title });
    else if (DEFERRED_COMPONENTS.has(component)) excluded.push({ component, nodes: nodes.length, reason: 'Deferred component; no docs route in the current scope' });
    else missing.push({ component, nodes: nodes.length });
  }
  return { matched, missing, excluded };
}

async function inspectComponent(page, baseUrl, component, timeout) {
  const result = {
    component: component.component,
    route: component.route,
    expectedNodes: component.nodes.length,
    renderedNodes: 0,
    matchedNodes: [],
    absentFromDemo: [],
    selectorErrors: [],
    highlight: null,
    portalHighlight: null,
    portalNodes: null,
    error: null,
  };

  try {
    await page.goto(`${baseUrl}/en-US/${component.route}`, { waitUntil: 'domcontentloaded', timeout: timeout * 2 });
    await page.waitForFunction(({ count, title }) =>
      document.title.toLowerCase().startsWith(`${title.toLowerCase()} -`)
      && document.querySelectorAll('.adb-semantic-preview-item').length === count,
    { timeout }, { count: component.nodes.length, title: component.title });
    await page.waitForNetworkIdle({ idleTime: 500, timeout: 2000 }).catch(() => {});
    await page.waitForFunction(() =>
      document.querySelector('.adb-semantic-preview-content')?.querySelector('*'),
    { timeout });
    await page.waitForFunction(() => {
      const content = document.querySelector('.adb-semantic-preview-content');
      const count = content?.querySelectorAll('*').length ?? 0;
      const state = window.__semanticPreviewAuditState ??= { count, stableSamples: 0 };
      if (state.count === count) state.stableSamples += 1;
      else {
        state.count = count;
        state.stableSamples = 0;
      }
      return count > 0 && state.stableSamples >= 4;
    }, { timeout, polling: 200 });

    result.renderedNodes = component.nodes.length;
    const inspected = await page.evaluate((nodes) => {
      const content = document.querySelector('.adb-semantic-preview-content');
      return nodes.map((node, index) => {
        try {
          return {
            index,
            name: node.name,
            selector: node.selector,
            count: content?.querySelectorAll(node.selector).length ?? 0,
            documentCount: document.querySelectorAll(node.selector).length,
          };
        } catch (error) {
          return { index, name: node.name, selector: node.selector, error: error.message };
        }
      });
    }, component.nodes);

    for (const node of inspected) {
      if (node.error) result.selectorErrors.push({ name: node.name, selector: node.selector, error: node.error });
      else if (node.count > 0) result.matchedNodes.push({ name: node.name, selector: node.selector, count: node.count });
      else result.absentFromDemo.push({
        name: node.name,
        selector: node.selector,
        presentOutsidePreview: node.documentCount > 0,
      });
    }

    const sample = result.matchedNodes[0];
    if (sample) {
      const index = component.nodes.findIndex((node) => node.name === sample.name);
      const activeKey = `${component.component}-${sample.name}`;
      result.highlight = { node: sample.name, activeKey, passed: false, error: null };
      try {
        await page.mouse.move(0, 0);
        await page.hover(`.adb-semantic-preview-item:nth-child(${index + 1})`);
        await page.waitForFunction((key) =>
          document.querySelector('.adb-semantic-preview-root')?.getAttribute('data-active-semantic') === key,
        { timeout }, activeKey);
        await page.waitForFunction((selector) => {
          const target = document.querySelector('.adb-semantic-preview-content')?.querySelector(selector);
          const style = target && getComputedStyle(target);
          return style?.outlineStyle === 'solid' && style.outlineWidth === '2px';
        }, { timeout }, sample.selector);
        const style = await page.evaluate((selector) => {
          const target = document.querySelector('.adb-semantic-preview-content')?.querySelector(selector);
          if (!target) return null;
          const computed = getComputedStyle(target);
          return {
            outline: computed.outline,
            outlineStyle: computed.outlineStyle,
            outlineWidth: computed.outlineWidth,
            boxShadow: computed.boxShadow,
          };
        }, sample.selector);
        result.highlight = {
          node: sample.name,
          activeKey,
          ...style,
          passed: style?.outlineStyle === 'solid' && style?.outlineWidth === '2px',
          error: null,
        };
      } catch (error) {
        result.highlight.error = String(error?.message ?? error).split('\n')[0];
      }
    }

    const portalTrigger = PORTAL_TRIGGERS[component.component];
    if (portalTrigger) {
      result.portalHighlight = { passed: false, trigger: portalTrigger.text ?? portalTrigger.selector, error: null };
      let portalStage = 'locating interaction trigger';
      try {
        const triggerBox = await page.evaluate(({ selector, text, component: key }) => {
          const content = document.querySelector('.adb-semantic-preview-content');
          if (!content) return null;
          const element = selector ? content.querySelector(selector)
            : [...content.querySelectorAll('button,a,[role="button"],span')]
              .find((candidate) => candidate.textContent.trim().includes(text));
          if (!element) return null;
          element.setAttribute('data-semantic-audit-trigger', key);
          const rect = element.getBoundingClientRect();
          document.documentElement.style.setProperty('scroll-behavior', 'auto', 'important');
          document.documentElement.scrollTo({
            top: Math.max(0, document.documentElement.scrollTop + rect.top - window.innerHeight / 2),
            behavior: 'instant',
          });
          const visibleRect = element.getBoundingClientRect();
          return { x: visibleRect.left + visibleRect.width / 2, y: visibleRect.top + visibleRect.height / 2 };
        }, { ...portalTrigger, component: component.component });
        if (!triggerBox) throw new Error('No portal interaction trigger in the first demo');
        portalStage = 'dispatching the interaction';
        await page.mouse.move(0, 0);
        await page.mouse.move(triggerBox.x, triggerBox.y);
        if (portalTrigger.event === 'click') await page.mouse.click(triggerBox.x, triggerBox.y);

        portalStage = 'waiting for a portal selector';
        await page.waitForFunction((nodes) => {
          const content = document.querySelector('.adb-semantic-preview-content');
          const index = nodes.findIndex((node) => {
            try {
              return [...document.querySelectorAll(node.selector)].some((element) => !content?.contains(element));
            } catch { return false; }
          });
          return index >= 0 ? index + 1 : false;
        }, { timeout }, component.nodes);
        const portalNodes = await page.evaluate((nodes) => {
          const content = document.querySelector('.adb-semantic-preview-content');
          return nodes.map((node) => {
            try {
              const count = [...document.querySelectorAll(node.selector)]
                .filter((element) => !content?.contains(element)).length;
              return { ...node, count };
            } catch (error) {
              return { ...node, count: 0, error: error.message };
            }
          });
        }, component.nodes);
        const presentPortalNodes = portalNodes.filter((node) => node.count > 0);
        result.portalNodes = {
          expected: component.nodes.length,
          matched: presentPortalNodes.map(({ name, selector, count }) => ({ name, selector, count })),
          absent: portalNodes.filter((node) => node.count === 0).map(({ name, selector, error }) => ({ name, selector, error })),
          highlighted: [],
          highlightErrors: [],
        };
        for (const node of presentPortalNodes) {
          const activeKey = `${component.component}-${node.name}`;
          portalStage = `activating ${node.name}`;
          await page.evaluate((name) => {
            [...document.querySelectorAll('.adb-semantic-preview-item')]
              .find((element) => element.querySelector('strong')?.textContent === name)?.click();
          }, node.name);
          await page.waitForFunction((key) =>
            document.querySelector('.adb-semantic-preview-root')?.getAttribute('data-active-semantic') === key,
          { timeout }, activeKey);
          portalStage = `waiting for the ${node.name} portal highlight`;
          await page.waitForFunction((selector) => {
            const content = document.querySelector('.adb-semantic-preview-content');
            const target = [...document.querySelectorAll(selector)].find((element) => !content?.contains(element));
            const style = target && getComputedStyle(target);
            return style?.outlineStyle === 'solid' && style.outlineWidth === '2px';
          }, { timeout }, node.selector);
          result.portalNodes.highlighted.push(node.name);
        }
        result.portalHighlight = {
          node: result.portalNodes.highlighted[0] ?? null,
          passed: result.portalNodes.highlighted.length === presentPortalNodes.length && presentPortalNodes.length > 0,
          error: null,
        };
      } catch (error) {
        result.portalHighlight.error = `${portalStage}: ${String(error?.message ?? error).split('\n')[0]}`;
        result.portalNodes ??= { expected: component.nodes.length, matched: [], absent: [], highlighted: [], highlightErrors: [] };
        result.portalNodes.highlightErrors.push(result.portalHighlight.error);
      }
    }
  } catch (error) {
    result.error = String(error?.message ?? error).split('\n')[0];
  }
  return result;
}

export async function runSemanticDomRuntimeAudit({
  repoRoot = process.cwd(),
  antdPath = path.resolve(repoRoot, '../ant-design'),
  baseUrl = 'http://127.0.0.1:8186',
  chrome,
  timeout = DEFAULT_TIMEOUT,
  limit = Number.POSITIVE_INFINITY,
  components: componentFilter,
} = {}) {
  const catalogSource = fs.readFileSync(path.join(repoRoot, 'docs/antd-v6/semantic-dom-catalog.json'), 'utf8');
  const catalog = getEffectiveSemanticCatalog(catalogSource);
  const response = await fetch(`${baseUrl}/_content/AntDesign.Docs/meta/menu.en-US.json`);
  if (!response.ok) throw new Error(`Cannot read Blazor docs menu (${response.status}) from ${baseUrl}`);
  const { matched, missing, excluded } = matchSemanticRoutes(catalog, await response.json());
  const selected = componentFilter?.length
    ? matched.filter((component) => componentFilter.includes(component.component))
    : matched;
  const targets = selected.slice(0, limit);
  const requireFromAntd = createRequire(path.join(antdPath, 'package.json'));
  const puppeteer = requireFromAntd('puppeteer');
  const browser = await puppeteer.launch({
    ...(chrome ? { executablePath: chrome } : {}),
    headless: true,
    args: ['--no-sandbox', '--disable-dev-shm-usage'],
  });

  const results = [];
  try {
    const page = await browser.newPage();
    await page.setViewport({ width: 1365, height: 900 });
    for (const component of targets) {
      const result = await inspectComponent(page, baseUrl, component, timeout);
      results.push(result);
      const ordinaryHighlight = result.highlight?.passed
        ? 'in-preview highlight ok'
        : result.highlight
          ? `in-preview highlight failed (${result.highlight.error ?? 'style mismatch'})`
          : 'no initial-demo node to highlight';
      const portalHighlight = result.portalHighlight
        ? `; portal highlight ${result.portalHighlight.passed ? `ok (${result.portalHighlight.node})` : `failed (${result.portalHighlight.error})`}`
        : '';
      process.stdout.write(`[${results.length}/${targets.length}] ${component.component}: ${result.matchedNodes.length}/${result.expectedNodes} initial selectors; ${ordinaryHighlight}${portalHighlight}${result.error ? `; ${result.error}` : ''}\n`);
    }
  } finally {
    await browser.close();
  }

  const flatNodes = results.flatMap((result) => [
    ...result.matchedNodes.map(() => 'matched'),
    ...result.absentFromDemo.map(() => 'absent'),
    ...result.selectorErrors.map(() => 'selectorError'),
  ]);
  return {
    schemaVersion: 1,
    generatedAt: new Date().toISOString(),
    baseUrl,
    summary: {
      catalogComponents: [...catalog.values()].filter((nodes) => nodes.length > 0).length,
      routedComponents: matched.length,
      auditedComponents: results.length,
      unroutedComponents: missing,
      outOfScopeComponents: excluded,
      semanticNodes: results.reduce((total, result) => total + result.expectedNodes, 0),
      presentInInitialDemo: flatNodes.filter((status) => status === 'matched').length,
      absentFromInitialDemo: flatNodes.filter((status) => status === 'absent').length,
      invalidSelectors: flatNodes.filter((status) => status === 'selectorError').length,
      highlightChecks: results.filter((result) => result.highlight).length,
      highlightsPassed: results.filter((result) => result.highlight?.passed).length,
      highlightErrors: results.filter((result) => result.highlight?.error).length,
      portalHighlightChecks: results.filter((result) => result.portalHighlight).length,
      portalHighlightsPassed: results.filter((result) => result.portalHighlight?.passed).length,
      portalHighlightErrors: results.filter((result) => result.portalHighlight?.error).length,
      portalSemanticSelectors: results.reduce((total, result) => total + (result.portalNodes?.expected ?? 0), 0),
      portalSemanticSelectorsMatched: results.reduce((total, result) => total + (result.portalNodes?.matched.length ?? 0), 0),
      portalSemanticHighlightsPassed: results.reduce((total, result) => total + (result.portalNodes?.highlighted.length ?? 0), 0),
      portalSemanticHighlightErrors: results.reduce((total, result) => total + (result.portalNodes?.highlightErrors.length ?? 0), 0),
      routeOrRenderErrors: results.filter((result) => result.error).length,
    },
    components: results,
  };
}

async function main() {
  const args = parseArgs(process.argv.slice(2));
  const repoRoot = path.resolve(args['repo-root'] ?? process.cwd());
  const report = await runSemanticDomRuntimeAudit({
    repoRoot,
    antdPath: path.resolve(args['antd-path'] ?? path.join(repoRoot, '../ant-design')),
    baseUrl: args['base-url'] ?? 'http://127.0.0.1:8186',
    chrome: args.chrome,
    timeout: Number(args.timeout ?? DEFAULT_TIMEOUT),
    limit: Number(args.limit ?? Number.POSITIVE_INFINITY),
    components: args.components?.split(',').map((component) => component.trim()).filter(Boolean),
  });
  const output = path.resolve(args.out ?? '/tmp/semantic-dom-runtime-audit.json');
  fs.mkdirSync(path.dirname(output), { recursive: true });
  fs.writeFileSync(output, `${JSON.stringify(report, null, 2)}\n`);
  process.stdout.write(`${JSON.stringify({ output, ...report.summary }, null, 2)}\n`);
  if (report.summary.invalidSelectors || report.summary.routeOrRenderErrors || report.summary.highlightErrors
    || report.summary.highlightsPassed !== report.summary.highlightChecks
    || report.summary.portalHighlightsPassed !== report.summary.portalHighlightChecks) process.exitCode = 1;
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) await main();
