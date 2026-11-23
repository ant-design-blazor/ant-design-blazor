---
category: Components
type: Data Display
title: Timeline
cover: https://gw.alipayobjects.com/zos/antfincdn/vJmo00mmgR/Timeline.svg
---

Vertical display timeline.

## When To Use

- When a series of information needs to be ordered by time (ascending or descending).
- When you need a timeline to make a visual connection.

## API

### Timeline

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Items | Data-driven timeline nodes. Use this or `TimelineItem` children. | `IReadOnlyList<TimelineItemData>` | - |
| Mode | Places nodes at the start, end, or alternately on both sides. `Left` maps to React `start`; `Right` maps to React `end`. | `TimelineMode?` (`Left`, `Right`, `Alternate`) | `Left` |
| Orientation | Timeline direction. | `TimelineOrientation` (`Vertical`, `Horizontal`) | `Vertical` |
| Reverse | Reverses the node order. | `bool` | `false` |
| Variant | Visual style of the nodes. | `TimelineVariant` (`Outlined`, `Filled`) | `Outlined` |
| TitleSpan | Spacing reserved for item titles; accepts a number, CSS length, or percentage. | `OneOf<double, string>` | `12` |
| Pending | Legacy ghost node content. Prefer an item with `Loading` and `Content`. | `OneOf<string, RenderFragment>` | - |
| PendingDot | Legacy dot for the `Pending` node. Prefer an item's `Icon`. | `RenderFragment` | Loading icon |
| ClassNames | Classes keyed by semantic slot. Blazor accepts a slot dictionary. | `IReadOnlyDictionary<string, string>` | - |
| Styles | Inline styles keyed by semantic slot. Blazor accepts a slot dictionary. | `IReadOnlyDictionary<string, string>` | - |
| ChildContent | `TimelineItem` child components. | `RenderFragment` | - |

React v6 uses `items` with `content`, `loading`, `icon`, `title`, and `placement`. Blazor exposes these through `TimelineItemData` below. `Pending` and `PendingDot` remain for compatibility with the earlier API.

### TimelineItemData

| Property | Description | Type |
| --- | --- | --- |
| Content | Plain text node content. | `string` |
| ContentContent | Rich node content; takes precedence over `Content`. | `RenderFragment` |
| Title | Plain text title. | `string` |
| TitleContent | Rich title; takes precedence over `Title`. | `RenderFragment` |
| Loading | Shows the loading icon for this node. | `bool` |
| Icon | Custom node icon. | `RenderFragment` |
| Color | Named or custom node color. | `OneOf<TimelineDotColor, string>` |
| Placement | Overrides the node's placement within the timeline mode. | `TimelineMode?` |
| Class / Style | Additional class and inline style for the node. | `string` |

### TimelineItem

| Property | Description | Type |
| --- | --- | --- |
| ChildContent | Node content. | `RenderFragment` |
| Label | Plain text title. | `string` |
| Dot | Custom node dot; use `TimelineItemData.Icon` with `Items`. | `RenderFragment` |
| Color | Named or custom node color. | `OneOf<TimelineDotColor, string>` |
| Placement | Overrides placement within the timeline mode. | `TimelineMode?` |
