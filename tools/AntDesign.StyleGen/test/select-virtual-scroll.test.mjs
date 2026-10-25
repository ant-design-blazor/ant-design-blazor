import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import vm from 'node:vm';
import test from 'node:test';
import ts from 'typescript';
import { JSDOM } from 'jsdom';

const source = readFileSync(new URL('../../../components/core/JsInterop/modules/components/selectHelper.ts', import.meta.url), 'utf8');
const script = ts.transpileModule(source, { compilerOptions: { target: ts.ScriptTarget.ES2020, module: ts.ModuleKind.CommonJS } }).outputText;
const exported = {};
vm.runInNewContext(script, { exports: exported, getComputedStyle: holder => ({ getPropertyValue: () => holder.optionHeight ?? '' }) });
const scrollToIndex = (...args) => exported.selectHelper.scrollToIndex(...args);
const holder = (scrollTop = 0, height = 32) => ({ scrollTop, clientHeight: 256, querySelector: () => ({ offsetHeight: height }) });

test('virtual navigation scrolls the real holder to distant unmounted rows without moving the page', () => {
  const list = holder();
  scrollToIndex(list, 0);
  assert.equal(list.scrollTop, 0);
  scrollToIndex(list, 7);
  assert.equal(list.scrollTop, 0);
  scrollToIndex(list, 99999);
  assert.equal(list.scrollTop, 100000 * 32 - 256);
  scrollToIndex(list, 0);
  assert.equal(list.scrollTop, 0);
});

test('row measurement and themed height are used instead of a fixed scroll offset', () => {
  const measured = holder(0, 40);
  scrollToIndex(measured, 10);
  assert.equal(measured.scrollTop, 440 - 256);
  const themed = { ...holder(), querySelector: () => null, optionHeight: '48px' };
  scrollToIndex(themed, 10);
  assert.equal(themed.scrollTop, 528 - 256);
});

test('invalid indices are ignored and unmeasured rows use the default option height', () => {
  const list = { ...holder(), querySelector: () => null };
  for (const index of [-1, NaN, 1.5, Infinity]) scrollToIndex(list, index);
  scrollToIndex(null, 1);
  assert.equal(list.scrollTop, 0);
  scrollToIndex(list, 10);
  assert.equal(list.scrollTop, 352 - 256);
});

function delayedFixture(t) {
  const dom = new JSDOM('<body><div id="holder"></div></body>');
  const list = dom.window.document.getElementById('holder');
  let height = 1000;
  let offset = 0;
  Object.defineProperties(list, {
    clientHeight: { get: () => 256 }, scrollHeight: { get: () => height },
    scrollTop: { get: () => offset, set: value => { offset = Math.max(0, Math.min(value, height - 256)); } },
  });
  list.getBoundingClientRect = () => ({ top: 0, bottom: 256, height: 256 });
  const addRow = (index, title) => {
    const row = dom.window.document.createElement('div');
    row.className = 'ant-select-item-option'; row.title = title;
    Object.defineProperty(row, 'offsetHeight', { get: () => 32 });
    row.getBoundingClientRect = () => ({ top: index * 32 - offset, bottom: (index + 1) * 32 - offset, height: 32 });
    list.append(row);
    return row;
  };
  addRow(0, 'First');
  t.after(() => { exported.selectHelper.cancelScroll(list); dom.window.close(); });
  return { dom, list, addRow, resize: value => { height = value; }, settle: () => new Promise(resolve => dom.window.setTimeout(resolve, 0)) };
}

test('a clamped jump resumes when Virtualize revises spacer heights and mounts the requested row', async t => {
  const { list, resize, addRow, settle } = delayedFixture(t);
  scrollToIndex(list, 999, 'Last');
  assert.equal(list.scrollTop, 744, 'initial estimated scroll range clamps the requested position');
  resize(32000); addRow(999, 'Last');
  await settle();
  assert.equal(list.scrollTop, 31744);
});

test('a stale cached target cannot finish a jump while the physical scroll range is still too short', async t => {
  const { list, resize, addRow, settle } = delayedFixture(t);
  const cached = addRow(30, 'Last');
  scrollToIndex(list, 999, 'Last');
  assert.equal(list.scrollTop, 744);
  assert.ok(cached.getBoundingClientRect().bottom <= 256, 'cached last row initially looks visible');
  cached.remove(); resize(32000); addRow(999, 'Last');
  await settle();
  assert.equal(list.scrollTop, 31744);
});

test('user scrolling or explicit close cancels a pending jump instead of fighting later DOM updates', async t => {
  for (const action of ['wheel', 'close']) {
    const { dom, list, resize, addRow, settle } = delayedFixture(t);
    scrollToIndex(list, 999, 'Last');
    if (action === 'wheel') list.dispatchEvent(new dom.window.Event('wheel'));
    else exported.selectHelper.cancelScroll(list);
    resize(32000); addRow(999, 'Last');
    await settle();
    assert.equal(list.scrollTop, 744);
  }
});

test('a newer key navigation supersedes an earlier pending target', async t => {
  const { list, resize, addRow, settle } = delayedFixture(t);
  scrollToIndex(list, 999, 'Last');
  scrollToIndex(list, 0, 'First');
  resize(32000); addRow(999, 'Last');
  await settle();
  assert.equal(list.scrollTop, 0);
});
