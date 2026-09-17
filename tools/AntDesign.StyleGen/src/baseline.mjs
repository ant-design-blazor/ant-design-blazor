import { createHash } from 'node:crypto';
import { mkdir, readFile, writeFile } from 'node:fs/promises';
import { dirname, join } from 'node:path';

export const BASELINE_SCHEMA_VERSION = 1;

export function sha256(value) {
  return createHash('sha256').update(value, 'utf8').digest('hex');
}

export function assertCssProtocol(css, artifactPath = '<inline>', options = {}) {
  if (typeof css !== 'string') throw new TypeError(`${artifactPath}: CSS must be a string`);
  if (css.includes('\r')) throw new Error(`${artifactPath}: CSS must use LF line endings`);
  if (!css.endsWith('\n') && !options.allowMissingTrailingNewline) throw new Error(`${artifactPath}: CSS must end with exactly one LF`);
  if (css.endsWith('\n\n')) throw new Error(`${artifactPath}: CSS has more than one trailing LF`);
  return css;
}

export function createArtifact({ id, path, layer, component, theme, css, allowMissingTrailingNewline = false }) {
  assertCssProtocol(css, path, { allowMissingTrailingNewline });
  return {
    id,
    path,
    layer,
    component: component ?? null,
    theme: theme ?? null,
    encoding: 'utf-8',
    lineEnding: 'lf',
    trailingNewline: css.endsWith('\n') ? 'one' : 'none',
    sha256: sha256(css),
    byteLength: Buffer.byteLength(css, 'utf8'),
  };
}

export function createBaseline({ upstream, generator, inputDigest, artifacts }) {
  return {
    schemaVersion: BASELINE_SCHEMA_VERSION,
    upstream,
    generator,
    protocol: { encoding: 'utf-8', lineEnding: 'lf', trailingNewline: artifacts.some((artifact) => artifact.trailingNewline === 'none') ? 'preserve' : 'one' },
    inputDigest,
    artifacts: [...artifacts].sort((left, right) => left.id.localeCompare(right.id)),
  };
}

export function validateBaseline(baseline) {
  if (baseline?.schemaVersion !== BASELINE_SCHEMA_VERSION) throw new Error('unsupported baseline schema version');
  if (baseline.protocol?.encoding !== 'utf-8' || baseline.protocol?.lineEnding !== 'lf' || !['one', 'preserve'].includes(baseline.protocol?.trailingNewline)) {
    throw new Error('baseline uses an unsupported CSS encoding protocol');
  }
  if (!Array.isArray(baseline.artifacts) || baseline.artifacts.length === 0) {
    throw new Error('baseline must contain at least one artifact');
  }
  const ids = new Set();
  for (const artifact of baseline.artifacts) {
    if (!artifact.id || ids.has(artifact.id)) throw new Error(`duplicate or missing artifact id: ${artifact.id ?? ''}`);
    if (!['static', 'token-values', 'runtime-overrides'].includes(artifact.layer)) {
      throw new Error(`invalid artifact layer for ${artifact.id}`);
    }
    if (!/^[a-f0-9]{64}$/.test(artifact.sha256) || !Number.isInteger(artifact.byteLength)) {
      throw new Error(`invalid integrity metadata for ${artifact.id}`);
    }
    ids.add(artifact.id);
  }
  return baseline;
}

export async function writeBaseline(root, baseline, cssById) {
  validateBaseline(baseline);
  for (const artifact of baseline.artifacts) {
    const css = cssById.get(artifact.id);
    assertCssProtocol(css, artifact.path, { allowMissingTrailingNewline: artifact.trailingNewline === 'none' });
    if (sha256(css) !== artifact.sha256 || Buffer.byteLength(css, 'utf8') !== artifact.byteLength) {
      throw new Error(`CSS changed after baseline metadata was created: ${artifact.id}`);
    }
    const output = join(root, artifact.path);
    await mkdir(dirname(output), { recursive: true });
    await writeFile(output, css, 'utf8');
  }
  await writeFile(join(root, 'manifest.json'), `${JSON.stringify(baseline, null, 2)}\n`, 'utf8');
}

export async function readBaseline(root) {
  const manifest = JSON.parse(await readFile(join(root, 'manifest.json'), 'utf8'));
  validateBaseline(manifest);
  const cssById = new Map();
  for (const artifact of manifest.artifacts) {
    const css = await readFile(join(root, artifact.path), 'utf8');
    assertCssProtocol(css, artifact.path, { allowMissingTrailingNewline: artifact.trailingNewline === 'none' });
    if (sha256(css) !== artifact.sha256 || Buffer.byteLength(css, 'utf8') !== artifact.byteLength) {
      throw new Error(`baseline integrity mismatch: ${artifact.id}`);
    }
    cssById.set(artifact.id, css);
  }
  return { manifest, cssById };
}
