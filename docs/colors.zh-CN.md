---
order: 7.1
title: 色彩
---

Ant Design 将色彩体系分为系统级和产品级。系统级定义基础、中性和数据可视化色板；
产品级基于这些颜色表达品牌特征及功能状态。本页将 React v6 色彩示例同步为可交互的 Blazor 组件。

## 色彩模型

HSB 模型便于预测色相、饱和度和亮度调整后的效果，也方便团队沟通。

## 基础色板

交互页面显示完整 120 色：薄暮、火山、日暮、青柠、金盏花、日出、极光绿、明青、
拂晓蓝、极客蓝、酱紫、法式洋红，每组十档。悬停查看色值，点击或通过键盘激活色块复制。

顺序、中英文名称及说明通过 CLI 从 React `ColorPalettes.tsx` 同步；120 个色值由共享 C#
色板算法生成，而不是手工复制维护。

## 中性色板

与 React 色彩页一致，默认展示前十档：`#fff`、`#fafafa`、`#f5f5f5`、`#f0f0f0`、
`#d9d9d9`、`#bfbfbf`、`#8c8c8c`、`#595959`、`#434343`、`#262626`。
同步目录也保留源码中的其余三档定义（`#1f1f1f`、`#141414`、`#000`），默认展示数量
从 React `Palette.tsx` 提取。这是设计规范的中性色条，不是生成式灰色色板。

## 数据可视化色板

结合基础和中性色板，遵循 AntV「有效、清晰、准确、美」的原则。
[查看色板](https://antv.antgroup.com/specification/language/palette)。

## 色板生成工具

页面使用真实的 Blazor `ColorPicker`。默认主色为 `#1890ff`，暗色工具还支持修改默认的
`#141414` 背景。两个工具调用 `ColorPalette.Generate`，按顺序保留不重复的色值。
修改颜色后，如果饱和度或亮度低于 70%，会显示建议。

## 在代码中使用色板

```csharp
using AntDesign.Colors;

var blue = ColorPalette.Generate("#1677ff"); // 完整十档
var primary = blue[5];
var secondary = blue[0];
var dark = ColorPalette.Generate("#1677ff", dark: true, backgroundColor: "#141414");
```

这与主题 Token 和双色 Icon 共用同一 C# 实现，迁移自 Ant v6 的 `@ant-design/colors`。
颜色计算不依赖 JavaScript；仅浏览器剪贴板操作使用 JS。

## 产品级色彩体系

### 品牌色

推荐使用色板第六档作为主色。Ant Design 品牌色是 `#1677ff`，用于关键行动点、重要信息高亮及状态反馈。

![品牌色](https://gw.alipayobjects.com/mdn/rms_08e378/afts/img/A*1c74TKxuEW4AAAAAAAAAAABkARQnAQ)

### 功能色

成功、错误、提醒及链接应使用一致的功能色，符合用户认知。

![功能色](https://gw.alipayobjects.com/mdn/rms_08e378/afts/img/A*QY4JRa92gHQAAAAAAAAAAABkARQnAQ)

### 中性色

页面对比 C# 默认与暗色主题的真实 Token 值：`colorTextHeading`、`colorText`、
`colorTextSecondary`、`colorTextDisabled`、`colorBorder`、`colorSplit`、`colorBgLayout`，
半透明颜色分别显示在明暗背景上。

## 企业级产品设计中的色彩应用

克制地使用色彩，优先服务信息传递、操作引导及交互反馈，避免降低清晰度和效率。
