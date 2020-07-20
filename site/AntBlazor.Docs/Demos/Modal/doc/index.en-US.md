---
type: Feedback
category: Components
title: Modal
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
| Footer            | Footer content, set as  footer={null} when you don't need default buttons | string\|RenderFragment        | 确定取消按钮  |
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
| Title             | The modal dialog's title                                     | string\|RenderFragment        | -             |
| Visible           | Whether the modal dialog is  visible or not                  | bool                          | -             |
| Width             | Width of the modal dialog                                    | string\|double                | 520           |
| WrapClassName     | The class name of the  container of the modal dialog         | string                        | -             |
| ZIndex            | The z-index of the Modal                                     | int                           | 1000          |
| OnCancel          | Specify a function that will  be called when a user clicks mask, close button on top right or Cancel button | EventCallback<MouseEventArgs> | -             |
| OnOk              | Specify a function that will  be called when a user clicks the OK button | EventCallback<MouseEventArgs> | -             |

#### Note

> The state of Modal will be preserved at it's component lifecycle by default, if you wish to open it with a brand new state everytime, set `DestroyOnClose` on it.

### Modal.method()

There are five ways to display the information based on the content's nature:

- `ModalService.Info`
- `ModalService.Success`
- `ModalService.Error`
- `ModalService.Warning`
- `ModalService.Confirm`

> Please confirm that the `<AntContainer />` component has been added to `App.Razor`.

The items listed above are all functions, expecting a settings object as parameter. The properties of the object are follows:

| CancelText        | Text  of the Cancel button with Modal.confirm                | string                         | Cancel  |
| ----------------- | ------------------------------------------------------------ | ------------------------------ | ------- |
| Centered          | Centered Modal                                               | bool                           | fasle   |
| ClassName         | className of container                                       | string                         | -       |
| Content           | Content                                                      | string\|RenderFragment         | -       |
| Icon              | custom icon                                                  | RenderFragment                 | -       |
| Keyboard          | Whether support press esc to  close                          | bool                           | true    |
| Mask              | Whether show mask or not.                                    | bool                           | true    |
| MaskClosable      | Whether to close the modal  dialog when the mask (area outside the modal) is clicked | bool                           | fasle   |
| OkText            | Text of the OK button                                        | string                         | OK      |
| OkType            | Button type of the OK button                                 | string                         | primary |
| OkButtonProps     | The ok button props                                          | ButtonProps                    | -       |
| CancelButtonProps | The cancel button props                                      | ButtonProps                    | -       |
| Title             | Title                                                        | string\|RenderFragment         | -       |
| Width             | Width of the modal dialog                                    | string\|double                 | 416     |
| ZIndex            | The z-index of the Modal                                     | int                            | 1000    |
| OnCancel          | Specify a function that will  be called when the user clicks the Cancel button. The parameter of this  function is a function whose execution should include closing the dialog. You  can also just return a promise and when the promise is resolved, the modal dialog  will also be closed | EventCallback<MouseEventArgs>? | null    |
| OnOk              | Specify a function that will  be called when the user clicks the OK button. The parameter of this function  is a function whose execution should include closing the dialog. You can also  just return a promise and when the promise is resolved, the modal dialog will  also be closed | EventCallback<MouseEventArgs>? | null    |

All the `ModalService.Method`s will return a reference, and then we can update and close the modal dialog by the reference.

```jsx
ConfirmOptions config = new ConfirmOptions();
ModalService.Info(config);

ModalService.Update(config);

ModalService.Destroy(config);
```

- `ModalService.DestroyAll`

`ModalService.DestroyAll()` could destroy all confirmation modal dialogs(ModalService.Info/ModalService.Success/ModalService.Error/ModalService.Warning/ModalService.Confirm). Usually, you can use it in router change event to destroy confirm modal dialog automatically without use modal reference to close( it's too complex to use for all modal dialogs)