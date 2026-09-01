---
category: Experimental
type: Data Display
title: AdvancedFilter
subtitle: Advanced Filter
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/f-SbcX2Lx/Table.svg
---

A generic advanced filter component that auto-resolves properties from a data type and generates appropriate filter inputs. Supports nested condition groups (AND/OR), multiple comparison operators, and works both standalone and with Table.

## When To Use

- Filter a list of data by multiple conditions.
- Build complex query expressions with AND/OR grouping.
- Integrate with Table component to extract column definitions.

## API

### AdvancedFilter

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Fields | Manually specified filterable fields. When null, fields are resolved from TItem's properties. | IReadOnlyList&lt;FilterFieldDescriptor&gt; | auto-resolved |
| FilterExpression | The current filter expression. | Expression&lt;Func&lt;TItem, bool&gt;&gt; | - |
| FilterExpressionChanged | Fires when filter expression changes. | EventCallback&lt;Expression&lt;Func&lt;TItem, bool&gt;&gt;&gt; | - |
| OnFilterChanged | Fires whenever filter conditions change. | EventCallback&lt;Expression&lt;Func&lt;TItem, bool&gt;&gt;&gt; | - |
| ConditionNode | The root condition node tree. | FilterConditionNode | auto-created |
| ConditionNodeChanged | Fires when the condition tree changes. | EventCallback&lt;FilterConditionNode&gt; | - |
| AllowGroup | Whether to allow condition group nesting. | bool | true |
| ShowHeader | Whether to show the "Matching all/any" header. | bool | true |
| Size | Component size. | string | small |
| Locale | Localization. `AdvancedFilterLocale` inherits `TableLocale` and falls back to Table filter locale values when AdvancedFilter-specific values are not supplied. | AdvancedFilterLocale | LocaleProvider.CurrentLocale.AdvancedFilter |
| FilterOptionsLocale | Filter operator localization. Defaults to `Locale.FilterOptions`, so it can share Table filter text. | FilterOptionsLocale | Locale.FilterOptions |
| InputResolver | Resolves the value input by property type and compare operator. Extend it with `Map(...)`. | AdvancedFilterInputResolver | default resolver |

### AdvancedFilter Methods

| Method | Description |
| --- | --- |
| BuildFilterExpression() | Build the current condition tree into an `Expression<Func<TItem, bool>>`. |
| GetConditionModel() | Get a cloned condition tree that can be stored or used as an applied state. |
| ReloadConditionModel(conditionNode, notifyChanged) | Restore a condition tree and optionally notify listeners. |
| SerializeConditionModel() | Serialize the current condition tree to JSON. |
| DeserializeConditionModel(json, notifyChanged) | Restore the current condition tree from JSON and optionally notify listeners. |

### FilterFieldDescriptor

| Property | Description | Type |
| --- | --- | --- |
| PropertyName | Property name on TItem | string |
| DisplayName | Display label | string |
| PropertyType | Property CLR type | Type |
| UnderlyingType | Underlying type (unwrapped from Nullable) | Type |
| FilterType | The field filter type | IFieldFilterType |
| PropertyAccess | Lambda expression to access the property | LambdaExpression |
| InputComponent | Preferred value input component. | AdvancedFilterValueInputKind? |
| ValueOptions | Finite value options used by Select, Radio, or Checkbox. | IReadOnlyList&lt;AdvancedFilterOption&gt; |
| CustomInput | Field-level custom value input, useful for TreeSelect, Cascader, or business-specific editors. | RenderFragment&lt;AdvancedFilterInputContext&gt; |

### Default Input Matching

AdvancedFilter resolves value inputs from a table of rules. The rules match both property value type and compare operator.

| Property value type | Compare operator | Default input |
| --- | --- | --- |
| DateTime, DateOnly, TimeOnly | Between | RangePicker |
| DateTime, DateOnly, TimeOnly | Other supported operators | DatePicker |
| bool | Equals, NotEquals, IsNull, IsNotNull | Switch |
| enum | Equals, NotEquals, Contains, NotContains | EnumSelect |
| numeric | Equals, NotEquals, GreaterThan, LessThan, GreaterThanOrEquals, LessThanOrEquals | InputNumber |
| string | Contains, StartsWith, EndsWith, Equals, NotEquals | Input |
| Guid | Equals, NotEquals | Input |
| field with ValueOptions | configurable | Select by default; can be Radio or Checkbox |

Use `FilterFieldDescriptor.InputComponent` to override the default input for one field. For example, a finite string field can use `Select`, `Radio`, or `Checkbox`. Checkbox values are translated to a collection contains expression.

Use `FilterFieldDescriptor.CustomInput` for structural property inputs such as TreeSelect or Cascader. The custom input receives `AdvancedFilterInputContext`; set `context.Value` when the component value changes.

Use `AdvancedFilter.InputResolver` to replace or extend the global matching table. Call `AdvancedFilterInputResolver.Map(...)` to map `(property type, compare operator)` to an `AdvancedFilterValueInputKind`. The rule object itself is internal; the supported extension surface is `Map`, field-level `InputComponent`, and field-level `CustomInput`.

### Persistence

AdvancedFilter conditions can be serialized like Table's QueryModel restore flow. Use `SerializeConditionModel()` or `AdvancedFilterStateSerializer.Serialize(node)` to save the condition JSON, then call `DeserializeConditionModel(json)` or `AdvancedFilterStateSerializer.Deserialize<TItem>(json, fields)` to restore it. During restore, values are converted back according to `FilterFieldDescriptor`, so selected options, enum values, checkbox arrays, and date ranges can be displayed again.

### Table Interop

AdvancedFilter reuses the Table filter primitives: `TableFilterCompareOperator`, `TableFilterCondition`, `TableFilterInputRenderOptions`, `TableFilter`, and `IFieldFilterType`. Use `AdvancedFilterTableFilterConverter.FromTableFilterModels(queryModel.FilterModel, fields)` to convert native Table filters into an AdvancedFilter condition tree, then call `ReloadConditionModel(...)`.

### FilterConditionNode

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| NodeType | Condition or Group | FilterNodeType | - |
| PropertyName | Property name (for Condition) | string | - |
| CompareOperator | Compare operator (for Condition) | TableFilterCompareOperator | Equals |
| Value | Filter value (for Condition) | object | - |
| LogicalOperator | AND/OR (for Group) | TableFilterCondition | And |
| Children | Child nodes (for Group) | List&lt;FilterConditionNode&gt; | - |
