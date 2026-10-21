---
category: Components
type: 数据展示
title: Card 
subtitle: 卡片
cols: 1
cover: https://gw.alipayobjects.com/zos/antfincdn/NqXt8DJhky/Card.svg
---

通用卡片容器.

## 何时使用

- 最基础的卡片容器，可承载文字、列表、图片、段落，常用于后台概览页面。


## API

Card

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Actions |卡片操作组，位置在卡片底部   | Array(RenderFragment) |-        |
| ActionTemplate | 用于放置 CardAction 的模板 | RenderFragment | -
| Body |卡片主要区域   | RenderFragment |-        |
| Extra |卡片右上角的操作区域 | RenderFragment |-        |
| Variant | 卡片视觉变体：`Outlined` 或 `Borderless`。 | `CardVariant` | `Outlined` |
| Bordered | 旧版边框开关。优先使用 `Variant`；设置 `Variant` 后以其为准。 | boolean | true |
| ClassNames | 按 Card 语义节点设置自定义 class：`root`、`header`、`title`、`extra`、`cover`、`body`、`actions`。 | `IReadOnlyDictionary<string, string>` | - |
| Styles | 按 Card 语义节点设置行内样式：`root`、`header`、`title`、`extra`、`cover`、`body`、`actions`。 | `IReadOnlyDictionary<string, string>` | - |
| HeadStyle | 已弃用的头部样式，请改用 `Styles["header"]`。 | string | - |
| BodyStyle | 已弃用的内容区域样式，请改用 `Styles["body"]`。 | string | - |
| CardAction | 一个单独的 卡片操作 组件，需要放在 ActionTemplate 内 | CardAction | -
| Cover |卡片封面 | RenderFragment |-        |
| Loading |当卡片内容还在加载中时，可以用 loading 展示一个占位 | boolean |-        |
| size |card 的尺寸 | RenderFragment |-        |
| Title |卡片标题 | String or RenderFragement |-        |
| Type |卡片类型，可设置为 inner 或 不设置 | string |-        |

Card.Grid

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| ChildContent |子容器 | RenderFragment |-        |
| Hoverable |	鼠标移过时可浮起 | boolean |-        |
| Style | 定义网格容器类名的样式 | CSS Properties |-        |

Card.Meta

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Avatar |  头像和图标 | RenderFragment |-        |
| ChildContent | 子容器 | RenderFragment |-        |
| Description | 描述内容 | boolean |-        |
| Style | 定义容器类名的样式 | CSS Properties |-        |
| Title |	标题内容 | String or RenderFragement |-        |
| ClassNames | 按 `root`、`section`、`avatar`、`title`、`description` 语义节点设置自定义 class。 | `IReadOnlyDictionary<string, string>` | - |
| Styles | 按 `root`、`section`、`avatar`、`title`、`description` 语义节点设置行内样式。 | `IReadOnlyDictionary<string, string>` | - |


