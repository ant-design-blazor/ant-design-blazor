---
type: Feedback
category: Components
title: Modal
cover: https://gw.alipayobjects.com/zos/alicdn/3StSdUlSH/Modal.svg
---

Modal dialogs.

## When To Use

When requiring users to interact with the application, but without jumping to a new page and interrupting the user's workflow, you can use `Modal` to create a new floating layer over the current page to get user feedback or display information. Additionally, if you need show a simple confirmation dialog, you can use `ModalService.Confirm()`, and so on.

## API

| Property          | Description                                                  | Type                          | Default       |
| ----------------- | ------------------------------------------------------------ | ----------------------------- | ------------- |
| AfterClose        | Specify a function that will  be called when modal is closed completely. | EventCallback                 | -             |
| BodyStyle         | Body style for modal body  element. Such as height, padding etc. | string                        | -             |
| CancelText        | Text of the Cancel button                                    | string\|RenderFragment        | Cancel        |
| Centered          | Centered Modal                                               | bool                          | false         |
| Closable          | Whether a close (x) button is  visible on top right of the modal dialog or not | bool                          | true          |
| CloseIcon         | custom close icon                                            | RenderFragment                | -             |
| ConfirmLoading    | Whether to apply loading  visual effect for OK button or not | bool                          | false         |
| DestroyOnClose    | Whether to unmount child  components on onClose              | bool                          | false         |
| Header            | Header content, set as Header="null" when you don't need default header      | RenderFragment                | -             |
| Footer            | Footer content, set as Footer="null" when you don't need default buttons | string\|RenderFragment    | confirm and cancel buttons  |
| ForceRender       | Force render Modal                                           | bool                          | false         |
| GetContainer      | Return the mount node for  Modal                             | ElementReference?             | document.body |
| Keyboard          | Whether support press esc to close                           | bool                          | true          |
| Mask              | Whether show mask or not.                                    | bool                          | true          |
| MaskClosable      | Whether to close the modal  dialog when the mask (area outside the modal) is clicked | bool                          | true          |
| MaskStyle         | Style for modal's mask  element.                             | string                        | -             |
| OkText            | Text of the OK button                                        | string\|RenderFragment        | OK            |
| OkType            | Button type of the OK button                                 | string                        | primary       |
| OkButtonProps     | The ok button props                                          | ButtonProps                   | -             |
| CancelButtonProps | The cancel button props                                      | ButtonProps                   | -             |
| Style             | Style of floating layer,  typically used at least for adjusting the position. | string                        | -             |
| Title             | The modal dialog's title, If the TitleTemplate is not null, the TitleTemplate takes precedence | string | null            |
| TitleTemplate | The modal dialog's title | RenderFragment | null |
| Visible           | Whether the modal dialog is  visible or not                  | bool                          | -             |
| Width             | Width of the modal dialog                                    | string\|double                | 520           |
| WrapClassName     | The class name of the  container of the modal dialog         | string                        | -             |
| ZIndex            | The z-index of the Modal                                     | int                           | 1000          |
| OnCancel          | Specify a function that will  be called when a user clicks mask, close button on top right or Cancel button | EventCallback<MouseEventArgs> | -             |
| OnOk              | Specify a function that will  be called when a user clicks the OK button | EventCallback<MouseEventArgs> | -             |
| Draggable | Is it allowed to drag Modal through its Header（if true, at least one of Title and TitleTemplate must have a value） | bool | false |
| DragInViewport | If Draggable is true, and is it only allowed drag Modal in the viewport | bool | true |
| MaxBodyHeight | Modal content max height | string? | null |
| Maximizable       | Whether to display the maximize button | bool   | false |
| MaximizeBtnIcon | The icon of the maximize button when the modal is in normal state    | RenderFragment | fullscreen       |
| RestoreBtnIcon  | The icon of the maximize button when the modal is in maximized state | RenderFragment | fullscreen-exit  |
| DefaultMaximized | Modal is maximized at initialization | bool   | false |
| Resizable | Can Modal be resized | bool | false |

#### Note

> The state of Modal will be preserved at it's component lifecycle by default, if you wish to open it with a brand new state everytime, set `DestroyOnClose` on it.

### ModalService.method()

There are some ways to display the information based on the content's nature:

1. For Confirm dialog

- `ModalService.Info`
- `ModalService.Success`
- `ModalService.Error`
- `ModalService.Warning`
- `ModalService.Confirm`
- `ModalService.CreateAsync`
- `ModalService.ConfirmAsync`
- `ModalService.InfoAsync`
- `ModalService.SuccessAsync`
- `ModalService.ErrorAsync`
- `ModalService.WarningAsync`
- `ModalService.UpdateConfirmAsync`
-  `ModalService.DestroyConfirmAsync`
-  `ModalService.DestroyAllConfirmAsync`
-  `ModalService.CreateConfirmAsync`

2. For Modal dialog

-  `ModalService.CreateModalAsync`


> Please confirm that the `<AntContainer />` component has been added to `App.Razor`.
> `ConfirmAsync`、`InfoAsync`、`SuccessAsync`、`ErrorAsync`、`WarningAsync` will return Task<bool>，it can be used to determine whether the button the user clicks is an OK button (true) or a cancel button (false)

### ConfirmService

`IConfirmService.Show` to show a simple Confirm dialog like MessageBox of Windows, it's different from ModalService. ModalService can only create OK-Cancel Confirm dialog and return ConfirmRef or OK button is clicked, but ConfirmService return ConfirmResult.


#### ConfirmOptions

| CancelText        | Text  of the Cancel button, equivalent to Button2Props.ChildContent | string\|RenderFragment                         | Cancel  |
| ----------------- | ------------------------------------------------------------ | ------------------------------ | ------- |
| Centered          | Centered Modal                                               | bool                           | false   |
| ClassName         | className of container                                       | string                         | -       |
| Content           | Content                                                      | string\|RenderFragment         | -       |
| Icon              | custom icon                                                  | RenderFragment                 | -       |
| Keyboard          | Whether support press esc to  close                          | bool                           | true    |
| Mask              | Whether show mask or not.                                    | bool                           | true    |
| MaskClosable      | Whether to close the modal  dialog when the mask (area outside the modal) is clicked | bool                           | false   |
| OkText            | Text of the OK button, equivalent to Button1Props.ChildContent | string                         | OK      |
| OkType            | Button type of the OK button, equivalent to Button1Props.Type  | string                         | primary |
| OkButtonProps     | The ok button props , equivalent to Button1Props  | ButtonProps                    | -       |
| CancelButtonProps | The cancel button props, equivalent to Button2Props | ButtonProps                    | -       |
| Title             | The modal dialog's title, If the TitleTemplate is not null, the TitleTemplate takes precedence | string | null            |
| TitleTemplate | The modal dialog's title | RenderFragment | null |
| Width             | Width of the modal dialog                                    | string\|double                 | 416     |
| ZIndex            | The z-index of the Modal                                     | int                            | 1000    |
| OnCancel          | Specify a function that will  be called when the user clicks the Cancel button. The parameter of this  function is a function whose execution should include closing the dialog. You  can also just return a promise and when the promise is resolved, the modal dialog  will also be closed. **It's only trigger in Confirm created by ModalService mode** | EventCallback<MouseEventArgs>? | null    |
| OnOk              | Specify a function that will  be called when the user clicks the OK button. The parameter of this function  is a function whose execution should include closing the dialog. You can also  just return a promise and when the promise is resolved, the modal dialog will  also be closed. **It's only trigger in Confirm created by ModalService mode** | EventCallback<MouseEventArgs>? | null    |
| Button1Props | the props of the leftmost button in LTR layout  | ButtonProps | Type = ButtonType.Primary, ChildContent is in the same order as ConfirmButtons |
| Button2Props | the props of the second button on the left is in the LTR layout  | ButtonProps |  ChildContent is in the same order as ConfirmButtons|
| Button3Props | the props of the rightmost button in LTR layout | ButtonProps | ChildContent is in the same order as ConfirmButtons |

All the `ModalService.Method`s will return a reference, and then we can update and close the modal dialog by the reference.

``` c#
ConfirmOptions config = new ConfirmOptions();
var modelRef = await ModalService.Info(config);

modelRef.UpdateConfirmAsync();

ModalService.DestroyConfirmAsync(modelRef);
```

- `ModalService.DestroyAllConfirmAsync`

`ModalService.DestroyAllConfirmAsync()` could destroy all confirmation modal dialogs(ModalService.Info/ModalService.Success/ModalService.Error/ModalService.Warning/ModalService.Confirm). Usually, you can use it in router change event to destroy confirm modal dialog automatically without use modal reference to close( it's too complex to use for all modal dialogs)

## FAQ

### Why does the custom component inherited from `FeedbackComponent<>` not execute the `Dispose` method when a Modal created by `ModalService.CreateModalAsync<>` is closed?

Modal will not be removed from DOM by default after closing, so the `Dispose` method of custom component will not be executed. You can change this default behavior by setting `modalOptions.DestroyOnClose=true`.
