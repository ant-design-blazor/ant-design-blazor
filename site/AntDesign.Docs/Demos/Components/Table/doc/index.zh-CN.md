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


### 快速渲染

`Table` 默认启用快速渲染，以减少每个单元格创建的组件实例。建议使用列的 `CellRender` 获取当前单元格数据：

```razor
<PropertyColumn Property="person => person.Name" Title="Name">
    <CellRender Context="cell">
        <a>@cell.FieldValue</a>
    </CellRender>
</PropertyColumn>
```

普通列 `ChildContent` 仍受支持。为保持旧有 `@context` 行为，使用这种写法时表格会逐行执行列模板，并记录 warning 日志建议改用 `CellRender`；该兼容路径会降低快速渲染的性能收益。需要强制使用标准渲染路径时，设置 `Quick="false"`。

#### 基准测试

基准程序使用 Blazor `HtmlRenderer` 对比 `Quick=true/false`，列均使用 `CellRender`。普通渲染覆盖 100、1,000 行；虚拟化覆盖 100、1,000、10,000 行。列数为 8 或 16。虚拟化场景模拟 400px 视口，实际渲染 28 行（包含 overscan）。每次基准运行先预热 1 次，再取 7 次（100 行）、5 次（1,000 行）或 3 次（10,000 行）的中位数；表格数据取三次 Release 运行结果的中位数。结果来自 Apple M4 Pro、macOS 27.0.1，使用 .NET SDK 11.0.100 和 .NET runtime 11.0.0：

| 行数 | 列数 | 虚拟化 | 本次渲染行数 | Quick=true 耗时 / 分配量 | Quick=false 耗时 / 分配量 | 耗时 / 分配量减少 |
| ---: | ---: | :---: | ---: | ---: | ---: | ---: |
| 100 | 8 | 否 | 100 | 21.59 ms / 4,394,640 B | 84.88 ms / 19,761,656 B | 74.6% / 77.8% |
| 100 | 16 | 否 | 100 | 40.62 ms / 6,244,304 B | 179.26 ms / 36,721,344 B | 77.3% / 83.0% |
| 100 | 8 | 是 | 28 | 3.65 ms / 738,376 B | 16.50 ms / 4,066,952 B | 77.9% / 81.8% |
| 100 | 16 | 是 | 28 | 4.04 ms / 1,084,848 B | 28.83 ms / 8,035,736 B | 86.0% / 86.5% |
| 1,000 | 8 | 否 | 1,000 | 183.21 ms / 48,614,608 B | 797.77 ms / 198,916,672 B | 77.0% / 75.6% |
| 1,000 | 16 | 否 | 1,000 | 130.88 ms / 69,913,560 B | 1,364.42 ms / 374,045,992 B | 90.4% / 81.3% |
| 1,000 | 8 | 是 | 28 | 2.83 ms / 1,086,104 B | 14.44 ms / 4,431,784 B | 80.4% / 75.5% |
| 1,000 | 16 | 是 | 28 | 3.63 ms / 1,415,680 B | 23.18 ms / 8,385,944 B | 84.3% / 83.1% |
| 10,000 | 8 | 是 | 28 | 12.91 ms / 5,162,768 B | 27.36 ms / 8,503,520 B | 52.8% / 39.3% |
| 10,000 | 16 | 是 | 28 | 14.11 ms / 5,490,792 B | 37.40 ms / 12,450,536 B | 62.3% / 55.9% |

这些是组件渲染管线的微基准结果，不包含浏览器布局、绘制或网络耗时。虚拟化通过调用框架的视口填充回调模拟 400px 视口，不测量浏览器 JS 与滚动性能；10,000 行仅测试虚拟化配置，避免完整渲染产生过大的运行开销。虚拟化虽然只渲染 28 行单元格，表格仍需处理完整数据源，因此 10,000 行的分配量会高于 100/1,000 行。计时会受机器负载影响，建议重点比较相同场景下 Quick 开关的相对变化。可通过以下命令重新运行：

```bash
dotnet run --project tests/AntDesign.Benchmarks/AntDesign.Benchmarks.csproj --configuration Release --framework net11.0
```

#### WASM 浏览器基准

下面的基准在 Release Blazor WebAssembly 应用中由 Chromium 实际渲染表格。它从浏览器调用基准页开始计时，在表格完成渲染、浏览器经过两帧并且行数稳定后结束；因此包含 JS 互操作、WASM 渲染和浏览器布局/绘制等待，不包含 WASM 启动、数据请求或网络时间。每个 Quick 模式先预热 1 次，再交替测量 3 次并取中位数。虚拟化场景使用真实 400px 表格视口，实际渲染 30 行。此基准只报告端到端 UI 延迟，不报告内存分配量。

结果来自 Apple M4 Pro、macOS 27.0.0、Chromium 159.0.8154.6、Node.js 24.14.0；使用 .NET SDK 11.0.100 / runtime 11.0.0，Release WASM（未启用 AOT）：

| 行数 | 列数 | 虚拟化 | 本次渲染行数 | Quick=true | Quick=false | 耗时减少 |
| ---: | ---: | :---: | ---: | ---: | ---: | ---: |
| 100 | 8 | 否 | 100 | 264.40 ms | 864.80 ms | 69.4% |
| 100 | 16 | 否 | 100 | 396.40 ms | 1,600.90 ms | 75.2% |
| 100 | 8 | 是 | 30 | 66.10 ms | 215.20 ms | 69.3% |
| 100 | 16 | 是 | 30 | 114.10 ms | 349.00 ms | 67.3% |
| 1,000 | 8 | 否 | 1,000 | 2,434.10 ms | 8,598.80 ms | 71.7% |
| 1,000 | 16 | 否 | 1,000 | 3,841.50 ms | 15,834.50 ms | 75.7% |
| 1,000 | 8 | 是 | 30 | 131.80 ms | 294.80 ms | 55.3% |
| 1,000 | 16 | 是 | 30 | 181.40 ms | 514.10 ms | 64.7% |
| 10,000 | 8 | 是 | 30 | 241.60 ms | 378.90 ms | 36.2% |
| 10,000 | 16 | 是 | 30 | 199.50 ms | 514.70 ms | 61.2% |

计时会受设备、浏览器版本和系统负载影响；这些结果用于同一环境下比较 Quick 开关，不应视为所有设备的固定性能指标。可按以下步骤重新运行：

```bash
ASPNETCORE_ENVIRONMENT=Production DOTNET_ENVIRONMENT=Production dotnet run --project site/AntDesign.Docs.Wasm/AntDesign.Docs.Wasm.csproj --configuration Release --framework net11.0 --no-launch-profile --urls http://localhost:8183
cd tests/AntDesign.Benchmarks.Wasm
npm ci
npx playwright install chromium
npm run benchmark -- --url=http://localhost:8183/en-US/table-benchmark
```

#### WASM 交互基准

浏览器基准还会测量触发表格更新的交互：翻到下一页、选择单行、全选当前页，以及展开/收起树节点。100 行场景使用 8 列，1,000 行场景使用 16 列。翻页场景每页显示一半数据；选择场景渲染全部数据行；树形场景使用一个根节点及指定数量的子节点，因此展开后渲染 101 或 1,001 行，收起时移除这些子行。

计时从浏览器操作前开始，在目标页、选中状态或行数连续两帧稳定后结束。计时包含事件分发、WebAssembly 组件渲染、DOM 更新和两帧浏览器等待；不包含 WASM 启动及表格初始化，每个场景会在计时前准备好。Quick 两种模式各预热 1 次，交替顺序各测量 3 次并取中位数。大型场景之间会重载两个浏览器页面，避免表格状态残留。

结果来自 Apple M4 Pro、macOS 27.0.0、Chromium 159.0.8154.6、Node.js 24.14.0；使用 .NET SDK 11.0.100 / runtime 11.0.0，Release WASM（未启用 AOT）：

| 交互 | 数据行数 | 列数 | 操作后的渲染行数 | Quick=true | Quick=false | 耗时减少 |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| 翻到下一页 | 100 | 8 | 50 | 154.40 ms | 557.10 ms | 72.3% |
| 翻到下一页 | 1,000 | 16 | 500 | 2,404.10 ms | 10,814.20 ms | 77.8% |
| 选择单行 | 100 | 8 | 100 | 375.10 ms | 924.30 ms | 59.4% |
| 选择单行 | 1,000 | 16 | 1,000 | 7,102.10 ms | 20,000.60 ms | 64.5% |
| 全选当前页 | 100 | 8 | 100 | 519.00 ms | 1,217.60 ms | 57.4% |
| 全选当前页 | 1,000 | 16 | 1,000 | 8,035.20 ms | 22,694.50 ms | 64.6% |
| 展开树节点 | 100 | 8 | 101 | 133.80 ms | 529.10 ms | 74.7% |
| 展开树节点 | 1,000 | 16 | 1,001 | 2,028.90 ms | 9,985.00 ms | 79.7% |
| 收起树节点 | 100 | 8 | 1 | 31.90 ms | 44.30 ms | 28.0% |
| 收起树节点 | 1,000 | 16 | 1 | 104.70 ms | 362.00 ms | 71.1% |

仅运行交互场景时，先按上文方式启动 Release WASM 应用，再运行：

```bash
npm run benchmark -- --url=http://localhost:8183/en-US/table-benchmark --interaction-only
```

## API

### TItem 泛型类型

从 0.16.0 开始，Table 已支持普通类、record、接口和抽象类作为 DataSource 的类型。但有几点需要说明：

- 当使用 record 且有属性值变化，请重写 `GetHasCode()` 方法，或者用 Table 的 `RowKey`，否则内部对每列保持的状态会无法正确工作。[#2837](https://github.com/ant-design-blazor/ant-design-blazor/issues/2837)
- 当使用抽象类时，首次渲染会等待 DataSource 获得元素，并取第一个来初始化。[#3471](https://github.com/ant-design-blazor/ant-design-blazor/issues/3471)

### Table
| 参数             | 说明             | 类型                         | 默认值 |
| ---------------- | ---------------- | ---------------------------- | ------ |
| RerenderStrategy | 重新渲染策略。可用来指定只有当有组件属性被修改时才重新渲染。 | [RerenderStrategy](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/RerenderStrategy.cs) | RerenderStrategy.Always |
| RowTemplate | 行模板 | RenderFragment | - |
| ExpandTemplate | 展开内容模板 | RenderFragment<RowData<TItem>> | - |
| DataSource | 数据来源 | IEnumerable<TItem> | - |
| Quick | 启用快速单元格渲染；默认启用，设为 false 时强制使用标准渲染路径 | bool | true |
| DefaultExpandAllRows | 初始时，是否展开所有行 | bool | false |
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
| Size | 表格尺寸大小 | [TableSize](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableSize.cs) | - |
| Locale | 默认文案设置，目前包括排序、过滤、空数据文案 | [TableLocale](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/table/TableLocale.cs) | LocaleProvider.CurrentLocale.Table |
| Bordered | 是否展示外边框和列边框 | bool | false |
| ScrollX | 设置横向滚动，也可用于指定滚动区域的宽，可以设置为像素值，百分比 | string | - |
| ScrollY | 设置纵向滚动，也可用于指定滚动区域的高，可以设置为像素值 | string | - |
| ScrollBarWidth | 滚动条宽度 | string | "17px" |
| IndentSize | 展示树形数据时，每层缩进的宽度，以 px 为单位 | int | 15 |
| ExpandIconColumnIndex | 自定义展开图标所在列索引 | int | - |
| RowClassName | 表格行的类名 | Func<RowData<TItem>, string> | _ => "" |
| RowKey | 设置比对 Key 来设置默认选中行，作用跟给 TItem 实现重写 `GetHashCode()` 一致。否则默认按引用来比对。 | Func<TItem, object> | - |
| ExpandedRowClassName | 展开行的 className | Func<RowData<TItem>, string> | _ => "" |
| OnExpand | 点击展开图标时触发 | EventCallback<RowData<TItem>> | - |
| SortDirections | 支持的排序方式，覆盖 Table 中 sortDirections | [SortDirection[]](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | SortDirection.Preset.Default |
| TableLayout | 表格元素的 table-layout 属性，设为 fixed 表示内容不会影响列的布局 | string | - |
| OnRowClick | 行点击事件(于antd v3中已废弃) | EventCallback<RowData<TItem>> | - |
| HidePagination| 隐藏分页器，PageSize 等于数据源的行数 | bool | false |
| Resizable | 启用可伸缩列 | bool | false |
| FieldFilterTypeResolver | 用于解析列的筛选器类型 | `IFilterTypeResolver` | 默认由全局注入 |
| Filtered   | 标识数据是否经过过滤，筛选图标会高亮 | bool |  false |

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
| DefaultSortOrder | 默认排序顺序 | [SortDirection](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/components/core/SortDirection.cs) | - |
| OnCell | 设置单元格属性 | Func<CellData, Dictionary<string, object>> | - |
| OnHeaderCell | 设置头部单元格属性 | Func<Dictionary<string, object>> | - |
| Filterable | 是否显示筛选器 | bool | false |
| Filters | 指定需要筛选菜单的列 | IEnumerable<TableFilter<TData>> | - |
| FilterMultiple | 指定筛选器多选和单选 | bool | true |
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


### 响应式

表格默认支持响应式，当屏幕宽度小于 960px 时，表格的数据列变为小屏模式。

在小屏模式下，目前只支持一定的表格功能，在有 分组合并、展开列、树形数据、总结栏 等属性的表格中会出现样式错乱。
如果想禁用响应式，可以设置 `Responsive="false"`。
