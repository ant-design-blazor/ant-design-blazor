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
| ChildContent            | Child content           | RenderFragment         |-       |
| Size |The size of the input box. Note: in the context of a form, the `large` size is used. Available: `large` `default` `small`       | string        | -         |
| Placeholder              | Provide prompt information that describes the expected value of the input field        | string        | -        |
| DefaultValue |  	The initial input content                              | string        | -         |
| MaxLength |  	 	max length       | int         |
| Disabled | Whether the input is disabled.                               | string        | -         |
| AllowClear | allow to remove input content with clear icon                               | boolean        | -         |
| Prefix | The prefix icon for the Input.                           | RenderFragment        | -        |
| Suffix | The suffix icon for the Input.                            | RenderFragment        | -         |
| Style | Set CSS style. When using, be aware that some styles can be set only by `WrapperStyle` | string | - |  |
| WrapperStyle | Set CSS style of wrapper. Is used when component has visible: `Prefix`/`Suffix` or has paramter set `AllowClear` or for components: `Password` & `Search`. In these cases, html `<span>` elements is used to wrap the html `<input>` element. `WrapperStyle` is used on the `<span>` element.   | string | - |  |
| Type            |The type of input, see: MDN(use `Input.TextArea` instead of type=`textarea`)         | string  | -         |
| OnChange |callback when the content is change                                | function(e)        | 0         |
| OnPressEnter |The callback function that is triggered when Enter key is pressed.                           | function(e)        | -         |
| OnInput |callback when user input                              | function(e)        | -         |

### TextArea

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| DefaultToEmptyString | When `false`, value will be set to `null` when content is empty or whitespace. When `true`, value will be set to empty string. | boolean        | false         |