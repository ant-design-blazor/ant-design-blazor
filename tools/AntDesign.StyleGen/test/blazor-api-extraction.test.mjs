import test from 'node:test';
import assert from 'node:assert/strict';
import { extractParameters } from '../src/extract-blazor-api-to-json.mjs';

test('extracts Blazor parameters with extra attributes and custom accessors', () => {
  const parameters = extractParameters(`
    [Parameter]
    [Obsolete]
    public int Count
    {
      get => _count;
      set => _count = value;
    }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> Attributes { get; set; }
  `);
  assert.deepEqual(parameters.map(parameter => parameter.name), ['Count', 'Attributes']);
  assert.equal(parameters[0].type, 'int');
  assert.equal(parameters[1].type, 'Dictionary<string, object>');
});
