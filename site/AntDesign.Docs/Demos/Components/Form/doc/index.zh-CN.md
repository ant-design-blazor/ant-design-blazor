---
category: Components
subtitle: 表单
type: 数据录入
cols: 1
title: Form
cover: https://gw.alipayobjects.com/zos/alicdn/ORmcdeaoO/Form.svg
---

高性能表单控件，自带数据域管理。包含数据录入、校验以及对应样式。

## 何时使用

- 用于创建一个实体或收集信息。
- 需要对输入的数据类型进行校验时。

## API
(Help Wanted)


### Rules
Rules 支持接收 FormValidationRule[] 进行配置

| 名称 | 说明 | 类型 |
| --- | --- | --- |
| DefaultField | 仅在 `Type` 为 `Array` 类型时有效，用于指定数组元素的校验规则（FormItem暂不支持） | [FormValidationRule](zh-CN/components/form#Rules) |
| OneOf | 值是否包含在指定的多个值中 | object\[] |
| Fields | 仅在 `Type` 为 `Array` 或 `Object` 类型时有效，用于指定子元素的校验规则（FormItem暂不支持） | Dictionary&lt;object, [FormValidationRule](zh-CN/components/form#Rules)> |
| Len | String 类型时为字符串长度；Number 类型时为确定数字； Array 类型时为数组长度 | decimal |
| Max | 必须设置 `Type`：String 类型为字符串最大长度；Number 类型时为最大值；Array 类型时为数组最大长度 | decimal |
| Message | 错误信息，不设置时会通过ValidateMessages(查看下方)自动生成 | string |
| Min | 必须设置 `Type`：String 类型为字符串最小长度；Number 类型时为最小值；Array 类型时为数组最小长度 | decimal |
| Pattern | 正则表达式匹配 | string |
| Required | 是否为必选字段 | bool |
| Transform | 将字段值转换成目标值后进行校验 | Func&lt;object,object> |
| Type | 类型，常见有 `String` \|`Number` \|`Boolean` \|`Url` \| `Email`。 | RuleFieldType |
| ValidateTrigger | TODO | string \| string\[] |
| Validator | 自定义校验，接收 ValidationResult 作为返回值。[示例](zh-CN/components/form#components-form-demo-dynamic-rule)参考 | Func&lt;FormValidationContext,ValidationResult> |

### FormValidateErrorMessages
(只支持在 Rules 验证模式下使用)

Form 为验证提供了默认的错误提示信息（FormValidateErrorMessages类），你可以通过配置 `ValidateMessages` 属性，修改对应的提示模板。一种常见的使用方式，是配置国际化提示信息：

```csharp
var validateMessages = new FormValidateErrorMessages {
  Required = "'{0}' 是必选字段",
  // ...
};

<Form ValidateMessages="validateMessages" />;
```

此外，[ConfigProvider](Demo TODO) 也提供了全局化配置方案，允许统一配置错误提示模板：

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
