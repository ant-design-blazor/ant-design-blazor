import test from 'node:test';
import assert from 'node:assert/strict';
import { extractReactV6ApiMigrations } from '../src/extract-react-v6-api-migrations.mjs';

test('extracts React v6 deprecated API replacements', () => {
  const result = extractReactV6ApiMigrations(`
- \`Drawer\`
  - \`width\` is deprecated and replaced by \`size\`.
  - \`maskClosable\` is deprecated and replaced by \`mask.closable\`. (6.3.0)
- \`Progress\`
  - \`trailColor\` is deprecated and replaced by \`railColor\`.
`);
  assert.equal(result.summary.migrations, 3);
  assert.deepEqual(result.migrations.map(item => [item.component, item.legacy, item.replacement]), [
    ['Drawer', 'width', 'size'],
    ['Drawer', 'maskClosable', 'mask.closable'],
    ['Progress', 'trailColor', 'railColor'],
  ]);
  assert.equal(result.migrations[1].since, '6.3.0');
});
