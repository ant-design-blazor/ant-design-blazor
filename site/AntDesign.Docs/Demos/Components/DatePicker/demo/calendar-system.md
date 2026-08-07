---
order: 14
title:
  zh-CN: .NET 日历
  en-US: .NET calendars
---

## zh-CN

通过 `DateCalendar` 传入任意 .NET `Calendar` 实例，以控制日期面板的年月日计算、选择和导航。

`CultureInfo` 仍用于界面文本、星期名称和日期格式；`PersianCalendar` 与 `ChineseLunisolarCalendar` 不能设置为 `CultureInfo.DateTimeFormat.Calendar`，因此需要直接传给 `DateCalendar`。

## en-US

Pass any .NET `Calendar` instance through `DateCalendar` to control the date calculations, selection, and navigation in the picker.

`CultureInfo` still controls UI text, weekday names, and formatting. `PersianCalendar` and `ChineseLunisolarCalendar` cannot be assigned to `CultureInfo.DateTimeFormat.Calendar`, so pass them directly to `DateCalendar`.
