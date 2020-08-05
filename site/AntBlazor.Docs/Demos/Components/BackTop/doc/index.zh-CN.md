---
category: Components
type: 其他
title: BackTop
subtitle: 回到顶部
cover: https://gw.alipayobjects.com/zos/alicdn/tJZ5jbTwX/BackTop.svg
---

返回页面顶部的操作按钮。

## 何时使用

- 当页面内容区域比较长时；
- 当用户需要频繁返回顶部查看相关内容时。


## API

BackTop

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Target | 设置需要监听其滚动事件的元素，值为一个返回对应 DOM 元素的函数 | RenderFragment         | -         |
| VisibilityHeight   | 滚动高度达到此参数值才出现 `BackTop`| int         |-    |
| OnClick | 点击按钮的回调函数 | function         |-       |
