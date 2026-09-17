---
order: 14
additionalFiles:
  - ChineseLunisolarCalendarFormatter.cs
title:
  zh-CN: 自定义日历
  en-US: Custom calendars
---

## zh-CN

通过 `DateCalendar` 传入任意 .NET `Calendar` 实例，以控制日期面板的年月日计算、选择和导航。

`CultureInfo` 仍用于界面文本、星期名称和日期格式。对于不能设置到 `CultureInfo.DateTimeFormat.Calendar` 的 Calendar（例如 `ChineseLunisolarCalendar`），可直接传给 `DateCalendar`。

格式器通过 `CalendarFormatter.Register` 注册。`CultureNames` 可限制其适用语言，例如简体中文使用 `["zh-CN", "zh-SG"]`；其他语言可为同一 Calendar 注册各自的 Formatter。

## en-US

Pass any .NET `Calendar` instance through `DateCalendar` to control the date calculations, selection, and navigation in the picker.

`CultureInfo` still controls UI text, weekday names, and formatting. For calendars that cannot be assigned to `CultureInfo.DateTimeFormat.Calendar`, such as `ChineseLunisolarCalendar`, pass them directly to `DateCalendar`.
