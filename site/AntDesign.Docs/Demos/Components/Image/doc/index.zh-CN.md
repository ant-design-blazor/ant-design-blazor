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

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Alt | 图像描述 | string | - | 0.6.0 |
| Fallback | 加载失败时容错图片的地址 | string | - | 0.6.0 |
| Height | 图像高度 | string | - | 4.6.0 |
| Locale | 语言对象 | ImageLocale | - |- |
| Placeholder | 加载占位 | RenderFragment | - | 0.6.0 |
| Preview | 预览参数，为 `false` 时禁用 | boolean | true | 0.6.0 |
| PreviewSrc | 加载完成前预览图的地址 | string | 与 `Src` 一样 | 0.6.0 |
| Src | 图片地址 | string | - | 0.6.0 |
| Width | 图像宽度 | string | - | 0.6.0 |
