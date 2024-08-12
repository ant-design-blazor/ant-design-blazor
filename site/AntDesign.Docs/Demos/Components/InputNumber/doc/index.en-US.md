---
category: Components
type: Data Entry
title: InputNumber
cover: https://gw.alipayobjects.com/zos/alicdn/XOS8qZ0kU/InputNumber.svg
---

Enter a number within certain range with the mouse or keyboard.

## When To Use

- When a numeric value needs to be provided.


## Types of Supports

`sbyte`, `byte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `float`, `double`, `decimal`

Nullable types of the above types are also supported. For example, `ushort?`, `int?`, etc.

## API

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| AutoFocus |get focus when component mounted                              | boolean        | -         |
| CultureInfo          | What Culture will be used when converting string to value and value to string           | CultureInfo         | CultureInfo.CurrentCulture       |
| DefaultValue            |initial value           | number         |
| Disabled            | disable the input          | boolean         |-       |
| Formatter | Specifies the format func of the value presented      | Func<double,string>        | -         |
| Format |  Specifies the format pattern of the value presented      | string        | -         |
| Max              | max value       | number        | -        |
| Min |  	min value                            | number        | -         |
| Parser |  Specifies the value extracted from formatter      | function(string, double)           |
| Size | 	height of input box                           | `large` ,`middle`,`small`        | -        |
| Step | The number to which the current value is increased or decreased. It can be an integer or decimal.                           | double        | -         |
| Value            |	current value | string  | -         |
| ValueChanged |The callback triggered when the value is changed.                     | function(double)        | -        |
| MaxLength | The max length of the input.| int | - |
| Width | The width of the input. | string | - |
