#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';

const repoRoot = process.argv[2] || process.cwd();
const inventoryPath = process.argv[3] || path.join(repoRoot, 'docs/antd-v6/semantic-dom-inventory.json');
const existingPath = process.argv[4] || path.join(repoRoot, 'site/AntDesign.Docs/Services/SemanticDomGeneratedCatalog.cs');
const outputPath = process.argv[5] || existingPath;

const inventory = JSON.parse(fs.readFileSync(inventoryPath, 'utf8'));
const existing = fs.existsSync(existingPath) ? fs.readFileSync(existingPath, 'utf8') : '';

function kebab(value) {
  return value
    .replace(/([a-z0-9])([A-Z])/g, '$1-$2')
    .replace(/[^a-zA-Z0-9]+/g, '-')
    .replace(/^-|-$/g, '')
    .toLowerCase();
}

function unescapeCSharp(value) {
  return value.replace(/\\"/g, '"').replace(/\\\\/g, '\\');
}

function parseExistingNodes(source) {
  const result = new Map();
  const componentPattern = /\["([^\"]+)"\]\s*=\s*(new\[\]|Array\.Empty<SemanticDomNode>\(\))\s*(?:\{([\s\S]*?)\}|,)/g;
  let componentMatch;
  while ((componentMatch = componentPattern.exec(source))) {
    const component = componentMatch[1];
    const body = componentMatch[3] || '';
    const nodes = new Map();
    const nodePattern = /new SemanticDomNode\("((?:\\.|[^"\\])*)",\s*"((?:\\.|[^"\\])*)",\s*"((?:\\.|[^"\\])*)",\s*"((?:\\.|[^"\\])*)"\)/g;
    let nodeMatch;
    while ((nodeMatch = nodePattern.exec(body))) {
      nodes.set(nodeMatch[1], {
        selector: unescapeCSharp(nodeMatch[2]),
        description: unescapeCSharp(nodeMatch[3]),
        version: unescapeCSharp(nodeMatch[4]),
      });
    }
    result.set(component, nodes);
  }
  return result;
}

function csharp(value) {
  return value.replaceAll('\\', '\\\\').replaceAll('"', '\\"');
}

function fallbackNode(component, semanticPath) {
  const base = `ant-${kebab(component)}`;
  const selector = semanticPath === 'root' ? `.${base}` : `.${base}-${kebab(semanticPath)}`;
  return {
    selector,
    description: `${component} ${semanticPath} Semantic DOM 节点`,
    version: '6.0.0',
  };
}

const existingNodes = parseExistingNodes(existing);
const inventoryComponents = new Map();
for (const entry of inventory.entries || []) {
  const paths = inventoryComponents.get(entry.component) || new Set();
  for (const semanticPath of entry.semanticPaths || []) paths.add(semanticPath);
  inventoryComponents.set(entry.component, paths);
}

// Preserve additional preview-only entries already present in the catalog.
for (const component of existingNodes.keys()) {
  if (!inventoryComponents.has(component)) inventoryComponents.set(component, new Set(existingNodes.get(component).keys()));
}

const components = [...inventoryComponents.keys()].sort((a, b) => a.localeCompare(b));
const lines = [
  'using System;',
  'using System.Collections.Generic;',
  'using System.Collections.ObjectModel;',
  '',
  'namespace AntDesign.Docs.Services;',
  '',
  'internal static class SemanticDomGeneratedCatalog',
  '{',
  '    internal static readonly IReadOnlyDictionary<string, IReadOnlyList<SemanticDomNode>> Nodes =',
  '        new ReadOnlyDictionary<string, IReadOnlyList<SemanticDomNode>>(',
  '            new Dictionary<string, IReadOnlyList<SemanticDomNode>>(StringComparer.OrdinalIgnoreCase)',
  '            {',
];

for (const component of components) {
  const oldNodes = existingNodes.get(component) || new Map();
  const semanticPaths = [...inventoryComponents.get(component)].sort((a, b) => a.localeCompare(b));
  const nodes = semanticPaths.map((semanticPath) => [
    semanticPath,
    oldNodes.get(semanticPath) || fallbackNode(component, semanticPath),
  ]);
  lines.push(`                ["${csharp(component)}"] = ${nodes.length ? 'new[]' : 'Array.Empty<SemanticDomNode>()'}${nodes.length ? '' : ','}`);
  if (nodes.length) {
    lines.push('                {');
    for (const [semanticPath, node] of nodes) {
      lines.push(`                    new SemanticDomNode("${csharp(semanticPath)}", "${csharp(node.selector)}", "${csharp(node.description)}", "${csharp(node.version)}"),`);
    }
    lines.push('                },');
  }
}

lines.push(
  '            });',
  '}',
  '',
);

fs.mkdirSync(path.dirname(outputPath), { recursive: true });
fs.writeFileSync(outputPath, lines.join('\n'));
console.log(JSON.stringify({ outputPath, components: components.length, inventoryComponents: inventoryComponents.size }));
