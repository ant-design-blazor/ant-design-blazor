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
| Preview | 设置是否启用预览功能 | bool | true | 0.6.0 |
| PreviewSrc | 加载完成前预览图的地址 | string | 与 `Src` 一样 | 0.6.0 |
| PreviewVisible | 设置是否在点击预览按钮时打开预览框。 | true | 0.10.0 |
| Src | 图片地址 | string | - | 0.6.0 |
| Width | 图像宽度 | string | - | 0.6.0 |
| OnClick | 在点击图片时触发 |  EventCallback<MouseEventArgs> | 0.10.0 |


### ImagePreviewGroup

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| PreviewVisible | 是否打开预览图片。支持双向绑定。 | bool | true | 0.10.0 |