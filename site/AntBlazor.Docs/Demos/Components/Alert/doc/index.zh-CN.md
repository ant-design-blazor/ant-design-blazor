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

时间轴

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| AfterClose | 关闭后调用 | function()         | -         |
| Banner   | 是否显示横幅 | bool         | false    |
| Closable | 是否可以关闭 | bool        | false       |
| CloseText | 关闭文本 | string         |
| Description | 附加说明内容 | string  | -  |
| Icon | 自定义图标, 当showIcon为true时显示 | RenderFragment  | -  |
| Message | 文本内容 | string  | -  |
| ShowIcon | 是否显示图标 | bool  | -  |
| Type | 控件样式类型, 选项: 成功, 消息, 警告, 错误 | string  | Warning  |
| OnClose | 关闭控件时回调 | function()  | -  |
| ChildContent | 附加内容 | RenderFragment  | -  |
