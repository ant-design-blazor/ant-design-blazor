#!/usr/bin/env node

import path from 'node:path';
import { spawnSync } from 'node:child_process';

const repoRoot = path.resolve(process.argv[2] || process.cwd());
const antdRoot = path.resolve(process.argv[3] || path.join(repoRoot, '../ant-design'));
const output = path.resolve(process.argv[4] || path.join(repoRoot, 'docs/antd-v6/semantic-dom-catalog.json'));
const inventory = path.join(repoRoot, 'docs/antd-v6/semantic-dom-inventory.json');
const scripts = path.join(repoRoot, 'tools/AntDesign.StyleGen/src');

function run(script, args) {
  const result = spawnSync(process.execPath, [path.join(scripts, script), ...args], {
    cwd: repoRoot,
    encoding: 'utf8',
    stdio: 'inherit',
  });
  if (result.status !== 0) throw new Error(`${script} failed with exit code ${result.status}`);
}

try {
  run('semantic-inventory.mjs', [repoRoot, path.join(antdRoot, 'components'), inventory]);
  run('generate-semantic-dom-catalog.mjs', [repoRoot, inventory, output, output]);
} catch (error) {
  console.error(error.message);
  process.exitCode = 1;
}
