import test from 'node:test';
import assert from 'node:assert/strict';
import { extractComponentTokenDefinitions } from '../src/component-tokens.mjs';

test('extracts ComponentToken properties inherited from local interfaces', () => {
  const definitions = extractComponentTokenDefinitions(`
    interface SharedComponentToken {
      paddingInline: number;
      inputFontSizeSM: number;
    }
    interface ComponentToken extends SharedComponentToken {}
  `);

  assert.deepEqual(definitions, [
    { name: 'inputFontSizeSM', type: 'number' },
    { name: 'paddingInline', type: 'number' },
  ]);
});

test('extracts inherited token properties before component-specific overrides', () => {
  const definitions = extractComponentTokenDefinitions(`
    interface Shared {
      color: string;
      size: number;
    }
    interface ComponentToken extends Shared {
      color: 'red' | 'blue';
    }
  `);

  assert.deepEqual(definitions, [
    { name: 'color', type: "'red' | 'blue'" },
    { name: 'size', type: 'number' },
  ]);
});

test('extracts bilingual token descriptions and deprecation metadata', () => {
  const definitions = extractComponentTokenDefinitions(`
    interface ComponentToken {
      /**
       * @deprecated use colorBorder instead
       * @desc 边框颜色
       * @descEN Border color
       */
      borderColor: string;
    }
  `);

  assert.deepEqual(definitions, [{
    name: 'borderColor',
    type: 'string',
    description: '边框颜色',
    descriptionEn: 'Border color',
    deprecated: true,
  }]);
});
