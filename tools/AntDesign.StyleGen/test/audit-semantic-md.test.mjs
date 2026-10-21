import assert from 'node:assert/strict';
import { mkdtemp, mkdir, readFile, rm, writeFile } from 'node:fs/promises';
import os from 'node:os';
import path from 'node:path';
import { spawnSync } from 'node:child_process';
import test from 'node:test';

const script = new URL('../src/audit-semantic-md.mjs', import.meta.url);

test('semantic markdown audit keeps demo marker classes separate and compares documented nodes', async () => {
  const root = await mkdtemp(path.join(os.tmpdir(), 'semantic-md-audit-'));
  try {
    const docs = path.join(root, 'react-site');
    const componentDocs = path.join(docs, 'components', 'image');
    const inventoryPath = path.join(root, 'inventory.json');
    const outputPath = path.join(root, 'catalog.json');
    await mkdir(componentDocs, { recursive: true });
    await writeFile(inventoryPath, JSON.stringify({ entries: [{
      component: 'image',
      semanticPaths: ['root', 'popup'],
      dumiSemanticSources: [{ semanticPaths: ['root', 'popup.root'] }],
    }] }));
    await writeFile(path.join(componentDocs, 'semantic.md'), [
      '## Image',
      '',
      '### Semantic Parts',
      '',
      '- root（`semantic-mark-root`）: Root element',
      '- popup.root（`semantic-mark-popup-root`）: Preview root',
      '',
      '### Abstract DOM Structure',
      '',
      '```html',
      '<div class="ant-image semantic-mark-root"><div class="ant-image-preview semantic-mark-popup-root"></div></div>',
      '```',
    ].join('\n'));

    const result = spawnSync(process.execPath, [script.pathname, root, inventoryPath, docs, outputPath], { encoding: 'utf8' });
    assert.equal(result.status, 0, result.stderr || result.stdout);
    const catalog = JSON.parse(await readFile(outputPath, 'utf8'));
    assert.equal(catalog.scope.componentsWithDifferences, 0);
    assert.deepEqual(catalog.pages[0].parts[0], {
      name: 'root',
      demoMarkerClass: 'semantic-mark-root',
      description: 'Root element',
      reactTagNames: ['div'],
      reactClassNames: ['ant-image'],
      selectorCandidate: '.ant-image',
    });
    const snapshot = catalog.abstractDomSnapshots[catalog.pages[0].abstractDomSnapshot];
    assert.deepEqual(snapshot.markers[1].parentDemoMarkerClasses, ['semantic-mark-root']);
    assert.equal(snapshot.tree[0].classNames[0], 'ant-image');
    assert.equal(snapshot.tree[0].children[0].demoMarkerClasses[0], 'semantic-mark-popup-root');
  } finally {
    await rm(root, { recursive: true, force: true });
  }
});

test('semantic markdown audit fails and reports a node absent from the API inventory', async () => {
  const root = await mkdtemp(path.join(os.tmpdir(), 'semantic-md-audit-'));
  try {
    const docs = path.join(root, 'react-site');
    const componentDocs = path.join(docs, 'components', 'image');
    const inventoryPath = path.join(root, 'inventory.json');
    const outputPath = path.join(root, 'catalog.json');
    await mkdir(componentDocs, { recursive: true });
    await writeFile(inventoryPath, JSON.stringify({ entries: [{ component: 'image', dumiSemanticSources: [] }] }));
    await writeFile(path.join(componentDocs, 'semantic.md'), '- root（`semantic-mark-root`）: Root element\n');

    const result = spawnSync(process.execPath, [script.pathname, root, inventoryPath, docs, outputPath], { encoding: 'utf8' });
    assert.equal(result.status, 1);
    const catalog = JSON.parse(await readFile(outputPath, 'utf8'));
    assert.deepEqual(catalog.comparison[0].documentedButMissingFromApi, ['root']);
  } finally {
    await rm(root, { recursive: true, force: true });
  }
});
