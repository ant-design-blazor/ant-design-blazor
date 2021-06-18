---
order: 11
title: 常见问题
---

以下整理了一些 Ant Design Blazor 社区常见的问题和官方答复，在提问之前建议找找有没有类似的问题。此外我们也维护了一个反馈较多 [FAQ issues 标签](https://github.com/ant-design-blazor/ant-design-blazor/labels/%F0%9F%8C%9F%20Q&A) 亦可参考。

如果你的问题与样式有关，请参考 [Ant Design 的常见问题列表](https://ant.design/docs/react/faq-cn).

---

### `Select Dropdown DatePicker TimePicker Popover Popconfirm` 会跟随滚动条上下移动？

这与浮层（overlay）的滚动区域有关，浮层默认只跟随 body 滚动。

使用 `<Select PopupContainerSelector="#some-scroll-area">`（[API 文档](https://antblazor.com/zh-CN/components/select#API)）来将组件渲染到滚动区域内（其中 `#some-scroll-area` 是滚动元素的 [CSS Selector](https://developer.mozilla.org/docs/Web/CSS/CSS_Selectors)）。

并且保证 `#some-scroll-area` 对应的元素是 `position: relative` 或 `position: absolute`。

### 如何修改 Ant Design 的默认主题？

可以参考[定制主题](/docs/customize-theme)。

### 如何修改 Ant Design 组件的默认样式？

你可以覆盖它们的样式，但是我们不推荐这么做。antd 是一系列 React 组件，但同样是一套设计规范。

### 为什么修改组件传入的对象或数组属性组件不会更新？

Blazor 内部会对 Parameters 进行浅比较实现性能优化。当状态变更，你总是应该传递一个新的对象。具体请参考 [Blazor 的文档](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/components/lifecycle?view=aspnetcore-3.1&WT.mc_id=DT-MVP-5003987#after-parameters-are-set-onparameterssetasync)

### 当我设置了 `Input`/`Select` 等的 `value` 时它就无法修改了。

尝试使用 @bind-Value。

### 多个组件放一排时没有垂直对齐怎么办？

尝试使用 [Space](https://antblazor.com/components/space) 组件来使他们对齐。

### AntDesign 覆盖了我的全局样式！

是的，antd 在设计的时候就是用来开发一个完整的应用的，为了方便，我们覆盖了一些全局样式，现在还不能移除，想要了解更多请追踪 [这个 issue](https://github.com/ant-design/ant-design/issues/4331)，或者参考这个教程 [How to avoid modifying global styles?](/docs/react/customize-theme#How-to-avoid-modifying-global-styles)

### AntDesign 在移动端体验不佳。

`antd` 并非针对移动端设计。也没有开发 AntDesign Mobile 的 Blazor 实现，如果有意向贡献代码，请联系我们。

### 你们有接受捐助的渠道吗，比如支付宝或者微信支付？

[https://opencollective.com/ant-design-blazor](https://opencollective.com/ant-design-blazor)

---

## 错误和警告

这里是一些你在使用 antd 的过程中可能会遇到的错误和警告，但是其中一些并不是 antd 的 bug。

### Col 组件的告警

由于 `Col` 组件会被 VS 识别为 `col` 元素，所以可以使用 `AntDesign.Col` 来避免告警。

### 表格组件中的 Column 与 AntDesign Charts 里的 Column 命名冲突

也是可以使用命名空间 `AntDesign.Column` 或 `AntDesign.Charts.Column` 来避免告警。