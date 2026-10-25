import { test } from 'node:test';
import assert from 'node:assert/strict';
import { checkComponentPageLayout } from '../src/component-page-layout-check.mjs';

const observation = { width:390,requestedWidth:390,clientWidth:390,scrollWidth:390,main:{x:0,right:390,width:390,top:100,paddingLeft:16,paddingRight:16},demoCount:16,boxes:[{id:'menu',x:16,right:374}] };
test('accepts a contained mobile docs page', () => assert.equal(checkComponentPageLayout([observation]).passed,true));
test('rejects desktop padding even when overflow is hidden', () => assert.equal(checkComponentPageLayout([{...observation,main:{...observation.main,paddingRight:164}}]).passed,false));
test('rejects the md-24 body wrapped below an md-6 sidebar', () => assert.equal(checkComponentPageLayout([{...observation,width:820,requestedWidth:820,clientWidth:820,scrollWidth:820,sidebar:{visible:true,top:100},main:{...observation.main,top:900}}]).passed,false));
test('rejects an incorrect viewport or horizontal overflow', () => {
  assert.equal(checkComponentPageLayout([{...observation,requestedWidth:320}]).passed,false);
  assert.equal(checkComponentPageLayout([{...observation,scrollWidth:502}]).passed,false);
});
test('rejects a page whose demos rendered error panels', () => assert.equal(checkComponentPageLayout([{...observation,errors:['InvalidOperationException']}]).passed,false));
