# React / Blazor 文档运行时对齐扫描

扫描时间：2026-10-28T15:53:42.386Z

## 范围与判定

- React: http://127.0.0.1:8000；Blazor: http://127.0.0.1:5000。React demo 名称作为缺漏基准，Blazor 独有 demo 不抵扣缺漏。
- 静态扫描在 viewport 1280×900、light theme 下逐组件进入文档页，滚动触发延迟加载，采集 demo 首屏 Ant Design 节点 HTML、选定 computed CSS 属性与几何位置。
- HTML 快照不一致仅标记为候选；HTML 中可能包含运行时包装、语义差异或无视觉影响的属性。CSS/几何差异按同名 Ant 类节点匹配，1px 内视为相同。
- 下方“交互状态”来自 DOM/style manifests 的实际浏览器操作；未覆盖的 demo 不能据此判为交互样式已对齐。

## 汇总

| 指标 | 数量 |
|---|---:|
| React / Blazor 组件路由 | 74 / 74 |
| 可比组件 | 68 |
| React / Blazor demo | 1098 / 1013 |
| 名称匹配 demo | 801 |
| React 有、Blazor 缺失 | 297 |
| 仅 Blazor 有（不计缺漏） | 212 |
| HTML 首屏快照候选差异 | 741 / 801 |
| computed CSS 候选差异 | 570 / 801 |
| 几何候选差异（>1px） | 535 / 801 |
| 页面采集阻塞 | 3 |

## 组件路由差异

- React 有、Blazor 缺失：`EditableProTable` (/components/editable-table)。
- Blazor 独有：`PageHeader` (/en-US/components/pageheader), `Comment` (/en-US/components/comment), `BackTop` (/en-US/components/backtop)。
- 按扫描配置排除：`FloatButton`, `Masonry 6.0.0`, `Tour`, `BorderBeam 6.4.0`, `Util`。

## React 有但 Blazor 缺失的 demo

仅按 React demo 名称计数；Blazor 额外 demo 不列为缺漏。

| 组件 | 缺失 React demo |
|---|---|
| Anchor | `_semantic` |
| App | `basic`, `config` |
| AutoComplete | `status`, `allowclear`, `options`, `variant`, `_semantic` |
| Badge | `ribbon-debug`, `component-token`, `style-class`, `colorful-with-count-debug`, `_semantic`, `_semantic_ribbon` |
| Breadcrumb | `withparams`, `overlay`, `debug-routes`, `component-token`, `withicon`, `separator-component`, `style-class`, `_semantic` |
| Calendar | `event-range`, `lunar`, `week`, `style-class`, `component-token`, `_semantic` |
| Card | `no-body-debug`, `button-alignment-debug` |
| Carousel | `autoplay`, `arrows`, `component-token`, `placement`, `fade`, `dot-duration` |
| Cascader | `fields-name`, `ellipsis-debug`, `custom-render`, `panel`, `render-panel`, `_semantic` |
| Checkbox | `controller`, `check-all`, `style-class`, `debug-disable-popover`, `custom-line-width`, `disabled`, `debug-line`, `debug-group-width`, `_semantic` |
| Collapse | `_semantic` |
| ColorPicker | `base`, `text-render`, `presets-line-gradient`, `style-class`, `line-gradient`, `trigger-event`, `panel-render`, `pure-panel`, `_semantic` |
| ConfigProvider | `locale`, `direction`, `direction`, `size`, `theme`, `wave`, `holderrender`, `prefixcls`, `useconfig`, `warning`, `focus-outline-debug` |
| DatePicker | `multiple`, `components`, `filled-debug`, `multiple-debug`, `render-panel`, `_semantic` |
| ProDescriptions | `text`, `padding`, `size`, `style`, `style-class`, `jsx`, `component-token`, `block`, `_semantic` |
| Drawer | `multi-level-drawer`, `style-class`, `scroll-debug`, `render-in-current`, `config-provider`, `render-panel`, `component-token`, `_semantic` |
| Dropdown | `placement`, `trigger`, `overlay-open`, `icon-debug`, `sub-menu-debug`, `context-menu`, `style-class`, `render-panel`, `_semantic` |
| Empty | `_semantic` |
| Flex | `debug` |
| ProForm | `_semantic` |
| Image | `toolbarrender`, `preview-mask`, `coverplacement`, `preview-group-top-progress`, `component-token`, `preview-imginfo`, `_semantic` |
| Input | `filled-debug`, `search-variant-debug`, `style-class`, `align`, `_semantic_input`, `_semantic_textarea`, `_semantic_search`, `_semantic_password`, `_semantic_otp` |
| InputNumber | `addon`, `digit`, `disabled-hover-debug`, `borderless-height-debug`, `debug-token`, `spinner`, `filled-debug`, `render-panel`, `_semantic` |
| ProLayout | `_semantic` |
| ProList | `pagination`, `grid-test`, `responsive`, `drag-sorting`, `drag-sorting-handler`, `grid-drag-sorting`, `grid-drag-sorting-handler`, `virtual-list`, `component-token`, `spin-debug`, `_semantic` |
| Listy 6.6.0 | `virtual`, `group`, `rich`, `drag-sorting`, `infinite`, `style-class`, `scroll-to`, `_semantic` |
| Mentions | `popuprender`, `autosize`, `render-panel`, `size`, `allowclear`, `autosize-textarea-debug`, `style-class`, `component-token`, `_semantic` |
| Menu | `_semantic` |
| Message | `hooks`, `style-class`, `component-token`, `stack`, `thenable`, `render-panel`, `_semantic` |
| Modal | `dark`, `render-panel`, `wireframe`, `_semantic` |
| Notification | `hooks`, `show-with-progress`, `progress-color`, `render-panel`, `stack`, `component-token`, `style-class`, `_semantic` |
| Pagination | `_semantic` |
| Popconfirm | `placement`, `async`, `style-class`, `wireframe`, `shift`, `promise`, `render-panel`, `_semantic` |
| Popover | `placement`, `style-class`, `wireframe`, `arrow`, `shift`, `render-panel`, `component-token`, `_semantic` |
| Progress | `circle-steps`, `info-position`, `circle-micro`, `size`, `style-class`, `_semantic` |
| QRCode | `_semantic` |
| Radio | `_semantic` |
| Result | `_semantic` |
| Segmented | `style-class`, `size-consistent`, `vertical`, `shape`, `with-name`, `controlled-two`, `componenttoken`, `_semantic` |
| Skeleton | `_semantic`, `_semantic_element` |
| Space | `compact`, `compact-debug`, `compact-nested`, `style-class`, `component-token`, `_semantic` |
| Spin | `percent`, `fullscreen`, `list-debug`, `_semantic` |
| Splitter | `nested-in-tabs`, `debug` |
| Statistic | `animated`, `timer`, `component-token`, `style-class`, `_semantic` |
| Steps | `max-count`, `style-class`, `component-token`, `_semantic`, `_semantic_items` |
| Switch | `text`, `style-class`, `disabled`, `component-token`, `_semantic` |
| ProTable | `_semantic` |
| Tabs | `_semantic` |
| Tag | `disabled`, `customize`, `component-token` |
| TimePicker | `12hours`, `colored-popup`, `render-panel`, `change-on-scroll`, `_semantic` |
| Timeline | `pending`, `horizontal-debug`, `end`, `pending-legacy`, `semantic`, `component-token`, `_semantic`, `_semantic_items` |
| Tooltip | `shift`, `force-align`, `debug`, `disabled-children`, `style-class`, `smooth-transition`, `destroy-on-close`, `render-panel`, `wrap-custom-component`, `_semantic` |
| Transfer | `component-token`, `_semantic` |
| Tree | `directory-debug`, `drag-debug`, `block-node`, `multiple-line`, `line-debug`, `scroll-to`, `component-token`, `style-class`, `_semantic` |
| TreeSelect | `render-panel`, `component-token`, `_semantic` |
| Typography | `title`, `paragraph-debug`, `editable`, `ellipsis-debug`, `componenttoken-debug`, `link-danger-debug`, `_semantic` |
| Upload | `debug-disabled`, `_semantic` |
| Watermark | `portal`, `debug` |

## 首屏 HTML 快照候选差异

以下名称的采集快照不完全相同，尚未人工确认每项都是实际缺陷。

| 组件 | 候选 demo |
|---|---|
| Affix | `basic`, `on-change`, `debug` |
| Alert | `basic`, `filled`, `description`, `smooth-closed`, `custom-icon`, `custom-title-alignment`, `style-class`, `style`, `closable`, `icon`, `loop-banner`, `error-boundary`, `action`, `component-token`, `_semantic` |
| Anchor | `basic`, `horizontal`, `static`, `onclick`, `customizehighlight`, `targetoffset`, `targetoffset-per-link`, `onchange`, `replace`, `legacy-anchor`, `style-class`, `component-token` |
| AutoComplete | `basic`, `custom`, `certain-category`, `customize-clear-debug`, `disabled-in-form-debug`, `form-debug`, `render-panel`, `non-case-sensitive`, `uncertain-category`, `style-class`, `disabled-custom-debug`, `filled-custom-debug`, `autocomplete-and-select` |
| Avatar | `basic`, `dynamic`, `group`, `toggle-debug`, `fallback`, `type`, `badge`, `max-count`, `responsive`, `component-token` |
| Badge | `basic`, `overflow`, `change`, `offset`, `status`, `ribbon`, `title`, `no-wrapper`, `dot`, `link`, `size`, `colorful`, `mix` |
| Breadcrumb | `separator` |
| Button | `debug-color-variant`, `icon-placement`, `debug-block`, `disabled`, `multiple`, `danger`, `legacy-group`, `component-token`, `wave`, `style-class`, `color-variant`, `icon`, `debug-icon`, `size`, `loading`, `ghost`, `block`, `chinese-chars-loading`, `linear-gradient`, `custom-disabled-bg`, `_semantic` |
| Calendar | `basic`, `notice-calendar`, `card`, `select`, `customize-header` |
| Card | `basic`, `border-less`, `simple`, `flexible-content`, `in-column`, `loading`, `grid-card`, `inner`, `tabs`, `meta`, `style-class`, `component-token`, `_semantic`, `_semantic_meta` |
| Carousel | `basic` |
| Cascader | `basic`, `custom-trigger`, `disabled-option`, `multiple`, `size`, `search`, `custom-dropdown`, `variant`, `style-class`, `component-token`, `default-value`, `hover`, `change-on-select`, `showcheckedstrategy`, `lazy`, `suffix`, `placement`, `status` |
| Checkbox | `basic`, `group`, `layout` |
| Collapse | `basic`, `size`, `borderless`, `custom`, `icon`, `noarrow`, `extra`, `ghost`, `collapsible`, `style-class`, `component-token` |
| ColorPicker | `controlled`, `disabled-alpha`, `trigger`, `format`, `size`, `disabled`, `allowclear`, `presets` |
| DatePicker | `basic`, `needconfirm`, `format`, `mask`, `disabled`, `allow-empty`, `preset-ranges`, `size`, `buddhist-era`, `variant`, `mode`, `suffix`, `component-token`, `range-picker`, `switchable`, `time`, `date-range`, `disabled-date`, `select-in-range`, `extra-footer`, `cell-render`, `external-panel`, `status`, `style-class`, `placement`, `start-end`, `suffixicon-debug` |
| ProDescriptions | `basic`, `border`, `responsive`, `vertical`, `vertical-border` |
| Divider | `component-token`, `style-class`, `with-text`, `customize-style`, `variant`, `_semantic` |
| Drawer | `basic-right`, `resizable`, `extra`, `form-in-drawer`, `mask`, `no-mask`, `placement`, `loading`, `user-profile`, `size`, `closable-placement` |
| Dropdown | `basic`, `item`, `dropdown-button`, `sub-menu`, `loading`, `selection`, `menu-full`, `extra`, `arrow`, `arrow-center`, `event`, `custom-dropdown`, `selectable` |
| Empty | `basic`, `simple`, `customize`, `config-provider`, `style-class`, `description` |
| Flex | `basic`, `align`, `gap`, `combination` |
| ProForm | `basic`, `control-hooks`, `layout`, `layout-multiple`, `disabled`, `variant`, `required-mark`, `size`, `layout-can-wrap`, `warning-only`, `usewatch`, `validate-trigger`, `validate-only`, `form-item-path`, `dynamic-form-item`, `dynamic-form-items`, `dynamic-form-items-drag-sorting`, `dynamic-form-items-no-style`, `dynamic-form-items-complex`, `nest-messages`, `complex-form-control`, `customized-form-controls`, `global-state`, `form-context`, `inline-login`, `login`, `register`, `advanced-search`, `time-related-controls`, `without-form-create`, `validate-static`, `dynamic-rule`, `form-dependencies`, `getvalueprops-normalize`, `validate-scroll-to-field`, `validate-other`, `style-class`, `disabled-input-debug`, `label-debug`, `col-24-debug`, `ref-item`, `custom-feedback-icons`, `component-token` |
| Grid | `playground`, `usebreakpoint` |
| Image | `basic`, `placeholder`, `fallback`, `preview-group`, `preview-group-visible`, `previewsrc`, `controlled-preview`, `imagerender`, `mask`, `style-class`, `nested` |
| Input | `basic`, `variant`, `compact-style`, `search-input`, `textarea`, `otp`, `presuffix`, `allowclear`, `advance-count`, `focus`, `borderless-debug`, `textarea-resize`, `component-token`, `size`, `addon`, `group`, `search-input-loading`, `autosize-textarea`, `tooltip`, `password-input`, `show-count`, `status`, `debug-addon` |
| InputNumber | `basic`, `keyboard`, `variant`, `presuffix`, `focus`, `controls`, `size`, `disabled`, `formatter`, `change-on-wheel`, `out-of-range`, `status`, `style-class` |
| ProLayout | `basic`, `top`, `top-side`, `top-side-2`, `custom-trigger`, `collapsible-overlay`, `responsive`, `custom-trigger-debug`, `component-token` |
| ProList | `simple`, `basic`, `loadmore`, `vertical`, `grid`, `infinite-load` |
| Listy 6.6.0 | `basic` |
| Mentions | `basic`, `variant`, `form`, `readonly`, `status`, `async`, `prefix`, `placement` |
| Menu | `horizontal`, `horizontal-dark`, `inline`, `inline-collapsed`, `tooltip`, `sider-current`, `vertical`, `theme`, `submenu-theme`, `switch-mode`, `style-class`, `style-debug`, `menu-v4`, `component-token`, `extra-style`, `extra-collapsed-debug`, `custom-popup-render`, `collapsed-icon-debug` |
| Message | `info`, `update` |
| Modal | `hooks`, `modal-render`, `static-info`, `nested`, `component-token`, `mask`, `footer-render`, `locale`, `width`, `confirm`, `style-class` |
| Notification | `with-icon`, `with-btn`, `placement` |
| Pagination | `basic`, `align`, `more`, `changer`, `jump`, `mini`, `simple`, `controlled`, `total`, `all`, `itemrender`, `components`, `wireframe`, `component-token`, `variant-debug`, `style-class` |
| Popconfirm | `basic`, `dynamic-trigger`, `locale`, `icon` |
| Popover | `basic`, `arrow-point-at-center`, `triggertype`, `hover-with-click` |
| Progress | `line`, `line-mini`, `circle-mini`, `format`, `segment`, `gradient-line`, `circle`, `dynamic`, `dashboard`, `linecap`, `steps` |
| QRCode | `base`, `status`, `type`, `customcolor`, `errorlevel`, `style-class`, `icon`, `customstatusrender`, `customsize`, `download` |
| Radio | `radiogroup`, `radiogroup-block`, `radiobutton`, `size`, `style-class`, `debug-group-width`, `component-token`, `disabled`, `radiogroup-more`, `radiogroup-options`, `radiogroup-with-name`, `radiobutton-solid`, `badge`, `wireframe`, `debug-upload` |
| Rate | `character`, `component-token`, `size`, `text`, `clear`, `character-function` |
| Result | `success`, `info`, `warning`, `403`, `404`, `500`, `error`, `customicon`, `style-class`, `component-token` |
| Segmented | `basic`, `block`, `disabled`, `custom`, `size`, `icon-only`, `controlled`, `dynamic`, `with-icon` |
| Select | `basic`, `search-filter-option`, `multiple`, `option-render`, `tags`, `coordinate`, `automatic-tokenization`, `select-users`, `custom-dropdown-menu`, `variant`, `clear-suffix-debug`, `custom-label-render`, `big-data`, `placement`, `debug`, `option-label-center`, `component-token`, `style-class`, `search`, `search-multi-field`, `size`, `search-sort`, `optgroup`, `label-in-value`, `custom-tokenization`, `suffix`, `hide-selected`, `filled-debug`, `custom-tag-render`, `responsive`, `status`, `placement-debug`, `render-panel`, `debug-flip-shift`, `maxcount`, `_semantic` |
| Skeleton | `basic`, `complex`, `active`, `element`, `children`, `list`, `style-class`, `componenttoken` |
| Slider | `basic`, `icon-slider`, `event`, `vertical`, `reverse`, `multiple`, `disabled-handle`, `component-token`, `input-number`, `tip-formatter`, `mark`, `show-tooltip`, `draggabletrack`, `editable`, `style-class`, `_semantic` |
| Space | `base`, `vertical`, `size`, `align`, `wrap`, `separator`, `compact-buttons`, `compact-button-vertical`, `debug`, `gap-in-line` |
| Spin | `nested`, `delayanddebounce`, `size`, `tip`, `custom-indicator`, `style-class` |
| Splitter | `control`, `vertical`, `collapsible`, `collapsibleicon`, `multiple`, `group`, `lazy`, `customize`, `style-class`, `reset`, `size-mix`, `_semantic` |
| Statistic | `basic`, `unit`, `card` |
| Steps | `simple`, `error`, `vertical`, `clickable`, `panel`, `icon`, `step-next`, `title-placement`, `progress-dot`, `customized-progress-dot`, `nav`, `progress`, `progress-debug`, `steps-in-steps`, `inline`, `inline-variant`, `variant-debug` |
| Switch | `basic`, `loading`, `size` |
| ProTable | `basic`, `jsx`, `row-selection`, `row-selection-and-operation`, `row-selection-custom`, `row-selection-debug`, `head`, `filter-in-tree`, `filter-search`, `multiple-sorter`, `reset-filter`, `custom-filter-panel`, `ajax`, `size`, `narrow`, `bordered`, `expand`, `expand-sticky`, `order-column`, `colspan-rowspan`, `tree-data`, `tree-table-ellipsis`, `fixed-header`, `auto-height`, `fixed-columns`, `fixed-gapped-columns`, `fixed-columns-header`, `hidden-columns`, `grouping-columns`, `edit-cell`, `edit-row`, `nested-table`, `drag-sorting`, `drag-column-sorting`, `drag-sorting-handler`, `ellipsis`, `column-defaults`, `ellipsis-custom-tooltip`, `custom-empty`, `summary`, `virtual-list`, `responsive`, `nest-table-border-debug`, `nested-table-in-tabs-debug`, `pagination`, `row-selection-custom-debug`, `sticky`, `dynamic-settings`, `style-class`, `selections-debug`, `component-token`, `measure-row-render` |
| Tabs | `basic`, `disabled`, `centered`, `icon`, `custom-indicator`, `slide`, `extra`, `size`, `placement`, `popuprender-search`, `card`, `editable-card`, `card-top`, `custom-add-trigger`, `custom-tab-bar`, `custom-tab-bar-node`, `style-class`, `animated`, `nest`, `component-token` |
| Tag | `basic`, `control`, `animation`, `status`, `draggable`, `colorful`, `checkable`, `icon`, `style-class`, `_semantic`, `_semantic_group` |
| TimePicker | `basic`, `size`, `disabled`, `interval-options`, `variant`, `suffix`, `value`, `need-confirm`, `hide-column`, `addon`, `range-picker`, `status`, `style-class` |
| Timeline | `basic`, `alternate`, `title-span`, `style-class`, `variant`, `horizontal`, `custom`, `title` |
| Tooltip | `placement`, `colorful`, `arrow`, `auto-adjust-overflow`, `disabled` |
| Transfer | `basic`, `oneway`, `search`, `advanced`, `custom-item`, `actions`, `large-data`, `table-transfer`, `tree-transfer`, `status`, `style-class`, `custom-select-all-labels` |
| Tree | `basic`, `draggable`, `search`, `customized-icon`, `virtual-scroll`, `basic-controlled`, `dynamic`, `line`, `directory`, `switcher-icon`, `big-data` |
| TreeSelect | `basic`, `treedata`, `async`, `placement`, `status`, `suffix`, `multiple`, `checkable`, `treeline`, `variant`, `maxcount`, `style-class` |
| Typography | `basic`, `text`, `copyable`, `ellipsis`, `ellipsis-controlled`, `ellipsis-middle`, `suffix` |
| Upload | `basic`, `defaultfilelist`, `picture-circle`, `drag`, `directory`, `upload-png-only`, `preview-file`, `transform-file`, `file-type`, `drag-sorting`, `customize-progress-bar`, `component-token`, `avatar`, `picture-card`, `filelist`, `paste`, `upload-manually`, `picture-style`, `max-count`, `upload-with-aliyun-oss`, `upload-custom-action-icon`, `crop-image`, `style-class` |
| Watermark | `custom` |

## 首屏 CSS 与几何候选差异

| 组件 | CSS 属性候选差异 | 几何候选差异（>1px） |
|---|---|---|
| Affix | — | `basic`, `on-change`, `debug` |
| Alert | `basic`, `filled`, `description`, `custom-icon`, `custom-title-alignment`, `style-class`, `style`, `closable`, `icon`, `loop-banner`, `error-boundary`, `action`, `component-token`, `_semantic` | `description`, `custom-icon`, `style`, `closable`, `icon`, `loop-banner`, `error-boundary`, `action`, `_semantic` |
| Anchor | `static`, `onclick`, `customizehighlight`, `onchange`, `legacy-anchor` | `static`, `onclick`, `customizehighlight`, `onchange`, `legacy-anchor` |
| AutoComplete | `certain-category`, `disabled-in-form-debug`, `form-debug`, `render-panel`, `uncertain-category`, `disabled-custom-debug`, `filled-custom-debug`, `autocomplete-and-select` | `certain-category`, `disabled-in-form-debug`, `form-debug`, `render-panel`, `uncertain-category`, `disabled-custom-debug`, `filled-custom-debug`, `autocomplete-and-select` |
| Avatar | `dynamic`, `group`, `toggle-debug`, `fallback`, `type`, `badge`, `max-count`, `component-token` | `dynamic`, `group`, `toggle-debug`, `fallback`, `type`, `badge`, `max-count`, `component-token` |
| Badge | `basic`, `overflow`, `change`, `offset`, `status`, `ribbon`, `title`, `no-wrapper`, `dot`, `link`, `size`, `colorful`, `mix` | `basic`, `overflow`, `change`, `offset`, `status`, `ribbon`, `title`, `no-wrapper`, `dot`, `link`, `size`, `colorful`, `mix` |
| Breadcrumb | `separator` | `separator` |
| Button | `debug-color-variant`, `icon-placement`, `disabled`, `multiple`, `danger`, `legacy-group`, `component-token`, `wave`, `style-class`, `color-variant`, `icon`, `debug-icon`, `size`, `loading`, `ghost`, `block`, `chinese-chars-loading`, `linear-gradient`, `custom-disabled-bg`, `_semantic` | `debug-color-variant`, `icon-placement`, `disabled`, `multiple`, `danger`, `legacy-group`, `component-token`, `wave`, `style-class`, `color-variant`, `icon`, `debug-icon`, `size`, `loading`, `ghost`, `block`, `chinese-chars-loading`, `custom-disabled-bg`, `_semantic` |
| Calendar | `card`, `select`, `customize-header` | `notice-calendar`, `card`, `select`, `customize-header` |
| Card | `basic`, `flexible-content`, `in-column`, `loading`, `inner`, `tabs`, `meta`, `style-class`, `component-token`, `_semantic`, `_semantic_meta` | `basic`, `border-less`, `flexible-content`, `in-column`, `loading`, `inner`, `tabs`, `meta`, `style-class`, `component-token`, `_semantic`, `_semantic_meta` |
| Cascader | `basic`, `multiple`, `custom-dropdown`, `style-class`, `component-token`, `showcheckedstrategy`, `suffix`, `placement`, `status` | `basic`, `multiple`, `custom-dropdown`, `variant`, `style-class`, `component-token`, `showcheckedstrategy`, `suffix`, `placement`, `status` |
| Checkbox | `layout` | — |
| Collapse | `size`, `custom`, `icon`, `ghost`, `collapsible`, `style-class`, `component-token` | `size`, `custom`, `icon`, `ghost`, `style-class` |
| ColorPicker | `trigger`, `format`, `size` | `controlled`, `trigger`, `format`, `size` |
| DatePicker | `format`, `mask`, `disabled`, `allow-empty`, `preset-ranges`, `size`, `buddhist-era`, `variant`, `mode`, `suffix`, `component-token`, `range-picker`, `switchable`, `time`, `disabled-date`, `extra-footer`, `cell-render`, `status`, `style-class`, `placement`, `start-end`, `suffixicon-debug` | `format`, `mask`, `disabled`, `allow-empty`, `preset-ranges`, `size`, `buddhist-era`, `variant`, `mode`, `suffix`, `component-token`, `range-picker`, `switchable`, `time`, `disabled-date`, `extra-footer`, `cell-render`, `status`, `style-class`, `placement`, `suffixicon-debug` |
| ProDescriptions | `basic`, `border`, `responsive`, `vertical`, `vertical-border` | `basic`, `border`, `responsive`, `vertical`, `vertical-border` |
| Divider | `component-token`, `style-class`, `with-text`, `plain`, `customize-style`, `variant`, `_semantic` | `size`, `vertical`, `component-token`, `style-class`, `with-text`, `customize-style`, `_semantic` |
| Drawer | `resizable`, `extra`, `mask`, `no-mask`, `placement`, `user-profile`, `size`, `closable-placement` | `resizable`, `extra`, `no-mask`, `placement`, `user-profile`, `size`, `closable-placement` |
| Dropdown | `dropdown-button`, `loading`, `selection`, `arrow`, `arrow-center` | `dropdown-button`, `loading`, `selection`, `arrow`, `arrow-center` |
| Empty | `style-class` | `style-class` |
| Flex | `gap`, `combination` | `gap`, `combination` |
| ProForm | `basic`, `control-hooks`, `layout`, `layout-multiple`, `disabled`, `variant`, `required-mark`, `size`, `layout-can-wrap`, `warning-only`, `usewatch`, `validate-trigger`, `validate-only`, `form-item-path`, `dynamic-form-item`, `dynamic-form-items`, `dynamic-form-items-drag-sorting`, `dynamic-form-items-no-style`, `dynamic-form-items-complex`, `nest-messages`, `complex-form-control`, `customized-form-controls`, `global-state`, `form-context`, `inline-login`, `login`, `advanced-search`, `time-related-controls`, `without-form-create`, `validate-static`, `dynamic-rule`, `form-dependencies`, `validate-other`, `style-class`, `disabled-input-debug`, `label-debug`, `ref-item`, `custom-feedback-icons`, `component-token` | `basic`, `control-hooks`, `layout`, `layout-multiple`, `disabled`, `variant`, `required-mark`, `size`, `layout-can-wrap`, `warning-only`, `usewatch`, `validate-trigger`, `validate-only`, `form-item-path`, `dynamic-form-item`, `dynamic-form-items`, `dynamic-form-items-drag-sorting`, `dynamic-form-items-no-style`, `dynamic-form-items-complex`, `nest-messages`, `complex-form-control`, `customized-form-controls`, `global-state`, `form-context`, `inline-login`, `login`, `advanced-search`, `time-related-controls`, `without-form-create`, `validate-static`, `dynamic-rule`, `form-dependencies`, `validate-other`, `style-class`, `disabled-input-debug`, `label-debug`, `ref-item`, `custom-feedback-icons`, `component-token` |
| Grid | `playground` | `playground` |
| Image | `basic`, `placeholder`, `fallback`, `preview-group`, `preview-group-visible`, `previewsrc`, `controlled-preview`, `imagerender`, `mask`, `style-class` | `placeholder`, `preview-group`, `controlled-preview`, `mask`, `style-class` |
| Input | `variant`, `search-input`, `otp`, `presuffix`, `allowclear`, `advance-count`, `focus`, `borderless-debug`, `textarea-resize`, `component-token`, `size`, `addon`, `group`, `tooltip`, `password-input`, `show-count`, `status`, `debug-addon` | `variant`, `search-input`, `otp`, `presuffix`, `advance-count`, `focus`, `borderless-debug`, `textarea-resize`, `component-token`, `size`, `addon`, `group`, `tooltip`, `password-input`, `show-count`, `debug-addon` |
| InputNumber | `keyboard`, `presuffix`, `focus`, `controls`, `out-of-range`, `status`, `style-class` | `presuffix`, `size`, `formatter`, `out-of-range`, `status`, `style-class` |
| ProLayout | `basic`, `top`, `top-side`, `top-side-2`, `custom-trigger`, `collapsible-overlay`, `responsive`, `custom-trigger-debug`, `component-token` | `basic`, `top`, `top-side`, `top-side-2`, `custom-trigger`, `collapsible-overlay`, `responsive`, `custom-trigger-debug`, `component-token` |
| ProList | `simple`, `basic`, `loadmore`, `vertical`, `grid` | `simple`, `basic`, `loadmore`, `vertical` |
| Listy 6.6.0 | `basic` | `basic` |
| Mentions | `form` | `form` |
| Menu | `horizontal`, `horizontal-dark`, `inline`, `inline-collapsed`, `tooltip`, `sider-current`, `vertical`, `theme`, `submenu-theme`, `switch-mode`, `style-class`, `style-debug`, `menu-v4`, `component-token`, `extra-style`, `extra-collapsed-debug`, `custom-popup-render`, `collapsed-icon-debug` | `horizontal`, `horizontal-dark`, `inline`, `inline-collapsed`, `tooltip`, `sider-current`, `vertical`, `theme`, `submenu-theme`, `switch-mode`, `style-class`, `style-debug`, `menu-v4`, `component-token`, `extra-style`, `extra-collapsed-debug`, `custom-popup-render`, `collapsed-icon-debug` |
| Message | `info`, `update` | `info`, `update` |
| Modal | `hooks`, `static-info`, `component-token`, `mask`, `footer-render`, `width`, `confirm`, `style-class` | `component-token`, `footer-render`, `locale`, `width`, `confirm`, `style-class` |
| Notification | `with-btn`, `placement` | `with-btn`, `placement` |
| Pagination | `more`, `changer`, `jump`, `mini`, `all`, `itemrender`, `components`, `wireframe`, `component-token`, `variant-debug`, `style-class` | `more`, `jump`, `mini`, `all`, `wireframe`, `component-token`, `variant-debug`, `style-class` |
| Popconfirm | — | `dynamic-trigger` |
| Popover | `basic`, `control`, `triggertype`, `hover-with-click` | `triggertype`, `hover-with-click` |
| Progress | `line`, `line-mini`, `circle-mini`, `format`, `segment`, `gradient-line`, `circle`, `dynamic`, `dashboard`, `linecap`, `steps` | `line`, `line-mini`, `circle-mini`, `format`, `segment`, `gradient-line`, `circle`, `dynamic`, `dashboard`, `linecap`, `steps` |
| QRCode | `status`, `customstatusrender`, `popover` | `status`, `customstatusrender` |
| Radio | `radiogroup`, `size`, `style-class`, `component-token`, `disabled`, `radiogroup-more`, `radiobutton-solid`, `badge`, `wireframe`, `debug-upload` | `radiogroup`, `size`, `style-class`, `component-token`, `disabled`, `radiogroup-more`, `radiogroup-options`, `badge`, `wireframe` |
| Rate | `character`, `component-token`, `size`, `text`, `clear`, `character-function` | `character`, `component-token`, `text`, `clear` |
| Result | `403`, `404`, `500`, `style-class`, `component-token` | `success`, `403`, `404`, `500`, `error`, `style-class`, `component-token` |
| Segmented | `basic`, `block`, `disabled`, `custom`, `size`, `icon-only`, `controlled`, `dynamic`, `with-icon` | `disabled`, `custom`, `size`, `dynamic`, `with-icon` |
| Select | `multiple`, `placement`, `debug`, `option-label-center`, `component-token`, `size`, `custom-tokenization`, `suffix`, `hide-selected`, `filled-debug`, `custom-tag-render`, `responsive`, `status`, `placement-debug`, `render-panel`, `maxcount`, `_semantic` | `coordinate`, `placement`, `option-label-center`, `component-token`, `suffix`, `filled-debug`, `custom-tag-render`, `responsive`, `placement-debug`, `render-panel`, `maxcount`, `_semantic` |
| Skeleton | `basic`, `complex`, `active`, `children`, `list`, `style-class`, `componenttoken` | `complex`, `list`, `style-class` |
| Slider | `basic`, `icon-slider`, `event`, `vertical`, `reverse`, `multiple`, `disabled-handle`, `component-token`, `input-number`, `tip-formatter`, `mark`, `show-tooltip`, `draggabletrack`, `editable`, `style-class`, `_semantic` | `basic`, `icon-slider`, `event`, `vertical`, `reverse`, `multiple`, `disabled-handle`, `component-token`, `input-number`, `mark`, `show-tooltip`, `editable`, `style-class`, `_semantic` |
| Space | `base`, `vertical`, `size`, `align`, `compact-button-vertical` | `base`, `vertical`, `size`, `align`, `compact-buttons`, `compact-button-vertical`, `gap-in-line` |
| Spin | `basic`, `nested`, `delayanddebounce`, `size`, `tip`, `custom-indicator`, `style-class` | `basic`, `nested`, `delayanddebounce`, `size`, `tip`, `custom-indicator`, `style-class` |
| Splitter | `size`, `control`, `vertical`, `collapsible`, `collapsibleicon`, `multiple`, `group`, `lazy`, `customize`, `style-class`, `reset`, `size-mix`, `_semantic` | `vertical`, `collapsible`, `multiple`, `group`, `lazy`, `customize`, `style-class`, `reset`, `size-mix`, `_semantic` |
| Statistic | `basic`, `unit`, `card` | `basic`, `unit`, `card` |
| Steps | `simple`, `error`, `vertical`, `clickable`, `panel`, `step-next`, `title-placement`, `progress-dot`, `nav`, `progress`, `progress-debug`, `steps-in-steps`, `inline`, `inline-variant`, `variant-debug` | `simple`, `error`, `vertical`, `clickable`, `panel`, `step-next`, `title-placement`, `progress-dot`, `nav`, `progress`, `progress-debug`, `steps-in-steps`, `inline`, `inline-variant`, `variant-debug` |
| Switch | `basic`, `loading` | `basic`, `loading` |
| ProTable | `basic`, `jsx`, `row-selection`, `row-selection-and-operation`, `row-selection-custom`, `row-selection-debug`, `head`, `filter-in-tree`, `filter-search`, `multiple-sorter`, `reset-filter`, `custom-filter-panel`, `ajax`, `size`, `narrow`, `bordered`, `expand`, `expand-sticky`, `order-column`, `colspan-rowspan`, `tree-data`, `tree-table-ellipsis`, `fixed-header`, `fixed-columns`, `fixed-gapped-columns`, `fixed-columns-header`, `hidden-columns`, `grouping-columns`, `edit-cell`, `edit-row`, `nested-table`, `drag-sorting`, `drag-column-sorting`, `drag-sorting-handler`, `ellipsis`, `column-defaults`, `ellipsis-custom-tooltip`, `custom-empty`, `summary`, `virtual-list`, `responsive`, `nest-table-border-debug`, `nested-table-in-tabs-debug`, `pagination`, `row-selection-custom-debug`, `sticky`, `dynamic-settings`, `style-class`, `selections-debug`, `component-token`, `measure-row-render` | `basic`, `jsx`, `row-selection`, `row-selection-and-operation`, `row-selection-custom`, `row-selection-debug`, `head`, `filter-in-tree`, `filter-search`, `multiple-sorter`, `reset-filter`, `custom-filter-panel`, `ajax`, `size`, `narrow`, `bordered`, `expand`, `expand-sticky`, `order-column`, `colspan-rowspan`, `tree-data`, `tree-table-ellipsis`, `fixed-header`, `fixed-columns`, `fixed-gapped-columns`, `fixed-columns-header`, `hidden-columns`, `grouping-columns`, `edit-cell`, `edit-row`, `nested-table`, `drag-sorting`, `drag-column-sorting`, `drag-sorting-handler`, `ellipsis`, `column-defaults`, `ellipsis-custom-tooltip`, `custom-empty`, `summary`, `virtual-list`, `responsive`, `nest-table-border-debug`, `nested-table-in-tabs-debug`, `pagination`, `row-selection-custom-debug`, `sticky`, `dynamic-settings`, `style-class`, `selections-debug`, `component-token`, `measure-row-render` |
| Tabs | `basic`, `disabled`, `centered`, `icon`, `custom-indicator`, `slide`, `extra`, `size`, `placement`, `popuprender-search`, `card`, `editable-card`, `card-top`, `custom-add-trigger`, `custom-tab-bar`, `custom-tab-bar-node`, `style-class`, `animated`, `component-token` | `basic`, `disabled`, `centered`, `icon`, `custom-indicator`, `slide`, `extra`, `size`, `placement`, `popuprender-search`, `card`, `editable-card`, `card-top`, `custom-add-trigger`, `custom-tab-bar`, `custom-tab-bar-node`, `style-class`, `animated`, `component-token` |
| Tag | `basic`, `control`, `animation`, `status`, `colorful`, `checkable`, `_semantic` | `basic`, `control`, `animation`, `status`, `colorful`, `checkable`, `_semantic`, `_semantic_group` |
| TimePicker | `basic`, `size`, `disabled`, `value`, `need-confirm`, `hide-column`, `addon` | `basic`, `size`, `disabled`, `value`, `need-confirm`, `hide-column`, `addon` |
| Timeline | `basic`, `alternate`, `title-span`, `style-class`, `variant`, `horizontal`, `custom`, `title` | `basic`, `alternate`, `title-span`, `style-class`, `variant`, `horizontal`, `title` |
| Tooltip | `placement`, `colorful`, `arrow`, `disabled` | `placement`, `colorful`, `arrow`, `disabled` |
| Transfer | `basic`, `oneway`, `search`, `advanced`, `custom-item`, `actions`, `large-data`, `table-transfer`, `tree-transfer`, `status`, `style-class` | `oneway`, `search`, `advanced`, `custom-item`, `actions`, `large-data`, `table-transfer`, `tree-transfer`, `status`, `style-class` |
| Tree | `basic`, `draggable`, `search`, `customized-icon`, `basic-controlled`, `dynamic`, `line`, `directory`, `switcher-icon` | `basic`, `draggable`, `search`, `customized-icon`, `basic-controlled`, `dynamic`, `line`, `directory`, `switcher-icon` |
| TreeSelect | `treedata`, `placement`, `suffix`, `multiple`, `checkable`, `treeline`, `variant`, `maxcount`, `style-class` | `treedata`, `placement`, `suffix`, `multiple`, `checkable`, `treeline`, `variant`, `maxcount`, `style-class` |
| Typography | `basic`, `ellipsis-controlled`, `suffix` | `basic`, `text`, `copyable`, `ellipsis`, `ellipsis-controlled`, `suffix` |
| Upload | `basic`, `defaultfilelist`, `picture-circle`, `drag`, `directory`, `upload-png-only`, `preview-file`, `transform-file`, `file-type`, `drag-sorting`, `customize-progress-bar`, `component-token`, `avatar`, `picture-card`, `filelist`, `paste`, `upload-manually`, `picture-style`, `max-count`, `upload-with-aliyun-oss`, `upload-custom-action-icon`, `crop-image`, `style-class` | `basic`, `defaultfilelist`, `picture-circle`, `drag`, `directory`, `upload-png-only`, `preview-file`, `transform-file`, `file-type`, `drag-sorting`, `customize-progress-bar`, `component-token`, `avatar`, `picture-card`, `filelist`, `paste`, `upload-manually`, `picture-style`, `max-count`, `upload-with-aliyun-oss`, `upload-custom-action-icon`, `crop-image`, `style-class` |
| Watermark | `custom` | `custom` |

## 页面错误或采集阻塞

| 组件 | React 页面 | Blazor 页面 |
|---|---|---|
| App | — | Waiting for selector `.code-box-demo` failed |
| Popover | — | Uncaught ReferenceError: define is not defined |
| Result | — | Uncaught ReferenceError: define is not defined |

## 交互状态验收覆盖

运行 DOM/style manifests 中的场景，对点击、输入、悬停、弹层和语义节点进行 React / Blazor 对称操作后比较 DOM、computed style 与几何。该覆盖不是全站所有 demo 的交互穷举；功能型 demo 需按控件行为补充 scenario。

- 本次运行：43 组、48 个场景；完整比较通过 34，有差异 14，阻塞 0。
- 视觉样式通过 47 个场景；存在视觉样式差异 1 个。

| 组件 / 场景 | 视觉样式差异样本 |
|---|---|
| Tooltip / hover-and-show-arrow | `content/content[0].transformOrigin: 46.7188px 17px → 111.125px 17px`<br>`content/content[0].width: 93.4375px → 222.25px`<br>`tooltip/tooltip[0].transformOrigin: 46.7188px 40px → 111.125px 40px`<br>`tooltip/tooltip[0].width: 93.4375px → 222.25px` |

- 按 manifest 中能关联到 React demo 锚点的场景，交互后状态覆盖 39/801 个同名 demo；其余 762 个仍未完成逐 demo 的功能状态比对。以 body 为 scope 的弹层场景没有 demo 锚点，不计入此数。

| 组件 | 已有交互状态覆盖 | 尚未覆盖的同名 demo |
|---|---|---|
| Affix | — | `basic`, `target`, `on-change`, `debug` |
| Alert | — | `basic`, `filled`, `description`, `banner`, `smooth-closed`, `custom-icon`, `custom-title-alignment`, `style-class`, `style`, `closable`, `icon`, `loop-banner`, `error-boundary`, `action`, `component-token`, `_semantic` |
| Anchor | — | `basic`, `horizontal`, `static`, `onclick`, `customizehighlight`, `targetoffset`, `targetoffset-per-link`, `onchange`, `replace`, `legacy-anchor`, `style-class`, `component-token` |
| AutoComplete | — | `basic`, `custom`, `certain-category`, `customize-clear-debug`, `disabled-in-form-debug`, `form-debug`, `render-panel`, `non-case-sensitive`, `uncertain-category`, `style-class`, `disabled-custom-debug`, `filled-custom-debug`, `autocomplete-and-select` |
| Avatar | — | `basic`, `dynamic`, `group`, `toggle-debug`, `fallback`, `type`, `badge`, `max-count`, `responsive`, `component-token` |
| Badge | — | `basic`, `overflow`, `change`, `offset`, `status`, `ribbon`, `title`, `no-wrapper`, `dot`, `link`, `size`, `colorful`, `mix` |
| Breadcrumb | `basic` | `separator` |
| Button | `basic` | `debug-color-variant`, `icon-placement`, `debug-block`, `disabled`, `multiple`, `danger`, `legacy-group`, `component-token`, `wave`, `style-class`, `color-variant`, `icon`, `debug-icon`, `size`, `loading`, `ghost`, `block`, `chinese-chars-loading`, `linear-gradient`, `custom-disabled-bg`, `_semantic` |
| Calendar | — | `basic`, `notice-calendar`, `card`, `select`, `customize-header` |
| Card | — | `basic`, `border-less`, `simple`, `flexible-content`, `in-column`, `loading`, `grid-card`, `inner`, `tabs`, `meta`, `style-class`, `component-token`, `_semantic`, `_semantic_meta` |
| Carousel | — | `basic` |
| Cascader | — | `basic`, `custom-trigger`, `disabled-option`, `multiple`, `size`, `search`, `custom-dropdown`, `variant`, `style-class`, `component-token`, `default-value`, `hover`, `change-on-select`, `showcheckedstrategy`, `lazy`, `suffix`, `placement`, `status` |
| Checkbox | `basic`, `group` | `layout` |
| Collapse | `basic`, `collapsible` | `size`, `accordion`, `mix`, `borderless`, `custom`, `icon`, `noarrow`, `extra`, `ghost`, `style-class`, `component-token` |
| ColorPicker | — | `controlled`, `disabled-alpha`, `trigger`, `format`, `size`, `disabled`, `allowclear`, `presets` |
| DatePicker | — | `basic`, `needconfirm`, `format`, `mask`, `disabled`, `allow-empty`, `preset-ranges`, `size`, `buddhist-era`, `variant`, `mode`, `suffix`, `component-token`, `range-picker`, `switchable`, `time`, `date-range`, `disabled-date`, `select-in-range`, `extra-footer`, `cell-render`, `external-panel`, `status`, `style-class`, `placement`, `start-end`, `suffixicon-debug` |
| ProDescriptions | — | `basic`, `border`, `responsive`, `vertical`, `vertical-border` |
| Divider | `horizontal` | `size`, `vertical`, `component-token`, `style-class`, `with-text`, `plain`, `customize-style`, `variant`, `_semantic` |
| Drawer | — | `basic-right`, `resizable`, `extra`, `form-in-drawer`, `mask`, `no-mask`, `placement`, `loading`, `user-profile`, `size`, `closable-placement` |
| Dropdown | — | `basic`, `item`, `dropdown-button`, `sub-menu`, `loading`, `selection`, `menu-full`, `extra`, `arrow`, `arrow-center`, `event`, `custom-dropdown`, `selectable` |
| Empty | — | `basic`, `simple`, `customize`, `config-provider`, `style-class`, `description` |
| Flex | `basic` | `align`, `gap`, `wrap`, `combination` |
| ProForm | — | `basic`, `control-hooks`, `layout`, `layout-multiple`, `disabled`, `variant`, `required-mark`, `size`, `layout-can-wrap`, `warning-only`, `usewatch`, `validate-trigger`, `validate-only`, `form-item-path`, `dynamic-form-item`, `dynamic-form-items`, `dynamic-form-items-drag-sorting`, `dynamic-form-items-no-style`, `dynamic-form-items-complex`, `nest-messages`, `complex-form-control`, `customized-form-controls`, `global-state`, `form-context`, `inline-login`, `login`, `register`, `advanced-search`, `form-in-modal`, `time-related-controls`, `without-form-create`, `validate-static`, `dynamic-rule`, `form-dependencies`, `getvalueprops-normalize`, `validate-scroll-to-field`, `validate-other`, `style-class`, `disabled-input-debug`, `label-debug`, `col-24-debug`, `ref-item`, `custom-feedback-icons`, `component-token` |
| Grid | `basic`, `gutter`, `offset`, `sort`, `flex`, `flex-align`, `flex-order`, `flex-stretch`, `responsive`, `responsive-flex`, `responsive-more`, `playground`, `usebreakpoint` | — |
| Icon | `basic`, `two-tone`, `custom`, `iconfont`, `scripturl` | — |
| Image | — | `basic`, `placeholder`, `fallback`, `preview-group`, `preview-group-visible`, `previewsrc`, `controlled-preview`, `imagerender`, `mask`, `style-class`, `nested` |
| Input | — | `basic`, `variant`, `compact-style`, `search-input`, `textarea`, `otp`, `presuffix`, `allowclear`, `advance-count`, `focus`, `borderless-debug`, `textarea-resize`, `component-token`, `size`, `addon`, `group`, `search-input-loading`, `autosize-textarea`, `tooltip`, `password-input`, `show-count`, `status`, `debug-addon` |
| InputNumber | — | `basic`, `keyboard`, `variant`, `presuffix`, `focus`, `controls`, `size`, `disabled`, `formatter`, `change-on-wheel`, `out-of-range`, `status`, `style-class` |
| ProLayout | — | `basic`, `top`, `top-side`, `top-side-2`, `side`, `custom-trigger`, `collapsible-overlay`, `responsive`, `fixed`, `fixed-sider`, `custom-trigger-debug`, `component-token` |
| ProList | — | `simple`, `basic`, `loadmore`, `vertical`, `grid`, `infinite-load` |
| Listy 6.6.0 | — | `basic` |
| Mentions | — | `basic`, `variant`, `form`, `readonly`, `status`, `async`, `prefix`, `placement` |
| Menu | — | `horizontal`, `horizontal-dark`, `inline`, `inline-collapsed`, `tooltip`, `sider-current`, `vertical`, `theme`, `submenu-theme`, `switch-mode`, `style-class`, `style-debug`, `menu-v4`, `component-token`, `extra-style`, `extra-collapsed-debug`, `custom-popup-render`, `collapsed-icon-debug` |
| Message | — | `duration`, `loading`, `info`, `other`, `update` |
| Modal | — | `basic`, `footer`, `loading`, `hooks`, `manual`, `modal-render`, `static-info`, `confirm-router`, `nested`, `custom-mouse-position`, `component-token`, `async`, `mask`, `footer-render`, `locale`, `position`, `button-props`, `width`, `confirm`, `style-class` |
| Notification | — | `with-icon`, `custom-icon`, `update`, `duration`, `with-btn`, `placement`, `basic` |
| Pagination | — | `basic`, `align`, `more`, `changer`, `jump`, `mini`, `simple`, `controlled`, `total`, `all`, `itemrender`, `components`, `wireframe`, `component-token`, `variant-debug`, `style-class` |
| Popconfirm | — | `basic`, `dynamic-trigger`, `locale`, `icon` |
| Popover | — | `basic`, `arrow-point-at-center`, `control`, `triggertype`, `hover-with-click` |
| Progress | — | `line`, `line-mini`, `circle-mini`, `format`, `segment`, `gradient-line`, `circle`, `dynamic`, `dashboard`, `linecap`, `steps` |
| QRCode | `base`, `status` | `type`, `customcolor`, `errorlevel`, `style-class`, `icon`, `customstatusrender`, `customsize`, `download`, `popover` |
| Radio | `basic`, `radiobutton` | `radiogroup`, `radiogroup-block`, `size`, `style-class`, `debug-group-width`, `component-token`, `disabled`, `radiogroup-more`, `radiogroup-options`, `radiogroup-with-name`, `radiobutton-solid`, `badge`, `wireframe`, `debug-upload` |
| Rate | `basic`, `half` | `disabled`, `character`, `component-token`, `size`, `text`, `clear`, `character-function` |
| Result | — | `success`, `info`, `warning`, `403`, `404`, `500`, `error`, `customicon`, `style-class`, `component-token` |
| Segmented | — | `basic`, `block`, `disabled`, `custom`, `size`, `icon-only`, `controlled`, `dynamic`, `with-icon` |
| Select | `basic`, `style-class`, `size`, `search`, `multiple` | `search-filter-option`, `option-render`, `tags`, `coordinate`, `automatic-tokenization`, `select-users`, `custom-dropdown-menu`, `variant`, `clear-suffix-debug`, `custom-label-render`, `big-data`, `placement`, `debug`, `option-label-center`, `component-token`, `search-multi-field`, `search-sort`, `optgroup`, `label-in-value`, `custom-tokenization`, `suffix`, `hide-selected`, `filled-debug`, `custom-tag-render`, `responsive`, `status`, `placement-debug`, `render-panel`, `debug-flip-shift`, `maxcount`, `_semantic` |
| Skeleton | — | `basic`, `complex`, `active`, `element`, `children`, `list`, `style-class`, `componenttoken` |
| Slider | — | `basic`, `icon-slider`, `event`, `vertical`, `reverse`, `multiple`, `disabled-handle`, `component-token`, `input-number`, `tip-formatter`, `mark`, `show-tooltip`, `draggabletrack`, `editable`, `style-class`, `_semantic` |
| Space | — | `base`, `vertical`, `size`, `align`, `wrap`, `separator`, `compact-buttons`, `compact-button-vertical`, `debug`, `gap-in-line` |
| Spin | — | `basic`, `nested`, `delayanddebounce`, `size`, `tip`, `custom-indicator`, `style-class` |
| Splitter | `size` | `control`, `vertical`, `collapsible`, `collapsibleicon`, `multiple`, `group`, `lazy`, `customize`, `style-class`, `reset`, `size-mix`, `_semantic` |
| Statistic | — | `basic`, `unit`, `card` |
| Steps | — | `simple`, `error`, `vertical`, `clickable`, `panel`, `icon`, `step-next`, `title-placement`, `progress-dot`, `customized-progress-dot`, `nav`, `progress`, `progress-debug`, `steps-in-steps`, `inline`, `inline-variant`, `variant-debug` |
| Switch | — | `basic`, `loading`, `size` |
| ProTable | — | `basic`, `jsx`, `row-selection`, `row-selection-and-operation`, `row-selection-custom`, `row-selection-debug`, `head`, `filter-in-tree`, `filter-search`, `multiple-sorter`, `reset-filter`, `custom-filter-panel`, `ajax`, `size`, `narrow`, `bordered`, `expand`, `expand-sticky`, `order-column`, `colspan-rowspan`, `tree-data`, `tree-table-ellipsis`, `fixed-header`, `auto-height`, `fixed-columns`, `fixed-gapped-columns`, `fixed-columns-header`, `hidden-columns`, `grouping-columns`, `edit-cell`, `edit-row`, `nested-table`, `drag-sorting`, `drag-column-sorting`, `drag-sorting-handler`, `ellipsis`, `column-defaults`, `ellipsis-custom-tooltip`, `custom-empty`, `summary`, `virtual-list`, `responsive`, `nest-table-border-debug`, `nested-table-in-tabs-debug`, `pagination`, `row-selection-custom-debug`, `sticky`, `dynamic-settings`, `style-class`, `selections-debug`, `component-token`, `measure-row-render` |
| Tabs | — | `basic`, `disabled`, `centered`, `icon`, `custom-indicator`, `slide`, `extra`, `size`, `placement`, `popuprender-search`, `card`, `editable-card`, `card-top`, `custom-add-trigger`, `custom-tab-bar`, `custom-tab-bar-node`, `style-class`, `animated`, `nest`, `component-token` |
| Tag | — | `basic`, `control`, `animation`, `status`, `draggable`, `colorful`, `checkable`, `icon`, `style-class`, `_semantic`, `_semantic_group` |
| TimePicker | — | `basic`, `size`, `disabled`, `interval-options`, `variant`, `suffix`, `value`, `need-confirm`, `hide-column`, `addon`, `range-picker`, `status`, `style-class` |
| Timeline | — | `basic`, `alternate`, `title-span`, `style-class`, `variant`, `horizontal`, `custom`, `title` |
| Tooltip | — | `basic`, `placement`, `colorful`, `arrow`, `auto-adjust-overflow`, `disabled` |
| Transfer | `basic` | `oneway`, `search`, `advanced`, `custom-item`, `actions`, `large-data`, `table-transfer`, `tree-transfer`, `status`, `style-class`, `custom-select-all-labels` |
| Tree | — | `basic`, `draggable`, `search`, `customized-icon`, `virtual-scroll`, `basic-controlled`, `dynamic`, `line`, `directory`, `switcher-icon`, `big-data` |
| TreeSelect | — | `basic`, `treedata`, `async`, `placement`, `status`, `suffix`, `multiple`, `checkable`, `treeline`, `variant`, `maxcount`, `style-class` |
| Typography | — | `basic`, `text`, `copyable`, `ellipsis`, `ellipsis-controlled`, `ellipsis-middle`, `suffix`, `table` |
| Upload | — | `basic`, `defaultfilelist`, `picture-circle`, `drag`, `directory`, `upload-png-only`, `preview-file`, `transform-file`, `file-type`, `drag-sorting`, `customize-progress-bar`, `component-token`, `avatar`, `picture-card`, `filelist`, `paste`, `upload-manually`, `picture-style`, `max-count`, `upload-with-aliyun-oss`, `upload-custom-action-icon`, `crop-image`, `style-class` |
| Watermark | — | `basic`, `multi-line`, `image`, `custom` |

首屏机器报告：`/tmp/antd-v6-runtime-parity-current.json`。
交互机器报告：`/tmp/antd-v6-interaction-parity-corrected.json`.
交互场景清单：`/tmp/antd-v6-interaction-cases-current.json`.

## 2026-10-29 交互样式复验状态

- 既有交互套件最近一次完整运行于 2026-10-28：43 组、48 个场景，34 个完整通过、14 个 DOM/结构差异、0 个阻塞。加上 2026-10-29 新增的 Select `search` 和 `multiple` 两个交互场景后，Demo 锚点覆盖为 39/801；其余 762 个 Demo 尚未完成交互后样式比对。
- Select 搜索 demo 修复后复验：将 React 搜索时保留的 placeholder、`ant-select-content-has-search-value`、搜索 input 的 DOM value 同步到 Blazor，并选择首个可用匹配项作为 active option。Blazor 的 `EnableVirtualization` 默认值同步为 `true`，与 React 默认可访问性列表行为一致；仍可设为 `false` 关闭。复验后触发器及 popup 的 DOM 树一致，采集的 computed CSS 与几何均一致；popup 根节点的原始 inline `style` 属性字符串仍不同（React 的 inset/arrow 值与 Blazor Overlay 的 absolute/transform-origin/变量串不同），因此原始 HTML 属性层仍记为差异，不能据此标成完全相同。
- Select 多选 demo 曾因文档服务器以 Production 环境启动，导致 AntDesign 静态 JS 资源 404、Blazor 交互 circuit 初始化失败而显示空白；改用 Development 环境启动后两个 Select 正常出现。该空白是验证环境问题，不能记作组件差异。
- 浏览器操作多选 demo：启用态下拉列表可展开，点击 `d13` 后标签和选中态更新、下拉保持打开；按 Escape 后下拉关闭且保留选择。禁用态 Select 仍不可操作。12 个子选项的虚拟窗口回收回归测试通过；这项验证只覆盖多选的基本点击和关闭路径。
- 修正交互运行器对隐藏/屏外 portal 的命中后，Select 搜索和多选两个场景均到达采样阶段，0 个阻塞，交互后视觉与 computed CSS 均一致。多选 `display` 差异来自 Blazor Demo 未像 React 一样用 `.ant-space-item` 包裹 Space 子项；补上两个 `SpaceItem` 后根节点恢复 `inline-flex`。默认启用隐藏 accessibility listbox 后，小列表搜索 DOM 树也与 React 一致。剩余 DOM 差异为 Select Overlay 的 inline 定位样式字符串，以及多选项勾选图标 SVG 结构；当前几何与选中视觉样式一致。
- 本次重跑全套 43 组交互时 Chromium target 关闭，未生成可用的全套新报告；因此交互数据继续以 10-28 最近一次完整结果为基线，并以上述 10-29 Select 场景作为新增结果。

本次 Select 小列表/多选最终复验：`/tmp/select-interaction-parity-after-small-list-virtual-a11y-2026-10-29.json`；此前多选子项结构复验：`/tmp/select-interaction-parity-after-spaceitem-fix-2026-10-29.json`；此前搜索复验：`/tmp/antd-v6-select-search-after-default-virtualization.json`；此前完整交互报告：`/tmp/antd-v6-select-interaction-parity-current.json`。完整逐 Demo 功能交互与交互后样式检查仍未完成；需继续按每个 Demo 的实际能力补充对称的 React/Blazor 操作场景，再以场景结果填满剩余覆盖矩阵。

## 2026-10-29 Icon 页面复验

- 针对 React v6 对应的 Icon demos 运行 `icon-dom-style-parity.json` 中 5 个场景：`icon-font`、`icon-font-resources`、`icon-basic`、`icon-two-tone`、`icon-custom`。结果为 5/5 通过，DOM 树、选定 computed CSS、视觉样式均一致，0 个差异、0 个阻塞。
- React 与 Blazor 文档页面已分别打开在浏览器 tab 18 和 19，供手工并排检查；页面截图中图标列表项名称在当前窄卡片内均有截断，React 与 Blazor 表现一致。
- 自动化结果：`/tmp/icon-dom-style-parity-after-select-2026-10-29.json`。Icon 五个对应示例目前没有发现 DOM/CSS/几何差异；其余全站组件和 Select 未覆盖交互仍按上表继续处理。
