---
category: Components
type: Feedback
title: Notification
cover: https://gw.alipayobjects.com/zos/alicdn/Jxm5nw61w/Notification.svg
---

Display a notification message globally.

## When To Use

To display a notification message at any of the four corners of the viewport. Typically it can be used in the following cases:

- A notification with complex content.
- A notification providing a feedback based on the user interaction. Or it may show some details about upcoming steps the user may have to follow.
- A notification that is pushed by the application.

## API

> Please confirm that the `<AntContainer />` component has been added to `App.Razor`.

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

The properties of config are as follows:

| Property | Description | Type | Default |
| ----------- | ------------------------------------------------------------ | ------------------------- | ----------------------------------- |
| Btn         | Customized close button                                           | RenderFragment            | null                                |
| ClassName   | Customized CSS class                                       | string                    | null                                |
| CloseIcon   | custom close icon                                        | RenderFragment            | null                                |
| Description | The content of notification box (required)                                      | string\|RenderFragment    | -                                   |
| Duration    | Time in seconds before Notification is closed. When set to 0 or null, it will never be closed automatically         | double?                   | 4.5                                 |
| Icon        | Customized icon                                              | RenderFragment            | null                                |
| Key         | The unique identifier of the Notification                                         | string                    | Guid.NewGuid().ToString()       |
| Message     | The title of notification box (required)                                  | string\|RenderFragment    | -                                   |
| OnClose     | Trigger when notification closed                                        | event Action                    | null                                |
| OnClick     | Specify a function that will be called when the notification is clicked                         |  event Action                    | null                                |
| Placement   | Position of Notification, can be one of `NotificationPlacement.TopLeft` `NotificationPlacement.TopRight` `NotificationPlacement.BottomLeft` `NotificationPlacement.BottomRight` | NotificationPlacement? | `NotificationPlacement.TopRight` |
| Style       | Customized inline style                                            | string                    | null                                |

`INotificationService` also provides a global `Config()` method that can be used for specifying the default options. Once this method is used, all the notification boxes will take into account these globally defined options when displaying.

`INotificationService.Config(config:NotificationGlobalConfig)`

| Property | Description | Type | Default |
| :-------- | :----------------------------------------------------------- | :------------------------- | :-------------------------------- |
| Bottom    | Distance from the bottom of the viewport, when `placement` is `NotificationPlacement.BottomLeft` or `NotificationPlacement.BottomRight` (unit: pixels).                 | double?                    | 24                                |
| Top       | Distance from the top of the viewport, when `placement` is `NotificationPlacement.TopLeft` or `NotificationPlacement.TopRight` (unit: pixels).                  | double?                    | 24                                |
| CloseIcon | custom close icon                                              | RenderFragment             | -                                 |
| Duration  |  Time in seconds before Notification is closed. When set to 0 or null, it will never be closed automatically                        | double?                    | 4.5                               |
| Placement | Position of Notification, can be one of  `NotificationPlacement.TopLeft` `NotificationPlacement.TopRight` `NotificationPlacement.BottomLeft` `NotificationPlacement.BottomRight` | NotificationPlacement? | NotificationPlacement.TopRight |
| Rtl       |  whether to enable RTL mode                                          | bool                       | `false`                           |

## NotificationRef

## Properties

| Name   | Description                          |
| ------ | ------------------------------------ |
| Config | get only 'Notificationconfig' object |

## Methods

| Name                                                         | Description                                                  |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| OpenAsync()                                                  | open the notification box                                    |
| UpdateConfigAsync()                                          | After modifying the `Config` property, update the notification box |
| UpdateConfigAsync(OneOf<string, RenderFragment>)             | update the notification box's `Description`                  |
| UpdateConfigAsync(OneOf<string, RenderFragment>, OneOf<string, RenderFragment>) | update the notification box's `Message` and `Description`    |
| CloseAsync()                                                 | close the notification box                                   |

