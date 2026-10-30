# React / Blazor 文档运行时对齐扫描

扫描时间：2026-10-29T21:45:53.922Z

## 范围与判定

- React: http://127.0.0.1:8000；Blazor: http://127.0.0.1:5000。React demo 名称作为缺漏基准，Blazor 独有 demo 不抵扣缺漏。
- 静态扫描在 viewport 1280×900、light theme 下逐组件进入文档页，滚动触发延迟加载，采集 demo 当前可见的 Ant Design 节点 HTML、选定 computed CSS 属性与几何位置；display:none、visibility:hidden 等关闭态节点不参与可见样式比较，另记隐藏节点数。
- HTML 快照不一致仅标记为候选；HTML 中可能包含运行时包装、语义差异或无视觉影响的属性。CSS/几何差异按同名 Ant 类节点匹配，1px 内视为相同。
- 下方“交互状态”来自 DOM/style manifests 的实际浏览器操作；未覆盖的 demo 不能据此判为交互样式已对齐。

## 汇总

| 指标 | 数量 |
|---|---:|
| React / Blazor 组件路由 | 74 / 74 |
| 可比组件 | 71 |
| React / Blazor demo | 1133 / 1053 |
| 名称匹配 demo | 841 |
| React 有、Blazor 缺失 | 292 |
| 仅 Blazor 有（不计缺漏） | 212 |
| HTML 首屏快照候选差异 | 741 / 841 |
| computed CSS 候选差异 | 581 / 841 |
| 几何候选差异（>1px） | 544 / 841 |
| 页面采集阻塞 | 3 |

## 组件路由差异

- React 有、Blazor 缺失：`BorderBeam 6.4.0` (/components/border-beam), `EditableProTable` (/components/editable-table)。
- **BorderBeam 6.4.0** 整个组件路由缺失；React 文档 demo：`basic`, `count`, `customized-color`, `size`, `non-uniform-radius`, `hover`, `custom-container`, `duration`, `line-width`, `component-token`。
- **EditableProTable** 整个组件路由缺失；React 文档 demo：当前无法采集；采集信息：Waiting for selector `.code-box-demo` failed。
- Blazor 独有：`PageHeader` (/en-US/components/pageheader), `Comment` (/en-US/components/comment), `BackTop` (/en-US/components/backtop)。
- 按扫描配置排除：`Util`。

## React 有但 Blazor 缺失的 demo

仅按 React demo 名称计数；Blazor 额外 demo 不列为缺漏。

| 组件 | 缺失 React demo |
|---|---|
| FloatButton | `_semantic`, `_semantic_group` |
| Typography | `title`, `paragraph-debug`, `editable`, `ellipsis-debug`, `componenttoken-debug`, `link-danger-debug`, `_semantic` |
| ProLayout | `_semantic` |
| Masonry 6.0.0 | `_semantic` |
| Space | `compact`, `compact-debug`, `compact-nested`, `style-class`, `component-token`, `_semantic` |
| Splitter | `nested-in-tabs`, `debug` |
| Anchor | `_semantic` |
| Breadcrumb | `withparams`, `overlay`, `debug-routes`, `component-token`, `withicon`, `separator-component`, `style-class`, `_semantic` |
| Dropdown | `placement`, `trigger`, `overlay-open`, `icon-debug`, `sub-menu-debug`, `context-menu`, `style-class`, `render-panel`, `_semantic` |
| Menu | `_semantic` |
| Pagination | `_semantic` |
| Steps | `max-count`, `style-class`, `component-token`, `_semantic`, `_semantic_items` |
| Tabs | `_semantic` |
| AutoComplete | `status`, `allowclear`, `options`, `variant`, `_semantic` |
| Cascader | `fields-name`, `ellipsis-debug`, `custom-render`, `panel`, `render-panel`, `_semantic` |
| ColorPicker | `base`, `text-render`, `presets-line-gradient`, `style-class`, `line-gradient`, `trigger-event`, `panel-render`, `pure-panel`, `_semantic` |
| DatePicker | `multiple`, `components`, `filled-debug`, `multiple-debug`, `render-panel`, `_semantic` |
| ProForm | `_semantic` |
| Input | `filled-debug`, `search-variant-debug`, `style-class`, `align`, `_semantic_input`, `_semantic_textarea`, `_semantic_search`, `_semantic_password`, `_semantic_otp` |
| InputNumber | `addon`, `digit`, `disabled-hover-debug`, `borderless-height-debug`, `debug-token`, `spinner`, `filled-debug`, `render-panel`, `_semantic` |
| Mentions | `popuprender`, `autosize`, `render-panel`, `size`, `allowclear`, `autosize-textarea-debug`, `style-class`, `component-token`, `_semantic` |
| Radio | `_semantic` |
| Switch | `text`, `style-class`, `disabled`, `component-token`, `_semantic` |
| TimePicker | `12hours`, `colored-popup`, `render-panel`, `change-on-scroll`, `_semantic` |
| Transfer | `component-token`, `_semantic` |
| TreeSelect | `render-panel`, `component-token`, `_semantic` |
| Upload | `debug-disabled`, `_semantic` |
| Badge | `ribbon-debug`, `component-token`, `style-class`, `colorful-with-count-debug`, `_semantic`, `_semantic_ribbon` |
| Calendar | `event-range`, `lunar`, `week`, `style-class`, `component-token`, `_semantic` |
| Card | `no-body-debug`, `button-alignment-debug` |
| Carousel | `autoplay`, `arrows`, `component-token`, `placement`, `fade`, `dot-duration` |
| Collapse | `_semantic` |
| ProDescriptions | `text`, `padding`, `size`, `style`, `style-class`, `jsx`, `component-token`, `block`, `_semantic` |
| Empty | `_semantic` |
| Image | `toolbarrender`, `preview-mask`, `coverplacement`, `preview-group-top-progress`, `component-token`, `preview-imginfo`, `_semantic` |
| ProList | `pagination`, `grid-test`, `responsive`, `drag-sorting`, `drag-sorting-handler`, `grid-drag-sorting`, `grid-drag-sorting-handler`, `virtual-list`, `component-token`, `spin-debug`, `_semantic` |
| Listy 6.6.0 | `virtual`, `group`, `rich`, `drag-sorting`, `infinite`, `style-class`, `scroll-to`, `_semantic` |
| Popover | `placement`, `style-class`, `wireframe`, `arrow`, `shift`, `render-panel`, `component-token`, `_semantic` |
| QRCode | `_semantic` |
| Segmented | `style-class`, `size-consistent`, `vertical`, `shape`, `with-name`, `controlled-two`, `componenttoken`, `_semantic` |
| Statistic | `animated`, `timer`, `component-token`, `style-class`, `_semantic` |
| ProTable | `_semantic` |
| Tag | `disabled`, `customize`, `component-token` |
| Timeline | `pending`, `horizontal-debug`, `end`, `pending-legacy`, `semantic`, `component-token`, `_semantic`, `_semantic_items` |
| Tooltip | `shift`, `force-align`, `debug`, `disabled-children`, `style-class`, `smooth-transition`, `destroy-on-close`, `render-panel`, `wrap-custom-component`, `_semantic` |
| Tour | `render-panel`, `_semantic` |
| Tree | `directory-debug`, `drag-debug`, `block-node`, `multiple-line`, `line-debug`, `scroll-to`, `component-token`, `style-class`, `_semantic` |
| Drawer | `multi-level-drawer`, `style-class`, `scroll-debug`, `render-in-current`, `config-provider`, `render-panel`, `component-token`, `_semantic` |
| Message | `hooks`, `style-class`, `component-token`, `stack`, `thenable`, `render-panel`, `_semantic` |
| Modal | `dark`, `render-panel`, `wireframe`, `_semantic` |
| Notification | `hooks`, `show-with-progress`, `progress-color`, `render-panel`, `stack`, `component-token`, `style-class`, `_semantic` |
| Popconfirm | `placement`, `async`, `style-class`, `wireframe`, `shift`, `promise`, `render-panel`, `_semantic` |
| Progress | `circle-steps`, `info-position`, `circle-micro`, `size`, `style-class`, `_semantic` |
| Result | `_semantic` |
| Skeleton | `_semantic`, `_semantic_element` |
| Spin | `percent`, `fullscreen`, `list-debug`, `_semantic` |
| Watermark | `portal`, `debug` |
| App | `basic`, `config` |
| ConfigProvider | `locale`, `direction`, `direction`, `size`, `theme`, `wave`, `holderrender`, `prefixcls`, `useconfig`, `warning`, `focus-outline-debug` |

## 首屏 HTML 快照候选差异

以下名称的采集快照不完全相同，尚未人工确认每项都是实际缺陷。

| 组件 | 候选 demo |
|---|---|
| Button | `debug-color-variant`, `icon-placement`, `debug-block`, `disabled`, `multiple`, `danger`, `legacy-group`, `component-token`, `wave`, `style-class`, `color-variant`, `icon`, `debug-icon`, `size`, `loading`, `ghost`, `block`, `chinese-chars-loading`, `linear-gradient`, `custom-disabled-bg`, `_semantic` |
| FloatButton | `render-panel` |
| Typography | `basic`, `text`, `copyable`, `ellipsis`, `ellipsis-controlled`, `ellipsis-middle`, `suffix` |
| Divider | `component-token`, `style-class`, `with-text`, `customize-style`, `variant`, `_semantic` |
| Flex | `basic`, `align`, `gap`, `combination`, `debug` |
| Grid | `playground`, `usebreakpoint` |
| ProLayout | `basic`, `top`, `top-side`, `top-side-2`, `custom-trigger`, `collapsible-overlay`, `responsive`, `custom-trigger-debug`, `component-token` |
| Masonry 6.0.0 | `basic`, `responsive`, `image`, `dynamic`, `style-class`, `fresh` |
| Space | `base`, `vertical`, `size`, `align`, `wrap`, `separator`, `compact-buttons`, `compact-button-vertical`, `debug`, `gap-in-line` |
| Splitter | `control`, `vertical`, `collapsible`, `collapsibleicon`, `multiple`, `group`, `lazy`, `customize`, `style-class`, `reset`, `size-mix`, `_semantic` |
| Anchor | `basic`, `horizontal`, `static`, `onclick`, `customizehighlight`, `targetoffset`, `targetoffset-per-link`, `onchange`, `replace`, `legacy-anchor`, `style-class`, `component-token` |
| Breadcrumb | `separator` |
| Dropdown | `basic`, `item`, `dropdown-button`, `sub-menu`, `loading`, `selection`, `menu-full`, `extra`, `arrow`, `arrow-center`, `event`, `custom-dropdown`, `selectable` |
| Menu | `horizontal`, `horizontal-dark`, `inline`, `inline-collapsed`, `tooltip`, `sider-current`, `vertical`, `theme`, `submenu-theme`, `switch-mode`, `style-class`, `style-debug`, `menu-v4`, `component-token`, `extra-style`, `extra-collapsed-debug`, `custom-popup-render`, `collapsed-icon-debug` |
| Pagination | `basic`, `align`, `more`, `changer`, `jump`, `mini`, `simple`, `controlled`, `total`, `all`, `itemrender`, `components`, `wireframe`, `component-token`, `variant-debug`, `style-class` |
| Steps | `simple`, `error`, `vertical`, `clickable`, `panel`, `icon`, `step-next`, `title-placement`, `progress-dot`, `customized-progress-dot`, `nav`, `progress`, `progress-debug`, `steps-in-steps`, `inline`, `inline-variant`, `variant-debug` |
| Tabs | `basic`, `disabled`, `centered`, `icon`, `custom-indicator`, `slide`, `extra`, `size`, `placement`, `popuprender-search`, `card`, `editable-card`, `card-top`, `custom-add-trigger`, `custom-tab-bar`, `custom-tab-bar-node`, `style-class`, `animated`, `nest`, `component-token` |
| AutoComplete | `basic`, `custom`, `certain-category`, `customize-clear-debug`, `disabled-in-form-debug`, `form-debug`, `render-panel`, `non-case-sensitive`, `uncertain-category`, `style-class`, `disabled-custom-debug`, `filled-custom-debug`, `autocomplete-and-select` |
| Cascader | `basic`, `multiple`, `search`, `custom-dropdown`, `variant`, `style-class`, `component-token`, `default-value`, `showcheckedstrategy`, `suffix`, `placement`, `status` |
| Checkbox | `basic`, `controller`, `check-all`, `style-class`, `debug-disable-popover`, `custom-line-width`, `disabled`, `group`, `layout`, `debug-line`, `debug-group-width`, `_semantic` |
| ColorPicker | `controlled`, `disabled-alpha`, `trigger`, `format`, `size`, `disabled`, `allowclear`, `presets` |
| DatePicker | `basic`, `needconfirm`, `format`, `mask`, `disabled`, `allow-empty`, `preset-ranges`, `size`, `buddhist-era`, `variant`, `mode`, `suffix`, `component-token`, `range-picker`, `switchable`, `time`, `date-range`, `disabled-date`, `select-in-range`, `extra-footer`, `cell-render`, `external-panel`, `status`, `style-class`, `placement`, `start-end`, `suffixicon-debug` |
| ProForm | `basic`, `control-hooks`, `layout`, `layout-multiple`, `disabled`, `variant`, `required-mark`, `size`, `layout-can-wrap`, `warning-only`, `usewatch`, `validate-trigger`, `validate-only`, `form-item-path`, `dynamic-form-item`, `dynamic-form-items`, `dynamic-form-items-drag-sorting`, `dynamic-form-items-no-style`, `dynamic-form-items-complex`, `nest-messages`, `complex-form-control`, `customized-form-controls`, `global-state`, `form-context`, `inline-login`, `login`, `register`, `advanced-search`, `time-related-controls`, `without-form-create`, `validate-static`, `dynamic-rule`, `form-dependencies`, `getvalueprops-normalize`, `validate-scroll-to-field`, `validate-other`, `style-class`, `disabled-input-debug`, `label-debug`, `col-24-debug`, `ref-item`, `custom-feedback-icons`, `component-token` |
| Input | `basic`, `variant`, `compact-style`, `search-input`, `textarea`, `otp`, `presuffix`, `allowclear`, `advance-count`, `focus`, `borderless-debug`, `textarea-resize`, `component-token`, `size`, `addon`, `group`, `search-input-loading`, `autosize-textarea`, `tooltip`, `password-input`, `show-count`, `status`, `debug-addon` |
| InputNumber | `basic`, `keyboard`, `variant`, `presuffix`, `focus`, `controls`, `size`, `disabled`, `formatter`, `change-on-wheel`, `out-of-range`, `status`, `style-class` |
| Mentions | `basic`, `variant`, `form`, `readonly`, `status`, `async`, `prefix`, `placement` |
| Radio | `radiogroup`, `radiogroup-block`, `radiobutton`, `size`, `style-class`, `debug-group-width`, `component-token`, `disabled`, `radiogroup-more`, `radiogroup-options`, `radiogroup-with-name`, `radiobutton-solid`, `badge`, `wireframe`, `debug-upload` |
| Rate | `character`, `component-token`, `size`, `text`, `clear`, `character-function` |
| Select | `variant`, `clear-suffix-debug`, `custom-label-render`, `placement`, `debug`, `option-label-center`, `component-token`, `style-class`, `size`, `optgroup`, `suffix`, `filled-debug`, `custom-tag-render`, `responsive`, `status`, `placement-debug`, `render-panel`, `debug-flip-shift`, `maxcount`, `_semantic` |
| Slider | `basic`, `icon-slider`, `event`, `vertical`, `reverse`, `multiple`, `disabled-handle`, `component-token`, `input-number`, `tip-formatter`, `mark`, `show-tooltip`, `draggabletrack`, `editable`, `style-class`, `_semantic` |
| Switch | `basic`, `loading`, `size` |
| TimePicker | `basic`, `size`, `disabled`, `interval-options`, `variant`, `suffix`, `value`, `need-confirm`, `hide-column`, `addon`, `range-picker`, `status`, `style-class` |
| Transfer | `basic`, `oneway`, `search`, `advanced`, `custom-item`, `actions`, `large-data`, `table-transfer`, `tree-transfer`, `status`, `style-class`, `custom-select-all-labels` |
| TreeSelect | `basic`, `treedata`, `placement`, `status`, `suffix`, `multiple`, `checkable`, `treeline`, `variant`, `maxcount`, `style-class` |
| Upload | `basic`, `defaultfilelist`, `picture-circle`, `drag`, `directory`, `upload-png-only`, `preview-file`, `transform-file`, `file-type`, `drag-sorting`, `customize-progress-bar`, `component-token`, `avatar`, `picture-card`, `filelist`, `paste`, `upload-manually`, `picture-style`, `max-count`, `upload-with-aliyun-oss`, `upload-custom-action-icon`, `crop-image`, `style-class` |
| Avatar | `basic`, `dynamic`, `group`, `toggle-debug`, `fallback`, `type`, `badge`, `max-count`, `responsive`, `component-token` |
| Badge | `basic`, `overflow`, `change`, `offset`, `status`, `ribbon`, `title`, `no-wrapper`, `dot`, `link`, `size`, `colorful`, `mix` |
| Calendar | `basic`, `notice-calendar`, `card`, `select`, `customize-header` |
| Card | `basic`, `border-less`, `simple`, `flexible-content`, `in-column`, `loading`, `grid-card`, `inner`, `tabs`, `meta`, `style-class`, `component-token`, `_semantic`, `_semantic_meta` |
| Carousel | `basic` |
| Collapse | `basic`, `size`, `borderless`, `custom`, `icon`, `noarrow`, `extra`, `ghost`, `collapsible`, `style-class`, `component-token` |
| ProDescriptions | `basic`, `border`, `responsive`, `vertical`, `vertical-border` |
| Empty | `basic`, `simple`, `customize`, `config-provider`, `style-class`, `description` |
| Image | `basic`, `placeholder`, `fallback`, `preview-group`, `preview-group-visible`, `previewsrc`, `controlled-preview`, `imagerender`, `mask`, `style-class`, `nested` |
| ProList | `simple`, `basic`, `loadmore`, `vertical`, `grid`, `infinite-load` |
| Listy 6.6.0 | `basic` |
| Popover | `basic`, `arrow-point-at-center`, `triggertype`, `hover-with-click` |
| QRCode | `base`, `status`, `type`, `customcolor`, `errorlevel`, `style-class`, `customstatusrender`, `customsize`, `download` |
| Segmented | `basic`, `block`, `disabled`, `custom`, `size`, `icon-only`, `controlled`, `dynamic`, `with-icon` |
| Statistic | `basic`, `unit`, `card` |
| ProTable | `basic`, `jsx`, `row-selection`, `row-selection-and-operation`, `row-selection-custom`, `row-selection-debug`, `head`, `filter-in-tree`, `filter-search`, `multiple-sorter`, `reset-filter`, `custom-filter-panel`, `ajax`, `size`, `narrow`, `bordered`, `expand`, `expand-sticky`, `order-column`, `colspan-rowspan`, `tree-data`, `tree-table-ellipsis`, `fixed-header`, `auto-height`, `fixed-columns`, `fixed-gapped-columns`, `fixed-columns-header`, `hidden-columns`, `grouping-columns`, `edit-cell`, `edit-row`, `nested-table`, `drag-sorting`, `drag-column-sorting`, `drag-sorting-handler`, `ellipsis`, `column-defaults`, `ellipsis-custom-tooltip`, `custom-empty`, `summary`, `virtual-list`, `responsive`, `nest-table-border-debug`, `nested-table-in-tabs-debug`, `pagination`, `row-selection-custom-debug`, `sticky`, `dynamic-settings`, `style-class`, `selections-debug`, `component-token`, `measure-row-render` |
| Tag | `basic`, `control`, `animation`, `status`, `draggable`, `colorful`, `checkable`, `icon`, `style-class`, `_semantic`, `_semantic_group` |
| Timeline | `basic`, `alternate`, `title-span`, `style-class`, `variant`, `horizontal`, `custom`, `title` |
| Tooltip | `placement`, `colorful`, `arrow`, `auto-adjust-overflow`, `disabled` |
| Tour | `basic`, `indicator`, `gap`, `non-modal`, `mask`, `actions-render`, `style-class` |
| Tree | `basic`, `draggable`, `search`, `customized-icon`, `virtual-scroll`, `basic-controlled`, `dynamic`, `line`, `directory`, `switcher-icon`, `big-data` |
| Alert | `basic`, `filled`, `description`, `smooth-closed`, `custom-icon`, `custom-title-alignment`, `style-class`, `style`, `closable`, `icon`, `loop-banner`, `error-boundary`, `action`, `component-token`, `_semantic` |
| Drawer | `basic-right`, `resizable`, `extra`, `form-in-drawer`, `mask`, `no-mask`, `placement`, `loading`, `user-profile`, `size`, `closable-placement` |
| Message | `info`, `update` |
| Modal | `hooks`, `modal-render`, `static-info`, `nested`, `component-token`, `mask`, `footer-render`, `locale`, `width`, `confirm`, `style-class` |
| Notification | `with-icon`, `with-btn`, `placement` |
| Popconfirm | `basic`, `dynamic-trigger`, `locale`, `icon` |
| Progress | `line`, `line-mini`, `circle-mini`, `format`, `segment`, `gradient-line`, `circle`, `dynamic`, `dashboard`, `linecap`, `steps` |
| Result | `success`, `info`, `warning`, `403`, `404`, `500`, `error`, `customicon`, `style-class`, `component-token` |
| Skeleton | `basic`, `complex`, `active`, `element`, `children`, `list`, `style-class`, `componenttoken` |
| Spin | `nested`, `delayanddebounce`, `size`, `tip`, `custom-indicator`, `style-class` |
| Watermark | `custom` |
| Affix | `basic`, `on-change`, `debug` |

## 首屏 CSS 与几何候选差异

| 组件 | CSS 属性候选差异 | 几何候选差异（>1px） |
|---|---|---|
| Button | `debug-color-variant`, `icon-placement`, `multiple`, `danger`, `legacy-group`, `component-token`, `wave`, `style-class`, `color-variant`, `icon`, `debug-icon`, `size`, `loading`, `ghost`, `block`, `chinese-chars-loading`, `linear-gradient`, `custom-disabled-bg`, `_semantic` | `debug-color-variant`, `icon-placement`, `multiple`, `danger`, `legacy-group`, `component-token`, `wave`, `style-class`, `color-variant`, `icon`, `debug-icon`, `size`, `loading`, `ghost`, `block`, `chinese-chars-loading`, `custom-disabled-bg`, `_semantic` |
| FloatButton | `render-panel` | `render-panel` |
| Typography | `basic`, `ellipsis-controlled`, `suffix` | `basic`, `text`, `copyable`, `ellipsis`, `ellipsis-controlled`, `suffix` |
| Divider | `component-token`, `style-class`, `with-text`, `plain`, `customize-style`, `variant`, `_semantic` | `size`, `vertical`, `component-token`, `style-class`, `with-text`, `customize-style`, `_semantic` |
| Flex | `gap`, `combination` | `gap`, `combination` |
| Grid | `playground` | `playground` |
| ProLayout | `basic`, `top`, `top-side`, `top-side-2`, `custom-trigger`, `collapsible-overlay`, `responsive`, `custom-trigger-debug`, `component-token` | `basic`, `top`, `top-side`, `top-side-2`, `custom-trigger`, `collapsible-overlay`, `responsive`, `custom-trigger-debug`, `component-token` |
| Masonry 6.0.0 | `responsive`, `dynamic`, `style-class`, `fresh` | `responsive`, `dynamic`, `style-class`, `fresh` |
| Space | `base`, `vertical`, `size`, `align`, `compact-button-vertical` | `base`, `vertical`, `size`, `align`, `compact-buttons`, `compact-button-vertical`, `gap-in-line` |
| Splitter | `vertical`, `collapsible`, `collapsibleicon`, `multiple`, `group`, `lazy`, `customize`, `style-class`, `size-mix`, `_semantic` | `vertical`, `collapsible`, `lazy`, `customize`, `style-class`, `size-mix`, `_semantic` |
| Anchor | `static`, `onclick`, `customizehighlight`, `onchange`, `legacy-anchor` | `static`, `onclick`, `customizehighlight`, `onchange`, `legacy-anchor` |
| Breadcrumb | `separator` | `separator` |
| Dropdown | `dropdown-button`, `loading`, `selection`, `arrow`, `arrow-center` | `dropdown-button`, `loading`, `selection`, `arrow`, `arrow-center` |
| Menu | `horizontal`, `horizontal-dark`, `inline`, `inline-collapsed`, `tooltip`, `sider-current`, `vertical`, `theme`, `submenu-theme`, `switch-mode`, `style-class`, `style-debug`, `menu-v4`, `component-token`, `extra-style`, `extra-collapsed-debug`, `collapsed-icon-debug` | `horizontal`, `horizontal-dark`, `inline`, `inline-collapsed`, `tooltip`, `sider-current`, `vertical`, `theme`, `submenu-theme`, `switch-mode`, `style-class`, `style-debug`, `menu-v4`, `component-token`, `extra-style`, `extra-collapsed-debug`, `collapsed-icon-debug` |
| Pagination | `more`, `changer`, `jump`, `mini`, `all`, `itemrender`, `components`, `wireframe`, `component-token`, `variant-debug`, `style-class` | `more`, `jump`, `mini`, `all`, `wireframe`, `component-token`, `variant-debug`, `style-class` |
| Steps | `simple`, `error`, `vertical`, `clickable`, `panel`, `step-next`, `title-placement`, `progress-dot`, `nav`, `progress`, `progress-debug`, `steps-in-steps`, `inline`, `inline-variant`, `variant-debug` | `simple`, `error`, `vertical`, `clickable`, `panel`, `step-next`, `title-placement`, `progress-dot`, `nav`, `progress`, `progress-debug`, `steps-in-steps`, `inline`, `inline-variant`, `variant-debug` |
| Tabs | `basic`, `disabled`, `centered`, `icon`, `custom-indicator`, `slide`, `extra`, `size`, `placement`, `popuprender-search`, `card`, `editable-card`, `card-top`, `custom-add-trigger`, `custom-tab-bar`, `custom-tab-bar-node`, `style-class`, `animated`, `component-token` | `basic`, `disabled`, `centered`, `icon`, `custom-indicator`, `slide`, `extra`, `size`, `placement`, `popuprender-search`, `card`, `editable-card`, `card-top`, `custom-add-trigger`, `custom-tab-bar`, `custom-tab-bar-node`, `style-class`, `animated`, `component-token` |
| AutoComplete | `certain-category`, `disabled-in-form-debug`, `form-debug`, `render-panel`, `uncertain-category`, `disabled-custom-debug`, `filled-custom-debug`, `autocomplete-and-select` | `certain-category`, `disabled-in-form-debug`, `form-debug`, `render-panel`, `uncertain-category`, `disabled-custom-debug`, `filled-custom-debug`, `autocomplete-and-select` |
| Cascader | `basic`, `multiple`, `custom-dropdown`, `style-class`, `component-token`, `showcheckedstrategy`, `suffix`, `placement`, `status` | `basic`, `multiple`, `custom-dropdown`, `variant`, `style-class`, `component-token`, `showcheckedstrategy`, `suffix`, `placement`, `status` |
| Checkbox | `style-class`, `debug-disable-popover`, `custom-line-width`, `disabled`, `layout`, `debug-line`, `_semantic` | `controller`, `style-class`, `debug-disable-popover`, `custom-line-width`, `debug-line`, `_semantic` |
| ColorPicker | `trigger`, `format`, `size` | `controlled`, `trigger`, `format`, `size` |
| DatePicker | `format`, `mask`, `disabled`, `allow-empty`, `preset-ranges`, `size`, `buddhist-era`, `variant`, `mode`, `suffix`, `component-token`, `range-picker`, `switchable`, `time`, `disabled-date`, `extra-footer`, `cell-render`, `status`, `style-class`, `placement`, `start-end`, `suffixicon-debug` | `format`, `mask`, `disabled`, `allow-empty`, `preset-ranges`, `size`, `buddhist-era`, `variant`, `mode`, `suffix`, `component-token`, `range-picker`, `switchable`, `time`, `disabled-date`, `extra-footer`, `cell-render`, `status`, `style-class`, `placement`, `suffixicon-debug` |
| ProForm | `basic`, `control-hooks`, `layout`, `layout-multiple`, `disabled`, `variant`, `required-mark`, `size`, `layout-can-wrap`, `warning-only`, `usewatch`, `validate-trigger`, `validate-only`, `form-item-path`, `dynamic-form-item`, `dynamic-form-items`, `dynamic-form-items-drag-sorting`, `dynamic-form-items-no-style`, `dynamic-form-items-complex`, `nest-messages`, `complex-form-control`, `customized-form-controls`, `global-state`, `form-context`, `inline-login`, `login`, `advanced-search`, `time-related-controls`, `without-form-create`, `validate-static`, `dynamic-rule`, `form-dependencies`, `validate-other`, `style-class`, `disabled-input-debug`, `label-debug`, `ref-item`, `custom-feedback-icons`, `component-token` | `basic`, `control-hooks`, `layout`, `layout-multiple`, `disabled`, `variant`, `required-mark`, `size`, `layout-can-wrap`, `warning-only`, `usewatch`, `validate-trigger`, `validate-only`, `form-item-path`, `dynamic-form-item`, `dynamic-form-items`, `dynamic-form-items-drag-sorting`, `dynamic-form-items-no-style`, `dynamic-form-items-complex`, `nest-messages`, `complex-form-control`, `customized-form-controls`, `global-state`, `form-context`, `inline-login`, `login`, `advanced-search`, `time-related-controls`, `without-form-create`, `validate-static`, `dynamic-rule`, `form-dependencies`, `validate-other`, `style-class`, `disabled-input-debug`, `label-debug`, `ref-item`, `custom-feedback-icons`, `component-token` |
| Input | `variant`, `search-input`, `otp`, `presuffix`, `allowclear`, `advance-count`, `focus`, `borderless-debug`, `textarea-resize`, `component-token`, `size`, `addon`, `group`, `tooltip`, `password-input`, `show-count`, `status`, `debug-addon` | `variant`, `search-input`, `otp`, `presuffix`, `advance-count`, `focus`, `borderless-debug`, `textarea-resize`, `component-token`, `size`, `addon`, `group`, `tooltip`, `password-input`, `show-count`, `debug-addon` |
| InputNumber | `keyboard`, `presuffix`, `focus`, `controls`, `out-of-range`, `status`, `style-class` | `presuffix`, `size`, `formatter`, `out-of-range`, `status`, `style-class` |
| Mentions | `form` | `form` |
| Radio | `radiogroup`, `size`, `style-class`, `component-token`, `disabled`, `radiogroup-more`, `radiobutton-solid`, `badge`, `wireframe`, `debug-upload` | `radiogroup`, `size`, `style-class`, `component-token`, `disabled`, `radiogroup-more`, `radiogroup-options`, `badge`, `wireframe` |
| Rate | `character`, `component-token`, `size`, `text`, `clear`, `character-function` | `character`, `component-token`, `text`, `clear` |
| Select | `debug`, `option-label-center`, `component-token`, `size`, `custom-tokenization`, `suffix`, `hide-selected`, `filled-debug`, `custom-tag-render`, `responsive`, `status`, `placement-debug`, `render-panel`, `maxcount`, `_semantic` | `option-label-center`, `component-token`, `filled-debug`, `custom-tag-render`, `responsive`, `placement-debug`, `render-panel`, `maxcount`, `_semantic` |
| Slider | `basic`, `icon-slider`, `event`, `vertical`, `reverse`, `multiple`, `disabled-handle`, `component-token`, `input-number`, `tip-formatter`, `mark`, `show-tooltip`, `draggabletrack`, `editable`, `style-class`, `_semantic` | `basic`, `icon-slider`, `event`, `vertical`, `reverse`, `multiple`, `disabled-handle`, `component-token`, `input-number`, `mark`, `show-tooltip`, `editable`, `style-class`, `_semantic` |
| Switch | `basic`, `loading` | `basic`, `loading` |
| TimePicker | `basic`, `size`, `disabled`, `value`, `need-confirm`, `hide-column`, `addon` | `basic`, `size`, `disabled`, `value`, `need-confirm`, `hide-column`, `addon` |
| Transfer | `basic`, `oneway`, `search`, `advanced`, `custom-item`, `actions`, `large-data`, `table-transfer`, `tree-transfer`, `status`, `style-class` | `oneway`, `search`, `advanced`, `custom-item`, `actions`, `large-data`, `table-transfer`, `tree-transfer`, `status`, `style-class` |
| TreeSelect | `treedata`, `placement`, `suffix`, `multiple`, `checkable`, `treeline`, `variant`, `maxcount`, `style-class` | `treedata`, `placement`, `suffix`, `multiple`, `checkable`, `treeline`, `variant`, `maxcount`, `style-class` |
| Upload | `basic`, `defaultfilelist`, `picture-circle`, `drag`, `directory`, `upload-png-only`, `preview-file`, `transform-file`, `file-type`, `drag-sorting`, `customize-progress-bar`, `component-token`, `avatar`, `picture-card`, `filelist`, `paste`, `upload-manually`, `picture-style`, `max-count`, `upload-with-aliyun-oss`, `upload-custom-action-icon`, `crop-image`, `style-class` | `basic`, `defaultfilelist`, `picture-circle`, `drag`, `directory`, `upload-png-only`, `preview-file`, `transform-file`, `file-type`, `drag-sorting`, `customize-progress-bar`, `component-token`, `avatar`, `picture-card`, `filelist`, `paste`, `upload-manually`, `picture-style`, `max-count`, `upload-with-aliyun-oss`, `upload-custom-action-icon`, `crop-image`, `style-class` |
| Avatar | `dynamic`, `group`, `toggle-debug`, `fallback`, `type`, `badge`, `max-count`, `component-token` | `dynamic`, `group`, `fallback`, `type`, `badge`, `max-count`, `component-token` |
| Badge | `basic`, `overflow`, `change`, `offset`, `status`, `ribbon`, `title`, `no-wrapper`, `dot`, `link`, `size`, `colorful`, `mix` | `basic`, `overflow`, `change`, `offset`, `status`, `ribbon`, `title`, `no-wrapper`, `dot`, `link`, `size`, `colorful`, `mix` |
| Calendar | `card`, `select`, `customize-header` | `notice-calendar`, `card`, `select`, `customize-header` |
| Card | `basic`, `flexible-content`, `in-column`, `loading`, `inner`, `tabs`, `meta`, `style-class`, `component-token`, `_semantic`, `_semantic_meta` | `basic`, `border-less`, `flexible-content`, `in-column`, `loading`, `inner`, `tabs`, `meta`, `style-class`, `component-token`, `_semantic`, `_semantic_meta` |
| Collapse | `size`, `custom`, `icon`, `ghost`, `collapsible`, `style-class`, `component-token` | `size`, `custom`, `icon`, `ghost`, `style-class` |
| ProDescriptions | `basic`, `border`, `responsive`, `vertical`, `vertical-border` | `basic`, `border`, `responsive`, `vertical`, `vertical-border` |
| Empty | `style-class` | `style-class` |
| Image | `basic`, `placeholder`, `fallback`, `preview-group`, `preview-group-visible`, `previewsrc`, `imagerender`, `mask`, `style-class` | `placeholder`, `preview-group`, `controlled-preview`, `mask`, `style-class` |
| ProList | `simple`, `basic`, `loadmore`, `vertical`, `grid` | `simple`, `basic`, `loadmore`, `vertical` |
| Listy 6.6.0 | `basic` | `basic` |
| Popover | `basic`, `control`, `triggertype`, `hover-with-click` | `triggertype`, `hover-with-click` |
| QRCode | `status`, `customstatusrender`, `popover` | `status`, `customstatusrender` |
| Segmented | `basic`, `block`, `disabled`, `custom`, `size`, `icon-only`, `controlled`, `dynamic`, `with-icon` | `disabled`, `custom`, `size`, `dynamic`, `with-icon` |
| Statistic | `basic`, `unit`, `card` | `basic`, `unit`, `card` |
| ProTable | `basic`, `jsx`, `row-selection`, `row-selection-and-operation`, `row-selection-custom`, `row-selection-debug`, `head`, `filter-in-tree`, `filter-search`, `multiple-sorter`, `reset-filter`, `custom-filter-panel`, `ajax`, `size`, `narrow`, `bordered`, `expand`, `expand-sticky`, `order-column`, `colspan-rowspan`, `tree-data`, `tree-table-ellipsis`, `fixed-header`, `fixed-columns`, `fixed-gapped-columns`, `fixed-columns-header`, `hidden-columns`, `grouping-columns`, `edit-cell`, `edit-row`, `nested-table`, `drag-sorting`, `drag-column-sorting`, `drag-sorting-handler`, `ellipsis`, `column-defaults`, `ellipsis-custom-tooltip`, `custom-empty`, `summary`, `virtual-list`, `responsive`, `nest-table-border-debug`, `nested-table-in-tabs-debug`, `pagination`, `row-selection-custom-debug`, `sticky`, `dynamic-settings`, `style-class`, `selections-debug`, `component-token`, `measure-row-render` | `basic`, `jsx`, `row-selection`, `row-selection-and-operation`, `row-selection-custom`, `row-selection-debug`, `head`, `filter-in-tree`, `filter-search`, `multiple-sorter`, `reset-filter`, `custom-filter-panel`, `ajax`, `size`, `narrow`, `bordered`, `expand`, `expand-sticky`, `order-column`, `colspan-rowspan`, `tree-data`, `tree-table-ellipsis`, `fixed-header`, `fixed-columns`, `fixed-gapped-columns`, `fixed-columns-header`, `hidden-columns`, `grouping-columns`, `edit-cell`, `edit-row`, `nested-table`, `drag-sorting`, `drag-column-sorting`, `drag-sorting-handler`, `ellipsis`, `column-defaults`, `ellipsis-custom-tooltip`, `custom-empty`, `summary`, `virtual-list`, `responsive`, `nest-table-border-debug`, `nested-table-in-tabs-debug`, `pagination`, `row-selection-custom-debug`, `sticky`, `dynamic-settings`, `style-class`, `selections-debug`, `component-token`, `measure-row-render` |
| Tag | `basic`, `control`, `animation`, `status`, `colorful`, `checkable`, `_semantic` | `basic`, `control`, `animation`, `status`, `colorful`, `checkable`, `_semantic`, `_semantic_group` |
| Timeline | `basic`, `alternate`, `title-span`, `style-class`, `variant`, `horizontal`, `custom`, `title` | `basic`, `alternate`, `title-span`, `style-class`, `variant`, `horizontal`, `title` |
| Tooltip | `placement`, `colorful`, `arrow`, `disabled` | `placement`, `colorful`, `arrow`, `disabled` |
| Tour | `basic`, `indicator`, `gap`, `non-modal`, `mask`, `actions-render`, `style-class` | `basic`, `indicator`, `gap`, `non-modal`, `mask`, `actions-render`, `style-class` |
| Tree | `basic`, `draggable`, `search`, `customized-icon`, `basic-controlled`, `dynamic`, `line`, `directory`, `switcher-icon` | `basic`, `draggable`, `search`, `customized-icon`, `basic-controlled`, `dynamic`, `line`, `directory`, `switcher-icon` |
| Alert | `basic`, `filled`, `description`, `custom-icon`, `custom-title-alignment`, `style-class`, `style`, `closable`, `icon`, `loop-banner`, `error-boundary`, `action`, `component-token`, `_semantic` | `description`, `custom-icon`, `style`, `closable`, `icon`, `loop-banner`, `error-boundary`, `action`, `_semantic` |
| Drawer | `resizable`, `extra`, `mask`, `no-mask`, `placement`, `user-profile`, `size`, `closable-placement` | `resizable`, `extra`, `no-mask`, `placement`, `user-profile`, `size`, `closable-placement` |
| Message | `info`, `update` | `info`, `update` |
| Modal | `hooks`, `static-info`, `component-token`, `mask`, `footer-render`, `width`, `confirm`, `style-class` | `component-token`, `footer-render`, `locale`, `width`, `confirm`, `style-class` |
| Notification | `with-btn`, `placement` | `with-btn`, `placement` |
| Popconfirm | — | `dynamic-trigger` |
| Progress | `line`, `line-mini`, `circle-mini`, `format`, `segment`, `gradient-line`, `circle`, `dynamic`, `dashboard`, `linecap`, `steps` | `line`, `line-mini`, `circle-mini`, `format`, `segment`, `gradient-line`, `circle`, `dynamic`, `dashboard`, `linecap`, `steps` |
| Result | `403`, `404`, `500`, `error`, `style-class`, `component-token` | `success`, `403`, `404`, `500`, `error`, `style-class`, `component-token` |
| Skeleton | `basic`, `complex`, `active`, `children`, `list`, `style-class`, `componenttoken` | `complex`, `list`, `style-class` |
| Spin | `basic`, `nested`, `delayanddebounce`, `size`, `tip`, `custom-indicator`, `style-class` | `basic`, `nested`, `delayanddebounce`, `size`, `tip`, `custom-indicator`, `style-class` |
| Watermark | `custom` | `custom` |
| Affix | — | `basic`, `on-change`, `debug` |

## 页面错误或采集阻塞

| 组件 | React 页面 | Blazor 页面 |
|---|---|---|
| Statistic | — | Uncaught ReferenceError: define is not defined |
| Notification | — | Uncaught ReferenceError: define is not defined |
| App | — | Waiting for selector `.code-box-demo` failed |

## 交互状态验收覆盖

运行 DOM/style manifests 中的场景，对点击、输入、悬停、弹层和语义节点进行 React / Blazor 对称操作后比较 DOM、computed style 与几何。该覆盖不是全站所有 demo 的交互穷举；功能型 demo 需按控件行为补充 scenario。

- 本次运行：44 组、48 个场景；DOM、CSS 与几何全部相同 3，至少一项有差异 45，阻塞 0。
- 视觉样式通过 48 个场景；存在视觉样式差异 0 个。

| 组件 / 场景 | 视觉样式差异样本 |
|---|---|

- 按 manifest 中能关联到 React demo 锚点的场景，交互后状态覆盖 38/841 个同名 demo；其余 803 个仍未完成逐 demo 的功能状态比对。以 body 为 scope 的弹层场景没有 demo 锚点，不计入此数。

| 组件 | 已有交互状态覆盖 | 尚未覆盖的同名 demo |
|---|---|---|
| Button | `basic` | `debug-color-variant`, `icon-placement`, `debug-block`, `disabled`, `multiple`, `danger`, `legacy-group`, `component-token`, `wave`, `style-class`, `color-variant`, `icon`, `debug-icon`, `size`, `loading`, `ghost`, `block`, `chinese-chars-loading`, `linear-gradient`, `custom-disabled-bg`, `_semantic` |
| FloatButton | — | `basic`, `shape`, `tooltip`, `group-menu`, `placement`, `back-top`, `badge`, `style-class`, `type`, `content`, `group`, `controlled`, `draggable`, `progress-ring`, `badge-debug`, `render-panel` |
| Icon | `basic`, `two-tone`, `custom`, `iconfont`, `scripturl` | — |
| Typography | — | `basic`, `text`, `copyable`, `ellipsis`, `ellipsis-controlled`, `ellipsis-middle`, `suffix`, `table` |
| Divider | `horizontal` | `size`, `vertical`, `component-token`, `style-class`, `with-text`, `plain`, `customize-style`, `variant`, `_semantic` |
| Flex | `basic` | `align`, `gap`, `wrap`, `combination`, `debug` |
| Grid | `basic`, `gutter`, `offset`, `sort`, `flex`, `flex-align`, `flex-order`, `flex-stretch`, `responsive`, `responsive-flex`, `responsive-more`, `playground`, `usebreakpoint` | — |
| ProLayout | — | `basic`, `top`, `top-side`, `top-side-2`, `side`, `custom-trigger`, `collapsible-overlay`, `responsive`, `fixed`, `fixed-sider`, `custom-trigger-debug`, `component-token` |
| Masonry 6.0.0 | — | `basic`, `responsive`, `image`, `dynamic`, `style-class`, `fresh` |
| Space | — | `base`, `vertical`, `size`, `align`, `wrap`, `separator`, `compact-buttons`, `compact-button-vertical`, `debug`, `gap-in-line` |
| Splitter | `size` | `control`, `vertical`, `collapsible`, `collapsibleicon`, `multiple`, `group`, `lazy`, `customize`, `style-class`, `reset`, `size-mix`, `_semantic` |
| Anchor | — | `basic`, `horizontal`, `static`, `onclick`, `customizehighlight`, `targetoffset`, `targetoffset-per-link`, `onchange`, `replace`, `legacy-anchor`, `style-class`, `component-token` |
| Breadcrumb | `basic` | `separator` |
| Dropdown | — | `basic`, `item`, `dropdown-button`, `sub-menu`, `loading`, `selection`, `menu-full`, `extra`, `arrow`, `arrow-center`, `event`, `custom-dropdown`, `selectable` |
| Menu | — | `horizontal`, `horizontal-dark`, `inline`, `inline-collapsed`, `tooltip`, `sider-current`, `vertical`, `theme`, `submenu-theme`, `switch-mode`, `style-class`, `style-debug`, `menu-v4`, `component-token`, `extra-style`, `extra-collapsed-debug`, `custom-popup-render`, `collapsed-icon-debug` |
| Pagination | — | `basic`, `align`, `more`, `changer`, `jump`, `mini`, `simple`, `controlled`, `total`, `all`, `itemrender`, `components`, `wireframe`, `component-token`, `variant-debug`, `style-class` |
| Steps | — | `simple`, `error`, `vertical`, `clickable`, `panel`, `icon`, `step-next`, `title-placement`, `progress-dot`, `customized-progress-dot`, `nav`, `progress`, `progress-debug`, `steps-in-steps`, `inline`, `inline-variant`, `variant-debug` |
| Tabs | — | `basic`, `disabled`, `centered`, `icon`, `custom-indicator`, `slide`, `extra`, `size`, `placement`, `popuprender-search`, `card`, `editable-card`, `card-top`, `custom-add-trigger`, `custom-tab-bar`, `custom-tab-bar-node`, `style-class`, `animated`, `nest`, `component-token` |
| AutoComplete | — | `basic`, `custom`, `certain-category`, `customize-clear-debug`, `disabled-in-form-debug`, `form-debug`, `render-panel`, `non-case-sensitive`, `uncertain-category`, `style-class`, `disabled-custom-debug`, `filled-custom-debug`, `autocomplete-and-select` |
| Cascader | — | `basic`, `custom-trigger`, `disabled-option`, `multiple`, `size`, `search`, `custom-dropdown`, `variant`, `style-class`, `component-token`, `default-value`, `hover`, `change-on-select`, `showcheckedstrategy`, `lazy`, `suffix`, `placement`, `status` |
| Checkbox | `basic`, `group` | `controller`, `check-all`, `style-class`, `debug-disable-popover`, `custom-line-width`, `disabled`, `layout`, `debug-line`, `debug-group-width`, `_semantic` |
| ColorPicker | — | `controlled`, `disabled-alpha`, `trigger`, `format`, `size`, `disabled`, `allowclear`, `presets` |
| DatePicker | — | `basic`, `needconfirm`, `format`, `mask`, `disabled`, `allow-empty`, `preset-ranges`, `size`, `buddhist-era`, `variant`, `mode`, `suffix`, `component-token`, `range-picker`, `switchable`, `time`, `date-range`, `disabled-date`, `select-in-range`, `extra-footer`, `cell-render`, `external-panel`, `status`, `style-class`, `placement`, `start-end`, `suffixicon-debug` |
| ProForm | — | `basic`, `control-hooks`, `layout`, `layout-multiple`, `disabled`, `variant`, `required-mark`, `size`, `layout-can-wrap`, `warning-only`, `usewatch`, `validate-trigger`, `validate-only`, `form-item-path`, `dynamic-form-item`, `dynamic-form-items`, `dynamic-form-items-drag-sorting`, `dynamic-form-items-no-style`, `dynamic-form-items-complex`, `nest-messages`, `complex-form-control`, `customized-form-controls`, `global-state`, `form-context`, `inline-login`, `login`, `register`, `advanced-search`, `form-in-modal`, `time-related-controls`, `without-form-create`, `validate-static`, `dynamic-rule`, `form-dependencies`, `getvalueprops-normalize`, `validate-scroll-to-field`, `validate-other`, `style-class`, `disabled-input-debug`, `label-debug`, `col-24-debug`, `ref-item`, `custom-feedback-icons`, `component-token` |
| Input | — | `basic`, `variant`, `compact-style`, `search-input`, `textarea`, `otp`, `presuffix`, `allowclear`, `advance-count`, `focus`, `borderless-debug`, `textarea-resize`, `component-token`, `size`, `addon`, `group`, `search-input-loading`, `autosize-textarea`, `tooltip`, `password-input`, `show-count`, `status`, `debug-addon` |
| InputNumber | — | `basic`, `keyboard`, `variant`, `presuffix`, `focus`, `controls`, `size`, `disabled`, `formatter`, `change-on-wheel`, `out-of-range`, `status`, `style-class` |
| Mentions | — | `basic`, `variant`, `form`, `readonly`, `status`, `async`, `prefix`, `placement` |
| Radio | `basic`, `radiobutton` | `radiogroup`, `radiogroup-block`, `size`, `style-class`, `debug-group-width`, `component-token`, `disabled`, `radiogroup-more`, `radiogroup-options`, `radiogroup-with-name`, `radiobutton-solid`, `badge`, `wireframe`, `debug-upload` |
| Rate | `basic`, `half` | `disabled`, `character`, `component-token`, `size`, `text`, `clear`, `character-function` |
| Select | `basic`, `multiple`, `style-class`, `size` | `search-filter-option`, `option-render`, `tags`, `coordinate`, `automatic-tokenization`, `select-users`, `custom-dropdown-menu`, `variant`, `clear-suffix-debug`, `custom-label-render`, `big-data`, `placement`, `debug`, `option-label-center`, `component-token`, `search`, `search-multi-field`, `search-sort`, `optgroup`, `label-in-value`, `custom-tokenization`, `suffix`, `hide-selected`, `filled-debug`, `custom-tag-render`, `responsive`, `status`, `placement-debug`, `render-panel`, `debug-flip-shift`, `maxcount`, `_semantic` |
| Slider | — | `basic`, `icon-slider`, `event`, `vertical`, `reverse`, `multiple`, `disabled-handle`, `component-token`, `input-number`, `tip-formatter`, `mark`, `show-tooltip`, `draggabletrack`, `editable`, `style-class`, `_semantic` |
| Switch | — | `basic`, `loading`, `size` |
| TimePicker | — | `basic`, `size`, `disabled`, `interval-options`, `variant`, `suffix`, `value`, `need-confirm`, `hide-column`, `addon`, `range-picker`, `status`, `style-class` |
| Transfer | `basic` | `oneway`, `search`, `advanced`, `custom-item`, `actions`, `large-data`, `table-transfer`, `tree-transfer`, `status`, `style-class`, `custom-select-all-labels` |
| TreeSelect | — | `basic`, `treedata`, `async`, `placement`, `status`, `suffix`, `multiple`, `checkable`, `treeline`, `variant`, `maxcount`, `style-class` |
| Upload | — | `basic`, `defaultfilelist`, `picture-circle`, `drag`, `directory`, `upload-png-only`, `preview-file`, `transform-file`, `file-type`, `drag-sorting`, `customize-progress-bar`, `component-token`, `avatar`, `picture-card`, `filelist`, `paste`, `upload-manually`, `picture-style`, `max-count`, `upload-with-aliyun-oss`, `upload-custom-action-icon`, `crop-image`, `style-class` |
| Avatar | — | `basic`, `dynamic`, `group`, `toggle-debug`, `fallback`, `type`, `badge`, `max-count`, `responsive`, `component-token` |
| Badge | — | `basic`, `overflow`, `change`, `offset`, `status`, `ribbon`, `title`, `no-wrapper`, `dot`, `link`, `size`, `colorful`, `mix` |
| Calendar | — | `basic`, `notice-calendar`, `card`, `select`, `customize-header` |
| Card | — | `basic`, `border-less`, `simple`, `flexible-content`, `in-column`, `loading`, `grid-card`, `inner`, `tabs`, `meta`, `style-class`, `component-token`, `_semantic`, `_semantic_meta` |
| Carousel | — | `basic` |
| Collapse | `basic`, `collapsible` | `size`, `accordion`, `mix`, `borderless`, `custom`, `icon`, `noarrow`, `extra`, `ghost`, `style-class`, `component-token` |
| ProDescriptions | — | `basic`, `border`, `responsive`, `vertical`, `vertical-border` |
| Empty | — | `basic`, `simple`, `customize`, `config-provider`, `style-class`, `description` |
| Image | — | `basic`, `placeholder`, `fallback`, `preview-group`, `preview-group-visible`, `previewsrc`, `controlled-preview`, `imagerender`, `mask`, `style-class`, `nested` |
| ProList | — | `simple`, `basic`, `loadmore`, `vertical`, `grid`, `infinite-load` |
| Listy 6.6.0 | — | `basic` |
| Popover | — | `basic`, `arrow-point-at-center`, `control`, `triggertype`, `hover-with-click` |
| QRCode | `base`, `status` | `type`, `customcolor`, `errorlevel`, `style-class`, `icon`, `customstatusrender`, `customsize`, `download`, `popover` |
| Segmented | — | `basic`, `block`, `disabled`, `custom`, `size`, `icon-only`, `controlled`, `dynamic`, `with-icon` |
| Statistic | — | `basic`, `unit`, `card` |
| ProTable | — | `basic`, `jsx`, `row-selection`, `row-selection-and-operation`, `row-selection-custom`, `row-selection-debug`, `head`, `filter-in-tree`, `filter-search`, `multiple-sorter`, `reset-filter`, `custom-filter-panel`, `ajax`, `size`, `narrow`, `bordered`, `expand`, `expand-sticky`, `order-column`, `colspan-rowspan`, `tree-data`, `tree-table-ellipsis`, `fixed-header`, `auto-height`, `fixed-columns`, `fixed-gapped-columns`, `fixed-columns-header`, `hidden-columns`, `grouping-columns`, `edit-cell`, `edit-row`, `nested-table`, `drag-sorting`, `drag-column-sorting`, `drag-sorting-handler`, `ellipsis`, `column-defaults`, `ellipsis-custom-tooltip`, `custom-empty`, `summary`, `virtual-list`, `responsive`, `nest-table-border-debug`, `nested-table-in-tabs-debug`, `pagination`, `row-selection-custom-debug`, `sticky`, `dynamic-settings`, `style-class`, `selections-debug`, `component-token`, `measure-row-render` |
| Tag | — | `basic`, `control`, `animation`, `status`, `draggable`, `colorful`, `checkable`, `icon`, `style-class`, `_semantic`, `_semantic_group` |
| Timeline | — | `basic`, `alternate`, `title-span`, `style-class`, `variant`, `horizontal`, `custom`, `title` |
| Tooltip | — | `basic`, `placement`, `colorful`, `arrow`, `auto-adjust-overflow`, `disabled` |
| Tour | — | `basic`, `placement`, `indicator`, `gap`, `non-modal`, `mask`, `actions-render`, `style-class` |
| Tree | — | `basic`, `draggable`, `search`, `customized-icon`, `virtual-scroll`, `basic-controlled`, `dynamic`, `line`, `directory`, `switcher-icon`, `big-data` |
| Alert | — | `basic`, `filled`, `description`, `banner`, `smooth-closed`, `custom-icon`, `custom-title-alignment`, `style-class`, `style`, `closable`, `icon`, `loop-banner`, `error-boundary`, `action`, `component-token`, `_semantic` |
| Drawer | — | `basic-right`, `resizable`, `extra`, `form-in-drawer`, `mask`, `no-mask`, `placement`, `loading`, `user-profile`, `size`, `closable-placement` |
| Message | — | `duration`, `loading`, `info`, `other`, `update` |
| Modal | — | `basic`, `footer`, `loading`, `hooks`, `manual`, `modal-render`, `static-info`, `confirm-router`, `nested`, `custom-mouse-position`, `component-token`, `async`, `mask`, `footer-render`, `locale`, `position`, `button-props`, `width`, `confirm`, `style-class` |
| Notification | — | `with-icon`, `custom-icon`, `update`, `duration`, `with-btn`, `placement`, `basic` |
| Popconfirm | — | `basic`, `dynamic-trigger`, `locale`, `icon` |
| Progress | — | `line`, `line-mini`, `circle-mini`, `format`, `segment`, `gradient-line`, `circle`, `dynamic`, `dashboard`, `linecap`, `steps` |
| Result | — | `success`, `info`, `warning`, `403`, `404`, `500`, `error`, `customicon`, `style-class`, `component-token` |
| Skeleton | — | `basic`, `complex`, `active`, `element`, `children`, `list`, `style-class`, `componenttoken` |
| Spin | — | `basic`, `nested`, `delayanddebounce`, `size`, `tip`, `custom-indicator`, `style-class` |
| Watermark | — | `basic`, `multi-line`, `image`, `custom` |
| Affix | — | `basic`, `target`, `on-change`, `debug` |

首屏机器报告：`docs/antd-v6/runtime-parity-scan-2026-10-30.json`。
交互机器报告：`docs/antd-v6/interaction-parity-scan-2026-10-30.json`.
交互场景清单：`tools/AntDesign.StyleGen/examples/documentation-interaction-parity-all.json`.
