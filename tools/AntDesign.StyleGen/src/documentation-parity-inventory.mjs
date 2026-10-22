#!/usr/bin/env node

import { createRequire } from 'node:module';
import { writeFile } from 'node:fs/promises';
import { resolve } from 'node:path';
import process from 'node:process';

const EXCLUDED_REACT_ROUTES = new Set(['float-button', 'masonry', 'tour', 'border-beam', '_util']);

function parseArgs(argv) {
  const result = {};
  for (let i = 0; i < argv.length; i += 1) {
    if (argv[i].startsWith('--')) result[argv[i].slice(2)] = argv[++i];
  }
  return result;
}

export function normalizeDemoId(id = '') {
  return id.replace(/^components-/i, '').replace(/[^a-z0-9]/gi, '').toLowerCase();
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
    await page.goto(url, { waitUntil: 'domcontentloaded', timeout: 30000 });
    await page.waitForSelector('.code-box-demo', { timeout: 20000 });
    await waitForSettledPage(page);
    await scrollToLoadAllDemos(page);
    await waitForSettledPage(page);
    const demos = await page.evaluate(() => [...document.querySelectorAll('.code-box-demo')].map((scope, index) => {
      const owner = scope.closest('[id]');
      const counts = new Map();
      const elements = [...scope.querySelectorAll('[class]')];
      for (const element of elements) {
        for (const name of element.classList) {
          if (name.startsWith('ant-')) counts.set(name, (counts.get(name) ?? 0) + 1);
        }
      }
      const roots = elements.filter((element) => {
        if (![...element.classList].some((name) => name.startsWith('ant-'))) return false;
        return !element.parentElement?.closest('[class*="ant-"]')
          || !scope.contains(element.parentElement.closest('[class*="ant-"]'));
      }).slice(0, 20).map((element) => ({
        tag: element.tagName.toLowerCase(),
        classes: [...element.classList].filter((name) => name.startsWith('ant-')),
      }));
      return {
        index,
        id: owner?.id ?? '',
        key: owner?.id ? owner.id.split('-demo-').slice(1).join('-demo-') : `index-${index}`,
        name: owner?.id ? owner.id.split('-demo-').slice(1).join('-demo-') : `index-${index}`,
        elementCount: elements.length,
        rootCandidates: roots,
        componentClasses: [...counts.entries()].sort((a, b) => b[1] - a[1]).slice(0, 12).map(([name, count]) => ({ name, count })),
      };
    }));
    return { demos, errors };
  } catch (error) {
    return { demos: [], errors: [...errors, String(error?.message ?? error).split('\n')[0]] };
  } finally {
    page.off('pageerror', onPageError);
  }
}

async function main() {
  const args = parseArgs(process.argv.slice(2));
  const reactRoot = resolve(args['antd-path'] ?? process.cwd());
  const requireFromReact = createRequire(resolve(reactRoot, 'package.json'));
  const puppeteer = requireFromReact('puppeteer');
  const reactBaseUrl = args['react-base-url'] ?? 'http://127.0.0.1:8000';
  const blazorBaseUrl = args['blazor-base-url'] ?? 'http://127.0.0.1:8186';
  const browser = await puppeteer.launch({
    headless: true,
    executablePath: args.chrome,
    args: ['--no-sandbox', '--disable-dev-shm-usage'],
  });
  try {
    const routePage = await browser.newPage();
    const reactRoutes = await readRoutes(routePage, `${reactBaseUrl}/components`);
    const blazorRoutes = await readRoutes(routePage, `${blazorBaseUrl}/en-US/components`);
    const pairs = matchComponentRoutes(reactRoutes, blazorRoutes);
    const page = await browser.newPage();
    const components = [];
    for (const pair of pairs.matched) {
      const react = await readPageDemos(page, `${reactBaseUrl}${pair.react.path}`);
      const blazor = await readPageDemos(page, `${blazorBaseUrl}${pair.blazor.path}`);
      const demoComparison = compareReactDemoNames(react.demos, blazor.demos);
      const matchedDemos = demoComparison.matched.map((reactDemo) => ({
        react: reactDemo,
        blazor: blazor.demos.find((demo) => normalizeDemoId(demo.name) === normalizeDemoId(reactDemo.name)),
      }));
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
      process.stdout.write(`[${components.length}/${pairs.matched.length}] ${pair.react.slug}: React ${react.demos.length}, Blazor ${blazor.demos.length}, matched ${matchedDemos.length}\n`);
    }
    const report = {
      generatedAt: new Date().toISOString(),
      bases: { react: reactBaseUrl, blazor: blazorBaseUrl },
      summary: {
        reactRoutes: reactRoutes.length,
        blazorRoutes: blazorRoutes.length,
        matchedComponents: pairs.matched.length,
        missingBlazorRoutes: pairs.missing,
        excludedReactRoutes: pairs.excluded,
        extraBlazorRoutes: pairs.extra,
        reactDemos: components.reduce((total, item) => total + item.demos.react, 0),
        blazorDemos: components.reduce((total, item) => total + item.demos.blazor, 0),
        matchedDemos: components.reduce((total, item) => total + item.demos.matched, 0),
        missingReactDemos: components.reduce((total, item) => total + item.demos.missingReactNames.length, 0),
        extraBlazorDemos: components.reduce((total, item) => total + item.demos.extraBlazorNames.length, 0),
        blockedPages: components.filter((item) => item.blockers.react.length || item.blockers.blazor.length).length,
      },
      components,
    };
    if (args.report) await writeFile(resolve(args.report), `${JSON.stringify(report, null, 2)}\n`);
    process.stdout.write(`${JSON.stringify({ report: args.report ?? null, ...report.summary }, null, 2)}\n`);
  } finally {
    await browser.close();
  }
}

if (import.meta.url === `file://${process.argv[1]}`) {
  main().catch((error) => {
    console.error(error);
    process.exitCode = 1;
  });
}
