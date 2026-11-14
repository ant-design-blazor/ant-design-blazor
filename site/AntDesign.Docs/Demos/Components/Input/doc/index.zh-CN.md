---
category: Components
type: 数据录入
title: Input
subtitle: 输入框
cover: https://gw.alipayobjects.com/zos/alicdn/xS9YEJhfe/Input.svg
---

通过鼠标或键盘输入内容，是最基础的表单域的包装。

## 何时使用

- 需要用户输入表单域内容时。
- 提供组合型输入框，带搜索的输入框，还可以进行大小选择。


## API

### Common API

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| AddOnBefore | 带标签的 input，设置前置标签                               | RenderFragment        | -         |
| AddOnAfter            | 带标签的 input，设置后置标签           | RenderFragment         |
| AllowClear |可以点击清除图标删除内容                               | boolean        | false         |
| AllowComplete | 控制 Input HTML 元素的自动完成属性.     | boolean        | true         |
| AutoFocus            | 自动聚焦.           | boolean         | false
| Bordered | 是否有边框        | boolean         | true
| BindOnInput        | 是否输入时绑定 | boolean | false |
| CultureInfo          | 文本格式化时区域本地化选项           | CultureInfo         | CultureInfo.CurrentCulture       |
| DebounceMilliseconds | 延迟 KeyUp 事件的处理，直到用户停止输入一段预定的时间，设置后会开启 `BindOnInput` | int        | 250         |
| DefaultValue |输入框默认内容                              | TValue        | -         |
| Disabled |是否禁用状态，默认为 false                               | boolean        | false         |
| InputElementSuffixClass |作为最后一个Css Class 添加到Input的类目中. | string        | -         | 0.9
| MaxLength |最大长度,-1 表示无限制      | int         | -1
| OnBlur | 输入框失去焦点时的回调                            | Action<FocusEventArgs>        | -         |
| OnChange |输入框内容变化时的回调                                | Action<TValue>        | -        |
| OnFocus |输入框获得焦点时的回调                              | Action<FocusEventArgs>        | -         |
| OnInput |输入框正在输入时的回调                        | Action<ChangeEventArgs>        | -         |
| OnKeyDown |键盘按键于输入框中按下的回调                                | Action<KeyboardEventArgs>        | -         |
| OnKeyUp |键盘按键于输入框中抬起时的回调                               | Action<KeyboardEventArgs>        | -         |
| OnMouseUp |鼠标抬起的回调                                | Action<MouseEventArgs>        | -         |
| OnPressEnter | 按下回车的回调                              | Action<KeyboardEventArgs>        | -         |
| Placeholder|提供可描述输入字段预期值的提示信息(hint)        | string        | -        |
| Prefix | 带有前缀图标的 input                               | RenderFragment        | -        |
| ReadOnly | 输入框是否为只读. | boolean | false    | 0.9
| Size |抽屉元素之间的子组件  `default`, `large`, `small`        | string        | -         |
| StopPropagation | 终止OnClick和Blur事件的进一步传播.    | boolean    | false      |
| Style | 设置 Input 的 CSS 样式 | string | - |  |
| Suffix | 带有后缀图标的 input                               | RenderFragment        | -         |
| Type            |声明 input 类型，同原生 input 标签的 type 属性，见：MDN(请直接使用 TextArea 代替 type="textarea")。         | string  | -         |
| Width | 输入框宽度                              | string        | -         |
| WrapperStyle | 设置外部 `<span>` 元素的样式。当使用了 `Prefix` 、 `Suffix`、 `AllowClear` 属性或者是 `Password` 或 `Search` 组件时，需要用 `WrapperStyle` 来设置整个组件的样式。  | string | - |  |

### Common Methods
| Name | Description | Parameters | Version |
| --- | --- | --- | --- |
| Blur() |移除焦点   | -        | 0.9         |
| Focus() |具有可选行为的Input组件的焦点行为  | (FocusBehavior: {enum: FocusAtLast, FocusAtFirst, FocusAndSelectAll, FocusAndClear }, bool: preventScroll )        | 0.9         |

### TextArea

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AutoSize | 将根据内容调整（扩大或缩小）“TextArea”。 可以与 `MaxRows` 和 `MinRows` 结合使用。 将 `textarea` 组件的 `resize` 属性设置为：`none`. | boolean        | false         |
| Bordered | 是否有边框        | boolean         | true
| DefaultToEmptyString |设置为`false`时，当内容为空或空白时，值将被设置为`null`,当为 `true` 时，值将被设置为空字符串(string.Empty). | boolean        | false         |
| MinRows |`TextArea` 将允许用户拖动缩小，直到可见行等于 `MinRows` 。 使用此属性将自动设置“AutoSize = true”。  | int        | 1         |
| MaxRows |`TextArea` 将允许用户拖动放大,直到可见行等于 `MaxRows` 。 使用此属性将自动设置“AutoSize = true”。  | int        | uint.MaxValue         |
| Rows | 设置行数以表示TextArea 的高度。 | uint        | 3         |
| ShowCount | 是否显示文本计数。 需要设置'MaxLength'属性 | boolean | false         | 0.9
| OnResize | 大小改变时回调                                | Action<OnResizeEventArgs>        | -         |

### Search

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ClassicSearchIcon | 搜索输入使用不使用按钮而使用后缀搜索图标呈现。 当 EnterButton 不为 false 时将被忽略。 | boolean        | false         | 0.9
| EnterButton | 是否有确认按钮，可设为按钮文字。该属性会与 addonAfter 冲突。 | boolean / string        | false         |
| Loading | 搜索 loading | boolean        | false         |
| OnSearch | 点击搜索图标、清除图标，或按下回车键时的回调 | Action<string>        | -    |

### InputGroup

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ChildContent | InputGroup 包装的内容(子元素) | RenderFragment | -         | 
| Compact | 是否使用紧凑模式 | boolean | false         |
| Size | `InputGroup` 中所有的 `Input` 的大小，可选 `large` `default` `small` | InputSize / string        | InputSize.Default         |

### InputPassword

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| IconRender | 自定义切换按钮 | RenderFragment | -         | 0.9
| ShowPassowrd | 是否显示密码 | bool | false         | 0.9
| VisibilityToggle | 是否显示切换按钮 | bool        | true         |
