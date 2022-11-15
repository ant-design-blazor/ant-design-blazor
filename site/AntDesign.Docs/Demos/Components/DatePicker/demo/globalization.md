---
order: 13
title:
  zh-CN: 国际化
  en-US: Globalization
---

## zh-CN

使用 `Locale` 和 `CultureInfo` 属性，可以自定义日历的区域特性。默认是 `CultureInfo.CuurrentCulture`。

> 注意：内置的本地化配置并未完善，可通过设置 星期的第一天(`FirstDayOfWeek`, 默认 `Sunday` ) 和 星期缩写 ( `ShortWeekDays`，默认 `[ "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" ]` ) 使日历组件展示符合的效果。
> 如果不设置，则会自动使用 [Globalization](https://learn.microsoft.com/zh-cn/dotnet/api/system.globalization?view=net-6.0&WT.mc_id=DT-MVP-5003987) 的默认配置。
> 当然，如果您可以帮助我们完善 [本地化文件](https://github.com/ant-design-blazor/ant-design-blazor/tree/master/components/locales)，我们将不胜感激。


## en-US

We can set the localization with the `Locale` parameter for localization and the `CultureInfo` for handling the parsing and formatting.

> Note: The built-in localization configuration is not perfect. You can set the first day of the week (`FirstDayOfWeek`, default `Sunday`) and the shortest day names of week (`ShortWeekDays`, The default `[" Su ", "Mo", "Tu", "We", "Th", "Fr", "Sa"]`) make the calendar component conforms to the display effect.
> If you do not set this parameter, the default configuration in the [Globalization](https://learn.microsoft.com/zh-cn/dotnet/api/system.globalization?view=net-6.0&WT.mc_id=DT-MVP-5003987) is automatically used.
> Of course, we would appreciate it if you could improve our [localization files](https://github.com/ant-design-blazor/ant-design-blazor/tree/master/components/locales).