import test from 'node:test';
import assert from 'node:assert/strict';
import { readFile } from 'node:fs/promises';
import {
  compareComputedStyles,
  compareDomSnapshots,
  compareRawHtml,
  normalizeRuntimeHtml,
} from '../src/dom-style-parity.mjs';
import { getFinalHoverAction, getPortalWaitTargets, validateParityManifest } from '../src/dom-style-parity-runner.mjs';
import { setTooltipTriggerState } from '../../../components/wwwroot/js/tooltipTriggerHelper.js';
import { getSemanticCatalogCoverage } from '../src/semantic-catalog-coverage.mjs';

test('validates the Select parity manifest before launching either browser', async () => {
  const source = await readFile(new URL('../examples/select-dom-style-parity.json', import.meta.url), 'utf8');
  const manifest = JSON.parse(source);
  assert.equal(validateParityManifest(manifest), manifest);

  const invalid = structuredClone(manifest);
  delete invalid.groups[0].blazor.rootSelector;
  assert.throws(() => validateParityManifest(invalid), /blazor requires scopeSelector and rootSelector/);

  const invalidSemantic = structuredClone(manifest);
  delete invalidSemantic.groups[0].semanticNodes[1].blazorSelector;
  assert.throws(() => validateParityManifest(invalidSemantic), /content requires a blazor selector/);
});

test('keeps React and Blazor Semantic DOM selectors and required counts explicit', async () => {
  const source = await readFile(new URL('../examples/select-dom-style-parity.json', import.meta.url), 'utf8');
  const manifest = JSON.parse(source);
  const content = manifest.groups[0].semanticNodes.find(({ path }) => path === 'content');
  assert.equal(content.reactSelector, '.ant-select-content');
  assert.equal(content.blazorSelector, '.ant-select-content');
  assert.equal(content.minimumCount, 3);
  assert.equal(manifest.groups[0].matchScopeWidth, 'react');
});

test('validates portal Semantic DOM selectors and explicit visibility wait actions', () => {
  const manifest = {
    schemaVersion: 1,
    groups: [{
      id: 'tooltip-basic',
      react: { url: 'http://react.test', scopeSelector: 'body', rootSelector: 'span' },
      blazor: { url: 'http://blazor.test', scopeSelector: 'body', rootSelector: 'span' },
      semanticNodes: [{
        path: 'tooltip',
        reactSelector: '.ant-tooltip',
        blazorSelector: '.ant-tooltip',
        portal: true,
      }],
      scenarios: [{
        id: 'hover',
        actions: [
          { type: 'hover', reactSelector: '#react-trigger', blazorSelector: '#blazor-trigger' },
          { type: 'waitForSelector', reactSelector: '.ant-tooltip:not(.ant-tooltip-hidden)', blazorSelector: '.ant-tooltip:not(.ant-tooltip-hidden)' },
        ],
      }],
    }],
  };
  assert.equal(validateParityManifest(manifest), manifest);
  assert.equal(getFinalHoverAction(manifest.groups[0].scenarios[0].actions).type, 'hover');
  assert.equal(getFinalHoverAction([{ type: 'hover' }, { type: 'click' }]), null);
  assert.equal(getFinalHoverAction([{ type: 'hover' }, { type: 'waitForSelector' }]).type, 'hover');
});

test('Tooltip pilot keeps portal capture scoped to its visible matching overlay', async () => {
  const source = await readFile(new URL('../examples/tooltip-basic-portal-parity.json', import.meta.url), 'utf8');
  const manifest = JSON.parse(source);
  assert.equal(validateParityManifest(manifest), manifest);
  assert.ok(manifest.groups[0].semanticNodes.filter(({ portal }) => portal).every(({ portalText, portalVisible }) => portalText === 'prompt text' && portalVisible));
  assert.ok(manifest.groups[0].scenarios[1].actions.some(({ type }) => type === 'waitForSelector'));
  assert.deepEqual(getPortalWaitTargets(manifest.groups[0].semanticNodes, 'react'), [
    { selector: '.ant-tooltip-container', text: 'prompt text' },
  ]);
  const demo = await readFile(new URL('../../../site/AntDesign.Docs/Demos/Components/Tooltip/demo/Basic.razor', import.meta.url), 'utf8');
  assert.match(demo, /<Unbound>\s*<span\s+@ref="@context\.Current">Tooltip will show on mouse enter\.<\/span>\s*<\/Unbound>/);
});

test('Tooltip portal copies the active trigger font when opened outside its scoped docs container', () => {
  const previousWindow = globalThis.window;
  const previousDocument = globalThis.document;
  const arrow = { style: {} };
  const variables = new Map();
  const popup = {
    style: { setProperty: (name, value) => variables.set(name, value) },
    classList: ['ant-tooltip-placement-top'],
    getBoundingClientRect: () => ({ left: 100, top: 200, width: 92, height: 34 }),
    querySelector: () => arrow,
  };
  globalThis.window = { getComputedStyle: () => ({ fontFamily: 'AlibabaSans, sans-serif' }) };
  globalThis.document = { getElementById: () => ({ closest: () => popup }) };
  const attributes = new Map();
  const trigger = {
    classList: { add() {}, remove() {} },
    getBoundingClientRect: () => ({ left: 150, top: 240, width: 20, height: 10 }),
    getAttribute: (name) => attributes.get(name) ?? null,
    setAttribute: (name, value) => attributes.set(name, value),
    removeAttribute: (name) => attributes.delete(name),
  };

  try {
    setTooltipTriggerState(trigger, 'ant-tooltip-open', 'tip-id', true, false);
    assert.equal(popup.style.fontFamily, 'AlibabaSans, sans-serif');
    assert.equal(arrow.style.left, '46px');
    assert.equal(variables.get('--arrow-x'), '46px');
    assert.equal(variables.get('--arrow-y'), '40px');
  } finally {
    globalThis.window = previousWindow;
    globalThis.document = previousDocument;
  }
});

test('Select preview catalog maps to actual Blazor v6 Semantic DOM classes', async () => {
  const catalog = JSON.parse(await readFile(new URL('../../../docs/antd-v6/semantic-dom-catalog.json', import.meta.url), 'utf8'));
  const selectNodes = catalog.components.find(({ name }) => name === 'select').nodes;
  for (const [name, selector] of [
    ['content', '.ant-select-content'],
    ['item', '.ant-select-selection-item'],
    ['itemContent', '.ant-select-selection-item-content'],
    ['itemRemove', '.ant-select-selection-item-remove'],
  ]) assert.ok(selectNodes.some((node) => node.name === name && node.selector.includes(selector)));

  const cardNodes = catalog.components.find(({ name }) => name === 'card').nodes;
  assert.equal(cardNodes.find(({ name }) => name === 'root').selector, '.ant-card');
  assert.equal(cardNodes.find(({ name }) => name === 'root').description, '卡片根元素，包含位置定位、背景色、边框、圆角、阴影、内边距等卡片容器的基础样式');
  const cardMetaNodes = catalog.components.find(({ name }) => name === 'card-meta').nodes;
  assert.equal(cardMetaNodes.find(({ name }) => name === 'root').selector, '.ant-card-meta');
  assert.equal(cardMetaNodes.find(({ name }) => name === 'title').selector, '.ant-card-meta-title');
});

test('React Semantic DOM documentation is the catalog target and nested type/Dumi paths normalize correctly', async () => {
  const inventory = JSON.parse(await readFile(new URL('../../../docs/antd-v6/semantic-dom-inventory.json', import.meta.url), 'utf8'));
  const catalog = JSON.parse(await readFile(new URL('../../../docs/antd-v6/semantic-dom-catalog.json', import.meta.url), 'utf8'));
  const semanticMd = JSON.parse(await readFile(new URL('../../../docs/antd-v6/semantic-md-catalog.json', import.meta.url), 'utf8'));
  const coverage = getSemanticCatalogCoverage(inventory, catalog, semanticMd);

  assert.ok(inventory.entries.find(({ component }) => component === 'table').dumiSemanticSources[0].semanticPaths.includes('body.cell'));
  assert.ok(!inventory.entries.find(({ component }) => component === 'table').dumiSemanticSources[0].semanticPaths.includes('Jim Green'));
  assert.deepEqual(coverage.catalogComponents, { total: 61, migrated: 61 });
  assert.deepEqual(coverage.definitions, { total: 70, migrated: 70 });
  assert.deepEqual(coverage.apiSlots, { total: 423, migrated: 423 });
  assert.deepEqual(coverage.dumiPaths, { total: 352, migrated: 352 });
  assert.deepEqual(coverage.documentedComponents, { total: 62, migrated: 62 });
  assert.deepEqual(coverage.documentedSlots, { total: 423, migrated: 423 });
  const treeSelectPopup = inventory.entries.find(({ semanticType }) => semanticType === 'TreeSelectPopupSemanticType');
  assert.ok(treeSelectPopup.classNames.includes('popup.itemSwitcher'));
  assert.ok(!treeSelectPopup.classNames.includes('itemSwitcher'));
});

test('DatePicker Dumi popup.root aliases the semantic.md popup node and its selector', async () => {
  const inventory = JSON.parse(await readFile(new URL('../../../docs/antd-v6/semantic-dom-inventory.json', import.meta.url), 'utf8'));
  const catalog = JSON.parse(await readFile(new URL('../../../docs/antd-v6/semantic-dom-catalog.json', import.meta.url), 'utf8'));
  const datePicker = catalog.components.find(({ name }) => name === 'date-picker');
  const popup = datePicker.nodes.find(({ name }) => name === 'popup');
  const demoPath = inventory.entries.find(({ component }) => component === 'date-picker').dumiSemanticSources[0].semanticPaths;
  assert.ok(demoPath.includes('popup.root'));
  assert.equal(popup.selector, '.ant-picker-dropdown');
  assert.equal(getSemanticCatalogCoverage(inventory, catalog).dumiPaths.migrated, 352);
});

test('generated Semantic DOM catalog includes nested React Dumi preview paths with v6 selectors', async () => {
  const catalog = JSON.parse(await readFile(new URL('../../../docs/antd-v6/semantic-dom-catalog.json', import.meta.url), 'utf8'));
  const css = await readFile(new URL('../../../components/wwwroot/css/antd-v6/light.css', import.meta.url), 'utf8');
  for (const [component, semanticPath, selector] of [
    ['date-picker', 'popup', '.ant-picker-dropdown'],
    ['image', 'popup.actions', '.ant-image-preview-actions'],
    ['input', 'button.content', '.ant-btn > span:not(.ant-btn-icon)'],
    ['table', 'body.cell', '.ant-table-tbody > tr > .ant-table-cell'],
    ['timeline', 'itemContent', '.ant-timeline-item-content'],
    ['transfer', 'source.header', '.ant-transfer > .ant-transfer-section:first-of-type .ant-transfer-list-header'],
  ]) {
    const node = catalog.components.find((entry) => entry.name === component)?.nodes.find((entry) => entry.name === semanticPath);
    assert.equal(node?.selector, selector, `${component}/${semanticPath} selector mapping is missing`);
    for (const className of selector.match(/\.ant-[\w-]+/g) ?? []) assert.ok(css.includes(className), `v6 CSS does not contain ${className}`);
  }
});

test('API-only Semantic DOM paths remain catalogued with explicit React selectors', async () => {
  const catalog = JSON.parse(await readFile(new URL('../../../docs/antd-v6/semantic-dom-catalog.json', import.meta.url), 'utf8'));
  for (const [component, semanticPath, selector] of [
    ['drawer', 'content', '.ant-drawer-section'],
    ['drawer', 'wrapper', '.ant-drawer-content-wrapper'],
    ['image', 'placeholder.progress.root', '.ant-image-progress-wrapper'],
    ['image', 'placeholder.progress.content', '.ant-image-progress-content'],
    ['image', 'placeholder.progress.rail', '.ant-image-progress-rail'],
    ['image', 'placeholder.progress.indicator', '.ant-image-progress-indicator'],
    ['menu', 'popup.root', '.ant-menu-submenu-popup'],
    ['spin', 'mask', '.ant-spin.ant-spin-fullscreen'],
    ['spin', 'tip', '.ant-spin-description'],
    ['splitter', 'dragger.default', '.ant-splitter-bar-dragger:not(.ant-splitter-bar-dragger-active)'],
    ['splitter', 'dragger.active', '.ant-splitter-bar-dragger.ant-splitter-bar-dragger-active'],
    ['menu', 'popup', '.ant-menu-submenu-popup'],
    ['menu', 'popup.root', '.ant-menu-submenu-popup'],
    ['menu', 'subMenu.item', '.ant-menu-item'],
    ['menu', 'subMenu.itemTitle', '.ant-menu-item-group-title'],
    ['menu', 'subMenu.list', '.ant-menu-item-group-list'],
    ['date-picker', 'popup.root', '.ant-picker-dropdown'],
    ['time-picker', 'popup.root', '.ant-picker-dropdown'],
  ]) {
    const node = catalog.components.find((entry) => entry.name === component)?.nodes.find((entry) => entry.name === semanticPath);
    assert.equal(node?.selector, selector, `${component}/${semanticPath} should have its explicit selector`);
  }
});

test('compiled v6 Select CSS styles the migrated trigger, tags, and clear button slots', async () => {
  const css = await readFile(new URL('../../../components/wwwroot/css/antd-v6/light.css', import.meta.url), 'utf8');

  for (const selector of [
    '.ant-select-css-var{',
    '.ant-select-content',
    '.ant-select-content-item-suffix',
    '.ant-select-content-item-prefix',
    '.ant-select-selection-item-content',
    '.ant-select-selection-item-remove',
    '.ant-select-clear{',
  ]) {
    assert.ok(css.includes(selector), `compiled antd v6 CSS is missing ${selector}`);
  }

  assert.match(css, /\.ant-select-clear\{[^}]*appearance:none/);
  assert.ok(css.includes('.ant-select-multiple{'));
  assert.ok(css.includes('--ant-select-multi-item-height'));
});

test('documentation demos inherit the active Ant Design v6 text token instead of a v5 color literal', async () => {
  const source = await readFile(new URL('../../../site/AntDesign.Docs/Shared/CodeBox.razor', import.meta.url), 'utf8');
  const demoRule = source.match(/\["& \.code-box-demo"\]\s*=\s*new System\.Collections\.Generic\.Dictionary<string, object>\s*\{([^}]*)\}/)?.[1] ?? '';
  assert.match(demoRule, /\["color"\]\s*=\s*"var\(--ant-color-text\)"/);
  assert.doesNotMatch(demoRule, /rgba\(0,\s*0,\s*0,\s*0\.85\)/);
});

test('normalizes only declared runtime ids while preserving id relationships', () => {
  const html = '<div aria-controls="ant-blazor-a1_list" id="ant-blazor-a1"><input id="ant-blazor-a1_list"></div>';
  assert.equal(normalizeRuntimeHtml(html, {
    runtimeIdPattern: '^ant-blazor-[a-z0-9_-]+$',
    runtimeIdReferenceAttributes: ['aria-controls'],
    removeAttributes: ['_bl_'],
  }), '<div aria-controls="__runtime-id-2" id="__runtime-id-1"><input id="__runtime-id-2"></div>');
});

test('removes only explicitly declared framework comment markers', () => {
  const html = '<div><!--!--><span><!--user comment--></span></div>';
  assert.equal(normalizeRuntimeHtml(html, { removeCommentMarkers: ['<!--!-->'] }),
    '<div><span><!--user comment--></span></div>');
  assert.equal(normalizeRuntimeHtml(html), html);
});

test('sorts CSS class tokens only when explicitly requested by the comparison policy', () => {
  const html = '<div class="ant-component is-active marker"></div>';
  assert.equal(normalizeRuntimeHtml(html, { sortClassTokens: true }),
    '<div class="ant-component is-active marker"></div>');
  assert.equal(normalizeRuntimeHtml('<div class="marker ant-component is-active"></div>', { sortClassTokens: true }),
    '<div class="ant-component is-active marker"></div>');
  assert.equal(normalizeRuntimeHtml('<div class="marker ant-component is-active"></div>'),
    '<div class="marker ant-component is-active"></div>');
});

test('drops an empty class attribute when all its tokens are declared generated classes', () => {
  assert.equal(normalizeRuntimeHtml('<div class="acss-1obg1u7"></div>', {
    removeClassPatterns: ['^acss-[a-zA-Z0-9]+$'],
  }), '<div></div>');
});

test('reports first raw HTML byte mismatch instead of silently whitespace-normalizing', () => {
  const result = compareRawHtml('<span>one</span>', '<span>one </span>');
  assert.equal(result.equal, false);
  assert.equal(result.firstDifference.offset, 9);
  assert.equal(result.firstDifference.expected, '<');
  assert.equal(result.firstDifference.actual, ' ');
});

test('compares computed styles per semantic path with exact values', () => {
  const result = compareComputedStyles(
    { root: { height: '32px', lineHeight: '22px' } },
    { root: { height: '33px', lineHeight: '22px' } },
  );
  assert.equal(result.equal, false);
  assert.deepEqual(result.differences, [{ path: 'root', property: 'height', expected: '32px', actual: '33px' }]);
});

test('DOM report keeps raw equality and parsed structure equality as separate gates', () => {
  const result = compareDomSnapshots(
    { html: '<button class="ant-btn" type="button">Save</button>', semantic: {} },
    { html: '<button type="button" class="ant-btn" id="__runtime-id-1">Save</button>', semantic: {} },
    { ignoredAttributes: ['id'] },
  );
  assert.equal(result.htmlExact.equal, false);
  assert.equal(result.domTree.equal, true);
});

test('semantic nodes compare by parsed DOM structure while retaining raw HTML details', () => {
  const result = compareDomSnapshots(
    {
      html: '<button class="ant-btn">Save</button>',
      semantic: { root: [{ html: '<button class="ant-btn">Save</button>', computedStyle: {} }] },
    },
    {
      html: '<button class="ant-btn">\n  Save\n</button>',
      semantic: { root: [{ html: '<button class="ant-btn">\n  Save\n</button>', computedStyle: {} }] },
    },
  );
  assert.equal(result.htmlExact.equal, false);
  assert.equal(result.semantic[0].domTree.equal, true);
  assert.equal(result.semantic[0].equal, true);
});

test('compares portal geometry relative to its trigger within the declared pixel tolerance', () => {
  const expectedNode = { html: '<div class="ant-tooltip"></div>', computedStyle: {}, relativeGeometry: { left: 64, top: -45, width: 93, height: 34 } };
  const actualNode = { ...expectedNode, relativeGeometry: { left: 64.5, top: -44, width: 93.4, height: 34 } };
  const snapshot = (node) => ({ html: '<span>trigger</span>', semantic: { tooltip: [node] } });

  assert.equal(compareDomSnapshots(snapshot(expectedNode), snapshot(actualNode), { geometryTolerancePx: 1.5 }).equal, true);
  const mismatch = compareDomSnapshots(snapshot(expectedNode), snapshot({ ...actualNode, relativeGeometry: { ...actualNode.relativeGeometry, top: -41 } }), { geometryTolerancePx: 1.5 });
  assert.equal(mismatch.equal, false);
  assert.equal(mismatch.semantic[0].geometry.differences[0].property, 'top');
});

test('ignores only declared popup inline-position attributes while retaining raw markup diagnostics', () => {
  const snapshot = (style) => ({
    html: '<span>trigger</span>',
    semantic: { tooltip: [{ html: `<div class="ant-tooltip" style="${style}"><div>tip</div></div>`, computedStyle: {} }] },
  });
  const comparison = compareDomSnapshots(snapshot('left: 10px; top: 20px'), snapshot('left: 30px; top: 40px'), {
    ignoredSemanticAttributes: { tooltip: ['style'] },
  });
  assert.equal(comparison.equal, true);
  assert.equal(comparison.semantic[0].html.equal, false);
  assert.equal(comparison.semantic[0].domTree.equal, true);
});

test('Semantic DOM requirements fail when a declared path is absent', () => {
  const result = compareDomSnapshots(
    { html: '<div></div>', semantic: { input: [] } },
    { html: '<div></div>', semantic: { input: [] } },
    { semanticMinimumCounts: { input: 1 } },
  );
  assert.equal(result.semantic[0].equal, false);
  assert.equal(result.semantic[0].requiredCount, 1);
  assert.equal(result.visualStyles.equal, false);
});

test('reports visual CSS parity independently from renderer-specific DOM differences', () => {
  const expected = {
    html: '<div class="ant-component"><span>React</span></div>',
    computedStyles: { 'root[0]': { width: '180px', borderWidth: '1px' } },
    semantic: { root: [{ html: '<div class="ant-component"></div>', computedStyle: { width: '180px' } }] },
  };
  const actual = {
    html: '<div class="ant-component"><label>Blazor</label></div>',
    computedStyles: { 'root[0]': { width: '180px', borderWidth: '1px' } },
    semantic: { root: [{ html: '<div class="ant-component"></div>', computedStyle: { width: '180px' } }] },
  };

  const result = compareDomSnapshots(expected, actual, { semanticMinimumCounts: { root: 1 } });
  assert.equal(result.equal, false);
  assert.equal(result.visualStyles.equal, true);
  assert.equal(result.domTree.equal, false);
});
