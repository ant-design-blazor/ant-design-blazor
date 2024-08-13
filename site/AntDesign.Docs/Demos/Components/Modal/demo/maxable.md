---
order: 17
title:
  zh-CN: 最大化
  en-US: Maximizable
---

## zh-CN

允许Modal在浏览器内最大化.
`Maximizable`参数将会展示 Modal 最大化的按钮；`DefaultMaximized` 参数可以控制在 Modal 初始化的时候最大化。
`Maximizable` 和 `DefaultMaximized` 并不相互耦合，即使 `Maximizable=false`, `DefaultMaximized=true` 也会保证 Modal 初始化的时候最大化。
注意：
1. 如果`Draggable`为true，恢复大小的时候将会重置位置
2. 当Modal最大化的时候被关闭，再次显示的时候会保持最大化的样子

## en-US

Allow Modal to maximize within the browser. 
The `Maximizable` parameter will display the button for maximizing Modal, and the `DefaultMaximized` parameter can control the maximization during Modal initialization.
`Maximizable` and `DefaultMaximized` are not coupled with each other. Even if `Maximizable=false` and `DefaultMaximized=true` will ensure that the Modal is maximized during initialization.

Note:
1. If `Draggable` is true, the position will be reset when the size is restored
2. It is turned off when the Modal is maximized, and it will remain maximized when it is displayed again
