import assert from 'node:assert/strict';
import test from 'node:test';
import { applyAction } from '../src/dom-style-parity-runner.mjs';

test('press actions use Puppeteer keyboard events after focusing the side-specific input', async () => {
  const calls = [];
  const page = {
    async bringToFront() { calls.push('front'); },
    locator(selector) { return { async focus() { calls.push(`focus:${selector}`); } }; },
    keyboard: { async press(key) { calls.push(`press:${key}`); } },
  };
  await applyAction(page, 'blazor', { type: 'press', key: 'Tab', reactSelector: '#react-input', blazorSelector: '#blazor-input' }, 1000);
  assert.deepEqual(calls, ['front', 'focus:#blazor-input', 'press:Tab']);
});
