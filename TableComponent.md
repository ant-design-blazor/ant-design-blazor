# Table Component

## Table.razor

1. Table.razor文件内宏观声明表格
   1. 使用级联参数向下层组件传递：当前表格组件实例、数据条目类型、列交互实例、是否为初始化状态
   2. 执行`@ChildContent(_fieldModel)`渲染列头
2. 使用RenderFragement委托配置头、行、列等的渲染逻辑
   1. `<Table>`
      1. `@colGroup((this, true))` => `<colgroup/>`
      2. `@header(this)` => `</thead>` 
      3. `<tbody>`
         1. `@body(this, _showItems, 0, _dataSourceCache)`
            1. `showItems.Select((data, index) => (data, index))`
               1. ChildContent: `table.bodyRow(table, level, rowDataCache)`
                  1. 如果当前数据行有子数据行且已展开 (`CurrentDataRaw.Expanded == true`)
                     1. 递归调用 `@body(table, table.SortFilterChildren(childrenData), currentRowData.Level + 1, childrenRowDataCache)` 以渲染子数据行
         
      4. `</tbody>`

   2. `</Table>`

3. 使用 `table.bodyRaw()` 方法生成数据行的 `RenderFragment<TItem data, int index>` 以渲染每一行数据
   1. `<TableRowWrapper RowData="currentRowData">`
      1. `<TableRow @key="currentRowData.Data">`
         1. `@table.ChildContent(currentRowData.Data)`


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
                                @_showItems.Select((data, index) => (data, index)).ForEach(tuple => table.bodyRow(table, level, rowDataCache).Invoke(data, index))
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
            }
        </TableRowWrapper>
    }
  </Template>;
}
```

## Column.razor

1. Column.razor 声明每一个列的结构
   1. `<Column>` 组件作为 `<Table>` 的 `ChildContent` 被定义在其内部，因此在每一行数据上循环执行 `@table.ChildContent()` 时，都将通过 `<Column>` 组件渲染每一行数据的所有列
   2. 使用级联参数 `IsHeader` 和 `IsBody` 等控制 `<Column>` 组件的具体渲染行为
   3. 渲染数据行时
      1. 如果当前列索引等于树状展开按钮的列
         1. 通过一个带 `padding-left: (RawData.Level * Table.IndentSize)px` 的 `<span>` 缩进该列的数据
         2. 如果当前数据行还有子数据行，则绘制一个树状展开按钮，否则绘制一个不可见的空白占位元素
            1. 点击按钮切换当前数据行的 `Expanded` 布尔属性，用于在 `Table` 组件中控制当前行的子数据行的可见性
      2. 根据数据字段表达式获取当前数据行在该列应该显示的数据 `CellData<TData>`
         1. 渲染格式化后的数据

```csharp
if (IsHeader && HeaderColSpan != 0)
{
    <CascadingValue Name="IsHeader" Value="false" IsFixed>
        <th>
        	@FilterToolTipSorter
        </th>
    </CascadingValue>
}
else if (IsBody && RowSpan != 0 && ColSpan != 0)
{
    var cellData = new CellData<TData>(RowData, Field, Format);
    <CascadingValue Name="IsBody" Value="false" IsFixed>
        <td>
            @if (ColIndex == Table.TreeExpandIconColumnIndex && Table.TreeMode)
            {
                <span style="padding-left: (RowData.Level * Table.IndentSize);"></span>
                <button type="button" @onclick="ToggleTreeNode"></button>
            }
 
            // Render cell value
    		@cellData.FormattedValue
        </td>
    </CascadingValue>
}
```

## Lifecycle

### OnInitialized()

1. 初始化 `ColumnContext(this)`
2. 初始化 `Dictionary<TItem, RowData<TItem>> _dataSourceCache` 和 `Dictionary<TItem, List<RowData<TItem>>> _allRowDataCache`

### OnParametersSet()

1. `ReloadAndInvokeChange()`

### OnAfterRenderAsync()

1. 首次渲染所有列后调用 `OnColumnInitialized()`
   1. `ReloadAndInvokeChange()`
2. 非首次渲染后调用 `FinishLoadPage()`

### ReloadAndInvokeChange()

1. `InternalReload()`
   1. `BuildQueryModel()` 根据每个列的过滤、排序、分组规则生成查询模型
   2. 在全局数据集合上应用 `QueryModel` 的排序和过滤规则
   3. 在全局数据集合上应用 `QueryModel` 的分组规则
