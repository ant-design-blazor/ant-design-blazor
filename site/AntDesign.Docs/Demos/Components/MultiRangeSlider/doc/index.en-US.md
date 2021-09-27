---
category: Components
type: Data Entry
title: MultiRangeSlider
cover: https://gw.alipayobjects.com/zos/alicdn/HZ3meFc6W/Silder.svg
---

A component allowing control over multiple ranges. Each range has a starting and ending value. All ranges are placed on a single rail.

## When To Use

- To control multiple ranges of range values.



## API

### MultiRangeGroup

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ChildContent | Content wrapped by `MultiRangeGroup`. | RenderFragment         | - | 0.11
| Height | Sets hight of the container. Used only when `Vertical="true"`. Examples: `100px`, `45%`, `21vh` | string | - | 0.11
| Marks | Tick mark of the `MultiRangeSlider`, type of key must be number, and must in closed interval [min, max], each mark can declare its own style. | RangeItemMark[] | - |0.11
| Reverse | Render the sliders with scale starting form left side to right or from bottom towards top. | bool | false | 0.11
| Vertical | If `true`, the slider will be vertical. | bool | false | 0.11

### MultiRangeSlider

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AllowOverlapping | Use AllowOverlapping to toggle overlapping mode. | bool | false | 0.11
| ChildContent | Used for rendering range items manually. | RenderFragment | - | 0.11
| Data | Collection of data objects implementing `AntDesign.IRangeItemData` that will be used to render the ranges. | IEnumerable<IRangeItemData>  | - | 0.11
| DataChanged | Gets or sets a callback that updates the bound value. Used by binding, will be automatically utilized by Blazor. | EventCallback<IEnumerable<IRangeItemData>> | - | 0.11
| Disabled | If `true`, the slider will not be intractable | bool | false | 0.11
| EqualIsOverlap | Useful only when `AllowOverlapping` is set to false. Does not allow edges to meet, because treats equal edge values as overlapping. | bool | false | 0.11
| ExpandStep | Whether to expand visually each step. | bool | false | 0.11
| HasTooltip | Will not render `Tooltip` if set to `false`. | bool | true | 0.11
| Marks | Tick mark of the `MultiRangeSlider`, type of key must be number, and must in closed interval [min, max], each mark can declare its own style. | RangeItemMark[] | - | 0.11
| Max | The maximum value the range slider | double | 0 | 0.11
| Min | The minimum value the range slider | double | 100 | 0.11
| OnAfterChange | Called when changes are done (`onmouseup` and `onkeyup`). | EventCallback<(double, double)> | - | 0.11
| OnEdgeAttaching | Called before edge is attached. If returns 'false', attaching is stopped. | Func<(RangeItem, RangeItem), (bool allowAttaching, bool detachExistingOnCancel)> | - | 0.11
| OnEdgeAttached | Called after edge is attached. | EventCallback<(RangeItem, RangeItem)> | - | 0.11
| OnEdgeDetaching | Called before edge is detached. If returns 'false', detaching is stopped. | Func<(RangeItem, RangeItem), bool> | - | 0.11
| OnEdgeDetached | Called after edge is detached. | EventCallback<(RangeItem, RangeItem)> | - | 0.11
| OnEdgeMoving | Called before edge is moved. If returns `false`, moving is canceled. | Func<(RangeItem, RangeEdge, double), bool> | - | 0.11
| OnEdgeMoved | Called after edge is moved. | EventCallback<(RangeItem, RangeEdge, double)> | - | 0.11
| OnChange | Called when the user changes one of the values. | EventCallback<(double, double)> | - | 0.11
| OverflowStyle | Used to style overflowing container. Avoid using unless in oversized mode (when `VisibleMin` > `Min` and/or `VisibleMax` < 'Max' ). | string | - | 0.11
| Reverse | Render the slider with scale starting form left side to right or from bottom towards top. | bool | false | 0.11
| Step | The granularity the slider can step through values. Must be greater than 0, and be divided by (max - min) . When marks no null, step can be null. | double | 1 | 0.11
| TipFormatter |Slider will pass its value to `TipFormatter`, and display its value in `Tooltip`. | Func<double, string> | (d) => d.ToString() | 0.11
| TooltipPlacement | Set `Tooltip` display position. Ref [`Tooltip`](/components/tooltip). | PlacementType | `PlacementType.Top` for horizontal slider & `PlacementType.Right` for vertical slider| 0.11
| TooltipVisible | If `true`, `Tooltip` will show always, or it will not show anyway, even if dragging or hovering. | boolean        | false        | 0.11
| Value | Get or set the values. | IEnumerable<(double, double)> | - | 0.11
| Vertical | If `true`, the slider will be vertical. | bool | false | 0.11
| VisibleMin | Used in connection with `VisibleMax`. If grater than `Min`, the slider is rendered with an overflow (oversized/zoomed). If lesser than `Min`, will be set to `Min`. | double | Min | 0.11
| VisibleMax | Used in connection with `VisibleMin`. If lesser than `Max`, the slider is rendered with an overflow (oversized/zoomed). If grater than `Max`, will be set to `Max`. | double | Max | 0.11

### MultiRangeSlider

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Color | Color of the range item.  | OneOf<AntDesign.Color, string> | - | 0.11
| Data | Data object implementing `AntDesign.IRangeItemData` that will be used to render the ranges. | IRangeItemData  | - | 0.11
| DefaultValue | The default value of slider.  | (double, double) | - | 0.11
| Description | Text visible on the range.  | string | - | 0.11
| Disabled | If `true`, the slider will not be intractable | bool | false | 0.11
| FocusBorderColor | Color of the range item's border when focused.  | OneOf<AntDesign.Color, string> | - | 0.11
| FocusColor | Color of the range item when focused.  | OneOf<AntDesign.Color, string> | - | 0.11
| FontColor | Color of the text visible on the range.  | OneOf<AntDesign.Color, string> | - | 0.11
| Color | Color of the text visible on the range.  | OneOf<AntDesign.Color, string> | - | 0.11
| Icon | Icon visible on the range.  | string | - | 0.11
| HasTooltip | Will not render `Tooltip` if set to `false`. | bool | true | 0.11
| OnAfterChange | Called when changes are done (`onmouseup` and `onkeyup`). | EventCallback<(double, double)> | - | 0.11
| OnChange | Called when the user changes one of the values. | EventCallback<(double, double)> | - | 0.11
| TooltipPlacement | Set `Tooltip` display position. Ref [`Tooltip`](/components/tooltip). | PlacementType | `PlacementType.Top` for horizontal slider & `PlacementType.Right` for vertical slider| 0.11
| TooltipVisible | If `true`, `Tooltip` will show always, or it will not show anyway, even if dragging or hovering. | boolean        | false        | 0.11

## Common Methods

| Property | Description | Parameters | Version |
| --- | --- | --- | --- |
| AttachEdges() | Will attach 2 edges. If `MultiRangeSlider.AllowOverlapping` is set to `false`, will only allow attaching neighboring edges. | RangeEdge currentRangeEdge, RangeItem attachToRange, RangeEdge attachToRangeEdge, bool detachExisting = false | 0.11
| AttachOverlappingEdges() | Will attach overlapping edges. Same as double clicking on overlapping edges. | RangeEdge currentRangeEdge, bool detachExisting = false | 0.11
| AttachSingle() | Will initiate attaching. Same as double clicking on an edge (that is not overlapping with another edge). | RangeEdge currentRangeEdge, bool detachExisting = false | 0.11
| DetachEdges() | Detaches edge(s). | - | 0.11