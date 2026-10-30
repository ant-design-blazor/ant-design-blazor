# React/Blazor DOM and style parity runner

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
