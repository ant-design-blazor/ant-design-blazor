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
| Type | 渲染类型：`canvas` 或 `svg` | string | `canvas` |
| Size | 二维码尺寸 | int | 160 |
| Icon | 中心图片的 URL | string | - |
| IconSize | 图片边长或独立的宽高，单位为像素 | OneOf\<int, QRCodeIconSize\> | 40 |
| ErrorLevel | 最低纠错等级：`L`、`M`、`Q`、`H` | string | `M` |
| BoostLevel | 在二维码版本不变时自动提升纠错等级 | bool | true |
| MarginSize | 以模块为单位的静区宽度，零表示不留白 | int | 0 |
| Status | 当前状态 | string | active |
| Color | 前景 CSS 颜色 | string | 主题文字颜色 |
| BackgroundColor | 背景 CSS 颜色 | string | `transparent` |
| StatusRender | 非 active 状态下显示的自定义内容 | RenderFragment | — |
| StatusTemplate | 获取当前状态、语言文本和刷新回调的状态模板 | RenderFragment\<QRCodeStatusRenderInfo\> | - |
| Bordered | 是否显示边框 | bool | true |
| OnRefresh | 点击刷新按钮时触发，提供此回调后显示刷新按钮 | EventCallback | - |
| Locale | 状态文字的语言配置 | QRCodeLocale | 当前语言 |
| ClassNames | 以 `root`、`cover` 为键的自定义样式类 | IReadOnlyDictionary\<string, string\> | - |
| Styles | 以 `root`、`cover` 为键的自定义行内样式 | IReadOnlyDictionary\<string, string\> | - |

### 方法

`DownloadAsync(string fileName = null)` 将当前 Canvas 下载为 PNG，或将当前 SVG 下载为 SVG。默认文件名为 `QRCode.png`、`QRCode.svg`。`StatusTemplate` 的优先级高于 `StatusRender`，两种状态模板都不会移除下方的二维码。
