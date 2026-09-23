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
        { name: 'Disabled', type: 'bool', optional: true },
        { name: 'ChildContent', type: 'RenderFragment', optional: true },
        { name: 'OnClick', type: 'EventCallback<MouseEventArgs>', optional: true },
      ],
    }],
  };

  const result = mapReactApiToBlazor(reactApi, blazorApi);
  const props = result.components[0].props;
  assert.equal(result.summary.reactProps, 4);
  assert.deepEqual(props.map(property => property.status), ['exact', 'alias', 'exact', 'missing']);
  assert.equal(props[1].blazorParameter, 'ChildContent');
  assert.equal(props[3].blazorParameter, null);
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
