#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import process from 'node:process';

function walk(directory) {
  return fs.readdirSync(directory, { withFileTypes: true }).flatMap((entry) => {
    const fullPath = path.join(directory, entry.name);
    if (entry.isDirectory()) return walk(fullPath);
    return /\.(cs|razor)$/.test(entry.name) ? [fullPath] : [];
  });
}

export function extractParameters(source) {
  const parameters = [];
  // A Parameter can have additional attributes, comments, a multiline type,
  // or a custom getter/setter. We only need the declaration header, so stop
  // at the next property opening brace instead of requiring `{ get; set; }`.
  const pattern = /\[Parameter(?:\([^\)]*\))?\][\s\S]{0,600}?\bpublic\s+(?<type>[\w<>,?.\[\]() :]+?)\s+(?<name>[A-Za-z_]\w*)\s*\{/g;
  for (const match of source.matchAll(pattern)) {
    parameters.push({
      name: match.groups.name,
      type: match.groups.type.trim().replace(/\s+/g, ' '),
      optional: true,
    });
  }
  return parameters;
}

export function extractBlazorApi(repoRoot) {
  const componentRoot = path.join(repoRoot, 'components');
  const byComponent = new Map();
  for (const file of walk(componentRoot)) {
    const relative = path.relative(componentRoot, file).replaceAll(path.sep, '/');
    const directory = relative.split('/')[0];
    if (!directory || directory === 'core') continue;
    const source = fs.readFileSync(file, 'utf8');
    const parameters = extractParameters(source);
    if (!parameters.length) continue;
    if (!byComponent.has(directory)) byComponent.set(directory, []);
    byComponent.get(directory).push(...parameters);
  }
  const components = [...byComponent.entries()].sort(([a], [b]) => a.localeCompare(b)).map(([component, values]) => ({
    component,
    parameters: values.filter((item, index, all) => all.findIndex((candidate) => candidate.name === item.name) === index),
  }));
  return {
    schemaVersion: 1,
    source: { repository: 'ant-design-blazor', root: repoRoot },
    summary: {
      components: components.length,
      parameters: components.reduce((count, item) => count + item.parameters.length, 0),
      callbacks: components.reduce((count, item) => count + item.parameters.filter((parameter) => parameter.type.includes('EventCallback')).length, 0),
    },
    components,
  };
}

if (import.meta.url === `file://${process.argv[1]}`) {
  const repoRoot = process.argv[2] ?? process.cwd();
  const output = process.argv[3] ?? path.join(process.cwd(), 'docs/antd-v6/blazor-api.json');
  const result = extractBlazorApi(repoRoot);
  fs.mkdirSync(path.dirname(output), { recursive: true });
  fs.writeFileSync(output, `${JSON.stringify(result, null, 2)}\n`);
  console.log(JSON.stringify({ output, ...result.summary }, null, 2));
}
