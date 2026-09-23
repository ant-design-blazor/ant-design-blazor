import test from 'node:test';
import assert from 'node:assert/strict';
import { generateReactDomCatalog } from '../src/generate-react-dom-catalog.mjs';
import { generateRedBaselineTests } from '../src/generate-red-baseline-tests.mjs';

test('discovers every React Jest snapshot case into the red baseline catalog', () => {
  const catalog = generateReactDomCatalog('/Volumes/Data/GitHub/ant-design');
  assert.ok(catalog.summary.components > 0);
  assert.ok(catalog.summary.cases > 0);
  assert.equal(catalog.summary.pending + catalog.summary.adapted, catalog.summary.cases);
  assert.ok(catalog.cases.some((item) => item.status === 'scenario-adapted'));
});

test('generates a failing TDD gate for every discovered React case', () => {
  const catalog = {
    cases: [{ id: 'button-1', component: 'button', react: { key: 'Button renders 1' }, status: 'scenario-adapted', blazorScenario: { id: 'button-renders-correctly', childContent: 'Follow' } }],
  };
  const files = generateRedBaselineTests(catalog);
  assert.ok(files['ReactDomRedBaselineSupport.g.cs']);
  assert.match(files['ReactDomRedBaselineSupport.g.cs'], /RenderComponent<Button>/);
  assert.match(files['ReactDomRedBaseline_ButtonTests.g.cs'], /RenderScenario/);
  assert.match(files['ReactDomRedBaseline_ButtonTests.g.cs'], /MarkupMatches/);
});
