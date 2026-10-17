import assert from 'node:assert/strict';
import { existsSync } from 'node:fs';
import { mkdtemp, readFile, rm } from 'node:fs/promises';
import { join, resolve } from 'node:path';
import { spawnSync } from 'node:child_process';
import { tmpdir } from 'node:os';
import test from 'node:test';
import { compareArtifactSets, compareCss } from '../src/style-diff.mjs';
import { createArtifact, readBaseline } from '../src/baseline.mjs';
import { normalizeBlazorThemeScope } from '../src/react-css-baseline.mjs';

const repo = resolve(new URL('../../..', import.meta.url).pathname);
const antdPath = process.env.ANTD_PATH ?? resolve(repo, '../ant-design');
const tsxPath = resolve(antdPath, 'node_modules/.bin/tsx');
const cliPath = resolve(repo, 'tools/AntDesign.StyleGen/src/cli.mjs');

test('normalizes React renderer scopes to the stable Blazor theme ancestor', () => {
  const reactCss = '.css-var-root.ant-btn{color:red}.css-var-root{--ant-color-primary:blue}.css-var-_R_39n_.ant-css-var{--ant-x:y}';
  assert.equal(
    normalizeBlazorThemeScope(reactCss),
    '.ant-theme .ant-btn{color:red}.ant-theme{--ant-color-primary:blue}.ant-theme .ant-css-var{--ant-x:y}',
  );
  assert.doesNotMatch(normalizeBlazorThemeScope(reactCss), /css-var-(?:root|_R_)/);
});

test('generates the phase-one React CSS catalog for all components and themes', { skip: !existsSync(join(antdPath, 'package.json')) || !existsSync(tsxPath) }, async () => {
  const output = await mkdtemp(join(tmpdir(), 'antd-v6-css-baseline-'));
  const docsThemeOutput = join(output, 'docs-css');
  try {
    const result = spawnSync(tsxPath, [cliPath, 'css-baseline-update', '--antd-path', antdPath, '--output', output, '--docs-theme-output', docsThemeOutput], {
      cwd: repo,
      encoding: 'utf8',
      maxBuffer: 32 * 1024 * 1024,
    });
    assert.equal(result.status, 0, result.stderr || result.stdout);
    const components = JSON.parse(await readFile(join(output, 'components.json'), 'utf8'));
    const themes = JSON.parse(await readFile(join(output, 'themes.json'), 'utf8'));
    const docsThemeModes = JSON.parse(await readFile(join(output, 'docs-theme-modes.json'), 'utf8'));
    const manifest = JSON.parse(await readFile(join(output, 'manifest.json'), 'utf8'));
    assert.equal(components.total, 72);
    assert.equal(components.generated, 72);
    assert.deepEqual(components.unsupported, []);
    assert.equal(themes.length, 13);
    assert.deepEqual(docsThemeModes.map((theme) => theme.key), ['light-compact', 'dark-compact']);
    assert.equal(manifest.upstream.version, '6.6.4');
    assert.ok(manifest.artifacts.length >= 72 * 13);
    assert.ok(existsSync(join(output, 'themes', 'light.css')));
    assert.ok(existsSync(join(output, 'themes', 'serene.css')));
    assert.ok(existsSync(join(output, 'themes', 'light-compact.css')));
    assert.ok(existsSync(join(output, 'themes', 'dark-compact.css')));
    assert.equal(await readFile(join(docsThemeOutput, 'light-compact.css'), 'utf8'), await readFile(join(output, 'themes', 'light-compact.css'), 'utf8'));
    assert.equal(await readFile(join(docsThemeOutput, 'dark-compact.css'), 'utf8'), await readFile(join(output, 'themes', 'dark-compact.css'), 'utf8'));
    assert.ok(existsSync(join(docsThemeOutput, 'docs-theme-modes.manifest.json')));

    const expected = await readBaseline(output);
    const same = compareArtifactSets({
      expectedManifest: expected.manifest,
      expectedCssById: expected.cssById,
      actualManifest: expected.manifest,
      actualCssById: expected.cssById,
    });
    assert.equal(same.equal, true, JSON.stringify(same.diagnostics));

    const missingManifest = { ...expected.manifest, artifacts: expected.manifest.artifacts.slice(1) };
    const missing = compareArtifactSets({
      expectedManifest: expected.manifest,
      expectedCssById: expected.cssById,
      actualManifest: missingManifest,
      actualCssById: expected.cssById,
    });
    assert.equal(missing.equal, false);
    assert.equal(missing.diagnostics[0].type, 'missing');

    const extraCss = '.ant-extra {\n  color: red;\n}\n';
    const extraArtifact = createArtifact({ id: 'extra.light.static', path: 'static/extra.light.css', layer: 'static', component: 'extra', theme: 'light', css: extraCss });
    const extraManifest = { ...expected.manifest, artifacts: [...expected.manifest.artifacts, extraArtifact] };
    const extraCssById = new Map(expected.cssById);
    extraCssById.set(extraArtifact.id, extraCss);
    const extra = compareArtifactSets({
      expectedManifest: expected.manifest,
      expectedCssById: expected.cssById,
      actualManifest: extraManifest,
      actualCssById: extraCssById,
    });
    assert.equal(extra.equal, false);
    assert.equal(extra.diagnostics.at(-1).type, 'added');

    const mutationId = expected.manifest.artifacts.find((artifact) => artifact.component === 'button' && artifact.theme === 'light').id;
    const mutatedCssById = new Map(expected.cssById);
    mutatedCssById.set(mutationId, mutatedCssById.get(mutationId).replace('ant-btn', 'ant-btX'));
    const mutated = compareArtifactSets({
      expectedManifest: expected.manifest,
      expectedCssById: expected.cssById,
      actualManifest: expected.manifest,
      actualCssById: mutatedCssById,
    });
    assert.equal(mutated.equal, false);
    assert.equal(mutated.diagnostics[0].id, mutationId);

    const sampleCss = expected.cssById.get(mutationId);
    assert.equal(compareCss(sampleCss, sampleCss.replaceAll('\n', '\r\n')).equal, false);
    assert.equal(compareCss(sampleCss, sampleCss.replace('color:', 'background:')).equal, false);
  } finally {
    await rm(output, { recursive: true, force: true });
  }
});
