# React/Blazor DOM and style parity runner

## Upload Avatar — 2026-11-15

Recreated React's two upload controls side by side: one `picture-card` and one
`picture-circle`, both sharing avatar image state, loading state, file type/size
validation, and the React mock upload endpoint. The raw button trigger and the
128px trigger sizing rule match the React example; the outer component wrappers
render as 102px boxes on both pages with a 16px gap.

The focused fixture passes 1/1 with zero normalized DOM, computed CSS,
geometry, or visual-style differences. The browser screenshot confirms both
wrappers render at 102px with a 16px gap. Report:
`/tmp/upload-avatar-parity-20261115.json`. The refreshed 22-demo route scan
reports 19/12/12 candidate H/C/G differences, with React hydration error #418
blocking clean broad capture; Avatar's focused comparison is clean.

## Upload PictureCard demo — 2026-11-15

Aligned the PictureCard demo with the React example: the same upload endpoint,
input name, six file states, native button content, and default-hidden download
action; replaced the old Modal preview with the hidden Image component used by
React and removed the demo-only legacy CSS. The preview Image is mounted hidden
first, then opened after render so the Image component's preview service sees a
false-to-true visibility transition. The focused fixture passes 1/1 with zero
normalized DOM, computed CSS, geometry, or visual-style differences. A 0.02px
width tolerance accounts for 1/64px computed-width serialization. Browser
interaction verified opening the correct image and closing the preview. The
PictureCircle path now uses the same post-mount preview transition and was also
verified open/close. The combined five-scenario Upload fixture passes 5/5 after
the Upload default download behavior change. Reports:
`/tmp/upload-picture-card-final-20261115.json`,
`/tmp/upload-picture-circle-regression-20261115.json`, and
`/tmp/upload-baseline-regression-20261115.json`. Docs.Server `net11.0` builds
with 0 errors (existing analyzer warnings remain).

## Upload PictureCircle demo — 2026-11-15

Reworked the PictureCircle file rows to use React's direct thumbnail, name, and
actions siblings, removed the default download action to match React, restored
the preview link URL while preventing navigation when the preview callback is
handled, and added the hidden Image preview flow. The demo now uses React's
`name="file"` and matching inline button style. Focused React/Blazor fixture
passes 1/1 with zero normalized DOM, computed-style, geometry, or visual-style
differences; there are no blocked scenarios. Docs.Server `net11.0` builds with
0 errors (existing analyzer warnings remain). Report:
`/tmp/upload-picture-circle-final-20261115.json`. This closes the PictureCircle
initial-render gap; file upload and preview interaction still need manual
interaction coverage, and the rest of Upload remains under audit.

## Documentation page top alignment — 2026-11-15

Rechecked the rendered React v6 page after the earlier zero-gap adjustment.
React's fixed 64px header is followed by a normal-flow 32px top margin on the
documentation `<main>`; that space scrolls away with the page. Blazor now uses
32px top padding on `.main-wrapper`, moving both the sidebar and article below
the header while keeping the space in the scrolling document. The component
title keeps its zero extra heading margin. Browser measurements on React were:
header bottom `64px`, main top `96px`, and after scrolling 120px the main top
was `-24px` while the header stayed fixed.

## Blazor brand gradient and Upload StyleClass — 2026-11-15

Applied Microsoft's Blazor brand purple-to-magenta gradient to the `Blazor`
word in both the fixed documentation header and the homepage title, matching
the official page's `#512BD4` to `#D600AA` gradient. Browser computed styles
confirm the same gradient is active in both locations. The homepage's existing
five-layer white happy-work `text-shadow` washed the gradient out; disabling
that shadow only on the gradient title restores the full gradient, verified in
a fresh browser screenshot. The docs wrapper now has the React-matched 32px
top space, which stays in normal page flow and scrolls away with the content.

Rebuilt Upload `StyleClass` to match React's two vertical Upload examples,
including identical three-file status data, `name="file"`, `multiple` on the
second instance, per-slot item/trigger styles, and root class styling. The
focused fixture passes 1/1 with zero normalized DOM, CSS, geometry, or visual
style differences: `/tmp/upload-style-class-parity-20261115.json`. Docs.Server
`net11.0` build succeeds with 0 errors (existing warnings remain).

## Upload Picture list demo — 2026-11-15

Replaced the placeholder PictureStyle page with React's single picture-list
example and exact three-file states. Added the dedicated `UploadListType.Picture`
render path so uploading, successful, and failed rows use React's direct
thumbnail/name/actions/progress DOM, including the thumbnail image URL and
remove-button semantics. Focused fixture passes 1/1 with zero normalized DOM,
computed CSS, geometry, and visual-style differences:
`/tmp/upload-picture-style-parity-20261115.json`.

## Select Semantic DOM and Icon gallery responsive labels — 2026-11-15

Rechecked Select `_semantic` in live React and Blazor pages after the broad
inventory had reported a closed Blazor popup. Both current pages have
`aria-expanded="true"`, the popup is mounted, the selected option and three
option labels match, and the selected-value content opacity is `0.25`. The
focused Select fixture aligns the trigger root to viewport `y=250` on both
pages, avoiding a false placement difference when the docs pages start at
different scroll positions. Single and Multiple states pass (2/2), with no
normalized DOM, computed CSS, geometry, or visual-style differences. The prior
38px popup offset was caused by comparing triggers at different viewport
heights; no Overlay code change was needed. The broad inventory's closed-popup
sample was taken before Blazor Server finished its interactive rerender; it is
a scan timing false positive, not a current component mismatch.

The Icon canonical DOM/style fixture passes all five scenarios (5/5), including
the corrected IconFont sprite registration order. The responsive gallery
fixture also passes desktop and narrow viewport scenarios (2/2). Long names
wrap and expand Blazor gallery cells to avoid clipping; React's gallery still
clips these labels, so that documented usability improvement is an intentional
visual difference.

The Icon gallery exposed actual label clipping at both desktop and narrow
viewports in the shared React layout: each cell had a fixed six-column grid,
`white-space: nowrap`, and a 100px height with hidden overflow. Updated the
Blazor documentation gallery to auto-fit columns at a 120px minimum and wrap
long icon names inside each cell. At the 500px viewport, the gallery now uses
two 136px columns instead of six 34.7px columns; long labels render on two
lines, the cell expands to 108px, and the document has no horizontal overflow.
At 1280px it retains the six-column layout; long labels wrap and expand cells
to 108px without clipping. React's current gallery still clips those long names,
so this is an intentional usability correction to the Blazor docs, not a claim
of pixel-identical behavior. Both React and Blazor Icon pages and the Select
Semantic DOM pages remain open for manual review.

## List drag sorting demos — 2026-11-15

Replaced the shared placeholder content with the matching React demo data and
structures: the two vertical demos contain the same five news rows; the two
grid demos contain six Card items in a four-column grid with a 16px gutter.
The handle variants use the small text Button with the Holder icon, matching
React's activator. Sorting now previews during pointer movement: the active
List.Item follows the pointer and sibling rows/cards shift into their projected
slots, then the updated order is committed to the Blazor data source on drop.
Vertical lists constrain movement to the y-axis; grid sorting measures the
nearest item in both axes. Keyboard pickup, arrow movement, drop, and cancel are
also supported.

Built Docs.Server for `net11.0` with zero errors and exercised all four demos
in the browser. Each showed an active-item transform and sibling preview, then
committed the expected order (items 1, 2, 3 became 2, 3, 1); transforms and
dragging classes were removed after the Blazor rerender. No browser console
errors were reported. The list root width and grid Card dimensions/positions
match the React page measurements recorded during the initial comparison. The
focused pointer interaction was verified for both normal and handle-only
variants in vertical and grid layouts. This closes the interaction and
structure gaps for these four demos; broader List component parity remains part
of the 71-component matrix in `runtime-parity-summary-2026-11-12.md`.

## Table Virtual List latest rerun — 2026-11-14

Re-ran the focused `scroll-to-index-999` fixture against the current worktree
using the installed Chrome executable. It remains different:

- React shows the virtual window beginning at row 992; Blazor begins at row 991.
- Live geometry at the shared `scrollTop=54600` confirms row 991 is wholly
  above the viewport (`-95px` to `-40px`). Row 992 and subsequent visible rows
  have the same positions and 55px heights in both implementations (row 992 is
  at `-40px`). Do not translate the Blazor rows to compensate for this extra
  offscreen realization.
- React's holder uses `overflow: hidden` and positions the virtual window with
  `translateY(54560px)`. Blazor uses native `overflow: auto` and .NET
  `Virtualize` spacer elements; its inner holder has no translated window.
- Normalized DOM differences are 235 (down from 254 in the earlier overscan
  report); the body-cell sample count is 130 in React and 90 in Blazor.
- The fixed selection/header cells now carry the expected fixed-start classes.
  Remaining fixed-column differences include CSS-expression offsets and the
  fixed-end shadow state.

The current automated report is `/tmp/table-virtual-list-current-20261114.json`.
The visual root style and visible row geometry match, but the row window DOM,
scroll-container behavior, body-cell sample counts, and some fixed-edge states
do not. Keep this fixture open as an unresolved Table virtualization issue; do
not count the earlier partial report as a passing result.

A 2026-11-15 rerun with the installed Chrome executable reduces the normalized
DOM-tree difference count to 211. Computed styles and visual styles pass, but
the fixture remains different because the virtual-window markup and row/cell
counts do not match. Latest report:
`/tmp/table-virtual-list-current-followup.json`.

## Table Virtual List scrollbar follow-up — 2026-11-14

Tried switching the Blazor holder to `overflow: hidden` to match React's
inline style. The .NET virtualizer then selected an earlier, larger row window
and the focused fixture regressed to 282 DOM differences. Reverted that
behavioral change. Instead, kept the native scroll container and hid its visual
scrollbar with `scrollbar-width: none` and the WebKit scrollbar rule in the Table
patch and docs parity stylesheet. Browser verification shows `overflow: auto`,
`scrollbar-width: none`, `scrollTop=54600`, and rows 992 onward remain in the same
positions as React. The fixture's computed-style comparison remains equal, but
it still reports 235 DOM differences and visual semantic differences because
the virtual-window markup/cell sample counts differ. Latest report:
`/tmp/table-virtual-list-scrollbar-hidden-final-20261114.json`.

## Table Virtual List .NET 11 range-boundary probe — 2026-11-15

Used the live Docs.Server page and headless Chrome to probe the virtual holder
after clicking `Scroll To index 999`. At `scrollTop=54600`, Blazor renders row
keys 991–1000, with row 992 at `-40px` relative to the holder and the leading
reserved spacer at `54505px`. React renders row keys 992–1001 at that same
visible position, with the virtual content translated by `54560px`.

Setting the Blazor scroll position to `54610px` causes .NET Virtualize to
advance the rendered range to rows 992–1001 and update the spacer to `54560px`,
but row 992 moves to `-50px`. At `54654px` and `54655px`, the same range remains
and row 992 moves to `-94px` and `-95px`, respectively. A separate probe that
changed `scrollTop` without dispatching the scroll event did not update the
virtual range. This confirms that a fixed scroll offset alone cannot correct
the window without changing visible geometry. No source workaround was
applied; keep the Table virtualization mismatch open until the virtual window
and its spacer are controlled together and revalidated during subsequent
scrolling, including row-span overlays.

## Table Fixed Gapped Columns browser recheck — 2026-11-14

Rechecked the fixed-edge shadow mismatch recorded on 2026-11-13 in the open
React and Blazor docs pages at a shared 1280px viewport. The current pages now
match for all 4,997 fixed header/body cells: fixed-start/end shadow classes,
inline offsets and z-offsets, computed positioning, dimensions, backgrounds,
borders, shadows, and pseudo-element styles. The focused fixture also passes
1/1 with zero DOM/CSS/geometry/visual-style differences. This supersedes the
earlier report of six missing fixed-edge shadow classes for the current
worktree. Puppeteer's pinned Chrome 153 was absent from its cache; supplying the
installed Chrome executable through `--chrome` allowed the automated fixture
to complete. Both user-facing React and Blazor pages remain open for manual
review.

## Upload Basic and Drag DOM/style alignment — 2026-11-15

The Upload Basic and Drag examples now share React v6's trigger DOM structure.
The Drag trigger has the expected `role="button"`/`tabindex="0"`, its file input
uses `display:none`, and click-to-select is wired for both Basic and Drag modes.
The Drag demo now uses the current React endpoint, `name="file"`, and matching
success/error feedback and hint text. Focused comparison of both examples passes
2/2 scenarios with equal normalized DOM trees, computed styles, geometry, and
visual styles; report: `/tmp/upload-demo-pair-parity-fixed.json`. The unnormalized
HTML still differs on framework comments/whitespace, and the React page reports
its existing hydration/contributors fetch errors; Blazor page navigation has no
errors. Docs Server `net11.0` builds with zero errors.

The Drag trigger also matches React's Enter-key activation and native file-drop
path. Headless browser interaction verified Enter invokes the file picker once,
and dropping a `probe.txt` file sets the hidden input and dispatches one change
event. The fixture was rerun after rebuilding the local interop bundle and
restarting Docs.Server; `/tmp/upload-demo-pair-parity-final.json` passes 2/2.

## List drag sensor robustness follow-up — 2026-11-15

Rechecked all four sorting demos after reports that the drag preview did not
appear. Pointer checks now wait for stable layout and align the lazy-loaded
demo with the viewport before starting input. The standard list, handle list,
grid, and handle grid all show the active-item transform and sibling slot
preview, then reorder on release (4/4). Pointerdown uses capture phase, and the
standard list accepts the full row as a drag sensor so blank row space does not
silently ignore the drag. Text selection is disabled only during an active drag
and restored on drop or cancel; handle variants remain restricted to the
visible handle.

The static DOM/CSS/geometry fixture passes 4/4 with no differences at 0.04px
width tolerance (`.tmp/list-drag-sorting-after-fix.json`). Docs.Server
`net11.0` builds with zero errors. The helper cache version is `20261115.10`.

## Upload Basic DOM and trigger alignment — 2026-11-15

Aligned the Blazor Basic demo with React's action endpoint, multipart field
name, authorization header, and success/error messages. Updated the shared
Upload trigger markup to match React's `div.ant-upload-select >
span.ant-upload > input[type=file] + button` order, removed the obsolete
list-type trigger class and extra trigger attributes, and exposed the input's
`name` attribute. The click helper now locates the input inside the trigger
instead of relying on a generated `data-fileid` and document lookup. The demo
button content no longer adds an extra nested span.

Docs.Server `net11.0` builds with zero errors. Rebuilt the local ignored JS
interop bundle from the TypeScript source and verified that clicking the demo
button invokes the nested file input without selecting or uploading a file.
The focused Upload Basic fixture passes 1/1 with zero normalized DOM, computed
CSS, geometry, or visual-style differences; React's page still logs its known
hydration error #418. Report: `/tmp/upload-basic-parity-final-bundle.json`.

## Current continuation checkpoint — 2026-11-15

The latest List drag fixture remains green: all four free-drag/handle-drag and
list/grid scenarios pass normalized DOM, computed style, and geometry checks;
pointer interaction confirms live preview, committed reorder, and cleanup. The
current report is `.tmp/list-drag-sorting-current.json`.

Rechecked Table Virtual List with the current Docs.Server build and installed
Chrome. The first fresh capture reported three `borderBottomStyle` differences
caused by the docs host's global `border: 0 solid` reset. Added a scoped
component patch for the Table wrapper and virtual rows; after rebuilding and
restarting the same `:5000` server, computed CSS/visual styles pass 1/1 with
zero differences. Normalized DOM still reports 221 differences. Blazor begins
at row 991 with a 54505px `Virtualize` spacer; React begins at row 992 with a
54560px translated inner window. Latest report:
`.tmp/table-virtual-list-border-fix.json`.

Source review narrows the scroll mismatch: React Table delegates
`scrollTo({ index })` to `@rc-component/virtual-list` with its `auto` alignment
when no explicit alignment is provided. Blazor's `ScrollTo(int)` currently
derives a pixel offset from the average height of mounted rows and scrolls the
holder directly. The target row lands in the same viewport, but the two window
start rules differ; changing only the final scroll offset could move visible
rows out of alignment. React derives its virtual item estimate from Table
tokens, while Blazor's .NET `Virtualize` adds framework spacers and measures
items at runtime. A durable fix needs to align the rendering window itself.

## List Vertical current-build recheck — 2026-11-15

Restarted Docs.Server from the current `net11.0` build and reran the
React/Blazor Vertical demo fixture at desktop (1280px), tablet (768px), and
narrow (390px) widths. All 3/3 states pass with zero normalized DOM,
computed-style, geometry, or visual-style differences; the reported style
mismatch does not reproduce on the current build. Report:
`.tmp/list-vertical-restarted-report.json`.

### Drop preview follow-up — 2026-11-15

The reported “no drag preview” also exposed a release-state gap: the active
item followed the pointer, but the drop committed immediately, before the
active row animated into the insertion slot. The client helper now keeps the
active item and displaced siblings in their projected positions for the same
200ms transition before requesting the Blazor reorder; canceled drags animate
back to their original slot. Removed the older native HTML5 `Draggable` sample,
which was not one of React List's four sortable demos and used the browser's
uncontrolled drag ghost. Incremented the module URL version so the browser
loads the updated helper.

After rebuilding/restarting Docs.Server, the four current demos were exercised
again through pointerdown/move/up in the browser. The active transform and
sibling preview were visible in all four; release reordered rows `2,3,4,5,1`
and cards `2,3,4,5,6,1`, then cleared transforms and drag classes. The page is
left open at Grid drag sorting with handle for review.

## List drag sorting current-build verification — 2026-11-15

After adding the dnd-kit-equivalent 1px pointer activation threshold, unique
per-demo item IDs, and exact per-demo `aria-describedby` wiring, rebuilt and
restarted Docs.Server so the browser served the new Docs assembly. Verified
the standard list, handle list, and free grid with real browser drag input; each
reordered the live Blazor data and cleared temporary transforms on release.
The handle grid also reordered by Space → ArrowRight → Space. The drag
description now points to the intended visually-hidden instructions rather
than an item ID. Removed demo-only `aria-label` values from handle buttons to
match React's rendered attributes. React and Blazor pages remain open for
manual review.

## Table drag column sorting — 2026-11-15

Replaced the demo's native HTML draggable wrapper with pointer-driven column
sorting and a floating header preview. The active header and its body cells
fade to 50% over a gray background; the active header follows the pointer,
intervening columns shift with a 200ms ease transition, and the prospective
target receives a dashed inline edge. Releasing commits the reordered columns
through Blazor and removes the preview. Space/Enter, arrow keys, and Escape
support keyboard pickup, movement, drop, and cancel.

The rendered headers now use the same numeric column IDs and sortable
accessibility attributes as React's dnd-kit demo (`role=button`, `tabindex=0`,
`aria-disabled=false`, `aria-roledescription=sortable`, and the associated
description ID); body cells carry the matching column IDs. Browser comparison
confirmed identical initial header widths (80.64, 81.61, 59.38, 193.28, and
87.53px) and 55px header height. A browser pointer-drag of Name over Age showed
the gray active state, sibling translation, dashed target feedback, and
floating Name preview, then committed the order Gender, Age, Name, Email,
Address and removed the preview. Docs and Docs.Server `net11.0` builds both
passed with 0 errors. React's reference implementation is
`components/table/demo/drag-column-sorting.tsx`; the React page remains open for
manual interaction comparison.

## Table Virtual List visible-window comparison — 2026-11-15

Re-ran the focused Virtual List scenario after narrowing the semantic row and
cell checks to the corresponding visible React/Blazor row (key `992`). The
current runner report is `/tmp/table-virtual-list-visible-final-20261115.json`:
root computed styles and the selected row/cell visual styles and geometry pass
(1/1 visual-style pass, 0 visual-style differences), while strict normalized
root DOM comparison still reports 221 differences.

The remaining structural differences are framework-specific: React renders a
translated inner window at `translateY(54560px)` with `overflow:hidden`; Blazor
uses .NET `Virtualize` spacer elements, `overflow:auto`, and a zero inner
transform. Blazor's first mounted row is key `991`, fully above the viewport;
the matched visible row `992` remains 40px above the holder top on both sides.
The row height (55px) and its column widths match. The extra row is not a visible
style defect, but it is a real normalized DOM difference, so Virtual List is
still not marked fully aligned. Removing/hiding the framework spacer in CSS
would risk breaking scroll-window calculations; matching React's DOM structure
requires replacing or adapting the virtualization rendering strategy.

Live browser recheck on dedicated React and Blazor pages confirmed the exact
window after clicking “Scroll To index 999”: both holders have `scrollTop=54600`
and `clientHeight=400`. React renders rows 992–1001 and translates its inner
window to 54560px. Blazor renders rows 991–1000 with a 54505px leading
Virtualize spacer and no inner transform. Both place row 992 at -40px, and both
render the same merged-cell overlay rows (992, 996, 1000). This confirms that
the visible geometry matches while the virtual range and renderer structure do
not; changing the scroll offset alone would only trade one mismatch for another.

A speculative follow-up tried advancing the `ScrollTo(index)` target by one
extra row. The fixture contains 10,000 items, so the extra 55px would change
the target from React's `scrollTop=54600` to `54655`; it is not clamped at the
end of the list. The experiment was removed because it moves the requested row
one position too far down. The focused report still has 221 normalized DOM
differences and 0 visual-style differences
(`/tmp/table-virtual-list-after-offset-20261115.json`); it was captured after
restoring the React-aligned scroll formula.

Source inspection of React's `@rc-component/virtual-list` confirms the required
renderer contract: the holder owns `max-height` and hidden overflow; its first
child is a full-height filler (`data.length * itemHeight`); the filler contains
an absolutely positioned `ant-table-tbody-virtual-holder-inner` translated to
the measured start offset; the mounted range starts at the first row whose
bottom is at or below `scrollTop`, and includes the first row below the viewport
plus one additional row for motion. At the fixture's `scrollTop=54600`, 55px
rows, and 400px height, this yields start index 992, offset 54560px, and rows
992–1001. React also renders custom vertical and horizontal scrollbars as
siblings of the filler. The current .NET `Virtualize` path instead owns the
spacers, so the next implementation should replace that path for client-side
virtual data and keep provider-backed data as a separate case.

## Table Virtual List current-build recheck — 2026-11-15

Rebuilt and restarted Docs.Server on `net11.0`, reloaded the user-facing browser
page with the current framework script, and clicked “Scroll To index 999”. The
demo mounted without a runtime alert and reached `scrollTop=54600` with a 400px
holder. The fresh focused report, `.tmp/table-virtual-list-after-rebuild.json`,
again passes the visible-row visual-style check (1/1, 0 visual-style diffs) and
retains 221 normalized DOM differences. This confirms the remaining gap is the
window/spacer DOM model, not a failure to render or scroll the demo.

## Table Virtual List viewport-height experiment — 2026-11-15

Tested using the full configured `ScrollY` as the inner `Virtualize` viewport
instead of subtracting 29px. The focused fixture still reports 211 normalized
DOM differences and 0 visual-style differences; the framework spacer remains
54505px and the rendered window still starts at row 991. Reverted this change
because it did not improve the window and did not move the target toward React's
rendered structure. Report: `/tmp/table-virtual-list-viewport-height-20261115.json`.

The remaining client-side virtual-list work needs a renderer that owns the
React-shaped filler, translated inner window, and visible row range instead of
trying to tune the framework `Virtualize` spacer. Keep provider-backed data on
its existing path while replacing only the client-side virtual rendering.

## Table drag sorting demos — 2026-11-15

Aligned the Table drag-row transition to dnd-kit's `transform 200ms ease` and
kept the normal drag-sort rows at `cursor: move` after drag cleanup. The handler
variant keeps the row cursor unchanged and uses the handle's `move` cursor.
Split the sample's first-row address by variant to match React: the standard
demo uses its long address and the handler demo uses `Long text Long`.

Browser comparison of the standard demo now matches row text, relative row
positions (55px, 308px, 385px), heights (253px, 77px, 77px), and cursors across
React and Blazor. The handler variant has three 77px rows and matching handle
cursor. Browser drag checks on both Blazor variants showed active-row
translation, adjacent-row insertion preview (`transform 200ms ease`), and the
final order `1, 3, 2`. React and Blazor Table pages remain open on the standard
drag-sort demo for manual review.

## Docs component-page mobile alignment — 2026-11-14

At 500px, the Blazor component article retained a 48px desktop sidebar offset,
placing the List Vertical code box at x=48 while React placed it at x=0. The
mobile article now uses a relative inline-start offset of -48px while retaining
its original padding and width. Browser comparison confirms the Vertical demo
starts at x=0 and is 288px wide on both pages. Its inner responsive layout,
image sizing, row height, typography, actions, and spacing remain aligned. The
Docs.Server net11 build passed with 0 errors; existing warnings remain.

## Select and Icon focused recheck — 2026-11-14

The Select Semantic DOM preview fixture passes 2/2 states with zero normalized
DOM, computed CSS, geometry, or visual-style differences
(`/tmp/select-semantic-preview-final-20261114.json`). The canonical Icon demo
fixture passes 5/5 with zero differences
(`/tmp/icon-dom-style-parity-final-20261114.json`), and the responsive Icon
gallery passes desktop and narrow scenarios 2/2 with zero differences
(`/tmp/icon-gallery-responsive-final-20261114.json`). The long-name wrapping
behavior remains an intentional documented deviation from React's clipped
labels.

## List Vertical computed-style recheck — 2026-11-14

Re-ran the existing `list-vertical-dom-style-parity.json` fixture against React
`http://127.0.0.1:8000/components/list` and Blazor
`http://127.0.0.1:5000/en-US/components/list`. The rendered DOM tree, root and
all 10 semantic node selectors, computed styles, geometry, and visual-style
checks pass on the desktop baseline with zero differences. This verifies the Vertical
demo's responsive image rule (`display: block; max-width: 100%`) and the title
weight (`400`), which are required by the actual React documentation cascade.
Report: `/tmp/list-vertical-parity-20261114-final.json`.

The viewport coverage was extended to desktop (1280), tablet (768), and narrow
mobile (390). That exposed the Blazor component-demo grid row shrinking below
its content width on mobile due to the Row's negative 8px gutters. Setting the
grid row to `calc(100% + 16px)` restores the expected demo width and prevents
the List preview from collapsing. After rebuilding and restarting Docs.Server,
all three responsive scenarios pass with zero DOM/CSS/geometry/visual-style
differences: `/tmp/list-vertical-responsive-final-20261114.json`.

The raw serialized HTML still differs only in Razor-emitted whitespace around
text content; the normalized DOM tree is equal. The Blazor List page is open at
`http://127.0.0.1:5000/en-US/components/list#components-list-demo-vertical`
for manual review. The broader 71-component inventory remains the dated
baseline in `runtime-parity-summary-2026-11-12.md`.

## Transfer Custom Select All Labels — 2026-11-13

Added the React-compatible `SelectAllLabels` parameter for static labels and
count-aware render templates, and completed the previously empty
`CustomSelectAllLabels` demo. The source/target labels now follow React's
separate first/second entries; selected and total counts are supplied through
`TransferSelectAllLabelInfo`. Updated the English and Chinese Transfer API
tables. The demo metadata was regenerated and the full Docs.Server build passed
with 0 errors. The server is running at `http://127.0.0.1:5000`. A focused
browser interaction and React DOM/CSS fixture remain pending because the shared
Chrome DevTools profile is currently owned by another process.

## Select responsive suffix and generated Icons catalog recheck (2026-11-08)

The responsive Select suffix offset differed by 0.016px because Razor emitted
leading/trailing whitespace in the aggregate tag text. Rendering the aggregate
label as one interpolated string removes those anonymous whitespace nodes. The
focused responsive fixture now passes 1/1 with no DOM, computed-style,
geometry, visual-style, or blocked differences:
`/tmp/select-responsive-expression-fix-20261108.json`.

After updating the generated Icons package, Select Custom Tag Render also
passes 1/1; close-icon SVG attributes now match, including `fill-rule`:
`/tmp/select-custom-tag-render-final-20261108.json`. The five canonical Icon
groups remain 5/5 with zero differences. These checks supersede the later
11/08 entries below that listed the suffix and close-icon issues as open;
Select's full-page demo inventory and other candidate demos remain incomplete.

## Select Size / Variant / Placement runtime checkpoint (2026-11-08)

Updated the React-facing string values in the Blazor Size and Placement demos and
matched React's eight-control Variant layout, including `flex: 1 1 0%`. Re-ran
six scenarios against React `:8000` and Blazor Development `:5050`: Size default,
large, small; Variant default; Placement default and bottom-left. All six passed
with zero DOM-tree, computed-style, geometry, SVG, or visual-style differences,
and no blocked scenarios. Report: `/tmp/select-focused-final-20261108.json`.
Strict outerHTML bytes still differ due to Razor inter-component whitespace; the
normalized runtime DOM tree is equal. The Placement radio group is outside each
Select root, so it is not treated as a Select semantic slot; its real click is
still exercised in the bottom-left scenario.

## Typography Basic runtime checkpoint (2026-11-08)

Aligned the Blazor Basic example with React's article, bilingual content, keyboard
and code samples, lists, divider, blockquote, and preformatted block. The runtime
comparison now passes ordered DOM-tree, all 11 declared Semantic DOM selectors,
computed CSS, and visual-style checks (1/1 scenario; zero differences). Strict
outerHTML bytes still differ because Razor emits whitespace between child
components; this is retained as a diagnostic and does not count as a DOM-tree
difference. The comparison also found and fixed the missing
`.ant-typography-link` class on Blazor `Link` elements. Report:
`/tmp/typography-basic-dom-style-parity-final.json`; the Blazor and React pages
are open in the browser for manual review.

The same current-session rerun revalidated the Select and Icon pilots before
continuing: Icon passed 5/5; Select size passed 3/3, popup passed 3/3,
interactions passed 3/3, Semantic DOM passed 2/2, suffix passed 1/1, and virtual
options passed 1/1. Each report has zero DOM-tree, computed-style, or visual-style
differences and zero blocked scenarios. Reports are in `/tmp/*-parity-current-2026-11-08.json`.

## Select first-open focus state recheck (2026-11-03)

The current fixture separates the closed/Escape scenarios from the popup-open
portal capture and captures Blazor before React. The closed and Escape group
passed 2/2; the popup-open group passed 1/1. All three isolated scenario
reports contain zero DOM-tree, computed-style, and visual-style differences.
The portal comparison uses the trigger's `aria-controls` reference, and both
popup tops were measured 4px below their triggers. Reports:
`/tmp/select-escape-single-report.json` and
`/tmp/select-popup-current-single-report.json`.

The combined fixture is still intermittently blocked: its latest run passed
the closed state but timed out in Escape and popup-open capture (1/3 passed,
2 blocked). Do not treat the isolated runs as proof that the combined browser
gate is stable. A real browser click on the rebuilt Blazor docs still opens the
popup, focuses the input, and leaves the root with both `ant-select-focused`
and `ant-select-open`. The fixture and runner diagnostics need further work
before Select's first-open automation can be considered fully settled.

## Dropdown Semantic DOM preview and demo inventory (2026-11-03)

The live React/Blazor route inventory found 22 React `.code-box-demo` scopes
and 23 Blazor scopes, with 21 matching names and two Blazor-only examples. The
only React-only name, `_semantic`, is React's separate Semantic DOM preview;
Blazor renders its Semantic DOM section outside `.code-box-demo`, so it is not
a missing public example. `DropdownSemanticPreview` uses the actual open
Dropdown, grouped and nested menu entries, icons, divider, danger item, and a
local popup container. Browser verification confirmed the expanded menu is
rendered in the Semantic DOM section and clicking the `item` selector
highlights the menu items. Ordinary matched demos currently show 21 normalized
HTML-tree differences, 9 selected computed-style differences, and 9 geometry
differences; follow-up should prioritize those runtime differences. Full local
inventory: `/tmp/dropdown-parity-followup-2026-11-03.json`.

## Select and Icon recheck (2026-11-01)

Fresh serial browser runs against the current React v6 docs (`:8000`) and the
current Blazor docs (`:5000`) pass Icon 5/5, Select size 3/3, Select interaction
3/3, Select Semantic DOM 2/2, and Select suffix 1/1. These runs report zero
DOM-tree, computed-style, Semantic DOM or visual-style differences. Select Big
Data also passes the DOM-tree, computed-style and visual-style gates; its byte
HTML gate differs only in Razor whitespace/renderer serialization.

Latest Select follow-up: the interaction fixture showed the focused class was
not refreshed when the input gained or lost focus. Select now refreshes its
class map on both transitions; the next interaction run passed visual styles
3/3, including the blue focus border and shadow. It also isolated
`aria-disabled="false"` on React's hidden virtual accessibility options as a
DOM difference; that attribute is now emitted only for disabled hidden
options, while visible Select options keep their explicit false value. After
this change, the final interaction run passed the unmatched-value clear flow
with zero differences; Search and Multiple were blocked by browser-runner
timeouts (Blazor click timeout and React roots not found), so the full
interaction fixture still needs a reliable rerun. Report:
`/tmp/select-interaction-verified.json`.

### Fresh targeted rerun (2026-11-02)

With React docs running at `:8000` and the rebuilt Blazor docs at `:5028`, fresh serial parity runs pass Icon 5/5 (IconFont, resources, basic, two-tone, custom), Select Basic/popup 3/3, Select Semantic DOM 2/2, and Select suffix 1/1; each reports zero DOM-tree and visual-style differences. The first Select interaction run found an actual Blazor render exception on the page: `TreeSelect<T>` declared `DropdownStyle` a second time even though it inherits that parameter from `SelectBase`. Removed the duplicate parameter and redundant assignments, rebuilt, and confirmed the Select page now renders through the later demos. The rerun passes all three Select interactions (search filtering, multiple selection, and unmatched-value clear), with zero DOM-tree, computed-style, or visual-style differences and no blocked cases. Report: `/tmp/select-interaction-after-treeselect-fix.json`; the fresh reports are local verification outputs, not committed fixtures.

An additional serial recheck of the interaction manifest on 2026-11-01 passed
the unmatched-value clear flow and found identical DOM trees, computed styles,
and visual styles after multiple selection (the only strict failure is raw HTML
whitespace/serialization). Its earlier search-filter blocker was the missing
focus class described above; subsequent visual checks confirm that repair.

The Select Basic popup's automated portal capture is still intermittent: one
run passed and another captured no visible Blazor portal, so that fixture is
not yet considered reliable. I opened the current React and Blazor Basic demos
in the browser and measured the popup relative to its trigger on both pages:
both are 120×136px, both use the floored trigger x-coordinate (`339px`), and
both popup top edges are `379.5px`. The Blazor trigger is at y=`343.914px` and
the React trigger at y=`344.031px`, a `0.117px` docs-flow difference. The
trigger-relative gap therefore differs by the same fraction. The popup
collector remains an open verification task before declaring Select complete.

## Select and Icon checkpoint (2026-10-31)

Rebuilt the documentation app from the current worktree and verified against
the React v6 docs at `http://127.0.0.1:8000` and the rebuilt Blazor docs at
`http://127.0.0.1:5012`. Select passes 9/9 focused scenarios: size/basic (3),
search/multiple/unmatched-value interactions (3), Semantic DOM closed/open
(2), and Big Data virtual options (1). Icon passes all 5 focused scenarios,
including IconFont, custom icons, and two-tone rendering. Every current run has
zero DOM-tree, computed-style, semantic-style, geometry, or visual-style
differences.

The fresh Big Data run exposed a real accessibility markup mismatch: enabled
virtual Select options omitted `aria-disabled`, while React emits
`aria-disabled="false"`. The virtual option renderer now emits explicit
`true`/`false`; the rebuilt app then passed the same Big Data comparison. The
earlier Select report with six strict differences came from a stale host and
comparison fixture; it is superseded by these fresh 5012 reports.

The rebuilt Blazor Select page remains open in the in-app browser at
`http://127.0.0.1:5012/en-US/components/select` for manual review. The current
full-component inventory still predates this checkpoint (2026-10-30); do not
interpret its cross-component runtime totals as refreshed by these focused
runs.

## Icon, Select and Typography checkpoint (2026-10-30)

The focused Icon browser manifest passes all 5 scenarios, including IconFont,
custom icons and two-tone rendering; computed-style and visual-style gates also
pass 5/5. IconFont's default accessible name was the actual DOM mismatch: Blazor
inferred an `aria-label` from the glyph type while React's
`createFromIconfontCN` wrapper does not. Blazor now forwards only an explicitly
supplied label for IconFont.

Select's focused size/basic manifests pass 6 scenarios. The suffix scenario
passes separately against the active Blazor server on port 5002 (1/1, zero DOM
or style differences). A previous combined report was misleading because it
included a blocked suffix capture against a stale server on port 5000. The
runner now scrolls the outer lazy demo anchor before waiting for its inner
`.code-box-demo`; this fixes the actual timeout cause. The virtual Big Data
scenario remains outside this focused result because its full-page browser
capture is slow; it must be audited separately rather than holding up every
component batch. React's 36 Select demo names are all present among the 80
Blazor demo names; the extra Blazor demos are not counted as missing coverage.

Typography previously had no Blazor demos. Its first parity batch now adds
Basic, Title Component, and Text and Link Component examples in both docs
locales, matching React demo names. Editable, copyable, ellipsis, suffix,
component-token and table examples remain to be migrated and audited.

The comparison contract is React-first: React routes and scenarios define the
reference, while Blazor is measured against them. A case can carry interaction
steps and the React/Blazor selector pair for each semantic node.

## Comparison gates

The JSON report deliberately keeps four gates separate:

- `htmlExact`: byte-for-byte HTML comparison after only manifest-declared
  runtime normalization. Attribute order and whitespace remain significant.
- `domTree`: ordered tag, text, attribute and child comparison. This gate is
  diagnostic and does not replace `htmlExact`.
- `semantic`: required Semantic DOM paths, occurrence counts, exact node HTML,
  and computed CSS for each corresponding path.
- `computedStyles`: exact values for the requested CSS properties, including
  component CSS variables and measured dimensions.
- `visualStyles`: an independent pass/fail summary for computed CSS, requested
  Semantic DOM styles and required Semantic DOM counts. It remains visible when
  the stricter HTML/tree gate fails for framework-specific markup differences.

Only per-render IDs, the exact Blazor `<!--!-->` renderer marker, and Blazor
renderer marker attributes are normalized in the Select pilot. The comment
marker is removed only because the manifest explicitly declares it. ID references are remapped consistently so relations such as
`aria-controls` are still checked. CSS-in-JS implementation classes are listed
explicitly in that manifest; Ant Design semantic classes are not removed.

The browser runner scrolls to each demo and waits until its Blazor Skeleton is
gone and the expected number of component roots exists. A timeout is reported
as `blocked`, never as a DOM pass or a skip. It uses the same Chromium build,
viewport and device scale for both applications.
When a demo container's width differs only because the documentation shells
have different gutters, a group may declare `matchScopeWidth: "react"`; the
runner then applies the React demo-scope width to the Blazor demo scope before
both sides are measured. This is reported as a comparison-fixture adjustment,
not a component CSS change.
Scenario steps support click, hover, focus, fill, keyboard press, transition
waits and viewport resize actions. Hidden native controls can use the manifest's
`force: true` click option, which dispatches their native DOM click event so
framework handlers still execute without assigning component state directly.

## Select pilot

Run with the React repository and both local documentation hosts available:

```bash
node tools/AntDesign.StyleGen/src/dom-style-parity-runner.mjs \
  --cases tools/AntDesign.StyleGen/examples/select-dom-style-parity.json \
  --antd-path /Volumes/Data/GitHub/ant-design \
  --chrome "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome" \
  --report /tmp/select-dom-style-parity.json
```

The manifest covers the Select size demo at Default, Large and Small, including
single, multiple and tags roots; content, input, item and suffix Semantic DOM;
and computed box dimensions, typography, borders and Select CSS variables. Each
Semantic DOM path has separate React and Blazor selectors plus a minimum count,
so wrapper differences cannot silently make the comparator inspect different
nodes.

The Transfer Basic demo has its own browser manifest. It checks both sections,
the actions, headers, bodies, lists, all 20 items and their text nodes, including
computed dimensions, borders, typography and Transfer CSS variables. This case
was added after a source-only bUnit class test passed while the already-running
Blazor server still served the old v5 list/operation classes. The browser CSS
selectors therefore did not match. Rebuilding and sampling the running page
exposed the actual mismatch. After updating the directional v6 icons and
omitting empty section `style` attributes, the normalized ordered DOM tree is
equal and the computed-style gate has zero differences. The raw HTML byte gate
still differs on Razor whitespace/comments, which are framework serialization
details rather than element, text, attribute, or style mismatches. The fixture
normalizes per-render IDs and the Checkbox CSS-in-JS marker, while preserving
all meaningful DOM attributes and the separate CSS checks. A browser
interaction also moved one item from source to target (11/9 became 10/10).

The browser fixtures are still a small pilot, not a claim of component-wide
coverage. Before this Transfer case, they covered only Button, Breadcrumb,
Select and Tooltip. A component with no browser fixture has not been visually
verified by this gate, even if its source classes or bUnit structure tests pass.

### Overlay coordinate contract

Portal captures retain the popup and trigger `getBoundingClientRect()` values in
viewport coordinates. When the React and Blazor triggers occupy the same page
coordinates, `geometryAnchor: { "mode": "viewport" }` compares popup edges and
dimensions with zero tolerance. When demo-page flow places the corresponding
triggers in different columns, `pixel-snap-to-trigger` checks the actual
placement contract (including Select's four-pixel vertical gap) with zero
tolerance; each side's absolute viewport coordinates remain available in the
report. Child portal slots are measured against their nearest matching overlay,
not another portal elsewhere on the page.

The Select Basic popup is exactly at `(339, 470)` with size `120 × 136` on both
pages. In the Big Data demo, the trigger is at `x=339.65625` in React and
`x=748.3125` in Blazor because the docs arrange surrounding demos differently;
both popup left edges are the trigger's floored pixel (`339` and `748`), with
the same bottom placement. Moving either popup to force matching page-global
coordinates would detach it from its trigger. Tooltip's visible popup, arrow and
content also pass zero-tolerance coordinate checks in the same viewport. The
shared `Overlay` placement tests cover menu-style right alignment, viewport
flipping, scrolling and fractional Select anchors.

Overlay idempotence was additionally checked across the Select's fast client
open followed by the Blazor Server render. That render can replace the Overlay
instance while the enter scale motion is running; the replacement now detects
active Web Animations and keeps the same final-pixel alignment policy as the
pre-motion calculation. The motion-end recalculation therefore reproduces the
same coordinate instead of shifting the popup by a pixel. Select's popup
geometry matches React exactly (`(339, 470)`, `120 × 136`), and the Tooltip
portal gate passes 2/2 scenarios with zero visual-style differences. The
DatePicker now waits for the visible popup nearest the clicked picker, avoiding
the stale offscreen calendars mounted by other Dumi examples. Its popup root
matches the React geometry relative to the trigger and the component styles
(`display: block`, z-index 1050, `288 × 348`). The remaining DatePicker
differences are outside the overlay root: the picker trigger is `inline-flex`
instead of React's `flex`, and footer controls have small position/size
differences. Those remain open; this is not a full DatePicker pass.

### Collapse first-open behavior (2026-10-27)

The Collapse Basic and Collapsible cases pass live React/Blazor DOM-tree and
computed-style comparison (2/2, zero reported differences). The Basic case
previously showed closed panel bodies in Blazor before they had ever opened;
panel bodies now mount on first activation, and the JS motion helper waits for
that hidden body before starting the client-side height transition. Focused
Collapse tests pass 14/14 and the helper passes TypeScript compilation.

The separate Semantic preview case is not a valid direct React selector pair:
`semantic-mark-root` is a Blazor-only class applied by the preview fixture, so
the React page has no matching node and the runner correctly reports that case
as blocked. It is excluded from the 2/2 parity result until the fixture maps
both sides to equivalent rendered components. Browser interaction against the
Blazor Server page initially still loaded the previous generated JavaScript
bundle. Running the interop Gulp task directly (`gulp --gulpfile
scripts/gulp/tasks/interop.ts ts`), copying the bundle to the local static assets
and restarting the host made the browser test pass: clicking panel 2 mounted
and expanded its content, while unopened panel 3 remained unrendered. The
combined `library:scripts` task still fails while loading `library.ts`, which
imports three missing legacy style-build modules; that legacy task wiring
remains to be repaired.

## Scaling this to all components

1. Generate cases from React Jest tests and Dumi demos, retaining source file,
   test key, render inputs, interaction timeline and expected assertions.
2. Reuse the generated Semantic DOM inventory to require every declared path
   and selector. Add a component adapter only when React props/events cannot be
   mechanically represented as Blazor parameters/events.
3. Group cases by route so each React and Blazor page is loaded once, then run
   scenario resets and interactions against both pages.
4. Aggregate failures by shared selectors, style variables and infrastructure
   component (for example Overlay) before scheduling fixes. This makes one
   shared fix close many component cases.
5. Keep bUnit for fast render/interaction contracts; use the browser runner for
   computed styles, pseudo-elements, portals, responsive states and keyboard
   behavior that bUnit cannot measure.

Raw HTML equality is a strict diagnostic target, but it is not by itself a
portable cross-framework contract: generated IDs and renderer markers differ.
The normalized strict gate and Semantic DOM/computed-style gates show precisely
which differences are implementation noise and which still affect consumers.

## Whole-site inventory checkpoint (2026-10-29)

The current React-first runtime inventory is saved in
[`runtime-parity-scan-2026-10-29-visible.json`](runtime-parity-scan-2026-10-29-visible.json).
It pairs 71 component routes and compares 1,133 React demos against the live
Blazor docs host. React demo names remain the deficit baseline: 292 names are
not present in Blazor, while 212 Blazor-only names are reported separately.
The 841 matched demo captures contain 743 normalized HTML-tree differences,
582 computed-style differences and 546 geometry differences. These are
triage counts, not completed fixes or a visual pass.

Four routes are incomplete captures: App has no `.code-box-demo`, and Button,
InputNumber and Upload report `define is not defined`. Those rows remain
blocked and must be rerun after the docs runtime/build issue is addressed.
Checkbox and Flex were recaptured after their latest changes. React demo names
are now complete for both pages (Checkbox 12/12, Flex 6/6). Their runtime
differences remain visible in the inventory for follow-up.
Select's Placement demo default was changed from `BottomLeft` to React's
`topLeft`. A post-build targeted scan was not accepted as evidence because it
captured the Blazor demo's lazy-load skeleton instead of the rendered controls;
the Select row in the inventory therefore remains the previous valid capture
until a hydrated recapture is available.

The generated phase-one audit separately confirms 72/72 style components,
3/3 algorithms, 423/423 Semantic DOM slots, 65/65 component token pages and
584/584 component token rows. It reports no token metadata parity mismatches.
This generated coverage does not establish browser runtime parity.

## Select and whole-site inventory checkpoint (2026-10-21)

Reran the Select size manifest against React `127.0.0.1:8000` and the rebuilt
Blazor Server `127.0.0.1:5021`. Default, Large and Small all pass the visual
style gate: root and Semantic DOM computed CSS and geometry have zero
differences, and all required node counts match. The strict DOM gate still has
four differences per scenario, all in the `d` attribute of the two selected
tag close icons. React's `@ant-design/icons-svg` build rounds the source SVG
path to two decimals; the Blazor `AntDesign.Icons` generator currently preserves
the raw submodule SVG precision. This remaining difference belongs to the icon
generation pipeline, not Select layout or CSS.

The manifest now explicitly strips React CSS-in-JS hash classes (`css-var-*`
and `css-*`) from tree comparison. They are implementation hashes; their
rendered styles are still compared separately. This reduced the reported Select
tree differences from seven to four without suppressing any Ant Design semantic
class, attribute, text or computed-style comparison.

The full documentation inventory was rerun against the current React and
Blazor hosts. It found 68 paired routes, 881 React demos, 952 Blazor demos and
225 unmatched React demo names. However, 41 pages were marked blocked because
the React host emitted minified error #418 or a page lacked a `.code-box-demo`;
therefore the raw unmatched count is a triage signal, not verified missing-demo
work. Resolve those inventory blockers before using the total as a completion
gate. The raw report is `/tmp/antd-v6-doc-parity-2026-10-21.json`.

### Splitter mixed-size follow-up (2026-10-21)

Removed Blazor-only `data-resizable`, `data-min`, and `data-max` panel
attributes; the interop now receives panel options directly. Panel inline styles
use React's `flex-basis`/`flex-grow` pair, the stale `spliter-bar` class is
removed, and separator `aria-valuenow/min/max` is calculated from panel sizes
and constraints. Initial percentage sizes follow React's `autoPtgSizes`
allocation. The Basic demo now includes React's `max="70%"`; the responsive
Mixed sizes demo scales the second size bucket's 300/100/600px constraints to
30%/10%/60% so its layout remains usable inside the narrower Blazor code box.
The bucket switch remounts Splitter so JS listeners and constraints follow the
new panels.

In the running browser, the 327px Blazor demo renders bucket 1 as 100/113.5/113.5px
and bucket 2 as 98.1/32.7/196.2px; bucket 2's separator ARIA values are 30/30/30
and 40/40/40, matching React's 30/10/60 proportions at its 1000px demo width.
A pointer interaction in bucket 1 resized the adjacent panels while respecting
their min/max limits. Splitter bUnit tests pass 12/12 and the .NET 10 docs build
passes. Exact whole-tree parity remains pending a stable runner capture; the
Puppeteer run lost its CDP connection during teardown, so no runner pass is
claimed. The browser remains open on the Mixed sizes demo for manual inspection.

## First browser pilot status (2026-09-24)

The Select size pilot now mounts three roots on both sides. `Routes` uses
`InteractiveAuto`; the browser check found no duplicate `blazor-ssr-end`
elements, and the hidden size radios were changed through native click events.
The runner also forces `prefers-color-scheme: light` for both pages, preventing
the host machine's dark preference from contaminating computed-style results.

The Blazor Size demo was corrected to derive option values using React's
base-36 rule and to use the same `a1`, `a10`, and `c12` selection values. Its
vertical spacing now comes from `Space`, not margins on the Select roots. The
browser action also asserts that both radios reach the requested checked value
before taking a snapshot.

Latest run: 3 scenarios, 0 fully passed, 3 with parity differences, 0 blocked.
All five declared Semantic DOM paths meet their required counts in every
scenario (root 3/3, content 3/3, input 3/3, item 4/4, suffix 3/3). The
unmatched single value `a1` now renders as a text node with `title="a1"`,
matching React's selected-value semantics; a focused bUnit regression test
covers this behavior. The Blazor demo now initializes this case with `Value`
like React's initial `defaultValue`, instead of simultaneously binding a value
and setting a default.

Remaining parity findings: the ordered DOM tree has 37 differences per
scenario (previous baseline: 52). Select now uses the v6 single content/input
structure, direct multiple content items (no legacy selection-overflow wrapper),
text inputs, and a `div.ant-select-suffix`; the multiple/tags control height is
now 32px in the browser comparison. Exact raw HTML remains different. Root
computed-style differences are 2 (Default), 15
(Large), and 12 (Small), versus 4/17/20 in the first valid run. The semantic
suffix styles now match with 0 differences in all three sizes: Multiple/Tags
were incorrectly suppressing the default arrow, which left empty suffix spans
with zero dimensions. The overall semantic-node style differences remain 12
(Default), 57 (Large), and 38 (Small); Default content styles contribute 6 of
those differences. Remaining work is primarily theme typography/size parity
for Large/Small and small item/root width deltas.

Verification for this pass: all Select bUnit tests passed (73/73), the StyleGen
suite passed (70/70), the docs WebApp build succeeded (0 errors; 423 warnings),
and `git diff --check` passed.

The React/Blazor name-overlap matrix covers 16 Select Dumi demos. The Blazor
root class order now matches React's stable class order (`ant-select`, variant,
CSS-variable scope, mode, state); runtime-only hash/dev classes remain
intentionally normalized by the comparator. Basic (4/4 roots), Responsive
(3/3 roots, React base-36 values and the third Tooltip/maxTagPlaceholder case),
BigData (1/1 root with 100,000 options), and Suffix (6/6 prefix/suffix,
single/multiple cases) now render the React-overlap scenarios. The Suffix demo
was Debug-only even though React v6 exposes it; it is now visible. Its source
and English/Chinese metadata were refreshed from the Razor demo because the
full `demo2json` command currently fails while reading malformed XML comments
elsewhere in the component assembly.

Latest full matrix after converting single selected labels from the extra
`.ant-select-selection-item` span to React-compatible text: 16/16 groups
rendered, 16/16 root counts match, 77/80 Semantic DOM path counts match, 341
ordered-tree differences, 649 semantic computed-style differences, 0 blocked,
and 0 exact DOM/style passes. The remaining Semantic item-count differences
are Responsive (React 2 / Blazor 11), BigData (2 / 1), and Tags (0 / 1); the
other four paths (root/content/input/suffix) match counts in every group. The
14 other Blazor Select demos remain outside this same-name React overlap and
need upstream mapping before parity claims.

Select bUnit coverage now passes 75/75, including regressions asserting plain
text for a selected single value and the `ant-select-content-has-value` class,
and that a disabled option supplied as an initial multiple value remains
selected.
The latest browser report was collected with React on `127.0.0.1:8000` and
Blazor on `127.0.0.1:8182` (metadata API on `8181`). The older isolated host
still logs 404s for the WASM boot manifest and generated WebApp styles bundle;
these do not prevent the interactive-server markup capture but need a
separate host-assets fix before relying on WASM Auto mode in production.

### Select Size recheck (2026-10-21)

After rebuilding the docs Server and WASM hosts against the locally generated
`AntDesign.Icons 0.2.0-beta.3-local.22` package, the Select Size browser case
was rerun against React `127.0.0.1:8000` and Blazor `127.0.0.1:8182`.
Default, Large and Small each mount three roots on both sides. All five
declared Semantic DOM paths (root/content/input/item/suffix), their requested
computed styles, root styles and measured geometry match exactly; the visual
style gate passes 3/3 with zero CSS differences. The normalized ordered DOM
tree also matches exactly in all scenarios. The byte-for-byte HTML diagnostic
still differs only at Blazor's renderer whitespace/indentation around child
markup, so these are reported as strict serialization differences rather than
Select structure or styling regressions.

The runner now explicitly foregrounds each app before width measurement and
capture, and rechecks demo readiness immediately before sampling. This avoids
intermittent missing-scope failures when React's docs page is backgrounded
while Blazor's interactive circuit hydrates. The StyleGen parity test suite
passes 24/24; `git diff --check` is clean.

### Button, Checkbox, Radio and Rate browser batch (2026-10-21)

Ran the seven available Basic/Group/Radio-button/Half-rate cases against the
live React and Blazor hosts (the checked-in manifests still referenced stopped
ports, so only temporary copies were retargeted to the current hosts). Button,
Checkbox and Radio already had matching computed styles. Both Rate cases
exposed a real 2.5px height excess: the legacy documentation shell overrode
the v6 `.anticon` box from `inline-flex` to `inline-block`, enlarging the star
line box from React's 20.5px to 23px. Added the v6 `inline-flex`/`align-items`
behavior to the token-theme parity stylesheet. The live Rate root now measures
20.5px, and the complete seven-case batch passes its computed visual-style gate
(7/7, zero style differences). Parsed tree/HTML diagnostics remain separate;
framework-only attributes, renderer comments and generated classes are not
misreported as CSS failures.

## Whole-site inventory and Tooltip portal pilot (2026-09-26)

The documentation inventory now waits for bounded DOM stability after initial
navigation and reports per-route progress. The stable scan paired 68 React
component routes with Blazor routes (73 React routes and 71 Blazor routes in
total; the four user-deferred React components are FloatButton, Masonry, Tour,
and BorderBeam, plus React's internal `_util`). Blazor has three additional
routes: PageHeader, Comment and BackTop. Two pages without code-box demos
(App and ConfigProvider) are reported as blocked rather than missing.

Across the 68 paired routes, the scan found 1,098 React demos and 1,215 Blazor
demos; 398 React demos have a same-name Blazor demo ID, leaving 700 React demo
IDs without a match. This is an inventory signal only: ID overlap does not
prove DOM/CSS parity, and unmatched IDs can include debug/semantic demos or
different casing/naming. The full inventory JSON was saved to
`/tmp/antd-v6-doc-parity-inventory-stable.json` for triage.

### Inventory rerun with route-isolated browser pages (2026-10-21)

The inventory runner now opens and closes a fresh browser page for each React
and Blazor route. This prevents a detached frame on one documentation route
from cascading into false zero-demo results for every later route. Regression
tests cover page cleanup on both success and failure. The current scan still
pairs 68 routes and reports 881 React demos, 952 Blazor demos, 656 same-name
matches, 225 unmatched React IDs, and 296 Blazor-only IDs. These counts include
Semantic DOM preview entries and are a name inventory, not a parity verdict.
React hydration errors block reliable demo extraction on 41 routes, so those
routes must not be interpreted as complete or as having no missing demos. The
scan report is `/tmp/antd-v6-doc-parity-current.json`.

Added a Tooltip Basic browser fixture that captures overlay content rendered
in `document.body`, waits for the visible tooltip after hover, and checks the
arrow Semantic DOM. The Blazor Basic demo content was aligned to React's single
`prompt text` example. The CodeBox demo text now uses `var(--ant-color-text)`
instead of a v5 `.85` literal, so it follows the active v6 theme. Latest report:
2 scenarios, 1 passed, 1 with differences, 0 blocked. The initial trigger now
passes exact HTML, DOM tree, Semantic DOM and requested computed styles. After
hover, React and Blazor both expose one tooltip, but remaining differences
include the trigger's `ant-tooltip-open`/`aria-describedby`, the popup/arrow
inline position styles, popup display/animation classes, and a 2.25px
tooltip-content width delta. These are genuine parity failures, not
portal-sampling noise, and are the next Tooltip repair targets. The report is at
`/tmp/tooltip-basic-portal-parity.json`.

### Select follow-up (2026-09-24)

The React `big-data.tsx` demo declares `defaultValue={['a10', 'c12']}` while
`a10` is disabled. Blazor now retains disabled items that are present in an
initial/default value (they remain unavailable for new user selection), with
a bUnit regression test. The React `tags.tsx` demo starts with no selected
value; the Blazor Tags demo no longer seeds `0f15`.

The runner previously captured React while its Puppeteer page was in the
background. Chrome throttled React's animation-frame/ResizeObserver layout,
leaving a temporary overflow node in the report. Activating the pages
sequentially captures React's measured state; Blazor also needs a short wait
for its JS size measurement to return through interactive rendering.

The Responsive mismatch was the custom `MaxTagPlaceholder` wrapper: React
renders the template under `.ant-select-selection-item >
.ant-select-selection-item-content`, while Blazor previously emitted only the
template. The wrapper is now present in both fixed-count and responsive
placeholder branches, covered by a bUnit regression. In the latest full run,
all 16 groups rendered, root counts matched 16/16, and Semantic path counts
matched 80/80. The matrix still has 375 ordered DOM-tree differences, 121
Semantic DOM computed-style differences, and 50 root-style differences; 0
blocked and 0 fully passing groups. Select bUnit is 76/76; the focused parity
runner tests pass 8/8. Select remains in progress because the broader DOM and
style differences are not yet resolved.

The report was collected with React on `127.0.0.1:8000` and Blazor on the
isolated test host at `127.0.0.1:8181`; the committed manifest defaults Blazor
to the docs host on `8182`. The isolated host still logs 404s for the WASM boot
manifest and generated WebApp styles bundle; they do not prevent this pilot's
current interactive-server markup capture, but need a separate host-assets fix
before relying on WASM Auto mode in production.

### Select demo fixture alignment (2026-09-24)

Aligned the four demo inputs flagged by the matrix: Label-in-value selects
Lucy (101), Coordinate uses React's 120px widths and no extra clear buttons,
Custom Dropdown uses 300px, and Select Users now starts in React's multiple,
searchable, label-in-value state with no selection or loading indicator. The
Select Users comparison confirms its root mode and width now match; its
remaining difference is the multiple-mode placeholder DOM wrapper, which is a
component markup issue rather than a demo parameter mismatch.

After rebuilding the docs site and rerunning all 16 groups, there are 0
blocked groups, root counts match 16/16, and Semantic DOM path counts match
80/80. Totals improved to 374 ordered DOM-tree differences, 90 Semantic DOM
computed-style differences, and 43 root-style differences. Label-in-value,
Coordinate, and Custom Dropdown now have zero computed-style differences;
Select Users' style differences fell from 29 to 3 and its root mode/width now
match. Its placeholder wrapper remains a component markup issue. Overall
exact-pass count remains 0/16.

### Empty multiple placeholder slot (2026-09-24)

Moved the empty multiple/tags placeholder into React's
`.ant-select-content-item.ant-select-content-item-prefix` wrapper, before the
suffix input, with a nested `div.ant-select-placeholder`. A new bUnit structure
regression first failed because the prefix slot was absent, then passed after
the markup change. Select bUnit now passes 77/77.

The refreshed 16-demo matrix still has 16/16 roots and all 80/80 Semantic DOM
path counts aligned, with 0 blocked and 0 exact-pass groups. Ordered DOM-tree
differences fell from 374 to 347; Select Users fell from 12 to 6 tree
differences. Semantic computed-style differences remain 90 and root-style
differences remain 43. Select Users now has two Semantic style differences
and one root style difference; its placeholder-slot ordering/structure is no
longer among its tree differences. The StyleGen parity tests pass 8/8 and
`git diff --check` passes.

### Multiple input sizing and suffix ordering (2026-09-24)

Aligned the empty multiple-mode input with React's `--select-input-width: 0`
CSS variable and let the v6 stylesheet provide the minimum width and prefix
spacing. When no values are selected, the suffix wrapper now omits React's
absent inline `order`; selected/responsive suffix ordering remains unchanged.
The bUnit regression asserts both the inline variable and suffix style.

The 16-demo browser matrix now reports 343 ordered-tree differences (down from
347), with Select Users down from 6 to 5. All 16 root counts and 80/80 Semantic
DOM path counts still match; semantic style differences remain 90, root style
differences remain 43, and 0/16 groups are exact passes. Select bUnit passes
77/77, the parity test suite passes 8/8, and the docs WebApp builds with 0
errors.

### Select input and root accessibility attributes (2026-09-24)

Removed the duplicate input id and non-React `id`/`tabindex` from the Select
root. Both modes now give the actual input the Select id, use React's
`autocomplete="new-password"`, serialize `aria-expanded` as an explicit
boolean string, and only render `aria-owns`/`aria-controls` while the popup is
open. Empty content no longer emits a blank `style` attribute. Regression
assertions cover the closed-state markup.

After restoring the local React Dumi server (it had exited during the first
matrix attempt), the complete matrix rendered all 16 groups with no blocked
scenarios. Ordered DOM-tree differences fell from 328 to 312; the Select Users
input now matches the measured 4px CSS-variable width, autocomplete, expanded
state, and list association behavior. Counts remain aligned at 16/16 roots and
80/80 Semantic DOM paths. Semantic style differences remain 90, root style
differences 43, and exact passes 0/16. Select bUnit passes 77/77, parity tests
8/8, and the docs WebApp build succeeds with 0 errors.

### Select down-arrow SVG markup (2026-09-24)

Replaced the library-wide legacy `Icon Type="down"` rendering inside the Select
suffix with the React-compatible DownOutlined DOM: `.anticon` wrapper with
`role="img"`/`aria-label`, the 64/64/896/896 viewBox, `data-icon="down"`, and
the matching path. This remains scoped to Select, leaving other legacy icon
consumers untouched. A bUnit regression asserts the wrapper and SVG attributes.

The refreshed matrix still renders all 16 demos without blockers. Ordered
DOM-tree differences fell from 312 to 266; Select Users now has one remaining
tree difference (the empty `value` attribute). Semantic path counts remain
80/80, Semantic style differences 90, root style differences 43, and exact
passes 0/16. Select bUnit passes 77/77, parity tests 8/8, and diff validation
passes. The React Dumi server was restarted locally for this run.

### Select multiple-search default alignment (2026-09-25)

React v6 documents `showSearch` as false by default for single mode and true
for multiple mode. Blazor now uses that same default for multiple/tags while
preserving `EnableSearch` as the existing single-mode opt-in. A focused bUnit
test checks the search class and input `readonly` state for all three modes.
The test harness now provides no-op defaults for Select's keyboard-prevention
interop, since searchable multiple selects register those handlers on first
render.

Verification: Select bUnit tests pass 78/78 on .NET 10; the docs WebApp builds
on .NET 10 with 0 errors. The three browser scenarios remain differences, not
passes: ordered DOM-tree differences are 27/30/30 (Default/Large/Small), root
computed-style differences 2/15/18, and declared Semantic DOM paths still
have computed-style differences. Root counts remain 3/3 on both sides. The
React-compatible `ant-select-show-search` class mismatch on multiple controls
is resolved. Remaining Select work includes the legacy remove-icon subtree and
dimensions/style differences; exact HTML parity is not claimed.

### AntDesign.Icons package integration (2026-09-25)

Integrated `AntDesign.Icons` `0.2.0-beta` as a component-library dependency and
replaced Select's old inline icon components for Down, Close, Search, Loading,
and Clear with generated icon components. NuGet restore succeeds. Select tests
pass 78/78 on .NET 10, and the docs WebApp builds with 0 errors.

The browser comparison is still 0/3 exact passes. Compared with the run before
switching the icons, DOM-tree differences increased from 27/30/30 to 36/39/39;
computed-style differences remain 2/15/18, and Semantic computed-style
differences remain 8/53/58. The added DOM differences are primarily generated
SVG attributes/classes that differ from React's rendered icon markup, not a
change to component box styling. Root counts remain 3/3. This is an explicit
tradeoff to consume the published icon package; exact SVG-level parity still
needs either a compatible icon-rendering API or upstream package markup aligned
to React v6.

### Select parity follow-up (2026-09-25)

Fixed the browser harness so each app is brought to the foreground before
interactions and captured separately after transitions settle. The previous
large/small measurements were false positives: React's class and CSS variable
had changed, while background-tab throttling left its computed size mid-transition.
The corrected comparison shows Select size tokens and component box styles
match apart from width inherited from the different documentation demo-column
widths. The 8 remaining semantic computed-style differences are width-only
(2 content, 4 selected-item, 2 root measurements); both sides still render
three roots and all required semantic-node counts.

Aligned more v6 markup details: multiple Select no longer gets the single-value
`content-has-value` class, selected tags expose their label as `title`, fixed
multiple mode does not assign an unnecessary search-slot `order`, and readonly
single inputs no longer carry a legacy inline `opacity: 0`. Blazor event
propagation now uses the framework event modifier rather than emitting a literal
`onclick` attribute. Select tests pass 81/81; the docs WebApp builds with 0
errors.

The corrected browser report is 24 ordered-tree differences in each of the
Default/Large/Small scenarios (down from 36/39/39 immediately after the icon
package integration). Most remaining differences are the published icon
package's SVG markup/paths and Blazor's serialization of empty input `value`;
the package uses a 1024 viewBox, unlike React v6's 896 viewBox. DOM exact-pass
remains 0/3. The two root-width and six semantic-width differences come from
the React and Blazor documentation demo containers having different available
widths, not from Select's sizing tokens.

### Button Basic and Select follow-up (2026-09-26)

Button's default text wrapper now matches React's unclassed `<span>`; the
Semantic DOM preview locates that slot structurally instead of relying on the
Blazor-only `.ant-btn-content` class. The internal
`ant-click-animating-without-extra-node` marker is omitted at rest and emitted
only while its animation state is active. Button focused tests pass 23/23.

Added a reusable Button Basic browser manifest and separated semantic subtree
comparison into parsed DOM-tree parity plus raw HTML diagnostics. At 1280px,
all five buttons have identical parsed DOM, declared Semantic DOM subtrees,
and computed styles (0 tree/style differences). Raw HTML is not byte-identical
because Blazor serializes attribute order and indentation differently; this
remains an explicit lexical mismatch, not a hidden pass.

Breadcrumb Basic had a component-level structural mismatch: Blazor nested the
separator span inside every item, including the last one, whereas React renders
each separator as its own `li[aria-hidden="true"]` between items. Breadcrumb now
renders that sibling list item only before non-first entries. Its Basic demo
also now has the same four labels as React (the extra fifth “A Page” was removed).
Breadcrumb bUnit tests pass 7/7. The browser run now has four React/Blazor items,
three separators, identical parsed DOM, Semantic DOM and computed styles (0
structural/style differences); raw HTML remains non-byte-identical due to
Blazor's serialization whitespace and attribute ordering.

The current Select Size run (Default/Large/Small, three roots each) has zero
computed-style differences, including the selected Semantic DOM styles. Four
DOM-tree differences per scenario remain, all in the two Clear icon SVG path
`d` strings: React emits rounded path coordinates, while the generated Blazor
icon package keeps higher precision. Raw HTML also differs in renderer-added
whitespace. The Icon geometry is visibly equivalent, but exact SVG/byte parity
is not claimed.

Parity unit tests pass 11/11; Button tests pass 23/23; Breadcrumb tests pass
7/7; the .NET 10 Docs WebApp build succeeds. Current phase-one inventory is
CSS components 72/72, themes 13/13, standard algorithms 0/3, Semantic DOM
catalog components 59/59, API definitions mapped 68/68 (structure migration
52/68), API slots 370/370, documented Dumi paths 350/350, Theme Preview 1/1,
Semantic DOM Preview 1/1.

### Select Basic loading icon follow-up (2026-10-21)

The Select Basic demo's loading state was still rendered by the legacy generic
`Icon` component while its arrow/clear icons already used generated v6 icons.
It now uses `LoadingOutlined Spin`, matching React's `anticon-loading
anticon-spin` accessible markup. The browser Basic demo shows all four Lucy
values at 32px; default/disabled/loading/clear states retain the expected
layout. Select render tests pass 18/18, including the v6 loading-icon label,
spin class, and SVG viewBox.

### QRCode and runtime theme verification (2026-10-22)

`qrcode-dom-style-parity.json` now exercises default canvas, input editing with a
real Tab key event, and the loading cover. Against the rebuilt .NET 10 server on
5540 and pinned React host on 8000, all three ordered normalized DOM trees are
equal and all requested root/semantic computed styles and geometry match.
The raw HTML gate still differs on Razor indentation/whitespace, so the runner
correctly returns a failing strict status rather than claiming character-level
equality. Report: `/Volumes/Data/antd-v6.OYfUQf/qr-check/recheck/parity-final.json`.
The QRCode functional checker separately verifies all 11 public React demo
names, resizing, error correction changes, actual PNG/SVG files, Popover,
cover/canvas presence, and 12 global-token rows, with no page errors. The cover's
Semantic DOM highlight was visually checked after changing it to an inner
outline so QRCode's rounded overflow clipping cannot hide it.
The final functional report is
`/Volumes/Data/antd-v6.OYfUQf/qr-check/final-latest/browser-check.json`; an earlier
timing failure is retained in `final/browser-check.json`. The checker now
requires initial canvas paint and the selected H radio state before testing
the error-level redraw, rather than treating a control click as proof.

Runtime algorithm comparison is now independent of token-name coverage:
`generate-theme-reference` records all 536 values returned by the actual React
`getDesignToken` in 12 scenarios, and C# compares all 6,432 values. Nested
ConfigProvider rendering is tested separately because a passing isolated
algorithm cannot prove correct inherited inputs. Browser verification caught
and fixed resolved-parent-palette leakage into the child dark algorithm; the
live Default/Dark/Compact demo now shows the correct 32/32/28px controls and dark
neutral colors/shadows. This does not prove every component state, arbitrary
token input, algorithm composition, or byte-identical token serialization.

### Icon canonical demos and Select Size recheck (2026-10-24)

Rechecked the pinned React 6.6.4 worktree against the .NET 10 server using the
in-app browser. At a verified 1280×900 viewport on both tabs, Select Size's
Medium/Large/Small states each have three roots, matching ordered normalized
DOM trees and every declared root/subtree computed property. The heights are
32/40/24px and widths are 200/342.664/342.664px. Selecting and deselecting b11
in the multiple Select was also verified with real browser clicks.

Added `icon-dom-style-parity.json` for the entire Space trees in Basic,
Two-tone, and Custom. All three ordered normalized trees and declared computed
styles now match, including SVG path geometry, two-tone fills, spin/rotation,
custom SVG titles and the nested Home icon. No SVG/style attributes are ignored.
Raw HTML equality remains false in all six scenarios (renderer whitespace and
attribute serialization); this is not reported as strict byte-level parity.
The two iconfont demos and other Select states/popups are outside this batch.

The fixes include generated icons rather than legacy name/theme markup in
Basic/Two-tone, normal one-em dimensions, SVG-level rotation, automatic Loading
spin, immutable per-render two-tone paths, generated secondary colors, root SVG
fill, and forwarded attributes for custom nested components. The icon package
is now `0.2.0-beta.3-local.20261024.2`, locally packed for all seven original
target frameworks and referenced by AntDesign. Icons-repository changes remain
uncommitted/unpushed. `Icon.ComponentTemplate` supplies SVG attributes without
breaking the existing RenderFragment Component API.

Space presets now use the existing v6 token gap classes rather than fixed inline
pixels. Split no longer halves custom gaps or adds the old separator margin.
Document CSS links have content hashes: browser CSSOM inspection caught a cached
old `.anticon > .anticon { line-height:0 }` rule after its source was removed;
the versioned stylesheet restores the nested icon's React-matching 14px line.

Icon metadata was regenerated with the existing docs CLI in an isolated offline
copy (translation disabled) and only the Icon records were merged, preserving
all other component metadata. Both languages now register the separate Blazor
dynamic-color extra, current source text and ComponentTemplate API. The gallery
has its own heading rather than a duplicate Examples heading. Browser search
for home and Filled/All theme switching work, and dynamic color updates leave
the canonical two-tone colors unchanged.

Verification: 143 selected main-repository .NET tests, 21 Icons generator/color
tests, and 30 parity/loader Node tests pass; both repository diffs pass whitespace
checks. Evidence is in `/Volumes/Data/antd-v6.OYfUQf/icon-select-recheck/`, including
raw captures and separate comparisons. The shared read-only collector is usable
through either browser adapter and rejects empty captures. No full-library,
whole Icon-page, or all-state CSS certification is implied by these six cases.

### Shared v6 colors and component metadata (2026-10-24)

Extracted independent `AntDesign.Colors` (netstandard2.1), porting complete
`@ant-design/colors` 8.0.1 palettes and required FastColor 3.0.1 conversions,
alpha/lightness/mixing operations. Default/dark themes, preset palettes,
neutral/alias/shadow colors, Tags and both Icon APIs use it. Removed the
theme-local palette/color math and the Icons-only secondary-color algorithm.
Icons now depends on the shared package, not on AntDesign (no dependency cycle).
Local Colors `.20261024.3` and Icons `.20261024.6` packages are in use; nothing
was published or pushed. The same file includes upstream license notices.

Actual pinned React packages generated 909 complete palette cases (9,090
swatches) and 1,212 color-operation comparisons. These, the existing 6,432
theme-token comparisons and selected Icon/Docs/Select/Theming tests pass:
1,846 main tests, 24 Icons tests, and 42 relevant Node tests. Browser clicking
Change Color confirms both generated and compatibility icons transition from
`#52c41a` / `#f6ffed` to `#f759ab` / `#fff0f5` without a JS color calculation.
This is not exhaustive coverage of arbitrary invalid color input or every
FastColor API.

Component metadata now obtains the real GitHub issue `total_count` for the
same repository/title query as its link. Shared in-flight/cache entries avoid
duplicate requests; rate-limit cooldown and unavailable/incomplete-result
states never masquerade as zero. Browser Select shows 33 at this check.
LLMs.md opens a real plain-text Markdown page containing compiled Blazor API,
Semantic DOM, Token tables and Razor examples. Multiword components resolve
the same kebab-case catalog keys as the page. CLI `llms` exports the same 75
components in both languages for static hosting; Select's live response and
static export compare byte-for-byte. The regular docs-build CLI pipeline now
includes this step. Existing raw Markdown asset URLs remain available.

The five public React Icon demos also pass ordered normalized Space-tree,
declared CSS and actual glyph-definition/paint gates. The font loader uses
reverse sequential resource ordering with shared pending requests, and SPA
round trips retain exactly three resource scripts and seven painted glyphs.
Raw HTML equality is still false due to renderer serialization. SVG paint
measurements declare only a 0.01px rounding tolerance; CSS stays exact.

An additional full-suite run did **not** pass: it exposed two Typography tests
still expecting old action-wrapper markup, then was aborted by an AutoComplete finalizer
null reference at `AutoComplete.razor.cs:650`. These are outside this change's
selected green suite and remain to be handled; no full-suite claim is made.
An isolated Typography run confirms four such outdated structural assertions
(73 pass, four fail); the SVG paths themselves are not the reported difference.
Evidence: `/Volumes/Data/antd-v6.OYfUQf/iconfont-check/` and
`/Volumes/Data/antd-v6.OYfUQf/llms-export-check/`. Browser tabs remain open and
temporary comparison viewport overrides were cleared.

### Colors specification and compile-time LLM documents (2026-10-24)

Added bilingual `/docs/colors` (also `/{language}/docs/spec/colors`) with the
React specification's 12 base palettes, neutral preview, generation tool,
brand/functional examples and seven light/dark neutral Token rows. The CLI
`sync-color-spec` extracts palette order, localized names/descriptions, primary
colors and neutral definitions/default count from the pinned React checkout.
The 120 swatches are calculated by shared C# `AntDesign.Colors`; the page uses
real Blazor ColorPicker controls. An additional, separately labelled dark tool
supports both primary and background changes.

Browser comparison discovered that React defines 13 neutral colors but renders
only ten by default. The Blazor preview now also renders ten, retaining all 13
definitions in the synchronized catalog. All 120 base colors, normalized text
labels and 44px row heights match the actual React page. The ten displayed
neutral colors and initial ten generated colors also match. This is a scoped
value/label/row-height comparison, not full raw DOM or complete page CSS parity.
Actual browser actions verified clipboard output `#fff1f0`, primary `#808080`
updating the palette and S/V recommendations, and dark background `#30243B`
recomputing dark swatches. At 390px, document width equals viewport width;
horizontal strips scroll locally. English and Chinese routes render correctly.

`AntDesign.Docs.Generator` now prepares full component Markdown from compiled
JSON inputs and general documents from source Markdown. It performs HTML/API
conversion, Semantic DOM and Token table construction at compilation, then
packs the text into compressed constants to avoid the CLR string-heap limit.
Runtime only decodes/caches the prepared text. Missing Token inputs are a build
error (`ADBLLMS001`), not silently incomplete documents. The docs-build pipeline
refreshes `llms-tokens.json` before compilation/export. Existing raw component
Markdown URLs remain available.

Selected Docs/Icon/Theming/Select tests: 1,854 pass, zero failures. Server build
passes (482 existing warnings). The new generator tests cover packed output,
changed inputs, missing-input diagnostics and bilingual general documents.
CLI color-catalog `--check` passes. All 176 exported documents (75 components
and 13 general docs per language) compare byte-for-byte with their live
plain-text endpoints; unknown documents return 404. Browser clicking the color
page's LLMs.md opens the generated Chinese Markdown. No full-suite claim is made;
the prior unrelated Typography/AutoComplete findings above remain outstanding.
Evidence: `/Volumes/Data/antd-v6.OYfUQf/colors-spec-check/`,
`/Volumes/Data/antd-v6.OYfUQf/colors-llms-export-check/` and
`/Volumes/Data/antd-v6.OYfUQf/colors-tests.log`.

### Select Basic and popup follow-up (2026-10-24)

Fresh browser comparison found the Basic popup was 128px wide for a 120px
trigger: width was applied to an inner wrapper, then popup padding was added.
Moved width/min-width constraints to the border-box popup root. OverlayTrigger
now forwards the existing Overlay display/CSS-z-index options; Select uses
block layout and its `zIndexPopup` token (1050 by default), not the shared
inline-flex/max-window-z-index fallback. Other triggers keep their defaults.

Added the real v6 dropdown-list/holder/inner structure and `popup.list` /
`popup.listItem` ClassNames/Styles hooks. Input ARIA controls now reference one
real list ID, options have stable IDs and explicit selected/disabled booleans,
and mouse/keyboard activity updates `aria-activedescendant`. Disabled options
do not become active. Explicit options remain mounted while filtered empty,
but only the visible empty-state listbox owns the ID; recovering the search
restores the ordinary listbox. Filtering explicitly refreshes the portaled
render tree instead of leaving stale list/empty markup.

Browser interaction exposed two additional gaps: async option registration
did not re-render the suffix/clear button, and readonly single Select did not
register native arrow-key/Enter prevention. The suffix now receives its clear
state explicitly (and multiple mode receives Loading). Arrow keys no longer
scroll the document before the server handler runs. Added the loading suffix
and allow-clear root classes, removed the extra disabled-input tabindex, and
matched v6 state-class order. Basic now uses four actual SpaceItem boundaries
and the React `Yiminghe` value. Its compiled bilingual demo source was refreshed
through the real offline docs CLI, retaining unrelated component metadata.
Semantic DOM highlighting now uses the synchronized Select catalog selectors
directly, without the old role-based list fallback.

Verification: .NET 10 server build succeeds; 1,890 selected Docs/Select/Menu/
Icon/Theming tests pass, including new popup/state/ARIA/empty-recovery tests.
At 1280×900, Basic's four closed selectors have zero ordered normalized DOM
differences and zero differences in the 15 declared CSS properties, including
all declared semantic nodes. Open trigger DOM/CSS also matches. Popup root,
list, four options/content/state nodes now have zero declared CSS differences.
The new manifest `select-basic-popup-dom-style-parity.json` preserves full DOM
and exact geometry comparisons: it does **not** claim that the popup gate is
green. React's default virtual-list accessibility structure differs from the
Blazor nonvirtual list; portal DOM and strict position differences remain.
The observed relative offset differs by 1px horizontally and about 0.117px
vertically; these remain red with zero tolerance rather than being hidden.

Real browser actions verified mouse selection, disabled click rejection,
ArrowDown Lucy → yiminghe with unchanged scrollY (204), Enter selection,
Clear restoring the placeholder, one empty listbox for a missing search,
and recovery to Lucy. Clicking Semantic DOM `popup.list` outlines the real
212×128px list. Native 575px document width equals its viewport width.
This is not all Select demos/states, virtual-list, or full-library completion.
The legacy Select style-class demo and remaining virtual-list/position parity
are follow-up work. Browser tabs and the server remain available for manual QA.
Evidence: `/Volumes/Data/antd-v6.OYfUQf/select-popup-check/`,
`/Volumes/Data/antd-v6.OYfUQf/select-popup-tests.log` and
`/Volumes/Data/antd-v6.OYfUQf/select-popup-build.log`.

### Mobile docs Sider outside dismissal (2026-10-24)

Compared React's `.dumi/theme/slots/Sidebar/index.tsx` and its installed
`@rc-component/drawer` mask click handler. React closes on mask click; its
ordinary mask does not pass the original click through. Retained the requested
Blazor Layout Sider and zero-width trigger, adding a transparent non-interactive
mask and a document capture listener only while expanded. Outside clicks close
the Sider without cancelling, stopping, or replaying the original event; inside
clicks are ignored. Closing and component disposal remove the listener.

Verification: .NET 10 docs build succeeds (482 existing warnings, zero errors),
1,892 selected Docs/Select/Menu/Icon/Theming tests pass, and five JavaScript
regressions cover original button events, native anchor defaults, stopped
bubbling, internal clicks, repeat registration, disposal, and stale receivers.
Browser checks at 390/575/700px confirm outside dismissal; at 575/700px one
outside Select click both collapses the Sider and opens the original dropdown.
At 700px one Clear click both collapses it and clears the original selection.
Internal group clicks keep it open; internal navigation still closes and routes
to Button. Resizing an expanded Sider to 1280px removes the mobile host/mask;
returning to mobile recreates a working collapsed Sider. Evidence and logs:
`/Volumes/Data/antd-v6.OYfUQf/mobile-sider-mask-check/`,
`/Volumes/Data/antd-v6.OYfUQf/mobile-sider-build.log`, and
`/Volumes/Data/antd-v6.OYfUQf/mobile-sider-tests.log`.

### Select semantic styles and shared Overlay alignment (2026-10-24)

Replaced the obsolete `.ant-select-selector` style-class demo with React's
two selectors: Object/Function placeholders, real MehOutlined prefixes,
GuangZhou/ShenZhen options, 300px width and 8px radius. ClassNames supplies the
root class; Styles supplies blue prefix/suffix colors, and a C# function returns
the filled variant's purple prefix/suffix and popup border. Both localized demo
sources/descriptions were refreshed with the offline metadata CLI, replacing
only that demo record. The compiled generator consumes these inputs as before.

Single-select prefixes now precede content as root children, matching React;
placeholders are `div.ant-select-placeholder`. Removed the obsolete 22px input
left compensation and empty semantic style attributes. Regenerated Cascader's
input/placeholder selector mapping through the catalog CLI as it shares this
trigger. Multiple/tags prefix layout is not claimed aligned by this batch.

Browser operation exposed an additional shared bug: Overlay concatenated an
unterminated custom border with its following left declaration. Fixed the
shared style separator, rather than adding a trailing semicolon only to the
demo. Also compared installed rc-trigger's useAlign/onVisibleChanged: Overlay
now retains fractional anchor/container coordinates until final alignment,
floors final offsets on unscaled axes, and realigns after its own popup motion
ends. Existing DOM info callers retain their rounded contract. Animation
listeners are removed on disposal; descendant/hidden animation events do not
reposition the popup. No Menu-specific placement correction was introduced.

Verification: final .NET 10 docs build succeeds; 1,895 selected
Docs/Select/Menu/Icon/Theming/Overlay tests pass. All 169 StyleGen JS tests pass,
including precise measurements, pixel alignment, scaled-axis floor behavior,
motion completion, and the new strict semantic-style manifest validation.
Browser checks confirm two real Meh SVGs, blue/purple prefix/suffix colors,
300px/8px roots, a real 1px purple popup border, and choosing/reopening ShenZhen.
Responsive Menu's More popup remains anchored; clicking Support updates its
real status to `Selected: support`. Browser dimensions were restored and both
Select and Menu pages left open for manual checking.

Strict gates remain intentionally incomplete:

- Basic closed selectors: zero normalized ordered DOM or declared CSS
  differences. Open trigger DOM/CSS also matches. Popup declared computed CSS
  matches; its previous 1px horizontal relative offset is eliminated. Exact
  relative vertical geometry still differs by 0.1171875px, with tolerance zero.
  The two documents have different fractional anchor positions; both now use
  the same final pixel-floor algorithm. Full popup DOM still differs because
  React defaults to its virtual accessibility list.
- Style-class: the 20 declared properties, including input offsets and
  transform, match for Flex/root/prefix/content/input/placeholder/suffix.
  Strict DOM remains red for hidden popup siblings, Flex's extra runtime ID,
  generated class identity/order, and inline CSS serialization. The filled
  popup's 14 declared computed properties match, including border and stacking;
  exact relative geometry remains red (0.3359375px horizontal and 0.6015625px
  vertical in this capture). No attributes were ignored or tolerances increased.
- Native Escape interaction did not change the observed open state, although
  manually dispatched bUnit keyup tests pass. This is not accepted as working
  browser parity; distinguish input delivery from application behavior next.

Evidence: `/Volumes/Data/antd-v6.OYfUQf/select-style-alignment-check/` contains
React/Blazor Basic and semantic-style snapshots, strict comparison reports,
and `semantic-style-final.png`. Build/test logs are
`select-style-alignment-build-final.log`, `select-style-alignment-tests-final.log`,
and `select-alignment-js-tests.log` in the same temporary workspace root.
The existing gulp graph references absent build scripts; the browser bundle
was regenerated with the installed React checkout's esbuild and verified in
the running document. This batch is not full Select or first-stage completion.

### Select virtual accessibility and real offscreen navigation (2026-10-24)

Revalidated the prior Escape observation before editing its handler. Both the
native and locator key channels close the popup correctly when verification
waits for the real input's `aria-expanded=false` and cleared `aria-controls`;
the immediate native accessibility snapshot had lagged behind the DOM. No
Escape implementation change was needed. The Basic parity manifest now includes
an Escape scenario with explicit expanded/collapsed DOM waits, covered by a
manifest regression test. This supersedes the preceding unresolved observation.

For the existing opt-in ungrouped DataSource virtual mode, added React's
independent zero-sized accessibility list containing the active option and its
immediate neighbors. Visual virtual rows no longer duplicate option roles/IDs;
the input points to a stable accessible option even when its visual row is not
mounted. Active changes refresh the portaled accessibility list as well as the
input. Recycled rows release their model's child reference without detaching a
newer row instance. Changing virtualization mode while open refreshes the actual
popup; unit tests switch virtual -> 40 real rows -> virtual, preserve the value,
and check there is one listbox and no duplicate real option IDs.

Added `selectHelper` to core JSInterop rather than docs JS. Keyboard navigation
scrolls the real virtual holder by model index, using measured/themed row height,
instead of attempting to scroll a missing element reference. Browser resizing
and popup motion exposed a further issue: Virtualize can temporarily clamp a
jump to an estimated scroll range, then revise spacer heights and strand the
visual window away from its active item. Pending jumps now observe the relevant
virtual DOM until both the physical requested range and target row are visible.
A stale cached last row cannot prematurely complete a clamped request. Newer
navigation, wheel/touch/pointer input, and popup closing cancel the pending
request; observers are disconnected and an expiry bounds stale requests.

Multiple selectors no longer scroll to/activate a disabled selected tag when
opening. Their initial active item is the first enabled visible option, as in
React's Big Data demo. The disabled `a10` default tag and `c12` remain selected.

Verification: .NET 10 docs build succeeds; all 1,898 selected
Docs/Select/Menu/Icon/Theming/Overlay tests pass. All 178 StyleGen JS tests pass,
including seven virtual-scroll cases covering changed spacer estimates, stale
cached rows, superseding requests, cancellation and themed heights.
Real browser checks on the 100,000-item demo verify initial active `00`, one
accessible listbox, 18 mounted visual rows rather than 100,000, End reaching
`255r99999`, Enter adding that value, Home returning to `00`, and Escape clearing
expanded/ownership state. The document scroll position remains unchanged.
After 575px -> 1280px -> native-width changes from the first window, closing,
reopening and End still reaches the real final row at holder scrollTop 3,199,744
of scrollHeight 3,200,000. The selected final row is left open for manual QA.

Added `select-virtual-dom-style-parity.json` and captured both running sites at
1280x900. The independent accessibility list and its two initial options have
zero normalized ordered DOM or declared computed-CSS differences. This is a
scoped result, **not** a full virtual-popup pass. The complete manifest remains
red: visual window/spacer/scrollbar structures differ; the disabled selected tag
still lacks React's disabled class and incorrectly exposes a removal icon;
popup width is 342.656px versus 342.664px (the C# two-decimal width formatting is
a follow-up), and exact relative geometry also differs. No tolerance or ignored
attribute policy was relaxed. Default virtualization remains false; explicit
SelectOptions/grouped lists and the full React virtual-window DOM are still
required migration work, not redefined as out of scope.

Evidence: `/Volumes/Data/antd-v6.OYfUQf/select-virtual-check/` includes running
React/Blazor snapshots, `open-report.json`, keyboard and resize/reopen state
records, and `end-selected-final.png`. The build and regression logs are
`select-virtual-build-final.log`, `select-virtual-tests-final.log`, and
`select-virtual-js-tests-final.log` under the same temporary workspace root.

### Select disabled tags, removal events and measured width (2026-10-25)

Compared the installed React Select MultipleContent and BaseSelect sources.
All three default tag layouts (unlimited, numeric MaxTagCount and responsive)
now give disabled items the disabled class and omit their remove control.
Whole-selector Disabled also omits removal controls without incorrectly marking
every item as individually disabled. Backspace skips disabled trailing tags;
custom/stale removal callbacks cannot bypass either disabled guard.

Popup sizing retains the measured decimal precision in invariant CSS notation
instead of rounding to two decimals. Tests include fr-FR and a tiny decimal
that must not be serialized as an exponent. Browser trigger and popup widths
are both 342.6640625px (computed CSS 342.664px).

Real mouse testing exposed an additional removal bug: a native unbound Overlay
trigger saw the nested remove click before Blazor's delegated stopPropagation.
Added a scoped descendant ignore selector to native trigger registration; it
skips only that listener, without intercepting the original delegated click.
Removal now runs on click and prevents mousedown's focus change, matching React.
Browser checks confirm expanded removal retains popup and focus, closed removal
does not open it, normal trigger clicks still work, disabled tags survive
Backspace, and End/Enter/removal/Home still reaches the real 100,000-item window.

Verification: .NET 10 docs build succeeds; 1,911 selected
Docs/Select/Menu/Icon/Theming/Overlay tests pass after rebuilding TestKit for the
interop signature and updating the two existing tag-removal tests to click.
All 181 StyleGen JS tests pass. The stricter virtual manifest also checks tag,
disabled-tag and remove DOM plus 19 declared CSS properties. Trigger DOM,
tag DOM/CSS and accessibility-list DOM/CSS now match the running React reference.
The full popup gate is still red: visual virtual-window/spacer/scrollbar DOM
differs, and exact relative offsets differ by 0.3359375px horizontally and
0.6328125px vertically in the final capture. No tolerance/normalization was
relaxed; default and grouped/explicit-option virtualization remain unfinished.

Evidence: `/Volumes/Data/antd-v6.OYfUQf/select-disabled-width-check/`, with
snapshots, strict report, mouse/keyboard state records and final screenshot.
Build/test logs use the `select-disabled-width-*-final.log` prefix in that
temporary workspace. This is scoped progress, not full first-stage completion.

### Collapse structure, semantic preview and component tokens (2026-10-25)

Compared pinned React Collapse/rc-collapse sources and running documentation.
Default whole-header interaction is now distinct from explicit Header (title
and icon only), Icon and Disabled modes. Collapsible is nullable to preserve
that distinction, with matching classes, trigger-owned roles/expanded/disabled
attributes and keyboard handling. Accordion exposes tablist/tab/tabpanel.
The default arrow uses the generated v6 SVG, including interactive state labels,
decorative hiding and RTL rotation. Initial active panels do not run appear
motion; closed retained content has the inactive and hidden classes.

Collapse now consumes ConfigProvider component tokens on its real root,
including root-local global overrides. Added React's component-token demo with
its exact four background/padding settings; both localized metadata records are
regenerated by the offline CLI. Basic no longer invents a disabled third item;
Accordion starts closed, and Collapsible matches the three React examples.
Semantic DOM uses the upstream single expanded panel and all five real slots,
not the unrelated Basic demo. Its bordered 600px preview frame and zero paragraph
margins match React rather than inheriting the surrounding Markdown margins.

The new strict browser manifest covers Basic, Collapsible (three roots), and
Semantic DOM. All three have zero normalized ordered DOM or declared CSS
differences at 1280x720. Meaningful attributes/classes are retained; only
unreferenced generated IDs, declared framework markers and style hashes are
normalized. Raw byte HTML remains unequal because of Razor indentation/newlines;
that gate is retained, not reported as passing. Real token-demo measurements
match React exactly, including all four custom variables, header/body padding,
header/content backgrounds, text, body width and root width.

Real browser clicks verify header whitespace does not toggle explicit Header,
title and Enter do, Icon ignores title clicks, and Disabled stays closed.
Clicking the body Semantic DOM entry highlights the visible real body. .NET 10
docs build succeeds; 1,926 selected Collapse/Docs/Select/Menu/Icon/Theming/Overlay
and Collapse-structure tests pass, and all 182 StyleGen JS tests pass. A broader
structure-only run still has three existing failures: Tag's old close selector,
Steps' missing matchMedia mock, and Cascader's old placeholder selector. They
are not counted as fixed or as a whole-suite pass.

Evidence: `/Volumes/Data/antd-v6.OYfUQf/collapse-parity-check/` contains paired
snapshots, all three strict reports, exact token measurements/report, interaction
state and screenshots. Logs use `collapse-parity-*-final.log` in that workspace.
These cases do not prove full Collapse API coverage or overall first-stage
completion. A further docs issue was identified for the next batch: component
token tables label React/TypeScript token types as “C# Type”; the shared generator
should expose the actual generated C# property types instead.

### Checkpoint commit and generated C# token documentation types (2026-10-25)

Created the user-requested local checkpoint `a3d38ea53` on
`codex/antd-v6-css-sync-strategy`, containing the accumulated component/theme/docs
work through Collapse. Temporary screenshots/build outputs remain ignored or
outside the repository. No push or separate Icons-repository commit was made;
the requested local Icons package reference is retained.

Subsequent work fixes the shared token table rather than patching one page.
StyleGen now uses one mapping for typed component-token properties and the new
ComponentTokenDefinition.CSharpType field. Type retains React's original type
for upstream metadata comparisons; the existing constructors remain available.
Regenerated all 671 definitions and LLM token inputs. Pages use CSharpType,
and compile-time LLM Markdown uses the same field with code formatting for
generic types. No runtime reflection/HTTP was added.

Verification: regenerating the C# file a second time is byte-identical. A test
oracle checks every metadata type against its actual C# property, including
the distinct qr-code/qrcode classes; all compiled component Markdown exports
in both locales are checked against their documented C# types. The .NET 10
server build succeeds, 1,930 selected regression tests pass, and all 183
StyleGen JS tests pass. Browser Collapse rows show string/OneOf<string, double>,
and both real English/Chinese HTTP Markdown responses contain code-formatted
C# types rather than CSSProperties. Final browser Accordion testing also
confirms three initially closed tabs, tablist/tabpanel roles, and Enter switching
exclusively from panel 1 to panel 2.

Evidence: `/Volumes/Data/antd-v6.OYfUQf/token-csharp-types-check/` includes the
regenerated-file comparison, browser row values, actual localized HTTP exports
and `collapse-types-final.png`. Logs use `token-csharp-types-*.log` in that
workspace. This post-checkpoint work remains uncommitted; overall API/virtual
DOM/browser coverage gaps remain open, as recorded above.

### Collapse client-side motion responsiveness (2026-10-25)

Collapse header clicks and Enter activation now update the panel, trigger ARIA
state, arrow, and height/opacity transition in the browser event handler before
the Server callback returns. The resulting active-key list is then synchronized
to .NET so ActiveKeysChanged, OnChange, and per-panel active callbacks continue
to run. Rapid reversal cancels the previous transition; completion uses the
computed CSS duration, honors reduced-motion, and only then marks a closing
panel hidden. This local handler and its eager hidden content are enabled only
when `Animation` is requested; the default non-animated path retains its lazy
render behavior and does not attach a client listener.

Verification: docs server rebuild succeeds and all 13 focused Collapse tests
pass. In the running browser, actual clicks opened and closed the animated Basic
demo and accordion remained mutually exclusive. With DevTools network emulation
set to 1200ms latency, a sampled in-flight close already had `aria-expanded=false`,
height `0px`, and a running CSS transition while the panel was still 99px tall.
The browser logged transient Blazor negotiation failures during server restarts;
after the final reload, the server received `1,2` from the multiple-open demo.
The Collapse helper itself logged no errors. Screenshot: `/Volumes/Data/antd-v6.OYfUQf/
collapse-local-motion-check/collapse-client-motion.png`. The main accumulated
worktree remains uncommitted after checkpoint `a3d38ea53`.

### Select Big Data settled-window recheck (2026-10-25)

The virtual Select browser scenario waits 400ms after opening so the
comparison samples the same post-open interval rather than racing popup motion.
The accessibility mirror emits explicit `aria-disabled="false"` for enabled
options. Blazor now uses the same 3,200,000px spacer and positioned inner
window as React instead of ASP.NET `Virtualize` top/bottom spacers. The visible
initial window is 10 options; a real browser End-key interaction reaches item
100,000 and renders only the tail window while preserving the active descendant
and hidden accessibility mirror. A custom scrollbar is included, and scrolling
is handled by the component's Select interop helper.

The fresh React `127.0.0.1:8000` / Blazor `127.0.0.1:5540` parity run reports
normalized DOM-tree equality and equality for all computed styles, including
the popup and hidden accessibility options. Remaining comparator differences
are byte-level Blazor marker/attribute serialization and subpixel popup
placement: 0.34375px horizontally and 0.859375px vertically in this run. The
comparator tolerance was not loosened. The .NET 10 docs server builds with 0
errors (6 existing dependency warnings); the server is running in Development
so `_content` assets resolve. The browser remains open on Big Data with the
last option selected for manual review. Report: `/tmp/select-virtual-window-impl.json`.

### Select open latency and Icon browser recheck (2026-10-26)

Select's pre-open listener now reveals the already-rendered and positioned
HiddenMode popup directly on `pointerdown`, before the Server callback, without
recreating Overlay or repeating its placement calculation. Listener ownership
is tracked in a `WeakSet`, not a DOM attribute, so the optimization adds no
runtime-only markup. Keyboard Enter/Space/ArrowDown uses the same eager path.
On the 100,000-item demo, three browser pointer-click samples measured a median
of 59ms on React and 31ms on Blazor; both exposed the expanded state and visible
popup immediately. A screen capture confirmed the options and popup placement.

The Icon manifest was run against React `127.0.0.1:8000` and Blazor
`127.0.0.1:5000`: all five canonical demo groups (Basic, Two-tone, Custom,
Iconfont and Script URL) passed, with zero DOM/style differences and zero
blocked groups. The manifest's Blazor host was updated to the live Development
server port. Report: `/tmp/icon-dom-style-parity-2026-10-26.json`.

The Select virtual Big Data manifest was rerun against the same live hosts.
Normalized ordered DOM and computed CSS values match; raw byte HTML still only
differs in Razor whitespace. Its strict semantic geometry gate still reports
subpixel popup descendant offsets (0.34375px left and 0.25px top), so this is
not counted as an exact pass; the comparator tolerance remains zero. Report:
`/tmp/select-virtual-window-2026-10-26-final.json`. Select popup tests pass,
ESLint/TypeScript checks pass, and the .NET 10 documentation server builds
without errors. The current browser remains open on the Blazor Big Data example
with its options visible for manual inspection.

An in-page timing probe now measures from the actual trigger `pointerdown` to
the first animation frame where the popup has nonzero opacity and a painted
rectangle. Across four runs of the same Big Data scenario and viewport, React
measured 412.8–427.2ms (median 426.9ms; the CSS entrance motion dominates);
Blazor measured 241.8–439.2ms (median 272.9ms). This does not reproduce the
reported multi-second wait; three of four Blazor samples were faster than
React. The timed runs continue to show equal normalized DOM and computed CSS.
The exact geometry gate remains red: popup descendants differ by 0.34375px
horizontally and 0.25–0.78125px vertically depending on the run, with no
tolerance change.
Reports: `/tmp/select-virtual-window-timing-2026-10-26.json`,
`/tmp/select-virtual-window-timing-repeat2-2026-10-26.json`,
`/tmp/select-virtual-window-timing-repeat3-2026-10-26.json`, and the final
post-probe run `/tmp/select-virtual-window-timing-final-2026-10-26.json`.

The strict geometry mismatch was caused by sampling Blazor during its still
active popup entrance animation. Waiting 800ms after the popup becomes visible
produces exact geometry for the popup root, accessibility list, and options;
computed styles and normalized ordered DOM are equal as well. The runner now
waits for that settled state in the Big Data manifest rather than weakening the
zero-pixel geometry gate. The remaining report difference is raw Razor HTML
serialization/whitespace, not rendered DOM or geometry. Report:
`/tmp/select-virtual-window-stable-open.json`.

Splitter Size was rechecked against the current .NET 10 server after the empty
dragger `style` attribute was removed. The normalized DOM tree, root, splitter
bar, dragger, and inspected flex/container properties match. The first pane's
actual box and computed width still differ (React `300.53125px`/`300.531px`,
Blazor `300.515625px`/`300.516px`); its right edge rasterizes to the same
device-pixel boundary at DPR 1, but the strict computed-style gate remains red
and Splitter is not marked complete. The expanded report captures the bar as a
semantic node: `/tmp/splitter-size-box-layout-recheck.json`.

### Radio semantic icon structure (2026-10-26)

Radio now follows React v6's semantic icon structure: the `.ant-radio` icon
contains only the native input, and its selected dot is rendered with the
icon's `::after` pseudo-element. The obsolete `.ant-radio-inner` child and
class mapper were removed; the Semantic DOM catalog now identifies `.ant-radio`
as the `icon` slot. The component CSS compatibility is in `radio/style/patch.less`
without editing the mirrored `index.less`.

Verification: 20 focused Radio tests pass. The React/Blazor Radio browser parity
runner reports both Basic and Radio.Button groups passed, with zero DOM/style
differences (2/2 visual style pass). A real click on each Basic demo selected
the radio and both pseudo-element dots reached opacity 1. The restarted .NET 10
docs server is available at `http://127.0.0.1:5000/en-US/components/radio`; the
browser remains open on that page for review. Report:
`/tmp/radio-dom-style-parity-2026-10-26-after.json`.

The next browser fixture, Breadcrumb Basic, was rebased from its stale Blazor
host to the live docs server and rerun against React v6. It passed with zero
DOM/style differences (1/1 visual style pass). Report:
`/tmp/breadcrumb-basic-dom-style-parity-2026-10-26.json`.

Transfer Basic passed with zero DOM/style differences (1/1). QRCode Base,
editable-value interaction, and Loading cover passed all three scenarios
(3/3). Grid Gutter and all ten Grid Layout examples also passed with zero
DOM/style differences (11/11). Their fixture hosts were rebased to the live
React 8000 / Blazor 5000 servers. Reports:
`/tmp/transfer-basic-dom-style-parity-2026-10-26.json`,
`/tmp/qrcode-dom-style-parity-2026-10-26.json`,
`/tmp/grid-gutter-dom-style-parity-2026-10-26.json`, and
`/tmp/grid-layout-dom-style-parity-2026-10-26.json`.

Tooltip Basic's first hover run exposed discarded asynchronous callbacks on the
unbound OverlayTrigger path. Those mouse/focus callbacks now return their
`Task`, so the exclusive DOM listener awaits them. Empty inline `style`
attributes are also omitted when Tooltip has no custom color. After rebuilding
and restarting the docs server, both initial and hover/portal scenarios passed
with zero DOM/style differences (2/2). Focused OverlayTrigger tests pass.
Final report: `/tmp/tooltip-basic-portal-parity-2026-10-26-final.json`.

Splitter Size remains open: computed styles match, but strict DOM comparison
finds inline-style serialization differences and an empty `style` attribute;
panel geometry differs by 0.015px. It is not counted as a pass. Report:
`/tmp/splitter-size-dom-style-parity-2026-10-26.json`.

### Overlay instance reuse during enter motion (2026-10-26)

The immediate-open path creates and portals an Overlay on pointerdown so
Blazor Server latency does not delay Select. The subsequent Blazor render now
reuses the existing instance when popup, container, trigger, boundary mode,
and trigger prefix still match. If those references temporarily differ during
an enter animation, replacement is deferred until the motion settles; repeated
renders update the pending parameters, and disposal cancels pending work.
Completed enter classes are removed without changing popup identity classes.

Five focused Overlay configuration/idempotence tests pass, TypeScript typecheck
and `git diff --check` pass, and the regenerated JavaScript bundle is served by
the local .NET 10 docs app. The manual review tab remains open at
`http://127.0.0.1:5022/en-US/components/select?theme=light#components-select-demo-basic`.
This verifies the page loads, not that all Overlay geometries are aligned.

### Splitter Size strict-geometry rerun (2026-10-26)

The earlier 0.015px first-panel width mismatch did not reproduce in two fresh
React/Blazor Chromium runs with the manifest's matched scope width and strict
zero-pixel gate. Both runs passed the ordered DOM tree, all four semantic
paths (root, panels, bar, dragger), computed styles, and visual geometry with
zero differences. Reports:
`/tmp/splitter-size-current-2026-10-26.json` and
`/tmp/splitter-size-repeat-2026-10-26.json`. No CSS tolerance was relaxed.

DatePicker remains a concrete open overlay-adjacent difference: a fresh Basic
popup run passes popup-root geometry and styles but fails the trigger display
(`flex` in React, `inline-flex` in Blazor) and footer control geometry (ranges
top differs by 0.265625px; Today differs in position and size, including a
5px height difference). It is not counted as a component pass. Report:
`/tmp/datepicker-overlay-current-2026-10-26.json`.

Select Basic was rerun after rebuilding the server. Closed state is exact and
all three visual-style checks pass. The open popup still differs in the live
React runner's `aria-disabled="false"` output for enabled options, while the
checked React checkout's semantic snapshot includes that attribute; no source
change was made from this contradictory evidence. The Escape scenario also
leaves React open while Blazor closes. Report:
`/tmp/select-basic-popup-after-revert-2026-10-26.json`. The local Select page is
open in a separate browser tab for manual follow-up.

### Select fixture and parity-runner correction (2026-10-26)

The previous popup comparison was not anchored to the Select trigger: its
`title="yiminghe"` selector could match another mounted Select demo. The Basic
popup fixture now selects the visible dropdown nearest to the clicked trigger
and scopes list/options/content/state capture to that exact popup. An in-page
check confirms React Basic's options are Jack, Lucy, Disabled, Bamboo, and the
React and Blazor enabled options both render `aria-disabled="false"`; the
earlier attribute mismatch is not accepted as a component defect.

The parity runner's `state: "hidden"` action now uses Puppeteer's explicit
`hidden: true` option (rather than `visible: false`). The Escape fixture waits
for the opening motion before sending Escape and allows the close motion to
settle before capture; an interactive browser check closes both versions.
Capture tests now prove popup descendants stay scoped to the popup selected
nearest their trigger. All 46 StyleGen runner/parity tests pass.

The fresh headless parity attempt is not counted as a pass: its standalone
Chromium session rendered an empty React body and timed out before finding the
Select demo, while the existing in-app React page is loaded. The Select Basic
and React pages remain open for manual review; rerun the strict browser gate
once the headless React route is available. Report:
`/tmp/select-basic-popup-nearest-trigger-2026-10-26.json`.

### Select overlay first-frame width and enter-motion reuse (2026-10-26)

Select's immediate-open path now measures the already-rendered popup while it is
hidden, locks its resolved width, and only then portals and reveals it. The
default strategy matches the trigger; content-fit and explicit width strategies
are also covered. Subsequent Blazor renders reuse the same Overlay when its
trigger/container context matches, and defer a replacement until an active
enter motion ends if the context temporarily changes.

Seven focused overlay configuration/idempotence tests pass, including hidden
width measurement and deferred rebuild. TypeScript typecheck passes. A live
Blazor browser check on Select Basic observed the popup at the trigger's 120px
width on its initial visible state; the content-fit case measured 169.23px for
the same trigger. The local .NET 10 docs server remains available at
`http://127.0.0.1:5000/en-US/components/select`.

DatePicker's parity capture still reports the trigger `display` as `flex` in
React and `inline-flex` in Blazor, but the pinned React v6 source explicitly
declares `inline-flex`. A live React page reports `flex` only while focused/open,
so this is not changed blindly; the open-state style source needs to be isolated
before treating it as a Blazor defect. Existing capture:
`/tmp/datepicker-overlay-current-2026-10-26.json`.

### Select semantic style capture follow-up (2026-10-26)

Corrected the closed-state capture roots from the enclosing Flex to the two
actual Select instances, excluding Blazor's intentionally pre-rendered hidden
popup wrappers. Both Select roots and all declared semantic nodes now match in
DOM, computed styles, and strict geometry. The parity runner also scrolls deep
documentation targets instantly before clicking, avoiding false timeouts caused
by smooth page scrolling; its focused suite passes 8/8.

The open semantic popup has matching DOM, computed styles, size, and viewport
top (`469px`) in React and Blazor. Relative geometry still reports a subpixel
delta (0.34375px horizontally, 0.3125px vertically) because the documentation
layouts place the triggers at different fractional coordinates; this case is
still recorded as a difference, not waived or counted as a complete pass. Report:
`/tmp/select-semantic-style-live-final-2026-10-26.json`.

After rebuilding the Server target (the first rebuild had left its copied
component assembly stale), the hidden virtual accessibility list was aligned:
enabled options omit `aria-disabled`, while disabled options retain
`aria-disabled="true"`, matching React. Popup root inline style serialization is
renderer-specific, so that one attribute is excluded only for `popup.root`; its
computed CSS and trigger-anchored device-pixel geometry remain exact checks with
zero tolerance. The current two-scenario Select semantic-style gate now passes
2/2, with both visual-style checks passing and no DOM/CSS/geometry differences.
Report: `/tmp/select-semantic-style-parity-current-2026-10-26.json`.

The full StyleGen suite passed 200/200 before this final ARIA markup refinement;
the focused parity-manifest suite passes 39/39 afterward. The targeted xUnit
virtual-list test currently fails earlier at its existing `data-virtual-count`
assertion (expected `40`, actual `null`), before reaching the updated ARIA
assertion; this is tracked as a separate test/runtime discrepancy and is not
claimed as verified by that test.

### Overlay popup width locking (2026-10-26)

Overlay now prepares Select popup width before container positioning and the
enter state: default `DropdownMatchSelectWidth=true` locks to the trigger;
`false` measures content-fit (respecting the trigger minimum and configured max
width), and explicit widths are measured as well. Width is retained in the
Overlay component's rendered style so the first Blazor Server rerender cannot
discard the pre-animation measurement. Reopening a hidden popup remeasures it;
an immediate pointer-open reuses its already prepared width without interrupting
the animation. Focused JS tests pass 8/8 and TypeScript typechecking passes. The
net10 docs server build passes. The full legacy Overlay JS suite has four
unrelated existing failures (one coordinate expectation and three jsdom
animation/ResizeObserver mocks); the browser automation connector was
unavailable because its shared Chrome profile is already owned by another
process, so interactive browser verification remains pending.

### DatePicker popup capture isolation (2026-10-27)

The StyleGen runner now supports `captureReactBeforeBlazor` for overlays that
close on blur. DatePicker's open-state capture completes the React actions and
snapshot before activating Blazor; the popup instance is no longer missing or
selected from a stale portal. This changed the report from a false missing-node
failure to a valid same-state comparison: popup dimensions/placement, footer
range DOM/CSS/geometry, and popup/footer CSS all match exactly. Remaining
verified differences are the trigger's `display` (`flex` in React docs vs
`inline-flex` in Blazor), empty-input value serialization, Icon SVG/accessibility
markup, and the docs font-family difference (which also changes the Today link
hit-box by up to 5px). The header month/year buttons now expose the localized
React-equivalent aria labels, and runtime element IDs/class token order are
normalized as framework details. Current report:
`/tmp/datepicker-overlay-sequential-current-2026-10-27.json`. Headless Chrome
capture completes against the running React and Blazor docs servers; DatePicker
is still in progress and is not counted as fully aligned. The StyleGen suite
passes 201/201, and the parity-focused file passes 40/40.

### Overlay enter-motion rebuild race (2026-10-27)

When a render changes an Overlay's positioning context during its enter motion,
the JS helper defers replacing the positioning instance until the motion ends.
The browser may apply the enter classes one frame before `getAnimations()`
exposes the corresponding Animation object; computed animation/transition
durations are now used to cover this startup window. A regression test verifies
the old instance remains registered until `animationend`, including when
`getAnimations()` initially returns an empty array. The focused Overlay
configuration test passes, TypeScript typechecking passes, and the net10 docs
server builds and restarts successfully. Its first full legacy Overlay JS run
had four failures: one geometry expectation, a jsdom RAF gap, and two
ResizeObserver callback mocks using the old single-entry shape. The headless
browser automation uses an isolated Chrome profile; the user's existing
browser tabs remain open for manual review.

### Select Semantic DOM cold-start capture (2026-10-27)

Re-ran the Select semantic-style gate against the current React and Blazor
servers. Cold navigation can exceed the runner's former 60-second default, so
the Select and Icon manifests now allow 120 seconds for navigation while
retaining the 20-second component-readiness timeout and zero DOM/CSS/geometry
tolerance. The fresh Select capture passes both closed and open-popup scenarios
(2/2), with no DOM, computed-style, or visual-geometry differences. Report:
`/tmp/select-semantic-style-parity-cold-start-2026-10-27.json`. The latest Icon
capture remains 5/5 canonical demos passed; those are the documented Icon demo
groups, not a claim over every generated glyph in the catalog.

### Overlay open-width and enter-instance stability follow-up (2026-10-27)

Select popup width is measured and written before the overlay is revealed. In a
live browser interaction, its width and trigger-relative coordinates were
identical immediately after opening and 350ms later (120px wide in both
captures). The positioning helper reuses the existing Overlay instance when
the trigger/container context is unchanged, and defers a context replacement
until enter motion ends; its focused regression case verifies the instance
remains registered and that only the newest placement/offset parameters are
applied afterward.

The old geometry fixture expected `right: -111px`, which would place a 372px
popup 30px beyond the visible viewport edge. The viewport boundary is
`scrollX + innerWidth`; the clamped left coordinate is 1612px and the equivalent
body-relative right offset is `-81px`. Updated the stale fixture to this
boundary-consistent result. The complete Overlay JS file now passes 53/53, and
TypeScript compilation and the net10 component build pass. Select's
semantic-DOM/CSS/geometry gate also passes 2/2 with zero differences. The
Escape mismatch was caused by switching focus between React and Blazor after
each action. Marked the Select basic group `captureReactBeforeBlazor`, matching
the existing DatePicker approach; the whole Select popup manifest now passes
all 3 scenarios, including Escape close, with no DOM/CSS/geometry differences.
The parity and runner tests pass 48/48. Report:
`/tmp/select-basic-popup-capture-order-report-2026-10-27.json`.

### DatePicker live follow-up (2026-10-27)

Re-ran the DatePicker basic popup manifest after the Overlay began copying
scoped Ant Design tokens before portaling. The earlier font-family difference
and the resulting Today-link hit-box offset are gone; popup, footer, and their
measured geometry now match. The fresh report is
`/tmp/datepicker-overlay-current-2026-10-27.json`.

Remaining differences are the trigger's computed `display` (`flex` in the live
React docs page and `inline-flex` in Blazor) and Icon serialization in the
trigger: React emits the 896-unit calendar SVG with `data-icon` and hidden
accessibility attributes, while Blazor currently emits the 1024-unit legacy
IconStore SVG. Do not change the component's upstream `inline-flex` rule based
on this report alone: the React v6 component stylesheet also declares
`inline-flex`, so the `flex` result needs its docs-cascade source identified.
DatePicker is not counted as fully aligned until these runtime differences are
resolved or shown to be a capture mismatch.

### Overlay pre-open width precision and component spot checks (2026-10-27)

Select measures and locks the popup while it is hidden, before placement and
enter animation. The default mode matches the trigger; content-fit and explicit
width modes use the measured dropdown content. A virtual-list capture exposed
that returning the fractional measurement as a JS number rounded `342.65625px`
to `342.656px` across Blazor interop, which snapped the popup one 1/64px step
narrower. The measured width now crosses interop as a string. React and Blazor
virtual Select popup widths and trigger-relative geometry now match exactly at
`342.65625px`; computed styles and visual geometry pass. The basic popup gate
passes 3/3 scenarios and the Semantic DOM gate passes 2/2. Virtual-list
accessibility options now match React's `aria-disabled="false"`; its only
remaining DOM difference is the renderer-specific inline style string on the
popup root, while computed styles and geometry match. Reports:
`/tmp/select-virtual-final-v2-2026-10-27.json`,
`/tmp/select-basic-popup-final-v2-2026-10-27.json`, and
`/tmp/select-semantic-final-2026-10-27.json`.

The Collapse basic, collapsible, and Semantic DOM gates pass all 3 scenarios
with zero differences (`/tmp/collapse-parity-live-report-2026-10-27-followup.json`).
Iconfont injects duplicate external SVG symbol definitions asynchronously. The
parity runner now waits for the referenced symbol set to stabilize before
capturing; all five canonical Icon demo groups then pass DOM, computed-style,
and visual checks (`/tmp/icon-parity-stable-svg-2026-10-27.json`).

### Overlay hidden measurement browser verification (2026-10-28)

Rebuilt the docs server after regenerating demo metadata so the current Select API
text documents the width modes accurately. On the local net10 docs server, the
Basic Select popup is 120px wide on first visible capture and remains 120px
350ms later. The Placement example with `DropdownMatchSelectWidth=false` opens
with the 180px trigger as its minimum width (the longest option is narrower),
and the popup remains 180px after 350ms. This confirms the first-frame match and
content-fit minimum behavior in a live Blazor browser. Server build and
TypeScript typecheck pass; a previous focused Overlay JS run passed 53/53.
The Overlay registry now reuses the existing positioning instance when the
popup, trigger, and container are unchanged. If Blazor supplies a new context
during enter motion, the helper retains the current popup and queues only the
latest rebuild parameters until the motion ends; an Overlay JS regression case
covers this deferred replacement path. This keeps the in-flight DOM node and
animation intact instead of disposing and recreating Overlay mid-entry.

### DatePicker parity follow-up (2026-10-28)

DatePicker now uses the generated `CalendarOutlined` and `ClockCircleOutlined`
components from the locally packed Icons package; its calendar icon SVG viewBox
and `data-icon` markup match React. The warm Basic popup capture confirms equal
trigger and popup geometry, including footer placement. It still reports two
DOM/style differences: React serializes `value=""` on the input while Blazor
omits the empty attribute (the live input value is empty in both), and the
trigger computes to `flex` in React versus `inline-flex` in Blazor. These are
recorded as outstanding rather than counting DatePicker as fully aligned.
Report: `/tmp/datepicker-overlay-current-2026-10-28-warm.json`.

The current Select basic popup, Select Semantic DOM, and five canonical Icon
demo groups remain passing. The virtual Select report's strict HTML comparison
still detects renderer-specific serialized whitespace/style text; its
normalized DOM tree, computed styles, and geometry match. Reports:
`/tmp/select-basic-popup-final-v2-2026-10-27.json`,
`/tmp/select-semantic-final-2026-10-27.json`,
`/tmp/icon-parity-stable-svg-2026-10-27.json`, and
`/tmp/select-virtual-final-v2-2026-10-27.json`.

### Current Select and Icon browser gates (2026-10-28)

Rebuilt the local JavaScript bundle before recapturing so the newly added
Overlay helpers are present in the served `ant-design-blazor.js`. The focused
Select Basic popup gate passes 3/3 scenarios, Select Semantic DOM passes 2/2,
and all five canonical Icon groups pass. Each has zero DOM/style differences
and zero visual-style differences. The Virtual Select visual/CSS/geometry gate
also passes; its raw popup-root `style` text differs because React and Blazor
express portal coordinates and measured width through different declarations.
The popup's DOM structure, selected computed styles, and trigger-relative
geometry match. The renderer-specific style attribute remains diagnostic while
its computed styles and geometry stay strict gates. Reports:
`/tmp/select-basic-popup-parity-current-2026-10-28.json`,
`/tmp/select-semantic-parity-current-2026-10-28.json`,
`/tmp/icon-parity-current-2026-10-28.json`, and
`/tmp/select-virtual-parity-current-2026-10-28-final.json`.

### DatePicker Basic strict DOM/CSS rerun (2026-10-28)

Aligned the Basic demo with React's vertical Flex wrapper (`gap="small"`,
start justification and alignment). This makes the focused DatePicker's
computed `display:flex` result come from the same flex-item context as React,
without overriding the component's `inline-flex` base style. DatePicker and
RangePicker now measure their calendar popup by content while retaining the
trigger as a minimum width, so the calendar opens at its natural 288px width
instead of being clamped to the 139px trigger. DatePicker input initialization
also preserves the empty `value=""` DOM attribute React emits.

The strict Basic popup manifest now passes DOM, computed-style, semantic-node
geometry, and visual-style checks with zero differences. Browser verification
opened the Basic calendar at `http://127.0.0.1:5004/en-US/components/datepicker`;
the trigger/popup measured 138px/288px, the trigger computed to `display:flex`,
and its input had `value=""`. Docs Server net10 build succeeded with 0 errors.
Report: `/tmp/datepicker-current-5004-2026-10-28-exact.json`.

The current Select Basic popup regression run also passes all 3 scenarios with
zero DOM/style and visual-style differences after the generic Overlay width
measurement changes: `/tmp/select-basic-current-5004-2026-10-28-verified.json`.

The Select Virtual gate now passes the same rendered-DOM, computed-style, and
zero-tolerance geometry checks. Its popup root's inline `style` attribute is
ignored only in the semantic-tree attribute comparison because React and
Blazor serialize portal coordinates, width, and inherited tokens differently;
the gate still compares the popup's computed styles, full descendant DOM, and
trigger-anchored geometry. Raw `outerHTML` remains available as a diagnostic
and is not treated as a DOM-tree failure. Report:
`/tmp/select-virtual-current-5004-2026-10-28-verified.json`.

Select Size was rerun against the rebuilt net10 Docs Server and passes all 3
size scenarios (default, large, small), including Semantic DOM slots, computed
styles, and visual-style checks with zero differences. Report:
`/tmp/select-size-current-5004-2026-10-28.json`.

A separate cold-start attempt to rerun the Select Semantic DOM manifest was
blocked because headless React did not mount the demo before its configured
timeout. The same-day warm report remains valid at 2/2 with zero differences;
an in-app React page loaded both roots and their five semantic nodes with
matching dimensions. The blocked cold run is recorded at
`/tmp/select-semantic-current-5004-2026-10-28-retry.json` and is not counted as
a component difference.

### Overlay popup width reveal regression guard (2026-10-28)

Popup width is measured while the rendered overlay is hidden and locked before
positioning/reveal (`match-trigger` remains the default; `content` and `custom`
remain available). The measurement phase now temporarily disables the popup's
own CSS transition, including consumer/theme `transition: all` rules, then
restores the original inline transition immediately before the enter classes
are applied. This prevents an intrinsic-to-final width transition from
continuing after the popup becomes visible. The targeted JS regression passes:
`npm run test-one -- tests/modules/components/overlay.test.ts --grep 'restores the popup transition' --timeout 15000`
(1 passing). A full Overlay run reached 50 passing but had three unrelated
async timeout failures under this machine's current load; the new regression
was rerun independently and passed. Browser visual confirmation remains
pending because the existing Chrome DevTools browser profile is already owned
by another running browser session.

The Icon page comparison also found that React v6's page has a trailing FAQ
section absent from Blazor. Both localized Blazor pages now include the same
question about icon styles and explicitly explain that React's CSS-layer
injection workaround does not apply to the generated Blazor SVG package. This
preserves the relevant topic without copying React-only implementation advice.

### Select interaction and Icon recheck (2026-10-29)

Restarted the React dumi server after its stale process served the HTML fallback
for `/umi.js`; both React and Blazor docs routes now load their runtime assets.
The Select interaction manifest was rerun against the live pages. Search/filter
and multiple-selection scenarios have matching normalized demo-root DOM trees,
computed styles, and geometry; both pass the visual-style gate (2/2). Semantic
popup subtrees still have attribute-level differences: portal positioning and
inherited token declarations are serialized differently on `popup.root`, and
the multiple-select check icon uses different SVG metadata/structure. These
remain DOM-parity follow-ups even though the rendered style and geometry gates
pass. Raw `outerHTML` also contains framework-specific whitespace. Report:
`/tmp/select-interaction-turn-final-2026-10-29.json`.

The five Icon manifest scenarios (font, font resources, basic, two-tone and
custom color) pass normalized DOM, computed CSS, and visual-style checks (5/5,
zero blocked). Raw HTML serialization differences remain diagnostic. Report:
`/tmp/icon-parity-turn-final-2026-10-29.json`.

The Select popup bUnit regression suite also passes 22/22. Width assertions now
check the `width` and `min-width` declarations independently so the test
protects measured values and fractional precision without prescribing harmless
CSS declaration order or whitespace.

### Virtual Select scrollbar visibility parity (2026-10-29)

The current Select interaction run isolated one remaining popup subtree
difference: React's `rc-virtual-list` adds
`ant-select-dropdown-list-scrollbar-visible` on mount and after scrolling, and
hides the scrollbar after three seconds. Blazor previously only toggled inline
visibility for 500 ms during scroll and never reflected the visible class. The
Select JS helper now matches the class and timer behavior. Its focused helper
test passes, and the live Select interaction manifest passes both scenarios
(2/2; zero DOM, computed-style, geometry, or visual-style differences). The
updated Blazor Select page remains open at the multiple-selection demo for
manual inspection. Browser report: `/tmp/select-interaction-parity-after-scrollbar-fix-2026-10-29.json`.

### Icon gallery toolbar and Two Tone palette (2026-10-29)

The Icon gallery toolbar now uses the shared `Affix` component at an 80px top
offset, matching the React docs' fixed search/filter bar. Browser inspection
after scrolling confirmed the affix wrapper is `position: fixed; top: 80px`.
The Two Tone filter applies the standard Ant primary blue to every gallery
icon; interactive browser inspection found `rgb(22, 119, 255)` and the
expected palette fills `#1677ff` / `#e6f4ff` across the displayed icons.

The Select whole-page scan was repeated against a fresh Development server
circuit after the earlier stale circuit captured only a skeleton. All 36 React
Select demo names now match, with 15 Blazor-only examples. The current scan
records 20 HTML, 15 computed-style, and 10 geometry candidate differences;
none of the React demos were missing. The `Tags` and `Coordinate` demos now
match normalized HTML, computed styles, and geometry exactly; their differences
were caused by non-matching tag value generation and the missing `SpaceItem` /
province-city value labels. Updated inventory:
`runtime-parity-scan-2026-10-29-visible.json`.

Browser recheck on the Icon page found that the server-driven Affix wrapper was
not applying its fixed style in the current docs scroll layout. The gallery
toolbar now uses CSS sticky positioning at the same 80px header offset so it
remains available while scrolling without waiting for a Blazor scroll event.
Every generated Two Tone icon now also receives the same `#1677ff` primary
color parameter; the earlier wrapper text-color rule alone did not recolor SVG
paths that declare explicit fills.

### Unmatched Select value clear behavior (2026-10-29)

The React-aligned Option Label Center demo renders a controlled string value
that intentionally has no matching option. Select now treats that raw value as
clearable and raises `ValueChanged` when its clear icon is used; the demo uses
two-way binding so the value returns to its placeholder state. The focused
regression test invokes the same DOM clear callback used by the browser and
passes. The whole-page scanner was rerun, but its Blazor capture contained
skeleton placeholders, so its runtime HTML counts were not accepted as parity
evidence; the most recent hydrated scan above remains the runtime baseline.

### Select Option Label Center browser interaction (2026-10-29)

The earlier browser attempt did not reach the target demo because the local
Docs Server was running as Production and lacked the generated `AntDesign`
interop namespace. The resulting `getWindow` JS exception disconnected the
Blazor circuit and stopped demo mounting at two code boxes. Restarting the host
with `ASPNETCORE_ENVIRONMENT=Development` restored the interop assets; all 51
Select code boxes mounted. In the Option Label Center example, the first Select
showed the unmatched value `long, long, long piece of text` and its clear
control. Clicking the clear control removed the label and clear affordance,
confirming the bound value was cleared. The focused regression test passes
1/1. The browser remains on this Select example for manual review.

### Select sizing and clear-focus recheck (2026-10-30)

Against the current Development build on port 5002, the Select Basic popup
manifest passed all three closed/open scenarios, and the Size manifest passed
Default, Large and Small (6/6 total with exact semantic CSS and geometry gates).
The Option Label Center clear interaction also passes its visual-style gate:
after clearing the unmatched value, React and Blazor both retain input focus,
the focused border token and the two-pixel focus shadow. The Blazor clear
button now prevents its default mousedown focus transfer and Select restores
input focus after the clear callback. Raw markup still has expected Razor
serialization differences.

The first automated Search captures sometimes blurred the Blazor input while
switching back to React, and a second demo scroll changed the popup's viewport
sample. The Search fixture now captures React before activating Blazor, waits
for the focused state after filtering, and the runner avoids scrolling during
capture. Two consecutive Search runs now pass all DOM, CSS, and geometry checks
(1/1 per run); the Lucy option is visible and the other filtered options remain hidden.
The broader Select bUnit slice is not green yet (90/111); strict JS interop
reports unconfigured Overlay helper calls in legacy tests, and the Basic demo
parity test has a stale component-query expectation after its data-source
migration.

### Select interaction and Two Tone source-color policy (2026-10-30)

Select interaction parity was rerun on the current Development server after
preventing option mousedown from stealing focus and aligning virtual-list
scrollbar visibility with the rendered popup. Search, unmatched-value clear,
and multiple-selection scenarios pass the DOM, computed-style, geometry, and
visual-style checks (3/3, zero differences). Focused Select virtual-scroll
tests pass 8/8; the full StyleGen Node suite passes 206/206.

Two Tone icons now preserve each SVG's original primary and secondary fills
when no color override is supplied. Supplying one primary computes the
secondary color from the shared Ant v6 palette; supplying both colors applies
the explicit pair. The Icon docs' main Two Tone example explicitly supplies
React's standard `#1677ff` primary, while the dynamic-color demo visibly shows
the source colors, calculated secondary, and explicit pair. The five React
Icon examples pass the runtime DOM/style and visual-style comparison (5/5,
zero differences), and the Two Tone color unit suite passes 23/23. Docs Server
net10 build succeeds with zero errors. The local Development site remains
available at `http://127.0.0.1:5002` with the Icon and Select pages open for
manual review.

## Table virtual API naming — 2026-11-15

Added the React-compatible `Virtual` parameter to `Table` and routed the
component's virtual rendering, scrolling, layout, and CSS-class decisions
through it. `EnableVirtualization` remains as a deprecated compatibility alias
that reads and writes `Virtual`; the Table demo pages and both localized API
tables now use/document `Virtual`. The existing Virtual List browser page
renders the `ant-table-virtual` class and virtual holder with the new parameter,
and reports no console errors. Component-library, Docs, and Docs.Server
`net11.0` builds pass with 0 errors. A `netstandard2.1` build currently fails
on unrelated existing `OneOf`, `IReadOnlySet`, `EditorRequired`, and
`IJSObjectReference` compatibility errors elsewhere in the worktree.

### Select suffix icon ID parity (2026-10-30)

React does not add an ID to an icon wrapper unless the caller supplies one.
`Icon` now follows that behavior while retaining an explicit `Id` parameter.
This also prevents nested icons from consuming a generated ID ahead of Select's
combobox input. Focused Icon parameter and Two Tone tests pass 28/28, and the
Docs Server net10 build succeeds with zero errors. The Suffix demo was recaptured
against React after restarting the Development server: all three sizes have
matching computed CSS; structural differences now remain in the SVG source and
serialization (React's 896-unit viewBox / path source versus the current Blazor
1024-unit icon catalog). The Suffix page remains open in the browser for review.

### Icon / Select closeout and next component (2026-10-30)

The repeated Icon and Select changes came from treating separate checks as one
unresolved component gap. Icon's five canonical React demo groups pass rendered
DOM, computed CSS, and visual-style comparison. Select's Basic popup, sizes,
Search, unmatched-value clear, and multiple-selection browser scenarios also
pass their DOM/CSS/geometry checks. Remaining Select issues are isolated to
legacy bUnit coverage and stale demo-structure assertions; they do not reproduce
in the current browser parity scenarios.

The Select test failures were initially inflated because Overlay gained JS
interop calls for pre-show width measurement, inherited token propagation,
reveal, and repositioning, while the shared bUnit fixture had no defaults for
them. The fixture now supplies layout-neutral defaults. A focused search test
still times out waiting for its legacy `OnSearch` callback, so the suite is not
marked green. The Icon Suffix capture also retains a source-level limitation:
computed styles match, while SVG markup differs because React's icon source uses
a 896-unit viewBox and the Blazor catalog currently uses 1024.

Typography's demos are discovered from `TypographyBase/demo`, while the prose
page lives in `Typography/doc`; this folder mismatch hid React examples. The
rebuilt page now registers all 14 React demo names, and the browser shows the
new examples. Divider is the next component under review; its current rebuilt
page is open in the browser.

### Select / Icon documentation recheck and mobile docs fixes (2026-10-30)

The Select MaxCount demo was exercised in the browser with its controlled values:
the count advanced from 1/3 to 2/3 and 3/3, a fourth option did not change the
selection, and removing a selected tag returned the count to 2/3. This confirms
the demo's two-way selection and external value state update.

The React Icon gallery search/filter bar uses an affixed position at `top: 80px`
while scrolling. Blazor's sticky toolbar was at `top: 24px`; it now uses 80px.
The refreshed Blazor browser reports the toolbar at y=80 during scroll. The
Two Tone demo retains the source fills (`#333` / `#E6E6E6`), computes
`#f6ffed` from primary `#52c41a`, and applies explicit `#52c41a` / `#1677ff`
when both colors are passed.

The current React/Blazor Icon manifest passes all 5 demos after waiting for
Iconfont's external SVG symbols to settle (5/5 DOM, CSS, and visual checks).
The Select interaction manifest passes Search, clear, and multiple-selection
scenarios (3/3, zero differences); the Suffix manifest also passes (1/1).
The MaxCount behavior was separately exercised through browser clicks as noted
above. A first Icon run observed a transient duplicate-symbol ordering difference
from the asynchronous iconfont resource; the stable rerun and direct browser
measurements agree on symbol order and rendered dimensions.

The mobile docs header menu was checked at 390px: its arrow center matches the
trigger center, and the Sider trigger icon is centered inside its 40px button.
The Upload demo no longer renders preprocessor directives as page text. The
Docs Server `net10.0` build succeeds with zero errors; runtime parity remains
in progress and the 2026-10-29 whole-site scan is only a baseline, not a fresh
completion audit. The Development site is available at `http://127.0.0.1:5011`
with the Icon Two Tone example open for manual review.

Next: continue the component-by-component runtime DOM/CSS/interaction review,
using the documented scan inventory to select the next unresolved component.

### Divider runtime pilot (2026-10-30)

The Divider manifest was rerun against React on port 8000 and the current
Development Blazor server on port 5011: all 9 groups pass rendered DOM,
computed-style, geometry and visual-style comparisons. The component-token demo
serializes token values through React's generated class and Blazor's inline
`style` attribute, so the manifest now normalizes the raw `style` attribute
while retaining the explicit computed CSS and visual checks. Final result:
9/9 passed, zero differences or blockers.

### FloatButton runtime inventory (2026-10-30)

A fresh one-component inventory against React port 8000 and Blazor port 5011
found 18 React demo names and 16 Blazor code-box demos, with 16 matching names.
The inventory labels `_semantic` and `_semantic_group` as missing because the
Blazor semantic previews render as documentation tables rather than
`.code-box-demo` cards; both semantic tables are present after enabling the
debug view. Of the 16 matched demos, only the internal `render-panel` sample
has candidate DOM/CSS/geometry differences; the other 15 match the scanner's
selected properties and 1px geometry tolerance. That internal sample should be
reviewed separately before deciding whether its output is part of the public
parity scope.

### Documentation mobile navigation follow-up (2026-10-31)

After rebuilding and restarting the Development server on port 5006, the
collapsed Sider trigger is positioned at 64px below the viewport top and its
40px button centers the icon. The header navigation continues to use Popover's
Overlay `InView` boundary adjustment. With the menu open at a 545px viewport,
the popup stays within the viewport (240px wide) and the arrow center matches
the trigger center exactly (x=513px). The ConfigProvider page body no longer
contains the raw `#if` / `#elif` / `#endif` text shown by the stale server
instance. The Docs Server net10 build succeeds with zero errors; the existing
warning set remains.

### Docs theme control and BackTop visibility motion (2026-11-01)

The theme selector lives in the header utilities, matching React's top-menu
placement. The global floating widget renders only BackTop. BackTop opacity
transitions for 200ms (the Ant Design `motionDurationMid` timing); it becomes
non-interactive as it fades out, then `visibility:hidden` is applied after the
transition. Re-crossing the visibility threshold cancels the pending hide and
reverses from the current opacity. On the FloatButton docs route, browser
interaction confirmed it starts hidden, appears after scrolling past the
threshold, remains visible during fade-out, and becomes hidden when the fade
completes. The BackTop click also returned the page to scroll position zero.
TypeScript bundle generation and the .NET 10 docs build succeeded; the
Development server is available at
`http://127.0.0.1:5014/en-US/components/floatbutton` for review.

Follow-up browser review found the theme dropdown inherited the 32px trigger
width, clipping its labels. The popup now has a content-sized width with a 160px
minimum (bounded by the viewport). BackTop now starts or reverses its opacity
animation only when it crosses the visibility threshold, rather than restarting
the animation for every scroll event. On port 5014 the menu opens at 160px wide;
BackTop was observed at intermediate opacity during fade-in and fade-out, became
hidden after fade-out, and returned the page to `scrollY = 0` when clicked. The
latest .NET 10 build succeeded with zero errors (492 existing warnings).

The 2026-11-01 follow-up rechecked the current development page on port 5000:
the header theme menu opens with Light, Dark, Compact, and Happy Work options;
switching to Dark and back to Light works. The global bottom-right control is a
BackTop FloatButton with `visibility:hidden` and `opacity:0` at the top of the
page, and its computed transition is `opacity 0.2s linear`. No theme control is
rendered in the floating widget.

### Select Big Data and Icon parity recheck (2026-11-01)

Rebuilt and restarted the .NET 10 docs server after a fresh Select virtual
list capture exposed an accessibility-only mismatch: React emits
`aria-disabled="false"` on enabled virtual listbox options. Blazor now emits
the same explicit ARIA value. The final React/Blazor Big Data gate passes 1/1:
popup visible in 775ms on React and 1212ms on Blazor in the final run (a 437ms
delta; a prior run was 13ms), rendered DOM and computed styles match, and the
visual-style gate passes with zero differences. The timing variance indicates
that opening latency is not yet stable enough to close as fully aligned.

The Icon manifest was rerun against the same live servers and passes all 5/5
demo groups with zero DOM, CSS, or visual-style differences. The docs server
build completed with zero errors. The user-facing browser remains open on the
FloatButton docs page; the Select Big Data gate used a separate automation
browser session.

### AutoComplete demos and variants (2026-11-01)

Added the React `status`, `allowClear`, `options`, and `variant` demos, and
extended Input/AutoComplete with a custom clear-icon template. AutoComplete now
passes its requested variant through to the underlying Input, so Filled and
Underlined render their matching Input classes. Refreshed local generated docs
metadata through `demo2json`; the metadata JSON is ignored build output.

The .NET 10 Docs Server build succeeds with zero errors. Browser checks confirm
the four demo cards render, dynamic email suggestions appear for typed input,
error/warning status classes are applied, and all four variants render their
corresponding input classes at a 200px root width. The custom `close-square`
clear icon is accessible as a button and clears the input when clicked. This
was a component demo/interaction check; a full React DOM/CSS manifest comparison
for AutoComplete remains pending. Next component: ConfigProvider, the largest
remaining demo gap in the 2026-10-30 baseline inventory (11 React demos).

### Full runtime inventory and current phase 1 status (2026-11-01)

The complete 71-route browser inventory is recorded in
[`runtime-parity-summary-2026-11-01.md`](runtime-parity-summary-2026-11-01.md)
and its per-demo DOM/CSS/geometry snapshots are in
[`runtime-parity-scan-2026-11-01.json`](runtime-parity-scan-2026-11-01.json).
The summary has one status row for every matched component. Missing demo counts
use React demo names; Blazor-only demos do not create a deficit. The scan found
206 public React demo names absent from Blazor. It separately records 60
underscore-prefixed `_semantic*` preview markers, represented by the page-level
Semantic DOM catalog/preview rather than public code-box demos.

Phase 1 generated coverage remains complete for the recorded scope: 72/72 CSS
component migrations, 3/3 algorithms, 61/61 Semantic DOM catalog components,
70/70 definitions, 423/423 API slots and 70/70 structure mappings. Component
token metadata has no detected parity mismatches. These generated-data counts
do not imply runtime visual parity; the raw runtime CSS totals include reset
noise such as `border-style` differences on zero-width borders, so review each
sample before changing a component.

The inventory confirms Select has all 36 React demo names represented (zero
public name gaps), but its broad sample set still has runtime differences. The
existing Big Data-specific parity gate remains the stronger targeted result:
DOM, computed styles and visual styles pass, while popup latency was variable
between runs. Icon passes all 5 React demo groups with zero captured DOM, CSS,
and geometry differences. Next, close the outstanding Select-specific suffix,
variant and preview/runtime mismatches, then take the largest public gaps from
the summary (ConfigProvider, Dropdown, ColorPicker, InputNumber and Mentions).

The docs metadata was regenerated with `demo2json`, the .NET 10 Docs Server
build succeeded with 0 errors, and the server was restarted with its static web
asset manifest on `http://127.0.0.1:5000`. Browser review confirmed the header
owns the theme menu, the FloatButton docs page exposes its Semantic DOM preview,
and BackTop fades through intermediate opacity values before becoming hidden;
clicking it returned the page to the top. The browser is left open on the
FloatButton page for manual review.

### Select and Icon interaction recheck (2026-11-01)

Reran the Icon runtime manifest: all 5/5 demo groups pass, with DOM trees,
computed styles, geometry, and visual-style checks equal. Select interaction
manifests cover search filtering, multiple selection, and clearing an unmatched
value. All 3 scenarios now reach their intended UI state with no blockers; DOM
trees, computed styles, Semantic DOM nodes, and visual styles match. Two groups
retain byte-level HTML differences because the Blazor server renderer emits
different whitespace/serialization; their normalized DOM trees are equal.

Rechecked Select Basic in the visible browser on React and the current Blazor
host (port 5007). Clicking the first enabled Select opens both popups. Both
render four options (`Jack`, `Lucy`, `yiminghe`, `Disabled`) in a 120 x 136 px
popup; popup x-coordinate matches, and trigger-to-popup vertical spacing differs
by less than 0.1 px. The automated Basic popup capture still fails to open the
Blazor control in its headless run, so this capture remains a runner issue to
resolve; the closed-state automated gate passes all four roots and Semantic DOM
slots. The parity fixture now clicks the actual input and its popup resolver
accepts an input element carrying `aria-controls`, but this alone did not remove
the headless capture failure.

The React and Blazor Select pages remain open in the browser for manual review.
Next: resolve the Basic popup automation capture, then continue with ConfigProvider,
which has the largest remaining public React demo-name deficit in the inventory.

### FloatButton Group square spacing browser verification (2026-11-01)

The Square group had been rendered with the compact-list class, which collapses
the inter-button spacing and joins adjacent borders. That class is now omitted
for non-menu groups, and the list uses the token-driven 16 px spacing. The
visible browser page and a fresh Development-host frame capture confirm four
40 x 40 px Square items, 16 px vertical gaps, and an 8 px radius; the Circle
group retains its circular shape and 16 px spacing. React v6 currently renders
Square through `Space.Compact` with zero gap, so this requested visual behavior
is an intentional user-directed difference from React. The browser is open on
the updated FloatButton Group demo for manual review.

### Select Basic headless popup capture follow-up (2026-11-01)

Fixed one collector readiness issue: body-scoped portal cases now scroll the
actual demo anchor before waiting for lazily mounted React demos. Select Basic
closed-state comparison passes all four Select roots and Semantic DOM slots.
The Escape scenario can still intermittently miss the Blazor open state in the
server-rendered run.

Popup capture remains an actual runtime difference. The Blazor trigger receives
`ant-select-open` and reports `aria-expanded="true"`, but the captured visible
portal set is empty; its trigger also lacks React's focused class and focus ring.
The automated click-to-visible measurement has varied from about 1.3s to 17.2s
for Blazor, so timing is not yet reliable. The parity runner now preserves the
pointer state during open-popup captures and processes React before Blazor, but
that did not eliminate the portal/focus discrepancy. Continue with direct
trigger-to-portal verification before changing Select behavior. Icon remains
5/5 passing. The complete 71-component inventory and remaining demo names are
in `runtime-parity-summary-2026-11-01.md`.

### Select Basic first-paint and runner follow-up (2026-11-01)

On the Development docs server at port 5014, a real browser pointer click on
Select Basic produced the open class and expanded state within 71 ms; the
popup was visibly painted in the same browser observation. At that first frame,
`aria-controls` had not yet been written, confirming that portal timing based
only on this attribute can miss the immediate-open frame. The runner now uses
instant scrolling for demo setup and can correlate a pre-ARIA popup to the
trigger; `node --check` and `git diff --check` pass. However, an independent
headless run still did not consistently hit or detect the popup (the measured
duration was null after a post-mount settle), and the open capture still differs
on the React focus class. Keep this as unresolved Select automation/runtime
work; do not count Select as visually aligned yet. The browser remains on the
Select page with Basic open for manual review.

### Select popup capture reliability follow-up (2026-11-01)

The parity runner's post-readiness `scrollIntoView` now explicitly uses instant
behavior too; the docs page applies smooth scrolling globally, and the omitted
behavior left a second scroll animation running into clicks and captures.
Independent Chromium 154 inspection confirms the Blazor Basic popup opens with
four options, 120 px width, and the same horizontal anchor as its trigger. Its
initial option-panel position is directly below the trigger.

The fresh two-page parity run still does not reliably open the Blazor Basic
trigger. In those failed captures it selected a different, already-open
Semantic DOM popup (three options), so those popup DOM differences are invalid
comparisons. The collector now supports binding a portal to the popup ID named
by the trigger's `aria-controls`/`aria-owns`; the Basic fixture uses this exact
relationship instead of selecting whichever portal is geometrically nearest.
The closed-state Basic comparison passes, but the automated open-state capture
remains unresolved until its Blazor interaction path consistently opens the
intended trigger. Do not count Select as fully runtime-aligned yet.

### React-first Select API names and inherited API inventory (2026-11-02)

Added Select parameters matching React's public names, using Blazor PascalCase:
`Options`, `ShowSearch`, `Virtual`, `PopupMatchSelectWidth`, `PopupRender`,
`ShowArrow`, and `OnOpenChange`. They delegate to the existing data source,
search, virtualization, width, render, arrow, and visibility implementations;
existing parameter names remain available for compatibility. The `showSearch`
alias supports React's boolean form and allows explicitly disabling search in
multiple/tags mode; React's object search configuration still needs a dedicated
Blazor configuration model.

The API inventory previously omitted parameters inherited from shared Blazor base
classes. Updated `extract-blazor-api-to-json.mjs` to follow component inheritance
chains before mapping names. Fresh extraction records 2,234 Blazor parameters and
the React-first API map now reports 765 exact names, 50 explicit aliases, and
1,048 unmapped properties across its 1,863 extracted React properties. This map
does not include inherited `rc-select` props absent from the React source
declaration extractor (including `options` and `virtual`); the Blazor Select API
does expose those canonical names.

AutoComplete also exposes `OnOpenChange` as the React-compatible callback
while retaining `OnPanelVisibleChange` for existing callers. Its English and
Chinese component docs now identify the canonical name.

`dotnet build components/AntDesign.csproj --no-restore -f net10.0` succeeded.
The multi-target build remains blocked by existing errors in older target
frameworks (`EditorRequired`/`IReadOnlySet`/`IJSObjectReference`) and missing
`IconCatalog.GetSourceSvg` generated API, outside this Select API change. Select
popup runtime capture remains unresolved as documented above.

### React-first Dropdown API naming and parameter inventory (2026-11-02)

Aligned Dropdown's remaining custom-render callback name with the React API:
`DropdownRender` is now available as a compatibility alias for `PopupRender`,
and the renderer prefers `PopupRender` when both are supplied. `PopupRender`
continues to match React's current name; `DropdownRender` preserves the
deprecated React `dropdownRender` name for migration. The shared trigger
parameter is `Trigger`, while controlled state and callbacks use `Open` and
`OnOpenChange`.

Fixed the Blazor API scanner so long XML comments and inline declaration
comments do not hide inherited parameters. The fresh inventory contains 76
components, 2,239 parameters, and 309 callbacks. The React mapping records
767 exact names, 50 explicit aliases, and 1,046 unmapped extracted React
properties. For Dropdown, `trigger`, `popupRender`, `dropdownRender`, `open`,
and `onOpenChange` all map to public Blazor parameters. Several remaining
React props are broader shape differences (notably its object-based `menu`)
and need API design work rather than name-only aliases.

Both the component and docs server builds succeeded for `net10.0`. The docs
metadata was regenerated and the served Dropdown API table contains both
`DropdownRender` and `PopupRender`; the Dropdown page is open in the browser.

### Dropdown `menu` data API shape follow-up (2026-11-02)

Expanded the React-style `Menu` configuration with `activeKey`, per-entry
classes and inline styles, and dashed dividers. Menu item, group, and submenu
data is rendered through the existing Blazor Menu components; the original
`Overlay` child-content path remains available for custom markup. The API map
now marks `menu` as shape-partial rather than treating the case-insensitive
name match as proof of complete `MenuProps` parity. React exposes additional
menu behaviors that are not represented by the current options model and stay
visible as follow-up API work.

The docs server returns 200 and renders the API tables, but the Basic demo is
still showing its lazy-load skeleton in the browser, so its interactive popup
could not yet be verified in this pass. Do not count the data-driven demo as
browser-verified until the runtime demo mounts and its menu opens.

After adding the missing parameters, a fresh inventory records 76 Blazor
components, 2,243 parameters, and 309 callbacks; the React map reports 770
exact-name matches, 50 explicit aliases, 1,043 unmapped properties, and one
explicit shape-difference note.

### React-first API inventory and Dropdown name parity follow-up (2026-11-02)

Corrected the Blazor API extractor to include inherited public parameters through
the component base-class chain. The regenerated inventory now contains 76
components, 2,591 parameters, and 318 callbacks. Against 1,863 extracted React
properties across 75 components, the explicit mapper finds 853 exact names, 96
aliases, 914 unmapped properties, and two shape differences. These are API-name
inventory figures, not a count of runtime-verified feature gaps; inherited
third-party React props and unsupported components still require review.

Dropdown now supports React's `openClassName` behavior by applying that class to
its trigger wrapper while the popup is open. The remaining Dropdown `align` and
`autoFocus` differences are recorded as shape notes: React accepts arbitrary
alignment objects and focuses the popup menu on open, while Blazor currently
offers placement/scalar offsets and has no equivalent popup focus management.
The latest map therefore reports those as explicit follow-up capabilities,
rather than silently treating similarly named parameters as equivalent.

### React-first Select API aliases (2026-11-02)

Select now exposes React-compatible `RootClassName`, `PopupClassName`,
`DropdownClassName`, `DropdownStyle`, and `DebounceTimeout` parameters, and a
custom `LoadingIcon` fragment. Root and popup classes/styles are applied to
their respective DOM elements; the legacy dropdown class name remains accepted.
The API map no longer lists these six Select properties as missing. React's
`fetchOptions` remains a real capability gap: Blazor still uses caller-managed
`DataSource` plus `OnSearch`, without an asynchronous fetch contract.

The newest API inventory records 2,609 Blazor parameters and 318 callbacks;
the React map reports 867 exact names, 96 aliases, 900 unmapped properties, and
five explicit shape differences. The generated all-component progress table is
in [sync-progress-2026-11-02.md](sync-progress-2026-11-02.md); its runtime
columns still reference the 2026-11-01 capture baseline.

### React API naming parity follow-up (2026-11-02)

Added React-named parameter aliases to Drawer and Modal (`Open`/`OpenChanged`)
while preserving `Visible`/`VisibleChanged`, and to Progress (`RailColor`) while
preserving `TrailColor`. The aliases share the same underlying state/render
behavior; closing a Drawer or Modal invokes both change callbacks when supplied.
Fresh API extraction and mapping now report Drawer 16 exact / 1 alias, Modal 31
exact / 1 alias, and Progress 16 exact / 1 alias. Global mapped counts are 870
exact, 93 aliases, 900 unmapped entries, and 5 shape differences. The remaining
unmapped total contains many internal implementation declarations, so it is
not a count of missing public component features.

The component library builds for .NET 10 with zero errors (396 existing
warnings). Dropdown's public `align` object remains a real shape difference and
needs a dedicated alignment model before it can be considered React-compatible.

### Select popup state and runtime recheck (2026-11-02)

The Basic popup interaction revealed that the pre-open JavaScript could show the
portal before the Blazor Server callbacks completed. The document-level
`mouseup` callback could then arrive after the trigger click and interpret that
same in-trigger mouseup as an outside click. `OverlayTrigger.OnMouseUp` now
checks the event coordinates against the trigger rectangle before closing.
Select also refreshes its root class map and cascading popup/ARIA state after
`OnVisibleChange`, so the root `ant-select-open` class and combobox attributes
follow the actual overlay state.

Updated the Select popup fixture to wait for a visible popup in the Escape
scenario. One clean serial run against React :8000 and Blazor Development :5025
passed 3/3 scenarios (closed, Escape close, popup open), with 0 DOM, computed
CSS, and visual-style differences. The snapshot is
[`select-basic-runtime-parity-2026-11-02.json`](select-basic-runtime-parity-2026-11-02.json).
A follow-up timing run captured the popup DOM correctly but returned a null
Blazor paint-time measurement; another retry encountered a cold React demo
mount and an ARIA-state timeout. Therefore the interaction correction has a
clean parity run, while opening-time stability remains open and needs another
serial browser pass.

### Select first-open focus state recheck (2026-11-03)

On the Development server at `:5030`, the closed and Escape-closed scenarios
passed with 0 DOM/CSS/visual differences. The popup opened, but the automation
intermittently checked `.ant-select-focused` before the immediate pointer-down
path had completed its focus round trip. The Select open path now focuses its
input when the popup becomes visible. Manual Chromium inspection then exposed a
second issue: rebuilding the root class map could drop `ant-select-open` after
focus arrived. Select now also records the visibility callback state and uses
it when rebuilding root classes. After rebuilding and starting a fresh
Development server on `:5033`, Chromium confirmed that the popup remains
visible, the input is focused, and the root retains both `ant-select-focused`
and `ant-select-open`. Closed and Escape-closed automated scenarios still pass
with 0 DOM/CSS/visual differences; the popup runner's immediate focus assertion
remains timing-sensitive, so it is not counted as an automated pass.

The 2026-11-03 rerun found that the popup manifest's focus selector used a
descendant combinator even though `ant-select-focused` belongs to the Select root.
The selector now targets `.ant-select.ant-select-focused`. With that harness
error corrected, the first fresh runner pass captured a transient scroll offset
after Select restored input focus. Programmatic focus now uses
`preventScroll: true`, so the browser cannot scroll the trigger after the popup
has measured its position. The subsequent serial runner pass against React `:8000`
and Blazor `:5000` passed all 3 scenarios (closed, Escape close, popup open), with
0 DOM-tree, computed-style, or visual-style differences. The popup top was exactly
4px below the trigger on both sides. The visible-popup timing was 532ms for React
and 98ms for Blazor in this run; this is a single-run observation, not a stable
benchmark. Report: `/tmp/select-basic-popup-prevent-scroll-report.json`.

### Icon gallery long-name clipping (2026-11-03)

Inspection of React's current `.dumi/theme/builtins/IconSearch/Category.tsx` and
`CopyableIcon.tsx` confirms that its gallery has no responsive column breakpoints:
the list always uses `repeat(6, minmax(0, 1fr))`. Each item is 100px tall and
`overflow: hidden`; the name is `white-space: nowrap`, centered, and scaled to
`0.8`. Therefore names wider than a grid cell are clipped in React too; the
implementation does not preserve the full label on narrow cells. Blazor initially
matched this behavior, but that still clipped names such as `StepBackwardOutlined`
at a 641px viewport. To meet the requirement that icon names remain readable, the
Blazor gallery now uses 4/3/2 columns at 992/767/480px breakpoints and permits
long names to wrap inside the tile. Labels now use an actual 11px font instead
of scaling a 16px line box, preventing the oversized layout box from leaving a
single character on its own line. The tile height was changed from a fixed 100px
to `min-height: 100px`, so additional wrapped lines can grow the tile instead of
being clipped by its fixed height. At 320px and 250px browser viewports, the page
uses two 130.5px and 95.5px columns respectively. A DOM measurement of all 842
icons across six categories found zero items whose scroll height exceeds their
client height. This intentionally improves on React's current clipping behavior.
The source review does not replace a fresh full Icon/Select parity scan.

### Icon gallery filter toolbar (2026-11-03)

React renders the gallery filter as an Ant `Segmented` with four options, each
containing an icon wrapper and label. Blazor used a `RadioGroup` and showed the
Appstore glyph for every theme. The gallery now uses the shared `Segmented`
component and the matching Appstore, outlined-square, filled-square, and half-tone
SVGs. The custom SVGs carry React's `1em` dimensions and paint/accessibility
attributes. A focused Chromium comparison at a 673px viewport found four roots on
both sides; option widths match exactly (63.664, 109.25, 85.281, and 115.922px),
and DOM-tree, selected computed-style, and visual-style comparisons pass. The
runner normalizes `name` and `checked` because generated radio names differ and
Blazor reflects checked state as a DOM property; browser interaction separately
confirmed that the four inputs share one name and exactly one remains checked.

This comparison also exposed two shared `Segmented` implementation gaps. Blazor
now emits `ant-segmented-item-selected-text` for the selected option and gives
radio inputs one generated group name, matching React's selected class and native
radio grouping. In-browser selection produced 445 Outlined, 249 Filled, 148 Two
Tone, and 842 All entries; every filtered icon had the expected suffix. Report:
`/tmp/icon-segmented-toolbar-5046-report.json`. The Icon page remains open at
`http://127.0.0.1:5046/en-US/components/icon#components-icon-demo-list` for manual
review. This focused toolbar check does not replace a full-page Icon/Select scan.

### Dropdown current-site inventory and Context Menu correction (2026-11-03)

A fresh route/demo inventory against React `:8000` and Blazor Development `:5046`
found 74 routes on each side, 71 matched components, and no blocked pages. Dropdown
has 22 React demos, 23 Blazor demos, 21 matching ordinary demos, and two Blazor-only
demos; React's `_semantic` is a Semantic DOM preview, not a missing ordinary demo.
The 21 matched demos still show HTML differences in all 21, selected computed-style
differences in 9, and geometry differences in 9. Report:
`/tmp/dropdown-current-site-5046.json`.

The Context Menu demo previously forced a 200px trigger and hard-coded colors; React
fills the demo container and reads `colorBgLayout`/`colorTextTertiary`. Blazor now
lets the trigger fill the available container and uses the matching Ant Design CSS
variables. A focused browser comparison shows matching computed text/background
colors and a remaining 0.016px width delta (342.656px Blazor, 342.672px React).
The raw DOM `style` attribute also differs because React serializes resolved token
values while Blazor keeps the theme-aware CSS variables. A real right-click in the
rebuilt Development page opens the three-item menu at the pointer. Docs.Server net10
build succeeded (0 errors, 496 warnings); the Context Menu remains open with its menu
visible for manual review. Focused report: `/tmp/dropdown-context-menu-5046-report.json`.

### Icon current-site demo and responsive gallery check (2026-11-03)

The current Icon fixture was rerun against React `:8000` and Blazor Development
`:5046`: icon-font, icon-font-resources, basic, two-tone, and custom all passed
(5/5; 0 DOM-tree, selected computed-style, or visual-style differences; 0
blocked). The temporary report is `/tmp/icon-current-site-parity-5046-report.json`.
At the 558px responsive viewport, React still uses six 42.5px columns; the
`StepBackwardOutlined` card is only 42.5px wide while its scaled nowrap label
occupies about 134px, and the card clips it with `overflow:hidden`. The Blazor
gallery uses three columns (154.3px each) at the same width and all 842 labels
fit without horizontal or vertical overflow. This confirms React's clipping is
not responsive label sizing; Blazor's wrapping behavior is the intentional
readability correction requested by the project. React and Blazor Icon pages
remain open for manual review.

### Select Basic current-site rerun — 2026-11-03

Reran the Basic closed, Escape-close, and popup-open scenarios against React
`:8000` and the running Blazor Development site `:5046`. The first run exposed
an automation click path that did not dispatch a real pointer event; the runner
now measures the target and clicks its center with Chromium's mouse API. That
revealed a delayed input focus callback scrolling the page after popup placement.
`SelectContent` now restores focus with `preventScroll: true`. The fresh run passed
all 3 scenarios with 0 DOM-tree, selected computed-style, or visual-style
differences and 0 blocked scenarios. The popup's trigger-relative position was
identical: left -0.656px, top 35.516px, width 120px, height 136px. Initial report:
`/tmp/select-basic-popup-preventscroll-5046.json`. After rebuilding Docs.Server for
the Dropdown correction, the same three Select scenarios passed again (3/3, 0
DOM/CSS/visual differences, 0 blocked) on the current `:5046` service; report:
`/tmp/select-basic-popup-current-5046-report.json`. The one-run popup timings vary
between runs and should not be treated as benchmarks. The Select page remains open
for manual review.

### Select size and ConfigProvider default precedence (2026-11-03)

The Select size demo initially showed a genuine runtime mismatch: choosing
`Large` or `Small` updated the radio selection but all three Select controls
remained at the default 32px size. React applies the explicit `size` prop over
the ConfigProvider default. In Blazor, `AntInputComponentBase.FormSize` was
unconditionally assigning its cascaded value to `Size`, overwriting an
explicitly supplied component parameter. The base now detects whether the
current `ParameterView` contains an explicit `Size`; the cascade is used only
when that parameter is omitted. The docs server rebuilt successfully for
net10. Browser interaction confirmed `Large` applies `ant-select-lg` to all
three controls, and switching to `Small` applies `ant-select-sm` to all three
and removes the large class. The default/large/small React comparison then
passed 3/3 scenarios with 0 DOM-tree, selected computed-style, or visual-style
differences and 0 blocked scenarios. Runner fixture clicks the visible radio
button wrapper and checks the corresponding native input. Report:
`/tmp/select-dom-5046-report-verified.json`.

### Select focused interaction and Semantic DOM regression set (2026-11-03)

After the size precedence fix, the current React/Blazor runs also passed the
Select Semantic DOM closed and filled-popup scenarios (2/2), suffix behavior
(1/1), and search filtering, multiple selection, and unmatched-value clear
interactions (3/3). Each scenario has matching parsed DOM trees, selected
computed styles, and visual-style results, with no blocked scenarios. Reports:
`/tmp/select-semantic-5046-report.json`,
`/tmp/select-suffix-5046-report.json`, and
`/tmp/select-interaction-5046-report.json`.

The virtualization popup scenario (`/tmp/select-virtual-5046-report.json`) also
has an identical parsed DOM tree, selected computed styles, and visual styles.
Its strict raw-HTML comparison still differs because Blazor emits indentation
whitespace between Razor elements while React serializes adjacent elements
without it; this is not a tag, attribute, text, or style difference. It remains
reported by the exact-HTML gate and is not counted as a zero-difference pass.
These focused cases do not cover all 18 Select documentation demos.

### Button Debug Color & Variant nested-provider layout (2026-11-03)

A fresh React/Blazor browser comparison initially found only one selected-style
difference: the first React Button computed to `display:flex`, while Blazor
computed `inline-flex`; their dimensions were already identical (106.359 × 32px).
The cause was the Blazor `ConfigProvider` layout wrapper (`.button-debug-global`)
between the Button and its Flex parent. React's provider emits no layout box, so
its Button is a flex item and its inline-level display is blockified to `flex`.
The Blazor demo now sets that provider scope wrapper to `display:contents`, which
keeps theme variables on the wrapper but lets its Button participate in the Flex
formatting context. A fresh Chromium run passed 1/1 scenario with 0 DOM-tree
differences within the selected Button roots, 0 selected-style differences, and
0 visual-style differences. Browser inspection confirmed the first Blazor button
computes to `flex`, has the same 106.359 × 32px geometry, and retains the custom
red link color. This is a demo-local layout fix; the provider `div` remains in the
raw DOM, so this check does not claim full page HTML parity. Build: Docs.Server
net10 succeeded with 0 errors (496 warnings). Report:
`/tmp/button-debug-color-variant-display-contents-report.json`. The Button page is
open on the Debug Color & Variant example at `http://127.0.0.1:5044/en-US/components/button#components-button-demo-debug-color-variant`.

## Space Compact demos and focused parity check — 2026-11-02

The React Space documentation has 16 demo names in the current extraction. Five Blazor demo source files already existed but were not discoverable because their Markdown files lacked bilingual `title` metadata; the generator therefore omitted them from both locale demo catalogs. Added titles and React-aligned order/debug metadata for `compact`, `compact-debug`, `compact-nested`, `style-class`, and `component-token`, then regenerated the local demo metadata. The Blazor Space page now renders these entries. The main Compact demo was expanded to 19 React-style compact groups using the corresponding real Blazor controls, and the invalid `Input.Search` element was corrected to `Search`.

A fresh 1280px browser capture reports matching outer Space widths and 19 direct Space items on each side (644px). The selected computed layout/style properties have no differences. The strict nested DOM comparison still reports real markup differences inside groups: input current values are properties rather than serialized attributes, some Blazor controls add trigger wrappers, and icon rendering differs. So this focused check confirms the missing demos, group count, outer geometry, and selected CSS, but it does not establish complete nested HTML parity for all 19 groups. The comparison fixture initially used the incomplete marker `<!--!--`, which left literal `>` characters in its parser input; that fixture was corrected to `<!--!-->` before interpreting the remaining differences. The temporary report and fixture are under `/tmp/space-compact-dom-style-parity-verified.json` and `/tmp/space-compact-dom-style-parity.json`.

## Splitter missing demos — 2026-11-02

Added the two React demos that were absent from the Blazor demo catalog. `nested-in-tabs` follows the React structure: two tabs with the Splitter in the second pane and three child panels, the first two collapsible. `debug` contains the same four cases as React: `[true, 0, false]`, `[false, 0, true]`, first panel min/max, and last panel min/max. Its metadata marks it as a debug example, consistent with the docs debug toggle. The regenerated locale catalogs now register `nested-in-tabs` and `debug`; the rebuilt browser page exposes the nested demo anchor and content. The docs project builds successfully for net10.0. The all-target build subsequently passed for net6.0 through net10.0 with 0 errors (2399 warnings); see `/tmp/antdesign-blazor-all-target-build-after-fixes.log`. Browser tab 9 was left open at the nested Splitter demo for manual review.

## Splitter size focused rerun — 2026-11-02

Reran the `splitter-size-basic` `default-render` scenario against the Development docs server on `:5028`, with the installed Chrome binary. The DOM tree and selected computed style properties matched (1/1 passed, 0 differences, 0 blocked); the report is `/tmp/splitter-size-dom-style-parity-report-dev.json`. The earlier Production-mode launch lacked static web assets required by the Blazor components, so that was a test-server configuration issue; the successful runner result uses the corrected Development launch.

## Breadcrumb items and route params — 2026-11-02

Added React-named `Items` and `Params` parameters and expanded `BreadcrumbOption` for title templates, links, and explicit separator entries. Route tokens such as `:id` now resolve in item titles and links. Added the React `withParams`, `withIcon`, and `separator-component` demos; the previous child-content icon example remains separately identified as a Blazor composition example. The demo catalog was regenerated and the Breadcrumb API map refreshed: its current React prop mapping is 6 exact, 2 aliases, and 9 unmapped (the remaining items include semantic `classNames`/`styles`, `itemRender`, dropdown menu options, and routing compatibility).

The fresh React/Blazor runner report is `/tmp/breadcrumb-item-dom-style-parity-2026-11-02.json`, with committed fixture `tools/AntDesign.StyleGen/examples/breadcrumb-with-params-dom-style-parity.json`. In 2/2 scenarios the normalized DOM trees match and no cases are blocked. The With Params computed style is an exact match; the per-separator example differs only in root width by 0.016px (342.672px React, 342.656px Blazor), while the other selected properties match. This tiny subpixel width delta remains reported instead of loosening the parity comparator. The browser review page is `http://127.0.0.1:5028/en-US/components/breadcrumb`.

## Icon gallery label clipping browser check — 2026-11-04

Inspected the live React Icon page and the current Blazor Icon page. At a 673px React viewport, its icon list remains six columns; 830 of 842 labels have a transformed label width larger than their 62px cell. The page uses `nowrap`, `scale(.8)`, and `overflow:hidden`, so those names are visibly clipped. At a 320px Blazor viewport, the gallery switches to two columns; all 842 labels fit their cells without horizontal or vertical label overflow. This confirms that Blazor's responsive wrapping avoids the React gallery's clipping behavior. The viewports differ in this check, so these counts establish each implementation's behavior rather than pixel parity at one common width. The Blazor Icon page is left open at `http://127.0.0.1:5052/en-US/components/icon#components-icon-demo-list` for manual review.

## Select 5052 popup retry and browser interaction — 2026-11-04

Reran the Select Basic fixture against React `:8000` and the currently running Blazor
Development server `:5052`, using the installed Chrome executable. The closed and
Escape-close scenarios passed. The popup-open scenario was blocked because the
automation click left the Blazor trigger at `aria-expanded=false`; its diagnostic
also found two other visible Select popups elsewhere on the page, so that selector
is not a reliable proof of the target popup state. The comparable Blazor page on
`:5046` was then opened through the browser's actual input locator: the Basic
trigger changed to `aria-expanded=true` and its Jack/Lucy/Yiminghe options became
visible. This confirms the control works in the manual browser path, but does not
clear the flaky automated popup gate. Runner report:
`/tmp/select-basic-popup-5052-fresh-report.json` (2 passed, 1 blocked).

## Sticky docs header, right TOC, and Select hash target — 2026-11-04

The React docs header is sticky (`top: 0`, `z-index: 1000`, 8px backdrop blur),
and the component TOC stays fixed at `headerHeight + marginXL - 4px` (92px with
the current tokens); both behaviors are now mirrored by the Blazor docs. The TOC
uses the same 992px hide breakpoint and a matching viewport-height allowance.
React also applies an 80px `scroll-margin-top` to all ID targets. Adding this to
the Blazor component page prevents a direct `#components-select-demo-basic` URL
from placing the first Select underneath the sticky header.

Browser verification at 1280px: after scrolling 700px, the header remained at
top 0 and the TOC remained fixed at top 92px. Opening the Select page directly
at the Basic demo placed its anchor at 80px, below the 64px header; clicking the
first Select set `aria-expanded=true` and displayed its popup. The automated
single-popup runner still reports the click as blocked even though the same
fresh page succeeds through browser interaction, so its failure remains an
automation limitation pending further diagnosis (not a parity pass). Latest
report: `/tmp/select-basic-popup-5055-after-anchor-offset-report.json`.

## Button Basic current-site parity — 2026-11-04

Reran `button-basic-dom-style-parity.json` against React `:8000` and the current
Blazor server on `:5055`. The five buttons have equal normalized DOM trees and
equal selected computed styles and visual styles (1 scenario passed, 0
differences, 0 blocked). Raw HTML serialization remains different: React capture
is 659 bytes and Blazor is 879 bytes, with attribute order already differing at
the first byte comparison. This focused case proves semantic structure and
rendered style parity for Button Basic only; it is not a full Button demo sweep.
Report: `/tmp/button-basic-5055-current.json`. The Button page remains open for
manual review.

## Phase 1 generated coverage recheck — 2026-11-04

Reran `tools/AntDesign.StyleGen/src/phase1-progress.mjs`. Generated/source-data
coverage is currently: CSS components 72/72; three theme algorithms; Semantic
DOM catalog 61/61, definitions 70/70, API slots 423/423, documented slots
423/423, and structure migration 70/70; component-token pages 65/65 and token
rows 584/584; global-token usage pages 64/64 and rows 1464/1464. All reported
token metadata checks (IR, descriptions, defaults, CSS variable names, and
global usage rows) have zero mismatches. This checks catalog/generator coverage,
not runtime DOM/CSS/visual parity across those components. JSON report:
`/tmp/phase1-current-20261104.json`.

## Button Size and Ghost follow-up — 2026-11-04

Expanded the Button comparison to 17 common demos. The first run showed `Size`
was ignored in the Size demo: it had Large selected while all buttons remained
32px. The Button `FormSize` cascading setter was writing over explicit `Size`;
it now applies the inherited size only when the caller did not pass `Size`.
The demo now mirrors React's Size layout (Medium option, Preview divider, nested
Flex groups, and the complete five icon-button shapes). Browser interaction
confirmed Large/Default/Small at 40/32/24px. The normalized DOM/CSS/visual
comparison for Size now passes 1/1 at `/tmp/button-size-5055-normalized-report.json`.

The Ghost component now maps a solid ghost button to outlined and the Ghost demo
uses React's Primary, Default, Dashed, and Danger examples. Direct browser
inspection showed matching classes, dimensions, colors, and borders. Automated
Ghost comparison reports visual styles 1/1 equal; the only DOM difference is an
extra `--antd-wave-shadow-color` inline style on Blazor's Danger button. The
comparison runner now waits for `document.fonts.ready` and two animation frames
before capturing a demo; this avoids transient font-metric width differences
seen on the first Ghost run. Final Ghost report:
`/tmp/button-ghost-5055-fonts-ready.json`.

The refreshed 17-demo Button run reports Basic and Size passing, 15 scenarios
with differences, 0 blocked. This is a targeted subset, not a full Button page
audit. Full report: `/tmp/button-broad-5055-normalized-correct-report.json`.

### Button Loading API and demo follow-up (2026-11-04)

Added the `LoadingIcon` RenderFragment parameter and rebuilt the Loading demo
to match React's nine examples: static loading states, a custom spinning icon,
icon placement/replacement, and click-triggered loading. The default Loading
demo passes normalized DOM, selected CSS, and visual-style comparison 1/1;
both pages render nine button roots. The click scenario confirms each side
enters the loading state, but a single synchronized DOM snapshot is not stable:
React's 3-second timer starts before the Blazor Server click completes, and the
React wave/loader exit transition crosses the capture window. Therefore only
the static scenario is counted as passing. Report:
`/tmp/button-loading-interaction-5059-report.json`.

### Responsive docs TOC follow-up (2026-11-04)

Removed the Blazor layout rule that hid `.toc-affix` below 992px. React keeps
its sticky header and right TOC at 820px (and retains the TOC in the narrow
viewport check), while Blazor had hidden it at that breakpoint. The rebuilt
Blazor page at 820px reports a sticky header and a displayed right TOC. The
header implementation already used `position: sticky`; it did not need a code
change for this correction.

### Static React docs server and Select regression set (2026-11-04)

Stopped the Dumi development server on port 8000, ran `npx --no-install dumi build`
in the React repository, and served the generated `_site` with `npx serve -s _site
-l 8000`. The build generated 2,551 assets; `_site` is 247 MB. Static component
routes return HTTP 200, and the React Select/Icon review tabs were reloaded from
the static host. The React side no longer runs the Dumi development compiler.

Before switching hosts, reran the existing Select size and interaction fixtures
against the current Blazor Development site on `:5059`: 6/6 scenarios passed with
zero normalized DOM, selected CSS, or visual-style differences. This includes
size changes, search filtering, multiple selection, and clearing an unmatched
value. Reports: `/tmp/select-current-5059-report.json` and
`/tmp/select-interaction-current-5059-report.json`. These results verify those
specific cases, not all Select demos or the full component API/token scope.
After the static host was started, a real browser click opened the Basic Select
on both static React and Blazor: both set `aria-expanded=true`; the target popup
was 120x136px at x=275px/y=158.5px on each page. Both Select tabs are left open
with their popup visible for review.

### Icon catalog, Two Tone, and narrow TOC alignment (2026-11-04)

In the static React build and Blazor Development page, all six Icon gallery
categories have equal counts and equal sorted icon-name signatures after
normalizing the React `6.5.0` badges. The Two Tone demo's original SVG path fills
also match for all three icons. At 820px, React's component article reserves
48px left and 164px right for its fixed TOC; Blazor had reduced both sides to
32px, allowing content under the TOC. At the 390px emulated viewport, React
still reserves 48px/164px while Blazor had 16px on each side. Updated both
Blazor breakpoints to reserve the React content width. After rebuilding and
restarting `:5059`, the 820px content width matches React exactly (394.75px),
with 16px between content and TOC; the mobile article also ends before the TOC.
The Icon and Select browser tabs remain open for review.

## Icon toolbar shrink behavior at 820px — 2026-11-04

Compared the live React static build (`:8000`) and Blazor Development docs at the same 820×900 viewport. React's filter Segmented measured 378.1px and uses `flex: 0 1 auto`; Blazor had a fixed `flex: 0 0 380px`. Updated the Blazor filter flex basis to `0 1 auto`. The first retest isolated a second cause of toolbar overflow: Blazor's addon padding gave Search a 105px min-content width, versus React's 79px compact control. Added the same outer 8px toolbar bleed as React and a <=992px compact Search sizing rule. Rebuilt Docs.Server successfully (net10.0, 0 errors). Browser measurements now match at the 820px viewport: outer toolbar 410.75×56px, Segmented 378.125×40px at x=250.25, Search 79×40px at x=644.375. Search interaction remained functional: entering `account` returned three icon cards, and clearing restored the full list. React/Blazor tabs remain open for manual inspection at `http://127.0.0.1:8000` and `http://127.0.0.1:5000/en-US/components/icon?theme=light#components-icon-demo-list`.

## Select Basic popup geometry after docs restart — 2026-11-04

Opened fresh React and Blazor Select Basic pages at the same 820px viewport. Clicking the first Blazor Select with the browser produced `aria-expanded=true`; its associated bottom-left popup measured 120×136px at x=275px/y=159px, against a 120×136px React popup at x=275px/y=158px. The trigger measured 120×32px in both (React y=122.48px, Blazor y=123.09px). This fresh circuit avoids the stale Blazor tab's terminated circuit during an earlier restart. Both fresh pages remain open for review.

Inspected the positioning inputs before changing Overlay: both pages have an 80px scroll margin on the demo anchor. React's trigger/popup absolute tops were 720.484/756px; Blazor's were 894.094/930px. The measured placements are consistent with applying a 4px vertical gap and flooring the final absolute top coordinate. The viewport screenshot differs by one raster pixel because each page scrolled to a different integer `scrollY` (598 vs 771) and the trigger has a different fractional document coordinate. After Escape, the first Select's `aria-expanded` became `false`, its `ant-select-open` class was removed, and its popup received `ant-select-dropdown-hidden`. No Overlay offset change was made based on this measurement.

## Select and Icon focused rerun against rebuilt Blazor docs — 2026-11-04

Rebuilt React's static `_site` and served it with `npx serve -s _site -l 8000`;
the React Dumi development server remains stopped. React Icon's five focused
examples pass (5/5); Select Default/Large/Small passes 3/3. Select interaction
coverage passes search filtering, multiple selection, and clearing a selected
value (3/3); Semantic DOM passes 2/2; suffix passes 1/1. Reports:
`/tmp/icon-current-20261104-static.json`,
`/tmp/select-current-20261104-static.json`,
`/tmp/select-interaction-current-20261104-static-fill.json`,
`/tmp/select-semantic-current-20261104-static.json`, and
`/tmp/select-suffix-current-20261104-static.json`.

The interaction fixture previously targeted React's debug-only
`option-label-center` demo, which is absent from a production static Dumi
build. It now uses the regular Basic demo's clearable Select. Popup-opening
actions use real coordinate clicks and search filtering uses an input fill;
this removes the intermittent readonly-input click/focus timing from the
fixture. The runner now also supports `alignRootViewportY` so long, differently
laid out docs pages can be compared with the same trigger location in the
viewport without modifying component styles.

That alignment exposed a genuine Big Data accessibility difference: Blazor's
hidden virtual options omitted `aria-disabled="false"`. The virtual option
renderer now emits the explicit enabled state React renders. After rebuilding
and restarting Docs.Server on `:5000`, Select Big Data passes DOM, selected
CSS, Semantic DOM and geometry checks (1/1), with the trigger aligned at y=200
on both pages. Report: `/tmp/select-virtual-current-20261104-final.json`.

Icon's focused demos and catalog remain aligned. The responsive gallery itself
still intentionally differs from React at 820px: React keeps six columns and
clips long labels; Blazor switches to four columns and wraps them to avoid
clipping. This preserves the reported no-clipping behavior but is not exact
grid CSS parity; revisit the user preference before changing it back to
React's clipping behavior. Both Icon review pages remain open in Chrome.

## Menu horizontal overflow follow-up — 2026-11-04

Reran the horizontal Menu demo using the rebuilt static React site and rebuilt
Blazor server. The scenario renders on both sides but is not yet aligned. The
root CSS differs in height (46/47px), `::before` height (45/46px), background
(React white, Blazor transparent), and position (React static, Blazor relative).
The overflow list also differs structurally: Blazor's measured hidden copy,
overflow indicator orientation/role, keyboard attributes, and item width
currently diverge from React's `rc-menu` overflow DOM. Report:
`/tmp/menu-horizontal-current-20261104-static.json`. This is the next component
task; do not mark Menu responsive behavior as verified from the existing
automated responsive demo alone.
## Blazor docs narrow header and TOC alignment — 2026-11-04

Compared the Dumi `DocAnchor` implementation with the Blazor component page.
React keeps the 64px header sticky and fixes the right TOC to the viewport at
`headerHeight + margin` (92px here). Blazor's header was already sticky, but
its TOC was attached to the article and wrapped in an extra `Affix`. The TOC
now uses a fixed viewport position, `Anchor Affix=false`, and matching 80px
anchor scroll offset. Follow-up browser inspection at 820px and 523px showed
that React's generated `max-width: 992` media query has no `px` unit and is
ignored, so the TOC remains visible at both widths. Blazor's former 992px
hide rule has now been removed to match the rendered React behavior.

## Menu horizontal overflow and title geometry follow-up — 2026-11-04

Rebuilt the React documentation with `npx dumi build` and served `_site` using `npx serve -s _site -l 8000`; the React site returned HTTP 200 without running the Dumi dev server. In `MenuItem`, replaced Razor's whitespace-producing title wrapper with a `RenderTreeBuilder` fragment, and added React-compatible `data-menu-id`, `aria-controls`, and submenu popup IDs. Hidden overflow items now use the same inline-style declaration order as React. The parity normalizer now supports runtime IDs carried by data attributes and excludes removed framework/measurement attributes while seeding references. Compared React and Blazor horizontal menu at 1280×900: first two item widths match exactly (159.859px and 160.078px), DOM tree passed, computed styles and visual styles passed, and no geometric differences remain (1/1 passed, 0 differences, 0 blocked). Direct browser inspection confirms both sites render the submenu ARIA references and the same item widths; the popup element is not mounted while the submenu is closed in either site. Docs.Server net10 build succeeds with 0 errors. The server at `http://127.0.0.1:5000/en-US/components/menu?theme=light#components-menu-demo-horizontal` is running in Development so static web assets load; running it in Production with `--no-launch-profile` caused the component JS to return 404 and is not a valid browser parity environment. Reusable fixture: `tools/AntDesign.StyleGen/examples/menu-horizontal-dom-style-parity.json`.

## Select focused parity reconciliation — 2026-11-04

Re-ran Select interaction and Semantic DOM fixtures independently against the
rebuilt React static site (`npx serve -s _site -l 8000`) and Blazor Docs.Server
on `:5000`. All 3 interaction scenarios (search, multiple selection, and clear)
and both Semantic DOM scenarios pass DOM, computed style, and visual style
checks. The prior merged 13-scenario report incorrectly marked the 3 scenarios
as differences because the combined manifest inherited a different global
class-normalization policy; standalone fixtures prove the Select behavior is
aligned. React clear retains focus and Blazor now does too in the actual runner.
Reports: `/tmp/select-interaction-standalone-20261104.json` and
`/tmp/select-semantic-standalone-20261104.json`. Remaining size, Basic popup,
suffix, and Big Data scenarios in the focused run had already passed; this
brings the 13 focused Select scenarios to 13/13 verified.

## Icon catalog and narrow docs shell recheck — 2026-11-04

After rebuilding React's Dumi output and serving it statically, the Icon
focused fixture passes all 5 scenarios (iconfont, iconfont resources, basic,
two-tone, and custom), including DOM, computed styles, SVG paint, and visual
styles. Report: `/tmp/icon-current-20261104-static.json`.

At 820px and 523px, React keeps its 64px sticky header and fixed 148px TOC at
`x = viewportWidth - 156px`, `y = 92px`; the document has no horizontal
overflow. Removed Blazor's `max-width: 992px` TOC hide rule and changed its top
offset from 96px to 92px. Browser checks now match React at both widths, and
after scrolling the Blazor header remains at y=0 and the TOC remains fixed at
y=92px. The Blazor and React Icon tabs are open at 523px for manual review.

## Icon gallery Affix parity — 2026-11-04

Replaced the Icon filter toolbar's CSS-only sticky positioning with the existing
Blazor `Affix` component and React's 80px top offset. The raised toolbar style
is applied only while affixed. Browser measurements against the static React
site at 523px and 820px match: the Affix wrapper is fixed at `top: 80px`, and
the toolbar is 327×56px with 8px padding, -8px margin, 6px radius, matching
shadow and white background. Before affixing, both render the unraised 311×40px
toolbar at 523px and 403×40px at 820px. The first pass exposed an 8px versus 6px
radius caused by using the large global radius token; it now uses the base
radius token. Docs.Server Release build succeeds with 0 errors. The React site
is served statically via `npx serve -s _site -l 8000`; no Dumi dev server is
running.

## Menu overflow popup coordinates — 2026-11-05

Rebuilt and served React docs as static files (`npx serve -s _site -l 8000`);
the Select route loaded in the browser with HTTP 200. The Blazor docs must run
with `ASPNETCORE_ENVIRONMENT=Development` so referenced-project static web
assets are available. Starting the same `--no-launch-profile` command in
Production returned 404 for CSS/JS assets and left demo skeletons unhydrated;
the Development server is now running and the demos hydrate normally.

At a 523×900 viewport, opened the horizontal Menu overflow popup on both sites
through browser interaction. React and Blazor now match the popup bounds
exactly: x=248, y=294, width=272.953px, height=136px, with the same three
overflowed entries. The Blazor popup uses Overlay's `BottomLeft` placement and
content-only width capped to the viewport; the overflow trigger's inner title
starts 16px inside its `li`, so its popup wrapper receives a 16px horizontal
margin to align with React's title anchor. Its vertical offset is 1px (the
other submenus retain the default 4px), which aligns the wrapper and popup top.
The browser leaves the React and Blazor Menu tabs open at 523px, with the
Blazor popup open for manual review. Automated horizontal-menu fixture passes
1/1 scenarios with zero DOM, computed-style, or visual-style differences;
report: `/tmp/menu-horizontal-after-offset-20261105.json`. A 400px emulation
collapsed the Blazor demo's containing menu width to zero, so that viewport is
not yet counted as verified.

## DatePicker Basic popup and calendar week start — 2026-11-05

Built the React documentation and served the generated `_site` using
`npx serve -s _site -l 8000`; no Dumi development server is running. The
Blazor documentation server on `:5000` is running in Development. The focused
DatePicker Basic popup fixture aligns both trigger roots at the same viewport
Y coordinate before clicking, so placement comparisons are not affected by
different article lengths. Added `preserveViewport` to coordinate-click
actions for this case. The first open-calendar comparison then exposed a real
calendar-grid difference: for November 2026, Blazor previously began the grid
at October 25 while React begins at November 1. `DayOfWeekHelper` now computes
the first displayed date from the configured first day of week, and both date
and datetime panels use it.

After rebuilding, the focused fixture passes 1/1: DOM tree, computed styles,
visual styles, trigger and popup geometry, footer ranges, and Today control all
match React. The first visible dates are November 1–10, 2026 on both sites, and
the popup is 288px wide. Report:
`/tmp/datepicker-basic-preserved-viewport-20261105.json`.
### Button `style-class` semantic object/function parity (2026-11-06)

React's `Button` accepts either object or callback values for `classNames` and
`styles`. The Blazor API previously accepted only dictionaries, so the demo's
function result was hard-coded. Button now exposes `ClassNamesFunc` and
`StylesFunc`, each receiving the current `Button` instance. The `style-class`
demo uses `StylesFunc` to return the primary root/content styles based on
`Button.Type`; its object-based example remains unchanged. The API extractor and
React-to-Blazor map now list both callback parameters and document the shape
translation.

After regenerating demo metadata, rebuilding the .NET 10 docs server, and
restarting it, browser inspection confirms the API table contains both function
parameters and the demo description mentions them. The rendered DOM consists
of one Flex root with two buttons and one text span per button on both sites.
At 1280×900, both button sizes match exactly (76.8125×40px and 91.3516×40px),
the gap is 8px, and selected computed colors, borders, radius, padding, and
shadows match. The root x-position differs by 0.008px from fractional layout.
The callback produces `rgb(23, 23, 23)` on the primary root and white content;
the object example retains its shadow. Browser console has no errors. This is a
focused demo/API check, not a full Button inventory pass.

## Theme menu reopen regression and AutoComplete Basic (2026-11-08)

The docs theme switch no longer binds the Dropdown's open state to a second
local visibility flag. With the .NET 10 docs server running in Development, a
browser run opened the theme menu, closed it by clicking outside, then reopened
it for five consecutive cycles. The menu also reopened once more after the
cycles.

Added a focused AutoComplete Basic DOM/style fixture for the uncontrolled and
controlled inputs, typing and dynamic suggestions, plus popup Semantic DOM.
After rebuilding the docs server, 3 of 4 scenarios pass with no root DOM or
computed-style differences. The remaining popup scenario has a measured
trigger-to-popup position mismatch (12px horizontally and 5px vertically); it
is recorded as unresolved and is not counted as aligned. Report:
`/tmp/autocomplete-basic-dom-style-parity.json`.

### AutoComplete popup anchor correction (2026-11-08)

The popup was anchored to AutoComplete's inner content node, whose 12px inline
padding moves it right of React's `.ant-select-auto-complete` root. The
component now passes its root `ElementReference` to `OverlayTrigger`, and
OverlayTrigger preserves an explicit `TriggerReference` instead of replacing
it with the Unbound child reference. The Docs.Server net10 build succeeds with
0 errors. The focused runner now reports 3 passed, 0 differences, and 1
blocked scenario; all captured DOM, computed-style, popup geometry and visual
style checks match. The remaining blocker is the Blazor dynamic suggestion
selector not appearing in the duplicate popup scenario even though the input
is expanded and the popup is visible. Report:
`/tmp/autocomplete-basic-dom-style-parity-root-anchor-20261108.json`.
React and Blazor AutoComplete pages remain open in the browser for manual
review.

## Select responsive/custom tag render and theme menu recheck (2026-11-08)

The theme menu used to set the bound `_open` field directly inside selection
handlers while Overlay was also completing its asynchronous hide transition.
Selection, compact-mode, and happy-work handlers now call the Dropdown
component's `Close()` method and let `OpenChanged` update the bound state. In
the rebuilt Development docs browser, selecting Light theme closed the menu;
clicking Theme settings immediately afterwards reopened it, and the popup's
visibility check returned true.

Added the reusable
`tools/AntDesign.StyleGen/examples/select-responsive-dom-style-parity.json`
fixture for the three responsive Select examples. The current run compares three
roots with matching selected computed styles and visual rendering, but exposes
one exact inline-position difference on the suffix: React reports
`inset-inline-start: 196.797px`, Blazor reports `196.813px`. The 0.016px
difference is not normalized away and is tracked as open. Report:
`/tmp/select-responsive-current-20261108.json`.

Re-ran the Select custom-tag-render fixture: root dimensions, selected styles,
geometry and visual styles match, but two generated close-icon SVG roots omit
React's `fill-rule="evenodd"`. The source is the bundled generated Icons
catalog used by `IconService`; changing Select markup would only mask this
shared icon discrepancy, so it remains an open Icon package/catalog item.
Report: `/tmp/select-custom-tag-render-recheck-20261108.json`.

Re-ran the five canonical Icon groups against the same current React and
Development Blazor sites: 5/5 passed with zero DOM tree, computed-style,
geometry, visual-style differences, and no blocked scenarios. This confirms
those documented groups; it does not cover every icon in the gallery or the
SVG close attribute discrepancy above. Report:
`/tmp/icon-dom-style-parity-current-20261108.json`.

## DatePicker component token inheritance — 2026-11-10

React v6 declares `DatePicker.ComponentToken` as extending
`Exclude<SharedComponentToken, 'addonBg'>`. The token synchronizer previously
merged every row from `components/version/token-meta.json` without applying
that TypeScript exclusion, so the generated Blazor token catalog documented
`addonBg` even though `DatePickerComponentToken` did not expose it. Updated the
token source parser to resolve inherited interfaces and `Exclude`/`Omit`
filters, and applied those filters to metadata-only rows before generating the
IR and C# catalog. The docs CLI then regenerated `llms-tokens.json`.

Focused StyleGen tests and the phase-one token audit pass: the expected React
component-token rows are now 583/583, with no missing or unexpected rows or
metadata mismatches. A live Chromium read of the rebuilt DatePicker page found
39 component-token rows, including `presetsWidth` and excluding `addonBg`.
The DatePicker Basic popup fixture also passes 1/1 with zero DOM-tree,
computed-style, geometry, and visual-style differences:
`/tmp/datepicker-basic-popup-after-token-sync-20261110.json`. This only verifies
the Basic popup state and token table; the two React `multiple` demos remain
unimplemented in Blazor and are open DatePicker work.

## DatePicker multiple demos — 2026-11-10

Added the React `multiple` and `multiple-debug` demos in Blazor with bilingual
metadata. The debug demo is present in both generated locale catalogs. The
multiple API includes responsive `MaxTagCount` measured from the actual
container and tag widths. Docs.Server net10 builds successfully with zero
errors. Earlier browser interaction verified that adding and removing dates
updates tags and calendar selection while keeping the popup open. The current
Cua browser bridge could not reconnect (native pipe unavailable), so the
rebuilt visual/debug state and narrow-width responsive count remain to be
rechecked in the browser. The Development server is running at port 5000.

## Table pagination and isolated row capture — 2026-11-10

The pagination-placement fixture initially showed one semantic `body.row` text
difference while DOM structure, computed styles, geometry, and visual styles
matched. The semantic capture serializes a standalone `<tr>`; the parity helper
parsed it under `<body>`, where jsdom drops invalid table children. Added
context-aware parsing for table fragments (`tr`, `td`, `th`, row groups, and
column groups), with a regression test for a row containing equivalent cell
text surrounded by renderer whitespace. The focused StyleGen test file passes
4/4, and the pagination-placement fixture now passes 1/1 with zero DOM, style,
geometry, or visual differences.

Re-ran the six current Table fixtures (seven scenarios): initially six passed
and Fixed Header differed by 15px. The served `ant-design-blazor.js` bundle was
stale relative to `tableHelper.ts`; after rebuilding it, the remaining issue
was that the header fluid column's initial inline width was mistaken for an
explicit width. Column sizing now reads explicit widths from the body colgroup,
where fluid columns remain widthless. Rebuilt the runtime bundle and Docs.Server,
then reran all six fixtures: 7/7 scenarios pass with zero DOM tree, computed
style, geometry, or visual-style differences. The Fixed Header fluid col is
451.328px on both sides. Report:
`/tmp/table-regression-suite-20261110-final.json`; detailed column capture:
`/tmp/table-fixed-header-final-20261110-v2.json`.

The repository's `npm run build:lib` task currently fails before compilation
because its `scripts/build/compile-styles`, `generate-less-vars`, and
`migration-styles` modules are absent. To validate and serve the current
TypeScript, bundled `components/main.ts` directly with the existing Browserify
and tsify toolchain. The Cua browser bridge remains unavailable, so the local
browser tab could not be left open for manual inspection this round; headless
Chrome parity runs completed successfully.

Correction (2026-11-10): although the Cua browser bridge could not enumerate
tabs, the native Chrome app was accessible. Opened the rebuilt Table page at
`http://127.0.0.1:5000/en-US/components/table#components-table-demo-fixed-header`
and confirmed the Table demos, Semantic DOM, and token section render in the
accessibility tree. The visible browser is left on the Table page for manual
inspection. This does not replace the headless fixture results or constitute
manual interaction verification of all Table demos.
# Select/Icon parity follow-up — 2026-11-10

Updated the `AntDesign.Icons` package from the sibling source generator fix so
Close SVGs preserve `fill-rule="evenodd"`; Select Custom Tag Render now matches
the React DOM tree, computed CSS, geometry, and visual styles. The first
Responsive capture exceeded its 20s budget while the docs page and lazily
mounted demo initialized. Re-running the same scenario with a 60s budget
passes 1/1 with zero DOM/style/geometry/visual differences. Icon's five public
demo fixtures also pass 5/5 with zero differences. A second Icon page issue
was caused by eager initialization of all CodeBox demos injecting duplicate
IconFont symbols; `CodeBox` now mounts the demo when its preview approaches the
viewport, through `demo-visibility.js`. Docs.Server responds at port 5000, and
the Select Responsive page is open in Chrome for inspection. These results
cover the named fixtures only; they do not certify every Select interaction or
the full Icon gallery.

## Select expanded parity pass — 2026-11-10

Rechecked Select against the live React `:8000` and Blazor Development `:5000`
servers. Core Size (default/large/small), Variant, Placement (default/bottom-left),
search filtering, multiple selection, basic/escape popup behavior, Suffix,
virtual big-data, Semantic DOM closed state, Responsive maxTagCount, and Custom
Tag Render were included across targeted fixtures. The successful scenarios
show equal DOM trees, computed styles, geometry, and visual styles. Icon's five
canonical demo fixtures also pass 5/5 after the latest docs build. The suffix
fixture previously waited on its outer section without scrolling the `.code-box-demo`,
so its lazy preview remained a skeleton; the fixture now targets the demo preview
and passes 1/1. The Semantic DOM popup fixture and Custom Tag Render fixture
blocked on lazy preview initialization in the latest isolated rerun; earlier
successful targeted results remain recorded above, but these two states should
be rerun after stabilizing browser activation. One long combined run produced
configuration artifacts because group-specific comparison policies were
replaced during temporary manifest assembly; those results were discarded.
Latest reports: `/tmp/select-dom-core-20261110-dev-report.json`,
`/tmp/select-basic-popup-20261110-report.json`,
`/tmp/select-suffix-20261110-report.json`,
`/tmp/select-expanded-suite-policy-20261110-report.json`, and
`/tmp/icon-dom-style-parity-after-header-20261110.json`.

## Dropdown Basic popup follow-up — 2026-11-10

Restored the docs header brand mark and title (`Ant Design` with a small
superscript `Blazor`) to preserve the React-like compact brand at narrow widths.
The docs server builds with zero errors. Dropdown Basic closed and hover-open
trigger scenarios pass 2/2; its popup geometry, computed CSS, and visual styles
also match after setting the Dropdown Overlay display mode to `block`. The popup
DOM fixture still reports structural differences: Blazor menu items emit empty
style attributes and runtime menu IDs, class ordering differs, and React's
trigger portal includes an empty `aria-hidden` sentinel. These are still open;
do not count Dropdown popup DOM as fully aligned. Report:
`/tmp/dropdown-basic-dom-style-parity-final-20261110.json`.

## DatePicker Select range dates and AntList Infinite Load — 2026-11-13

Implemented the DatePicker “Select range dates” demo with the same 7-day and
6-month `disabledDate` rules as React, including month/year boundaries.
RangePicker now retains its popup DOM while hidden, avoiding the first-open
`HierarchyRequestError` caused by portaling before the popup exists. Browser
interaction selected 2026-11-15 and verified the exact seven-day boundary:
2026-11-08 and 2026-11-22 are disabled, while 2026-11-09 and 2026-11-21
remain available. This exposed the legacy range-order guard incorrectly
disabling dates before the selected start even when the new contextual
`DisabledDateWithInfo` callback is supplied; the guard now applies only to the
legacy `DisabledDate` path. The focused parity scenario is no longer blocked,
but strict DOM/style comparison still reports RangePicker input class, range
attribute, and separator accessibility differences. Full structural parity
remains pending. Report: `/tmp/datepicker-select-in-range-current2.json`.

Implemented AntList Infinite Load with a fixed-height scroll container,
IntersectionObserver-based bottom detection, ten-item batches, a skeleton
loader, and the end message after 50 items. Browser interaction verified the
rendered item count advances from 10 to 20 after scrolling, with no demo
runtime errors. This uses deterministic local sample records instead of React's
remote mock API; a focused React DOM/CSS fixture remains to be added.

## AntList Infinite & virtualized follow-up — 2026-11-13

Completed the previously empty Infinite & virtualized demo. It renders a fixed
window of list rows with top/bottom spacers, updates the window as the user
scrolls, and appends twenty deterministic local records at the bottom. A
browser interaction check verified the initial 20-item logical list renders 19
rows at a time, then grows to 40 after a bottom-scroll gesture while keeping
19 rows mounted; no runtime errors occurred. The existing framework
`Virtualize` component currently raises a JS interop argument mismatch in this
runtime, so this demo uses its own fixed-row windowing instead. The behavior
fixture for React DOM/CSS remains pending.

## Recovered focused progress notes — 2026-11-13

- Form Basic validation: the focused fixture passes initial, visible-password,
  and invalid-submit states 3/3; required messages, error classes, and
  `aria-describedby` associations match. Report:
  `/tmp/form-basic-active-goal-20261113-final4.json`.
- Form nested dynamic lists: seven add/remove/edit states pass with zero
  normalized DOM, computed-style, geometry, or visual-style differences.
  Report: `/tmp/form-dynamic-complex-complete.json`.
- Form passenger list: four initial/add/add-at-head/remove states pass with zero
  normalized DOM/CSS/geometry/visual-style differences. Report:
  `/tmp/form-dynamic-form-item-passengers-final2.json`.
- Table tree data: initial, strict-mode toggle, and parent-selection states pass
  3/3 with zero normalized DOM/CSS/geometry/visual-style differences. Report:
  `/tmp/table-tree-data-final4.json`.
- Table Dynamic Settings: browser checks confirmed TopStart pagination, Medium
  size, and London/New York filtering. Automated capture was blocked by React
  docs navigation timeout; main-column width and Action-column sorting remain
  differences. Report: `/tmp/table-dynamic-settings-20261113.json`.
## DatePicker generated demo and Tabs Nest — 2026-11-13

Regenerated the DatePicker demo metadata after the source `.razor` and Markdown
had been updated but the runtime metadata still contained `TODO`; the docs build
now includes the 7-day and 6-month range examples. The docs server has been
restarted with the rebuilt assembly. Build: 0 errors (existing warnings).

Replaced the empty Tabs Nest placeholder with the React demo structure: four
Select controls drive the parent/child tab placements and types, and the nested
Tabs render 20 child panes. The Docs.Server build passes with 0 errors. A
focused React-vs-Blazor browser fixture is still pending; Chrome DevTools could
not attach because its shared browser profile is already held by another
process. Component inventory baseline is in
`runtime-parity-summary-2026-11-12.md`; the broad runtime parity work remains
incomplete.

## Tree BigData / VirtualScroll assessment — 2026-11-13

React browser capture confirms the BigData demo uses a 400px viewport with 17
mounted `.ant-tree-treenode` rows, and VirtualScroll uses a 233px viewport
with 11 rows. The matching Blazor demos are still placeholders. Blazor's
`Tree.razor` recursively creates all `TreeNode` components from `DataSource`;
`TreeNode.RealDisplay` suppresses markup for collapsed descendants but keeps
all node components registered. There is no `Height` parameter or virtual
window. A simple overflow wrapper will therefore not align either the DOM
window or the 10,000+ node rendering cost. A data-bound flattened virtual-row
path must preserve expansion state and existing non-virtual/manual-node
behavior before the two demos can be ported. React reference capture:
`.ant-tree-treenode` count 17 at 400px and 11 at 233px.

## Select responsive tag measurement parity — 2026-11-13

Multiple Select with `maxTagCount="responsive"` now always renders React's
aggregate/rest tag node. When no tags overflow, it remains mounted as a hidden
measurement node (`aria-hidden`, zero height, absolute positioning, and
`+ 0 ...` content); when tags overflow, the same node becomes the visible
aggregate tag. The focused browser comparison confirms the measurement node
is present in Blazor and has the same markup as React. Select's interaction
fixture passes 4/4 states with zero normalized DOM/CSS/geometry/visual-style
differences (`/tmp/select-interaction-final-20261113.json`). The standalone
semantic-preview fixture still flags strict serialized attribute differences
(class token order, style whitespace/semicolon, and the focused class after a
mode-switch click); it no longer indicates a missing responsive tag node.

IconFont sprite precedence was also rechecked: all five canonical Icon
scenarios pass with zero DOM/CSS/geometry/visual-style differences after
preserving earlier demo symbols as the duplicate-ID winner
(`/tmp/icon-parity-final-20261113.json`).

## Select controlled-open focus parity and Icon gallery recheck — 2026-11-13

When `Open=true` is controlled, Select no longer moves focus into its search
input on popup visibility. React leaves focus on the external control that
changed the surrounding UI; this removes the extra `ant-select-focused` class
and its focus visual from the Blazor Semantic DOM preview after switching from
Single to Multiple. The normalized semantic-preview DOM tree and visual styles
now match in both states; the raw serialized HTML still differs in framework
markup/attribute serialization. A repeatable fixture passes 2/2 states with
zero normalized DOM/CSS/geometry/visual-style differences:
`tools/AntDesign.StyleGen/examples/select-semantic-preview-dom-style-parity.json`
(`/tmp/select-semantic-preview-repeatable-20261113.json`). Normal unbound
Select focus behavior remains intact: the search, multiple selection, clear,
and outside-dismiss fixture
passes 4/4 with no DOM/CSS/geometry/visual-style differences
(`/tmp/select-interaction-focus-fix-retry-20261113.json`).

The Icon gallery responsive fixture also passes at desktop and narrow viewport
sizes (2/2, no normalized DOM/CSS/geometry/visual-style differences;
`/tmp/icon-gallery-responsive-current2-20261113.json`). Blazor retains its
readable wrapping of long icon names; React's current gallery clips these names
with nowrap/overflow hidden, a known intentional difference to preserve the
previous explicit no-clipping requirement. Docs.Server remains available at
`http://127.0.0.1:5000`; an independent Chrome review window is open on the
Select Semantic DOM demo.

## Demo titles in dark mode and DatePicker range selection follow-up — 2026-11-13

Aligned CodeBox demo titles with React v6: dark title background now matches the
container (`#141414`) instead of using a raised `#262626`, the title uses the
6px corner radius, and RTL mirrors the React title shape. Browser computed-style
comparison against React confirms the dark title background, text color, and
radius match.

Completed the DatePicker `Select range dates in 7 days` interaction. RangePicker
now retains the in-progress start date while validating the end-date panel and
supplies it through `DatePickerDisabledDateInfo.From`. This is needed because
RangePicker can display an in-progress start in its input before committing the
pair through `ValueChanged`. Browser interaction selected 2026-11-13: 2026-11-06
and 2026-11-20 were disabled, while 2026-11-07 and 2026-11-19 remained enabled.
The 6-month example also uses the same in-progress range context. Docs.Server
`net10.0` build passed with 0 errors; broad demo/runtime parity remains open.

## Table Reset Filter interaction follow-up — 2026-11-13

Replaced the demo's hand-built checkbox filter popups and locally filtered row
list with Table's native `Filters` / `OnFilter` API, matching React's native
column filters and ellipsis settings. A browser interaction check confirmed:

- Initial rows match React's source data and order.
- `Sort age` orders ages descending (42, then 32).
- Selecting `Jim` in the Name filter and confirming leaves Jim Green and Jim
  Red.
- `Clear filters` restores all rows while retaining the Age sorter.
- `Clear filters and sorters` restores the original row order and clears the
  sorter.

The React documentation page currently logs hydration error #418, so its button
handlers do not execute in this browser session; event-state comparison against
the live React page remains pending. Its checked-in `reset-filter.tsx` source
specifies the same descending sorter and filter-reset behavior. The Reset Filter
demo had to be scrolled into view repeatedly while the docs layout settled; once
visible, its Blazor lazy-loaded demo rendered normally.

## CodeBox edit icon dark-mode follow-up — 2026-11-13

The dark demo title was already using the dark container color, but its edit
icon still inherited the light-mode black color. Added explicit dark-mode
default and hover colors for the title's edit icon. Browser verification on the
`Infinite Load` demo reports white at 45% opacity normally and white at 85%
opacity on hover in dark mode; the light theme retains its original color.
Docs.Server `net10.0` build passed with 0 errors.

## List Vertical demo follow-up — 2026-11-13

Rebuilt the Vertical demo from React's source: 23 deterministic records, large
List size, three items per page, item-specific Dicebear avatars, the same action
icons and counts, and the non-draggable 272px extra image. The image now shrinks
to its flex column (`max-width: 100%`), and the List meta title uses React's
400-weight rendering. Removed React-only `key` attributes that had been emitted
into Blazor HTML and restored the two `SpaceItem` wrappers for each action.

The focused browser fixture passes 1/1 state with zero normalized DOM, computed
CSS, geometry, and visual-style differences:
`tools/AntDesign.StyleGen/examples/list-vertical-dom-style-parity.json`
(`/tmp/list-vertical-parity-20261113-verified.json`). Both pages show 3 items,
the same eight-page pagination, and matching footer text. Docs.Server
`net10.0` build passed with 0 errors.

## Table Fixed Gapped Columns follow-up — 2026-11-13

Removed demo-only column widths and class names, selected `TableLayout="auto"`
to match React's `table-layout: auto`, and added the row key. The focused
fixture now has matching computed styles, geometry, and visual styles. Six DOM
differences remain: the start/end shadow classes are missing from the final
fixed header/body cells. Follow up in the shared fixed-column class calculation
before treating this demo as complete. Evidence:
`tools/AntDesign.StyleGen/examples/table-fixed-gapped-columns-dom-style-parity.json`
(`/tmp/table-fixed-gapped-columns-parity-20261113-rowkey.json`).

## Select Semantic preview focus and Icon gallery recheck — 2026-11-13

The Select Semantic DOM preview's Multiple mode was incorrectly given input
focus when responsive-tag measurement ran for its controlled-open popup. The
focused root class changed its border and shadow from React's unfocused state.
Responsive-tag measurement now restores focus only for an internally opened
popup or when the caller explicitly requests focus restoration. The semantic
preview passes both Single and Multiple mode states (2/2) with zero normalized
DOM, computed-style, geometry, and visual-style differences:
`/tmp/select-semantic-preview-focus-fix-20261113.json`.

Re-ran the Icon gallery responsive fixture at desktop and narrow widths; both
states pass (2/2) with zero normalized differences:
`/tmp/icon-gallery-responsive-followup-20261113.json`. This covers the gallery
toolbar and responsive structure, not every Icon search/two-tone interaction.

The Table Fixed Gapped Columns fixture was also rerun after rebuilding the
current server. It still reports six fixed-edge shadow class differences while
computed CSS, geometry, and visual styles match. An attempted index-order
variant did not remove the differences and was reverted; keep this Table issue
open rather than crediting that change as a fix.

## Table fixed-column shadow segments — 2026-11-13

The fixed-edge classes must mark every boundary of a contiguous fixed-column
segment. For example, the gapped-columns demo has two isolated start-fixed
columns and two isolated end-fixed columns, so React marks all four. The prior
global first/last-column calculation marked only one per side. `ColumnBase`
now detects a segment boundary from the immediately adjacent column on that side.
The gapped demo and the regular fixed-columns and fixed-columns-header demos all
pass their focused fixtures (1/1 each) with zero normalized DOM, computed-style,
geometry, or visual-style differences:

- `/tmp/table-fixed-gapped-columns-segment-fix-20261113.json`
- `/tmp/table-fixed-columns-regression-20261113.json`
- `/tmp/table-fixed-columns-header-regression-20261113.json`

The Docs.Server `net10.0` build passes with 0 errors. The earlier Table entry
above records the initial mismatch and is superseded by these passing reruns.

## Select and Icon documentation inventory recheck — 2026-11-13

The refreshed name/runtime inventory reports all 5 React Icon demos matched,
all 28 React Select demos matched, no missing React demo names, and no blocked
pages. Icon has no runtime differences in this inventory. Select has one raw
HTML and one geometry flag, both on `_semantic`: HTML samples are style
attribute whitespace serialization (the focused normalized Semantic DOM states
pass 2/2), while the geometry samples are the popup captured after the
inventory's full-page lazy-load scroll. Select also has 23 Blazor-only examples.
Report: `/tmp/select-icon-inventory-recheck-20261113.json`.

## Table Reset Filter and ellipsis title alignment — 2026-11-13

The Reset Filter toolbar now wraps each Button in `SpaceItem`, matching React's
`.ant-space-item` wrappers and inline-flex button layout. Numeric ellipsis cells
now use `CellData.FormattedValue` for their `title` attribute; previously only
string-valued fields received a title. The focused Table fixture checks the
toolbar separately from the table root and passes the initial table, Sort age
state, and toolbar structure (3/3 total) with zero normalized DOM, computed CSS,
geometry, or visual-style differences:
`tools/AntDesign.StyleGen/examples/table-reset-filter-dom-style-parity.json`
(`/tmp/table-reset-filter-verified-20261113.json`).

A fresh Table route inventory has all 43 React demo names matched, no blocked
page, and currently counts 35 raw HTML, 8 computed-style, and 8 geometry demo
differences; the previous scan counted 35/9/8. Reset Filter no longer appears
among the computed-style and geometry differences; its inventory HTML flag is
only the raw `margin-bottom:16px` versus `margin-bottom: 16px` serialization on
the Space toolbar, which the focused fixture normalizes. Continue with remaining
Table differences, especially Dynamic Settings, Style and Class, Virtual List,
and drag demos.
Report: `/tmp/table-inventory-reset-filter-20261113.json`.

## List Vertical responsive recheck and Table Dynamic Settings — 2026-11-14

Rechecked List Vertical at desktop, tablet, and narrow viewport widths after the
reported layout issue. The registered desktop and tablet scenarios pass, and a
direct narrow-width browser inspection confirms the List root stays within the
demo card with no horizontal overflow. The focused fixture remains 1/1 with no
normalized DOM, computed CSS, geometry, or visual-style differences:
`tools/AntDesign.StyleGen/examples/list-vertical-dom-style-parity.json`
(`/tmp/list-vertical-final.json`). The reported overflow could not be reproduced
in the current page state.

Completed the Table Dynamic Settings comparison. The demo now uses the React
sortable Action column, matching expand-before-selection column order, React's
inline control-bar spacing, and the same Space wrappers for action links. The
control bar and rendered Table groups pass all three initial/interaction states
(3/3) with zero normalized DOM, computed CSS, geometry, or visual-style
differences. The shared expand-column and Radio-in-Form behavior also pass the
Table row-selection and expand-column regression fixtures (3/3 each):

- `tools/AntDesign.StyleGen/examples/table-dynamic-settings-dom-style-parity.json`
  (`/tmp/table-dynamic-settings-final-20261114.json`)
- `/tmp/table-row-selection-regression-20261114.json`
- `/tmp/table-expand-columns-regression-20261114.json`

Docs.Server `net10.0` builds successfully with 0 errors. React and Blazor Table
Dynamic Settings pages are open for manual inspection. Next Table demo: Style
and Class, then Virtual List and drag demos.

## Table Style and Class semantic styles — 2026-11-14

Replaced the placeholder Style and Class demo with the React example's two
tables, shared four-row data, semantic object styles, custom root class, title,
footer, three-row pagination, and a size-sensitive style callback. Added
`ClassNamesFunc` and `StylesFunc` to Table, matching the existing semantic
callback pattern used by other components. Table's `root` semantic class/style
now applies to `.ant-table-wrapper`, as observed in React, while the component's
`Class` and `Style` remain on the inner `.ant-table` element.

The non-virtual Medium table's complete DOM, computed styles, geometry, and
visual styles pass (1/1):
`tools/AntDesign.StyleGen/examples/table-style-class-dom-style-parity.json`
(`/tmp/table-style-class-final4-20261114.json`). The first React table uses
`virtual`; the local Blazor docs server runs a `net10.0` app rolled forward to
.NET 11 and its `Virtualize` call fails because
`Blazor._internal.Virtualize.setAnchorMode` is missing from the served runtime
script. The Blazor demo therefore currently uses `ScrollY` without virtual
rows, so the first table still has a React DOM/layout gap. Keep this explicit
and address Table virtualization before marking the whole demo complete.

To close incidental parity gaps exposed by the fixture, title/footer markup no
longer emits indentation as content; simple Pagination now applies `ItemStyle`
and emits React-compatible `aria-disabled` strings and the Go to input label.
The parity runner gained opt-in attribute-order normalization, and the
row-selection fixture now normalizes equivalent inline-style spacing. Focused
regressions pass: Dynamic Settings 3/3, row selection 3/3, and expand columns
3/3, all with zero normalized DOM/CSS/geometry/visual-style differences:

- `/tmp/table-dynamic-settings-regression-20261114.json`
- `/tmp/table-row-selection-final-20261114.json`
- `/tmp/table-expand-columns-regression-20261114b.json`

Next: investigate Table virtualization/runtime alignment, then continue the
remaining high-volume Table demos (Virtual List and drag demos).

## Table virtualization follow-up — 2026-11-14

Moved `AntDesign.Docs.Server` to native `net11.0` and added `net11.0` to the
Docs target frameworks so the app serves the .NET 11 Blazor runtime required by
the .NET 11 component assembly. Confirmed the served `blazor.server.js`
contains `Blazor._internal.Virtualize.setAnchorMode`. Restored
`EnableVirtualization` on the Style and Class demo's first table. Build passes
with 0 errors (23 existing package/target-framework warnings).

The ordinary Medium table in the same demo still passes its focused normalized
DOM, computed CSS, geometry, and visual-style fixture (1/1):
`/tmp/table-style-class-function-20261114.json`. The virtual table fixture
`table-style-class-virtual-dom-style-parity.json` exercises the React virtual
demo and exposes a remaining structural mismatch: Blazor renders `Virtualize`
spacer rows inside the normal `<tbody>` and keeps the standard `.ant-table-body`
table layout, while React uses `.ant-table-virtual`,
`.ant-table-tbody-virtual`, a virtual holder, and flex `div` rows/cells.
Therefore, upgrading the runtime restores the required JS API but does not by
itself align Table's virtual rendering. Keep this issue open; do not count the
first Style and Class table as aligned yet. Report:
`/tmp/table-style-class-virtual-20261114.json`.

The rebuilt Blazor docs page is open at
`http://127.0.0.1:5000/en-US/components/table#components-table-demo-style-class`
for manual inspection. This remains a subset of the 71-component baseline;
the full progress matrix is in `runtime-parity-summary-2026-11-12.md`.

## Table virtual rows follow-up — 2026-11-14

After enabling the React-like virtual holder, the Blazor holder now uses
`overflow:hidden` when the current rows fit and `overflow:auto` when the rows
need a scroll window. The virtual-width synchronizer measures the inner
`.ant-table` content box directly. This removes the remaining normalized DOM
difference in the small Style and Class virtual table. The ordinary
function-style Table still passes 1/1 with no normalized DOM/CSS/geometry/
visual-style differences:

- Virtual table: `/tmp/table-style-class-virtual-final-20261114.json`.
- Ordinary table: `/tmp/table-style-class-normal-final-20261114.json`.

The virtual table now has zero normalized DOM differences and matching root
styles. Its visual-style report retains six subpixel measurements: three body
rows and their final cells are 0.016px narrower than React (731.312px vs
731.328px for rows; 185.312px vs 185.328px for the final cells). This is below
one CSS pixel but remains recorded as a visual geometry difference; do not
claim complete virtual-table parity yet. The Blazor page is left open on the
Style and Class demo for manual review. React's docs page continues to emit its
known `define is not defined`/contributor-fetch errors; the focused fixture
still captures the expected static DOM.

## Table Virtual List window follow-up — 2026-11-14

Reduced `Virtualize` overscan from 10 to 0 in both Table virtualization paths.
At the demo's 400px viewport and 55px row size, the Blazor window shrank from
30 rendered rows (981–1010) to 10 (991–1000); React renders 10 rows (992–1001)
in the same `Scroll To index 999` state. Both sides use a 2,000px table,
matching header column widths `[48,100,120,120,120,100,414,414,414,150]`,
55px rows, and 550,000px virtual content height. The visible index-999 state,
Empty toggle, and expandable switch were exercised in both browser pages.

Focused report: `/tmp/table-virtual-list-parity-overscan0-20261114.json`.
Root computed styles match, but full DOM/visual parity remains incomplete:
Blazor uses `Virtualize` spacer elements and leaves its inner transform at zero,
while React uses a translated window; the Blazor row window starts one row
earlier, and fixed-column sticky offsets differ in the full rendered snapshot.
The runner reports 254 normalized DOM differences and semantic visual-style
differences, so this demo is not marked aligned. Next, align virtual window
offset/spacer markup and fixed-column offsets, then continue Table drag demos.

The focused fixture now also checks rowSpan cloning and merged-cell geometry:
`tools/AntDesign.StyleGen/examples/table-virtual-list-dom-style-parity.json`.
The JS synchronizer creates React-like `.ant-table-row-extra` overlays for the
four-row Group spans, hides the corresponding cells in normal rows, removes
Blazor private attributes from clones, and applies React's negative trailing
margin to merged columns. At index 999, both sides have `scrollTop=54600`; the
Group overlays for rows 992/996/1000 are each 220px high, and row 992 remains
40px above the holder top. Semantic counts now match (130 body cells, 13 rows)
and geometry checks pass. Strict comparison still fails (250 normalized DOM
tree differences and one component-level computed-style difference): Blazor's
Virtualize spacer and extra preceding row 991 remain, as do transform/overflow,
header colgroup, fixed-offset serialization, and six merged-cell width
positions in semantic ordering. The 71-component baseline is still partial.

Follow-up alignment for this state made all virtual header `<col>` widths
explicit, converted fixed-column offsets from `calc(...)` strings to measured
pixel values, synchronized the right fixed-column shadow class on the actual
`.ant-table` root, and removed the extra `scope="col"` from the virtual
selection header. The focused runner now reports 223 normalized DOM
differences (down from 250); geometry still matches. Remaining computed
differences include the extra Blazor row 991/spacer, scroll holder overflow and
inner transform, and the shifted semantic ordering of merged Age cells. Latest
report: `/tmp/table-virtual-list-final-current-20261114b.json`.

## List Vertical mobile offset correction — 2026-11-14

The 500px browser comparison exposed a page-container offset that the List-only
fixture does not measure: React places the Vertical demo card at `x=48`, while
Blazor's mobile component page shifted its main container left by 48px and
placed the same card at `x=0`. Removed that extra `inset-inline-start` from the
mobile component-page rule in `MainLayout.razor`. After rebuilding and
reloading, both pages place the demo card at `x=48` with a width of 288px; the
List root and first item also match at `x=73` with a width of 238px. Item
dimensions and internal vertical layout remain aligned.

## Table Virtual List fixed-shadow recheck — 2026-11-14

The current live page showed the fixed-end shadow class on the table root but
Blazor rerendering removed the corresponding class from fixed header cells.
Added a header class observer that re-applies the measured fixed-edge state
after render; browser inspection at `scrollTop=54600` confirms the header and
all rendered fixed-end cells retain `ant-table-cell-fix-end-shadow-show`.
Focused runner differences decreased from 223 to 221. Computed root styles
remain equal and visible geometry remains aligned. Remaining differences are
led by framework `Virtualize` spacer markup, the extra row 991, holder
overflow/inner transform, and merged-cell semantic ordering. Report:
`/tmp/table-virtual-list-shadow-fixed-20261114.json`.

## List drag demos DOM and preview follow-up — 2026-11-15

Added a four-demo DOM/style fixture at
`tools/AntDesign.StyleGen/examples/list-drag-sorting-dom-style-parity.json`.
The sortable content now uses the same plain child structure as React (without
the internal `data-sort-handle` marker), and handle mode locates the Ant Button
directly instead of adding a demo-only class. Handle-mode text includes the
item number and uses `ListItem.NoFlex` to match React. ListItem omits an empty
inline `style` attribute, and grid mode now applies React's
`.ant-list-container` class to the row.

The Docs.Server `net11.0` build succeeded with zero errors. The final
React/Blazor DOM, computed-style, and geometry fixture passes all four demos
(0.04px tolerance for subpixel width serialization); report:
`.tmp/list-drag-sorting-current.json`. Browser pointer checks also pass in all
four modes: the active row/card receives a live transform and `z-index:9999`,
the order updates after release, and the temporary transform/dragging class is
removed. No page errors occurred. The current development Docs.Server remains
running on `http://127.0.0.1:5000` with the List page open for manual review.

## Icon gallery Two Tone fill parity — 2026-11-14

When a primary color is passed to a generated Two Tone icon, React applies it
to SVG paths that do not declare their own fill. The Blazor icon generator had
only replaced the source's explicit primary and secondary placeholders, leaving
these paths to inherit `currentColor`. Updated `TwoToneColorHelper` to apply the
requested primary color to unfilled `<path>` elements while preserving explicit
fills such as `none`, style-based fills, the generated secondary swatch, and the
source palette when no primary is supplied. The icon package is now referenced
at `0.2.0-beta.3-local.20261114.1` by the docs workspace.

The icon-library helper suite passes 26/26 with .NET major roll-forward. Docs
Server `net11.0` builds with 0 errors. The full 842-icon gallery DOM/CSS
comparison passes desktop and narrow viewports 2/2 with no DOM or computed CSS
differences; its fixture omits `transform` because the gallery contains an
always-spinning loading icon whose animation phase is nondeterministic. Report:
`/tmp/icon-gallery-grid-current-20261114-final.json`.

## Select Semantic DOM preview and List Vertical recheck — 2026-11-14

The controlled-open Select Semantic DOM preview passes both Single and Multiple
states with no normalized DOM, computed CSS, geometry, or visual-style
differences (`/tmp/select-semantic-preview-current-20261114.json`). List
Vertical also passes desktop, tablet, and narrow-screen comparison 3/3
(`/tmp/list-vertical-current-20261114.json`). Browser inspection at 500px
confirmed the image, main content, and action row match React; a site-wide
sponsorship overlay temporarily covered the top of the Blazor viewport and was
not a List layout difference.

## List drag sorting previews and style alignment — 2026-11-15

Aligned the four List sorting demos with the React dnd-kit behavior: pointer
movement is restricted to the Y axis for vertical lists and remains free on both
axes for grids. Like React, the active item follows the pointer with a raised
stacking order while sibling items animate to the prospective insertion slots;
the transition is `transform 200ms`, matching the inline transition emitted by
`@dnd-kit/sortable`. No extra opacity or cursor state is added to the active row.
Temporary transforms and stacking
styles are cleared after drop or cancel, and the client module URL is versioned
to prevent stale browser module cache from hiding the fix.

Browser interaction checks covered drag sorting, drag sorting with handle, grid
drag sorting, and grid drag sorting with handle. Each showed the active item
transform and sibling preview before drop, reordered the Blazor data on pointer
release, and cleared temporary inline styles afterward. The vertical drag
continued to move 0px horizontally even when the pointer moved sideways. React and Blazor
vertical List item geometry/computed styles match at 422x47px with 12px vertical
padding and `cursor: move`. The helper omits an extra `ease` token and
`will-change` that are absent from React. The live Blazor page was reloaded with
the new module; synthetic pointer movement produced the active translation and
three sibling slot previews. React and Blazor List docs are open for manual
review.

## Upload Default Files and File List follow-up — 2026-11-15

Updated the Upload `Default Files` demo to use React's three `xxx.png`/`yyy.png`/
`zzz.png` records with uploading/done/error states. Text-list items now follow
React's item-container and sibling icon/name/actions structure, and status
classes are emitted on the item. Progress omits an empty rail-color declaration
when no custom trail color is set. The File List demo now uses React's `xxx.png`
record, URL, field name, and direct button text. The expanded Basic/Default
Files/File List/Drag fixture passes 4/4 with zero normalized DOM, computed CSS,
geometry, or visual-style differences: `/tmp/upload-four-demos-final.json`.
The only geometry allowance is a 0.02px tolerance for 1/64px computed width
serialization. Docs.Server `net11.0` builds with zero errors; other Upload demos
remain in the page-level audit.

The `_semantic` Upload demo is now registered in generated docs metadata and
passes through the same Semantic DOM preview used by other components. Added
the root/trigger/list/item marker classes from React's preview. The focused
five-demo fixture (Basic, Default Files, File List, Semantic DOM, Drag) passes
5/5 with zero normalized DOM/CSS/geometry/visual-style differences:
`/tmp/upload-five-demos-final-20261115.json`. Remaining Upload demos and the
full page inventory are still outstanding.

The refreshed Upload route inventory reports 22 React demos, 25 Blazor demos,
22 matched names, no missing React names, and three Blazor-only demos. Broad
initial-state H/C/G candidates remain in 19/17/17 matched demos; one page was
blocked by a React runtime error. This whole-page scan is a triage signal, not
an acceptance gate, because the five scoped Upload examples pass the dedicated
fixture. Report: `/tmp/upload-inventory-latest-20261115.json`.

## Table responsive columns and documentation spacing — 2026-11-15

Added the React-compatible `Responsive` breakpoint parameter to Table columns,
including selection and action columns. The responsive demo now uses the same
single data row and breakpoint-driven columns as React, with its default
pagination retained. Initial viewport width is read before first-render early
returns; resize updates flow through a changing cascading viewport value so
already-mounted column components recalculate visibility.

The focused fixture at
`tools/AntDesign.StyleGen/examples/table-responsive-columns-dom-style-parity.json`
passes desktop (1280px), medium (800px), and narrow (600px) scenarios 3/3, with
zero normalized DOM, computed-style, geometry, or visual-style differences.
Report: `/tmp/table-responsive-columns-20261115e.json`. Docs.Server `net11.0`
build succeeds with 0 errors.

The documentation shell keeps React's 32px normal-flow gap below its 64px
sticky header. Browser measurements match: the Blazor article content starts at
y=96px and moves to y=-24px after scrolling 120px while the header stays at
y=0..64px. The containing `.main-wrapper` begins at y=64px and supplies 32px
top padding.

## Documentation header theme controls — 2026-11-15

Added the missing vertical separator beside desktop navigation and made the
theme controls available as menu items so they flow into the responsive
overflow menu. At 900px, browser DOM inspection confirms the overflow popup
contains Follow System, Light theme, Dark theme, Compact theme, Happy Work
Effect, Theme Generator, and Theme Editor. Dark mode uses the same `#141414`
documentation surface for `.main-wrapper` and the central article area,
including the normal-flow gap below the sticky header. Browser measurements at
1280px confirm the 1px by 24px separator and matching dark backgrounds. Docs.Server
`net11.0` builds with 0 errors; opening the overflow popup and selecting an
option was then checked in the narrow navigation: selecting Light theme closed
the popup and changed the theme; reopening it and selecting Dark theme restored
the original dark appearance.

## Table responsive columns follow-up — 2026-11-15

The focused React v6 comparison covers desktop, medium, and narrow viewport
states for responsive columns. All three report scenarios pass with matching
normalized DOM and computed styles; report:
`/tmp/table-responsive-columns-20261115e.json`. This confirms the added
`Responsive` breakpoint support for data, selection, and action columns at the
tested states. The larger Table virtual-list fixture remains the next Table
gap: its React-style translated virtual window and fixed-column offsets still
have structural differences, recorded above.

## Select and Icon documentation recheck — 2026-11-15

Re-ran the Select core manifest (size, variant, and placement) at the shared
1280px viewport: 6/6 scenarios pass with zero normalized DOM, computed CSS,
geometry, or visual-style differences (`/tmp/select-core-review-20261115.json`).
The controlled-open Semantic DOM preview passes 2/2 with the same zero-difference
result (`/tmp/select-semantic-review-20261115.json`).

The five canonical Icon demos pass 5/5, and the 842-icon responsive gallery
passes desktop and narrow layouts 2/2; both have zero normalized differences
(`/tmp/icon-canonical-review-20261115.json` and
`/tmp/icon-gallery-review-20261115.json`). A browser measurement at 1280px,
after scrolling both docs pages to `scrollY=1200`, places the segmented/search
controls at y=80 / y=88 in React and y=80 / y=89 in Blazor, respectively.

## Icon gallery Search geometry correction — 2026-11-15

The Blazor Search component's group wrapper was 44px tall while React's
compact Search wrapper is 40px; its inner input group also started 1px lower.
The gallery now constrains the Search wrapper to 40px and offsets the inner
group up by 1px. Fresh React/Blazor browser measurements at 1280px and 800px
show the same 40px segmented control, 40px toolbar Search wrapper, 24px input,
and an 8px input-to-segment top offset in both pages. This closes the reported
1px input offset in the live toolbar. The 842-icon responsive grid fixture
continues to pass desktop/narrow checks. React still clips long labels while
Blazor wraps them intentionally. Whole-route Select/Icon demo/API/token
coverage remains incomplete; continue with Table's virtual-list structure.
## Table Virtual List range preservation — 2026-11-15

Changed the client-rendered virtual window so a data refresh with an unchanged
row count preserves the current start/end indices. Reset to the initial range
only on first initialization or when the total row count changes. The callback
also uses a dedicated one-render request flag to avoid the Table's shared
`ShouldRender` gate dropping the scroll update.

Verified against React's `table-demo-virtual-list` and Blazor's
`components-table-demo-virtual-list` after clicking “Scroll To index 999”:

- Blazor `scrollTop=54600`, viewport `400px`, window indices `992–1001`, and
  translated inner window `54560px`, matching the React capture.
- Added React-compatible virtual scrollbar nodes and matched their range,
  position, visibility, and thumb style. The focused comparison
  `/tmp/table-virtual-final-report.json` passes 1/1 with zero normalized DOM,
  computed CSS, geometry, and visual-style differences.
- Toggling Empty and restoring the 10,000-row source resets to row 0 and a
  `translateY(0px)` window.
- Dragging the vertical scrollbar thumb moved `scrollTop` from 54600 to 141379
  and changed the visible row window from 992 to 2570.

The docs server is built with the .NET 11 preview SDK and left running on port
5011 for manual review. This closes the focused Virtual List fixture; other
Table demos and the rest of the component matrix remain in progress.

## Table Nested Table and initial expansion keys — 2026-11-15

Added `Table.DefaultExpandedRowKeys`, matching the React `defaultExpandedRowKeys`
behavior against each row's `RowKey`, and documented the parameter in both Table
API pages. The nested row content remains mounted but hidden after its first
expansion, matching React's collapse/reopen DOM lifecycle.

Rebuilt the Nested Table demo to match React's three outer tables (default,
medium, and small), each with three rows and a nested table in the first row.
The nested table uses the same columns, status badge, action links, dropdown,
and sample data. `Unbound` keeps the dropdown trigger anchor free of an extra
wrapper.

The focused fixture `tools/AntDesign.StyleGen/examples/table-nested-table-dom-style-parity.json` (verified via `/tmp/table-nested-table-current-report5-20261115.json`)
passes initial expansion, collapse, and reopen (3/3) with zero normalized DOM,
computed CSS, geometry, or visual-style differences. The Tree Data regression
fixture `/tmp/table-tree-data-current-report-20261115.json` also passes all
three states after the shared row-state change.

The docs site was rebuilt for `net11.0` with 0 errors and restarted on port
5011. The browser remains open at
`http://127.0.0.1:5011/en-US/components/table#components-table-demo-nested-table`
for manual review. The broad 71-component inventory remains a dated baseline;
remaining Table demos and all other first-phase API, token, docs, Theme Preview,
and Semantic DOM work are still in progress.

## Select and Icon focused recheck — 2026-11-16

Re-ran the Select interaction and Semantic DOM fixtures against the current
Docs.Server on port 5011, preserving each fixture's own normalization rules.
The interaction scenarios pass 4/4 and the controlled single/multiple Semantic
DOM previews pass 2/2, with zero normalized DOM, computed CSS, geometry, or
visual-style differences. Reports:
`/tmp/select-interaction-current-report-20261116.json` and
`/tmp/select-semantic-preview-current-report-20261115.json`. The latter report
uses the same current Select markup; only its filename reflects the first
capture date. The prior six-scenario Select core fixture still covers size,
variant, and placement; whole-route API, demo, and token audits remain open.

The Icon toolbar responsive fixture passes desktop and narrow states (2/2):
`/tmp/icon-gallery-responsive-current-report-20261115.json`. The five canonical
Icon demo fixtures were rechecked on 2026-11-15 and pass 5/5. The 842-icon grid
fixture has matching normalized DOM and desktop root geometry; its text labels
and narrow layout intentionally differ from React. React keeps names on one
line inside 100px cells and clips overflow; Blazor wraps names and uses two
readable columns at 500px so names remain visible. Tightened the wrapped label
line height to 1.25 while keeping the 8px icon spacing; this keeps the desktop
cell height at 100px. Latest raw comparisons are in
`/tmp/icon-gallery-grid-current-report-20261116c.json` (2 scenarios; no capture
blockers; expected label and narrow-grid differences remain). Keep this
intentional accessibility/legibility tradeoff visible in follow-up comparisons;
do not treat it as strict React visual parity. Full Icon docs/API/token audit
remains open.

The full 71-component matrix at the beginning of
`runtime-parity-summary-2026-11-12.md` is the progress index; its untouched rows
remain the prior broad-scan baseline and are not claims of full parity. The
current next focus is the remaining Select route/API/token coverage, then the
Icon docs/token audit before continuing down the component matrix.

## Slider Basic parity — 2026-11-16

Compared React's `components/slider/demo/basic.tsx` and rendered Slider nodes to
the Blazor Basic demo. The Blazor sample initially used 35 instead of React's
30 and rendered an extra marks/range slider. Its ordinary values also inserted
an `.ant-slider-tracks` wrapper around the active track, emitted an empty
`.ant-slider-mark`, serialized percentages as `30.00%`, added `right:auto`, and
omitted `aria-orientation` from handles.

The Basic demo now uses the React values and only the two sliders plus Disabled
switch. The component renders tracks directly for ordinary sliders, reserves
the tracks wrapper for multi-value tracks, omits marks when not configured,
emits `aria-orientation`, and formats percentage styles without unnecessary
decimal places or `auto` declarations. `user-select:none` is supplied by the
component stylesheet rather than an extra inline root style.

The focused fixture
`tools/AntDesign.StyleGen/examples/slider-basic-dom-style-parity.json` passes
1/1 against React v6 on 2026-11-16. Normalized DOM tree, selected computed CSS,
geometry, and visual styles all match; raw HTML remains different due to
attribute ordering and Blazor renderer markers. Report:
`/tmp/slider-basic-dom-style-parity-20261116-final.json`. This result covers
only Basic; the remaining 14 demos, API, Semantic DOM, and component token docs
remain open. Continue with the Select API/docs reconciliation, then Icon docs
and tokens, per the first-stage priority.

## Select DefaultOpen behavior — 2026-11-16

Added the `DefaultOpen` API corresponding to React v6 `defaultOpen`, added its
API rows in English and Chinese, and created a Blazor-only focused demo because
React has no demo with this API state. Refreshed generated demo metadata using
the CLI under .NET 11 major roll-forward (the CLI project targets .NET 10, which
is not installed in this environment); rebuilt Docs.Server for `net11.0` with
0 errors. Chrome verified that the popup is visible on first render and closes
after an outside click. This confirms the Blazor behavior and route, but does
not count as React DOM/CSS parity. The current tab remains on
`http://127.0.0.1:5011/en-US/components/select#components-select-demo-default-open`.

## Select OptionRender API — 2026-11-16

Added the React-compatible `OptionRender` parameter; `ItemTemplate` remains a
supported alias and is used when `OptionRender` is omitted. Updated the existing
Option Render demo and the Select API tables in both locales. The focused
React/Blazor open-popup fixture
`tools/AntDesign.StyleGen/examples/select-option-render-dom-style-parity.json`
passes normalized DOM tree, computed CSS, geometry, SVG use, and visual-style
checks. The runner still reports raw `htmlExact` difference from Razor's
whitespace serialization; the structured DOM tree is equal. Report:
`/tmp/select-option-render-option-api-20261116-final.json`. Docs.Server
`net11.0` build succeeds with 0 errors. Browser is left at
`http://127.0.0.1:5011/en-US/components/select#components-select-demo-option-render`.

## Select selection and clear callbacks — 2026-11-16

Added React-compatible `OnSelect` and `OnDeselect` callbacks that receive the
selected `SelectOptionItem<TItemValue, TItem>` (value, label, and source item).
`OnDeselect` runs only when an item is toggled off in multiple or tags mode.
Added `OnClear` for the clear action while preserving the existing
`OnClearSelected` callback. Both English and Chinese Select API tables now
describe these parameters. Docs.Server `net11.0` builds with 0 errors, and the
browser confirms the published English Markdown contains all three API rows.
The Select inventory is now 28 React / 53 Blazor / 28 matched; the Events demo
is a Blazor-only API exercise.

## Select input and popup event callbacks — 2026-11-16

Added React-named `OnInputKeyDown` and `OnPopupScroll` event parameters. The
input callback receives `KeyboardEventArgs`; the popup callback receives the
scroll event as `EventArgs`. Both event rows are documented in English and
Chinese, and the new Events demo is registered in both generated component
catalogs and in the compiled docs metadata/type map.

Docs.Server `net11.0` builds with 0 errors (4,187 existing warnings). In the
browser at 390×844, the demo produced `Selected: Option 1`, `Deselected: Option
1`, `Cleared selection`, `Key: ArrowDown`/`Key: Enter`, and `Popup scrolled`.
The Select page is left open at `http://127.0.0.1:5011/en-US/components/select#components-select-demo-events`;
the browser console had no errors after the fresh navigation. This verifies
the event wiring and docs entry, not parity of all Select demos or remaining
API, token, Semantic DOM, Icon, and docs scope.

## Design Token value previews — 2026-11-16

Added a reusable `DesignTokenValue` display to component and global token
reference tables. Color tokens show an inline color dot next to their value;
`motionEase*` cubic-bezier tokens show an SVG grid, easing curve, and control
points. Other values remain code text. Docs.Server `net11.0` builds with 0
errors (591 existing warnings). Browser verification on Button Design Token
found 25 color swatches and one cubic-bezier chart, with visible grid and blue
curve strokes. This validates the visualization, not underlying token value
parity or complete coverage across all docs pages.

## Icon documentation structure and API summaries — 2026-11-16

The Blazor Icon Gallery now renders as a standalone section before Examples, as
in React v6; it is no longer nested inside the examples heading/card list. The
TOC links to “List of icons” independently. Browser inspection confirmed the
React and Blazor section order, the Blazor gallery search, six categories, and
842 rendered icon entries. Corrected the Icon `Role` and `Spin` XML summaries,
added the Chinese translations, regenerated component metadata, and documented
`OnKeyDown`. The live API table displays the updated summaries and keydown row.
Docs.Server `net11.0` builds with 0 errors (591 existing warnings). This closes
these documentation issues only; it does not complete Icon API/token/FAQ or
all-demo visual parity.

## Select React-named list and icon props — 2026-11-16

Added `ListHeight` as a React-compatible alias for `PopupContainerMaxHeight`.
Added React-named `Prefix`, plus customizable `RemoveIcon` and
`MenuItemSelectedIcon` render fragments. Existing Blazor parameters and default
icons remain available. Added the English/Chinese API rows and an `api-aliases`
demo with a multiple Select and a 128px option list.

Docs.Server `net11.0` builds with 0 errors (591 existing warnings). The demo is
present in generated metadata for both locales. Browser inspection confirms
the prefix, custom remove glyphs, custom selected-option glyph, and
`max-height: 128px`; clicking a remove glyph removes its selected tag. The
console has no errors. This verifies the newly added API wiring and docs, not
whole-route parity. Select currently has 54 Blazor demos, 28 React demos, and
all 28 React demo names represented. The broad runtime H/C/G inventory remains
stale and must be refreshed before using those counts as current parity status.

## Select multi-field search configuration — 2026-11-16

Added `tools/AntDesign.StyleGen/examples/select-multi-field-search-dom-style-parity.json` for the React v6 `showSearch.optionFilterProp` multi-field case. The focused runner passes 1/1: normalized DOM, selected computed styles, geometry, and visual-style checks agree. Raw serialized HTML differs only because of Razor whitespace and attribute serialization. Browser interaction comparison with query `c11` returns the matching `a11` item in both React and Blazor. This is one targeted fixture; it does not refresh the broad 71-component H/C/G inventory.

## Select controlled search value — 2026-11-16

Added a Blazor controlled-search demo using `@bind-SearchValue`. Browser interaction verification changes the external input to `jack`; the Select's internal search input updates to `jack` and its popup filters to the `Jack` option. The Docs.Server page has no console errors. This is a behavior/API check for a Blazor-only exercise, not a React DOM/style parity fixture; the broad 71-component H/C/G inventory was not rerun.

## Select field defaults and dim-sum demo renderers — 2026-11-16

Aligned Select's default `FieldNames.GroupLabel` with React v6 (`label`) and
the fallback `OptionFilterProp` with React v6 (`value`). Set the legacy
top-level Blazor alias default explicitly so the generated API table also shows
`value`. English and Chinese FieldNames rows now display React's complete
default mapping. The current React API table confirms the same defaults and
the `string[]` OR search behavior.

Browser review exposed that dim-sum was emitting demo type names as strings
without emitting a runtime `Type`; the old `DemoService` reflection hookup
therefore left demos blank. Updated dim-sum to consume the type name only while
generating source and emit a strongly typed `RenderFragment` for every demo.
Removed the runtime type-name/Type lookup from `DemoService`; source paths are
generated at compile time. Docs.Server `net11.0` builds with 0 errors (641
existing warnings). Browser confirms the Field Names demo now renders its
Select and source edit link, the API table shows `groupLabel: "label"` and
`OptionFilterProp` default `value`, and the freshly navigated page has no
console errors. This validates the generator path and Select defaults, not the
full component DOM/style inventory.

`AntDesign.Docs.Build.CLI` has been removed completely. Token metadata input
generation now runs through StyleGen's `sync-v6`; static LLM Markdown export
uses StyleGen's `export-llms` against the compiled docs routes. Documentation
builds no longer use the old CLI project or its runtime translation service.

## Slider Reverse and Vertical demo source alignment — 2026-11-16

Compared against the current Ant Design v6 demo sources at
`components/slider/demo/reverse.tsx` and `vertical.tsx` from the upstream GitHub
repository. Reverse used `33` where React uses `30`; Vertical used `31` instead
of `30`, left-only margins instead of `marginInlineStart`, and the Unicode
degree-Celsius symbol instead of React's `°C` labels. Updated these source-level
discrepancies. This is demo source alignment, not a browser DOM comparison: the
React docs server is unavailable in this environment. A focused browser capture
and runtime DOM/CSS/geometry comparison remain outstanding.

## Slider Tooltip and TipFormatter demo source alignment — 2026-11-16

Compared with upstream React v6 `show-tooltip.tsx` and `tip-formatter.tsx`.
The always-visible tooltip example already matches React's default value 30 and
open tooltip behavior. The TipFormatter example previously rendered one
controlled slider at 34; React renders two uncontrolled default sliders, one
with a percent formatter and one with the tooltip disabled. Updated the Blazor
demo and localized guidance to match these two scenarios (`HasTooltip=false`
is the Blazor API equivalent of React's `tooltip.formatter: null`). Source-level
alignment only; live React DOM/CSS/geometry capture remains pending because the
React docs service is unavailable.

## Slider Multiple Handles and Disabled per handle — 2026-11-16

Compared to React v6 `multiple.tsx` and `disabled-handle.tsx`. Multiple Handles
now recalculates both gradient endpoint colors from the first and last slider
values (matching React's RGB interpolation), instead of using a fixed full-range
gradient. Disabled per handle now supplies React's `minCount: 2` and
`maxCount: 5` along with draggable track and the three per-handle disabled
controls. Updated both localized descriptions. Runtime DOM/CSS/geometry
comparison against React remains pending because its docs server is unavailable.

## Header navigation and utility icons — 2026-11-16

Updated the desktop documentation header after comparing it with the supplied
React v6 reference. Top-level section links now target their section root so
`/components/*` routes select the Components item and show the active underline.
Language and direction controls use Global and Translation icons with accessible
labels and tooltips; theme and GitHub remain icon controls. Mobile-only overflow
items are hidden in the desktop navigation, and the navigation divider remains
visible.

The Docs project builds for `net10.0` with 0 errors. Browser DOM inspection on
`http://127.0.0.1:5026/en-US/components/slider?header-style-check=4` confirms the
Components item is selected, its href is `/en-US/Components`, the language,
direction, and theme icons render, and duplicate desktop overflow items are
hidden. The page is left open for manual review. Click behavior and cross-browser
visual parity remain unverified in this local runtime.

A fresh `phase1-progress.mjs` audit at `/tmp/phase1-progress-current-20261116.json`
reports: CSS IR 72/72, theme algorithms 3/3, Semantic DOM API/documented slots
423/423, component token docs 65/65 with 583/583 rows, global-token usage docs
64/64 with 1464/1464 rows, and zero token metadata mismatches. These are generated
catalog and documentation coverage checks, not runtime proof. Ten entries have
no component-specific token rows; the audit lists them explicitly and confirms
no missing React token metadata. The full dated component-by-component runtime
candidate matrix remains in `runtime-parity-summary-2026-11-12.md`; it still
requires focused acceptance for every component and demo.

## Theme Preview dark-background switch — 2026-11-16

Compared the home preview switch against React v6
`.dumi/pages/index/components/ThemePreview/index.tsx`. React applies
`tabsDark` and `tabsItemDark` when the selected theme has a dark background
image. Blazor previously rendered the same segmented switch styles for every
theme. It now conditionally applies the matching translucent switch surface,
light thumb, and high-contrast item/hover colors for themes whose generated
catalog marks `DarkBackground`.

The focused `HomeThemePreviewTests` suite passes (5/5 on `net10.0`, with
`DOTNET_ROLL_FORWARD=Major` for the installed .NET 11 runtime). The test now
checks that theme state changes the switch/item classes and that the dark
surface and item colors are present in generated CSS. The Docs.Server `net11.0`
build now succeeds and the server renders the Table route; Theme Preview's
visual comparison still needs a browser pass. Component and demo DOM/CSS parity remains incomplete; see
`runtime-parity-summary-2026-11-12.md` for the dated 71-component candidate
matrix.

## Table responsive breakpoints from theme tokens — 2026-11-16

React v6 Table filters `column.responsive` using `useBreakpoint`; the responsive
observer builds its media queries from the resolved theme's `screenXSMax`,
`screenSM`, `screenMD`, `screenLG`, `screenXL`, `screenXXL`, and `screenXXXL`
tokens. Blazor previously hard-coded equivalent numeric cutoffs, including a
2000px `xxxl` cutoff, so `ConfigProvider` breakpoint overrides did not affect
columns. `Table.IsResponsiveColumnHidden` now compares the viewport against the
active ConfigProvider's resolved global tokens. Its default `xxxl` threshold is
1920px, matching React.

The focused `TableResponsiveTests` test renders a custom `screenMD=900` theme:
the column is hidden at 899px and visible at 900px (2/2). `AntDesign.csproj`
builds on `net10.0`; `Docs.Server` builds on `net11.0` with 0 errors. The
Responsive columns demo now documents that default thresholds are customizable
through `ConfigProvider`. The Table docs page is open for manual review at
`http://127.0.0.1:5000/en-US/components/table?theme=light#components-table-demo-responsive`.

The broader `AntDesign.Tests.Table` filter is not fully green: 5/10 tests fail
because existing tests use strict bUnit JS interop without configuring the
Table's `bindTableRowHover` call. The focused responsive test passes. The full
71-component candidate matrix remains at
`runtime-parity-summary-2026-11-12.md`; this fix does not close the remaining
API, demo, token, docs, or Semantic DOM audit for Table or other components.

## Header menu responsive overflow follow-up — 2026-11-17

The responsive header must keep the React-style overflow behavior; it must not
disable or replace it with a fixed breakpoint-only menu. The desktop `Menu`
receives the full ordered list of navigation and utility entries, and its
available flex width is bounded so Menu's overflow observer moves trailing
items into the built-in unordered-list More submenu as space runs out.

At 820px in the running Docs.Server browser, the menu is 355px wide. Docs,
Components, and Charts remain visible; Experimental, AI, Github, 简体中文, and
RTL have `aria-hidden=true` and zero opacity in the inline row. Clicking the
built-in More item opens `rc-menu-uuid-nav-rc-menu-more-popup` with those five
entries. The page is left open at 820px with More expanded for manual review.
`git diff --check` passes. This verifies the responsive overflow behavior at
one narrow desktop width; the component-wide v6 audit and remaining runtime,
API, token, documentation, Theme Preview, and Semantic DOM work are still
open.

## Header navigation style follow-up — 2026-11-17

Aligned the desktop header columns and responsive menu with React v6. At
1512px, the logo column is 315px and the search control is 220px; top-level
Github/language/RTL entries are hidden while the utility controls remain at the
right. Below 1120px, the desktop utility row is hidden and the navigation gets a
bounded flex slot so the Menu's own overflow behavior can move trailing links
into its More submenu. Responsive CSS is emitted after base rules to prevent
the utilities and menu width from being overridden.

Browser inspection at 820px confirms the header stays within the viewport,
search remains 220px, the utility row is hidden, Docs/Components/Charts remain
inline, and opening More exposes Experimental, AI, Github, 简体中文, and RTL.
The Docs.Server `net11.0` build succeeds (0 errors; 591 existing warnings).
The focused responsive menu check is complete; broader component parity work
remains open.

The Table Dynamic Settings fixture was rerun against this build as part of the
same continuation. All three scenarios pass with zero normalized DOM, CSS,
geometry, or visual-style differences (2 groups / 3 scenarios); only raw HTML
serialization differs in attribute order and Razor renderer output. Report:
`/tmp/table-dynamic-settings-20261117.json`.

## Header utility controls — 2026-11-17

Synced the React header utility controls: the version is a searchable Select
with the React version list and 88px filled small control; its popup is hosted
beside the utility row. Sponsor avatars open hover/focus Popovers with the
localized SponsorCard content. Language switching uses the overlapping `中`
and `En` glyphs and localized Tooltip text. Direction switching uses the
React-matching target-direction Tooltip, `title="RTL"`/`title="LTR"`, and
`aria-label="RTL Switch Button"`. GitHub has a Tooltip and the external link
has `title`, `aria-label`, `target`, and `rel` attributes.

At 1512px, Blazor and React version controls both measure 88×24px and their
dropdowns both measure 88×264px with the same ten options. Browser checks
confirm the English language Tooltip, RTL Tooltip/title/ARIA, GitHub Tooltip
and link attributes, and sponsor card content with its image loaded. Both
Blazor locales have no browser console errors. Docs.Server builds on `net11.0`
with 0 errors (591 existing warnings). The full 71-component parity matrix and
remaining header dark/mobile edge cases remain open.

## Select Design Token contract — 2026-11-17

Compared React v6 `components/select/style/token.ts` against the C# generated
`SelectComponentToken`, its 24 `ComponentTokenDefinition` entries, and both
localized Select docs tables. The 24 public component token names match in all
four places; the docs include each React CSS variable, descriptions, and
defaults expressed in terms of their dependent global tokens. Browser review
at `/components/select?theme=light#select-design-token` confirms that React
and Blazor each render 24 rows, from `activeBorderColor` through `zIndexPopup`.
This closes the Select component-token surface audit, not its broader API,
demo, runtime, Semantic DOM, or Theme Preview audit.

## Table Editable Cells — 2026-11-17

Fixed the Form context in the editable Table demo, which previously threw when
an editable cell was clicked. The editor now validates required names, keeps
the input open when validation fails, saves valid edits on blur/Enter, and
supports adding rows. The Popconfirm trigger uses `Unbound` to avoid an extra
wrapper. Editable cell padding, hover border, and validation row height match
React.

The StyleGen fixture compares initial, edit, and subsequent edit states across
React and Blazor. All 3 scenarios pass with zero normalized DOM, CSS,
geometry, or visual-style differences. Framework-generated form `name`
attributes and equivalent inline-style serialization are normalized; computed
styles are still compared. Manual browser interaction also verified required
validation, save, and Add a row. Build: Docs and Docs.Server on `net11.0`, 0
errors. Report: `/tmp/table-edit-cell-dom-style-parity-20261117-verified.json`.
This closes only the Editable Cells demo; the remaining Table route is still
open.

## Table Expand Sticky — 2026-11-17

Added React's `expandedRowOffset: 3` and second-column expand icon ordering to
the Blazor demo. Expanded rows now extend the row spans of the expand icon and
all leading offset columns by one, matching React's inserted expanded row.
The Team demo keeps its source `rowSpan=2`; the table renderer extends it to 3
while expanded. The detail cell spans the same three remaining columns.

The focused fixture passes both initial and first-row-expanded scenarios with
zero normalized DOM, computed CSS, geometry, or visual-style differences.
Manual browser inspection confirms Team=`3`, expand icon=`2`, Name=`2`, and
detail `colSpan=3`. Raw HTML serialization differs only in framework whitespace
and renderer markers. Fixture: `tools/AntDesign.StyleGen/examples/table-expand-sticky-dom-style-parity.json`;
report: `/tmp/table-expand-sticky-dom-style-parity-20261117-final.json`.

## Table Drag Sorting — 2026-11-17

Aligned the sortable-row screen-reader instructions with React's dnd-kit text
and hidden-node behavior (`display: none`). Each row's `aria-describedby` points
to the local instructions node; its generated id differs from dnd-kit's
page-order-dependent `DndDescribedBy-*` value. Browser inspection confirmed
both references resolve to equivalent instructions.

`tools/AntDesign.StyleGen/examples/table-drag-sorting-dom-style-parity.json`
passes 1/1 with zero normalized DOM, computed CSS, geometry, and visual-style
differences. The fixture ignores `aria-describedby`'s opaque generated id; a
separate browser check verified the relationship and text. Interaction checks
confirmed keyboard Space → ArrowDown → Space moves John below Jim, and a pointer
drag applies row transforms before release and reorders the data to Jim, John,
Joe. Docs.Server `net11.0` builds with 0 errors (591 existing warnings). The
Table and App pages remain open in separate browser tabs.

This closes only the basic row-drag demo. Drag sorting with a handle, drag
column sorting, the remaining Table demos, and the full 71-component API/token/
docs/Semantic DOM/Theme Preview/runtime audit remain open. Next: Table drag
sorting with a handle.

## Table Drag Sorting with Handler — 2026-11-17

Moved the drag handle into the leading action column, matching React's column
order and using the first-cell icon button as the pointer target. The focused
fixture passes both Table row-sorting groups (2/2) with equal normalized DOM,
computed styles, and visual styles; no geometry differences were reported.
Manual pointer interaction on both React and Blazor commits the reordered
rows. With the same logical target (move John Brown to the second row), both
end in Jim Green, John Brown, Joe Black. Blazor's intermediate transform
preview was sampled directly; React's intermediate preview was not separately
captured in this check.

This closes only the handler demo. Drag Column Sorting and the remaining Table
demos plus the full 71-component audit remain open. Next: Table Drag Column
Sorting.

## Table current-build follow-up — 2026-11-17

The current `scroll-to-index-999` Virtual List fixture passes 1/1 across
normalized DOM, computed styles, and visual styles; report:
`/tmp/table-virtual-list-active-20261117.json`. This supersedes older temporary
reports that recorded the pre-fix virtual window.

The initial Style/Class virtual-table fixture had 17 normalized DOM
differences, all in inline table/column width values. The width synchronization
now sets the virtual table to the available container width and distributes
fluid columns using rc-table's floor/remainder behavior. The follow-up report
`/tmp/table-style-class-virtual-width-fixed-20261117.json` has equal normalized
DOM trees and selected computed styles. Three repeated final-column cell
measurements remain 0.016px narrower in Chromium (185.312px vs 185.328px); the
inline styles match exactly, and row/cell geometry samples match. This is an
open subpixel visual-style discrepancy; do not add a DOM/style hack to force a
fractional layout unit. Raw HTML differs only in Razor whitespace. Continue
with remaining Table demos and revisit only if a browser-supported layout
change can remove the subpixel difference without changing the React-matching
inline column values.

## Table Ajax — 2026-11-17

Aligned the Ajax demo with React's remote-data example using the same MockAPI
endpoint, Name/Gender/Email columns, and server-side page, filter, and sort
parameters. The initial request no longer sends the default `SortDirection.None`
as a descending order. The fixture covers initial loaded rows, sorting by Name,
and filtering to Male. All 3 scenarios pass with equal normalized DOM trees,
selected computed CSS, geometry, and visual styles; no differences or blocked
states remain. The filter interaction targets the menu item by its stable
`data-menu-id` suffix because React and Blazor render different checkbox wrapper
classes. Report: `/tmp/table-ajax-dom-style-parity-20261117-final.json`.
This closes only the Table Ajax demo; remaining Table demos and the full
71-component API/token/docs/Semantic DOM/Theme Preview/runtime audit remain
open.

## Table Hidden Columns — 2026-11-17

Rebuilt the `net11.0` Docs.Server and started it in Development mode so the
AntDesign static web assets load correctly. Updated the fixture to compare
against the local server at `http://127.0.0.1:5000`; the earlier 5011 attempt
was a stale Production host whose missing `ant-design-blazor.js` left demos in
Skeleton state and is not parity evidence.

The demo now renders the eight column toggles through `CheckboxGroup` and
applies its top margin to the Table wrapper. The fixture normalizes equivalent
CSS declaration whitespace and declares per-scenario semantic node counts, so
hiding one column expects seven header cells and fourteen body cells. Both
initial and column-hidden scenarios pass (2/2) with equal normalized DOM,
computed CSS, geometry, and visual styles; report:
`/tmp/table-hidden-columns-20261117-current-final.json`. Manual browser
interaction confirmed the counts transition `8 → 7 → 8` after hiding and
restoring Column 2. The Blazor page is open at
`http://127.0.0.1:5000/en-US/components/table#components-table-demo-hidden-columns`
for continued manual review. This closes only the Hidden Columns demo.
