import assert from 'node:assert/strict';
import { mkdtemp, readFile, rm, writeFile } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import test from 'node:test';
import {
  assertCssProtocol,
  createArtifact,
  createBaseline,
  readBaseline,
  sha256,
  writeBaseline,
} from '../src/baseline.mjs';

const css = await readFile(new URL('./fixtures/react-baseline/button.default.css', import.meta.url), 'utf8');

test('writes and reads an exact React CSS baseline', async () => {
  const root = await mkdtemp(join(tmpdir(), 'antd-style-baseline-'));
  try {
    const artifact = createArtifact({
      id: 'Button.default.static',
      path: 'static/Button.default.css',
      layer: 'static',
      component: 'Button',
      theme: 'light',
      css,
    });
    const baseline = createBaseline({
      upstream: { repository: 'ant-design', revision: '599b251da9', version: '6.6.4' },
      generator: { name: 'antd', version: '6.6.4' },
      inputDigest: sha256('fixture-input'),
      artifacts: [artifact],
    });
    await writeBaseline(root, baseline, new Map([[artifact.id, css]]));
    const result = await readBaseline(root);
    assert.equal(result.cssById.get(artifact.id), css);
    assert.deepEqual(result.manifest.artifacts[0], artifact);
  } finally {
    await rm(root, { recursive: true, force: true });
  }
});

test('rejects CSS protocol drift before writing a baseline', () => {
  assert.throws(() => assertCssProtocol('.ant-btn {}\r\n'), /LF/);
  assert.throws(() => assertCssProtocol('.ant-btn {}'), /exactly one LF/);
  assert.throws(() => assertCssProtocol('.ant-btn {}\n\n'), /more than one/);
});

test('preserves an upstream CSS string without a trailing newline', async () => {
  const root = await mkdtemp(join(tmpdir(), 'antd-baseline-preserve-'));
  const artifact = createArtifact({ id: 'upstream', path: 'static/upstream.css', layer: 'static', css: '.x {}', allowMissingTrailingNewline: true });
  const baseline = createBaseline({ upstream: { revision: 'abc1234' }, generator: { version: '1' }, inputDigest: 'input', artifacts: [artifact] });
  await writeBaseline(root, baseline, new Map([['upstream', '.x {}']]));
  const loaded = await readBaseline(root);
  assert.equal(loaded.cssById.get('upstream'), '.x {}');
  assert.equal(loaded.manifest.protocol.trailingNewline, 'preserve');
});

test('detects a single-byte mutation in a saved baseline', async () => {
  const root = await mkdtemp(join(tmpdir(), 'antd-style-baseline-'));
  try {
    const artifact = createArtifact({ id: 'Button.default.static', path: 'static/Button.default.css', layer: 'static', css });
    const baseline = createBaseline({
      upstream: { repository: 'ant-design', revision: '599b251da9', version: '6.6.4' },
      generator: { name: 'antd', version: '6.6.4' },
      inputDigest: sha256('fixture-input'),
      artifacts: [artifact],
    });
    await writeBaseline(root, baseline, new Map([[artifact.id, css]]));
    await writeFile(join(root, artifact.path), css.replace('primary', 'Primary'), 'utf8');
    await assert.rejects(() => readBaseline(root), /integrity mismatch/);
  } finally {
    await rm(root, { recursive: true, force: true });
  }
});
