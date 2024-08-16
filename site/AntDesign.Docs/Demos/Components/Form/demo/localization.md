---
order: 7.1
title:
  zh-CN: 本地化
  en-US: Localization
---

## zh-CN

默认会尝试从内置的全球化资源文件中获取验证信息模板，这个语言包可以通过 Locale 统一替换。也可通过 数据声明特性 的 `ErrorMessage` 、Rules 的 `Message` 来修改。

另外，也支持 [数据声明特性（DataAnnotations）的本地化方式](https://learn.microsoft.com/zh-CN/aspnet/core/fundamentals/localization/make-content-localizable?view=aspnetcore-8.0&WT.mc_id=DT-MVP-5003987#dataannotations-localization)。
当 `FormItem.Label` 未指定时，会自动从 `DisplayAttribute` 或 `DisplayNameAttribute` 获取。

## en-US

Default will try to get the error message templates from the built-in globalization resource file,, which can be replaced by the `Locale` parameter.
It can also be modified by the `ErrorMessage` of the data declaration feature and the `Message` of the Rules parameter.

Otherwise, also supports the [DataAnnotations localization](https://learn.microsoft.com/zh-CN/aspnet/core/fundamentals/localization/make-content-localizable?view=aspnetcore-8.0&WT.mc_id=DT-MVP-5003987#dataannotations-localization) of Blazor forms. 
When `FormItem.Label` is not specified, it is automatically obtained from `DisplayAttribute` or `DisplayNameAttribute`.