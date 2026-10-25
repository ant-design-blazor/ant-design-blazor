import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import vm from 'node:vm';
import test from 'node:test';
import ts from 'typescript';
import { JSDOM } from 'jsdom';

const source = readFileSync(new URL('../../../components/core/JsInterop/modules/dom/eventHelper.ts', import.meta.url), 'utf8');
const script = ts.transpileModule(source, { compilerOptions: { target: ts.ScriptTarget.ES2020, module: ts.ModuleKind.CommonJS } }).outputText;

function fixture(t, ignoreSelector) {
  const dom = new JSDOM('<body class="remove"><div id="trigger"><span class="remove"><svg><path/></svg>Remove</span><span class="label">Label</span></div></body>');
  const document = dom.window.document;
  const trigger = document.getElementById('trigger');
  const calls = [];
  const exported = {};
  vm.runInNewContext(script, {
    exports: exported, Node: dom.window.Node, Element: dom.window.Element, Window: dom.window.Window,
    window: dom.window,
    require: name => name === './exports' ? { domInfoHelper: { get: element => element } } : { state: {} },
    setTimeout: callback => callback(),
  });
  exported.eventHelper.addDomEventListener(trigger, 'click', false,
    { _id: 'test', invokeMethodAsync: (...args) => calls.push(args) }, false, ignoreSelector);
  t.after(() => dom.window.close());
  const click = target => {
    const event = new dom.window.MouseEvent('click', { bubbles: true, cancelable: true, composed: true });
    target.dispatchEvent(event);
    return event;
  };
  return { document, trigger, calls, click };
}

test('a nested remove icon bypasses only the native trigger and reaches delegated handlers once', t => {
  const { document, calls, click } = fixture(t, '.remove');
  let delegated = 0;
  document.addEventListener('click', () => delegated++);
  for (const target of [document.querySelector('path'), document.querySelector('.remove svg'), document.querySelector('#trigger .remove').lastChild]) {
    assert.equal(click(target).defaultPrevented, false);
  }
  assert.equal(calls.length, 0);
  assert.equal(delegated, 3);
});

test('normal label and root clicks still trigger; matching ancestors outside the trigger are ignored', t => {
  const { document, trigger, calls, click } = fixture(t, '.remove');
  click(document.querySelector('.label'));
  click(trigger);
  assert.equal(calls.length, 2);
});

test('listeners without an ignore selector retain their existing behavior', t => {
  const { document, calls, click } = fixture(t);
  click(document.querySelector('path'));
  assert.equal(calls.length, 1);
});
