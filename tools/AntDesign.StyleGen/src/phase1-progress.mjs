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
const algorithmSource = fs.readFileSync(path.join(repoRoot, 'components/theming/TokenCompiler.cs'), 'utf8');
const algorithmNames = ['DefaultThemeAlgorithm', 'DarkThemeAlgorithm', 'CompactThemeAlgorithm'];
const implementedAlgorithms = algorithmNames.filter(name => new RegExp(`public sealed class ${name}\\b`).test(algorithmSource));
const semanticEntries = inventory.entries ?? [];
const structureMigrated = semanticEntries.filter(entry => entry.migrationStatus === 'structure-migrated');
const semanticCoverage = getSemanticCatalogCoverage(inventory, semanticCatalog);
const metric = (total, migrated) => ({ total, migrated, pending: Math.max(total - migrated, 0) });

const report = {
  schemaVersion: 1,
  phase: 1,
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
    structureMigration: metric(inventory.scope.semanticDefinitions, structureMigrated.length),
  },
  documentation: {
    semanticPreview: metric(1, fs.existsSync(path.join(repoRoot, 'site/AntDesign.Docs/Components/SemanticDomPreview.razor')) ? 1 : 0),
    themePreview: metric(1, fs.existsSync(path.join(repoRoot, 'site/AntDesign.Docs/Pages/ThemeMigrationPreview.razor')) ? 1 : 0),
  },
};

fs.mkdirSync(path.dirname(output), { recursive: true });
fs.writeFileSync(output, `${JSON.stringify(report, null, 2)}\n`);
console.log(JSON.stringify({ output, ...report }, null, 2));
