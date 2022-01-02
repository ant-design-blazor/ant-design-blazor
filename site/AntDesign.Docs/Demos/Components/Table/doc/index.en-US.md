---
category: Components
cols: 1
type: Data Display
title: Table
cover: https://gw.alipayobjects.com/zos/alicdn/f-SbcX2Lx/Table.svg
---

A table displays rows of data.

## When To Use

- To display a collection of structured data.
- To sort, search, paginate, filter data.

## How To Use

Specify `dataSource` of Table as an array of data.


## API
| Parameter             | Instruction             | Type                         | Defaults |
| ---------------- | ---------------- | ---------------------------- | ------ |
| RenderMode | Rendering mode | [RenderMode](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/RenderMode.cs) | RenderMode.Always |
| RowTemplate | Row template | RenderFragment | - |
| ExpandTemplate | Expand content template | RenderFragment<RowData<TItem>> | - |
| DataSource | Data Sources | IEnumerable<TItem> | - |
| DefaultExpandAllRows | Initially, whether to expand all rows | bool | false |
| RowExpandable | Set whether to allow row expansion | bool | false | - |
| TreeChildren | Used to select child nodes when displaying tree data | Func<TItem, IEnumerable<TItem>> | - |
| OnChange | Triggered when paging, sorting, and filtering changes | EventCallback<QueryModel<TItem>> | - |
| OnRow | Set row attributes | Func<RowData<TItem>, Dictionary<string, object>> | - |
| OnHeaderRow | Set header row attributes | Func<Dictionary<string, object>> | - |
| Loading | Is the table loading | bool | false |
| Title | Table title | string | - |
| TitleTemplate | Title template | RenderFragment | - |
| Footer | Table Footer | string | - |
| FooterTemplate | Footer Template | RenderFragment | - |
| Size | Table Size | [TableSize](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableSize.cs) | - |
| Locale | Default copywriting settings, currently including sorting, filtering, and empty data copywriting | [TableLocale](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableLocale.cs) | LocaleProvider.CurrentLocale.Table |
| Bordered | Whether to display the outer border and column border | bool | false |
| ScrollX | Set horizontal scrolling, can also be used to specify the width of the scrolling area, can be set as pixel value, percentage | string | - |
| ScrollY | Set the vertical scroll, can also be used to specify the height of the scrolling area, can be set as a pixel value | string | - |
| ScrollBarWidth | Scroll bar control width | int | 17 |
| IndentSize | When displaying tree data, the width of each level of indentation, in px | int | 15 |
| ExpandIconColumnIndex | Index of the column where the custom expand icon is located | int | - |
| RowClassName | The class name of the table row | Func<RowData<TItem>, string> | _ => "" |
| ExpandedRowClassName | The className of the expanded row | Func<RowData<TItem>, string> | _ => "" |
| OnExpand | Triggered when the expand icon is clicked | EventCallback<RowData<TItem>> | - |
| SortDirections | Supported sorting methods, covering sortDirections in Table | [SortDirection[]](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | SortDirection.Preset.Default |
| TableLayout | The table-layout attribute of the table element, set to fixed means that the content will not affect the layout of the column | string | - |
| OnRowClick | Row click event (deprecated in antd v3) | EventCallback<RowData<TItem>> | - |
### Column
| Parameter             | Instruction             | Type                         | Defaults |
| ---------------- | ---------------- | ---------------------------- | ------ |
| FieldChanged | Field change event | EventCallback<TData | - |
| FieldExpression | Parse Field data | Expression<Func<TData>> FieldExpression | - |
| DataIndex | The corresponding path of the column data in the data item, support for querying the nested path through the array | int | - |
| Format | Column data serialization rules, such as DateTime.ToString("XXX"). | string | - |
| Sortable | Whether to allow sorting | bool | false |
| SorterCompare | Comparison function with custom sort | Func<TData, TData, int> | - |
| SorterMultiple | Number of sorts | int | - |
| SortDirections | Supported sorting method | [SortDirection[]](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | - |
| DefaultSortOrder | Default sort order | [SortDirection](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | - |
| OnCell | Set cell properties | Func<CellData, Dictionary<string, object>> | - |
| OnHeaderCell | Set header cell properties | Func<Dictionary<string, object>> | - |
| Filterable | Whether to show filter | bool | false |
| Filters | Specify the column to filter the menu | IEnumerable<TableFilter<TData>> | - |
| FilterMultiple | Specify filter multiple selection and single selection | bool | true |
| OnFilter | Filter current data | Expression<Func<TData, TData, bool>> | - |

### Responsive

The table supports responsive by default, and when the screen width is less than 960px, the table would switch to small-screen mode.

In small-screen mode, only certain features are currently supported, and mis-styling will occur in tables with some features such as group, expanded columns, tree data, summary cell, etc. 

If you want to disable responsive, you can set `Responsive="false"`.
