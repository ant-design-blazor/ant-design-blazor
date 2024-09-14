---
category: Components
subtitle: 全局提示
type: 反馈
title: Message
cover: https://gw.alipayobjects.com/zos/alicdn/hAkKTIW0K/Message.svg
---

全局展示操作反馈信息。

## 何时使用

- 可提供成功、警告和错误等反馈信息。
- 顶部居中显示并自动消失，是一种不打断用户操作的轻量级提示方式。

## API

> 请确认已经在 `App.Razor` 中添加了 `<AntContainer />` 组件。

该组件提供了一些方法，用法和参数如下：

- `IMessageService.Success(content, [duration], onClose)`
- `IMessageService.Error(content, [duration], onClose)`
- `IMessageService.Info(content, [duration], onClose)`
- `IMessageService.Warning(content, [duration], onClose)`
- `IMessageService.Warn(content, [duration], onClose)` // alias of warning
- `IMessageService.Loading(content, [duration], onClose)`

| 参数     | 说明                                          | 类型                      | 默认值 |
| -------- | --------------------------------------------- | ------------------------- | ------ |
| content  | 提示内容                                      | string\|RenderFragment\|MessageConfig | -      |
| duration | 自动关闭的延时，单位秒。设为 0 或者 null 时不自动关闭。 | double?                    | 3      |
| onClose  | 关闭时触发的回调函数                          | Action                  | -      |

组件同时提供 ContinueWith 接口。

- `IMessageService.[level](content, [duration]).ContinueWith(afterClose)`
- `IMessageService.[level](content, [duration], onClose).ContinueWith(afterClose)`

其中 `[level]` 是组件已经提供的方法。 `ContinueWith` 接口的返回值为Task。

也可以对象的形式传递参数：

- `IMessageService.Open(config:MessageConfig)`
- `IMessageService.Success(config:MessageConfig)`
- `IMessageService.Error(config:MessageConfig)`
- `IMessageService.Info(config:MessageConfig)`
- `IMessageService.Warning(config:MessageConfig)`
- `IMessageService.Warn(config:MessageConfig)` // alias of warning
- `IMessageService.Loading(config:MessageConfig)`

`config` 对象属性如下：

| 参数     | 说明                                          | 类型           | 默认值 |
| -------- | --------------------------------------------- | -------------- | ------ |
| Content  | 提示内容                                      | string\|RenderFragment      | -      |
| Duration | 自动关闭的延时，单位秒。设为 0 或者 null 时不自动关闭。 | double?         | 3      |
| OnClose  | 关闭时触发的回调函数                          | event Action       | -      |
| Icon     | 自定义图标                                    | RenderFragment      | -      |
| Key      | 当前提示的唯一标志                            | string | -      |

### 全局方法

还提供了全局配置和全局销毁方法：

- `IMessageService.Config(options:MessageGlobalConfig)`
- `IMessageService.Destroy()`

#### IMessageService.Config

```c#
@inject IMessageService MessageService;

MessageService.Config(new MessageGlobalConfig{
  Top: 100,
  Duration: 2,
  MaxCount: 3,
  Rtl: true,
});
```

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Duration | 默认自动关闭延时，单位秒 | double | 3 |
| MaxCount | 最大显示数, 超过限制时，最早的消息会被自动关闭 | int | 0 |
| Top | 消息距离顶部的位置 | double | 24 |
| Rtl | 是否开启 RTL 模式 | bool | `false` |