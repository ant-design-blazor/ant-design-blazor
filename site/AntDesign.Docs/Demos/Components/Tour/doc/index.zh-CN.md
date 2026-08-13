---
category: Components
subtitle: 漫游式引导
type: 数据展示
title: Tour
cover: https://gw.alipayobjects.com/zos/alicdn/RT_USzA48/DatePicker.svg
---

用于分步引导用户了解产品功能的气泡式说明。

## 何时使用

当页面中有较多新功能，或需要向用户介绍复杂操作流程时，可以使用 Tour 将步骤和页面元素关联起来。

## 代码演示

基础、非模态、位置、自定义遮罩、自定义指示器、自定义高亮区域、自定义操作按钮和自定义样式均与 React Tour 示例对应。

## API

### Tour

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Steps | 引导步骤 | `IReadOnlyList<TourStep>` | `[]` |
| Open | 是否显示引导 | `bool` | `false` |
| Current | 当前步骤，从 0 开始 | `int` | `0` |
| Mask | 是否显示遮罩 | `bool` | `true` |
| MaskColor / MaskStyle | 遮罩颜色和样式 | `string` | `rgba(0,0,0,0.5)` / - |
| MaskClosable | 点击遮罩是否关闭 | `bool` | `true` |
| Keyboard | 是否支持 Esc 关闭 | `bool` | `true` |
| DisabledInteraction | 是否禁止与目标元素交互 | `bool` | `false` |
| Type | 引导样式，可选 `Default`、`Primary` | `TourType` | `Default` |
| GapOffset / GapOffsetX / GapOffsetY / GapRadius | 高亮区域与目标的间距和圆角；可使用统一间距或分别设置水平、垂直间距 | `int` / `int?` | `6` / - / - / `2` |
| IndicatorsRender | 自定义步骤指示器 | `RenderFragment<(int Current, int Total)>` | - |
| ActionsRender | 自定义操作区域，`Origin` 为默认按钮 | `RenderFragment<TourActionsRenderContext>` | - |
| ZIndex | Tour 层级 | `int` | `1070` |
| OnChange | 切换步骤回调 | `EventCallback<int>` | - |
| OnClose | 关闭回调 | `EventCallback` | - |
| OnFinish | 完成回调 | `EventCallback` | - |

### TourStep

| 参数 | 说明 | 类型 |
| --- | --- | --- |
| Target | 目标元素的 `ElementReference` 或 CSS 选择器字符串，例如 `#save-button`；与 `TargetSelector` 二选一 | `ElementReference` / `string` |
| TargetSelector | 目标元素的 CSS 选择器，例如 `#save-button`；设置后优先于 `Target` | `string` |
| Title | 步骤标题 | `RenderFragment` |
| Description | 步骤描述 | `RenderFragment` |
| Cover | 顶部插图或自定义内容 | `RenderFragment` |
| Placement | 气泡位置 | `Placement` |
| Mask | 覆盖当前步骤的遮罩设置 | `bool?` |
| MaskColor / MaskStyle | 当前步骤的遮罩颜色和样式 | `string` |
| Closable | 是否显示关闭按钮 | `bool?` |
| NextText / PreviousText / FinishText | 步骤操作按钮文字 | `string` |
