#!/usr/bin/env node

import { mkdir, readFile, readdir, writeFile } from 'node:fs/promises';
import { existsSync } from 'node:fs';
import { dirname, join, relative, resolve } from 'node:path';
import { spawnSync } from 'node:child_process';
import Ajv from 'ajv';
import { createArtifact, createBaseline, readBaseline, sha256, writeBaseline } from './baseline.mjs';
import { compareArtifactSets, compareCss } from './style-diff.mjs';

const usage = `Usage:
  node src/cli.mjs sync --antd-path <path> [--blazor-path <path>] [--revision <revision>] --output <path>
  node src/cli.mjs baseline-update --antd-path <path> [--revision <revision>] --output <path>
  node src/cli.mjs generate-csharp-tokens --ir <style-ir.json> [--antd-path <path>] --output <GeneratedComponentTokens.cs>
  node src/cli.mjs generate-csharp-theme-tokens --antd-path <path> --output <GeneratedThemeTokens.cs>
  node src/cli.mjs exact-check --expected <baseline-dir> --actual <candidate-dir> [--allowlist <id,id>] [--report <path>]
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
    const tokenFile = existsSync(candidateTokenFile) ? candidateTokenFile : null;
    const tokenRelativePath = tokenFile ? relative(root, tokenFile).replaceAll('\\', '/') : null;
    const tokenDefinitions = tokenFile ? await discoverTokenDefinitions(tokenFile) : [];
    components.push({
      name: entry.name,
      className: `ant-${kebabCase(entry.name)}`,
      styleSource: styleRelativePath,
      tokenSource: tokenRelativePath,
      hasTokenSource: Boolean(tokenRelativePath),
      tokenDefinitions,
    });
  }
  return components.sort((left, right) => left.name.localeCompare(right.name));
}

async function discoverTokenDefinitions(tokenPath) {
  const source = await readFile(tokenPath, 'utf8');
  const definitions = new Map();
  for (const match of source.matchAll(/interface\s+ComponentToken[^\{]*\{([\s\S]*?)\}/g)) {
    for (const property of match[1].matchAll(/^\s*([A-Za-z_$][\w$]*)\??\s*:\s*([^;\n]+);?/gm)) {
      definitions.set(property[1], property[2].trim());
    }
  }
  return [...definitions].sort(([left], [right]) => left.localeCompare(right)).map(([name, type]) => ({ name, type }));
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
  const generatedAt = new Date().toISOString();
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
    const definitions = component.tokenDefinitions.length > 0 ? component.tokenDefinitions : [{ name: 'componentToken', type: 'unknown' }];
    return definitions.map(({ name, type }) => ({
      name,
      kind: 'component',
      type: normalizeTokenType(name, type),
      sourceType: type,
      value: null,
      cssVariable: `--ant-${kebabCase(component.name)}-${kebabCase(name)}`,
      component: component.name,
      source: component.tokenSource ?? component.styleSource,
      reserved: !component.hasTokenSource || !component.implementedInBlazor,
    }));
  });
  const ir = { schemaVersion: 1, upstream, generator, tokens, styles, themes, unsupported: [] };
  const schema = await readJson(join(new URL('..', import.meta.url).pathname, 'schema/style-ir.schema.json'), 'style IR schema');
  const validate = new Ajv({ allErrors: true, strict: false }).compile(schema);
  if (!validate(ir)) {
    throw new Error(`generated IR does not satisfy schema: ${JSON.stringify(validate.errors)}`);
  }
  const manifest = {
    schemaVersion: 1,
    upstream,
    generator,
    generatedAt,
    sourcePath: root,
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
    inputDigest: sha256(`${revision}:${packageJson.version}:${cssPath}`),
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
    '        public bool Reserved { get; }',
    '        public ComponentTokenDefinition(string component, string name, string cssVariable, bool reserved)',
    '        { Component = component; Name = name; CssVariable = cssVariable; Reserved = reserved; }',
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
    ...tokens.map((token) => `            new ComponentTokenDefinition(${quote(token.component)}, ${quote(token.name)}, ${quote(token.cssVariable)}, ${Boolean(token.reserved).toString().toLowerCase()}),`),
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
  const lines = [
    '// <auto-generated /> Generated by AntDesign.StyleGen. Do not edit manually.',
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
    '        public static IThemeAlgorithm Create(string name) => string.Equals(name, "dark", System.StringComparison.OrdinalIgnoreCase)'
      + '? new DarkThemeAlgorithm()'
      + ': string.Equals(name, "compact", System.StringComparison.OrdinalIgnoreCase)'
      + '? new CompactThemeAlgorithm()'
      + ': new DefaultThemeAlgorithm();',
    '    }',
    '}',
    '',
  ];
  const output = resolve(args.output);
  await mkdir(dirname(output), { recursive: true });
  await writeFile(output, lines.join('\n'), 'utf8');
  console.log(JSON.stringify({ output, revision: await git(root, ['rev-parse', 'HEAD']), models: models.map((model) => ({ name: model.name, fields: model.fields.length })) }, null, 2));
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
if (args.command === 'sync') {
  sync(args).catch((error) => fail(error.message));
} else if (args.command === 'baseline-update') {
  updateBaseline(args).catch((error) => fail(error.message));
} else if (args.command === 'exact-check') {
  exactCheck(args).catch((error) => fail(error.message));
} else if (args.command === 'generate-csharp-tokens') {
  generateCsharpTokens(args).catch((error) => fail(error.message));
} else if (args.command === 'generate-csharp-theme-tokens') {
  generateCsharpThemeTokens(args).catch((error) => fail(error.message));
} else {
  fail('supported commands are sync, baseline-update, generate-csharp-tokens and exact-check');
}
