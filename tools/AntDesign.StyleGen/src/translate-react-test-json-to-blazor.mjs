#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';

// Candidate generation is intentionally conservative: a JSX tag must map to a
// concrete AntDesign Blazor component. Custom demo components and React-only
// subcomponents remain adapter work instead of producing uncompilable tests.
const KNOWN_BLAZOR_COMPONENTS = new Set([
  'Affix', 'Alert', 'Anchor', 'AutoComplete', 'Avatar', 'BackTop', 'Badge', 'Breadcrumb', 'Button',
  'Calendar', 'Card', 'Carousel', 'Cascader', 'Checkbox', 'Collapse', 'ConfigProvider',
  'DatePicker', 'Descriptions', 'Divider', 'Drawer', 'Dropdown', 'Empty', 'Flex', 'Form',
  'Image', 'Input', 'InputNumber', 'Layout', 'List', 'Mentions', 'Menu', 'Modal', 'Pagination', 'Progress',
  'Radio', 'Rate', 'Result', 'Segmented', 'Select', 'Skeleton', 'Slider', 'Space', 'Spin',
  'Splitter', 'Statistic', 'Steps', 'Switch', 'Table', 'Tabs', 'Tag', 'TimePicker', 'Timeline', 'Tooltip',
  'Tour', 'Transfer', 'Tree', 'TreeSelect', 'Typography', 'Upload', 'Watermark',
]);
const TYPED_CALLBACK_COMPONENTS = new Map([
  ['Button:onClick', { parameter: 'OnClick', generic: 'MouseEventArgs' }],
  ['Switch:onClick', { parameter: 'OnClick', generic: null }],
  ['BackTop:onClick', { parameter: 'OnClick', generic: null }],
  ['Pagination:onChange', { parameter: 'OnChange', generic: 'PaginationEventArgs' }],
  ['Input:onChange', { parameter: 'OnChange', generic: 'string' }],
  ['TextArea:onChange', { parameter: 'OnChange', generic: 'string' }],
  ['Checkbox:onChange', { parameter: 'CheckedChanged', generic: 'bool' }],
  ['Radio:onChange', { parameter: 'CheckedChanged', generic: 'bool' }],
  ['Tabs:onChange', { parameter: 'OnChange', generic: 'string' }],
  ['Calendar:onChange', { parameter: 'OnChange', generic: 'DateTime' }],
  ['Calendar:onSelect', { parameter: 'OnSelect', generic: 'DateTime' }],
  ['DatePicker:onChange', { parameter: 'OnChange', generic: 'DateTimeChangedEventArgs<DateTime>' }],
  ['AutoComplete:onSelect', { parameter: 'OnSelectionChange', generic: 'AutoCompleteOption' }],
]);
import process from 'node:process';

function parseJsxLiteral(value) {
  const trimmed = value.trim();
  if (trimmed === 'true') return true;
  if (trimmed === 'false') return false;
  if (trimmed === 'null') return null;
  if (/^-?\d+(?:\.\d+)?$/.test(trimmed)) return Number(trimmed);
  return undefined;
}

function parseSimpleOpening(source, start) {
  const nameMatch = source.slice(start).match(/^<([A-Z][\w.]*)\b/);
  if (!nameMatch) return null;
  const component = nameMatch[1];
  let index = start + nameMatch[0].length;
  let braceDepth = 0;
  let quote = null;
  for (; index < source.length; index += 1) {
    const character = source[index];
    if (quote) {
      if (character === quote && source[index - 1] !== '\\') quote = null;
      continue;
    }
    if (character === '"' || character === "'") {
      quote = character;
    } else if (character === '{') {
      braceDepth += 1;
    } else if (character === '}') {
      braceDepth -= 1;
      if (braceDepth < 0) return null;
    } else if (character === '>' && braceDepth === 0) {
      return { component, opening: source.slice(start + nameMatch[0].length, index), end: index + 1 };
    }
  }
  return null;
}

function parseSimpleAttributes(source) {
  const parameters = {};
  let index = 0;
  while (index < source.length) {
    while (/\s/.test(source[index] ?? '')) index += 1;
    if (index >= source.length || source[index] === '/') break;
    const name = source.slice(index).match(/^([\w:-]+)/);
    if (!name) return null;
    index += name[0].length;
    while (/\s/.test(source[index] ?? '')) index += 1;
    if (source[index] !== '=') {
      parameters[name[1]] = true;
      continue;
    }
    index += 1;
    while (/\s/.test(source[index] ?? '')) index += 1;
    if (source[index] === '"' || source[index] === "'") {
      const quote = source[index++];
      const end = source.indexOf(quote, index);
      if (end < 0) return null;
      parameters[name[1]] = source.slice(index, end);
      index = end + 1;
      continue;
    }
    if (source[index] !== '{') return null;
    let end = index + 1;
    let depth = 1;
    let quote = null;
    for (; end < source.length && depth > 0; end += 1) {
      const character = source[end];
      if (quote) {
        if (character === quote && source[end - 1] !== '\\') quote = null;
      } else if (character === '"' || character === "'") {
        quote = character;
      } else if (character === '{') {
        depth += 1;
      } else if (character === '}') {
        depth -= 1;
      }
    }
    if (depth !== 0) return null;
    const expression = source.slice(index + 1, end - 1).trim();
    const literal = parseJsxLiteral(expression);
    if (literal === undefined) {
      if (!/^on[A-Z]/.test(name[1]) || !/^(?:[A-Za-z_$][\w$]*|\([^)]*\)\s*=>|[A-Za-z_$][\w$]*\s*=>)/.test(expression)) return null;
      parameters[name[1]] = { kind: 'callback', source: expression };
    } else {
      parameters[name[1]] = literal;
    }
    index = end;
  }
  return parameters;
}

function findElementEnd(source, opening) {
  const component = opening.component;
  let depth = 1;
  let index = opening.end;
  while (index < source.length) {
    const next = source.slice(index).match(new RegExp(`<\\/?${component.replace(/[.*+?^${}()|[\\]\\]/g, '\\$&')}\\b`));
    if (!next) return -1;
    const start = index + next.index;
    const parsed = parseSimpleOpening(source, start);
    if (parsed) {
      if (!/\/\s*$/.test(parsed.opening)) depth += 1;
      index = parsed.end;
      continue;
    }
    const closing = source.slice(start).match(new RegExp(`^</${component.replace(/[.*+?^${}()|[\\]\\]/g, '\\$&')}\\s*>`));
    if (closing) {
      depth -= 1;
      if (depth === 0) return { start, end: start + closing[0].length };
      index = start + closing[0].length;
    } else {
      index = start + 2;
    }
  }
  return -1;
}

function parseChildNodes(source) {
  const nodes = [];
  let index = 0;
  while (index < source.length) {
    const nextTag = source.slice(index).search(/<[A-Z][\w.]*/);
    if (nextTag < 0) {
      const text = source.slice(index).trim();
      if (/[<>]/.test(text)) return null;
      if (text) nodes.push({ text });
      break;
    }
    const text = source.slice(index, index + nextTag).trim();
    if (/[<>]/.test(text)) return null;
    if (text) nodes.push({ text });
    const start = index + nextTag;
    const opening = parseSimpleOpening(source, start);
    if (!opening) return null;
    const selfClosing = /\/\s*$/.test(opening.opening);
    const endInfo = selfClosing ? { start: start, end: opening.end } : findElementEnd(source, opening);
    if (endInfo < 0 || !endInfo) return null;
    const expression = source.slice(start, endInfo.end);
    const parsed = parseSimpleJsx(expression);
    if (!parsed) return null;
    nodes.push(parsed);
    index = endInfo.end;
  }
  return nodes;
}

function parseSimpleJsx(expression) {
  const start = expression.search(/<[A-Z][\w.]*/);
  if (start < 0) return null;
  const opening = parseSimpleOpening(expression, start);
  if (!opening) return null;
  const selfClosing = /\/\s*$/.test(opening.opening);
  if (selfClosing) {
    const parameters = parseSimpleAttributes(opening.opening.slice(0, -1));
    if (!parameters) return null;
    return {
      component: opening.component,
      parameters,
      childContent: '',
      extraction: 'simple-jsx',
      unsupportedReason: KNOWN_BLAZOR_COMPONENTS.has(opening.component) ? null : 'component-not-found',
    };
  }
  const endInfo = findElementEnd(expression, opening);
  if (endInfo < 0 || !endInfo) return null;
  const childContent = expression.slice(opening.end, endInfo.start).trim();
  let childTree = null;
  if (childContent.includes('<')) {
    const childNodes = parseChildNodes(childContent);
    if (!childNodes || childNodes.some((node) => node.unsupportedReason || node.childTree?.some?.((child) => child.unsupportedReason))) return null;
    if (childNodes.length === 1 && !childNodes[0].text) childTree = childNodes[0];
    else childTree = childNodes;
  } else if (/\r?\n/.test(childContent) || /[{}]/.test(childContent)) {
    return null;
  }
  const parameters = parseSimpleAttributes(opening.opening);
  if (!parameters) return null;
  return {
    component: opening.component,
    parameters,
    childContent: childTree ? '' : childContent,
    ...(childTree ? { childTree } : {}),
    extraction: 'simple-jsx',
    unsupportedReason: KNOWN_BLAZOR_COMPONENTS.has(opening.component) ? null : 'component-not-found',
  };
}

function unquote(value) {
  const trimmed = value.trim();
  if ((trimmed.startsWith("'") && trimmed.endsWith("'")) || (trimmed.startsWith('"') && trimmed.endsWith('"'))) {
    return trimmed.slice(1, -1);
  }
  return null;
}

function selectorFromTarget(target) {
  const querySelector = target.match(/querySelector\(\s*(['"])([^'"]+)\1\s*\)/);
  if (querySelector) return querySelector[2];

  const testId = target.match(/(?:getByTestId|queryByTestId)\(\s*(['"])([^'"]+)\1\s*\)/);
  if (testId) return `[data-testid="${testId[2]}"]`;

  const role = target.match(/(?:getByRole|queryByRole)\(\s*(['"])([^'"]+)\1/);
  if (role) return `[role="${role[2]}"]`;

  return null;
}

function extractInteraction(call) {
  const source = call.source ?? '';
  const wait = source.match(/\b(waitFakeTimer|waitRaf|waitFor|waitForTimeout|waitForNextUpdate)\s*\(/);
  if (wait) {
    const milliseconds = source.match(/waitForTimeout\s*\(\s*(\d+)/)?.[1];
    return { type: 'wait', reason: wait[1], ...(milliseconds ? { milliseconds: Number(milliseconds) } : {}) };
  }
  const action = source.match(/(?:fireEvent|userEvent)\.(click|change|focus|blur|keyboard|type)\s*\(/);
  if (!action) return null;
  const selector = selectorFromTarget(source.slice(action.index + action[0].length));
  if (!selector) return null;
  const value = action[1] === 'change' || action[1] === 'type'
    ? (source.match(/value\s*:\s*(['"])(.*?)\1/)?.[2] ?? '')
    : undefined;
  return { type: action[1], selector, ...(value !== undefined ? { value } : {}) };
}

function extractSimpleInteractions(item) {
  return (item.interactionCalls ?? []).map(extractInteraction).filter(Boolean);
}

function extractAssertion(call) {
  const source = call.source ?? '';
  const callback = source.match(/expect\(\s*([A-Za-z_$][\w$]*)\s*\)(\.?not)?\.(toHaveBeenCalled|toHaveBeenCalledTimes)\s*\(\s*(\d+)?/);
  if (callback) {
    return {
      type: 'callback-called',
      callback: callback[1],
      negative: Boolean(callback[2]),
      ...(callback[4] ? { expected: Number(callback[4]) } : {}),
    };
  }
  const targetMatch = source.match(/expect\(\s*([\s\S]*?)\s*\)\s*\./);
  if (!targetMatch) return null;
  const selector = selectorFromTarget(targetMatch[1]);
  if (!selector) return null;

  const negative = /\.not\./.test(source);
  if (/\.toBeTruthy\(\)|\.toBeFalsy\(\)|\.toBeNull\(\)|\.toBeUndefined\(\)/.test(source)) {
    const truthy = /\.toBeTruthy\(\)/.test(source);
    return { type: (truthy !== negative) ? 'exists' : 'not-exists', selector };
  }

  const classMatch = source.match(/\.toHaveClass\(\s*(['"])([^'"]+)\1/);
  if (classMatch) return { type: negative ? 'not-class' : 'class', selector, expected: classMatch[2] };
  const attributeMatch = source.match(/\.toHaveAttribute\(\s*(['"])([^'"]+)\1\s*,\s*(['"])([^'"]*)\3/);
  if (attributeMatch) return { type: negative ? 'not-attribute' : 'attribute', selector, name: attributeMatch[2], expected: attributeMatch[4] };
  const valueMatch = source.match(/\.toHaveValue\(\s*(['"])([^'"]*)\1/);
  if (valueMatch) return { type: negative ? 'not-attribute' : 'attribute', selector, name: 'value', expected: valueMatch[2] };
  const textMatch = source.match(/\.toHaveTextContent\(\s*(['"])([^'"]*)\1/);
  if (textMatch) return { type: negative ? 'not-text' : 'text', selector, expected: textMatch[2] };
  const countMatch = source.match(/\.toHaveLength\(\s*(\d+)\s*\)/);
  if (countMatch && /querySelectorAll|queryAllBy/.test(targetMatch[1])) {
    return { type: 'count', selector, expected: Number(countMatch[1]) };
  }
  return null;
}

function extractAssertions(item) {
  return (item.assertionCalls ?? []).map(extractAssertion).filter(Boolean);
}

function classifyRenderAdapter(item) {
  const source = (item.renderCalls ?? []).map((call) => call.source ?? '').join('\n');
  if (/\bon[A-Z][A-Za-z0-9]*\s*=\s*\{/.test(source)) return 'callback-prop-requires-typed-event-adapter';
  if (/[{[]/.test(source) && /render\s*\(/.test(source)) return 'dynamic-jsx-expression-requires-adapter';
  return 'render-expression-requires-component-adapter';
}

export function translateReactTests(input) {
  const result = typeof input === 'string' ? JSON.parse(fs.readFileSync(input, 'utf8')) : input;
  const cases = result.cases.map((item) => {
    const renderCalls = item.renderCalls ?? [];
    const parsedRenders = renderCalls.map((call) => ({ call, render: parseSimpleJsx(call.source) }));
    const render = parsedRenders.map((entry) => entry.render).find(Boolean);
    const renderSteps = parsedRenders.slice(1)
      .filter((entry) => entry.render)
      .map((entry) => ({
        type: entry.call.name,
        source: entry.call.source,
        component: entry.render.component,
        parameters: entry.render.parameters,
        childContent: entry.render.childContent,
        ...(entry.render.childTree ? { childTree: entry.render.childTree } : {}),
      }));
    const hasUnsupportedRenderStep = parsedRenders.slice(1).some((entry) => !entry.render);
    const interactions = extractSimpleInteractions(item);
    const assertions = extractAssertions(item).filter((assertion) => {
      if (assertion.type !== 'callback-called') return true;
      return render && Object.values(render.parameters)
        .some((value) => value?.kind === 'callback' && value.source === assertion.callback);
    });
    const unsupportedCallback = render && Object.entries(render.parameters).some(([name, value]) =>
      value?.kind === 'callback' && !TYPED_CALLBACK_COMPONENTS.has(`${render.component}:${name}`));
    return {
      id: item.id,
      react: item,
      blazor: render
        ? {
          status: render.unsupportedReason || hasUnsupportedRenderStep
            ? 'skipped-component-not-found'
            : unsupportedCallback ? 'requires-component-adapter' : 'candidate',
          component: render.component,
          parameters: render.parameters,
          childContent: render.childContent,
          ...(render.childTree ? { childTree: render.childTree } : {}),
          extraction: render.extraction,
          interactions,
          ...(assertions.length ? { assertions } : {}),
          ...(renderSteps.length ? { renderSteps } : {}),
          ...((render.unsupportedReason || hasUnsupportedRenderStep || unsupportedCallback)
            ? { reason: render.unsupportedReason ?? (hasUnsupportedRenderStep ? 'rerender-requires-adapter' : 'callback-prop-requires-typed-event-adapter') }
            : {}),
        }
        : { status: 'requires-component-adapter', component: null, parameters: {}, childContent: null, reason: classifyRenderAdapter(item) },
    };
  });
  return { schemaVersion: 1, source: result.source, summary: { cases: cases.length, candidates: cases.filter((item) => item.blazor.status === 'candidate').length }, cases };
}

if (import.meta.url === `file://${process.argv[1]}`) {
  const input = process.argv[2];
  const output = process.argv[3] ?? path.join(path.dirname(input), 'blazor-test-cases.json');
  if (!input) { console.error('Usage: node translate-react-test-json-to-blazor.mjs <react-test-cases.json> [output]'); process.exit(2); }
  const result = translateReactTests(input);
  fs.writeFileSync(output, `${JSON.stringify(result, null, 2)}\n`);
  console.log(JSON.stringify({ output, ...result.summary }, null, 2));
}
