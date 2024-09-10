---
category: Components
type: Data Entry
title: Checkbox
cover: https://gw.alipayobjects.com/zos/alicdn/8nbVbHEm_/CheckBox.svg
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
| OnChange |The callback function that is triggered when the state changes| function(e)|-     |

Checkbox Group

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| CheckboxItems | Check box items                             | IList<AntCheckbox>        | -         |
| Disable | Disable all checkbox                               | boolean        | false         |
| MixedMode            | Applies only when bot `Options` and `ChildContent` is used. Allows to select which section (`Options` or `ChildContent`) will be rendered first.           | (enum)CheckboxGroupMixedMode         |ChildContentFirst       |
| Options            |Specifies options         | CheckboxOption[]         |-       |
| Value | List of selected value     | IList<string>        | Array.Empty<string>()         |
| ValueChanged |The callback function that is triggered when the state changes| function(checkValue)|-     |


