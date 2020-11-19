---
category: Components
subtitle: 头像
type: 数据展示
title: Avatar
cover: https://gw.alipayobjects.com/zos/antfincdn/aBcnbw68hP/Avatar.svg
---

用来代表用户或事物，支持图片、图标或字符展示。

## API

### Avatar

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- | --- |
| Shape | 指定头像的形状 | string | null |
| Size | 设置头像的大小 | `string|AntSizeLDSType{"default","large","small"}` | AntSizeLDSType.Default |
| Src | 图片类头像的资源地址 | string |  |
| SrcSet | 设置图片类头像响应式资源地址 | string |  |
| Alt | 图像无法显示时的替代文本 | string |  |
| Icon | 设置头像图标 | string |  |
| Error | 图片加载失败的事件，返回 false 会关闭组件默认的 fallback 行为 | function()=>ErrorEventArgs |  |


### AvaterGroup

| 属性 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| ChildContent | 自定义内容 | RenderFragment | - |
| MaxCount | 组最大显示数量 | int | - |
| MaxStyle | 组合并部分显示样式 | string | - |