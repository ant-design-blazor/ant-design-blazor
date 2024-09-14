---
category: Components
type: Data Entry
title: Radio
cover: https://gw.alipayobjects.com/zos/alicdn/8cYb5seNB/Radio.svg
---

Radio

## When To Use

-Used to select a single state from multiple options.
-The difference from Select is that Radio is visible to the user and can facilitate the comparison of choice, which means there shouldn't be too many of them.



## API

Radio/Radio.Button

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| AutoFocus | get focus when component mounted                               | boolean        | -         |
| Checked            | Specifies whether the radio is selected.           | boolean         |
| DefaultChecked            | Specifies the initial state: whether or not the radio is selected.        | boolean         |-       |
| Disabled |		Disable radio        | string        | -         |
| RadioButton | Set to TRUE to style the radio as button group. | bool | false |
| Value              | 	According to value for comparison, to determine whether the selected        | string        | -        |

RadioGroup

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| ButtonStyle            | style type of radio button          | `outline`,`solid`         |-       |
| Disabled |Disable all radio buttons      | string        | -         |
| Value              | Used for setting the currently selected value.        | string        | -        |
| Name            | The name property of all input[type="radio"] children          | boolean         |-       |
| Size |	size for radio button style       | string        | -         |
| onChange              | The callback function that is triggered when the state changes.	     | function(e)        | -        |