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


完整的基准测试环境、方法和结果见仓库 `docs/table-quick-render-benchmark.zh-CN.md`。

## API

<!-- API-EXTRA-START -->
### TItem 泛型类型

从 0.16.0 开始，Table 已支持普通类、record、接口和抽象类作为 DataSource 的类型。但有几点需要说明：

- 当使用 record 且有属性值变化，请重写 `GetHashCode()` 方法，或者用 Table 的 `RowKey`，否则内部对每列保持的状态会无法正确工作。[#2837](https://github.com/ant-design-blazor/ant-design-blazor/issues/2837)
- 当使用抽象类时，首次渲染会等待 DataSource 获得元素，并取第一个来初始化。[#3471](https://github.com/ant-design-blazor/ant-design-blazor/issues/3471)

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
<!-- API-EXTRA-END -->
