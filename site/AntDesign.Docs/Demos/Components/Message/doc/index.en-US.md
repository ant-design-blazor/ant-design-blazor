---
category: Components
type: Feedback
title: Message
cover: https://gw.alipayobjects.com/zos/alicdn/hAkKTIW0K/Message.svg
---

Display global messages as feedback in response to user operations.

## When To Use

- To provide feedback such as success, warning, error etc.
- A message is displayed at top and center and will be dismissed automatically, as a non-interrupting light-weighted prompt.

## API

> Please confirm that the `<AntContainer />` component has been added to `App.Razor`.

This components provides some static methods, with usage and arguments as following:

- `MessageService.Success(content, [duration], onClose)`
- `MessageService.Error(content, [duration], onClose)`
- `MessageService.Info(content, [duration], onClose)`
- `MessageService.Warning(content, [duration], onClose)`
- `MessageService.Warn(content, [duration], onClose)` // alias of warning
- `MessageService.Loading(content, [duration], onClose)`

| Argument | Description | Type | Default |
| --- | --- | --- | --- |
| content | content of the message | string\|ReactNode\|config | - |
| duration | time(seconds) before auto-dismiss, don't dismiss if set to 0 or null | double? | 3 |
| onClose | Specify a function that will be called when the message is closed | Action | - |

`afterClose` can be called in thenable interface:

- `MessageService.[level](content, [duration]).ContinueWith(afterClose)`
- `MessageService.[level](content, [duration], onClose).ContinueWith(afterClose)`

where `level` refers one static methods of `Message`. The result of `ContinueWith` method will be a Task.

Supports passing parameters wrapped in an object:

- `MessageService.Open(config:MessageConfig)`
- `MessageService.Success(config:MessageConfig)`
- `MessageService.Error(config:MessageConfig)`
- `MessageService.Info(config:MessageConfig)`
- `MessageService.Warning(config:MessageConfig)`
- `MessageService.Warn(config:MessageConfig)` // alias of warning
- `MessageService.Loading(config:MessageConfig)`

The properties of config are as follows:

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Content | content of the message | string\|RenderFragment | - |
| Duration | time(seconds) before auto-dismiss, don't dismiss if set to 0 or null | double? | 3 |
| OnClose | Specify a function that will be called when the message is closed | event Action  | - |
| Icon | Customized Icon | RenderFragment | - |
| Key | The unique identifier of the Message | string | - |

### Global static methods

Methods for global configuration and destruction are also provided:

- `MessageService.Config(options:MessageGlobalConfig)`
- `MessageService.Destroy()`

#### message.config

```c#
MessageService.Config(new MessageGlobalConfig{
  Top: 100,
  Duration: 2,
  MaxCount: 3,
  Rtl: true,
});
```

| Argument | Description | Type | Default |
| --- | --- | --- | --- |
| Duration | time before auto-dismiss, in seconds | double | 3 |
| MaxCount | max message show, drop oldest if exceed limit | int | - |
| Top | distance from top | double | 24 |
| Rtl | whether to enable RTL mode | bool | `false` |