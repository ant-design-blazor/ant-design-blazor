---
order: 100
title:
  zh-CN: 验证方式
  en-US: Validation
---

## zh-CN

表单验证同时支持两种方式：

- **Attribute**: 通过在模型类上添加验证特性来添加验证规则
    
    **适用场景**：
    - 模型类与 API 共用验证规则，与[服务端模型验证](https://learn.microsoft.com/aspnet/core/mvc/models/validation?view=aspnetcore-9.0)行为一致
    - 支持自定义 ValidationAttribute，实现自定义验证规则

- **Rules**: 通过在 FormItem 上的 `Rules` 属性来添加验证规则
    
    **适用场景**：
    - 需要复用 model 类，但不同的表单有不同的验证规则时，可以避免重复创建多个类似的 model 类
    - `FormValidationRule` 类型可定义更加丰富和灵活的验证规则，如限定枚举值、限定可选值、子属性验证等，也支持指定 ValidationAttribute

## en-US

The form supports two validation modes:

- **Attribute**: Add validation rules by decorating the model class with validation attributes.

    **Scenarios:**
    - When you want to share validation rules between the model and APIs, consistent with [server-side model validation](https://learn.microsoft.com/aspnet/core/mvc/models/validation?view=aspnetcore-9.0).
    - When you need to use or implement custom `ValidationAttribute` for custom validation logic.

- **Rules**: Add validation rules via the `Rules` property on `FormItem`.

    **Scenarios:**
    - When you want to reuse a model class but need different validation rules for different forms, avoiding the need to create multiple similar model classes.
    - The `FormValidationRule` type allows for more flexible and rich validation, such as limiting enum values, specifying allowed values, validating child properties, or using a specific `ValidationAttribute`.
