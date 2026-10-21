#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';

const repoRoot = process.argv[2] || process.cwd();
const inventoryPath = process.argv[3] || path.join(repoRoot, 'docs/antd-v6/semantic-dom-inventory.json');
const existingPath = process.argv[4] || path.join(repoRoot, 'docs/antd-v6/semantic-dom-catalog.json');
const legacyCatalogPath = path.join(repoRoot, 'site/AntDesign.Docs/Services/SemanticDomGeneratedCatalog.cs');
const legacyManualPath = path.join(repoRoot, 'site/AntDesign.Docs/Services/SemanticDomCatalog.cs');
const outputPath = process.argv[5] || existingPath;

const inventory = JSON.parse(fs.readFileSync(inventoryPath, 'utf8'));
const existingJson = fs.existsSync(existingPath) ? fs.readFileSync(existingPath, 'utf8') : '';
const existingCatalog = fs.existsSync(legacyCatalogPath) ? fs.readFileSync(legacyCatalogPath, 'utf8') : '';
const existingManual = fs.existsSync(legacyManualPath) ? fs.readFileSync(legacyManualPath, 'utf8') : '';
const existing = `${existingCatalog}\n${existingManual}`;
const semanticMdPath = process.argv[6] ? path.resolve(process.argv[6]) : null;
const semanticMdCatalog = semanticMdPath && fs.existsSync(semanticMdPath)
  ? JSON.parse(fs.readFileSync(semanticMdPath, 'utf8'))
  : null;

function kebab(value) {
  return value
    .replace(/([a-z0-9])([A-Z])/g, '$1-$2')
    .replace(/[^a-zA-Z0-9]+/g, '-')
    .replace(/^-|-$/g, '')
    .toLowerCase();
}

function unescapeCSharp(value) {
  return value.replace(/\\"/g, '"').replace(/\\\\/g, '\\');
}

function parseExistingNodes(source) {
  const result = new Map();
  const componentPattern = /\["([^\"]+)"\]\s*=\s*(new\[\]|Array\.Empty<SemanticDomNode>\(\))\s*(?:\{([\s\S]*?)\}|,)/g;
  let componentMatch;
  while ((componentMatch = componentPattern.exec(source))) {
    const component = componentMatch[1];
    const body = componentMatch[3] || '';
    const nodes = new Map();
    const nodePattern = /new SemanticDomNode\("((?:\\.|[^"\\])*)",\s*"((?:\\.|[^"\\])*)",\s*"((?:\\.|[^"\\])*)",\s*"((?:\\.|[^"\\])*)"\)/g;
    let nodeMatch;
    while ((nodeMatch = nodePattern.exec(body))) {
      nodes.set(nodeMatch[1], {
        selector: unescapeCSharp(nodeMatch[2]),
        description: unescapeCSharp(nodeMatch[3]),
        version: unescapeCSharp(nodeMatch[4]),
      });
    }
    result.set(component, nodes);
  }
  return result;
}

function csharp(value) {
  return value.replaceAll('\\', '\\\\').replaceAll('"', '\\"');
}

function fallbackNode(component, semanticPath) {
  const base = `ant-${kebab(component)}`;
  const selector = semanticPath === 'root' ? `.${base}` : `.${base}-${kebab(semanticPath)}`;
  return {
    selector,
    description: `${component} ${semanticPath} Semantic DOM 节点`,
    version: '6.0.0',
  };
}

const semanticPathSelectors = {
  'auto-complete/root': '.ant-select.ant-select-auto-complete',
  'auto-complete/content': '.ant-select-content',
  'auto-complete/input': '.ant-select-selection-search-input',
  'auto-complete/placeholder': '.ant-select-selection-placeholder',
  'auto-complete/prefix': '.ant-select-prefix',
  'auto-complete/suffix': '.ant-select-suffix',
  'auto-complete/clear': '.ant-select-clear',
  'auto-complete/popup': '.ant-select-dropdown',
  'badge/content': '.ant-badge-count',
  'badge/indicator': '.ant-badge-dot, .ant-badge-status-dot',
  'button/root': '.ant-btn',
  'button/content': '.ant-btn > span:not(.ant-btn-icon)',
  'button/icon': '.ant-btn-icon',
  'card/header': '.ant-card-head',
  'card/root': '.ant-card',
  'card/title': '.ant-card-head-title',
  'card/section': '.ant-card-body',
  'card/avatar': '.ant-card-meta-avatar',
  'card/description': '.ant-card-meta-description',
  'card-meta/root': '.ant-card-meta',
  'card-meta/section': '.ant-card-meta-section',
  'card-meta/avatar': '.ant-card-meta-avatar',
  'card-meta/title': '.ant-card-meta-title',
  'card-meta/description': '.ant-card-meta-description',
  'cascader/input': '.ant-select-selection-search-input',
  'cascader/placeholder': '.ant-select-selection-placeholder',
  'checkbox/icon': '.ant-checkbox-inner',
  'checkbox/label': '.ant-checkbox-wrapper',
  'date-picker/root': '.ant-picker',
  'date-picker/popup.root': '.ant-picker-dropdown',
  'date-picker/input': '.ant-picker-input',
  'date-picker/prefix': '.ant-picker-prefix',
  'date-picker/suffix': '.ant-picker-suffix',
  'date-picker/popup': '.ant-picker-dropdown',
  'date-picker/container': '.ant-picker-panel-container',
  'date-picker/header': '.ant-picker-header',
  'date-picker/body': '.ant-picker-body',
  'date-picker/content': '.ant-picker-content',
  'date-picker/item': '.ant-picker-cell',
  'date-picker/footer': '.ant-picker-footer',
  'descriptions/label': '.ant-descriptions-item-label',
  'descriptions/content': '.ant-descriptions-item-content',
  'divider/content': '.ant-divider-inner-text',
  'drawer/dragger': '.ant-drawer-resizable-dragger',
  'drawer/content': '.ant-drawer-section',
  'drawer/wrapper': '.ant-drawer-content-wrapper',
  'dropdown/item': '.ant-dropdown-menu-item',
  'dropdown/itemContent': '.ant-dropdown-menu-title-content',
  'dropdown/itemIcon': '.ant-dropdown-menu-item-icon',
  'dropdown/itemTitle': '.ant-dropdown-menu-title-content',
  'float-button/root': '.ant-float-btn',
  'float-button/list': '.ant-float-btn-group-list',
  'float-button/item': '.ant-float-btn',
  'float-button/itemContent': '.ant-float-btn-content',
  'float-button/itemIcon': '.ant-float-btn-icon',
  'float-button/trigger': '.ant-float-btn-group-trigger',
  'float-button/triggerContent': '.ant-float-btn-group-trigger .ant-float-btn-content',
  'float-button/triggerIcon': '.ant-float-btn-group-trigger .ant-float-btn-icon',
  'image/root': '.ant-image',
  'image/placeholder.progress.root': '.ant-image-progress-wrapper',
  'image/placeholder.progress.content': '.ant-image-progress-content',
  'image/placeholder.progress.rail': '.ant-image-progress-rail',
  'image/placeholder.progress.indicator': '.ant-image-progress-indicator',
  'image/image': '.ant-image-img',
  'image/popup': '.ant-image-preview',
  'image/actions': '.ant-image-preview-actions',
  'image/body': '.ant-image-preview-body',
  'image/close': '.ant-image-preview-close',
  'image/footer': '.ant-image-preview-footer',
  'image/mask': '.ant-image-preview-mask',
  'image/placeholder': '.ant-image-placeholder',
  'image/progress': '.ant-image-progress-wrapper',
  'image/popup.root': '.ant-image-preview',
  'input/button': '.ant-input-search .ant-btn',
  'input/button.root': '.ant-input-search .ant-btn',
  'input/button.content': '.ant-input-search .ant-btn > span:not(.ant-btn-icon)',
  'input/button.icon': '.ant-input-search .ant-btn-icon',
  'mentions/popup': '.ant-select-dropdown',
  'menu/root': '.ant-menu',
  'menu/item': '.ant-menu-item',
  'menu/itemContent': '.ant-menu-title-content',
  'menu/itemIcon': '.ant-menu-item-icon',
  'menu/itemTitle': '.ant-menu-item-group-title',
  'menu/list': '.ant-menu-item-group-list',
  'menu/popup': '.ant-menu-submenu-popup',
  'menu/popup.root': '.ant-menu-submenu-popup',
  'menu/subMenu.item': '.ant-menu-item',
  'menu/subMenu.itemContent': '.ant-menu-title-content',
  'menu/subMenu.itemIcon': '.ant-menu-item-icon',
  'menu/subMenu.itemTitle': '.ant-menu-item-group-title',
  'menu/subMenu.list': '.ant-menu-item-group-list',
  'message/icon': '.ant-message-notice-icon',
  'message/list': '.ant-message-list',
  'message/listContent': '.ant-message-list-content',
  'message/root': '.ant-message-notice',
  'message/title': '.ant-message-notice-title',
  'message/wrapper': '.ant-message-notice-wrapper',
  'modal/wrapper': '.ant-modal-wrap',
  'notification/actions': '.ant-notification-notice-actions',
  'notification/close': '.ant-notification-notice-close',
  'notification/description': '.ant-notification-notice-description',
  'notification/icon': '.ant-notification-notice-icon',
  'notification/progress': '.ant-notification-notice-progress',
  'notification/root': '.ant-notification-notice',
  'notification/list': '.ant-notification-list',
  'notification/listContent': '.ant-notification-list-content',
  'notification/section': '.ant-notification-notice-section',
  'notification/title': '.ant-notification-notice-title',
  'notification/wrapper': '.ant-notification-notice-wrapper',
  'popconfirm/content': '.ant-popconfirm-inner-content',
  'qr-code/root': '.ant-qrcode',
  'qr-code/cover': '.ant-qrcode-cover',
  'radio/icon': '.ant-radio-inner',
  'radio/label': '.ant-radio-wrapper',
  'skeleton/content': '.ant-skeleton',
  'space/separator': '.ant-space-item',
  'spin/tip': '.ant-spin-description',
  'spin/mask': '.ant-spin.ant-spin-fullscreen',
  'splitter/dragger.default': '.ant-splitter-bar-dragger:not(.ant-splitter-bar-dragger-active)',
  'splitter/dragger.active': '.ant-splitter-bar-dragger.ant-splitter-bar-dragger-active',
  'switch/content': '.ant-switch-inner',
  'switch/indicator': '.ant-switch-handle',
  'table/section': '.ant-table-container',
  'tabs/body': '.ant-tabs-body-holder',
  'tabs/popup': '.ant-tabs-dropdown',
  'tag/close': '.ant-tag-close-icon',
  'tag/content': '.ant-tag-content',
  'tag/icon': '.ant-tag-icon',
  'tag/root': '.ant-tag, .ant-tag-checkable-group',
  'tag/item': '.ant-tag-checkable-group-item',
  'time-picker/root': '.ant-picker',
  'time-picker/popup.root': '.ant-picker-dropdown',
  'time-picker/input': '.ant-picker-input',
  'time-picker/prefix': '.ant-picker-prefix',
  'time-picker/suffix': '.ant-picker-suffix',
  'time-picker/popup': '.ant-picker-dropdown',
  'time-picker/container': '.ant-picker-panel-container',
  'time-picker/content': '.ant-picker-content',
  'time-picker/item': '.ant-picker-cell',
  'time-picker/footer': '.ant-picker-footer',
  'transfer/body': '.ant-transfer-list-body',
  'transfer/header': '.ant-transfer-list-header',
  'transfer/title': '.ant-transfer-list-header-title',
  'transfer/footer': '.ant-transfer-list-footer',
  'transfer/item': '.ant-transfer-list-content-item',
  'transfer/itemContent': '.ant-transfer-list-content-item-text',
  'transfer/itemIcon': '.ant-transfer-list-checkbox',
  'transfer/source': '.ant-transfer > .ant-transfer-section:first-of-type',
  'transfer/target': '.ant-transfer > .ant-transfer-section:last-of-type',
  'transfer/section': '.ant-transfer-section',
  'transfer/actions': '.ant-transfer-actions',
  'transfer/list': '.ant-transfer-list-content',
  'tree-select/root': '.ant-select.ant-tree-select',
  'tree-select/content': '.ant-select-content',
  'tree-select/input': '.ant-select-input',
  'tree-select/item': '.ant-select-selection-item',
  'tree-select/itemContent': '.ant-select-selection-item-content',
  'tree-select/itemRemove': '.ant-select-selection-item-remove',
  'tree-select/itemSwitcher': '.ant-tree-switcher.ant-select-tree-switcher',
  'tree-select/itemTitle': '.ant-tree-title.ant-select-tree-title',
  'tree-select/placeholder': '.ant-select-placeholder',
  'tree-select/popup': '.ant-select-dropdown',
  'tree-select/popup.root': '.ant-select-dropdown.ant-tree-select-dropdown',
  'tree-select/popup.item': '.ant-tree-treenode.ant-select-tree-treenode',
  'tree-select/popup.itemTitle': '.ant-tree-title.ant-select-tree-title',
  'tree-select/popup.itemSwitcher': '.ant-tree-switcher.ant-select-tree-switcher',
  'tree-select/prefix': '.ant-select-prefix',
  'tree-select/suffix': '.ant-select-suffix',
  'typography/textarea': '.ant-typography-edit-content',
  'color-picker/popup.root': '.ant-popover.ant-color-picker',
  'date-picker/popup.root': '.ant-picker-dropdown',
  'date-picker/popup.container': '.ant-picker-panel-container',
  'date-picker/popup.header': '.ant-picker-header',
  'date-picker/popup.body': '.ant-picker-body',
  'date-picker/popup.content': '.ant-picker-content',
  'date-picker/popup.item': '.ant-picker-cell',
  'date-picker/popup.footer': '.ant-picker-footer',
  'float-button/content': '.ant-float-btn-content',
  'float-button/icon': '.ant-float-btn-icon',
  'image/popup.root': '.ant-image-preview',
  'image/popup.mask': '.ant-image-preview-mask',
  'image/popup.body': '.ant-image-preview-body',
  'image/popup.footer': '.ant-image-preview-footer',
  'image/popup.actions': '.ant-image-preview-actions',
  'image/popup.close': '.ant-image-preview-close',
  'input-number/action': '.ant-input-number-action',
  'input/button.root': '.ant-input-search-button',
  'input/button.content': '.ant-btn > span:not(.ant-btn-icon)',
  'input/button.icon': '.ant-btn-icon',
  'popconfirm/arrow': '.ant-popover-arrow',
  'popconfirm/container': '.ant-popover-container',
  'popconfirm/content': '.ant-popconfirm-inner-content',
  'popconfirm/icon': '.ant-popconfirm-message-icon',
  'popconfirm/root': '.ant-popover',
  'popconfirm/title': '.ant-popconfirm-title',
  'popover/arrow': '.ant-popover-arrow',
  'popover/container': '.ant-popover-container',
  'popover/root': '.ant-popover',
  'steps/content': '.ant-steps-item-content',
  'steps/header': '.ant-steps-item-header',
  'steps/icon': '.ant-steps-item-icon',
  'steps/rail': '.ant-steps-item-rail',
  'steps/section': '.ant-steps-item-section',
  'steps/subtitle': '.ant-steps-item-subtitle',
  'steps/title': '.ant-steps-item-title',
  'steps/wrapper': '.ant-steps-item-wrapper',
  'table/header.wrapper': '.ant-table-thead',
  'table/header.row': '.ant-table-thead > tr',
  'table/header.cell': '.ant-table-thead > tr > .ant-table-cell',
  'table/body.wrapper': '.ant-table-tbody',
  'table/body.row': '.ant-table-tbody > tr',
  'table/body.cell': '.ant-table-tbody > tr > .ant-table-cell',
  'table/pagination.root': '.ant-table-pagination',
  'table/pagination.item': '.ant-table-pagination .ant-pagination-item',
  'tabs/popup.root': '.ant-tabs-dropdown',
  'timeline/item': '.ant-timeline-item',
  'timeline/itemWrapper': '.ant-timeline-item-wrapper',
  'timeline/itemIcon': '.ant-timeline-item-icon',
  'timeline/itemHeader': '.ant-timeline-item-header',
  'timeline/itemTitle': '.ant-timeline-item-title',
  'timeline/itemSection': '.ant-timeline-item-section',
  'timeline/itemContent': '.ant-timeline-item-content',
  'timeline/itemRail': '.ant-timeline-item-rail',
  'timeline/content': '.ant-timeline-item-content',
  'timeline/header': '.ant-timeline-item-header',
  'timeline/icon': '.ant-timeline-item-icon',
  'timeline/rail': '.ant-timeline-item-rail',
  'timeline/section': '.ant-timeline-item-section',
  'timeline/title': '.ant-timeline-item-title',
  'timeline/wrapper': '.ant-timeline-item-wrapper',
  'transfer/source.section': '.ant-transfer > .ant-transfer-section:first-of-type',
  'transfer/source.header': '.ant-transfer > .ant-transfer-section:first-of-type .ant-transfer-list-header',
  'transfer/source.title': '.ant-transfer > .ant-transfer-section:first-of-type .ant-transfer-list-header-title',
  'transfer/source.body': '.ant-transfer > .ant-transfer-section:first-of-type .ant-transfer-list-body',
  'transfer/source.list': '.ant-transfer > .ant-transfer-section:first-of-type .ant-transfer-list-content',
  'transfer/source.item': '.ant-transfer > .ant-transfer-section:first-of-type .ant-transfer-list-content-item',
  'transfer/source.itemIcon': '.ant-transfer > .ant-transfer-section:first-of-type .ant-transfer-list-checkbox',
  'transfer/source.itemContent': '.ant-transfer > .ant-transfer-section:first-of-type .ant-transfer-list-content-item-text',
  'transfer/source.footer': '.ant-transfer > .ant-transfer-section:first-of-type .ant-transfer-list-footer',
  'transfer/target.section': '.ant-transfer > .ant-transfer-section:last-of-type',
  'transfer/target.header': '.ant-transfer > .ant-transfer-section:last-of-type .ant-transfer-list-header',
  'transfer/target.title': '.ant-transfer > .ant-transfer-section:last-of-type .ant-transfer-list-header-title',
  'transfer/target.body': '.ant-transfer > .ant-transfer-section:last-of-type .ant-transfer-list-body',
  'transfer/target.list': '.ant-transfer > .ant-transfer-section:last-of-type .ant-transfer-list-content',
  'transfer/target.item': '.ant-transfer > .ant-transfer-section:last-of-type .ant-transfer-list-content-item',
  'transfer/target.itemIcon': '.ant-transfer > .ant-transfer-section:last-of-type .ant-transfer-list-checkbox',
  'transfer/target.itemContent': '.ant-transfer > .ant-transfer-section:last-of-type .ant-transfer-list-content-item-text',
  'transfer/target.footer': '.ant-transfer > .ant-transfer-section:last-of-type .ant-transfer-list-footer',
};

const existingNodes = parseExistingNodes(existing);
if (existingJson) {
  try {
    const parsed = JSON.parse(existingJson);
    for (const component of parsed.components || []) {
      const nodes = existingNodes.get(component.name) || new Map();
      for (const node of component.nodes || []) {
        nodes.set(node.name, {
          selector: node.selector,
          description: node.description,
          version: node.version || '6.0.0',
        });
      }
      existingNodes.set(component.name, nodes);
    }
  } catch (error) {
    if (existingJson.trim()) throw new Error(`Invalid existing Semantic DOM JSON: ${error.message}`);
  }
}
const inventoryComponents = new Map();
for (const entry of inventory.entries || []) {
  const paths = inventoryComponents.get(entry.component) || new Set();
  for (const semanticPath of entry.semanticPaths || []) paths.add(semanticPath);
  inventoryComponents.set(entry.component, paths);
}

const documentedNodes = new Map();
const documentedPaths = new Map();
for (const page of semanticMdCatalog?.pages ?? []) {
  const paths = documentedPaths.get(page.component) ?? new Set();
  for (const part of page.parts ?? []) {
    paths.add(part.name);
    const key = `${page.component}/${part.name}`;
    const previous = documentedNodes.get(key) ?? {};
    documentedNodes.set(key, {
      description: page.locale === 'zh-CN' || !previous.description ? part.description : previous.description,
      selectorCandidate: page.variant === null
        ? part.selectorCandidate || previous.selectorCandidate || null
        : previous.selectorCandidate || part.selectorCandidate || null,
    });
  }
  documentedPaths.set(page.component, paths);
}
for (const component of documentedPaths.keys()) {
  if (!inventoryComponents.has(component)) inventoryComponents.set(component, new Set());
}

// Include every React component from the synchronized component catalog, even
// when that component has not declared Semantic DOM slots yet.
for (const component of inventory.componentCatalog || []) {
  if (!inventoryComponents.has(component.name)) inventoryComponents.set(component.name, new Set());
}

const components = [...inventoryComponents.keys()].sort((a, b) => a.localeCompare(b));
const catalog = {
  schemaVersion: 1,
  source: semanticMdCatalog
    ? {
      ...semanticMdCatalog.source,
      apiInventory: inventory.source.path,
      apiSemanticSource: inventory.source.semanticSource,
    }
    : inventory.source,
  components: components.map((component) => {
    const oldNodes = existingNodes.get(component) || new Map();
    // The exported semantic.md is the documentation target, but its demo only
    // renders a subset of public SemanticType paths. Keep the API inventory as
    // a completeness source and merge both sets so documented slots never hide
    // public classNames/styles slots that are not currently shown in the demo.
    const semanticPaths = [...new Set([
      ...(documentedPaths.get(component) || []),
      ...(inventoryComponents.get(component) || []),
    ])].sort((a, b) => a.localeCompare(b));
    return {
      name: component,
      nodes: semanticPaths.map((name) => {
        const previous = oldNodes.get(name);
        const documented = documentedNodes.get(`${component}/${name}`);
        const fallbackSelector = fallbackNode(component, name).selector;
        const previousIsFallback = previous?.selector === fallbackSelector;
        const selector = semanticPathSelectors[`${component}/${name}`]
          || documented?.selectorCandidate
          || (previous?.selector && !previousIsFallback ? previous.selector : null)
          || previous?.selector
          || fallbackSelector;
        const generatedDescription = `${component} ${name} Semantic DOM 节点`;
        return {
          name,
          selector,
          description: documented?.description || (previous?.description && previous.description !== generatedDescription
            ? previous.description
            : previous?.description || generatedDescription),
          version: previous?.version || '6.0.0',
        };
      }),
    };
  }),
};

fs.mkdirSync(path.dirname(outputPath), { recursive: true });
fs.writeFileSync(outputPath, `${JSON.stringify(catalog, null, 2)}\n`);
console.log(JSON.stringify({
  outputPath,
  components: components.length,
  slots: catalog.components.reduce((count, component) => count + component.nodes.length, 0),
  documentedSlots: documentedNodes.size,
  documentedSelectorCandidates: [...documentedNodes.values()].filter((node) => node.selectorCandidate).length,
}));
