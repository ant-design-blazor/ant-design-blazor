---
category: Components
type: 导航
title: PageHeader
subtitle: 页头
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/6bKE0Cq0R/PageHeader.svg
---

页头位于页容器中，页容器顶部，起到了内容概览和引导页级操作的作用。包括由面包屑、标题、页面内容简介、页面级操作等、页面级导航组成。

## 何时使用

当需要使用户快速理解当前页是什么以及方便用户使用页面功能时使用，通常也可被用作页面间导航。

## API

### PageHeader
| 参数 | 说明 | 类型 | 默认值 | 全局配置 |
| --- | --- | --- | --- | --- |
| `Ghost` | 使背景色透明 | `boolean` | `true` | - |
| `Title` | title 文字 | `string \| RenderFragment` | - | - |
| `Subtitle` | subTitle 文字 | `string \| RenderFragment` | - | - |
| `BackIcon` | 自定义 back icon | `bool? \| string \| RenderFragment` | - | - |
| `OnBack` | 返回按钮的点击事件 | `EventCallback` | 未订阅该事件时默认调用 history.back| - |

### Page Header 组成部分
| 元素 | 说明 |
| ----- | ----------- | ---- | ------------- |
| `PageHeaderTitle` | title 部分，`Title` 优先级更高 |
| `PageHeaderSubtitle` | subtitle 部分，`Subtitle` 优先级更高 |
| `PageHeaderContent` | 内容部分 |
| `PageHeaderFooter` | 底部部分 |
| `PageHeaderTags` |  title 旁的 tag 列表容器 |
| `PageHeaderExtra` | title 的行尾操作区部分 |
| `PageHeaderBreadcrumb` | 面包屑部分 |
| `PageHeaderAvatar` | 头像部分 |