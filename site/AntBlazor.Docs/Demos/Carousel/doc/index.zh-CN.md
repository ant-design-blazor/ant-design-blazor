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
| Autoplay   | 是否自动切换 | TimeSpan         |
| BeforeChange    | (ToDo)切换面板的回调 | function(from, to)         |-       |
| DotPosition |	面板指示点位置，可选 `top` `bottom` `left` `right` | string  | -  |
| Dots | (ToDo)是否显示面板指示点，如果为 object 则同时可以指定 dotsClass    | string | -  |
| Easing | (ToDo)动画效果   | string        | -         |
| Effect | 动画效果函数，可取 scrollx, fade | string        | -         |

