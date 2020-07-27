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

```jsx
<Pagination onChange={onChange} total={50} />
```

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Current | Current page number | number | - |  |
| DefaultCurrent | Default initial page number | number | 1 |  |
| DefaultPageSize | Default number of data items per page | number | 10 |  |
| Disabled | Disable pagination | boolean | - |  |
| HideOnSinglePage | Whether to hide pager on single page | boolean | false |  |
| ItemRender | To customize item's innerHTML | (page, type: 'page' \| 'prev' \| 'next', originalElement) => React.ReactNode | - |  |
| PageSize | Number of data items per page | number | - |  |
| PageSizeOptions | Specify the sizeChanger options | string\[] | \['10', '20', '50', '100'] |  |
| ShowLessItems | Show less page items | boolean | false |  |
| ShowQuickJumper | Determine whether you can jump to pages directly | boolean \| `{ goButton: ReactNode }` | false |  |
| ShowSizeChanger | Determine whether to show `pageSize` select, it will be `true` when `total>=50` | boolean | - |  |
| ShowTitle | Show page item's title | boolean | true |  |
| ShowTotal | To display the total number and range | Function(total, range) | - |  |
| Simple | Whether to use simple mode | boolean | - |  |
| Size | Specify the size of `Pagination`, can be set to `small`. | 'default' \| 'small'. | "" |  |
| Responsive | If `size` is not specified, `Pagination` would resize according to the width of the window | boolean | - |  |
| Total | Total number of data items | number | 0 |  |
| OnChange | Called when the page number is changed, and it takes the resulting page number and pageSize as its arguments | Function(page, pageSize) | noop |  |
| OnShowSizeChange | Called when `pageSize` is changed | Function(current, size) | noop |  |
