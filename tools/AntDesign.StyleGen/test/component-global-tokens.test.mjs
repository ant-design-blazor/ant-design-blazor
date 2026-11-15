import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import test from 'node:test';
import { collectComponentGlobalTokens, renderComponentGlobalTokenCatalog } from '../src/component-global-tokens.mjs';

test('component global token catalogs follow React usage, exclude undocumented metadata, and normalize route aliases', () => {
  const metadata = { global: { colorText: {}, paddingSM: {}, colorSplit: {} } };
  const data = { QRCode: { global: ['paddingSM', 'colorText', 'colorSplit', 'colorText', 'internalToken'] }, Other: { component: {} } };
  assert.deepEqual(collectComponentGlobalTokens(data, metadata), { qrcode: ['colorSplit', 'colorText', 'paddingSM'] });
  assert.match(renderComponentGlobalTokenCatalog(data, metadata), /\["qrcode"\] = new string\[\] \{ "colorSplit", "colorText", "paddingSM" \}/);
});

test('the generated global usage catalog is up to date with the pinned React source', () => {
  const root = new URL('../../../', import.meta.url);
  const inventory = JSON.parse(readFileSync(new URL('docs/antd-v6/semantic-dom-inventory.json', root), 'utf8'));
  const upstream = new URL(`${inventory.source.path.replace(/components\/?$/, '')}components/version/`, root);
  const data = JSON.parse(readFileSync(new URL('token.json', upstream), 'utf8'));
  const metadata = JSON.parse(readFileSync(new URL('token-meta.json', upstream), 'utf8'));
  const generated = readFileSync(new URL('components/theming/GeneratedThemeTokens.cs', root), 'utf8');
  assert.ok(generated.includes(renderComponentGlobalTokenCatalog(data, metadata)), 'Regenerate C# theme tokens from React token.json');
  assert.equal(collectComponentGlobalTokens(data, metadata).qrcode.length, 12);
});

test('generated ThemeToken shortcuts cover every React v6 global token', () => {
  const root = new URL('../../../', import.meta.url);
  const inventory = JSON.parse(readFileSync(new URL('docs/antd-v6/semantic-dom-inventory.json', root), 'utf8'));
  const upstream = new URL(`${inventory.source.path.replace(/components\/?$/, '')}components/version/`, root);
  const metadata = JSON.parse(readFileSync(new URL('token-meta.json', upstream), 'utf8'));
  const generated = readFileSync(new URL('components/theming/ThemeTokenProperties.cs', root), 'utf8');
  const generatedNames = new Set([...generated.matchAll(/this\["([A-Za-z0-9]+)"\]/g)].map((match) => match[1]));
  const missing = Object.keys(metadata.global ?? {}).filter((name) => !generatedNames.has(name));

  assert.equal(Object.keys(metadata.global ?? {}).length, 234, 'Update the expected count when React publishes new global tokens');
  assert.deepEqual(missing, [], 'Regenerate ThemeTokenProperties.cs from React token-meta.json');
});
