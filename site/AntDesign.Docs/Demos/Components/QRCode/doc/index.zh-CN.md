---
category: Components
subtitle: 二维码
type: 数据展示
title: QRCode
---

QRCode 将文本内容渲染为二维码。

## API

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Value | 二维码内容 | string | - |
| Size | 二维码尺寸 | int | 160 |
| Status | 当前状态 | string | active |
| Color | HEX 格式的前景色 | string | `#000000` |
| BackgroundColor | HEX 格式的背景色 | string | `#ffffff` |
| StatusRender | 非 active 状态下显示的自定义内容 | RenderFragment | — |
| Bordered | 是否显示边框 | bool | true |
