using System.Collections.Generic;
using AntDesign.Docs.Services;
using Xunit;

namespace AntDesign.Tests.Theming;

public sealed class CssInJsTests
{
    [Fact]
    public void CreateStyles_GeneratesStableNamedClassesAndCssRules()
    {
        var definitions = new Dictionary<string, object>
        {
            ["root"] = new Dictionary<string, object>
            {
                ["display"] = "flex",
                ["padding"] = 12,
                ["lineHeight"] = 1.5,
                ["&:hover"] = new Dictionary<string, object>
                {
                    ["backgroundColor"] = "var(--ant-color-fill-tertiary)"
                }
            }
        };

        var styles = CssInJs.CreateStyles("theme-preview", definitions);

        Assert.Equal("adb-theme-preview-root", styles.Classes["root"]);
        Assert.Equal(
            ".adb-theme-preview-root{display:flex;padding:12px;line-height:1.5;}.adb-theme-preview-root:hover{background-color:var(--ant-color-fill-tertiary);}",
            styles.CssText);
    }

    [Fact]
    public void CreateStyles_ExpandsNestedSelectorsAndMediaRules()
    {
        var definitions = new Dictionary<string, object>
        {
            ["list"] = new Dictionary<string, object>
            {
                ["& > li"] = new Dictionary<string, object> { ["paddingInline"] = "8px" },
                ["@media (max-width: 600px)"] = new Dictionary<string, object>
                {
                    ["& > li"] = new Dictionary<string, object> { ["paddingInline"] = "4px" }
                }
            }
        };

        var styles = CssInJs.CreateStyles("semantic-preview", definitions);

        Assert.Equal(
            ".adb-semantic-preview-list > li{padding-inline:8px;}@media (max-width: 600px){.adb-semantic-preview-list > li{padding-inline:4px;}}",
            styles.CssText);
    }

    [Fact]
    public void CreateStyles_UsesReadableNamesForCamelCaseStyleKeys()
    {
        var styles = CssInJs.CreateStyles("home-theme-preview", new Dictionary<string, object>
        {
            ["themeBlock"] = new Dictionary<string, object> { ["width"] = 20 },
            ["previewTabs"] = new Dictionary<string, object> { ["padding"] = 3 }
        });

        Assert.Equal("adb-home-theme-preview-theme-block", styles["themeBlock"]);
        Assert.Equal("adb-home-theme-preview-preview-tabs", styles["previewTabs"]);
    }
}
