---
order: 6
title:
  zh-CN: 动态双色图标
  en-US: Dynamic two-tone color
---

## zh-CN

Blazor 扩展示例：左侧为生成式组件，右侧为兼容的 `Icon` 组件。两者只设置主色，次色均由 C# 色板算法的第一档浅色（`generate(primary)[0]`）自动计算。点击按钮切换主色，无需 JavaScript，首次渲染与服务端预渲染也会直接得到正确的双色。

## en-US

An additional Blazor example: the generated component (left) and compatibility `Icon` (right) both receive only a primary color. C# computes the secondary color as the first light palette swatch (`generate(primary)[0]`), without JavaScript, including on the first render and during server prerendering. Click the button to change the primary color.
