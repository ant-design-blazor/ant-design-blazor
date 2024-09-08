---
category: Components
type: Data Display
title: Badge
cover: https://gw.alipayobjects.com/zos/antfincdn/6%26GF9WHwvY/Badge.svg
---

Small numerical value or status descriptor for UI elements.

## When To Use

Badge normally appears in proximity to notifications or user avatars with eye-catching appeal, typically displaying unread messages count.

## API

### Badge

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Color | Customize Badge status dot color. Usage of this parameter will make the badge a status dot. | string | - |  |
| Count | Number to show in badge | int |  |  |
| CountTemplate | Template to show in place of Count| RenderFragment |  |  |
| Dot | Whether to display a red dot instead of `count` | boolean | `false` |  |
| Offset | Set offset of the badge dot, like`[x, y]` | `[number, number]` | - |  |
| OverflowCount | Max count to show | number | 99 |  |
| ShowZero | Whether to show badge when `count` is zero | boolean | `false` |  |
| Status | Set Badge dot to a status color. Usage of this parameter will make the badge a status dot. | `success` \| `processing` \| `default` \| `error` \| `warning` | `''` |  |
| Size | If `count` is set, `size` sets the size of badge | `default` \| `small` | - |  |
| Text | The display text next to the status dot | string | `''` |  |
| Title | Text to show when hovering over the badge | string | `count` |  |


### Badge.Ribbon

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Color | Customize Ribbon color | string | - |  |
| Placement | The placement of the Ribbon  | `start` \| `end` | `end` |  |
| Text | Content inside the Ribbon | String or RenderFragment | - |  |
