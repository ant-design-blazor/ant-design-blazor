import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import vm from 'node:vm';
import test from 'node:test';
import ts from 'typescript';

const source = readFileSync(new URL('../../../components/core/JsInterop/modules/dom/infoHelper.ts', import.meta.url), 'utf8');
const script = ts.transpileModule(source, { compilerOptions: { target: ts.ScriptTarget.ES2020, module: ts.ModuleKind.CommonJS } }).outputText;
const exported = {};
vm.runInNewContext(script, {
  exports: exported,
  HTMLElement: class {},
  document: { documentElement: { scrollLeft: 12.5, scrollTop: 158.5 } },
  require: () => ({}),
});
const element = { getBoundingClientRect: () => ({ left: 327.1640625, top: 434.03125 }) };

test('legacy DOM info retains its rounded position contract', () => {
  const result = exported.infoHelper.getInfo(element);
  assert.equal(result.absoluteLeft, 340);
  assert.equal(result.absoluteTop, 593);
});

test('overlay measurements can preserve scroll-adjusted subpixel positions until alignment', () => {
  const result = exported.infoHelper.getInfo(element, true);
  assert.equal(result.absoluteLeft, 339.6640625);
  assert.equal(result.absoluteTop, 592.53125);
});
