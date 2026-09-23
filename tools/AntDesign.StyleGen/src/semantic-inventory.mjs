#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';

const repoRoot = path.resolve(new URL('../../..', import.meta.url).pathname);
const reactRoot = path.resolve(repoRoot, '../ant-design/components');
const output = path.resolve(repoRoot, 'docs/antd-v6/semantic-dom-inventory.json');
const componentCatalogPath = path.resolve(repoRoot, '.vibedev/generated/antd-v6/ir/components.json');
const migratedStructureComponents = new Set(['alert', 'anchor', 'auto-complete', 'badge', 'breadcrumb', 'button', 'calendar', 'card', 'checkbox', 'collapse', 'color-picker', 'descriptions', 'divider', 'dropdown', 'empty', 'form', 'input', 'input-number', 'layout', 'listy', 'mentions', 'pagination', 'popover', 'popconfirm', 'progress', 'qr-code', 'radio', 'result', 'segmented', 'skeleton', 'slider', 'space', 'spin', 'splitter', 'statistic', 'steps', 'switch', 'table', 'tabs', 'tag', 'tooltip', 'tree', 'typography', 'upload']);
const unavailableSemanticPaths = new Map([['popconfirm', new Set(['content'])]]);
const blazorDirectoryAliases = new Map([['row', 'grid'], ['col', 'grid'], ['qrcode', 'qr-code']]);

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

function dumiSemanticDemos(component) {
  const directory = path.join(reactRoot, component, 'demo');
  if (!fs.existsSync(directory)) return [];
  return fs.readdirSync(directory).filter((file) => /^_semantic.*\.tsx$/.test(file)).sort().map((file) => {
    const relative = path.join(component, 'demo', file);
    const source = fs.readFileSync(path.join(reactRoot, relative), 'utf8');
    return {
      source: path.posix.join('components', component, 'demo', file),
      semanticPaths: [...new Set([...source.matchAll(/name\s*:\s*['"]([^'"]+)['"]/g)].map((match) => match[1]))].sort(),
    };
  });
}

const entries = [];
for (const file of walk(reactRoot).sort()) {
  const source = fs.readFileSync(file, 'utf8');
  for (const type of source.matchAll(/export type (\w+SemanticType)\s*=\s*\{/g)) {
    const body = objectBody(source, type.index + type[0].length - 1);
    const classNames = keys(body, 'classNames');
    const styles = keys(body, 'styles');
    const relative = path.relative(reactRoot, file).split(path.sep);
    const dumiSemanticSources = dumiSemanticDemos(relative[0]);
    // Inherited semantic types (for example PopoverSemanticType or
    // StepsSemanticType) are not expanded by the local source parser. Dumi's
    // semantic demonstrations enumerate the effective public slots.
    const paths = [...new Set([...classNames, ...styles, ...dumiSemanticSources.flatMap((demo) => demo.semanticPaths)])].sort();
    entries.push({
      component: relative[0],
      reactSource: path.posix.join('components', ...relative),
      blazorDirectory: fs.existsSync(path.join(repoRoot, 'components', relative[0])) ? path.posix.join('components', relative[0]) : null,
      semanticType: type[1],
      semanticPaths: paths,
      classNames,
      styles,
      dumiSemanticSources,
      classNameMapping: paths.map((semanticPath) => ({ semanticPath, reactClassName: null, blazorClassName: null, status: unavailableSemanticPaths.get(relative[0])?.has(semanticPath) ? 'pending-blazor-api' : migratedStructureComponents.has(relative[0]) ? 'structure-migrated-pending-semantic-injection' : 'pending' })),
      htmlSnapshot: { react: 'pending-runtime-render', blazor: migratedStructureComponents.has(relative[0]) ? 'structure-test-covered' : 'pending-render' },
      migrationStatus: migratedStructureComponents.has(relative[0]) ? 'structure-migrated' : 'inventory-only',
    });
  }
}

entries.sort((a, b) => `${a.component}/${a.semanticType}`.localeCompare(`${b.component}/${b.semanticType}`));
const result = {
  schemaVersion: 1,
  phase: 1,
  source: { repository: 'ant-design', path: '../ant-design/components', semanticSource: 'export *SemanticType definitions' },
  scope: { semanticDefinitions: entries.length, uniqueComponents: new Set(entries.map((entry) => entry.component)).size, uniqueSemanticPaths: new Set(entries.flatMap((entry) => entry.semanticPaths)).size },
  entries,
};
const dumiSources = entries.flatMap((entry) => entry.dumiSemanticSources);
result.scope.dumiSemanticDemoFiles = new Set(dumiSources.map((source) => source.source)).size;
result.scope.dumiSemanticPaths = new Set(dumiSources.flatMap((source) => source.semanticPaths)).size;
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
