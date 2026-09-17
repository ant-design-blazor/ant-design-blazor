import { Buffer } from 'node:buffer';
import { assertCssProtocol, sha256 } from './baseline.mjs';

export function compareCss(expected, actual, context = {}) {
  const expectedBytes = Buffer.from(expected ?? '', 'utf8');
  const actualBytes = Buffer.from(actual ?? '', 'utf8');
  const length = Math.max(expectedBytes.length, actualBytes.length);
  let offset = -1;
  for (let index = 0; index < length; index += 1) {
    if (expectedBytes[index] !== actualBytes[index]) {
      offset = index;
      break;
    }
  }

  if (offset < 0) return { equal: true, ...context, expectedSha256: sha256(expected), actualSha256: sha256(actual) };

  const expectedPrefix = expectedBytes.subarray(0, offset).toString('utf8');
  const line = expectedPrefix.split('\n').length;
  const column = expectedPrefix.length - expectedPrefix.lastIndexOf('\n');
  return {
    equal: false,
    ...context,
    expectedSha256: sha256(expected),
    actualSha256: sha256(actual),
    expectedByteLength: expectedBytes.length,
    actualByteLength: actualBytes.length,
    firstDifference: {
      byteOffset: offset,
      line,
      column,
      expectedByte: expectedBytes[offset] ?? null,
      actualByte: actualBytes[offset] ?? null,
      expectedChar: expectedBytes[offset] === undefined ? '<eof>' : String.fromCharCode(expectedBytes[offset]),
      actualChar: actualBytes[offset] === undefined ? '<eof>' : String.fromCharCode(actualBytes[offset]),
    },
  };
}

export function compareArtifactSets({ expectedManifest, expectedCssById, actualManifest, actualCssById, allowlist = [] }) {
  const expected = new Map(expectedManifest.artifacts.map((artifact) => [artifact.id, artifact]));
  const actual = new Map(actualManifest.artifacts.map((artifact) => [artifact.id, artifact]));
  const allowed = (id) => allowlist.some((item) => item === id || (item instanceof RegExp && item.test(id)));
  const diagnostics = [];

  for (const [id, artifact] of expected) {
    if (!actual.has(id)) {
      if (!allowed(id)) diagnostics.push({ equal: false, type: 'missing', id, expected: artifact });
      continue;
    }
    const result = compareCss(expectedCssById.get(id), actualCssById.get(id), {
      id,
      layer: artifact.layer,
      component: artifact.component,
      theme: artifact.theme,
    });
    if (!result.equal && !allowed(id)) diagnostics.push(result);
  }
  for (const [id, artifact] of actual) {
    if (!expected.has(id) && !allowed(id)) diagnostics.push({ equal: false, type: 'added', id, actual: artifact });
  }
  return { equal: diagnostics.length === 0, diagnostics };
}

export function validateComparableCss(css, path = '<inline>') {
  return assertCssProtocol(css, path);
}
