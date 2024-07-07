// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CssInCSharp;

namespace AntDesign
{
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

        public static CSSObject ResetComponent(GlobalToken token)
        {
            return new CSSObject();
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
            return new CSSObject();
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

        public static CSSObject GenCommonStyle(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GenFocusOutline(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GenFocusStyle(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GenCompactItemStyle(GlobalToken token, CompactItemOptions options = null)
        {
            return new CSSObject();
        }

        public static CSSObject GenCompactItemVerticalStyle(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GenPanelStyle(GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject GetCheckboxStyle(string key, GlobalToken token)
        {
            return new CSSObject();
        }

        public static CSSObject OperationUnit(GlobalToken token)
        {
            return new CSSObject();
        }
    }

    public class CompactItemOptions
    {
        public bool Focus { get; set; }
        public string BorderElCls { get; set; }
        public string FocusElCls { get; set; }
    }
}
