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

Specify `dataSource` of Table as an array of data, the `OnChange` event and its incoming query state can be paged and filtered.

### Two data column types

- **PropertyColumn** inherits from Column and is bound with 'Property="c=> c.User.Name "' to support cascading access.  If used below .NET 6, need to specify the type of 'TItem', 'TProp'.
- **Column** bound with `@bind-Field="context.UserName"`, but does not support cascading access to class attributes (for example, `context.User.Name`), but it can be bound with `DataIndex="'User.Name'"`.

### Other column types

- **ActionColumn** for operation buttons or template, would not bind the data.
- **Selection** for add checkboxes for row selection.

## API

### The TItem Generic type parameter

Since 0.16.0, Table has supported ordinary classes, record, interface, and abstract classes as DataSource types. But there are a few caveats:

- When using record and some property values would be changed, please override the `GetHasCode()` method, or the `RowKey` of Table. Otherwise the state maintained internally for each column will not work correctly. [#2837](https://github.com/ant-design-blazor/ant-design-blazor/issues/2837)
- When using abstract classes, the Table would waits for the DataSource to get the element and takes the first one to initialize at the first render. [#3471](https://github.com/ant-design-blazor/ant-design-blazor/issues/3471)

### Table

| Parameter             | Instruction             | Type                         | Defaults |
| ---------------- | ---------------- | ---------------------------- | ------ |
| AutoColIndexes | Automatically assign column indexes; disable when setting `ColIndex` manually in a complex column tree. Blazor-only. | bool | true |
| ChildContent | Compose table columns with child components. Blazor-only; React uses `columns`. | RenderFragment<TItem> | - |
| ColumnDefinitions | Define columns from a row item. Blazor-only. | RenderFragment<TItem> | - |
| HeaderTemplate | Custom header template. Blazor-only. | RenderFragment<TItem> | - |
| RerenderStrategy | Used to control the ShouldRender property. Can be used to specify that a property should be rerendered only when it has been modified | [RerenderStrategy](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/RerenderStrategy.cs) | RerenderStrategy.Always |
| RowTemplate | Row template | RenderFragment | - |
| ExpandTemplate | Expand content template | RenderFragment<RowData<TItem>> | - |
| ShowExpandAll | Show an expand/collapse-all control for expandable rows on the current page. Only applies with `ExpandTemplate`. React `showExpandAll` equivalent. | bool | false |
| OnExpandAll | Callback invoked with the next expanded state when the expand-all control is clicked. React `onExpandAll` equivalent. | EventCallback<bool> | - |
| OnExpandedRowsChange | Callback invoked with the keys of expanded rows when expansion changes. React `onExpandedRowsChange` equivalent. | EventCallback<IEnumerable<string>> | - |
| DataSource | Data Sources | IEnumerable<TItem> | - |
| DefaultExpandMaxLevel | Maximum depth for `DefaultExpandAllRows`, preventing unbounded expansion in cyclic data. Blazor-only. | int | 4 |
| DefaultExpandAllRows | Initially, whether to expand all rows | bool | false |
| DefaultExpandedRowKeys | Initially expanded row keys, matched against `RowKey` | string[] | [] |
| RowExpandable | Set whether to allow row expansion | bool | false | - |
| TreeChildren | Used to select child nodes when displaying tree data | Func<TItem, IEnumerable<TItem>> | - |
| OnChange | Triggered when paging, sorting, and filtering changes | EventCallback<QueryModel<TItem>> | - |
| OnRow | Set row attributes | Func<RowData<TItem>, Dictionary<string, object>> | - |
| OnHeaderRow | Set header row attributes | Func<Dictionary<string, object>> | - |
| Loading | Is the table loading | bool | false |
| Title | Table title | string | - |
| TitleTemplate | Title template | RenderFragment | - |
| GroupTitleTemplate | Row group title template | RenderFragment | - |
| GroupFooterTemplate | Group foot template, often used to aggregate statistics | RenderFragment<GroupResult<TItem>> | - |
| Footer | Table Footer | string | - |
| FooterTemplate | Footer Template | RenderFragment | - |
| Size | Table size. `Default` maps to React's `large` size (the default); `Medium` maps to `medium`; `Small` maps to `small`. | [TableSize](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableSize.cs) | TableSize.Default |
| Column | Shared default properties for child columns; corresponds to React's `column` configuration. | TableColumnDefaults | - |
| Locale | Default copywriting settings, currently including sorting, filtering, and empty data copywriting | [TableLocale](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableLocale.cs) | LocaleProvider.CurrentLocale.Table |
| Bordered | Whether to display the outer border and column border | bool | false |
| Striped | Render alternating row backgrounds. Blazor-only. | bool | false |
| ScrollX | Set horizontal scrolling, can also be used to specify the width of the scrolling area, can be set as pixel value, percentage | string | - |
| ScrollY | Set the vertical scroll, can also be used to specify the height of the scrolling area, can be set as a pixel value | string | - |
| ScrollToFirstRowOnChange | Return to the first row after pagination, sorting, or filtering changes when scrolling is configured. React `scroll.scrollToFirstRowOnChange` equivalent. | bool | true |
| AutoHeight | Automatically expand the table body to fill the available screen height. Blazor-only. | bool | false |
| Virtual | Enable virtual rendering for large data sets. Use with `ScrollY`. | bool | false |
| EnableVirtualization | Deprecated compatibility alias for `Virtual`. | bool | false |
| ScrollBarWidth | Scroll bar width | string | "17px" |
| IndentSize | When displaying tree data, the width of each level of indentation, in px | int | 15 |
| ExpandIconColumnIndex | Index of the column where the custom expand icon is located | int | - |
| ExpandedRowOffset | Number of leading columns kept fixed for expanded rows. Legacy compatibility option; prefer fixed columns. | int? | - |
| RowClassName | The class name of the table row | Func<RowData<TItem>, string> | _ => "" |
| RowHoverable | Whether rows show a hover highlight when the pointer moves over them | bool | true |
| ShowHeader | Whether to show table header | bool | true |
| OnScroll | Callback invoked when the table body is scrolled | EventCallback<EventArgs> | - |
| GetPopupContainer | Gets the popup container for table filter menus and their nested controls | Func<ElementReference, ElementReference> | - |
| RowKey | Set the compare key to set default selection rows. It works the same way as `GetHashCode()`. Otherwise, the comparison is by reference by default. | Func<TItem, object> | - |
| ExpandedRowClassName | The className of the expanded row | Func<RowData<TItem>, string> | _ => "" |
| OnExpand | Triggered when the expand icon is clicked | EventCallback<RowData<TItem>> | - |
| SortDirections | Supported sorting methods, covering sortDirections in Table | [SortDirection[]](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | SortDirection.Preset.Default |
| ShowSorterTooltip | Whether to show sorter tooltips, or configure which header area triggers them. React-compatible. | bool \| TableSorterTooltipOptions | TableSorterTooltipOptions |
| StickyOffsetHeader | Enables sticky header behavior and sets its top offset in pixels; corresponds to React's `sticky.offsetHeader`. | int? | - |
| RemoteDataSource | Explicitly mark data as server-side when total-based detection is insufficient. Blazor-only. | bool | false |
| Responsive | Enable small-screen layout and responsive column hiding below 960px. Blazor-only. | bool | false |
| EmptyTemplate | Custom content shown when the table has no rows. Blazor-only; React uses `locale.emptyText`. | RenderFragment | - |
| ClassNames | Semantic class names keyed by Table DOM slots. React-compatible semantic API. | IReadOnlyDictionary<string, string> | - |
| RootClassName | Additional class applied to the Table root wrapper. React-compatible `rootClassName`. | string | - |
| ClassNamesFunc | Compute semantic class names from the current Table instance; Blazor callback form of `classNames`. | Func<Table<TItem>, IReadOnlyDictionary<string, string>> | - |
| Styles | Semantic inline styles keyed by Table DOM slots. React-compatible semantic API. | IReadOnlyDictionary<string, string> | - |
| StylesFunc | Compute semantic inline styles from the current Table instance; Blazor callback form of `styles`. | Func<Table<TItem>, IReadOnlyDictionary<string, string>> | - |
| TableLayout | The table-layout attribute of the table element, set to fixed means that the content will not affect the layout of the column | string | - |
| OnRowClick | Row click event (deprecated in antd v3) | EventCallback<RowData<TItem>> | - |
| PageIndex | Current page number (1-based); maps to React `pagination.current`. | int | 1 |
| PageIndexChanged | Callback when the current page changes. | EventCallback<int> | - |
| PageSize | Rows per page; maps to React `pagination.pageSize`. | int | 10 |
| PageSizeChanged | Callback when rows per page changes. | EventCallback<int> | - |
| Total | Total record count; maps to React `pagination.total`. | int | 0 / inferred from data |
| TotalChanged | Callback when the total count changes. | EventCallback<int> | - |
| PaginationPosition | Placement of the built-in pager (`topLeft` through `bottomRight`). | string | bottomRight |
| PaginationPlacement | Independent top/bottom pager placement; supports `none`. | string[] | - |
| PaginationTemplate | Replace the built-in pager with a custom template receiving page size, page index, total, CSS class, and change callback. | RenderFragment<(int PageSize, int PageIndex, int Total, string PaginationClass, EventCallback<PaginationEventArgs> HandlePageChange)> | - |
| OnPageIndexChange | Callback when the page changes, with the paging event arguments. | EventCallback<PaginationEventArgs> | - |
| OnPageSizeChange | Callback when page size changes, with the paging event arguments. | EventCallback<PaginationEventArgs> | - |
| HidePagination| To hide the pager, PageSize would equals the number of rows in the data source | bool | false |
| Resizable | Enable resizable column | bool | false |
| FieldFilterTypeResolver | Used to resolve filter types for columns | `IFilterTypeResolver` | Injected |


#### React API mapping notes

React's `pagination` object is composed from Blazor's `PageIndex`, `PageSize`,
`Total`, `HidePagination`, `PaginationPosition`/`PaginationPlacement`, and
`PaginationTemplate`; the change callbacks are `PageIndexChanged`,
`PageSizeChanged`, `TotalChanged`, `OnPageIndexChange`, and `OnPageSizeChange`.
React's `expandable` object is represented by `ExpandTemplate`,
`DefaultExpandAllRows`, `DefaultExpandedRowKeys`, `DefaultExpandMaxLevel`,
`RowExpandable`, `ExpandedRowClassName`, `OnExpand`, and
`ExpandIconColumnIndex`. These are Blazor's component-shaped equivalents, not
single object-valued parameters. React's `components` prop, which replaces
internal table elements, has no Blazor Table equivalent.

React's `columns` array is declared with Blazor `<Column>` child components or
`ColumnDefinitions`. A React `sorter` config maps to `Sortable`,
`SorterCompare`, and `SorterMultiple`; `DefaultSortOrder` sets only the initial
sort order, so the controlled `sortOrder` prop is not currently available.
React `filterDropdownProps` maps partially to `FilterDropdownOpen`,
`FilterDropdownAutoClose`, and `GetPopupContainer`. `expandedRowRender` maps to
`ExpandTemplate`, while controlled `expandedRowKeys`, `expandRowByClick`, and a
custom `expandIcon` renderer are not currently available.

### Column

The Column definition of the previous version, For .NET 6 and above, `PropertyColumn` is recommended.

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
| SortIcon | Render a custom sorter icon. The `SortDirection?` context is null when unsorted. React `sortIcon` equivalent. | RenderFragment<SortDirection?> | - |
| DefaultSortOrder | Default sort order | [SortDirection](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | - |
| OnCell | Set cell properties | Func<CellData, Dictionary<string, object>> | - |
| OnHeaderCell | Set header cell properties | Func<Dictionary<string, object>> | - |
| Width | Set the column width. | string | - |
| MinWidth | Set the minimum column width in pixels. Works with automatic table layout, matching React's `minWidth`. | int? | - |
| RowScope | Set the scope for each body cell in this column, such as `row` or `rowgroup` | string | - |
| Filterable | Whether to show filter | bool | false |
| Filters | Specify the column to filter the menu | IEnumerable<TableFilter<TData>> | - |
| FilterMultiple | Specify filter multiple selection and single selection | bool | true |
| FilterOnClose | Apply selected filter values when the default filter dropdown closes. Set to `false` to require confirmation with the dropdown's OK button. React `filterOnClose` equivalent. | bool | true |
| DefaultFilteredValue | Filter values selected by default when the column is initialized. React `defaultFilteredValue` equivalent. | IEnumerable<TData> | - |
| FilterResetToDefaultFilteredValue | When true, Reset restores `DefaultFilteredValue`; Reset changes only the pending selection and leaves confirmation to OK, matching React. | bool | false |
| OnFilter | Filter current data | Expression<Func<TData, TData, bool>> | - |
| FilterDropdown | Custom Filter Dropdown Template | RenderFragment | - |
| FieldFilterType | Specifies what filter options to display and how to filter the data | `IFieldFilterType` | Resolved using Table's `FieldFilterTypeResolver` |
| Filtered   |  Whether the dataSource is filtered. Filter icon will be actived when it is true. | bool |  false |
| Grouping | Specify that this column is used for grouping | bool | - |
| GroupBy | A delegate that converts the value of a group | Func<TData, object> | - |

### PropertyColumn

Inherit from `Column`.

| Parameter             | Instruction             | Type                         | Defaults |
| ---------------- | ---------------- | ---------------------------- | ------ |
| Property         |  Defines the value to be displayed in this column's cells. | Expression<Func<TItem, TProp>> | - |


### Selection

| Parameter             | Instruction             | Type                         | Defaults |
| ---------------- | ---------------- | ---------------------------- | ------ |
| CheckStrictly | Check table row precisely; parent row and children rows are not associated | boolean | true |
| Type | `checkbox` or `radio` | `checkbox` \| `radio` | `checkbox` | 
| Disabled         |  Whether to disable check     |      bool            |   false      |

### GenerateColumns

Columns can be automatically generated by the 'TItem' type. See the [demo](#components-table-demo-generate-columns).

| Parameter             | Instruction             | Type                         | Defaults |
| ---------------- | ---------------- | ---------------------------- | ------ |
| Definitions | An Action to defined each column. The first parameter is the property name, the second one is the Column instance. | Action<string, object> Definitions |  -  |
| HideColumnsByName | Hide the columns by the property names.  | IEnumerable<string>   |  -  |
| Range            | Specific the range of the columns that need to display. | Range |  -  |

### Public methods

The Table can be controlled using the methods provided in the instance of the `@ref` binding `ITable`.

| Methods           | Instruction                                | 
| ---------------- | ------------------------------------------- | 
| ReloadData       | Reloads the data, with three overloads, specifying paging, filtering, and sorting   | 
| ResetData        | Reset table paging, filtering, sorting status                   |
| GetQueryModel    | Gets table state, which can be used for persistence and recovery              |
| SetSelection     | Set selection column                                 |
| SelectAll        | Select all rows                                   |
| UnselectAll      | Unselect all rows                                    |
| ExpandAll        | Expand all rows                             |
| CollapseAll      | Collapse all rows                     |
| ScrollTo         | Scroll to a row by index/key, or to a vertical offset; supports start, center, end, and nearest alignment |

### Table ref

| Member | Description |
| --- | --- |
| NativeElement | The root wrapper element of the table. |
| ScrollTo | Accepts `TableScrollToOptions` with `Index`, `Key`, `Top`, `Offset`, and `Align`. `Offset` aligns the row's top edge to the requested viewport offset and cannot be combined with `Top`. `Align` supports `start`, `center`, `end`, and `nearest` (default). Center alignment is not supported by virtual scrolling. |


### Responsive

The table supports responsive by default, and when the screen width is less than 960px, the table would switch to small-screen mode.

In small-screen mode, only certain features are currently supported, and mis-styling will occur in tables with some features such as group, expanded columns, tree data, summary cell, etc. 

If you want to disable responsive, you can set `Responsive="false"`.
