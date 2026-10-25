using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AntColorPalette = AntDesign.Colors.ColorPalette;
using ColorMath = AntDesign.Colors.ThemeColorMath;

namespace AntDesign.Theming
{
    public interface IThemeAlgorithm
    {
        string Name { get; }
        IEnumerable<ResolvedToken> Resolve(IEnumerable<DesignToken> tokens);
    }

    public sealed class DefaultThemeAlgorithm : IThemeAlgorithm
    {
        public string Name => "default";
        public IEnumerable<ResolvedToken> Resolve(IEnumerable<DesignToken> tokens)
        {
            var values = GeneratedSeedTokenDefaults.All.ToDictionary(token => token.Key, token => token.Value, StringComparer.Ordinal);
            foreach (var token in tokens) values[token.Name] = token.Value;
            AddDerived(values, "controlHeightSM", values.Get("controlHeight", "32"), value => Scale(value, .75));
            AddDerived(values, "controlHeightXS", values.Get("controlHeight", "32"), value => Scale(value, .5));
            AddDerived(values, "controlHeightLG", values.Get("controlHeight", "32"), value => Scale(value, 1.25));
            var sizeStep = Parse(values.Get("sizeStep", "4"), 4);
            AddDerived(values, "sizeXXL", values.Get("sizeUnit", "4"), value => Scale(value, sizeStep + 8));
            AddDerived(values, "sizeXL", values.Get("sizeUnit", "4"), value => Scale(value, sizeStep + 4));
            AddDerived(values, "sizeLG", values.Get("sizeUnit", "4"), value => Scale(value, sizeStep + 2));
            AddDerived(values, "sizeMD", values.Get("sizeUnit", "4"), value => Scale(value, sizeStep + 1));
            AddDerived(values, "sizeMS", values.Get("sizeUnit", "4"), value => Scale(value, sizeStep));
            AddDerived(values, "size", values.Get("sizeUnit", "4"), value => Scale(value, sizeStep));
            AddDerived(values, "sizeSM", values.Get("sizeUnit", "4"), value => Scale(value, sizeStep - 1));
            AddDerived(values, "sizeXS", values.Get("sizeUnit", "4"), value => Scale(value, sizeStep - 2));
            AddDerived(values, "sizeXXS", values.Get("sizeUnit", "4"), value => Scale(value, sizeStep - 3));
            AddFontMap(values);
            AddRadiusMap(values);
            AddCommonMap(values);
            if (values.TryGetValue("colorTextBase", out var text)) values["colorText"] = ColorMath.Alpha(text, .88);
            if (values.TryGetValue("colorBgBase", out var background)) values["colorBgContainer"] = background;
            foreach (var name in new[] { "colorPrimary", "colorSuccess", "colorWarning", "colorError", "colorInfo", "colorLink" })
            {
                if (!values.TryGetValue(name, out var color)) continue;
                var palette = AntColorPalette.Generate(color);
                if (palette.Length == 10)
                {
                    var mapped = new[] { palette[0], palette[1], palette[2], palette[3], palette[4], palette[5], palette[6], palette[4], palette[5], palette[6] };
                    for (var index = 0; index < mapped.Length; index++)
                    {
                        values[$"{name}{index + 1}"] = mapped[index];
                        values[$"{name}-{index + 1}"] = mapped[index];
                    }
                }
            }
            foreach (var preset in PresetColorPalettes.TokenPalettes)
            {
                var color = values.Get(preset.Key, PresetColorPalettes.Primary(preset.Key));
                var palette = AntColorPalette.Generate(color);
                for (var index = 0; index < palette.Length; index++)
                {
                    values[$"{preset.Key}{index + 1}"] = palette[index];
                    values[$"{preset.Key}-{index + 1}"] = palette[index];
                }
                values[$"{preset.Key}Hover"] = palette[4];
                values[$"{preset.Key}Active"] = palette[6];
            }
            AddNeutralColorMap(values, false);
            AddColorSemanticMap(values);
            AddAliasMap(values);
            AddResponsiveAndShadowMap(values);
            return values.Select(item => new ResolvedToken(item.Key, item.Value)).OrderBy(item => item.Name, StringComparer.Ordinal);
        }

        private static void AddDerived(IDictionary<string, string> values, string name, string source, Func<string, string> derive)
        {
            if (!values.ContainsKey(name)) values[name] = derive(source);
        }

        private static string Scale(string value, double factor)
        {
            if (!double.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var number)) return value;
            return (number * factor).ToString("0.####################", System.Globalization.CultureInfo.InvariantCulture);
        }

        internal static void AddFontMap(IDictionary<string, string> values)
        {
            if (!double.TryParse(values.Get("fontSize", "14"), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var baseSize)) return;
            var sizes = new double[10];
            for (var index = 0; index < sizes.Length; index++)
            {
                var raw = baseSize * Math.Exp((index - 1) / 5d);
                var integer = index > 1 ? Math.Floor(raw) : Math.Ceiling(raw);
                sizes[index] = Math.Floor(integer / 2) * 2;
            }
            sizes[1] = baseSize;
            values["fontSizeSM"] = Number(sizes[0]);
            values["fontSize"] = Number(sizes[1]);
            values["fontSizeLG"] = Number(sizes[2]);
            values["fontSizeXL"] = Number(sizes[3]);
            for (var heading = 1; heading <= 5; heading++)
            {
                var size = sizes[7 - heading];
                values[$"fontSizeHeading{heading}"] = Number(size);
                values[$"lineHeightHeading{heading}"] = Number((size + 8) / size);
            }
            values["lineHeight"] = Number((baseSize + 8) / baseSize);
            values["lineHeightSM"] = Number((sizes[0] + 8) / sizes[0]);
            values["lineHeightLG"] = Number((sizes[2] + 8) / sizes[2]);
            values["fontHeight"] = Number(RoundJs(Parse(values["lineHeight"], 0) * baseSize));
            values["fontHeightSM"] = Number(RoundJs(Parse(values["lineHeightSM"], 0) * sizes[0]));
            values["fontHeightLG"] = Number(RoundJs(Parse(values["lineHeightLG"], 0) * sizes[2]));
        }

        internal static void AddRadiusMap(IDictionary<string, string> values)
        {
            var radius = Parse(values.Get("borderRadius", "6"), 6);
            values["borderRadiusXS"] = Number(radius >= 6 ? 2 : radius >= 2 ? 1 : radius);
            values["borderRadiusSM"] = Number(radius >= 16 ? 8 : radius >= 14 ? 7 : radius >= 8 ? 6 : radius >= 7 ? 5 : radius >= 5 ? 4 : radius);
            values["borderRadiusLG"] = Number(radius >= 16 ? 16 : radius >= 6 ? radius + 2 : radius >= 5 ? radius + 1 : radius);
            values["borderRadiusOuter"] = Number(radius >= 8 ? 6 : radius > 4 ? 4 : radius);
        }

        internal static void AddCommonMap(IDictionary<string, string> values)
        {
            var baseMotion = Parse(values.Get("motionBase", "0.1"), .1);
            var unit = Parse(values.Get("motionUnit", "0.1"), .1);
            values["motionDurationFast"] = (baseMotion + unit).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) + "s";
            values["motionDurationMid"] = (baseMotion + unit * 2).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) + "s";
            values["motionDurationSlow"] = (baseMotion + unit * 3).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) + "s";
            values["lineWidthBold"] = Number(Parse(values.Get("lineWidth", "1"), 1) + 1);
        }

        internal static void AddNeutralColorMap(IDictionary<string, string> values, bool dark)
        {
            var background = values.Get("colorBgBase", dark ? "#000000" : "#ffffff");
            var text = values.Get("colorTextBase", dark ? "#ffffff" : "#000000");
            if (string.IsNullOrEmpty(background)) background = dark ? "#000000" : "#ffffff";
            if (string.IsNullOrEmpty(text)) text = dark ? "#ffffff" : "#000000";
            values["colorBgBase"] = background;
            values["colorTextBase"] = text;
            values["colorShadow"] = values.Get("colorShadow", dark ? "rgba(255, 255, 255, 0.2)" : "#000000");
            var textAlpha = dark ? new[] { .85, .65, .45, .25 } : new[] { .88, .65, .45, .25 };
            values["colorText"] = ColorMath.Alpha(text, textAlpha[0]);
            values["colorTextSecondary"] = ColorMath.Alpha(text, textAlpha[1]);
            values["colorTextTertiary"] = ColorMath.Alpha(text, textAlpha[2]);
            values["colorTextQuaternary"] = ColorMath.Alpha(text, textAlpha[3]);
            var fillAlpha = dark ? new[] { .18, .12, .08, .04 } : new[] { .15, .06, .04, .02 };
            values["colorFill"] = ColorMath.Alpha(text, fillAlpha[0]);
            values["colorFillSecondary"] = ColorMath.Alpha(text, fillAlpha[1]);
            values["colorFillTertiary"] = ColorMath.Alpha(text, fillAlpha[2]);
            values["colorFillQuaternary"] = ColorMath.Alpha(text, fillAlpha[3]);
            values["colorBgSolid"] = ColorMath.Alpha(text, dark ? .95 : 1);
            values["colorBgSolidHover"] = ColorMath.Alpha(text, dark ? 1 : .75);
            values["colorBgSolidActive"] = ColorMath.Alpha(text, dark ? .9 : .95);
            values["colorBgLayout"] = dark ? ColorMath.Lighten(background, 0) : ColorMath.Darken(background, 4);
            values["colorBgContainer"] = dark ? ColorMath.Lighten(background, 8) : ColorMath.Darken(background, 0);
            values["colorBgElevated"] = dark ? ColorMath.Lighten(background, 12) : ColorMath.Darken(background, 0);
            values["colorBgSpotlight"] = dark ? ColorMath.Lighten(background, 26) : ColorMath.Alpha(text, .85);
            values["colorBgBlur"] = dark ? ColorMath.Alpha(text, .04) : "transparent";
            values["colorBorder"] = dark ? ColorMath.Lighten(background, 26) : ColorMath.Darken(background, 15);
            values["colorBorderDisabled"] = values["colorBorder"];
            values["colorBorderSecondary"] = dark ? ColorMath.Lighten(background, 19) : ColorMath.Darken(background, 6);
            values["colorWhite"] = "#ffffff";
        }

        internal static void AddColorSemanticMap(IDictionary<string, string> values, bool dark = false)
        {
            foreach (var name in new[] { "Primary", "Success", "Warning", "Error", "Info" })
            {
                var prefix = "color" + name;
                values[$"{prefix}Bg"] = values.Get(prefix + "1", "");
                values[$"{prefix}BgHover"] = values.Get(prefix + "2", "");
                values[$"{prefix}Border"] = values.Get(prefix + "3", "");
                values[$"{prefix}BorderHover"] = values.Get(prefix + "4", "");
                values[$"{prefix}Hover"] = values.Get(prefix + (name is "Primary" or "Error" ? "5" : "4"), "");
                values[prefix] = values.Get(prefix + "6", values.Get(prefix, ""));
                values[$"{prefix}Active"] = values.Get(prefix + "7", "");
                values[$"{prefix}TextHover"] = values.Get(prefix + "8", "");
                values[$"{prefix}Text"] = values.Get(prefix + "9", "");
                values[$"{prefix}TextActive"] = values.Get(prefix + "10", "");
            }
            var link = values.Get("colorLink", values.Get("colorInfo", ""));
            if (string.IsNullOrEmpty(link)) link = values.Get("colorInfo", "");
            var linkPalette = AntColorPalette.Generate(link, dark);
            if (linkPalette.Length == 10)
            {
                values["colorLinkHover"] = linkPalette[3];
                values["colorLink"] = linkPalette[5];
                values["colorLinkActive"] = dark ? linkPalette[4] : linkPalette[6];
            }
            values["colorErrorBgFilledHover"] = ColorMath.Mix(values.Get("colorError1", ""), values.Get("colorError3", ""), 50);
            values["colorErrorBgActive"] = values.Get("colorError3", "");
            values["colorBgMask"] = ColorMath.Alpha("#000000", .45);
        }

        internal static void AddAliasMap(IDictionary<string, string> values)
        {
            var aliases = new[]
            {
                ("colorFillContent", "colorFillSecondary"), ("colorFillContentHover", "colorFill"), ("colorFillAlter", "colorFillQuaternary"),
                ("colorBgContainerDisabled", "colorFillTertiary"), ("colorBorderBg", "colorBgContainer"), ("colorTextPlaceholder", "colorTextQuaternary"),
                ("colorTextDisabled", "colorTextQuaternary"), ("colorTextHeading", "colorText"), ("colorTextLabel", "colorTextSecondary"),
                ("colorTextDescription", "colorTextTertiary"), ("colorTextLightSolid", "colorWhite"), ("colorHighlight", "colorError"),
                ("colorBgTextHover", "colorFillSecondary"), ("colorBgTextActive", "colorFill"), ("colorIcon", "colorTextTertiary"),
                ("colorIconHover", "colorText"), ("colorErrorOutline", "colorErrorBg"), ("colorWarningOutline", "colorWarningBg"),
                ("colorErrorAffix", "colorError"), ("colorWarningAffix", "colorWarning"), ("controlItemBgHover", "colorFillTertiary"),
                ("controlItemBgActive", "colorPrimaryBg"), ("controlItemBgActiveHover", "colorPrimaryBgHover"), ("controlItemBgActiveDisabled", "colorFill")
            };
            foreach (var pair in aliases) values[pair.Item1] = values.Get(pair.Item2, "");
            values["colorErrorOutline"] = ColorMath.GetAlphaColor(values.Get("colorErrorBg", ""), values.Get("colorBgContainer", ""));
            values["colorWarningOutline"] = ColorMath.GetAlphaColor(values.Get("colorWarningBg", ""), values.Get("colorBgContainer", ""));
            values["colorSplit"] = ColorMath.GetAlphaColor(values.Get("colorBorderSecondary", ""), values.Get("colorBgContainer", ""));
            values["controlOutline"] = ColorMath.GetAlphaColor(values.Get("colorPrimaryBg", ""), values.Get("colorBgContainer", ""));
            values["controlTmpOutline"] = values.Get("colorFillQuaternary", "");
            values["fontSizeIcon"] = values.Get("fontSizeSM", "");
            values["fontHeight"] = Number(RoundJs(Parse(values.Get("fontSize", "14"), 14) * Parse(values.Get("lineHeight", "1"), 1)));
            values["fontHeightSM"] = Number(RoundJs(Parse(values.Get("fontSizeSM", "12"), 12) * Parse(values.Get("lineHeightSM", "1"), 1)));
            values["fontHeightLG"] = Number(RoundJs(Parse(values.Get("fontSizeLG", "16"), 16) * Parse(values.Get("lineHeightLG", "1"), 1)));
            values["fontWeightStrong"] = "600";
            values["opacityLoading"] = ".65";
            values["linkDecoration"] = values["linkHoverDecoration"] = values["linkFocusDecoration"] = "none";
            values["controlPaddingHorizontal"] = "12";
            values["controlPaddingHorizontalSM"] = "8";
            values["controlOutlineWidth"] = Scale(values.Get("lineWidth", "1"), 2);
            values["controlInteractiveSize"] = Scale(values.Get("controlHeight", "32"), .5);
            values["lineWidthFocus"] = values.Get("focusOutline", "true") == "false" ? "0" : Scale(values.Get("lineWidth", "1"), 3);
            foreach (var pair in new[] { ("paddingXXS", "sizeXXS"), ("paddingXS", "sizeXS"), ("paddingSM", "sizeSM"), ("padding", "size"), ("paddingMD", "sizeMD"), ("paddingLG", "sizeLG"), ("paddingXL", "sizeXL"), ("marginXXS", "sizeXXS"), ("marginXS", "sizeXS"), ("marginSM", "sizeSM"), ("margin", "size"), ("marginMD", "sizeMD"), ("marginLG", "sizeLG"), ("marginXL", "sizeXL"), ("marginXXL", "sizeXXL") }) values[pair.Item1] = values.Get(pair.Item2, "");
            values["paddingContentHorizontalLG"] = values.Get("sizeLG", "");
            values["paddingContentVerticalLG"] = values.Get("sizeMS", "");
            values["paddingContentHorizontal"] = values.Get("sizeMS", "");
            values["paddingContentVertical"] = values.Get("sizeSM", "");
            values["paddingContentHorizontalSM"] = values.Get("size", "");
            values["paddingContentVerticalSM"] = values.Get("sizeXS", "");
        }

        internal static void AddResponsiveAndShadowMap(IDictionary<string, string> values)
        {
            var xs = 480d; var sm = 576d; var md = 768d; var lg = 992d; var xl = 1200d; var xxl = 1600d; var xxxl = 1920d;
            foreach (var pair in new[] { ("screenXS", xs), ("screenSM", sm), ("screenMD", md), ("screenLG", lg), ("screenXL", xl), ("screenXXL", xxl), ("screenXXXL", xxxl) }) values[pair.Item1] = Number(pair.Item2);
            foreach (var pair in new[] { ("screenXSMin", xs), ("screenSMMin", sm), ("screenMDMin", md), ("screenLGMin", lg), ("screenXLMin", xl), ("screenXXLMin", xxl), ("screenXXXLMin", xxxl) }) values[pair.Item1] = Number(pair.Item2);
            foreach (var pair in new[] { ("screenXSMax", sm - 1), ("screenSMMax", md - 1), ("screenMDMax", lg - 1), ("screenLGMax", xl - 1), ("screenXLMax", xxl - 1), ("screenXXLMax", xxxl - 1) }) values[pair.Item1] = Number(pair.Item2);
            var shadow = values.Get("colorShadow", "#000000");
            string Shadow(double alpha) => ColorMath.WithAlpha(shadow, alpha);
            var box = $"0 6px 16px 0 {Shadow(.08)}, 0 3px 6px -4px {Shadow(.12)}, 0 9px 28px 8px {Shadow(.05)}";
            values["boxShadow"] = box;
            values["boxShadowSecondary"] = box;
            values["boxShadowTertiary"] = $"0 1px 2px 0 {Shadow(.05)}, 0 1px 6px -1px {Shadow(.03)}, 0 2px 4px 0 {Shadow(.03)}";
            values["boxShadowPopoverArrow"] = $"2px 2px 5px {Shadow(.05)}";
            values["dropShadowPopover"] = $"drop-shadow(0 6px 16px {Shadow(.08)}) drop-shadow(0 3px 6px {Shadow(.12)}) drop-shadow(0 9px 28px {Shadow(.05)})";
            values["boxShadowCard"] = $"0 1px 2px -2px {Shadow(.16)}, 0 3px 6px 0 {Shadow(.12)}, 0 5px 12px 4px {Shadow(.09)}";
            values["boxShadowDrawerRight"] = $"-6px 0 16px 0 {Shadow(.08)}, -3px 0 6px -4px {Shadow(.12)}, -9px 0 28px 8px {Shadow(.05)}";
            values["boxShadowDrawerLeft"] = $"6px 0 16px 0 {Shadow(.08)}, 3px 0 6px -4px {Shadow(.12)}, 9px 0 28px 8px {Shadow(.05)}";
            values["boxShadowDrawerUp"] = box;
            values["boxShadowDrawerDown"] = $"0 -6px 16px 0 {Shadow(.08)}, 0 -3px 6px -4px {Shadow(.12)}, 0 -9px 28px 8px {Shadow(.05)}";
            values["boxShadowTabsOverflowLeft"] = $"inset 10px 0 8px -8px {Shadow(.08)}";
            values["boxShadowTabsOverflowRight"] = $"inset -10px 0 8px -8px {Shadow(.08)}";
            values["boxShadowTabsOverflowTop"] = $"inset 0 10px 8px -8px {Shadow(.08)}";
            values["boxShadowTabsOverflowBottom"] = $"inset 0 -10px 8px -8px {Shadow(.08)}";
            if (string.Equals(values.Get("motion", "true"), "false", StringComparison.OrdinalIgnoreCase))
                values["motionDurationFast"] = values["motionDurationMid"] = values["motionDurationSlow"] = "0s";
        }

        internal static double Parse(string value, double fallback) => double.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var number) ? number : fallback;
        private static double RoundJs(double value) => Math.Floor(value + .5);
        internal static string Number(double value) => value.ToString("0.####################", System.Globalization.CultureInfo.InvariantCulture);
    }

    public sealed class DarkThemeAlgorithm : IThemeAlgorithm
    {
        public string Name => "dark";
        public IEnumerable<ResolvedToken> Resolve(IEnumerable<DesignToken> tokens)
        {
            var input = tokens.ToArray();
            var overrides = input.ToDictionary(token => token.Name, token => token.Value, StringComparer.Ordinal);
            var values = new DefaultThemeAlgorithm().Resolve(input).ToList();
            var map = values.ToDictionary(token => token.Name, token => token.Value, StringComparer.Ordinal);
            map["colorBgBase"] = overrides.Get("colorBgBase", "");
            map["colorTextBase"] = overrides.Get("colorTextBase", "");
            map["colorShadow"] = overrides.Get("colorShadow", "rgba(255, 255, 255, 0.2)");
            foreach (var name in new[] { "colorPrimary", "colorSuccess", "colorWarning", "colorError", "colorInfo", "colorLink" })
            {
                if (!map.TryGetValue(name, out var color)) continue;
                var palette = AntColorPalette.Generate(color, true);
                if (palette.Length == 10)
                {
                    var mapped = new[] { palette[0], palette[1], palette[2], palette[3], palette[6], palette[5], palette[4], palette[6], palette[5], palette[4] };
                    for (var index = 0; index < mapped.Length; index++)
                    {
                        map[$"{name}{index + 1}"] = mapped[index];
                        map[$"{name}-{index + 1}"] = mapped[index];
                    }
                }
            }
            foreach (var preset in PresetColorPalettes.TokenPalettes)
            {
                var color = map.Get(preset.Key, PresetColorPalettes.Primary(preset.Key));
                var palette = AntColorPalette.Generate(color, true);
                for (var index = 0; index < palette.Length; index++)
                {
                    map[$"{preset.Key}{index + 1}"] = palette[index];
                    map[$"{preset.Key}-{index + 1}"] = palette[index];
                }
                map[$"{preset.Key}Hover"] = palette[4];
                map[$"{preset.Key}Active"] = palette[6];
            }
            DefaultThemeAlgorithm.AddNeutralColorMap(map, true);
            DefaultThemeAlgorithm.AddColorSemanticMap(map, dark: true);
            map["colorPrimaryBg"] = map.Get("colorPrimary3", map.Get("colorPrimaryBg", ""));
            map["colorPrimaryBgHover"] = map.Get("colorPrimary4", map.Get("colorPrimaryBgHover", ""));
            DefaultThemeAlgorithm.AddAliasMap(map);
            DefaultThemeAlgorithm.AddResponsiveAndShadowMap(map);
            return map.Select(item => new ResolvedToken(item.Key, item.Value)).OrderBy(item => item.Name, StringComparer.Ordinal);
        }
    }

    public sealed class CompactThemeAlgorithm : IThemeAlgorithm
    {
        public string Name => "compact";

        public IEnumerable<ResolvedToken> Resolve(IEnumerable<DesignToken> tokens)
        {
            var map = new DefaultThemeAlgorithm().Resolve(tokens).ToDictionary(token => token.Name, token => token.Value, StringComparer.Ordinal);
            map["fontSize"] = map.Get("fontSizeSM", "12");
            var compactStep = DefaultThemeAlgorithm.Parse(map.Get("sizeStep", "4"), 4) - 2;
            var unit = DefaultThemeAlgorithm.Parse(map.Get("sizeUnit", "4"), 4);
            map["sizeXXL"] = DefaultThemeAlgorithm.Number(unit * (compactStep + 10));
            map["sizeXL"] = DefaultThemeAlgorithm.Number(unit * (compactStep + 6));
            map["sizeLG"] = DefaultThemeAlgorithm.Number(unit * (compactStep + 2));
            map["sizeMD"] = DefaultThemeAlgorithm.Number(unit * (compactStep + 2));
            map["sizeMS"] = DefaultThemeAlgorithm.Number(unit * (compactStep + 1));
            map["size"] = DefaultThemeAlgorithm.Number(unit * compactStep);
            map["sizeSM"] = map["size"];
            map["sizeXS"] = DefaultThemeAlgorithm.Number(unit * (compactStep - 1));
            map["sizeXXS"] = map["sizeXS"];
            var controlHeight = DefaultThemeAlgorithm.Parse(map.Get("controlHeight", "32"), 32) - 4;
            map["controlHeight"] = DefaultThemeAlgorithm.Number(controlHeight);
            map["controlHeightSM"] = DefaultThemeAlgorithm.Number(controlHeight * .75);
            map["controlHeightXS"] = DefaultThemeAlgorithm.Number(controlHeight * .5);
            map["controlHeightLG"] = DefaultThemeAlgorithm.Number(controlHeight * 1.25);
            DefaultThemeAlgorithm.AddFontMap(map);
            DefaultThemeAlgorithm.AddRadiusMap(map);
            DefaultThemeAlgorithm.AddCommonMap(map);
            DefaultThemeAlgorithm.AddAliasMap(map);
            DefaultThemeAlgorithm.AddResponsiveAndShadowMap(map);
            return map.Select(item => new ResolvedToken(item.Key, item.Value)).OrderBy(item => item.Name, StringComparer.Ordinal);
        }
    }



    internal static class TokenDictionaryExtensions
    {
        public static string Get(this IDictionary<string, string> values, string name, string fallback)
        {
            return values.TryGetValue(name, out var value) ? value : fallback;
        }
    }

    public static class PresetColorPalettes
    {
        private static readonly IReadOnlyDictionary<string, string> PrimaryColors = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["red"] = "#f5222d", ["volcano"] = "#fa541c", ["orange"] = "#fa8c16", ["gold"] = "#faad14",
            ["yellow"] = "#fadb14", ["lime"] = "#a0d911", ["green"] = "#52c41a", ["cyan"] = "#13c2c2",
            ["blue"] = "#1677ff", ["geekblue"] = "#2f54eb", ["purple"] = "#722ed1", ["magenta"] = "#eb2f96", ["pink"] = "#eb2f96",
            ["grey"] = "#8c8c8c"
        };

        // The @ant-design/colors public preset export excludes the deprecated
        // pink alias; Ant Design's theme derivative nevertheless emits its tokens.
        internal static IReadOnlyDictionary<string, string[]> TokenPalettes { get; } = PrimaryColors.ToDictionary(item => item.Key, item => AntColorPalette.Generate(item.Value), StringComparer.Ordinal);
        public static IReadOnlyDictionary<string, string[]> All { get; } = TokenPalettes.Where(item => item.Key != "pink").ToDictionary(item => item.Key, item => item.Value, StringComparer.Ordinal);
        public static IReadOnlyDictionary<string, string[]> Dark { get; } = PrimaryColors.Where(item => item.Key != "pink").ToDictionary(item => item.Key, item => AntColorPalette.Generate(item.Value, true), StringComparer.Ordinal);
        public static string Primary(string name) => PrimaryColors.TryGetValue(name, out var color) ? color : null;
    }

    public sealed class TokenCompilationManifest
    {
        public string Layer { get; set; } = "token-values";
        public string Theme { get; set; } = "default";
        public string Scope { get; set; } = ":root";
        public string Algorithm { get; set; } = "default";
        public string Digest { get; set; } = string.Empty;
        public string ManagedFile { get; set; } = "token-values.css";
        public string CacheKey { get; set; } = string.Empty;
    }

    public sealed class TokenCompilationResult
    {
        public IReadOnlyList<ResolvedToken> Resolved { get; internal set; } = Array.Empty<ResolvedToken>();
        public IReadOnlyList<CssVarToken> CssVariables { get; internal set; } = Array.Empty<CssVarToken>();
        public string Css { get; internal set; } = string.Empty;
        public TokenCompilationManifest Manifest { get; internal set; } = new();
    }

    public static class TokenCompiler
    {
        public static TokenCompilationResult Compile(
            IEnumerable<DesignToken> tokens,
            string selector,
            string theme = "default",
            IThemeAlgorithm algorithm = null,
            string managedFile = "token-values.css")
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            if (string.IsNullOrWhiteSpace(selector)) throw new ArgumentException("A CSS selector is required.", nameof(selector));
            algorithm ??= new DefaultThemeAlgorithm();
            var resolved = algorithm.Resolve(tokens).OrderBy(token => token.Name, StringComparer.Ordinal).ToList();
            var cssTokens = resolved.Select(token => new DesignToken
            {
                Name = token.Name,
                Value = token.Value,
                CssVariable = $"--ant-{TokenView.ToKebabCase(token.Name)}",
                Source = token.Source
            }).ToList();
            var css = TokenCssSerializer.SerializeVariables(selector, cssTokens);
            var digest = Sha256(css);
            var normalizedTheme = theme ?? "default";
            return new TokenCompilationResult
            {
                Resolved = resolved,
                CssVariables = cssTokens.Select(token => new CssVarToken(token.Name, token.CssVariable)).ToList(),
                Css = css,
                Manifest = new TokenCompilationManifest
                {
                    Theme = normalizedTheme,
                    Scope = selector,
                    Algorithm = algorithm.Name,
                    Digest = digest,
                    ManagedFile = managedFile ?? string.Empty,
                    CacheKey = $"token-values:{normalizedTheme}:{selector}:{algorithm.Name}:{digest}"
                }
            };
        }

        private static string Sha256(string value)
        {
            using var hash = SHA256.Create();
            return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(value))).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
