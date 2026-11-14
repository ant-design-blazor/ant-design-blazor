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
| AutoGenerate | 自动通过路由匹配菜单数据生成路径 | bool | - | 
| ChildContent | 子内容 | RenderFragment   | -         |
| Separator |分隔符自定义| string  | -  |


BreadcrumbItem

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Href | 链接的目的地 | int         | -         |
| Overlay   | 下拉菜单的内容 | RenderFragment         |-         |
| OnClick | 单击事件 | EventCallback<MouseEventArgs>  |-       |
| ChildContent | 标题模板内容 | RenderFragment  | -  |


