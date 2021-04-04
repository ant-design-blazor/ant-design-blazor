---
category: Components
subtitle: 警告提示
type: 反馈
title: Alert
cover: https://gw.alipayobjects.com/zos/alicdn/8emPa3fjl/Alert.svg
---

警告提示，展现需要关注的信息。

## 何时使用

- 当某个页面需要向用户显示警告的信息时。
- 非浮层的静态展现形式，始终展现，不会自动消失，用户可以点击关闭。

## API

### Alert Props

| 参数             | 说明  | 类型          | 默认值    |
| --- | --- | --- | --- | --- |
| AfterClose | 	关闭动画结束后触发的回调函数 | EventCallback&lt;MouseEventArgs> | - |  |
| Banner | 是否用作顶部公告 | Banner | false |  |
| Closable | 默认不显示关闭按钮 | bool | false |  |
| CloseText | 自定义关闭按钮 | string | - |  |
| Description | A自定义关闭按钮 | string | - |  |
| Icon | 自定义图标，`ShowIcon` 为 `true` 时有效 | RenderFragment | - |  |
| Message | 警告提示内容 | string | - |  |
| MessageTemplate | Message 的模板 | RenderFragment | - | |
| ShowIcon | 是否显示辅助图标 | bool | false |  |
| Type | 指定警告提示的样式，有四种选择 `success` \| `info` \| `warning` \| `error` | string | warning |  |
| OnClose | 关闭时触发的回调函数 | EventCallback&lt;MouseEventArgs> | - |  |
| ChildContent | 附加内容 | RenderFragment | - |  |
