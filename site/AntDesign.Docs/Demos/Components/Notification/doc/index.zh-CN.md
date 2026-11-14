---
category: Components
type: 反馈
subtitle: 通知提醒框
title: Notification
cover: https://gw.alipayobjects.com/zos/alicdn/Jxm5nw61w/Notification.svg
---

全局展示通知提醒信息。

## 何时使用

在系统四个角显示通知提醒信息。经常用于以下情况：

- 较为复杂的通知内容。
- 带有交互的通知，给出用户下一步的行动点。
- 系统主动推送。

## API

> 请确认已经在 `App.Razor` 中添加了 `<AntContainer />` 组件。

- `INotificationService.Open(config:NotificationConfig)`
- `INotificationService.Info(config:NotificationConfig)`
- `INotificationService.Success(config:NotificationConfig)`
- `INotificationService.Warning(config:NotificationConfig)`
- `INotificationService.Warn(config:NotificationConfig)`
- `INotificationService.Error(config:NotificationConfig)`
- `INotificationService.Close(key:string)`
- `INotificationService.Destroy()`
- `INotificationService.CreateAsync()`
- `INotificationService.UpdateAsync(key:string, description:OneOf<string, RenderFragment>, message:OneOf<string, RenderFragment>? = null)`

config 参数如下：

| 参数        | 说明                                                         | 类型                      | 默认值                              |
| ----------- | ------------------------------------------------------------ | ------------------------- | ----------------------------------- |
| Btn         | 自定义关闭按钮                                               | RenderFragment            | null                                |
| ClassName   | 自定义 CSS class                                             | string                    | null                                |
| CloseIcon   | 自定义关闭图标                                               | RenderFragment            | null                                |
| Description | 通知提醒内容，必选                                           | string\|RenderFragment    | -                                   |
| Duration    | 默认 4.5 秒后自动关闭，配置为 null 或者 0 则不自动关闭         | double?                   | 4.5                                 |
| Icon        | 自定义图标                                                   | RenderFragment            | null                                |
| Key         | 当前通知唯一标志                                             | string                    | Guid.NewGuid().ToString()       |
| Message     | 通知提醒标题，必选                                           | string\|RenderFragment    | -                                   |
| OnClose     | 当通知关闭时触发的事件                                      |  event Action             | null                                |
| OnClick     | 点击通知时触发的事件                                |  event Action              | null                                |
| Placement   | 弹出位置，可选 `NotificationPlacement.TopLeft` `NotificationPlacement.TopRight` `NotificationPlacement.BottomLeft` `NotificationPlacement.BottomRight` | NotificationPlacement? | `NotificationPlacement.TopRight` |
| Style       | 自定义内联样式                                               | string                    | null                                |

还提供了一个全局配置方法，在调用前提前配置，全局一次生效。

`INotificationService.Config(config:NotificationGlobalConfig)`

| 参数      | 说明                                                         | 类型                       | 默认值                            |
| :-------- | :----------------------------------------------------------- | :------------------------- | :-------------------------------- |
| Bottom    | 消息从底部弹出时，距离底部的位置，单位像素。                 | double?                    | 24                                |
| Top       | 消息从顶部弹出时，距离顶部的位置，单位像素。                 | double?                    | 24                                |
| CloseIcon | 自定义关闭图标                                               | RenderFragment             | -                                 |
| Duration  | 默认自动关闭延时，单位秒。                                   | double?                    | 4.5                               |
| Placement | 弹出位置，可选 `NotificationPlacement.TopLeft` `NotificationPlacement.TopRight` `NotificationPlacement.BottomLeft` `NotificationPlacement.BottomRight` | NotificationPlacement? | NotificationPlacement.TopRight |
| Rtl       | 是否开启 RTL 模式                                            | bool                       | `false`                           |

## NotificationRef

### 属性

| 名称   | 说明                           |
| ------ | ------------------------------ |
| Config | 只读 `NotificationConfig` 对象 |

### 方法

| 名称                                                         | 说明                                 |
| ------------------------------------------------------------ | ------------------------------------ |
| OpenAsync()                                                  | 打开一个通知框                       |
| UpdateConfigAsync()                                          | 修改Config属性后，更新通知框         |
| UpdateConfigAsync(OneOf<string, RenderFragment>)             | 更新通知框的`Description`            |
| UpdateConfigAsync(OneOf<string, RenderFragment>, OneOf<string, RenderFragment>) | 更新通知框的`Message`和`Description` |
| CloseAsync()                                                 | 关闭通知框                           |

