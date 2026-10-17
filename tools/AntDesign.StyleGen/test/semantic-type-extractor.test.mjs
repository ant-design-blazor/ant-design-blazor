import assert from 'node:assert/strict';
import test from 'node:test';
import { extractSemanticTypes } from '../src/semantic-type-extractor.mjs';

test('expands inherited semantic aliases and nested slot types from React TypeScript ASTs', () => {
  const definitions = extractSemanticTypes([
    {
      file: 'tooltip/index.tsx',
      source: `
        export type TooltipSemanticType = {
          classNames?: { root?: string; arrow?: string };
          styles?: { root?: React.CSSProperties; arrow?: React.CSSProperties };
        };
        export type TooltipSemanticAllType = GenerateSemantic<TooltipSemanticType, TooltipProps>;
      `,
    },
    {
      file: 'pagination/index.tsx',
      source: `export type PaginationSemanticType = { classNames?: { root?: string; item?: string }; styles?: { root?: object; item?: object } };`,
    },
    {
      file: 'table/index.tsx',
      source: `
        type BodyNames = { wrapper?: string; cell?: string };
        type BodyStyles = { wrapper?: object; cell?: object };
        export type TableSemanticType = {
          classNames?: { body?: BodyNames; pagination?: PaginationSemanticType['classNames'] } & TooltipSemanticAllType['classNames'];
          styles?: { body?: BodyStyles; pagination?: PaginationSemanticType['styles'] } & TooltipSemanticAllType['styles'];
        };
      `,
    },
  ]);

  const table = definitions.find(({ semanticType }) => semanticType === 'TableSemanticType');
  assert.deepEqual(table.classNames, ['arrow', 'body', 'body.cell', 'body.wrapper', 'pagination', 'pagination.item', 'pagination.root', 'root']);
  assert.deepEqual(table.styles, ['arrow', 'body', 'body.cell', 'body.wrapper', 'pagination', 'pagination.item', 'pagination.root', 'root']);
});
