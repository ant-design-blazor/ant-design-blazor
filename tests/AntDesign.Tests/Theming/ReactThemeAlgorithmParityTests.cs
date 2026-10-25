using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using AntDesign.Theming;
using Xunit;

namespace AntDesign.Tests.Theming;

public sealed class ReactThemeAlgorithmParityTests
{
    [Fact]
    public void Runtime_algorithms_match_the_pinned_React_reference()
    {
        using var stream = typeof(ReactThemeAlgorithmParityTests).Assembly.GetManifestResourceStream("AntDesign.Tests.Theming.Fixtures.react-theme-tokens.json");
        Assert.NotNull(stream);
        using var fixture = JsonDocument.Parse(stream!);
        var differences = new List<string>();
        var checkedTokens = 0;
        foreach (var scenario in fixture.RootElement.GetProperty("scenarios").EnumerateArray())
        {
            var id = scenario.GetProperty("id").GetString();
            var theme = new ThemeConfig { Algorithm = scenario.GetProperty("algorithm").GetString()! };
            foreach (var input in scenario.GetProperty("token").EnumerateObject())
                theme.Token[input.Name] = Text(input.Value);
            var actual = theme.GetResolvedTokenValues();
            foreach (var token in scenario.GetProperty("expected").EnumerateObject())
            {
                checkedTokens++;
                var expected = Text(token.Value);
                if (!actual.TryGetValue(token.Name, out var value))
                    differences.Add($"{id}.{token.Name}: missing (React {expected})");
                else if (!Equivalent(expected, value))
                    differences.Add($"{id}.{token.Name}: React {expected} / Blazor {value}");
            }
        }
        Assert.True(checkedTokens > 4000, "The fixture must cover all tokens, not a hand-picked subset.");
        Assert.True(differences.Count == 0, $"{differences.Count} differences in {checkedTokens} comparisons:\n" + string.Join("\n", differences));
    }

    private static string Text(JsonElement value) => value.ValueKind == JsonValueKind.String ? value.GetString()! : value.GetRawText();

    private static bool Equivalent(string expected, string actual)
    {
        if (double.TryParse(expected, NumberStyles.Float, CultureInfo.InvariantCulture, out var left)
            && double.TryParse(actual, NumberStyles.Float, CultureInfo.InvariantCulture, out var right))
            return Math.Abs(left - right) < 1e-12;
        return NormalizeCss(expected) == NormalizeCss(actual);
    }

    // Only CSS-equivalent serialization is normalized: whitespace, short hex,
    // case of hex digits, and fully opaque rgb/rgba. No token names are skipped.
    private static string NormalizeCss(string value)
    {
        value = Regex.Replace(value, @"#[0-9a-fA-F]{3,8}\b", match => {
            var hex = match.Value.Substring(1).ToLowerInvariant();
            if (hex.Length == 3) hex = string.Concat(hex.Select(c => new string(c, 2)));
            if (hex.Length != 6) return "#" + hex;
            return $"rgb({int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber)},{int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber)},{int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber)})";
        });
        value = Regex.Replace(value.Trim(), @"\s+", " ");
        value = Regex.Replace(value, @"\s*([(),])\s*", "$1");
        return Regex.Replace(value, @"rgba\((\d+,\d+,\d+),1\)", "rgb($1)");
    }
}
