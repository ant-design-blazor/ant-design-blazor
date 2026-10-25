import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import test from 'node:test';
import { JSDOM } from 'jsdom';
import ts from 'typescript';

const source = readFileSync(new URL('../../../components/core/JsInterop/modules/components/iconHelper.ts', import.meta.url), 'utf8');
const compiled = ts.transpileModule(source, { compilerOptions: { target: ts.ScriptTarget.ES2017, module: ts.ModuleKind.ESNext } }).outputText;
let sequence = 0;

async function fixture(t) {
  const dom = new JSDOM('<body></body>'); // Never execute scripts or fetch resources.
  const saved = globalThis.document;
  globalThis.document = dom.window.document;
  t.after(() => {
    if (saved === undefined) delete globalThis.document; else globalThis.document = saved;
    dom.window.close();
  });
  const { iconHelper } = await import(`data:text/javascript;base64,${Buffer.from(compiled).toString('base64')}#${++sequence}`);
  return { helper: iconHelper, document: dom.window.document, event: type => new dom.window.Event(type) };
}

test('multiple iconfont resources load sequentially in React order without mutating the array', async t => {
  const { helper, document, event } = await fixture(t);
  const urls = ['first.js', 'last.js'];
  const done = helper.createIconFromfontCN(urls);
  assert.deepEqual(urls, ['first.js', 'last.js']);
  assert.equal(document.scripts.length, 1);
  assert.equal(document.scripts[0].getAttribute('src'), 'last.js');
  document.scripts[0].dispatchEvent(event('load'));
  await new Promise(setImmediate);
  assert.equal(document.scripts.length, 2);
  assert.equal(document.scripts[1].getAttribute('src'), 'first.js');
  document.scripts[1].dispatchEvent(event('load'));
  await done;
});

test('concurrent icon instances share pending requests and cached resources do not skip later URLs', async t => {
  const { helper, document, event } = await fixture(t);
  const first = helper.createIconFromfontCN(['a.js', 'b.js']);
  const second = helper.createIconFromfontCN(['a.js', 'b.js']);
  assert.equal(document.scripts.length, 1);
  document.scripts[0].dispatchEvent(event('load'));
  await new Promise(setImmediate);
  assert.equal(document.scripts.length, 2);
  document.scripts[1].dispatchEvent(event('load'));
  await Promise.all([first, second]);
  const next = helper.createIconFromfontCN(['new.js', 'b.js']);
  await new Promise(setImmediate);
  assert.equal(document.scripts.length, 3);
  assert.equal(document.scripts[2].getAttribute('src'), 'new.js');
  document.scripts[2].dispatchEvent(event('load'));
  await next;
});

test('resource failure continues the remaining scripts and duplicate URLs load once', async t => {
  const { helper, document, event } = await fixture(t);
  const done = helper.createIconFromfontCN(['a.js', 'a.js', 'bad.js']);
  document.scripts[0].dispatchEvent(event('error'));
  await new Promise(setImmediate);
  assert.equal(document.scripts[1].getAttribute('src'), 'a.js');
  document.scripts[1].dispatchEvent(event('load'));
  await done;
  assert.equal(document.scripts.length, 2);
});

test('existing scripts and quoted URLs are handled without CSS selector interpolation', async t => {
  const { helper, document, event } = await fixture(t);
  const url = '/quoted"resource.js';
  const script = document.createElement('script');
  script.setAttribute('data-namespace', url);
  document.body.appendChild(script);
  await helper.createIconFromfontCN(url);
  assert.equal(document.scripts.length, 1);
  const done = helper.createIconFromfontCN('fresh.js');
  document.scripts[1].dispatchEvent(event('load'));
  await done;
});
