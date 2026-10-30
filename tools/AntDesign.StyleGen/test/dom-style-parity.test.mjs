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
import { captureDomStyles } from '../src/dom-style-capture.mjs';
import { setTooltipTriggerState } from '../../../components/wwwroot/js/tooltipTriggerHelper.js';
import { getSemanticCatalogCoverage } from '../src/semantic-catalog-coverage.mjs';
import { JSDOM } from 'jsdom';

test('Select virtual gate retains full popup DOM checks alongside its independent accessibility list', async () => {
  const manifest = JSON.parse(await readFile(new URL('../examples/select-virtual-dom-style-parity.json', import.meta.url), 'utf8'));
  validateParityManifest(manifest);
  assert.equal(manifest.config.comparisonPolicy.ignoredAttributes, undefined);
  assert.equal(manifest.config.comparisonPolicy.geometryTolerancePx, undefined);
  assert.deepEqual(manifest.groups[0].semanticNodes.map(node => node.path), ['item', 'item.disabled', 'itemRemove', 'popup.root', 'popup.a11y', 'popup.a11yOption']);
  assert.ok(manifest.groups[0].styleProperties.includes('overflow'));
  assert.ok(manifest.groups[0].styleProperties.includes('cursor'));
  assert.equal(manifest.groups[0].semanticNodes.find(node => node.path === 'item.disabled').minimumCount, 1);
  assert.deepEqual(manifest.groups[0].semanticNodes.find(node => node.path === 'popup.root').geometryAnchor,
    { mode: 'pixel-snap-to-trigger', verticalOffset: 4 });
});

test('Collapse gate retains meaningful attributes and requires every React semantic slot', async () => {
  const manifest = JSON.parse(await readFile(new URL('../examples/collapse-dom-style-parity.json', import.meta.url), 'utf8'));
  validateParityManifest(manifest);
  assert.equal(manifest.config.comparisonPolicy.ignoredAttributes, undefined);
  assert.equal(manifest.config.comparisonPolicy.geometryTolerancePx, undefined);
  assert.deepEqual(manifest.config.comparisonPolicy.normalization.removeAttributes, ['_bl_*', 'data-collapse-*']);
  const basic = manifest.groups.find(group => group.id === 'collapse-basic');
  assert.deepEqual(basic.semanticNodes.map(node => node.path), ['root', 'header', 'icon', 'title', 'body']);
  assert.ok(basic.styleProperties.includes('--ant-collapse-header-padding'));
  assert.equal(manifest.groups.find(group => group.id === 'collapse-collapsible').scenarios[0].minimumRoots, 3);
});

test('Select Escape gate settles the open and close motions before capture', async () => {
  const manifest = JSON.parse(await readFile(new URL('../examples/select-basic-popup-dom-style-parity.json', import.meta.url), 'utf8'));
  validateParityManifest(manifest);
  assert.equal(manifest.groups[0].captureReactBeforeBlazor, true);
  assert.deepEqual(manifest.groups.find(group => group.id === 'select-basic-popup').semanticNodes.find(node => node.path === 'popup.root').geometryAnchor,
    { mode: 'pixel-snap-to-trigger', verticalOffset: 4 });
  const scenario = manifest.groups[0].scenarios.find(scenario => scenario.id === 'escape-closed');
  assert.equal(scenario.actions[1].type, 'waitForSelector');
  assert.match(scenario.actions[1].blazorSelector, /aria-expanded='true'/);
  assert.deepEqual(scenario.actions[2], { type: 'wait', milliseconds: 300 });
  assert.equal(scenario.actions[3].type, 'press');
  assert.equal(scenario.actions[3].key, 'Escape');
  assert.deepEqual(scenario.actions[4], { type: 'wait', milliseconds: 300 });
});

test('Select semantic-style gate compares popup CSS and pixel geometry while ignoring renderer-specific inline positioning serialization', async () => {
  const manifest = JSON.parse(await readFile(new URL('../examples/select-semantic-style-dom-style-parity.json', import.meta.url), 'utf8'));
  validateParityManifest(manifest);
  assert.equal(manifest.config.comparisonPolicy.ignoredAttributes, undefined);
  assert.deepEqual(manifest.config.comparisonPolicy.ignoredSemanticAttributes['popup.root'], ['style']);
  assert.equal(manifest.config.comparisonPolicy.geometryTolerancePx, undefined);
  const [closed, opened] = manifest.groups;
  assert.ok(closed.styleProperties.includes('left'));
  assert.ok(closed.styleProperties.includes('transform'));
  assert.equal(closed.semanticNodes.find(node => node.path === 'input').minimumCount, 2);
  assert.ok(opened.styleProperties.includes('borderColor'));
  assert.match(opened.scenarios[0].actions[0].blazorSelector, /ant-select-filled/);
});

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

test('Icon parity covers full Space trees, SVG geometry, colors and required node counts', async () => {
  const manifest = JSON.parse(await readFile(new URL('../examples/icon-dom-style-parity.json', import.meta.url), 'utf8'));
  validateParityManifest(manifest);
  assert.deepEqual(manifest.groups.map(group => group.id).sort(), ['icon-basic', 'icon-custom', 'icon-font', 'icon-font-resources', 'icon-two-tone']);
  for (const group of manifest.groups) {
    assert.equal(group.react.rootSelector, '.ant-space');
    assert.equal(group.blazor.rootSelector, '.ant-space');
    assert.ok(group.semanticNodes.every(node => node.minimumCount > 0));
    assert.match(group.react.scopeSelector, /#components-icon-demo-/);
  }
  assert.ok(manifest.groups.find(group => group.id === 'icon-two-tone').styleProperties.includes('fill'));
  for (const [id, count] of [['icon-font', 3], ['icon-font-resources', 4]]) {
    assert.equal(manifest.groups.find(group => group.id === id).semanticNodes.find(node => node.path === 'use').minimumCount, count);
  }
  assert.ok(!manifest.config.comparisonPolicy.normalization.removeAttributes.includes('id'), 'static SVG symbol IDs must not be discarded');
  assert.equal(manifest.config.comparisonPolicy.normalization.removeUnreferencedRuntimeIds, true);
  assert.equal(manifest.config.comparisonPolicy.ignoredAttributes, undefined, 'SVG paths and inline styles are not ignored');
});

test('optional runtime ID removal keeps static symbols, ARIA references and SVG fragment relationships', () => {
  const policy = { runtimeIdPattern: '^ant-blazor-[a-z]+$', removeUnreferencedRuntimeIds: true };
  const html = '<div id="ant-blazor-unused"><svg><symbol id="icon-cart"></symbol><use xlink:href="#icon-cart"></use>' +
    '<linearGradient id="ant-blazor-gradient"></linearGradient><path fill="url(#ant-blazor-gradient)"></path></svg>' +
    '<label for="ant-blazor-input"></label><input id="ant-blazor-input" aria-describedby="ant-blazor-note"><span id="ant-blazor-note"></span></div>';
  const normalized = normalizeRuntimeHtml(html, policy);
  assert.ok(!normalized.includes('ant-blazor-unused'));
  assert.match(normalized, /id="icon-cart"/);
  assert.match(normalized, /xlink:href="#icon-cart"/);
  assert.match(normalized, /id="__runtime-id-1"/);
  assert.match(normalized, /fill="url\(#__runtime-id-1\)"/);
  assert.match(normalized, /for="__runtime-id-2"/);
  assert.match(normalized, /id="__runtime-id-2"/);
  assert.match(normalized, /aria-describedby="__runtime-id-3"/);
  assert.match(normalized, /id="__runtime-id-3"/);
  assert.ok(normalizeRuntimeHtml('<div id="ant-blazor-unused"></div>', { runtimeIdPattern: policy.runtimeIdPattern }).includes('id='), 'default normalization must keep ID presence');
});

test('SVG use gates require painted glyphs, matching definitions and the declared subpixel tolerance', () => {
  const snapshot = () => ({ html: '<svg><use href="#cart"></use></svg>', semantic: { use: [{
    html: '<use href="#cart"></use>', computedStyle: {},
    svgUse: { href: '#cart', width: 11.2, height: 11.2, definitions: ['<symbol id="cart"><path d="M0 0"></path></symbol>'] },
  }] } });
  const expected = snapshot();
  const actual = snapshot();
  const policy = { svgUsePaths: ['use'], svgUsePaintTolerancePx: 0.01, semanticMinimumCounts: { use: 1 } };
  actual.semantic.use[0].svgUse.height += 0.0013;
  assert.equal(compareDomSnapshots(expected, actual, policy).visualStyles.equal, true);
  actual.semantic.use[0].svgUse.width += 1;
  assert.equal(compareDomSnapshots(expected, actual, policy).visualStyles.equal, false);
  const unpainted = snapshot();
  unpainted.semantic.use[0].svgUse.width = 0;
  assert.equal(compareDomSnapshots(unpainted, unpainted, policy).visualStyles.equal, false, 'two unloaded glyphs are not a match');
  const missing = snapshot();
  delete missing.semantic.use[0].svgUse;
  assert.equal(compareDomSnapshots(missing, missing, policy).visualStyles.equal, false);
  const missingPath = { ...snapshot(), semantic: {} };
  assert.equal(compareDomSnapshots(missingPath, missingPath, policy).visualStyles.equal, false);
  const different = snapshot();
  different.semantic.use[0].svgUse.definitions = ['<symbol id="cart"><path d="M1 1"></path></symbol>'];
  assert.equal(compareDomSnapshots(expected, different, policy).visualStyles.equal, false);
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
  assert.equal(manifest.groups[0].geometryTolerancePx, 0);
  assert.deepEqual(manifest.groups[0].semanticNodes.find(({ path }) => path === 'tooltip').geometryAnchor,
    { mode: 'pixel-snap-to-trigger' });
  assert.equal(manifest.groups[0].semanticNodes.find(({ path }) => path === 'arrow').geometryParentSelector,
    '.ant-tooltip:not(.ant-tooltip-hidden)');
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

test('DatePicker popup parity captures the blur-closing React overlay before activating Blazor', async () => {
  const manifest = JSON.parse(await readFile(new URL('../examples/datepicker-basic-popup-dom-style-parity.json', import.meta.url), 'utf8'));
  validateParityManifest(manifest);
  const group = manifest.groups.find(({ id }) => id === 'datepicker-basic-popup');
  assert.equal(group.captureReactBeforeBlazor, true);
  assert.equal(group.scenarios[0].actions[0].type, 'click');
  assert.ok(group.semanticNodes.find(({ path }) => path === 'popup').portalVisible);
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

test('v6 theme parity restores the React icon flex box after legacy docs CSS', async () => {
  const css = await readFile(new URL('../../../components/wwwroot/css/antd-v6/component-parity.css', import.meta.url), 'utf8');
  assert.match(css, /\.ant-theme\s+\.anticon\s*\{[^}]*display:\s*inline-flex;[^}]*align-items:\s*center;/s);
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

test('normalizes only CSS declaration separator whitespace when explicitly requested', () => {
  const html = '<div style="width: 120px; color: rgb(0, 0, 0);"></div>';
  assert.equal(normalizeRuntimeHtml(html, { normalizeStyleAttributes: true }),
    '<div style="width:120px;color:rgb(0, 0, 0)"></div>');
  assert.equal(normalizeRuntimeHtml(html), html);
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

test('DOM parity uses parsed structure as the gate and retains raw HTML only as a diagnostic', () => {
  const result = compareDomSnapshots(
    { html: '<button class="ant-btn" type="button">Save</button>', semantic: {} },
    { html: '<button type="button" class="ant-btn" id="__runtime-id-1">Save</button>', semantic: {} },
    { ignoredAttributes: ['id'] },
  );
  assert.equal(result.htmlExact.equal, false);
  assert.equal(result.domTree.equal, true);
  assert.equal(result.equal, true);
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

test('compares popup pixel-snap and child-relative geometry exactly without tolerance', () => {
  const trigger = '<span class="ant-select"></span>';
  const expectedNode = { html: '<div class="ant-select-dropdown"></div>', computedStyle: {},
    relativeGeometry: { left: -0.65625, top: 35.015625, width: 342.65625, height: 264 },
    comparisonGeometry: { left: 0, top: 0, width: 342.65625, height: 264 } };
  const actualNode = { ...expectedNode,
    relativeGeometry: { left: -0.3125, top: 35.265625, width: 342.65625, height: 264 } };
  const snapshot = (node) => ({ html: trigger, semantic: { 'popup.root': [node] } });

  const comparison = compareDomSnapshots(snapshot(expectedNode), snapshot(actualNode));
  assert.equal(comparison.semantic[0].geometry.equal, true);
  assert.deepEqual(comparison.semantic[0].geometry.differences, []);
  assert.notDeepEqual(expectedNode.relativeGeometry, actualNode.relativeGeometry,
    'retain raw per-page trigger-relative coordinates for diagnostics');

  const mismatch = compareDomSnapshots(snapshot(expectedNode), snapshot({
    ...actualNode,
    comparisonGeometry: { ...actualNode.comparisonGeometry, left: 0.25 },
  }));
  assert.equal(mismatch.semantic[0].geometry.equal, false);
  assert.equal(mismatch.semantic[0].geometry.differences[0].property, 'left');
});

test('compares popup viewport coordinates exactly and retains trigger coordinates for diagnosis', () => {
  const reactNode = { html: '<div class="ant-select-dropdown"></div>', computedStyle: {},
    comparisonGeometry: { left: 339, top: 470, right: 459, bottom: 606, width: 120, height: 136 },
    viewportGeometry: { left: 339, top: 470, right: 459, bottom: 606, width: 120, height: 136 },
    triggerViewportGeometry: { left: 339.65625, top: 434.484375, right: 459.65625, bottom: 466.484375, width: 120, height: 32 } };
  const blazorNode = { ...reactNode,
    triggerViewportGeometry: { ...reactNode.triggerViewportGeometry, top: 434.359375, bottom: 466.359375 } };
  const snapshot = node => ({ html: '<span>trigger</span>', semantic: { 'popup.root': [node] } });
  assert.equal(compareDomSnapshots(snapshot(reactNode), snapshot(blazorNode)).semantic[0].geometry.equal, true,
    'a trigger subpixel difference must not be misreported as popup movement');

  const moved = { ...blazorNode, comparisonGeometry: { ...reactNode.comparisonGeometry, top: 470.125 } };
  const mismatch = compareDomSnapshots(snapshot(reactNode), snapshot(moved));
  assert.equal(mismatch.semantic[0].geometry.equal, false);
  assert.deepEqual(mismatch.semantic[0].geometry.differences.map(({ property, expected, actual }) => ({ property, expected, actual })),
    [{ property: 'top', expected: 470, actual: 470.125 }]);
});

test('portal capture selects exact viewport geometry when requested by the manifest', () => {
  const previous = { document: globalThis.document, getComputedStyle: globalThis.getComputedStyle,
    innerWidth: globalThis.innerWidth, innerHeight: globalThis.innerHeight, devicePixelRatio: globalThis.devicePixelRatio };
  const rect = (left, top, width, height) => ({ left, top, right: left + width, bottom: top + height, width, height });
  const trigger = { outerHTML: '<div class="ant-select"></div>', tagName: 'DIV', getBoundingClientRect: () => rect(339.65625, 434.484375, 120, 32), querySelectorAll: () => [] };
  const popup = { outerHTML: '<div class="ant-select-dropdown"></div>', tagName: 'DIV', matches: () => false, closest: () => null, getBoundingClientRect: () => rect(339, 470, 120, 136) };
  globalThis.document = { documentElement: {}, querySelector: selector => selector === '#scope' ? { querySelectorAll: () => [trigger] } : null,
    querySelectorAll: selector => selector === '.popup' ? [popup] : [] };
  globalThis.getComputedStyle = () => ({ getPropertyValue: () => '', zoom: '1' });
  globalThis.innerWidth = 1280; globalThis.innerHeight = 900; globalThis.devicePixelRatio = 1;
  try {
    const result = captureDomStyles({ side: 'react', scopeSelector: '#scope', rootSelector: '.ant-select',
      semanticNodes: [{ path: 'popup.root', selector: '.popup', portal: true, geometryAnchor: { mode: 'viewport' } }] });
    assert.deepEqual(result.semantic['popup.root'][0].comparisonGeometry, result.semantic['popup.root'][0].viewportGeometry);
    assert.deepEqual(result.semantic['popup.root'][0].viewportGeometry, { left: 339, top: 470, right: 459, bottom: 606, width: 120, height: 136 });
    assert.equal(result.semantic['popup.root'][0].triggerViewportGeometry.left, 339.65625);
  } finally {
    for (const [key, value] of Object.entries(previous)) {
      if (value === undefined) delete globalThis[key];
      else globalThis[key] = value;
    }
  }
});

test('capture includes only the requested computed Ant Design token prefixes', () => {
  const previous = { document: globalThis.document, getComputedStyle: globalThis.getComputedStyle,
    innerWidth: globalThis.innerWidth, innerHeight: globalThis.innerHeight, devicePixelRatio: globalThis.devicePixelRatio };
  const tokenEntries = [['--ant-select-color', 'rgb(1, 2, 3)'], ['--ant-primary-color', 'red'], ['--unrelated', 'ignored']];
  const trigger = { outerHTML: '<div class="ant-select"></div>', tagName: 'DIV',
    getBoundingClientRect: () => ({ left: 0, top: 0, right: 120, bottom: 32, width: 120, height: 32 }),
    querySelectorAll: () => [] };
  globalThis.document = { documentElement: {}, querySelector: selector => selector === '#scope' ? { querySelectorAll: () => [trigger] } : null };
  globalThis.getComputedStyle = () => ({ length: tokenEntries.length, item: index => tokenEntries[index][0],
    getPropertyValue: property => tokenEntries.find(([name]) => name === property)?.[1] ?? '', zoom: '1' });
  globalThis.innerWidth = 1280; globalThis.innerHeight = 900; globalThis.devicePixelRatio = 1;
  try {
    const result = captureDomStyles({ side: 'react', scopeSelector: '#scope', rootSelector: '.ant-select',
      styleProperties: ['display'], includeAntDesignTokens: ['--ant-select-'] });
    assert.deepEqual(result.computedStyles['root[0]'], { display: '', '--ant-select-color': 'rgb(1, 2, 3)' });
  } finally {
    for (const [key, value] of Object.entries(previous)) {
      if (value === undefined) delete globalThis[key];
      else globalThis[key] = value;
    }
  }
});

test('visible portal capture ignores offscreen stale picker popups before nearest-trigger matching', () => {
  const previous = { document: globalThis.document, getComputedStyle: globalThis.getComputedStyle,
    innerWidth: globalThis.innerWidth, innerHeight: globalThis.innerHeight, devicePixelRatio: globalThis.devicePixelRatio };
  const rect = (left, top, width, height) => ({ left, top, right: left + width, bottom: top + height, width, height });
  const trigger = { outerHTML: '<div class="ant-picker"></div>', tagName: 'DIV', getBoundingClientRect: () => rect(100, 100, 140, 32), querySelectorAll: () => [] };
  const popup = (name, bounds) => {
    const element = { outerHTML: `<div class="ant-picker-dropdown ${name}"></div>`, tagName: 'DIV',
      matches: selector => selector.includes('.ant-picker-dropdown'),
      closest: selector => selector.includes('.ant-picker-dropdown') ? element : null,
      classList: { contains: () => false }, getBoundingClientRect: () => bounds };
    return element;
  };
  const active = popup('active', rect(100, 136, 288, 348));
  const stale = popup('stale', rect(700, 5000, 288, 348));
  globalThis.document = { documentElement: {}, querySelector: selector => selector === '#scope' ? { querySelectorAll: () => [trigger] } : null,
    querySelectorAll: selector => selector === '.popup' ? [stale, active] : [] };
  globalThis.getComputedStyle = () => ({ display: 'block', visibility: 'visible', opacity: '1', getPropertyValue: () => '', zoom: '1' });
  globalThis.innerWidth = 1280; globalThis.innerHeight = 900; globalThis.devicePixelRatio = 1;
  try {
    const result = captureDomStyles({ side: 'react', scopeSelector: '#scope', rootSelector: '.ant-picker',
      semanticNodes: [{ path: 'popup', selector: '.popup', portal: true, portalMatch: 'nearest-trigger', portalVisible: true,
        geometryAnchor: { mode: 'pixel-snap-to-trigger', verticalOffset: 4 } }] });
    assert.equal(result.semantic.popup.length, 1);
    assert.equal(result.semantic.popup[0].html, active.outerHTML);
    assert.deepEqual(result.semantic.popup[0].comparisonGeometry, { left: 0, top: 0, width: 288, height: 348 });
  } finally {
    for (const [key, value] of Object.entries(previous)) {
      if (value === undefined) delete globalThis[key];
      else globalThis[key] = value;
    }
  }
});

test('portal capture can select the popup nearest to the declared trigger', () => {
  const previous = { document: globalThis.document, getComputedStyle: globalThis.getComputedStyle,
    innerWidth: globalThis.innerWidth, innerHeight: globalThis.innerHeight, devicePixelRatio: globalThis.devicePixelRatio };
  const rect = (left, top, width, height) => ({ left, top, right: left + width, bottom: top + height, width, height });
  const trigger = { outerHTML: '<div class="ant-picker"></div>', tagName: 'DIV', getBoundingClientRect: () => rect(100, 100, 140, 32), querySelectorAll: () => [] };
  const nearby = { outerHTML: '<div class="popup nearby"></div>', tagName: 'DIV', matches: () => false, closest: () => null, getBoundingClientRect: () => rect(100, 136, 288, 348) };
  const unrelated = { outerHTML: '<div class="popup unrelated"></div>', tagName: 'DIV', matches: () => false, closest: () => null, getBoundingClientRect: () => rect(700, 500, 288, 348) };
  globalThis.document = { documentElement: {}, querySelector: selector => selector === '#scope' ? { querySelectorAll: () => [trigger] } : null,
    querySelectorAll: selector => selector === '.popup' ? [unrelated, nearby] : [] };
  globalThis.getComputedStyle = () => ({ getPropertyValue: () => '', zoom: '1' });
  globalThis.innerWidth = 1280; globalThis.innerHeight = 900; globalThis.devicePixelRatio = 1;
  try {
    const result = captureDomStyles({ side: 'react', scopeSelector: '#scope', rootSelector: '.ant-picker',
      semanticNodes: [{ path: 'popup', selector: '.popup', portal: true, portalMatch: 'nearest-trigger', geometryAnchor: { mode: 'pixel-snap-to-trigger' } }] });
    assert.equal(result.semantic.popup.length, 1);
    assert.equal(result.semantic.popup[0].html, nearby.outerHTML);
    assert.deepEqual(result.semantic.popup[0].comparisonGeometry, { left: 0, top: 4, width: 288, height: 348 });
  } finally {
    for (const [key, value] of Object.entries(previous)) {
      if (value === undefined) delete globalThis[key];
      else globalThis[key] = value;
    }
  }
});

test('portal child capture is scoped to the popup selected nearest to its trigger', () => {
  const previous = { document: globalThis.document, getComputedStyle: globalThis.getComputedStyle,
    innerWidth: globalThis.innerWidth, innerHeight: globalThis.innerHeight, devicePixelRatio: globalThis.devicePixelRatio };
  const dom = new JSDOM('<button class="trigger disabled"></button><button class="trigger selected"></button><div class="popup" id="near"><div class="option">near option</div></div><div class="popup" id="far"><div class="option">far option</div></div>');
  const rect = (left, top, width, height) => ({ left, top, right: left + width, bottom: top + height, width, height });
  const disabledTrigger = dom.window.document.querySelector('.disabled');
  const selectedTrigger = dom.window.document.querySelector('.selected');
  const near = dom.window.document.querySelector('#near');
  const far = dom.window.document.querySelector('#far');
  disabledTrigger.getBoundingClientRect = () => rect(700, 500, 40, 24);
  selectedTrigger.getBoundingClientRect = () => rect(100, 100, 40, 24);
  near.getBoundingClientRect = () => rect(100, 128, 120, 80);
  far.getBoundingClientRect = () => rect(700, 500, 120, 80);
  try {
    globalThis.document = dom.window.document;
    globalThis.getComputedStyle = dom.window.getComputedStyle.bind(dom.window);
    globalThis.innerWidth = 1280; globalThis.innerHeight = 900; globalThis.devicePixelRatio = 1;
    const result = captureDomStyles({ side: 'react', scopeSelector: 'body', rootSelector: '.trigger',
      semanticNodes: [
        { path: 'popup.root', selector: '.popup', portal: true, portalMatch: 'nearest-trigger', reactTriggerSelector: '.selected' },
        { path: 'popup.option', selector: '.option', portal: true, portalParentPath: 'popup.root', geometryParentSelector: '.popup' },
      ] });
    assert.equal(result.semantic['popup.root'].length, 1);
    assert.match(result.semantic['popup.root'][0].html, /id="near"/);
    assert.equal(result.semantic['popup.root'][0].triggerViewportGeometry.left, 100);
    assert.equal(result.semantic['popup.option'].length, 1);
    assert.match(result.semantic['popup.option'][0].html, /near option/);
  } finally {
    dom.window.close();
    for (const [key, value] of Object.entries(previous)) {
      if (value === undefined) delete globalThis[key];
      else globalThis[key] = value;
    }
  }
});

test('nested portal geometry resolves its own nearest overlay instead of another page portal', () => {
  const previous = { document: globalThis.document, getComputedStyle: globalThis.getComputedStyle,
    innerWidth: globalThis.innerWidth, innerHeight: globalThis.innerHeight, devicePixelRatio: globalThis.devicePixelRatio };
  const rect = (left, top, width, height) => ({ left, top, right: left + width, bottom: top + height, width, height });
  const trigger = { outerHTML: '<button></button>', tagName: 'BUTTON', getBoundingClientRect: () => rect(100, 100, 40, 24), querySelectorAll: () => [] };
  const nearestOverlay = { getBoundingClientRect: () => rect(404, 394, 93, 34) };
  const unrelatedOverlay = { getBoundingClientRect: () => rect(20, 30, 80, 40) };
  const child = { outerHTML: '<div class="ant-tooltip-arrow"></div>', tagName: 'DIV', matches: () => false,
    closest: selector => selector === '.ant-tooltip' ? nearestOverlay : null,
    getBoundingClientRect: () => rect(410, 400, 6, 6) };
  globalThis.document = { documentElement: {}, querySelector: selector => selector === '#scope' ? { querySelectorAll: () => [trigger] } : unrelatedOverlay,
    querySelectorAll: selector => selector === '.arrow' ? [child] : [] };
  globalThis.getComputedStyle = () => ({ getPropertyValue: () => '', zoom: '1' });
  globalThis.innerWidth = 1280; globalThis.innerHeight = 900; globalThis.devicePixelRatio = 1;
  try {
    const result = captureDomStyles({ side: 'react', scopeSelector: '#scope', rootSelector: 'button',
      semanticNodes: [{ path: 'tooltip.arrow', selector: '.arrow', portal: true, geometryParentSelector: '.ant-tooltip' }] });
    assert.deepEqual(result.semantic['tooltip.arrow'][0].comparisonGeometry, { left: 6, top: 6, width: 6, height: 6 });
  } finally {
    for (const [key, value] of Object.entries(previous)) {
      if (value === undefined) delete globalThis[key];
      else globalThis[key] = value;
    }
  }
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
