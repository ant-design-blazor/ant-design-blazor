---
category: Components
type: Data Display
title: Statistic
cover: https://gw.alipayobjects.com/zos/antfincdn/rcBNhLBrKbE/Statistic.svg
---

Display statistic number.

## When To Use

- When want to highlight some data.
- When want to display statistic data with description.

## API

#### Statistic

| Property         | Description                   | Type                 | Default | Version |
| ---------------- | ----------------------------- | -------------------- | ------- | ------- |
| CultureInfo      | culture info for the number format | CultureInfo     | CurrentCultureInfo |  |
| DecimalSeparator | decimal separator             | string               | .       |         |
| Formatter        | customize value display logic | (value) => ReactNode | -       |         |
| GroupSeparator   | group separator               | string               | ,       |         |
| Precision        | precision of input value      | number               | -       |         |
| Prefix           | prefix node of value          | string \| ReactNode  | -       |         |
| Suffix           | suffix node of value          | string \| ReactNode  | -       |         |
| Title            | Display title                 | string \| ReactNode  | -       |         |
| Value            | Display value                 | string \| number     | -       |         |
| ValueStyle       | Set value css style           | CSSProperties        | -       |         |

#### Countdown

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Format | Format as [TimeSpan](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/custom-timespan-format-strings?WT.mc_id=DT-MVP-5003987) | string | @"hh\:mm\:ss" |  |
| OnFinish | Trigger when time's up | () => void | - |  |
| Prefix | prefix node of value | string \| ReactNode | - |  |
| Suffix | suffix node of value | string \| ReactNode | - |  |
| Title | Display title | string \| ReactNode | - |  |
| Value | Set target countdown time | number \| moment | - |  |
| ValueStyle | Set value css style | CSSProperties | - |  |
