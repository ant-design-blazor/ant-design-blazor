#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { extractSemanticTypes } from './semantic-type-extractor.mjs';

const defaultRepoRoot = path.resolve(new URL('../../..', import.meta.url).pathname);
const repoRoot = path.resolve(process.argv[2] || defaultRepoRoot);
const reactRoot = path.resolve(process.argv[3] || path.join(repoRoot, '../ant-design/components'));
const output = path.resolve(process.argv[4] || path.join(repoRoot, 'docs/antd-v6/semantic-dom-inventory.json'));
const componentCatalogPath = path.resolve(repoRoot, '.vibedev/generated/antd-v6/ir/components.json');
const migratedStructureComponents = new Set(['alert', 'anchor', 'auto-complete', 'badge', 'breadcrumb', 'button', 'calendar', 'card', 'cascader', 'checkbox', 'collapse', 'color-picker', 'date-picker', 'descriptions', 'divider', 'drawer', 'dropdown', 'empty', 'float-button', 'form', 'image', 'input', 'input-number', 'layout', 'listy', 'masonry', 'mentions', 'menu', 'message', 'modal', 'notification', 'pagination', 'popover', 'popconfirm', 'progress', 'qr-code', 'radio', 'result', 'segmented', 'select', 'skeleton', 'slider', 'space', 'spin', 'splitter', 'statistic', 'steps', 'switch', 'table', 'tabs', 'tag', 'time-picker', 'timeline', 'tooltip', 'tour', 'transfer', 'tree', 'tree-select', 'typography', 'upload']);
const semanticInjectionComponents = new Set(['float-button', 'masonry', 'tour', 'transfer', 'tree-select']);
const unavailableSemanticPaths = new Map([['popconfirm', new Set(['content'])]]);
const blazorDirectoryAliases = new Map([['row', 'grid'], ['col', 'grid'], ['qrcode', 'qr-code']]);
const nestedSemanticTypePrefixes = new Map([['TreeSelectPopupSemanticType', 'popup']]);

function hasBlazorSource(component) {
  const directory = path.join(repoRoot, 'components', blazorDirectoryAliases.get(component) ?? component);
  if (!fs.existsSync(directory)) return false;
  return fs.readdirSync(directory, { withFileTypes: true }).some((entry) => {
    const file = path.join(directory, entry.name);
    return entry.isDirectory()
      ? hasBlazorSourceDirectory(file)
      : /\.(razor|cs)$/.test(entry.name);
  });
}

function hasBlazorSourceDirectory(directory) {
  return fs.readdirSync(directory, { withFileTypes: true }).some((entry) => {
    const file = path.join(directory, entry.name);
    return entry.isDirectory()
      ? hasBlazorSourceDirectory(file)
      : /\.(razor|cs)$/.test(entry.name);
  });
}

function walk(root) {
  return fs.readdirSync(root, { withFileTypes: true }).flatMap((entry) => {
    if (entry.name.startsWith('_')) return [];
    const file = path.join(root, entry.name);
    return entry.isDirectory() ? walk(file) : /\.(tsx?|ts)$/.test(entry.name) ? [file] : [];
  });
}

function objectBody(source, start) {
  const open = source.indexOf('{', start);
  let depth = 0;
  for (let i = open; i < source.length; i += 1) {
    if (source[i] === '{') depth += 1;
    if (source[i] === '}') depth -= 1;
    if (open >= 0 && depth === 0) return source.slice(open + 1, i);
  }
  return '';
}

function keys(source, name) {
  const match = new RegExp(`${name}\\s*\\?:\\s*\\{`).exec(source);
  if (!match) return [];
  const body = objectBody(source, match.index + match[0].length - 1);
  return [...body.matchAll(/^\s*([A-Za-z][\w]*)\??\s*:/gm)].map((item) => item[1]);
}

function bracketBody(source, openIndex, openToken, closeToken) {
  const start = source.indexOf(openToken, openIndex);
  if (start < 0) return '';
  let depth = 0;
  for (let index = start; index < source.length; index += 1) {
    if (source.startsWith(openToken, index)) {
      depth += 1;
      index += openToken.length - 1;
    } else if (source.startsWith(closeToken, index)) {
      depth -= 1;
      if (depth === 0) return source.slice(start + openToken.length, index);
      index += closeToken.length - 1;
    }
  }
  return '';
}

function semanticPreviewPaths(source) {
  const paths = [];
  const previewPattern = /<SemanticPreview\b/g;
  let preview;
  while ((preview = previewPattern.exec(source))) {
    const semanticsIndex = source.indexOf('semantics={', preview.index);
    const nextPreviewIndex = source.indexOf('<SemanticPreview', preview.index + preview[0].length);
    if (semanticsIndex < 0 || (nextPreviewIndex >= 0 && nextPreviewIndex < semanticsIndex)) continue;
    const body = bracketBody(source, semanticsIndex, '[', ']');
    paths.push(...[...body.matchAll(/\bname\s*:\s*['"]([^'"]+)['"]/g)].map((match) => match[1]));
  }
  return [...new Set(paths)].sort();
}

function dumiSemanticDemos(component, semanticType) {
  const directory = path.join(reactRoot, component, 'demo');
  if (!fs.existsSync(directory)) return [];
  return fs.readdirSync(directory).filter((file) => {
    if (!/^_semantic.*\.tsx$/.test(file)) return false;
    if (component === 'card' && semanticType === 'CardMetaSemanticType') return file === '_semantic_meta.tsx';
    if (component === 'card' && semanticType === 'CardSemanticType') return file === '_semantic.tsx';
    return true;
  }).sort().map((file) => {
    const relative = path.join(component, 'demo', file);
    const source = fs.readFileSync(path.join(reactRoot, relative), 'utf8');
    return {
      source: path.posix.join('components', component, 'demo', file),
      semanticPaths: semanticPreviewPaths(source),
    };
  });
}

const sourceFiles = walk(reactRoot).sort().map((file) => ({ file, source: fs.readFileSync(file, 'utf8') }));
const semanticTypes = extractSemanticTypes(sourceFiles);
const entries = [];
for (const type of semanticTypes) {
    const file = type.file;
    const relative = path.relative(reactRoot, file).split(path.sep);
    const component = type.semanticType === 'CardMetaSemanticType' ? `${relative[0]}-meta` : relative[0];
    const dumiSemanticSources = dumiSemanticDemos(relative[0], type.semanticType);
    const semanticPrefix = nestedSemanticTypePrefixes.get(type.semanticType);
    const prefixPaths = paths => semanticPrefix ? paths.map(semanticPath => `${semanticPrefix}.${semanticPath}`) : paths;
    const classNames = prefixPaths(type.classNames);
    const styles = prefixPaths(type.styles);
    const paths = [...new Set([...classNames, ...styles, ...dumiSemanticSources.flatMap((demo) => demo.semanticPaths)])].sort();
    entries.push({
      component,
      reactSource: path.posix.join('components', ...relative),
      blazorDirectory: fs.existsSync(path.join(repoRoot, 'components', relative[0])) ? path.posix.join('components', relative[0]) : null,
      semanticType: type.semanticType,
      semanticPaths: paths,
      classNames,
      styles,
      dumiSemanticSources,
      classNameMapping: paths.map((semanticPath) => ({ semanticPath, reactClassName: null, blazorClassName: null, status: unavailableSemanticPaths.get(relative[0])?.has(semanticPath) ? 'pending-blazor-api' : semanticInjectionComponents.has(relative[0]) ? 'semantic-injection-tested-pending-class-mapping' : migratedStructureComponents.has(relative[0]) ? 'structure-migrated-pending-semantic-injection' : 'pending' })),
      htmlSnapshot: { react: 'pending-runtime-render', blazor: migratedStructureComponents.has(relative[0]) ? 'structure-test-covered' : 'pending-render' },
      migrationStatus: migratedStructureComponents.has(relative[0]) ? 'structure-migrated' : 'inventory-only',
    });
}

entries.sort((a, b) => `${a.component}/${a.semanticType}`.localeCompare(`${b.component}/${b.semanticType}`));
const result = {
  schemaVersion: 1,
  phase: 1,
  source: { repository: 'ant-design', path: path.relative(repoRoot, reactRoot).split(path.sep).join('/'), semanticSource: 'exported SemanticType definitions and public Props semantic schemas' },
  scope: { semanticDefinitions: entries.length, uniqueComponents: new Set(entries.map((entry) => entry.component)).size, uniqueSemanticPaths: new Set(entries.flatMap((entry) => entry.semanticPaths)).size },
  entries,
};
const dumiSources = entries.flatMap((entry) => entry.dumiSemanticSources);
result.scope.dumiSemanticDemoFiles = new Set(dumiSources.map((source) => source.source)).size;
result.scope.dumiSemanticPaths = new Set(entries.flatMap((entry) => (entry.dumiSemanticSources ?? [])
  .flatMap((source) => (source.semanticPaths ?? []).map((semanticPath) => `${entry.component}/${semanticPath}`)))).size;
if (fs.existsSync(componentCatalogPath)) {
  const catalog = JSON.parse(fs.readFileSync(componentCatalogPath, 'utf8'));
  result.componentCatalog = catalog.map((component) => ({
    name: component.name,
    className: component.className,
    reactStyleSource: component.styleSource,
    implementedInBlazor: component.implementedInBlazor || hasBlazorSource(component.name),
    hasSemanticDefinition: entries.some((entry) => entry.component === component.name),
    migrationStatus: migratedStructureComponents.has(component.name) ? 'structure-migrated' : 'inventory-only',
  }));
  result.scope.cssComponents = result.componentCatalog.length;
  result.scope.existingBlazorComponents = result.componentCatalog.filter((component) => component.implementedInBlazor).length;
  result.scope.existingBlazorComponentsWithSemanticDefinition = result.componentCatalog.filter((component) => component.implementedInBlazor && component.hasSemanticDefinition).map((component) => component.name);
  result.scope.existingBlazorComponentsWithoutSemanticDefinition = result.componentCatalog.filter((component) => component.implementedInBlazor && !component.hasSemanticDefinition).map((component) => component.name);
}
fs.mkdirSync(path.dirname(output), { recursive: true });
fs.writeFileSync(output, `${JSON.stringify(result, null, 2)}\n`);
console.log(JSON.stringify({ output, ...result.scope }));
