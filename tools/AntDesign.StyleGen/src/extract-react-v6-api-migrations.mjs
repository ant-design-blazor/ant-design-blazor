#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';

export function extractReactV6ApiMigrations(markdown, source = 'migration-v6.en-US.md') {
  const migrations = [];
  let component = null;
  let subsection = null;

  for (const line of markdown.split(/\r?\n/)) {
    const heading = line.match(/^[-*]\s+`([^`]+)`\s*$/);
    if (heading) {
      component = heading[1];
      subsection = null;
      continue;
    }

    const item = line.match(/^\s{2,}[-*]\s+`([^`]+)`\s+is deprecated(?:\s+and replaced by\s+`([^`]+)`)?\.?(?:\s+\(([^)]+)\))?/);
    if (!item || !component) continue;

    const legacy = item[1];
    const replacement = item[2] ?? null;
    const since = item[3] ?? '6.0.0';
    const itemComponent = component.includes('.') ? component : component;
    migrations.push({
      component: itemComponent,
      legacy,
      replacement,
      kind: replacement ? 'renamed-or-replaced' : 'deprecated-without-direct-replacement',
      since,
      source,
    });
  }

  const unique = migrations.filter((item, index, all) => all.findIndex(candidate =>
    candidate.component === item.component && candidate.legacy === item.legacy && candidate.replacement === item.replacement) === index);
  return {
    schemaVersion: 1,
    source,
    summary: {
      migrations: unique.length,
      components: new Set(unique.map(item => item.component)).size,
      directReplacements: unique.filter(item => item.replacement).length,
    },
    migrations: unique,
  };
}

if (process.argv[1] && path.resolve(process.argv[1]) === path.resolve(new URL(import.meta.url).pathname)) {
  const input = process.argv[2] ?? '/Volumes/Data/GitHub/ant-design/docs/react/migration-v6.en-US.md';
  const output = process.argv[3] ?? path.join(process.cwd(), 'docs/antd-v6/react-v6-api-migrations.json');
  const result = extractReactV6ApiMigrations(fs.readFileSync(input, 'utf8'), input);
  fs.mkdirSync(path.dirname(output), { recursive: true });
  fs.writeFileSync(output, `${JSON.stringify(result, null, 2)}\n`);
  console.log(JSON.stringify({ output, ...result.summary }, null, 2));
}
