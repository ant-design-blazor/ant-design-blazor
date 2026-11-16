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


The full benchmark methodology, environment, and results are in `docs/table-quick-render-benchmark.en-US.md` in the repository.

## API

<!-- API-EXTRA-START -->
### The TItem Generic type parameter

Since 0.16.0, Table has supported ordinary classes, records, interfaces, and abstract classes as DataSource types. There are a few caveats:

- When using a record and changing property values, override `GetHashCode()` or set the Table `RowKey`; otherwise, the state maintained internally for each column may not work correctly. [#2837](https://github.com/ant-design-blazor/ant-design-blazor/issues/2837)
- When using abstract classes, the first render waits for the DataSource to contain an item and uses its first item for initialization. [#3471](https://github.com/ant-design-blazor/ant-design-blazor/issues/3471)

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
<!-- API-EXTRA-END -->
