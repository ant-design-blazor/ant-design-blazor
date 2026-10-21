import ts from 'typescript';

function nameOf(node) {
  return ts.isIdentifier(node) ? node.text : ts.isQualifiedName(node) ? node.right.text : undefined;
}

function propertyName(node) {
  if (ts.isIdentifier(node) || ts.isStringLiteral(node) || ts.isNumericLiteral(node)) return node.text;
  return undefined;
}

export function extractSemanticTypes(sources) {
  const declarations = new Map();
  const parsed = sources.map(({ file, source }) => ({
    file,
    source,
    ast: ts.createSourceFile(file, source, ts.ScriptTarget.Latest, true, file.endsWith('.tsx') ? ts.ScriptKind.TSX : ts.ScriptKind.TS),
  }));

  for (const { ast } of parsed) {
    for (const statement of ast.statements) {
      if ((ts.isTypeAliasDeclaration(statement) || ts.isInterfaceDeclaration(statement)) && ts.isIdentifier(statement.name)) {
        const current = declarations.get(statement.name.text) ?? [];
        current.push(statement);
        declarations.set(statement.name.text, current);
      }
    }
  }

  function unwrapDeclaration(declaration) {
    if (ts.isInterfaceDeclaration(declaration)) return declaration;
    const type = declaration.type;
    if (ts.isTypeLiteralNode(type)) return type;
    if (ts.isIntersectionTypeNode(type)) return type;
    if (ts.isTypeReferenceNode(type) && type.typeArguments?.length) return type.typeArguments[0];
    return type;
  }

  function findProperty(typeNode, wanted, visiting = new Set()) {
    if (!typeNode) return undefined;
    if (ts.isIntersectionTypeNode(typeNode)) {
      for (const member of typeNode.types) {
        const found = findProperty(member, wanted, visiting);
        if (found) return found;
      }
    }
    if (ts.isTypeLiteralNode(typeNode) || ts.isInterfaceDeclaration(typeNode)) {
      const members = ts.isInterfaceDeclaration(typeNode) ? typeNode.members : typeNode.members;
      return members.find((member) => ts.isPropertySignature(member) && propertyName(member.name) === wanted)?.type;
    }
    if (ts.isTypeReferenceNode(typeNode)) {
      const targetName = nameOf(typeNode.typeName);
      if (!targetName || visiting.has(targetName)) return undefined;
      const nextVisiting = new Set(visiting).add(targetName);
      if (['NonNullable', 'Required', 'Partial', 'Readonly'].includes(targetName) && typeNode.typeArguments?.length) {
        return findProperty(typeNode.typeArguments[0], wanted, nextVisiting);
      }
      for (const declaration of declarations.get(targetName) ?? []) {
        const target = unwrapDeclaration(declaration);
        const found = findProperty(target, wanted, nextVisiting);
        if (found) return found;
      }
    }
    if (ts.isIndexedAccessTypeNode(typeNode)) {
      const index = ts.isLiteralTypeNode(typeNode.indexType) && ts.isStringLiteral(typeNode.indexType.literal)
        ? typeNode.indexType.literal.text
        : undefined;
      return index ? findProperty(findProperty(typeNode.objectType, index, visiting), wanted, visiting) : undefined;
    }
    return undefined;
  }

  function collect(typeNode, prefix, output, visiting = new Set()) {
    if (!typeNode) return;
    if (ts.isUnionTypeNode(typeNode)) {
      for (const member of typeNode.types) collect(member, prefix, output, visiting);
      return;
    }
    if (ts.isFunctionTypeNode(typeNode)) {
      collect(typeNode.type, prefix, output, visiting);
      return;
    }
    if (ts.isIntersectionTypeNode(typeNode)) {
      for (const member of typeNode.types) collect(member, prefix, output, visiting);
      return;
    }
    if (ts.isTypeLiteralNode(typeNode) || ts.isInterfaceDeclaration(typeNode)) {
      const members = typeNode.members;
      for (const member of members) {
        if (!ts.isPropertySignature(member) || !member.name || !member.type) continue;
        const name = propertyName(member.name);
        if (!name) continue;
        const semanticPath = prefix ? `${prefix}.${name}` : name;
        collect(member.type, semanticPath, output, visiting);
        // Nested records such as popup, body, and pagination are namespaces,
        // not DOM nodes. Keep only leaf paths such as popup.root and body.cell.
        if (![...output].some((path) => path.startsWith(`${semanticPath}.`))) output.add(semanticPath);
      }
      return;
    }
    if (ts.isIndexedAccessTypeNode(typeNode)) {
      const index = ts.isLiteralTypeNode(typeNode.indexType) && ts.isStringLiteral(typeNode.indexType.literal)
        ? typeNode.indexType.literal.text
        : undefined;
      if (index) collect(findProperty(typeNode.objectType, index), prefix, output, visiting);
      return;
    }
    if (ts.isTypeReferenceNode(typeNode)) {
      const referenceName = nameOf(typeNode.typeName);
      if (!referenceName || visiting.has(referenceName)) return;
      const nextVisiting = new Set(visiting).add(referenceName);
      if (['GenerateSemantic', 'NonNullable', 'Required', 'Partial', 'Readonly'].includes(referenceName) && typeNode.typeArguments?.length) {
        collect(typeNode.typeArguments[0], prefix, output, nextVisiting);
        return;
      }
      for (const declaration of declarations.get(referenceName) ?? []) {
        collect(unwrapDeclaration(declaration), prefix, output, nextVisiting);
      }
    }
  }

  const result = [];
  for (const { file, ast } of parsed) {
    const fileStart = result.length;
    for (const statement of ast.statements) {
      if (!ts.isTypeAliasDeclaration(statement) || !statement.name.text.endsWith('SemanticType')) continue;
      const semanticType = statement.name.text;
      const body = unwrapDeclaration(statement);
      const classNames = new Set();
      const styles = new Set();
      collect(findProperty(body, 'classNames'), '', classNames, new Set([semanticType]));
      collect(findProperty(body, 'styles'), '', styles, new Set([semanticType]));
      result.push({ file, semanticType, classNames: [...classNames].sort(), styles: [...styles].sort() });
    }

    // Some components expose their semantic records through Props but do not
    // declare a `*SemanticType` alias (Menu is the current example). Preserve
    // those public API slots instead of silently dropping them from the API
    // inventory. Prefer the explicit SemanticType declarations whenever one
    // exists in the same module.
    if (result.length === fileStart) {
      const componentName = file.split(/[\\/]/).at(-2)
        .split('-')
        .filter(Boolean)
        .map((part) => part[0].toUpperCase() + part.slice(1))
        .join('');
      const componentPropsName = `${componentName}Props`;
      for (const statement of ast.statements) {
        if (!statement.modifiers?.some((modifier) => modifier.kind === ts.SyntaxKind.ExportKeyword)) continue;
        if ((!ts.isInterfaceDeclaration(statement) && !ts.isTypeAliasDeclaration(statement))
          || statement.name.text !== componentPropsName) continue;
        const classNamesType = findProperty(unwrapDeclaration(statement), 'classNames');
        const stylesType = findProperty(unwrapDeclaration(statement), 'styles');
        if (!classNamesType && !stylesType) continue;
        const classNames = new Set();
        const styles = new Set();
        collect(classNamesType, '', classNames);
        collect(stylesType, '', styles);
        if (classNames.size || styles.size) {
          result.push({
            file,
            semanticType: `${statement.name.text}SemanticSchema`,
            classNames: [...classNames].sort(),
            styles: [...styles].sort(),
          });
        }
      }
    }
  }
  return result;
}
