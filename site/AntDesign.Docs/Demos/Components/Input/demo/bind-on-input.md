---
order: 0
title:
  zh-CN: 输入时绑定
  en-US: Bind on input
---

## zh-CN

从 0.13.0 开始，输入框的值绑定事件默认变为 `onchange`，如果需要在输入时绑定，请设置 `BindOnInput`。

> 注意：当在 Server Side 使用输入时变更，会存在绑定延迟和文本回退问题，可尝试加大 `DebounceMilliseconds` 的值来解决。[#579](https://github.com/ant-design-blazor/ant-design-blazor/issues/579)


## en-US

Starting from 0.13.0, the input box value binding event defaults to 'onchange'. If you need to bind at input time, set 'BindOnInput'.

> Note: When using input changes on the Server Side, there are binding delays and text rollback issues, you can try to address them by increasing the values of `DebounceMilliseconds`. [#579](https://github.com/ant-design-blazor/ant-design-blazor/issues/579)