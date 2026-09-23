import { createRequire } from 'node:module';
import { mkdir, readFile, readdir, writeFile } from 'node:fs/promises';
import { existsSync } from 'node:fs';
import { join, resolve } from 'node:path';
import { spawnSync } from 'node:child_process';
import Module from 'node:module';
import { createArtifact, createBaseline, sha256, writeBaseline } from './baseline.mjs';

const THEME_ORDER = [
  'light', 'dark', 'mui', 'shadcn', 'bootstrap', 'cartoon', 'illustration',
  'glass', 'geek', 'lark', 'blossom', 'v4', 'serene',
];

const COMPONENT_ALIASES = {
  'back-top': 'FloatButton',
  'qr-code': 'QRCode',
  qrcode: 'QRCode',
  message: 'message',
  notification: 'notification',
};

function pascalCase(value) {
  return String(value).split(/[-_\s]+/).filter(Boolean)
    .map((part) => part[0].toUpperCase() + part.slice(1)).join('');
}

function git(root, args) {
  const result = spawnSync('git', ['-C', root, ...args], { encoding: 'utf8' });
  if (result.status !== 0) throw new Error(result.stderr.trim() || `git ${args.join(' ')} failed`);
  return result.stdout.trim();
}

function findObjectAfter(source, property) {
  const match = new RegExp(`\\b${property}\\s*:`).exec(source);
  if (!match) return null;
  const start = source.indexOf('{', match.index + match[0].length);
  if (start < 0) return null;
  let depth = 0; let quote = null; let escaped = false;
  for (let index = start; index < source.length; index += 1) {
    const character = source[index];
    if (quote) {
      if (escaped) escaped = false;
      else if (character === '\\') escaped = true;
      else if (character === quote) quote = null;
      continue;
    }
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
    if (quote) {
      if (escaped) escaped = false;
      else if (character === '\\') escaped = true;
      else if (character === quote) quote = null;
      continue;
    }
    if (character === '"' || character === "'") { quote = character; continue; }
    if ('{[('.includes(character)) depth += 1;
    if ('}])'.includes(character)) depth -= 1;
    if (character === ',' && depth === 0) { result.push(source.slice(start, index).trim()); start = index + 1; }
  }
  if (source.slice(start, -1).trim()) result.push(source.slice(start, -1).trim());
  return result;
}

function parseLiteral(value, replacements = {}) {
  const object = value?.trim();
  if (!object?.startsWith('{')) return {};
  const result = {};
  for (const entry of splitTopLevel(object)) {
    if (entry.startsWith('...')) continue;
    const colon = entry.indexOf(':');
    if (colon < 0) continue;
    const key = entry.slice(0, colon).trim().replace(/^['"]|['"]$/g, '');
    const raw = entry.slice(colon + 1).trim();
    if (raw.startsWith('{')) result[key] = parseLiteral(raw, replacements);
    else if (/^['"]/.test(raw)) result[key] = raw.slice(1, raw.lastIndexOf(raw[0])).replace(/\\(['"])/g, '$1');
    else if (/^(true|false)$/.test(raw)) result[key] = raw === 'true';
    else if (/^-?\d+(\.\d+)?$/.test(raw)) result[key] = Number(raw);
    else if (replacements[raw] !== undefined) result[key] = replacements[raw];
    else if (/^.+\?\?.+$/.test(raw)) result[key] = replacements[raw.split('??').pop().trim()];
  }
  return result;
}

function extractObject(source, property, replacements = {}) {
  return parseLiteral(findObjectAfter(source, property), replacements);
}

function mergeObjects(base, override) {
  const result = structuredClone(base ?? {});
  for (const [key, value] of Object.entries(override ?? {})) {
    result[key] = value && typeof value === 'object' && !Array.isArray(value)
      ? { ...(result[key] ?? {}), ...value }
      : value;
  }
  return result;
}

async function discoverComponents(root, antd) {
  const entries = await readdir(join(root, 'components'), { withFileTypes: true });
  const exports = new Set(Object.keys(antd));
  const components = [];
  for (const entry of entries) {
    if (!entry.isDirectory() || entry.name.startsWith('_')) continue;
    if (!existsSync(join(root, 'components', entry.name, 'style', 'index.ts'))) continue;
    const exportName = COMPONENT_ALIASES[entry.name] ?? pascalCase(entry.name);
    components.push({
      directory: entry.name,
      exportName,
      available: exports.has(exportName) || ['message', 'notification'].includes(exportName),
    });
  }
  return components.sort((left, right) => left.directory.localeCompare(right.directory));
}

async function discoverThemes(root) {
  const previewRoot = join(root, '.dumi/pages/index/components/ThemePreview/previewThemes');
  const indexSource = await readFile(join(previewRoot, 'index.ts'), 'utf8');
  const shared = extractObject(indexSource, 'previewThemeComponents', { DEFAULT_COLOR: '#1677FF' });
  const dark = {
    Layout: extractObject(indexSource, 'darkPreviewLayoutToken'),
    Menu: extractObject(indexSource, 'darkPreviewMenuToken'),
    Progress: extractObject(indexSource, 'darkPreviewProgressToken', { DEFAULT_COLOR: '#1677FF' }),
  };
  const themes = [];
  for (const key of THEME_ORDER) {
    if (key === 'light' || key === 'dark') {
      themes.push({ key, algorithm: key === 'dark' ? 'darkAlgorithm' : 'defaultAlgorithm', token: {}, components: mergeObjects(shared, key === 'dark' ? dark : {}) });
      continue;
    }
    const sourcePath = join(previewRoot, `${key}Theme.ts`);
    const source = await readFile(sourcePath, 'utf8');
    themes.push({
      key,
      algorithm: source.includes('theme.darkAlgorithm') ? 'darkAlgorithm' : 'defaultAlgorithm',
      token: extractObject(source, 'token', { DEFAULT_COLOR: '#1677FF', DEFAULT_COLOR_V4: '#1890ff' }),
      components: extractObject(source, 'components', { DEFAULT_COLOR: '#1677FF', DEFAULT_COLOR_V4: '#1890ff' }),
    });
  }
  return themes;
}

function discoverDocsThemeModes() {
  return [
    { key: 'light-compact', algorithm: ['defaultAlgorithm', 'compactAlgorithm'], token: {}, components: {} },
    { key: 'dark-compact', algorithm: ['darkAlgorithm', 'compactAlgorithm'], token: {}, components: {} },
  ];
}

function loadReactRuntime(root) {
  const originalResolve = Module._resolveFilename;
  const componentsEntry = join(root, 'components', 'index.ts');
  Module._resolveFilename = function resolveFilename(request, ...args) {
    if (request === 'antd') return componentsEntry;
    return originalResolve.call(this, request, ...args);
  };
  const require = createRequire(join(root, 'package.json'));
  const React = require('react');
  const antd = require(componentsEntry);
  const extractor = require('@ant-design/static-style-extract');
  return { React, antd, extractStyle: extractor.extractStyle };
}

function renderCss(runtime, theme, includes) {
  const algorithmNames = Array.isArray(theme.algorithm) ? theme.algorithm : [theme.algorithm];
  const algorithm = algorithmNames.map((name) => runtime.antd.theme[name]);
  const themeConfig = { hashed: false, algorithm, token: theme.token, components: theme.components };
  return runtime.extractStyle({
    includes,
    antdInstance: runtime.antd,
    customTheme: (node) => runtime.React.createElement(runtime.antd.ConfigProvider, { theme: themeConfig }, node),
  });
}

export async function generateReactCssBaseline({ antdPath, output, docsThemeOutput }) {
  const root = resolve(antdPath);
  const target = resolve(output);
  const packageJson = JSON.parse(await readFile(join(root, 'package.json'), 'utf8'));
  if (packageJson.name !== 'antd') throw new Error(`expected package name "antd", got "${packageJson.name ?? ''}"`);
  const runtime = loadReactRuntime(root);
  const components = await discoverComponents(root, runtime.antd);
  const themes = await discoverThemes(root);
  const docsThemeModes = discoverDocsThemeModes();
  const available = components.filter((component) => component.available);
  const unsupported = components.filter((component) => !component.available).map((component) => component.directory);
  const revision = git(root, ['rev-parse', 'HEAD']);
  const artifacts = [];
  const cssById = new Map();
  const themeFullCss = new Map();

  for (const theme of [...themes, ...docsThemeModes]) {
    for (const component of available) {
      const css = renderCss(runtime, theme, [component.exportName]);
      const fileName = `${component.directory}.${theme.key}.css`;
      const id = `${component.directory}.${theme.key}.static`;
      const artifact = createArtifact({ id, path: `static/${fileName}`, layer: 'static', component: component.directory, theme: theme.key, css, allowMissingTrailingNewline: true });
      artifacts.push(artifact); cssById.set(id, css);
    }
    const fullCss = renderCss(runtime, theme);
    const fullName = `${theme.key}.css`;
    const fullId = `all.${theme.key}.static`;
    const fullArtifact = createArtifact({ id: fullId, path: `full/${fullName}`, layer: 'static', component: 'all', theme: theme.key, css: fullCss, allowMissingTrailingNewline: true });
    artifacts.push(fullArtifact); cssById.set(fullId, fullCss); themeFullCss.set(theme.key, fullCss);
  }

  const upstream = { repository: 'ant-design', revision, version: packageJson.version };
  const generator = { name: '@ant-design-blazor/react-css-baseline', version: '0.1.0' };
  const baseline = createBaseline({ upstream, generator, inputDigest: sha256(JSON.stringify({ revision, themes, docsThemeModes, components })), artifacts });
  await writeBaseline(target, baseline, cssById);
  await mkdir(join(target, 'themes'), { recursive: true });
  for (const [theme, css] of themeFullCss) await writeFile(join(target, 'themes', `${theme}.css`), css, 'utf8');
  await writeFile(join(target, 'components.json'), `${JSON.stringify({ total: components.length, generated: available.length, unsupported }, null, 2)}\n`, 'utf8');
  await writeFile(join(target, 'themes.json'), `${JSON.stringify(themes, null, 2)}\n`, 'utf8');
  await writeFile(join(target, 'docs-theme-modes.json'), `${JSON.stringify(docsThemeModes, null, 2)}\n`, 'utf8');
  if (docsThemeOutput) {
    const destination = resolve(docsThemeOutput);
    await mkdir(destination, { recursive: true });
    for (const theme of docsThemeModes) {
      await writeFile(join(destination, `${theme.key}.css`), themeFullCss.get(theme.key), 'utf8');
    }
    await writeFile(join(destination, 'docs-theme-modes.manifest.json'), `${JSON.stringify({ upstream, generator, modes: docsThemeModes.map(({ key }) => ({ key, file: `${key}.css`, sha256: sha256(themeFullCss.get(key)) })) }, null, 2)}\n`, 'utf8');
  }
  return { output: target, revision, version: packageJson.version, components: components.length, generatedComponents: available.length, themes: themes.length, docsThemeModes: docsThemeModes.length, artifacts: artifacts.length, unsupported };
}
