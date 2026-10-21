---
category: Components
subtitle: 图片
type: 数据展示
title: Image
cols: 2
cover: https://gw.alipayobjects.com/zos/antfincdn/D1dXz9PZqa/image.svg
---

可预览的图片。

## 何时使用

- 需要展示图片时使用。
- 加载大图时显示 loading 或加载失败时容错处理。

## API

### Image

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Alt | 图像描述 | string | - | 0.6.0 |
| Fallback | 加载失败时容错图片的地址 | string | - | 0.6.0 |
| Height | 图像高度 | string | - | 4.6.0 |
| Locale | 语言对象 | ImageLocale | - |- |
| Placeholder | 加载占位 | RenderFragment | - | 0.6.0 |
| ProgressPercent | 显示确定进度的加载占位符；数值会四舍五入并限制在 0–100 | `double?` | - | 6.0.0 |
| ProgressRender | 自定义确定进度的占位符；上下文提供规范化百分比和默认进度 UI | `RenderFragment<ImageProgressRenderContext>` | 内置进度样式 | 6.0.0 |
| Preview | 设置是否启用预览功能 | bool | true | 0.6.0 |
| ActionsRender | 自定义预览工具栏操作区；上下文包含变换状态和操作回调 | `RenderFragment<ImagePreviewActionsRenderContext>` | 默认操作 | 6.0.0 |
| ImageRender | 自定义预览图片内容；上下文包含默认图片节点和变换/图片信息 | `RenderFragment<ImagePreviewImageRenderContext>` | 默认图片 | 6.0.0 |
| PreviewMaskVisible | 是否显示预览弹层遮罩 | bool | true |  |
| PreviewMaskBlur | 是否模糊预览弹层遮罩后方的内容 | bool | false |  |
| PreviewCover | 鼠标悬停在缩略图上时显示的自定义内容 | RenderFragment | 内置预览图标 |  |
| PreviewCoverPlacement | 自定义缩略图遮罩内容的位置：`Center`、`Top` 或 `Bottom` | ImageCoverPlacement | Center |  |
| ClassNames | 设置 `root`、`image`、`cover`、`popup.*` 以及进度占位符 `placeholder.progress.*`（`root`、`content`、`rail`、`indicator`）槽位的类名 | `IReadOnlyDictionary<string, string>` | - | 6.0.0 |
| Styles | 设置 `root`、`image`、`cover`、`popup.*` 以及进度占位符 `placeholder.progress.*`（`root`、`content`、`rail`、`indicator`）槽位的内联样式 | `IReadOnlyDictionary<string, string>` | - | 6.0.0 |
| PreviewSrc | 加载完成前预览图的地址 | string | 与 `Src` 一样 | 0.6.0 |
| PreviewVisible | 设置是否在点击预览按钮时打开预览框。 | true | 0.10.0 |
| Src | 图片地址 | string | - | 0.6.0 |
| Width | 图像宽度 | string | - | 0.6.0 |
| OnClick | 在点击图片时触发 |  EventCallback<MouseEventArgs> | 0.10.0 |


### ImagePreviewGroup

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| PreviewVisible | 是否打开预览图片。支持双向绑定。 | bool | true | 0.10.0 |
