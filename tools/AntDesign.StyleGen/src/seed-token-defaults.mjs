import ts from 'typescript';

/** Read primitive seed defaults and object spreads without executing upstream code. */
export function extractSeedTokenDefaults(source) {
  const file = ts.createSourceFile('seed.ts', source, ts.ScriptTarget.Latest, true);
  const declarations = new Map();
  for (const statement of file.statements) {
    if (!ts.isVariableStatement(statement)) continue;
    for (const declaration of statement.declarationList.declarations) {
      if (ts.isIdentifier(declaration.name)) declarations.set(declaration.name.text, declaration.initializer);
    }
  }
  const evaluate = (node, visiting = new Set()) => {
    if (!node) throw new Error('Missing React seed initializer');
    if (ts.isStringLiteral(node) || ts.isNoSubstitutionTemplateLiteral(node)) return node.text;
    if (ts.isNumericLiteral(node)) return Number(node.text);
    if (node.kind === ts.SyntaxKind.TrueKeyword) return true;
    if (node.kind === ts.SyntaxKind.FalseKeyword) return false;
    if (ts.isIdentifier(node)) {
      if (visiting.has(node.text)) throw new Error(`Circular seed initializer: ${node.text}`);
      return evaluate(declarations.get(node.text), new Set([...visiting, node.text]));
    }
    if (ts.isObjectLiteralExpression(node)) {
      const result = {};
      for (const property of node.properties) {
        if (ts.isSpreadAssignment(property)) Object.assign(result, evaluate(property.expression, visiting));
        else if (ts.isPropertyAssignment(property)) result[property.name.text] = evaluate(property.initializer, visiting);
        else throw new Error(`Unsupported React seed property: ${property.getText(file)}`);
      }
      return result;
    }
    throw new Error(`Unsupported React seed expression: ${node.getText(file)}`);
  };
  return evaluate(declarations.get('seedToken'));
}

export function renderSeedTokenDefaults(source) {
  return [
    '    public static class GeneratedSeedTokenDefaults',
    '    {',
    '        public static IReadOnlyDictionary<string, string> All { get; } =',
    '            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(System.StringComparer.Ordinal)',
    '            {',
    ...Object.entries(extractSeedTokenDefaults(source)).map(([name, value]) => `                [${JSON.stringify(name)}] = ${JSON.stringify(String(value))},`),
    '            });',
    '    }',
    '',
  ].join('\n');
}
