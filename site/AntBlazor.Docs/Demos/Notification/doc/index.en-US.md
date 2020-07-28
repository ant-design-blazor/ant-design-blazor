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

- `NotificationService.Open(config:NotificationConfig)`
- `NotificationService.Info(config:NotificationConfig)`
- `NotificationService.Success(config:NotificationConfig)`
- `NotificationService.Warning(config:NotificationConfig)`
- `NotificationService.Warn(config:NotificationConfig)`
- `NotificationService.Error(config:NotificationConfig)`
- `NotificationService.Close(key:string)`
- `NotificationService.Destroy()`

The properties of config are as follows:

| Property | Description | Type | Default |
| ----------- | ------------------------------------------------------------ | ------------------------- | ----------------------------------- |
| Btn         | Customized close button                                           | RenderFragment            | null                                |
| ClassName   | Customized CSS class                                       | string                    | null                                |
| CloseIcon   | custom close icon                                        | RenderFragment            | null                                |
| Description | The content of notification box (required)                                      | string\|RenderFragment    | -                                   |
| Duration    | Time in seconds before Notification is closed. When set to 0 or null, it will never be closed automatically         | double?                   | 4.5                                 |
| Icon        | Customized icon                                              | RenderFragment            | null                                |
| Key         | The unique identifier of the Notification                                         | string                    | null                                |
| Message     | The title of notification box (required)                                  | string\|RenderFragment    | -                                   |
| OnClose     | Trigger when notification closed                                        | event Action                    | null                                |
| OnClick     | Specify a function that will be called when the notification is clicked                         |  event Action                    | null                                |
| Placement   | Position of Notification, can be one of `NotificationPlacement.TopLeft` `NotificationPlacement.TopRight` `NotificationPlacement.BottomLeft` `NotificationPlacement.BottomRight` | NotificationPlacement? | `NotificationPlacement.TopRight` |
| Style       | Customized inline style                                            | string                    | null                                |

`NotificationService` also provides a global `Config()` method that can be used for specifying the default options. Once this method is used, all the notification boxes will take into account these globally defined options when displaying.

`NotificationService.Config(config:NotificationGlobalConfig)`

| Property | Description | Type | Default |
| :-------- | :----------------------------------------------------------- | :------------------------- | :-------------------------------- |
| Bottom    | Distance from the bottom of the viewport, when `placement` is `NotificationPlacement.BottomLeft` or `NotificationPlacement.BottomRight` (unit: pixels).                 | double?                    | 24                                |
| Top       | Distance from the top of the viewport, when `placement` is `NotificationPlacement.TopLeft` or `NotificationPlacement.TopRight` (unit: pixels).                  | double?                    | 24                                |
| CloseIcon | custom close icon                                              | RenderFragment             | -                                 |
| Duration  |  Time in seconds before Notification is closed. When set to 0 or null, it will never be closed automatically                        | double?                    | 4.5                               |
| Placement | Position of Notification, can be one of  `NotificationPlacement.TopLeft` `NotificationPlacement.TopRight` `NotificationPlacement.BottomLeft` `NotificationPlacement.BottomRight` | NotificationPlacement? | NotificationPlacement.TopRight |
| Rtl       |  whether to enable RTL mode                                          | bool                       | `false`                           |
