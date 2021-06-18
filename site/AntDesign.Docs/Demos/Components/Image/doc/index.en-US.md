---
category: Components
type: Data Display
title: Image
cols: 2
cover: https://gw.alipayobjects.com/zos/antfincdn/D1dXz9PZqa/image.svg
---

Previewable image.

## When To Use

- When you need to display pictures.
- Display when loading a large image or fault tolerant handling when loading fail.

## API

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Alt | Image description | string | - | 0.6.0 |
| Fallback | Load failure fault-tolerant src | string | - | 0.6.0 |
| Height | Image height | string | - | 0.6.0 |
| Locale | Locale Object | ImageLocale | - |- |
| Placeholder | Load placeholder | RenderFragment | - | 0.6.0 |
| Preview | preview config, disabled when `false` | boolean  | true | 0.6.0 |
| PreviewSrc | path of the preview image before loading is complete | string | the same as `Src` | 0.6.0 |
| Src | Image path | string | - | 0.6.0 |
| Width | Image width | string | - | 0.6.0 |

