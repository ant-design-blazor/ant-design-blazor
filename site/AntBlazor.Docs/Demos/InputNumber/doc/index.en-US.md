---
category: Components
type: Data Entry
title: InputNumber
---

Enter a number within certain range with the mouse or keyboard.

## When To Use

- When a numeric value needs to be provided.



## API

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| AutoFocus | get focus when component mounted                              | boolean        | -         |
| DefaultValue            | 	initial value           | number         |
| Disabled            | disable the input          | boolean         |-       |
| Formatter |Specifies the format of the value presented      | function(double,string)        | -         |
| Max              | max value       | number        | -        |
| Min |  	min value                            | number        | -         |
| Parser |  	Specifies the value extracted from formatter      | function(string, double)           |
| Size | 		height of input box                           | `large` ,`middle`,`small`        | -        |
| Step | The number to which the current value is increased or decreased. It can be an integer or decimal.                           | double        | -         |
| Value            |	current value | string  | -         |
| ValueChanged |  	The callback triggered when the value is changed.                     | function(double)        | -        |