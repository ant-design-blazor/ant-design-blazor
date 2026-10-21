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
  assert.deepEqual(table.classNames, ['arrow', 'body.cell', 'body.wrapper', 'pagination.item', 'pagination.root', 'root']);
  assert.deepEqual(table.styles, ['arrow', 'body.cell', 'body.wrapper', 'pagination.item', 'pagination.root', 'root']);
});

test('flattens nested semantic records through indexed aliases without treating containers as DOM nodes', () => {
  const definitions = extractSemanticTypes([
    {
      file: 'select/index.tsx',
      source: `
        export type SelectSemanticType = { classNames?: { root?: string; popup?: { root?: string; list?: string; listItem?: string } } };
        export type SelectSemanticAllType = GenerateSemantic<SelectSemanticType, SelectProps>;
      `,
    },
    {
      file: 'auto-complete/index.tsx',
      source: `
        export type AutoCompleteSemanticType = {
          classNames?: {
            root?: string;
            popup?: NonNullable<SelectSemanticAllType['classNames']>['popup'];
          };
        };
      `,
    },
  ]);

  const autocomplete = definitions.find(({ semanticType }) => semanticType === 'AutoCompleteSemanticType');
  assert.deepEqual(autocomplete.classNames, ['popup.list', 'popup.listItem', 'popup.root', 'root']);
});

test('extracts semantic slots exposed by exported Props schemas without a SemanticType alias', () => {
  const definitions = extractSemanticTypes([
    {
      file: 'menu/menu.tsx',
      source: `
        type ItemNames = { item?: string; itemContent?: string; itemIcon?: string; itemTitle?: string };
        type ItemStyles = { item?: object; itemContent?: object; itemIcon?: object; itemTitle?: object };
        type MenuNames = { root?: string; list?: string };
        type MenuStyles = { root?: object; list?: object };
        type MenuClassNamesSchemaType = MenuNames & {
          popup?: { root?: string } | string;
          subMenu?: ItemNames;
        } & ItemNames;
        type MenuStylesSchemaType = MenuStyles & {
          popup?: { root?: object } | object;
          subMenu?: ItemStyles;
        } & ItemStyles;
        export type MenuProps = {
          classNames?: MenuClassNamesSchemaType | ((info: { props: MenuProps }) => MenuClassNamesSchemaType);
          styles?: MenuStylesSchemaType | ((info: { props: MenuProps }) => MenuStylesSchemaType);
        };
      `,
    },
  ]);

  assert.equal(definitions.length, 1);
  assert.deepEqual(definitions[0].classNames, [
    'item', 'itemContent', 'itemIcon', 'itemTitle', 'list', 'popup.root', 'root',
    'subMenu.item', 'subMenu.itemContent', 'subMenu.itemIcon', 'subMenu.itemTitle',
  ]);
  assert.deepEqual(definitions[0].styles, definitions[0].classNames);
});
