import assert from 'node:assert/strict';
import { readFile } from 'node:fs/promises';
import test from 'node:test';
import Ajv from 'ajv';

const schema = JSON.parse(
  await readFile(new URL('../schema/style-ir.schema.json', import.meta.url), 'utf8'),
);
const fixture = JSON.parse(
  await readFile(new URL('./fixtures/button-style-ir.json', import.meta.url), 'utf8'),
);
const validate = new Ajv({ allErrors: true, strict: false }).compile(schema);

test('accepts a representative Button Token and split-style IR', () => {
  assert.equal(validate(fixture), true, JSON.stringify(validate.errors));
});

test('rejects an invalid revision and unknown token kind', () => {
  const invalid = structuredClone(fixture);
  invalid.upstream.revision = 'not-a-revision';
  invalid.tokens[0].kind = 'derived';

  assert.equal(validate(invalid), false);
  assert.ok(validate.errors?.some((error) => error.dataPath === '.upstream.revision'));
  assert.ok(validate.errors?.some((error) => error.dataPath === '.tokens[0].kind'));
});

test('rejects styles that do not declare a build layer', () => {
  const invalid = structuredClone(fixture);
  delete invalid.styles[0].layer;

  assert.equal(validate(invalid), false);
  assert.ok(validate.errors?.some((error) => error.dataPath === '.styles[0]'));
});
