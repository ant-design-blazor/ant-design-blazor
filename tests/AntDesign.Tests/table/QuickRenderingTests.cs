using AntDesign.TableModels;
using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class QuickRenderingTests : AntDesignTestBase
    {
        private sealed class Person
        {
            public string Name { get; set; } = string.Empty;
        }

        private static readonly Person[] People =
        {
            new() { Name = "Alice" },
            new() { Name = "Bob" }
        };

        [Fact]
        public void Table_defaults_to_legacy_child_content_rendering()
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
    }
}
