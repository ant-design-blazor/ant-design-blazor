---
order: 7.1
title:
  zh-CN: 本地化
  en-US: Localization
---

## zh-CN

默认会尝试从内置的全球化资源文件中获取验证信息模板，但希望修改模板时，也可用现有方式修改。
另外，也支持 Blazor 的表单 [数据声明特性（DataAnnotations）的本地化方式](https://learn.microsoft.com/zh-CN/aspnet/core/fundamentals/localization/make-content-localizable?view=aspnetcore-8.0&WT.mc_id=DT-MVP-5003987#dataannotations-localization)。
当 `FormItem.Label` 未指定时，会自动从 `DisplayAttribute` 或 `DisplayNameAttribute` 获取。

## en-US


Default will try to get the error message templates from the built-in globalization resource file, but if you want to modify the template, you can also modify it in the existing way.

Otherwise, also supports the [DataAnnotations localization](https://learn.microsoft.com/zh-CN/aspnet/core/fundamentals/localization/make-content-localizable?view=aspnetcore-8.0&WT.mc_id=DT-MVP-5003987#dataannotations-localization) of Blazor forms. 
When `FormItem.Label` is not specified, it is automatically obtained from `DisplayAttribute` or `DisplayNameAttribute`.