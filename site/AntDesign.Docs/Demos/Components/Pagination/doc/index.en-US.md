---
category: Components
type: Navigation
title: Pagination
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/1vqv2bj68/Pagination.svg
---

A long list can be divided into several pages using `Pagination`, and only one page will be loaded at a time.

## When To Use

- When it will take a long time to load/render all items.
- If you want to browse the data by navigating through pages.

## API


| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Current | Current page number | int | - |  |
| DefaultCurrent | Default initial page number | int | 1 |  |
| DefaultPageSize | Default number of data items per page | int | 10 |  |
| Disabled | Disable pagination | bool | - |  |
| HideOnSinglePage | Whether to hide pager on single page | bool | false |  |
| ItemRender | To customize item's innerHTML | RenderFragment(PaginationItemRenderContext) | - |  |
| PageSize | Number of data items per page | int | - |  |
| PageSizeOptions | Specify the sizeChanger options | int\[] | \{10, 20, 50, 100} |  |
| ShowLessItems | Show less page items | bool | false |  |
| ShowQuickJumper | Determine whether you can jump to pages directly | bool | false |  |
| GoButton | Quick jumper confirm button, this is for react version `ShowQuickJumper: { goButton: ReactNode }` | RenderFragment? | null |  |
| ShowSizeChanger | Determine whether to show `PageSize` select, it will be `true` when `Total >= TotalBoundaryShowSizeChanger` | boolean | - |  |
| ShowTitle | Show page item's title | bool | true |  |
| ShowTotal | To display the total number and range | Func<PaginationTotalContext, string>, RenderFragment<PaginationTotalContext> | - |  |
| Simple | Whether to use simple mode | bool | - |  |
| Size | Specify the size of `Pagination`, can be set to `small`. | "default" \| "small". | "" |  |
| Responsive | (Not implemented) If `Size` is not specified, `Pagination` would resize according to the width of the window | bool | - |  |
| Total | Total number of data items | int | 0 |  |
| OnChange | Called when the page number is changed, and it takes the resulting page number and pageSize as its arguments | Function(PaginationEventArgs) | null |  |
| OnShowSizeChange | Called when `PageSize` is changed | Function(PaginationEventArgs) | null |  |
| TotalBoundaryShowSizeChanger | When `Total` larger than it, `ShowSizeChanger` will be true | int | 50 |  |
