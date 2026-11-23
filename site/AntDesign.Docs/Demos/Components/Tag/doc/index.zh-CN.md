---
category: Components
type: 数据展示
title: Tag
subtitle: 标签
cover: https://gw.alipayobjects.com/zos/alicdn/cH1BOLfxC/Tag.svg
---

进行标记和分类的小标签。

## 何时使用

- 按属性或维度标记事物。
- 对相关项目分组或分类。

## API

### Tag

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Bordered | 已弃用的兼容参数，请使用 `Variant`。 | bool? | - |
| Checkable | Blazor 提供的可选标签快捷方式。React 对应 `Tag.CheckableTag`。 | bool | false |
| Checked | 可选标签是否选中。 | bool | false |
| CheckedChanged | 可选状态变化时触发。 | EventCallback<bool> | - |
| ChildContent | 标签内容。 | RenderFragment | - |
| Class | 根节点 CSS 类名。 | string | - |
| ClassNames | `root`、`icon`、`content`、`close` 语义节点的类名。 | IReadOnlyDictionary<string, string> | - |
| CloseIcon | 自定义关闭图标内容；未设置 `Closable` 时，提供此参数会启用关闭控件。 | RenderFragment | - |
| Closable | 是否显示关闭控件。显式设置为 `false` 时，即使设置了 `CloseIcon` 也不会显示。 | bool? | false |
| Color | 预设色、状态色或 CSS 自定义颜色。 | OneOf<TagColor, string> | TagColor.Default |
| Disabled | 禁用标签交互并应用禁用样式。 | bool | false |
| Href | 设置后根节点渲染为链接。 | string | - |
| Icon | 在内容前显示的图标名称。 | string | - |
| IconTemplate | 在内容前渲染的自定义图标。 | RenderFragment | - |
| IconTheme | `Icon` 的图标主题。 | IconThemeType | Outline |
| OnClick | 点击标签时触发。 | EventCallback | - |
| OnKeyDown | 根节点按键时触发。 | EventCallback<KeyboardEventArgs> | - |
| OnClose | 标签隐藏前触发。 | EventCallback<MouseEventArgs> | - |
| OnClosing | Blazor 扩展回调，可通过 `CloseEventArgs.Cancel` 取消关闭。 | EventCallback<CloseEventArgs<MouseEventArgs>> | - |
| RootClassName | 根节点附加类名，对应 React `rootClassName`。 | string | - |
| Styles | `root`、`icon`、`content`、`close` 语义节点的内联样式。 | IReadOnlyDictionary<string, string> | - |
| Style | 根节点内联样式。 | string | - |
| Target | 设置 `Href` 后使用的浏览上下文。 | string | - |
| Variant | 外观：`Filled`、`Solid` 或 `Outlined`。 | TagVariant? | Filled |
| Visible | 初始可见状态；关闭时保留根节点并添加 `ant-tag-hidden`。Blazor 专有。 | bool | true |

`Disabled`、`Href`、`Target`、`CloseIcon`、`Bordered` 和 `RootClassName` 对应 React Tag 行为。`OnClosing` 和 `Visible` 是 Blazor 扩展。

React v6 还提供 `Disabled` 和 `Customize close` 示例；Blazor 中也补齐了对应示例。

### CheckableTagGroup

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Options | 选项值或包含 `Value`、`Label` 及可选项 `Class` / `Style` 的对象。 | IReadOnlyList<CheckableTagOption> | - |
| Multiple | 启用多选模式。 | bool | false |
| DefaultValue | 非受控单选或多选模式的初始选中值。 | OneOf<string, IReadOnlyList<string>> | - |
| Value | 单选模式下的选中值。 | string | - |
| ValueChanged | 单选值变化回调。 | EventCallback<string> | - |
| Values | 多选模式下的选中值列表。 | IReadOnlyList<string> | - |
| ValuesChanged | 多选值变化回调。 | EventCallback<IReadOnlyList<string>> | - |
| Disabled | 禁用全部选项。 | bool | false |
| RootClassName | 根节点附加类名。 | string | - |
| ClassNames | `root` 和 `item` 语义节点的类名。 | IReadOnlyDictionary<string, string> | - |
| Styles | `root` 和 `item` 语义节点的样式。 | IReadOnlyDictionary<string, string> | - |

`CheckableTagGroup` 是 Blazor 便捷组件。React v6 提供受控的 `CheckableTag`。此分组支持受控的 `Value` / `Values`，也支持非受控的 `DefaultValue`；仅支持字符串选项值是 Blazor 的扩展模型。
