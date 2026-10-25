---
order: 5
title:
  zh-CN: 使用 iconfont.cn 的多个资源
  en-US: Multiple resources from iconfont.cn
---

## zh-CN

将多个资源地址作为数组传给 `IconFont.ScriptUrl`，即可管理 [iconfont.cn](https://iconfont.cn/) 图标。组件与 React 一样反向、逐个加载资源，保留数组定义的同名图标覆盖顺序。多个实例共享加载中的请求，某个资源加载失败也不会阻止后续资源。



## en-US

Pass an array of resource URLs to `IconFont.ScriptUrl` to combine [iconfont.cn](https://iconfont.cn/) collections. As in React, scripts load sequentially in reverse order to preserve array-defined precedence for duplicate icon names. Instances share pending requests, and a failed resource does not block the remaining resources.
