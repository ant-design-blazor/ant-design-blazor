import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import test from 'node:test';

const source = readFileSync(new URL('../../../components/wwwroot/AntDesign.lib.module.js', import.meta.url), 'utf8');

test('documentation styles use content versions so rebuilt v6 fixes are not cached under an unchanged URL', () => {
  const host = readFileSync(new URL('../../../site/AntDesign.Docs.Server/Pages/_Layout.cshtml', import.meta.url), 'utf8');
  for (const file of ['docs-layout.css', 'dumi-docs.css']) {
    const line = host.split('\n').find(line => line.includes(`AntDesign.Docs/css/${file}`));
    assert.match(line, /asp-append-version="true"/);
  }
});

test('initializer recognizes explicit script URLs including content hashes and subpath hosting', async () => {
  const saved = globalThis.document;
  try {
    for (const [index, src] of [
      '_content/AntDesign/js/ant-design-blazor.js',
      '/_content/AntDesign/js/ant-design-blazor.js?v=content-hash',
      'https://example.com/app/_content/AntDesign/js/ant-design-blazor.js?v=content-hash#main',
      null,
    ].entries()) {
      const inserted = [];
      globalThis.document = {
        baseURI: 'https://example.com/app/',
        querySelectorAll: () => src ? [{ getAttribute: () => src }] : [],
        querySelector: selector => selector.startsWith('[href') ? {} : null,
        createElement: tag => ({ tag, setAttribute(key, value) { this[key] = value; } }),
        body: { appendChild: node => inserted.push(node) }, head: { appendChild: node => inserted.push(node) },
      };
      const module = await import(`data:text/javascript;base64,${Buffer.from(source).toString('base64')}#${index}`);
      module.beforeStart();
      module.beforeStart();
      assert.equal(inserted.length, src ? 0 : 1, src ?? 'automatically loads once without an explicit reference');
    }
  } finally {
    if (saved === undefined) delete globalThis.document; else globalThis.document = saved;
  }
});
