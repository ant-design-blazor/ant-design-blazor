import assert from 'node:assert/strict';
import test from 'node:test';
import { applyAction, capture, scrollScopeAnchorIntoView, waitForSvgUses } from '../src/dom-style-parity-runner.mjs';
import { JSDOM } from 'jsdom';

test('SVG readiness requires all declared glyphs to resolve and paint', async () => {
  const saved = globalThis.document;
  const savedWindow = globalThis.window;
  const dom = new JSDOM('<div id="demo"><span class="anticon"><svg><use href="#a"></use><use href="#b"></use></svg></span></div><svg><symbol id="a"></symbol></svg>');
  try {
    globalThis.document = dom.window.document;
    globalThis.window = dom.window;
    let predicate;
    let input;
    const page = { async waitForFunction(fn, options, arg) {
      assert.equal(options.timeout, 500);
      predicate = fn; input = arg;
    } };
    const spec = { scopeSelector: '#demo', rootSelector: '.anticon' };
    await waitForSvgUses(page, 'blazor', spec,
      [{ path: 'use', blazorSelector: 'use', verifySvgUse: true, minimumCount: 2 }], 500);
    for (const use of document.querySelectorAll('use')) use.getBoundingClientRect = () => ({ width: 14, height: 14 });
    assert.equal(predicate(input), false, 'missing symbol');
    const symbol = document.createElementNS('http://www.w3.org/2000/svg', 'symbol');
    symbol.id = 'b'; document.querySelector('symbol').parentElement.append(symbol);
    assert.equal(predicate(input), false, 'symbol set needs to settle');
    assert.equal(predicate(input), false, 'symbol set needs another stable poll');
    assert.equal(predicate(input), false, 'symbol set needs a third stable poll');
    assert.equal(predicate(input), true);
    document.querySelector('use').getBoundingClientRect = () => ({ width: 0, height: 14 });
    assert.equal(predicate(input), false, 'unpainted glyph');
    document.querySelector('use').remove();
    assert.equal(predicate(input), false, 'too few glyphs');
  } finally {
    dom.window.close();
    if (saved === undefined) delete globalThis.document; else globalThis.document = saved;
    if (savedWindow === undefined) delete globalThis.window; else globalThis.window = savedWindow;
  }
});

test('SVG readiness does not wait when no glyph verification is declared', async () => {
  await waitForSvgUses({ waitForFunction() { assert.fail('unexpected wait'); } }, 'react', {}, [], 500);
});

test('read-only capture rejects empty roots instead of comparing two empty snapshots', async () => {
  const saved = globalThis.document;
  try {
    globalThis.document = { querySelector: () => ({ querySelectorAll: () => [] }) };
    await assert.rejects(capture({ evaluate: (fn, input) => fn(input) }, 'react',
      { scopeSelector: '#demo', rootSelector: '.anticon' }, [], []), /capture roots not found/);
  } finally {
    if (saved === undefined) delete globalThis.document; else globalThis.document = saved;
  }
});

test('press actions use Puppeteer keyboard events after focusing the side-specific input', async () => {
  const calls = [];
  const page = {
    async bringToFront() { calls.push('front'); },
    async waitForSelector(selector) { calls.push(`ready:${selector}`); },
    async focus(selector) { calls.push(`focus:${selector}`); },
    keyboard: { async press(key) { calls.push(`press:${key}`); } },
  };
  await applyAction(page, 'blazor', { type: 'press', key: 'Tab', reactSelector: '#react-input', blazorSelector: '#blazor-input' }, 1000);
  assert.deepEqual(calls, ['front', 'ready:#blazor-input', 'focus:#blazor-input', 'press:Tab']);
});

test('click actions scroll deep documentation targets into view instantly before clicking', async () => {
  const calls = [];
  const page = {
    async bringToFront() { calls.push('front'); },
    async $eval(selector, callback) {
      calls.push(`scroll:${selector}`);
      callback({ scrollIntoView(options) { calls.push(options); } });
    },
    locator(selector) {
      return { async click(options) { calls.push(`click:${selector}`); calls.push(options); } };
    },
  };
  await applyAction(page, 'blazor', {
    type: 'click', reactSelector: '#react-demo', blazorSelector: '#blazor-demo',
  }, 1000);
  assert.deepEqual(calls, [
    'front', 'scroll:#blazor-demo',
    { block: 'center', inline: 'nearest', behavior: 'instant' },
    'click:#blazor-demo', { timeout: 1000 },
  ]);
});

test('hidden selector actions wait for the selector to disappear or become hidden', async () => {
  const calls = [];
  const page = {
    async bringToFront() { calls.push('front'); },
    async waitForSelector(selector, options) { calls.push({ selector, options }); },
  };
  await applyAction(page, 'react', {
    type: 'waitForSelector',
    state: 'hidden',
    reactSelector: '.ant-select-open',
    blazorSelector: '.ant-select-open',
  }, 1000);
  assert.deepEqual(calls, [
    'front',
    { selector: '.ant-select-open', options: { hidden: true, timeout: 1000 } },
  ]);
});

test('lazy demo anchors are scrolled into view before waiting for the inner scope', async () => {
  const calls = [];
  const savedDocument = globalThis.document;
  globalThis.document = {
    querySelector(selector) {
      calls.push(`query:${selector}`);
      return { scrollIntoView(options) { calls.push(options); } };
    },
  };
  const page = {
    async evaluate(callback, selector) {
      calls.push(`evaluate:${selector}`);
      callback(selector);
    },
  };
  try {
    await scrollScopeAnchorIntoView(page, '#select-demo-suffix .code-box-demo');
    assert.deepEqual(calls, [
      'evaluate:#select-demo-suffix',
      'query:#select-demo-suffix',
      { block: 'center', inline: 'nearest' },
    ]);
  } finally {
    if (savedDocument === undefined) delete globalThis.document;
    else globalThis.document = savedDocument;
  }
});

test('measureClickVisible times from input dispatch until the popup is paint-visible', async () => {
  const calls = [];
  const page = {
    async bringToFront() { calls.push('front'); },
    async evaluate(fn, ...args) {
      if (fn.toString().includes('window.__antDesignParityVisibilityPromise = new Promise')) {
        calls.push(`start-on-pointerdown:${args[0]}:${args[1]}`);
        return;
      }
      if (fn.toString().includes('window.__antDesignParityVisibilityPromise')) return 42.5;
      assert.fail('unexpected browser evaluation');
    },
    locator(selector) {
      return { async click() { calls.push(`click:${selector}`); } };
    },
  };
  const elapsed = await applyAction(page, 'react', {
    type: 'measureClickVisible',
    reactSelector: '#trigger',
    reactVisibleSelector: '.popup',
  }, 1000);
  assert.equal(elapsed, 42.5);
  assert.deepEqual(calls, ['front', 'start-on-pointerdown:#trigger:.popup', 'click:#trigger']);
});

test('waitForPortal is scoped to a visible popup near the interacted trigger', async () => {
  const calls = [];
  const page = {
    async bringToFront() { calls.push('front'); },
    async waitForFunction(predicate, options, args) { calls.push({ predicate: String(predicate), options, args }); },
  };

  await applyAction(page, 'react', {
    type: 'waitForPortal',
    reactTriggerSelector: '#date-picker-demo-basic .ant-picker:first-of-type',
    reactPortalSelector: '.ant-picker-dropdown:not(.ant-picker-dropdown-hidden)',
  }, 5000);

  assert.deepEqual(calls[0], 'front');
  assert.match(calls[1].predicate, /getBoundingClientRect/);
  assert.deepEqual(calls[1].options, { timeout: 5000 });
  assert.deepEqual(calls[1].args, {
    triggerSelector: '#date-picker-demo-basic .ant-picker:first-of-type',
    portalSelector: '.ant-picker-dropdown:not(.ant-picker-dropdown-hidden)',
    maxDistance: 100,
  });
});
