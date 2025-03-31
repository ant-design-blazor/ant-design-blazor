---
order: 100
title:
  zh-CN: 验证模式
  en-US: Validate Mode
---

## zh-CN

表单验证规则支持两种种方式：

- Attribute: 通过在模型类上添加验证特性来添加验证规则
    适用场景：模型类与API共用验证规则，也可自定义 ValidationAttribute，实现自定义验证规则
- Rules: 通过在 FormItem 上的 `Rules` 属性来添加验证规则
    适用场景：需要复用model类，但不同的页面有不同的验证规则，采用Rules模式可以避免重复创建多个类似的model类

## en-US

There are two ways to define validation rules for form items:

- Attribute: Add validation attributes to the model class.
    Use case: Share validation rules between the model class and API. You can also create custom `ValidationAttribute` to implement custom validation rules.
- Rules: Add validation rules to the `Rules` property of the `FormItem`.
    Use case: Reuse the model class, but different pages have different validation rules. Using the `Rules` mode can avoid creating multiple similar model classes.
