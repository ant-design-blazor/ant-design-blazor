---
order: 4
title: FAQ
---

Here are the frequently asked questions about Ant Design and Blazor that you should look up before you ask in the community or create a new issue. We also maintain a [FAQ issues label](https://github.com/ant-design-blazor/ant-design-blazor/labels/%F0%9F%8C%9F%20Q&A) for common github issues.

If your question is style-related, please refer to the [list of frequently asked questions about Ant Design React](https://ant.design/docs/react/faq-cn)。

---

### `Select Dropdown DatePicker TimePicker Popover Popconfirm` disappears when I click another popup component inside it. How do I resolve this?

This is related to the scrolling area of the overlay, which by default only scrolls with the body.

Use `<Select PopupContainerSelector="#some-scroll-area">` ([API Documentation](https://antblazor.com/zh-CN/components/select#API)) to render the component into the scroll area ( where `#some-scroll-area` is the [CSS Selector](https://developer.mozilla.org/docs/Web/CSS/CSS_Selectors) of the scrolling element).

And make sure that `#some-scroll-area` element is `position: relative` or `position: absolute`.

### How do I modify the default theme of Ant Design?

See: [https://ant.design/docs/react/customize-theme](https://ant.design/docs/react/customize-theme)

### Why does modifying props in mutable way not trigger a component update?

Blazor use shallow compare of props to optimize performance. You should always pass the new object when updating the state. Please ref [React's document](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/components/lifecycle?view=aspnetcore-3.1&WT.mc_id=DT-MVP-5003987#after-parameters-are-set-onparameterssetasync)

### Components are not vertically aligned when placed in single row.

Try [Space](https://antblazor/components/space) component to make them aligned.

### `antd` doesn't work well in mobile.

`antd` is not designed for mobile. There is also no Blazor implementation of AntDesign Mobile in development, so if you are interested in contributing code, please contact us.

### Do you guys have any channel or website for submitting monetary donations, like through PayPal or Alipay?

[https://opencollective.com/ant-design-blazor](https://opencollective.com/ant-design-blazor)

### Why is CSS isolation not working for Ant Design Blazor components

Blazor applies scope attributes only to native HTML elements to enable CSS isolation. [Official documentation](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/components/css-isolation?view=aspnetcore-5.0&WT.mc_id=DT-MVP-5003987#child-component-support)   
This does not include AntDesign Blazor components, thus these components need to be wrapped in a native HTML element to support CSS isolation.

As you can see, the `<div>` is enclosing the `<Layout Class="layout">` which is an AntDesign Blazor component.
```
@page "/test" <--- Blazor
<div> <--- HTML
    <Layout Class="layout"> <--- AntDesign
[...]
```
More explanation in [Zonciu´s comment](https://github.com/ant-design-blazor/ant-design-blazor/issues/732#issuecomment-739125806)

Now you can create a ".razor.css" file for that page and add your CSS for the "layout" class of the Layout AntDesign component.
```
::deep .layout {
    background: #fff;
}
```
The `::deep` prefix has to be used on all the CSS for AntDesign Blazor components using CSS isolation.  
If you get warnings like "`::deep` is not a valid pseudo-element." then you can ignore these. [See the documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/css-isolation?view=aspnetcore-5.0#child-component-support)

---

## Errors and Warnings

Here are some errors & warnings that you may encounter while using AntDesign Blazor, although most of these are not actual bugs of antd itself.

### When binding events of components, a compilation error occurs: CS1503 cannot convert from 'method group' to 'Microsoft.AspNetCore.Components.EventCallback'

This is because the component is generic, and it is necessary to explicitly set the generic type parameters of the component (starting with "T").

```html
<Select TItem="string"
        TItemValue="string"
        DataSource="@_personNames"
        OnSelectedItemChanged="@((personName) => {...}))">
</Select>
```

### Build error for Col components

Since the `Col` component is recognised by VS as a `col` element, you can use `AntDesign.Col` to avoid error.

### Naming conflicts between the Column in the Form component and the Column in AntDesign Charts

It is also possible to use the namespace `AntDesign.Column` or `AntDesign.Charts.Column` to avoid error.
