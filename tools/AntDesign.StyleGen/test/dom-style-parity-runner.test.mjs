import assert from 'node:assert/strict';
import test from 'node:test';
import { applyAction, capture, waitForSvgUses } from '../src/dom-style-parity-runner.mjs';
import { JSDOM } from 'jsdom';

test('SVG readiness requires all declared glyphs to resolve and paint', async () => {
  const saved = globalThis.document;
  const dom = new JSDOM('<div id="demo"><span class="anticon"><svg><use href="#a"></use><use href="#b"></use></svg></span></div><svg><symbol id="a"></symbol></svg>');
  try {
    globalThis.document = dom.window.document;
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
    assert.equal(predicate(input), true);
    document.querySelector('use').getBoundingClientRect = () => ({ width: 0, height: 14 });
    assert.equal(predicate(input), false, 'unpainted glyph');
    document.querySelector('use').remove();
    assert.equal(predicate(input), false, 'too few glyphs');
  } finally {
    dom.window.close();
    if (saved === undefined) delete globalThis.document; else globalThis.document = saved;
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
