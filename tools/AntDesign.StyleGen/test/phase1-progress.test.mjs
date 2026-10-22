import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { mkdtempSync, readFileSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join, resolve } from 'node:path';
import test from 'node:test';
import { fileURLToPath } from 'node:url';

const repoRoot = resolve(fileURLToPath(new URL('../../../', import.meta.url)));
const progressScript = join(repoRoot, 'tools/AntDesign.StyleGen/src/phase1-progress.mjs');

test('phase-one progress audits every React component token row and both runtime locales', () => {
  const temporaryDirectory = mkdtempSync(join(tmpdir(), 'antd-v6-progress-'));
  try {
    const outputPath = join(temporaryDirectory, 'progress.json');
    const result = spawnSync(process.execPath, [progressScript, repoRoot, outputPath], { encoding: 'utf8' });
    assert.equal(result.status, 0, result.stderr || result.stdout);

    const report = JSON.parse(readFileSync(outputPath, 'utf8'));
    assert.deepEqual(report.documentation.componentTokenPages, { total: 65, migrated: 65, pending: 0 });
    assert.deepEqual(report.documentation.componentTokenRows, { total: 584, migrated: 584, pending: 0 });
    assert.deepEqual(report.documentation.componentGlobalTokenPages, { total: 64, migrated: 64, pending: 0 });
    assert.deepEqual(report.documentation.componentGlobalTokenRows, { total: 1464, migrated: 1464, pending: 0 });
    for (const [name, metric] of Object.entries(report.componentTokenDocumentation.tokenMetadataParity)) {
      assert.equal(metric.count, 0, `${name}: ${metric.samples.join(', ')}`);
    }
    assert.deepEqual(report.componentTokenDocumentation.missingEnglish, []);
    assert.deepEqual(report.componentTokenDocumentation.missingChinese, []);
  } finally {
    rmSync(temporaryDirectory, { recursive: true, force: true });
  }
});
