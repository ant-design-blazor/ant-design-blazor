// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CssInCSharp;
using static AntDesign.GlobalStyle;

namespace AntDesign
{
    public class ArrowPlacement
    {
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Top { get; set; }
        public bool Bottom { get; set; }
    }

    public class PlacementArrowOptions
    {
        public string ColorBg { get; set; }
        public string ShowArrowCls { get; set; }
        public double ContentRadius { get; set; }
        public bool LimitVerticalRadius { get; set; }
        public double ArrowDistance { get; set; }
        public ArrowPlacement ArrowPlacement { get; set; }
    }

    public class ArrowOffsetOptions
    {
        public double ContentRadius { get; set; }
        public bool LimitVerticalRadius { get; set; }
    }

    public class ArrowOffset
    {
        public double DropdownArrowOffset { get; set; }
        public double DropdownArrowOffsetVertical { get; set; }
    }

    public class PlacementArrow
    {
        public const double MAX_VERTICAL_CONTENT_RADIUS = 8;

        public static ArrowOffset GetArrowOffset(ArrowOffsetOptions options)
        {
            var maxVerticalContentRadius = MAX_VERTICAL_CONTENT_RADIUS;
            var contentRadius = options.ContentRadius;
            var limitVerticalRadius = options.LimitVerticalRadius;
            var dropdownArrowOffset = contentRadius > 12 ? contentRadius + 2 : 12;
            var dropdownArrowOffsetVertical = limitVerticalRadius ? maxVerticalContentRadius : dropdownArrowOffset;

            return new ArrowOffset
            {
                DropdownArrowOffset = dropdownArrowOffset,
                DropdownArrowOffsetVertical = dropdownArrowOffsetVertical,
            };
        }

        private static CSSObject IsInject(bool valid, CSSObject code)
        {
            if (!valid) return new CSSObject();
            return code;
        }

        public static CSSObject GetArrowStyle(TokenWithCommonCls token, PlacementArrowOptions options)
        {
            var componentCls = token.ComponentCls;
            var offset = GetArrowOffset(new ArrowOffsetOptions
            {
                ContentRadius = token.BorderRadiusLG,
                LimitVerticalRadius = options.LimitVerticalRadius
            });
            var arrowPlacement = options.ArrowPlacement;
            var arrowDistance = options.ArrowDistance;
            var dropdownArrowOffset = offset.DropdownArrowOffset;
            var dropdownArrowOffsetVertical = offset.DropdownArrowOffsetVertical;

            return new CSSObject()
            {
                [$"{componentCls}-arrow"] = new CSSObject()
                {
                    [$"${componentCls}-arrow"] = new CSSObject()
                    {
                        Position = "absolute",
                        ZIndex = 1,
                        Display = "block",
                        ["..."] = RoundedArrow(token.SizePopupArrow,
                            token.BorderRadiusXS,
                            token.BorderRadiusOuter,
                            options.ColorBg,
                            token.BoxShadowPopoverArrow
                        ),
                        ["&:before"] = new CSSObject()
                        {
                            Background = options.ColorBg,
                        }
                    }
                },

                ["..."] = IsInject(arrowPlacement.Top, new CSSObject()
                {
                    [$"&-placement-top {componentCls}-arrow,&-placement-topLeft {componentCls}-arrow,&-placement-topRight {componentCls}-arrow"] = new CSSObject()
                    {
                        Bottom = arrowDistance,
                        Transform = "translateY(100%) rotate(180deg)",
                    },

                    [$"&-placement-top {componentCls}-arrow"] = new CSSObject()
                    {
                        Left = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = "50%",
                        },
                        Transform = "translateX(-50%) translateY(100%) rotate(180deg)",
                    },

                    [$"&-placement-topLeft {componentCls}-arrow"] = new CSSObject()
                    {
                        Left = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = offset.DropdownArrowOffset,
                        },
                    },

                    [$"&-placement-topRight {componentCls}-arrow"] = new CSSObject()
                    {
                        Right = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = offset.DropdownArrowOffset,
                        },
                    },
                }),

                ["..."] = IsInject(arrowPlacement.Bottom, new CSSObject()
                {
                    [$"&-placement-bottom {componentCls}-arrow,&-placement-bottomLeft {componentCls}-arrow,&-placement-bottomRight {componentCls}-arrow"] = new CSSObject()
                    {
                        Top = arrowDistance,
                        Transform = "translateY(-100%)",
                    },

                    [$"&-placement-bottom {componentCls}-arrow"] = new CSSObject()
                    {
                        Left = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = "50%",
                        },
                        Transform = "translateX(-50%) translateY(-100%)",
                    },

                    [$"&-placement-bottomRight {componentCls}-arrow"] = new CSSObject()
                    {
                        Left = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = dropdownArrowOffset,
                        },
                    },

                    [$"&-placement-topRight {componentCls}-arrow"] = new CSSObject()
                    {
                        Right = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = dropdownArrowOffset,
                        },
                    },
                }),

                ["..."] = IsInject(arrowPlacement.Left, new CSSObject()
                {
                    [$"&-placement-left {componentCls}-arrow,&-placement-leftTop {componentCls}-arrow,&-placement-leftBottom {componentCls}-arrow"] = new CSSObject()
                    {
                        Right = arrowDistance,
                        Transform = "translateX(100%) rotate(90deg)",
                    },

                    [$"&-placement-left {componentCls}-arrow"] = new CSSObject()
                    {
                        Top = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = "50%",
                        },
                        Transform = "translateY(-50%) translateX(100%) rotate(90deg)",
                    },

                    [$"&-placement-leftTop {componentCls}-arrow"] = new CSSObject()
                    {
                        Top = dropdownArrowOffsetVertical,
                    },

                    [$"&-placement-leftBottom {componentCls}-arrow"] = new CSSObject()
                    {
                        Bottom = dropdownArrowOffsetVertical,
                    },
                }),

                ["..."] = IsInject(arrowPlacement.Right, new CSSObject()
                {
                    [$"&-placement-right {componentCls}-arrow,&-placement-rightTop {componentCls}-arrow,&-placement-rightBottom {componentCls}-arrow"] = new CSSObject()
                    {
                        Left = arrowDistance,
                        Transform = "translateX(-100%) rotate(-90deg)",
                    },

                    [$"&-placement-right {componentCls}-arrow"] = new CSSObject()
                    {
                        Top = new PropertySkip()
                        {
                            SkipCheck = true,
                            Value = "50%",
                        },
                        Transform = "translateY(-50%) translateX(-100%) rotate(-90deg)",
                    },

                    [$"&-placement-rightTop {componentCls}-arrow"] = new CSSObject()
                    {
                        Top = dropdownArrowOffsetVertical,
                    },

                    [$"&-placement-rightBottom {componentCls}-arrow"] = new CSSObject()
                    {
                        Bottom = dropdownArrowOffsetVertical,
                    },
                }),
            };
        }
    }
}
