---
category: Components
type: Data Entry
title: Input
cover: https://gw.alipayobjects.com/zos/alicdn/xS9YEJhfe/Input.svg
---

A basic widget for getting the user input is a text field. Keyboard and mouse can be used for providing or changing data.

## When To Use

- A user input in a form field is needed.
- A search input is required.


## API

### Common API

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| AddOnBefore | The label text displayed before (on the left side of) the input field.                             | RenderFragment        | -         |
| AddOnAfter            | The label text displayed after (on the right side of) the input field.           | RenderFragment         |
| AllowClear | Allow to remove input content with clear icon                               | boolean        | false         |
| AllowComplete | Controls the autocomplete attribute of the input HTML element.     | boolean        | true         |
| AutoFocus            | Focus on input element.           | boolean         | false
| Bordered | Whether has border style         | boolean         | true
| BindOnInput        | Whether to bind on input | boolean | false |
| CultureInfo          | What Culture will be used when converting string to value and value to string. Useful for InputNumber component.           | CultureInfo         | CultureInfo.CurrentCulture       |
| DebounceMilliseconds | Delays the processing of the KeyUp event until the user has stopped typing for a predetermined amount of time. The setting will enable 'BindOnInput'. | int        | 250         |
| DefaultValue |  	The initial input content                              | TValue        | -         |
| Disabled | Whether the input is disabled.                               | boolean        | false     |
| InputElementSuffixClass |Css class that will be  added to input element class as the last class entry. | string        | -         | 0.9
| MaxLength |  	 	Max length. -1 means unlimited.      | int         | -1
| OnBlur | Callback when input looses focus                              | Action<FocusEventArgs>        | -         |
| OnChange |Callback when the content changes                                | Action<TValue>        | -         |
| OnFocus |Callback when input receives focus                              | Action<FocusEventArgs>        | -         |
| OnInput |Callback when value is inputed                              | Action<ChangeEventArgs>        | -         |
| OnKeyDown |Callback when a key is pressed                                | Action<KeyboardEventArgs>        | -         |
| OnKeyUp |Callback when a key is released                                | Action<KeyboardEventArgs>        | -         |
| OnMouseUp |Callback when a mouse button is released                                | Action<MouseEventArgs>        | -         |
| OnPressEnter |The callback function that is triggered when Enter key is pressed.                           | Action<KeyboardEventArgs>        | -         |
| Placeholder              | Provide prompt information that describes the expected value of the input field        | string        | -        |
| Prefix | The prefix icon for the Input.                           | RenderFragment        | -        |
| ReadOnly | When present, it specifies that an input field is read-only. | boolean | false    | 0.9
| ShowClear | Overrides whether the clear button should be shown when `AllowClear` is true (otherwise this has no effect). If null, the default behavior is used, and the clear button is only shown if the input is not empty. | boolean?     | -      |
| Size |The size of the input box. Note: in the context of a form, the `large` size is used. Available: `large` `default` `small`       | string        | -         |
| StopPropagation Controls onclick & blur event propagation.    | boolean    | false      | 0.10.0
| Style | Set CSS style. When using, be aware that some styles can be set only by `WrapperStyle` | string | - |  |
| Suffix | The suffix icon for the Input.                            | RenderFragment        | -         |
| Type            |The type of input, see: MDN(use `Input.TextArea` instead of type=`textarea`)         | string  | -         |
| Width         | The width of the input box. | string | - |
| WrapperStyle | Set CSS style of wrapper. Is used when component has visible: `Prefix`/`Suffix` or has parameter set `AllowClear` or for components: `Password` & `Search`. In these cases, html `<span>` elements is used to wrap the html `<input>` element. `WrapperStyle` is used on the `<span>` element.   | string | - |  |

### Common Methods
| Name | Description | Parameters | Version |
| --- | --- | --- | --- |
| Blur() |Remove focus.   | -        | 0.9         |
| Focus() |Focus behavior for input component with optional behaviors.   | (FocusBehavior: {enum: FocusAtLast, FocusAtFirst, FocusAndSelectAll, FocusAndClear }, bool: preventScroll )        | 0.9         |

### TextArea

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| AutoSize | Will adjust (grow or shrink) the `TextArea` according to content. Can work in connection with `MaxRows` & `MinRows`. Sets `resize` attribute of the `textarea` HTML element to: `none`. | boolean        | false         |
| Bordered | Whether has border style         | boolean         | true
| DefaultToEmptyString | When `false`, value will be set to `null` when content is empty or whitespace. When `true`, value will be set to empty string. | boolean        | false         |
| MinRows | `TextArea` will allow shrinking, but it will stop when visible rows = `MinRows` (will not shrink further). Using this property will autoset `AutoSize = true`.  | int        | 1         |
| MaxRows | `TextArea` will allow growing, but it will stop when visible rows = `MaxRows` (will not grow further). Using this property will autoset `AutoSize = true`.  | int        | uint.MaxValue         |
| Rows | Sets the height of the TextArea expressed in number of rows. | uint        | 3         |
| ShowCount | Whether show text count. Requires `MaxLength` attribute to be present  | boolean | false         | 0.9
| OnResize | Callback when the size changes | Action<OnResizeEventArgs>        | -         |

### Search

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ClassicSearchIcon | Search input is rendered with suffix search icon, not as a button. Will be ignored when EnterButton is not false. | boolean        | false         | 0.9
| EnterButton | Whether to show an enter button after input. This property conflicts with the `AddonAfter` property | boolean / string        | false         |
| Loading | Search box with loading. | boolean        | false         |
| OnSearch | Search input is rendered with suffix search icon, not as a button. Will be ignored when EnterButton is not false. | Action<string>        | -    |

### InputGroup

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ChildContent | Content wrapped by InputGroup. | RenderFragment | -         | 
| Compact | Whether use compact style | boolean | false         |
| Size | The size of `InputGroup` specifies the size of the included `Input` fields. Available: `large` `default` `small` | InputSize / string        | InputSize.Default         |

### InputPassword

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| IconRender | Custom toggle button. | RenderFragment | -         | 0.9
| ShowPassowrd | Whether to show password | bool | false         | 0.9
| VisibilityToggle | Whether show toggle button | bool        | true         |