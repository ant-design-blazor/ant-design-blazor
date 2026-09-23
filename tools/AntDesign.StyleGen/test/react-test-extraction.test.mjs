import test from 'node:test';
import assert from 'node:assert/strict';
import { extractReactTests } from '../src/extract-react-tests-to-json.mjs';
import { translateReactTests } from '../src/translate-react-test-json-to-blazor.mjs';

test('extracts React test source, renders, interactions and assertions', () => {
  const result = extractReactTests('/Volumes/Data/GitHub/ant-design');
  assert.ok(result.summary.testFiles > 0);
  assert.ok(result.summary.cases > 0);
  const button = result.cases.find((item) => item.name === 'renders correctly' && item.sourceFile.includes('button'));
  assert.ok(button);
  assert.ok(button.renderCalls.length > 0);
  assert.ok(button.snapshotCalls.length > 0);
});

test('keeps the complete Jest matcher chain for expect calls', () => {
  const result = extractReactTests('/Volumes/Data/GitHub/ant-design');
  const assertion = result.cases
    .flatMap((item) => item.assertionCalls)
    .find((call) => /toHaveClass|toBeTruthy|toHaveTextContent/.test(call.source));
  assert.ok(assertion);
  assert.match(assertion.source, /expect\([\s\S]+\)\.(toHaveClass|toBeTruthy|toHaveTextContent)/);
});

test('translates simple JSX render expressions into Blazor candidates', () => {
  const result = translateReactTests({ source: {}, cases: [{ id: 'x', renderCalls: [{ source: '<Button type="primary">Follow</Button>' }] }] });
  assert.equal(result.summary.candidates, 1);
  assert.deepEqual(result.cases[0].blazor, {
    status: 'candidate', component: 'Button', parameters: { type: 'primary' }, childContent: 'Follow', extraction: 'simple-jsx', interactions: [],
  });
});

test('generates a skipped case for a simple JSX component missing in Blazor', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'demo-component',
    renderCalls: [{ source: 'render(<ReactOnlyDemo>test</ReactOnlyDemo>)' }],
  }] });
  assert.equal(result.cases[0].blazor.status, 'skipped-component-not-found');
  assert.equal(result.cases[0].blazor.component, 'ReactOnlyDemo');
});

test('does not confuse JSX callback attributes with child content', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'affix-null-target',
    renderCalls: [{ source: 'render(<Affix target={() => null}>test</Affix>)' }],
  }] });
  assert.equal(result.summary.candidates, 0);
  assert.equal(result.cases[0].blazor.status, 'requires-component-adapter');
});

test('translates one nested JSX component into a child tree', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'nested',
    renderCalls: [{ source: 'render(<Affix><Button>top</Button></Affix>)' }],
  }] });
  assert.equal(result.cases[0].blazor.status, 'candidate');
  assert.equal(result.cases[0].blazor.childTree.component, 'Button');
  assert.equal(result.cases[0].blazor.childTree.childContent, 'top');
});

test('translates multiple JSX children without dropping their order', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'multiple-children',
    renderCalls: [{ source: 'render(<Space><Button>one</Button><Button>two</Button></Space>)' }],
  }] });
  assert.equal(result.cases[0].blazor.status, 'candidate');
  assert.equal(Array.isArray(result.cases[0].blazor.childTree), true);
  assert.deepEqual(result.cases[0].blazor.childTree.map((node) => node.childContent), ['one', 'two']);
});

test('does not treat unsupported native HTML children as plain text', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'native-html-child',
    renderCalls: [{ source: 'render(<Carousel><div>one</div><div>two</div></Carousel>)' }],
  }] });
  assert.equal(result.cases[0].blazor.status, 'requires-component-adapter');
});

test('translates waits, input values and negative matchers', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'async-input-flow',
    renderCalls: [{ source: 'render(<Input />)' }],
    interactionCalls: [{ name: 'waitFakeTimer', source: 'await waitFakeTimer()' }, { name: 'fireEvent.change', source: "fireEvent.change(container.querySelector('input')!, { target: { value: 'abc' } })" }],
    assertionCalls: [
      { name: 'expect', source: "expect(container.querySelector('input')).toHaveValue('abc')" },
      { name: 'expect', source: "expect(container.querySelector('.error')).not.toBeTruthy()" },
      { name: 'expect', source: "expect(container.querySelector('.field')).not.toHaveClass('invalid')" },
    ],
  }] });
  assert.deepEqual(result.cases[0].blazor.interactions, [
    { type: 'wait', reason: 'waitFakeTimer' },
    { type: 'change', selector: 'input', value: 'abc' },
  ]);
  assert.deepEqual(result.cases[0].blazor.assertions, [
    { type: 'attribute', selector: 'input', name: 'value', expected: 'abc' },
    { type: 'not-exists', selector: '.error' },
    { type: 'not-class', selector: '.field', expected: 'invalid' },
  ]);
});

test('classifies callback JSX as a typed adapter requirement', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'callback-prop',
    renderCalls: [{ source: 'render(<Anchor onChange={handleChange} />)' }],
  }] });
  assert.equal(result.cases[0].blazor.status, 'requires-component-adapter');
  assert.equal(result.cases[0].blazor.reason, 'callback-prop-requires-typed-event-adapter');
});

test('translates a Button callback prop as an executable callback parameter', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'button-callback',
    renderCalls: [{ source: 'render(<Button onClick={handleClick}>run</Button>)' }],
  }] });
  assert.equal(result.cases[0].blazor.status, 'candidate');
  assert.deepEqual(result.cases[0].blazor.parameters.onClick, { kind: 'callback', source: 'handleClick' });
});

test('accepts callback types with known component parameter metadata', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'pagination-callback',
    renderCalls: [{ source: 'render(<Pagination onChange={handleChange} />)' }],
  }] });
  assert.equal(result.cases[0].blazor.status, 'candidate');
});

test('accepts checkbox and radio change callbacks through CheckedChanged metadata', () => {
  for (const component of ['Checkbox', 'Radio']) {
    const result = translateReactTests({ source: {}, cases: [{
      id: `${component}-callback`,
      renderCalls: [{ source: `render(<${component} onChange={handleChange} />)` }],
    }] });
    assert.equal(result.cases[0].blazor.status, 'candidate');
  }
});

test('extracts callback invocation assertions', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'callback-assertion',
    renderCalls: [{ source: 'render(<Button onClick={handleClick} />)' }],
    assertionCalls: [
      { name: 'expect', source: 'expect(handleClick).toHaveBeenCalledTimes(1)' },
      { name: 'expect', source: 'expect(handleClick).not.toHaveBeenCalled()' },
    ],
  }] });
  assert.deepEqual(result.cases[0].blazor.assertions, [
    { type: 'callback-called', callback: 'handleClick', negative: false, expected: 1 },
    { type: 'callback-called', callback: 'handleClick', negative: true },
  ]);
});

test('translates common DOM interactions and assertions into executable steps', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'interaction-flow',
    renderCalls: [{ source: 'render(<Button />)' }],
    interactionCalls: [
      { name: 'fireEvent.click', source: "fireEvent.click(container.querySelector('.trigger')!)" },
      { name: 'fireEvent.change', source: "fireEvent.change(container.querySelector('input')!, { target: { value: 'abc' } })" },
    ],
    assertionCalls: [
      { name: 'expect', source: "expect(container.querySelector('.result')).toBeTruthy()" },
      { name: 'expect', source: "expect(container.querySelector('input')).toHaveValue('abc')" },
      { name: 'expect', source: "expect(container.querySelector('.result')).toHaveClass('active')" },
      { name: 'expect', source: "expect(container.querySelector('.result')).toHaveTextContent('done')" },
    ],
  }] });
  assert.deepEqual(result.cases[0].blazor.interactions, [
    { type: 'click', selector: '.trigger' },
    { type: 'change', selector: 'input', value: 'abc' },
  ]);
  assert.deepEqual(result.cases[0].blazor.assertions, [
    { type: 'exists', selector: '.result' },
    { type: 'attribute', selector: 'input', name: 'value', expected: 'abc' },
    { type: 'class', selector: '.result', expected: 'active' },
    { type: 'text', selector: '.result', expected: 'done' },
  ]);
});

test('translates rerender calls into parameter transition steps', () => {
  const result = translateReactTests({ source: {}, cases: [{
    id: 'rerender-flow',
    renderCalls: [
      { name: 'render', source: 'render(<Alert message="before" />)' },
      { name: 'rerender', source: 'rerender(<Alert message="after" />)' },
    ],
  }] });
  assert.equal(result.cases[0].blazor.status, 'candidate');
  assert.deepEqual(result.cases[0].blazor.renderSteps, [{
    type: 'rerender', source: 'rerender(<Alert message="after" />)', component: 'Alert',
    parameters: { message: 'after' }, childContent: '',
  }]);
});
