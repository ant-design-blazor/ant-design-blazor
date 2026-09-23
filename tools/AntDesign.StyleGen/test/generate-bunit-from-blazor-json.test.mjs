import test from 'node:test';
import assert from 'node:assert/strict';
import { generateBunitFromBlazorJson, writeGeneratedBunitTests } from '../src/generate-bunit-from-blazor-json.mjs';
import { verifyGeneratedBunitTests } from '../src/verify-generated-bunit-tests.mjs';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';

test('generates a real DynamicComponent bUnit render from translated JSON', () => {
  const files = generateBunitFromBlazorJson({ cases: [{
    id: 'button-1',
    react: {
      name: 'renders correctly',
      source: 'render(<Button>Follow</Button>);',
      snapshots: [{ key: 'Button renders correctly 1', markup: '<button>Follow</button>' }],
    },
    blazor: { status: 'candidate', component: 'Button', parameters: {}, childContent: 'Follow', interactions: [{ type: 'click', selector: '.change-btn' }] },
  }] });
  assert.match(files['ReactSource_button-tests.g.cs'], /RenderComponent<DynamicComponent>/);
  assert.match(files['ReactSource_button-tests.g.cs'], /ChildContent/);
  assert.match(files['ReactSource_button-tests.g.cs'], /render\(<Button>Follow<\/Button>\)/);
  assert.match(files['ReactSource_button-tests.g.cs'], /MarkupMatches\(@"<button>Follow<\/button>"\)/);
  assert.match(files['ReactSource_button-tests.g.cs'], /Find\("\.change-btn"\)\.Click\(\)/);
});

test('generates a red test for a missing Blazor component without Skip or Assert.True', () => {
  const files = generateBunitFromBlazorJson({ cases: [{
    id: 'react-only',
    react: { name: 'react only demo', source: 'render(<ReactOnlyDemo />);' },
    blazor: { status: 'skipped-component-not-found', component: 'ReactOnlyDemo', parameters: {}, childContent: '' },
  }] });
  assert.match(files['ReactSource_reactonlydemo-tests.g.cs'], /\[Fact\]/);
  assert.match(files['ReactSource_reactonlydemo-tests.g.cs'], /ReactOnlyDemo.*does not exist yet/);
  assert.doesNotMatch(files['ReactSource_reactonlydemo-tests.g.cs'], /Skip/);
  assert.doesNotMatch(files['ReactSource_reactonlydemo-tests.g.cs'], /Assert\.True/);
  assert.doesNotMatch(files['ReactSource_reactonlydemo-tests.g.cs'], /ResolveComponentType/);
});

test('generates a red test for every requires-component-adapter case', () => {
  const files = generateBunitFromBlazorJson({ cases: [{
    id: 'adapter-1',
    react: { name: 'complex scenario', sourceFile: 'components/modal/__tests__/index.test.tsx', source: 'render(<Demo />);' },
    blazor: { status: 'requires-component-adapter', component: null, parameters: {}, childContent: null },
  }] });
  assert.match(files['ReactSource_modal-tests.g.cs'], /\[Fact\]/);
  assert.match(files['ReactSource_modal-tests.g.cs'], /requires a component adapter/);
});

test('generates a RenderFragment for a nested JSX child component', () => {
  const files = generateBunitFromBlazorJson({ cases: [{
    id: 'nested',
    react: { name: 'nested', source: 'render(<Affix><Button>top</Button></Affix>);' },
    blazor: {
      status: 'candidate', component: 'Affix', parameters: {}, childContent: '',
      childTree: { component: 'Button', parameters: {}, childContent: 'top' },
    },
  }] });
  assert.match(files['ReactSource_affix-tests.g.cs'], /OpenComponent\(4, ReactSourceTestSupport.ResolveComponentType\("Button"\)\)/);
  assert.match(files['ReactSource_affix-tests.g.cs'], /AddContent\(0, "top"\)/);
});

test('generates ordered RenderFragment content for multiple JSX children', () => {
  const files = generateBunitFromBlazorJson({ cases: [{
    id: 'multiple-children',
    react: { name: 'multiple children', source: 'render(<Space />);' },
    blazor: {
      status: 'candidate', component: 'Space', parameters: {}, childContent: '',
      childTree: [
        { component: 'Button', parameters: {}, childContent: 'one' },
        { component: 'Button', parameters: {}, childContent: 'two' },
      ],
    },
  }] });
  const output = files['ReactSource_space-tests.g.cs'];
  assert.equal((output.match(/ResolveComponentType\("Button"\)/g) ?? []).length, 2);
  assert.match(output, /AddContent\(0, "one"\)[\s\S]*AddContent\(0, "two"\)/);
});

test('generates real bUnit interaction and DOM assertions from the translated flow', () => {
  const files = generateBunitFromBlazorJson({ cases: [{
    id: 'interaction-flow',
    react: { name: 'interaction flow', source: 'render(<Button />);' },
    blazor: {
      status: 'candidate', component: 'Button', parameters: {}, childContent: '',
      interactions: [
        { type: 'click', selector: '.trigger' },
        { type: 'change', selector: 'input', value: 'abc' },
      ],
      assertions: [
        { type: 'exists', selector: '.result' },
        { type: 'class', selector: '.result', expected: 'active' },
        { type: 'text', selector: '.result', expected: 'done' },
        { type: 'count', selector: '.item', expected: 2 },
      ],
    },
  }] });
  const output = files['ReactSource_button-tests.g.cs'];
  assert.match(output, /Find\("\.trigger"\)\.Click\(\)/);
  assert.match(output, /Find\("input"\)\.Change\("abc"\)/);
  assert.match(output, /Assert\.NotNull\(cut\.Find\("\.result"\)\)/);
  assert.match(output, /Assert\.Contains\("active", cut\.Find\("\.result"\)\.ClassList\)/);
  assert.match(output, /Assert\.Contains\("done", cut\.Find\("\.result"\)\.TextContent\)/);
  assert.match(output, /Assert\.Equal\(2, cut\.FindAll\("\.item"\)\.Count\)/);
  assert.doesNotMatch(output, /Assert\.True/);
});

test('generates SetParametersAndRender for a rerender transition', () => {
  const files = generateBunitFromBlazorJson({ cases: [{
    id: 'rerender-flow',
    react: { name: 'rerender flow', source: 'render(<Alert message="before" />);' },
    blazor: {
      status: 'candidate', component: 'Alert', parameters: { message: 'before' }, childContent: '',
      renderSteps: [{ type: 'rerender', source: 'rerender(<Alert message="after" />)', component: 'Alert', parameters: { message: 'after' }, childContent: '' }],
    },
  }] });
  const output = files['ReactSource_alert-tests.g.cs'];
  assert.match(output, /rerenderParameters0\["Message"\] = "after"/);
  assert.match(output, /SetParametersAndRender/);
  assert.match(output, /x => x\.Parameters, rerenderParameters0/);
});

test('generates real wait, value and negative matcher steps', () => {
  const files = generateBunitFromBlazorJson({ cases: [{
    id: 'async-input-flow',
    react: { name: 'async input flow', source: 'render(<Input />);' },
    blazor: {
      status: 'candidate', component: 'Input', parameters: {}, childContent: '',
      interactions: [{ type: 'wait', reason: 'waitFakeTimer' }, { type: 'change', selector: 'input', value: 'abc' }],
      assertions: [
        { type: 'attribute', selector: 'input', name: 'value', expected: 'abc' },
        { type: 'not-exists', selector: '.error' },
        { type: 'not-class', selector: '.field', expected: 'invalid' },
      ],
    },
  }] });
  const output = files['ReactSource_input-tests.g.cs'];
  assert.match(output, /WaitForState\(\(\) => true/);
  assert.match(output, /Find\("input"\)\.Change\("abc"\)/);
  assert.match(output, /GetAttribute\("value"\)/);
  assert.match(output, /Assert\.DoesNotContain\("invalid"/);
});

test('generates a typed EventCallback adapter for Button.OnClick', () => {
  const files = generateBunitFromBlazorJson({ cases: [{
    id: 'button-callback',
    react: { name: 'button callback', source: 'render(<Button onClick={handleClick}>run</Button>);' },
    blazor: {
      status: 'candidate', component: 'Button',
      parameters: { onClick: { kind: 'callback', source: 'handleClick' } }, childContent: 'run',
      interactions: [{ type: 'click', selector: 'button' }],
    },
  }] });
  const output = files['ReactSource_button-tests.g.cs'];
  assert.match(output, /EventCallback\.Factory\.Create<MouseEventArgs>\(this/);
  assert.match(output, /Find\("button"\)\.Click\(\)/);
});

test('generates component-specific callback generic types', () => {
  const files = generateBunitFromBlazorJson({ cases: [{
    id: 'pagination-callback',
    react: { name: 'pagination callback', source: 'render(<Pagination onChange={handleChange} />);' },
    blazor: { status: 'candidate', component: 'Pagination', parameters: { onChange: { kind: 'callback', source: 'handleChange' } }, childContent: '' },
  }] });
  assert.match(files['ReactSource_pagination-tests.g.cs'], /EventCallback\.Factory\.Create<PaginationEventArgs>\(this/);
});

test('maps React Checkbox onChange to Blazor CheckedChanged', () => {
  const files = generateBunitFromBlazorJson({ cases: [{
    id: 'checkbox-callback',
    react: { name: 'checkbox callback', source: 'render(<Checkbox onChange={handleChange} />);' },
    blazor: { status: 'candidate', component: 'Checkbox', parameters: { onChange: { kind: 'callback', source: 'handleChange' } }, childContent: '' },
  }] });
  assert.match(files['ReactSource_checkbox-tests.g.cs'], /\["CheckedChanged"\] = EventCallback\.Factory\.Create<bool>\(this, _ =>/);
});

test('cleans and verifies generated files using a manifest', () => {
  const output = fs.mkdtempSync(path.join(os.tmpdir(), 'antd-bunit-generated-'));
  const input = { cases: [{ id: 'button-1', react: { name: 'renders', source: 'render(<Button />);' }, blazor: { status: 'candidate', component: 'Button', parameters: {}, childContent: '' } }] };
  const first = writeGeneratedBunitTests(input, output);
  assert.equal(verifyGeneratedBunitTests(output).valid, true);
  fs.writeFileSync(path.join(output, 'handwritten.g.cs'), 'class Handwritten {}');
  const second = writeGeneratedBunitTests(input, output);
  assert.equal(second.removed.includes('ReactSource_button-tests.g.cs'), true);
  assert.equal(fs.existsSync(path.join(output, 'handwritten.g.cs')), true);
  assert.equal(first.manifest.files['ReactSource_button-tests.g.cs'], second.manifest.files['ReactSource_button-tests.g.cs']);
});
