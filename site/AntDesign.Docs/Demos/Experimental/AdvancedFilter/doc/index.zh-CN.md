---
category: Experimental
type: 数据展示
title: AdvancedFilter
subtitle: 高级筛选器
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/f-SbcX2Lx/Table.svg
---

基于泛型类型的高级筛选器组件，自动从数据类型解析属性并生成对应类型的筛选输入组件。支持嵌套条件组（且/或）、多种比较运算符，可独立使用也可与 Table 组件结合。

## 何时使用

- 需要对列表数据进行多条件筛选。
- 需要构建复杂的查询表达式，支持 AND/OR 分组。
- 与 Table 组件集成，从列定义中提取筛选字段。

## API

### AdvancedFilter

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Fields | 手动指定可筛选字段列表。为 null 时自动从 TItem 属性解析。 | IReadOnlyList&lt;FilterFieldDescriptor&gt; | 自动解析 |
| FilterExpression | 当前筛选表达式。 | Expression&lt;Func&lt;TItem, bool&gt;&gt; | - |
| FilterExpressionChanged | 筛选表达式变更时触发。 | EventCallback&lt;Expression&lt;Func&lt;TItem, bool&gt;&gt;&gt; | - |
| OnFilterChanged | 筛选条件变更时触发。 | EventCallback&lt;Expression&lt;Func&lt;TItem, bool&gt;&gt;&gt; | - |
| ConditionNode | 条件节点树的根节点。 | FilterConditionNode | 自动创建 |
| ConditionNodeChanged | 条件树变更时触发。 | EventCallback&lt;FilterConditionNode&gt; | - |
| AllowGroup | 是否允许条件组嵌套（括号分组）。 | bool | true |
| ShowHeader | 是否显示"匹配所有/任意条件"头部。 | bool | true |
| Size | 组件尺寸。 | string | small |
| Locale | 本地化配置。 | AdvancedFilterLocale | - |
| FilterOptionsLocale | 筛选运算符本地化配置。 | FilterOptionsLocale | - |
| InputResolver | 根据属性值类型和条件类型解析值输入组件，可通过 `Map(...)` 扩展。 | AdvancedFilterInputResolver | 默认解析器 |

### AdvancedFilter 方法

| 方法 | 说明 |
| --- | --- |
| BuildFilterExpression() | 将当前条件树构建为 `Expression<Func<TItem, bool>>`。 |
| GetConditionModel() | 获取克隆后的条件树，可用于保存或作为已应用状态。 |
| ReloadConditionModel(conditionNode, notifyChanged) | 恢复条件树，并可选择是否通知监听器。 |
| SerializeConditionModel() | 将当前条件树序列化为 JSON。 |
| DeserializeConditionModel(json, notifyChanged) | 从 JSON 恢复当前条件树，并可选择是否通知监听器。 |

### FilterFieldDescriptor

| 参数 | 说明 | 类型 |
| --- | --- | --- |
| PropertyName | TItem 上的属性名 | string |
| DisplayName | 显示标签 | string |
| PropertyType | 属性 CLR 类型 | Type |
| UnderlyingType | 底层类型（从 Nullable 解包） | Type |
| FilterType | 字段筛选类型 | IFieldFilterType |
| PropertyAccess | 访问属性的 Lambda 表达式 | LambdaExpression |
| InputComponent | 字段偏好的值输入组件。 | AdvancedFilterValueInputKind? |
| ValueOptions | 有限值选项，用于 Select、Radio 或 Checkbox。 | IReadOnlyList&lt;AdvancedFilterOption&gt; |
| CustomInput | 字段级自定义值输入，适合 TreeSelect、Cascader 或业务组件。 | RenderFragment&lt;AdvancedFilterInputContext&gt; |

### 默认输入匹配表

AdvancedFilter 通过规则表解析值输入组件，规则同时匹配属性值类型和条件类型。

| 属性值类型 | 条件类型 | 默认组件 |
| --- | --- | --- |
| DateTime、DateOnly、TimeOnly | Between | RangePicker |
| DateTime、DateOnly、TimeOnly | 其他支持的条件 | DatePicker |
| bool | Equals、NotEquals、IsNull、IsNotNull | Switch |
| enum | Equals、NotEquals、Contains、NotContains | EnumSelect |
| numeric | Equals、NotEquals、GreaterThan、LessThan、GreaterThanOrEquals、LessThanOrEquals | InputNumber |
| string | Contains、StartsWith、EndsWith、Equals、NotEquals | Input |
| Guid | Equals、NotEquals | Input |
| 配置了 ValueOptions 的字段 | 可配置 | 默认 Select，可指定 Radio 或 Checkbox |

可以通过 `FilterFieldDescriptor.InputComponent` 覆盖单个字段的默认输入组件。例如，枚举或有限值字符串字段可以配置为 `Select`、`Radio` 或 `Checkbox`。Checkbox 多选值会转换为集合包含表达式。

对于属性结构类输入，例如 TreeSelect、Cascader 或业务专用组件，使用 `FilterFieldDescriptor.CustomInput`。自定义组件会收到 `AdvancedFilterInputContext`，值变化时设置 `context.Value` 即可。

如果需要全局扩展匹配规则，设置 `AdvancedFilter.InputResolver` 并调用 `AdvancedFilterInputResolver.Map(...)`，即可把“属性类型 + 条件类型”映射到指定 `AdvancedFilterValueInputKind`。规则对象本身是 internal；对用户开放的扩展面是 `Map`、字段级 `InputComponent` 和字段级 `CustomInput`。

### 条件持久化

AdvancedFilter 条件可以像 Table 的 QueryModel 一样序列化与恢复。使用 `SerializeConditionModel()` 或 `AdvancedFilterStateSerializer.Serialize(node)` 保存条件 JSON，再调用 `DeserializeConditionModel(json)` 或 `AdvancedFilterStateSerializer.Deserialize<TItem>(json, fields)` 恢复。恢复时会依据 `FilterFieldDescriptor` 转回对应类型，因此已选选项、枚举值、Checkbox 数组和日期范围都可以正确回显。

### Table 互操作

AdvancedFilter 复用 Table 的筛选基础类型：`TableFilterCompareOperator`、`TableFilterCondition`、`TableFilterInputRenderOptions`、`TableFilter` 和 `IFieldFilterType`。使用 `AdvancedFilterTableFilterConverter.FromTableFilterModels(queryModel.FilterModel, fields)` 可将 Table 原生筛选转换为 AdvancedFilter 条件树，然后调用 `ReloadConditionModel(...)` 回显并应用。

### FilterConditionNode

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| NodeType | 条件或组 | FilterNodeType | - |
| PropertyName | 属性名（用于条件） | string | - |
| CompareOperator | 比较运算符（用于条件） | TableFilterCompareOperator | Equals |
| Value | 筛选值（用于条件） | object | - |
| LogicalOperator | 且/或（用于组） | TableFilterCondition | And |
| Children | 子节点（用于组） | List&lt;FilterConditionNode&gt; | - |
