import test from 'node:test';
import assert from 'node:assert/strict';
import { mkdtemp, readFile } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { spawnSync } from 'node:child_process';
import { createArtifact, createBaseline, writeBaseline } from '../src/baseline.mjs';

const cli = join(process.cwd(), 'src', 'cli.mjs');
const css = '.ant-btn {\n  color: var(--ant-color-text);\n}\n';

async function createFixture(root, value = css) {
  const artifact = createArtifact({ id: 'button.light.static', path: 'static/button.css', layer: 'static', component: 'Button', theme: 'light', css: value });
  await writeBaseline(root, createBaseline({ upstream: { revision: 'mvp' }, generator: { version: '1' }, inputDigest: 'input', artifacts: [artifact] }), new Map([[artifact.id, value]]));
}

test('exact-check succeeds and writes a machine-readable report', async () => {
  const root = await mkdtemp(join(tmpdir(), 'antd-style-diff-'));
  const expected = join(root, 'expected');
  const actual = join(root, 'actual');
  const report = join(root, 'reports', 'diff.json');
  await createFixture(expected);
  await createFixture(actual);
  const result = spawnSync(process.execPath, [cli, 'exact-check', '--expected', expected, '--actual', actual, '--report', report], { encoding: 'utf8' });
  assert.equal(result.status, 0, result.stderr);
  assert.equal(JSON.parse(await readFile(report, 'utf8')).equal, true);
});

test('exact-check returns non-zero for a byte mutation', async () => {
  const root = await mkdtemp(join(tmpdir(), 'antd-style-diff-'));
  const expected = join(root, 'expected');
  const actual = join(root, 'actual');
  await createFixture(expected);
  await createFixture(actual, css.replace('text', 'texT'));
  const result = spawnSync(process.execPath, [cli, 'exact-check', '--expected', expected, '--actual', actual], { encoding: 'utf8' });
  assert.equal(result.status, 1);
  const output = JSON.parse(result.stdout);
  assert.equal(output.equal, false);
  assert.equal(output.diagnostics[0].firstDifference.byteOffset, 39);
});
