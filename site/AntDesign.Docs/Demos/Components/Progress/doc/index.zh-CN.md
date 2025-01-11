---
category: Components
subtitle: 进度条
type: 反馈
title: Progress
cover: https://gw.alipayobjects.com/zos/alicdn/xqsDu4ZyR/Progress.svg
---

展示操作的当前进度。

## 何时使用

在操作需要较长时间才能完成时，为用户显示该操作的当前进度和状态。

- 当一个操作会打断当前界面，或者需要在后台运行，且耗时可能超过 2 秒时；
- 当需要显示一个操作完成的百分比时。

## API

各类型共用的属性。

| 属性 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Size | - | ProgressSize | `ProgressSize.Default` |
| Type | 类型 | ProgressType | `ProgressType.Line` |
| Format | 内容的模板函数 | Func<double, string> | `percent => percent + "%"` |
| Percent | 百分比 | number | 0 |
| ShowInfo | 是否显示进度数值或状态图标 | boolean | true |
| Status | 状态| ProgressStatus | `ProgressStatus.Normal` |
| StrokeLinecap | - | ProgressStrokeLinecap | `ProgressStrokeLinecap.Round` |
| StrokeColor | 进度条的色彩 | string | - |
| SuccessPercent | 已完成的分段百分比 | number | 0 |
| TrailColor | 未完成的分段的颜色 | string | - |

### `Type="ProgressType.Line"`

| 属性 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| StrokeWidth | 进度条线的宽度，单位 px | number | 10 |
| StrokeColor | 进度条的色彩，传入 object 时为渐变 | string \| { from: string; to: string; direction: string } | - |
| Steps | 进度条总共步数 | number | - |

### `Type="ProgressType.Circle"`

| 属性        | 说明                                             | 类型             | 默认值 |
| ----------- | ------------------------------------------------ | ---------------- | ------ |
| Width       | 圆形进度条画布宽度，单位 px                      | number           | 132    |
| StrokeWidth | 圆形进度条线的宽度，单位是进度条画布宽度的百分比 | number           | 6      |
| StrokeColor | 圆形进度条线的色彩，传入 object 时为渐变         | string \| object | -      |

### `Type="ProgressType.Dashboard"`

| 属性 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Width | 仪表盘进度条画布宽度，单位 px | number | 132 |
| StrokeWidth | 仪表盘进度条线的宽度，单位是进度条画布宽度的百分比 | number | 6 |
| GapDegree | 仪表盘进度条缺口角度，可取值 0 ~ 360 | number | 0 |
| GapPosition | 仪表盘进度条缺口位置 | ProgressGapPosition | `ProgressGapPosition.Top` |
