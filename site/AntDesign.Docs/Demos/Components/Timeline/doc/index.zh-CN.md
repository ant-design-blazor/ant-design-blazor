---
category: Components
type: 数据展示
title: Timeline
subtitle: 时间轴
cover: https://gw.alipayobjects.com/zos/antfincdn/vJmo00mmgR/Timeline.svg
---

垂直展示的时间流信息。

## 何时使用

- 当有一系列信息需按时间排列时，可正序和倒序。
- 需要有一条时间轴进行视觉上的串联时。

## API

### Timeline

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Items | 时间轴节点数据。可与 `TimelineItem` 子组件二选一。 | `IReadOnlyList<TimelineItemData>` | - |
| Mode | 节点显示在起始侧、结束侧或两侧交替。`Left` 对应 React `start`，`Right` 对应 React `end`。 | `TimelineMode?`（`Left`、`Right`、`Alternate`） | `Left` |
| Orientation | 时间轴方向。 | `TimelineOrientation`（`Vertical`、`Horizontal`） | `Vertical` |
| Reverse | 反转节点顺序。 | `bool` | `false` |
| Variant | 节点的视觉样式。 | `TimelineVariant`（`Outlined`、`Filled`） | `Outlined` |
| TitleSpan | 标题区域的间距，支持数字、CSS 长度或百分比。 | `OneOf<double, string>` | `12` |
| Pending | 旧版幽灵节点内容。新代码建议用节点的 `Loading` 和 `Content`。 | `OneOf<string, RenderFragment>` | - |
| PendingDot | 旧版 `Pending` 节点的时间点。新代码建议用节点的 `Icon`。 | `RenderFragment` | Loading 图标 |
| ClassNames | 按语义槽位设置类名；Blazor 接收槽位字典。 | `IReadOnlyDictionary<string, string>` | - |
| Styles | 按语义槽位设置内联样式；Blazor 接收槽位字典。 | `IReadOnlyDictionary<string, string>` | - |
| ChildContent | `TimelineItem` 子组件。 | `RenderFragment` | - |

React v6 使用 `items`，通过 `content`、`loading`、`icon`、`title` 和 `placement` 配置节点。Blazor 对应属性位于下方的 `TimelineItemData`。`Pending` 和 `PendingDot` 为兼容旧版 API 保留。

### TimelineItemData

| 属性 | 说明 | 类型 |
| --- | --- | --- |
| Content | 节点文本内容。 | `string` |
| ContentContent | 富文本节点内容，优先于 `Content`。 | `RenderFragment` |
| Title | 纯文本标题。 | `string` |
| TitleContent | 富文本标题，优先于 `Title`。 | `RenderFragment` |
| Loading | 为该节点显示加载图标。 | `bool` |
| Icon | 自定义节点图标。 | `RenderFragment` |
| Color | 预设或自定义节点颜色。 | `OneOf<TimelineDotColor, string>` |
| Placement | 覆盖该节点在当前模式中的位置。 | `TimelineMode?` |
| Class / Style | 节点附加类名和内联样式。 | `string` |

### TimelineItem

| 属性 | 说明 | 类型 |
| --- | --- | --- |
| ChildContent | 节点内容。 | `RenderFragment` |
| Label | 纯文本标题。 | `string` |
| Dot | 自定义节点时间点；使用 `Items` 时可改用 `TimelineItemData.Icon`。 | `RenderFragment` |
| Color | 预设或自定义节点颜色。 | `OneOf<TimelineDotColor, string>` |
| Placement | 覆盖该节点在当前模式中的位置。 | `TimelineMode?` |
