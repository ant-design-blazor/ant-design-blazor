import assert from 'node:assert/strict';
import test from 'node:test';
import { JSDOM } from 'jsdom';
import {
  compareReactDemoNames,
  getReferencedOverlayRoots,
  isElementRenderedInCurrentState,
  matchComponentRoutes,
  normalizeDemoId,
  withIsolatedPage,
} from '../src/documentation-parity-inventory.mjs';

test('pairs ARIA-referenced portal overlays with their demo and skips in-scope overlays', () => {
  const { window } = new JSDOM(`
    <section id="demo">
      <div class="ant-select"><input aria-controls="portal-list"></div>
    </section>
    <div class="ant-select-dropdown"><div id="portal-list" role="listbox"></div></div>
    <section id="local-demo">
      <div class="ant-select"><input aria-controls="local-list"></div>
      <div class="ant-select-dropdown"><div id="local-list" role="listbox"></div></div>
    </section>
  `);
  const portalScope = window.document.querySelector('#demo');
  const localScope = window.document.querySelector('#local-demo');

  assert.deepEqual(getReferencedOverlayRoots(portalScope, window.document).map((element) => element.className), [
    'ant-select-dropdown',
  ]);
  assert.deepEqual(getReferencedOverlayRoots(localScope, window.document), []);
  window.close();
});

test('excludes display-none and visibility-hidden portal descendants from rendered-state scans', () => {
  const { window } = new JSDOM(`
    <div id="visible"><span id="shown"></span></div>
    <div id="closed" style="display:none"><span id="closed-child"></span></div>
    <div style="visibility:hidden"><span id="hidden-child"></span></div>
  `);
  const { document } = window;
  assert.equal(isElementRenderedInCurrentState(document.querySelector('#shown')), true);
  assert.equal(isElementRenderedInCurrentState(document.querySelector('#closed-child')), false);
  assert.equal(isElementRenderedInCurrentState(document.querySelector('#hidden-child')), false);
  window.close();
});

test('closes each isolated browser page after both success and failure', async () => {
  const pages = [];
  const browser = {
    async newPage() {
      const page = {
        closed: false,
        isClosed() { return this.closed; },
        async close() { this.closed = true; },
      };
      pages.push(page);
      return page;
    },
  };

  assert.equal(await withIsolatedPage(browser, async (page) => pages.includes(page)), true);
  await assert.rejects(withIsolatedPage(browser, async () => { throw new Error('route failed'); }), /route failed/);
  assert.equal(pages.length, 2);
  assert.ok(pages.every(({ closed }) => closed));
});

test('normalizes Dumi and Blazor demo id differences without conflating demo suffixes', () => {
  assert.equal(normalizeDemoId('components-button-demo-Basic'), 'buttondemobasic');
  assert.equal(normalizeDemoId('button-demo-basic'), 'buttondemobasic');
  assert.notEqual(normalizeDemoId('button-demo-basic'), normalizeDemoId('button-demo-loading'));
});

test('counts only missing React demo names and reports Blazor-only demos separately', () => {
  const result = compareReactDemoNames(
    [{ name: 'Basic' }, { name: 'OnChange' }, { name: 'Target' }],
    [{ name: 'basic' }, { name: 'Debug' }, { name: 'Scroll' }],
  );

  assert.deepEqual(result.matched.map(({ name }) => name), ['Basic']);
  assert.deepEqual(result.missingReact.map(({ name }) => name), ['OnChange', 'Target']);
  assert.deepEqual(result.extraBlazor.map(({ name }) => name), ['Debug', 'Scroll']);
});

test('pairs route slugs and reports unmatched React components', () => {
  const result = matchComponentRoutes(
    [
      { slug: 'auto-complete', name: 'AutoComplete' },
      { slug: 'float-button', name: 'FloatButton' },
      { slug: '_util', name: '_util' },
    ],
    [
      { slug: 'autocomplete', name: 'AutoComplete' },
      { slug: 'button', name: 'Button' },
    ],
  );
  assert.deepEqual(result.excluded.map(({ slug }) => slug), ['_util']);
  assert.deepEqual(result.matched.map(({ react }) => react.slug), ['auto-complete']);
  assert.deepEqual(result.missing.map(({ slug }) => slug), ['float-button']);
  assert.deepEqual(result.extra.map(({ slug }) => slug), ['button']);
});
