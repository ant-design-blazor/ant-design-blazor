---
category: Experimental
type: 数据展示
title: RelationColumn
subtitle: Table 关联列
cols: 1
cover: https://gw.alipayobjects.com/zos/antfincdn/lkI2hNEDr2V/Table.svg
---

Table 关联数据自动加载功能,解决 N+1 查询问题。

## 何时使用

- 当表格中某列需要显示关联数据(如通过用户ID显示用户名)时
- 当需要批量加载关联数据以避免 N+1 查询问题时
- 当需要在多个表格间共享关联数据缓存时
- 当需要灵活控制关联数据的加载和渲染逻辑时

## 功能特性

- **批量加载**: 自动收集所有需要加载的关联ID,一次性批量加载,避免 N+1 查询
- **自动去重**: 智能去重,相同ID只加载一次
- **零反射**: 使用委托方式访问字段值,性能最优
- **共享缓存**: 支持跨表格共享关联数据缓存
- **三种用法**: 支持 C# 类、Razor 组件、特性标注三种开发方式

## 使用方式

### 方式1: C# 类继承 RelationComponentBase

创建一个类继承 `RelationComponentBase<TItem, TData>`:

```csharp
public class UserNameRelation : RelationComponentBase<Order, int>
{
    protected override Task OnLoadBatch(IEnumerable<int> userIds)
    {
        // 批量加载用户数据
        return Task.CompletedTask;
    }
    
    protected override RenderFragment RenderContent(int userId, Order order)
    {
        // 渲染内容
        return builder => builder.AddContent(0, "用户名");
    }
}
```

在 Table 中使用:

```html
<PropertyColumn Property="c=>c.UserId" Title="用户">
    <UserNameRelation />
</PropertyColumn>
```

### 方式2: Razor 组件

创建 Razor 文件继承 `RelationComponentBase<TItem, TData>`:

```razor
@inherits RelationComponentBase<Employee, int>

@if (departments.TryGetValue(CurrentFieldValue, out var dept))
{
    <Tag>@dept.Name</Tag>
}

@code {
    protected override Task OnLoadBatch(IEnumerable<int> ids) 
    { 
        return Task.CompletedTask; 
    }
}
```

### 方式3: 特性标注

在实体类属性上添加 `[RelationColumn]` 特性:

```csharp
public class Product
{
    [RelationColumn(typeof(CategoryNameRelation))]
    public int CategoryId { get; set; }
}
```

## API

### RelationComponentBase<TItem, TData>

| 属性 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| CurrentRowData | (Razor组件专用) 当前行数据 | TItem | - |
| CurrentFieldValue | (Razor组件专用) 当前字段值 | TData | - |

| 方法 | 说明 | 参数 | 返回值 |
| --- | --- | --- | --- |
| OnLoadBatch | 批量加载关联数据(简化版) | IEnumerable&lt;TData&gt; fieldValues | Task |
| OnLoadBatch | 批量加载关联数据(完整版) | IEnumerable&lt;TItem&gt; items, QueryModel queryModel | Task |
| RenderContent | 渲染单元格内容 | TData fieldValue, TItem item | RenderFragment |
| GetFieldValue | 获取指定行的字段值 | TItem item | TData |

### RelationColumnAttribute

| 属性 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| ComponentType | 关联组件类型 | Type | - |
| Parameters | 组件参数(可选) | string[] | null |

## 性能优化

1. **批量加载**: 所有关联组件的数据加载会在表格数据加载完成后统一触发,使用 `Task.WhenAll` 并行执行
2. **自动去重**: 框架会自动去重相同的关联ID,避免重复加载
3. **零反射**: 使用编译时生成的委托访问字段值,无反射开销
4. **共享缓存**: 通过 `RelationCache` 参数可以在多个表格间共享缓存

## 注意事项

- `OnLoadBatch` 方法会在表格数据加载后自动调用,无需手动触发
- Razor 组件方式不需要重写 `RenderContent` 方法,直接在 Razor 文件中编写 UI
- 特性标注方式最简洁,但功能相对有限,适合简单场景
- 建议将关联数据缓存到组件的字段中,避免重复加载
