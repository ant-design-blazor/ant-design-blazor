import test from 'node:test';
import assert from 'node:assert/strict';
import { csharpTokenType, extractComponentTokenDefinitions, extractExcludedComponentTokenNames } from '../src/component-tokens.mjs';

test('C# property and documentation types use one shared mapping without changing React source types', () => {
  for (const [source, expected] of [
    ['number', 'double'], ['string', 'string'], ['boolean', 'bool'], ['true | false', 'bool'],
    ['number | string', 'OneOf<double, string>'], ["CSSProperties['padding']", 'OneOf<string, double>'],
    ['CSSObject["borderRadius"]', 'OneOf<string, double>'], ['unknown', 'string'],
  ]) assert.equal(csharpTokenType(source), expected, source);
});

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

test('honors Exclude and Omit utility types in inherited component tokens', () => {
  const definitions = extractComponentTokenDefinitions(`
    interface SharedComponentToken {
      addonBg: string;
      activeBg: string;
      borderColor: string;
    }
    interface PanelComponentToken {
      cellHeight: number;
    }
    interface ComponentToken extends Exclude<SharedComponentToken, 'addonBg' | 'borderColor'>, PanelComponentToken {
      ownToken: boolean;
    }
  `);

  assert.deepEqual(definitions, [
    { name: 'activeBg', type: 'string' },
    { name: 'cellHeight', type: 'number' },
    { name: 'ownToken', type: 'boolean' },
  ]);

  const omitted = extractComponentTokenDefinitions(`
    interface Shared {
      visible: boolean;
      internal: string;
    }
    interface ComponentToken extends Omit<Shared, 'internal'> {}
  `);
  assert.deepEqual(omitted, [{ name: 'visible', type: 'boolean' }]);

  assert.deepEqual([...extractExcludedComponentTokenNames(`
    interface ComponentToken extends Exclude<SharedComponentToken, 'addonBg' | 'colorBg'> {}
  `)], ['addonBg', 'colorBg']);
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
