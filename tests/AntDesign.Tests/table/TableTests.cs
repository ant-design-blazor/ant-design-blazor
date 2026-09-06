// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.TableModels;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

#pragma warning disable CS0612, CS0618

namespace AntDesign.Tests.Table
{
    public class TableTests : AntDesignTestBase
    {
        private sealed class Person
        {
            public int Id { get; set; }
            public string Name { get; set; } = default!;
            public string Surname { get; set; } = default!;
        }


        private IRenderedComponent<Table<Person>> CreatePersonsTable(
            IReadOnlyList<Person> persons,
            Action<ComponentParameterCollectionBuilder<Table<Person>>>? callback = null,
            bool enableSelection = false)
        {
            return Context.RenderComponent<Table<Person>>(x =>
            {
                x
                    .Add(b => b.DataSource, persons)
                    .Add(b => b.ChildContent, p =>
                    {
                        var selection = new ComponentParameterCollectionBuilder<Selection>()
                                .Add(q => q.Key, p.Id.ToString())
                                .Build()
                                .ToRenderFragment<Selection>();

                        var nameCol = new ComponentParameterCollectionBuilder<Column<string>>()
                                .Add(q => q.Field, p.Name)
                                .Build()
                                .ToRenderFragment<Column<string>>();

                        var surnameCol = new ComponentParameterCollectionBuilder<Column<string>>()
                                .Add(q => q.Field, p.Surname)
                                .Build()
                                .ToRenderFragment<Column<string>>();

                        return builder =>
                        {
                            if (enableSelection) selection(builder);
                            nameCol(builder);
                            surnameCol(builder);
                        };
                    }
                    );

                callback?.Invoke(x);
            }
            );
        }

        [Fact]
        public void Renders_an_empty_table()
        {
            var persons = Array.Empty<Person>();

            var cut = CreatePersonsTable(persons);

            Assert.Single(cut.FindAll("tbody tr.ant-table-placeholder"));
            Assert.Contains("No Data", cut.Find("tbody").TextContent);
        }

        [Fact]
        public void Renders_a_table_with_two_rows()
        {
            var persons = new[]
            {
                new Person {Id = 1, Name = "John", Surname = "Smith"},
                new Person {Id = 2, Name = "Jane", Surname = "Doe"}
            };

            var cut = CreatePersonsTable(persons);

            var rows = cut.FindAll("tbody tr.ant-table-row");
            Assert.Equal(2, rows.Count);
            Assert.Contains("John", rows[0].TextContent);
            Assert.Contains("Jane", rows[1].TextContent);
        }

        [Fact]
        public void Can_render_after_changes_to_the_dataSource()
        {
            var persons = new List<Person>
            {
                new Person {Id = 1, Name = "John", Surname = "Smith"},
                new Person {Id = 2, Name = "Jane", Surname = "Doe"},
                new Person {Id = 3, Name = "Joe", Surname = "Doe"}
            };

            var cut = CreatePersonsTable(persons, b => b
                .Add(q => q.PageSize, 1)
                .Add(q => q.PageIndex, 3)
            );

            persons.RemoveAt(0);

            cut.SetParametersAndRender(b => b
                .Add(q => q.DataSource, persons)
                .Add(q => q.PageIndex, 2));

            var row = Assert.Single(cut.FindAll("tbody tr.ant-table-row"));
            Assert.Contains("Joe", row.TextContent);
        }

        [Fact]
        public void Set_colspan_and_rowspan()
        {
            var persons = new[]
             {
                new Person {Id = 1, Name = "John", Surname = "Smith"},
                new Person {Id = 2, Name = "Jane", Surname = "Doe"}
            };

            var cut = Context.RenderComponent<Table<Person>>(x =>
            {
                x.Add(b => b.DataSource, persons)
                .Add(b => b.ChildContent, p =>
                {
                    var selection = new ComponentParameterCollectionBuilder<Selection>()
                        .Add(q => q.Key, p.Id.ToString())
                        .Add(q => q.HeaderColSpan, 2)
                        .Add(q => q.ColSpan, 1)
                        .Add(q => q.RowSpan, 2)
                        .Build()
                        .ToRenderFragment<Selection>();

                    var nameCol = new ComponentParameterCollectionBuilder<Column<string>>()
                        .Add(q => q.Field, p.Name)
                        .Add(q => q.HeaderColSpan, 0)
                        .Add(q => q.ColSpan, 2)
                        .Add(q => q.RowSpan, 1)
                        .Build()
                        .ToRenderFragment<Column<string>>();

                    var surnameCol = new ComponentParameterCollectionBuilder<Column<string>>()
                        .Add(q => q.Field, p.Surname)
                        .Add(q => q.HeaderColSpan, 1)
                        .Add(q => q.ColSpan, 0)
                        .Add(q => q.RowSpan, 0)
                        .Build()
                        .ToRenderFragment<Column<string>>();

                    return builder =>
                    {
                        nameCol(builder);
                        surnameCol(builder);
                    };
                });
            });

            Assert.Single(cut.FindAll("thead th"));
            var cells = cut.FindAll("tbody td");
            Assert.Equal(2, cells.Count);
            Assert.All(cells, cell => Assert.Equal("2", cell.GetAttribute("colspan")));
        }

        [Fact]
        public async Task Can_select_all_unselect_all_and_set_selection()
        {
            var persons = new[]
            {
                new Person { Id = 1, Name = "John", Surname = "Smith" },
                new Person { Id = 2, Name = "Jane", Surname = "Doe" }
            };
            IEnumerable<Person>? changedRows = null;
            var cut = CreatePersonsTable(persons, x => x
                .Add(q => q.SelectedRowsChanged, rows => changedRows = rows), enableSelection: true);

            await cut.InvokeAsync(() => cut.Instance.SelectAll());
            Assert.Equal(persons, changedRows);
            Assert.Equal(2, cut.FindAll("tbody tr.ant-table-row-selected").Count);

            await cut.InvokeAsync(() => cut.Instance.UnselectAll());
            Assert.Empty(changedRows!);
            Assert.Empty(cut.FindAll("tbody tr.ant-table-row-selected"));

            await cut.InvokeAsync(() => cut.Instance.SetSelection(persons[1]));
            Assert.Equal(new[] { persons[1] }, changedRows);
            Assert.Single(cut.FindAll("tbody tr.ant-table-row-selected"));
        }

        [Fact]
        public async Task Can_expand_and_collapse_rows_with_an_expand_template()
        {
            var persons = new[]
            {
                new Person { Id = 1, Name = "John", Surname = "Smith" },
                new Person { Id = 2, Name = "Jane", Surname = "Doe" }
            };
            var cut = CreatePersonsTable(persons, x => x
                .Add(q => q.ExpandTemplate, row => $"Details for {row.Data.Name}".ToRenderFragment()));

            Assert.Empty(cut.FindAll(".ant-table-expanded-row"));

            await cut.Find("button[aria-label='Expand row']").ClickAsync(new MouseEventArgs());
            Assert.Contains("Details for John", cut.Find(".ant-table-expanded-row").TextContent);

            cut.InvokeAsync(() => cut.Instance.ExpandAll());
            Assert.Equal(2, cut.FindAll(".ant-table-expanded-row").Count);

            cut.InvokeAsync(() => cut.Instance.CollapseAll());
            Assert.Empty(cut.FindAll(".ant-table-expanded-row"));
        }

        [Fact]
        public async Task ReloadData_can_change_page_size_and_page_index_and_raise_callbacks()
        {
            var persons = Enumerable.Range(1, 5)
                .Select(id => new Person { Id = id, Name = $"Person {id}", Surname = "Test" })
                .ToArray();
            var changedIndexes = new List<int>();
            var changedSizes = new List<int>();
            var queries = new List<QueryModel<Person>>();
            var cut = CreatePersonsTable(persons, x => x
                .Add(q => q.PageIndexChanged, index => changedIndexes.Add(index))
                .Add(q => q.PageSizeChanged, size => changedSizes.Add(size))
                .Add(q => q.OnChange, query => queries.Add(query)));

            await cut.InvokeAsync(() => cut.Instance.ReloadData(2, 2));

            Assert.Equal(new[] { 2 }, changedIndexes);
            Assert.Equal(new[] { 2 }, changedSizes);
            var query = Assert.IsType<QueryModel<Person>>(Assert.Single(queries, q => q.PageIndex == 2));
            Assert.Equal(2, query.PageIndex);
            Assert.Equal(2, query.PageSize);
            Assert.Equal(2, query.StartIndex);
            var rows = cut.FindAll("tbody tr.ant-table-row");
            Assert.Contains(rows, row => row.TextContent.Contains("Person 3"));
            Assert.Contains(rows, row => row.TextContent.Contains("Person 4"));
            Assert.DoesNotContain(rows, row => row.TextContent.Contains("Person 1"));
        }

        [Fact]
        public void Fixed_columns_apply_sticky_styles_and_scroll_classes()
        {
            var persons = new[]
            {
                new Person { Id = 1, Name = "John", Surname = "Smith" }
            };
            var cut = Context.RenderComponent<Table<Person>>(x => x
                .Add(q => q.DataSource, persons)
                .Add(q => q.ScrollX, "1000px")
                .Add(q => q.ChildContent, person => builder =>
                {
                    builder.AddContent(0, new ComponentParameterCollectionBuilder<Column<string>>()
                        .Add(q => q.Field, person.Name)
                        .Add(q => q.Width, "100px")
                        .Add(q => q.Fixed, ColumnFixPlacement.Left)
                        .Build()
                        .ToRenderFragment<Column<string>>());
                    builder.AddContent(1, new ComponentParameterCollectionBuilder<Column<string>>()
                        .Add(q => q.Field, person.Surname)
                        .Add(q => q.Width, "100px")
                        .Add(q => q.Fixed, ColumnFixPlacement.Right)
                        .Build()
                        .ToRenderFragment<Column<string>>());
                }));

            Assert.Contains("ant-table-scroll-horizontal", cut.Find(".ant-table").ClassList);
            var headers = cut.FindAll("thead th");
            Assert.Contains(headers, cell => cell.GetAttribute("style")?.Contains("position: sticky") == true && cell.GetAttribute("style")!.Contains("left: 0px"));
            Assert.Contains(headers, cell => cell.GetAttribute("style")?.Contains("position: sticky") == true && cell.GetAttribute("style")!.Contains("right: 0px"));
        }

        [Fact]
        public void Summary_rows_render_with_fixed_cell_styles()
        {
            var persons = new[]
            {
                new Person { Id = 1, Name = "John", Surname = "Smith" }
            };
            var cut = Context.RenderComponent<Table<Person>>(x => x
                .Add(q => q.DataSource, persons)
                .Add(q => q.HidePagination, true)
                .Add(q => q.ChildContent, person => builder =>
                {
                    builder.AddContent(0, new ComponentParameterCollectionBuilder<Column<string>>()
                        .Add(q => q.Field, person.Name)
                        .Add(q => q.Fixed, ColumnFixPlacement.Left)
                        .Build()
                        .ToRenderFragment<Column<string>>());
                    builder.AddContent(1, new ComponentParameterCollectionBuilder<Column<string>>()
                        .Add(q => q.Field, person.Surname)
                        .Build()
                        .ToRenderFragment<Column<string>>());
                    builder.AddContent(2, new ComponentParameterCollectionBuilder<SummaryRow>()
                        .Add(q => q.ChildContent, builder =>
                        {
                            builder.AddContent(0, new ComponentParameterCollectionBuilder<SummaryCell>()
                                .Add(q => q.Fixed, ColumnFixPlacement.Left)
                                .Add(q => q.ChildContent, "Total".ToRenderFragment())
                                .Build()
                                .ToRenderFragment<SummaryCell>());
                            builder.AddContent(1, new ComponentParameterCollectionBuilder<SummaryCell>()
                                .Add(q => q.ColSpan, 2)
                                .Add(q => q.ChildContent, "1 person".ToRenderFragment())
                                .Build()
                                .ToRenderFragment<SummaryCell>());
                        })
                        .Build()
                        .ToRenderFragment<SummaryRow>());
                }));

            var summaryRow = Assert.Single(cut.FindAll("tfoot tr"));
            Assert.Contains("Total", summaryRow.TextContent);
            Assert.Contains("1 person", summaryRow.TextContent);
            Assert.Equal(2, summaryRow.QuerySelectorAll("td").Length);
            var fixedCell = summaryRow.QuerySelector("td.ant-table-cell-fix-left");
            Assert.NotNull(fixedCell);
            Assert.Contains("position: sticky", fixedCell!.GetAttribute("style"));
            Assert.Empty(cut.FindAll(".ant-pagination"));
        }

        [Fact]
        public void Action_column_renders_content_and_fixed_header()
        {
            var persons = new[]
            {
                new Person { Id = 1, Name = "John", Surname = "Smith" }
            };
            var cut = Context.RenderComponent<Table<Person>>(x => x
                .Add(q => q.DataSource, persons)
                .Add(q => q.ChildContent, person => builder =>
                {
                    builder.AddContent(0, new ComponentParameterCollectionBuilder<Column<string>>()
                        .Add(q => q.Field, person.Name)
                        .Build()
                        .ToRenderFragment<Column<string>>());
                    builder.AddContent(1, new ComponentParameterCollectionBuilder<ActionColumn>()
                        .Add(q => q.Title, "Action")
                        .Add(q => q.Width, "100px")
                        .Add(q => q.Fixed, ColumnFixPlacement.Right)
                        .Add(q => q.ChildContent, "edit".ToRenderFragment())
                        .Build()
                        .ToRenderFragment<ActionColumn>());
                }));

            var headers = cut.FindAll("thead th");
            Assert.Contains(headers, cell => cell.TextContent.Contains("Action") && cell.GetAttribute("style")?.Contains("position: sticky") == true && cell.GetAttribute("style")!.Contains("right: 0px"));
            Assert.Contains(cut.FindAll("tbody td"), cell => cell.TextContent.Contains("edit"));
        }

        [Fact]
        public void Simple_table_header_maps_colspan_and_child_content_to_header()
        {
            var persons = new[]
            {
                new Person { Id = 1, Name = "John", Surname = "Smith" }
            };
            var cut = Context.RenderComponent<Table<Person>>(x => x
                .Add(q => q.DataSource, persons)
                .Add(q => q.ChildContent, person => builder =>
                {
                    builder.AddContent(0, new ComponentParameterCollectionBuilder<SimpleTableHeader>()
                        .Add(q => q.Title, "Fallback")
                        .Add(q => q.ColSpan, 2)
                        .Add(q => q.ChildContent, "Custom header".ToRenderFragment())
                        .Build()
                        .ToRenderFragment<SimpleTableHeader>());
                    builder.AddContent(1, new ComponentParameterCollectionBuilder<Column<string>>()
                        .Add(q => q.Field, person.Name)
                        .Build()
                        .ToRenderFragment<Column<string>>());
                }));

            var header = cut.Find("thead th");
            Assert.Equal("2", header.GetAttribute("colspan"));
            Assert.Contains("Custom header", header.TextContent);
            Assert.DoesNotContain("Fallback", header.TextContent);
        }

        [Fact]
        public async Task Custom_pagination_template_receives_context_and_handles_page_changes()
        {
            var persons = Enumerable.Range(1, 4)
                .Select(id => new Person { Id = id, Name = $"Person {id}", Surname = "Test" })
                .ToArray();
            var changedIndexes = new List<int>();
            var cut = CreatePersonsTable(persons, x => x
                .Add(q => q.PageSize, 2)
                .Add(q => q.PageIndexChanged, index => changedIndexes.Add(index))
                .Add(q => q.PaginationTemplate, context => new RenderFragment((RenderTreeBuilder builder) =>
                {
                    builder.AddContent(0, $"Total {context.Total}");
                    builder.OpenElement(1, "button");
                    builder.AddAttribute(2, "type", "button");
                    builder.AddAttribute(3, "data-custom-pagination", "true");
                    builder.AddAttribute(4, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => context.HandlePageChange.InvokeAsync(new PaginationEventArgs(2, context.PageSize))));
                    builder.AddContent(5, "Next page");
                    builder.CloseElement();
                })));

            Assert.Contains("Total 4", cut.Markup);
            await cut.InvokeAsync(() => cut.Find("button[data-custom-pagination]").Click());

            Assert.Equal(new[] { 2 }, changedIndexes);
            var rows = cut.FindAll("tbody tr.ant-table-row");
            Assert.Equal(2, rows.Count);
            Assert.Contains(rows, row => row.TextContent.Contains("Person 3"));
            Assert.Contains(rows, row => row.TextContent.Contains("Person 4"));
        }

        [Fact]
        public async Task Grouping_column_renders_group_rows_and_expands_children()
        {
            var persons = new[]
            {
                new Person { Id = 1, Name = "John", Surname = "Smith" },
                new Person { Id = 2, Name = "Jane", Surname = "Smith" },
                new Person { Id = 3, Name = "Joe", Surname = "Doe" }
            };
            var cut = CreatePersonsTable(persons, x => x
                .Add(q => q.GroupTitleTemplate, group => $"Group {group.Key}: {group.Items.Count}".ToRenderFragment()));

            cut.SetParametersAndRender(parameters => parameters
                .Add(q => q.ChildContent, person => builder =>
                {
                    builder.AddContent(0, new ComponentParameterCollectionBuilder<PropertyColumn<Person, string>>()
                        .Add(q => q.Property, item => item.Surname)
                        .Add(q => q.Grouping, true)
                        .Build()
                        .ToRenderFragment<PropertyColumn<Person, string>>());
                }));

            var groupRows = cut.FindAll("tbody tr.ant-table-row-grouping");
            Assert.Equal(2, groupRows.Count);
            Assert.Contains(groupRows, row => row.TextContent.Contains("Group Smith: 2"));
            Assert.Contains(groupRows, row => row.TextContent.Contains("Group Doe: 1"));

            await cut.InvokeAsync(() => cut.Find("button[aria-label='Expand row']").Click());
            Assert.Equal(2, cut.FindAll("tbody tr.ant-table-row-level-1").Count);
        }

        [Fact]
        public async Task Can_render_tree_children_and_collapse_the_tree()
        {
            var persons = new[]
            {
                new Person { Id = 1, Name = "Parent", Surname = "Root" },
                new Person { Id = 2, Name = "Child", Surname = "Leaf" }
            };
            var cut = CreatePersonsTable(persons, x => x
                .Add(q => q.TreeChildren, person => person.Id == 1 ? new[] { persons[1] } : Array.Empty<Person>()));

            Assert.Equal(2, cut.FindAll("tbody tr.ant-table-row").Count);

            await cut.Find("button[aria-label='Expand row']").ClickAsync(new MouseEventArgs());
            Assert.Equal(3, cut.FindAll("tbody tr.ant-table-row").Count);
            var childRow = Assert.Single(cut.FindAll("tbody tr.ant-table-row-level-1"));
            Assert.Contains("Child", childRow.TextContent);

            cut.InvokeAsync(() => cut.Instance.CollapseAll());
            Assert.Equal(2, cut.FindAll("tbody tr.ant-table-row").Count);
        }
    }
}
