import { Buffer } from 'node:buffer';
import { compareMarkup } from './html-structure-compare.mjs';

function compilePattern(source) {
  if (source instanceof RegExp) return source;
  return source ? new RegExp(source, 'g') : null;
}

/**
 * Replaces only declared per-render IDs and strips only declared framework
 * markers. A fresh ID map is used per document so references such as
 * aria-controls continue to point at the corresponding normalized ID.
 */
export function normalizeRuntimeHtml(html, policy = {}) {
  const idPattern = compilePattern(policy.runtimeIdPattern);
  const referenceAttributes = new Set(policy.runtimeIdReferenceAttributes ?? [
    'aria-activedescendant', 'aria-controls', 'aria-describedby', 'aria-labelledby', 'aria-owns', 'for',
    'aria-details', 'aria-errormessage', 'aria-flowto', 'headers', 'form', 'list', 'popovertarget', 'commandfor',
  ]);
  const removeAttributes = policy.removeAttributes ?? [];
  const removeClassPatterns = (policy.removeClassPatterns ?? []).map((pattern) => compilePattern(pattern));
  const idMap = new Map();
  let nextId = 1;

  const isRuntimeId = (value) => {
    if (!idPattern) return false;
    idPattern.lastIndex = 0;
    return idPattern.test(value);
  };
  const normalizeId = (value) => {
    if (!isRuntimeId(value)) return value;
    if (!idMap.has(value)) idMap.set(value, `__runtime-id-${nextId++}`);
    return idMap.get(value);
  };
  let source = String(html ?? '');
  for (const marker of policy.removeCommentMarkers ?? []) {
    source = source.split(marker).join('');
  }
  const referencedIds = new Set();
  if (policy.removeUnreferencedRuntimeIds) {
    for (const [, name, , value] of source.matchAll(/\s+([\w:-]+)=(['"])(.*?)\2/g)) {
      if (referenceAttributes.has(name)) for (const id of value.split(/\s+/)) referencedIds.add(id);
      if ((name === 'href' || name === 'xlink:href') && value.startsWith('#')) referencedIds.add(value.slice(1));
      for (const match of value.matchAll(/url\(\s*['"]?#([^'"\s)]+)/g)) referencedIds.add(match[1]);
    }
  }
  const dropRuntimeId = value => policy.removeUnreferencedRuntimeIds && isRuntimeId(value) && !referencedIds.has(value);
  if (idPattern) {
    // Seed IDs in DOM order before rewriting any references. Markup often
    // emits aria-controls before id, and the mapping must remain relationship-
    // stable regardless of each framework's attribute order.
    for (const match of source.matchAll(/\sid=(['"])(.*?)\1/g)) {
      if (!dropRuntimeId(match[2])) normalizeId(match[2]);
    }
  }

  return source.replace(/\s+([\w:-]+)=(['"])(.*?)\2/g, (attribute, name, quote, value) => {
    if (removeAttributes.some((removed) => removed.endsWith('*')
      ? name.startsWith(removed.slice(0, -1))
      : name === removed)) return '';

    if (name === 'id' && idPattern) {
      if (dropRuntimeId(value)) return '';
      return ` ${name}=${quote}${normalizeId(value)}${quote}`;
    }
    if (referenceAttributes.has(name) && idPattern) {
      const normalizedReferences = value.replace(/\S+/g, normalizeId);
      return ` ${name}=${quote}${normalizedReferences}${quote}`;
    }
    if (idPattern && (name === 'href' || name === 'xlink:href') && value.startsWith('#')) {
      return ` ${name}=${quote}#${normalizeId(value.slice(1))}${quote}`;
    }
    if (idPattern && value.includes('url(')) {
      const normalized = value.replace(/(url\(\s*['"]?#)([^'"\s)]+)/g, (_, prefix, id) => prefix + normalizeId(id));
      return ` ${name}=${quote}${normalized}${quote}`;
    }

    if (name === 'class' && (removeClassPatterns.length || policy.sortClassTokens)) {
      const classes = value.split(/\s+/).filter(Boolean).filter((className) => !removeClassPatterns.some((pattern) => {
        pattern.lastIndex = 0;
        return pattern.test(className);
      }));
      if (policy.sortClassTokens) classes.sort();
      if (classes.length === 0) return '';
      return ` ${name}=${quote}${classes.join(' ')}${quote}`;
    }

    return attribute;
  });
}

export function compareRawHtml(expected, actual) {
  const expectedBytes = Buffer.from(String(expected ?? ''), 'utf8');
  const actualBytes = Buffer.from(String(actual ?? ''), 'utf8');
  const length = Math.max(expectedBytes.length, actualBytes.length);
  let offset = 0;
  while (offset < length && expectedBytes[offset] === actualBytes[offset]) offset += 1;
  if (offset === length) return { equal: true, byteLength: expectedBytes.length };

  const snippet = (bytes) => bytes.subarray(Math.max(0, offset - 40), Math.min(bytes.length, offset + 80)).toString('utf8');
  return {
    equal: false,
    firstDifference: {
      offset,
      expected: expectedBytes[offset] === undefined ? '<eof>' : String.fromCharCode(expectedBytes[offset]),
      actual: actualBytes[offset] === undefined ? '<eof>' : String.fromCharCode(actualBytes[offset]),
      expectedSnippet: snippet(expectedBytes),
      actualSnippet: snippet(actualBytes),
    },
    expectedByteLength: expectedBytes.length,
    actualByteLength: actualBytes.length,
  };
}

export function compareComputedStyles(expected = {}, actual = {}) {
  const differences = [];
  const paths = [...new Set([...Object.keys(expected), ...Object.keys(actual)])].sort();
  for (const path of paths) {
    const expectedProperties = expected[path] ?? {};
    const actualProperties = actual[path] ?? {};
    const properties = [...new Set([...Object.keys(expectedProperties), ...Object.keys(actualProperties)])].sort();
    for (const property of properties) {
      const expectedValue = expectedProperties[property] ?? null;
      const actualValue = actualProperties[property] ?? null;
      if (expectedValue !== actualValue) differences.push({ path, property, expected: expectedValue, actual: actualValue });
    }
  }
  return { equal: differences.length === 0, differences };
}

function compareSvgUses(left, right, enabled, tolerancePx) {
  const differences = [];
  if (!enabled) return { equal: true, differences, tolerancePx };
  if (left.length === 0 || right.length === 0) differences.push({ field: 'emptySvgUseCapture' });
  for (let index = 0; index < Math.max(left.length, right.length); index += 1) {
    const expected = left[index]?.svgUse;
    const actual = right[index]?.svgUse;
    if (!expected || !actual) {
      differences.push({ index, field: 'missingSvgUseCapture' });
      continue;
    }
    for (const [side, glyph] of [['expected', expected], ['actual', actual]]) {
      if (!glyph.definitions?.length || !Number.isFinite(glyph.width) || !Number.isFinite(glyph.height) || !(glyph.width > 0) || !(glyph.height > 0)) {
        differences.push({ index, field: 'unpaintedSvgUse', side, href: glyph.href });
      }
    }
    if (expected.href !== actual.href) differences.push({ index, field: 'svgUseHref', expected: expected.href, actual: actual.href });
    if (JSON.stringify(expected.definitions) !== JSON.stringify(actual.definitions)) {
      differences.push({ index, field: 'svgSymbolDefinitionOrder', href: expected.href });
    }
    for (const property of ['width', 'height']) {
      if (Math.abs(expected[property] - actual[property]) > tolerancePx) {
        differences.push({ index, field: `svgUse.${property}`, expected: expected[property], actual: actual[property], tolerancePx });
      }
    }
  }
  return { equal: differences.length === 0, differences, tolerancePx };
}

/** Build independent strict-HTML, browser-DOM-tree, Semantic DOM and CSS gates. */
export function compareDomSnapshots(expected, actual, policy = {}) {
  const expectedHtml = normalizeRuntimeHtml(expected?.html, policy.normalization);
  const actualHtml = normalizeRuntimeHtml(actual?.html, policy.normalization);
  const ignoredAttributes = policy.ignoredAttributes ?? [];
  const domTree = compareMarkup(expectedHtml, actualHtml, { ignoredAttributes });

  const expectedSemantic = expected?.semantic ?? {};
  const actualSemantic = actual?.semantic ?? {};
  const semanticPaths = [...new Set([...Object.keys(expectedSemantic), ...Object.keys(actualSemantic),
    ...Object.keys(policy.semanticMinimumCounts ?? {}), ...(policy.svgUsePaths ?? [])])].sort();
  const semantic = semanticPaths.map((path) => {
    const left = expectedSemantic[path] ?? [];
    const right = actualSemantic[path] ?? [];
    const requiredCount = policy.semanticMinimumCounts?.[path] ?? 0;
    const htmlComparison = left.length === 1 && right.length === 1 ? compareRawHtml(
      normalizeRuntimeHtml(left[0].html, policy.normalization),
      normalizeRuntimeHtml(right[0].html, policy.normalization),
    ) : null;
    const computedStyles = compareComputedStyles(
      Object.fromEntries(left.map((node, index) => [`${path}[${index}]`, node.computedStyle ?? {}])),
      Object.fromEntries(right.map((node, index) => [`${path}[${index}]`, node.computedStyle ?? {}])),
    );
    const semanticTree = left.length === right.length
      ? left.reduce((result, node, index) => {
        if (!result.equal) return result;
        return compareMarkup(
          normalizeRuntimeHtml(node.html, policy.normalization),
          normalizeRuntimeHtml(right[index]?.html, policy.normalization),
          { ignoredAttributes: [...ignoredAttributes, ...(policy.ignoredSemanticAttributes?.[path] ?? [])] },
        );
      }, { equal: true, differences: [] })
      : { equal: false, differences: [{ field: 'count', expected: left.length, actual: right.length }] };
    const count = left.length >= requiredCount && right.length >= requiredCount;
    const svgUse = compareSvgUses(left, right,
      policy.svgUsePaths?.includes(path) || [...left, ...right].some(node => node.svgUse), policy.svgUsePaintTolerancePx ?? 0);
    const geometryDifferences = [];
    const geometryTolerancePx = policy.geometryTolerancePx ?? 0;
    if (left.length !== right.length) {
      geometryDifferences.push({ field: 'count', expected: left.length, actual: right.length });
    } else {
      left.forEach((node, index) => {
        const expectedGeometry = node.relativeGeometry;
        const actualGeometry = right[index]?.relativeGeometry;
        if (!expectedGeometry && !actualGeometry) return;
        if (!expectedGeometry || !actualGeometry) {
          geometryDifferences.push({ index, expected: expectedGeometry ?? null, actual: actualGeometry ?? null });
          return;
        }
        for (const property of ['left', 'top', 'width', 'height']) {
          const expectedValue = expectedGeometry[property];
          const actualValue = actualGeometry[property];
          if (Math.abs(expectedValue - actualValue) > geometryTolerancePx) {
            geometryDifferences.push({ index, property, expected: expectedValue, actual: actualValue,
              difference: Math.abs(expectedValue - actualValue), tolerance: geometryTolerancePx });
          }
        }
      });
    }
    return {
      path,
      equal: semanticTree.equal && count && computedStyles.equal && geometryDifferences.length === 0 && svgUse.equal,
      requiredCount,
      expectedCount: left.length,
      actualCount: right.length,
      html: htmlComparison,
      domTree: semanticTree,
      computedStyles,
      geometry: { equal: geometryDifferences.length === 0, tolerancePx: geometryTolerancePx, differences: geometryDifferences },
      svgUse,
    };
  });
  const styles = compareComputedStyles(expected?.computedStyles, actual?.computedStyles);
  const htmlExact = compareRawHtml(expectedHtml, actualHtml);
  const equal = htmlExact.equal && domTree.equal && styles.equal && semantic.every((item) => item.equal);
  const semanticStyleDifferences = semantic.flatMap((item) => [
    ...item.computedStyles.differences,
    ...item.geometry.differences.map((difference) => ({ path: item.path, ...difference })),
    ...item.svgUse.differences.map((difference) => ({ path: item.path, ...difference })),
  ]);
  const missingSemanticPaths = semantic
    .filter((item) => item.expectedCount < item.requiredCount || item.actualCount < item.requiredCount)
    .map(({ path, requiredCount, expectedCount, actualCount }) => ({ path, requiredCount, expectedCount, actualCount }));
  const visualStyles = {
    equal: styles.equal && semanticStyleDifferences.length === 0 && missingSemanticPaths.length === 0,
    root: styles,
    semanticDifferences: semanticStyleDifferences,
    missingSemanticPaths,
  };

  return { equal, htmlExact, domTree, semantic, computedStyles: styles, visualStyles };
}
