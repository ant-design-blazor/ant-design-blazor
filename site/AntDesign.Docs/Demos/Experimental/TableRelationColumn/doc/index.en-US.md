---
category: Experimental
type: Data Display
title: RelationColumn
cols: 1
cover: https://gw.alipayobjects.com/zos/antfincdn/lkI2hNEDr2V/Table.svg
---

Automatic relation data loading for Table component, solving the N+1 query problem.

## When To Use

- When a table column needs to display related data (e.g., displaying username via user ID)
- When batch loading of related data is needed to avoid N+1 query issues
- When relation data cache needs to be shared across multiple tables
- When flexible control over relation data loading and rendering logic is required

## Features

- **Batch Loading**: Automatically collects all relation IDs that need to be loaded and loads them in one batch, avoiding N+1 queries
- **Auto Deduplication**: Smart deduplication, same ID is loaded only once
- **Zero Reflection**: Uses delegates to access field values for optimal performance
- **Shared Cache**: Supports sharing relation data cache across tables
- **Three Usage Patterns**: Supports C# class, Razor component, and attribute annotation

## Usage

### Method 1: C# Class Inheriting RelationComponentBase

Create a class inheriting `RelationComponentBase<TItem, TData>`:

```csharp
public class UserNameRelation : RelationComponentBase<Order, int>
{
    protected override Task OnLoadBatch(IEnumerable<int> userIds)
    {
        // Batch load user data
        return Task.CompletedTask;
    }
    
    protected override RenderFragment RenderContent(int userId, Order order)
    {
        // Render content
        return builder => builder.AddContent(0, "Username");
    }
}
```

Use in Table:

```html
<PropertyColumn Property="c=>c.UserId" Title="User">
    <UserNameRelation />
</PropertyColumn>
```

### Method 2: Razor Component

Create a Razor file inheriting `RelationComponentBase<TItem, TData>`:

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

### Method 3: Attribute Annotation

Add `[RelationColumn]` attribute to entity property:

```csharp
public class Product
{
    [RelationColumn(typeof(CategoryNameRelation))]
    public int CategoryId { get; set; }
}
```

## API

### RelationComponentBase<TItem, TData>

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| CurrentRowData | (Razor component only) Current row data | TItem | - |
| CurrentFieldValue | (Razor component only) Current field value | TData | - |

| Method | Description | Parameters | Return |
| --- | --- | --- | --- |
| OnLoadBatch | Batch load relation data (simplified) | IEnumerable&lt;TData&gt; fieldValues | Task |
| OnLoadBatch | Batch load relation data (full version) | IEnumerable&lt;TItem&gt; items, QueryModel queryModel | Task |
| RenderContent | Render cell content | TData fieldValue, TItem item | RenderFragment |
| GetFieldValue | Get field value of specified row | TItem item | TData |

### RelationColumnAttribute

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| ComponentType | Relation component type | Type | - |
| Parameters | Component parameters (optional) | string[] | null |

## Performance Optimization

1. **Batch Loading**: All relation component data loading is triggered uniformly after table data loading, executed in parallel using `Task.WhenAll`
2. **Auto Deduplication**: Framework automatically deduplicates same relation IDs to avoid redundant loading
3. **Zero Reflection**: Uses compile-time generated delegates to access field values, no reflection overhead
4. **Shared Cache**: Cache can be shared across multiple tables via `RelationCache` parameter

## Notes

- `OnLoadBatch` method is automatically called after table data loading, no manual trigger needed
- Razor component approach doesn't need to override `RenderContent` method, just write UI directly in Razor file
- Attribute annotation approach is most concise but relatively limited in functionality, suitable for simple scenarios
- It's recommended to cache relation data in component fields to avoid repeated loading
