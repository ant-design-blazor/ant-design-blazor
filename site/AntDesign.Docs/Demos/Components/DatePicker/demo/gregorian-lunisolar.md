---
order: 15
additionalFiles:
  - ChineseLunisolarCalendarFormatter.cs
title:
  zh-CN: 公历和农历
  en-US: Gregorian and lunar dates
---

## zh-CN

通过 `DateRender` 自定义日期格：大字展示公历日期，小字展示由 `ChineseLunisolarCalendar` 计算的农历日期。仅在农历初一显示月份。示例显式使用中文 `CultureInfo` 和中文星期文本，选择和导航仍使用公历。

## en-US

Use `DateRender` to display the Gregorian date prominently and the corresponding `ChineseLunisolarCalendar` date underneath. Selection and navigation remain Gregorian.
