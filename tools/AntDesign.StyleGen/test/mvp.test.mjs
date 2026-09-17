import test from 'node:test';
import assert from 'node:assert/strict';
import { compareArtifactSets, compareCss } from '../src/style-diff.mjs';
import { createArtifact, createBaseline, sha256, validateBaseline } from '../src/baseline.mjs';

const components = ['Button', 'Alert', 'Input'];
const themes = ['light', 'dark'];

function scenarioCss(component, theme, override = false) {
  const className = `ant-${component.toLowerCase()}`;
  const text = theme === 'dark' ? '#ffffff' : '#000000';
  const selector = override ? `.${className}.mvp-override` : `.${className}`;
  const property = override ? 'border-width' : 'color';
  const value = override ? '2px' : 'var(--ant-color-text)';
  return `${selector} {\n  ${property}: ${value};\n}\n`;
}

function tokenCss(theme, override = false) {
  const value = theme === 'dark' ? '#ffffff' : '#000000';
  const extra = override ? '  --ant-button-border-width: 2px;\n' : '';
  return `:where(.mvp-${theme}) {\n  --ant-color-text: ${value};\n${extra}}\n`;
}

function buildMvpArtifacts() {
  const artifacts = [];
  const cssById = new Map();
  for (const component of components) {
    for (const theme of themes) {
      for (const [layer, css] of [
        ['static', scenarioCss(component, theme)],
        ['token-values', tokenCss(theme)],
      ]) {
        const id = `${component.toLowerCase()}.${theme}.${layer}`;
        artifacts.push(createArtifact({ id, path: `${layer}/${id}.css`, layer, component, theme, css }));
        cssById.set(id, css);
      }
    }
  }
  const overrideId = 'button.light.runtime-overrides';
  const overrideCss = scenarioCss('Button', 'light', true);
  artifacts.push(createArtifact({ id: overrideId, path: `runtime-overrides/${overrideId}.css`, layer: 'runtime-overrides', component: 'Button', theme: 'light', css: overrideCss }));
  cssById.set(overrideId, overrideCss);
  const overrideTokenId = 'button.light.override.token-values';
  const overrideTokenCss = tokenCss('light', true);
  artifacts.push(createArtifact({ id: overrideTokenId, path: `token-values/${overrideTokenId}.css`, layer: 'token-values', component: 'Button', theme: 'light', css: overrideTokenCss }));
  cssById.set(overrideTokenId, overrideTokenCss);
  return { manifest: createBaseline({ upstream: { repository: 'ant-design', revision: 'mvp', version: '6.6.4' }, generator: { name: 'mvp-fixture', version: '1' }, inputDigest: sha256('button-alert-input-light-dark-override'), artifacts }), cssById };
}

test('MVP covers Button, Alert, Input, light/dark and component token override', () => {
  const react = buildMvpArtifacts();
  const blazor = buildMvpArtifacts();
  validateBaseline(react.manifest);
  const result = compareArtifactSets({
    expectedManifest: react.manifest,
    expectedCssById: react.cssById,
    actualManifest: blazor.manifest,
    actualCssById: blazor.cssById,
  });
  assert.equal(result.equal, true, JSON.stringify(result.diagnostics));
  assert.equal(react.manifest.artifacts.length, 14);
  assert.ok([...react.cssById.values()].some((css) => css.includes('--ant-button-border-width')));
});

test('MVP gate fails on an intentional single-byte mutation', () => {
  const react = buildMvpArtifacts();
  const blazor = buildMvpArtifacts();
  const id = 'input.dark.token-values';
  blazor.cssById.set(id, blazor.cssById.get(id).replace('#ffffff', '#fffffe'));
  const result = compareArtifactSets({
    expectedManifest: react.manifest,
    expectedCssById: react.cssById,
    actualManifest: blazor.manifest,
    actualCssById: blazor.cssById,
  });
  assert.equal(result.equal, false);
  assert.equal(result.diagnostics[0].id, id);
  assert.equal(result.diagnostics[0].firstDifference.line, 2);
  assert.equal(compareCss(react.cssById.get(id), blazor.cssById.get(id)).equal, false);
});
