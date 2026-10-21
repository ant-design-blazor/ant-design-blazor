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
| ProgressPercent | Show the determinate loading progress placeholder; values are rounded and clamped to 0–100 | `double?` | - | 6.0.0 |
| ProgressRender | Customize the determinate progress placeholder; context exposes normalized percent and default progress UI | `RenderFragment<ImageProgressRenderContext>` | built-in progress | 6.0.0 |
| Preview | Whether to enable the preview function | boolean  | true | 0.6.0 |
| ActionsRender | Customize the preview toolbar action group; context exposes transform state and action callbacks | `RenderFragment<ImagePreviewActionsRenderContext>` | default actions | 6.0.0 |
| ImageRender | Customize preview image content; context includes the default image node and transform/image metadata | `RenderFragment<ImagePreviewImageRenderContext>` | default image | 6.0.0 |
| PreviewMaskVisible | Whether the preview popup mask is visible | bool | true |  |
| PreviewMaskBlur | Whether to blur the background through the preview popup mask | bool | false |  |
| PreviewCover | Custom content rendered over the image thumbnail on hover | RenderFragment | built-in preview icon |  |
| PreviewCoverPlacement | Placement of the custom thumbnail cover: `Center`, `Top`, or `Bottom` | ImageCoverPlacement | Center |  |
| ClassNames | Customize classes for semantic slots `root`, `image`, `cover`, `popup.*`, and progress placeholder `placeholder.progress.*` (`root`, `content`, `rail`, `indicator`) | `IReadOnlyDictionary<string, string>` | - | 6.0.0 |
| Styles | Customize inline styles for semantic slots `root`, `image`, `cover`, `popup.*`, and progress placeholder `placeholder.progress.*` (`root`, `content`, `rail`, `indicator`) | `IReadOnlyDictionary<string, string>` | - | 6.0.0 |
| PreviewSrc | path of the preview image before loading is complete | string | the same as `Src` | 0.6.0 |
| PreviewVisible | Whether to open the preview image when clicking the preview button | true | 0.10.0 |
| Src | Image path | string | - | 0.6.0 |
| Width | Image width | string | - | 0.6.0 |
| OnClick | Triggered when the image is clicked |  EventCallback<MouseEventArgs> | 0.10.0 |

### ImagePreviewGroup

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| PreviewVisible | Whether to open the preview image. Two-way binding. | bool | true | 0.10.0 |
