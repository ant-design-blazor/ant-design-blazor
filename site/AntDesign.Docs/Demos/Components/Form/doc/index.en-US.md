---
category: Components
type: Data Entry
cols: 1
title: Form
cover: https://gw.alipayobjects.com/zos/alicdn/ORmcdeaoO/Form.svg
---

High performance Form component with data scope management. Including data collection, verification, and styles.

## When to use

- When you need to create an instance or collect information.
- When you need to validate fields in certain rules.

## API
(Help Wanted)


### Rules
Rules supports a config FormValidationRule[]

| Name | Description | Type |
| --- | --- | --- |
| DefaultField | Validate rule for all array elements, valid when `type` is `Array`(FormItem not support now) | [FormValidationRule](en-US/components/form#Rule) |
| OneOf | Whether the value is in specified values | object\[] |
| Fields | Validate rule for child elements, valid when `type` is `Array` or `Object`(FormItem not support now) | Dictionary&lt;object, [FormValidationRule](en-US/components/form#Rule)> |
| Len | Length of String, Number, Array | decimal |
| Max | `type` required: max length of `String`, `Number`, `Array` | decimal |
| Message | Error message. Will auto generate by ValidateMessages(see below) if not provided | string |
| Min | `type` required: min length of `String`, `Number`, `Array` | decimal |
| Pattern | Regex pattern | string |
| Required | Required field | bool |
| Transform | Transform value to the rule before validation | Func&lt;object,object> |
| Type | Normally `String` \|`Number` \|`Boolean` \|`Url` \| `Email`. | RuleFieldType |
| ValidateTrigger | TODO | string \| string\[] |
| Validator |Customize validation rule. Accept ValidationResult as return see [example](en-US/components/form#components-form-demo-dynamic-rule) | Func&lt;FormValidationContext,ValidationResult> |


### ValidateMessages
(only support Rules validate mode)

Form provides default verification error messages(FormValidateErrorMessages). You can modify the template by configuring `ValidateMessages` property. A common usage is to configure localization:

```csharp
var validateMessages = new FormValidateErrorMessages {
  Required = "'{0}' is required!",
  // ...
};

<Form ValidateMessages="validateMessages" />;
```

Besides, [ConfigProvider](Demo TODO) also provides a global configuration scheme that allows for uniform configuration error notification templates:

```csharp
var validateMessages = new FormValidateErrorMessages {
  Required = "'{0}' is Required!",
  // ...
};

var formConfig = new FromConfig {
    ValidateMessages = validateMessages
}

<ConfigProvider Form="formConfig">
  <Form />
</ConfigProvider>;
```

<style>
.code-box-demo .ant-form:not(.ant-form-inline):not(.ant-form-vertical) {
  max-width: 600px;
}
.markdown.api-container table td:nth-of-type(4) {
  white-space: nowrap;
  word-wrap: break-word;
}
</style>

<style>
  .site-form-item-icon {
    color: rgba(0, 0, 0, 0.25);
  }
  [data-theme="dark"] .site-form-item-icon {
    color: rgba(255,255,255,.3);
  }
</style>
