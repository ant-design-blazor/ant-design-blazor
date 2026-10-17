import assert from 'node:assert/strict';
import test from 'node:test';
import {
  getEffectiveSemanticCatalog,
  matchSemanticRoutes,
  normalizeSemanticRouteKey,
  parseSemanticSelectorCatalog,
} from '../src/semantic-dom-runtime-audit.mjs';

test('parses semantic node names and selectors from synchronized JSON catalog', () => {
  const catalog = parseSemanticSelectorCatalog({ components: [
    { name: 'button', nodes: [
      { name: 'root', selector: '.ant-btn' },
      { name: 'content', selector: '.ant-btn > span:not(.ant-btn-icon)' },
    ] },
    { name: 'empty', nodes: [] },
  ] });
  assert.deepEqual(catalog.get('button'), [
    { name: 'root', selector: '.ant-btn' },
    { name: 'content', selector: '.ant-btn > span:not(.ant-btn-icon)' },
  ]);
  assert.deepEqual(catalog.get('empty'), []);
});

test('effective runtime catalog is the synchronized JSON catalog', () => {
  const catalog = getEffectiveSemanticCatalog({ components: [
    { name: 'button', nodes: [{ name: 'root', selector: '.ant-btn' }] },
  ] });
  assert.deepEqual(catalog.get('button'), [{ name: 'root', selector: '.ant-btn' }]);
});

test('normalizes semantic component and documentation route slugs', () => {
  assert.equal(normalizeSemanticRouteKey('page-header'), 'pageheader');
  const result = matchSemanticRoutes(new Map([
    ['page-header', [{ name: 'root', selector: '.ant-page-header' }]],
    ['listy', [{ name: 'root', selector: '.ant-list' }]],
    ['missing', [{ name: 'root', selector: '.ant-missing' }]],
  ]), [
    { Title: 'PageHeader', Url: 'components/pageheader', Children: [] },
    { Title: 'List', Url: 'components/list', Children: [] },
  ]);
  assert.deepEqual(result.matched.map(({ component, route }) => [component, route]), [
    ['page-header', 'components/pageheader'],
    ['listy', 'components/list'],
  ]);
  assert.deepEqual(result.missing, [{ component: 'missing', nodes: 1 }]);
});

test('keeps explicitly deferred component pages out of the route failure count', () => {
  const result = matchSemanticRoutes(new Map([
    ['float-button', [{ name: 'root', selector: '.ant-float-btn' }]],
  ]), []);
  assert.deepEqual(result.missing, []);
  assert.deepEqual(result.excluded.map(({ component }) => component), ['float-button']);
});
