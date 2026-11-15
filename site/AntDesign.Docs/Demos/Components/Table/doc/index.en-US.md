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
| RerenderStrategy | Used to control the ShouldRender property. Can be used to specify that a property should be rerendered only when it has been modified | [RerenderStrategy](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/RerenderStrategy.cs) | RerenderStrategy.Always |
| RowTemplate | Row template | RenderFragment | - |
| ExpandTemplate | Expand content template | RenderFragment<RowData<TItem>> | - |
| DataSource | Data Sources | IEnumerable<TItem> | - |
| Quick | Enable quick cell rendering; set to false to force the standard rendering path | bool | true |
| DefaultExpandAllRows | Initially, whether to expand all rows | bool | false |
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
| Size | Table Size | [TableSize](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableSize.cs) | - |
| Locale | Default copywriting settings, currently including sorting, filtering, and empty data copywriting | [TableLocale](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableLocale.cs) | LocaleProvider.CurrentLocale.Table |
| Bordered | Whether to display the outer border and column border | bool | false |
| ScrollX | Set horizontal scrolling, can also be used to specify the width of the scrolling area, can be set as pixel value, percentage | string | - |
| ScrollY | Set the vertical scroll, can also be used to specify the height of the scrolling area, can be set as a pixel value | string | - |
| ScrollBarWidth | Scroll bar width | string | "17px" |
| IndentSize | When displaying tree data, the width of each level of indentation, in px | int | 15 |
| ExpandIconColumnIndex | Index of the column where the custom expand icon is located | int | - |
| RowClassName | The class name of the table row | Func<RowData<TItem>, string> | _ => "" |
| RowKey | Set the compare key to set default selection rows. It works the same way as `GetHashCode()`. Otherwise, the comparison is by reference by default. | Func<TItem, object> | - |
| ExpandedRowClassName | The className of the expanded row | Func<RowData<TItem>, string> | _ => "" |
| OnExpand | Triggered when the expand icon is clicked | EventCallback<RowData<TItem>> | - |
| SortDirections | Supported sorting methods, covering sortDirections in Table | [SortDirection[]](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | SortDirection.Preset.Default |
| TableLayout | The table-layout attribute of the table element, set to fixed means that the content will not affect the layout of the column | string | - |
| OnRowClick | Row click event (deprecated in antd v3) | EventCallback<RowData<TItem>> | - |
| HidePagination| To hide the pager, PageSize would equals the number of rows in the data source | bool | false |
| Resizable | Enable resizable column | bool | false |
| FieldFilterTypeResolver | Used to resolve filter types for columns | `IFilterTypeResolver` | Injected |


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
| DefaultSortOrder | Default sort order | [SortDirection](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | - |
| OnCell | Set cell properties | Func<CellData, Dictionary<string, object>> | - |
| OnHeaderCell | Set header cell properties | Func<Dictionary<string, object>> | - |
| Filterable | Whether to show filter | bool | false |
| Filters | Specify the column to filter the menu | IEnumerable<TableFilter<TData>> | - |
| FilterMultiple | Specify filter multiple selection and single selection | bool | true |
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


### Responsive

The table supports responsive by default, and when the screen width is less than 960px, the table would switch to small-screen mode.

In small-screen mode, only certain features are currently supported, and mis-styling will occur in tables with some features such as group, expanded columns, tree data, summary cell, etc. 

If you want to disable responsive, you can set `Responsive="false"`.

### Quick rendering

`Table` enables quick rendering by default to reduce the number of component instances created for cells. Use a column's `CellRender` to access the current cell data:

```razor
<PropertyColumn Property="person => person.Name" Title="Name">
    <CellRender Context="cell">
        <a>@cell.FieldValue</a>
    </CellRender>
</PropertyColumn>
```

Regular column `ChildContent` remains supported. To preserve the existing `@context` behavior, the table executes the column template for each row and logs a warning recommending `CellRender`; this compatibility path reduces the quick-rendering performance benefit. Set `Quick="false"` to force standard rendering.

#### Benchmark

The benchmark uses Blazor's `HtmlRenderer` to compare `Quick=true/false`; all columns use `CellRender`. Non-virtualized cases cover 100 and 1,000 rows; virtualized cases cover 100, 1,000, and 10,000 rows. Each table has 8 or 16 columns. Virtualized cases simulate a 400px viewport and render 28 rows including overscan. Each case has one warmup, then takes the median of 7 runs (100 rows), 5 runs (1,000 rows), or 3 runs (10,000 rows). Results were collected on an Apple M4 Pro, macOS 27.0.1, and .NET 11 RC3:

| Rows | Columns | Virtualized | Rows rendered | Quick=true time / allocations | Quick=false time / allocations | Time / allocation reduction |
| ---: | ---: | :---: | ---: | ---: | ---: | ---: |
| 100 | 8 | No | 100 | 39.10 ms / 4,565,432 B | 151.61 ms / 19,685,312 B | 74.2% / 76.8% |
| 100 | 16 | No | 100 | 62.16 ms / 6,737,064 B | 235.09 ms / 36,901,856 B | 73.6% / 81.7% |
| 100 | 8 | Yes | 28 | 6.30 ms / 938,448 B | 20.09 ms / 4,082,800 B | 68.6% / 77.0% |
| 100 | 16 | Yes | 28 | 14.83 ms / 1,492,088 B | 39.18 ms / 8,067,528 B | 62.2% / 81.5% |
| 1,000 | 8 | No | 1,000 | 200.64 ms / 48,812,128 B | 782.03 ms / 197,566,440 B | 74.3% / 75.3% |
| 1,000 | 16 | No | 1,000 | 371.89 ms / 70,302,080 B | 14,277.07 ms / 371,741,904 B | 97.4% / 81.1% |
| 1,000 | 8 | Yes | 28 | 84.10 ms / 1,276,944 B | 182.86 ms / 4,420,968 B | 54.0% / 71.1% |
| 1,000 | 16 | Yes | 28 | 235.38 ms / 1,782,080 B | 1,282.56 ms / 8,355,888 B | 81.6% / 78.7% |
| 10,000 | 8 | Yes | 28 | 170.66 ms / 5,349,728 B | 1,398.73 ms / 8,492,248 B | 87.8% / 37.0% |
| 10,000 | 16 | Yes | 28 | 148.57 ms / 5,856,912 B | 583.24 ms / 12,461,744 B | 74.5% / 53.0% |

These are component-rendering microbenchmark results; they exclude browser layout, painting, and network time. Virtualization simulates a 400px viewport by invoking the framework's viewport-fill callback, so browser JavaScript and scrolling performance are not measured. The 10,000-row cases only use virtualization to avoid the cost of rendering every cell. Virtualization renders only 28 rows here, but the table still processes the full data source, so allocations increase with 10,000 rows. Timings are sensitive to machine load; compare the Quick modes within the same scenario. Rerun with:

```bash
dotnet run --project tests/AntDesign.Benchmarks/AntDesign.Benchmarks.csproj --configuration Release --framework net11.0
```
