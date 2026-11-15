import assert from 'node:assert/strict';
import { mkdtemp, mkdir, readFile, rm, writeFile } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import test from 'node:test';
import { generateIconCategoriesSource, parseAntDesignIconCategories, syncAntDesignIconCategories } from '../src/icon-categories.mjs';

const source = `
const direction = ['StepBackward', 'ArrowUp'];
const logo = ['AntDesign'];
const all = ['StepBackward', 'ArrowUp', 'AntDesign', 'NewIcon'];
const datum = [...direction, ...logo];
const other = all.filter((name) => !datum.includes(name));
export const categories = {
  direction,
  logo,
  other,
};
`;

test('parses upstream categories and preserves export order for computed other icons', () => {
  assert.equal(
    parseAntDesignIconCategories(source, 'fixture/fields.ts', ['StepBackwardOutlined', 'ArrowUpOutlined', 'AntDesignOutlined', 'NewIconOutlined']),
    '# Generated from Ant Design documentation; do not edit by hand.\n'
      + '# Source: fixture/fields.ts\n'
      + 'direction\tStepBackward\n'
      + 'direction\tArrowUp\n'
      + 'logo\tAntDesign\n'
      + 'other\tNewIcon\n',
  );
});

test('generates the docs-side category catalog from synchronized TSV', () => {
  const generated = generateIconCategoriesSource(parseAntDesignIconCategories(source));
  assert.match(generated, /Categories .*"direction", "logo", "other"/);
  assert.match(generated, /Categories .*"direction", "logo", "other"\s*\}\);/);
  assert.match(generated, /\["StepBackward"\] = \("direction", 0\)/);
  assert.match(generated, /\["AntDesign"\] = \("logo", 2\)/);
  assert.match(generated, /GetCategory\(string componentName\)/);
  assert.match(generated, /GetThemeOrder\(string componentName\)/);
});

test('syncs category TSV only into generated docs output', async (t) => {
  const root = await mkdtemp(join(tmpdir(), 'antd-icon-categories-'));
  t.after(() => rm(root, { recursive: true, force: true }));

  const antdPath = join(root, 'ant-design');
  const sourcePath = join(antdPath, '.dumi/theme/builtins/IconSearch/fields.ts');
  await mkdir(join(antdPath, '.dumi/theme/builtins/IconSearch'), { recursive: true });
  await writeFile(sourcePath, source, 'utf8');
  const iconModulePath = join(antdPath, 'node_modules/@ant-design/icons/lib/icons');
  await mkdir(iconModulePath, { recursive: true });
  await writeFile(join(iconModulePath, 'index.js'), 'module.exports = { StepBackwardOutlined: {}, ArrowUpOutlined: {}, AntDesignOutlined: {}, NewIconOutlined: {} };', 'utf8');

  const outputPath = join(root, 'generated/icon-categories.tsv');
  const result = await syncAntDesignIconCategories({ antdPath, outputPath });

  assert.equal(result.entries, 4);
  assert.deepEqual(result.outputs, [outputPath]);
  assert.equal(await readFile(outputPath, 'utf8'), parseAntDesignIconCategories(source, '.dumi/theme/builtins/IconSearch/fields.ts', ['StepBackwardOutlined', 'ArrowUpOutlined', 'AntDesignOutlined', 'NewIconOutlined']));
});
