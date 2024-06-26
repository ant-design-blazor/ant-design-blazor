---
category: Components
subtitle: 水印
type: 反馈
title: Watermark
cover: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*wr1ISY50SyYAAAAAAAAAAAAADrJ8AQ/original
coverDark: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*duAQQbjHlHQAAAAAAAAAAAAADrJ8AQ/original
cols: 1
---

给页面的某个区域加上水印。

## 何时使用

- 页面需要添加水印标识版权时使用。
- 适用于防止信息盗用。


## API

### Watermark

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Width | 水印的宽度，`content` 的默认值为自身的宽度 | number | 120 |  |
| Height | 水印的高度，`content` 的默认值为自身的高度 | number | 64 |  |
| Rotate | 水印绘制时，旋转的角度，单位 `°` | number | -22 |  |
| ZIndex | 追加的水印元素的 z-index | number | 9 |  |
| Image | 图片源，建议导出 2 倍或 3 倍图，优先级高 (支持 base64 格式) | string | - |  |
| Content | 水印文字内容 | string \| string[] | - |  |
| Contents | 多行水印文字内容 | string[] | - |  |
| FontColor | 字体颜色 | string | rgba(0,0,0,.15) |  |
| FontSize | 字体大小 | number | 16 |  |
| FontWeight | 字体粗细 | `normal` \| `light` \| `weight` \| number | normal |  |
| FontFamily | 字体类型 | string | sans-serif |  |
| FontStyle | 字体样式 | `none` \| `normal` \| `italic` \| `oblique` | normal |  |
| Gap | 水印之间的间距 | \(number, number\) | \(100, 100\) |  |
| Offset | 水印距离容器左上角的偏移量，默认为 `gap/2` | \(number, number\) | \(gap\[0\]/2, gap\[1\]/2\) |  |
| Alpha | 水印整体透明度，取值范围 [0-1] | float | 1 | |
| LineSpace | 行间距，只作用在多行（content 配置为数组）情况下 | int | 16 | |
| Scrolling | 	无限滚动效果 | bool | false |  |
| ScrollingSpeed | 运动位移的间隙，单位：毫秒 | int | 3000 |  |
| Repeat | 是否重复出现 | bool | true | |
| Grayscale | 图片是否需要灰阶显示（彩色变灰色） | bool | false |  |
| ChildContent | 子内容 | RenderFargment | - |  |

