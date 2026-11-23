#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { getSemanticCatalogCoverage } from './semantic-catalog-coverage.mjs';

const repoRoot = process.argv[2] ?? process.cwd();
const output = process.argv[3] ?? path.join(repoRoot, 'docs/antd-v6/phase1-progress.json');
const readJson = relative => JSON.parse(fs.readFileSync(path.join(repoRoot, relative), 'utf8'));
const countFiles = (relative, pattern) => fs.readdirSync(path.join(repoRoot, relative)).filter(name => pattern.test(name)).length;

const inventory = readJson('docs/antd-v6/semantic-dom-inventory.json');
const styleIr = readJson('.vibedev/generated/antd-v6/ir/style-ir.json');
const semanticCatalog = readJson('docs/antd-v6/semantic-dom-catalog.json');
const semanticMdCatalog = readJson('docs/antd-v6/semantic-md-catalog.json');
const algorithmSource = fs.readFileSync(path.join(repoRoot, 'components/theming/TokenCompiler.cs'), 'utf8');
const algorithmNames = ['DefaultThemeAlgorithm', 'DarkThemeAlgorithm', 'CompactThemeAlgorithm'];
const implementedAlgorithms = algorithmNames.filter(name => new RegExp(`public sealed class ${name}\\b`).test(algorithmSource));
const semanticEntries = inventory.entries ?? [];
const structureMigrated = semanticEntries.filter(entry => entry.migrationStatus === 'structure-migrated');
const semanticCoverage = getSemanticCatalogCoverage(inventory, semanticCatalog, semanticMdCatalog);
const metric = (total, migrated) => ({ total, migrated, pending: Math.max(total - migrated, 0) });

const reactComponentsRoot = path.resolve(repoRoot, inventory.source?.path ?? '../ant-design/components');
const componentsPage = fs.readFileSync(path.join(repoRoot, 'site/AntDesign.Docs/Pages/Components.razor'), 'utf8');
const componentTokenRenderer = fs.readFileSync(path.join(repoRoot, 'site/AntDesign.Docs/Components/ComponentTokenDocumentation.razor'), 'utf8');
const generatedComponentTokens = fs.readFileSync(path.join(repoRoot, 'components/theming/GeneratedComponentTokens.cs'), 'utf8');
const generatedThemeTokens = fs.readFileSync(path.join(repoRoot, 'components/theming/GeneratedThemeTokens.cs'), 'utf8');
const reactTokenMetadata = JSON.parse(fs.readFileSync(path.join(reactComponentsRoot, 'version/token-meta.json'), 'utf8'));
const reactTokenValues = JSON.parse(fs.readFileSync(path.join(reactComponentsRoot, 'version/token.json'), 'utf8'));
const componentTokenIr = styleIr.tokens.filter(token => token.kind === 'component');
const normalizeTokenOwner = value => String(value).replace(/[^a-z0-9]/gi, '').toLowerCase();
const tokenKey = (component, name) => `${normalizeTokenOwner(component)}:${name}`;
const componentTokenIrByKey = new Map(componentTokenIr.map(token => [tokenKey(token.component, token.name), token]));
const generatedTokenDocumentation = new Map(generatedComponentTokens.split('\n').flatMap(line => {
  const match = line.match(/new ComponentTokenDefinition\("([^"]+)", "([^"]+)".*?, (true|false), (true|false), (true|false)\),?$/);
  return match ? [[tokenKey(match[1], match[2]), { deprecated: match[3] === 'true', documented: match[4] === 'true', reserved: match[5] === 'true' }]] : [];
}));
const componentTokenSectionIsWired = /<ComponentTokenDocumentation\s+Component="@SemanticComponentKey"\s+Language="@CurrentLanguage"\s*\/>/.test(componentsPage)
  && /ComponentTokenCatalog\.Get\(Component\)/.test(componentTokenRenderer)
  && /Language\s*\{\s*get;\s*set;\s*\}/.test(componentTokenRenderer);
const componentTokenDocs = (inventory.componentCatalog ?? []).flatMap(component => {
  const reactDoc = path.join(reactComponentsRoot, component.name, 'index.en-US.md');
  if (!fs.existsSync(reactDoc)) return [];
  const reactMarkdown = fs.readFileSync(reactDoc, 'utf8');
  if (!/^##\s+Design Token\b/m.test(reactMarkdown)
    || !/<ComponentTokenTable\b/.test(reactMarkdown)) return [];

  const hasGeneratedRows = new RegExp(`new ComponentTokenDefinition\\("${component.name}",[^\\n]*, false, true, false\\)`).test(generatedComponentTokens);

  return [{
    component: component.name,
    hasEnglish: componentTokenSectionIsWired,
    hasChinese: componentTokenSectionIsWired,
    hasGeneratedRows,
  }];
});
const tokenDocsComplete = componentTokenDocs.filter(component => component.hasEnglish && component.hasChinese).length;
const componentTokenDocsMissingRows = componentTokenDocs.filter(component => !component.hasGeneratedRows).map(component => component.component);
const documentedComponentNames = new Set(componentTokenDocs.map(component => normalizeTokenOwner(component.component)));
const expectedDocumentedTokens = Object.entries(reactTokenMetadata.components ?? {}).flatMap(([component, tokens]) => documentedComponentNames.has(normalizeTokenOwner(component))
  ? tokens.map(token => ({ component, token }))
  : []);
const tokenMetadataParity = { missingIrTokens: [], missingDocumentedRows: [], unexpectedDocumentedRows: [], mismatchedDescriptions: [], mismatchedEnglishDescriptions: [], mismatchedDefaults: [], mismatchedCssVariables: [] };
const globalSectionIsWired = /ComponentTokenCatalog\.GetGlobal\(Component\)/.test(componentTokenRenderer)
  && /global-token-table/.test(componentTokenRenderer);
const globalUsageRows = new Set([...generatedThemeTokens.matchAll(/\["([^"\n]+)"\] = new string\[\] \{ ([^\n]*) \}/g)]
  .flatMap(([, component, names]) => [...names.matchAll(/"([^"\n]+)"/g)].map(([, name]) => tokenKey(component, name))));
const expectedGlobalUsage = Object.entries(reactTokenValues).flatMap(([component, values]) =>
  documentedComponentNames.has(normalizeTokenOwner(component))
    ? (values.global ?? []).filter(name => Object.hasOwn(reactTokenMetadata.global ?? {}, name)).map(name => ({ component, name }))
    : []);
const expectedGlobalUsageKeys = new Set(expectedGlobalUsage.map(({ component, name }) => tokenKey(component, name)));
tokenMetadataParity.missingGlobalUsageRows = expectedGlobalUsage.filter(({ component, name }) => !globalSectionIsWired || !globalUsageRows.has(tokenKey(component, name)))
  .map(({ component, name }) => `${component}.${name}`);
tokenMetadataParity.unexpectedGlobalUsageRows = [...globalUsageRows].filter(key => documentedComponentNames.has(key.split(':')[0]) && !expectedGlobalUsageKeys.has(key));
const globalUsageComponents = [...new Set(expectedGlobalUsage.map(({ component }) => normalizeTokenOwner(component)))];
for (const { component, token } of expectedDocumentedTokens) {
  const key = tokenKey(component, token.token);
  const irToken = componentTokenIrByKey.get(key);
  const documentation = generatedTokenDocumentation.get(key);
  const upstreamValue = reactTokenValues[token.source]?.component?.[token.token];
  const expectedDefault = upstreamValue == null ? undefined : typeof upstreamValue === 'object' ? JSON.stringify(upstreamValue) : String(upstreamValue);
  const toCssKebab = value => value.replace(/([a-z0-9])([A-Z])/g, '$1-$2').replace(/[^A-Za-z0-9]+/g, '-').replace(/^-|-$/g, '').toLowerCase();
  const expectedCssVariable = `--ant-${toCssKebab(irToken?.component ?? component)}-${toCssKebab(token.token)}`;
  if (!irToken) tokenMetadataParity.missingIrTokens.push(`${component}.${token.token}`);
  else {
    if ((irToken.description ?? '') !== (token.desc ?? '')) tokenMetadataParity.mismatchedDescriptions.push(`${component}.${token.token}`);
    if ((irToken.descriptionEn ?? '') !== (token.descEn ?? '')) tokenMetadataParity.mismatchedEnglishDescriptions.push(`${component}.${token.token}`);
    if (irToken.defaultValue !== expectedDefault) tokenMetadataParity.mismatchedDefaults.push(`${component}.${token.token}`);
    if (irToken.cssVariable !== expectedCssVariable) tokenMetadataParity.mismatchedCssVariables.push(`${component}.${token.token}`);
  }
  if (!documentation?.documented) tokenMetadataParity.missingDocumentedRows.push(`${component}.${token.token}`);
}
for (const [key, definition] of generatedTokenDocumentation) {
  if (definition.documented && !expectedDocumentedTokens.some(({ component, token }) => tokenKey(component, token.token) === key)) {
    tokenMetadataParity.unexpectedDocumentedRows.push(key);
  }
}

const report = {
  schemaVersion: 1,
  phase: 1,
  verificationScope: 'Generated API, token, documentation, and semantic catalog coverage. Runtime DOM/CSS/visual parity requires the component-specific browser reports recorded in dom-style-parity.md.',
  source: {
    semanticInventory: 'docs/antd-v6/semantic-dom-inventory.json',
    styleIr: '.vibedev/generated/antd-v6/ir/style-ir.json',
  },
  css: {
    components: metric(inventory.scope.cssComponents, Array.isArray(styleIr.styles) ? styleIr.styles.length : 0),
    themes: metric(13, countFiles('components/wwwroot/css/antd-v6', /^.+\.css$/)),
    algorithms: metric(3, implementedAlgorithms.length),
  },
  semanticDom: {
    catalogComponents: metric(semanticCoverage.catalogComponents.total, semanticCoverage.catalogComponents.migrated),
    definitions: metric(semanticCoverage.definitions.total, semanticCoverage.definitions.migrated),
    apiSlots: metric(semanticCoverage.apiSlots.total, semanticCoverage.apiSlots.migrated),
    dumiPaths: metric(semanticCoverage.dumiPaths.total, semanticCoverage.dumiPaths.migrated),
    documentedComponents: metric(semanticCoverage.documentedComponents.total, semanticCoverage.documentedComponents.migrated),
    documentedSlots: metric(semanticCoverage.documentedSlots.total, semanticCoverage.documentedSlots.migrated),
    structureMigration: metric(inventory.scope.semanticDefinitions, structureMigrated.length),
  },
  documentation: {
    semanticPreview: metric(1, fs.existsSync(path.join(repoRoot, 'site/AntDesign.Docs/Components/SemanticDomPreview.razor')) ? 1 : 0),
    themePreview: metric(1, fs.existsSync(path.join(repoRoot, 'site/AntDesign.Docs/Pages/ThemeMigrationPreview.razor')) ? 1 : 0),
    componentTokenPages: metric(componentTokenDocs.length, tokenDocsComplete),
    componentTokenRows: metric(expectedDocumentedTokens.length, expectedDocumentedTokens.length - tokenMetadataParity.missingDocumentedRows.length),
    componentGlobalTokenPages: metric(globalUsageComponents.length, globalSectionIsWired ? globalUsageComponents.filter(component =>
      expectedGlobalUsage.filter(value => normalizeTokenOwner(value.component) === component).every(({ name }) => globalUsageRows.has(tokenKey(component, name)))).length : 0),
    componentGlobalTokenRows: metric(expectedGlobalUsage.length, expectedGlobalUsage.length - tokenMetadataParity.missingGlobalUsageRows.length),
  },
  componentTokenDocumentation: {
    totalReactComponentsWithTokenTable: componentTokenDocs.length,
    completeBlazorPages: tokenDocsComplete,
    delivery: 'shared component route renders the generated token table in both locales',
    missingEnglish: componentTokenDocs.filter(component => !component.hasEnglish).map(component => component.component),
    missingChinese: componentTokenDocs.filter(component => !component.hasChinese).map(component => component.component),
    componentsWithoutDocumentedTokenRows: componentTokenDocsMissingRows,
    tokenMetadataParity: Object.fromEntries(Object.entries(tokenMetadataParity).map(([name, values]) => [name, { count: values.length, samples: values.slice(0, 20) }])),
  },
};

fs.mkdirSync(path.dirname(output), { recursive: true });
fs.writeFileSync(output, `${JSON.stringify(report, null, 2)}\n`);
console.log(JSON.stringify({ output, ...report }, null, 2));
