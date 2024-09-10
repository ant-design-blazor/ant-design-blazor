---
category: Components
type: Data Entry
title: Switch
cover: https://gw.alipayobjects.com/zos/alicdn/zNdJQMhfm/Switch.svg
---

Switching Selector.

## When To Use

- If you need to represent the switching between two states or on-off state.
- The difference between `Switch` and `Checkbox` is that Switch will trigger a state change directly when you toggle it, while Checkbox is generally used for state marking, which should work in conjunction with submit operation.


## API

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| AutoFocus | get focus when component mounted                             | boolean        | false         |
| Checked            | determine whether the Switch is checked         | boolean         |
| CheckedChildren            | content to be shown when the state is checked        | RenderFragment         |-       |
| Control  | determine whether fully control state by user | boolean | false |
| DefaultChecked |to set the initial state     | string        | -         |
| Disabled              |Disable switch       | string        | -        |
| Loading | loading state of switch                             | boolean        | -         |
| Size | the size of the Switch, options: default small      | string         |
| UnCheckedChildren | content to be shown when the state is unchecked                       | RenderFragment        | -         |
| OnChange |trigger when the checked state is changing                            | function(e)        | -         |
| OnClick |trigger when the is clicked. Very useful in combination with `Control` parameter | function(e)        | -         |
