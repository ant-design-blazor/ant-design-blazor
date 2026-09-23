import test from 'node:test';
import assert from 'node:assert/strict';
import { compareReactApis } from '../src/extract-react-tag-api-diff.mjs';

test('extracts added and removed React props from tag API snapshots', () => {
  const result = compareReactApis({ props: [
    { component: 'drawer', properties: [{ name: 'width' }] },
    { component: 'button', properties: [{ name: 'iconPosition' }] },
  ] }, { props: [
    { component: 'drawer', properties: [{ name: 'size' }] },
    { component: 'button', properties: [{ name: 'iconPlacement' }] },
  ] }, '4.24.16', '6.6.4');
  assert.deepEqual(result.summary, { components: 2, removedProperties: 2, addedProperties: 2 });
  assert.deepEqual(result.components[0], { component: 'button', removed: ['iconPosition'], added: ['iconPlacement'] });
});
