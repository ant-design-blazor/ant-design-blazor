---
category: Components
type: 导航
title: Breadcrumb
subtitle: 面包屑
cover: https://gw.alipayobjects.com/zos/alicdn/9Ltop8JwH/Breadcrumb.svg
---

显示当前页面在系统层级结构中的位置，并能向上返回。

## 何时使用

- 当系统拥有超过两级以上的层级结构时；
- 当需要告知用户『你在哪里』时；
- 当需要向上导航的功能时。


## API

Breakcrumb

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| ItemRender | 自定义链接函数，和 react-router 配置使用 | int   | -         |
| Params   | 	路由的参数| int   |-      |
| Routes | 	router 的路由栈信息 | string         |-       |
| Separator |分隔符自定义| string  | -  |


BreadcrumbItem

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Href | 链接的目的地 | int         | -         |
| Overlay   | 下拉菜单的内容 | int         |-         |
| OnClick | 单击事件 | EventCallback<MouseEventArgs>  |-       |
| DropdownProps |弹出下拉菜单的自定义配置 | string  | -  |


