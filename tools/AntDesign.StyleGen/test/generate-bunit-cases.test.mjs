import test from 'node:test';
import assert from 'node:assert/strict';
import { generateBunitCases } from '../src/generate-bunit-cases.mjs';

test('generates a bUnit test from a React snapshot case', () => {
  const source = '/Volumes/Data/GitHub/ant-design-blazor/tools/AntDesign.StyleGen/examples/dom-cases.json';
  const output = generateBunitCases(source);
  assert.match(output, /React_button-basic_matches_Blazor/);
  assert.match(output, /RenderComponent<AntDesign\.Button>/);
  assert.match(output, /MarkupMatches/);
  assert.match(output, /Step: disable-button/);
  assert.match(output, /SetParametersAndRender/);
  assert.match(output, /Assert\.Contains\("ant-btn-disabled"/);
  assert.match(output, /Assert\.Equal\(true, cut\.Instance\.Disabled\)/);
});
