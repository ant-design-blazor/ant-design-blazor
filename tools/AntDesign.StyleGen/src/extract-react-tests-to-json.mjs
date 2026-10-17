#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import process from 'node:process';
import ts from 'typescript';
import { readReactSnapshot } from './html-structure-compare.mjs';

const defaultReactRoot = '/Volumes/Data/GitHub/ant-design';
const defaultOutput = '/Volumes/Data/GitHub/ant-design-blazor/docs/antd-v6/react-test-cases.json';

function walk(directory) {
  return fs.readdirSync(directory, { withFileTypes: true }).flatMap((entry) => {
    const fullPath = path.join(directory, entry.name);
    return entry.isDirectory() ? walk(fullPath) : [fullPath];
  });
}

function text(node, sourceFile) {
  return node ? node.getText(sourceFile) : null;
}

function callName(expression) {
  if (ts.isIdentifier(expression)) return expression.text;
  if (ts.isPropertyAccessExpression(expression)) return `${callName(expression.expression)}.${expression.name.text}`;
  return null;
}

function extractCalls(node, sourceFile, names) {
  const calls = [];
  function visit(current) {
    if (ts.isCallExpression(current)) {
      const name = callName(current.expression);
      if (!names || names.has(name) || [...names].some((prefix) => name?.startsWith(`${prefix}.`) || name?.endsWith(`.${prefix}`))) {
        // `expect(value).toHaveClass(...)` is represented by several nested
        // AST nodes. Preserve the complete matcher chain so the downstream
        // Blazor translator can map the assertion instead of receiving only
        // `expect(value)`.
        let sourceNode = current;
        if (name === 'expect') {
          let parent = current.parent;
          while (parent && (ts.isPropertyAccessExpression(parent) || ts.isCallExpression(parent))) {
            sourceNode = parent;
            parent = parent.parent;
          }
        }
        calls.push({ name, source: text(sourceNode, sourceFile), line: sourceFile.getLineAndCharacterOfPosition(current.getStart()).line + 1 });
      }
    }
    ts.forEachChild(current, visit);
  }
  visit(node);
  return calls;
}

function extractFile(file, reactRoot) {
  const source = fs.readFileSync(file, 'utf8');
  const sourceFile = ts.createSourceFile(file, source, ts.ScriptTarget.Latest, true, ts.ScriptKind.TSX);
  const snapshotFile = path.join(path.dirname(file), '__snapshots__', `${path.basename(file)}.snap`);
  const snapshotSource = fs.existsSync(snapshotFile) ? fs.readFileSync(snapshotFile, 'utf8') : '';
  const snapshotEntries = [...snapshotSource.matchAll(/exports\[`([\s\S]*?)`\]\s*=\s*`/g)].map((match) => ({
    key: match[1],
    markup: readReactSnapshot(snapshotSource, match[1]),
  }));
  const cases = [];
  function visit(node) {
    if (ts.isCallExpression(node) && ts.isIdentifier(node.expression) && ['it', 'test', 'fit', 'xit'].includes(node.expression.text)) {
      const nameNode = node.arguments[0];
      const callback = node.arguments[1];
      if (nameNode && callback && (ts.isFunctionLike(callback))) {
        const name = nameNode.text ?? text(nameNode, sourceFile);
        const snapshots = snapshotEntries.filter((entry) =>
          entry.key.toLowerCase().includes(String(name).toLowerCase()));
        cases.push({
          id: `${path.relative(reactRoot, file).replaceAll(path.sep, '/')}:${name}:${node.getStart()}`,
          sourceFile: path.relative(reactRoot, file).replaceAll(path.sep, '/'),
          name,
          source: text(node, sourceFile),
          renderCalls: extractCalls(callback, sourceFile, new Set(['render', 'rerender'])),
          interactionCalls: extractCalls(callback, sourceFile, new Set(['fireEvent', 'userEvent', 'act', 'waitFakeTimer'])),
          assertionCalls: extractCalls(callback, sourceFile, new Set(['expect'])),
          snapshotCalls: extractCalls(callback, sourceFile, new Set(['toMatchSnapshot', 'toMatchInlineSnapshot'])),
          snapshots,
          status: 'extracted',
        });
      }
    }
    ts.forEachChild(node, visit);
  }
  visit(sourceFile);
  return cases;
}

export function extractReactTests(reactRoot = defaultReactRoot) {
  const files = walk(path.join(reactRoot, 'components'))
    .filter((file) => file.includes(`${path.sep}__tests__${path.sep}`))
    .filter((file) => !file.includes(`${path.sep}__snapshots__${path.sep}`))
    .filter((file) => /\.(tsx?|jsx?)$/.test(file));
  const cases = files.flatMap((file) => extractFile(file, reactRoot));
  return {
    schemaVersion: 1,
    source: { repository: 'ant-design/ant-design', root: reactRoot },
    summary: { testFiles: files.length, cases: cases.length },
    cases,
  };
}

if (import.meta.url === `file://${process.argv[1]}`) {
  const reactRoot = process.argv[2] ?? defaultReactRoot;
  const output = process.argv[3] ?? defaultOutput;
  const result = extractReactTests(reactRoot);
  fs.mkdirSync(path.dirname(output), { recursive: true });
  fs.writeFileSync(output, `${JSON.stringify(result, null, 2)}\n`);
  console.log(JSON.stringify({ output, ...result.summary }, null, 2));
}
