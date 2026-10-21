---
category: Components
subtitle: 颜色选择器
type: 数据录入
title: ColorPicker
---

颜色选择器用于选择颜色，并支持受控的打开状态和值变化。

## 何时使用

当需要让用户选择颜色或预览主题色时使用。

## API

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Value | 当前颜色值 | string | `#1677ff` |
| ValueChanged | 颜色变化时触发；使用 `@bind-Value` 进行双向绑定 | `EventCallback<string>` | — |
| ChildContent | 可选的自定义触发器内容，替代默认色块 | `RenderFragment` | — |
| Size | 触发器尺寸：`small`、`middle` 或 `large` | string | `middle` |
| Open | 是否打开面板 | bool | false |
| ShowText | 是否显示颜色文本 | bool | false |
| Disabled | 是否禁用 | bool | false |
| AllowClear | 是否允许清除已选颜色 | bool | false |
| Presets | 带标签的预设颜色组 | `IReadOnlyList<ColorPickerPreset>` | 空 |
| OnClear | 清除颜色时触发的回调 | `EventCallback` | — |
| Format | 颜色输入格式：`hex`、`rgb` 或 `hsb` | string | `hex` |
| FormatChanged | 输入格式变化时触发 | `EventCallback<string>` | — |
| DisabledAlpha | 隐藏透明度滑块 | bool | false |
| DisabledFormat | 隐藏格式选择器 | bool | false |
| AriaLabel | 颜色按钮的无障碍标签 | string | Choose color |
