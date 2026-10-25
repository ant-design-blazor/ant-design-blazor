import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import test from 'node:test';
import { JSDOM } from 'jsdom';

const source = readFileSync(new URL('../../../site/AntDesign.Docs/wwwroot/js/carbon-ad.js', import.meta.url), 'utf8');
let sequence = 0;

async function fixture(t) {
  // No scripts or network requests are executed in these lifecycle tests.
  const dom = new JSDOM('<body><div id="first"></div><div id="next"></div></body>');
  const saved = globalThis.document;
  globalThis.document = dom.window.document;
  t.after(() => {
    if (saved === undefined) delete globalThis.document; else globalThis.document = saved;
    dom.window.close();
  });
  const module = await import(`data:text/javascript;base64,${Buffer.from(source).toString('base64')}#${++sequence}`);
  return { module, document: dom.window.document, first: dom.window.document.getElementById('first'), next: dom.window.document.getElementById('next') };
}

test('interactive rerenders inject one Carbon script and preserve its vendor-owned ad', async t => {
  const { module, document, first } = await fixture(t);
  module.mount(first);
  const script = document.getElementById('_carbonads_js');
  const ad = document.createElement('div');
  ad.id = 'carbonads';
  script.parentElement.appendChild(ad);
  for (let i = 0; i < 10; i++) module.mount(first);
  assert.equal(document.querySelectorAll('script').length, 1);
  assert.equal(document.querySelectorAll('#carbonads').length, 1);
  assert.equal(first.querySelector('#carbonads'), ad);
  assert.equal(script.src, 'https://cdn.carbonads.com/carbon.js?serve=CW7I6K7I&placement=antblazorcom&format=cover');
});

test('SPA navigation reuses the ad and stale disposal cannot detach the next page', async t => {
  const { module, document, first, next } = await fixture(t);
  module.mount(first);
  const slot = first.firstElementChild;
  module.mount(next);
  module.unmount(first);
  assert.equal(next.firstElementChild, slot);
  assert.equal(first.childElementCount, 0);
  module.unmount(next);
  module.unmount(next);
  assert.ok(slot.parentElement.hidden);
  assert.ok(slot.isConnected);
  module.mount(first);
  assert.equal(first.firstElementChild, slot);
  assert.equal(document.querySelectorAll('#_carbonads_js').length, 1);
  assert.equal(document.querySelectorAll('[data-carbon-ad-parking]').length, 1);
});

test('a late ad response remains connected while its page is disposed', async t => {
  const { module, document, first, next } = await fixture(t);
  module.mount(first);
  module.unmount(first);
  first.remove();
  const script = document.getElementById('_carbonads_js');
  assert.ok(script.isConnected);
  const ad = document.createElement('div');
  ad.id = 'carbonads';
  script.parentElement.appendChild(ad);
  module.mount(next);
  assert.equal(next.querySelector('#carbonads'), ad);
  assert.equal(document.querySelectorAll('#_carbonads_js').length, 1);
});

test('detached hosts are ignored and docs routes do not prerender executable ad scripts', async t => {
  const { module, document } = await fixture(t);
  module.mount(document.createElement('div'));
  module.mount(null);
  assert.equal(document.querySelectorAll('script').length, 0);
  for (const [file, component] of [
    ['Pages/Components', 'ComponentOverview'],
    ['Shared/ComponentOverview', 'CarbonAd'],
    ['Pages/Docs', 'CarbonAd'],
  ]) {
    const razor = readFileSync(new URL(`../../../site/AntDesign.Docs/${file}.razor`, import.meta.url), 'utf8');
    assert.match(razor, new RegExp(`<${component}\\b`));
    assert.doesNotMatch(razor, /cdn\.carbonads\.com|_carbonads_js/);
  }
});
