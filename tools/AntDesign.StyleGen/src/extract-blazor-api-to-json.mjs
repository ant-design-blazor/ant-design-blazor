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
  // Do not cap the XML documentation length between [Parameter] and the
  // declaration. Some public parameters (for example OverlayTrigger.Trigger)
  // have longer remarks. Stop at the next [Parameter] so a missing/malformed
  // declaration cannot accidentally capture the following parameter.
  const pattern = /\[Parameter(?:\([^\)]*\))?\](?:(?!\[Parameter)[\s\S])*?\bpublic\s+(?<type>[\w<>,?.\[\]() :]+?)\s+(?<name>[A-Za-z_]\w*)\s*(?:\/\/[^\r\n]*)?\s*\{/g;
  for (const match of source.matchAll(pattern)) {
    parameters.push({
      name: match.groups.name,
      type: match.groups.type.trim().replace(/\s+/g, ' '),
      optional: true,
      releaseVersion: match[0].match(/\[\s*PublicApi(?:Attribute)?\s*\(\s*"([^"]+)"\s*\)\s*\]/)?.[1] ?? null,
    });
  }
  return parameters;
}

export function extractBlazorApi(repoRoot) {
  const componentRoot = path.join(repoRoot, 'components');
  const files = walk(componentRoot);
  const sourceByFile = new Map(files.map((file) => [file, fs.readFileSync(file, 'utf8')]));
  const classByName = new Map();
  // Component API is inherited from non-partial bases too (notably
  // AntDomComponentBase's Class/Style/Id/RefBack). Accept modifiers in either
  // order and do not require `partial`, or the API map reports common React
  // props as missing even though the Blazor components expose them.
  const classPattern = /\bpublic\s+(?:(?:abstract|sealed|static)\s+)*(?:partial\s+)?class\s+(?<name>[A-Za-z_]\w*)(?:\s*<[^>{}]*>)?\s*:\s*(?<base>[A-Za-z_]\w*)(?:\s*<[^>{}]*>)?/g;
  for (const [file, source] of sourceByFile) {
    for (const match of source.matchAll(classPattern)) {
      const entries = classByName.get(match.groups.name) ?? [];
      entries.push({ base: match.groups.base, parameters: extractParameters(source) });
      classByName.set(match.groups.name, entries);
    }
  }

  const byComponent = new Map();
  for (const file of files) {
    const relative = path.relative(componentRoot, file).replaceAll(path.sep, '/');
    const directory = relative.split('/')[0];
    if (!directory || directory === 'core') continue;
    const source = fs.readFileSync(file, 'utf8');
    const parameters = extractParameters(source);
    if (!parameters.length) continue;
    if (!byComponent.has(directory)) byComponent.set(directory, []);
    byComponent.get(directory).push(...parameters);
  }

  // Include parameters declared on component base classes. React's public
  // APIs inherit common props too, so omitting Blazor base parameters creates
  // false missing-API results for Dropdown, Tooltip, and Select.
  const normalize = (value) => value.replace(/[^a-z0-9]/gi, '').toLowerCase();
  for (const [directory, parameters] of byComponent) {
    const candidates = [...classByName.entries()].filter(([name]) => normalize(name) === normalize(directory));
    const pending = candidates.map(([name]) => name);
    const visited = new Set();
    while (pending.length) {
      const className = pending.pop();
      if (visited.has(className)) continue;
      visited.add(className);
      for (const declaration of classByName.get(className) ?? []) {
        parameters.push(...declaration.parameters);
        if (classByName.has(declaration.base)) pending.push(declaration.base);
      }
    }
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
