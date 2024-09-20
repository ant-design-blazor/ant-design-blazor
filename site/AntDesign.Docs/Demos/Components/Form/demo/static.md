---
order: 1
title:
  zh-CN: 静态表单
  en-US: Static Form
---

## zh-CN

Form 支持 .NET 8 静态服务端渲染的表单验证，但有如下限制：

1. 必须设置 Form 的 `Name` 属性, 另外也支持设置 `Method` 和 `Enhance` (分别对应 EditForm 的 `FormName`、`Method`、`Enhance`)。
2. 表单组件的属性绑定，必须要用 `Model.Field`, 不能用 `@context.Field`。
3. 绑定的 Model 实例，需要标注 [SupplyParameterFromForm] 特性，当有多个表单时，还需要指定 `FormName` 参数与 Form 的 `Name` 一致。

注意：
1. 由于在静态页面失去交互性，支持的输入组件仅限 Input、InputPassword、TextArea、Checkbox、Radio 以及使用 InputFile 的 Upload。其他的输入组件请使用原生 html 元素代替，也欢迎贡献。
2.由于Form组件内部封装了EditForm，因此[防伪标记](https://learn.microsoft.com/zh-cn/aspnet/core/blazor/forms/?view=aspnetcore-8.0#antiforgery-support) 会默认设置，如需取消请参考文档。


## en-US

Form supports .NET 8's static server-side rendered form validation with the following limitations:

1. The `Name` parameter of Form component must be set. `Method` and `Enhance` can also be set. (corresponding to the EditForm's `FormName`, `Method`, and `Enhance`)
2. The property binding of the Form component must use `Model.Field`, not `@context.Field`.
3. The bound Model instance needs to be marked with the `[SupplyParameterFromForm]` feature. If there are multiple forms, the `FormName` parameter must be consistent with the `Name` of the Form.

Notice:

1. Due to the loss of interactivity in static pages, the supported components are limited to Input, InputPassword, TextArea, Checkbox, Radio, and Upload using InputFile. For other input components, please use native html elements instead, and contributions are also welcome.
2. Since the EditForm is encapsulated inside the Form component, So the [Antiforgery](https://learn.microsoft.com/zh-cn/aspnet/core/blazor/forms/?view=aspnetcore-8.0#antiforgery-support) will be the default enabled, To cancel, please refer to the documentation.