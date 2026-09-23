#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';

const repoRoot = process.argv[2] ?? process.cwd();
const output = process.argv[3] ?? path.join(repoRoot, 'docs/antd-v6/phase1-progress.json');
const readJson = relative => JSON.parse(fs.readFileSync(path.join(repoRoot, relative), 'utf8'));
const countFiles = (relative, pattern) => fs.readdirSync(path.join(repoRoot, relative)).filter(name => pattern.test(name)).length;

const inventory = readJson('docs/antd-v6/semantic-dom-inventory.json');
const styleIr = readJson('.vibedev/generated/antd-v6/ir/style-ir.json');
const semanticCatalog = fs.readFileSync(path.join(repoRoot, 'site/AntDesign.Docs/Services/SemanticDomGeneratedCatalog.cs'), 'utf8');
const catalogComponents = new Set([...semanticCatalog.matchAll(/^                \["([^"]+)"\]/gm)].map(match => match[1]));
const semanticEntries = inventory.entries ?? [];
const structureMigrated = semanticEntries.filter(entry => entry.migrationStatus === 'structure-migrated');
const allDumiSemanticPaths = new Set(semanticEntries.flatMap(entry => (entry.dumiSemanticSources ?? []).flatMap(source => source.semanticPaths ?? [])));
const migratedDumiSemanticPaths = new Set(structureMigrated.flatMap(entry => (entry.classNameMapping ?? [])
  .filter(mapping => mapping.status === 'structure-migrated-pending-semantic-injection')
  .map(mapping => mapping.semanticPath)
  .filter(semanticPath => allDumiSemanticPaths.has(semanticPath))));
const semanticPathsMigrated = migratedDumiSemanticPaths.size;
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
  },
  semanticDom: {
    catalogComponents: metric(new Set(semanticEntries.map(entry => entry.component)).size, [...new Set(semanticEntries.map(entry => entry.component))].filter(component => catalogComponents.has(component)).length),
    definitions: metric(inventory.scope.semanticDefinitions, structureMigrated.length),
    paths: metric(inventory.scope.dumiSemanticPaths, semanticPathsMigrated),
  },
  documentation: {
    semanticPreview: metric(1, fs.existsSync(path.join(repoRoot, 'site/AntDesign.Docs/Components/SemanticDomPreview.razor')) ? 1 : 0),
    themePreview: metric(1, fs.existsSync(path.join(repoRoot, 'site/AntDesign.Docs/Pages/ThemeMigrationPreview.razor')) ? 1 : 0),
  },
};

fs.mkdirSync(path.dirname(output), { recursive: true });
fs.writeFileSync(output, `${JSON.stringify(report, null, 2)}\n`);
console.log(JSON.stringify({ output, ...report }, null, 2));
