// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using CssInCSharp;

namespace AntDesign
{
    public class CompactItemOptions
    {
        public bool Focus { get; set; }
        public string BorderElCls { get; set; }
        public string FocusElCls { get; set; }
    }

    internal partial class GlobalStyle
    {
        public static CSSObject TextEllipsis = new()
        {
            Overflow = "hidden",
            WhiteSpace = "nowrap",
            TextOverflow = "ellipsis"
        };

        public static CSSObject SegmentedTextEllipsisCss = new()
        {
            Overflow = "hidden",
            // handle text ellipsis
            ["..."] = TextEllipsis,
        };

        public static CSSObject ResetComponent(GlobalToken token, bool needInheritFontFamily = false)
        {
            return new CSSObject
            {
                BoxSizing = "border-box",
                Margin = 0,
                Padding = 0,
                Color = token.ColorText,
                FontSize = token.FontSize,
                // font-variant: @font-variant-base;
                LineHeight = token.LineHeight,
                ListStyle = "none",
                // font-feature-settings: @font-feature-settings-base;
                FontFamily = needInheritFontFamily ? "inherit" : token.FontFamily,
            };
        }

        public static CSSObject ResetIcon()
        {
            return new CSSObject
            {
                Display = "inline-flex",
                AlignItems = "center",
                Color = "inherit",
                FontStyle = "normal",
                LineHeight = 0,
                TextAlign = "center",
                TextTransform = "none",
                VerticalAlign = "-0.125em",
                TextRendering = "optimizeLegibility",
                WebkitFontSmoothing = "antialiased",
                MozOsxFontSmoothing = "grayscale",
                ["> *"] = new CSSObject
                {
                    LineHeight = 1,
                },
                ["svg"] = new CSSObject
                {
                    Display = "inline-block",
                }
            };
        }

        public static CSSObject ClearFix()
        {
            return new CSSObject()
            {
                ["&::before"] = new CSSObject()
                {
                    Display = "table",
                    Content = "\"\""
                },
                ["&::after"] = new CSSObject()
                {
                    Display = "table",
                    Clear = "both",
                    Content = "\"\""
                },
            };
        }

        public static CSSObject GenLinkStyle(GlobalToken token)
        {
            return new CSSObject
            {
                ["a"] = new CSSObject
                {
                    Color = token.ColorLink,
                    TextDecoration = token.LinkDecoration,
                    BackgroundColor = "transparent",
                    Outline = "none",
                    Cursor = "pointer",
                    Transition = $"color {token.MotionDurationSlow}",
                    WebkitTextDecorationSkip = "objects",

                    ["&:hover"] = new CSSObject
                    {
                        Color = token.ColorLinkHover,
                    },

                    ["&:active"] = new CSSObject
                    {
                        Color = token.ColorLinkActive,
                    },

                    ["&:active,&:hover"] = new CSSObject
                    {
                        TextDecoration = token.LinkHoverDecoration,
                        Outline = 0,
                    },

                    ["&:focus"] = new CSSObject
                    {
                        TextDecoration = token.LinkFocusDecoration,
                        Outline = 0,
                    },

                    ["&[disabled]"] = new CSSObject
                    {
                        Color = token.ColorTextDisabled,
                        Cursor = "not-allowed",
                    },
                }
            };
        }

        public static CSSObject GenCommonStyle(GlobalToken token, string componentPrefixCls = "")
        {
            var fontFamily = token.FontFamily;
            var fontSize = token.FontSize;
            var rootPrefixSelector = $"[class^=\"${componentPrefixCls}\"], [class*=\" ${componentPrefixCls}\"]";

            return new CSSObject()
            {
                [$"{rootPrefixSelector}"] = new CSSObject()
                {
                    FontFamily = fontFamily,
                    FontSize = fontSize,
                    BoxSizing = "border-box",
                    ["&::before, &::after"] = new CSSObject()
                    {
                        BoxSizing = "border-box",
                    },
                    [$"{rootPrefixSelector}"] = new CSSObject()
                    {
                        BoxSizing = "border-box",
                        ["&::before, &::after"] = new CSSObject()
                        {
                            BoxSizing = "border-box",
                        },
                    }
                }
            };
        }

        public static CSSObject GenFocusOutline(GlobalToken token)
        {
            return new CSSObject()
            {
                Outline = $"{token.LineWidthFocus}px solid {token.ColorPrimaryBorder}",
                OutlineOffset = 1,
                Transition = "outline-offset 0s, outline 0s",
            };
        }

        public static CSSObject GenFocusStyle(GlobalToken token)
        {
            return new CSSObject()
            {
                ["&:focus-visible"] = GenFocusOutline(token)
            };
        }

        public static CSSObject CompactItemBorder(TokenWithCommonCls token, string parentCls, CompactItemOptions options)
        {
            var focusElCls = options.FocusElCls;
            var focus = options.Focus;
            var borderElCls = options.BorderElCls;
            var childCombinator = !string.IsNullOrEmpty(borderElCls) ? "> *" : "";
            var hoverEffects = new string[] { "hover", focus ? "focus" : null, "active" }
                .Where(x => x != null)
                .Select(n => $"&:{n} {childCombinator}")
                .Join(",");
            return new CSSObject()
            {
                [$"&-item:not({parentCls}-last-item)"] = new CSSObject()
                {
                    MarginInlineEnd = -token.LineWidth,
                },
                ["&-item"] = new CSSObject()
                {
                    [hoverEffects] = new CSSObject()
                    {
                        ZIndex = 2,
                    },
                    ["..."] = !string.IsNullOrEmpty(focusElCls)
                        ? new CSSObject()
                        {
                            [$"&{focusElCls}"] = new CSSObject()
                            {
                                ZIndex = 2,
                            }
                        }
                        : new CSSObject(),
                    [$"&[disabled] {childCombinator}"] = new CSSObject()
                    {
                        ZIndex = 2,
                    }
                }
            };
        }

        public static CSSObject CompactItemBorderRadius(string prefixCls, string parentCls, CompactItemOptions options)
        {
            var borderElCls = options.BorderElCls;
            var childCombinator = borderElCls != null ? $"> { borderElCls}" : "";
            return new CSSObject()
            {
                [$"&-item:not({parentCls}-first-item):not({parentCls}-last-item) {childCombinator}"] = new CSSObject()
                {
                    BorderRadius = 0,
                },
                [$"&-item:not({parentCls}-last-item){parentCls}-first-item"] = new CSSObject()
                {
                    [$"& {childCombinator}, &{prefixCls}-sm {childCombinator}, &{prefixCls}-lg {childCombinator}"] = new CSSObject()
                    {
                        BorderStartEndRadius = 0,
                        BorderEndEndRadius = 0,
                    }
                },
                [$"&-item:not({parentCls}-first-item){parentCls}-last-item"] = new CSSObject()
                {
                    [$"& {childCombinator}, &{prefixCls}-sm {childCombinator}, &{prefixCls}-lg {childCombinator}"] = new CSSObject()
                    {
                        BorderStartStartRadius = 0,
                        BorderEndStartRadius = 0,
                    }
                }
            };
        }

        public static CSSObject GenCompactItemStyle(TokenWithCommonCls token, CompactItemOptions options = null)
        {
            var componentCls = token.ComponentCls;
            var compactCls = $"${componentCls}-compact";
            return new CSSObject()
            {
                [compactCls] = new CSSObject()
                {
                    ["..."] = CompactItemBorder(token, compactCls, options),
                    ["..."] = CompactItemBorderRadius(componentCls, compactCls, options),
                }
            };
        }

        public static CSSObject CompactItemVerticalBorder(TokenWithCommonCls token, string parentCls)
        {
            return new CSSObject()
            {
                [$"&-item:not({parentCls}-last-item)"] = new CSSObject
                {
                    MarginBottom = -token.LineWidth,
                },
                ["&-item"] = new CSSObject()
                {
                    ["&:hover,&:focus,&:active"] = new CSSObject()
                    {
                        ZIndex = 2,
                    },
                    ["&[disabled]"] = new CSSObject()
                    {
                        ZIndex = 0,
                    },
                }
            };
        }

        public static CSSObject CompactItemBorderVerticalRadius(string prefixCls, string parentCls)
        {
            return new CSSObject()
            {
                [$"&-item:not({parentCls}-first-item):not({parentCls}-last-item)"] = new CSSObject()
                {
                    BorderRadius = 0,
                },

                [$"&-item{parentCls}-first-item:not({parentCls}-last-item)"] = new CSSObject()
                {
                    [$"&, &{prefixCls}-sm, &{prefixCls}-lg"] = new CSSObject()
                    {
                        BorderEndEndRadius = 0,
                        BorderEndStartRadius = 0,
                    }
                },

                [$"&-item{parentCls}-last-item:not({parentCls}-first-item)"] = new CSSObject()
                {
                    [$"&, &{prefixCls}-sm, &{prefixCls}-lg"] = new CSSObject()
                    {
                        BorderStartStartRadius = 0,
                        BorderStartEndRadius = 0,
                    }
                },
            };
        }

        public static CSSObject GenCompactItemVerticalStyle(TokenWithCommonCls token)
        {
            var compactCls = $"{token.ComponentCls}-compact-vertical";
            return new CSSObject()
            {
                [compactCls] = new CSSObject()
                {
                    ["..."] = CompactItemVerticalBorder(token, compactCls),
                    ["..."] = CompactItemBorderVerticalRadius(token.ComponentCls, compactCls),
                }
            };
        }

        public static CSSObject OperationUnit(TokenWithCommonCls token)
        {
            return new CSSObject()
            {
                Color = token.ColorLink,
                TextDecoration = "none",
                Outline = "",
                Cursor = "",
                Transition = $"color {token.MotionDurationSlow}",
                ["&:focus, &:hover"] = new CSSObject()
                {
                    Color = token.ColorLinkHover,
                },
                ["&:active"] = new CSSObject()
                {
                    Color = token.ColorLinkActive,
                },
            };
        }
    }
}
