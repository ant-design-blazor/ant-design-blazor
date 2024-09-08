---
category: Components
subtitle: 统计数值
type: 数据展示
title: Statistic
cover: https://gw.alipayobjects.com/zos/antfincdn/rcBNhLBrKbE/Statistic.svg
---

展示统计数值。

## 何时使用

- 当需要突出某个或某组数字时。
- 当需要展示带描述的统计类数据时使用。

## API

#### Statistic

| 参数             | 说明             | 类型                         | 默认值 | 版本 |
| ---------------- | ---------------- | ---------------------------- | ------ | ---- |
| CultureInfo      | 用来获取数字格式的 CultureInfo 类 | CultureInfo     | CurrentCultureInfo |  |
| DecimalSeparator | 设置小数点       | string                       | .      |      |
| GroupSeparator   | 设置千分位标识符 | string                       | ,      |      |
| Precision        | 数值精度         | int                          | -      |      |
| Prefix           | 设置数值的前缀   | string\|RenderFragment       | -      |      |
| Suffix           | 设置数值的后缀   | string\|RenderFragment       | -      |      |
| Title            | 数值的标题       | string\|RenderFragment       | -      |      |
| Value            | 数值内容         | decimal\|Datetime\|TimeSpan  | -      |      |
| ValueStyle       | 设置数值的样式   | string                       | -      |      |

#### Statistic.Countdown

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Format | 格式化倒计时展示，参考 [TimeSpan](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/custom-timespan-format-strings?WT.mc_id=DT-MVP-5003987) | string | @"hh\:mm\:ss" |  |
| OnFinish | 倒计时完成时触发 | () => void | - |  |
| Prefix | 设置数值的前缀 | string \| RenderFragment | - |  |
| Suffix | 设置数值的后缀 | string \| RenderFragment | - |  |
| Title | 数值的标题 | string \| RenderFragment | - |  |
| Value | 数值内容 | Datetime \| TimeSpan | - |  |
| ValueStyle | 设置数值的样式 | string | - |  |
