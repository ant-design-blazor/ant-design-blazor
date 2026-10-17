#!/usr/bin/env node

import fs from 'node:fs';
import process from 'node:process';
import { JSDOM } from 'jsdom';

const DEFAULT_IGNORED_ATTRIBUTES = new Set(['id']);

function escapeRegExp(value) {
  return value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

export function readReactSnapshot(source, key) {
  const expression = new RegExp(
    "exports\\[`" + escapeRegExp(key) + "`\\]\\s*=\\s*`([\\s\\S]*?)`;",
  );
  const match = source.match(expression);
  if (!match) throw new Error(`React snapshot key not found: ${key}`);
  return match[1].trim();
}

function normalizeNode(node, ignoredAttributes) {
  if (node.nodeType === 3) {
    const text = node.textContent.replace(/\s+/g, ' ').trim();
    return text ? { type: 'text', text } : null;
  }
  if (node.nodeType !== 1) return null;
  const attributes = [...node.attributes]
    .filter(({ name }) => !ignoredAttributes.has(name))
    .map(({ name, value }) => [name, value])
    .sort(([left], [right]) => left.localeCompare(right));
  return {
    type: 'element',
    tag: node.tagName.toLowerCase(),
    attributes,
    children: [...node.childNodes].map((child) => normalizeNode(child, ignoredAttributes)).filter(Boolean),
  };
}

function parseMarkup(markup, ignoredAttributes) {
  const document = new JSDOM(`<body>${markup}</body>`).window.document;
  return [...document.body.childNodes].map((node) => normalizeNode(node, ignoredAttributes)).filter(Boolean);
}

function compareTrees(expected, actual, path = 'root', differences = []) {
  if (!expected || !actual) {
    if (expected !== actual) differences.push({ path, expected, actual });
    return differences;
  }
  if (expected.type !== actual.type) {
    differences.push({ path, field: 'type', expected: expected.type, actual: actual.type });
    return differences;
  }
  if (expected.type === 'text') {
    if (expected.text !== actual.text) differences.push({ path, field: 'text', expected: expected.text, actual: actual.text });
    return differences;
  }
  if (expected.tag !== actual.tag) differences.push({ path, field: 'tag', expected: expected.tag, actual: actual.tag });
  if (JSON.stringify(expected.attributes) !== JSON.stringify(actual.attributes)) {
    differences.push({ path, field: 'attributes', expected: expected.attributes, actual: actual.attributes });
  }
  if (expected.children.length !== actual.children.length) {
    differences.push({ path, field: 'children.length', expected: expected.children.length, actual: actual.children.length });
  }
  const childCount = Math.max(expected.children.length, actual.children.length);
  for (let index = 0; index < childCount; index += 1) {
    compareTrees(expected.children[index], actual.children[index], `${path}/${index}`, differences);
  }
  return differences;
}

export function compareMarkup(expectedMarkup, actualMarkup, options = {}) {
  const ignoredAttributes = new Set(options.ignoredAttributes ?? DEFAULT_IGNORED_ATTRIBUTES);
  const expected = parseMarkup(expectedMarkup, ignoredAttributes);
  const actual = parseMarkup(actualMarkup, ignoredAttributes);
  const differences = compareTrees(
    { type: 'element', tag: '__root__', attributes: [], children: expected },
    { type: 'element', tag: '__root__', attributes: [], children: actual },
  );
  return { equal: differences.length === 0, differences, expected, actual };
}

function parseArgs(args) {
  const values = {};
  for (let index = 0; index < args.length; index += 1) {
    if (!args[index].startsWith('--')) continue;
    values[args[index].slice(2)] = args[index + 1];
    index += 1;
  }
  return values;
}

if (import.meta.url === `file://${process.argv[1]}`) {
  const args = parseArgs(process.argv.slice(2));
  if (!args['react-snapshot'] || !args['react-key'] || !args['blazor-html']) {
    console.error('Usage: node html-structure-compare.mjs --react-snapshot <file> --react-key <key> --blazor-html <file> [--ignore-attr <name>]');
    process.exit(2);
  }
  const ignoredAttributes = new Set(DEFAULT_IGNORED_ATTRIBUTES);
  if (args['ignore-attr']) ignoredAttributes.add(args['ignore-attr']);
  const result = compareMarkup(
    readReactSnapshot(fs.readFileSync(args['react-snapshot'], 'utf8'), args['react-key']),
    fs.readFileSync(args['blazor-html'], 'utf8'),
    { ignoredAttributes },
  );
  console.log(JSON.stringify(result, null, 2));
  if (!result.equal) process.exitCode = 1;
}
