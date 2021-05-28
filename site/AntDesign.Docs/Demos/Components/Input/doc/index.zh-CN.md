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

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| AddOnBefore | 带标签的 input，设置前置标签                               | RenderFragment        | -         |
| AddOnAfter            | 带标签的 input，设置后置标签           | RenderFragment         |
| AllowClear |可以点击清除图标删除内容                               | boolean        | false         |
| AutoFocus            | Focus on input element.           | boolean         | false
| Bordered | Whether has border style         | boolean         | true
| CultureInfo          | What Culture will be used when converting string to value and value to string           | CultureInfo         | CultureInfo.CurrentCulture       |
| DebounceMilliseconds | Delays the processing of the KeyUp event until the user has stopped typing for a predetermined amount of time | int        | 250         |
| DefaultValue |输入框默认内容                              | TValue        | -         |
| Disabled |是否禁用状态，默认为 false                               | boolean        | false         |
| InputElementSuffixClass |Css class that will be  added to input element class as the last class entry. | string        | -         | 0.9
| MaxLength |最大长度   -1 means unlimitted.      | int         | -1
| OnBlur | Callback when input looses focus                              | Action<FocusEventArgs>        | -         |
| OnChange |输入框内容变化时的回调                                | Action<TValue>        | -        |
| OnFocus |Callback when input receives focus                              | Action<FocusEventArgs>        | -         |
| OnInput |输入时的回调                               | Action<ChangeEventArgs>        | -         |
| OnkeyDown |Callback when a key is pressed                                | Action<KeyboardEventArgs>        | -         |
| OnkeyUp |Callback when a key is released                                | Action<KeyboardEventArgs>        | -         |
| OnMouseUp |Callback when a mouse button is released                                | Action<MouseEventArgs>        | -         |
| OnPressEnter | 按下回车的回调                              | Action<KeyboardEventArgs>        | -         |
| Placeholder              |提供可描述输入字段预期值的提示信息        | string        | -        |
| Prefix | 带有前缀图标的 input                               | RenderFragment        | -        |
| ReadOnly | When present, it specifies that an input field is read-only. | boolean | false    | 0.9
| Size |抽屉元素之间的子组件  `default`, `large`, `small`        | string        | -         |
| Style | 设置 `<input>` HTML 元素的 CSS 样式 | string | - |  |
| Suffix | 带有后缀图标的 input                               | RenderFragment        | -         |
| Type            |声明 input 类型，同原生 input 标签的 type 属性，见：MDN(请直接使用 Input.TextArea 代替 type="textarea")。         | string  | -         |
| WrapperStyle | 设置外部 `<span>` 元素的样式。当使用了 `Prefix` 、 `Suffix`、 `AllowClear` 属性或者是 `Password` 或 `Search` 组件时，需要用 `WrapperStyle` 来设置整个组件的样式。  | string | - |  |

### Common Methods
| Name | Description | Parameters | Version |
| --- | --- | --- | --- |
| Blur() |Remove focus.   | -        | 0.9         |
| Focus() |Focus behavior for input component with optional behaviors.   | (FocusBehavior: {enum: FocusAtLast, FocusAtFirst, FocusAndSelectAll, FocusAndClear }, bool: preventScroll )        | 0.9         |

### TextArea

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AutoSize |  | boolean        | false         |
| DefaultToEmptyString | When `false`, value will be set to `null` when content is empty or whitespace. When `true`, value will be set to empty string. | boolean        | false         |
| MinRows | `TextArea` will allow shrinking, but it will stop when visible rows = MinRows (will not shrink further).  | int        | 1         |
| MaxRows | `TextArea` will allow growing, but it will stop when visible rows = MaxRows (will not grow further).  | int        | uint.MaxValue         |
| ShowCount | Whether show text count. Requires `MaxLength` attribute to be present  | boolean | false         | 0.9
| OnResize | Callback when the size changes                                | Action<OnResizeEventArgs>        | -         |

### Search

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ClassicSearchIcon | Search input is rendered with suffix search icon, not as a button. Will be ignored when EnterButton is not false. | boolean        | false         | 0.9
| EnterButton | 是否有确认按钮，可设为按钮文字。该属性会与 addonAfter 冲突。 | boolean / string        | false         |
| Loading | 搜索 loading | boolean        | false         |
| OnSearch | 点击搜索图标、清除图标，或按下回车键时的回调 | Action<string>        | -    |

### InputGroup

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ChildContent | Content wrapped by InputGroup. | RenderFragment | -         | 
| Compact | 是否用紧凑模式 | boolean | false         |
| Size | `InputGroup` 中所有的 `Input` 的大小，可选 `large` `default` `small` | InputSize / string        | InputSize.Default         |

### InputPassword

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| IconRender | 自定义切换按钮 | RenderFragment | -         | 0.9
| ShowPassowrd | Whether to show password | bool | false         | 0.9
| VisibilityToggle | 是否显示切换按钮 | bool        | true         |