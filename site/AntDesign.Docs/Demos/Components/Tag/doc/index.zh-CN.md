---
category: Components
type: 数据展示
title: Tag
subtitle: 标签
cover: https://gw.alipayobjects.com/zos/alicdn/cH1BOLfxC/Tag.svg
---

进行标记和分类的小标签。

## 何时使用

- 用于标记事物的属性和维度。
- 进行分类。


## API

| 参数             | 说明                                         | 类型          | 默认值    | Version 
| ---------------- | -------------------------------------------- | ------------- | --------- | ----- 
| Checkable | 值标签是否可以选择 | boolean         |false   |
| Disabled | 是否禁用可选择标签 | boolean | false |
| Checked | 标签是否关闭所对应的值 | boolean         |false   |
| CheckedChange | 点击标签时触发的回调 | Action<bool>         |-       |
| ChildContent | Contents of the `Tag`| RenderFragment  |-       |
| Class | Any css class that will be added to tag. Use case: adding animation. | string   | -  | 0.9 
| ClassNames | 按 `root`、`icon`、`content` 和 `close` 语义节点设置自定义类名。 | IReadOnlyDictionary<string, string> | - |
| Styles | 按 `root`、`icon`、`content` 和 `close` 语义节点设置内联样式。 | IReadOnlyDictionary<string, string> | - |
| Closable | 值标签是否可以关闭| boolean         |false       |
| Color | 预设标签色 | string   | "default"         |
| Icon | 设置图标  | string        | -         |
| IconTheme |  设置图标风格 | string   | `fill` \| `outline` \| `twotone`       |
| OnClick | Callback executed when the `Tag` is clicked (excluding closing button) | Action | -         |
| OnClose | 关闭时的回调     | Action<MouseEventArgs>         | -         |
| OnClosing | Callback executed when the `Tag` is being closed. Closing can be canceled here.     | Action<CloseEventArgs<MouseEventArgs>>        | -         |
| Visible | 是否显示标签 | boolean         |true

`CheckableTagGroup` 支持通过 `Options` 设置选项；单选使用 `Value` / `ValueChanged`，多选设置 `Multiple` 并使用 `Values` / `ValuesChanged`。它的 `ClassNames` 与 `Styles` 字典支持 `root` 和 `item` 语义节点。

Tag 会将未声明的 HTML 属性和事件处理器透传到根元素，因此可以直接使用 `draggable` 等原生属性。
