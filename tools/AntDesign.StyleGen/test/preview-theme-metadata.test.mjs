import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import test from 'node:test';
import { extractPreviewThemeMetadata, extractPreviewAlgorithm, extractCompatibleV4Defaults } from '../src/preview-theme-metadata.mjs';

test('preview metadata comes from catalog entries, not commented images or handwritten color lists', () => {
  const source = `const locales = { en: { default: 'Ant Design', glass: 'Glass' } }; const useThemes=()=>[ {icon:'ant.svg',name:locale.default,key:'light',props:base}, {name:locale.glass,icon:GlassIcon,/* bgImg:'unused.png', */props:glass,bgImgDark:true} ];`;
  assert.deepEqual(extractPreviewThemeMetadata(source), [
    { key: 'light', localeKey: 'default', name: 'Ant Design', icon: 'ant.svg', background: '', darkBackground: false },
    { key: 'glass', localeKey: 'glass', name: 'Glass', icon: 'GlassIcon', background: '', darkBackground: true },
  ]);
  assert.equal(extractPreviewAlgorithm('const theme = { theme: { algorithm: theme.darkAlgorithm } };'), 'dark');
});

test('the pinned React preview catalog and compatible literals are reflected in generated C# metadata', () => {
  const root = new URL('../../../', import.meta.url);
  const inventory = JSON.parse(readFileSync(new URL('docs/antd-v6/semantic-dom-inventory.json', root), 'utf8'));
  const upstream = new URL(`${inventory.source.path}/../`, root);
  const preview = new URL('.dumi/pages/index/components/ThemePreview/previewThemes/', upstream);
  const metadata = extractPreviewThemeMetadata(readFileSync(new URL('index.ts', preview), 'utf8'));
  assert.equal(metadata.length, 13);
  assert.equal(metadata.find(t => t.key === 'glass').background, '');
  assert.equal(metadata.find(t => t.key === 'lark').name, 'Document');
  assert.equal(metadata.find(t => t.key === 'geek').darkBackground, true);
  assert.equal(extractPreviewAlgorithm(readFileSync(new URL('geekTheme.ts', preview), 'utf8')), 'dark');
  const compatible = extractCompatibleV4Defaults(readFileSync(new URL('node_modules/@ant-design/compatible/lib/theme/index.js', upstream), 'utf8'));
  assert.equal(compatible.token.colorPrimary, '#1890ff');
  assert.equal(compatible.token.wireframe, 'true');
  assert.equal(compatible.components.Menu.activeBarWidth, '3');
  const generated = readFileSync(new URL('components/theming/GeneratedPreviewThemes.cs', root), 'utf8');
  for (const theme of metadata) {
    assert.ok(generated.includes(JSON.stringify(theme.icon)));
    assert.ok(generated.includes(JSON.stringify(theme.background)));
  }
});
