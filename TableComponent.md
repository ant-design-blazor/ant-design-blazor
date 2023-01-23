# Table Component

## Table.razor

1. Razor文件内宏观声明表格
   1. 使用级联参数向下层组件传递：当前表格组件实例、数据条目类型、列交互实例、是否为初始化状态
   2. 执行`@ChildContent(_fieldModel)`渲染列头

2. 使用RenderFragement委托配置头、行、列等的渲染逻辑
   1. `<Table>`
      1. `@colGroup((this, true))` => `<colgroup/>`
      2. `@header(this)` => `</thead>` 
      3. `<tbody>`
         1. `@body(this, _showItems, 0, _dataSourceCache)`

      4. `</tbody>`

   2. `</Table>`

3. 核心渲染行数据的逻辑在于 bodyRow 委托之内

- **<Table>**
  - RenderFragement **body**
    - RenderGragement **bodyRow**
      - **<TableRowWrapper>**
        - **<TableRow>**
          - **@table.ChildContent(currentRowData.Data)**


```html
<Spin>
    <Pagination @if PaginationPosition.Contains("top")/>
    <CascadingValue @this：ITable>
        <CascadingValue @typeof(TItem):Type>
            <CascadingValue @ColumnContext:ColumnContext>
                @ChildContent(_fieldModel)
                @TitleTemplate ?? @Title
                <div class="ant-table-container">
                    <div class="ant-table-content">
                        <table>
                            @colGroup((this, true))
                            // RenderFragment<Table<TItem>> header(this) = table => @<template>
                            <thead class="ant-table-thead">
                                <CascadingValue Name="IsHeader" Value="true" IsFixed>
                                    <CascadingValue Value="table" TValue="ITable" Name="AntDesign.TableRow.Table">
                                        <CascadingValue Value="headerRowAttributes" Name="AntDesign.TableRow.RowAttributes">
                                            <TableRow TItem="TItem">
                                                @table.ChildContent(_fieldModel)
                                            </TableRow>
                                        </CascadingValue>
                                    </CascadingValue>
                                </CascadingValue>
                            </thead>
                            // </template>
                            <tbody class="ant-table-tbody">
                                // Func<Table<TItem>, IEnumerable<TItem>, int, Dictionary<TItem, RowData<TItem>>, RenderFragment> body(this, _showItems, 0, _dataSourceCache) = (table, showItems, level, rowDataCache) => @<Template>
                                @_showItems.Select((data, index) => (data, index))
                    ChildContent="table.bodyRow(table = this, level = 0, rowDataCache = _dataSourceCache)" />
<ForeachLoop Items="showItems.Select((data, index) => (data, index))"
                    ChildContent="table.bodyRow(table, level, rowDataCache)" />
                                // </Template>
                            </tbody>
                            @tfoot(this)
                        </table>
                    </div>
                </div>
                @FooterTemplate ?? @Footer
            </CascadingValue>
        </CascadingValue>
    </CascadingValue>
    <Pagination @if PaginationPosition.Contains("bottom")/>
</Spin>
```

```csharp
@code
{
    Func<Table<TItem>, int, IDictionary<TItem, RowData<TItem>>, RenderFragment<(TItem data, int index)>> bodyRow = table, level, rowDataCache) => tuple =>
    @<Template @key="tuple">
    @{
        // Make sure current row is already in cache
        if (!rowDataCache.ContainsKey(data) || rowDataCache[data] == null)
        {
            rowDataCache[data] = new RowData<TItem>(rowIndex, table.PageIndex, data);
        }
        // Get current row from cache
        var currentRowData = rowDataCache[data];
  
        // Make sure current row is already in all cache
        if (!table._allRowDataCache.ContainsKey(data) || !table._allRowDataCache[data].Contains(currentRowData))
        {
            table._allRowDataCache[data].Add(currentRowData);
        }
  
        // TableRowWrapper
        <TableRowWrapper RowData="currentRowData" RowDataSelectedChanged="table.RowDataSelectedChanged">
            @{
                <CascadingValue Value="true" Name="IsBody" IsFixed>
                    <CascadingValue Value="rowAttributes" Name="AntDesign.TableRow.RowAttributes">
                        <CascadingValue Value="rowClassName" Name="AntDesign.TableRow.RowClassName">
                            // Render Row
                            <TableRow @key="currentRowData.Data" TItem="TItem">
                                @table.ChildContent(currentRowData.Data)
                            </TableRow>
                        </CascadingValue>
                    </CascadingValue>
                </CascadingValue>
 
                // Render Children Data
                @if (currentRowData.HasChildren && currentRowData.Expanded)
                {
                    @table.body(table, table.SortFilterChildren(childrenData), currentRowData.Level + 1, childrenRowDataCache);
                }
                @if (!currentRowData.HasChildren && table.ExpandTemplate != null && table.RowExpandable(currentRowData) && currentRowData.Expanded)
                {
                    <tr>
                        <td>
                            @table.ExpandTemplate(currentRowData)
                        </td>
                    </tr>
                }
            }
        </TableRowWrapper>
    }
  </Template>;
}
```

## Column.razor

```csharp
if (IsHeader && HeaderColSpan != 0)
{
    <CascadingValue Name="IsHeader" Value="false" IsFixed>
        <th>
            @if (Sortable || (_filterable && _filters?.Any() == true))
            {
                @FilterToolTipSorter
            }
            else if (TitleTemplate != null)
            {
                @TitleTemplate
            }
            else
            {
                @Title
            }
        </th>
    </CascadingValue>
}
else if (IsBody && RowSpan != 0 && ColSpan != 0)
{
    var fieldText = !string.IsNullOrWhiteSpace(Format) ? Formatter<TData>.Format(Field, Format) : Field?.ToString();
    @if (AppendExpandColumn)
    {
        <td class="ant-table-cell ant-table-row-expand-icon-cell">
            @if (Table.RowExpandable(RowData) && (!Table.TreeMode || !RowData.HasChildren))
            {
                <button type="button" @onclick="ToggleTreeNode"></button>
            }
        </td>
    }
 
    // Get cell data value from Field (compiled Property expression) property
    var cellData = new CellData<TData>(RowData, Field, Format);
    <CascadingValue Name="IsBody" Value="false" IsFixed>
        <td>
            @if (ColIndex == Table.TreeExpandIconColumnIndex && Table.TreeMode)
            {
                <span></span>
                @if (RowData.HasChildren)
                {
                    <button type="button" @onclick="ToggleTreeNode"></button>
                }
                else
                {
                    <button type="button"></button>
                }
            }
 
            // Render cell value
            @if (CellRender != null)
            {
                @CellRender(cellData)
            }
            else if (ChildContent != null)
            {
                @ChildContent
            }
            else
            {
                @cellData.FormattedValue
            }
        </td>
    </CascadingValue>
}
```

