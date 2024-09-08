---
category: Components
type: Data Display
title: Avatar
cover: https://gw.alipayobjects.com/zos/antfincdn/aBcnbw68hP/Avatar.svg
---

Avatars can be used to represent people or objects. It supports images, `Icon`s, or letters.

## API

### Avatar Props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Alt | This attribute defines the alternative text describing the image | string | - |  |
| Icon | Custom icon type for an icon avatar | string | - |  |
| OnError | Handler when img load error | EventCallback&lt;ErrorEventArgs> | - |  |
| Shape | The shape of avatar | string | - |  |
| Size | The size of the avatar `default` \| `small` \| `large` | string | `default` |  |
| Src | The address of the image for an image avatar | string | - |  |
| SrcSet | A list of sources to use for different screen resolutions | string | - |  |

> Tip: You can set `Icon` or `ChildContent` as the fallback for image load error, with the priority of `Icon` > `ChildContent`

### AvatarGroup Props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| MaxCount | Max avatars to show | int | - |  |
| MaxPopoverPlacement | The placement of excess avatar Popover | `top` \| `bottom` | `top` |  |
| MaxStyle | The style of excess avatar style | string | - |  |
