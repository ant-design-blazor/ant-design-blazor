---
category: Components
type: 数据展示
title: Comment
subtitle: 评论
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/ILhxpGzBO/Comment.svg
---

对网站内容的反馈、评价和讨论。

## 何时使用

评论组件可用于对事物的讨论，例如页面、博客文章、问题等等。

## API

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Actions | 在评论内容下面呈现的操作项列表 | IEnumerable<RenderFragment> | - |  |
| Author | 要显示为注释作者的元素 | string | - |  |
| AuthorTemplate | 要显示为注释作者的模板 | RenderFragment | - |  |
| Avatar | 要显示为评论头像的元素 - 通常是 antd `Avatar` 或者 src | string | - |  |
| AvatarTemplate | 要显示为评论头像的模板 - 通常是 antd `Avatar` 或者 src | RenderFragment | - |  |
| ChildContent | 嵌套注释应作为注释的子项提供 | RenderFragment | - |  |
| Content | 评论的主要内容 | string | - |  |
| ContentTemplate | 评论的主要内容模板 | RenderFragment | - |  |
| Datetime | 展示时间描述 | string | - |  |
| DatetimeTemplate | 展示时间描述模板 | RenderFragment | - |  |
| Placement | 头像的位置.  | `left` \| `right` |  `left` | 0.18.0 |