---
order: 20
title:
  zh-CN: 开启组件值变更时验证
  en-US: Enable validation when component values change
---

## zh-CN

（v0.5+）为了性能考虑，默认关闭内容变更验证，在调用 `form.Validate()` 时才验证。使用 `ValidateOnChange` 属性可开启。

## en-US

(v0.5+) Validation is turned off by default for performance reasons. Validate only when `form.Validate ()` is called. It would be enabled by setting the 'ValidateOnChange' parameter.