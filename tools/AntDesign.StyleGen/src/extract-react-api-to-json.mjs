#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import process from 'node:process';
import ts from 'typescript';

function walk(directory) {
  return fs.readdirSync(directory, { withFileTypes: true }).flatMap((entry) => {
    const fullPath = path.join(directory, entry.name);
    if (entry.isDirectory()) return walk(fullPath);
    return /\.(tsx?|jsx?)$/.test(entry.name) && !entry.name.includes('.test.') ? [fullPath] : [];
  });
}

function typeText(node, sourceFile) {
  return node ? node.getText(sourceFile).replace(/\s+/g, ' ').trim() : null;
}

function propertyFromMember(member, sourceFile) {
  if (!ts.isPropertySignature(member) || !member.name) return null;
  const name = member.name.getText(sourceFile).replace(/^['"]|['"]$/g, '');
  const type = typeText(member.type, sourceFile) ?? 'unknown';
  const callback = type.includes('=>') || /^\(?[^)]*\)?\s*=>/.test(type);
  return {
    name,
    optional: Boolean(member.questionToken),
    type,
    ...(callback ? { kind: 'callback' } : { kind: 'value' }),
  };
}

function collectDeclarations(sourceFile) {
  const declarations = [];
  function visit(node) {
    if (ts.isInterfaceDeclaration(node) && /Props$/.test(node.name.text)) {
      declarations.push({ name: node.name.text, extends: node.heritageClauses?.flatMap((clause) => clause.types.map((type) => type.expression.getText(sourceFile))) ?? [], members: node.members.map((member) => propertyFromMember(member, sourceFile)).filter(Boolean) });
    }
    if (ts.isTypeAliasDeclaration(node) && /Props$/.test(node.name.text) && ts.isTypeLiteralNode(node.type)) {
      declarations.push({ name: node.name.text, extends: [], members: node.type.members.map((member) => propertyFromMember(member, sourceFile)).filter(Boolean) });
    }
    ts.forEachChild(node, visit);
  }
  visit(sourceFile);
  const byName = new Map(declarations.map((declaration) => [declaration.name, declaration]));
  function expand(declaration, seen = new Set()) {
    if (seen.has(declaration.name)) return [];
    seen.add(declaration.name);
    const inherited = declaration.extends.flatMap((name) => {
      const parent = byName.get(name);
      return parent ? [...expand(parent, seen), ...parent.members] : [];
    });
    return [...inherited, ...declaration.members].filter((member, index, all) => all.findIndex((candidate) => candidate.name === member.name) === index);
  }
  return declarations.map((declaration) => ({ ...declaration, members: expand(declaration) }));
}

export function extractReactApi(reactRoot) {
  const componentRoot = path.join(reactRoot, 'components');
  const files = walk(componentRoot);
  const props = [];
  for (const file of files) {
    const source = fs.readFileSync(file, 'utf8');
    const sourceFile = ts.createSourceFile(file, source, ts.ScriptTarget.Latest, true, ts.ScriptKind.TSX);
    for (const declaration of collectDeclarations(sourceFile)) {
      const relative = path.relative(reactRoot, file).replaceAll(path.sep, '/');
      const component = relative.split('/')[1] ?? path.basename(file, path.extname(file));
      props.push({
        component,
        declaration: declaration.name,
        sourceFile: relative,
        properties: declaration.members,
      });
    }
  }
  return {
    schemaVersion: 1,
    source: { repository: 'ant-design/ant-design', root: reactRoot },
    summary: {
      propDeclarations: props.length,
      components: new Set(props.map((item) => item.component)).size,
      callbacks: props.reduce((count, item) => count + item.properties.filter((property) => property.kind === 'callback').length, 0),
    },
    props,
  };
}

if (import.meta.url === `file://${process.argv[1]}`) {
  const reactRoot = process.argv[2] ?? '/Volumes/Data/GitHub/ant-design';
  const output = process.argv[3] ?? path.join(process.cwd(), 'docs/antd-v6/react-api.json');
  const result = extractReactApi(reactRoot);
  fs.mkdirSync(path.dirname(output), { recursive: true });
  fs.writeFileSync(output, `${JSON.stringify(result, null, 2)}\n`);
  console.log(JSON.stringify({ output, ...result.summary }, null, 2));
}
