import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import test from 'node:test';
import { JSDOM } from 'jsdom';

const source = readFileSync(new URL('../../../site/AntDesign.Docs/wwwroot/js/mobile-docs-navigation.js', import.meta.url), 'utf8');
const module = await import(`data:text/javascript;base64,${Buffer.from(source).toString('base64')}`);

function fixture(t) {
  const dom = new JSDOM('<body><div id="host"><button id="trigger">Menu</button><nav>Navigation</nav></div><button id="outside">Action</button><a href="#target">Link</a></body>', { url: 'https://docs.example/' });
  const document = dom.window.document;
  const host = document.getElementById('host');
  const calls = [];
  const receiver = { invokeMethodAsync(name) { calls.push(name); return Promise.resolve(); } };
  const click = target => {
    const event = new dom.window.MouseEvent('click', { bubbles: true, cancelable: true, composed: true });
    target.dispatchEvent(event);
    return event;
  };
  t.after(() => { module.unmount(host); dom.window.close(); });
  return { dom, document, host, calls, receiver, click };
}

test('outside click closes once and reaches the original target without cancellation or replay', t => {
  const { document, host, calls, receiver, click } = fixture(t);
  let actions = 0;
  document.getElementById('outside').addEventListener('click', event => { actions++; event.stopPropagation(); });
  module.setOpen(host, true, receiver);
  module.setOpen(host, true, receiver);
  const event = click(document.getElementById('outside'));
  assert.deepEqual(calls, ['CloseFromOutside']);
  assert.equal(event.defaultPrevented, false);
  assert.equal(actions, 1);
  click(document.getElementById('outside'));
  assert.equal(actions, 2);
  assert.equal(calls.length, 1);
});

test('inside navigation and trigger clicks are not outside clicks', t => {
  const { document, host, calls, receiver, click } = fixture(t);
  module.setOpen(host, true, receiver);
  click(document.querySelector('nav'));
  click(document.getElementById('trigger'));
  assert.deepEqual(calls, []);
  click(document.getElementById('outside'));
  assert.deepEqual(calls, ['CloseFromOutside']);
});

test('outside native link default navigation is preserved', async t => {
  const { dom, document, host, calls, receiver, click } = fixture(t);
  module.setOpen(host, true, receiver);
  assert.equal(click(document.querySelector('a')).defaultPrevented, false);
  await new Promise(resolve => dom.window.setTimeout(resolve, 10));
  assert.equal(dom.window.location.hash, '#target');
  assert.deepEqual(calls, ['CloseFromOutside']);
});

test('closing and disposal remove listeners; reopening registers one listener', t => {
  const { document, host, calls, receiver, click } = fixture(t);
  module.setOpen(host, true, receiver);
  module.setOpen(host, false, receiver);
  click(document.getElementById('outside'));
  assert.deepEqual(calls, []);
  module.setOpen(host, true, receiver);
  module.unmount(host);
  module.unmount(host);
  click(document.getElementById('outside'));
  assert.deepEqual(calls, []);
  module.setOpen(host, true, receiver);
  click(document.getElementById('outside'));
  assert.deepEqual(calls, ['CloseFromOutside']);
});

test('disconnected hosts are ignored and a stale receiver cannot retain listeners', async t => {
  const { document, host, calls, receiver, click } = fixture(t);
  module.setOpen(null, true, receiver);
  module.setOpen(document.createElement('div'), true, receiver);
  module.setOpen(host, true, { invokeMethodAsync() { return Promise.reject(new Error('disposed')); } });
  click(document.getElementById('outside'));
  await Promise.resolve();
  module.setOpen(host, true, receiver);
  click(document.getElementById('outside'));
  assert.deepEqual(calls, ['CloseFromOutside']);
});
