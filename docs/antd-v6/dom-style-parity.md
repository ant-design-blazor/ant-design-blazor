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
exposed the actual mismatch; the final Transfer run reports exact equality for
the selected root and Semantic DOM computed styles, while the HTML/tree gate
continues to report framework markup differences separately.

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
