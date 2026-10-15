import { mkdir, readFile, writeFile } from 'node:fs/promises';
import { dirname, join, resolve } from 'node:path';

const sourceRelativePath = '.dumi/theme/builtins/IconSearch/fields.ts';

export function parseAntDesignIconCategories(source, sourceLabel = sourceRelativePath) {
  const categoryBlock = source.match(/export\s+const\s+categories\s*=\s*\{([\s\S]*?)\};/);
  if (!categoryBlock) {
    throw new Error(`Could not find the categories export in ${sourceLabel}`);
  }

  const categoryNames = [...categoryBlock[1].matchAll(/^\s*([A-Za-z_$][\w$]*)\s*,?\s*$/gm)]
    .map((match) => match[1]);
  if (categoryNames.length === 0) {
    throw new Error(`No category names were found in ${sourceLabel}`);
  }

  const rows = [
    '# Generated from Ant Design documentation; do not edit by hand.',
    `# Source: ${sourceLabel}`,
  ];

  for (const category of categoryNames) {
    if (category === 'other') continue; // Computed by the icon generator from the SVG inventory.

    const categoryArray = source.match(new RegExp(`(?:const|let)\\s+${category}\\s*=\\s*\\[([\\s\\S]*?)\\];`));
    if (!categoryArray) {
      throw new Error(`Category "${category}" is not backed by a literal array in ${sourceLabel}`);
    }

    const iconNames = [...categoryArray[1].matchAll(/(['"])([^'"\n]+)\1/g)].map((match) => match[2]);
    if (iconNames.length === 0) {
      throw new Error(`Category "${category}" contains no icon names in ${sourceLabel}`);
    }

    rows.push(...iconNames.map((iconName) => `${category}\t${iconName}`));
  }

  return `${rows.join('\n')}\n`;
}

export async function syncAntDesignIconCategories({ antdPath, outputPath, iconsBlazorPath }) {
  const sourcePath = resolve(antdPath, sourceRelativePath);
  const source = await readFile(sourcePath, 'utf8');
  const tsv = parseAntDesignIconCategories(source, sourceRelativePath);

  const outputs = [];
  for (const path of [outputPath, iconsBlazorPath ? join(iconsBlazorPath, 'eng', 'icon-categories.tsv') : null]) {
    if (!path) continue;
    const absolutePath = resolve(path);
    await mkdir(dirname(absolutePath), { recursive: true });
    await writeFile(absolutePath, tsv, 'utf8');
    outputs.push(absolutePath);
  }

  return {
    source: sourcePath,
    entries: tsv.split('\n').filter((line) => line && !line.startsWith('#')).length,
    outputs,
  };
}
