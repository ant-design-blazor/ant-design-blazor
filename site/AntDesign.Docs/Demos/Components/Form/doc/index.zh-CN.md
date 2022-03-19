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
### From
| 名称 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Layout | 表单布局 | [FormLayout](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/form/types/FormLayout.cs) | FormLayout.Horizontal |
| LabelCol | label 标签布局，同 \<Col\> 组件，设置 span offset 值 | [ColLayoutParam](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/form/ColLayoutParam.cs) | - |
| LabelColSpan | 等同于LabelCol.Span | int | - |
| LabelColOffset | 等同于LabelCol.Offset | int | - |
| WrapperCol | 需要为输入控件设置布局样式时，使用该属性，用法同 labelCol | [ColLayoutParam](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/form/ColLayoutParam.cs) | - |
| WrapperColSpan | 等同于WrapperCol.Span | int | - |
| WrapperColOffset | 等同于WrapperCol.Offset | int | - |
| Size | 设置字段组件的尺寸（仅限 antd 组件）|  small \| middle \| large | middle |
| Name | 表单名称，会作为表单字段 id 前缀使用 | string | - |
| Model | 操作的泛型对象 | T | - |
| Loading | 表单是否处于加载中 | bool | false |
| OnFinish | 提交事件 | EventCallback\<EditContext\> | - |
| OnFinishFailed | 提交失败(校验失败)回调事件 | EventCallback\<EditContext\> | - |
| ValidateOnChange | 是否在更改时校验 | bool | false |
### FromItem
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
### DataForm

DataForm 为指定的实体类自动生成编辑表单

| 名称 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| TItem | 实体类类型 | Type | - |
| CurrentItem | `TItem` 的实例，界面上的元素将与该对象的属性进行双向绑定 | T | - |
| ColumnsCount | 默认情况下，采用几列来展现各个字段(1-6) | int | 1 |
| ReferenceForm | DataForm 中，实际的 `AntDesign.IForm` 对象，可用于执行提交、校验等操作。| IForm | - |

### DataForm.Annotations

DataForm 支持根据实体类属性上添加的Attributes标签，实现定制化UI元素

| Annotation | 说明 | 类型 |参数 |
| --- | --- | --- | --- |
| 原生 | 系统自带Annotation，包括[Display],[DisplayFormat]以及各类校验Annotation | System.ComponentModel.DataAnnotations.* | - |
| SizeInDataForm | 该属性生成的UI元素在DataForm中的尺寸 | AntDesign.DataAnnotations.SizeInDataFormAttribute | Normal,FullLine,TwoLines,FourLines |
| UIControl | 如果默认生成的UI元素不符合实际需求，可以通过该标签指定UI元素类型 | AntDesign.DataAnnotations.UIControlAttribute | uicontroltype: UI元素类型，需要是ComponentBase的子类；bindproperty：UI元素中一个支持@bind-{bindproperty}形式绑定的属性; extraProperties与extraPropertyValues，可以额外为控件设置属性 |
| DataSourceBind | 对于Select<,>等类型的控件，指定DataSource数据源和LableName、ValueName属性 | AntDesign.DataAnnotations.DataSourceBindAttribute | 数据源类型建议是类中的静态属性，若是非静态属性，将创建一个类的实例 |
| QueryConditionOperator | 在查询模式下，指定相应条件字段使用的运算符，比如等于，包含，大于 等… | AntDesign.DataAnnotations.QueryConditionOperatorAttribute | - |
| AutoGenerateBehavior | 指定该属性在DataForm中的可见性和可用性，注意：当设置[Display(AutoGenerateField=false)]时，该属性无效。 | AntDesign.DataAnnotations.AutoGenerateBehaviorAttribute | DataFormEnabled, DataFormVisibility |


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
