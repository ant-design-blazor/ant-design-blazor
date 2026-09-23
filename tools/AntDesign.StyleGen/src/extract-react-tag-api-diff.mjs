#!/usr/bin/env node

import { execFileSync } from 'node:child_process';
import fs from 'node:fs';
import path from 'node:path';
import os from 'node:os';
import { extractReactApi } from './extract-react-api-to-json.mjs';

export function compareReactApis(oldApi, newApi, oldTag, newTag) {
  const collect = api => {
    const props = new Map();
    for (const declaration of api.props) {
      const row = props.get(declaration.component) ?? new Set();
      for (const property of declaration.properties) row.add(property.name);
      props.set(declaration.component, row);
    }
    return props;
  };
  const oldProps = collect(oldApi);
  const newProps = collect(newApi);
  const names = new Set([...oldProps.keys(), ...newProps.keys()]);
  const rows = [...names].map(component => {
    const oldSet = oldProps.get(component) ?? new Set();
    const newSet = newProps.get(component) ?? new Set();
    return {
      component,
      removed: [...oldSet].filter(name => !newSet.has(name)).sort(),
      added: [...newSet].filter(name => !oldSet.has(name)).sort(),
    };
  }).filter(row => row.removed.length || row.added.length).sort((a, b) => a.component.localeCompare(b.component));
  return {
    schemaVersion: 1,
    source: { oldTag, newTag, method: 'git-tag-api-ir-diff', scope: 'React Props declarations' },
    summary: {
      components: rows.length,
      removedProperties: rows.reduce((n, row) => n + row.removed.length, 0),
      addedProperties: rows.reduce((n, row) => n + row.added.length, 0),
    },
    components: rows,
  };
}

if (process.argv[1] && path.resolve(process.argv[1]) === path.resolve(new URL(import.meta.url).pathname)) {
  const repo = process.argv[2] ?? '/Volumes/Data/GitHub/ant-design';
  const oldTag = process.argv[3] ?? '4.24.16';
  const newTag = process.argv[4] ?? '6.6.4';
  const output = process.argv[5] ?? path.join(process.cwd(), 'docs/antd-v6/react-v4-v6-api-diff.json');
  const tempRoot = fs.mkdtempSync(path.join(process.env.TMPDIR ?? os.tmpdir(), 'react-api-tag-diff-'));
  const extractTag = tag => {
    const archive = execFileSync('git', ['-C', repo, 'archive', tag, '--', 'components'], { maxBuffer: 256 * 1024 * 1024 });
    const archivePath = path.join(tempRoot, `${tag}.tar`);
    const tagRoot = path.join(tempRoot, tag);
    fs.mkdirSync(tagRoot, { recursive: true });
    fs.writeFileSync(archivePath, archive);
    execFileSync('tar', ['-xf', archivePath, '-C', tagRoot]);
    return extractReactApi(tagRoot);
  };
  const result = compareReactApis(extractTag(oldTag), extractTag(newTag), oldTag, newTag);
  fs.rmSync(tempRoot, { recursive: true, force: true });
  fs.mkdirSync(path.dirname(output), { recursive: true });
  fs.writeFileSync(output, `${JSON.stringify(result, null, 2)}\n`);
  console.log(JSON.stringify({ output, ...result.summary }, null, 2));
}
