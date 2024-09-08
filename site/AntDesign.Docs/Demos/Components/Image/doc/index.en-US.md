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

### Image

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Alt | Image description | string | - | 0.6.0 |
| Fallback | Load failure fault-tolerant src | string | - | 0.6.0 |
| Height | Image height | string | - | 0.6.0 |
| Locale | Locale Object | ImageLocale | - |- |
| Placeholder | Load placeholder | RenderFragment | - | 0.6.0 |
| Preview | Whether to enable the preview function | boolean  | true | 0.6.0 |
| PreviewSrc | path of the preview image before loading is complete | string | the same as `Src` | 0.6.0 |
| PreviewVisible | Whether to open the preview image when clicking the preview button | true | 0.10.0 |
| Src | Image path | string | - | 0.6.0 |
| Width | Image width | string | - | 0.6.0 |
| OnClick | Triggered when the image is clicked |  EventCallback<MouseEventArgs> | 0.10.0 |

### ImagePreviewGroup

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| PreviewVisible | Whether to open the preview image. Two-way binding. | bool | true | 0.10.0 |