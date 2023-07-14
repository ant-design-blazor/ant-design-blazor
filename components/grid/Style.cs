using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class GridRowToken
    {
    }

    public partial class GridColToken
    {
        public int GridColumns { get; set; }

    }

    public partial class Grid
    {
        public CSSObject GenGridRowStyle(Unknown_1 token)
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

        public CSSObject GenGridColStyle(Unknown_2 token)
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

        public CSSObject GenLoopGridColumnsStyle(GridColToken token, string sizeCls)
        {
            var componentCls = token.ComponentCls;
            var gridColumns = token.GridColumns;
            var gridColumnsStyle = new Unknown_3()
            {
            };
        }

        public CSSObject GenGridStyle(GridColToken token, string sizeCls)
        {
            return GenLoopGridColumnsStyle(token, sizeCls);
        }

        public CSSObject GenGridMediaStyle(GridColToken token, int screenSize, string sizeCls)
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

}