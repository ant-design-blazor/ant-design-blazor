---
category: Components
type: 反馈
title: Drawer
subtitle: 抽屉
cover: https://gw.alipayobjects.com/zos/alicdn/7z8NJQhFb/Drawer.svg
---

屏幕边缘滑出的浮层面板。

## 何时使用

抽屉从父窗体边缘滑入，覆盖住部分父窗体内容。用户在抽屉内操作时不必离开当前任务，操作完成后，可以平滑地回到到原任务。

- 当需要一个附加的面板来控制父窗体内容，这个面板在需要时呼出。比如，控制界面展示样式，往界面中添加内容。
- 当需要在当前任务流中插入临时任务，创建或预览附加内容。比如展示协议条款，创建子对象。

## API

### Drawer

| 参数          | 说明                                                    | 类型           | 默认值  |
| ------------- | ------------------------------------------------------- | -------------- | ------- |
| Title | 抽屉的标题 | OneOf<RenderFragment, 字符串> | - |
| Closable | 在抽屉对话框的右上角是否可见关闭 (x) 按钮。| 布尔值 | 真实 |
| ChildContent | 子组件 | 渲染片段 | - |
| Content | 抽屉的内容。当与 ChildContent 一起给出时，这显示在 ChildContent 上方。| OneOf<RenderFragment, 字符串> | - |
| MaskClosable | 单击遮罩（抽屉外区域）将关闭或关闭抽屉。| 布尔值 | 真实 |
| Mask | 是否显示面具。当为真时，抽屉周围的区域在可见时会变暗。| 布尔值 | 真实 |
| Placement | 抽屉的位置，选项可以是`left`，`top`，`right`，`bottom` | 字符串 | `正确` |
| Width | 抽屉的宽度。仅在放置为“左”或“右”时使用。| 整数 | 256 |
| Height | 抽屉高度。仅在 Placement 为“top”或“bottom”时使用 | 整数 | 256 |
| ZIndex | 抽屉的 z-index。| 整数 | 1000 |
| OffsetX | X 坐标偏移量(px)。仅在放置为“左”或“右”时使用。| 整数 | 0 |
| OffsetY | Y 坐标偏移量(px)。仅在放置为“顶部”或“底部”时使用。| 整数 | 0 |
| Visible | 抽屉对话框是否可见。| 布尔值 | - |
| Keyboard | 是否支持按esc关闭。**目前未实施** | 布尔值 | 真实 |
| Handler | 呈现为抽屉内容的兄弟的内容 | 渲染片段 | - |
| OnClose | 指定当用户单击掩码、关闭按钮或取消按钮时将调用的回调。| 事件回调 | - |
| OnOpen | 指定将在抽屉呈现后调用的回调 | 函数<任务> | - |
| MaskStyle | 抽屉遮罩元素的样式。| 字符串 | - |
| HeaderStyle   | 可用于设置 Drawer 标题部分的样式                          | string         | -        |
| BodyStyle | 抽屉内容部分的样式| 字符串 | - |
| WrapClassName | Drawer 对话框的容器的类名。| 字符串 | - |

### DrawerService

| 方法名      | 说明                  | 参数                            | 返回           |
| ----------- | --------------------- | ------------------------------- | -------------- |
| CreateAsync | 创建并打开一个 Drawer | `DrawerConfig`                  | `DrawerRef`    |
| CreateAsync | 创建并打开一个 Drawer | `DrawerConfig` , TContentParams | `DrawerRef<R>` |

### DrawerOptions

| 参数              | 说明                                                    | 类型                                     | 默认值    |
| ----------------- | ------------------------------------------------------- | ---------------------------------------- | --------- |
| Content           | Drawer body 的内容                                      | `OneOf<RenderFragment, string>`          | -         |
| ContentParams     | 内容组件的输入参数 / Template 的 context                | `D`                                      | -         |
| Closable          | 是否显示右上角的关闭按钮                                | `boolean`                                | `true`    |
| MaskClosable      | 点击蒙层是否允许关闭                                    | `boolean`                                | `true`    |
| Mask              | 是否展示遮罩                                            | `boolean`                                | `true`    |
| CloseOnNavigation | 导航历史变化时是否关闭抽屉组件                          | `boolean`                                | `true`    |
| Keyboard          | 是否支持键盘 esc 关闭                                   | `boolean`                                | `true`    |
| MaskStyle         | 遮罩样式                                                | `string`                                 | `{}`      |
| BodyStyle         | Drawer body 样式                                         | `string`                                 | `{}`      |
| HeaderStyle       | 可用于设置 Drawer 标题部分的样式                          | `string`         | -        |
| Title             | 标题                                                    | `OneOf<RenderFragment, string>`          | -         |
| Width             | 宽度                                                    | `int`                                    | `256`     |
| Height            | 高度, 只在方向为 `'top'`或`'bottom'` 时生效             | `int`                                    | `256`     |
| WrapClassName     | 对话框外层容器的类名                                    | `string`                                 | -         |
| ZIndex            | 设置 Drawer 的 `z-index`                                | `int`                                    | `1000`    |
| Placement         | 抽屉的方向                                              | `'top' \| 'right' \| 'bottom' \| 'left'` | `'right'` |
| OffsetX           | X 轴方向的偏移量，只在方向为 `'left'`或`'right'` 时生效 | `int`                                    | `0`       |
| OffsetY           | Y 轴方向的偏移量，只在方向为 `'top'`或`'bottom'` 时生效 | `int`                                    | `0`       |

### DrawerRef

| 名称       | 说明        | 类型 |
| ---------- | ----------- | ---- |
| CloseAsync | 关闭 Drawer |      |
| OpenAsync  | 打开 Drawer |      |
