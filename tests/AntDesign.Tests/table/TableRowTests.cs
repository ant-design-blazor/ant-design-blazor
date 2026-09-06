// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using AntDesign.TableModels;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class TableRowTests : AntDesignTestBase
    {
        private sealed record Person(int Id, string Name);

        [Fact]
        public void Body_row_combines_row_and_additional_attributes()
        {
            var rowData = new RowData<Person>(new TableDataItem<Person>(new Person(1, "Alice"), null!))
            {
                RowIndex = 2,
            };
            var cut = Context.RenderComponent<TableRow>(parameters => parameters
                .AddCascadingValue("IsBody", true)
                .AddCascadingValue("IsHeader", false)
                .AddCascadingValue("RowData", rowData)
                .AddCascadingValue("AntDesign.TableRow.RowAttributes", new Dictionary<string, object>
                {
                    ["data-row"] = "row",
                    ["class"] = "row-class",
                })
                .AddUnmatched("data-extra", "extra")
                .AddUnmatched("class", "extra-class")
                .Add(component => component.ChildContent, "<td>Alice</td>".ToRenderFragment()));

            var row = cut.Find("tr");
            Assert.Equal("row", row.GetAttribute("data-row"));
            Assert.Equal("extra", row.GetAttribute("data-extra"));
            Assert.Contains("extra-class", row.GetAttribute("class"));
            Assert.Equal("1", row.GetAttribute("data-row-key"));
        }

        [Fact]
        public void Body_row_uses_only_additional_attributes_when_row_attributes_are_absent()
        {
            var rowData = new RowData<Person>(new TableDataItem<Person>(new Person(1, "Alice"), null!));
            var cut = Context.RenderComponent<TableRow>(parameters => parameters
                .AddCascadingValue("IsBody", true)
                .AddCascadingValue("IsHeader", false)
                .AddCascadingValue("RowData", rowData)
                .AddUnmatched("data-extra", "extra")
                .Add(component => component.ChildContent, "<td>Alice</td>".ToRenderFragment()));

            Assert.Equal("extra", cut.Find("tr").GetAttribute("data-extra"));
        }

        [Fact]
        public void Header_row_uses_row_attributes_and_renders_scrollbar_cell_when_scroll_y_is_set()
        {
            var rowData = new RowData<Person>(new TableDataItem<Person>(new Person(1, "Alice"), null!));
            var cut = Context.RenderComponent<TableRow>(parameters => parameters
                .AddCascadingValue("IsBody", false)
                .AddCascadingValue("IsHeader", true)
                .AddCascadingValue("RowData", rowData)
                .AddCascadingValue("AntDesign.TableRow.RowAttributes", new Dictionary<string, object>
                {
                    ["data-header"] = "header",
                })
                .AddCascadingValue("AntDesign.TableRow.ScrollY", "240px")
                .AddCascadingValue("AntDesign.TableRow.HasFixRight", true)
                .Add(component => component.ChildContent, "<th>Name</th>".ToRenderFragment()));

            var row = cut.Find("tr");
            Assert.Equal("header", row.GetAttribute("data-header"));
            var scrollbar = cut.Find("th.ant-table-cell-scrollbar");
            Assert.Contains("ant-table-cell-fix-right", scrollbar.ClassList);
            Assert.Contains("right: 0px", scrollbar.GetAttribute("style"));
        }
    }
}
