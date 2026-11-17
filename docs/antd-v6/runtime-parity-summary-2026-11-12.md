# React v6 / Blazor v6 runtime parity inventory — 2026-11-12 snapshot (focused updates through 2026-11-17)

## Scope and interpretation

This is a fresh rendered-page inventory of the current React static docs at `http://127.0.0.1:8000` and Blazor Development docs at `http://127.0.0.1:5000`. It covers 71 matched component routes. Demo counts and names were collected from the live pages; for each matched demo, the scanner compared its selected rendered component nodes for normalized DOM, computed CSS, and geometry. A difference count below means the number of matched demo captures with at least one difference in that category, not the number of individual DOM attributes or CSS properties.

Across the inventory: React 912 demos; Blazor 1106; 762 matched names; 150 React demos not matched by name; 344 Blazor-only demo names. Matched-demo captures report 615 DOM, 454 CSS, and 411 geometry differences.

**Capture limitation:** all 71 React pages logged hydration error #418. Eight pages also logged `define is not defined`; one logged a spacer callback argument error; App could not find `.code-box-demo`. The scanner still recorded the DOM that was present, but the inventory is a candidate map, not a clean browser acceptance gate. Use focused fixtures and browser interaction checks before marking any component complete.

## Phase 1 source and documentation coverage

| Area | Coverage | Current evidence |
|---|---:|---|
| v6 component CSS IR | 72/72 | `phase1-progress.json` |
| Theme files / algorithms | 17/13 theme targets; 3/3 algorithms | Source coverage only |
| Semantic DOM catalog / definitions / slots | 61/61; 70/70; 423/423 | Includes 70/70 structure mappings |
| Component token docs | 65/65 pages; 583/583 rows | Generated component documentation |
| Global token docs | 64/64 pages; 1464/1464 rows | Generated usage mapping |
| Runtime DOM/CSS and interaction parity | Partial | Per-component fixtures listed in `dom-style-parity.md`; all-component runtime parity is not complete |

## Current status by component

`H/C/G` are matched demos with a DOM / computed CSS / geometry difference, respectively. “React-only demo names” are current names missing from Blazor; an em dash means none. A zero in H/C/G is only the broad inventory capture result and does not independently prove full interaction, token, Semantic DOM, or visual acceptance.

| Component | React demos | Blazor demos | Matched | React-only demo names | H | C | G |
|---|---:|---:|---:|---|---:|---:|---:|
| Button | 16 | 23 | 16 | — | 3 | 7 | 1 |
| FloatButton | 16 | 18 | 16 | — | 2 | 2 | 2 |
| Icon | 5 | 6 | 5 | — | 0 | 0 | 0 |
| Typography | 11 | 14 | 10 | _semantic | 7 | 1 | 4 |
| Divider | 8 | 10 | 8 | — | 1 | 4 | 3 |
| Flex | 5 | 6 | 5 | — | 3 | 1 | 1 |
| Grid | 13 | 13 | 13 | — | 2 | 1 | 1 |
| ProLayout | 11 | 13 | 11 | — | 8 | 7 | 8 |
| Masonry 6.0.0 | 6 | 7 | 6 | — | 6 | 4 | 4 |
| Space | 11 | 18 | 11 | — | 10 | 8 | 9 |
| Splitter | 12 | 15 | 12 | — | 11 | 9 | 6 |
| Anchor | 10 | 13 | 10 | — | 10 | 5 | 5 |
| Breadcrumb | 9 | 8 | 5 | overlay, debug-routes, style-class, _semantic | 2 | 4 | 2 |
| Dropdown | 18 | 23 | 17 | _semantic | 16 | 7 | 7 |
| Menu | 12 | 23 | 11 | _semantic | 11 | 9 | 9 |
| Pagination | 14 | 17 | 13 | _semantic | 7 | 4 | 5 |
| Steps | 15 | 19 | 11 | max-count, style-class, _semantic, _semantic_items | 11 | 10 | 10 |
| Tabs | 17 | 21 | 16 | _semantic | 16 | 16 | 16 |
| AutoComplete | 11 | 24 | 11 | — | 11 | 7 | 5 |
| Cascader | 21 | 21 | 17 | fields-name, custom-render, panel, _semantic | 11 | 8 | 9 |
| Checkbox | 8 | 17 | 8 | — | 5 | 4 | 3 |
| ColorPicker | 15 | 9 | 8 | base, text-render, panel-render, line-gradient, trigger-event, style-class, _semantic | 8 | 3 | 4 |
| DatePicker | 26 | 36 | 24 | components, _semantic | 23 | 19 | 18 |
| ProForm | 38 | 57 | 37 | _semantic | 37 | 18 | 16 |
| Input | 23 | 36 | 17 | style-class, _semantic_input, _semantic_textarea, _semantic_search, _semantic_password, _semantic_otp | 17 | 12 | 10 |
| InputNumber | 15 | 19 | 12 | spinner, digit, _semantic | 12 | 7 | 6 |
| Mentions | 14 | 9 | 8 | popuprender, autosize, style-class, size, allowclear, _semantic | 8 | 1 | 1 |
| Radio | 12 | 25 | 11 | _semantic | 8 | 6 | 6 |
| Rate | 8 | 9 | 8 | — | 5 | 5 | 3 |
| Select | 28 | 54 | 28 | — | 1 | 1 | 1 |
| Slider | 15 | 17 | 15 | — | 15 | 15 | 13 |
| Switch | 7 | 6 | 3 | text, disabled, style-class, _semantic | 3 | 2 | 2 |
| TimePicker | 16 | 14 | 13 | 12hours, change-on-scroll, _semantic | 13 | 7 | 7 |
| Transfer | 12 | 13 | 11 | _semantic | 11 | 11 | 10 |
| TreeSelect | 13 | 18 | 12 | _semantic | 11 | 9 | 9 |
| Upload | 22 | 25 | 22 | — | 19 | 12 | 12 |
| Avatar | 7 | 10 | 7 | — | 7 | 6 | 6 |
| Badge | 14 | 13 | 11 | style-class, _semantic, _semantic_ribbon | 11 | 11 | 11 |
| Calendar | 10 | 5 | 5 | event-range, lunar, week, style-class, _semantic | 5 | 5 | 5 |
| Card | 13 | 18 | 13 | — | 13 | 10 | 11 |
| Carousel | 6 | 5 | 1 | autoplay, arrows, placement, fade, dot-duration | 1 | 0 | 0 |
| Collapse | 13 | 14 | 12 | _semantic | 10 | 6 | 5 |
| ProDescriptions | 9 | 7 | 5 | size, style-class, block, _semantic | 5 | 5 | 5 |
| Empty | 7 | 6 | 6 | _semantic | 6 | 1 | 1 |
| Image | 13 | 14 | 11 | toolbarrender, _semantic | 11 | 9 | 5 |
| ProList | 14 | 19 | 14 | — | 14 | 9 | 7 |
| Listy 6.6.0 | 8 | 1 | 1 | virtual, group, rich, drag-sorting, infinite, style-class, _semantic | 1 | 1 | 1 |
| Popover | 9 | 6 | 4 | placement, shift, arrow, style-class, _semantic | 3 | 4 | 2 |
| QRCode | 12 | 13 | 11 | _semantic | 8 | 3 | 2 |
| Segmented | 14 | 9 | 9 | style-class, vertical, shape, with-name, _semantic | 9 | 9 | 5 |
| Statistic | 7 | 5 | 3 | animated, timer, style-class, _semantic | 3 | 3 | 3 |
| ProTable (Table route) | 43 | 77 | 43 | — | 37 | 10 | 10 |
| Tag | 11 | 14 | 11 | — | 11 | 5 | 8 |
| Timeline | 13 | 13 | 8 | pending, end, semantic, _semantic, _semantic_items | 8 | 8 | 7 |
| Tooltip | 11 | 11 | 6 | shift, wrap-custom-component, smooth-transition, force-align, _semantic | 5 | 5 | 5 |
| Tour | 9 | 9 | 9 | — | 8 | 7 | 7 |
| Tree | 14 | 14 | 10 | scroll-to, style-class, block-node, _semantic | 10 | 9 | 9 |
| Alert | 14 | 17 | 14 | — | 13 | 12 | 8 |
| Drawer | 14 | 19 | 10 | multi-level-drawer, style-class, render-in-current, _semantic | 10 | 7 | 6 |
| Message | 10 | 7 | 5 | hooks, style-class, stack, thenable, _semantic | 2 | 2 | 2 |
| Modal | 18 | 34 | 17 | _semantic | 9 | 7 | 5 |
| Notification | 13 | 9 | 7 | hooks, show-with-progress, progress-color, stack, style-class, _semantic | 3 | 2 | 2 |
| Popconfirm | 10 | 6 | 4 | placement, async, style-class, shift, promise, _semantic | 4 | 0 | 1 |
| Progress | 17 | 13 | 11 | circle-steps, info-position, circle-micro, size, style-class, _semantic | 11 | 11 | 11 |
| Result | 10 | 12 | 9 | _semantic | 9 | 4 | 6 |
| Skeleton | 9 | 8 | 7 | _semantic, _semantic_element | 7 | 6 | 3 |
| Spin | 10 | 8 | 7 | percent, fullscreen, _semantic | 6 | 7 | 7 |
| Watermark | 5 | 6 | 4 | portal | 1 | 1 | 1 |
| Affix | 3 | 6 | 3 | — | 1 | 0 | 1 |
| App | 2 | 2 | 2 | — | 0 | 0 | 0 |
| ConfigProvider | 7 | 6 | 4 | locale, wave, holderrender | 4 | 3 | 3 |

The Upload row is refreshed from the 2026-11-15 one-component route scan; the
other rows remain the original broad-scan snapshot and must be rechecked before
they are treated as current status.

## Focused rechecks completed this round

| Component / scope | Result | Evidence |
|---|---|---|
| Slider — Basic | Changed default value 35→30 and removed the Blazor-only third marks slider. Aligned single/range root-child structure with React (no tracks wrapper for ordinary values, no empty mark node), handle orientation ARIA, percent serialization, and moved `user-select:none` to the component stylesheet behavior. | Focused fixture passes 1/1 with normalized DOM tree, selected computed CSS, geometry, and visual styles matching. Raw HTML serialization still differs by attribute order/renderer markers. `/tmp/slider-basic-dom-style-parity-20261116-final.json`; Docs.Server `net11.0` build: 0 errors (repository warnings remain). Remaining Slider demos, API, Semantic DOM and token docs are not complete. |
| Table — registered fixture suite | 16 groups / 20 scenario states pass with zero normalized DOM/CSS/geometry/visual-style differences. Covers Basic, bordered/size, colspan/rowspan, defaults/expand, custom empty, ellipsis/tooltip, fixed columns/header, sorting, pagination placement, row selection, and summary. This is not all 43 matched demos. | `/tmp/table-suite-active-goal-20261113-final.json`; details in `dom-style-parity.md` |
| Table — Ajax | Aligned the MockAPI request, page/sort/filter parameters, Name/Gender/Email columns, and Male filter interaction. | Initial load, Name sort, and Male filter pass 3/3 with zero normalized DOM/CSS/geometry/visual-style differences. `/tmp/table-ajax-dom-style-parity-20261117-final.json`; details in `dom-style-parity.md`. |
| Table — Responsive columns | Added React-compatible breakpoint values to data, selection, and action columns; aligned the demo's columns, sample row, and default pagination. Responsive breakpoints now resolve from the active `ConfigProvider` theme tokens, as React's `responsiveObserver` does, instead of fixed widths; `xxxl` follows the 1920px default. | Desktop 1280px, medium 800px, and narrow 600px pass 3/3 with zero normalized DOM/CSS/geometry/visual-style differences. New bUnit coverage confirms custom `screenMD=900` hides at 899px and shows at 900px (2/2). Components and Docs.Server `net11.0` builds succeed. `/tmp/table-responsive-columns-20261115e.json`; focused test `TableResponsiveTests`; details in `dom-style-parity.md`. |
| Table — Tree Data | Replaced the nonmatching radio/action/lazy-load demo with React's three-level tree, CheckStrictly switch, and checkbox selection. Initial, strict-mode toggle, and selecting a parent pass 3/3 with zero normalized DOM/CSS/geometry/visual-style differences. Removed implicit checkbox `name`, added tree expansion `aria-expanded`, and aligned selected-row accessible labels. | `/tmp/table-tree-data-final4.json`; details in `dom-style-parity.md` |
| Select — refreshed full-route inventory | 28/28 React demo names matched; 23 Blazor-only names, no React-only names. With 0.02px serialization tolerance, broad scan reports 1 HTML / 1 CSS / 1 geometry demo difference; the remaining differences are concentrated in the `_semantic` documentation preview, while the focused Select fixtures below pass. React hydration #418 still blocks clean capture. | `/tmp/select-inventory-active-goal-20261113-final.json`; details in `dom-style-parity.md` |
| Icon — IconFont verification | Matched sprite precedence across lazy-loaded demos by ordering the loaded SVG sprites against their owning demo positions. All five React Icon examples now pass with zero DOM/CSS/geometry/visual differences. | `/tmp/icon-dom-style-parity-final-20261113.json`; details in `dom-style-parity.md` |
| Table — Filter in Tree | Replaced the checkbox-only custom popup with the nested native Table Tree filter and aligned data/column behavior with React. The fixture passes initial and open-popup states (2/2). Broad inventory now has 37 HTML, 10 computed CSS, and 10 geometry differences across 43 matched names; 33 Blazor-only demo names; no React-only names. React page capture remains blocked by hydration error #418. | `/tmp/table-filter-in-tree-active-goal-20261113-final.json`; `/tmp/table-inventory-active-goal-20261113-final.json`; details in `dom-style-parity.md` |
| Table — Dynamic Settings | Matched React's inline controls and labels, separate top/bottom pagination, default 10-row page, Address filters, and More actions chevron. Browser confirmed TopStart pagination, Medium size, and London/New York filtering. Automated capture was blocked because isolated Chrome timed out navigating the React docs; the docs main-column width and Action-column sorter remain differences. | `/tmp/table-dynamic-settings-20261113.json`; React/Blazor browser comparison; details in `dom-style-parity.md` |
| Form — Basic validation | Required messages, error structure, `aria-describedby` IDs and password status placement now match React. Initial, password-visible, and empty-submit states pass 3/3; browser confirms validation errors and ID associations. | `/tmp/form-basic-active-goal-20261113-final4.json`; details in `dom-style-parity.md` |
| Form — Nested dynamic lists | React-like Card/Form.List hierarchy and registered-value preview; adding/removing rows and typing field values pass all 7 focused scenarios with zero normalized DOM/CSS/geometry/visual-style differences. Browser confirms live preview updates and no Blazor-only input `name` attributes. | `/tmp/form-dynamic-complex-complete.json`; details in `dom-style-parity.md` |
| Form — Dynamic passenger list | Responsive passenger fields, initial empty state, add, add-at-head, delete icons, and minimum two-passenger feedback. Four focused states pass with zero normalized DOM/CSS/geometry/visual-style differences. | `/tmp/form-dynamic-form-item-passengers-final2.json`; details in `dom-style-parity.md` |
| FloatButton — square Group and Draggable | Square group: 16px row gaps, no shared list shadow, per-item shadow; draggable pointer interaction moved the button by (-146px, -102px). Global BackTop remains at viewport bottom-right. Focused Blazor browser check only; no full React comparison fixture. | `/tmp` browser session at `http://127.0.0.1:5010/en-US/components/floatbutton#components-floatbutton-demo-draggable`; details in `dom-style-parity.md` |
| Select — Semantic DOM initial open and mode switch | Fixed React parity: constant-open popup survives initial render and Single/Multiple mode changes. Browser capture shows same popup/option sizes, relative placement, and selected-option background. Framework IDs and Blazor render markers remain in raw HTML. | Docs.Server `net10.0` build (0 errors); React and Blazor browser tabs; details in `dom-style-parity.md` |
| Select — responsive tag measurement | Responsive multiple Select now keeps React's hidden `+ 0 ...` aggregate tag mounted for measurement when no chips overflow; the rendered semantic node matches React. | `/tmp/select-semantic-preview-repeatable-20261113.json`; details in `dom-style-parity.md` |
| Select — controlled-open focus; Icon — responsive gallery | Controlled-open Select leaves focus on the external mode control, matching React. Normalized semantic DOM/CSS/geometry/visual styles pass in both preview states (2/2); normal Select interactions pass 4/4. Icon gallery responsive fixture passes desktop/narrow states 2/2. | `/tmp/select-semantic-preview-repeatable-20261113.json`; `/tmp/select-interaction-focus-fix-retry-20261113.json`; `/tmp/icon-gallery-responsive-current2-20261113.json` |
| Select — search, multi-select, clear, outside dismissal | 4/4 automated interaction scenarios pass with no normalized DOM/CSS/geometry/visual-style differences. Fixture explicitly associates portal popups with the correct Select trigger to avoid stale lazy-mounted popups. | `/tmp/select-interaction-parity-20261113.json`; `select-interaction-dom-style-parity.json` |
| DatePicker — Basic and Multiple | Basic popup plus Flex demo layout pass 2/2; Multiple responsive tags pass 1/1. Updated the Basic demo to use React's `flex-start` alignment classes. Broad inventory still has two React-only names (`components`, `_semantic`). | `/tmp/datepicker-basic-flex-sync-20261113-final.json`; `/tmp/datepicker-multiple-continue-20261113.json` |
| Select — outside-dismiss follow-up | Outside-click scenario passes after event serialization began including `clientX/clientY`; full fixture has 3/4 strict passes because the search-filter scenario's raw Razor HTML whitespace differs, while normalized DOM and computed styles match. | `/tmp/select-outside-close-recheck-20261113.json`; details in `dom-style-parity.md` |
| Select — Custom Tag Render | 1/1 passed; normalized DOM, selected CSS, geometry, visual style; no blocked cases | `/tmp/select-custom-tag-render-active-goal-20261112.json` |
| Select — Semantic DOM closed/open | 2/2 passed; no DOM/CSS/geometry/visual differences; no blocked cases | `/tmp/select-semantic-current-active-goal-20261112.json` |
| Icon — gallery toolbar desktop/narrow | 2/2 passed; no DOM/CSS/geometry/visual differences; no blocked cases | `/tmp/icon-gallery-responsive-current-active-goal-20261112.json` |

Icon’s fixed six-column gallery label behavior remains an intentional documented difference: Blazor wraps labels to avoid clipping. Focused Icon canonical demos were previously recorded as 5/5, and Select popup/size/interaction fixtures have prior focused passes; see `dom-style-parity.md` for the detailed dated records. Those runs do not complete all demos in either component.

## Next work at snapshot time (superseded by the follow-up below)

1. Finish Select's `_semantic` documentation-preview discrepancy and register focused checks for remaining component states; its existing 10 focused scenarios pass, but one broad demo remains different.
2. Continue Icon gallery, search, and two-tone behavior checks; the canonical IconFont duplicate-symbol discrepancy now passes after sprite precedence was aligned.
3. Continue Form with the remaining high-difference demos and validation states; Basic validation and Nested Dynamic Form Lists now have focused passes.
4. Resume Table by adding focused fixtures for the 26 currently unregistered matched demos, then triage its remaining broad-inventory differences.
5. Repair or isolate the React documentation hydration error (#418), which still blocks reliable broad-page DOM capture.

## Follow-up on 2026-11-13

The table above is the 71-component inventory snapshot and remains the
component-by-component baseline. Focused demo implementation and verification
since that snapshot:

| Component | Current update | Verification / remaining gap |
|---|---|---|
| Docs CodeBox — dark demo title | Added dark-mode default and hover colors for the demo title's edit icon; title and demo backgrounds both compute to `#141414`. | Browser checked the `Infinite Load` title: edit icon white at 45% opacity normally and white at 85% on hover. Light theme retains its prior color. |
| List — Vertical | Rebuilt the demo to match React's 23 rows, Large layout, 3-row pagination, avatar URLs, actions, footer, and responsive image; removed leaked `key` attributes from shared ListItem markup. | Focused fixture passes 1/1 with zero normalized DOM/CSS/geometry/visual-style differences. Build passes with 0 errors. |
| Table — Fixed Gapped Columns | Removed extra widths from auto-sized columns and the demo-only class; set `TableLayout="auto"` and added React's row key. | Computed CSS, geometry, and visual styles now match; six DOM differences remain in missing fixed-edge shadow classes. Continue in shared Table fixed-column class logic. |
| DatePicker | Select range dates demo now implements React's 7-day and 6-month disabled-date behavior. | Browser selected 2026-11-15; 11-08/11-22 disabled and 11-09/11-21 enabled. Docs.Server `net10.0` build passes; RangePicker DOM attributes/classes still differ. |
| Table — Reset Filter | Replaced custom checkbox popups and local row filtering with native Table column filters, matching React's filters, ellipsis, and sort controls. | Browser verified descending age sort, Jim filter, clearing filters while retaining sort, and clearing both filters and sorters. React page button handlers remain unavailable in this session due hydration error #418; behavior is cross-checked against `components/table/demo/reset-filter.tsx`. |
| AntList | Infinite Load and Infinite & virtualized are implemented; the latter window-renders 19 rows and appends pages on bottom scroll. | Browser checks confirmed 10→20 and 20→40 logical rows respectively, without runtime errors. Focused React DOM/CSS fixtures are still pending; sample data is local and deterministic. |
| Tabs | Replaced the empty Nest placeholder with React's four parent/child placement/type controls and 20 nested panes. | Full Docs.Server build passes with 0 errors. Focused browser parity fixture remains pending. |
| Transfer | Added React-compatible `SelectAllLabels` static/render-template API and implemented the empty Custom Select All Labels demo with source/target-specific labels and count context. | Demo metadata regenerated; full Docs.Server build passes with 0 errors. Focused browser interaction and React DOM/CSS comparison remain pending. |
| Tree | BigData and VirtualScroll remain unimplemented placeholders; the component has no `Height` or virtual-window API. | `Tree.razor` recursively instantiates every data node, while `TreeNode.RealDisplay` only suppresses markup for collapsed descendants and all node instances remain registered for selection/expansion state. Implement a state-preserving flattened visible-row window before enabling the 10k/11,110-node React demos; wrapping the current tree in a fixed-height scroller would not match React virtualization. |

## Latest focused parity updates — 2026-11-13

The 71-component matrix above remains the full component-by-component baseline;
this table records the current focused rechecks and the next component. Passing
focused fixtures do not imply that every demo on a component page is complete.

| Component | Current status | Evidence / remaining work |
|---|---|---|
| Select | Semantic DOM preview Single/Multiple states pass 2/2; interactions pass 4/4. Inventory matched all 28 React demo names with no blocked pages; its lone raw-HTML/geometry flag is on `_semantic` (style whitespace serialization and off-screen popup coordinates after lazy-load scrolling). | `/tmp/select-semantic-preview-focus-fix-20261113.json`; `/tmp/select-interaction-after-focus-fix-20261113.json`; `/tmp/select-icon-inventory-recheck-20261113.json`; continue remaining demo and token coverage. |
| Icon | Five canonical React demos and 842-icon responsive gallery pass focused checks; search input geometry now matches React at 1280px and 800px. React clips long labels while Blazor intentionally wraps them. | `/tmp/icon-canonical-review-20261115.json`; `/tmp/icon-gallery-review-20261115.json`; toolbar geometry details in `dom-style-parity.md`. Remaining whole-route demo/API/token coverage is open. |
| List | Vertical demo passes 1/1 against React, including item count, pagination, avatar, action, image and footer structure/styles. | `/tmp/list-vertical-parity-20261113-verified.json`; other List demos still need coverage. |
| Table | Registered suite passes 16 groups/20 states. Gapped fixed columns, regular fixed columns, and fixed-column headers pass 1/1 each; Reset Filter initial, sort, and toolbar checks pass 3/3. Latest inventory: 43/43 React demo names present; 35 HTML, 8 CSS, and 8 geometry demo flags. | `/tmp/table-suite-active-goal-20261113-final.json`; `/tmp/table-fixed-gapped-columns-segment-fix-20261113.json`; `/tmp/table-fixed-columns-regression-20261113.json`; `/tmp/table-fixed-columns-header-regression-20261113.json`; `/tmp/table-reset-filter-verified-20261113.json`; `/tmp/table-inventory-reset-filter-20261113.json`. Next: Dynamic Settings / Style and Class / Virtual List and drag demos. |
| Next | Resume Select and Icon documentation-page discrepancies first, then continue the remaining component/demo mismatch queue by volume. | Full 71-component matrix above; detailed dated evidence in `dom-style-parity.md`. |

The latest Docs.Server full build passed with 0 errors (3996 existing warnings
across the multi-target build); a focused `AntDesign.Docs` `net10.0` build also
passed with 0 errors. The React documentation hydration error and the incomplete broad
runtime inventory remain open. Detailed targeted results and limitations are
recorded in `dom-style-parity.md`.

## Follow-up on 2026-11-14

| Component | Current update | Verification / next |
|---|---|---|
| List — Vertical | Rechecked desktop, tablet, and narrow widths; the reported overflow was not reproducible and the demo stays within its card. | Focused fixture 1/1; `/tmp/list-vertical-final.json`. |
| Table — Dynamic Settings | Aligned React sortable Action column, expand/selection order, inline control spacing, and action wrappers. | Focused states 3/3; `/tmp/table-dynamic-settings-regression-20261114.json`. |
| Table — Style and Class | Added Table `ClassNamesFunc` / `StylesFunc`, moved semantic root customization onto `.ant-table-wrapper`, and rebuilt the two-table semantic styling demo. | Medium function-style table passes 1/1 with zero normalized DOM/CSS/geometry/visual differences. First React table still uses virtual rows while Blazor currently uses a scroll container; see dated details in `dom-style-parity.md`. |
| Table — shared regressions | Pagination simple-mode accessibility/style, Table title/footer whitespace, and row/expand column behavior rechecked. | Row selection 3/3 (`/tmp/table-row-selection-final-20261114.json`); expand columns 3/3 (`/tmp/table-expand-columns-regression-20261114b.json`). |
| Next | Resolve Table virtualization/runtime alignment, then continue Virtual List and drag demos. | Full component inventory remains the baseline above; detailed evidence and limitations are in `dom-style-parity.md`. |

Docs.Server `net10.0` build passes with 0 errors (3996 existing warnings). The
React and Blazor Table Style and Class pages are open for manual comparison. No
changes were committed.

## Focused continuation — 2026-11-14

| Component | Current update | Verification / next |
|---|---|---|
| List — Vertical | Confirmed and retained the component patch matching the rendered React docs cascade: extra image `display:block; max-width:100%`, meta title `font-weight:400`. | Focused DOM/CSS/geometry/visual-style fixture passes 1/1 with zero differences. Normalized DOM tree and all 10 semantic selectors match; raw HTML serialization has Razor whitespace differences only. `/tmp/list-vertical-parity-20261114-final.json`. |
| Next | Resolve Table virtualization/runtime alignment, then proceed with Table Virtual List and drag demos. | Full 71-component inventory is the baseline matrix above; updated component-specific results are tracked in `dom-style-parity.md`. |

The List page is open at `http://127.0.0.1:5000/en-US/components/list#components-list-demo-vertical` for manual review. The focused `AntDesign` net10.0 build passed with 0 errors (3 existing dependency/runtime advisory warnings). The broad 71-component baseline remains incomplete and no changes were committed.

The viewport matrix was extended after discovering the narrow-screen grid-width collapse. `Components.razor` now compensates for the demo Row's 8px gutters. Rebuilt Docs.Server and rechecked: List Vertical passes desktop, tablet, and 390px narrow-screen scenarios (3/3), with zero normalized DOM, computed CSS, geometry, or visual-style differences. Report: `/tmp/list-vertical-responsive-final-20261114.json`.

## Table virtualization continuation — 2026-11-14

| Component | Current status | Verification / next |
|---|---|---|
| List — Vertical | Desktop, tablet, and 390px responsive scenarios pass; normalized DOM/CSS/geometry/visual styles all match React. | `/tmp/list-vertical-responsive-final-20261114.json` |
| Table — Style and Class, ordinary table | Medium function-style table remains aligned after rebuilding and switching Docs.Server to native .NET 11. | `/tmp/table-style-class-function-20261114.json` (1/1 pass) |
| Table — Style and Class, virtual table | Virtualization is enabled and .NET 11's served runtime includes `Blazor._internal.Virtualize.setAnchorMode`; DOM remains structurally different from React's virtual table. Blazor emits Virtualize spacer rows within a regular `<tbody>`; React uses a virtual holder with flex `div` rows/cells. | `/tmp/table-style-class-virtual-20261114.json` (1 scenario, differences); implement React-compatible virtual markup/layout. |
| Next | Continue Table virtualization, then Virtual List and drag demos; retain Select/Icon page-level checks and global/component Design Token coverage. | Full 71-component inventory and per-component counts remain the baseline table above. |

Docs.Server now targets `net11.0`; Docs targets `net11.0` in addition to its
existing frameworks. The `net11.0` build succeeds with 0 errors (23 warnings).
The Blazor Style and Class Table page remains open at
`http://127.0.0.1:5000/en-US/components/table#components-table-demo-style-class`
for manual review. The 71-component inventory above is still a dated baseline,
not a complete parity result.

## Focused continuation — 2026-11-14 (latest)

| Component | Current update | Verified state / remaining |
|---|---|---|
| List — Vertical | Rechecked after the reported style concern. | Desktop, tablet, and 390px narrow-screen comparisons remain 3/3 with zero normalized DOM/CSS/geometry/visual-style differences; current report `/tmp/list-vertical-current-20261114.json`. |
| Table — Style and Class, ordinary | Rebuilt and restarted the native .NET 11 docs server after the virtual Table changes. | `/tmp/table-style-class-normal-final-20261114.json`: 1/1, zero normalized DOM/CSS/geometry/visual-style differences. |
| Table — Style and Class, virtual | Matched React's holder overflow mode based on whether rows need scrolling; synchronized virtual widths from `.ant-table` content width. | `/tmp/table-style-class-virtual-final-20261114.json`: normalized DOM and root styles match. Six visual geometry measurements remain 0.016px narrow on 3 rows and their last cells; continue without adding a visual-width hack. |
| React docs capture | Current React Table route still logs `define is not defined` and contributor endpoint 404 errors. | Focused fixture captured the Table demo despite page-level errors; broad route inventory remains a candidate list, not an acceptance gate. |
| Next | Continue Table's remaining high-coverage demos: Virtual List, then drag sorting; retain the full 71-component baseline and Select/Icon targeted checks. | Complete baseline matrix and dated focused results are in this document and `dom-style-parity.md`. |

## Focused continuation — 2026-11-14

| Component | Current update | Verified state / remaining |
|---|---|---|
| Table — Virtual List | Reduced virtual-window overscan from 10 to 0 in both Table virtualization branches. | Runner: `/tmp/table-virtual-list-parity-overscan0-20261114.json`. At index 999, Blazor renders 10 rows (991–1000), React renders 10 (992–1001); both match 2,000px table width, 55px row height, 550,000px content height, and all header column widths. Root computed styles match; 254 normalized DOM differences and semantic visual-style differences remain, including Blazor spacer markup, one-row window offset, and fixed-column sticky offsets. |
| Table — Virtual List interactions | Exercised Scroll To index 999, Empty toggle on/off, and expandable switch in both pages. | Both reached `scrollTop=54600`; Empty removed all rows then restored them. Expanded mode toggle and row expand controls respond in both pages. Keep the page open for manual review. |
| Next | Align Table's virtual window/spacer structure and fixed-column offsets, then continue Table drag demos; retain Select/Icon and global/component token coverage. | The complete 71-component baseline matrix above remains a dated snapshot, not a declaration that all components are aligned. |

Docs.Server `net11.0` build succeeds with 0 errors (3,591 existing warnings).
Blazor's Table Virtual List page is open at
`http://127.0.0.1:5000/en-US/components/table#components-table-demo-virtual-list`.

## Focused continuation — 2026-11-14 (latest)

| Component | Current update | Verified state / remaining |
|---|---|---|
| Docs mobile component layout / List Vertical | Removed an extra -48px mobile offset from the component page. | At 500px the Vertical demo starts at x=48 and is 288px wide on both React and Blazor; List root/item start at x=73 and are 238px wide. Docs.Server net11 build: 0 errors. `/tmp/list-vertical-responsive-final-20261114.json` covers desktop/tablet/390px; the latest 500px comparison is recorded in `dom-style-parity.md`. |
| Select — Semantic DOM preview | Rechecked controlled Single and Multiple states. | `/tmp/select-semantic-preview-final-20261114.json`: 2/2, zero normalized DOM/CSS/geometry/visual-style differences. |
| Icon — canonical demos and responsive gallery | Rechecked icon-font, resource, basic, two-tone, custom and responsive gallery. | `/tmp/icon-dom-style-parity-final-20261114.json`: 5/5; `/tmp/icon-gallery-responsive-final-20261114.json`: 2/2. Both have zero normalized differences. Readable long-name wrapping remains intentionally different from React's clipping. |
| Table — Fixed Gapped Columns | Superseded the previous six-shadow-class discrepancy with current live and automated evidence. | `/tmp/table-fixed-gapped-columns-final-20261114.json`: 1/1, zero DOM/CSS/geometry/visual-style differences. Live comparison also matched fixed-cell classes, offsets, z-index values, and computed pseudo styles across 4,997 cells. |
| Next | Continue Table Virtual List parity by aligning the virtual window/spacer DOM while preserving matched visible geometry; then continue Table drag demos. Keep Select/Icon page checks and global/component Design Token documentation coverage open. | The full 71-component baseline matrix above remains a broad route inventory, not a completion claim. |

The React and Blazor docs pages remain open in the browser for manual review.

## Latest verification — 2026-11-14

| Component / scope | Current result | Next action |
|---|---|---|
| List — Vertical | Removed the mobile page's extra -48px offset and stopped the demo's generic overflow wrapper from clipping the vertical list action/content edge. | At 500px React and Blazor align at demo x=48/width=288px and list x=73/width=238px; vertical content is no longer clipped. The 1280/768/390px fixture remains 3/3. Docs.Server net11 build: 0 errors. |
| Table — Virtual List | Added React-like `ant-table-row-extra` clones for rowSpan, fixed merged-cell trailing margins, explicit virtual col widths, measured pixel offsets, and a header class observer that restores fixed-edge shadow classes after Blazor rerenders. | `/tmp/table-virtual-list-shadow-fixed-20261114.json`: 0/1 strict pass, 221 normalized DOM differences (down from 250); root styles and visible geometry match, semantic counts and row-span overlays match. Remaining: .NET spacer/extra row 991, holder overflow/inner transform, and merged-cell semantic ordering. Fixture: `tools/AntDesign.StyleGen/examples/table-virtual-list-dom-style-parity.json`. |
| Inventory / tokens / docs | The 71-component H/C/G matrix above is still the 2026-11-12 broad candidate snapshot. Generated token and Semantic DOM coverage remains as listed in Phase 1 coverage; runtime parity is partial. | Continue component-by-component focused checks and keep global/component token and docs gaps open; the matrix is not a completion claim. |

The current Table report and precise differences are recorded in
`dom-style-parity.md`. The Docs.Server and React docs remain available in the
browser for continued manual review.

## Focused continuation — 2026-11-14 (Icon and Select recheck)

| Component | Current update | Verified state / remaining |
|---|---|---|
| Icon — Two Tone gallery | Generated SVG paths without explicit fill now receive the caller-provided primary color; absent primary still preserves original SVG colors. | `ant-design-icons-blazor` helper tests pass 26/26; Docs.Server `net11.0` builds with 0 errors; 842-icon gallery passes desktop and narrow-screen DOM/CSS/visual comparison 2/2. Report: `/tmp/icon-gallery-grid-current-20261114-final.json`. |
| Select — Semantic DOM preview | Rechecked controlled Single and Multiple previews after rebuilding the docs site. | `/tmp/select-semantic-preview-current-20261114.json`: 2/2, zero normalized DOM/CSS/geometry/visual-style differences. Full route/demo parity is still open. |
| List — Vertical | Rechecked the user-reported layout at 500px and reran its three viewport fixture. | `/tmp/list-vertical-current-20261114.json`: 3/3, zero normalized differences. The visible sponsor overlay on Blazor was covering the top of the demo. |
| Next | Continue the highest-impact remaining Select route differences, then Table Virtual List structure and the component inventory. | The all-component status and token coverage matrix above remains the broad baseline; runtime parity and documentation coverage are still partial. |

The React and Blazor Icon and Select docs remain open in the browser for manual
review. Docs.Server is running at `http://127.0.0.1:5000`.

## Focused continuation — 2026-11-15 (List drag demos)

| Component | Current update | Verified state / remaining |
|---|---|---|
| List — four drag sorting demos | Kept the sortable preview visible while the active item animates into its drop slot; canceled drags return to their original slot. Removed the extra native HTML5 draggable demo, which does not exist in React List. | Rebuilt and restarted Docs.Server `net11.0`; all four pointer flows now show active/sibling previews, reorder correctly, and clear temporary styles after drop. Vertical geometry remains matched to React. Detailed evidence: `dom-style-parity.md`. |
| Next | Continue Table Virtual List structural parity and broaden the component/demo runtime matrix by remaining differences. | The existing 71-component baseline and Select/Icon notes remain coverage inventories, not full v6 completion claims. |

React and Blazor List documentation pages remain open in the browser.

### Current-build recheck

The shared drag helper now waits for a 1px pointer movement before activating,
uses unique IDs for each demo, and associates each handle with its own hidden
instructions node. After rebuilding and restarting Docs.Server, browser drag
input reordered the standard list, handle list, and grid; the handle grid also
reordered with Space → ArrowRight → Space. All checked states returned to
normal inline styles after drop. Handle buttons now omit custom `aria-label`
attributes to match the React demo DOM. The List page remains open in the
browser for manual review.

## Focused continuation — 2026-11-15 (Table column drag sorting)

| Component | Current update | Verified state / remaining |
|---|---|---|
| Table — Drag Column Sorting | Matched React's sortable header IDs/accessibility attributes, live active/target styling, sibling movement preview, and floating dragged-header preview. | Docs and Docs.Server `net11.0` builds: 0 errors. Browser drag Name over Age committed `Gender, Age, Name, Email, Address`, and temporary styles/overlay cleared. Header widths and height match React. Reference interaction recheck remains open. Evidence: `dom-style-parity.md`. |
| Next | Continue Table's remaining demos by mismatch volume, while carrying forward unresolved Virtual List structure and broad 71-component coverage. | Full component status and design-token coverage remain tracked in the inventory above; no full-v6 completion claim. |

## Focused continuation — 2026-11-15 (Table Virtual List)

| Component | Current update | Verified state / remaining |
|---|---|---|
| List — Vertical | Restarted Docs.Server from the current `net11.0` build and compared React/Blazor at desktop, tablet, and narrow widths. | 3/3 scenarios pass with zero normalized DOM/CSS/geometry/visual-style differences; the reported style issue was not reproducible. Latest report: `.tmp/list-vertical-restarted-report.json`; see `dom-style-parity.md`. |
| Table — Virtual List | Rebuilt/restarted the `net11.0` docs server, reloaded the browser with current framework assets, and clicked “Scroll To index 999”. | Demo has no runtime alert and reaches `scrollTop=54600`, `clientHeight=400`. Visual-style check passes 1/1 with 0 differences, while normalized DOM remains 221 differences: React rows 992–1001 with `translateY(54560px)` vs Blazor 991–1000 with a 54505px Virtualize spacer and zero transform. Full parity needs a renderer strategy matching React's window; details in `dom-style-parity.md`. |
| Table — Drag sorting | Matched dnd-kit easing, normal-row cursor, and variant-specific first-row text height. | Standard demo rows match React's text, relative positions, heights, and cursors; handler demo rows are all 77px on both sides. Blazor drag checks showed the live preview and reordered `1, 3, 2` in both modes. React/Blazor pages remain open on the standard drag demo. |
| Next | Continue the remaining Table demos and investigate a virtualization renderer that can match React's window DOM without breaking scroll behavior. | The full 71-component inventory remains a baseline, not a completion claim. |

## Focused continuation — 2026-11-15 (Table virtual API name)

| Component | Current update | Verified state / remaining |
|---|---|---|
| Table — API | Added React-compatible `Virtual`; deprecated `EnableVirtualization` as a forwarding compatibility alias. Migrated Table docs demos and added localized API table entries. | Component library, Docs, and Docs.Server `net11.0` builds pass with 0 errors; browser confirms `ant-table-virtual`, virtual holder, and no console errors. `netstandard2.1` remains blocked by existing unrelated compatibility errors (`OneOf`, `IReadOnlySet`, `EditorRequired`, `IJSObjectReference`). Virtual List DOM structure still differs from React. Evidence: `dom-style-parity.md`. |
| Next | Continue Table runtime differences, prioritizing the virtual-window renderer; then resume the next highest-difference components. | The 71-component matrix and global/component Design Token coverage remain incomplete; focused progress does not imply full v6 parity. |

## Focused continuation — 2026-11-15 (List drag demos)

| Component | Current update | Verified state / remaining |
|---|---|---|
| List — Drag sorting (4 demos) | Removed the private handle marker/class, aligned handle text and `NoFlex`, omitted empty inline style attributes, and added the React `.ant-list-container` grid class. | Docs.Server `net11.0` build succeeds with 0 errors. All 4 DOM/CSS/geometry scenarios pass (0.04px subpixel width tolerance). Pointer verification passes 4/4: active transform and stacking preview appear, reorder commits after release, and drag styles clear; no page errors. Latest report: `.tmp/list-drag-sorting-current.json`; full notes: `dom-style-parity.md`. |
| Next | Return to the broad Select/Icon route differences, then Table virtual-window structure and proceed through the remaining component/demo mismatches. | The 71-component inventory and global/component Design Token coverage remain incomplete; this focused update does not claim full v6 parity. |

## Focused continuation — 2026-11-15 (List transition and Upload Semantic DOM)

| Component | Current result | Remaining / next |
|---|---|---|
| List — four drag demos | Matched React dnd-kit sibling transition exactly (`transform 200ms`; removed the extra `ease` token and non-React `will-change`). | Rebuilt and restarted Docs.Server `net11.0`; headless browser verified live active/sibling preview and committed reorder in all four demos (4/4). Fixture remains 4/4 with zero normalized differences: `.tmp/list-drag-sorting-current.json`. |
| Upload — Basic, Default Files, File List, Drag, Semantic DOM | Registered the `_semantic` demo in generated metadata, added React's semantic marker classes, and aligned the four fixture demo structures and data. | Focused fixture passes 5/5 DOM/CSS/geometry/visual-style scenarios; report `/tmp/upload-five-demos-final-20261115.json`. This covers 5 of 22 React Upload demos; the remaining route is not yet audited. |
| Upload — refreshed full-route inventory | React 22 demos, Blazor 25, 22 names matched, no React-only demos, 3 Blazor-only (`input-file`, `file-type`, `component-token`). | Broad initial-state scan: H/C/G differences in 19/17/17 matched demos, with one blocked page. The scanner is a candidate map; focused five-demo fixture passes, and the remaining demos need scoped checks. Report: `/tmp/upload-inventory-latest-20261115.json`. |
| All 71 components | The full matrix at the top remains the dated broad scan; current focused changes are logged in this section and below. | Select/Icon focused differences, Table virtual-window DOM, full component APIs/tokens/docs, Theme Preview, and Semantic DOM audits remain incomplete. |
| Next | Triage Upload's highest remaining scoped differences, then resume the Select/Icon documentation-page checks and broad component mismatch queue. | Do not treat the dated H/C/G matrix as current post-change runtime evidence. |

## Focused continuation — 2026-11-15 (fresh Table virtual-list capture)

| Component | Current progress | Next verification |
|---|---|---|
| Table — Virtual List | Scoped `patch.less` fixes the docs-reset-induced `borderBottomStyle` differences; fresh DOM/CSS fixture now reports 221 DOM differences and 0 visual-style differences. Blazor begins at row 991 after a 54505px framework spacer; React begins at row 992 with a 54560px translated window. | Align the virtual window and verify row interactions. Report: `.tmp/table-virtual-list-border-fix.json`. |
| List — drag sorting | Four drag/grid variants remain passing: normalized DOM/CSS/geometry 4/4 and live pointer preview/reorder/cleanup 4/4. | No remaining issue from this focused request. |
| All components | The 71-component baseline matrix above remains the all-component progress index. Select/Icon, Design Token docs, Theme Preview, API parity, and Semantic DOM remain tracked as partial until their full scoped audits pass. | Continue by mismatch volume after the Table virtual-window pass. |

## Focused continuation — 2026-11-15 (List drag pointer follow-up)

| Component | Current update | Verified state / remaining |
|---|---|---|
| List — four drag demos | Pointerdown now uses capture; standard rows also start dragging from the full row area; text selection is suppressed only during an active drag. | Headless browser pointer checks show live active/sibling previews and committed reorder in all four demos (4/4). DOM/CSS/geometry fixture: 4/4, no differences. Docs.Server net11 build: 0 errors. Latest report: `.tmp/list-drag-sorting-after-fix.json`. |
| Next | Continue the current high-difference Table and remaining component/demo work. | The 71-component runtime inventory and full design-token/documentation sync remain incomplete. |

## Focused continuation — 2026-11-15 (Upload Basic and Drag)

| Component | Current progress | Verification / next |
|---|---|---|
| Upload — Basic and Drag | Aligned trigger semantics, hidden input structure, demo text, endpoint/name and status feedback; restored Enter-key activation and file-drop handling for Drag. | React/Blazor fixture passes 2/2 with zero normalized DOM, computed CSS, geometry, or visual-style differences. Keyboard and drop interactions both pass. `/tmp/upload-demo-pair-parity-final.json`; Docs.Server `net11.0` build has 0 errors. The other Upload demos/tokens/API details still need full-page coverage. |
| All 71 components | The complete component/demo mismatch inventory remains the baseline above; current focused updates are recorded below it. Select and Icon retain their focused wins plus unresolved broader route/token audits; Table still has virtual-list structural differences. | Treat zeroes in the old inventory as scan results only; global/component token docs, Theme Preview, Semantic DOM, and full demo coverage remain incomplete. |
| Next | Continue Upload's remaining high-difference demos and component-token/Semantic DOM coverage, then return to the broader Table and component inventory. | The current Upload fixture covers only Basic and Drag, not the full 22-demo React page. |

## Focused continuation — 2026-11-15 (Upload text list)

| Component | Current progress | Verification / next |
|---|---|---|
| Upload — Basic, Default Files, File List, Drag | Synced the Basic/Drag trigger, implemented React-shaped file-list items and states, fixed the progress rail's empty color serialization, and aligned File List sample content. | Focused four-demo fixture passes 4/4 with zero normalized DOM/CSS/visual differences; 0.02px allows only 1/64px width serialization. Docs.Server `net11.0` build: 0 errors. Keyboard/drag-drop interaction verified. Full route still has many uncovered DOM/style gaps and React hydration #418. |
| All 71 components | Full component/demo inventory remains the baseline table at the beginning of this document; this focused update changes Upload only. | Select/Icon focused checks remain green but whole-page/token audits are incomplete. Table Virtual List still differs structurally. Global/component tokens, Theme Preview, docs and Semantic DOM remain in scope. |
| Next | Continue Upload's remaining mismatched demos, then broaden to the next highest component mismatch. | Re-run page inventory after the current Upload edits and prioritize its largest remaining runtime gaps. |

## Focused continuation — 2026-11-15 (documentation top spacing)

| Area | Current update | Verification / next |
|---|---|---|
| Documentation shell / Select Semantic DOM (initial spacing check) | Initially removed `.main-wrapper` top padding based on an incomplete screenshot interpretation; later React viewport measurements showed a 32px normal-flow gap. The correction is recorded in the latest docs-spacing entry below. | Select `_semantic` fixture passes 2/2 DOM/CSS/geometry/visual checks: `/tmp/select-semantic-recheck-final-20261115.json`. |
| All 71 components | The full matrix above remains the all-component progress index. Select/Icon focused checks, Table virtual-window DOM, all component APIs/tokens/docs, Theme Preview, and Semantic DOM audits remain incomplete. | Continue the 71-component mismatch queue and retain global/component Design Token coverage as open work. |
| Next | Continue with Table virtual-window structure, then return to whole-route Select/Icon and the remaining component mismatch queue. | See the full component matrix above and dated focused records in `dom-style-parity.md`. |

## Focused continuation — 2026-11-15 (nested documentation navigation)

| Area | Current progress | Verification / next |
|---|---|---|
| Mobile header — Theme menu | Replaced the disabled flat Theme label and peer theme items with a real inline `SubMenu`; its seven theme actions are now nested children. | Docs.Server `net11.0` build succeeds with 0 errors. At 550px, browser inspection confirms `.ant-menu-submenu-inline`, seven child items under the submenu `<ul>`, and 24px parent / 48px child indentation; no page errors. Page remains open at `http://127.0.0.1:5011/en-US/components/select`. |
| Table — Virtual List | Added a client-rendered translated window and native scroll-window callbacks, and rebuilt the JS interop bundle so the new callbacks are available at runtime. | Focused browser fixture now mounts without the previous missing-JS-function error, but does not pass: `/tmp/table-virtual-current-report.json` reports 166 normalized DOM differences after scrolling to index 999. The inner transform and rendered row window still remain at the initial state while `scrollTop` changes, so the window update path needs further debugging. |
| All 71 components | The component matrix above is still the inventory index, not a completion claim. | APIs, global/component Design Tokens, all demos, Theme Preview, Semantic DOM, and runtime DOM/CSS/visual checks remain incomplete. |
| Next | Fix Table virtual-window rerendering and rerun the focused fixture; then continue the highest-difference component queue. | Keep this active until focused interactions and the broader inventory pass. |
## Focused continuation — 2026-11-15 (Upload PictureCircle)

| Component | Current progress | Verification / next |
|---|---|---|
| Upload — PictureCircle | Synced the picture-circle file row DOM, hidden default download action, preview links and callback flow, `file` input name, and upload-button inline style with React. | Focused fixture passes 1/1 with zero DOM/CSS/geometry/visual-style differences. Docs.Server `net11.0` build: 0 errors. Report: `/tmp/upload-picture-circle-final-20261115.json`. File-selection/upload and image-preview interactions remain to be exercised. |
| All 71 components | See the full runtime mismatch matrix above; this focused fix covers one Upload demo only. | Full Upload demo/token/API coverage, Select/Icon page-level work, Table virtual-list structure, shared design tokens and docs remain incomplete. |
| Next | Continue with Upload's next largest scoped mismatch, then resume the broad component/demo queue. | Details and prior verification records are in `dom-style-parity.md`. |

## Focused continuation — 2026-11-15 (Upload PictureCard)

| Component | Current progress | Verification / next |
|---|---|---|
| Upload — PictureCard and PictureCircle | Matched PictureCard demo structure and default actions, replaced its legacy Modal with React-style Image preview, and fixed dynamic Image preview initialization in both picture demos. | PictureCard fixture 1/1, PictureCircle regression 1/1, existing Upload baseline fixture 5/5. All report zero normalized DOM/CSS/geometry/visual-style differences. Browser opened and closed the preview in both demos. Docs.Server `net11.0` build: 0 errors. Reports linked in `dom-style-parity.md`. |
| Upload — remaining demos | The broad scan still lists DOM/CSS/geometry candidate mismatches in Directory, Preview File, custom actions, Crop, Style Class, Avatar, Paste, sorting, token and other demos. | Re-scan after the PictureCard/PictureCircle fixes and continue the highest-impact remaining scoped demo. Full Upload is still open. |
| All 71 components | The complete 71-route snapshot table above remains the component progress index; this update changes two Upload demos only. | Select/Icon page-level work, Table virtual-window structure, component API/token/docs/Semantic DOM and Theme Preview parity remain incomplete. |
| Next | Refresh Upload's route inventory, then continue its highest remaining mismatch before returning to the 71-component queue. | The matrix is a broad candidate scan; use focused demo fixtures and interaction checks to close each mismatch. |

## Focused continuation — 2026-11-15 (docs top gap and Upload StyleClass)

| Area | Current progress | Verification / next |
|---|---|---|
| Documentation shell / brand | Matched Microsoft's `#512BD4` → `#D600AA` gradient in header and home title; removed the happy-work white text shadow that washed out the homepage gradient. The wrapper spacing was subsequently corrected to React's 32px. | Computed styles and fresh homepage screenshot verified. Docs.Server `net11.0` build: 0 errors. |
| Upload — StyleClass and PictureStyle | Replaced the semantic placeholder with React's two StyleClass Uploads and reconstructed PictureStyle's one picture-list demo plus direct status-specific row markup. | Both focused fixtures pass 1/1 with zero normalized DOM/CSS/geometry/visual differences: `/tmp/upload-style-class-parity-20261115.json`, `/tmp/upload-picture-style-parity-20261115.json`. |
| Upload — refreshed route inventory | React 22 demos, Blazor 25, 22 matched, no React demos missing. | Candidate DOM/CSS/geometry differences: 19/14/14; React route logs hydration error #418, so use focused fixtures for acceptance. Report: `/tmp/upload-inventory-current-20261115.json`. |
| All 71 components | The full matrix above remains the broad component/demo mismatch index. | Runtime, API, global/component tokens, docs, Theme Preview, and Semantic DOM parity remain incomplete. |
| Next | Continue Upload's next highest scoped gap, then proceed through the other components in descending mismatch volume. | Full progress matrix and focused records are in this file and `dom-style-parity.md`. |

## Focused continuation — 2026-11-15 (Upload Avatar)

| Area | Current progress | Verification / next |
|---|---|---|
| Upload — Avatar | Matched React's paired picture-card and picture-circle avatar uploaders, shared validation/loading/image state, upload endpoint, wrapper class, and trigger markup. | Focused React/Blazor fixture passes 1/1 with zero normalized DOM, computed CSS, geometry, and visual-style differences: `/tmp/upload-avatar-parity-20261115.json`. Browser screenshot confirms both wrappers render at 102px with a 16px gap. Docs.Server `net11.0` build succeeds with 0 errors. |
| Upload — refreshed route inventory | React 22 demos, Blazor 25, 22 matched, no React demo names missing; Blazor-only: `input-file`, `file-type`, `component-token`. | Candidate H/C/G differences are 19/12/12. One React page capture remains blocked by hydration error #418; focused fixtures are the acceptance evidence. Report: `/tmp/upload-inventory-avatar-20261115.json`. |
| All 71 components | The table above remains the full component status index; Upload's CSS and geometry candidate counts were refreshed. | Component API, Design Token, docs, Theme Preview, Semantic DOM, and runtime parity are not complete across all components. |
| Next | Continue with the highest remaining mismatch volume, beginning with Table's remaining demo and runtime differences. | Use focused per-demo fixtures and update this matrix after each component batch. |

## Focused continuation — 2026-11-15 (React docs top spacing correction)

| Area | Current progress | Verification / next |
|---|---|---|
| Documentation shell / top spacing | Corrected the earlier zero-gap change. React uses a 64px sticky header and places its main content at y=96px (32px below the header); the main content scrolls normally, so the gap leaves the viewport with it. Blazor `.main-wrapper` now has 32px top padding, moving the sidebar and article together. | Both browser measurements match: content starts at y=96px and moves to -24px at scrollY=120; sticky header remains at y=0..64px. |
| Table — responsive column API | Added React-style `Responsive` breakpoints to Table columns, including selection/action columns, and rewrote the responsive demo around a single row and declarative column breakpoints. | Docs.Server `net11.0` build succeeds. Desktop 1280px, medium 800px, and narrow 600px browser comparison passes 3/3 with zero normalized DOM/CSS/geometry/visual-style differences. `/tmp/table-responsive-columns-20261115e.json`. |

## Focused continuation — 2026-11-15 (Table virtual window)

| Area | Current progress | Verification / next |
|---|---|---|
| Table — Virtual List | Preserved the client virtual window across same-count data refreshes; matched hidden `rowspan/colspan` attributes, React `SpaceItem` action wrappers, and native-style virtual scrollbar DOM/metrics. | Final focused fixture `/tmp/table-virtual-final-report.json` passes 1/1 with zero normalized DOM, computed CSS, geometry, and visual-style differences. Scroll-to-999 mounts rows 992–1001 at `translateY(54560px)`; Empty restore returns to row 0. Dragging the vertical scrollbar changes `scrollTop` 54600→141379 and updates the visible window to row 2570. |
| All 71 components | The full component matrix above remains the source of truth; Select/Icon page-level differences, runtime details, APIs, global/component Design Tokens, docs, Theme Preview, and Semantic DOM still need full audit and alignment. | Do not count a visual match as full DOM parity; per-demo strict DOM/CSS reports remain required. |
| Table — virtual window render callback and scrollbars | Same-count data refreshes preserve the active range; one-render request bypasses the general render gate. Added vertical/horizontal virtual scrollbar nodes with React-compatible dimensions, scroll position, visibility timeout, and pointer dragging. | Component and Docs.Server `net11.0` builds succeed with 0 errors; JS interop bundle rebuilt. Final focused fixture passes 1/1 with 0 DOM/CSS/geometry/visual differences. Browser checks cover scroll-to-999, empty/restore, and thumb dragging. |
| Next | Continue with the next highest remaining component/demo mismatch volume after Table's virtual list focused case is closed. | Browser remains open at `http://127.0.0.1:5011/en-US/components/table#components-table-demo-virtual-list`; full all-71 component matrix above remains the coverage index. |

## Focused continuation — 2026-11-15 (Table Nested Table)

| Component / scope | Progress | Verification |
|---|---|---|
| Table — `DefaultExpandedRowKeys` | Added the React-compatible initial expanded row-key parameter and documented it in English and Chinese Table API docs. The Table row cache now retains expanded-row content after its first expansion, matching React's collapse behavior. | Component and Docs.Server `net11.0` builds succeed with 0 errors. |
| Table — Nested Table demo | Replaced the single lazy-loaded table with React's three outer tables (default/medium/small), three nested rows per table, and the first outer row expanded by default. Matched nested row keys and used `Unbound` to render the dropdown anchor without a wrapper. | `tools/AntDesign.StyleGen/examples/table-nested-table-dom-style-parity.json` (verified via `/tmp/table-nested-table-current-report5-20261115.json`): initial, collapse, and reopen pass 3/3 with zero normalized DOM/CSS/geometry/visual-style differences. |
| Table — Tree Data | Rechecked the existing React-matched sample after the shared row expansion change. | `/tmp/table-tree-data-current-report-20261115.json`: 3/3 states pass, zero normalized DOM/CSS/geometry/visual-style differences. |
| Docs navigation — Theme submenu | Bound the navigation menu's open keys so Theme operates as a collapsible second-level submenu and starts closed. | Docs `net11.0` build passes; the browser is left on Table Nested Table for review. |
| Next | Continue the Table demo queue with the next unverified React example, then refresh the broad component scan and return to the highest remaining mismatch volume. | All 71 component rows at the top of this file are still the dated broad baseline; focused results update coverage only where noted. API, token, docs, Theme Preview, and Semantic DOM audits remain in scope. |

## Focused continuation — 2026-11-16 (Select and Icon)

| Component | Current progress | Remaining scope |
|---|---|---|
| Select | 28/28 React demo names were present in the latest route inventory. Focused interaction fixture passes 4/4 and controlled Semantic DOM Single/Multiple previews pass 2/2 with zero normalized DOM/CSS/geometry/visual-style differences. Core size/variant/placement fixture previously passed 6/6. | Full page API/token documentation and the remaining demo-by-demo audit are open. The broad React page scanner still reports hydration error #418; focused fixtures are the acceptance gate. |
| Icon | Five canonical demos pass 5/5; responsive toolbar passes 2/2. The 842-icon grid has matching normalized DOM and desktop cell geometry. | Per user's no-clipped-name requirement, Blazor wraps labels and changes to a readable two-column grid at 500px. React clips names in its fixed six-column gallery; these label/narrow-grid style differences are intentional and remain visible in the strict report. Whole-page docs/API/token audit is open. |
| Other 69 rows | See the 71-component table at the beginning of this document. Rows not specifically updated in dated entries remain the earlier broad-scan baseline, not newly verified statuses. | Runtime demo coverage, API, global/component Design Tokens, docs, Theme Preview, and Semantic DOM remain open according to each component's row and focused evidence. |
| Next | Continue Select's remaining route/API/token coverage, then Icon's docs/token audit. | Focused evidence: `/tmp/select-interaction-current-report-20261116.json`, `/tmp/select-semantic-preview-current-report-20261115.json`, `/tmp/icon-gallery-grid-current-report-20261116c.json`; the broad matrix is an index, not an acceptance result. |

Docs.Server `net11.0` builds with 0 errors (591 existing warnings). It is
running on port 5011, and the visible browser is left at the Icon gallery for
manual review.

## Focused continuation — 2026-11-16 (Select docs and mobile Theme hierarchy)

| Component / area | Current progress | Verification / next |
|---|---|---|
| Select — docs structure | Reordered page sections to React v6's API → Semantic DOM → Design Token → FAQ sequence, added the Semantic DOM TOC anchor, and added translated FAQ content covering tags search, popup state, custom popup mouse events, virtual row height, accessibility, and custom tag close behavior. Both generated metadata inputs now contain the FAQ HTML. | Docs and Docs.Server `net11.0` builds succeed with 0 errors; the Select page DOM exposes the Semantic DOM and Design Token anchors and all six FAQ entries. Full API parity and demo-by-demo audit remain open. |
| Docs navigation — mobile utility hierarchy | Mobile navigation groups utility actions under “More”, with Theme as a second-level submenu and theme options nested below it. Desktop keeps its separate theme control. | Build succeeds. The browser is left at Select on a narrow viewport, but the header did not enter its mobile rendering during this pass; menu interaction still needs a viewport/breakpoint runtime check. |
| All 71 components | The full component table at the beginning remains the all-component progress index. This turn adds Select docs coverage and header navigation structure only. | Runtime demos, APIs, global/component Design Tokens, docs, Theme Preview, and Semantic DOM remain incomplete for components without focused closure evidence. |
| Next | Continue Select's API and remaining route/demo audit, then complete the Icon docs/token audit and proceed down the component mismatch matrix. | Browser remains at `http://127.0.0.1:5011/en-US/components/select?theme=light` for manual review. |

## Focused continuation — 2026-11-16 (Select docs heading and token check)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Select — Design Token and docs | Compared React `components/select/style/token.ts` to the generated C# token catalog and live documentation. The Select page renders all 24 documented React component tokens and its React-used global-token table; generated metadata parity reports zero missing names, descriptions, defaults, CSS variables, and global-token usage rows. | `docs/antd-v6/phase1-progress.json`; browser DOM on port 5011. Full Select API inventory and remaining demos still need audit. |
| Select — FAQ heading | Changed generated English FAQ section title from “FAQs” to React's “FAQ”; Chinese heading remains localized. | Docs.Server `net11.0` build succeeds with 0 errors (4,191 existing warnings); restarted server and verified rendered HTML and browser accessibility tree show “FAQ”. |
| All 71 components | Full component/demo status remains indexed in the 71-row baseline table at the beginning of this file. Only component/area rows with focused dated evidence should be treated as refreshed; untouched H/C/G counts are stale scan candidates. | Token metadata is mechanically covered for 65 pages, but runtime Theme Preview and complete per-component API/docs/Semantic DOM audits remain open. |
| Next | Continue Select API reconciliation against React's public API, then finish Select demo coverage and the Icon docs/token audit. After those focused pages, return to the highest remaining runtime mismatch volume (Table). | Keep the current Docs.Server on port 5011 and the Select page open for manual review. |

The documentation server is running at `http://127.0.0.1:5011/en-US/components/select?theme=light`; the browser tab remains open for manual review. `git diff --check` passes.

## Focused continuation — 2026-11-16 (Select API deprecation and docs preview)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Select — `Bordered` API | React v6 marks `bordered` deprecated in favor of `variant`. Blazor now marks `Bordered` obsolete while retaining compatibility; English/Chinese API docs mark it deprecated, and the old Bordered demo now uses `Variant="InputVariant.Borderless"`. | Docs.Server `net11.0` build succeeds with 0 errors. Generated API tables and the live Select page show the Obsolete marker. Reconcile the API inventory and continue the remaining Select API/demo audit. |
| Docs navigation — mobile Theme hierarchy | Theme remains a submenu inside More. Removed a forced child-item indent that flattened the visual hierarchy; Menu's native level indentation now distinguishes Theme from its options. | Docs.Server `net11.0` build succeeds. Interactive narrow-screen submenu state remains unverified because the browser's mobile viewport did not render the mobile navigation component. |
| Preview runtime | Select route returns the full docs page and remains open in the browser for manual review. | Home route currently fails because the development server requests `/_content/AntDesign.Docs/data/recommend.en-US.json` and gets 404; the docs data exists under the package `wwwroot/data` but is not served by this launch configuration. This does not block the Select route. |
| All 71 components | The complete inventory remains the 71-row matrix at the beginning of this file. Focused status is refreshed only for areas called out in dated entries; the broad unmatched demo/API/runtime candidates remain incomplete. | First-phase global/component tokens, APIs, demo HTML/CSS, docs pages, Theme Preview, and Semantic DOM have not all passed end-to-end verification. |
| Next | Continue Select's React v6 API and demo reconciliation, then close Icon's full docs/token audit and resume the highest-volume rows in the 71-component matrix. | Keep the Select docs page open at `http://127.0.0.1:5011/en-US/components/select?theme=light` for hand review. |

## Focused continuation — 2026-11-16 (Slider Basic structure)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Slider — Basic | Matched React's default value (30), removed a Blazor-only marks slider, removed the tracks wrapper for ordinary single/range values, omitted the empty marks node, added handle orientation ARIA, normalized percentage styles, and moved root text selection prevention to component CSS. | `/tmp/slider-basic-dom-style-parity-20261116-final.json` passes 1/1: normalized DOM tree, selected computed CSS, geometry and visual styles match. Raw HTML serialization differs by attribute order and Blazor renderer markers. Docs.Server `net11.0` build succeeds with 0 errors; existing repository warnings remain. The other 14 Slider demos, APIs, component tokens, docs and Semantic DOM remain unaudited. |
| All 71 components | The full 71-row matrix at the beginning remains the component progress index; H/C/G counts are broad-scan candidates and untouched rows are not current acceptance evidence. | Continue Select API/route closure first, then Icon docs/token audit, then the remaining component mismatch queue. Global tokens, Theme Preview, and full API/docs/Semantic DOM coverage remain open. |
| Next | Resume Select API reconciliation and focused route differences, then complete the Icon docs/token audit. | Docs.Server remains running at port 5011; Select docs tab remains open for manual review. |

## Focused continuation — 2026-11-16 (Select DefaultOpen)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Select — `DefaultOpen` | Added the React v6 `defaultOpen` equivalent as `DefaultOpen`, initializing the popup only when `Open` is uncontrolled; documented the API in both locales and added a focused demo. Refreshed compiled demo metadata so the new demo appears in the live route/sidebar. | Docs.Server `net11.0` build succeeds (0 errors, 591 existing warnings). Browser verified initial popup visible and outside click closes it. This is an API behavior check, not a React DOM parity fixture because React v6 has no corresponding `defaultOpen` demo. |
| Select — remaining audit | React route inventory remains 28/28 matched; Blazor now has 52 demos (24 Blazor-only names, including this API-focused example). | Complete the remaining public API/interaction inventory and focused demo diffs; do not treat the old broad H/C/G row as freshly rescanned. |
| Icon | Five canonical demos and responsive toolbar have prior focused evidence; 842-icon gallery DOM/desktop geometry match with documented intentional text-wrap/narrow-grid divergence. | Complete whole-page API/docs/Design Token audit. |
| Other 69 components | See the 71-component status matrix at the start of this file; untouched scan counts are dated candidate data rather than acceptance evidence. | Runtime DOM/CSS, API, component/global tokens, docs, Theme Preview, and Semantic DOM remain in scope. |
| Next | Continue Select API reconciliation, then close Icon's docs/token audit and resume the highest-volume component/demo gaps. | User-visible Docs.Server remains on port 5011 with the Select Default Open example open for manual review. |

## Focused continuation — 2026-11-16 (Select OptionRender API)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Select — `OptionRender` | Added the React-compatible `OptionRender` parameter as the canonical public name for dropdown option templates while retaining `ItemTemplate` as a supported alias. Updated the matching demo and both locale descriptions. | Docs.Server `net11.0` build succeeds with 0 errors. React/Blazor open-popup fixture: normalized DOM tree, computed CSS, geometry, SVG use, and visual styles match. Raw HTML exact comparison still differs due Razor whitespace. `/tmp/select-option-render-option-api-20261116-final.json`. |
| Select — API/demo status | React has 28 demos; Blazor has 52 and matches all 28 React names. The DefaultOpen API demo is Blazor-only. | Remaining API and interaction reconciliation is open; broad H/C/G counts are not rescanned. |
| Icon | Canonical demos and responsive toolbar have focused evidence; complete docs/API/Design Token audit remains open. | Continue after Select API closure. |
| Other 69 components | Full 71-component index is the table at the beginning of this file. Untouched rows are dated broad-scan candidates, not acceptance results. | Runtime DOM/CSS, API, global/component tokens, docs, Theme Preview, and Semantic DOM still need coverage. |
| Next | Continue Select's remaining public API and popup interaction audit, then complete Icon docs/tokens and resume the highest-volume component gaps. | Browser remains open on the Blazor OptionRender demo at port 5011. |

## Focused continuation — 2026-11-16 (Select callbacks and Theme hierarchy)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Select — callbacks | Added `OnSelect` and `OnDeselect` with the selected option item as the callback payload, plus React-compatible `OnClear`; legacy `OnClearSelected` remains available. Updated both locale API tables. | Docs.Server `net11.0` build succeeds with 0 errors. The served English Markdown contains all three new API rows. Callback interaction scenarios remain unverified. |
| Docs navigation — mobile Theme | Rendered the Theme submenu explicitly inside the mobile More submenu; `DocsThemeSwitch Inline` now renders only the theme option items. This keeps the hierarchy explicit as More → Theme → theme options instead of flattening options into the More list. | Docs.Server `net11.0` build succeeds with 0 errors (591 existing warnings). At 390×844, browser DOM confirms Theme owns all seven options and both More and Theme lists start collapsed. The menu trigger itself did not open reliably in this browser session, so click-through behavior still needs a clean interactive pass. |
| Select | React/Blazor demo inventory is 28/53/28 matched; Events is a Blazor-only API exercise. Focused OptionRender normalized DOM/CSS/geometry/visual styles passed; raw HTML whitespace differs. | Continue the remaining public API and demo audits, then finish Icon docs/tokens. |
| Icon | Canonical demos and responsive toolbar have prior focused evidence; gallery labels intentionally wrap in Blazor to avoid clipping. | Full docs/API/Design Token audit remains open. |
| Other 69 components | Full 71-component matrix at the beginning is the all-component progress index; untouched rows remain historical broad-scan candidates. | Runtime demo parity, APIs, global/component Design Tokens, docs, Theme Preview, and Semantic DOM remain in scope. |
| Next | Continue Select's API/interaction inventory, then finish the Icon docs/token audit and return to the highest-volume remaining component mismatches. | The 71-row matrix at the beginning remains the all-component index. The browser is left on the Select Events demo at Docs.Server port 5011 for manual review. |

## Focused continuation — 2026-11-16 (Design Token value previews)

| Area | Current progress | Verification / remaining |
|---|---|---|
| Design Token tables | Added a shared value renderer to component and global token tables. CSS color values display a colored swatch, including component names such as `defaultActiveBg`; shadow tokens stay plain. `motionEase*` cubic-bezier values display a grid, easing curve, correctly colored control handles and the source value. Other token values retain plain code rendering. | Docs.Server `net11.0` builds with 0 errors (591 existing warnings). Browser on Button Design Token confirms 47 color swatches and 1 cubic-bezier preview; computed SVG curve stroke is Ant primary blue, green/red handles match the React reference positions, and the preview is 160px square. This is a display-level cross-page enhancement; token data coverage and value parity remain tracked separately. |
| All 71 components | No demo-count or runtime parity status changed. | The matrix at the beginning remains the broad candidate index; continue focused runtime, API, docs, Semantic DOM and Theme Preview audits. |
| Next | Continue remaining Select API/demo reconciliation, then complete Icon docs/token audit and resume highest-volume component gaps. | Docs.Server is running on port 5011 and the browser is left at `http://127.0.0.1:5011/en-US/components/button#design-token`. |

## Focused continuation — 2026-11-16 (Icon docs structure and API descriptions)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Icon — docs page | Moved the Icon Gallery out of the Examples demo list and rendered it as a standalone “List of icons” section before Examples, matching React's section hierarchy. The page TOC links to this section independently. | React and Blazor browser heading order matches for How to use → List of icons → Examples → API → Design Token. Blazor gallery renders its search control, six icon categories and 842 icon entries. The five canonical React demo names remain present; the additional dynamic-color demo is Blazor-specific. |
| Icon — API docs | Corrected the XML summaries for `Role` and `Spin`, added their Chinese translations, regenerated English/Chinese component metadata, and included `OnKeyDown` in the hand-maintained API reference. | Live Blazor API tables show the corrected summaries and `OnKeyDown`; Docs.Server `net11.0` builds with 0 errors (591 existing warnings). Full Icon API semantics, tokens, FAQ and individual demo parity remain open. |
| All 71 components | The broad component/demo index at the beginning remains the complete progress matrix; this round changes Icon documentation only. | Other components retain their dated scan status and require focused verification for runtime DOM/CSS, API, token, documentation, Theme Preview and Semantic DOM. |
| Next | Continue Select's remaining API/demo audit, then complete the remaining Icon docs/token review and return to the highest-volume component mismatch queue. | Docs.Server is running at port 5011. The browser is left on `http://127.0.0.1:5011/en-US/components/icon`; React reference remains open in the adjacent tab at port 8000. |

## Focused continuation — 2026-11-16 (Select React-named icon and list props)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Select API | Added React-named `ListHeight`, `Prefix`, `RemoveIcon`, and `MenuItemSelectedIcon`; existing Blazor names remain usable. The popup-height alias controls the live list holder, and custom fragments replace the prefix, tag remove glyph and selected-option check glyph. Documented all four in English and Chinese and added the `api-aliases` demo. | Docs.Server `net11.0` builds with 0 errors (591 existing warnings). Generated metadata includes the demo in both locales. Browser confirms 128px list max-height, prefix, two custom remove glyphs and custom check glyphs; removing a selected tag works. Console has no errors. The focused browser demo checks the Blazor API behavior; it does not claim full React demo DOM parity. |
| Select inventory | 28/28 React demo names remain matched; Blazor now has 54 demos, including 26 Blazor-only docs/API exercises. The broad H/C/G row remains an older scan and was not refreshed by this API change. | Re-run the focused React/Blazor API and demo inventory after the remaining exact-name props are addressed. |
| Icon | Standalone gallery structure and API summaries remain verified in the previous entry. | Complete full API, token, FAQ and individual demo audit. |
| Other 69 components | The 71-component matrix at the beginning remains the all-component progress index; no other route changed this round. | Continue focused DOM/CSS, API, Design Token, docs, Semantic DOM and Theme Preview validation. |
| Next | Continue Select's remaining exact React API gaps (`fieldNames`, `optionFilterProp`, controlled `searchValue`), then finish Icon docs/tokens before selecting the next component by remaining React demo volume. | Docs.Server is running in Development on port 5011. Blazor Select is open at the new API demo and React Select remains open in the adjacent browser tab. |

## Focused continuation — 2026-11-16 (Select filtering and selected-label API)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Select — filtering | Added `FilterOption` and `FilterSort` API parameters and documentation. `FilterOption=false` keeps the options visible while typing; `FilterSort` receives the two option records and current search text. | Browser comparison on Search and sort: typing `i` yields the same ordered labels in React and Blazor (`Communicated`, `Identified`, `Not Identified`). `FilterOption=false` remains visible with a nonmatching query. This verifies the focused interactions, not the entire Select demo surface. |
| Select — selected label | Added `LabelRender` as the React-named alias for `LabelTemplate`, switched the existing Custom Label Render demo to the new name, updated English/Chinese API and demo copy, and regenerated component metadata with the CLI. | Docs.Server `net11.0` builds with 0 errors (591 warnings). Browser verifies the selected unmatched value renders “No option match”; Chinese API table shows the translated `LabelRender` summary. React Select remains open in the adjacent tab. |
| Select inventory | Remains 28/28 React demo names matched and 54 Blazor demos; no demo was added in this round. | The broad H/C/G scan remains historical and was not rerun. Continue controlled search value and option field mapping, then refresh route-level checks. |
| Other 70 components | The 71-component progress matrix at the beginning remains the component-by-component index; no other component changed in this round. | Their API, Design Token, docs, Semantic DOM, Theme Preview, and demo parity still require focused completion. |
| Next | Implement React `fieldNames`, `optionFilterProp`, and controlled `searchValue` semantics; then finish the remaining Select route audit and return to Icon documentation/token gaps. | Docs.Server runs on port 5011; Blazor Select is open on Custom Label Render, and React Select remains in the adjacent browser tab. |

## Focused continuation — 2026-11-16 (Select `showSearch` configuration)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Select — search configuration | `ShowSearch` now accepts `bool?` or a configuration object. The object supports `OptionFilterProp` as one or multiple option fields and an optional `FilterOption` callback toggle. Multiple fields match with OR semantics. The old top-level `OptionFilterProp` remains as an obsolete compatibility alias; the multi-field demo now follows React v6's `showSearch={{ optionFilterProp: [...] }}` shape. English/Chinese API and demo descriptions and generated metadata are refreshed. | Docs.Server `net11.0` build succeeds (0 errors; 591 existing warnings). The browser comparison uses `c11` and both pages return the `a11` option. Focused DOM/style fixture passes 1/1 with normalized DOM, CSS, geometry, and visual checks; strict serialized HTML differs only in Razor whitespace/attribute serialization. This is a focused case, not full Select route parity. |
| Select inventory | Still 28/28 React demo names represented across 54 Blazor demos. | Broad H/C/G inventory remains historical and was not rerun. Controlled `SearchValue`, `fieldNames`, and the remaining Select route/API review are open. |
| Icon and other components | No additional route status changed this pass. The 71-component table at the beginning remains the all-component progress index. | Icon's full API/token/FAQ/demo audit and the other 69 routes' runtime DOM/CSS, API, Design Token, docs, Semantic DOM, and Theme Preview work remain open. |
| Next | Continue Select with controlled `SearchValue`, then close the remaining Select route/API audit and resume Icon docs/tokens. | Docs.Server is running on port 5011; leave the Blazor and React Select pages open for comparison. |

## Focused continuation — 2026-11-17 (Responsive header overflow)

| Area | Current progress | Verification / remaining |
|---|---|---|
| Header navigation — width-driven overflow | Preserved React-style responsive overflow. The desktop Menu receives the complete ordered nav/utility item list and is allowed to shrink in the flex row; its own overflow observer moves trailing entries into the built-in More submenu. | Live Docs.Server browser at 820px: menu width 355px; Docs, Components, Charts remain inline; Experimental, AI, Github, 简体中文, RTL are `aria-hidden` in the inline list and appear in the opened More popup. The tab remains at 820px with More open for manual review. Details in `dom-style-parity.md`. |
| All 71 component routes | The complete 71-row H/C/G and demo-name inventory is the table above. It is a dated candidate scan; focused follow-ups are recorded separately, and rows without fresh focused evidence are not considered closed. | Runtime demos, APIs, global/component tokens, docs, Theme Preview, and Semantic DOM remain active scope. |
| Next | Continue from remaining Table/Select focused gaps, then refresh the component queue by current mismatch volume. | The Blazor Table page remains open at `http://127.0.0.1:5000/en-US/components/table?theme=light#components-table-demo-drag-sorting`. |

## Focused continuation — 2026-11-17 (Header menu responsive styling)

| Area | Current progress | Verification / remaining |
|---|---|---|
| Header menu styles | Matched the React desktop grid sizing and corrected the 1120px responsive rule order. Below the breakpoint, utilities hide and the bounded Menu retains width-driven overflow into its built-in More submenu. | `Docs.Server` net11 build succeeds (0 errors, 591 existing warnings). Browser at 820px: header elements remain in viewport, search width is 220px, utility row is hidden; Docs/Components/Charts stay inline and More opens with Experimental/AI/Github/简体中文/RTL. Desktop at 1512px keeps the menu and right utility controls separated without duplicate text links. |
| All 71 component routes | The 71-row H/C/G and demo inventory above remains the overall component index. | Runtime demos, APIs, tokens, docs, Theme Preview, and Semantic DOM audits remain active. |
| Next | Continue the active component parity queue from Table and Select, then select subsequent work by current mismatch volume. | Docs.Server remains on port 5000; browser tabs for Blazor/React Table and Blazor header comparison remain open. |

## Focused continuation — 2026-11-16 (Select controlled search value)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Select — controlled `SearchValue` | Added `SearchValue` / `SearchValueChanged` with `@bind-SearchValue` support. External value changes refresh option filtering; closing the popup no longer destroys the internal controlled search string. Added a controlled-search demo and regenerated both locale metadata files. | Docs.Server `net11.0` build succeeds (0 errors; 591 existing warnings). Browser test: changing the external input to `jack` updates the Select search input to `jack` and leaves only the `Jack` option in its popup. The page has no browser console errors. This validates the controlled interaction, not React DOM parity. |
| Select inventory | Now 55 Blazor demos / 28 React demo names represented. | The broad H/C/G scan remains historical. Continue exact API gaps such as `fieldNames`, then review the full route. |
| Icon and other components | No route-wide status changed. The 71-component table at the beginning remains the component-by-component progress index. | Full Icon API/token/FAQ/demo audit and the other 69 routes' runtime DOM/CSS, APIs, tokens, docs, Semantic DOM, and Theme Preview remain open. |
| Next | Continue Select API reconciliation and full route review, then resume Icon docs/tokens. | Docs.Server Development is running at port 5011. Blazor Select controlled-search demo and React Select reference pages remain open in the browser. |

## Focused continuation — 2026-11-17 (Header utility controls)

| Area | Current progress | Verification / remaining |
|---|---|---|
| Header version selector | Uses the React-style filled small Select with the current React v6 version (`6.7.1`) and all ten entries from React's `versions.json`. Selecting 6.x returns to the same localized Blazor route; historical versions preserve the current component path, query, and hash while removing the Blazor language prefix. The popup is hosted in the header utility row. | Browser confirmed the selected value is `6.7.1`, all ten labels render, the control measures 88×24px, and its dropdown 88×264px. |
| Header sponsors / tooltips | Sponsor avatars open localized hover/focus Popovers sized from their 320px card content and constrained to the viewport. Language and direction controls expose matching Tooltip text plus native `title` and accessible labels; theme and GitHub controls expose title/tooltip and GitHub link attributes. | Browser confirmed the localized YouMind card, a 344×173px popup containing a 320px card, language description `中文 / English`, direction description/title `LTR`, theme title, and GitHub Tooltip plus `title`, `aria-label`, `target`, and `rel`. |
| Build and component queue | Docs.Server `net11.0` and AntDesign.Docs `net11.0` both build with 0 errors (existing dependency/analyzer warnings). The 71-component matrix at the beginning remains the complete inventory; this header work does not close the component parity audit. | Remaining component DOM/CSS, API, token, docs, Theme Preview, and Semantic DOM parity remain open. |
| Next | Continue the highest-volume Table/Select gaps, then refresh the component queue from the focused inventory. | Docs.Server runs at `http://127.0.0.1:5000`; Blazor English/Chinese Table pages and the React reference remain open. |

## Focused continuation — 2026-11-17 (Select component Design Tokens)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Select — component Design Tokens | Audited React's `token.ts`, the C# `SelectComponentToken`, generated CSS token definitions, and English/Chinese component docs. All 24 public token names are present in the C# model, generated metadata, and both localized docs, including the corresponding `--ant-select-*` variables and token-based defaults. | React and Blazor Select Design Token tables each render 24 rows in the browser. No component token gaps found in this focused audit; remaining Select API, demos, runtime DOM/CSS, Semantic DOM, and Theme Preview work remains open. |
| All 71 components | The full 71-row inventory above remains the component-by-component status matrix; these focused Select token findings are an addition to that dated candidate scan. | Other routes' API, docs, token, runtime DOM/CSS, Semantic DOM, and Theme Preview statuses remain as recorded in their focused entries and are not implied complete. |
| Next | Continue the Icon documentation/API comparison, then return to the largest remaining Table/Select runtime mismatch. | React Select and Blazor Select Design Token pages remain open at 1512px; Docs.Server is running at port 5000. |

## Focused continuation — 2026-11-17 (Table Editable Cells)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Table — Editable Cells | Fixed the edit-state Form context that previously threw at runtime, added React-matching required-name validation and save behavior, aligned the editable cell and validation row sizing, and removed the redundant Popconfirm trigger wrapper. | net11 Docs and Docs.Server builds succeed. Browser interaction confirms blank values show `name is required.`, valid values save, and Add a row appends a row. StyleGen compares React/Blazor initial, edit, and edit-state scenarios: 3/3 pass with 0 DOM/CSS/geometry/visual differences after normalizing framework-generated form attributes and equivalent inline style serialization. Fixture: `tools/AntDesign.StyleGen/examples/table-edit-cell-dom-style-parity.json`; report: `/tmp/table-edit-cell-dom-style-parity-20261117-verified.json`. |
| All 71 component routes | The full 71-row H/C/G and demo inventory above remains the overall index; this focused Table result updates one demo only. | Other Table demos and every component's remaining API, docs, tokens, Semantic DOM, runtime, and Theme Preview audits remain open. |
| Next | Continue Table by remaining mismatch volume; revisit Select/Icon route and API gaps as tracked, then refresh the queue. | Docs.Server remains on port 5000 with Table Editable Cells open. |

## Focused continuation — 2026-11-17 (App and FloatButton.BackTop)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| App | Added both React App demos (`basic`, `config`) in Razor and bilingual demo metadata; added message/modal/notification access through `AntDesignAppContext`, Message/Notification defaults, and React-compatible `Component="@false"` root omission. | Docs and Docs.Server build on net11 with 0 errors. Browser confirms the App route, generated menu entry, both demo panels, API table and tokens render. Config applies to shared Blazor service providers; true per-App isolation remains an architectural difference. Demo iframe runtime interaction was not verified in this pass. |
| FloatButton / BackTop | BackTop behavior is documented only under FloatButton as `FloatButton.BackTop`, with Blazor `<FloatButtonBackTop>` parameter names; removed the separate BackTop documentation route and marked the legacy `<BackTop>` component obsolete for source compatibility. | Docs build and App page navigation succeed. FloatButton's demo already renders `<FloatButtonBackTop>`; dedicated scroll interaction was not re-run here. |
| All 71 routes | App's broad inventory row now has both React demos matched by names. This focused change does not close the component API, runtime, token, docs, Semantic DOM, or Theme Preview audits. | The 71-component matrix remains the complete progress index; other rows keep their previously recorded broad-scan status. |
| Next | Resume the remaining Table runtime demo mismatch, including expanded-row spanning in `expand-sticky`, then continue by remaining demo/API mismatch volume. | Docs.Server remains running at port 5000 with the browser on the App page. |

## Focused continuation — 2026-11-17 (Table Expand Sticky)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Table — Expand Sticky | Matched React's expanded-row span adjustments: inserted detail rows extend the expand icon and leading offset cells; the demo uses the source Team `rowSpan=2`, which renders as 3 only while expanded. | Initial and expanded-first-row fixture scenarios pass 2/2 with zero normalized DOM/CSS/geometry/visual-style differences. Manual browser DOM confirms Team=3, expander=2, Name=2, detail `colSpan=3`. Raw HTML still differs by framework whitespace/renderer markers. Report: `/tmp/table-expand-sticky-dom-style-parity-20261117-final.json`. |
| All 71 routes | This closes one Table demo only. The App and FloatButton status remains as recorded above; other components' token/docs/API/runtime/Semantic DOM audits remain open. | Broad inventory values remain a candidate map and should not be treated as complete acceptance. |
| Next | Continue Table demos by remaining React-only/demo mismatch volume, then refresh the broader queue. | Docs.Server is running on port 5000; browser is left on the Table `expand-sticky` demo for manual review. |

## Focused continuation — 2026-11-17 (App demo iframe routing)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| App — demo iframe rendering | Fixed the default isolated-demo URL in `CodeBox` to include the active locale (`/{culture}/mock?demoId=...`). The previous unlocalized `/mock` route had no matching culture route, so its generated demo lookup returned an empty result. The same localized URL is used by the open-in-new-window action; explicit demo links remain honored. | Docs.Server `net11.0` build succeeds (0 errors). Direct requests for both `en-US/mock?demoId=components-app-demo-basic` and `...config` return rendered Ant Design markup. Browser inspection confirms both iframe bodies show their buttons. `git diff --check` passes; App page remains open in the browser for manual review. |
| All 71 component routes | This resolves the actual rendering gap for the two App demos; it does not close App API/token/runtime/Theme Preview audits or change the other component rows in the 71-route index. | Continue to treat the 71-row matrix as active and incomplete; the focused evidence above only applies to App demo routing. |
| Next | Continue the remaining Table runtime demo mismatch queue, then refresh the broader component queue by current React/Blazor mismatch volume. | Docs.Server is running on port 5000 with the App page open. |

## Focused continuation — 2026-11-17 (Table Drag Sorting)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Table — Drag Sorting | Synchronized the row-sort demo's accessible drag instructions text and hidden-node behavior with React. The local `aria-describedby` id remains unique and resolves correctly; React's dnd-kit id is page-order generated. | StyleGen fixture passes 1/1 with zero normalized DOM/CSS/geometry/visual-style differences. Browser confirmed keyboard reorder and pointer movement preview/reorder. Docs.Server `net11.0` build succeeds (0 errors, 591 existing warnings). Fixture ignores only the dynamic `aria-describedby` value; browser separately verifies the relation and text. |
| All 71 component routes | This closes one Table demo and does not close Table's remaining demos or any full API/token/docs/Semantic DOM/Theme Preview/runtime audit. | The 71-row component inventory above remains the active index; broad scan values are historical candidates. |
| Next | Continue Table with Drag Sorting with Handler, then Drag Column Sorting and the remaining Table queue. | Docs.Server is running on port 5000; App and Table pages remain open in separate browser tabs. |

## Focused continuation — 2026-11-17 (App demos and generated Semantic DOM registry)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| App | Added localized demo descriptions, made Message/Modal/Notification demo actions resilient when the cascading App context is absent, documented the `.ant-app` root Semantic DOM and clarified use of shared global tokens. React's App page currently has no Semantic DOM section. | Docs.Server net11 build succeeds (0 errors; 641 existing warnings). Browser clicked Basic message/modal/notification and Config message/notification actions; each rendered the expected overlay with no captured exceptions. App semantic preview rendered `.ant-app`; global Design Token table is present. |
| Semantic DOM preview infrastructure | Dim Sum `DemoGenerator` emits a case-insensitive component key → component `Type` dictionary, selecting `_semantic` when available and otherwise the first ordered demo. Dedicated preview components override generic choices; FloatButton Group and Card Meta supplemental previews use a second generated registry. `Components.razor` now resolves these registries and renders with `DynamicComponent`, without component-name conditionals or runtime reflection. | Build and App route are verified. Full registry coverage/visual parity across all components remains open. The browser is open on the App docs page at port 5000 for manual review. |
| Next | Continue the active Table queue with Drag Sorting with Handler, then Drag Column Sorting. | App and Table work remains within the broader 71-route audit; counts in the matrix above are historical scan values and are not a fresh full-site H/C/G scan. |

The generated preview registry received additional browser smoke checks on Select (mounted Semantic DOM Select and popup) and Table (mounted semantic table with three body rows); both routes completed without browser exceptions. Temporary validation tabs were closed, leaving the App page open.

## Focused continuation — 2026-11-17 (Table Drag Sorting with Handler)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Table — Drag Sorting with Handler | Moved the drag handle into the leading action column and targeted that first-cell icon button, matching React's demo structure. | Fresh React/Blazor fixture: 2 groups and 2 scenarios passed; normalized DOM, computed styles, and visual styles are equal. Pointer interaction on both pages produced the same logical order (Jim, John, Joe); the Blazor transform preview was directly sampled, while React's intermediate preview was not separately captured. Report: `/tmp/table-drag-sorting-handler-20261117-final.json`. |
| All 71 component routes | This closes one Table demo only. API, Design Token, documentation, Semantic DOM, Theme Preview, and remaining runtime audits are still active. | The 71-row matrix above remains the full component index; broad inventory counts are historical and do not establish completion. |
| Next | Continue Table with Drag Column Sorting, then choose remaining Table work by React/Blazor mismatch volume. | Docs.Server remains on port 5000. The App manual-review page stays open; temporary parity pages were closed. |

## Focused continuation — 2026-11-17 (Table current-build audit)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Table — Drag Column Sorting | The existing implementation already matches React's sortable header accessibility, active/target styling, sibling movement, and floating header preview. | Prior browser interaction recorded in `dom-style-parity.md` moved Name over Age and committed the new order; the fresh fixture pages in this turn did not hydrate their offscreen React demo, so this was not counted as a new interaction pass. |
| Table — Virtual List | Re-ran the current `scroll-to-index-999` scenario against both live sites. | 1/1 passed: normalized DOM, selected computed styles, and visual styles match. Report: `/tmp/table-virtual-list-active-20261117.json`. |
| Table — Style/Class virtual table | Synchronized virtual table and fluid-column widths to rc-table's available-width distribution. | Follow-up `/tmp/table-style-class-virtual-width-fixed-20261117.json`: normalized DOM trees and selected computed styles match; 3 repeated last-column visual width measurements remain 0.016px narrow. Inline values and sampled geometry match. Keep the subpixel difference open without a layout hack. |
| All 71 component routes | This round closes the 17 inline-width DOM differences for this Table fixture, leaving one tiny rendered-width discrepancy; it does not complete Table or any global API/token/docs/Semantic DOM/Theme Preview/runtime audit. | The full component matrix above remains the progress index; its broad counts are a historical snapshot and need refresh before acceptance. |
| Next | Continue remaining Table demos by React-only/mismatch volume; revisit the 0.016px final-column quantization only if it can be solved without changing React-matching inline styles. | Docs.Server stays on port 5000; keep the App manual-review tab open. |

## Focused continuation — 2026-11-17 (Table Auto Height)

| Component | Current status | Evidence / next step |
|---|---|---|
| Table — Auto Height | React-matched content, More Data toggle (30/2 records), 400px host, 20/20/60 columns, responsive height measurement, fixed-header section/body heights, and semantic measurement classes. | Focused fixture passes 2/2; normalized DOM and visual styles pass. Inline column widths are normalized to 0.1px to absorb subpixel serialization only. `/tmp/table-auto-height-20261117-verified-final.json`. Continue the remaining Table demos and page-level audit. |
| Next | Continue Table demos by current mismatch priority; then return to Select/Icon page/API/token audits and remaining Design Token, documentation, Semantic DOM, and Theme Preview coverage. | The 71-component H/C/G table above remains a dated broad-scan snapshot. Focused passes close only the named demo scenarios; the full component matrix remains incomplete. |

## Focused continuation — 2026-11-18 (Table Measure Row Render)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Table — Measure Row Render | Synced the fixed-header split, percentage column measurement, measurement-row filter/sorter controls, and initial Address sorter tooltip state to React. | Docs.Server `net11.0` build: 0 errors (4194 existing warnings). Focused fixture 1/1 passed with equal normalized DOM, selected computed styles, geometry, and visual styles. First cold navigation timed out; the warm retry passed. Report: `/tmp/table-measure-row-render-20261118-warm.json`. |
| Table / all 71 components | This closes one Table demo only; Table's remaining demos and component API, tokens, docs, Semantic DOM, Theme Preview, and runtime audits remain open. | Full 71-component matrix above remains a historical inventory and is not a completion claim. |
| Next | Continue Table demos by remaining React/Blazor difference volume, then resume Select/Icon and global/component token and docs audits. | React and Blazor Table pages remain open for manual review. |

## Focused continuation — 2026-11-18 (Table Narrow and regression)

| Component / area | Current progress | Verification / remaining |
|---|---|---|
| Table — Narrow | Fixed React's normal pagination page-window behavior when both jump controls are present, and stopped emitting the internal column-count CSS variable for tables that have neither sticky nor fixed columns. | Focused Narrow fixture passes 1/1 with equal normalized DOM, selected computed styles, geometry, and visual styles. Report: `/tmp/table-narrow-dom-style-parity-20261118-fixed-rerun.json`. |
| Table — Measure Row Render | Sticky-table behavior still emits the internal column-count variable as required; the prior demo remains aligned. | Regression fixture passes 1/1 with 0 DOM/CSS/geometry/visual-style differences. Report: `/tmp/table-measure-row-render-20261118-final.json`. |
| All 71 component routes | This closes two focused Table demo states only. The rest of Table and the full API, token, docs, Semantic DOM, Theme Preview, and runtime audit remain open. | See the 71-row matrix above; it is a historical broad-scan snapshot, not a current acceptance result. |
| Next | Continue remaining Table demos in descending React/Blazor mismatch volume, then resume Select/Icon route and API/token/docs audits. | React and Blazor Narrow demo pages remain open in the browser for manual review. |
