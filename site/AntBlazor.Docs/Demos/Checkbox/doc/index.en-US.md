---
category: Components
type: Data Entry
title: Checkbox
---

Checkbox component.

## When To Use

- Used for selecting multiple values from several options.
- If you use only one checkbox, it is the same as using Switch to toggle between two states. 
The difference is that Switch will trigger the state change directly, but Checkbox just marks the state as changed and this needs to be submitted.


## API

Checkbox

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| AutoFocus | get focus when component mounted                             | boolean        | false         |
| Checked            | Specifies whether the checkbox is selected.           | boolean         |false|
| Disabled            | Disable checkbox           | boolean         |false       |
| Indeterminate |indeterminate checked state of checkbox       | boolean        | false         |
| CheckedChange | The callback function that is triggered when the state changes| function(e)|-     |

Checkbox Group

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| CheckboxItems | Check box items                             | IList<AntCheckbox>        | -         |
| Disable | Disable all checkbox                               | boolean        | false         |
| Options            |Specifies options         | CheckBoxOption[]         |-       |
| Value | List of selected value     | IList<string>        | Array.Empty<string>()         |
| ValueChanged |  The callback function that is triggered when the state changes| function(checkValue)|-     |


