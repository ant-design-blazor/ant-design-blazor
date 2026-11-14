---
category: Components
subtitle: 分页
type: 导航
title: Pagination
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/1vqv2bj68/Pagination.svg
---

采用分页的形式分隔长列表，每次只加载一个页面。

## 何时使用

- 当加载/渲染所有数据将花费很多时间时；
- 可切换页码浏览数据。

## API


| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Current | 当前页数 | int | - |  |
| DefaultCurrent | 默认的当前页数 | int | 1 |  |
| DefaultPageSize | 默认的每页条数 | int | 10 |  |
| Disabled | 禁用分页 | bool | - |  |
| HideOnSinglePage | 只有一页时是否隐藏分页器 | bool | false |  |
| ItemRender | 用于自定义页码的结构，可用于优化 SEO | RenderFragment(PaginationItemRenderContext) | - |  |
| PageSize | 每页条数 | int | - |  |
| PageSizeOptions | 指定每页可以显示多少条 | string\[] | \['10', '20', '50', '100'] |  |
| ShowLessItems | 是否显示较少页面内容 | bool | false |  |
| ShowQuickJumper | 是否可以快速跳转至某页 | bool | false |  |
| GoButton | 快速跳转按钮,对应原版 ShowQuickJumper: { goButton: ReactNode } | RenderFragment? | null |  |
| ShowSizeChanger | 是否展示 `PageSize` 切换器，当 `Total` 大于 `TotalBoundaryShowSizeChanger` 时默认为 `true` | bool | - |  |
| ShowTitle | 是否显示原生 tooltip 页码提示 | bool | true |  |
| ShowTotal | 用于显示数据总量和当前数据顺序 | Func<PaginationTotalContext, string>, RenderFragment<PaginationTotalContext> | - |  |
| Simple | 当添加该属性时，显示为简单分页 | bool | - |  |
| Size | 当为「small」时，是小尺寸分页 | "default" \| "small" | "" |  |
| Responsive | (未实现)当 size 未指定时，根据屏幕宽度自动调整尺寸 | bool | - |  |
| Total | 数据总数 | int | 0 |  |
| OnChange | 页码改变的回调，参数是改变后的页码及每页条数 | Function(PaginationEventArgs) | null |  |
| OnShowSizeChange | PageSize 变化的回调 | Function(PaginationEventArgs) | null |  |
| TotalBoundaryShowSizeChanger | 当 `Total` 大于该值, ShowSizeChanger 默认为 `true` | int | 50 |  |
