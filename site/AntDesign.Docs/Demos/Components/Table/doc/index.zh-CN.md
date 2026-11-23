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

指定表格的数据源 `DataSource` 为一个数组，可用 OnChange 事件传入的查询状态进行分页和筛选。

### 两个数据列类型:

- **PropertyColumn** 继承自 Column，用 `Property="c=>c.User.Name"` 绑定列，支持级联访问。在 .NET6 以下使用需指定 `TItem`, `TProp` 的类型。
- **Column** 用 `@bind-Field="context.UssrName"` 绑定时不支持级联访问类的属性（例如：`context.User.Name`），但可以用 `DataIndex="'User.Name'"` 绑定。

### 其他列类型

- **ActionColumn** 用于放置操作按钮，也可以作为不绑定类型的模板。
- **Selection** 用来开启选择列，并提供选择框。

## API

### TItem 泛型类型

从 0.16.0 开始，Table 已支持普通类、record、接口和抽象类作为 DataSource 的类型。但有几点需要说明：

- 当使用 record 且有属性值变化，请重写 `GetHasCode()` 方法，或者用 Table 的 `RowKey`，否则内部对每列保持的状态会无法正确工作。[#2837](https://github.com/ant-design-blazor/ant-design-blazor/issues/2837)
- 当使用抽象类时，首次渲染会等待 DataSource 获得元素，并取第一个来初始化。[#3471](https://github.com/ant-design-blazor/ant-design-blazor/issues/3471)

### Table
| 参数             | 说明             | 类型                         | 默认值 |
| ---------------- | ---------------- | ---------------------------- | ------ |
| AutoColIndexes | 自动分配列索引；复杂列结构手动设置 `ColIndex` 时可关闭。Blazor 专有参数。 | bool | true |
| ChildContent | 通过子组件组合表格列。Blazor 专有；React 使用 `columns`。 | RenderFragment<TItem> | - |
| ColumnDefinitions | 根据行数据定义列。Blazor 专有参数。 | RenderFragment<TItem> | - |
| HeaderTemplate | 自定义表头模板。Blazor 专有参数。 | RenderFragment<TItem> | - |
| RerenderStrategy | 重新渲染策略。可用来指定只有当有组件属性被修改时才重新渲染。 | [RerenderStrategy](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/RerenderStrategy.cs) | RerenderStrategy.Always |
| RowTemplate | 行模板 | RenderFragment | - |
| ExpandTemplate | 展开内容模板 | RenderFragment<RowData<TItem>> | - |
| ShowExpandAll | 显示当前页可展开行的全部展开/收起控件，仅在设置 `ExpandTemplate` 时生效。对应 React `showExpandAll`。 | bool | false |
| OnExpandAll | 点击全部展开/收起控件时触发，参数表示新的展开状态。对应 React `onExpandAll`。 | EventCallback<bool> | - |
| OnExpandedRowsChange | 展开状态变化时触发，参数为当前已展开行的 key。对应 React `onExpandedRowsChange`。 | EventCallback<IEnumerable<string>> | - |
| DataSource | 数据来源 | IEnumerable<TItem> | - |
| DefaultExpandMaxLevel | `DefaultExpandAllRows` 的最大展开深度，用于避免循环数据导致无限展开。Blazor 专有参数。 | int | 4 |
| DefaultExpandAllRows | 初始时，是否展开所有行 | bool | false |
| DefaultExpandedRowKeys | 初始展开的行 key，与 `RowKey` 返回值匹配 | string[] | [] |
| RowExpandable | 设置是否允许行展开 | bool | false | - |
| TreeChildren | 用于树形数据展示时选择子节点 | Func<TItem, IEnumerable<TItem>> | - |
| OnChange | 分页、排序、筛选变化时触发 | EventCallback<QueryModel<TItem>> | - |
| OnRow | 设置行属性 | Func<RowData<TItem>, Dictionary<string, object>> | - |
| OnHeaderRow | 设置头部行属性 | Func<Dictionary<string, object>> | - |
| Loading | 表格是否加载中 | bool | false |
| Title | 表格标题 | string | - |
| TitleTemplate | 标题模板 | RenderFragment | - |
| GroupTitleTemplate | 分组标题模板，不设置默认标题为该层级分组 Key 的字符串转换 | RenderFragment<GroupResult<TItem>> | - |
| GroupFooterTemplate | 分组脚部模板，常用于对分组的统计 | RenderFragment<GroupResult<TItem>> | - |
| Footer | 表格尾部 | string | - |
| FooterTemplate | 表格尾部模板 | RenderFragment | - |
| Size | 表格尺寸（`Default`、`Medium` 或 `Small`）。 | [TableSize](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableSize.cs) | TableSize.Default |
| Column | 子列共享的默认属性，对应 React 的 `column` 配置。 | TableColumnDefaults | - |
| Locale | 默认文案设置，目前包括排序、过滤、空数据文案 | [TableLocale](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableLocale.cs) | LocaleProvider.CurrentLocale.Table |
| Bordered | 是否展示外边框和列边框 | bool | false |
| Striped | 为表格行显示交替背景色。Blazor 专有参数。 | bool | false |
| ScrollX | 设置横向滚动，也可用于指定滚动区域的宽，可以设置为像素值，百分比 | string | - |
| ScrollY | 设置纵向滚动，也可用于指定滚动区域的高，可以设置为像素值 | string | - |
| ScrollToFirstRowOnChange | 配置滚动后，分页、排序或筛选变化时是否返回表格首行，对应 React `scroll.scrollToFirstRowOnChange`。 | bool | true |
| AutoHeight | 自动扩展表格主体以填满可用屏幕高度。Blazor 专有参数。 | bool | false |
| Virtual | 为大数据集启用虚拟渲染，需与 `ScrollY` 一起使用 | bool | false |
| EnableVirtualization | `Virtual` 的兼容旧参数，已弃用 | bool | false |
| ScrollBarWidth | 滚动条宽度 | string | "17px" |
| IndentSize | 展示树形数据时，每层缩进的宽度，以 px 为单位 | int | 15 |
| ExpandIconColumnIndex | 自定义展开图标所在列索引 | int | - |
| ExpandedRowOffset | 展开行中保持固定的前置列数。兼容旧用法；建议改用固定列。 | int? | - |
| RowClassName | 表格行的类名 | Func<RowData<TItem>, string> | _ => "" |
| RowHoverable | 是否启用鼠标悬停时的行高亮效果 | bool | true |
| ShowHeader | 是否显示表头 | bool | true |
| OnScroll | 表格内容滚动时触发的回调 | EventCallback<EventArgs> | - |
| GetPopupContainer | 获取表格筛选菜单及其内部控件的弹层容器 | Func<ElementReference, ElementReference> | - |
| RowKey | 设置比对 Key 来设置默认选中行，作用跟给 TItem 实现重写 `GetHashCode()` 一致。否则默认按引用来比对。 | Func<TItem, object> | - |
| ExpandedRowClassName | 展开行的 className | Func<RowData<TItem>, string> | _ => "" |
| OnExpand | 点击展开图标时触发 | EventCallback<RowData<TItem>> | - |
| SortDirections | 支持的排序方式，覆盖 Table 中 sortDirections | [SortDirection[]](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | SortDirection.Preset.Default |
| ShowSorterTooltip | 是否显示排序提示，或配置触发提示的表头区域。与 React 参数对应。 | bool \| TableSorterTooltipOptions | TableSorterTooltipOptions |
| StickyOffsetHeader | 启用粘性表头并设置顶部像素偏移，对应 React 的 `sticky.offsetHeader`。 | int? | - |
| RemoteDataSource | 在默认总数判断不适用时，显式标记数据由服务端提供。Blazor 专有参数。 | bool | false |
| Responsive | 启用小屏布局，并在宽度小于 960px 时隐藏响应式列。Blazor 专有参数。 | bool | false |
| EmptyTemplate | 表格无数据时显示的自定义内容。Blazor 专有；React 使用 `locale.emptyText`。 | RenderFragment | - |
| ClassNames | 通过 Table DOM 语义槽位设置 class。与 React 语义化 API 对应。 | IReadOnlyDictionary<string, string> | - |
| RootClassName | 添加到 Table 根节点的 class。对应 React `rootClassName`。 | string | - |
| ClassNamesFunc | 根据当前 Table 实例计算语义 class；Blazor 对应 `classNames` 的回调形式。 | Func<Table<TItem>, IReadOnlyDictionary<string, string>> | - |
| Styles | 通过 Table DOM 语义槽位设置行内样式。与 React 语义化 API 对应。 | IReadOnlyDictionary<string, string> | - |
| StylesFunc | 根据当前 Table 实例计算语义样式；Blazor 对应 `styles` 的回调形式。 | Func<Table<TItem>, IReadOnlyDictionary<string, string>> | - |
| TableLayout | 表格元素的 table-layout 属性，设为 fixed 表示内容不会影响列的布局 | string | - |
| OnRowClick | 行点击事件(于antd v3中已废弃) | EventCallback<RowData<TItem>> | - |
| PageIndex | 当前页码（从 1 开始），对应 React 的 `pagination.current`。 | int | 1 |
| PageIndexChanged | 当前页码变化时触发。 | EventCallback<int> | - |
| PageSize | 每页行数，对应 React 的 `pagination.pageSize`。 | int | 10 |
| PageSizeChanged | 每页行数变化时触发。 | EventCallback<int> | - |
| Total | 总记录数，对应 React 的 `pagination.total`。 | int | 0 / 根据数据推断 |
| TotalChanged | 总数变化时触发。 | EventCallback<int> | - |
| HidePagination | 隐藏内置分页器；设置 `PageSize` 后由调用方自行处理分页。 | bool | false |
| PaginationPosition | 内置分页器位置（`topLeft` 至 `bottomRight`）。 | string | bottomRight |
| PaginationPlacement | 分别设置表格顶部/底部的分页器位置；支持 `none`。 | string[] | - |
| PaginationTemplate | 使用自定义模板替换内置分页器，模板接收页大小、页码、总数、CSS 类和变化回调。 | RenderFragment<(int PageSize, int PageIndex, int Total, string PaginationClass, EventCallback<PaginationEventArgs> HandlePageChange)> | - |
| OnPageIndexChange | 页码变化时触发，参数包含分页事件数据。 | EventCallback<PaginationEventArgs> | - |
| OnPageSizeChange | 每页行数变化时触发，参数包含分页事件数据。 | EventCallback<PaginationEventArgs> | - |
| HidePagination| 隐藏分页器，PageSize 等于数据源的行数 | bool | false |
| Resizable | 启用可伸缩列 | bool | false |
| FieldFilterTypeResolver | 用于解析列的筛选器类型 | `IFilterTypeResolver` | 默认由全局注入 |
| Filtered   | 标识数据是否经过过滤，筛选图标会高亮 | bool |  false |

#### React API 映射说明

React 的 `pagination` 对象在 Blazor 中由 `PageIndex`、`PageSize`、`Total`、
`HidePagination`、`PaginationPosition`/`PaginationPlacement` 和
`PaginationTemplate` 组合实现；相关回调为 `PageIndexChanged`、
`PageSizeChanged`、`TotalChanged`、`OnPageIndexChange` 和 `OnPageSizeChange`。
React 的 `expandable` 对象对应 Blazor 的 `ExpandTemplate`、
`DefaultExpandAllRows`、`DefaultExpandedRowKeys`、`DefaultExpandMaxLevel`、
`RowExpandable`、`ExpandedRowClassName`、`OnExpand` 和
`ExpandIconColumnIndex`。这些是 Blazor 的组件参数组合，并非单一对象参数。
React 的 `components` 参数可替换表格内部元素，Blazor Table 当前没有对应 API。

React `columns` 数组在 Blazor 中通过 `<Column>` 子组件或 `ColumnDefinitions`
声明。React `sorter` 配置对应 `Sortable`、`SorterCompare` 和 `SorterMultiple`；
`DefaultSortOrder` 仅设置初始排序状态，因此目前没有受控的 `sortOrder` 参数。
React `filterDropdownProps` 部分对应 `FilterDropdownOpen`、
`FilterDropdownAutoClose` 和 `GetPopupContainer`。`expandedRowRender` 对应
`ExpandTemplate`；受控 `expandedRowKeys`、`expandRowByClick` 和自定义
`expandIcon` 渲染器目前尚未提供。

### Column

先前版本的列定义，对于 .NET 6 及以上版本，建议使用 `PropertyColumn`。

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
| SortIcon | 自定义排序图标；未排序时 `SortDirection?` 上下文为 null，对应 React `sortIcon`。 | RenderFragment<SortDirection?> | - |
| DefaultSortOrder | 默认排序顺序 | [SortDirection](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | - |
| OnCell | 设置单元格属性 | Func<CellData, Dictionary<string, object>> | - |
| OnHeaderCell | 设置头部单元格属性 | Func<Dictionary<string, object>> | - |
| Width | 设置列宽 | string | - |
| MinWidth | 设置列最小宽度（像素）。仅自动表格布局生效，对应 React 的 `minWidth`。 | int? | - |
| RowScope | 设置此列所有单元格的 scope，例如 `row` 或 `rowgroup` | string | - |
| Filterable | 是否显示筛选器 | bool | false |
| Filters | 指定需要筛选菜单的列 | IEnumerable<TableFilter<TData>> | - |
| FilterMultiple | 指定筛选器多选和单选 | bool | true |
| FilterOnClose | 默认筛选面板关闭时是否应用已选值；设为 `false` 时需点击 OK 确认，对应 React `filterOnClose`。 | bool | true |
| DefaultFilteredValue | 列初始化时默认选中的筛选值，对应 React `defaultFilteredValue`。 | IEnumerable<TData> | - |
| FilterResetToDefaultFilteredValue | 设为 true 时，Reset 恢复 `DefaultFilteredValue`；Reset 只修改待确认选择，需点击 OK 应用，与 React 一致。 | bool | false |
| FilterDropdown | 自定义列筛选器模板 | RenderFragment | - |
| FieldFilterType | 筛选器配置 ，可用于自定义额外的筛选器 | `IFieldFilterType` | 由 `FieldFilterTypeResolver` 根据类型解析内置筛选器 |
| OnFilter | 筛选当前数据 | Expression<Func<TData, TData, bool>> | - |
| Grouping | 指定该列用于分组 | bool | - |
| GroupBy | 一个委托，用于转换分组的值 | Func<TData, object> | - |

### PropertyColumn

继承自 `Column`.

| 参数              | 说明             | 类型                         | 默认值 |
| ---------------- | ---------------- | ---------------------------- | ------ |
| Property         |  指定要绑定的属性 | Expression<Func<TItem, TProp>> | - |

### Selection

| 参数              | 说明             | 类型                         | 默认值 |
| ---------------- | ---------------- | ---------------------------- | ------ |
| CheckStrictly    | checkable 状态下节点选择完全受控（父子数据选中状态不再关联）|  bool |  true  |
| Type             |  多选/单选        | `checkbox` \| `radio`      | `checkbox` |
| Disabled         |  是否禁用         |      bool            |   false      |

### GenerateColumns

用于通过 'TItem' 类型的属性自动生成列。 查看 [demo](#components-table-demo-generate-columns).

| Parameter             | Instruction             | Type                         | Defaults |
| ---------------- | ---------------- | ---------------------------- | ------ |
| Definitions | 一个两参数委托。第一个参数是属性名, 第二个是对于的 Column 实例。 | Action<string, object> Definitions |  -  |
| HideColumnsByName | 指定隐藏的属性名。  | IEnumerable<string>   |  -  |
| Range            | 一个范围值，指定某些列可以显示。 | Range |  -  |

### 公开方法

可以使用 `@ref` 绑定 `ITable` 的实例中提供的方法操作 Table。

| 方法名             | 说明                                       | 
| ---------------- | ------------------------------------------- | 
| ReloadData       | 重新加载数据，有三个重载，可指定分页、筛选和排序   | 
| ResetData        | 重置表格分页、筛选、排序状态                    |
| GetQueryModel    | 获取表格状态，可用于持久化和恢复                |
| SetSelection     | 设置选择列                                   |
| SelectAll        | 全选                                        |
| UnselectAll      | 取消全选                                     |
| ExpandAll        | 展开全部列                                   |
| CollapseAll      | 收起所有展开的的列                            |
| ScrollTo         | 根据索引或键滚动到行，也可以滚动到指定的垂直位置；支持 start、center、end 和 nearest 对齐方式 |

### Table ref

| 成员 | 说明 |
| --- | --- |
| NativeElement | 表格最外层的包装元素。 |
| ScrollTo | 接收 `TableScrollToOptions`，可设置 `Index`、`Key`、`Top`、`Offset` 和 `Align`。`Offset` 会将目标行顶边对齐到滚动视口的指定偏移，不能与 `Top` 同时设置。`Align` 支持 `start`、`center`、`end` 和默认的 `nearest`。虚拟滚动不支持居中对齐。 |


### 响应式

表格默认支持响应式，当屏幕宽度小于 960px 时，表格的数据列变为小屏模式。

在小屏模式下，目前只支持一定的表格功能，在有 分组合并、展开列、树形数据、总结栏 等属性的表格中会出现样式错乱。
如果想禁用响应式，可以设置 `Responsive="false"`。
