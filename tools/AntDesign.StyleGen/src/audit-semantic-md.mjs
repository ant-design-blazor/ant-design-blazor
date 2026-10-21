#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { createHash } from 'node:crypto';
import { JSDOM } from 'jsdom';

const repoRoot = path.resolve(process.argv[2] || process.cwd());
const inventoryPath = path.resolve(process.argv[3] || path.join(repoRoot, 'docs/antd-v6/semantic-dom-inventory.json'));
const docsRoot = path.resolve(process.argv[4] || path.join(repoRoot, '../ant-design/_site'));
const outputPath = path.resolve(process.argv[5] || path.join(repoRoot, 'docs/antd-v6/semantic-md-catalog.json'));

function walk(directory) {
  if (!fs.existsSync(directory)) return [];
  return fs.readdirSync(directory, { withFileTypes: true }).flatMap((entry) => {
    const file = path.join(directory, entry.name);
    return entry.isDirectory() ? walk(file) : entry.name.startsWith('semantic') && entry.name.endsWith('.md') ? [file] : [];
  });
}

function collectMarkdownParts(markdown) {
  const parts = [];
  const pattern = /^-\s+([\w.-]+)（`(semantic-mark-[\w-]+)`）:\s*(.*)$/gm;
  for (const match of markdown.matchAll(pattern)) {
    parts.push({ name: match[1], demoMarkerClass: match[2], description: match[3].trim() });
  }
  return parts;
}

function collectHtmlStructure(markdown) {
  const html = markdown.match(/```html\s*([\s\S]*?)\s*```/i)?.[1];
  if (!html) return { markers: [], tree: null };
  const document = new JSDOM(html).window.document;
  const markers = [...document.querySelectorAll('[class*="semantic-mark-"]')].flatMap((element) => {
    const markerClasses = [...element.classList].filter((name) => name.startsWith('semantic-mark-'));
    return markerClasses.map((demoMarkerClass) => ({
      demoMarkerClass,
      tagName: element.tagName.toLowerCase(),
      classNames: [...element.classList].filter((name) => !name.startsWith('semantic-mark-')),
      parentTagName: element.parentElement?.tagName.toLowerCase() ?? null,
      parentDemoMarkerClasses: [...(element.parentElement?.classList ?? [])].filter((name) => name.startsWith('semantic-mark-')),
    }));
  });

  function describeElement(element) {
    const classNames = [...element.classList];
    return {
      tagName: element.tagName.toLowerCase(),
      classNames: classNames.filter((name) => !name.startsWith('semantic-mark-')),
      demoMarkerClasses: classNames.filter((name) => name.startsWith('semantic-mark-')),
      attributes: Object.fromEntries([...element.attributes]
        // SVG path data accounts for most of the generated snapshot size and
        // does not affect DOM structure or CSS selector parity.
        .filter(({ name }) => name !== 'class' && name !== 'd')
        .map(({ name, value }) => [name, value])
        .sort(([left], [right]) => left.localeCompare(right))),
      children: [...element.children].map(describeElement),
    };
  }

  return { markers, tree: [...document.body.children].map(describeElement) };
}

function parsePage(file) {
  const relative = path.relative(docsRoot, file).split(path.sep).join('/');
  const match = relative.match(/^components\/([^/]+)\/(semantic(?:_[^/]+)?\.md)$/);
  if (!match) return null;
  const [, directoryName, fileName] = match;
  const component = directoryName.replace(/-cn$/, '');
  const locale = directoryName.endsWith('-cn') ? 'zh-CN' : 'en-US';
  const variant = fileName.replace(/^semantic/, '').replace(/\.md$/, '').replace(/^_/, '');
  const semanticComponent = component === 'card' && variant === 'meta' ? 'card-meta' : component;
  const markdown = fs.readFileSync(file, 'utf8');
  const parts = collectMarkdownParts(markdown);
  const abstractDom = collectHtmlStructure(markdown);
  for (const part of parts) {
    const markerClass = part.demoMarkerClass;
    const matchingNodes = abstractDom.markers.filter((marker) => marker.demoMarkerClass === markerClass);
    const commonClasses = matchingNodes.length
      ? matchingNodes[0].classNames.filter((className) => className.startsWith('ant-')
        && matchingNodes.every((marker) => marker.classNames.includes(className)))
      : [];
    part.reactTagNames = sorted(matchingNodes.map((marker) => marker.tagName));
    part.reactClassNames = commonClasses;
    part.selectorCandidate = commonClasses.length ? `.${commonClasses[0]}` : null;
  }
  return {
    component: semanticComponent,
    locale,
    variant: variant || null,
    source: relative,
    parts,
    abstractDom,
  };
}

function sorted(values) {
  return [...new Set(values)].sort();
}

const inventory = JSON.parse(fs.readFileSync(inventoryPath, 'utf8'));
const pages = walk(path.join(docsRoot, 'components')).map(parsePage).filter(Boolean);
if (pages.length === 0) {
  throw new Error(`No generated semantic*.md files found under ${path.join(docsRoot, 'components')}. Build/export the React docs first.`);
}

const apiPaths = new Map();
const previewPaths = new Map();
for (const entry of inventory.entries ?? []) {
  const paths = apiPaths.get(entry.component) ?? new Set();
  for (const semanticPath of entry.semanticPaths ?? []) paths.add(semanticPath);
  apiPaths.set(entry.component, paths);
  const preview = previewPaths.get(entry.component) ?? new Set();
  for (const demo of entry.dumiSemanticSources ?? []) {
    for (const semanticPath of demo.semanticPaths ?? []) preview.add(semanticPath);
  }
  previewPaths.set(entry.component, preview);
}

const pagePaths = new Map();
for (const page of pages) {
  const names = pagePaths.get(page.component) ?? new Set();
  for (const part of page.parts) names.add(part.name);
  pagePaths.set(page.component, names);
}

const comparison = [...pagePaths.keys()].sort().map((component) => {
  const documented = pagePaths.get(component);
  const api = apiPaths.get(component) ?? new Set();
  const preview = previewPaths.get(component) ?? new Set();
  const equivalent = (name, target) => target.has(name)
    || (name === 'popup' && target.has('popup.root'))
    || (name === 'popup.root' && target.has('popup'));
  return {
    component,
    documentedParts: documented.size,
    apiSemanticPaths: api.size,
    previewSemanticPaths: preview.size,
    documentedButMissingFromApi: sorted([...documented].filter((name) => !equivalent(name, api))),
    apiButMissingFromDocs: sorted([...api].filter((name) => !equivalent(name, documented))),
    documentedButMissingFromPreview: preview.size
      ? sorted([...documented].filter((name) => !equivalent(name, preview)))
      : [],
    previewButMissingFromDocs: preview.size
      ? sorted([...preview].filter((name) => !equivalent(name, documented)))
      : [],
  };
});

const abstractDomMarkers = pages.reduce((total, page) => total + page.abstractDom.markers.length, 0);
const abstractDomSnapshots = new Map();
for (const page of pages) {
  const serialized = JSON.stringify(page.abstractDom);
  const id = createHash('sha256').update(serialized).digest('hex').slice(0, 16);
  abstractDomSnapshots.set(id, page.abstractDom);
  page.abstractDomSnapshot = id;
  delete page.abstractDom;
}

const result = {
  schemaVersion: 1,
  source: { repository: 'ant-design', kind: 'generated-dumi-semantic-markdown', docsRoot: path.relative(repoRoot, docsRoot).split(path.sep).join('/') },
  scope: {
    files: pages.length,
    components: pagePaths.size,
    documentedParts: pages.reduce((total, page) => total + page.parts.length, 0),
    abstractDomMarkers,
    pagesWithoutSemanticParts: pages.filter((page) => page.parts.length === 0).length,
    componentsWithDifferences: comparison.filter((item) => item.documentedButMissingFromApi.length || item.apiButMissingFromDocs.length).length,
  },
  pages,
  abstractDomSnapshots: Object.fromEntries(abstractDomSnapshots),
  comparison,
};

fs.mkdirSync(path.dirname(outputPath), { recursive: true });
fs.writeFileSync(outputPath, `${JSON.stringify(result, null, 2)}\n`);
console.log(JSON.stringify({ output: outputPath, ...result.scope }, null, 2));

if (result.scope.componentsWithDifferences > 0 || result.scope.pagesWithoutSemanticParts > 0) process.exitCode = 1;
