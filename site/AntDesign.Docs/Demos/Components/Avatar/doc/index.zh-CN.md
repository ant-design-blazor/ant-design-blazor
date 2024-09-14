---
category: Components
subtitle: 头像
type: 数据展示
title: Avatar
cover: https://gw.alipayobjects.com/zos/antfincdn/aBcnbw68hP/Avatar.svg
---

用来代表用户或事物，支持图片、图标或字符展示。

## API

### Avatar Props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Alt | 图像无法显示时的替代文本 | string | - |  |
| Icon | 设置头像的自定义图标 | string | - |  |
| OnError | 图片加载失败的事件 | EventCallback&lt;ErrorEventArgs> | - |  |
| Shape | 指定头像的形状 | string | - |  |
| Size | 设置头像的大小 `default` \| `small` \| `large` | string | `default` |  |
| Src | 图片类头像的资源地址或者图片元素 | string | - |  |
| SrcSet |设置图片类头像响应式资源地址 | string | - |  |

> Tip：你可以设置 `Icon` 或 `ChildContent` 作为图片加载失败的默认 fallback 行为，优先级为 `Icon` > `ChildContent`

### AvatarGroup Props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| MaxCount | 显示的最大头像个数 | int | - |  |
| MaxPopoverPlacement | 多余头像气泡弹出位置 | `top` \| `bottom` | `top` |  |
| MaxStyle | 多余头像样式 | string | - |  |
