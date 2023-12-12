using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.GridStyle;

namespace AntDesign
{
    public class GridRowToken : TokenWithCommonCls
    {
    }

    public class GridColToken : TokenWithCommonCls
    {
        public double GridColumns
        {
            get => (double)_tokens["gridColumns"];
            set => _tokens["gridColumns"] = value;
        }

    }

    public class GridStyle
    {
        public static CSSObject GenGridRowStyle(GridRowToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    Display = "flex",
                    FlexFlow = "row wrap",
                    MinWidth = 0,
                    ["&::before, &::after"] = new CSSObject()
                    {
                        Display = "flex",
                    },
                    ["&-no-wrap"] = new CSSObject()
                    {
                        FlexWrap = "nowrap",
                    },
                    ["&-start"] = new CSSObject()
                    {
                        JustifyContent = "flex-start",
                    },
                    ["&-center"] = new CSSObject()
                    {
                        JustifyContent = "center",
                    },
                    ["&-end"] = new CSSObject()
                    {
                        JustifyContent = "flex-end",
                    },
                    ["&-space-between"] = new CSSObject()
                    {
                        JustifyContent = "space-between",
                    },
                    ["&-space-around"] = new CSSObject()
                    {
                        JustifyContent = "space-around",
                    },
                    ["&-space-evenly"] = new CSSObject()
                    {
                        JustifyContent = "space-evenly",
                    },
                    ["&-top"] = new CSSObject()
                    {
                        AlignItems = "flex-start",
                    },
                    ["&-middle"] = new CSSObject()
                    {
                        AlignItems = "center",
                    },
                    ["&-bottom"] = new CSSObject()
                    {
                        AlignItems = "flex-end",
                    },
                },
            };
        }

        public static CSSObject GenGridColStyle(GridColToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    Position = "relative",
                    MaxWidth = "100%",
                    MinHeight = 1,
                },
            };
        }

        public static CSSObject GenLoopGridColumnsStyle(GridColToken token, string sizeCls)
        {
            var componentCls = token.ComponentCls;
            var gridColumns = token.GridColumns;
            var gridColumnsStyle = new CSSObject()
            {
            };
            for (var i = gridColumns; i >= 0; i--)
            {
                if (i == 0)
                {
                  gridColumnsStyle[@$"{componentCls}{sizeCls}-{i}"] = new CSSObject
                  {
                    Display = "none",
                  };
                  gridColumnsStyle[@$"{componentCls}-push-{i}"] = new CSSObject
                  {
                    InsetInlineStart = "auto",
                  };
                  gridColumnsStyle[@$"{componentCls}-pull-{i}"] = new CSSObject
                  {
                    InsetInlineEnd = "auto",
                  };
                  gridColumnsStyle[@$"{componentCls}{sizeCls}-push-{i}"] = new CSSObject
                  {
                    InsetInlineStart = "auto",
                  };
                  gridColumnsStyle[@$"{componentCls}{sizeCls}-pull-{i}"] = new CSSObject
                  {
                    InsetInlineEnd = "auto",
                  };
                  gridColumnsStyle[@$"{componentCls}{sizeCls}-offset-{i}"] = new CSSObject
                  {
                    MarginInlineStart = 0,
                  };
                  gridColumnsStyle[@$"{componentCls}{sizeCls}-order-{i}"] = new CSSObject{
                    Order = 0,
                  };
                }
                else
                {
                  gridColumnsStyle[@$"{componentCls}{sizeCls}-{i}"] = new CSSInterpolation []
                  {
                    new CSSObject {
                      // ["--ant-display"] = "block",
                      Display = "block",
                    },
                    new CSSObject {
                      Display = "block", // "var(--ant-display)",
                      Flex = @$"0 0 {(i / gridColumns) * 100}%",
                      MaxWidth = @$"{(i / gridColumns) * 100}%",
                    },
                  };
                  gridColumnsStyle[@$"{componentCls}{sizeCls}-push-{i}"] = new CSSObject
                  {
                    InsetInlineStart = @$"{(i / gridColumns) * 100}%",
                  };
                  gridColumnsStyle[@$"{componentCls}{sizeCls}-pull-{i}"] = new CSSObject
                  {
                    InsetInlineEnd = @$"{(i / gridColumns) * 100}%",
                  };
                  gridColumnsStyle[@$"{componentCls}{sizeCls}-offset-{i}"] = new CSSObject
                  {
                    MarginInlineStart = @$"{(i / gridColumns) * 100}%",
                  };
                  gridColumnsStyle[@$"{componentCls}{sizeCls}-order-{i}"] = new CSSObject
                  {
                    Order = i,
                  };
                }
            }

            return gridColumnsStyle;
        }

        public static CSSObject GenGridStyle(GridColToken token, string sizeCls)
        {
            return GenLoopGridColumnsStyle(token, sizeCls);
        }

        public static CSSObject GenGridMediaStyle(GridColToken token, double screenSize, string sizeCls)
        {
            return new CSSObject()
            {
                [$"@media (min-width: {screenSize}px)"] = new CSSObject()
                {
                    ["..."] = GenGridStyle(token, sizeCls)
                },
            };
        }
    }

    public partial class Row
    {
        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook("Grid", (token) =>
            {
                return new CSSInterpolation[] { GenGridRowStyle(MergeToken(token, new GridRowToken())) };
            });
        }
    }

    public partial class Col
    {
        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook("Grid", (token) =>
            {
                var gridToken = MergeToken<GridColToken>(token, new GridColToken 
                {
                    GridColumns = 24,
                });
                return new CSSInterpolation[]
                {
                    GenGridColStyle(gridToken),
                    GenGridStyle(gridToken, ""),
                    GenGridStyle(gridToken, "-xs"),
                    GenGridMediaStyle(gridToken, gridToken.ScreenSMMin, "-sm"),
                    GenGridMediaStyle(gridToken, gridToken.ScreenMDMin, "-md"),
                    GenGridMediaStyle(gridToken, gridToken.ScreenLGMin, "-lg"),
                    GenGridMediaStyle(gridToken, gridToken.ScreenXLMin, "-xl"),
                    GenGridMediaStyle(gridToken, gridToken.ScreenXXLMin, "-xxl"),
                };
            });
        }
    }
}
