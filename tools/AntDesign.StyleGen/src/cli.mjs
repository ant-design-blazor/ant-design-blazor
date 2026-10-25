#!/usr/bin/env node

import { copyFile, mkdir, readFile, readdir, writeFile } from 'node:fs/promises';
import { existsSync } from 'node:fs';
import { dirname, join, relative, resolve } from 'node:path';
import { spawnSync } from 'node:child_process';
import { createArtifact, createBaseline, readBaseline, sha256, writeBaseline } from './baseline.mjs';
import { compareArtifactSets, compareCss } from './style-diff.mjs';
import { generateReactCssBaseline } from './react-css-baseline.mjs';
import { generateIconCategoriesSource, syncAntDesignIconCategories } from './icon-categories.mjs';
import { extractComponentTokenDefinitions } from './component-tokens.mjs';
import { syncGlobalTokenDocs } from './global-token-docs.mjs';
import { renderComponentGlobalTokenCatalog } from './component-global-tokens.mjs';
import { renderSeedTokenDefaults } from './seed-token-defaults.mjs';
import { generateReactThemeReference } from './react-theme-reference.mjs';
import { extractPreviewThemeMetadata, extractPreviewAlgorithm, extractCompatibleV4Defaults } from './preview-theme-metadata.mjs';
import { syncColorSpec } from './color-spec-catalog.mjs';

const usage = `Usage:
  node src/cli.mjs sync --antd-path <path> [--blazor-path <path>] [--revision <revision>] --output <path>
  node src/cli.mjs baseline-update --antd-path <path> [--revision <revision>] --output <path>
  node src/cli.mjs generate-csharp-tokens --ir <style-ir.json> [--antd-path <path>] --output <GeneratedComponentTokens.cs>
  node src/cli.mjs generate-csharp-theme-tokens --antd-path <path> --output <GeneratedThemeTokens.cs>
  node src/cli.mjs generate-theme-reference --antd-path <path> --output <react-theme-tokens.json>
  node src/cli.mjs sync-color-spec --antd-path <path> --output <ColorSpecCatalog.g.cs> [--check]
  node src/cli.mjs generate-preview-theme-catalog --ir <themes.json> --output <GeneratedPreviewThemes.cs>
  node src/cli.mjs sync-v6 --antd-path <path> [--blazor-path <path>] [--output <path>]
  node src/cli.mjs sync-semantic-dom --antd-path <path> [--blazor-path <path>] [--output <semantic-dom-catalog.json>] [--semantic-docs-path <react-_site>] [--semantic-md-output <path>]
  node src/cli.mjs css-baseline-update --antd-path <path> --output <path> [--docs-theme-output <path>]
  node src/cli.mjs exact-check --expected <baseline-dir> --actual <candidate-dir> [--allowlist <id,id>] [--report <path>]
  node src/cli.mjs theme-exact-check --expected <react-theme-css-dir> --actual <blazor-theme-css-dir> [--themes <key,key>] [--report <path>]
`;

function parseArgs(argv) {
  const args = {};
  for (let index = 0; index < argv.length; index += 1) {
    const value = argv[index];
    if (!value.startsWith('--')) {
      args.command ??= value;
      continue;
    }
    const key = value.slice(2);
    const next = argv[index + 1];
    if (!next || next.startsWith('--')) {
      args[key] = true;
    } else {
      args[key] = next;
      index += 1;
    }
  }
  return args;
}

function fail(message) {
  console.error(`antdesign-style: ${message}`);
  console.error(usage);
  process.exitCode = 1;
}

function git(path, args) {
  const result = spawnSync('git', ['-C', path, ...args], { encoding: 'utf8' });
  if (result.status !== 0) {
    throw new Error(result.stderr.trim() || `git ${args.join(' ')} failed`);
  }
  return result.stdout.trim();
}

function runReactExtractor(root) {
  const extractor = join(root, 'scripts', 'build-style.tsx');
  if (!existsSync(extractor)) throw new Error(`React extractor not found: ${extractor}`);
  const packageManager = existsSync(join(root, 'pnpm-lock.yaml')) ? 'pnpm' : 'npm';
  const result = spawnSync(packageManager, ['exec', 'tsx', 'scripts/build-style.tsx'], {
    cwd: root,
    encoding: 'utf8',
    stdio: ['ignore', 'pipe', 'pipe'],
  });
  if (result.status !== 0) {
    const details = (result.stderr || result.stdout || '').trim();
    throw new Error(`React CSS extraction failed. Install the ant-design checkout dependencies and retry.${details ? `\n${details}` : ''}`);
  }
  const cssPath = join(root, 'components', 'style', 'antd.css');
  if (!existsSync(cssPath)) throw new Error(`React extractor completed without producing ${cssPath}`);
  return cssPath;
}

function kebabCase(value) {
  return value
    .replace(/([a-z0-9])([A-Z])/g, '$1-$2')
    .replace(/[^A-Za-z0-9]+/g, '-')
    .replace(/^-|-$/g, '')
    .toLowerCase();
}

function normalizeTokenType(name, sourceType) {
  const type = String(sourceType ?? '').toLowerCase();
  if (type.includes('color') || /color|background|border/i.test(name)) return 'color';
  if (type.includes('shadow')) return 'shadow';
  if (type.includes('font')) return 'font';
  if (type.includes('number') || type === 'number') return 'number';
  if (type.includes('motion') || type.includes('duration')) return 'motion';
  if (type.includes('length') || type.includes('size') || type.includes('padding') || type.includes('margin')) return 'length';
  if (type.includes('string')) return 'string';
  return 'unknown';
}

async function readJson(path, label) {
  try {
    return JSON.parse(await readFile(path, 'utf8'));
  } catch (error) {
    throw new Error(`cannot read ${label}: ${error.message}`);
  }
}

async function discoverComponents(componentsPath, root) {
  const entries = await readdir(componentsPath, { withFileTypes: true });
  const components = [];
  for (const entry of entries) {
    if (!entry.isDirectory() || entry.name.startsWith('_')) continue;
    const componentPath = join(componentsPath, entry.name);
    const files = await readdir(componentPath, { withFileTypes: true });
    const stylePath = files.some((file) => file.name === 'style' && file.isDirectory())
      ? join(componentPath, 'style', 'index.ts')
      : null;
    if (!stylePath) continue;
    const styleRelativePath = relative(root, stylePath).replaceAll('\\', '/');
    const candidateTokenFile = join(componentPath, 'style', 'token.ts');
    // A few components (including Modal) declare ComponentToken beside the
    // style generators in index.ts instead of a separate token.ts file.
    const tokenFile = existsSync(candidateTokenFile) ? candidateTokenFile : stylePath;
    const tokenDefinitions = tokenFile ? await discoverTokenDefinitions(tokenFile) : [];
    const hasTokenSource = tokenDefinitions.length > 0;
    const tokenRelativePath = hasTokenSource ? relative(root, tokenFile).replaceAll('\\', '/') : null;
    const docsPath = join(componentPath, 'index.en-US.md');
    const docsSource = existsSync(docsPath) ? await readFile(docsPath, 'utf8') : '';
    components.push({
      name: entry.name,
      className: `ant-${kebabCase(entry.name)}`,
      styleSource: styleRelativePath,
      tokenSource: tokenRelativePath,
      hasTokenSource,
      hasTokenTable: /<ComponentTokenTable\b/.test(docsSource),
      tokenDefinitions,
    });
  }
  return components.sort((left, right) => left.name.localeCompare(right.name));
}

async function discoverTokenDefinitions(tokenPath) {
  const source = await readFile(tokenPath, 'utf8');
  return extractComponentTokenDefinitions(source);
}

async function discoverBlazorComponents(componentsPath) {
  const entries = await readdir(componentsPath, { withFileTypes: true });
  return new Set(entries
    .filter((entry) => entry.isDirectory() && !entry.name.startsWith('_'))
    .map((entry) => entry.name.toLowerCase()));
}

async function discoverThemes(themePath, root) {
  const indexPath = join(themePath, 'index.ts');
  const source = await readFile(indexPath, 'utf8');
  const themeNames = [...source.matchAll(/name:\s+locale\.([A-Za-z0-9]+)/g)].map((match) => match[1]);
  const sourcePath = relative(root, indexPath).replaceAll('\\', '/');
  const labels = {
    default: 'Ant Design',
    dark: 'Dark',
    mui: 'MUI',
    shadcn: 'shadcn',
    bootstrap: 'Bootstrap',
    cartoon: 'Cartoon',
    illustration: 'Illustration',
    glass: 'Glass',
    geek: 'Geek',
    lark: 'Document',
    blossom: 'Blossom',
    v4: 'Ant Design V4',
    serene: 'Serene',
  };
  return themeNames.map((key) => ({
    key: key === 'default' ? 'light' : key,
    name: labels[key] ?? key,
    appearance: key === 'dark' ? 'dark' : 'light',
    source: sourcePath,
  }));
}

async function sync(args) {
  if (!args['antd-path'] || !args.output) throw new Error('sync requires --antd-path and --output');

  const root = resolve(args['antd-path']);
  const output = resolve(args.output);
  const packageJson = await readJson(join(root, 'package.json'), 'ant-design package.json');
  if (packageJson.name !== 'antd') throw new Error(`expected package name "antd", got "${packageJson.name ?? ''}"`);

  const revision = args.revision ? git(root, ['rev-parse', '--verify', `${args.revision}^{commit}`]) : git(root, ['rev-parse', 'HEAD']);
  const version = packageJson.version;
  const components = await discoverComponents(join(root, 'components'), root);
  const blazorRoot = args['blazor-path']
    ? resolve(args['blazor-path'])
    : resolve(new URL('../../../', import.meta.url).pathname);
  const blazorComponents = await discoverBlazorComponents(join(blazorRoot, 'components'));
  for (const component of components) {
    component.implementedInBlazor = blazorComponents.has(component.name.toLowerCase());
  }
  const themes = await discoverThemes(join(root, '.dumi/pages/index/components/ThemePreview/previewThemes'), root);
  const tokenMetaPath = join(root, 'components/version/token-meta.json');
  const tokenDataPath = join(root, 'components/version/token.json');
  const tokenMeta = existsSync(tokenMetaPath) ? await readJson(tokenMetaPath, 'React Design Token metadata') : null;
  const tokenData = existsSync(tokenDataPath) ? await readJson(tokenDataPath, 'React Design Token values') : null;
  const normalizeTokenOwner = (value) => String(value).replace(/[^a-z0-9]/gi, '').toLowerCase();
  const componentTokenMetadata = new Map(Object.entries(tokenMeta?.components ?? {}).map(([name, definitions]) => [normalizeTokenOwner(name), definitions]));
  const generatedAt = args['generated-at'] ?? new Date().toISOString();
  const upstream = { repository: 'ant-design', revision, version };
  const generator = { name: '@ant-design-blazor/style-generator', version: '0.1.0' };

  const styles = components.map((component) => ({
    component: component.name,
    layer: 'static',
    selector: `.${component.className}`,
    declarations: [],
    atRules: [],
    source: component.styleSource,
  }));
  const tokens = components.flatMap((component) => {
    const metadata = componentTokenMetadata.get(normalizeTokenOwner(component.name)) ?? [];
    const definitions = [...component.tokenDefinitions];
    for (const item of metadata) {
      if (!definitions.some((definition) => definition.name === item.token)) {
        definitions.push({
          name: item.token,
          type: item.type ?? 'unknown',
          description: item.desc,
          descriptionEn: item.descEn,
        });
      }
    }
    if (definitions.length === 0) definitions.push({ name: 'componentToken', type: 'unknown' });
    return definitions.map(({ name, type, description, descriptionEn, deprecated }) => {
      const docsDefinition = metadata.find((item) => item.token === name);
      const value = docsDefinition ? tokenData?.[docsDefinition.source]?.component?.[name] : undefined;
      const defaultValue = value === undefined || value === null
        ? undefined
        : typeof value === 'object' ? JSON.stringify(value) : String(value);
      return {
      name,
      kind: 'component',
      type: normalizeTokenType(name, type),
      sourceType: type,
      ...((docsDefinition?.desc ?? description) ? { description: docsDefinition?.desc ?? description } : {}),
      ...((docsDefinition?.descEn ?? descriptionEn) ? { descriptionEn: docsDefinition?.descEn ?? descriptionEn } : {}),
      ...(deprecated ? { deprecated: true } : {}),
      ...(defaultValue !== undefined ? { defaultValue } : {}),
      documented: Boolean(component.hasTokenTable && docsDefinition),
      value: null,
      cssVariable: `--ant-${kebabCase(component.name)}-${kebabCase(name)}`,
      component: component.name,
      source: component.tokenSource ?? component.styleSource,
      reserved: !component.implementedInBlazor || (!component.hasTokenSource && !docsDefinition),
      };
    });
  });
  const ir = { schemaVersion: 1, upstream, generator, tokens, styles, themes, unsupported: [] };
  const schema = await readJson(join(new URL('..', import.meta.url).pathname, 'schema/style-ir.schema.json'), 'style IR schema');
  const { default: Ajv } = await import('ajv');
  const validate = new Ajv({ allErrors: true, strict: false }).compile(schema);
  if (!validate(ir)) {
    throw new Error(`generated IR does not satisfy schema: ${JSON.stringify(validate.errors)}`);
  }
  const manifest = {
    schemaVersion: 1,
    upstream,
    generator,
    generatedAt,
    sourcePath: args['source-path'] ?? root,
    artifacts: {
      ir: 'style-ir.json',
      components: 'components.json',
      themes: 'themes.json',
      unsupported: 'unsupported.json',
    },
  };

  await mkdir(output, { recursive: true });
  await writeFile(join(output, 'style-ir.json'), `${JSON.stringify(ir, null, 2)}\n`, 'utf8');
  await writeFile(join(output, 'manifest.json'), `${JSON.stringify(manifest, null, 2)}\n`, 'utf8');
  await writeFile(join(output, 'components.json'), `${JSON.stringify(components, null, 2)}\n`, 'utf8');
  await writeFile(join(output, 'themes.json'), `${JSON.stringify(themes, null, 2)}\n`, 'utf8');
  await writeFile(join(output, 'unsupported.json'), '[]\n', 'utf8');
  console.log(JSON.stringify({ output, revision, version, components: components.length, themes: themes.length }, null, 2));
}

async function updateBaseline(args) {
  if (!args['antd-path'] || !args.output) throw new Error('baseline-update requires --antd-path and --output');
  const root = resolve(args['antd-path']);
  const output = resolve(args.output);
  const packageJson = await readJson(join(root, 'package.json'), 'ant-design package.json');
  if (packageJson.name !== 'antd') throw new Error(`expected package name "antd", got "${packageJson.name ?? ''}"`);
  const revision = args.revision ? git(root, ['rev-parse', '--verify', `${args.revision}^{commit}`]) : git(root, ['rev-parse', 'HEAD']);
  const cssById = new Map();
  const artifacts = [];
  const cssPath = args['react-css'] ? resolve(args['react-css']) : null;
  const cssDir = args['react-css-dir'] ? resolve(args['react-css-dir']) : null;
  const cssInputs = cssDir
    ? (await readdir(cssDir, { withFileTypes: true })).filter((entry) => entry.isFile() && entry.name.endsWith('.css')).map((entry) => join(cssDir, entry.name))
    : [cssPath ?? runReactExtractor(root)];
  for (const input of cssInputs) {
    const css = await readFile(input, 'utf8');
    const fileName = input.split('/').at(-1).replace(/\.css$/, '');
    const parts = fileName.split('.');
    const layer = ['static', 'token-values', 'runtime-overrides'].includes(parts.at(-1)) ? parts.pop() : 'static';
    const theme = parts.length > 1 ? parts.pop() : 'light';
    const component = parts.join('.') || 'all';
    const id = `${component}.${theme}.${layer}`;
    const artifact = createArtifact({ id, path: `${layer}/${fileName}.css`, layer, component, theme, css, allowMissingTrailingNewline: true });
    artifacts.push(artifact);
    cssById.set(id, css);
  }
  const upstream = { repository: 'ant-design', revision, version: packageJson.version };
  const generator = { name: 'antd-react-static-style-extract', version: packageJson.version };
  const baseline = createBaseline({
    upstream,
    generator,
    inputDigest: sha256(`${revision}:${packageJson.version}:${args['input-id'] ?? cssPath}`),
    artifacts,
  });
  await writeBaseline(output, baseline, cssById);
  console.log(JSON.stringify({ output, source: cssInputs, revision, version: packageJson.version, artifacts: artifacts.length }, null, 2));
}

async function exactCheck(args) {
  if (!args.expected || !args.actual) throw new Error('exact-check requires --expected and --actual');
  const expected = await readBaseline(resolve(args.expected));
  const actual = await readBaseline(resolve(args.actual));
  const allowlist = String(args.allowlist ?? '').split(',').map((item) => item.trim()).filter(Boolean);
  const result = compareArtifactSets({
    expectedManifest: expected.manifest,
    expectedCssById: expected.cssById,
    actualManifest: actual.manifest,
    actualCssById: actual.cssById,
    allowlist,
  });

  if (args['expected-full-css'] || args['actual-full-css']) {
    if (!args['expected-full-css'] || !args['actual-full-css']) throw new Error('full CSS comparison requires both --expected-full-css and --actual-full-css');
    const expectedFull = await readFile(resolve(args['expected-full-css']), 'utf8');
    const actualFull = await readFile(resolve(args['actual-full-css']), 'utf8');
    const fullResult = compareCss(expectedFull, actualFull, { id: 'full.css', layer: 'full', component: null, theme: null });
    if (!fullResult.equal) result.diagnostics.push(fullResult);
    result.equal = result.equal && fullResult.equal;
  }

  const report = { equal: result.equal, expected: resolve(args.expected), actual: resolve(args.actual), allowlist, diagnostics: result.diagnostics };
  if (args.report) {
    await mkdir(dirname(resolve(args.report)), { recursive: true });
    await writeFile(resolve(args.report), `${JSON.stringify(report, null, 2)}\n`, 'utf8');
  }
  console.log(JSON.stringify(report, null, 2));
  if (!result.equal) process.exitCode = 1;
}

async function themeExactCheck(args) {
  if (!args.expected || !args.actual) throw new Error('theme-exact-check requires --expected and --actual');
  const expectedRoot = resolve(args.expected);
  const actualRoot = resolve(args.actual);
  const explicitThemes = String(args.themes ?? '').split(',').map((item) => item.trim()).filter(Boolean);
  const discover = async (root) => (await readdir(root, { withFileTypes: true }))
    .filter((entry) => entry.isFile() && entry.name.endsWith('.css'))
    .map((entry) => entry.name.slice(0, -4));
  const expectedThemes = explicitThemes.length ? explicitThemes : await discover(expectedRoot);
  const actualThemes = await discover(actualRoot);
  const allThemes = [...new Set([...expectedThemes, ...actualThemes])].sort();
  const diagnostics = [];
  for (const theme of allThemes) {
    const expectedPath = join(expectedRoot, `${theme}.css`);
    const actualPath = join(actualRoot, `${theme}.css`);
    if (!existsSync(expectedPath)) {
      diagnostics.push({ id: `${theme}.full`, theme, kind: 'missing-expected', actual: actualPath });
      continue;
    }
    if (!existsSync(actualPath)) {
      diagnostics.push({ id: `${theme}.full`, theme, kind: 'missing-actual', expected: expectedPath });
      continue;
    }
    const result = compareCss(
      await readFile(expectedPath, 'utf8'),
      await readFile(actualPath, 'utf8'),
      { id: `${theme}.full`, layer: 'full', component: null, theme },
    );
    if (!result.equal) diagnostics.push(result);
  }
  const report = {
    equal: diagnostics.length === 0,
    expected: expectedRoot,
    actual: actualRoot,
    themes: allThemes,
    compared: allThemes.length - diagnostics.filter((item) => item.kind?.startsWith('missing-')).length,
    diagnostics,
  };
  if (args.report) {
    await mkdir(dirname(resolve(args.report)), { recursive: true });
    await writeFile(resolve(args.report), `${JSON.stringify(report, null, 2)}\n`, 'utf8');
  }
  console.log(JSON.stringify(report, null, 2));
  if (!report.equal) process.exitCode = 1;
}

async function generateCsharpTokens(args) {
  if (!args.ir || !args.output) throw new Error('generate-csharp-tokens requires --ir and --output');
  const ir = await readJson(resolve(args.ir), 'style IR');
  if (!Array.isArray(ir.tokens)) throw new Error('style IR tokens must be an array');
  const tokens = ir.tokens
    .filter((token) => token.kind === 'component' && token.component && token.name && token.cssVariable)
    .sort((left, right) => `${left.component}:${left.name}`.localeCompare(`${right.component}:${right.name}`));
  const quote = (value) => JSON.stringify(String(value));
  const pascal = (value) => String(value).replace(/(^|[-_\s])([A-Za-z0-9])/g, (_, __, character) => character.toUpperCase());
  const csharpType = (type) => {
    const source = String(type ?? 'unknown').replace(/\s+/g, ' ').trim();
    const members = source.includes('|') ? source.split('|').map((item) => item.trim()) : [source];
    const mapped = [...new Set(members.map((member) => {
      if (/^(string|CSSProperties\[|CSSObject\[|React\.CSSProperties)/.test(member)) return member === 'string' ? 'string' : 'OneOf<string, double>';
      if (/^(number|CSSNumericValue)/.test(member)) return 'double';
      if (member === 'boolean') return 'bool';
      if (/^(true|false)$/.test(member)) return 'bool';
      return 'string';
    }))];
    return mapped.length === 1 ? mapped[0] : `OneOf<${mapped.join(', ')}>`;
  };
  const byComponent = new Map();
  for (const token of tokens) (byComponent.get(token.component) ?? byComponent.set(token.component, []).get(token.component)).push(token);
  const compositions = args['antd-path'] ? await discoverStyleCompositions(resolve(args['antd-path'])) : [];
  const lines = [
    '// <auto-generated /> Generated by AntDesign.StyleGen. Do not edit manually.',
    `// Upstream: ${ir.upstream?.repository ?? 'ant-design'} ${ir.upstream?.revision ?? 'unknown'} (${ir.upstream?.version ?? 'unknown'})`,
    'using System.Collections.Generic;',
    'using System.Collections.ObjectModel;',
    'using OneOf;',
    'namespace AntDesign.Theming',
    '{',
    '    public sealed class ComponentTokenDefinition',
    '    {',
    '        public string Component { get; }',
    '        public string Name { get; }',
    '        public string CssVariable { get; }',
    '        public string Type { get; }',
    '        public string Description { get; }',
    '        public string DescriptionEn { get; }',
    '        public string DefaultValue { get; }',
    '        public bool Deprecated { get; }',
    '        public bool Documented { get; }',
    '        public bool Reserved { get; }',
    '        public ComponentTokenDefinition(string component, string name, string cssVariable, string type, string description, string descriptionEn, string defaultValue, bool deprecated, bool documented, bool reserved)',
    '        { Component = component; Name = name; CssVariable = cssVariable; Type = type; Description = description; DescriptionEn = descriptionEn; DefaultValue = defaultValue; Deprecated = deprecated; Documented = documented; Reserved = reserved; }',
    '        public ComponentTokenDefinition(string component, string name, string cssVariable, bool reserved)',
    '            : this(component, name, cssVariable, string.Empty, string.Empty, string.Empty, string.Empty, false, false, reserved) { }',
    '    }',
    '',
    ...[...byComponent.entries()].flatMap(([component, componentTokens]) => [
      `    public sealed class ${pascal(component)}ComponentToken`,
      '    {',
      ...componentTokens.map((token) => `        public ${csharpType(token.sourceType ?? token.type)} ${pascal(token.name)} { get; set; }`),
      '    }',
      '',
    ]),
    ...compositions.flatMap((composition) => [
      `    public sealed class ${pascal(composition.component)}StyleToken`,
      '    {',
      ...composition.members.map((member) => `        public ${member.type} ${pascal(member.name)} { get; set; } = new ${member.type}();`),
      `        public IReadOnlyList<string> StyleGenerators { get; } = ${composition.generators.length ? `new[] { ${composition.generators.map(quote).join(', ')} }` : 'System.Array.Empty<string>()'};`,
      '    }',
      '',
    ]),
    '    public sealed class GeneratedStyleTokenSet',
    '    {',
    ...compositions.map((composition) => `        public ${pascal(composition.component)}StyleToken ${pascal(composition.component)} { get; set; } = new ${pascal(composition.component)}StyleToken();`),
    '    }',
    '',
    '    public sealed class GeneratedComponentTokenSet',
    '    {',
    ...[...byComponent.keys()].map((component) => `        public ${pascal(component)}ComponentToken ${pascal(component)} { get; set; } = new ${pascal(component)}ComponentToken();`),
    '    }',
    '',
    '    public static class GeneratedComponentTokens',
    '    {',
    '        public static IReadOnlyList<ComponentTokenDefinition> All { get; } = new ReadOnlyCollection<ComponentTokenDefinition>(new[]',
    '        {',
    ...tokens.map((token) => `            new ComponentTokenDefinition(${quote(token.component)}, ${quote(token.name)}, ${quote(token.cssVariable)}, ${quote(token.sourceType ?? token.type)}, ${quote(token.description ?? '')}, ${quote(token.descriptionEn ?? '')}, ${quote(token.defaultValue ?? '')}, ${Boolean(token.deprecated).toString().toLowerCase()}, ${Boolean(token.documented).toString().toLowerCase()}, ${Boolean(token.reserved).toString().toLowerCase()}),`),
    '        });',
    '    }',
    '}',
    '',
  ];
  const output = resolve(args.output);
  await mkdir(dirname(output), { recursive: true });
  await writeFile(output, lines.join('\n'), 'utf8');
  console.log(JSON.stringify({ output, revision: ir.upstream?.revision, tokens: tokens.length, compositions: compositions.length }, null, 2));
}

async function generateCsharpThemeTokens(args) {
  if (!args['antd-path'] || !args.output) throw new Error('generate-csharp-theme-tokens requires --antd-path and --output');
  const root = resolve(args['antd-path']);
  const sources = [
    ['Seed', join(root, 'components/theme/interface/seeds.ts')],
    ['Map', join(root, 'components/theme/interface/maps/index.ts')],
    ['Alias', join(root, 'components/theme/interface/alias.ts')],
  ];
  const quote = (value) => JSON.stringify(String(value));
  const pascal = (value) => pascalCase(value);
  const typeOf = (source) => {
    const type = String(source).replace(/\s+/g, ' ').trim();
    if (type.includes('|')) return 'OneOf<string, double>';
    if (type === 'number') return 'double';
    if (type === 'boolean') return 'bool';
    return 'string';
  };
  const models = [];
  for (const [name, path] of sources) {
    const source = await readFile(path, 'utf8');
    const match = source.match(new RegExp(`interface\\s+${name}Token([^\\{]*)\\{([\\s\\S]*?)\\}`));
    const fields = match ? [...match[2].matchAll(/^\s*([A-Za-z_$][\w$]*)\??\s*:\s*([^;\n]+);?/gm)].map((item) => ({ name: item[1], type: typeOf(item[2]), sourceType: item[2].trim() })) : [];
    const bases = match?.[1]?.match(/extends\s+([\s\S]*)/)?.[1]?.split(',').map((item) => item.trim()).filter(Boolean) ?? [];
    models.push({ name, fields, bases });
  }
  const tokenMetaPath = join(root, 'components/version/token-meta.json');
  const tokenMetadata = existsSync(tokenMetaPath) ? await readJson(tokenMetaPath, 'React Design Token metadata') : null;
  const tokenDataPath = join(root, 'components/version/token.json');
  const tokenData = existsSync(tokenDataPath) ? await readJson(tokenDataPath, 'React component token usage') : {};
  const seedSource = await readFile(join(root, 'components/theme/themes/seed.ts'), 'utf8');
  const globalTokens = Object.entries(tokenMetadata?.global ?? {}).map(([name, token]) => ({
    name,
    description: token.desc ?? '',
    descriptionEn: token.descEn ?? '',
    type: token.type ?? 'unknown',
    source: token.source ?? 'other',
    cssVariable: `--ant-${name.replace(/([a-z0-9])([A-Z])/g, '$1-$2').replace(/([A-Z])([A-Z][a-z])/g, '$1-$2').toLowerCase()}`,
  }));
  const lines = [
    '// <auto-generated /> Generated by AntDesign.StyleGen. Do not edit manually.',
    'using System.Collections.Generic;',
    'using System.Collections.ObjectModel;',
    'using OneOf;',
    'namespace AntDesign.Theming',
    '{',
    ...models.flatMap((model) => [
      `    public sealed class Generated${model.name}Token`,
      '    {',
      ...model.bases.map((base) => {
        const name = base.replace(/<.*>/, '').replace(/[^A-Za-z0-9]/g, '');
        const type = name === 'SeedToken' ? 'GeneratedSeedToken' : name === 'MapToken' ? 'GeneratedMapToken' : 'TokenMap<string>';
        return `        public ${type} ${name || 'Base'} { get; set; } = new ${type}();`;
      }),
      ...model.fields.map((field) => `        public ${field.type} ${pascal(field.name)} { get; set; }`),
      '    }',
      '',
    ]),
    ...renderSeedTokenDefaults(seedSource).split('\n'),
    '    public sealed class GlobalTokenDefinition',
    '    {',
    '        public string Name { get; }',
    '        public string Description { get; }',
    '        public string DescriptionEn { get; }',
    '        public string Type { get; }',
    '        public string Source { get; }',
    '        public string CssVariable { get; }',
    '        public GlobalTokenDefinition(string name, string description, string descriptionEn, string type, string source, string cssVariable)',
    '        { Name = name; Description = description; DescriptionEn = descriptionEn; Type = type; Source = source; CssVariable = cssVariable; }',
    '    }',
    '',
    '    public static class GeneratedGlobalTokens',
    '    {',
    '        public static IReadOnlyList<GlobalTokenDefinition> All { get; } = new ReadOnlyCollection<GlobalTokenDefinition>(new[]',
    '        {',
    ...globalTokens.map((token) => `            new GlobalTokenDefinition(${quote(token.name)}, ${quote(token.description)}, ${quote(token.descriptionEn)}, ${quote(token.type)}, ${quote(token.source)}, ${quote(token.cssVariable)}),`),
    '        });',
    '    }',
    '',
    ...renderComponentGlobalTokenCatalog(tokenData, tokenMetadata ?? {}).split('\n'),
    '    public sealed class GeneratedThemeAlgorithmDefinition',
    '    {',
    '        public string Name { get; }',
    '        public string Source { get; }',
    '        public GeneratedThemeAlgorithmDefinition(string name, string source) { Name = name; Source = source; }',
    '    }',
    '',
    '    public static class GeneratedThemeAlgorithms',
    '    {',
    '        public static GeneratedThemeAlgorithmDefinition[] All { get; } = new[]',
    '        {',
    `            new GeneratedThemeAlgorithmDefinition(${quote('default')}, ${quote('components/theme/themes/default/index.ts')}),`,
    `            new GeneratedThemeAlgorithmDefinition(${quote('dark')}, ${quote('components/theme/themes/dark/index.ts')}),`,
    `            new GeneratedThemeAlgorithmDefinition(${quote('compact')}, ${quote('components/theme/themes/compact/index.ts')}),`,
    '        };',
    '    }',
    '',
    '    public static class ThemeAlgorithmFactory',
    '    {',
    '        public static IThemeAlgorithm Create(string name) => string.Equals(name, "dark", System.StringComparison.OrdinalIgnoreCase)',
    '            ? new DarkThemeAlgorithm()',
    '            : string.Equals(name, "compact", System.StringComparison.OrdinalIgnoreCase)',
    '                ? new CompactThemeAlgorithm()',
    '                : new DefaultThemeAlgorithm();',
    '    }',
    '}',
    '',
  ];
  const output = resolve(args.output);
  await mkdir(dirname(output), { recursive: true });
  await writeFile(output, lines.join('\n'), 'utf8');
  if (args['shortcuts-output']) {
    const mapFields = [
      'colorText', 'colorBgContainer', 'colorBorder', 'colorPrimaryBg', 'colorPrimaryBgHover', 'colorPrimaryBorder', 'colorPrimaryBorderHover', 'colorPrimaryHover', 'colorPrimaryActive', 'colorPrimaryTextHover', 'colorPrimaryText', 'colorPrimaryTextActive',
      'colorSuccessBg', 'colorSuccessBgHover', 'colorSuccessBorder', 'colorSuccessBorderHover', 'colorSuccessHover', 'colorSuccessActive', 'colorSuccessTextHover', 'colorSuccessText', 'colorSuccessTextActive',
      'colorWarningBg', 'colorWarningBgHover', 'colorWarningBorder', 'colorWarningBorderHover', 'colorWarningHover', 'colorWarningActive', 'colorWarningTextHover', 'colorWarningText', 'colorWarningTextActive',
      'colorErrorBg', 'colorErrorBgHover', 'colorErrorBgFilledHover', 'colorErrorBgActive', 'colorErrorBorder', 'colorErrorBorderHover', 'colorErrorHover', 'colorErrorActive', 'colorErrorTextHover', 'colorErrorText', 'colorErrorTextActive',
      'colorInfoBg', 'colorInfoBgHover', 'colorInfoBorder', 'colorInfoBorderHover', 'colorInfoHover', 'colorInfoActive', 'colorInfoTextHover', 'colorInfoText', 'colorInfoTextActive', 'colorLinkHover', 'colorLinkActive',
      'colorFill', 'colorFillSecondary', 'colorFillTertiary', 'colorFillQuaternary', 'colorTextSecondary', 'colorTextTertiary', 'colorTextQuaternary', 'colorBgLayout', 'colorBgElevated', 'colorBgSpotlight', 'colorBgBlur', 'colorBgMask', 'colorWhite', 'colorBorderSecondary', 'colorBorderDisabled', 'colorShadow',
      'fontSizeSM', 'fontSizeLG', 'fontSizeXL', 'fontSizeHeading5', 'fontSizeHeading4', 'fontSizeHeading3', 'fontSizeHeading2', 'fontSizeHeading1', 'lineHeight', 'lineHeightSM', 'lineHeightLG', 'fontHeight', 'fontHeightSM', 'fontHeightLG', 'controlHeightSM', 'controlHeightXS', 'controlHeightLG',
      'sizeXXL', 'sizeXL', 'sizeLG', 'sizeMD', 'sizeMS', 'size', 'sizeSM', 'sizeXS', 'sizeXXS', 'motionDurationFast', 'motionDurationMid', 'motionDurationSlow', 'lineWidthBold', 'borderRadiusXS', 'borderRadiusSM', 'borderRadiusLG', 'borderRadiusOuter',
      'screenXS', 'screenXSMin', 'screenXSMax', 'screenSM', 'screenSMMin', 'screenSMMax', 'screenMD', 'screenMDMin', 'screenMDMax', 'screenLG', 'screenLGMin', 'screenLGMax', 'screenXL', 'screenXLMin', 'screenXLMax', 'screenXXL', 'screenXXLMin', 'screenXXLMax', 'screenXXXL', 'screenXXXLMin',
      'boxShadow', 'boxShadowSecondary', 'boxShadowTertiary', 'boxShadowPopoverArrow', 'dropShadowPopover', 'boxShadowCard', 'boxShadowDrawerRight', 'boxShadowDrawerLeft', 'boxShadowDrawerUp', 'boxShadowDrawerDown', 'boxShadowTabsOverflowLeft', 'boxShadowTabsOverflowRight', 'boxShadowTabsOverflowTop', 'boxShadowTabsOverflowBottom',
    ];
    const shortcutNames = [...new Set([...models.flatMap((model) => model.fields.map((field) => field.name)), ...mapFields])].filter((name) => name !== 'presetColorType');
    const shortcutLines = [
      '// <auto-generated /> Generated by AntDesign.StyleGen. Do not edit manually.',
      'namespace AntDesign.Theming',
      '{',
      '    public sealed partial class ThemeToken',
      '    {',
      ...shortcutNames.map((name) => `        public string ${pascal(name)} { get => this[${quote(name)}]; set => this[${quote(name)}] = value; }`),
      '    }',
      '}',
      '',
    ];
    const shortcutPath = resolve(args['shortcuts-output']);
    await mkdir(dirname(shortcutPath), { recursive: true });
    await writeFile(shortcutPath, shortcutLines.join('\n'), 'utf8');
  }
  console.log(JSON.stringify({ output, revision: await git(root, ['rev-parse', 'HEAD']), models: models.map((model) => ({ name: model.name, fields: model.fields.length })) }, null, 2));
}

async function syncV6(args) {
  if (!args['antd-path']) throw new Error('sync-v6 requires --antd-path');
  const antdPath = resolve(args['antd-path']);
  const blazorPath = resolve(args['blazor-path'] ?? new URL('../../../', import.meta.url).pathname);
  const output = resolve(args.output ?? join(blazorPath, '.vibedev', 'generated', 'antd-v6'));
  const generatedTheme = resolve(args['theme-output'] ?? join(blazorPath, 'components', 'theming', 'GeneratedThemeTokens.cs'));
  const generatedComponents = resolve(args['component-output'] ?? join(blazorPath, 'components', 'theming', 'GeneratedComponentTokens.cs'));
  const generatedShortcuts = resolve(args['shortcuts-output'] ?? join(blazorPath, 'components', 'theming', 'ThemeTokenProperties.cs'));
  const generatedThemes = resolve(args['themes-output'] ?? join(blazorPath, 'components', 'theming', 'GeneratedPreviewThemes.cs'));
  const revision = args.revision ?? 'master';
  const irOutput = join(output, 'ir');
  const baselineOutput = join(output, 'react-baseline');
  const iconCategoriesOutput = join(output, 'icon-categories.tsv');
  const iconCategoriesSourceOutput = resolve(args['icon-categories-source-output'] ?? join(blazorPath, 'site', 'AntDesign.Docs', 'Services', 'IconSearchCategories.g.cs'));

  await sync({ 'antd-path': antdPath, 'blazor-path': blazorPath, output: irOutput, revision, 'generated-at': revision, 'source-path': '<antd-path>' });
  const iconCategories = await syncAntDesignIconCategories({ antdPath, outputPath: iconCategoriesOutput });
  await mkdir(dirname(iconCategoriesSourceOutput), { recursive: true });
  await writeFile(iconCategoriesSourceOutput, generateIconCategoriesSource(await readFile(iconCategoriesOutput, 'utf8')), 'utf8');
  await generateCsharpTokens({ ir: join(irOutput, 'style-ir.json'), output: generatedComponents, 'antd-path': antdPath });
  await generateCsharpThemeTokens({ 'antd-path': antdPath, output: generatedTheme, 'shortcuts-output': generatedShortcuts });
  const themeReference = join(blazorPath, 'tests/AntDesign.Tests/Theming/Fixtures/react-theme-tokens.json');
  await generateReactThemeReference(antdPath, themeReference);
  const globalTokenCount = await syncGlobalTokenDocs({
    metadataPath: join(antdPath, 'components/version/token-meta.json'),
    englishPath: join(blazorPath, 'docs/customize-theme.en-US.md'),
    chinesePath: join(blazorPath, 'docs/customize-theme.zh-CN.md'),
  });
  await renderRuntimeGlobalTokenDocs(blazorPath);
  await generatePreviewThemeCatalog({ ir: join(irOutput, 'themes.json'), 'antd-path': antdPath, output: generatedThemes });
  const structure = await verifyGeneratedStructure({
    antdPath,
    ir: join(irOutput, 'style-ir.json'),
    components: generatedComponents,
    theme: generatedTheme,
    shortcuts: generatedShortcuts,
    globalTokenDocs: [join(blazorPath, 'docs/customize-theme.en-US.md'), join(blazorPath, 'docs/customize-theme.zh-CN.md')],
  });
  await updateBaseline({ 'antd-path': antdPath, output: baselineOutput, revision, 'react-css': args['react-css'], 'input-id': 'antd-react-css' });

  const colorsTestSource = join(antdPath, 'node_modules', '@ant-design', 'colors', 'tests');
  const colorTests = {
    source: existsSync(colorsTestSource) ? colorsTestSource : '@ant-design/colors official test cases',
    generatedAt: revision,
    cases: {
      lightBlue: ['#e6f7ff', '#bae7ff', '#91d5ff', '#69c0ff', '#40a9ff', '#1890ff', '#096dd9', '#0050b3', '#003a8c', '#002766'],
      darkBlue: ['#111d2c', '#112a45', '#15395b', '#164c7e', '#1765ad', '#177ddc', '#3c9ae8', '#65b7f3', '#8dcff8', '#b7e3fa'],
      presetOrder: ['red', 'volcano', 'orange', 'gold', 'yellow', 'lime', 'green', 'cyan', 'blue', 'geekblue', 'purple', 'magenta', 'grey'],
      presetBlue: ['#e6f4ff', '#bae0ff', '#91caff', '#69b1ff', '#4096ff', '#1677ff', '#0958d9', '#003eb3', '#002c8c', '#001d66'],
    },
  };
  await writeFile(join(output, 'color-test-cases.json'), `${JSON.stringify(colorTests, null, 2)}\n`, 'utf8');
  const manifest = {
    schemaVersion: 1,
    upstream: { repository: 'ant-design', revision: git(antdPath, ['rev-parse', `${revision}^{commit}`]) },
    outputs: {
      ir: relative(blazorPath, join(irOutput, 'style-ir.json')),
      iconCategories: relative(blazorPath, iconCategoriesOutput),
      iconCategoriesSource: relative(blazorPath, iconCategoriesSourceOutput),
      components: relative(blazorPath, generatedComponents),
      theme: relative(blazorPath, generatedTheme),
      themeReference: relative(blazorPath, themeReference),
      shortcuts: relative(blazorPath, generatedShortcuts),
      themes: relative(blazorPath, generatedThemes),
      globalTokenDocs: [relative(blazorPath, join(blazorPath, 'docs/customize-theme.en-US.md')), relative(blazorPath, join(blazorPath, 'docs/customize-theme.zh-CN.md'))],
      reactBaseline: relative(blazorPath, baselineOutput),
      colorTests: relative(blazorPath, join(output, 'color-test-cases.json')),
      structure: relative(blazorPath, join(output, 'structure-report.json')),
    },
    command: 'node tools/AntDesign.StyleGen/src/cli.mjs sync-v6 --antd-path <antd-path>',
  };
  await writeFile(join(output, 'structure-report.json'), `${JSON.stringify(structure, null, 2)}\n`, 'utf8');
  await writeFile(join(output, 'sync-manifest.json'), `${JSON.stringify(manifest, null, 2)}\n`, 'utf8');
  console.log(JSON.stringify({ output, revision: manifest.upstream.revision, generatedComponents, generatedTheme, baselineOutput, iconCategories, iconCategoriesSourceOutput, globalTokenCount }, null, 2));
}

async function renderRuntimeGlobalTokenDocs(blazorPath) {
  const siteDocs = join(blazorPath, 'site/AntDesign.Docs/wwwroot/docs');
  for (const language of ['en-US', 'zh-CN']) {
    await copyFile(join(blazorPath, `docs/customize-theme.${language}.md`), join(siteDocs, `customize-theme.${language}.md`));
  }
  const cliProject = join(blazorPath, 'site/AntDesign.Docs.Build.CLI/AntDesign.Docs.Build.CLI.csproj');
  const result = spawnSync('dotnet', [
    'run', '--no-launch-profile', '--framework', 'net10.0', '--project', cliProject,
    'docs2html', siteDocs, siteDocs,
  ], { cwd: blazorPath, encoding: 'utf8', stdio: 'pipe' });
  if (result.status !== 0) {
    throw new Error(`Failed to render synchronized Design Token docs: ${result.stderr || result.stdout || `dotnet exited ${result.status}`}`);
  }
}

async function generatePreviewThemeCatalog(args) {
  if (!args.ir || !args.output) throw new Error('generate-preview-theme-catalog requires --ir and --output');
  const themes = await readJson(resolve(args.ir), 'theme catalog');
  const previewRoot = join(resolve(args['antd-path']), '.dumi/pages/index/components/ThemePreview/previewThemes');
  const indexPath = join(previewRoot, 'index.ts');
  const indexSource = existsSync(indexPath) ? await readFile(indexPath, 'utf8') : '';
  const metadata = extractPreviewThemeMetadata(indexSource);
  const compatiblePath = join(resolve(args['antd-path']), 'node_modules/@ant-design/compatible/lib/theme/index.js');
  const compatible = existsSync(compatiblePath) ? extractCompatibleV4Defaults(await readFile(compatiblePath, 'utf8')) : null;
  const previewThemeComponents = parseThemeLiteral(findNamedObject(indexSource, 'previewThemeComponents'), { DEFAULT_COLOR: '#1677FF' });
  const darkPreviewComponents = {
    Layout: parseThemeLiteral(findNamedObject(indexSource, 'darkPreviewLayoutToken')),
    Menu: parseThemeLiteral(findNamedObject(indexSource, 'darkPreviewMenuToken')),
    Progress: parseThemeLiteral(findNamedObject(indexSource, 'darkPreviewProgressToken'), { DEFAULT_COLOR: '#1677FF' }),
  };
  for (const theme of themes) {
    const entry = metadata.find(item => item.key === theme.key);
    if (!entry) throw new Error(`Missing React preview metadata for ${theme.key}`);
    theme.icon = entry.icon;
    theme.background = entry.background;
    theme.darkBackground = entry.darkBackground;
    theme.name = entry.name;
    theme.algorithm = theme.key === 'dark' ? 'dark' : 'default';
    const sourcePath = join(previewRoot, `${theme.key}Theme.ts`);
    if (existsSync(sourcePath)) {
      const source = await readFile(sourcePath, 'utf8');
      theme.algorithm = extractPreviewAlgorithm(source);
      theme.tokens = extractThemeObject(source, 'token', { DEFAULT_COLOR: '#1677FF' });
      theme.components = extractThemeObject(source, 'components', { DEFAULT_COLOR: '#1677FF', DEFAULT_COLOR_V4: '#1890ff' });
      theme.unresolved = [...new Set([...findUnresolvedSpreads(source, 'token'), ...findUnresolvedSpreads(source, 'components')])];
      if (theme.key === 'v4' && compatible) {
        theme.tokens = compatible.token;
        theme.components = mergeThemeObjects(compatible.components, theme.components);
        theme.components.Progress.defaultColor = compatible.token.colorPrimary;
        theme.unresolved = ['algorithm:@ant-design/compatible/defaultAlgorithm'];
      }
      theme.source = `.dumi/pages/index/components/ThemePreview/previewThemes/${theme.key}Theme.ts`;
    } else {
      theme.tokens = {};
      theme.components = {};
      theme.unresolved = [];
    }
    // The light and dark entries are created by getBasePreviewThemeProps in index.ts,
    // so their component tokens do not live in a per-theme file. Keep that shared
    // source in the generated catalog instead of silently generating empty themes.
    if (theme.key === 'light' || theme.key === 'dark') {
      theme.components = mergeThemeObjects(previewThemeComponents, theme.key === 'dark' ? darkPreviewComponents : {});
      theme.source = '.dumi/pages/index/components/ThemePreview/previewThemes/index.ts';
    }
  }
  const quote = (value) => JSON.stringify(String(value));
  const lines = [
    '// <auto-generated /> Generated by AntDesign.StyleGen. Do not edit manually.',
    'using System.Collections.Generic;',
    'using System.Collections.ObjectModel;',
    'namespace AntDesign.Theming',
    '{',
    '    public sealed class PreviewThemeDefinition',
    '    {',
    '        public string Key { get; }',
    '        public string Name { get; }',
    '        public string Appearance { get; }',
    '        public string Algorithm { get; }',
    '        public string ReactSource { get; }',
    '        public string Icon { get; }',
    '        public string Background { get; }',
    '        public bool DarkBackground { get; }',
    '        public IReadOnlyDictionary<string, string> Tokens { get; }',
    '        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Components { get; }',
    '        public IReadOnlyList<string> Unresolved { get; }',
    '        public PreviewThemeDefinition(string key, string name, string appearance, string algorithm, string reactSource, IReadOnlyDictionary<string, string> tokens, IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> components, IReadOnlyList<string> unresolved, string icon = null, string background = null, bool darkBackground = false)',
    '        { Key = key; Name = name; Appearance = appearance; Algorithm = algorithm; ReactSource = reactSource; Tokens = tokens; Components = components; Unresolved = unresolved; Icon = icon; Background = background; DarkBackground = darkBackground; }',
    '    }',
    '',
    '    public static class GeneratedPreviewThemes',
    '    {',
    '        public static IReadOnlyList<PreviewThemeDefinition> All { get; } = new ReadOnlyCollection<PreviewThemeDefinition>(new[]',
    '        {',
    ...themes.map((theme) => `            new PreviewThemeDefinition(${quote(theme.key)}, ${quote(theme.name)}, ${quote(theme.appearance)}, ${quote(theme.algorithm)}, ${quote(theme.source)}, ${toCsharpStringDictionary(theme.tokens)}, ${toCsharpNestedStringDictionary(theme.components)}, ${toCsharpStringList(theme.unresolved)}, ${quote(theme.icon)}, ${quote(theme.background)}, ${theme.darkBackground}),`),
    '        });',
    '    }',
    '}',
    '',
  ];
  const output = resolve(args.output);
  await mkdir(dirname(output), { recursive: true });
  await writeFile(output, lines.join('\n'), 'utf8');
  console.log(JSON.stringify({ output, themes: themes.length }, null, 2));
}

function findObjectAfter(source, property) {
  const match = new RegExp(`\\b${property}\\s*:`).exec(source);
  if (!match) return null;
  const start = source.indexOf('{', match.index + match[0].length);
  if (start < 0) return null;
  let depth = 0; let quote = null; let escaped = false;
  for (let index = start; index < source.length; index += 1) {
    const character = source[index];
    if (quote) { if (escaped) escaped = false; else if (character === '\\') escaped = true; else if (character === quote) quote = null; continue; }
    if (character === '"' || character === "'") { quote = character; continue; }
    if (character === '{') depth += 1;
    if (character === '}' && --depth === 0) return source.slice(start, index + 1);
  }
  return null;
}

function splitTopLevel(source) {
  const result = []; let start = 1; let depth = 0; let quote = null; let escaped = false;
  for (let index = 1; index < source.length - 1; index += 1) {
    const character = source[index];
    if (quote) { if (escaped) escaped = false; else if (character === '\\') escaped = true; else if (character === quote) quote = null; continue; }
    if (character === '"' || character === "'") { quote = character; continue; }
    if ('{[('.includes(character)) depth += 1;
    if ('}])'.includes(character)) depth -= 1;
    if (character === ',' && depth === 0) { result.push(source.slice(start, index).trim()); start = index + 1; }
  }
  if (source.slice(start, -1).trim()) result.push(source.slice(start, -1).trim());
  return result;
}

function parseThemeLiteral(value, replacements = {}) {
  const object = value?.trim();
  if (!object?.startsWith('{')) return {};
  const result = {};
  for (const entry of splitTopLevel(object)) {
    if (entry.startsWith('...')) continue;
    const colon = entry.indexOf(':');
    if (colon < 0) continue;
    const key = entry.slice(0, colon).trim().replace(/^['"]|['"]$/g, '');
    const raw = entry.slice(colon + 1).trim();
    if (raw.startsWith('{')) result[key] = parseThemeLiteral(raw, replacements);
    else if (/^['"]/.test(raw)) result[key] = raw.slice(1, raw.lastIndexOf(raw[0])).replace(/\\(['"])/g, '$1');
    else if (/^(true|false)$/.test(raw)) result[key] = raw;
    else if (/^-?\d+(\.\d+)?$/.test(raw)) result[key] = raw;
    else if (replacements[raw] !== undefined) result[key] = replacements[raw];
    else if (/^.+\?\?.+$/.test(raw)) {
      const fallback = raw.split('??').pop().trim();
      if (replacements[fallback] !== undefined) result[key] = replacements[fallback];
    } else if (/^(theme\.)/.test(raw)) result[key] = raw.replace(/^theme\./, '');
  }
  return result;
}

function extractThemeObject(source, property, replacements = {}) {
  return parseThemeLiteral(findObjectAfter(source, property), replacements);
}

function findNamedObject(source, name) {
  const match = new RegExp(`\\b${name}\\s*(?:=|:)`).exec(source);
  if (!match) return null;
  const start = source.indexOf('{', match.index + match[0].length);
  if (start < 0) return null;
  let depth = 0; let quote = null; let escaped = false;
  for (let index = start; index < source.length; index += 1) {
    const character = source[index];
    if (quote) { if (escaped) escaped = false; else if (character === '\\') escaped = true; else if (character === quote) quote = null; continue; }
    if (character === '"' || character === "'") { quote = character; continue; }
    if (character === '{') depth += 1;
    if (character === '}' && --depth === 0) return source.slice(start, index + 1);
  }
  return null;
}

function mergeThemeObjects(base, override) {
  const result = structuredClone(base ?? {});
  for (const [key, value] of Object.entries(override ?? {})) {
    result[key] = value && typeof value === 'object' && !Array.isArray(value)
      ? { ...(result[key] ?? {}), ...value }
      : value;
  }
  return result;
}

function findUnresolvedSpreads(source, property) {
  const object = findObjectAfter(source, property);
  return object ? [...object.matchAll(/\.\.\.([^,}]+)/g)].map((match) => `${property}:${match[1].trim()}`) : [];
}

function toCsharpStringDictionary(value) {
  const entries = Object.entries(value ?? {}).filter(([, item]) => typeof item === 'string' || typeof item === 'number' || typeof item === 'boolean');
  return `new Dictionary<string, string> { ${entries.map(([key, item]) => `[${JSON.stringify(key)}] = ${JSON.stringify(String(item))}`).join(', ')} }`;
}

function toCsharpNestedStringDictionary(value) {
  const entries = Object.entries(value ?? {}).filter(([, item]) => item && typeof item === 'object' && !Array.isArray(item));
  return `new Dictionary<string, IReadOnlyDictionary<string, string>> { ${entries.map(([key, item]) => `[${JSON.stringify(key)}] = ${toCsharpStringDictionary(item)}`).join(', ')} }`;
}

function toCsharpStringList(value) {
  return `new string[] { ${(value ?? []).map((item) => JSON.stringify(item)).join(', ')} }`;
}

async function verifyGeneratedStructure(args) {
  const ir = await readJson(resolve(args.ir), 'generated style IR');
  const componentSource = await readFile(resolve(args.components), 'utf8');
  const themeSource = await readFile(resolve(args.theme), 'utf8');
  const shortcutSource = await readFile(resolve(args.shortcuts), 'utf8');
  const tokenMetadata = await readJson(join(args.antdPath, 'components/version/token-meta.json'), 'React Design Token metadata');
  const globalTokenSource = tokenMetadata.global ?? {};
  const pascal = (value) => pascalCase(value);
  const componentExpected = [...new Set((ir.tokens ?? []).filter((token) => token.kind === 'component' && token.name).map((token) => pascal(token.name)))];
  const componentMissing = componentExpected.filter((name) => !new RegExp(`public\\s+[^\\r\\n]+\\s+${name}\\s*\\{`).test(componentSource));
  const upstreamFields = {};
  for (const [name, path] of [['Seed', join(args.antdPath, 'components/theme/interface/seeds.ts')], ['Alias', join(args.antdPath, 'components/theme/interface/alias.ts')]]) {
    const source = await readFile(path, 'utf8');
    const match = source.match(new RegExp(`interface\\s+${name}Token[^\\{]*\\{([\\s\\S]*?)\\}`));
    upstreamFields[name] = match ? [...match[1].matchAll(/^\s*([A-Za-z_$][\w$]*)\??\s*:/gm)].map((item) => pascal(item[1])) : [];
  }
  const themeMissing = Object.values(upstreamFields).flat().filter((name) => !new RegExp(`public\\s+[^\\s]+\\s+${name}\\s*\\{`).test(themeSource));
  const shortcutMissing = [...new Set(Object.values(upstreamFields).flat())].filter((name) => !new RegExp(`public\\s+string\\s+${name}\\s*\\{`).test(shortcutSource));
  const missingGlobalDefinitions = Object.keys(globalTokenSource).filter((name) => !themeSource.includes(`new GlobalTokenDefinition("${name}"`));
  const globalDocsMissing = [];
  for (const path of args.globalTokenDocs ?? []) {
    const source = await readFile(path, 'utf8');
    const missing = Object.keys(globalTokenSource).filter((name) => !source.includes(`| \`${name}\` |`));
    if (missing.length) globalDocsMissing.push({ path, missing });
  }
  const report = {
    valid: componentMissing.length === 0 && themeMissing.length === 0 && shortcutMissing.length === 0 && missingGlobalDefinitions.length === 0 && globalDocsMissing.length === 0,
    componentTokenCount: componentExpected.length,
    globalTokenCount: Object.keys(globalTokenSource).length,
    missingComponentTokens: componentMissing,
    missingThemeFields: themeMissing,
    missingShortcuts: shortcutMissing,
    missingGlobalDefinitions,
    globalDocsMissing,
  };
  if (!report.valid) throw new Error(`generated Token structure is incomplete: ${JSON.stringify(report)}`);
  return report;
}

async function discoverStyleCompositions(root) {
  const componentsRoot = join(root, 'components');
  const entries = await readdir(componentsRoot, { withFileTypes: true });
  const result = [];
  for (const entry of entries) {
    if (!entry.isDirectory() || entry.name.startsWith('_')) continue;
    const tokenPath = join(componentsRoot, entry.name, 'style', 'token.ts');
    const indexPath = join(componentsRoot, entry.name, 'style', 'index.ts');
    if (!existsSync(tokenPath) || !existsSync(indexPath)) continue;
    const source = await readFile(tokenPath, 'utf8');
    const component = pascalCase(entry.name);
    const match = source.match(new RegExp(`interface\\s+${component}Token\\s+extends\\s+([^\\{]+)\\{`));
    const aliasMatch = source.match(new RegExp(`type\\s+${component}Token\\s*=\\s*([^;]+);`));
    const compositionSource = match?.[1] ?? aliasMatch?.[1];
    if (!compositionSource) continue;
    const members = compositionSource.split(/[,&]/).map((member) => member.trim()).filter(Boolean).map((member) => {
      const generic = member.match(/^(Pick|Partial|Record)<([^,>]+)/);
      const type = member.startsWith('FullToken') ? 'GlobalToken'
        : generic?.[1] === 'Pick' ? `TokenPick<${generic[2].trim() === 'GlobalToken' ? 'GlobalToken' : 'GlobalToken'}>`
          : generic?.[1] === 'Partial' ? `TokenPartial<${generic[2].trim() === 'GlobalToken' ? 'GlobalToken' : 'GlobalToken'}>`
            : member.includes('[Key in') || generic?.[1] === 'Record' ? 'TokenMap<string>'
              : 'System.Collections.Generic.Dictionary<string, string>';
      return { name: member.replace(/<.*>/, '').replace(/[^A-Za-z0-9]/g, '') || 'Global', type };
    });
    const styleSource = await readFile(indexPath, 'utf8');
    const arrayMatch = styleSource.match(/return\s*\[([\s\S]*?)\];/);
    const generators = arrayMatch ? [...arrayMatch[1].matchAll(/(^|,|\n)\s*([A-Za-z_$][\w$]*)\(/g)].map((item) => item[2]) : [];
    result.push({ component: entry.name, members, generators });
  }
  return result.sort((left, right) => left.component.localeCompare(right.component));
}

function pascalCase(value) {
  return String(value).split(/[-_\s]+/).filter(Boolean).map((part) => part[0].toUpperCase() + part.slice(1)).join('');
}

const args = parseArgs(process.argv.slice(2));
if (args.command === 'sync-color-spec') {
  if (!args['antd-path'] || !args.output) fail('sync-color-spec requires --antd-path and --output');
  else syncColorSpec({ antdPath: args['antd-path'], output: args.output, check: Boolean(args.check) }).then(result => console.log(JSON.stringify(result))).catch(error => fail(error.message));
} else if (args.command === 'sync') {
  sync(args).catch((error) => fail(error.message));
} else if (args.command === 'baseline-update') {
  updateBaseline(args).catch((error) => fail(error.message));
} else if (args.command === 'exact-check') {
  exactCheck(args).catch((error) => fail(error.message));
} else if (args.command === 'theme-exact-check') {
  themeExactCheck(args).catch((error) => fail(error.message));
} else if (args.command === 'generate-csharp-tokens') {
  generateCsharpTokens(args).catch((error) => fail(error.message));
} else if (args.command === 'generate-csharp-theme-tokens') {
  generateCsharpThemeTokens(args).catch((error) => fail(error.message));
} else if (args.command === 'generate-theme-reference') {
  if (!args['antd-path'] || !args.output) fail('generate-theme-reference requires --antd-path and --output');
  else generateReactThemeReference(args['antd-path'], args.output).then(result => console.log(JSON.stringify({ source: result.source, scenarios: result.scenarios.length }))).catch(error => fail(error.message));
} else if (args.command === 'generate-preview-theme-catalog') {
  generatePreviewThemeCatalog(args).catch((error) => fail(error.message));
} else if (args.command === 'sync-v6') {
  syncV6(args).catch((error) => fail(error.message));
} else if (args.command === 'sync-semantic-dom') {
  if (!args['antd-path']) fail('sync-semantic-dom requires --antd-path');
  else {
    const repoRoot = resolve(args['blazor-path'] || process.cwd());
    const output = resolve(args.output || join(repoRoot, 'docs/antd-v6/semantic-dom-catalog.json'));
    const semanticDocsPath = resolve(args['semantic-docs-path'] || join(args['antd-path'], '_site'));
    if (!existsSync(semanticDocsPath)) {
      fail(`Generated React semantic.md files were not found at ${semanticDocsPath}; build/export the Ant Design docs first or pass --semantic-docs-path <path>.`);
    } else {
      const inventoryScript = join(repoRoot, 'tools/AntDesign.StyleGen/src/semantic-inventory.mjs');
      const inventoryOutput = join(repoRoot, 'docs/antd-v6/semantic-dom-inventory.json');
      const inventory = spawnSync(process.execPath, [inventoryScript, repoRoot, join(resolve(args['antd-path']), 'components'), inventoryOutput], { encoding: 'utf8', stdio: 'inherit' });
      if (inventory.status !== 0) fail(`Semantic DOM API inventory sync failed with exit code ${inventory.status}`);
      else {
        const auditScript = join(repoRoot, 'tools/AntDesign.StyleGen/src/audit-semantic-md.mjs');
        const auditOutput = resolve(args['semantic-md-output'] || join(repoRoot, 'docs/antd-v6/semantic-md-catalog.json'));
        const audit = spawnSync(process.execPath, [auditScript, repoRoot, join(repoRoot, 'docs/antd-v6/semantic-dom-inventory.json'), semanticDocsPath, auditOutput], { encoding: 'utf8', stdio: 'inherit' });
        const generatorScript = join(repoRoot, 'tools/AntDesign.StyleGen/src/generate-semantic-dom-catalog.mjs');
        const generated = spawnSync(process.execPath, [generatorScript, repoRoot, join(repoRoot, 'docs/antd-v6/semantic-dom-inventory.json'), output, output, auditOutput], { encoding: 'utf8', stdio: 'inherit' });
        if (generated.status !== 0) fail(`Semantic DOM catalog generation failed with exit code ${generated.status}`);
        if (audit.status !== 0) fail(`Generated semantic.md audit found documented/API differences (exit code ${audit.status}); the catalog was still refreshed from the documentation nodes.`);
      }
    }
  }
} else if (args.command === 'css-baseline-update') {
  if (!args['antd-path'] || !args.output) fail('css-baseline-update requires --antd-path and --output');
  else generateReactCssBaseline({ antdPath: args['antd-path'], output: args.output, docsThemeOutput: args['docs-theme-output'] })
    .then((result) => console.log(JSON.stringify(result, null, 2)))
    .catch((error) => fail(error.message));
} else {
  fail('supported commands are sync, baseline-update, css-baseline-update, generate-csharp-tokens, generate-csharp-theme-tokens, generate-theme-reference, sync-v6, sync-semantic-dom, exact-check and theme-exact-check');
}
