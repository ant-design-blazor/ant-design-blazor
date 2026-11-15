---
category: Components
type: 数据展示
title: Carousel
subtitle: 走马灯
cover: https://gw.alipayobjects.com/zos/antfincdn/%24C9tmj978R/Carousel.svg
---

旋转木马，一组轮播的区域。

## 何时使用

- 当有一组平级的内容。
- 当内容空间不足时，可以用走马灯的形式进行收纳，进行轮播展现。
- 常用于一组图片或卡片轮播。



## API

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| AfterChange | (ToDo)切换面板的回调 | function(current)        | -         |
| ChildContent | 子内容 | RenderFragment        | -         |
| Autoplay   | 是否自动切换；可通过 `CarouselAutoplayOptions` 配置指示点进度。为兼容早期版本，也可传入 `TimeSpan` | bool 或 CarouselAutoplayOptions 或 TimeSpan | false |
| AutoplaySpeed | 自动切换间隔，单位为毫秒 | int | 3000 |
| Arrows | 是否显示切换箭头 | bool | false |
| BeforeChange    | (ToDo)切换面板的回调 | function(from, to)         |-       |
| DotPlacement | 面板指示点位置，可选 `top`、`bottom`、`start`、`end` | CarouselDotPosition | bottom |
| DotPosition | `DotPlacement` 的旧名称 | CarouselDotPosition | bottom |
| DotDuration | `Autoplay.DotDuration` 的旧式便捷参数 | bool | false |
| Dots | 是否显示面板指示点 | bool | true |
| DotsClass | 面板指示点样式类名 | string        | -         |
| Easing | (ToDo)动画效果   | string        | -         |
| Effect | 动画效果函数，可取 scrollx, fade | string        | -         |
| Infinite | 是否在首尾循环切换 | bool | true |

