#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import process from 'node:process';
import { readReactSnapshot } from './html-structure-compare.mjs';
import { resolveReactScenario } from './react-scenario-adapters.mjs';

const defaultReactRoot = '/Volumes/Data/GitHub/ant-design';
const defaultOutput = '/Volumes/Data/GitHub/ant-design-blazor/docs/antd-v6/react-dom-test-catalog.json';

function walk(directory) {
  return fs.readdirSync(directory, { withFileTypes: true }).flatMap((entry) => {
    const fullPath = path.join(directory, entry.name);
    return entry.isDirectory() ? walk(fullPath) : [fullPath];
  });
}

function snapshotCases(file, component) {
  const source = fs.readFileSync(file, 'utf8');
  const sourcePath = resolveReactSource(file, source);
  const cases = [];
  const expression = /exports\[`([\s\S]*?)`\]\s*=\s*`/g;
  let match;
  while ((match = expression.exec(source))) {
    const scenario = resolveReactScenario(component, match[1]);
    const originalSource = sourcePath && fs.existsSync(sourcePath) ? fs.readFileSync(sourcePath, 'utf8') : '';
    cases.push({
      component,
      react: {
        snapshot: file,
        key: match[1],
        expectedMarkup: readReactSnapshot(source, match[1]),
        sourceFile: sourcePath,
        sourceSnippet: originalSource.split('\n').slice(0, 180).join('\n'),
      },
      classification: /semantic|a11y|keyboard|delay|wave|image|demo/i.test(file) ? 'dynamic-or-special' : 'snapshot',
      status: scenario.status,
      blazorScenario: scenario,
    });
  }
  return cases;
}

function resolveReactSource(snapshotFile, snapshotSource) {
  const demoMatch = snapshotSource.match(/renders (components\/[\w-]+\/demo\/[\w-]+\.tsx)/);
  if (demoMatch) {
    const componentsMarker = `${path.sep}components${path.sep}`;
    const markerIndex = snapshotFile.indexOf(componentsMarker);
    const reactRoot = markerIndex >= 0 ? snapshotFile.slice(0, markerIndex) : path.dirname(snapshotFile);
    const candidate = path.join(reactRoot, demoMatch[1]);
    if (fs.existsSync(candidate)) return candidate;
  }
  const snapshotName = path.basename(snapshotFile).replace(/\.snap$/, '');
  const candidate = path.join(path.dirname(snapshotFile), '..', snapshotName);
  return fs.existsSync(candidate) ? candidate : null;
}

export function generateReactDomCatalog(reactRoot = defaultReactRoot) {
  const snapshotFiles = walk(path.join(reactRoot, 'components'))
    .filter((file) => file.includes(`${path.sep}__snapshots__${path.sep}`) && file.endsWith('.snap'));
  const cases = snapshotFiles.flatMap((file) => {
    const relative = path.relative(path.join(reactRoot, 'components'), file);
    const component = relative.split(path.sep)[0];
    if (component === '__tests__' || component.startsWith('_')) return [];
    return snapshotCases(file, component);
  }).map((item, index) => ({ ...item, id: `${item.component}-${index + 1}` }));
  const components = [...new Set(cases.map(({ component }) => component))].sort();
  const adapted = cases.filter((item) => item.status === 'scenario-adapted').length;
  return {
    schemaVersion: 1,
    phase: 'red-baseline',
    source: { repository: 'ant-design/ant-design', root: reactRoot },
    summary: {
      components: components.length,
      snapshotFiles: snapshotFiles.length,
      cases: cases.length,
      adapted,
      pending: cases.length - adapted,
    },
    components,
    cases,
  };
}

if (import.meta.url === `file://${process.argv[1]}`) {
  const reactRoot = process.argv[2] ?? defaultReactRoot;
  const output = process.argv[3] ?? defaultOutput;
  const catalog = generateReactDomCatalog(reactRoot);
  fs.mkdirSync(path.dirname(output), { recursive: true });
  fs.writeFileSync(output, `${JSON.stringify(catalog, null, 2)}\n`);
  console.log(JSON.stringify({ output, ...catalog.summary }, null, 2));
}
