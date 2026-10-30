# Ant Design v6 文档 Demo 运行时对齐扫描

扫描时间：2026-10-28T04:59:26.505Z

## 范围与方法

- 顺序访问 React v6 与 Blazor 文档，比较 68 个同名组件路由；light theme，viewport 1280×900。
- demo 按文档 id 归一名称；runtime 树限定在 demo 内的 Ant 组件子树。比较规范化 HTML 树、Ant 节点 computed style（含全部 --ant-*）及节点几何（1px 容差）。
- 结果是首次加载状态的差异候选，不等于已确认缺陷。运行时版本、字体、全局 CSS 变量继承和组件内部包装可能产生噪声。
- 本轮未对全部 776 个匹配 demo 逐一操作。仓库有 43 个 DOM/style manifest group；实际浏览器门禁执行了 48 个 scenario、23 个 action，覆盖 33 个匹配 demo。

## 总览

| 项目 | 数量 |
|---|---:|
| React / Blazor 组件路由 | 74 / 74 |
| 可比组件 | 68 |
| React / Blazor demo scopes | 1098 / 988 |
| 名称匹配 demo | 776 |
| React 有、Blazor 缺失 | 322 |
| 仅 Blazor 有 | 212 |
| HTML 树有差异 | 716 |
| computed style 有差异 | 728 |
| 几何差异 >1px | 549 |
| Blazor 页面有 JS/渲染错误 | 9 |

## 组件路由差异

- React 有、Blazor 没有：EditableProTable (/components/editable-table).
- Blazor 独有路由：PageHeader, Comment, BackTop.
- 扫描策略排除 React 扩展路由：FloatButton, Masonry 6.0.0, Tour, BorderBeam 6.4.0, Util.

## React demo 缺失及 Blazor 额外 demo

| 组件 | React 有但 Blazor 缺失 | 仅 Blazor 有 |
|---|---|---|
| Button | — | download |
| Icon | — | two-tone-color |
| Typography | title, paragraph-debug, editable, ellipsis-debug, componenttoken-debug, link-danger-debug, _semantic | Level, Style, Interactive |
| Flex | debug | — |
| ProLayout | _semantic | — |
| Space | compact, compact-debug, compact-nested, debug, style-class, component-token, _semantic | customize, split |
| Splitter | nested-in-tabs, debug | — |
| Anchor | _semantic | — |
| Breadcrumb | withparams, overlay, debug-routes, component-token, withicon, separator-component, style-class, _semantic | Integration, Dropdownmenu, With |
| Dropdown | placement, trigger, overlay-open, icon-debug, sub-menu-debug, context-menu, style-class, render-panel, _semantic | context-menu-trigger, boundary-adjust-mode, placementTypes, triggerMode, overlay-visible, popup-container |
| Menu | horizontal-dark, style-debug, menu-v4, component-token, extra-style, extra-collapsed-debug, collapsed-icon-debug, _semantic | responsive, sider-current2, router, icontemplate, semantic-style |
| Pagination | _semantic | customIcon |
| Steps | max-count, style-class, component-token, _semantic, _semantic_items | small-size, vertical-small |
| Tabs | nest, _semantic | contextmenu |
| AutoComplete | status, allowclear, options, variant, _semantic | format, custom-filter, performance, ObjectValue, property, popup-container |
| Cascader | fields-name, ellipsis-debug, custom-render, panel, render-panel, _semantic | default, disabled, change |
| Checkbox | controller, check-all, style-class, debug-disable-popover, custom-line-width, disabled, debug-line, debug-group-width, _semantic | Controlled, enum-group, Disable, CheckboxAll, MixedMode |
| ColorPicker | base, text-render, presets-line-gradient, style-class, line-gradient, trigger-event, panel-render, pure-panel, _semantic | basic |
| DatePicker | multiple, components, filled-debug, multiple-debug, render-panel, _semantic | globalization, suffix-icon, bordered |
| ProForm | _semantic | static, nest-table, is-modified, customized-form-validator, localization, dynamic-model, dynamic-field, generate-form-item, validate-onchange, colLayout, validate-mode, validate-with-feedback, set-validate-message |
| Input | filled-debug, search-variant-debug, style-class, align, _semantic_input, _semantic_textarea, _semantic_search, _semantic_password, _semantic_otp | SearchLoading, Area, Prefix, Clear, Borderless, Generic, bind-on-input, SearchBox, TextAreaBasic, chatbox, WithTooltip, Password, CharCount |
| InputNumber | addon, digit, disabled-hover-debug, borderless-height-debug, debug-token, spinner, filled-debug, render-panel, _semantic | Disable, Generic, Culture, Decimal, borderless, Precision |
| Mentions | popuprender, autosize, render-panel, size, allowclear, autosize-textarea-debug, style-class, component-token, _semantic | textareatemplate |
| Radio | debug-group-width, component-token, badge, debug-upload, _semantic | Group, Name, Optional, Generic, Disable, Vgroup, Style, Button, enum-group |
| Select | clear-suffix-debug, debug, filled-debug, placement-debug, debug-flip-shift, _semantic | TrackItemChanges, search-box, DefaultModal, table-select, select-placement, maxTagChange, PopupContainer, simple-select, search-filter-customize, DefaultToggleDatasource, option-label-prop, DefaultForm, bordered, list-box-style, enum-select |
| Slider | _semantic | Tooltip |
| Switch | text, style-class, disabled, component-token, _semantic | Icon1, Disable, Control |
| TimePicker | 12hours, colored-popup, render-panel, change-on-scroll, _semantic | hours12 |
| Transfer | custom-select-all-labels, component-token, _semantic | semantic-style |
| TreeSelect | render-panel, component-token, _semantic | node-selectable, ShowCheckedStrategy, search, node-checkable, custom-dropdown-menu, semantic-style |
| Upload | debug-disabled, _semantic | input-file |
| Badge | ribbon-debug, title, component-token, style-class, colorful-with-count-debug, _semantic, _semantic_ribbon | — |
| Calendar | event-range, lunar, week, style-class, component-token, _semantic | — |
| Card | no-body-debug, button-alignment-debug | Customized, Support |
| Carousel | autoplay, arrows, component-token, placement, fade, dot-duration | Automatic, CustomButtons, Position, Fadein |
| Collapse | component-token, _semantic | controlled-activekeys |
| ProDescriptions | text, padding, size, style, style-class, jsx, component-token, block, _semantic | custom-size, custom-style |
| Empty | _semantic | — |
| Image | toolbarrender, preview-mask, coverplacement, preview-group-top-progress, component-token, preview-imginfo, _semantic | actions-render, progress-placeholder, progress-render |
| ProList | pagination, grid-test, responsive, drag-sorting, drag-sorting-handler, grid-drag-sorting, grid-drag-sorting-handler, virtual-list, component-token, spin-debug, _semantic | resposive, infinite-virtualized-load, draggable |
| Listy 6.6.0 | virtual, group, rich, drag-sorting, infinite, style-class, scroll-to, _semantic | — |
| Popover | placement, style-class, wireframe, arrow, shift, render-panel, component-token, _semantic | placementType |
| QRCode | _semantic | size, basic |
| Segmented | style-class, size-consistent, vertical, shape, with-name, controlled-two, componenttoken, _semantic | — |
| Statistic | animated, timer, component-token, style-class, _semantic | Separator, countdown |
| ProTable | _semantic | tree-table-preserve-selected-row-keys, basic-form, data-index, basic-bind-field, generate-columns, on-row-and-on-cell, router-pagination, datatable, dynamic, custom-sort, filter, custom-field-filter, restore-query-state, odata, tree-data-with-circular-reference, row-template, custom-row-style, grouping-rows, resizable-column, toggle-column, responsive-mode, column-alignment, virtualization, custom-pagination |
| Tag | disabled, customize, component-token | Dynamic, TwoTone, Event |
| Timeline | pending, horizontal-debug, end, pending-legacy, semantic, component-token, _semantic, _semantic_items | Reverse, Right, Label, Color, semantic-sample |
| Tooltip | shift, force-align, debug, disabled-children, style-class, smooth-transition, auto-adjust-overflow, destroy-on-close, render-panel, wrap-custom-component, _semantic | placementType, icon, arrow-point-at-center |
| Tree | directory-debug, drag-debug, block-node, multiple-line, line-debug, scroll-to, big-data, component-token, style-class, _semantic | node-selectable, Droppable, node-checkable |
| Alert | — | close-text |
| Drawer | multi-level-drawer, style-class, scroll-debug, render-in-current, config-provider, render-panel, component-token, _semantic | offset, multiple, drawer_service, Basic, render_dom, drawer_event, semantic-style, dropdown-in-drawer |
| Message | hooks, style-class, component-token, stack, thenable, render-panel, _semantic | continue-with, loading-when |
| Modal | dark, render-panel, wireframe, _semantic | info, draggable_modal, confirm_service, form-in-modal, resizeable, dropdown-in-modal, modal_service_for_modal, header, confirm-promise, OverrideLocale, maxable, max-body-height, modal_service_for_confirm, custom-modal-footer |
| Notification | hooks, show-with-progress, progress-color, render-panel, stack, component-token, style-class, _semantic | notificationRef, error-boundary |
| Popconfirm | placement, async, style-class, wireframe, shift, promise, render-panel, _semantic | placementType, button-props |
| Progress | circle-steps, info-position, circle-micro, size, style-class, _semantic | TrailColor, circledynamic |
| Result | _semantic | error-boundary, changeTheResult |
| Skeleton | _semantic, _semantic_element | — |
| Spin | percent, fullscreen, list-debug, _semantic | inside |
| Watermark | portal, debug | scrolling, image-scrolling |
| Affix | — | CallBack, Scroll |
| App | basic, config | — |
| ConfigProvider | locale, direction, direction, size, theme, wave, holderrender, prefixcls, useconfig, warning, focus-outline-debug | token-customization, scoped-theme, algorithms |

## 首屏运行时差异 demo 清单

每列列出该类快照不相等的 demo 名称；数量按 demo 计。

| 组件 | HTML 树 | computed style | 几何 >1px |
|---|---|---|---|
| Button | debug-color-variant, icon-placement, debug-block, disabled, multiple, danger, legacy-group, component-token, wave, style-class, color-variant, icon, debug-icon, size, loading, ghost, block, chinese-chars-loading, linear-gradient, custom-disabled-bg, _semantic | basic, debug-color-variant, icon-placement, debug-block, disabled, multiple, danger, legacy-group, component-token, wave, style-class, color-variant, icon, debug-icon, size, loading, ghost, block, chinese-chars-loading, linear-gradient, custom-disabled-bg, _semantic | debug-color-variant, icon-placement, disabled, multiple, danger, legacy-group, component-token, wave, style-class, color-variant, icon, debug-icon, size, loading, ghost, block, chinese-chars-loading, custom-disabled-bg, _semantic |
| Icon | — | basic, two-tone, custom, iconfont, scripturl | — |
| Typography | basic, text, copyable, ellipsis, ellipsis-controlled, ellipsis-middle, suffix | basic, text, copyable, ellipsis, ellipsis-controlled, ellipsis-middle, suffix, table | basic, text, copyable, ellipsis, ellipsis-controlled, ellipsis-middle, suffix |
| Divider | component-token, style-class, with-text, customize-style, variant, _semantic | horizontal, size, vertical, component-token, style-class, with-text, plain, customize-style, variant, _semantic | size, vertical, component-token, style-class, with-text, customize-style, _semantic |
| Flex | basic, align, gap, combination | basic, align, gap, wrap, combination | gap, combination |
| Grid | playground, usebreakpoint | basic, gutter, offset, sort, flex, flex-align, flex-order, flex-stretch, responsive, responsive-flex, responsive-more, playground, usebreakpoint | playground |
| ProLayout | basic, top, top-side, top-side-2, custom-trigger, collapsible-overlay, responsive, custom-trigger-debug, component-token | basic, top, top-side, top-side-2, custom-trigger, collapsible-overlay, responsive, custom-trigger-debug, component-token | basic, top, top-side, top-side-2, custom-trigger, collapsible-overlay, responsive, custom-trigger-debug, component-token |
| Space | base, vertical, size, align, wrap, separator, compact-buttons, compact-button-vertical, gap-in-line | base, vertical, size, align, wrap, separator, compact-buttons, compact-button-vertical, gap-in-line | base, vertical, size, align, compact-buttons, compact-button-vertical, gap-in-line |
| Splitter | control, vertical, collapsible, collapsibleicon, multiple, group, lazy, customize, style-class, reset, size-mix, _semantic | size, control, vertical, collapsible, collapsibleicon, multiple, group, lazy, customize, style-class, reset, size-mix, _semantic | vertical, collapsible, multiple, group, lazy, customize, style-class, reset, size-mix, _semantic |
| Anchor | basic, horizontal, static, onclick, customizehighlight, targetoffset, targetoffset-per-link, onchange, replace, legacy-anchor, style-class, component-token | static, onclick, customizehighlight, onchange, legacy-anchor | static, onclick, customizehighlight, onchange, legacy-anchor |
| Breadcrumb | separator | basic, separator | separator |
| Dropdown | basic, item, dropdown-button, sub-menu, loading, selection, menu-full, extra, arrow, arrow-center, event, custom-dropdown, selectable | dropdown-button, loading, selection, arrow, arrow-center | dropdown-button, loading, selection, arrow, arrow-center |
| Menu | horizontal, inline, inline-collapsed, tooltip, sider-current, vertical, theme, submenu-theme, switch-mode, style-class, custom-popup-render | horizontal, inline, inline-collapsed, tooltip, sider-current, vertical, theme, submenu-theme, switch-mode, style-class, custom-popup-render | horizontal, inline, inline-collapsed, tooltip, sider-current, vertical, theme, submenu-theme, switch-mode, style-class, custom-popup-render |
| Pagination | basic, align, more, changer, jump, mini, simple, controlled, total, all, itemrender, components, wireframe, component-token, variant-debug, style-class | basic, align, more, changer, jump, mini, simple, controlled, total, all, itemrender, components, wireframe, component-token, variant-debug, style-class | more, changer, jump, mini, all, itemrender, components, wireframe, component-token, variant-debug, style-class |
| Steps | simple, error, vertical, clickable, panel, icon, step-next, title-placement, progress-dot, customized-progress-dot, nav, progress, progress-debug, steps-in-steps, inline, inline-variant, variant-debug | simple, error, vertical, clickable, panel, icon, step-next, title-placement, progress-dot, customized-progress-dot, nav, progress, progress-debug, steps-in-steps, inline, inline-variant, variant-debug | simple, error, vertical, clickable, panel, step-next, title-placement, progress-dot, nav, progress, progress-debug, steps-in-steps, inline, inline-variant, variant-debug |
| Tabs | basic, disabled, centered, icon, custom-indicator, slide, extra, size, placement, popuprender-search, card, editable-card, card-top, custom-add-trigger, custom-tab-bar, custom-tab-bar-node, style-class, animated, component-token | basic, disabled, centered, icon, custom-indicator, slide, extra, size, placement, popuprender-search, card, editable-card, card-top, custom-add-trigger, custom-tab-bar, custom-tab-bar-node, style-class, animated, component-token | basic, disabled, centered, icon, custom-indicator, slide, extra, size, placement, popuprender-search, card, editable-card, card-top, custom-add-trigger, custom-tab-bar, custom-tab-bar-node, style-class, animated, component-token |
| AutoComplete | basic, custom, certain-category, customize-clear-debug, disabled-in-form-debug, form-debug, render-panel, non-case-sensitive, uncertain-category, style-class, disabled-custom-debug, filled-custom-debug, autocomplete-and-select | basic, custom, certain-category, customize-clear-debug, disabled-in-form-debug, form-debug, render-panel, non-case-sensitive, uncertain-category, style-class, disabled-custom-debug, filled-custom-debug, autocomplete-and-select | basic, custom, certain-category, customize-clear-debug, disabled-in-form-debug, form-debug, render-panel, non-case-sensitive, uncertain-category, style-class, disabled-custom-debug, filled-custom-debug, autocomplete-and-select |
| Cascader | basic, custom-trigger, disabled-option, multiple, size, search, custom-dropdown, variant, style-class, component-token, default-value, hover, change-on-select, showcheckedstrategy, lazy, suffix, placement, status | basic, disabled-option, multiple, size, search, custom-dropdown, variant, style-class, component-token, default-value, hover, change-on-select, showcheckedstrategy, lazy, suffix, placement, status | basic, multiple, custom-dropdown, variant, style-class, component-token, showcheckedstrategy, suffix, placement, status |
| Checkbox | basic, group, layout | basic, group, layout | — |
| ColorPicker | controlled, disabled-alpha, trigger, format, size, disabled, allowclear, presets | controlled, disabled-alpha, trigger, format, size, disabled, allowclear, presets | controlled, disabled-alpha, trigger, format, size, allowclear, presets |
| DatePicker | basic, needconfirm, format, mask, disabled, allow-empty, preset-ranges, size, buddhist-era, variant, mode, suffix, component-token, range-picker, switchable, time, date-range, disabled-date, select-in-range, extra-footer, cell-render, external-panel, status, style-class, placement, start-end, suffixicon-debug | basic, needconfirm, format, mask, disabled, allow-empty, preset-ranges, size, buddhist-era, variant, mode, suffix, component-token, range-picker, switchable, time, date-range, disabled-date, extra-footer, cell-render, external-panel, status, style-class, placement, start-end, suffixicon-debug | format, mask, disabled, allow-empty, preset-ranges, size, buddhist-era, variant, mode, suffix, component-token, range-picker, switchable, time, disabled-date, extra-footer, cell-render, external-panel, status, style-class, placement, suffixicon-debug |
| ProForm | basic, control-hooks, layout, layout-multiple, disabled, variant, required-mark, size, layout-can-wrap, warning-only, usewatch, validate-trigger, validate-only, form-item-path, dynamic-form-item, dynamic-form-items, dynamic-form-items-drag-sorting, dynamic-form-items-no-style, dynamic-form-items-complex, nest-messages, complex-form-control, customized-form-controls, global-state, form-context, inline-login, login, register, advanced-search, time-related-controls, without-form-create, validate-static, dynamic-rule, form-dependencies, getvalueprops-normalize, validate-scroll-to-field, validate-other, style-class, disabled-input-debug, label-debug, col-24-debug, ref-item, custom-feedback-icons, component-token | basic, control-hooks, layout, layout-multiple, disabled, variant, required-mark, size, layout-can-wrap, warning-only, usewatch, validate-trigger, validate-only, form-item-path, dynamic-form-item, dynamic-form-items, dynamic-form-items-drag-sorting, dynamic-form-items-no-style, dynamic-form-items-complex, nest-messages, complex-form-control, customized-form-controls, global-state, form-context, inline-login, login, advanced-search, form-in-modal, time-related-controls, without-form-create, validate-static, dynamic-rule, form-dependencies, validate-other, style-class, disabled-input-debug, label-debug, ref-item, custom-feedback-icons, component-token | basic, control-hooks, layout, layout-multiple, disabled, variant, required-mark, size, layout-can-wrap, warning-only, usewatch, validate-trigger, validate-only, form-item-path, dynamic-form-item, dynamic-form-items, dynamic-form-items-drag-sorting, dynamic-form-items-no-style, dynamic-form-items-complex, nest-messages, complex-form-control, customized-form-controls, global-state, form-context, inline-login, login, advanced-search, time-related-controls, without-form-create, validate-static, dynamic-rule, form-dependencies, validate-other, style-class, disabled-input-debug, label-debug, ref-item, custom-feedback-icons, component-token |
| Input | basic, variant, compact-style, search-input, textarea, otp, presuffix, allowclear, advance-count, focus, borderless-debug, textarea-resize, component-token, size, addon, group, search-input-loading, autosize-textarea, tooltip, password-input, show-count, status, debug-addon | variant, compact-style, search-input, textarea, otp, presuffix, allowclear, advance-count, focus, borderless-debug, textarea-resize, component-token, size, addon, group, search-input-loading, autosize-textarea, password-input, show-count, status, debug-addon | variant, compact-style, search-input, otp, presuffix, advance-count, focus, borderless-debug, textarea-resize, component-token, size, addon, group, search-input-loading, autosize-textarea, password-input, show-count, debug-addon |
| InputNumber | basic, keyboard, variant, presuffix, focus, controls, size, disabled, formatter, change-on-wheel, out-of-range, status, style-class | basic, keyboard, variant, presuffix, focus, controls, size, disabled, formatter, change-on-wheel, out-of-range, status, style-class | presuffix, size, formatter, out-of-range, status, style-class |
| Mentions | basic, variant, form, readonly, status, async, prefix, placement | basic, variant, form, status, async, prefix | variant, form, status, async, prefix |
| Radio | radiogroup, radiogroup-block, radiobutton, size, style-class, disabled, radiogroup-more, radiogroup-options, radiogroup-with-name, radiobutton-solid, wireframe | basic, radiogroup, radiogroup-block, radiobutton, size, style-class, disabled, radiogroup-more, radiogroup-options, radiogroup-with-name, radiobutton-solid, wireframe | radiogroup, size, style-class, disabled, radiogroup-more, radiogroup-options, wireframe |
| Rate | character, component-token, size, text, clear, character-function | basic, half, disabled, character, component-token, size, text, clear, character-function | character, component-token, text, clear |
| Select | basic, search-filter-option, multiple, option-render, tags, coordinate, automatic-tokenization, select-users, custom-dropdown-menu, variant, custom-label-render, big-data, placement, option-label-center, component-token, style-class, search, search-multi-field, size, search-sort, optgroup, label-in-value, custom-tokenization, suffix, hide-selected, custom-tag-render, responsive, status, render-panel, maxcount | basic, search-filter-option, multiple, option-render, tags, coordinate, automatic-tokenization, select-users, custom-dropdown-menu, variant, custom-label-render, big-data, placement, option-label-center, component-token, style-class, search, search-multi-field, size, search-sort, optgroup, label-in-value, custom-tokenization, suffix, hide-selected, custom-tag-render, responsive, status, render-panel, maxcount | multiple, coordinate, automatic-tokenization, variant, placement, option-label-center, component-token, custom-tokenization, suffix, hide-selected, custom-tag-render, responsive, status, render-panel, maxcount |
| Slider | basic, icon-slider, event, vertical, reverse, multiple, disabled-handle, component-token, input-number, tip-formatter, mark, show-tooltip, draggabletrack, editable, style-class | basic, icon-slider, event, vertical, reverse, multiple, disabled-handle, component-token, input-number, tip-formatter, mark, show-tooltip, draggabletrack, editable, style-class | basic, icon-slider, event, vertical, reverse, multiple, disabled-handle, component-token, input-number, mark, show-tooltip, draggabletrack, editable, style-class |
| Switch | basic, loading, size | basic, loading, size | basic, loading |
| TimePicker | basic, size, disabled, interval-options, variant, suffix, value, need-confirm, hide-column, addon, range-picker, status, style-class | basic, size, disabled, value, need-confirm, hide-column, addon | basic, size, disabled, value, need-confirm, hide-column, addon |
| Transfer | basic, oneway, search, advanced, custom-item, actions, large-data, table-transfer, tree-transfer, status, style-class | basic, oneway, search, advanced, custom-item, actions, large-data, table-transfer, tree-transfer, status, style-class | oneway, search, advanced, custom-item, actions, large-data, table-transfer, tree-transfer, status, style-class |
| TreeSelect | basic, treedata, async, placement, status, suffix, multiple, checkable, treeline, variant, maxcount, style-class | basic, treedata, async, placement, status, suffix, multiple, checkable, treeline, variant, maxcount, style-class | treedata, placement, suffix, multiple, checkable, treeline, variant, maxcount, style-class |
| Upload | basic, defaultfilelist, picture-circle, drag, directory, upload-png-only, preview-file, transform-file, file-type, drag-sorting, customize-progress-bar, component-token, avatar, picture-card, filelist, paste, upload-manually, picture-style, max-count, upload-with-aliyun-oss, upload-custom-action-icon, crop-image, style-class | basic, defaultfilelist, picture-circle, drag, directory, upload-png-only, preview-file, transform-file, file-type, drag-sorting, customize-progress-bar, component-token, avatar, picture-card, filelist, paste, upload-manually, picture-style, max-count, upload-with-aliyun-oss, upload-custom-action-icon, crop-image, style-class | basic, defaultfilelist, picture-circle, drag, directory, upload-png-only, preview-file, transform-file, file-type, drag-sorting, customize-progress-bar, component-token, avatar, picture-card, filelist, paste, upload-manually, picture-style, max-count, upload-with-aliyun-oss, upload-custom-action-icon, crop-image, style-class |
| Avatar | basic, dynamic, group, toggle-debug, fallback, type, badge, max-count, responsive, component-token | basic, dynamic, group, toggle-debug, fallback, type, badge, max-count, responsive, component-token | basic, dynamic, group, toggle-debug, fallback, type, badge, max-count, component-token |
| Badge | basic, overflow, change, offset, status, ribbon, no-wrapper, dot, link, size, colorful, mix | basic, overflow, change, offset, status, ribbon, no-wrapper, dot, link, size, colorful, mix | basic, overflow, change, offset, status, ribbon, no-wrapper, dot, link, size, colorful, mix |
| Calendar | basic, notice-calendar, card, select, customize-header | basic, notice-calendar, card, select, customize-header | notice-calendar, card, customize-header |
| Card | basic, border-less, simple, flexible-content, in-column, loading, grid-card, inner, tabs, meta, style-class, component-token, _semantic, _semantic_meta | basic, border-less, simple, flexible-content, in-column, loading, grid-card, inner, tabs, meta, style-class, component-token, _semantic, _semantic_meta | basic, border-less, flexible-content, in-column, loading, inner, tabs, meta, style-class, component-token, _semantic, _semantic_meta |
| Carousel | basic | basic | — |
| Collapse | basic, size, borderless, custom, icon, noarrow, extra, ghost, collapsible, style-class | basic, size, accordion, mix, borderless, custom, icon, noarrow, extra, ghost, collapsible, style-class | size, custom, icon, ghost, style-class |
| ProDescriptions | basic, border, responsive, vertical, vertical-border | basic, border, responsive, vertical, vertical-border | basic, border, responsive, vertical, vertical-border |
| Empty | basic, simple, customize, config-provider, style-class, description | basic, simple, customize, style-class, description | style-class |
| Image | basic, placeholder, fallback, preview-group, preview-group-visible, previewsrc, controlled-preview, imagerender, mask, style-class, nested | basic, placeholder, fallback, preview-group, preview-group-visible, previewsrc, controlled-preview, imagerender, mask, style-class, nested | placeholder, preview-group, controlled-preview, mask, style-class, nested |
| ProList | simple, basic, loadmore, vertical, grid, infinite-load | simple, basic, loadmore, vertical, grid | basic, loadmore, vertical |
| Listy 6.6.0 | basic | basic | basic |
| Popover | basic, arrow-point-at-center, triggertype, hover-with-click | basic, control, triggertype, hover-with-click | triggertype, hover-with-click |
| QRCode | base, status, type, customcolor, errorlevel, style-class, icon, customstatusrender, customsize, download | base, status, type, customcolor, errorlevel, style-class, icon, customstatusrender, customsize, download, popover | status |
| Segmented | basic, block, disabled, custom, size, icon-only, controlled, dynamic, with-icon | basic, block, disabled, custom, size, icon-only, controlled, dynamic, with-icon | disabled, custom, size, dynamic, with-icon |
| Statistic | basic, unit, card | basic, unit, card | basic, unit, card |
| ProTable | basic, jsx, row-selection, row-selection-and-operation, row-selection-custom, row-selection-debug, head, filter-in-tree, filter-search, multiple-sorter, reset-filter, custom-filter-panel, ajax, size, narrow, bordered, expand, expand-sticky, order-column, colspan-rowspan, tree-data, tree-table-ellipsis, fixed-header, auto-height, fixed-columns, fixed-gapped-columns, fixed-columns-header, hidden-columns, grouping-columns, edit-cell, edit-row, nested-table, drag-sorting, drag-column-sorting, drag-sorting-handler, ellipsis, column-defaults, ellipsis-custom-tooltip, custom-empty, summary, virtual-list, responsive, nest-table-border-debug, nested-table-in-tabs-debug, pagination, row-selection-custom-debug, sticky, dynamic-settings, style-class, selections-debug, component-token, measure-row-render | basic, jsx, row-selection, row-selection-and-operation, row-selection-custom, row-selection-debug, head, filter-in-tree, filter-search, multiple-sorter, reset-filter, custom-filter-panel, ajax, size, narrow, bordered, expand, expand-sticky, order-column, colspan-rowspan, tree-data, tree-table-ellipsis, fixed-header, fixed-columns, fixed-gapped-columns, fixed-columns-header, hidden-columns, grouping-columns, edit-cell, edit-row, nested-table, drag-sorting, drag-column-sorting, drag-sorting-handler, ellipsis, column-defaults, ellipsis-custom-tooltip, custom-empty, summary, virtual-list, responsive, nest-table-border-debug, nested-table-in-tabs-debug, pagination, row-selection-custom-debug, sticky, dynamic-settings, style-class, selections-debug, component-token, measure-row-render | basic, jsx, row-selection, row-selection-and-operation, row-selection-custom, row-selection-debug, head, filter-in-tree, filter-search, multiple-sorter, reset-filter, custom-filter-panel, ajax, size, narrow, bordered, expand, expand-sticky, order-column, colspan-rowspan, tree-data, tree-table-ellipsis, fixed-header, fixed-columns, fixed-gapped-columns, fixed-columns-header, hidden-columns, grouping-columns, edit-cell, edit-row, nested-table, drag-sorting, drag-column-sorting, drag-sorting-handler, ellipsis, column-defaults, ellipsis-custom-tooltip, custom-empty, summary, virtual-list, responsive, nest-table-border-debug, nested-table-in-tabs-debug, pagination, row-selection-custom-debug, sticky, dynamic-settings, style-class, selections-debug, component-token, measure-row-render |
| Tag | basic, control, animation, status, draggable, colorful, checkable, icon, style-class, _semantic, _semantic_group | basic, control, animation, status, draggable, colorful, checkable, icon, style-class, _semantic, _semantic_group | basic, control, animation, status, colorful, checkable, icon, style-class, _semantic, _semantic_group |
| Timeline | basic, alternate, title-span, style-class, variant, horizontal, custom, title | basic, alternate, title-span, style-class, variant, horizontal, custom, title | title-span, style-class, variant, horizontal, title |
| Tooltip | placement, colorful, arrow, disabled | placement, colorful, arrow, disabled | placement, colorful, arrow, disabled |
| Tree | basic, draggable, search, customized-icon, virtual-scroll, basic-controlled, dynamic, line, directory, switcher-icon | basic, draggable, search, customized-icon, basic-controlled, dynamic, line, directory, switcher-icon | basic, draggable, search, customized-icon, basic-controlled, dynamic, line, directory, switcher-icon |
| Alert | basic, filled, description, smooth-closed, custom-icon, custom-title-alignment, style-class, style, closable, icon, loop-banner, error-boundary, action, component-token, _semantic | basic, filled, description, smooth-closed, custom-icon, custom-title-alignment, style-class, style, closable, icon, loop-banner, error-boundary, action, component-token, _semantic | description, custom-icon, style, closable, icon, loop-banner, error-boundary, action, _semantic |
| Drawer | basic-right, resizable, extra, form-in-drawer, mask, no-mask, placement, loading, user-profile, size, closable-placement | basic-right, resizable, extra, form-in-drawer, mask, no-mask, placement, loading, user-profile, size, closable-placement | resizable, extra, no-mask, placement, user-profile, size, closable-placement |
| Message | info, update | duration, loading, info, other, update | info, update |
| Modal | hooks, modal-render, static-info, nested, component-token, mask, footer-render, locale, width, confirm, style-class | basic, footer, loading, hooks, manual, modal-render, static-info, confirm-router, nested, custom-mouse-position, component-token, async, mask, footer-render, locale, position, button-props, width, confirm, style-class | modal-render, nested, component-token, footer-render, locale, width, confirm, style-class |
| Notification | with-icon, with-btn, placement | with-icon, custom-icon, update, duration, with-btn, placement, basic | with-icon, with-btn, placement |
| Popconfirm | basic, dynamic-trigger, locale, icon | dynamic-trigger | dynamic-trigger |
| Progress | line, line-mini, circle-mini, format, segment, gradient-line, circle, dynamic, dashboard, linecap, steps | line, line-mini, circle-mini, format, segment, gradient-line, circle, dynamic, dashboard, linecap, steps | line, line-mini, circle-mini, format, segment, gradient-line, circle, dynamic, dashboard, linecap, steps |
| Result | success, info, warning, 403, 404, 500, error, customicon, style-class, component-token | success, info, warning, 403, 404, 500, error, customicon, style-class, component-token | success, 403, 404, 500, error, style-class, component-token |
| Skeleton | basic, complex, active, element, children, list, style-class, componenttoken | basic, complex, active, children, list, style-class, componenttoken | complex, list, style-class |
| Spin | nested, delayanddebounce, size, tip, custom-indicator, style-class | basic, nested, delayanddebounce, size, tip, custom-indicator, style-class | basic, nested, delayanddebounce, size, tip, custom-indicator, style-class |
| Watermark | custom | custom | custom |
| Affix | basic, on-change, debug | basic, target, on-change, debug | basic, on-change, debug |

## 页面错误与无法扫描

- **ProLayout**：捕获 12 个 Blazor demos；页面错误：`SyntaxError: Failed to execute 'replace' on 'DOMTokenList': The token provided must not be empty.`.
- **Breadcrumb**：捕获 5 个 Blazor demos；页面错误：`Uncaught ReferenceError: define is not defined`.
- **Menu**：捕获 16 个 Blazor demos；页面错误：`SyntaxError: Failed to execute 'replace' on 'DOMTokenList': The token provided must not be empty.`.
- **AutoComplete**：捕获 19 个 Blazor demos；页面错误：`SyntaxError: Failed to execute 'replace' on 'DOMTokenList': The token provided must not be empty.`.
- **Cascader**：捕获 21 个 Blazor demos；页面错误：`SyntaxError: Failed to execute 'replace' on 'DOMTokenList': The token provided must not be empty.`.
- **DatePicker**：捕获 30 个 Blazor demos；页面错误：`Uncaught ReferenceError: define is not defined`.
- **Select**：捕获 45 个 Blazor demos；页面错误：`SyntaxError: Failed to execute 'replace' on 'DOMTokenList': The token provided must not be empty.`.
- **TreeSelect**：捕获 18 个 Blazor demos；页面错误：`SyntaxError: Failed to execute 'replace' on 'DOMTokenList': The token provided must not be empty.`.
- **App**：捕获 0 个 Blazor demos；页面错误：`Waiting for selector `.code-box-demo` failed`.
- **没有匹配 demo 可比较**：App (React 2 / Blazor 0), ConfigProvider (React 11 / Blazor 3).

## 系统性样式信号

computed style 差异样本中，最常见的是 Blazor 端出现、React 端没有的全局 token；这类结果不一定是元素视觉差异。样本频次前 10：

- `--ant-error-color`: 681
- `--ant-error-color-active`: 680
- `--ant-error-color-deprecated-bg`: 677
- `--ant-error-color-deprecated-border`: 601
- `--ant-error-color-hover`: 601
- `--ant-error-color-outline`: 601
- `--ant-font-family`: 601
- `--ant-info-color`: 565
- `--ant-info-color-deprecated-bg`: 565
- `--ant-info-color-deprecated-border`: 565

这些高频项包含 `--ant-error-color*`、`--ant-info-color*`、`--ant-primary-1/2` 及字体变量。先剔除跨 demo 一致的全局 token 注入/继承差异，再把剩余组件级样式差异作为修复清单。

## Select 与 Icon 专项复核

- **Select demo 名称**：React 有 6 个 demo 尚未同步到 Blazor：`clear-suffix-debug`、`debug`、`filled-debug`、`placement-debug`、`debug-flip-shift`、`_semantic`。Blazor 独有 demo 15 个，不按缺漏计数。
- **Select 已覆盖状态**：Basic 弹层、尺寸、Semantic DOM 与虚拟列表专用 manifest 共 9 个 scenario 复跑通过，0 个 CSS/DOM difference、0 个 blocked。浏览器打开 Basic 弹层时，两端 popup 都是 120×136px，左上坐标约 `(337, 380)`；trigger 高度和横坐标一致，纵坐标相差约 0.12px。
- **Select 首屏结构**：扫描捕获到每个 Blazor Select 的隐藏 dropdown 节点嵌在 demo 内；React 使用的 portal/挂载范围不同。Basic demo 内 Blazor 捕获到 59 个 Ant 节点，React 22 个，其中 Blazor 多出的 37 个主要是隐藏 dropdown 子树。此差异需要按闭合状态的预期行为继续判断，不能用开启状态门禁通过来掩盖。
- **Icon**：5 个 React demo 均有对应 Blazor demo；HTML 和几何相同。自定义计算样式的差异集中在全局 `--ant-*` 变量，Icon 专用 manifest 的 5/5 场景通过。
- 扫描器已增加 ARIA 引用到 portal 根节点的配对逻辑；单测验证 portal 会合并进所属 demo，demo 内挂载的 overlay 不会重复计入。当前门禁样例以实际 DOM 和样式数据为准。

## 功能状态覆盖与下一阶段

- 现有浏览器 manifests：43 组、48 个 scenario、23 个实际交互 action，覆盖 33 个匹配 demo。另有 DOM test case 配置，不属于这次浏览器执行。
- 合并运行的初步结果为 45/48 个 scenario 的 computed/root style 通过、3 个有差异；随后按各自 manifest 配置复跑，确认 Icon 5/5、Select 9/9 场景通过。合并运行遗漏 Icon 的 SVG 容差配置，相关两条亚像素告警是比较配置造成的误报。
- Select 在浏览器中同时打开 React 与 Blazor Basic demo 后，trigger 几何分别为 `[337.375, 344.48, 120, 32]` 和 `[337.375, 344.36, 120, 32]`；popup 几何两边均为 `[337, 380, 120, 136]`，目视位置一致。
- Tooltip `hover-and-show-arrow` 的合并扫描曾报告宽度/坐标差异；按单独 manifest 复跑后，Blazor 页面报 `define is not defined`，且采集器没有找到与 trigger 对应的可见 popup。因此该项目前**未能验证**，先前的尺寸数值不应作为确认缺陷。需要修复页面脚本错误并改为按 trigger 的 `aria-controls` 关联 popup 后重测。
- 本轮匹配 demo 中其余 743 个未被现有 gate 覆盖到交互状态；其交互后 HTML 和样式还未自动验收。
- 后续应为每个 demo 补录 React / Blazor 对称动作（点击、选择、输入、展开、分页、拖动等），等状态稳定后比较 DOM、computed styles、popup/portal 与几何。动画、异步和随机内容控件需专用稳定条件。
- 优先修复 Tooltip 页面的脚本错误并精确关联目标 popup，之后逐组件收敛首屏差异；同时处理 DOM 属性比较中的 runtime/hash 噪声，再按组件扩充交互 scenario。首屏差异计数不能直接当成已确认缺陷数。

## Typography 专项复扫

- React 15 个 demo、Blazor 11 个，名称匹配 8 个；React 缺口为 `title`、`paragraph-debug`、`editable`、`ellipsis-debug`、`componenttoken-debug`、`link-danger-debug`、`_semantic`。Blazor 多出的 `Level`、`Style`、`Interactive` 不计入缺口。
- 重新按 Ant class 集合匹配运行时节点，并排除继承来的 `--ant-*` 自定义属性后，8 个匹配 demo 中首屏 HTML 树仍有候选差异 7 个，普通 computed CSS 差异 3 个，几何差异 6 个。按 demo：`basic`(31/4/23)、`text`(31/0/12)、`copyable`(22/0/4)、`ellipsis`(11/0/4)、`ellipsis-controlled`(19/3/7)、`ellipsis-middle`(7/0/0)、`suffix`(14/1/3)、`table`(0/0/0)。格式依次为 HTML 属性/节点差异数、CSS 属性差异数、超过 1px 的几何差异数。
- `copyable` 的 React 使用 `<button>`，Blazor 当前使用带 `role=button` 的 `<div>`；这是已确认的结构及键盘语义差异候选。`ellipsis` 与 `ellipsis-controlled` 的省略行数、展开按钮尺寸/位置也有差异候选，需要在同一控件状态下逐项复核。`table` 首屏三类检查一致。
- 全量报告原来的 CSS 总数包含所有继承 `--ant-*` 变量，不能当作缺陷计数；Typography 专项采用修正后的节点匹配和 CSS 口径。全量组件重跑后再更新全局汇总。

## 原始数据与复现

机器可读结果：`/tmp/antd-v6-runtime-demo-parity-2026-10-28.json`。扫描命令：

```sh
node tools/AntDesign.StyleGen/src/documentation-parity-inventory.mjs --antd-path ../ant-design --chrome '/Applications/Google Chrome.app/Contents/MacOS/Google Chrome' --react-base-url http://127.0.0.1:8000 --blazor-base-url http://127.0.0.1:5004 --report /tmp/antd-v6-runtime-demo-parity-2026-10-28.json
```
