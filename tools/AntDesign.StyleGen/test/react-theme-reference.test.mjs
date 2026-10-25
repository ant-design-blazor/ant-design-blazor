import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';
import test from 'node:test';
import { generateReactThemeReference } from '../src/react-theme-reference.mjs';

test('C# parity fixtures reproduce every token returned by the pinned React algorithms', async () => {
  const root = new URL('../../../', import.meta.url);
  const inventory = JSON.parse(readFileSync(new URL('docs/antd-v6/semantic-dom-inventory.json', root), 'utf8'));
  const upstream = fileURLToPath(new URL(`${inventory.source.path}/..`, root));
  const expected = JSON.parse(readFileSync(new URL('tests/AntDesign.Tests/Theming/Fixtures/react-theme-tokens.json', root), 'utf8'));
  const actual = await generateReactThemeReference(upstream);
  assert.deepEqual(actual, expected);
  assert.equal(actual.scenarios.length, 12);
  for (const scenario of actual.scenarios) {
    assert.equal(Object.keys(scenario.expected).length, 536);
    assert.ok(scenario.expected.colorPrimary);
    assert.ok(scenario.expected.boxShadow);
  }
});
