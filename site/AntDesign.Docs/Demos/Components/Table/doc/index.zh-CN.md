---
category: Components
cols: 1
type: 数据展示
title: Table
subtitle: 表格
cover: https://gw.alipayobjects.com/zos/alicdn/f-SbcX2Lx/Table.svg
---

展示行列数据。

## 何时使用

- 当有大量结构化的数据需要展现时；
- 当需要对数据进行排序、搜索、分页、自定义操作等复杂行为时。

## 如何使用

指定表格的数据源 `dataSource` 为一个数组。


## API
### Table
| 参数             | 说明             | 类型                         | 默认值 |
| ---------------- | ---------------- | ---------------------------- | ------ |
| RenderMode | 渲染模式 | [RenderMode](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/RenderMode.cs) | RenderMode.Always |
| RowTemplate | 行模板 | RenderFragment | - |
| ExpandTemplate | 展开内容模板 | RenderFragment<RowData<TItem>> | - |
| DataSource | 数据来源 | IEnumerable<TItem> | - |
| DefaultExpandAllRows | 初始时，是否展开所有行 | bool | false |
| RowExpandable | 设置是否允许行展开 | bool | false | - |
| TreeChildren | 用于树形数据展示时选择子节点 | Func<TItem, IEnumerable<TItem>> | - |
| OnChange | 分页、排序、筛选变化时触发 | EventCallback<QueryModel<TItem>> | - |
| OnRow | 设置行属性 | Func<RowData<TItem>, Dictionary<string, object>> | - |
| OnHeaderRow | 设置头部行属性 | Func<Dictionary<string, object>> | - |
| Loading | 表格是否加载中 | bool | false |
| Title | 表格标题 | string | - |
| TitleTemplate | 标题模板 | RenderFragment | - |
| Footer | 表格尾部 | string | - |
| FooterTemplate | 表格尾部模板 | RenderFragment | - |
| Size | 表格尺寸大小 | [TableSize](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableSize.cs) | - |
| Locale | 默认文案设置，目前包括排序、过滤、空数据文案 | [TableLocale](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableLocale.cs) | LocaleProvider.CurrentLocale.Table |
| Bordered | 是否展示外边框和列边框 | bool | false |
| ScrollX | 设置横向滚动，也可用于指定滚动区域的宽，可以设置为像素值，百分比 | string | - |
| ScrollY | 设置纵向滚动，也可用于指定滚动区域的高，可以设置为像素值 | string | - |
| ScrollBarWidth | 滚动条控件宽度 | int | 17 |
| IndentSize | 展示树形数据时，每层缩进的宽度，以 px 为单位 | int | 15 |
| ExpandIconColumnIndex | 自定义展开图标所在列索引 | int | - |
| RowClassName | 表格行的类名 | Func<RowData<TItem>, string> | _ => "" |
| ExpandedRowClassName | 展开行的 className | Func<RowData<TItem>, string> | _ => "" |
| OnExpand | 点击展开图标时触发 | EventCallback<RowData<TItem>> | - |
| SortDirections | 支持的排序方式，覆盖 Table 中 sortDirections | [SortDirection[]](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | SortDirection.Preset.Default |
| TableLayout | 表格元素的 table-layout 属性，设为 fixed 表示内容不会影响列的布局 | string | - |
| OnRowClick | 行点击事件(于antd v3中已废弃) | EventCallback<RowData<TItem>> | - |
### Column
| 参数             | 说明             | 类型                         | 默认值 |
| ---------------- | ---------------- | ---------------------------- | ------ |
| FieldChanged | Field更改事件 | EventCallback<TData | - |
| FieldExpression | 解析Field数据 | Expression<Func<TData>> FieldExpression | - |
| DataIndex | 列数据在数据项中对应的路径，支持通过数组查询嵌套路径 | int | - |
| Format | 列数据序列化规则,比如DateTime.ToString("XXX"). | string | - |
| Sortable | 是否允许排序 | bool | false |
| SorterCompare | 比较函数以自定义排序 | Func<TData, TData, int> | - |
| SorterMultiple | 排序数量 | int | - |
| SortDirections | 支持的排序方式 | [SortDirection[]](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | - |
| DefaultSortOrder | 默认排序顺序 | [SortDirection](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | - |
| OnCell | 设置单元格属性 | Func<CellData, Dictionary<string, object>> | - |
| OnHeaderCell | 设置头部单元格属性 | Func<Dictionary<string, object>> | - |
| Filterable | 是否显示筛选器 | bool | false |
| Filters | 指定需要筛选菜单的列 | IEnumerable<TableFilter<TData>> | - |
| FilterMultiple | 指定筛选器多选和单选 | bool | true |
| OnFilter | 筛选当前数据 | Expression<Func<TData, TData, bool>> | - |
### 响应式

表格默认支持响应式，当屏幕宽度小于 960px 时，表格的数据列变为小屏模式。

在小屏模式下，目前只支持一定的表格功能，在有 分组合并、展开列、树形数据、总结栏 等属性的表格中会出现样式错乱。
如果想禁用响应式，可以设置 `Responsive="false"`。

### DynamicTable
| 参数             | 说明             | 类型                         | 默认值 |
| ---------------- | ---------------- | ---------------------------- | ------ |
| TItem | 泛型参数类型 | Type | - |
| DataSource | 要显示的数据源 | IEnumerable<TItem> | - |
| PageIndex | 分页页号 | int | 1 |
| PageSize | 分页每页记录数 | int | 20 |
| TotalCount | 不分页情况下的记录总数 | int | 0 |
| PaginationPosition | 分页功能条位置，请参考Table的同名属性 | string | bottomRight |
| PageChanged | 分页参数变化时触发，事件 | EventCallBack<PaginationEventArgs> | null |
| RefTable | 内部实际的AntDesign.Table对象 | Table | - |
| IsReadOnly | 是否只读，当为false时，直接支持行编辑 | bool | false |
| ExternalVaildateErrors | 列表中的错误信息对应的单元格会被标记 | IEnumerable<VaildateErrorItem> | null |
| EntityCheckedPropertyName | 用于绑定行勾选状态的属性名，若实体类实现了接口 IDynamicTableCheckedColumnDefinition，则无需额外设置 | string | null/"Checked" |
| MulitSelect | 多选/单选，只有设置了 EntityCheckedPropertyName 属性后才会生效 | bool | true |
| CheckColumnVisibility | 用户定义函数，可以在运行时决定列是否可编辑 | Func<string, bool> | null |
| CheckCellEditable | 用户定义函数，可以在运行时决定单元格是否可编辑 | Func<string, TItem, bool> | null |

### DynamicTable.Annotations

DynamicTable 支持根据实体类属性上添加的Attributes标签，实现定制化UI元素

| Annotation | 说明 | 类型 |参数 |
| --- | --- | --- | --- |
| 原生 | 系统自带Annotation，包括[Display],[DisplayFormat]以及各类校验Annotation | System.ComponentModel.DataAnnotations.* | - |
| DataSourceBind | 对于Select<,>等类型的控件，指定DataSource数据源和LableName、ValueName属性 | AntDesign.DataAnnotations.DataSourceBindAttribute | 数据源类型建议是类中的静态属性，若是非静态属性，将创建一个类的实例 |
| AutoGenerateBehavior | 指定该属性在DynamicTable中的可见性，注意：当设置[Display(AutoGenerateField=false)]时，该属性无效。 | AntDesign.DataAnnotations.AutoGenerateBehaviorAttribute | DynamicTableVisibility |
| DynamicTableView | 如果默认生成的展现UI元素不符合实际需求，可以通过该标签指定UI元素类型 | AntDesign.DataAnnotations.DynamicTableViewAttribute | uicontroltype: UI元素类型，需要是ComponentBase的子类；bindproperty：UI元素中一个支持@bind-{bindproperty}形式绑定的属性; extraProperties与extraPropertyValues，可以额外为控件设置属性 |
| DynamicTableView.ConverterType | 可以实现比Format更复杂更自由的格式转换 | IValueConverter | - |
| DynamicTableEdit | 如果默认生成的编辑UI元素不符合实际需求，可以通过该标签指定UI元素类型 | AntDesign.DataAnnotations.DynamicTableEditAttribute | uicontroltype: UI元素类型，需要是ComponentBase的子类；bindproperty：UI元素中一个支持@bind-{bindproperty}形式绑定的属性; extraProperties与extraPropertyValues，可以额外为控件设置属性 |
| DisplayConverter | 等效于 DynamicTableView.ConverterType，但可以在DiffForm中使用 | AntDesign.DataAnnotations.DisplayConverterAttribute | - |
| DynamicTableColumnSetup | 可以设置列的 IsReadOnly,CanSort,Width,Frozen 属性 | AntDesign.DynamicTableColumnSetupAttribute | - |