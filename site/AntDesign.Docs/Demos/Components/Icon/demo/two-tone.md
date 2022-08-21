---
order: 2
title:
  zh-CN: 多色图标
  en-US: Two-tone icon and colorful icon
---

## zh-CN

双色图标可以通过 `twoToneColor` 属性设置主题色。

> 注意：由于利用了JS 互操作来计算颜色，因此在预渲染阶段不能加载颜色。如果不希望在预渲染时展示黑色图标，请使用 `AvoidPrerendering`。

## en-US

You can set `twoToneColor` prop to specific primary color for two-tone icons.

> Note: Since we used JS interop to calculate colors, we could not load colors during the pre-render phase. 
Use 'AvoidPrerendering' if you don't want to show black icons during pre-rendering. 