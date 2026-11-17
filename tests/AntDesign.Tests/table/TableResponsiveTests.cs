using System;
using System.Linq;
using System.Linq.Expressions;
using AntDesign.JsInterop;
using AntDesign.Theming;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Xunit;

namespace AntDesign.Tests.Table;

public sealed class TableResponsiveTests : AntDesignTestBase
{
    [Theory]
    [InlineData(899, false)]
    [InlineData(900, true)]
    public void Responsive_columns_follow_the_configured_theme_breakpoint(decimal viewportWidth, bool ageColumnVisible)
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.Setup<Window>(JSInteropConstants.GetWindow, _ => true)
            .SetResult(new Window { InnerWidth = viewportWidth, InnerHeight = 800 });

        var columns = (RenderFragment<ResponsiveRow>)(_ => builder =>
        {
            builder.OpenComponent<PropertyColumn<ResponsiveRow, int>>(0);
            builder.AddAttribute(1, nameof(PropertyColumn<ResponsiveRow, int>.Property), (Expression<Func<ResponsiveRow, int>>)(row => row.Age));
            builder.AddAttribute(2, nameof(Column<int>.Title), "Age");
            builder.AddAttribute(3, nameof(Column<int>.Responsive), new[] { "md" });
            builder.CloseComponent();
        });

        var table = (RenderFragment)(builder =>
        {
            builder.OpenComponent<Table<ResponsiveRow>>(0);
            builder.AddAttribute(1, nameof(Table<ResponsiveRow>.DataSource), new[] { new ResponsiveRow { Age = 42 } });
            builder.AddAttribute(2, nameof(Table<ResponsiveRow>.ChildContent), columns);
            builder.CloseComponent();
        });

        var cut = Context.RenderComponent<ConfigProvider>(parameters => parameters
            .Add(component => component.ThemeConfig, new ThemeConfig
            {
                Token = new ThemeToken { ["screenMD"] = "900" }
            })
            .AddChildContent(table));

        cut.WaitForAssertion(() => Assert.Equal(ageColumnVisible, cut.FindAll("th").Any(cell => cell.TextContent == "Age")));
    }

    private sealed class ResponsiveRow
    {
        public int Age { get; set; }
    }
}
