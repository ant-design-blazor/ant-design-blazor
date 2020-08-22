---
type: 反馈
category: Components
subtitle: 对话框
title: Modal
cover: https://gw.alipayobjects.com/zos/alicdn/3StSdUlSH/Modal.svg
---

模态对话框。

## 何时使用

需要用户处理事务，又不希望跳转页面以致打断工作流程时，可以使用 `Modal` 在当前页面正中打开一个浮层，承载相应的操作。

另外当需要一个简洁的确认框询问用户时，可以使用 `ModalService.Confirm()` 等语法糖方法。

## API

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| AfterClose | Modal 完全关闭后的回调 | EventCallback | - |
| BodyStyle | Modal body 样式 | string | - |
| CancelText | 取消按钮文字 | string\|RenderFragment | Cancel |
| Centered | 垂直居中展示 Modal | bool | `false` |
| Closable | 是否显示右上角的关闭按钮 | bool | true |
| CloseIcon | 自定义关闭图标 | RenderFragment | - |
| ConfirmLoading | 确定按钮 loading | bool | false |
| DestroyOnClose | 关闭时销毁 Modal 里的子元素 | bool | false |
| Footer | 底部内容，当不需要默认底部按钮时，可以设为 `null` | string\|RenderFragment | 确定取消按钮 |
| ForceRender | 强制渲染 Modal | bool | false |
| GetContainer | 指定 Modal 挂载的 HTML 节点, false 为挂载在当前 dom | ElementReference? | document.body |
| Keyboard | 是否支持键盘 esc 关闭 | bool | true |
| Mask | 是否展示遮罩 | bool | true |
| MaskClosable | 点击蒙层是否允许关闭 | bool | true |
| MaskStyle | 遮罩样式 | string | {} |
| OkText | 确认按钮文字 | string\|RenderFragment | OK |
| OkType | 确认按钮类型 | string | primary |
| OkButtonProps | ok 按钮 props | ButtonProps                   | - |
| CancelButtonProps | cancel 按钮 props | ButtonProps | - |
| Style | 可用于设置浮层的样式，调整浮层位置等 | string | - |
| Title | 标题 | string\|RenderFragment | - |
| Visible | 对话框是否可见 | bool | - |
| Width | 宽度 | string\|number | 520 |
| WrapClassName | 对话框外层容器的类名 | string | - |
| ZIndex | 设置 Modal 的 `z-index` | int | 1000 |
| OnCancel | 点击遮罩层或右上角叉或取消按钮的回调 | EventCallback<MouseEventArgs> | - |
| OnOk | 点击确定回调 | EventCallback<MouseEventArgs> | - |

#### 注意

> `<Modal />` 默认关闭后状态不会自动清空, 如果希望每次打开都是新内容，请设置 `DestroyOnClose`。

### ModalService

包括：

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

> 请确认已经在 `App.Razor` 中添加了 `<AntContainer />` 组件。
> `ConfirmAsync`、`InfoAsync`、`SuccessAsync`、`ErrorAsync`、`WarningAsync` 返回值为Task<bool>，可用于判断用户点击的按钮是 OK按钮(true) 还是 Cancel按钮(false)

#### ConfirmOptions

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| AutoFocusButton | 指定自动获得焦点的按钮 | ConfirmAutoFocusButton | `ConfirmAutoFocusButton.Ok` |
| CancelText | 设置 Modal.confirm 取消按钮文字 | string | Cancel                      |
| Centered | 垂直居中展示 Modal | bool | `false` |
| ClassName | 容器类名 | string | - |
| Content | 内容 | string\|RenderFragment | - |
| Icon | 自定义图标 | RenderFragment | -                           |
| MaskClosable | 点击蒙层是否允许关闭 | bool | `false` |
| OkText | 确认按钮文字 | string | 确定 |
| OkType | 确认按钮类型 | string | primary |
| OkButtonProps | ok 按钮 props | ButtonProps | - |
| CancelButtonProps | cancel 按钮 props | ButtonProps | - |
| Title | 标题 | string\|RenderFragment | - |
| Width | 宽度 | string\|double | 416 |
| ZIndex | 设置 Modal 的 `z-index` | int | 1000 |
| OnCancel | 取消回调，参数为关闭函数，返回 promise 时 resolve 后自动关闭 | EventCallback<MouseEventArgs>?         | null |
| OnOk | 点击确定回调，参数为关闭函数，返回 promise 时 resolve 后自动关闭 | EventCallback<MouseEventArgs>?         | null |

以上函数调用后，会返回一个引用，可以通过该引用更新和关闭弹窗。

``` c#
ConfirmOptions config = new ConfirmOptions();
var modelRef = await ModalService.Info(config);

modelRef.UpdateConfig();

ModalService.Destroy(modelRef);
```

- `ModalService.DestroyAll`

使用 `ModalService.DestroyAll()` 可以销毁弹出的确认窗（即上述的 ModalService.Info、ModalService.Success、ModalService.Error、ModalService.Warning、ModalService.Confirm）。通常用于路由监听当中，处理路由前进、后退不能销毁确认对话框的问题，而不用各处去使用实例的返回值进行关闭（ModalService.Destroy(config) 适用于主动关闭，而不是路由这样被动关闭）


