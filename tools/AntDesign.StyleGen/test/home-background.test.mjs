import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import test from 'node:test';

test('luminous background matches React ranges and cleans timers on reduced motion and removal', async () => {
  const source = readFileSync(new URL('../../../site/AntDesign.Docs/wwwroot/js/home-background.js', import.meta.url), 'utf8');
  const saved = Object.fromEntries(['window', 'document', 'MutationObserver'].map(key => [key, globalThis[key]]));
  const savedRandom = Math.random;
  const timers = new Map();
  const listeners = new Map();
  let id = 0;
  let disconnected = 0;
  let mutation;
  const motion = { matches: false, addEventListener: (key, fn) => listeners.set(key, fn), removeEventListener: key => listeners.delete(key) };
  const bubbles = [[2, 1], [1, 1], [1, 2]].map(([x, y]) => ({
    dataset: { offsetX: x, offsetY: y }, values: new Map(),
    style: { setProperty(key, value) { this.owner.values.set(key, value); } },
  }));
  bubbles.forEach(bubble => { bubble.style.owner = bubble; });
  const root = { isConnected: true, querySelectorAll: () => bubbles };
  try {
    Math.random = () => .75;
    globalThis.window = { matchMedia: () => motion, setTimeout: (fn, ms) => { timers.set(++id, { fn, ms }); return id; }, clearTimeout: key => timers.delete(key) };
    globalThis.document = { body: {} };
    globalThis.MutationObserver = class { constructor(fn) { mutation = fn; } observe() {} disconnect() { disconnected++; } };
    const module = await import(`data:text/javascript;base64,${Buffer.from(source).toString('base64')}`);
    module.mount(root);
    module.mount(root);
    assert.equal(timers.size, 3, 'mount is idempotent');
    assert.equal(bubbles[0].values.get('--bubble-x'), '200px');
    assert.equal(bubbles[0].values.get('--bubble-y'), '100px');
    assert.equal(bubbles[2].values.get('--bubble-y'), '200px');
    assert.equal(bubbles[0].values.get('--bubble-light-opacity'), '0.1375');
    assert.equal(bubbles[0].values.get('--bubble-dark-opacity'), '0.25');
    assert.equal(bubbles[0].values.get('--bubble-scale'), '1.75');
    assert.ok([...timers.values()].every(timer => timer.ms === 4500));
    motion.matches = true;
    listeners.get('change')();
    assert.equal(timers.size, 0);
    motion.matches = false;
    listeners.get('change')();
    assert.equal(timers.size, 3);
    root.isConnected = false;
    mutation();
    assert.equal(timers.size, 0);
    assert.equal(listeners.size, 0);
    assert.equal(disconnected, 1);
    module.unmount(root);
    assert.equal(disconnected, 1, 'unmount is idempotent');
    root.isConnected = true;
    motion.matches = true;
    module.mount(root);
    assert.equal(timers.size, 0, 'initial reduced motion does not schedule animation');
    module.unmount(root);
  } finally {
    Math.random = savedRandom;
    for (const [key, value] of Object.entries(saved)) {
      if (value === undefined) delete globalThis[key]; else globalThis[key] = value;
    }
  }
});
