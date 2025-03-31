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

This components provides some methods, with usage and arguments as following:

- `IMessageService.Success(content, [duration], onClose)`
- `IMessageService.Error(content, [duration], onClose)`
- `IMessageService.Info(content, [duration], onClose)`
- `IMessageService.Warning(content, [duration], onClose)`
- `IMessageService.Warn(content, [duration], onClose)` // alias of warning
- `IMessageService.Loading(content, [duration], onClose)`

| Argument | Description | Type | Default |
| --- | --- | --- | --- |
| content | content of the message | string\|ReactNode\|config | - |
| duration | time(seconds) before auto-dismiss, don't dismiss if set to 0 or null | double? | 3 |
| onClose | Specify a function that will be called when the message is closed | Action | - |

`afterClose` can be called in thenable interface:

- `IMessageService.[level](content, [duration]).ContinueWith(afterClose)`
- `IMessageService.[level](content, [duration], onClose).ContinueWith(afterClose)`

where `level` refers a method of `IMessageService`. The result of `ContinueWith` method will be a Task.

Supports passing parameters wrapped in an object:

- `IMessageService.Open(config:MessageConfig)`
- `IMessageService.Success(config:MessageConfig)`
- `IMessageService.Error(config:MessageConfig)`
- `IMessageService.Info(config:MessageConfig)`
- `IMessageService.Warning(config:MessageConfig)`
- `IMessageService.Warn(config:MessageConfig)` // alias of warning
- `IMessageService.Loading(config:MessageConfig)`

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

- `IMessageService.Config(options:MessageGlobalConfig)`
- `IMessageService.Destroy()`

#### message.config

```c#
@inject IMessageService MessageService;

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

### Extension Methods

The `MessageExtensions` class provides extension methods to display loading indicators during asynchronous operations:

#### LoadingWhen<T>

Displays a loading message while an asynchronous operation is being executed, and returns the result of the operation.

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| asyncFunc | The asynchronous operation to execute | Func<Task<T>> | - |
| content | The message to display during loading | string\|RenderFragment\|MessageConfig | - |
| onClose | The action to perform when the loading message is closed | Action | null |

#### LoadingWhen (version without return value)

Displays a loading message while an asynchronous operation is being executed. Suitable for operations without a return value.

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| asyncFunc | The asynchronous operation to execute | Func<Task> | - |
| content | The message to display during loading | string\|RenderFragment\|MessageConfig | - |
| onClose | The action to perform when the loading message is closed | Action | null |