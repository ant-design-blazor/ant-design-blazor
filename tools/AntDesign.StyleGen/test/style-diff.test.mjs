import test from 'node:test';
import assert from 'node:assert/strict';
import { compareArtifactSets, compareCss, validateComparableCss } from '../src/style-diff.mjs';

const css = '.ant-btn {\n  color: var(--ant-color-text);\n}\n';

test('compares static, token-value and runtime CSS byte-for-byte', () => {
  const artifacts = ['static', 'token-values', 'runtime-overrides'].map((layer) => ({ id: layer, layer, path: `${layer}.css` }));
  const result = compareArtifactSets({
    expectedManifest: { artifacts },
    expectedCssById: new Map(artifacts.map(({ id }) => [id, css])),
    actualManifest: { artifacts },
    actualCssById: new Map(artifacts.map(({ id }) => [id, css])),
  });
  assert.deepEqual(result, { equal: true, diagnostics: [] });
});

test('reports the first differing byte and its CSS location', () => {
  const result = compareCss(css, css.replace('text', 'texT'), { id: 'button', layer: 'static' });
  assert.equal(result.equal, false);
  assert.equal(result.firstDifference.line, 2);
  assert.equal(result.firstDifference.column, 29);
  assert.notEqual(result.firstDifference.expectedByte, result.firstDifference.actualByte);
});

test('treats line endings, declaration order and token values as exact differences', () => {
  assert.equal(compareCss(css, css.replace('color:', 'background:')).equal, false);
  assert.equal(compareCss(css, css.replaceAll('\n', '\r\n')).equal, false);
  assert.equal(compareCss('--ant-a: 1;\n', '--ant-a: 2;\n').equal, false);
});

test('reports missing and added artifacts, with explicit allowlist support', () => {
  const expectedManifest = { artifacts: [{ id: 'button', layer: 'static' }, { id: 'alert', layer: 'static' }] };
  const actualManifest = { artifacts: [{ id: 'button', layer: 'static' }, { id: 'input', layer: 'static' }] };
  const strict = compareArtifactSets({ expectedManifest, expectedCssById: new Map([['button', css]]), actualManifest, actualCssById: new Map([['button', css]]) });
  assert.deepEqual(strict.diagnostics.map(({ type, id }) => ({ type, id })), [{ type: 'missing', id: 'alert' }, { type: 'added', id: 'input' }]);
  const allowed = compareArtifactSets({ expectedManifest, expectedCssById: new Map([['button', css]]), actualManifest, actualCssById: new Map([['button', css]]), allowlist: [/^(alert|input)$/] });
  assert.equal(allowed.equal, true);
});

test('enforces the shared UTF-8/LF/trailing-newline protocol', () => {
  assert.equal(validateComparableCss(css), css);
  assert.throws(() => validateComparableCss('.x {}', 'actual.css'), /exactly one LF/);
  assert.throws(() => validateComparableCss('.x {}\r\n', 'actual.css'), /LF line endings/);
});
