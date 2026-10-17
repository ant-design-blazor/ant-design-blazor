import assert from 'node:assert/strict';
import test from 'node:test';
import { compareReactDemoNames, matchComponentRoutes, normalizeDemoId } from '../src/documentation-parity-inventory.mjs';

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

test('pairs route slugs with hyphen conventions and keeps deferred React components explicit', () => {
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
  assert.deepEqual(result.matched.map(({ react }) => react.slug), ['auto-complete']);
  assert.deepEqual(result.excluded.map(({ slug }) => slug), ['float-button', '_util']);
  assert.deepEqual(result.missing, []);
  assert.deepEqual(result.extra.map(({ slug }) => slug), ['button']);
});
