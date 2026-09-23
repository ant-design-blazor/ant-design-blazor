import test from 'node:test';
import assert from 'node:assert/strict';
import { extractReactApi } from '../src/extract-react-api-to-json.mjs';

test('extracts React props as the canonical API source', () => {
  const result = extractReactApi('/Volumes/Data/GitHub/ant-design');
  assert.equal(result.schemaVersion, 1);
  assert.ok(result.summary.components >= 70);
  assert.ok(result.summary.callbacks >= 200);

  const button = result.props.find((item) => item.component === 'button' && item.declaration === 'ButtonProps');
  assert.ok(button);
  assert.ok(button.properties.some((property) => property.name === 'loading'));
  assert.ok(result.summary.callbacks > 0);
});
