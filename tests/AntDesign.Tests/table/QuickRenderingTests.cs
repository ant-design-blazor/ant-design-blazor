using AntDesign.TableModels;
using Bunit;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class QuickRenderingTests : AntDesignTestBase
    {
        public sealed class Person
        {
            public string Name { get; set; } = string.Empty;
        }

        private static readonly Person[] People =
        {
            new() { Name = "Alice" },
            new() { Name = "Bob" }
        };

        [Fact]
        public void Table_defaults_to_quick_rendering_with_legacy_child_content_compatibility()
        {
            var cut = Context.RenderComponent<Table<Person>>(parameters => parameters
                .Add(table => table.DataSource, People)
                .Add(table => table.ChildContent, person => builder =>
                {
                    builder.OpenComponent<ActionColumn>(0);
                    builder.AddAttribute(1, nameof(ActionColumn.ChildContent), (RenderFragment)(contentBuilder =>
                        contentBuilder.AddContent(0, person.Name)));
                    builder.CloseComponent();
                }));

            Assert.True(cut.Instance.Quick);
            Assert.Contains("Alice", cut.Markup);
            Assert.Contains("Bob", cut.Markup);
        }

        [Fact]
        public void Quick_mode_renders_cell_templates_against_each_row()
        {
            var cut = Context.RenderComponent<Table<Person>>(parameters => parameters
                .Add(table => table.DataSource, People)
                .Add(table => table.Quick, true)
                .Add(table => table.ChildContent, _ => builder =>
                {
                    builder.OpenComponent<Column<string>>(0);
                    builder.AddAttribute(1, nameof(Column<string>.Title), "Name");
                    builder.AddAttribute(2, nameof(Column<string>.CellRender), (RenderFragment<CellData<string>>)(cell => contentBuilder =>
                    {
                        var row = (RowData<Person>)cell.RowData;
                        contentBuilder.AddContent(0, $"template:{row.Data.Name}");
                    }));
                    builder.CloseComponent();
                }));

            Assert.Contains("template:Alice", cut.Markup);
            Assert.Contains("template:Bob", cut.Markup);
        }

        [Fact]
        public void Quick_mode_renders_legacy_child_content_against_each_row()
        {
            var cut = Context.RenderComponent<Table<Person>>(parameters => parameters
                .Add(table => table.DataSource, People)
                .Add(table => table.Quick, true)
                .Add(table => table.ChildContent, person => builder =>
                {
                    builder.OpenComponent<ActionColumn>(0);
                    builder.AddAttribute(1, nameof(ActionColumn.Title), "Name");
                    builder.AddAttribute(2, nameof(ActionColumn.ChildContent), (RenderFragment)(contentBuilder =>
                        contentBuilder.AddContent(0, $"legacy:{person.Name}")));
                    builder.CloseComponent();

                    builder.OpenComponent<PropertyColumn<Person, string>>(3);
                    builder.AddAttribute(4, nameof(PropertyColumn<Person, string>.Title), "Property");
                    builder.AddAttribute(5, nameof(PropertyColumn<Person, string>.Property), (Expression<Func<Person, string>>)(row => row.Name));
                    builder.AddAttribute(6, nameof(PropertyColumn<Person, string>.ChildContent), (RenderFragment)(contentBuilder =>
                        contentBuilder.AddContent(0, $"property:{person.Name}")));
                    builder.CloseComponent();
                }));

            Assert.Contains("legacy:Alice", cut.Markup);
            Assert.Contains("legacy:Bob", cut.Markup);
            Assert.Contains("property:Alice", cut.Markup);
            Assert.Contains("property:Bob", cut.Markup);
        }

        [Fact]
        public void Quick_mode_logs_once_when_legacy_child_content_uses_compatibility_rendering()
        {
            var logger = new Mock<ILogger<Table<Person>>>();
            Services.AddSingleton(logger.Object);

            Context.RenderComponent<Table<Person>>(parameters => parameters
                .Add(table => table.DataSource, Array.Empty<Person>())
                .Add(table => table.Quick, true)
                .Add(table => table.ChildContent, person => builder =>
                {
                    builder.OpenComponent<ActionColumn>(0);
                    builder.AddAttribute(1, nameof(ActionColumn.ChildContent), (RenderFragment)(contentBuilder =>
                        contentBuilder.AddContent(0, person.Name)));
                    builder.CloseComponent();
                }));

            var warnings = logger.Invocations
                .Where(invocation => invocation.Method.Name == nameof(ILogger.Log)
                    && invocation.Arguments[0] is LogLevel.Warning)
                .ToArray();

            var warning = Assert.Single(warnings);
            Assert.Contains("CellRender", warning.Arguments[2]!.ToString());
        }

        [Fact]
        public void Quick_mode_invokes_child_content_once_to_register_columns()
        {
            var childContentInvocationCount = 0;
            var cut = Context.RenderComponent<Table<Person>>(parameters => parameters
                .Add(table => table.DataSource, People)
                .Add(table => table.ScrollY, "240px")
                .Add(table => table.Quick, true)
                .Add(table => table.ChildContent, _ => builder =>
                {
                    childContentInvocationCount++;
                    builder.OpenComponent<Column<string>>(0);
                    builder.AddAttribute(1, nameof(Column<string>.Title), "Name");
                    builder.AddAttribute(2, nameof(Column<string>.CellRender), (RenderFragment<CellData<string>>)(cell => contentBuilder =>
                    {
                        var row = (RowData<Person>)cell.RowData;
                        contentBuilder.AddContent(0, row.Data.Name);
                    }));
                    builder.CloseComponent();
                }));

            Assert.Equal(1, childContentInvocationCount);
            Assert.Contains("Name", cut.Find("thead").TextContent);
            Assert.Contains("Alice", cut.Markup);
            Assert.Contains("Bob", cut.Markup);
        }

        [Fact]
        public void Legacy_mode_keeps_child_content_rendering_for_each_table_stage()
        {
            var childContentInvocationCount = 0;
            var cut = Context.RenderComponent<Table<Person>>(parameters => parameters
                .Add(table => table.DataSource, People)
                .Add(table => table.Quick, false)
                .Add(table => table.ScrollY, "240px")
                .Add(table => table.ChildContent, _ => builder =>
                {
                    childContentInvocationCount++;
                    builder.OpenComponent<Column<string>>(0);
                    builder.AddAttribute(1, nameof(Column<string>.Title), "Name");
                    builder.AddAttribute(2, nameof(Column<string>.CellRender), (RenderFragment<CellData<string>>)(cell => contentBuilder =>
                    {
                        var row = (RowData<Person>)cell.RowData;
                        contentBuilder.AddContent(0, row.Data.Name);
                    }));
                    builder.CloseComponent();
                }));

            Assert.True(childContentInvocationCount > 1);
            Assert.Contains("Alice", cut.Markup);
            Assert.Contains("Bob", cut.Markup);
        }
    }
}
