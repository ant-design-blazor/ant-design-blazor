// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AntDesign.Core.JsInterop;
using AntDesign.Core.JsInterop.Modules.Components;
using AntDesign.Filters;
using AntDesign.JsInterop;
using AntDesign.TableModels;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Xunit;

namespace AntDesign.Tests.Table
{
    /// <summary>
    /// Component-level regression coverage for the behavior demonstrated by the Table demos.
    /// Demos with the same public feature are covered together so the tests verify behavior rather
    /// than duplicating the demo markup.
    /// </summary>
    public class TableDemoCoverageTests : AntDesignTestBase
    {
        private sealed class Item
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
            public string City { get; set; } = string.Empty;
            public string Group { get; set; } = string.Empty;
            public List<Item> Children { get; set; } = new();
        }

        private static readonly Item[] Items =
        {
            new() { Id = 1, Name = "Charlie", Age = 30, City = "London", Group = "B" },
            new() { Id = 2, Name = "Alice", Age = 20, City = "Paris", Group = "A" },
            new() { Id = 3, Name = "Bob", Age = 25, City = "Rome", Group = "A" },
        };

        private IRenderedComponent<Table<Item>> RenderTable(
            RenderFragment<Item>[] columns,
            Action<ComponentParameterCollectionBuilder<Table<Item>>>? configure = null,
            IReadOnlyList<Item>? dataSource = null)
        {
            return Context.RenderComponent<Table<Item>>(parameters =>
            {
                parameters
                    .Add(table => table.DataSource, dataSource ?? Items)
                    .Add(table => table.ChildContent, item => builder =>
                    {
                        var sequence = 0;
                        foreach (var column in columns)
                        {
                            builder.AddContent(sequence++, column(item));
                        }
                    });
                configure?.Invoke(parameters);
            });
        }

        private static RenderFragment<Item> PropertyColumn<TValue>(
            string title,
            Expression<Func<Item, TValue>> property,
            params (string Name, object Value)[] additionalParameters)
        {
            return _ => builder =>
            {
                builder.OpenComponent<PropertyColumn<Item, TValue>>(0);
                builder.AddAttribute(1, nameof(PropertyColumn<Item, TValue>.Title), title);
                builder.AddAttribute(2, nameof(PropertyColumn<Item, TValue>.Property), property);
                var sequence = 3;
                foreach (var (name, value) in additionalParameters)
                {
                    builder.AddAttribute(sequence++, name, value);
                }
                builder.CloseComponent();
            };
        }

        private static RenderFragment<Item> SelectionColumn() => _ => builder =>
        {
            builder.OpenComponent<Selection>(0);
            builder.CloseComponent();
        };

        private static RenderFragment<Item> DataIndexColumn<TValue>(string title, string dataIndex) => _ => builder =>
        {
            builder.OpenComponent<Column<TValue>>(0);
            builder.AddAttribute(1, nameof(Column<TValue>.Title), title);
            builder.AddAttribute(2, nameof(Column<TValue>.DataIndex), dataIndex);
            builder.CloseComponent();
        };

        private static RenderFragment<Item> ActionColumn(RenderFragment<CellData>? cellRender = null) => _ => builder =>
        {
            builder.OpenComponent<AntDesign.ActionColumn>(0);
            builder.AddAttribute(1, nameof(AntDesign.ActionColumn.Title), "Actions");
            if (cellRender != null)
            {
                builder.AddAttribute(2, nameof(AntDesign.ActionColumn.CellRender), cellRender);
            }
            builder.CloseComponent();
        };

        [Fact]
        public void Basic_data_binding_data_index_and_action_columns_render_rows()
        {
            // Basic, BasicBindField, DataIndex, DataTable, RouterPagination.
            var cut = RenderTable(new[]
            {
                PropertyColumn("Name", item => item.Name),
                PropertyColumn("Age", item => item.Age),
                DataIndexColumn<string>("City", "City"),
                ActionColumn(_ => builder => builder.AddContent(0, "Open")),
            });

            Assert.Equal(new[] { "Charlie", "Alice", "Bob" }, cut.FindAll("tbody tr.ant-table-row").Select(row => row.TextContent.Trim().Split(' ')[0]));
            Assert.Contains("Open", cut.Markup);
            Assert.Contains("Name", cut.Find("thead").TextContent);
            Assert.Contains("Age", cut.Find("thead").TextContent);
            Assert.Contains("Paris", cut.Markup);
        }

        [Fact]
        public void Bordered_size_alignment_and_ellipsis_options_are_applied()
        {
            // Bordered, Size, ColumnAlignment, Ellipsis, FixedColumns.
            var cut = RenderTable(new[]
            {
                PropertyColumn("Name", item => item.Name, (nameof(Column<string>.Align), ColumnAlign.Right)),
                PropertyColumn("City", item => item.City, (nameof(Column<string>.Ellipsis), true), (nameof(Column<string>.Width), "80px")),
            }, parameters => parameters
                .Add(table => table.Bordered, true)
                .Add(table => table.Size, TableSize.Small)
                .Add(table => table.ScrollX, "500px"));

            Assert.Contains("ant-table-bordered", cut.Markup);
            Assert.Contains("ant-table-small", cut.Markup);
            Assert.Contains("text-align: right", cut.Find("tbody tr.ant-table-row td").GetAttribute("style"));
            Assert.Contains("ant-table-cell-ellipsis", cut.Find("tbody tr.ant-table-row td:nth-child(2)").ClassName);
        }

        [Fact]
        public void Table_title_footer_and_empty_template_render()
        {
            // Bordered, Head, empty-state behavior.
            var cut = RenderTable(new[] { PropertyColumn("Name", item => item.Name) }, parameters => parameters
                .Add(table => table.Title, "People")
                .Add(table => table.Footer, "3 records")
                .Add(table => table.HidePagination, true));

            Assert.Equal("People", cut.Find(".ant-table-title").TextContent.Trim());
            Assert.Equal("3 records", cut.Find(".ant-table-footer").TextContent.Trim());

            var empty = RenderTable(new[] { PropertyColumn("Name", item => item.Name) }, parameters => parameters
                .Add(table => table.HidePagination, true)
                .Add(table => table.EmptyTemplate, builder => builder.AddContent(0, "No people")),
                Array.Empty<Item>());
            Assert.Contains("No people", empty.Markup);
        }

        [Fact]
        public void Column_and_row_span_are_preserved()
        {
            // ColspanRowspan, GroupingColumns.
            var cut = RenderTable(new[]
            {
                PropertyColumn("Name", item => item.Name, (nameof(Column<string>.HeaderColSpan), 2), (nameof(Column<string>.RowSpan), 2)),
                PropertyColumn("Age", item => item.Age),
            });

            Assert.Equal("2", cut.Find("thead th").GetAttribute("colspan"));
            Assert.Equal("2", cut.Find("tbody td").GetAttribute("rowspan"));
        }

        [Fact]
        public void Custom_row_and_cell_attributes_are_applied()
        {
            // CustomRowStyle, OnRowAndOnCell.
            var cut = RenderTable(new[]
            {
                PropertyColumn("Name", item => item.Name,
                    (nameof(Column<string>.OnCell), (Func<CellData, Dictionary<string, object>>)(cell => new()
                    {
                        ["data-cell-row"] = ((RowData<Item>)cell.RowData).Data.Name,
                    }))),
            }, parameters => parameters
                .Add(table => table.OnRow, row => new Dictionary<string, object> { ["data-row-id"] = row.Data.Id })
                .Add(table => table.RowClassName, row => row.Data.Age < 25 ? "young" : "adult"));

            Assert.Equal("1", cut.Find("tbody tr").GetAttribute("data-row-id"));
            Assert.Contains("adult", cut.Find("tbody tr").ClassName);
            Assert.Equal("Charlie", cut.Find("tbody td").GetAttribute("data-cell-row"));
        }

        [Fact]
        public void Header_template_and_row_template_can_render_custom_rows()
        {
            // GroupingColumns, RowTemplate.
            var cut = RenderTable(Array.Empty<RenderFragment<Item>>(), parameters => parameters
                .Add(table => table.HeaderTemplate, _ => builder =>
                {
                    builder.OpenComponent<TableRow>(0);
                    builder.AddAttribute(1, nameof(TableRow.ChildContent), (RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenElement(0, "th");
                        rowBuilder.AddContent(1, "Custom header");
                        rowBuilder.CloseElement();
                    }));
                    builder.CloseComponent();
                })
                .Add(table => table.RowTemplate, row => builder =>
                {
                    builder.OpenComponent<TableRow>(0);
                    builder.AddAttribute(1, nameof(TableRow.ChildContent), (RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenComponent<TableCell>(0);
                        rowBuilder.AddAttribute(1, nameof(TableCell.ChildContent), (RenderFragment)(cellBuilder => cellBuilder.AddContent(0, $"Custom {row.Data.Name}")));
                        rowBuilder.CloseComponent();
                    }));
                    builder.CloseComponent();
                }));

            Assert.Contains("Custom header", cut.Find("thead").TextContent);
            Assert.Contains("Custom Charlie", cut.Find("tbody").TextContent);
        }

        [Fact]
        public void Hidden_columns_are_removed_and_can_be_restored()
        {
            // ToggleColumn, Dynamic.
            var columns = new[]
            {
                PropertyColumn("Name", item => item.Name),
                PropertyColumn("Age", item => item.Age, (nameof(Column<int>.Hidden), false)),
            };
            var cut = RenderTable(columns);

            Assert.Contains("Age", cut.Find("thead").TextContent);
            cut.SetParametersAndRender(parameters => parameters.Add(table => table.ChildContent, item => builder =>
            {
                builder.AddContent(0, PropertyColumn("Name", row => row.Name)(item));
                builder.AddContent(1, PropertyColumn("Age", row => row.Age, (nameof(Column<int>.Hidden), true))(item));
            }));
            Assert.DoesNotContain("Age", cut.Find("thead").TextContent);
        }

        [Fact]
        public void Generate_columns_demo_honors_hidden_fields_and_column_definitions()
        {
            var cut = Context.RenderComponent<Table<Item>>(parameters => parameters
                .Add(table => table.DataSource, Items)
                .Add(table => table.HidePagination, true)
                .Add(table => table.ChildContent, _ => builder =>
                {
                    builder.OpenComponent<GenerateColumns<Item>>(0);
                    builder.AddAttribute(1, nameof(GenerateColumns<Item>.HideColumnsByName), new[] { nameof(Item.City) });
                    builder.AddAttribute(2, nameof(GenerateColumns<Item>.Definitions), (Action<string, IFieldColumn>)((name, column) => column.Title = name));
                    builder.CloseComponent();
                }));

            Assert.Contains(nameof(Item.Name), cut.Find("thead").TextContent);
            Assert.Contains(nameof(Item.Age), cut.Find("thead").TextContent);
            Assert.DoesNotContain(nameof(Item.City), cut.Find("thead").TextContent);
        }

        [Fact]
        public void Selection_column_keeps_selection_controls_and_selected_rows()
        {
            // BasicForm, RowSelection, FixedHeader, AutoHeight, Virtualization.
            var selectedRows = Array.Empty<Item>();
            var cut = RenderTable(new[]
            {
                SelectionColumn(),
                PropertyColumn("Name", item => item.Name),
            }, parameters => parameters
                .Add(table => table.SelectedRows, selectedRows)
                .Add(table => table.SelectedRowsChanged, EventCallback.Factory.Create<IEnumerable<Item>>(this, rows => selectedRows = rows.ToArray())));

            Assert.NotEmpty(cut.FindAll(".ant-checkbox-input"));
            Assert.Contains("Charlie", cut.Markup);
        }

        [Fact]
        public void Expand_template_is_shown_for_expandable_rows()
        {
            // Expand, NestedTable.
            var cut = RenderTable(new[] { PropertyColumn("Name", item => item.Name) }, parameters => parameters
                .Add(table => table.ExpandTemplate, row => builder => builder.AddContent(0, $"Details {row.Data.Name}"))
                .Add(table => table.RowExpandable, row => row.Data.Name != "Charlie"));

            Assert.Contains("ant-table-row-expand-icon", cut.Markup);
            Assert.DoesNotContain("Details Charlie", cut.Markup);
        }

        [Fact]
        public void Nested_table_demo_renders_child_table_when_parent_row_is_expanded()
        {
            var parent = new Item { Id = 10, Name = "Parent" };
            parent.Children.Add(new Item { Id = 11, Name = "Nested child" });
            var cut = RenderTable(new[] { PropertyColumn("Name", item => item.Name) }, parameters => parameters
                .Add(table => table.DefaultExpandAllRows, true)
                .Add(table => table.ExpandTemplate, (RenderFragment<RowData<Item>>)(row => builder =>
                {
                    builder.OpenComponent<Table<Item>>(0);
                    builder.AddAttribute(1, nameof(Table<Item>.DataSource), row.Data.Children);
                    builder.AddAttribute(2, nameof(Table<Item>.HidePagination), true);
                    builder.AddAttribute(3, nameof(Table<Item>.ChildContent), (RenderFragment<Item>)(child => childBuilder =>
                        childBuilder.AddContent(0, PropertyColumn("Name", item => item.Name)(child))));
                    builder.CloseComponent();
                })), new[] { parent });

            Assert.Contains("Parent", cut.Markup);
            Assert.Contains("Nested child", cut.Markup);
        }

        [Fact]
        public void Tree_children_and_grouped_rows_are_visible()
        {
            // TreeData, TreeDataWithCircularReference, GroupingRows.
            var parent = new Item { Id = 4, Name = "Parent", Age = 40, Group = "C" };
            parent.Children.Add(new Item { Id = 5, Name = "Child", Age = 10, Group = "C" });
            var treeData = new[] { parent };
            var cut = RenderTable(new[]
            {
                PropertyColumn("Name", item => item.Name),
            }, parameters => parameters
                .Add(table => table.TreeChildren, item => item.Children)
                .Add(table => table.DefaultExpandAllRows, true),
                treeData);

            Assert.Contains("Parent", cut.Markup);
            Assert.Contains("Child", cut.Markup);
            Assert.Contains("ant-table-row-expand-icon", cut.Markup);
        }

        [Fact]
        public void Circular_tree_data_renders_without_losing_selection_or_rows()
        {
            // TreeDataWithCircularReference.
            var first = new Item { Id = 20, Name = "First" };
            var second = new Item { Id = 21, Name = "Second" };
            first.Children.Add(second);
            second.Children.Add(first);
            var cut = RenderTable(new[]
            {
                SelectionColumn(),
                PropertyColumn("Name", item => item.Name),
            }, parameters => parameters
                .Add(table => table.TreeChildren, item => item.Children)
                .Add(table => table.DefaultExpandAllRows, true), new[] { first, second });

            Assert.Contains("First", cut.Markup);
            Assert.Contains("Second", cut.Markup);
            Assert.NotEmpty(cut.FindAll(".ant-checkbox-input"));
        }

        [Fact]
        public void Grouped_rows_render_group_titles_and_values()
        {
            // GroupingRows, GroupingColumns, including Quick rendering.
            var cut = RenderTable(new[]
            {
                PropertyColumn("Group", item => item.Group, (nameof(Column<string>.Grouping), true)),
                PropertyColumn("Name", item => item.Name),
            }, parameters => parameters
                .Add(table => table.HidePagination, true)
                .Add(table => table.Quick, true));

            Assert.Contains("A", cut.Markup);
            Assert.Contains("B", cut.Markup);
            var groupBRow = cut.FindAll("tbody tr.ant-table-row").First(row => row.TextContent.Contains("B"));
            var groupBExpand = groupBRow.QuerySelector(".ant-table-row-expand-icon");
            Assert.NotNull(groupBExpand);
            groupBExpand.Click();
            Assert.Contains("Charlie", cut.Markup);
        }

        [Fact]
        public async Task Sorting_and_multiple_sorting_update_the_visible_row_order()
        {
            // CustomSort, MultipleSorter.
            var cut = RenderTable(new[]
            {
                PropertyColumn("Age", item => item.Age, (nameof(Column<int>.Sortable), true),
                    (nameof(Column<int>.SorterMultiple), 2)),
                PropertyColumn("Name", item => item.Name, (nameof(Column<string>.Sortable), true),
                    (nameof(Column<string>.SorterMultiple), 1)),
            });

            Assert.NotEmpty(cut.FindAll(".ant-table-column-sorter-up"));
            await cut.Find(".ant-table-column-sorter-up").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
            var firstRow = cut.Find("tbody tr.ant-table-row").TextContent;
            Assert.Contains("20", firstRow);
            Assert.Contains("Alice", firstRow);
        }

        [Fact]
        public async Task Filterable_columns_render_filter_controls_and_custom_filter_content()
        {
            // Filter, CustomFieldFilter, CustomFilterPanel, Head.
            JSInterop.SetupVoid("AntDesign.interop.styleHelper.addCls", _ => true);
            JSInterop.Setup<OverlayPosition>(JSInteropConstants.OverlayComponentHelper.AddOverlayToContainer, _ => true)
                .SetResult(new OverlayPosition());
            JSInterop.Setup<OverlayPosition>(JSInteropConstants.OverlayComponentHelper.UpdateOverlayPosition, _ => true)
                .SetResult(new OverlayPosition());
            JSInterop.Setup<DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
                .SetResult(new DomRect());
            var filters = new[]
            {
                new TableFilter<string> { Text = "London", Value = "London" },
                new TableFilter<string> { Text = "Paris", Value = "Paris" },
            };
            var cut = RenderTable(new[]
            {
                PropertyColumn("City", item => item.City,
                    (nameof(Column<string>.Filterable), true),
                    (nameof(Column<string>.Filters), filters),
                    (nameof(Column<string>.FilterDropdown), (RenderFragment<TableFilterDropdownContext>)(_ => builder => builder.AddContent(0, "Custom filter panel")))),
            });

            Assert.Contains("ant-table-filter-trigger", cut.Markup);
            Assert.NotNull(cut.FindComponent<Column<string>>().Instance.FilterDropdown);
            await cut.Find(".ant-table-filter-trigger").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
            Assert.Contains("ant-dropdown-open", cut.Find(".ant-table-filter-trigger").ClassList);
        }

        [Fact]
        public async Task Pagination_callbacks_and_custom_pagination_template_work()
        {
            // PaginationDemo, CustomPagination, CustomSort, Ajax, OData, RestoreQueryState.
            QueryModel<Item>? changedQuery = null;
            var cut = RenderTable(new[] { PropertyColumn("Name", item => item.Name) }, parameters => parameters
                .Add(table => table.PageSize, 1)
                .Add(table => table.OnChange, query => changedQuery = (QueryModel<Item>)query));

            var pager = cut.FindComponent<Pagination>();
            await pager.InvokeAsync(() => pager.Instance.OnChange.InvokeAsync(new PaginationEventArgs(2, 1)));

            Assert.Equal(2, cut.Instance.PageIndex);
            Assert.NotNull(changedQuery);

            var custom = RenderTable(new[] { PropertyColumn("Name", item => item.Name) }, parameters => parameters
                .Add(table => table.PageSize, 1)
                .Add(table => table.PaginationTemplate, context => builder => builder.AddContent(0, $"Page {context.PageIndex}")));
            Assert.Contains("Page 1", custom.Markup);
        }

        [Fact]
        public void Fixed_sticky_resizable_responsive_and_auto_height_options_are_preserved()
        {
            // FixedColumns, FixedColumnsHeader, FixedHeader, Sticky, ResizableColumn, ResponsiveMode, AutoHeight.
            var fixedTable = RenderTable(new[]
            {
                PropertyColumn("Name", item => item.Name,
                    (nameof(Column<string>.Fixed), ColumnFixPlacement.Left),
                    (nameof(Column<string>.Width), "120px")),
                PropertyColumn("Age", item => item.Age),
            }, parameters => parameters
                .Add(table => table.ScrollX, "500px")
                .Add(table => table.ScrollY, "180px")
                .Add(table => table.StickyOffsetHeader, 64)
                .Add(table => table.Resizable, true)
                .Add(table => table.Responsive, true));

            Assert.Contains("ant-table-cell-fix-left", fixedTable.Markup);
            Assert.Contains("ant-table-sticky-holder", fixedTable.Markup);
            Assert.Contains("ant-table-responsive", fixedTable.Markup);
            Assert.True(fixedTable.Instance.Resizable);

            var autoHeight = RenderTable(new[] { PropertyColumn("Name", item => item.Name) }, parameters => parameters
                .Add(table => table.AutoHeight, true));
            Assert.True(autoHeight.Instance.AutoHeight);
        }

        [Fact]
        public void Virtualization_configuration_does_not_change_column_definitions()
        {
            // Virtualization, Ajax, AutoHeight.
            var cut = RenderTable(new[]
            {
                SelectionColumn(),
                PropertyColumn("Name", item => item.Name),
            }, parameters => parameters
                .Add(table => table.EnableVirtualization, true)
                .Add(table => table.ScrollY, "300px")
                .Add(table => table.HidePagination, true));

            Assert.NotEmpty(cut.FindComponents<Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize<RowData<Item>>>());
            Assert.Contains("Name", cut.Find("thead").TextContent);
        }

        [Fact]
        public void Quick_mode_falls_back_for_tree_data()
        {
            // QuickTable compatibility with TreeData.
            var parent = new Item { Id = 8, Name = "Parent", Group = "Root" };
            parent.Children.Add(new Item { Id = 9, Name = "Quick child", Group = "Root" });
            var cut = RenderTable(new[]
            {
                PropertyColumn("Group", item => item.Group),
                PropertyColumn("Name", item => item.Name),
            }, parameters => parameters
                .Add(table => table.Quick, true)
                .Add(table => table.TreeChildren, item => item.Children)
                .Add(table => table.DefaultExpandAllRows, true),
                new[] { parent });

            Assert.Contains("Parent", cut.Markup);
            Assert.Contains("Quick child", cut.Markup);
        }

        [Fact]
        public void Summary_rows_render_after_data_rows()
        {
            // Summary.
            var cut = Context.RenderComponent<Table<Item>>(parameters => parameters
                .Add(table => table.DataSource, Items)
                .Add(table => table.HidePagination, true)
                .Add(table => table.ChildContent, item => builder =>
                {
                    builder.AddContent(0, PropertyColumn("Name", row => row.Name)(item));
                    builder.OpenComponent<SummaryRow>(0);
                    builder.AddAttribute(1, nameof(SummaryRow.ChildContent), (RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenComponent<SummaryCell>(0);
                        rowBuilder.AddAttribute(1, nameof(SummaryCell.ChildContent), (RenderFragment)(cellBuilder => cellBuilder.AddContent(0, "Total")));
                        rowBuilder.CloseComponent();
                    }));
                    builder.CloseComponent();
                }));

            Assert.Contains("Total", cut.Find("tfoot").TextContent);
        }

        [Fact]
        public void Quick_mode_falls_back_for_selection_and_custom_row_templates()
        {
            // QuickTable plus RowSelection and RowTemplate compatibility.
            var cut = RenderTable(new[]
            {
                SelectionColumn(),
                PropertyColumn("Name", item => item.Name),
            }, parameters => parameters
                .Add(table => table.Quick, true));

            Assert.NotEmpty(cut.FindAll(".ant-checkbox-input"));
            Assert.Contains("Charlie", cut.Markup);
        }

        [Fact]
        public void Quick_mode_preserves_property_column_cell_templates()
        {
            // QuickTable with the PropertyColumn templates used by ToggleColumn and EditCell.
            var cut = Context.RenderComponent<Table<Item>>(parameters => parameters
                .Add(table => table.DataSource, Items)
                .Add(table => table.Quick, true)
                .Add(table => table.ChildContent, item => builder =>
                {
                    builder.OpenComponent<PropertyColumn<Item, string>>(0);
                    builder.AddAttribute(1, nameof(PropertyColumn<Item, string>.Title), "Name");
                    builder.AddAttribute(2, nameof(PropertyColumn<Item, string>.Property), (Expression<Func<Item, string>>)(row => row.Name));
                    builder.AddAttribute(3, nameof(PropertyColumn<Item, string>.ChildContent), (RenderFragment)(contentBuilder => contentBuilder.AddContent(0, "custom cell")));
                    builder.CloseComponent();
                }));

            Assert.Equal(Items.Length, cut.FindAll("tbody tr.ant-table-row").Count(row => row.TextContent.Contains("custom cell")));
        }

        [Fact]
        public void QuickTable_demo_uses_quick_rendering_and_keeps_row_values()
        {
            var cut = Context.RenderComponent<QuickTable<Item>>(parameters => parameters
                .Add(table => table.DataSource, Items)
                .Add(table => table.ChildContent, item => builder =>
                    builder.AddContent(0, PropertyColumn("Name", row => row.Name)(item))));

            Assert.True(cut.Instance.Quick);
            Assert.Contains("Charlie", cut.Markup);
            Assert.Contains("Alice", cut.Markup);
        }
    }
}
