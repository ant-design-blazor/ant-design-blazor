import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import test from 'node:test';
import { extractSeedTokenDefaults, renderSeedTokenDefaults } from '../src/seed-token-defaults.mjs';

test('React seed defaults preserve object spreads, empty strings, zeroes, booleans, and template literals', () => {
  const values = extractSeedTokenDefaults("const colors = { blue: '#1677FF' }; const seedToken = { ...colors, motionBase: 0, colorLink: '', motion: true, fontFamily: `System\nFont` };");
  assert.deepEqual(values, { blue: '#1677FF', motionBase: 0, colorLink: '', motion: true, fontFamily: 'System\nFont' });
});

test('generated C# seed defaults match the pinned React source', () => {
  const root = new URL('../../../', import.meta.url);
  const inventory = JSON.parse(readFileSync(new URL('docs/antd-v6/semantic-dom-inventory.json', root), 'utf8'));
  const source = readFileSync(new URL(`${inventory.source.path}/theme/themes/seed.ts`, root), 'utf8');
  const generated = readFileSync(new URL('components/theming/GeneratedThemeTokens.cs', root), 'utf8');
  assert.ok(generated.includes(renderSeedTokenDefaults(source)));
  const defaults = extractSeedTokenDefaults(source);
  assert.equal(defaults.colorPrimary, '#1677ff');
  assert.equal(defaults.motionBase, 0);
  assert.equal(defaults.fontSize, 14);
});
