---
category: Components
type: Data Display
title: Avatar
cover: https://gw.alipayobjects.com/zos/antfincdn/aBcnbw68hP/Avatar.svg
---

Avatars can be used to represent people or objects. It supports images, `Icon`s, or letters.

## API

### Avatar

| Property | Description | Type | Default |
| --- | --- | --- | --- | --- |
| Shape | Set avatar shape | string | null |
| Size | Set avatar size | `string|AntSizeLDSType{"default","large","small"}` | AntSizeLDSType.Default |
| Src | Set resource address of the avatar image | string |  |
| SrcSet | Set responsive resource address where avatar type is image | string |  |
| Alt | Replace text when the image can't display | string |  |
| Icon | Set avatra icon | string |  |
| Error | Callback function when the image load faile,return false will close fallback action of default | function()=>ErrorEventArgs |  |

### AvaterGroup

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| ChildContent | Additional Content | RenderFragment | - |
| MaxCount | Group max show number  | int | - |
| MaxStyle | Group style when over the max show number | string | - |