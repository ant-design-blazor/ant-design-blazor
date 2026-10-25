import ts from 'typescript';

const propertyName = node => node.name?.text;
const properties = node => new Map(node?.properties?.filter(ts.isPropertyAssignment).map(p => [propertyName(p), p.initializer]) ?? []);

function literal(node) {
  if (!node) return undefined;
  if (ts.isStringLiteral(node) || ts.isNoSubstitutionTemplateLiteral(node)) return node.text;
  if (ts.isNumericLiteral(node)) return Number(node.text);
  if (node.kind === ts.SyntaxKind.TrueKeyword) return true;
  if (node.kind === ts.SyntaxKind.FalseKeyword) return false;
  if (ts.isPrefixUnaryExpression(node) && node.operator === ts.SyntaxKind.MinusToken) return -literal(node.operand);
  if (ts.isObjectLiteralExpression(node)) return Object.fromEntries([...properties(node)].map(([key, value]) => [key, literal(value)]));
  throw new Error(`Unsupported preview metadata literal: ${node.getText()}`);
}

/** Read catalog metadata without executing React hooks, locale code, or styles. */
export function extractPreviewThemeMetadata(source) {
  const file = ts.createSourceFile('previewThemes.ts', source, ts.ScriptTarget.Latest, true);
  const themes = [];
  let english = {};
  const visit = node => {
    if (ts.isVariableDeclaration(node) && node.name.getText(file) === 'locales') english = literal(properties(node.initializer).get('en'));
    if (ts.isObjectLiteralExpression(node)) {
      const p = properties(node);
      if (p.has('icon') && p.has('name') && p.has('props')) {
        const name = p.get('name');
        const localeKey = ts.isPropertyAccessExpression(name) ? name.name.text : literal(name);
        const key = p.has('key') ? literal(p.get('key')) : localeKey;
        const icon = p.get('icon');
        themes.push({ key, localeKey, icon: ts.isIdentifier(icon) ? icon.text : literal(icon),
          background: literal(p.get('bgImg')) ?? '', darkBackground: literal(p.get('bgImgDark')) ?? false });
      }
    }
    ts.forEachChild(node, visit);
  };
  visit(file);
  return themes.map(theme => ({ ...theme, name: english[theme.localeKey] ?? theme.localeKey }));
}

export function extractPreviewAlgorithm(source) {
  const file = ts.createSourceFile('previewTheme.ts', source, ts.ScriptTarget.Latest, true);
  let algorithm = 'default';
  const visit = node => {
    if (ts.isPropertyAssignment(node) && propertyName(node) === 'theme' && ts.isObjectLiteralExpression(node.initializer)) {
      const value = properties(node.initializer).get('algorithm');
      if (value && ts.isPropertyAccessExpression(value)) algorithm = value.name.text.replace(/Algorithm$/, '');
    }
    ts.forEachChild(node, visit);
  };
  visit(file);
  return algorithm;
}

/** @ant-design/compatible's emitted module contains plain v4Token/components
 * literals. Scan them, not its require('antd') runtime or ObjectSpread helpers. */
export function extractCompatibleV4Defaults(source) {
  const file = ts.createSourceFile('compatible-theme.js', source, ts.ScriptTarget.Latest, true);
  let token;
  let components;
  const visit = node => {
    if (ts.isVariableDeclaration(node) && node.name.getText(file) === 'v4Token') token = literal(node.initializer).token;
    if (ts.isVariableDeclaration(node) && node.name.getText(file) === 'defaultTheme') {
      const find = child => {
        if (ts.isPropertyAssignment(child) && propertyName(child) === 'components') components = literal(child.initializer);
        ts.forEachChild(child, find);
      };
      find(node.initializer);
    }
    ts.forEachChild(node, visit);
  };
  visit(file);
  if (!token || !components) throw new Error('Could not find the compatible v4 theme literals');
  const stringify = object => Object.fromEntries(Object.entries(object).map(([key, value]) => [key, typeof value === 'object' ? stringify(value) : String(value)]));
  return { token: stringify(token), components: stringify(components) };
}
