---
order: 100
title:
  zh-CN: 验证方式
  en-US: Validation
---

## zh-CN

表单支持两种校验方式：

- **Attribute**：在模型属性上添加验证特性。适合复用模型校验规则，也支持自定义 `ValidationAttribute`。
- **Rules**：通过 `FormItem.Rules` 设置规则。适合为不同表单配置不同规则，也支持枚举、可选值和子属性校验。

.NET 11+ 支持 `AsyncValidationAttribute`。Form 走 `UseRulesValidator` 分支时使用内置 rules 校验；异步校验请走 `DataAnnotationsValidator` 分支。异步校验器可接收 `CancellationToken`，示例见 `ValidateMode.razor` 源码。

## en-US

The form supports two validation modes:

- **Attribute**: Add validation attributes to model properties. This works well for reusable model rules and custom `ValidationAttribute` implementations.
- **Rules**: Set rules with `FormItem.Rules`. Use this for form-specific rules, including enum, allowed-value, and child-property validation.

On .NET 11+, `AsyncValidationAttribute` runs through `DataAnnotationsValidator`. The `UseRulesValidator` branch uses built-in rules validation; async validation needs the `DataAnnotationsValidator` branch. Async validators receive a `CancellationToken`; see the `ValidateMode.razor` demo source.
