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

#### onRow usage

Same as `onRow` `onHeaderRow` `onCell` `onHeaderCell`

```razor
<Table TItem="Data" OnRow="OnRow">
    <Column @bind-Field="@context.Name" OnCell="OnCell">
        <a>@context.Name</a>
    </Column>
</Table>

@code {
    Dictionary<string, object> OnRow(RowData<Data> row)
        {
            Action<MouseEventArgs> OnClick = (args) =>
            {
                Console.WriteLine($"row {row.Data.Key} was clicked");
            };

            return new Dictionary<string, object>()
            {
                { "onclick", OnClick },
                { "id", row.Data.Key },
            };
        }
    }

    Dictionary<string, object> OnCell(RowData rowData)
    {
        var row = (RowData<Data>)rowData;

        Action<MouseEventArgs> OnClick = args =>
        {
            Console.WriteLine($"cell {row.Data.Name} was clicked");
        };

        return new Dictionary<string, object>()
        {
            { "onclick", OnClick },
        };
    }
```
