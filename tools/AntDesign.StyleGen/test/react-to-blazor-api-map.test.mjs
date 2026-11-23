import test from 'node:test';
import assert from 'node:assert/strict';
import { mapReactApiToBlazor } from '../src/map-react-api-to-blazor.mjs';

test('keeps every React prop and reports target-side mapping gaps', () => {
  const reactApi = {
    props: [{
      component: 'button',
      declaration: 'ButtonProps',
      properties: [
        { name: 'disabled', type: 'boolean', kind: 'value' },
        { name: 'children', type: 'ReactNode', kind: 'value' },
        { name: 'onClick', type: 'MouseEventHandler', kind: 'callback' },
        { name: 'unknownReactProp', type: 'string', kind: 'value' },
      ],
    }],
  };
  const blazorApi = {
    components: [{
      component: 'button',
      parameters: [
        { name: 'Disabled', type: 'bool', optional: true, releaseVersion: null },
        { name: 'ChildContent', type: 'RenderFragment', optional: true },
        { name: 'OnClick', type: 'EventCallback<MouseEventArgs>', optional: true, releaseVersion: '1.6.3' },
      ],
    }],
  };

  const result = mapReactApiToBlazor(reactApi, blazorApi);
  const props = result.components[0].props;
  assert.equal(result.summary.reactProps, 4);
  assert.deepEqual(props.map(property => property.status), ['exact', 'alias', 'exact', 'missing']);
  assert.equal(props[1].blazorParameter, 'ChildContent');
  assert.equal(props[2].blazorReleaseVersion, '1.6.3');
  assert.equal(props[3].blazorParameter, null);
});

test('does not treat demo-local helper props as a public component API', () => {
  const result = mapReactApiToBlazor({
    props: [
      {
        component: 'select',
        declaration: 'SelectProps',
        sourceFile: 'components/select/index.tsx',
        properties: [{ name: 'mode', type: 'SelectMode', kind: 'value' }],
      },
      {
        component: 'select',
        declaration: 'DebounceSelectProps',
        sourceFile: 'components/select/demo/select-users.tsx',
        properties: [{ name: 'fetchOptions', type: 'FetchOptions', kind: 'callback' }],
      },
    ],
  }, {
    components: [{ component: 'select', parameters: [{ name: 'Mode', type: 'SelectMode', optional: true }] }],
  });

  assert.deepEqual(result.components[0].declarations, ['SelectProps']);
  assert.deepEqual(result.components[0].props.map(property => property.reactProp), ['mode']);
  assert.equal(result.summary.missing, 0);
});

test('applies semantic aliases only in their declared component scope', () => {
  const result = mapReactApiToBlazor({
    props: [{
      component: 'progress',
      declaration: 'ProgressProps',
      properties: [{ name: 'railColor', type: 'string', kind: 'value' }],
    }],
  }, {
    components: [{ component: 'progress', parameters: [{ name: 'TrailColor', type: 'string', optional: true }] }],
  });
  assert.equal(result.components[0].props[0].status, 'alias');
  assert.equal(result.components[0].props[0].blazorParameter, 'TrailColor');
});

test('maps controlled visibility and open-state callbacks to Blazor counterparts', () => {
  const result = mapReactApiToBlazor({
    props: [
      { component: 'drawer', declaration: 'DrawerProps', properties: [{ name: 'open', type: 'boolean', kind: 'value' }] },
      { component: 'modal', declaration: 'ModalProps', properties: [{ name: 'open', type: 'boolean', kind: 'value' }] },
      { component: 'select', declaration: 'SelectProps', properties: [{ name: 'onOpenChange', type: '(open: boolean) => void', kind: 'callback' }] },
    ],
  }, {
    components: [
      { component: 'drawer', parameters: [{ name: 'Visible', type: 'bool', optional: true }] },
      { component: 'modal', parameters: [{ name: 'Visible', type: 'bool', optional: true }] },
      { component: 'select', parameters: [{ name: 'OpenChanged', type: 'EventCallback<bool>', optional: true }] },
    ],
  });
  assert.deepEqual(result.components.flatMap(c => c.props.map(p => [p.status, p.blazorParameter])), [
    ['alias', 'Visible'], ['alias', 'Visible'], ['alias', 'OpenChanged'],
  ]);
});

test('maps established component API renames without global name guessing', () => {
  const result = mapReactApiToBlazor({
    props: [
      { component: 'alert', declaration: 'AlertProps', properties: [{ name: 'title', type: 'ReactNode', kind: 'value' }, { name: 'isClosable', type: 'boolean', kind: 'value' }] },
      { component: 'button', declaration: 'ButtonProps', properties: [{ name: 'href', type: 'string', kind: 'value' }] },
      { component: 'form', declaration: 'FormItemProps', properties: [{ name: 'isRequired', type: 'boolean', kind: 'value' }] },
    ],
  }, {
    components: [
      { component: 'alert', parameters: [{ name: 'Message', type: 'string', optional: true }, { name: 'Closable', type: 'bool', optional: true }] },
      { component: 'button', parameters: [{ name: 'Url', type: 'string', optional: true }] },
      { component: 'form', parameters: [{ name: 'Required', type: 'bool', optional: true }] },
    ],
  });
  assert.deepEqual(result.components.flatMap(c => c.props.map(p => p.blazorParameter)), ['Message', 'Closable', 'Url', 'Required']);
});

test('maps Select onChange to mode-specific value callbacks', () => {
  const result = mapReactApiToBlazor({
    props: [{
      component: 'select',
      declaration: 'SelectProps',
      properties: [{ name: 'onChange', type: '(value, option) => void', kind: 'callback' }],
    }],
  }, {
    components: [{
      component: 'select',
      parameters: [
        { name: 'ValueChanged', type: 'EventCallback<TItemValue>' },
        { name: 'ValuesChanged', type: 'EventCallback<IEnumerable<TItemValue>>' },
      ],
    }],
  });

  assert.equal(result.components[0].props[0].status, 'alias');
  assert.equal(result.components[0].props[0].blazorParameter, 'ValueChanged');
  assert.match(result.components[0].props[0].shapeNote, /ValueChanged.*ValuesChanged/);
});

test('maps React Tag.CheckableTag onChange to Blazor CheckedChanged in Tag scope', () => {
  const result = mapReactApiToBlazor({
    props: [{
      component: 'tag',
      declaration: 'CheckableTagProps',
      properties: [{ name: 'onChange', type: '(checked: boolean) => void', kind: 'callback' }],
    }],
  }, {
    components: [{ component: 'tag', parameters: [{ name: 'CheckedChanged', type: 'EventCallback<bool>' }] }],
  });

  assert.equal(result.components[0].props[0].status, 'alias');
  assert.equal(result.components[0].props[0].blazorParameter, 'CheckedChanged');
});

test('maps React Table Column.filterOnClose to the Blazor Column API', () => {
  const result = mapReactApiToBlazor({
    props: [{
      component: 'table',
      declaration: 'ColumnProps',
      sourceFile: 'components/table/Column.ts',
      properties: [{ name: 'filterOnClose', type: 'boolean', kind: 'value' }],
    }],
  }, {
    components: [{
      component: 'table',
      parameters: [{ name: 'FilterOnClose', type: 'bool', optional: true }],
    }],
  });

  assert.deepEqual(result.components[0].props[0], {
    reactProp: 'filterOnClose',
    reactType: 'boolean',
    kind: 'value',
    blazorComponent: 'table',
    blazorParameter: 'FilterOnClose',
    blazorType: 'bool',
    blazorReleaseVersion: null,
    status: 'exact',
    reason: 'case-insensitive-name-match',
    shapeNote: null,
  });
});

test('maps React Table filterResetToDefaultFilteredValue to the Blazor Column API', () => {
  const result = mapReactApiToBlazor(
    { props: [{ component: 'table', declaration: 'ColumnProps', sourceFile: 'components/table/Column.ts', properties: [{ name: 'filterResetToDefaultFilteredValue', type: 'boolean', kind: 'value' }] }] },
    { components: [{ component: 'table', parameters: [{ name: 'FilterResetToDefaultFilteredValue', type: 'bool' }] }] },
  );

  assert.deepEqual(result.components[0].props[0], {
    reactProp: 'filterResetToDefaultFilteredValue',
    reactType: 'boolean',
    kind: 'value',
    blazorComponent: 'table',
    blazorParameter: 'FilterResetToDefaultFilteredValue',
    blazorType: 'bool',
    blazorReleaseVersion: null,
    status: 'exact',
    reason: 'case-insensitive-name-match',
    shapeNote: null,
  });
});

test('maps the React TimePicker API to the inherited Blazor DatePicker surface', () => {
  const result = mapReactApiToBlazor({
    props: [{
      component: 'time-picker',
      declaration: 'TimePickerProps',
      properties: [{ name: 'use12Hours', type: 'boolean', kind: 'value' }],
    }],
  }, {
    components: [{ component: 'date-picker', parameters: [{ name: 'Use12Hours', type: 'bool' }] }],
  });

  assert.equal(result.components[0].status, 'mapped');
  assert.equal(result.components[0].blazorComponent, 'date-picker');
  assert.equal(result.components[0].props[0].status, 'exact');
  assert.equal(result.components[0].props[0].blazorParameter, 'Use12Hours');
});

test('maps only public React Table props and column declarations', () => {
  const result = mapReactApiToBlazor({
    props: [
      { component: 'table', declaration: 'TableProps', sourceFile: 'components/table/InternalTable.tsx', properties: [{ name: 'dataSource', type: 'Record[]', kind: 'value' }] },
      { component: 'table', declaration: 'ColumnProps', sourceFile: 'components/table/Column.ts', properties: [{ name: 'filterOnClose', type: 'boolean', kind: 'value' }] },
      { component: 'table', declaration: 'FilterDropdownProps', sourceFile: 'components/table/hooks/useFilter/FilterDropdown.tsx', properties: [{ name: 'confirm', type: '() => void', kind: 'callback' }] },
      { component: 'table', declaration: 'SorterTooltipProps', sourceFile: 'components/table/interface.ts', properties: [{ name: 'target', type: 'string', kind: 'value' }] },
    ],
  }, {
    components: [{ component: 'table', parameters: [{ name: 'DataSource', type: 'IEnumerable<T>' }, { name: 'FilterOnClose', type: 'bool' }] }],
  });

  assert.deepEqual(result.components[0].declarations, ['TableProps', 'ColumnProps']);
  assert.deepEqual(result.components[0].props.map(property => property.reactProp), ['dataSource', 'filterOnClose']);
});
