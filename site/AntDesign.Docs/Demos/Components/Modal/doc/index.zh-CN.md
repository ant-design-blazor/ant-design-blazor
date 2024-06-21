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
| AfterClose | Modal 关闭后的回调 | EventCallback | - |
| BodyStyle | Modal body 样式 | string | - |
| CancelText | 取消按钮文字 | string\|RenderFragment | Cancel |
| Centered | 垂直居中展示 Modal | bool | `false` |
| Closable | 是否显示右上角的关闭按钮 | bool | true |
| CloseIcon | 自定义关闭图标 | RenderFragment | - |
| ConfirmLoading | 确定按钮 loading | bool | false |
| DestroyOnClose | 关闭时销毁 Modal 里的子元素 | bool | false |
| Header | 头部内容，当不需要头部时，可以设置为`null` | RenderFragment | - |
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
| Title | 标题，如果 TitleTemplate 不为null，则优先显示 TitleTemplate | string | null |
| TitleTemplate | 标题 | RenderFragment | null |
| Visible | 对话框是否可见 | bool | - |
| Width | 宽度 | string\|number | 520 |
| WrapClassName | 对话框外层容器的类名 | string | - |
| ZIndex | 设置 Modal 的 `z-index` | int | 1000 |
| OnCancel | 点击遮罩层或右上角叉或取消按钮的回调 | EventCallback<MouseEventArgs> | - |
| OnOk | 点击确定回调 | EventCallback<MouseEventArgs> | - |
| Draggable | 是否允许通过 Modal的 Header 拖动 Modal（如果为true，Title 和 TitleTemplate 至少有一个必须有值） | bool | false |
| DragInViewport | 如果 Draggable 为 true，是否仅在视窗内拖动Modal | bool | true |
| MaxBodyHeight | Modal 内容的最大高度 | string? | null |
| Maximizable | 是否显示最大化按钮 | bool | false |
| MaximizeBtnIcon | Modal在正常状态下的最大化按钮icon | RenderFragment | fullscreen       |
| RestoreBtnIcon  | Modal在最大化状态下的还原按钮icon | RenderFragment | fullscreen-exit  |
| DefaultMaximized | Modal 在初始化时即为最大化状态 | bool | false |
| Resizable | 是否可以对 Modal 进行大小调整 | bool | false |


#### 注意

> `<Modal />` 默认关闭后状态不会自动清空, 如果希望每次打开都是新内容，请设置 `DestroyOnClose`。

### ModalService

1. 用于创建 Confirm dialog

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

2. 用于创建 Modal dialog

-  `ModalService.CreateModalAsync`


> 请确认已经在 `App.Razor` 中添加了 `<AntContainer />` 组件。
> `ConfirmAsync`、`InfoAsync`、`SuccessAsync`、`ErrorAsync`、`WarningAsync` 返回值为Task<bool>，可用于判断用户点击的按钮是 OK按钮(true) 还是 Cancel按钮(false)。

### ConfirmService

`IConfirmService.Show` 用于弹出类似于在WinForm中使用 MessageBox 方式弹出的对话框。这和 ModalService 不同的是，ModalService 只可以创建 具有 OK-Cancel 按钮的对话框并返回 ConfirmRef 对象或者是否OK按钮被点击，而 ConfirmService 总是返回 ConfirmResult ，以指示哪个按钮被点击。


#### ConfirmOptions

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| AutoFocusButton | 指定自动获得焦点的按钮 | ConfirmAutoFocusButton | `ConfirmAutoFocusButton.Ok` |
| CancelText | 取消按钮文字，等价于 Button2Props.ChildContent               | string | Cancel                      |
| Centered | 垂直居中展示 Modal | bool | `false` |
| ClassName | 容器(.ant-modal)类名 | string | - |
| Content | 内容 | string\|RenderFragment | - |
| Icon | 自定义图标 | RenderFragment | -                           |
| MaskClosable | 点击蒙层是否允许关闭 | bool | `false` |
| OkText | 确认按钮文字，等价于 Button1Props.ChildContent | string\|RenderFragment | 确定 |
| OkType | 确认按钮类型，等价于 Button1Props.Type | string | primary |
| OkButtonProps | ok 按钮 props，与 Button1Props 等价 | ButtonProps | - |
| CancelButtonProps | cancel 按钮 props，与 Button2Props 等价 | ButtonProps | - |
| Title | 标题，如果 TitleTemplate 不为null，则优先显示 TitleTemplate | string | null |
| TitleTemplate | 标题 | RenderFragment | null |
| Width | 宽度 | string\|double | 416 |
| ZIndex | 设置 Modal 的 `z-index` | int | 1000 |
| OnCancel | 取消回调，参数为关闭函数，仅会在通过 ModalService 创建的 Confirm 触发 | EventCallback<MouseEventArgs>?         | null |
| OnOk | 点击确定回调，参数为关闭函数，仅会在通过 ModalService 创建的 Confirm 触发 | EventCallback<MouseEventArgs>?         | null |
| Button1Props | 在LTR模式中最左侧按钮的属性 | ButtonProps | Type = ButtonType.Primary, ChildContent 与 ConfirmButtons 顺序相同 |
| Button2Props | 在LTR模式中左边第二个按钮的属性 | ButtonProps |  ChildContent 与 ConfirmButtons 顺序相同|
| Button3Props | 在LTR模式中左边第三个按钮的属性 | ButtonProps |  ChildContent 与 ConfirmButtons 顺序相同 |

以上函数调用后，会返回一个引用，可以通过该引用更新和关闭弹窗。

``` c#
ConfirmOptions config = new ConfirmOptions();
var modelRef = await ModalService.Info(config);

modelRef.UpdateConfirmAsync();

ModalService.DestroyConfirmAsync(modelRef);
```

- `ModalService.DestroyAllConfirmAsync`

使用 `ModalService.DestroyAllConfirmAsync()` 可以销毁弹出的确认窗（即上述的 ModalService.Info、ModalService.Success、ModalService.Error、ModalService.Warning、ModalService.Confirm）。通常用于路由监听当中，处理路由前进、后退不能销毁确认对话框的问题，而不用各处去使用实例的返回值进行关闭（ModalService.DestroyConfirmAsync(config) 适用于主动关闭，而不是路由这样被动关闭）


## FAQ

### 为什么通过`ModalService.CreateModalAsync<>`创建的Modal，在关闭后，继承自`FeedbackComponent<>`的自定义组件不会执行`Dispose`方法？

Modal在关闭后默认不会从DOM中移除，因此自定义组件的`Dispose`方法不会被执行。可以通过设置`modalOptions.DestroyOnClose=true`来改变此默认行为。
