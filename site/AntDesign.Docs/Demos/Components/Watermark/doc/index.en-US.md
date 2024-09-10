---
category: Components
type: Feedback
title: Watermark
cover: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*wr1ISY50SyYAAAAAAAAAAAAADrJ8AQ/original
coverDark: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*duAQQbjHlHQAAAAAAAAAAAAADrJ8AQ/original
cols: 1
---

Add specific text or patterns to the page.

## When To Use

- Use when the page needs to be watermarked to identify the copyright.
- Suitable for preventing information theft.

## API

### Watermark

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Width | The width of the watermark, the default value of `content` is its own width | number | 120 |  |
| Height | The height of the watermark, the default value of `content` is its own height | number | 64 |  |
| Rotate | When the watermark is drawn, the rotation Angle, unit `°` | number | -22 |  |
| ZIndex | The z-index of the appended watermark element | number | 9 |  |
| Image | Image source, it is recommended to export 2x or 3x image, high priority (support base64 format) | string | - |  |
| Content | Watermark text content | string | - |  |
| Contents | Multiple Watermark text contents | string[] | - |  |
| FontColor | font color | string | rgba(0,0,0,.15) |  |
| FontSize | font size | number | 16 |  |
| FontWeight | font weight | `normal` \| `light` \| `weight` \| number | normal |  |
| FontFamily | font family | string | sans-serif |  |
| FontStyle | font style  | `none` \| `normal` \| `italic` \| `oblique` | normal |  |
| Gap | The spacing between watermarks | \(number, number\) | \(100, 100\) |  |
| Offset | The offset of the watermark from the upper left corner of the container. The default is `gap/2` | \(number, number\) | \(gap\[0\]/2, gap\[1\]/2\) |  |
| Alpha | transparency, value range [0-1] | float | 1 | |
| LineSpace | The line spacing, only applies when there are multiple lines (content is configured as an array) | int | 16 | |
| Scrolling | Enable infinite scrolling text effect | bool | false |  |
| ScrollingSpeed | The interval of motion displacement, in milliseconds | int | 3000 |  | 
| Repeat | Repeat or not | bool | true | |
| Grayscale | Whether the picture needs gray scale display (color to gray) | bool | false |  |
| ChildContent |  The child Content  | RenderFargment | - |  |