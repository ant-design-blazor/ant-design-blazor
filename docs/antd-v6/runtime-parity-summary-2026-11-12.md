# React v6 / Blazor v6 runtime parity inventory — 2026-11-12 snapshot (focused updates through 2026-11-15)

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
| Select | 28 | 51 | 28 | — | 1 | 1 | 1 |
| Slider | 15 | 17 | 15 | — | 15 | 15 | 13 |
| Switch | 7 | 6 | 3 | text, disabled, style-class, _semantic | 3 | 2 | 2 |
| TimePicker | 16 | 14 | 13 | 12hours, change-on-scroll, _semantic | 13 | 7 | 7 |
| Transfer | 12 | 13 | 11 | _semantic | 11 | 11 | 10 |
| TreeSelect | 13 | 18 | 12 | _semantic | 11 | 9 | 9 |
| Upload | 22 | 25 | 22 | — | 19 | 14 | 14 |
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
| App | 2 | 0 | 0 | basic, config | 0 | 0 | 0 |
| ConfigProvider | 7 | 6 | 4 | locale, wave, holderrender | 4 | 3 | 3 |

The Upload row is refreshed from the 2026-11-15 one-component route scan; the
other rows remain the original broad-scan snapshot and must be rechecked before
they are treated as current status.

## Focused rechecks completed this round

| Component / scope | Result | Evidence |
|---|---|---|
| Table — registered fixture suite | 16 groups / 20 scenario states pass with zero normalized DOM/CSS/geometry/visual-style differences. Covers Basic, bordered/size, colspan/rowspan, defaults/expand, custom empty, ellipsis/tooltip, fixed columns/header, sorting, pagination placement, row selection, and summary. This is not all 43 matched demos. | `/tmp/table-suite-active-goal-20261113-final.json`; details in `dom-style-parity.md` |
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
| Icon | Five canonical React demos pass 5/5; gallery toolbar/responsive states pass 2/2. Inventory matched all 5 React demo names with no runtime differences. | `/tmp/icon-parity-final-20261113.json`; `/tmp/icon-gallery-responsive-followup-20261113.json`; verify remaining gallery search and two-tone states. |
| List | Vertical demo passes 1/1 against React, including item count, pagination, avatar, action, image and footer structure/styles. | `/tmp/list-vertical-parity-20261113-verified.json`; other List demos still need coverage. |
| Table | Registered suite passes 16 groups/20 states. Gapped fixed columns, regular fixed columns, and fixed-column headers pass 1/1 each; Reset Filter initial, sort, and toolbar checks pass 3/3. Latest inventory: 43/43 React demo names present; 35 HTML, 8 CSS, and 8 geometry demo flags. | `/tmp/table-suite-active-goal-20261113-final.json`; `/tmp/table-fixed-gapped-columns-segment-fix-20261113.json`; `/tmp/table-fixed-columns-regression-20261113.json`; `/tmp/table-fixed-columns-header-regression-20261113.json`; `/tmp/table-reset-filter-verified-20261113.json`; `/tmp/table-inventory-reset-filter-20261113.json`. Next: Dynamic Settings / Style and Class / Virtual List and drag demos. |
| Next | Continue Table by high mismatch volume, while retaining remaining Select/Icon page-level checks and global/component Design Token coverage. | Full 71-component matrix above; detailed dated evidence in `dom-style-parity.md`. |

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
| Documentation shell / Select Semantic DOM | Removed the extra `.main-wrapper` top padding so the docs region starts directly under the fixed header, as requested against the React reference; retained the white container background and zeroed the component title's extra top margin. | Docs.Server `net11.0` build: 0 errors. Existing Select `_semantic` fixture passes 2/2 DOM/CSS/geometry/visual checks: `/tmp/select-semantic-recheck-final-20261115.json`. |
| All 71 components | The full matrix above remains the all-component progress index. Select/Icon focused checks, Table virtual-window DOM, all component APIs/tokens/docs, Theme Preview, and Semantic DOM audits remain incomplete. | Continue the 71-component mismatch queue and retain global/component Design Token coverage as open work. |
| Next | Continue Upload's remaining mismatched demos, then return to Select/Icon route-level gaps and Table virtual-window parity. | See the full component matrix above and dated focused records in `dom-style-parity.md`. |
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
| Documentation shell / brand | Removed extra wrapper top padding; matched Microsoft's `#512BD4` → `#D600AA` gradient in header and home title; removed the happy-work white text shadow that washed out the homepage gradient. | Computed styles, fresh homepage screenshot, and zero wrapper top padding verified. Docs.Server `net11.0` build: 0 errors. |
| Upload — StyleClass and PictureStyle | Replaced the semantic placeholder with React's two StyleClass Uploads and reconstructed PictureStyle's one picture-list demo plus direct status-specific row markup. | Both focused fixtures pass 1/1 with zero normalized DOM/CSS/geometry/visual differences: `/tmp/upload-style-class-parity-20261115.json`, `/tmp/upload-picture-style-parity-20261115.json`. |
| Upload — refreshed route inventory | React 22 demos, Blazor 25, 22 matched, no React demos missing. | Candidate DOM/CSS/geometry differences: 19/14/14; React route logs hydration error #418, so use focused fixtures for acceptance. Report: `/tmp/upload-inventory-current-20261115.json`. |
| All 71 components | The full matrix above remains the broad component/demo mismatch index. | Runtime, API, global/component tokens, docs, Theme Preview, and Semantic DOM parity remain incomplete. |
| Next | Continue Upload's next highest scoped gap, then proceed through the other components in descending mismatch volume. | Full progress matrix and focused records are in this file and `dom-style-parity.md`. |
