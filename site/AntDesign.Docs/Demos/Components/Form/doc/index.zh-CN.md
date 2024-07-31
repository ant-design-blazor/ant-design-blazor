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
### Form
| 名称 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Autocomplete | 默认情况下，浏览器是否可以自动完成输入元素的值 | string | on \| off | off |
| Layout | 表单布局 | [FormLayout](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/form/types/FormLayout.cs) | FormLayout.Horizontal |
| LabelCol | label 标签布局，同 \<Col\> 组件，设置 span offset 值 | [ColLayoutParam](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/form/ColLayoutParam.cs) | - |
| LabelColSpan | 等同于LabelCol.Span | int | - |
| LabelColOffset | 等同于LabelCol.Offset | int | - |
| WrapperCol | 需要为输入控件设置布局样式时，使用该属性，用法同 labelCol | [ColLayoutParam](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/form/ColLayoutParam.cs) | - |
| WrapperColSpan | 等同于WrapperCol.Span | int | - |
| WrapperColOffset | 等同于WrapperCol.Offset | int | - |
| Size | 设置字段组件的尺寸（仅限 antd 组件）|  small \| middle \| large | middle |
| Name | 表单名称，会作为表单字段 id 前缀使用。在静态渲染模式中，还作为 [EditForm](https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web/src/Forms/EditForm.cs#L96) 的 `FromName` 属性，指定 Form Handler。 | string | - |
| Model | 操作的泛型对象 | T | - |
| Method | 提交表单的 Http 方法 | string | get |
| Loading | 表单是否处于加载中 | bool | false |
| OnFinish | 提交事件 | EventCallback\<EditContext\> | - |
| OnFinishFailed | 提交失败(校验失败)回调事件 | EventCallback\<EditContext\> | - |
| ValidateOnChange | 是否在更改时校验 | bool | false |
| RequiredMark | 更改必填/可选字段标签在表单上的显示方式。 | FormRequiredMark | FormRequiredMark.Required |


### FormItem
| 名称 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Label | **label** 标签的文本 | string | input组件的Display或者DisplayName特性 |
| LabelTemplate | **label**模板 | RenderFragment  | - |
| LabelCol | **label** 标签布局，同 \<Col\> 组件，设置 span offset 值 | [ColLayoutParam](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/form/ColLayoutParam.cs) | - |
| LabelAlign | **label** 标签文本对齐方式 | left \| right | left |
| LabelColSpan | 同LabelCol.Span | int | - |
| LabelColOffset | 同LabelCol.Offset | int | - |
| WrapperCol |需要为输入控件设置布局样式时，使用该属性，用法同 labelCol | [ColLayoutParam](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/form/ColLayoutParam.cs) | - |
| WrapperColSpan | 同WrapperCol.Span | int | - |
| WrapperColOffset | 同WrapperCol.Offset | int | - |
| NoStyle | 为 true 时不带样式，作为纯字段控件使用 | bool | false |
| Required | 是否为必填项,true会在label后面生成 * 号 | bool | input绑定属性的Required特性 |
| LabelStyle | **Label**标签的Style | string | - |
| Rules | 校验规则,设置字段的校验逻辑 | [FormValidationRule[]](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/form/Validate/Rule/FormValidationRule.cs) | - |
| HasFeedback | 配合 **validateStatus** 属性使用，展示校验状态图标，建议只配合 Input 组件使用 | bool | false |
| ValidateStatus | 校验状态，如不设置，则会根据校验规则自动生成 | [FormValidateStatus](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/form/FormValidateStatus.cs) | FormValidateStatus.Default |
| Help | 提示信息 | string | 根据校验规则自动生成 |
  
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
//in App.Razor
var validateMessages = new FormValidateErrorMessages {
  Required = "'{0}' is Required!",
  // ...
};

var formConfig = new FormConfig {
    ValidateMessages = validateMessages
}

<ConfigProvider Form="formConfig">
  <Router AppAssembly="@typeof(Program).Assembly" PreferExactMatches="@true">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
    </Found>
    <NotFound>
        <LayoutView Layout="@typeof(MainLayout)">
            <p>Sorry, there's nothing at this address.</p>
        </LayoutView>
    </NotFound>
  </Router>
</ConfigProvider>;
```

### GenerateFormItem
可通过 `TItem` 类型自动生成表单元素。当前支持string、DateTime、number、enum类型成员自动生成，支持嵌套类型自动生成。
> **注意:** 此功能正在迭代中，后续本版可能会存在不兼容的变更。

| 名称 | 说明                                                                | 类型                                        | 默认值 |
| --- |-------------------------------------------------------------------|-------------------------------------------|--|
| ValidateRules | 一个单参数范型委托，第一个参数是熟悉PropertyInfo，返回值为FormValidationRule[]?          | Func<PropertyInfo, FormValidationRule[]?> | null |
| Definitions | 一个两参数范型委托，第一个参数是熟悉PropertyInfo，第二个参数是TItem, 返回值为RenderFragment    | Func<PropertyInfo, TItem, RenderFragment>? | null |
| NotGenerate | 一个两参数范型委托，第一个参数是熟悉PropertyInfo，第二个参数是TItem, 返回值为bool，返回true则不自动生成 | Func<PropertyInfo, TItem, bool>?          | null |
| SubformStyle | 嵌套表单风格，默认为Collapse风格，可选Block风格                                    | string                                    | Collapse |


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
