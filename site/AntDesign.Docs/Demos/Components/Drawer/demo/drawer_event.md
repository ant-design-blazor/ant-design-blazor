---
order: 11
title:
  zh-CN: 事件
  en-US: Event
---

## zh-CN

Drawer 的事件处理

对于 Drawer 组件:
 1. OnOpen: 在打开前执行，并可以通过参数的 `Cancel` 属性取消打开操作。
 2. OnClose: 在关闭前执行，你需要通过它来控制 Drawer 组件的 `Visible` 属性。

对于 DrawerService:
 1. DrawerRef.OnOpen: 在打开前执行，内部是在 `Drawe.OnOpen` 事件中调用该方法
 2. DrawerRef.OnClosing: 在关闭前执行
 3. DrawerRef.OnClosed: 在关闭后执行。



## en-US

Drawer event handling

For Drawer component:
 1. OnOpen: Execute before opening, and you can cancel the opening operation through the `Cancel` attribute of the parameter.
 2. OnClose: Before closing, you need to control the `Visible` paramter of the Drawer component through it.

For DrawerService:
 1. DrawerRef.OnOpen: Execute before opening, and call this method in `Drawe.OnOpen` event internally.
 2. DrawerRef.OnClosing: Execute before closing
 3. DrawerRef.OnClosed: Execute after shutdown