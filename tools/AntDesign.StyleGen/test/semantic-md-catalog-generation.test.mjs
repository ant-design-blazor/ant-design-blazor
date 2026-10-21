import assert from 'node:assert/strict';
import { mkdtemp, readFile, rm, writeFile } from 'node:fs/promises';
import os from 'node:os';
import path from 'node:path';
import { spawnSync } from 'node:child_process';
import test from 'node:test';

const generator = new URL('../src/generate-semantic-dom-catalog.mjs', import.meta.url);

test('Semantic DOM catalog includes React semantic.md nodes, localized descriptions and observed class selectors', async () => {
  const root = await mkdtemp(path.join(os.tmpdir(), 'semantic-md-catalog-'));
  try {
    const inventoryPath = path.join(root, 'inventory.json');
    const existingPath = path.join(root, 'existing.json');
    const outputPath = path.join(root, 'generated.json');
    const semanticMdPath = path.join(root, 'semantic-md.json');
    await writeFile(inventoryPath, JSON.stringify({
      schemaVersion: 1,
      source: { repository: 'ant-design', path: 'components', semanticSource: 'SemanticType' },
      entries: [
        { component: 'list', semanticPaths: ['actions', 'extra', 'legacyWrapper'] },
        { component: 'image', semanticPaths: ['root', 'popup.root'] },
        { component: 'card', semanticPaths: ['actions', 'body', 'cover', 'extra', 'header', 'root', 'title'] },
        { component: 'card-meta', semanticPaths: ['avatar', 'description', 'root', 'section', 'title'] },
        { component: 'spin', semanticPaths: ['container', 'description', 'indicator', 'mask', 'root', 'section', 'tip'] },
      ],
      componentCatalog: [{ name: 'list' }, { name: 'image' }, { name: 'card' }, { name: 'spin' }],
    }));
    await writeFile(semanticMdPath, JSON.stringify({
      source: { repository: 'ant-design', kind: 'generated-dumi-semantic-markdown', docsRoot: '_site' },
      pages: [
      {
        component: 'list', locale: 'en-US', parts: [
          { name: 'actions', description: 'Actions', selectorCandidate: '.ant-list-item-action' },
          { name: 'extra', description: 'Extra content', selectorCandidate: '.ant-list-item-extra' },
        ],
      },
      {
        component: 'list', locale: 'zh-CN', parts: [
          { name: 'actions', description: '操作区域', selectorCandidate: '.ant-list-item-action' },
          { name: 'extra', description: '额外内容', selectorCandidate: '.ant-list-item-extra' },
        ],
      },
      {
        component: 'image', locale: 'en-US', parts: [
          { name: 'root', description: 'Root element', selectorCandidate: '.ant-image' },
          { name: 'popup.root', description: 'Preview root', selectorCandidate: '.ant-image-preview' },
        ],
      },
      {
        component: 'card', locale: 'en-US', parts: [
          { name: 'root', description: 'Card root', selectorCandidate: '.ant-card' },
          { name: 'title', description: 'Card title', selectorCandidate: '.ant-card-head-title' },
        ],
      },
      {
        component: 'card-meta', locale: 'en-US', variant: 'meta', parts: [
          { name: 'root', description: 'Meta root', selectorCandidate: '.ant-card-meta' },
          { name: 'section', description: 'Meta content', selectorCandidate: '.ant-card-meta-section' },
          { name: 'title', description: 'Meta title', selectorCandidate: '.ant-card-meta-title' },
        ],
      },
      {
        component: 'spin', locale: 'en-US', parts: [
          { name: 'root', description: 'Root' },
          { name: 'section', description: 'Section' },
          { name: 'indicator', description: 'Indicator' },
          { name: 'description', description: 'Description' },
          { name: 'container', description: 'Container' },
        ],
      },
      ],
    }));

    const result = spawnSync(process.execPath, [generator.pathname, root, inventoryPath, existingPath, outputPath, semanticMdPath], { encoding: 'utf8' });
    assert.equal(result.status, 0, result.stderr || result.stdout);
    const catalog = JSON.parse(await readFile(outputPath, 'utf8'));
    assert.deepEqual(catalog.source, {
      repository: 'ant-design',
      kind: 'generated-dumi-semantic-markdown',
      docsRoot: '_site',
      apiInventory: 'components',
      apiSemanticSource: 'SemanticType',
    });
    const nodes = catalog.components.find(({ name }) => name === 'list').nodes;
    assert.deepEqual(nodes.map(({ name }) => name), ['actions', 'extra', 'legacyWrapper']);
    assert.equal(nodes[0].selector, '.ant-list-item-action');
    assert.equal(nodes[0].description, '操作区域');
    assert.equal(nodes[0].version, '6.0.0');

    const imageNodes = catalog.components.find(({ name }) => name === 'image').nodes;
    assert.equal(imageNodes.find(({ name }) => name === 'popup.root').selector, '.ant-image-preview');

    const cardNodes = catalog.components.find(({ name }) => name === 'card').nodes;
    assert.equal(cardNodes.find(({ name }) => name === 'root').selector, '.ant-card');
    assert.equal(cardNodes.find(({ name }) => name === 'title').selector, '.ant-card-head-title');
    const cardMetaNodes = catalog.components.find(({ name }) => name === 'card-meta').nodes;
    assert.equal(cardMetaNodes.find(({ name }) => name === 'root').selector, '.ant-card-meta');
    assert.equal(cardMetaNodes.find(({ name }) => name === 'section').selector, '.ant-card-meta-section');
    assert.equal(cardMetaNodes.find(({ name }) => name === 'title').selector, '.ant-card-meta-title');

    const spinNodes = catalog.components.find(({ name }) => name === 'spin').nodes;
    assert.deepEqual(spinNodes.map(({ name }) => name), ['container', 'description', 'indicator', 'mask', 'root', 'section', 'tip']);
  } finally {
    await rm(root, { recursive: true, force: true });
  }
});

test('Drawer dragger selector follows the React v6 resizable DOM class', async () => {
  const root = await mkdtemp(path.join(os.tmpdir(), 'drawer-semantic-selector-'));
  try {
    const inventoryPath = path.join(root, 'inventory.json');
    const semanticMdPath = path.join(root, 'semantic-md.json');
    const outputPath = path.join(root, 'generated.json');
    await writeFile(inventoryPath, JSON.stringify({
      schemaVersion: 1,
      source: { repository: 'ant-design', path: 'components', semanticSource: 'SemanticType' },
      entries: [{ component: 'drawer', classNames: ['dragger'] }],
      componentCatalog: [{ name: 'drawer' }],
    }));
    await writeFile(semanticMdPath, JSON.stringify({
      source: { repository: 'ant-design', kind: 'generated-dumi-semantic-markdown', docsRoot: '_site' },
      pages: [{
        component: 'drawer',
        locale: 'en-US',
        parts: [{ name: 'dragger', description: 'Resize handle', selectorCandidate: '.ant-drawer-resizable-dragger' }],
      }],
    }));

    const result = spawnSync(process.execPath, [generator.pathname, root, inventoryPath, path.join(root, 'missing.json'), outputPath, semanticMdPath], { encoding: 'utf8' });
    assert.equal(result.status, 0, result.stderr || result.stdout);
    const catalog = JSON.parse(await readFile(outputPath, 'utf8'));
    assert.equal(catalog.components.find(({ name }) => name === 'drawer').nodes[0].selector, '.ant-drawer-resizable-dragger');
    assert.doesNotMatch(await readFile(generator, 'utf8'), /ant-drawer-resize-handle/);
  } finally {
    await rm(root, { recursive: true, force: true });
  }
});

test('Tag Semantic DOM selectors target the Blazor slot wrappers', async () => {
  const root = await mkdtemp(path.join(os.tmpdir(), 'tag-semantic-selectors-'));
  try {
    const inventoryPath = path.join(root, 'inventory.json');
    const outputPath = path.join(root, 'generated.json');
    await writeFile(inventoryPath, JSON.stringify({
      schemaVersion: 1,
      source: { repository: 'ant-design', path: 'components', semanticSource: 'SemanticType' },
      entries: [{ component: 'tag', semanticPaths: ['root', 'icon', 'content', 'close', 'item'] }],
      componentCatalog: [{ name: 'tag' }],
    }));

    const result = spawnSync(process.execPath, [generator.pathname, root, inventoryPath, path.join(root, 'missing.json'), outputPath], { encoding: 'utf8' });
    assert.equal(result.status, 0, result.stderr || result.stdout);
    const nodes = JSON.parse(await readFile(outputPath, 'utf8')).components.find(({ name }) => name === 'tag').nodes;
    assert.equal(nodes.find(({ name }) => name === 'root').selector, '.ant-tag, .ant-tag-checkable-group');
    assert.equal(nodes.find(({ name }) => name === 'icon').selector, '.ant-tag-icon');
    assert.equal(nodes.find(({ name }) => name === 'content').selector, '.ant-tag-content');
    assert.equal(nodes.find(({ name }) => name === 'close').selector, '.ant-tag-close-icon');
    assert.equal(nodes.find(({ name }) => name === 'item').selector, '.ant-tag-checkable-group-item');
  } finally {
    await rm(root, { recursive: true, force: true });
  }
});
