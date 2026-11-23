// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AntDesign.Core.JsInterop.Modules.Components;
using AntDesign.JsInterop;
using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

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
            bool enableSelection = false,
            IEnumerable<TableFilter<string>>? nameFilters = null,
            bool closeFilterOnOutsideClick = true,
            bool filterOnClose = true,
            int? surnameMinWidth = null,
            bool sortableName = false,
            RenderFragment<SortDirection?>? sortIcon = null)
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

                        var nameColumnBuilder = new ComponentParameterCollectionBuilder<Column<string>>()
                                .Add(q => q.Field, p.Name);
                        if (nameFilters != null)
                        {
                            nameColumnBuilder.Add(q => q.Filters, nameFilters);
                            nameColumnBuilder.Add(q => q.FilterDropdownAutoClose, closeFilterOnOutsideClick);
                            nameColumnBuilder.Add(q => q.FilterOnClose, filterOnClose);
                        }
                        if (sortableName)
                        {
                            nameColumnBuilder.Add(q => q.Sortable, true);
                        }
                        if (sortIcon != null)
                        {
                            nameColumnBuilder.Add(q => q.SortIcon, sortIcon);
                        }
                        var nameCol = nameColumnBuilder.Build().ToRenderFragment<Column<string>>();

                        var surnameColumnBuilder = new ComponentParameterCollectionBuilder<Column<string>>()
                                .Add(q => q.Field, p.Surname);
                        if (surnameMinWidth.HasValue)
                        {
                            surnameColumnBuilder.Add(q => q.MinWidth, surnameMinWidth.Value);
                        }
                        var surnameCol = surnameColumnBuilder.Build().ToRenderFragment<Column<string>>();

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
        public void Table_applies_react_compatible_root_class_name()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            var table = CreatePersonsTable(
                new[] { new Person { Id = 1, Name = "Ada", Surname = "Lovelace" } },
                callback: parameters => parameters.Add(component => component.RootClassName, "react-root"));

            Assert.Contains("react-root", table.Find(".ant-table-wrapper").ClassList);
        }

        [Fact]
        public void Column_min_width_is_applied_to_col_header_and_body_cells()
        {
            JSInterop.SetupVoid("AntDesign.interop.styleHelper.addCls", _ => true).SetVoidResult();
            JSInterop.SetupVoid(JSInteropConstants.TableComponentHelper.BindTableRowHover, _ => true).SetVoidResult();

            var table = CreatePersonsTable(new[] { new Person { Id = 1, Name = "John", Surname = "Smith" } }, surnameMinWidth: 180);

            Assert.Contains("min-width: 180px", table.Markup);
            Assert.Contains(table.FindAll("col"), column => column.GetAttribute("style")?.Contains("min-width: 180px") == true);
        }

        [Fact]
        public void Column_sort_icon_renders_custom_content_with_unsorted_context()
        {
            JSInterop.SetupVoid(JSInteropConstants.TableComponentHelper.BindTableRowHover, _ => true).SetVoidResult();
            SortDirection? receivedSortOrder = SortDirection.Ascending;
            RenderFragment<SortDirection?> sortIcon = sortOrder => builder =>
            {
                receivedSortOrder = sortOrder;
                builder.AddMarkupContent(0, "<span class=\"custom-sort-icon\">sort</span>");
            };

            var table = CreatePersonsTable(
                new[] { new Person { Id = 1, Name = "John", Surname = "Smith" } },
                sortableName: true,
                sortIcon: sortIcon);

            Assert.Contains(table.FindAll(".custom-sort-icon"), _ => true);
            Assert.DoesNotContain(table.FindAll(".ant-table-column-sorter-up"), _ => true);
            Assert.Null(receivedSortOrder);
        }

        [Fact]
        public async Task ShowExpandAll_toggles_expandable_rows_and_reports_state()
        {
            JSInterop.SetupVoid(JSInteropConstants.TableComponentHelper.BindTableRowHover, _ => true).SetVoidResult();
            var expandedStates = new List<bool>();
            var expandedKeys = new List<string[]>();
            var persons = new[]
            {
                new Person { Id = 1, Name = "John", Surname = "Smith" },
                new Person { Id = 2, Name = "Jane", Surname = "Doe" }
            };
            var table = Context.RenderComponent<Table<Person>>(parameters => parameters
                .Add(p => p.DataSource, persons)
                .Add(p => p.RowKey, person => person.Id)
                .Add(p => p.ShowExpandAll, true)
                .Add(p => p.OnExpandAll, state => expandedStates.Add(state))
                .Add(p => p.OnExpandedRowsChange, keys => expandedKeys.Add(keys.ToArray()))
                .Add(p => p.ExpandTemplate, _ => builder => builder.AddMarkupContent(0, "<span class=\"expanded-content\">Details</span>"))
                .Add(p => p.ChildContent, _ => builder =>
                {
                    builder.OpenComponent<Column<string>>(0);
                    builder.AddAttribute(1, nameof(Column<string>.Title), "Name");
                    builder.AddAttribute(2, nameof(Column<string>.Field), "Name");
                    builder.CloseComponent();
                }));

            var expandAll = table.Find("thead .ant-table-row-expand-icon");
            Assert.Equal("Expand all rows", expandAll.GetAttribute("aria-label"));
            await expandAll.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

            Assert.Equal(new[] { true }, expandedStates);
            Assert.Equal(new[] { "1", "2" }, expandedKeys[0]);
            Assert.Equal(2, table.FindAll(".ant-table-expanded-row").Count);
            Assert.Equal("Collapse all rows", table.Find("thead .ant-table-row-expand-icon").GetAttribute("aria-label"));

            await table.Find("thead .ant-table-row-expand-icon").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
            Assert.Equal(new[] { true, false }, expandedStates);
            Assert.Empty(expandedKeys[1]);
            Assert.Equal(0, table.FindAll(".ant-table-expanded-row:not([style*=display])").Count);

            await table.Find("tbody .ant-table-row-expand-icon-collapsed").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());
            Assert.Equal(new[] { "1" }, expandedKeys[2]);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public async Task ScrollToFirstRowOnChange_controls_sort_scroll(bool enabled, int expectedCalls)
        {
            var scrollToIdentifier = JSInteropConstants.TableComponentHelper.ScrollTo;
            JSInterop.SetupVoid(scrollToIdentifier, _ => true).SetVoidResult();
            JSInterop.SetupVoid(JSInteropConstants.TableComponentHelper.BindTableRowHover, _ => true).SetVoidResult();
            JSInterop.SetupVoid(JSInteropConstants.BindTableScroll, _ => true).SetVoidResult();
            JSInterop.Setup<double>(JSInteropConstants.DomMainpulationHelper.GetScrollBarSize, _ => true).SetResult(0);
            var table = CreatePersonsTable(
                new[] { new Person { Id = 1, Name = "John", Surname = "Smith" } },
                parameters => parameters
                    .Add(component => component.ScrollY, "120px")
                    .Add(component => component.ScrollToFirstRowOnChange, enabled),
                sortableName: true);

            var column = table.FindComponents<Column<string>>().First(component => component.Instance.IsHeader && !component.Instance.IsMeasure).Instance;
            var changeSorter = typeof(ITable).GetMethod("ColumnSorterChange", BindingFlags.Instance | BindingFlags.NonPublic)!;
            await table.InvokeAsync(() => changeSorter.Invoke(table.Instance, new object[] { column }));

            Assert.Equal(expectedCalls, JSInterop.Invocations[scrollToIdentifier].Count);
            if (enabled)
            {
                Assert.Equal(0d, JSInterop.Invocations[scrollToIdentifier][0].Arguments[3]);
            }
        }

        [Fact]
        public void RowHoverable_binds_by_default_and_can_be_toggled()
        {
            var bindIdentifier = JSInteropConstants.TableComponentHelper.BindTableRowHover;
            var unbindIdentifier = JSInteropConstants.TableComponentHelper.UnbindTableRowHover;
            JSInterop.SetupVoid(bindIdentifier, _ => true).SetVoidResult();
            JSInterop.SetupVoid(unbindIdentifier, _ => true).SetVoidResult();
            var cut = CreatePersonsTable(new[] { new Person { Id = 1, Name = "Lucy", Surname = "Liu" } });

            Assert.Contains(JSInterop.Invocations, call => call.Identifier == bindIdentifier);

            cut.SetParametersAndRender(parameters => parameters.Add(table => table.RowHoverable, false));
            Assert.Contains(JSInterop.Invocations, call => call.Identifier == unbindIdentifier);

            var unbindCount = JSInterop.Invocations[unbindIdentifier].Count;
            cut.SetParametersAndRender(parameters => parameters.Add(table => table.RowHoverable, true));
            Assert.Equal(unbindCount, JSInterop.Invocations[unbindIdentifier].Count);
            Assert.Equal(2, JSInterop.Invocations[bindIdentifier].Count);
        }

        [Fact]
        public void OnScroll_is_invoked_for_table_content_scroll()
        {
            var scrollCount = 0;
            JSInterop.SetupVoid(JSInteropConstants.TableComponentHelper.BindTableRowHover, _ => true).SetVoidResult();
            var cut = CreatePersonsTable(
                new[] { new Person { Id = 1, Name = "Lucy", Surname = "Liu" } },
                parameters => parameters.Add(table => table.OnScroll,
                    EventCallback.Factory.Create<EventArgs>(this, _ => scrollCount++)));

            cut.Find(".ant-table-content").TriggerEvent("onscroll", EventArgs.Empty);

            Assert.Equal(1, scrollCount);
        }

        [Fact]
        public void GetPopupContainer_is_forwarded_to_table_filter_dropdown()
        {
            JSInterop.SetupVoid(JSInteropConstants.StyleHelper.AddCls, _ => true).SetVoidResult();
            JSInterop.SetupVoid(JSInteropConstants.TableComponentHelper.BindTableRowHover, _ => true).SetVoidResult();
            Func<ElementReference, ElementReference> getPopupContainer = element => element;
            var filters = new[] { new TableFilter<string>("Lucy", "Lucy", false, default, default) };
            var cut = CreatePersonsTable(
                new[] { new Person { Id = 1, Name = "Lucy", Surname = "Liu" } },
                parameters => parameters.Add(table => table.GetPopupContainer, getPopupContainer),
                nameFilters: filters);

            Assert.Contains(cut.FindComponents<AntDesign.Dropdown>(), dropdown => dropdown.Instance.GetPopupContainer == getPopupContainer);
        }

        [Fact]
        public async Task Ref_exposes_the_native_element_and_scrollTo_options()
        {
            var scrollToIdentifier = JSInteropConstants.TableComponentHelper.ScrollTo;
            JSInterop.SetupVoid(scrollToIdentifier, _ => true).SetVoidResult();
            JSInterop.SetupVoid(JSInteropConstants.TableComponentHelper.BindTableRowHover, _ => true).SetVoidResult();
            var cut = CreatePersonsTable(new[] { new Person { Id = 1, Name = "Lucy", Surname = "Liu" } });
            ITable tableRef = cut.Instance;

            await tableRef.ScrollTo(new TableScrollToOptions { Index = 0, Offset = 12, Align = "start" });

            Assert.NotNull(tableRef.NativeElement);
            var invocation = Assert.Single(JSInterop.Invocations[scrollToIdentifier]);
            Assert.Equal(0, invocation.Arguments[2]);
            Assert.Equal(12d, invocation.Arguments[4]);
            Assert.Equal("start", invocation.Arguments[5]);
        }

        //[Fact]
        public void Renders_an_empty_table()
        {
            var persons = Array.Empty<Person>();

            var cut = CreatePersonsTable(persons);

            cut.RecordedMarkupMatches();
        }

        //[Fact]
        public void Renders_a_table_with_two_rows()
        {
            var persons = new[]
            {
                new Person {Id = 1, Name = "John", Surname = "Smith"},
                new Person {Id = 2, Name = "Jane", Surname = "Doe"}
            };

            var cut = CreatePersonsTable(persons);

            cut.RecordedMarkupMatches();
        }

        //[Fact]
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

            cut.SetParametersAndRender(b => b.Add(q => q.DataSource, persons));

            cut.RecordedMarkupMatches();
        }

        //[Fact]
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

            cut.RecordedMarkupMatches();
        }

        [Fact]
        public void Cascaded_pagination_props_override_the_table_internal_pagination()
        {
            var props = new PaginationProps
            {
                ShowQuickJumper = true,
                ShowSizeChanger = true,
            };

            JSInterop.Setup<DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
                .SetResult(new DomRect());
            JSInterop.SetupVoid(JSInteropConstants.TableComponentHelper.BindTableRowHover, _ => true).SetVoidResult();

            var cut = Context.RenderComponent<CascadingValue<PaginationProps>>(parameters => parameters
                .Add(p => p.Value, props)
                .Add(p => p.ChildContent, builder =>
                {
                    builder.OpenComponent<Table<Person>>(0);
                    builder.AddAttribute(1, nameof(Table<Person>.DataSource), new[]
                    {
                        new Person { Id = 1, Name = "John", Surname = "Smith" },
                        new Person { Id = 2, Name = "Jane", Surname = "Doe" },
                    });
                    builder.AddAttribute(2, nameof(Table<Person>.PageSize), 20);
                    builder.AddAttribute(3, nameof(Table<Person>.ChildContent), (RenderFragment<Person>)(_ => builder => { }));
                    builder.CloseComponent();
                }));

            var pagination = cut.FindComponent<Pagination>().Instance;
            Assert.Same(props, pagination.CascadingPaginationProps);
            Assert.True(pagination.ShowQuickJumper);
            Assert.True(pagination.ShowSizeChanger);
            Assert.Equal(20, pagination.PageSize);
        }

        [Fact]
        public void Cascaded_props_collection_targets_each_table_pagination_by_key()
        {
            var props = new ComponentPropsCollection(new PaginationProps { ShowQuickJumper = true })
            {
                [Table<Person>.BottomPaginationId] = new PaginationProps { ShowSizeChanger = true },
            };

            JSInterop.Setup<DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
                .SetResult(new DomRect());
            JSInterop.SetupVoid(JSInteropConstants.TableComponentHelper.BindTableRowHover, _ => true).SetVoidResult();

            var cut = Context.RenderComponent<CascadingValue<ComponentPropsCollection>>(parameters => parameters
                .Add(p => p.Value, props)
                .Add(p => p.ChildContent, builder =>
                {
                    builder.OpenComponent<Table<Person>>(0);
                    builder.AddAttribute(1, nameof(Table<Person>.DataSource), new[] { new Person { Id = 1 } });
                    builder.AddAttribute(2, nameof(Table<Person>.PaginationPosition), "topRight,bottomRight");
                    builder.AddAttribute(3, nameof(Table<Person>.ChildContent), (RenderFragment<Person>)(_ => builder => { }));
                    builder.CloseComponent();
                }));

            var paginations = cut.FindComponents<Pagination>();
            Assert.Equal(2, paginations.Count);
            Assert.True(paginations[0].Instance.ShowQuickJumper);
            Assert.False(paginations[0].Instance.ShowSizeChanger);
            Assert.True(paginations[1].Instance.ShowQuickJumper);
            Assert.True(paginations[1].Instance.ShowSizeChanger);
        }

        [Fact]
        public void Component_props_provider_applies_global_and_id_specific_props()
        {
            JSInterop.Setup<DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
                .SetResult(new DomRect());
            JSInterop.SetupVoid(JSInteropConstants.TableComponentHelper.BindTableRowHover, _ => true).SetVoidResult();

            var cut = Context.RenderComponent<ComponentPropsProvider>(parameters => parameters
                .Add(p => p.Props, new ComponentPropsCollection(new PaginationProps { ShowQuickJumper = true }))
                .Add(p => p.ChildContent, builder =>
                {
                    builder.OpenComponent<ComponentPropsProvider>(0);
                    builder.AddAttribute(1, nameof(ComponentPropsProvider.Props), new ComponentPropsCollection { [Table<Person>.BottomPaginationId] = new PaginationProps { ShowSizeChanger = true } });
                    builder.AddAttribute(2, nameof(ComponentPropsProvider.ChildContent), (RenderFragment)(childBuilder =>
                    {
                        childBuilder.OpenComponent<Table<Person>>(0);
                        childBuilder.AddAttribute(1, nameof(Table<Person>.DataSource), new[] { new Person { Id = 1 } });
                        childBuilder.AddAttribute(2, nameof(Table<Person>.PaginationPosition), "topRight,bottomRight");
                        childBuilder.AddAttribute(3, nameof(Table<Person>.ChildContent), (RenderFragment<Person>)(_ => builder => { }));
                        childBuilder.CloseComponent();
                    }));
                    builder.CloseComponent();
                }));

            var paginations = cut.FindComponents<Pagination>();
            Assert.True(paginations[0].Instance.ShowQuickJumper);
            Assert.False(paginations[0].Instance.ShowSizeChanger);
            Assert.True(paginations[1].Instance.ShowQuickJumper);
            Assert.True(paginations[1].Instance.ShowSizeChanger);
        }

        [Fact]
        public async Task Filter_dropdown_closes_on_outside_click_by_default()
        {
            var cut = CreateFilterableTable();
            cut.Find(".ant-table-filter-trigger").Click();

            await cut.InvokeAsync(() => cut.FindComponent<AntDesign.Dropdown>().Instance.OnMaskClick.InvokeAsync());

            cut.WaitForAssertion(() => Assert.DoesNotContain("ant-dropdown-open", cut.Find(".ant-table-filter-trigger").ClassList));
        }

        [Fact]
        public async Task Filter_dropdown_can_remain_open_on_outside_click()
        {
            var cut = CreateFilterableTable(closeOnOutsideClick: false);
            cut.Find(".ant-table-filter-trigger").Click();

            Assert.False(cut.FindComponent<AntDesign.Dropdown>().Instance.CloseOnOutsideClick);
            cut.WaitForAssertion(() => Assert.Contains("ant-dropdown-open", cut.Find(".ant-table-filter-trigger").ClassList));
        }

        [Fact]
        public async Task Filter_dropdown_does_not_apply_selected_values_when_filterOnClose_is_false()
        {
            var cut = CreateFilterableTable(filterOnClose: false);
            cut.Find(".ant-table-filter-trigger").Click();
            cut.Find(".ant-dropdown-menu-item").Click();
            cut.WaitForAssertion(() => Assert.Contains(GetFilterableHeaderColumn(cut).Filters, filter => filter.Selected));

            await cut.InvokeAsync(() => cut.FindComponent<AntDesign.Dropdown>().Instance.OnMaskClick.InvokeAsync());

            cut.WaitForAssertion(() =>
            {
                Assert.DoesNotContain("ant-dropdown-open", cut.Find(".ant-table-filter-trigger").ClassList);
                Assert.Equal(2, cut.FindAll(".ant-table-tbody > tr").Count);
            });
        }

        [Fact]
        public async Task Filter_dropdown_applies_selected_values_when_filterOnClose_is_true()
        {
            var cut = CreateFilterableTable();
            cut.Find(".ant-table-filter-trigger").Click();
            cut.Find(".ant-dropdown-menu-item").Click();
            cut.WaitForAssertion(() => Assert.Contains(GetFilterableHeaderColumn(cut).Filters, filter => filter.Selected));

            await cut.InvokeAsync(() => cut.FindComponent<AntDesign.Dropdown>().Instance.OnMaskClick.InvokeAsync());

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(".ant-table-tbody > tr")));
        }

        [Fact]
        public async Task Filter_reset_restores_default_values_as_pending_selection()
        {
            var cut = CreateFilterableTable(
                filterResetToDefaultFilteredValue: true,
                defaultFilteredValue: new[] { "John" });
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(".ant-table-tbody > tr")));
            cut.Find(".ant-table-filter-trigger").Click();

            Assert.True(GetFilterableHeaderColumn(cut).FilterResetToDefaultFilteredValue);
            Assert.Equal("John", Assert.Single(GetFilterableHeaderColumn(cut).DefaultFilteredValue));
            Assert.Single(GetFilterableHeaderColumn(cut).Filters.Where(filter => filter.Selected));
            Assert.True(GetFilterResetButton(cut).Instance.Disabled);
            cut.FindAll(".ant-dropdown-menu-item")[1].Click();
            cut.WaitForAssertion(() => Assert.Equal(2, GetFilterableHeaderColumn(cut).Filters.Count(filter => filter.Selected)));
            cut.WaitForAssertion(() => Assert.False(GetFilterResetButton(cut).Instance.Disabled));
            Assert.Single(cut.FindAll(".ant-table-tbody > tr"));

            cut.Find(".ant-table-filter-dropdown-btns .ant-btn-link").Click();

            Assert.Single(GetFilterableHeaderColumn(cut).Filters.Where(filter => filter.Selected));
            Assert.True(GetFilterResetButton(cut).Instance.Disabled);
            Assert.Contains("ant-dropdown-open", cut.Find(".ant-table-filter-trigger").ClassList);
            Assert.Single(cut.FindAll(".ant-table-tbody > tr"));

            cut.Find(".ant-table-filter-dropdown-btns .ant-btn-primary").Click();
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(".ant-table-tbody > tr")));
        }

        private IRenderedComponent<Table<Person>> CreateFilterableTable(
            bool closeOnOutsideClick = true,
            bool filterOnClose = true,
            bool filterResetToDefaultFilteredValue = false,
            IEnumerable<string>? defaultFilteredValue = null)
        {
            var persons = new[]
            {
                new Person { Id = 1, Name = "John", Surname = "Smith" },
                new Person { Id = 2, Name = "Jane", Surname = "Doe" }
            };
            var filters = new[]
            {
                new TableFilter<string>("John", "John", false, default, default),
                new TableFilter<string>("Jane", "Jane", false, default, default)
            };
            JSInterop.SetupVoid("AntDesign.interop.styleHelper.addCls", _ => true);
            JSInterop.Setup<OverlayPosition>(JSInteropConstants.OverlayComponentHelper.AddOverlayToContainer, _ => true)
                .SetResult(new OverlayPosition());
            JSInterop.Setup<OverlayPosition>(JSInteropConstants.OverlayComponentHelper.UpdateOverlayPosition, _ => true)
                .SetResult(new OverlayPosition());
            JSInterop.Setup<DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
                .SetResult(new DomRect());
            JSInterop.SetupVoid(JSInteropConstants.TableComponentHelper.BindTableRowHover, _ => true)
                .SetVoidResult();

            return Context.RenderComponent<Table<Person>>(x => x
                .Add(q => q.DataSource, persons)
                .Add(q => q.ChildContent, _ => builder =>
                {
                    new ComponentParameterCollectionBuilder<PropertyColumn<Person, string>>()
                        .Add(q => q.Property, person => person.Name)
                        .Add(q => q.Filters, filters)
                        .Add(q => q.FilterDropdownAutoClose, closeOnOutsideClick)
                        .Add(q => q.FilterOnClose, filterOnClose)
                        .Add(q => q.FilterResetToDefaultFilteredValue, filterResetToDefaultFilteredValue)
                        .Add(q => q.DefaultFilteredValue, defaultFilteredValue)
                        .Build()
                        .ToRenderFragment<PropertyColumn<Person, string>>()(builder);
                }));
        }

        private static PropertyColumn<Person, string> GetFilterableHeaderColumn(IRenderedComponent<Table<Person>> table)
        {
            return table.FindComponents<PropertyColumn<Person, string>>()
                .Single(column => column.Instance.IsHeader && !column.Instance.IsMeasure)
                .Instance;
        }

        private static IRenderedComponent<AntDesign.Button> GetFilterResetButton(IRenderedComponent<Table<Person>> table)
        {
            return table.FindComponents<AntDesign.Button>()
                .Single(button => button.Instance.Type == ButtonType.Link);
        }
    }
}
