---
category: Components
type: 反馈
title: Drawer
subtitle: 抽屉
---

屏幕边缘滑出的浮层面板。

## 何时使用

抽屉从父窗体边缘滑入，覆盖住部分父窗体内容。用户在抽屉内操作时不必离开当前任务，操作完成后，可以平滑地回到到原任务。

- 当需要一个附加的面板来控制父窗体内容，这个面板在需要时呼出。比如，控制界面展示样式，往界面中添加内容。
- 当需要在当前任务流中插入临时任务，创建或预览附加内容。比如展示协议条款，创建子对象。

## API

### Drawer

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Title            | 标题         | string or slot | -         |
| BodyStyle |可用于设置 Drawer 内容部分的样式                               | object        | -         |
| Closable            |是否显示右上角的关闭按钮           | boolean | true         |
| ChildContent |抽屉元素之间的子组件                                | object        | -         |
| MaskClosable              |点击蒙层是否允许关闭         | boolean        | true        |
| MaskStyle | 遮罩样式                               | object        | -         |
| Placement | 抽屉的方向,可选值为 `left` , `top`,`right`,`bottom`        | string  | `right`         |
| WrapClassName | 对话框外层容器的类名                               | string        | -         |
| Width | 宽度                               | string\|int        | 256         |
| Height | 高度, 在 placement 为 top 或 bottom 时使用                               | |int        | 256        |
| ZIndex | 设置 Drawer 的 z-index                               | int        | -         |
| OffsetX | X轴方向的偏移量                                | int        | 0         |
| OffsetY | Y轴方向的偏移量                               | int        | 0         |
| Visible | Drawer 是否可见                               | boolean        | -         |
| Keyboard | 是否支持键盘 esc 关闭                               | boolean        | true         |
| OnClose        | 点击遮罩层或右上角叉或取消按钮的回调                                   | function(e)     | -         |
| OnViewInit             | 抽屉显示之前回调事件 | function(e)        | - |

### NzDrawerService

| 方法名 | 说明                  | 参数                       | 返回             |
| ------ | --------------------- | -------------------------- | ---------------- |
| CreateAsync | 创建并打开一个 Drawer | `DrawerConfig`  | `DrawerRef` |
| CreateAsync | 创建并打开一个 Drawer | `DrawerConfig` , TContentParams  | `DrawerRef<R>` |

### NzDrawerOptions

| 参数                | 说明                                                                                                                 | 类型                                                                | 默认值    |
| ------------------- | -------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------- | --------- |
| Content           | Drawer body 的内容                                                                                                   | `OneOf<RenderFragment, string>`       | -         |
| ContentParams     | 内容组件的输入参数 / Template的 context                                                                              | `D`                                                                 | -         |
| OnCancel          | 点击遮罩层或右上角叉时执行,将自动关闭对话框（返回false可阻止关闭） | `() => Promise<any>`                              `Func<bool?>`                                                        | -         |
| Closable          | 是否显示右上角的关闭按钮                                                                                             | `boolean`                                                           | `true`    |
| MaskClosable      | 点击蒙层是否允许关闭                                                                                                 | `boolean`                                                           | `true`    |
| Mask              | 是否展示遮罩                                                                                                         | `boolean`                                                           | `true`    |
| CloseOnNavigation | 导航历史变化时是否关闭抽屉组件                                                                                       | `boolean`                                                           | `true`    |
| Keyboard          | 是否支持键盘esc关闭                                                                                                  | `boolean`                                                           | `true`    |
| MaskStyle         | 遮罩样式                                                                                                             | `string`                                                            | `{}`      |
| BodyStyle         | Modal body 样式                                                                                                      | `string`                                                            | `{}`      |
| Title             | 标题                                                                                                                 | `OneOf<RenderFragment, string>`                                       | -         |
| Width             | 宽度                                                                                                                 | `int`                                                  | `256`     |
| Height            | 高度, 只在方向为 `'top'`或`'bottom'` 时生效                                                                          | `int`                                                  | `256`     |
| WrapClassName     | 对话框外层容器的类名                                                                                                 | `string`                                                            | -         |
| ZIndex            | 设置 Drawer 的 `z-index`                                                                                             | `int`                                                            | `1000`    |
| Placement         | 抽屉的方向                                                                                                           | `'top' \| 'right' \| 'bottom' \| 'left'`                            | `'right'` |
| OffsetX           | x 坐标移量(px)                                                                                                       | `int`                                                            | `0`       |
| OffsetY           | y 坐标移量(px), 高度, 只在方向为 `'top'`或`'bottom'` 时生效                                                          | `int`                                                            | `0`       |

### NzDrawerRef

#### 方法

| 名称  | 说明        | 类型                   |
| ----- | ----------- | ---------------------- |
| CloseAsync | 关闭 Drawer | `` |
| OpenAsync  | 打开 Drawer | ``           |