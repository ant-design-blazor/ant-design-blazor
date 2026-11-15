# Ant Design v6 runtime parity inventory — 2026-11-01

Full browser inventory of 71 matched React/Blazor routes. Detailed snapshots: [runtime-parity-scan-2026-11-01.json](runtime-parity-scan-2026-11-01.json). Public demo deficits count React names absent from Blazor; Blazor-only demos do not increase the deficit.

## Phase 1 coverage

- V6 CSS: 72/72 components; 3/3 algorithms; token themes 17 migrated with 0 pending.
- Semantic DOM: 61/61 catalog components; 70/70 definitions; 423/423 slots; 70/70 structure mappings.
- Component token pages: 65/65; token rows 584/584; documented global usage rows 1464/1464. All token metadata parity mismatch counts are zero.

## Runtime inventory

- Routes: 74 React, 74 Blazor; 71 matched. React-only routes: BorderBeam 6.4.0, EditableProTable. Blazor-only routes: PageHeader, Comment, BackTop.
- Demos: 1133 React and 1074 Blazor; 867 matching names. Missing public React demo names: 206. 60 underscore-prefixed _semantic* samples are tracked separately because the Blazor equivalent is the page-level Semantic DOM preview, rather than a code-box demo.
- 5 captures have blockers.
- Raw capture reports 757 HTML, 659 CSS, and 560 geometry differences among matched demos. CSS totals are diagnostic: reset-only values such as border-style on zero-width borders inflate them. Review per-demo snapshots before treating a difference as a visible defect. Geometry tolerance is 1px.

| Component | React demos | Blazor demos | Name matches | Missing public React demos | Semantic preview samples | Raw HTML diffs | CSS diffs | Geometry diffs | Capture blockers |
|---|---:|---:|---:|---|---|---:|---:|---:|---|
| Button | 22 | 23 | 22 | — | — | 21 | 19 | 18 | — |
| FloatButton | 18 | 16 | 16 | — | _semantic, _semantic_group | 1 | 0 | 0 | — |
| Icon | 5 | 6 | 5 | — | — | 0 | 0 | 0 | — |
| Typography | 15 | 14 | 14 | — | _semantic | 13 | 7 | 11 | — |
| Divider | 10 | 10 | 10 | — | — | 2 | 6 | 5 | — |
| Flex | 6 | 6 | 6 | — | — | 5 | 2 | 2 | Uncaught ReferenceError: define is not defined |
| Grid | 13 | 13 | 13 | — | — | 2 | 1 | 1 | — |
| ProLayout | 13 | 12 | 12 | — | _semantic | 9 | 9 | 9 | — |
| Masonry 6.0.0 | 7 | 6 | 6 | — | _semantic | 6 | 4 | 4 | — |
| Space | 16 | 12 | 10 | compact, compact-debug, compact-nested, style-class, component-token | _semantic | 10 | 5 | 7 | Uncaught ReferenceError: define is not defined |
| Splitter | 15 | 13 | 13 | nested-in-tabs, debug | — | 12 | 10 | 7 | — |
| Anchor | 13 | 12 | 12 | — | _semantic | 12 | 5 | 5 | — |
| Breadcrumb | 10 | 5 | 2 | withparams, overlay, debug-routes, component-token, withicon, separator-component, style-class | _semantic | 1 | 1 | 1 | — |
| Dropdown | 22 | 19 | 13 | placement, trigger, overlay-open, icon-debug, sub-menu-debug, context-menu, style-class, render-panel | _semantic | 13 | 5 | 5 | — |
| Menu | 19 | 23 | 18 | — | _semantic | 18 | 17 | 17 | — |
| Pagination | 17 | 17 | 16 | — | _semantic | 13 | 11 | 8 | — |
| Steps | 22 | 19 | 17 | max-count, style-class, component-token | _semantic, _semantic_items | 17 | 15 | 15 | — |
| Tabs | 21 | 21 | 20 | — | _semantic | 20 | 19 | 19 | — |
| AutoComplete | 18 | 23 | 17 | — | _semantic | 17 | 10 | 10 | — |
| Cascader | 24 | 21 | 18 | fields-name, ellipsis-debug, custom-render, panel, render-panel | _semantic | 12 | 9 | 10 | — |
| Checkbox | 12 | 17 | 12 | — | — | 12 | 7 | 6 | — |
| ColorPicker | 17 | 9 | 8 | base, text-render, presets-line-gradient, style-class, line-gradient, trigger-event, panel-render, pure-panel | _semantic | 8 | 3 | 4 | — |
| DatePicker | 33 | 31 | 28 | multiple, components, multiple-debug, render-panel | _semantic | 28 | 23 | 22 | — |
| ProForm | 45 | 57 | 44 | — | _semantic | 43 | 39 | 39 | — |
| Input | 32 | 36 | 23 | filled-debug, search-variant-debug, style-class, align | _semantic_input, _semantic_textarea, _semantic_search, _semantic_password, _semantic_otp | 23 | 18 | 16 | — |
| InputNumber | 22 | 19 | 13 | addon, digit, disabled-hover-debug, borderless-height-debug, debug-token, spinner, filled-debug, render-panel | _semantic | 13 | 13 | 6 | — |
| Mentions | 17 | 9 | 8 | popuprender, autosize, render-panel, size, allowclear, autosize-textarea-debug, style-class, component-token | _semantic | 8 | 1 | 1 | — |
| Radio | 17 | 25 | 16 | — | _semantic | 15 | 16 | 10 | — |
| Rate | 9 | 9 | 9 | — | — | 6 | 9 | 4 | — |
| Select | 36 | 51 | 36 | — | — | 18 | 35 | 10 | — |
| Slider | 16 | 17 | 16 | — | — | 16 | 16 | 14 | — |
| Switch | 8 | 6 | 3 | text, style-class, disabled, component-token | _semantic | 3 | 3 | 1 | — |
| TimePicker | 18 | 14 | 13 | 12hours, colored-popup, render-panel, change-on-scroll | _semantic | 13 | 7 | 7 | — |
| Transfer | 14 | 13 | 12 | component-token | _semantic | 12 | 11 | 10 | — |
| TreeSelect | 15 | 18 | 12 | render-panel, component-token | _semantic | 11 | 12 | 9 | — |
| Upload | 25 | 24 | 23 | debug-disabled | _semantic | 23 | 23 | 23 | — |
| Avatar | 10 | 10 | 10 | — | — | 10 | 9 | 8 | — |
| Badge | 19 | 13 | 13 | ribbon-debug, component-token, style-class, colorful-with-count-debug | _semantic, _semantic_ribbon | 13 | 13 | 13 | — |
| Calendar | 11 | 5 | 5 | event-range, lunar, week, style-class, component-token | _semantic | 5 | 5 | 5 | — |
| Card | 16 | 16 | 14 | no-body-debug, button-alignment-debug | — | 14 | 14 | 12 | — |
| Carousel | 7 | 5 | 1 | autoplay, arrows, component-token, placement, fade, dot-duration | — | 1 | 1 | 0 | — |
| Collapse | 14 | 14 | 13 | — | _semantic | 11 | 13 | 5 | — |
| ProDescriptions | 14 | 7 | 5 | text, padding, size, style, style-class, jsx, component-token, block | _semantic | 5 | 5 | 5 | — |
| Empty | 7 | 6 | 6 | — | _semantic | 6 | 5 | 1 | — |
| Image | 18 | 14 | 11 | toolbarrender, preview-mask, coverplacement, preview-group-top-progress, component-token, preview-imginfo | _semantic | 11 | 10 | 5 | — |
| ProList | 17 | 19 | 17 | — | — | 17 | 16 | 9 | — |
| Listy 6.6.0 | 9 | 1 | 1 | virtual, group, rich, drag-sorting, infinite, style-class, scroll-to | _semantic | 1 | 1 | 1 | — |
| Popover | 13 | 6 | 5 | placement, style-class, wireframe, arrow, shift, render-panel, component-token | _semantic | 4 | 4 | 2 | — |
| QRCode | 12 | 13 | 11 | — | _semantic | 8 | 9 | 2 | — |
| Segmented | 17 | 9 | 9 | style-class, size-consistent, vertical, shape, with-name, controlled-two, componenttoken | _semantic | 9 | 9 | 5 | — |
| Statistic | 8 | 5 | 3 | animated, timer, component-token, style-class | _semantic | 3 | 3 | 3 | — |
| ProTable | 53 | 76 | 52 | — | _semantic | 52 | 47 | 51 | — |
| Tag | 14 | 14 | 11 | disabled, customize, component-token | — | 11 | 7 | 8 | — |
| Timeline | 16 | 13 | 8 | pending, horizontal-debug, end, pending-legacy, semantic, component-token | _semantic, _semantic_items | 8 | 8 | 7 | — |
| Tooltip | 16 | 11 | 8 | shift, force-align, debug, smooth-transition, destroy-on-close, render-panel, wrap-custom-component | _semantic | 7 | 6 | 6 | — |
| Tour | 10 | 8 | 8 | render-panel | _semantic | 7 | 7 | 7 | — |
| Tree | 20 | 14 | 11 | directory-debug, drag-debug, block-node, multiple-line, line-debug, scroll-to, component-token, style-class | _semantic | 11 | 9 | 9 | — |
| Alert | 16 | 17 | 16 | — | — | 15 | 14 | 9 | — |
| Drawer | 19 | 19 | 11 | multi-level-drawer, style-class, scroll-debug, render-in-current, config-provider, render-panel, component-token | _semantic | 11 | 8 | 7 | — |
| Message | 12 | 7 | 5 | hooks, style-class, component-token, stack, thenable, render-panel | _semantic | 2 | 2 | 2 | — |
| Modal | 24 | 34 | 20 | dark, render-panel, wireframe | _semantic | 11 | 8 | 6 | — |
| Notification | 15 | 9 | 7 | hooks, show-with-progress, progress-color, render-panel, stack, component-token, style-class | _semantic | 3 | 2 | 2 | Uncaught ReferenceError: define is not defined |
| Popconfirm | 12 | 6 | 4 | placement, async, style-class, wireframe, shift, promise, render-panel | _semantic | 4 | 0 | 1 | — |
| Progress | 17 | 13 | 11 | circle-steps, info-position, circle-micro, size, style-class | _semantic | 11 | 11 | 11 | — |
| Result | 11 | 12 | 10 | — | _semantic | 10 | 6 | 7 | — |
| Skeleton | 10 | 8 | 8 | — | _semantic, _semantic_element | 8 | 7 | 3 | — |
| Spin | 11 | 8 | 7 | percent, fullscreen, list-debug | _semantic | 6 | 7 | 7 | Uncaught ReferenceError: define is not defined |
| Watermark | 6 | 6 | 4 | portal, debug | — | 1 | 1 | 1 | — |
| Affix | 4 | 6 | 4 | — | — | 3 | 0 | 3 | — |
| App | 2 | 0 | 0 | basic, config | — | 0 | 0 | 0 | Waiting for selector `.code-box-demo` failed |
| ConfigProvider | 11 | 6 | 4 | locale, wave, holderrender, prefixcls, useconfig, warning, focus-outline-debug | — | 2 | 1 | 1 | — |

## Largest public demo gaps

| Component | Missing count | React demo names to add/align |
|---|---:|---|
| Dropdown | 8 | placement, trigger, overlay-open, icon-debug, sub-menu-debug, context-menu, style-class, render-panel |
| ColorPicker | 8 | base, text-render, presets-line-gradient, style-class, line-gradient, trigger-event, panel-render, pure-panel |
| InputNumber | 8 | addon, digit, disabled-hover-debug, borderless-height-debug, debug-token, spinner, filled-debug, render-panel |
| Mentions | 8 | popuprender, autosize, render-panel, size, allowclear, autosize-textarea-debug, style-class, component-token |
| ProDescriptions | 8 | text, padding, size, style, style-class, jsx, component-token, block |
| Tree | 8 | directory-debug, drag-debug, block-node, multiple-line, line-debug, scroll-to, component-token, style-class |
| Breadcrumb | 7 | withparams, overlay, debug-routes, component-token, withicon, separator-component, style-class |
| ConfigProvider | 7 | locale, wave, holderrender, prefixcls, useconfig, warning, focus-outline-debug |
| Listy 6.6.0 | 7 | virtual, group, rich, drag-sorting, infinite, style-class, scroll-to |
| Popover | 7 | placement, style-class, wireframe, arrow, shift, render-panel, component-token |
| Segmented | 7 | style-class, size-consistent, vertical, shape, with-name, controlled-two, componenttoken |
| Tooltip | 7 | shift, force-align, debug, smooth-transition, destroy-on-close, render-panel, wrap-custom-component |
| Drawer | 7 | multi-level-drawer, style-class, scroll-debug, render-in-current, config-provider, render-panel, component-token |
| Notification | 7 | hooks, show-with-progress, progress-color, render-panel, stack, component-token, style-class |
| Popconfirm | 7 | placement, async, style-class, wireframe, shift, promise, render-panel |
| Carousel | 6 | autoplay, arrows, component-token, placement, fade, dot-duration |
| Image | 6 | toolbarrender, preview-mask, coverplacement, preview-group-top-progress, component-token, preview-imginfo |
| Timeline | 6 | pending, horizontal-debug, end, pending-legacy, semantic, component-token |
| Message | 6 | hooks, style-class, component-token, stack, thenable, render-panel |
| Space | 5 | compact, compact-debug, compact-nested, style-class, component-token |
