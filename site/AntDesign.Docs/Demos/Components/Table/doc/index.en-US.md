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

### DynamicTable
| Parameter             | Instruction             | Type                         | Defaults |
| ---------------- | ---------------- | ---------------------------- | ------ |
| TItem | Generic parameter | Type | - |
| DataSource | Datasource to be dispalyed in the table | IEnumerable<TItem> | - |
| PageIndex | Pagination index | int | 1 |
| PageSize | Record count of each page  | int | 20 |
| TotalCount | Total record count before Pagination | int | 0 |
| PaginationPosition | Pagination bar's position, ref: Table.PaginationPosition | string | bottomRight |
| PageChanged | Event, invoke when PageIndex/PageSize is changed | EventCallBack<PaginationEventArgs> | null |
| RefTable | The instance of AntDesign.Table object inside the DynamicTable | Table | - |
| IsReadOnly | When value is set to false, inside table edit is allowed. | bool | false |
| ExternalVaildateErrors | Cell positions of the datas included in the error list will be marked | IEnumerable<VaildateErrorItem> | null |
| EntityCheckedPropertyName | For binding checkbox columns with an entity's property£¬but if IDynamicTableCheckedColumnDefinition interface is implemented, manual binding isn't needed | string | null/"Checked" |
| MulitSelect | multi/single select for rows£¬only effective if EntityCheckedPropertyName is set. | bool | true |
| CheckColumnVisibility | User defined function, check if the column is editable in runtime | Func<string, bool> | null |
| CheckCellEditable | User defined function, check if the cell is editable in runtime | Func<string, TItem, bool> | null |

### DynamicTable.Annotations

DynamicTable allows customized UI elements by using Attributes for properties

| Annotation | Description | Type |Parameters |
| --- | --- | --- | --- |
| .NET built-in | The built-in Annotations£¬include [Display],[DisplayFormat] and validation Annotations | System.ComponentModel.DataAnnotations.* | - |
| DataSourceBind | For UI elements like Select<,>, designate DataSource,LableName and ValueName | AntDesign.DataAnnotations.DataSourceBindAttribute | It's advised to use a static property as datasource, ohterwise will create a new instance. |
| AutoGenerateBehavior | Define visibilty in DynamicTable. Attention£º[Display(AutoGenerateField=false)] will override this attribute's settings | AntDesign.DataAnnotations.AutoGenerateBehaviorAttribute | DynamicTableVisibility |
| DynamicTableView | You can customize UI elements for the property if not satisfied with the default ones in view mode | AntDesign.DataAnnotations.DynamicTableViewAttribute | uicontroltype: UI element's type, it must be ComponentBase's subclass; bindproperty£ºa bind property in UI element could be used like @bind-{bindproperty}; extraProperties and extraPropertyValues: you can set any properties for the UI element |
| DynamicTableView.ConverterType | Provide a highly flexible format convert method | IValueConverter | - |
| DynamicTableEdit | You can customized UI elements for the property if not satisfied with the default ones in edit mode | uicontroltype: UI element's type', it must be ComponentBase's subclass; bindproperty£ºa bind property in UI element could be used like @bind-{bindproperty}; extraProperties and extraPropertyValues: you can set any properties for the UI element |
| DisplayConverter | Same as DynamicTableView.ConverterType, but also effective in DiffForm | AntDesign.DataAnnotations.DisplayConverterAttribute | - |
| DynamicTableColumnSetup | Provide a way to set column's IsReadOnly,CanSort,Width,Frozen propertys. | AntDesign.DynamicTableColumnSetupAttribute | - |