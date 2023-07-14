using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class TableToken
    {
    }

    public partial class TableToken : TokenWithCommonCls
    {
        public int TableFontSize { get; set; }

        public string TableBg { get; set; }

        public int TableRadius { get; set; }

        public int TablePaddingHorizontal { get; set; }

        public int TablePaddingVertical { get; set; }

        public int TablePaddingHorizontalMiddle { get; set; }

        public int TablePaddingVerticalMiddle { get; set; }

        public int TablePaddingHorizontalSmall { get; set; }

        public int TablePaddingVerticalSmall { get; set; }

        public string TableBorderColor { get; set; }

        public string TableHeaderTextColor { get; set; }

        public string TableHeaderBg { get; set; }

        public string TableFooterTextColor { get; set; }

        public string TableFooterBg { get; set; }

        public string TableHeaderCellSplitColor { get; set; }

        public string TableHeaderSortBg { get; set; }

        public string TableHeaderSortHoverBg { get; set; }

        public string TableHeaderIconColor { get; set; }

        public string TableHeaderIconColorHover { get; set; }

        public string TableBodySortBg { get; set; }

        public string TableFixedHeaderSortActiveBg { get; set; }

        public string TableHeaderFilterActiveBg { get; set; }

        public string TableFilterDropdownBg { get; set; }

        public int TableFilterDropdownHeight { get; set; }

        public string TableRowHoverBg { get; set; }

        public string TableSelectedRowBg { get; set; }

        public string TableSelectedRowHoverBg { get; set; }

        public int TableFontSizeMiddle { get; set; }

        public int TableFontSizeSmall { get; set; }

        public int TableSelectionColumnWidth { get; set; }

        public string TableExpandIconBg { get; set; }

        public int TableExpandColumnWidth { get; set; }

        public string TableExpandedRowBg { get; set; }

        public int TableFilterDropdownWidth { get; set; }

        public int TableFilterDropdownSearchWidth { get; set; }

        public int ZIndexTableFixed { get; set; }

        public int ZIndexTableSticky { get; set; }

        public int TableScrollThumbSize { get; set; }

        public string TableScrollThumbBg { get; set; }

        public string TableScrollThumbBgHover { get; set; }

        public string TableScrollBg { get; set; }

    }

    public partial class Table
    {
        public Unknown_1 GenTableStyle(Unknown_19 token)
        {
            var componentCls = token.ComponentCls;
            var fontWeightStrong = token.FontWeightStrong;
            var tablePaddingVertical = token.TablePaddingVertical;
            var tablePaddingHorizontal = token.TablePaddingHorizontal;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var tableBorderColor = token.TableBorderColor;
            var tableFontSize = token.TableFontSize;
            var tableBg = token.TableBg;
            var tableRadius = token.TableRadius;
            var tableHeaderTextColor = token.TableHeaderTextColor;
            var motionDurationMid = token.MotionDurationMid;
            var tableHeaderBg = token.TableHeaderBg;
            var tableHeaderCellSplitColor = token.TableHeaderCellSplitColor;
            var tableRowHoverBg = token.TableRowHoverBg;
            var tableSelectedRowBg = token.TableSelectedRowBg;
            var tableSelectedRowHoverBg = token.TableSelectedRowHoverBg;
            var tableFooterTextColor = token.TableFooterTextColor;
            var tableFooterBg = token.TableFooterBg;
            var paddingContentVerticalLG = token.PaddingContentVerticalLG;
            var tableBorder = @$"{lineWidth}px {lineType} {tableBorderColor}";
            return new Unknown_20()
            {
                [$"{componentCls}-wrapper"] = new Unknown_21()
                {
                    Clear = "both",
                    MaxWidth = "100%",
                    ["..."] = ClearFix(),
                    [componentCls] = new Unknown_22()
                    {
                        ["..."] = ResetComponent(token),
                        FontSize = tableFontSize,
                        Background = tableBg,
                        BorderRadius = @$"{tableRadius}px {tableRadius}px 0 0",
                    },
                    ["table"] = new Unknown_23()
                    {
                        Width = "100%",
                        TextAlign = "start",
                        BorderRadius = @$"{tableRadius}px {tableRadius}px 0 0",
                        BorderCollapse = "separate",
                        BorderSpacing = 0,
                    },
                    [$"{componentCls}-thead>tr>th,{componentCls}-tbody>tr>th,{componentCls}-tbody>tr>td,tfoot>tr>th,tfoot>tr>td"] = new Unknown_24()
                    {
                        Position = "relative",
                        Padding = @$"{paddingContentVerticalLG}px {tablePaddingHorizontal}px",
                        OverflowWrap = "break-word",
                    },
                    [$"{componentCls}-title"] = new Unknown_25()
                    {
                        Padding = @$"{tablePaddingVertical}px {tablePaddingHorizontal}px",
                    },
                    [$"{componentCls}-thead"] = new Unknown_26()
                    {
                        [">tr>th,>tr>td"] = new Unknown_27()
                        {
                            Position = "relative",
                            Color = tableHeaderTextColor,
                            FontWeight = fontWeightStrong,
                            TextAlign = "start",
                            Background = tableHeaderBg,
                            BorderBottom = tableBorder,
                            Transition = @$"background {motionDurationMid} ease",
                            ["&[colspan]:not([colspan=\"1\"])"] = new Unknown_28()
                            {
                                TextAlign = "center",
                            },
                            [$"&:not(:last-child):not({componentCls}-selection-column):not({componentCls}-row-expand-icon-cell):not([colspan])::before"] = new Unknown_29()
                            {
                                Position = "absolute",
                                Top = "50%",
                                InsetInlineEnd = 0,
                                Width = 1,
                                Height = "1.6em",
                                BackgroundColor = tableHeaderCellSplitColor,
                                Transform = "translateY(-50%)",
                                Transition = @$"background-color {motionDurationMid}",
                                Content = "\"\"",
                            },
                        },
                        ["> tr:not(:last-child) > th[colspan]"] = new Unknown_30()
                        {
                            BorderBottom = 0,
                        },
                    },
                    [$"{componentCls}-tbody"] = new Unknown_31()
                    {
                        ["> tr"] = new Unknown_32()
                        {
                            ["> th, > td"] = new Unknown_33()
                            {
                                Transition = @$"background {motionDurationMid}, border-color {motionDurationMid}",
                                BorderBottom = tableBorder,
                                [$">{componentCls}-wrapper:only-child,>{componentCls}-expanded-row-fixed>{componentCls}-wrapper:only-child"] = new Unknown_34()
                                {
                                    [componentCls] = new Unknown_35()
                                    {
                                        MarginBlock = @$"-{tablePaddingVertical}px",
                                        MarginInline = @$"{
                  token.TableExpandColumnWidth - tablePaddingHorizontal
                }px -{tablePaddingHorizontal}px",
                                        [$"{componentCls}-tbody > tr:last-child > td"] = new Unknown_36()
                                        {
                                            BorderBottom = 0,
                                            ["&:first-child, &:last-child"] = new Unknown_37()
                                            {
                                                BorderRadius = 0,
                                            },
                                        },
                                    },
                                },
                            },
                            ["> th"] = new Unknown_38()
                            {
                                Position = "relative",
                                Color = tableHeaderTextColor,
                                FontWeight = fontWeightStrong,
                                TextAlign = "start",
                                Background = tableHeaderBg,
                                BorderBottom = tableBorder,
                                Transition = @$"background {motionDurationMid} ease",
                            },
                            [$"&{componentCls}-row:hover>th,&{componentCls}-row:hover>td,>th{componentCls}-cell-row-hover>td{componentCls}-cell-row-hover"] = new Unknown_39()
                            {
                                Background = tableRowHoverBg,
                            },
                            [$"&{componentCls}-row-selected"] = new Unknown_40()
                            {
                                ["> th, > td"] = new Unknown_41()
                                {
                                    Background = tableSelectedRowBg,
                                },
                                ["&:hover > th, &:hover > td"] = new Unknown_42()
                                {
                                    Background = tableSelectedRowHoverBg,
                                },
                            },
                        },
                    },
                    [$"{componentCls}-footer"] = new Unknown_43()
                    {
                        Padding = @$"{tablePaddingVertical}px {tablePaddingHorizontal}px",
                        Color = tableFooterTextColor,
                        Background = tableFooterBg,
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_44 token)
        {
            var controlItemBgActive = token.ControlItemBgActive;
            var controlItemBgActiveHover = token.ControlItemBgActiveHover;
            var colorTextPlaceholder = token.ColorTextPlaceholder;
            var colorTextHeading = token.ColorTextHeading;
            var colorSplit = token.ColorSplit;
            var colorBorderSecondary = token.ColorBorderSecondary;
            var fontSize = token.FontSize;
            var padding = token.Padding;
            var paddingXS = token.PaddingXS;
            var paddingSM = token.PaddingSM;
            var controlHeight = token.ControlHeight;
            var colorFillAlter = token.ColorFillAlter;
            var colorIcon = token.ColorIcon;
            var colorIconHover = token.ColorIconHover;
            var opacityLoading = token.OpacityLoading;
            var colorBgContainer = token.ColorBgContainer;
            var borderRadiusLG = token.BorderRadiusLG;
            var colorFillContent = token.ColorFillContent;
            var colorFillSecondary = token.ColorFillSecondary;
            var checkboxSize = token.ControlInteractiveSize;
            var baseColorAction = New TinyColor(colorIcon);
            var baseColorActionHover = New TinyColor(colorIconHover);
            var tableSelectedRowBg = controlItemBgActive;
            var zIndexTableFixed = 2;
            var colorFillSecondarySolid = New TinyColor(colorFillSecondary)
    .onBackground(colorBgContainer)
    .toHexShortString();
            var colorFillContentSolid = New TinyColor(colorFillContent)
    .onBackground(colorBgContainer)
    .toHexShortString();
            var colorFillAlterSolid = New TinyColor(colorFillAlter)
    .onBackground(colorBgContainer)
    .toHexShortString();
            var tableToken = MergeToken(
                token,
                new Unknown_45()
                {
                    TableFontSize = fontSize,
                    TableBg = colorBgContainer,
                    TableRadius = borderRadiusLG,
                    TablePaddingVertical = padding,
                    TablePaddingHorizontal = padding,
                    TablePaddingVerticalMiddle = paddingSM,
                    TablePaddingHorizontalMiddle = paddingXS,
                    TablePaddingVerticalSmall = paddingXS,
                    TablePaddingHorizontalSmall = paddingXS,
                    TableBorderColor = colorBorderSecondary,
                    TableHeaderTextColor = colorTextHeading,
                    TableHeaderBg = colorFillAlterSolid,
                    TableFooterTextColor = colorTextHeading,
                    TableFooterBg = colorFillAlterSolid,
                    TableHeaderCellSplitColor = colorBorderSecondary,
                    TableHeaderSortBg = colorFillSecondarySolid,
                    TableHeaderSortHoverBg = colorFillContentSolid,
                    TableHeaderIconColor = BaseColorAction
      .clone()
      .setAlpha(baseColorAction.GetAlpha() * opacityLoading)
      .toRgbString(),
                    TableHeaderIconColorHover = BaseColorActionHover
      .clone()
      .setAlpha(baseColorActionHover.GetAlpha() * opacityLoading)
      .toRgbString(),
                    TableBodySortBg = colorFillAlterSolid,
                    TableFixedHeaderSortActiveBg = colorFillSecondarySolid,
                    TableHeaderFilterActiveBg = colorFillContent,
                    TableFilterDropdownBg = colorBgContainer,
                    TableRowHoverBg = colorFillAlterSolid,
                    TableSelectedRowBg = tableSelectedRowBg,
                    TableSelectedRowHoverBg = controlItemBgActiveHover,
                    ZIndexTableFixed = zIndexTableFixed,
                    ZIndexTableSticky = zIndexTableFixed + 1,
                    TableFontSizeMiddle = fontSize,
                    TableFontSizeSmall = fontSize,
                    TableSelectionColumnWidth = controlHeight,
                    TableExpandIconBg = colorBgContainer,
                    TableExpandColumnWidth = checkboxSize + 2 * token.Padding,
                    TableExpandedRowBg = colorFillAlter,
                    TableFilterDropdownWidth = 120,
                    TableFilterDropdownHeight = 264,
                    TableFilterDropdownSearchWidth = 140,
                    TableScrollThumbSize = 8,
                    TableScrollThumbBg = colorTextPlaceholder,
                    TableScrollThumbBgHover = colorTextHeading,
                    TableScrollBg = colorSplit,
                });
            return new Unknown_46
            {
                GenTableStyle(tableToken),
                GenPaginationStyle(tableToken),
                GenSummaryStyle(tableToken),
                GenSorterStyle(tableToken),
                GenFilterStyle(tableToken),
                GenBorderedStyle(tableToken),
                GenRadiusStyle(tableToken),
                GenExpandStyle(tableToken),
                GenSummaryStyle(tableToken),
                GenEmptyStyle(tableToken),
                GenSelectionStyle(tableToken),
                GenFixedStyle(tableToken),
                GenStickyStyle(tableToken),
                GenEllipsisStyle(tableToken),
                GenSizeStyle(tableToken),
                GenRtlStyle(tableToken)
            };
        }

        public Unknown_4 GenBorderedStyle(Unknown_47 token)
        {
            var componentCls = token.ComponentCls;
            var tableBorder = @$"{token.LineWidth}px {token.LineType} {token.TableBorderColor}";
            var getSizeBorderStyle = ('small' | 'middle' size, int paddingVertical, int paddingHorizontal) => {
                return new Unknown_48()
                {
                    [$"&{componentCls}-{size}"] = new Unknown_49()
                    {
                        [$"> {componentCls}-container"] = new Unknown_50()
                        {
                            [$"> {componentCls}-content, > {componentCls}-body"] = new Unknown_51()
                            {
                                [">table>tbody>tr>th,>table>tbody>tr>td"] = new Unknown_52()
                                {
                                    [$"> {componentCls}-expanded-row-fixed"] = new Unknown_53()
                                    {
                                        Margin = @$"-{paddingVertical}px -{paddingHorizontal + token.LineWidth}px",
                                    },
                                },
                            },
                        },
                    },
                };
            };
            return new Unknown_54()
            {
                [$"{componentCls}-wrapper"] = new Unknown_55()
                {
                    [$"{componentCls}{componentCls}-bordered"] = new Unknown_56()
                    {
                        [$"> {componentCls}-title"] = new Unknown_57()
                        {
                            Border = tableBorder,
                            BorderBottom = 0,
                        },
                        [$"> {componentCls}-container"] = new Unknown_58()
                        {
                            BorderInlineStart = tableBorder,
                            BorderTop = tableBorder,
                            [$">{componentCls}-content,>{componentCls}-header,>{componentCls}-body,>{componentCls}-summary"] = new Unknown_59()
                            {
                                ["> table"] = new Unknown_60()
                                {
                                    [">thead>tr>th,>thead>tr>td,>tbody>tr>th,>tbody>tr>td,>tfoot>tr>th,>tfoot>tr>td"] = new Unknown_61()
                                    {
                                        BorderInlineEnd = tableBorder,
                                    },
                                    ["> thead"] = new Unknown_62()
                                    {
                                        ["> tr:not(:last-child) > th"] = new Unknown_63()
                                        {
                                            BorderBottom = tableBorder,
                                        },
                                        ["> tr > th::before"] = new Unknown_64()
                                        {
                                            BackgroundColor = "transparent !important",
                                        },
                                    },
                                    [">thead>tr,>tbody>tr,>tfoot>tr"] = new Unknown_65()
                                    {
                                        [$"> {componentCls}-cell-fix-right-first::after"] = new Unknown_66()
                                        {
                                            BorderInlineEnd = tableBorder,
                                        },
                                    },
                                    [">tbody>tr>th,>tbody>tr>td"] = new Unknown_67()
                                    {
                                        [$"> {componentCls}-expanded-row-fixed"] = new Unknown_68()
                                        {
                                            Margin = @$"-{token.TablePaddingVertical}px -{
                    token.TablePaddingHorizontal + token.LineWidth
                  }px",
                                            ["&::after"] = new Unknown_69()
                                            {
                                                Position = "absolute",
                                                Top = 0,
                                                InsetInlineEnd = token.LineWidth,
                                                Bottom = 0,
                                                BorderInlineEnd = tableBorder,
                                                Content = "\"\"",
                                            },
                                        },
                                    },
                                },
                            },
                        },
                        [$"&{componentCls}-scroll-horizontal"] = new Unknown_70()
                        {
                            [$"> {componentCls}-container > {componentCls}-body"] = new Unknown_71()
                            {
                                ["> table > tbody"] = new Unknown_72()
                                {
                                    [$">tr{componentCls}-expanded-row,>tr{componentCls}-placeholder"] = new Unknown_73()
                                    {
                                        ["> th, > td"] = new Unknown_74()
                                        {
                                            BorderInlineEnd = 0,
                                        },
                                    },
                                },
                            },
                        },
                        ["..."] = GetSizeBorderStyle("middle", token.TablePaddingVerticalMiddle, token.TablePaddingHorizontalMiddle),
                        ["..."] = GetSizeBorderStyle("small", token.TablePaddingVerticalSmall, token.TablePaddingHorizontalSmall),
                        [$"> {componentCls}-footer"] = new Unknown_75()
                        {
                            Border = tableBorder,
                            BorderTop = 0,
                        },
                    },
                    [$"{componentCls}-cell"] = new Unknown_76()
                    {
                        [$"{componentCls}-container:first-child"] = new Unknown_77()
                        {
                            BorderTop = 0,
                        },
                        ["&-scrollbar:not([rowspan])"] = new Unknown_78()
                        {
                            BoxShadow = @$"0 {token.LineWidth}px 0 {token.LineWidth}px {token.TableHeaderBg}",
                        },
                    },
                    [$"{componentCls}-bordered {componentCls}-cell-scrollbar"] = new Unknown_79()
                    {
                        BorderInlineEnd = tableBorder,
                    },
                },
            };
        }

        public Unknown_5 GenEllipsisStyle(Unknown_80 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_81()
            {
                [$"{componentCls}-wrapper"] = new Unknown_82()
                {
                    [$"{componentCls}-cell-ellipsis"] = new Unknown_83()
                    {
                        ["..."] = textEllipsis,
                        WordBreak = "keep-all",
                        [$"&{componentCls}-cell-fix-left-last,&{componentCls}-cell-fix-right-first"] = new Unknown_84()
                        {
                            Overflow = "visible",
                            [$"{componentCls}-cell-content"] = new Unknown_85()
                            {
                                Display = "block",
                                Overflow = "hidden",
                                TextOverflow = "ellipsis",
                            },
                        },
                        [$"{componentCls}-column-title"] = new Unknown_86()
                        {
                            Overflow = "hidden",
                            TextOverflow = "ellipsis",
                            WordBreak = "keep-all",
                        },
                    },
                },
            };
        }

        public Unknown_6 GenEmptyStyle(Unknown_87 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_88()
            {
                [$"{componentCls}-wrapper"] = new Unknown_89()
                {
                    [$"{componentCls}-tbody > tr{componentCls}-placeholder"] = new Unknown_90()
                    {
                        TextAlign = "center",
                        Color = token.ColorTextDisabled,
                        ["&:hover>th,&:hover>td,"] = new Unknown_91()
                        {
                            Background = token.ColorBgContainer,
                        },
                    },
                },
            };
        }

        public Unknown_7 GenExpandStyle(Unknown_92 token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var checkboxSize = token.ControlInteractiveSize;
            var motionDurationSlow = token.MotionDurationSlow;
            var lineWidth = token.LineWidth;
            var paddingXS = token.PaddingXS;
            var lineType = token.LineType;
            var tableBorderColor = token.TableBorderColor;
            var tableExpandIconBg = token.TableExpandIconBg;
            var tableExpandColumnWidth = token.TableExpandColumnWidth;
            var borderRadius = token.BorderRadius;
            var fontSize = token.FontSize;
            var fontSizeSM = token.FontSizeSM;
            var lineHeight = token.LineHeight;
            var tablePaddingVertical = token.TablePaddingVertical;
            var tablePaddingHorizontal = token.TablePaddingHorizontal;
            var tableExpandedRowBg = token.TableExpandedRowBg;
            var paddingXXS = token.PaddingXXS;
            var halfInnerSize = checkboxSize / 2 - lineWidth;
            var expandIconSize = halfInnerSize * 2 + lineWidth * 3;
            var tableBorder = @$"{lineWidth}px {lineType} {tableBorderColor}";
            var expandIconLineOffset = paddingXXS - lineWidth;
            return new Unknown_93()
            {
                [$"{componentCls}-wrapper"] = new Unknown_94()
                {
                    [$"{componentCls}-expand-icon-col"] = new Unknown_95()
                    {
                        Width = tableExpandColumnWidth,
                    },
                    [$"{componentCls}-row-expand-icon-cell"] = new Unknown_96()
                    {
                        TextAlign = "center",
                        [$"{componentCls}-row-expand-icon"] = new Unknown_97()
                        {
                            Display = "inline-flex",
                            Float = "none",
                            VerticalAlign = "sub",
                        },
                    },
                    [$"{componentCls}-row-indent"] = new Unknown_98()
                    {
                        Height = 1,
                        Float = "left",
                    },
                    [$"{componentCls}-row-expand-icon"] = new Unknown_99()
                    {
                        ["..."] = OperationUnit(token),
                        Position = "relative",
                        Float = "left",
                        BoxSizing = "border-box",
                        Width = expandIconSize,
                        Height = expandIconSize,
                        Padding = 0,
                        Color = "inherit",
                        LineHeight = @$"{expandIconSize}px",
                        Background = tableExpandIconBg,
                        Border = tableBorder,
                        BorderRadius = borderRadius,
                        Transform = @$"scale({checkboxSize / expandIconSize})",
                        Transition = @$"all {motionDurationSlow}",
                        UserSelect = "none",
                        ["&:focus, &:hover, &:active"] = new Unknown_100()
                        {
                            BorderColor = "currentcolor",
                        },
                        ["&::before, &::after"] = new Unknown_101()
                        {
                            Position = "absolute",
                            Background = "currentcolor",
                            Transition = @$"transform {motionDurationSlow} ease-out",
                            Content = "\"\"",
                        },
                        ["&::before"] = new Unknown_102()
                        {
                            Top = halfInnerSize,
                            InsetInlineEnd = expandIconLineOffset,
                            InsetInlineStart = expandIconLineOffset,
                            Height = lineWidth,
                        },
                        ["&::after"] = new Unknown_103()
                        {
                            Top = expandIconLineOffset,
                            Bottom = expandIconLineOffset,
                            InsetInlineStart = halfInnerSize,
                            Width = lineWidth,
                            Transform = "rotate(90deg)",
                        },
                        ["&-collapsed::before"] = new Unknown_104()
                        {
                            Transform = "rotate(-180deg)",
                        },
                        ["&-collapsed::after"] = new Unknown_105()
                        {
                            Transform = "rotate(0deg)",
                        },
                        ["&-spaced"] = new Unknown_106()
                        {
                            ["&::before, &::after"] = new Unknown_107()
                            {
                                Display = "none",
                                Content = "none",
                            },
                            Background = "transparent",
                            Border = 0,
                            Visibility = "hidden",
                        },
                    },
                    [$"{componentCls}-row-indent + {componentCls}-row-expand-icon"] = new Unknown_108()
                    {
                        MarginTop = (fontSize * lineHeight - lineWidth * 3) / 2 -
          Math.Ceil((fontSizeSM * 1.4 - lineWidth * 3) / 2),
                        MarginInlineEnd = paddingXS,
                    },
                    [$"tr{componentCls}-expanded-row"] = new Unknown_109()
                    {
                        ["&, &:hover"] = new Unknown_110()
                        {
                            ["> th, > td"] = new Unknown_111()
                            {
                                Background = tableExpandedRowBg,
                            },
                        },
                        [$"{antCls}-descriptions-view"] = new Unknown_112()
                        {
                            Display = "flex",
                            ["table"] = new Unknown_113()
                            {
                                Flex = "auto",
                                Width = "auto",
                            },
                        },
                    },
                    [$"{componentCls}-expanded-row-fixed"] = new Unknown_114()
                    {
                        Position = "relative",
                        Margin = @$"-{tablePaddingVertical}px -{tablePaddingHorizontal}px",
                        Padding = @$"{tablePaddingVertical}px {tablePaddingHorizontal}px",
                    },
                },
            };
        }

        public Unknown_8 GenFilterStyle(Unknown_115 token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var iconCls = token.IconCls;
            var tableFilterDropdownWidth = token.TableFilterDropdownWidth;
            var tableFilterDropdownSearchWidth = token.TableFilterDropdownSearchWidth;
            var paddingXXS = token.PaddingXXS;
            var paddingXS = token.PaddingXS;
            var colorText = token.ColorText;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var tableBorderColor = token.TableBorderColor;
            var tableHeaderIconColor = token.TableHeaderIconColor;
            var fontSizeSM = token.FontSizeSM;
            var tablePaddingHorizontal = token.TablePaddingHorizontal;
            var borderRadius = token.BorderRadius;
            var motionDurationSlow = token.MotionDurationSlow;
            var colorTextDescription = token.ColorTextDescription;
            var colorPrimary = token.ColorPrimary;
            var tableHeaderFilterActiveBg = token.TableHeaderFilterActiveBg;
            var colorTextDisabled = token.ColorTextDisabled;
            var tableFilterDropdownBg = token.TableFilterDropdownBg;
            var tableFilterDropdownHeight = token.TableFilterDropdownHeight;
            var controlItemBgHover = token.ControlItemBgHover;
            var controlItemBgActive = token.ControlItemBgActive;
            var boxShadowSecondary = token.BoxShadowSecondary;
            var dropdownPrefixCls = @$"{antCls}-dropdown";
            var tableFilterDropdownPrefixCls = @$"{componentCls}-filter-dropdown";
            var treePrefixCls = @$"{antCls}-tree";
            var tableBorder = @$"{lineWidth}px {lineType} {tableBorderColor}";
            return new Unknown_116
            {
                new Unknown_117()
                {
                    [$"{componentCls}-wrapper"] = new Unknown_118()
                    {
                        [$"{componentCls}-filter-column"] = new Unknown_119()
                        {
                            Display = "flex",
                            JustifyContent = "space-between",
                        },
                        [$"{componentCls}-filter-trigger"] = new Unknown_120()
                        {
                            Position = "relative",
                            Display = "flex",
                            AlignItems = "center",
                            MarginBlock = -paddingXXS,
                            MarginInline = @$"{paddingXXS}px {-tablePaddingHorizontal / 2}px",
                            Padding = @$"0 {paddingXXS}px",
                            Color = tableHeaderIconColor,
                            FontSize = fontSizeSM,
                            BorderRadius = borderRadius,
                            Cursor = "pointer",
                            Transition = @$"all {motionDurationSlow}",
                            ["&:hover"] = new Unknown_121()
                            {
                                Color = colorTextDescription,
                                Background = tableHeaderFilterActiveBg,
                            },
                            ["&.active"] = new Unknown_122()
                            {
                                Color = colorPrimary,
                            },
                        },
                    },
                },
                new Unknown_123()
                {
                    [$"{antCls}-dropdown"] = new Unknown_124()
                    {
                        [tableFilterDropdownPrefixCls] = new Unknown_125()
                        {
                            ["..."] = ResetComponent(token),
                            MinWidth = tableFilterDropdownWidth,
                            BackgroundColor = tableFilterDropdownBg,
                            BorderRadius = borderRadius,
                            BoxShadow = boxShadowSecondary,
                            [$"{dropdownPrefixCls}-menu"] = new Unknown_126()
                            {
                                MaxHeight = tableFilterDropdownHeight,
                                OverflowX = "hidden",
                                Border = 0,
                                BoxShadow = "none",
                                ["&:empty::after"] = new Unknown_127()
                                {
                                    Display = "block",
                                    Padding = @$"{paddingXS}px 0",
                                    Color = colorTextDisabled,
                                    FontSize = fontSizeSM,
                                    TextAlign = "center",
                                    Content = "\"Not Found\"",
                                },
                            },
                            [$"{tableFilterDropdownPrefixCls}-tree"] = new Unknown_128()
                            {
                                PaddingBlock = @$"{paddingXS}px 0",
                                PaddingInline = paddingXS,
                                [treePrefixCls] = new Unknown_129()
                                {
                                    Padding = 0,
                                },
                                [$"{treePrefixCls}-treenode {treePrefixCls}-node-content-wrapper:hover"] = new Unknown_130()
                                {
                                    BackgroundColor = controlItemBgHover,
                                },
                                [$"{treePrefixCls}-treenode-checkbox-checked {treePrefixCls}-node-content-wrapper"] = new Unknown_131()
                                {
                                    ["&, &:hover"] = new Unknown_132()
                                    {
                                        BackgroundColor = controlItemBgActive,
                                    },
                                },
                            },
                            [$"{tableFilterDropdownPrefixCls}-search"] = new Unknown_133()
                            {
                                Padding = paddingXS,
                                BorderBottom = tableBorder,
                                ["&-input"] = new Unknown_134()
                                {
                                    Input = new Unknown_135()
                                    {
                                        MinWidth = tableFilterDropdownSearchWidth,
                                    },
                                    [iconCls] = new Unknown_136()
                                    {
                                        Color = colorTextDisabled,
                                    },
                                },
                            },
                            [$"{tableFilterDropdownPrefixCls}-checkall"] = new Unknown_137()
                            {
                                Width = "100%",
                                MarginBottom = paddingXXS,
                                MarginInlineStart = paddingXXS,
                            },
                            [$"{tableFilterDropdownPrefixCls}-btns"] = new Unknown_138()
                            {
                                Display = "flex",
                                JustifyContent = "space-between",
                                Padding = @$"{paddingXS - lineWidth}px {paddingXS}px",
                                Overflow = "hidden",
                                BorderTop = tableBorder,
                            },
                        },
                    },
                },
                new Unknown_139()
                {
                    [$"{antCls}-dropdown {tableFilterDropdownPrefixCls}, {tableFilterDropdownPrefixCls}-submenu"] = new Unknown_140()
                    {
                        [$"{antCls}-checkbox-wrapper + span"] = new Unknown_141()
                        {
                            PaddingInlineStart = paddingXS,
                            Color = colorText,
                        },
                        ["> ul"] = new Unknown_142()
                        {
                            MaxHeight = "calc(100vh - 130px)",
                            OverflowX = "hidden",
                            OverflowY = "auto",
                        },
                    },
                },
            };
        }

        public Unknown_9 GenFixedStyle(Unknown_143 token)
        {
            var componentCls = token.ComponentCls;
            var lineWidth = token.LineWidth;
            var colorSplit = token.ColorSplit;
            var motionDurationSlow = token.MotionDurationSlow;
            var zIndexTableFixed = token.ZIndexTableFixed;
            var tableBg = token.TableBg;
            var zIndexTableSticky = token.ZIndexTableSticky;
            var shadowColor = colorSplit;
            return new Unknown_144()
            {
                [$"{componentCls}-wrapper"] = new Unknown_145()
                {
                    [$"{componentCls}-cell-fix-left,{componentCls}-cell-fix-right"] = new Unknown_146()
                    {
                        Position = "sticky !important" as "sticky",
                        ZIndex = zIndexTableFixed,
                        Background = tableBg,
                    },
                    [$"{componentCls}-cell-fix-left-first::after,{componentCls}-cell-fix-left-last::after"] = new Unknown_147()
                    {
                        Position = "absolute",
                        Top = 0,
                        Right = new Unknown_148()
                        {
                            SkipCheck = true,
                            Value = 0,
                        },
                        Bottom = -lineWidth,
                        Width = 30,
                        Transform = "translateX(100%)",
                        Transition = @$"box-shadow {motionDurationSlow}",
                        Content = "\"\"",
                        PointerEvents = "none",
                    },
                    [$"{componentCls}-cell-fix-left-all::after"] = new Unknown_149()
                    {
                        Display = "none",
                    },
                    [$"{componentCls}-cell-fix-right-first::after,{componentCls}-cell-fix-right-last::after"] = new Unknown_150()
                    {
                        Position = "absolute",
                        Top = 0,
                        Bottom = -lineWidth,
                        Left = new Unknown_151()
                        {
                            SkipCheck = true,
                            Value = 0,
                        },
                        Width = 30,
                        Transform = "translateX(-100%)",
                        Transition = @$"box-shadow {motionDurationSlow}",
                        Content = "\"\"",
                        PointerEvents = "none",
                    },
                    [$"{componentCls}-container"] = new Unknown_152()
                    {
                        ["&::before, &::after"] = new Unknown_153()
                        {
                            Position = "absolute",
                            Top = 0,
                            Bottom = 0,
                            ZIndex = zIndexTableSticky + 1,
                            Width = 30,
                            Transition = @$"box-shadow {motionDurationSlow}",
                            Content = "\"\"",
                            PointerEvents = "none",
                        },
                        ["&::before"] = new Unknown_154()
                        {
                            InsetInlineStart = 0,
                        },
                        ["&::after"] = new Unknown_155()
                        {
                            InsetInlineEnd = 0,
                        },
                    },
                    [$"{componentCls}-ping-left"] = new Unknown_156()
                    {
                        [$"&:not({componentCls}-has-fix-left) {componentCls}-container"] = new Unknown_157()
                        {
                            Position = "relative",
                            ["&::before"] = new Unknown_158()
                            {
                                BoxShadow = @$"inset 10px 0 8px -8px {shadowColor}",
                            },
                        },
                        [$"{componentCls}-cell-fix-left-first::after,{componentCls}-cell-fix-left-last::after"] = new Unknown_159()
                        {
                            BoxShadow = @$"inset 10px 0 8px -8px {shadowColor}",
                        },
                        [$"{componentCls}-cell-fix-left-last::before"] = new Unknown_160()
                        {
                            BackgroundColor = "transparent !important",
                        },
                    },
                    [$"{componentCls}-ping-right"] = new Unknown_161()
                    {
                        [$"&:not({componentCls}-has-fix-right) {componentCls}-container"] = new Unknown_162()
                        {
                            Position = "relative",
                            ["&::after"] = new Unknown_163()
                            {
                                BoxShadow = @$"inset -10px 0 8px -8px {shadowColor}",
                            },
                        },
                        [$"{componentCls}-cell-fix-right-first::after,{componentCls}-cell-fix-right-last::after"] = new Unknown_164()
                        {
                            BoxShadow = @$"inset -10px 0 8px -8px {shadowColor}",
                        },
                    },
                },
            };
        }

        public Unknown_10 GenPaginationStyle(Unknown_165 token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            return new Unknown_166()
            {
                [$"{componentCls}-wrapper"] = new Unknown_167()
                {
                    [$"{componentCls}-pagination{antCls}-pagination"] = new Unknown_168()
                    {
                        Margin = @$"{token.Margin}px 0",
                    },
                    [$"{componentCls}-pagination"] = new Unknown_169()
                    {
                        Display = "flex",
                        FlexWrap = "wrap",
                        RowGap = token.PaddingXS,
                        ["> *"] = new Unknown_170()
                        {
                            Flex = "none",
                        },
                        ["&-left"] = new Unknown_171()
                        {
                            JustifyContent = "flex-start",
                        },
                        ["&-center"] = new Unknown_172()
                        {
                            JustifyContent = "center",
                        },
                        ["&-right"] = new Unknown_173()
                        {
                            JustifyContent = "flex-end",
                        },
                    },
                },
            };
        }

        public Unknown_11 GenRadiusStyle(Unknown_174 token)
        {
            var componentCls = token.ComponentCls;
            var tableRadius = token.TableRadius;
            return new Unknown_175()
            {
                [$"{componentCls}-wrapper"] = new Unknown_176()
                {
                    [componentCls] = new Unknown_177()
                    {
                        [$"{componentCls}-title, {componentCls}-header"] = new Unknown_178()
                        {
                            BorderRadius = @$"{tableRadius}px {tableRadius}px 0 0",
                        },
                        [$"{componentCls}-title + {componentCls}-container"] = new Unknown_179()
                        {
                            BorderStartStartRadius = 0,
                            BorderStartEndRadius = 0,
                            [$"{componentCls}-header, table"] = new Unknown_180()
                            {
                                BorderRadius = 0,
                            },
                            ["table > thead > tr:first-child"] = new Unknown_181()
                            {
                                ["th:first-child, th:last-child, td:first-child, td:last-child"] = new Unknown_182()
                                {
                                    BorderRadius = 0,
                                },
                            },
                        },
                        ["&-container"] = new Unknown_183()
                        {
                            BorderStartStartRadius = tableRadius,
                            BorderStartEndRadius = tableRadius,
                            ["table > thead > tr:first-child"] = new Unknown_184()
                            {
                                ["> *:first-child"] = new Unknown_185()
                                {
                                    BorderStartStartRadius = tableRadius,
                                },
                                ["> *:last-child"] = new Unknown_186()
                                {
                                    BorderStartEndRadius = tableRadius,
                                },
                            },
                        },
                        ["&-footer"] = new Unknown_187()
                        {
                            BorderRadius = @$"0 0 {tableRadius}px {tableRadius}px",
                        },
                    },
                },
            };
        }

        public Unknown_12 GenStyle(Unknown_188 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_189()
            {
                [$"{componentCls}-wrapper-rtl"] = new Unknown_190()
                {
                    Direction = "rtl",
                    ["table"] = new Unknown_191()
                    {
                        Direction = "rtl",
                    },
                    [$"{componentCls}-pagination-left"] = new Unknown_192()
                    {
                        JustifyContent = "flex-end",
                    },
                    [$"{componentCls}-pagination-right"] = new Unknown_193()
                    {
                        JustifyContent = "flex-start",
                    },
                    [$"{componentCls}-row-expand-icon"] = new Unknown_194()
                    {
                        ["&::after"] = new Unknown_195()
                        {
                            Transform = "rotate(-90deg)",
                        },
                        ["&-collapsed::before"] = new Unknown_196()
                        {
                            Transform = "rotate(180deg)",
                        },
                        ["&-collapsed::after"] = new Unknown_197()
                        {
                            Transform = "rotate(0deg)",
                        },
                    },
                    [$"{componentCls}-container"] = new Unknown_198()
                    {
                        ["&::before"] = new Unknown_199()
                        {
                            InsetInlineStart = "unset",
                            InsetInlineEnd = 0,
                        },
                        ["&::after"] = new Unknown_200()
                        {
                            InsetInlineStart = 0,
                            InsetInlineEnd = "unset",
                        },
                    },
                },
            };
        }

        public Unknown_13 GenSelectionStyle(Unknown_201 token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var iconCls = token.IconCls;
            var fontSizeIcon = token.FontSizeIcon;
            var padding = token.Padding;
            var paddingXS = token.PaddingXS;
            var tableHeaderIconColor = token.TableHeaderIconColor;
            var tableHeaderIconColorHover = token.TableHeaderIconColorHover;
            var tableSelectionColumnWidth = token.TableSelectionColumnWidth;
            return new Unknown_202()
            {
                [$"{componentCls}-wrapper"] = new Unknown_203()
                {
                    [$"{componentCls}-selection-col"] = new Unknown_204()
                    {
                        Width = tableSelectionColumnWidth,
                        [$"&{componentCls}-selection-col-with-dropdown"] = new Unknown_205()
                        {
                            Width = tableSelectionColumnWidth + fontSizeIcon + padding / 4,
                        },
                    },
                    [$"{componentCls}-bordered {componentCls}-selection-col"] = new Unknown_206()
                    {
                        Width = tableSelectionColumnWidth + paddingXS * 2,
                        [$"&{componentCls}-selection-col-with-dropdown"] = new Unknown_207()
                        {
                            Width = tableSelectionColumnWidth + fontSizeIcon + padding / 4 + paddingXS * 2,
                        },
                    },
                    [$"tabletrth{componentCls}-selection-column,tabletrtd{componentCls}-selection-column"] = new Unknown_208()
                    {
                        PaddingInlineEnd = token.PaddingXS,
                        PaddingInlineStart = token.PaddingXS,
                        TextAlign = "center",
                        [$"{antCls}-radio-wrapper"] = new Unknown_209()
                        {
                            MarginInlineEnd = 0,
                        },
                    },
                    [$"table tr th{componentCls}-selection-column{componentCls}-cell-fix-left"] = new Unknown_210()
                    {
                        ZIndex = token.ZIndexTableFixed + 1,
                    },
                    [$"table tr th{componentCls}-selection-column::after"] = new Unknown_211()
                    {
                        BackgroundColor = "transparent !important",
                    },
                    [$"{componentCls}-selection"] = new Unknown_212()
                    {
                        Position = "relative",
                        Display = "inline-flex",
                        FlexDirection = "column",
                    },
                    [$"{componentCls}-selection-extra"] = new Unknown_213()
                    {
                        Position = "absolute",
                        Top = 0,
                        ZIndex = 1,
                        Cursor = "pointer",
                        Transition = @$"all {token.MotionDurationSlow}",
                        MarginInlineStart = "100%",
                        PaddingInlineStart = @$"{token.TablePaddingHorizontal / 4}px",
                        [iconCls] = new Unknown_214()
                        {
                            Color = tableHeaderIconColor,
                            FontSize = fontSizeIcon,
                            VerticalAlign = "baseline",
                            ["&:hover"] = new Unknown_215()
                            {
                                Color = tableHeaderIconColorHover,
                            },
                        },
                    },
                },
            };
        }

        public Unknown_15 GenSizeStyle(Unknown_216 token)
        {
            var componentCls = token.ComponentCls;
            var getSizeStyle = ('small' | 'middle' size, int paddingVertical, int paddingHorizontal, int fontSize) => {
                return new Unknown_217()
                {
                    [$"{componentCls}{componentCls}-{size}"] = new Unknown_218()
                    {
                        FontSize = fontSize,
                        [$"{componentCls}-title,{componentCls}-footer,{componentCls}-thead>tr>th,{componentCls}-tbody>tr>th,{componentCls}-tbody>tr>td,tfoot>tr>th,tfoot>tr>td"] = new Unknown_219()
                        {
                            Padding = @$"{paddingVertical}px {paddingHorizontal}px",
                        },
                        [$"{componentCls}-filter-trigger"] = new Unknown_220()
                        {
                            MarginInlineEnd = @$"-{paddingHorizontal / 2}px",
                        },
                        [$"{componentCls}-expanded-row-fixed"] = new Unknown_221()
                        {
                            Margin = @$"-{paddingVertical}px -{paddingHorizontal}px",
                        },
                        [$"{componentCls}-tbody"] = new Unknown_222()
                        {
                            [$"{componentCls}-wrapper:only-child {componentCls}"] = new Unknown_223()
                            {
                                MarginBlock = @$"-{paddingVertical}px",
                                MarginInline = @$"{
            token.TableExpandColumnWidth - paddingHorizontal
          }px -{paddingHorizontal}px",
                            },
                        },
                        [$"{componentCls}-selection-column"] = new Unknown_224()
                        {
                            PaddingInlineStart = @$"{paddingHorizontal / 4}px",
                        },
                    },
                };
            };
            return new Unknown_225()
            {
                [$"{componentCls}-wrapper"] = new Unknown_226()
                {
                    ["..."] = GetSizeStyle("middle", token.TablePaddingVerticalMiddle, token.TablePaddingHorizontalMiddle, token.TableFontSizeMiddle),
                    ["..."] = GetSizeStyle("small", token.TablePaddingVerticalSmall, token.TablePaddingHorizontalSmall, token.TableFontSizeSmall)
                },
            };
        }

        public Unknown_16 GenSorterStyle(Unknown_227 token)
        {
            var componentCls = token.ComponentCls;
            var marginXXS = token.MarginXXS;
            var fontSizeIcon = token.FontSizeIcon;
            var tableHeaderIconColor = token.TableHeaderIconColor;
            var tableHeaderIconColorHover = token.TableHeaderIconColorHover;
            return new Unknown_228()
            {
                [$"{componentCls}-wrapper"] = new Unknown_229()
                {
                    [$"{componentCls}-thead th{componentCls}-column-has-sorters"] = new Unknown_230()
                    {
                        Outline = "none",
                        Cursor = "pointer",
                        Transition = @$"all {token.MotionDurationSlow}",
                        ["&:hover"] = new Unknown_231()
                        {
                            Background = token.TableHeaderSortHoverBg,
                            ["&::before"] = new Unknown_232()
                            {
                                BackgroundColor = "transparent !important",
                            },
                        },
                        ["&:focus-visible"] = new Unknown_233()
                        {
                            Color = token.ColorPrimary,
                        },
                        [$"&{componentCls}-cell-fix-left:hover,&{componentCls}-cell-fix-right:hover"] = new Unknown_234()
                        {
                            Background = token.TableFixedHeaderSortActiveBg,
                        },
                    },
                    [$"{componentCls}-thead th{componentCls}-column-sort"] = new Unknown_235()
                    {
                        Background = token.TableHeaderSortBg,
                        ["&::before"] = new Unknown_236()
                        {
                            BackgroundColor = "transparent !important",
                        },
                    },
                    [$"td{componentCls}-column-sort"] = new Unknown_237()
                    {
                        Background = token.TableBodySortBg,
                    },
                    [$"{componentCls}-column-title"] = new Unknown_238()
                    {
                        Position = "relative",
                        ZIndex = 1,
                        Flex = 1,
                    },
                    [$"{componentCls}-column-sorters"] = new Unknown_239()
                    {
                        Display = "flex",
                        Flex = "auto",
                        AlignItems = "center",
                        JustifyContent = "space-between",
                        ["&::after"] = new Unknown_240()
                        {
                            Position = "absolute",
                            Inset = 0,
                            Width = "100%",
                            Height = "100%",
                            Content = "\"\"",
                        },
                    },
                    [$"{componentCls}-column-sorter"] = new Unknown_241()
                    {
                        MarginInlineStart = marginXXS,
                        Color = tableHeaderIconColor,
                        FontSize = 0,
                        Transition = @$"color {token.MotionDurationSlow}",
                        ["&-inner"] = new Unknown_242()
                        {
                            Display = "inline-flex",
                            FlexDirection = "column",
                            AlignItems = "center",
                        },
                        ["&-up, &-down"] = new Unknown_243()
                        {
                            FontSize = fontSizeIcon,
                            ["&.active"] = new Unknown_244()
                            {
                                Color = token.ColorPrimary,
                            },
                        },
                        [$"{componentCls}-column-sorter-up + {componentCls}-column-sorter-down"] = new Unknown_245()
                        {
                            MarginTop = "-0.3em",
                        },
                    },
                    [$"{componentCls}-column-sorters:hover {componentCls}-column-sorter"] = new Unknown_246()
                    {
                        Color = tableHeaderIconColorHover,
                    },
                },
            };
        }

        public Unknown_17 GenStickyStyle(Unknown_247 token)
        {
            var componentCls = token.ComponentCls;
            var opacityLoading = token.OpacityLoading;
            var tableScrollThumbBg = token.TableScrollThumbBg;
            var tableScrollThumbBgHover = token.TableScrollThumbBgHover;
            var tableScrollThumbSize = token.TableScrollThumbSize;
            var tableScrollBg = token.TableScrollBg;
            var zIndexTableSticky = token.ZIndexTableSticky;
            var tableBorder = @$"{token.LineWidth}px {token.LineType} {token.TableBorderColor}";
            return new Unknown_248()
            {
                [$"{componentCls}-wrapper"] = new Unknown_249()
                {
                    [$"{componentCls}-sticky"] = new Unknown_250()
                    {
                        ["&-holder"] = new Unknown_251()
                        {
                            Position = "sticky",
                            ZIndex = zIndexTableSticky,
                            Background = token.ColorBgContainer,
                        },
                        ["&-scroll"] = new Unknown_252()
                        {
                            Position = "sticky",
                            Bottom = 0,
                            Height = @$"{tableScrollThumbSize}px !important",
                            ZIndex = zIndexTableSticky,
                            Display = "flex",
                            AlignItems = "center",
                            Background = tableScrollBg,
                            BorderTop = tableBorder,
                            Opacity = opacityLoading,
                            ["&:hover"] = new Unknown_253()
                            {
                                TransformOrigin = "center bottom",
                            },
                            ["&-bar"] = new Unknown_254()
                            {
                                Height = tableScrollThumbSize,
                                BackgroundColor = tableScrollThumbBg,
                                BorderRadius = 100,
                                Transition = @$"all {token.MotionDurationSlow}, transform none",
                                Position = "absolute",
                                Bottom = 0,
                                ["&:hover, &-active"] = new Unknown_255()
                                {
                                    BackgroundColor = tableScrollThumbBgHover,
                                },
                            },
                        },
                    },
                },
            };
        }

        public Unknown_18 GenSummaryStyle(Unknown_256 token)
        {
            var componentCls = token.ComponentCls;
            var lineWidth = token.LineWidth;
            var tableBorderColor = token.TableBorderColor;
            var tableBorder = @$"{lineWidth}px {token.LineType} {tableBorderColor}";
            return new Unknown_257()
            {
                [$"{componentCls}-wrapper"] = new Unknown_258()
                {
                    [$"{componentCls}-summary"] = new Unknown_259()
                    {
                        Position = "relative",
                        ZIndex = token.ZIndexTableFixed,
                        Background = token.TableBg,
                        ["> tr"] = new Unknown_260()
                        {
                            ["> th, > td"] = new Unknown_261()
                            {
                                BorderBottom = tableBorder,
                            },
                        },
                    },
                    [$"div{componentCls}-summary"] = new Unknown_262()
                    {
                        BoxShadow = @$"0 -{lineWidth}px 0 {tableBorderColor}",
                    },
                },
            };
        }

    }

}