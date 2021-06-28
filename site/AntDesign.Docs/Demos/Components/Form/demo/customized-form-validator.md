---
order: 7
title:
  zh-CN: 自定义表单验证器
  en-US: Customized form validator
---

## zh-CN

使用Form Validator属性或`<Validator>`元素设置自定义验证器，会覆盖内置验证器。当不需要验证时，可以设置为 `null` 来提高表单的性能。

注意：使用`<Validator>`元素设置验证器时，需要将Form内其他组件用`<ChildContent>`包起来。

自定义验证组件开发见文档：[ASP.NET Core Blazor 窗体和验证 - 验证器组件](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/forms-validation?view=aspnetcore-5.0&WT.mc_id=DT-MVP-5003987#validator-components)

## en-US

Setting a custom validator using the Form Validator property or the `<Validator>` element overrides the built-in validator. When validation is not required, it can be set to NULL to improve performance.

Note: When using the `<Validator>` element to set the validator, you need to wrap the other components within the Form with `<ChildContent>`.

For custom validator development, see document: [ASP.NET Core Blazor forms and validation - Validator components](https://docs.microsoft.com/en-US/aspnet/core/blazor/forms-validation?view=aspnetcore-5.0&WT.mc_id=DT-MVP-5003987#validator-components)
