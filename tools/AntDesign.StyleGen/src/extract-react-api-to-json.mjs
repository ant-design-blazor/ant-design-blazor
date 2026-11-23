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

function propertyFromSymbol(symbol, checker) {
  const declaration = symbol.valueDeclaration ?? symbol.declarations?.[0];
  if (!declaration) return null;
  const type = checker.typeToString(checker.getTypeOfSymbolAtLocation(symbol, declaration));
  const callback = type.includes('=>');
  return {
    name: symbol.getName(),
    optional: Boolean(symbol.flags & ts.SymbolFlags.Optional),
    type,
    ...(callback ? { kind: 'callback' } : { kind: 'value' }),
  };
}

function collectDeclarations(sourceFile, checker) {
  const allDeclarations = [];
  function visit(node) {
    if (ts.isInterfaceDeclaration(node)) {
      allDeclarations.push({ name: node.name.text, typeNode: node, extends: node.heritageClauses?.flatMap((clause) => clause.types.map((type) => type.expression.getText(sourceFile))) ?? [] });
    }
    if (ts.isTypeAliasDeclaration(node)) {
      allDeclarations.push({ name: node.name.text, typeNode: node.type, extends: [] });
    }
    ts.forEachChild(node, visit);
  }
  visit(sourceFile);
  const byName = new Map(allDeclarations.map((declaration) => [declaration.name, declaration]));

  function membersFromDeclaration(name, seen = new Set()) {
    const declaration = byName.get(name);
    if (!declaration || seen.has(name)) return [];
    const nextSeen = new Set(seen).add(name);
    const ownMembers = ts.isInterfaceDeclaration(declaration.typeNode)
      ? declaration.typeNode.members.map((member) => propertyFromMember(member, sourceFile)).filter(Boolean)
      : membersFromType(declaration.typeNode, nextSeen);
    const inherited = declaration.extends.flatMap((baseName) => membersFromDeclaration(baseName, nextSeen));
    return [...inherited, ...ownMembers];
  }

  function membersFromType(typeNode, seen = new Set()) {
    if (!typeNode) return [];
    if (ts.isParenthesizedTypeNode(typeNode)) return membersFromType(typeNode.type, seen);
    if (ts.isTypeLiteralNode(typeNode)) return typeNode.members.map((member) => propertyFromMember(member, sourceFile)).filter(Boolean);
    if (ts.isIntersectionTypeNode(typeNode) || ts.isUnionTypeNode(typeNode)) {
      return typeNode.types.flatMap((type) => membersFromType(type, seen));
    }
    if (ts.isTypeReferenceNode(typeNode)) {
      const name = typeNode.typeName.getText(sourceFile);
      return membersFromDeclaration(name, seen);
    }
    return [];
  }

  function membersFromTypeChecker(declaration) {
    if (!checker) return null;
    try {
      const type = checker.getTypeAtLocation(declaration);
      return checker.getPropertiesOfType(type)
        // React's generic HTMLElement/ARIA attributes are documented through
        // the shared Common props contract. They are not component-specific
        // props and would otherwise overwhelm the component mapping with the
        // same DOM surface on every inherited control.
        .filter((symbol) => !symbol.declarations?.every((member) =>
          /[/\\]node_modules[/\\]@types[/\\]react[/\\].*\.d\.ts$/i.test(member.getSourceFile().fileName)))
        .map((symbol) => propertyFromSymbol(symbol, checker))
        .filter(Boolean);
    } catch {
      return null;
    }
  }

  return allDeclarations
    .filter((declaration) => /Props$/.test(declaration.name))
    .map((declaration) => ({
      name: declaration.name,
      members: (membersFromTypeChecker(declaration.typeNode) ?? membersFromDeclaration(declaration.name))
        .filter((member, index, all) => all.findIndex((candidate) => candidate.name === member.name) === index),
    }));
}

export function extractReactApi(reactRoot) {
  const componentRoot = path.join(reactRoot, 'components');
  const files = walk(componentRoot);
  const apiFiles = files.filter((file) => {
    if (/(?:^|[/\\])demo(?:[/\\])/.test(file)) return false;
    return /(?:interface|type)\s+[A-Za-z_$][\w$]*Props\b/.test(fs.readFileSync(file, 'utf8'));
  });
  const configPath = path.join(reactRoot, 'tsconfig.json');
  let compilerOptions = {
    target: ts.ScriptTarget.Latest,
    module: ts.ModuleKind.ESNext,
    moduleResolution: ts.ModuleResolutionKind.Bundler,
    jsx: ts.JsxEmit.React,
    skipLibCheck: true,
    noEmit: true,
    allowSyntheticDefaultImports: true,
  };
  if (fs.existsSync(configPath)) {
    const config = ts.readConfigFile(configPath, ts.sys.readFile);
    if (!config.error) {
      compilerOptions = {
        ...compilerOptions,
        ...ts.parseJsonConfigFileContent(config.config, ts.sys, reactRoot).options,
        skipLibCheck: true,
        noEmit: true,
      };
    }
  }
  const program = ts.createProgram(apiFiles, compilerOptions);
  const checker = program.getTypeChecker();
  const props = [];
  for (const file of files) {
    const source = fs.readFileSync(file, 'utf8');
    const sourceFile = program.getSourceFile(file)
      ?? ts.createSourceFile(file, source, ts.ScriptTarget.Latest, true, ts.ScriptKind.TSX);
    const sourceChecker = program.getSourceFile(file) ? checker : null;
    for (const declaration of collectDeclarations(sourceFile, sourceChecker)) {
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
