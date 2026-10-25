import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import test from 'node:test';
import ts from 'typescript';

const source = readFileSync(new URL('../../../components/core/JsInterop/modules/components/menuOverflowHelper.ts', import.meta.url), 'utf8');
const script = ts.transpileModule(source, { compilerOptions: { target: ts.ScriptTarget.ES2020, module: ts.ModuleKind.ES2020 } }).outputText;
const module = await import(`data:text/javascript;base64,${Buffer.from(script).toString('base64')}`);

test('horizontal Menu reserves rest width only when all items do not fit', () => {
  assert.equal(module.fitMenuItems([80, 100, 60], 240, 40), 3);
  assert.equal(module.fitMenuItems([80, 100, 60], 239, 40), 2);
  assert.equal(module.fitMenuItems([80, 100, 60], 219, 40), 1);
  assert.equal(module.fitMenuItems([80, 100, 60], 119, 40), 0);
  assert.equal(module.fitMenuItems([80, 100, 60], 240, 400), 3);
  assert.equal(module.fitMenuItems([80, 100, 60], 0, 40), 0);
});

test('fractional widths, dynamic labels, display:none and a custom wide rest indicator', () => {
  assert.equal(module.fitMenuItems([78.25, 91.5, 70.5], 239.75, 56.25), 3);
  assert.equal(module.fitMenuItems([78.25, 91.5, 70.5], 239, 56.25), 2);
  assert.equal(module.fitMenuItems([80, 100, 60, 0], 239, 95), 1);
  assert.equal(module.fitMenuItems([80, 100, 60, 0], 320, 95), 4);
  assert.equal(module.fitMenuItems([NaN], 300, 40), 0);
});

test('measurement observes real direct children and awaits a pending callback before disconnecting', async () => {
  const names = ['window', 'document', 'ResizeObserver', 'MutationObserver', 'requestAnimationFrame', 'cancelAnimationFrame', 'getComputedStyle'];
  const saved = Object.fromEntries(names.map(name => [name, globalThis[name]]));
  const frames = new Map(), listeners = new Map(), observed = new Set(), calls = [];
  let sequence = 0, release, resize, mutations;
  const element = (id, width, rest = false) => ({ id, width,
    hasAttribute: name => rest ? name === 'data-menu-overflow-rest' : name === 'data-menu-overflow-item',
    getAttribute: () => id, getBoundingClientRect: () => ({ width }),
  });
  const first = element('one', 80), second = element('two', 100), third = element('three', 60), rest = element('rest', 40, true);
  const hidden = element('hidden',100); hidden.hidden = true;
  const root = { isConnected: true, clientWidth: 239, children: [first, second, third, hidden, rest] };
  try {
    globalThis.getComputedStyle = item => ({ display: item.hidden ? 'none' : 'block', marginLeft: '0', marginRight: '0', paddingLeft: '0', paddingRight: '0', columnGap: '0' });
    globalThis.window = { addEventListener: (key, fn) => listeners.set(key, fn), removeEventListener: key => listeners.delete(key) };
    globalThis.document = {};
    globalThis.requestAnimationFrame = fn => { frames.set(++sequence, fn); return sequence; };
    globalThis.cancelAnimationFrame = id => frames.delete(id);
    globalThis.ResizeObserver = class { constructor(fn) { resize = fn; } observe(node) { observed.add(node); } unobserve(node) { observed.delete(node); } disconnect() { observed.clear(); } };
    globalThis.MutationObserver = class { constructor(fn) { mutations = fn; } observe() {} disconnect() { mutations = null; } };
    const flush = async () => { const pending = [...frames.values()]; frames.clear(); pending.forEach(fn => fn()); await new Promise(resolve => setImmediate(resolve)); };
    module.observe(root, { invokeMethodAsync: async (method, ids) => { calls.push({ method, ids }); if (calls.length === 3) await new Promise(resolve => { release = resolve; }); } });
    await flush();
    assert.deepEqual(calls[0], { method: 'SetOverflowItems', ids: ['three'] });
    assert.equal(observed.size, 5);
    assert.equal(observed.has(hidden), false);
    root.clientWidth = 240;
    resize();
    await flush();
    assert.deepEqual(calls[1].ids, []);
    root.children = [third, first, second, rest];
    root.clientWidth = 150;
    mutations();
    await flush();
    assert.deepEqual(calls[2].ids, ['one', 'two']);
    let stopped = false;
    const stop = module.unobserve(root).then(() => { stopped = true; });
    await new Promise(resolve => setImmediate(resolve));
    assert.equal(stopped, false);
    release(); await stop;
    assert.equal(observed.size, 0);
    assert.equal(listeners.size, 0);
    assert.equal(frames.size, 0);
    assert.equal(mutations, null);
  } finally {
    for (const [name, value] of Object.entries(saved)) {
      if (value === undefined) delete globalThis[name]; else globalThis[name] = value;
    }
  }
});
