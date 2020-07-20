---
order: 6
title:
  zh-CN: 复杂一点的控件
  en-US: complex form control
---

## zh-CN

这里演示 `FormItem` 内有多个元素的使用方式。

这里展示了三种典型场景：

- `Username`：输入框后面有描述文案或其他组件，在 `FormItem` 内只有使用了@bind-Value的组件才会绑定到FormItem，其他组件可任意添加。
- `Address`：有两个控件，在 `FormItem` 内使用两个 `<FormItem NoStyle />` 分别绑定对应控件（一个FormItem下只能出现一个使用了@bing-Value的控件），对FormItem使用NoStyle，则FormItem的Grid布局会被忽略，就算主动使用了LabelCol或WrapperCol也不会产生效果。

  这个场景还展示了复杂类型的表单验证，Address是一个包含了两个属性的类结构，通过附加ValidateComplexType，表单可以对其所有属性进行验证。详情可参考Blazor文档：[嵌套模型、集合类型和复杂类型](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/forms-validation)
- `BirthDate`：有两个内联控件，错误信息展示各自控件下，使用两个 `<FormItem />` 分别绑定对应控件，并修改 `style` 使其内联布局。

更复杂的封装复用方式可以参考下面的 `自定义表单控件` 。

## en-US

Help Wanted