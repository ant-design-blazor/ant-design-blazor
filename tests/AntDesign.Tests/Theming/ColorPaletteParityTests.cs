using System;
using System.Text.Json;
using AntDesign.Colors;
using AntDesign.Icons;
using Xunit;

namespace AntDesign.Tests.Theming;

public sealed class ColorPaletteParityTests
{
    [Fact]
    public void All_swatches_match_the_actual_v6_color_library()
    {
        using var stream = typeof(ColorPaletteParityTests).Assembly.GetManifestResourceStream("AntDesign.Tests.Theming.Fixtures.react-color-palettes.json");
        using var fixture = JsonDocument.Parse(stream!);
        var count = 0;
        foreach (var item in fixture.RootElement.GetProperty("cases").EnumerateArray())
        {
            var color = item.GetProperty("color").GetString()!;
            var dark = item.GetProperty("dark").GetBoolean();
            var background = item.TryGetProperty("backgroundColor", out var bg) ? bg.GetString() : null;
            var actual = ColorPalette.Generate(color, dark, background);
            var i = 0;
            foreach (var expected in item.GetProperty("expected").EnumerateArray())
            {
                Assert.True(expected.GetString() == actual[i], $"{color} dark={dark} background={background} swatch={i}: {expected} != {actual[i]}");
                i++; count++;
            }
            Assert.Equal(10, i);
        }
        Assert.True(count > 8000);
        foreach (var item in fixture.RootElement.GetProperty("operations").EnumerateArray())
        {
            var color = item.GetProperty("color").GetString()!;
            Assert.Equal(item.GetProperty("alpha").GetString()!.Replace(" ", ""), ColorPalette.SetAlpha(color, .35).Replace(" ", ""));
            Assert.Equal(item.GetProperty("lighten").GetString(), ColorPalette.Lighten(color, 19));
            Assert.Equal(item.GetProperty("darken").GetString(), ColorPalette.Darken(color, 15));
            Assert.Equal(item.GetProperty("mix").GetString(), ColorPalette.Mix(color, "#1677ff80", 37));
        }
    }

    [Theory]
    [InlineData("#1677ff")]
    [InlineData("#52c41a")]
    [InlineData("hsl(210.5deg 50% 40%)")]
    [InlineData("rebeccapurple")]
    public void Icon_compatibility_API_delegates_to_the_shared_palette(string color)
        => Assert.Equal(ColorPalette.Generate(color)[0], IconColorPalette.GetSecondaryColor(color));

    [Fact]
    public void Calls_return_independent_palettes()
    {
        var first = ColorPalette.Generate("#1677ff");
        first[0] = "changed";
        Assert.Equal("#e6f4ff", ColorPalette.Generate("#1677ff")[0]);
    }
}
