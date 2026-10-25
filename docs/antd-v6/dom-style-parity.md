# React/Blazor DOM and style parity runner

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
