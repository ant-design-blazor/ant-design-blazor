---
order: 5
title:
  zh-CN: 使用 iconfont.cn 的多个资源
  en-US: Multiple resources from iconfont.cn
---

## zh-CN

使用`scriptUrl` 可引用多个资源，用户可灵活的管理 [iconfont.cn](http://iconfont.cn/) 图标。如果资源的图标出现重名，会按照数组顺序进行覆盖。

> 注意：这个方法会涉及JS互操作，因此需要确保在 `firstRender=true` 时调用。

## en-US

You can use `scriptUrl`  multiple times, manage icons in one `<Icon />` from multiple [iconfont.cn](http://iconfont.cn/) resources. If icon with a duplicate name in resources, it will overrided in array order.

> Note: This method involves JS interope, so make sure you call it when 'firstRender=true' 