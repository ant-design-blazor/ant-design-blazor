import assert from 'node:assert/strict';
import test from 'node:test';
import { renderGlobalTokenReference } from '../src/global-token-docs.mjs';

const metadata = {
  global: {
    colorPrimary: { desc: '主色', descEn: 'Primary color', type: 'string', source: 'seed' },
    controlHeight: { desc: '控件高度', descEn: 'Control height', type: 'number', source: 'map' },
    colorText: { desc: '文本色 | 基础', descEn: 'Text color', type: 'string', source: 'alias' },
  },
};

test('global token reference keeps React metadata localized, categorized and CSS-variable ready', () => {
  const english = renderGlobalTokenReference(metadata, 'en-US');
  const chinese = renderGlobalTokenReference(metadata, 'zh-CN');

  assert.match(english, /3 tokens in total/);
  assert.match(english, /--ant-color-primary/);
  assert.match(english, /Seed \(foundation\)/);
  assert.match(english, /Primary color/);
  assert.match(chinese, /共 3 项/);
  assert.match(chinese, /主色/);
  assert.match(chinese, /文本色 \\| 基础/);
});

test('global token reference rejects incomplete React metadata', () => {
  assert.throws(() => renderGlobalTokenReference({}, 'en-US'), /global token map/);
});
