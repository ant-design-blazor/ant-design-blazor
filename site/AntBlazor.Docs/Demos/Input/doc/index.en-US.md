---
category: Components
type: DataEntry
title: Input
---

A basic widget for getting the user input is a text field. Keyboard and mouse can be used for providing or changing data.

## When To Use

- A user input in a form field is needed.
- A search input is required.


## API

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| AddOnBefore | The label text displayed before (on the left side of) the input field.                             | RenderFragment        | -         |
| AddOnAfter            | The label text displayed after (on the right side of) the input field.           | RenderFragment         |
| ChildContent            | Child content           | RenderFragment         |-       |
| Size |The size of the input box. Note: in the context of a form, the `large` size is used. Available: `large` `default` `small`       | string        | -         |
| Placeholder              | 提供可描述输入字段预期值的提示信息        | string        | -        |
| DefaultValue |  	The initial input content                              | string        | -         |
| MaxLength |  	 	max length       | int         |
| Disabled | Whether the input is disabled.                               | string        | -         |
| AllowClear | allow to remove input content with clear icon                               | boolean        | -         |
| Prefix | The prefix icon for the Input.                           | RenderFragment        | -        |
| Suffix | The suffix icon for the Input.                            | RenderFragment        | -         |
| Type            |The type of input, see: MDN(use `Input.TextArea` instead of type=`textarea`)         | string  | -         |
| OnChange |  	callback when the content is change                                | function(e)        | 0         |
| OnPressEnter | The callback function that is triggered when Enter key is pressed.                           | function(e)        | -         |
| OnInput | callback when user input                              | function(e)        | -         |