---
order: 2
title:
  zh-CN: 带图标的步骤条
  en-US: With icon
---

## zh-CN

通过设置 `Steps.Step` 的 `icon` 属性，可以启用自定义图标。

此外，新增 `IconTemplate` 参数（类型为 `RenderFragment<StepsStatus>`），可以通过模板来自定义渲染图标并获取当前 step 的状态。

## en-US

You can use your own custom icons by setting the property `icon` for `Steps.Step`.

Also, a new `IconTemplate` parameter (type `RenderFragment<StepsStatus>`) is available to render custom icons with access to the current step status.
