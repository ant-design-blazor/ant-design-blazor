---
category: Components
type: Data Display
title: Segmented
cover: https://gw.alipayobjects.com/zos/bmw-prod/a3ff040f-24ba-43e0-92e9-c845df1612ad.svg
---

Segmented Controls. This component is available since `v0.12.0`.

## When To Use

- When displaying multiple options and user can select a single option;
- When switching the selected option, the content of the associated area changes.

## API

> This component is available since `v0.12.0`

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Block | Option to fit width to its parent\'s width | boolean | false |  |
| DefaultValue | Default selected value | TValue |  |  |
| Disabled | Disable all segments | boolean | false |  |
| Labels | Use a string array as both label and value | string[] |  |  |
| OnChange | The callback function that is triggered when the state changes | EventCallback<TValue> |  |  |
| Options | Set children optional | SegmentedOption[] | [] |  |
| Size | The size of the Segmented. | `large` \| `middle` \| `small` | - |  |
| Value | Currently selected value | TValue |  |  |

