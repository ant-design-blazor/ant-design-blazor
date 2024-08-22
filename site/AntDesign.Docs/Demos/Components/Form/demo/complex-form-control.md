---
order: 6
title:
  zh-CN: 复杂一点的控件
  en-US: Complex form control
---

## zh-CN

这里演示 `FormItem` 内有多个元素的使用方式。

这里展示了三种典型场景：

- `Username`：输入框后面有描述文案或其他组件，在 `FormItem` 内只有使用了@bind-Value的组件才会绑定到FormItem，其他组件可任意添加。
- `Address`：有两个控件，在 `FormItem` 内使用两个 `<FormItem NoStyle />` 分别绑定对应控件（一个FormItem下只能出现一个使用了@bing-Value的控件），对FormItem使用NoStyle，则FormItem的Grid布局会被忽略，就算主动使用了LabelCol或WrapperCol也不会产生效果。

  这个场景还展示了复杂类型的表单验证，Address 属性是一个包含了两个属性的类型，当级联表单项绑定了它的属性时，会继承这个属性的所有 Attributes，但由于表单未绑定这个属性，外部表单无法获取 Required 和 Label。

- `BirthDate`：有两个内联控件，错误信息展示各自控件下，使用两个 `<FormItem />` 分别绑定对应控件，并修改 `style` 使其内联布局。

更复杂的封装复用方式可以参考下面的 `自定义表单控件` 。

## en-US

This demonstrates the use of multiple elements within a `FormItem`.

Three typical scenarios are shown here.

- `Username`: there is a description text or other component behind the input box, within `FormItem` only the component that uses @bind-Value will be bound to the FormItem, other components can be added at will.
- `Address`: there are two controls, use two `<FormItem NoStyle />` within the `FormItem` to bind the corresponding controls separately (only one control with @bind-Value can appear under a FormItem), use NoStyle for the FormItem, then The Grid layout of the FormItem will be ignored, even if LabelCol or WrapperCol is actively used.

The scenario also shows complex types of form validation. The Address property is a type that contains two properties. When a cascading form entry binds its property, it inherits all attributes of that property, but because the form does not bind this property, the external form cannot obtain Required and Label.

- `BirthDate`: there are two inline controls with error messages displayed under each control, using two `<FormItem />`s to bind the corresponding controls separately, and modifying the `style` to make the layout inline.

For a more complex way of wrapping and reusing controls see `Custom Form Controls` below.