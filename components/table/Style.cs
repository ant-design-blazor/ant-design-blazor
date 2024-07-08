using System;
using CssInCSharp;
using CssInCSharp.Colors;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class TableToken
    {
        public string HeaderBg
        {
            get => (string)_tokens["headerBg"];
            set => _tokens["headerBg"] = value;
        }

        public string HeaderColor
        {
            get => (string)_tokens["headerColor"];
            set => _tokens["headerColor"] = value;
        }

        public string HeaderSortActiveBg
        {
            get => (string)_tokens["headerSortActiveBg"];
            set => _tokens["headerSortActiveBg"] = value;
        }

        public string HeaderSortHoverBg
        {
            get => (string)_tokens["headerSortHoverBg"];
            set => _tokens["headerSortHoverBg"] = value;
        }

        public string BodySortBg
        {
            get => (string)_tokens["bodySortBg"];
            set => _tokens["bodySortBg"] = value;
        }

        public string RowHoverBg
        {
            get => (string)_tokens["rowHoverBg"];
            set => _tokens["rowHoverBg"] = value;
        }

        public string RowSelectedBg
        {
            get => (string)_tokens["rowSelectedBg"];
            set => _tokens["rowSelectedBg"] = value;
        }

        public string RowSelectedHoverBg
        {
            get => (string)_tokens["rowSelectedHoverBg"];
            set => _tokens["rowSelectedHoverBg"] = value;
        }

        public string RowExpandedBg
        {
            get => (string)_tokens["rowExpandedBg"];
            set => _tokens["rowExpandedBg"] = value;
        }

        public double CellPaddingBlock
        {
            get => (double)_tokens["cellPaddingBlock"];
            set => _tokens["cellPaddingBlock"] = value;
        }

        public double CellPaddingInline
        {
            get => (double)_tokens["cellPaddingInline"];
            set => _tokens["cellPaddingInline"] = value;
        }

        public double CellPaddingBlockMD
        {
            get => (double)_tokens["cellPaddingBlockMD"];
            set => _tokens["cellPaddingBlockMD"] = value;
        }

        public double CellPaddingInlineMD
        {
            get => (double)_tokens["cellPaddingInlineMD"];
            set => _tokens["cellPaddingInlineMD"] = value;
        }

        public double CellPaddingBlockSM
        {
            get => (double)_tokens["cellPaddingBlockSM"];
            set => _tokens["cellPaddingBlockSM"] = value;
        }

        public double CellPaddingInlineSM
        {
            get => (double)_tokens["cellPaddingInlineSM"];
            set => _tokens["cellPaddingInlineSM"] = value;
        }

        public string BorderColor
        {
            get => (string)_tokens["borderColor"];
            set => _tokens["borderColor"] = value;
        }

        public double HeaderBorderRadius
        {
            get => (double)_tokens["headerBorderRadius"];
            set => _tokens["headerBorderRadius"] = value;
        }

        public string FooterBg
        {
            get => (string)_tokens["footerBg"];
            set => _tokens["footerBg"] = value;
        }

        public string FooterColor
        {
            get => (string)_tokens["footerColor"];
            set => _tokens["footerColor"] = value;
        }

        public double CellFontSize
        {
            get => (double)_tokens["cellFontSize"];
            set => _tokens["cellFontSize"] = value;
        }

        public double CellFontSizeMD
        {
            get => (double)_tokens["cellFontSizeMD"];
            set => _tokens["cellFontSizeMD"] = value;
        }

        public double CellFontSizeSM
        {
            get => (double)_tokens["cellFontSizeSM"];
            set => _tokens["cellFontSizeSM"] = value;
        }

        public string HeaderSplitColor
        {
            get => (string)_tokens["headerSplitColor"];
            set => _tokens["headerSplitColor"] = value;
        }

        public string FixedHeaderSortActiveBg
        {
            get => (string)_tokens["fixedHeaderSortActiveBg"];
            set => _tokens["fixedHeaderSortActiveBg"] = value;
        }

        public string HeaderFilterHoverBg
        {
            get => (string)_tokens["headerFilterHoverBg"];
            set => _tokens["headerFilterHoverBg"] = value;
        }

        public string FilterDropdownMenuBg
        {
            get => (string)_tokens["filterDropdownMenuBg"];
            set => _tokens["filterDropdownMenuBg"] = value;
        }

        public string FilterDropdownBg
        {
            get => (string)_tokens["filterDropdownBg"];
            set => _tokens["filterDropdownBg"] = value;
        }

        public string ExpandIconBg
        {
            get => (string)_tokens["expandIconBg"];
            set => _tokens["expandIconBg"] = value;
        }

        public double SelectionColumnWidth
        {
            get => (double)_tokens["selectionColumnWidth"];
            set => _tokens["selectionColumnWidth"] = value;
        }

        public string StickyScrollBarBg
        {
            get => (string)_tokens["stickyScrollBarBg"];
            set => _tokens["stickyScrollBarBg"] = value;
        }

        public double StickyScrollBarBorderRadius
        {
            get => (double)_tokens["stickyScrollBarBorderRadius"];
            set => _tokens["stickyScrollBarBorderRadius"] = value;
        }

    }

    public partial class TableToken : TokenWithCommonCls
    {
        public double TableFontSize
        {
            get => (double)_tokens["tableFontSize"];
            set => _tokens["tableFontSize"] = value;
        }

        public string TableBg
        {
            get => (string)_tokens["tableBg"];
            set => _tokens["tableBg"] = value;
        }

        public double TableRadius
        {
            get => (double)_tokens["tableRadius"];
            set => _tokens["tableRadius"] = value;
        }

        public double TablePaddingHorizontal
        {
            get => (double)_tokens["tablePaddingHorizontal"];
            set => _tokens["tablePaddingHorizontal"] = value;
        }

        public double TablePaddingVertical
        {
            get => (double)_tokens["tablePaddingVertical"];
            set => _tokens["tablePaddingVertical"] = value;
        }

        public double TablePaddingHorizontalMiddle
        {
            get => (double)_tokens["tablePaddingHorizontalMiddle"];
            set => _tokens["tablePaddingHorizontalMiddle"] = value;
        }

        public double TablePaddingVerticalMiddle
        {
            get => (double)_tokens["tablePaddingVerticalMiddle"];
            set => _tokens["tablePaddingVerticalMiddle"] = value;
        }

        public double TablePaddingHorizontalSmall
        {
            get => (double)_tokens["tablePaddingHorizontalSmall"];
            set => _tokens["tablePaddingHorizontalSmall"] = value;
        }

        public double TablePaddingVerticalSmall
        {
            get => (double)_tokens["tablePaddingVerticalSmall"];
            set => _tokens["tablePaddingVerticalSmall"] = value;
        }

        public string TableBorderColor
        {
            get => (string)_tokens["tableBorderColor"];
            set => _tokens["tableBorderColor"] = value;
        }

        public string TableHeaderTextColor
        {
            get => (string)_tokens["tableHeaderTextColor"];
            set => _tokens["tableHeaderTextColor"] = value;
        }

        public string TableHeaderBg
        {
            get => (string)_tokens["tableHeaderBg"];
            set => _tokens["tableHeaderBg"] = value;
        }

        public string TableFooterTextColor
        {
            get => (string)_tokens["tableFooterTextColor"];
            set => _tokens["tableFooterTextColor"] = value;
        }

        public string TableFooterBg
        {
            get => (string)_tokens["tableFooterBg"];
            set => _tokens["tableFooterBg"] = value;
        }

        public string TableHeaderCellSplitColor
        {
            get => (string)_tokens["tableHeaderCellSplitColor"];
            set => _tokens["tableHeaderCellSplitColor"] = value;
        }

        public string TableHeaderSortBg
        {
            get => (string)_tokens["tableHeaderSortBg"];
            set => _tokens["tableHeaderSortBg"] = value;
        }

        public string TableHeaderSortHoverBg
        {
            get => (string)_tokens["tableHeaderSortHoverBg"];
            set => _tokens["tableHeaderSortHoverBg"] = value;
        }

        public string TableHeaderIconColor
        {
            get => (string)_tokens["tableHeaderIconColor"];
            set => _tokens["tableHeaderIconColor"] = value;
        }

        public string TableHeaderIconColorHover
        {
            get => (string)_tokens["tableHeaderIconColorHover"];
            set => _tokens["tableHeaderIconColorHover"] = value;
        }

        public string TableBodySortBg
        {
            get => (string)_tokens["tableBodySortBg"];
            set => _tokens["tableBodySortBg"] = value;
        }

        public string TableFixedHeaderSortActiveBg
        {
            get => (string)_tokens["tableFixedHeaderSortActiveBg"];
            set => _tokens["tableFixedHeaderSortActiveBg"] = value;
        }

        public string TableHeaderFilterActiveBg
        {
            get => (string)_tokens["tableHeaderFilterActiveBg"];
            set => _tokens["tableHeaderFilterActiveBg"] = value;
        }

        public string TableFilterDropdownBg
        {
            get => (string)_tokens["tableFilterDropdownBg"];
            set => _tokens["tableFilterDropdownBg"] = value;
        }

        public double TableFilterDropdownHeight
        {
            get => (double)_tokens["tableFilterDropdownHeight"];
            set => _tokens["tableFilterDropdownHeight"] = value;
        }

        public string TableRowHoverBg
        {
            get => (string)_tokens["tableRowHoverBg"];
            set => _tokens["tableRowHoverBg"] = value;
        }

        public string TableSelectedRowBg
        {
            get => (string)_tokens["tableSelectedRowBg"];
            set => _tokens["tableSelectedRowBg"] = value;
        }

        public string TableSelectedRowHoverBg
        {
            get => (string)_tokens["tableSelectedRowHoverBg"];
            set => _tokens["tableSelectedRowHoverBg"] = value;
        }

        public double TableFontSizeMiddle
        {
            get => (double)_tokens["tableFontSizeMiddle"];
            set => _tokens["tableFontSizeMiddle"] = value;
        }

        public double TableFontSizeSmall
        {
            get => (double)_tokens["tableFontSizeSmall"];
            set => _tokens["tableFontSizeSmall"] = value;
        }

        public double TableSelectionColumnWidth
        {
            get => (double)_tokens["tableSelectionColumnWidth"];
            set => _tokens["tableSelectionColumnWidth"] = value;
        }

        public string TableExpandIconBg
        {
            get => (string)_tokens["tableExpandIconBg"];
            set => _tokens["tableExpandIconBg"] = value;
        }

        public double TableExpandColumnWidth
        {
            get => (double)_tokens["tableExpandColumnWidth"];
            set => _tokens["tableExpandColumnWidth"] = value;
        }

        public string TableExpandedRowBg
        {
            get => (string)_tokens["tableExpandedRowBg"];
            set => _tokens["tableExpandedRowBg"] = value;
        }

        public double TableFilterDropdownWidth
        {
            get => (double)_tokens["tableFilterDropdownWidth"];
            set => _tokens["tableFilterDropdownWidth"] = value;
        }

        public double TableFilterDropdownSearchWidth
        {
            get => (double)_tokens["tableFilterDropdownSearchWidth"];
            set => _tokens["tableFilterDropdownSearchWidth"] = value;
        }

        public double ZIndexTableFixed
        {
            get => (double)_tokens["zIndexTableFixed"];
            set => _tokens["zIndexTableFixed"] = value;
        }

        public double ZIndexTableSticky
        {
            get => (double)_tokens["zIndexTableSticky"];
            set => _tokens["zIndexTableSticky"] = value;
        }

        public double TableScrollThumbSize
        {
            get => (double)_tokens["tableScrollThumbSize"];
            set => _tokens["tableScrollThumbSize"] = value;
        }

        public string TableScrollThumbBg
        {
            get => (string)_tokens["tableScrollThumbBg"];
            set => _tokens["tableScrollThumbBg"] = value;
        }

        public string TableScrollThumbBgHover
        {
            get => (string)_tokens["tableScrollThumbBgHover"];
            set => _tokens["tableScrollThumbBgHover"] = value;
        }

        public string TableScrollBg
        {
            get => (string)_tokens["tableScrollBg"];
            set => _tokens["tableScrollBg"] = value;
        }

    }

    public class TableStyle
    {
        public CSSObject GenTableStyle(TableToken token)
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
            var tableFooterTextColor = token.TableFooterTextColor;
            var tableFooterBg = token.TableFooterBg;
            var tableBorder = @$"{lineWidth}px {lineType} {tableBorderColor}";
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    Clear = "both",
                    MaxWidth = "100%",
                    ["..."] = ClearFix(),
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        FontSize = tableFontSize,
                        Background = tableBg,
                        BorderRadius = @$"{tableRadius}px {tableRadius}px 0 0",
                    },
                    ["table"] = new CSSObject()
                    {
                        Width = "100%",
                        TextAlign = "start",
                        BorderRadius = @$"{tableRadius}px {tableRadius}px 0 0",
                        BorderCollapse = "separate",
                        BorderSpacing = 0,
                    },
                    [$"{componentCls}-cell,{componentCls}-thead>tr>th,{componentCls}-tbody>tr>th,{componentCls}-tbody>tr>td,tfoot>tr>th,tfoot>tr>td"] = new CSSObject()
                    {
                        Position = "relative",
                        Padding = @$"{tablePaddingVertical}px {tablePaddingHorizontal}px",
                        OverflowWrap = "break-word",
                    },
                    [$"{componentCls}-title"] = new CSSObject()
                    {
                        Padding = @$"{tablePaddingVertical}px {tablePaddingHorizontal}px",
                    },
                    [$"{componentCls}-thead"] = new CSSObject()
                    {
                        [">tr>th,>tr>td"] = new CSSObject()
                        {
                            Position = "relative",
                            Color = tableHeaderTextColor,
                            FontWeight = fontWeightStrong,
                            TextAlign = "start",
                            Background = tableHeaderBg,
                            BorderBottom = tableBorder,
                            Transition = @$"background {motionDurationMid} ease",
                            ["&[colspan]:not([colspan=\"1\"])"] = new CSSObject()
                            {
                                TextAlign = "center",
                            },
                            [$"&:not(:last-child):not({componentCls}-selection-column):not({componentCls}-row-expand-icon-cell):not([colspan])::before"] = new CSSObject()
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
                        ["> tr:not(:last-child) > th[colspan]"] = new CSSObject()
                        {
                            BorderBottom = 0,
                        },
                    },
                    [$"{componentCls}-tbody"] = new CSSObject()
                    {
                        ["> tr"] = new CSSObject()
                        {
                            ["> th, > td"] = new CSSObject()
                            {
                                Transition = @$"background {motionDurationMid}, border-color {motionDurationMid}",
                                BorderBottom = tableBorder,
                                [$">{componentCls}-wrapper:only-child,>{componentCls}-expanded-row-fixed>{componentCls}-wrapper:only-child"] = new CSSObject()
                                {
                                    [componentCls] = new CSSObject()
                                    {
                                        MarginBlock = @$"-{tablePaddingVertical}px",
                                        MarginInline = @$"{
                  token.TableExpandColumnWidth - tablePaddingHorizontal
                }px -{tablePaddingHorizontal}px",
                                        [$"{componentCls}-tbody > tr:last-child > td"] = new CSSObject()
                                        {
                                            BorderBottom = 0,
                                            ["&:first-child, &:last-child"] = new CSSObject()
                                            {
                                                BorderRadius = 0,
                                            },
                                        },
                                    },
                                },
                            },
                            ["> th"] = new CSSObject()
                            {
                                Position = "relative",
                                Color = tableHeaderTextColor,
                                FontWeight = fontWeightStrong,
                                TextAlign = "start",
                                Background = tableHeaderBg,
                                BorderBottom = tableBorder,
                                Transition = @$"background {motionDurationMid} ease",
                            },
                        },
                    },
                    [$"{componentCls}-footer"] = new CSSObject()
                    {
                        Padding = @$"{tablePaddingVertical}px {tablePaddingHorizontal}px",
                        Color = tableFooterTextColor,
                        Background = tableFooterBg,
                    },
                },
            };
        }

        public UseComponentStyleResult ExportDefault()
        {
            return GenComponentStyleHook(
                "Table",
                (token) =>
                {
                    var colorTextHeading = token.ColorTextHeading;
                    var colorSplit = token.ColorSplit;
                    var colorIcon = token.ColorIcon;
                    var colorIconHover = token.ColorIconHover;
                    var opacityLoading = token.OpacityLoading;
                    var colorBgContainer = token.ColorBgContainer;
                    var checkboxSize = token.ControlInteractiveSize;
                    var headerBg = token.HeaderBg;
                    var headerColor = token.HeaderColor;
                    var headerSortActiveBg = token.HeaderSortActiveBg;
                    var headerSortHoverBg = token.HeaderSortHoverBg;
                    var bodySortBg = token.BodySortBg;
                    var rowHoverBg = token.RowHoverBg;
                    var rowSelectedBg = token.RowSelectedBg;
                    var rowSelectedHoverBg = token.RowSelectedHoverBg;
                    var rowExpandedBg = token.RowExpandedBg;
                    var cellPaddingBlock = token.CellPaddingBlock;
                    var cellPaddingInline = token.CellPaddingInline;
                    var cellPaddingBlockMD = token.CellPaddingBlockMD;
                    var cellPaddingInlineMD = token.CellPaddingInlineMD;
                    var cellPaddingBlockSM = token.CellPaddingBlockSM;
                    var cellPaddingInlineSM = token.CellPaddingInlineSM;
                    var borderColor = token.BorderColor;
                    var footerBg = token.FooterBg;
                    var footerColor = token.FooterColor;
                    var headerBorderRadius = token.HeaderBorderRadius;
                    var cellFontSize = token.CellFontSize;
                    var cellFontSizeMD = token.CellFontSizeMD;
                    var cellFontSizeSM = token.CellFontSizeSM;
                    var headerSplitColor = token.HeaderSplitColor;
                    var fixedHeaderSortActiveBg = token.FixedHeaderSortActiveBg;
                    var headerFilterHoverBg = token.HeaderFilterHoverBg;
                    var filterDropdownBg = token.FilterDropdownBg;
                    var expandIconBg = token.ExpandIconBg;
                    var selectionColumnWidth = token.SelectionColumnWidth;
                    var stickyScrollBarBg = token.StickyScrollBarBg;
                    var baseColorAction = new TinyColor(colorIcon);
                    var baseColorActionHover = new TinyColor(colorIconHover);
                    var zIndexTableFixed = 2;
                    var tableToken = MergeToken(
                        token,
                        new TableToken()
                        {
                            TableFontSize = cellFontSize,
                            TableBg = colorBgContainer,
                            TableRadius = headerBorderRadius,
                            TablePaddingVertical = cellPaddingBlock,
                            TablePaddingHorizontal = cellPaddingInline,
                            TablePaddingVerticalMiddle = cellPaddingBlockMD,
                            TablePaddingHorizontalMiddle = cellPaddingInlineMD,
                            TablePaddingVerticalSmall = cellPaddingBlockSM,
                            TablePaddingHorizontalSmall = cellPaddingInlineSM,
                            TableBorderColor = borderColor,
                            TableHeaderTextColor = headerColor,
                            TableHeaderBg = headerBg,
                            TableFooterTextColor = footerColor,
                            TableFooterBg = footerBg,
                            TableHeaderCellSplitColor = headerSplitColor,
                            TableHeaderSortBg = headerSortActiveBg,
                            TableHeaderSortHoverBg = headerSortHoverBg,
                            TableHeaderIconColor = baseColorAction.Clone().SetAlpha(baseColorAction.GetAlpha() * opacityLoading).ToRgbString(),
                            TableHeaderIconColorHover = baseColorActionHover.Clone().SetAlpha(baseColorActionHover.GetAlpha() * opacityLoading).ToRgbString(),
                            TableBodySortBg = bodySortBg,
                            TableFixedHeaderSortActiveBg = fixedHeaderSortActiveBg,
                            TableHeaderFilterActiveBg = headerFilterHoverBg,
                            TableFilterDropdownBg = filterDropdownBg,
                            TableRowHoverBg = rowHoverBg,
                            TableSelectedRowBg = rowSelectedBg,
                            TableSelectedRowHoverBg = rowSelectedHoverBg,
                            ZIndexTableFixed = zIndexTableFixed,
                            ZIndexTableSticky = zIndexTableFixed + 1,
                            TableFontSizeMiddle = cellFontSizeMD,
                            TableFontSizeSmall = cellFontSizeSM,
                            TableSelectionColumnWidth = selectionColumnWidth,
                            TableExpandIconBg = expandIconBg,
                            TableExpandColumnWidth = checkboxSize + 2 * token.Padding,
                            TableExpandedRowBg = rowExpandedBg,
                            TableFilterDropdownWidth = 120,
                            TableFilterDropdownHeight = 264,
                            TableFilterDropdownSearchWidth = 140,
                            TableScrollThumbSize = 8,
                            TableScrollThumbBg = stickyScrollBarBg,
                            TableScrollThumbBgHover = colorTextHeading,
                            TableScrollBg = colorSplit,
                        });
                    return new CSSInterpolation[]
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
                        GenRtlStyle(tableToken),
                        GenVirtualStyle(tableToken),
                    };
                },
                (token) =>
                {
                    var colorFillAlter = token.ColorFillAlter;
                    var colorBgContainer = token.ColorBgContainer;
                    var colorTextHeading = token.ColorTextHeading;
                    var colorFillSecondary = token.ColorFillSecondary;
                    var colorFillContent = token.ColorFillContent;
                    var controlItemBgActive = token.ControlItemBgActive;
                    var controlItemBgActiveHover = token.ControlItemBgActiveHover;
                    var padding = token.Padding;
                    var paddingSM = token.PaddingSM;
                    var paddingXS = token.PaddingXS;
                    var colorBorderSecondary = token.ColorBorderSecondary;
                    var borderRadiusLG = token.BorderRadiusLG;
                    var fontSize = token.FontSize;
                    var controlHeight = token.ControlHeight;
                    var colorTextPlaceholder = token.ColorTextPlaceholder;
                    var colorFillSecondarySolid = new TinyColor(colorFillSecondary).OnBackground(colorBgContainer).ToHexShortString();
                    var colorFillContentSolid = new TinyColor(colorFillContent).OnBackground(colorBgContainer).ToHexShortString();
                    var colorFillAlterSolid = new TinyColor(colorFillAlter).OnBackground(colorBgContainer).ToHexShortString();
                    return new TableToken()
                    {
                        HeaderBg = colorFillAlterSolid,
                        HeaderColor = colorTextHeading,
                        HeaderSortActiveBg = colorFillSecondarySolid,
                        HeaderSortHoverBg = colorFillContentSolid,
                        BodySortBg = colorFillAlterSolid,
                        RowHoverBg = colorFillAlterSolid,
                        RowSelectedBg = controlItemBgActive,
                        RowSelectedHoverBg = controlItemBgActiveHover,
                        RowExpandedBg = colorFillAlter,
                        CellPaddingBlock = padding,
                        CellPaddingInline = padding,
                        CellPaddingBlockMD = paddingSM,
                        CellPaddingInlineMD = paddingXS,
                        CellPaddingBlockSM = paddingXS,
                        CellPaddingInlineSM = paddingXS,
                        BorderColor = colorBorderSecondary,
                        HeaderBorderRadius = borderRadiusLG,
                        FooterBg = colorFillAlterSolid,
                        FooterColor = colorTextHeading,
                        CellFontSize = fontSize,
                        CellFontSizeMD = fontSize,
                        CellFontSizeSM = fontSize,
                        HeaderSplitColor = colorBorderSecondary,
                        FixedHeaderSortActiveBg = colorFillSecondarySolid,
                        HeaderFilterHoverBg = colorFillContent,
                        FilterDropdownMenuBg = colorBgContainer,
                        FilterDropdownBg = colorBgContainer,
                        ExpandIconBg = colorBgContainer,
                        SelectionColumnWidth = controlHeight,
                        StickyScrollBarBg = colorTextPlaceholder,
                        StickyScrollBarBorderRadius = 100,
                    };
                });
        }

        public CSSObject GenBorderedStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            var tableBorder = @$"{token.LineWidth}px {token.LineType} {token.TableBorderColor}";
            var getSizeBorderStyle = (string size, double paddingVertical, double paddingHorizontal) => {
                return new CSSObject()
                {
                    [$"&{componentCls}-{size}"] = new CSSObject()
                    {
                        [$"> {componentCls}-container"] = new CSSObject()
                        {
                            [$"> {componentCls}-content, > {componentCls}-body"] = new CSSObject()
                            {
                                [">table>tbody>tr>th,>table>tbody>tr>td"] = new CSSObject()
                                {
                                    [$"> {componentCls}-expanded-row-fixed"] = new CSSObject()
                                    {
                                        Margin = @$"-{paddingVertical}px -{paddingHorizontal + token.LineWidth}px",
                                    },
                                },
                            },
                        },
                    },
                };
            };
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}{componentCls}-bordered"] = new CSSObject()
                    {
                        [$"> {componentCls}-title"] = new CSSObject()
                        {
                            Border = tableBorder,
                            BorderBottom = 0,
                        },
                        [$"> {componentCls}-container"] = new CSSObject()
                        {
                            BorderInlineStart = tableBorder,
                            BorderTop = tableBorder,
                            [$">{componentCls}-content,>{componentCls}-header,>{componentCls}-body,>{componentCls}-summary"] = new CSSObject()
                            {
                                ["> table"] = new CSSObject()
                                {
                                    [">thead>tr>th,>thead>tr>td,>tbody>tr>th,>tbody>tr>td,>tfoot>tr>th,>tfoot>tr>td"] = new CSSObject()
                                    {
                                        BorderInlineEnd = tableBorder,
                                    },
                                    ["> thead"] = new CSSObject()
                                    {
                                        ["> tr:not(:last-child) > th"] = new CSSObject()
                                        {
                                            BorderBottom = tableBorder,
                                        },
                                        ["> tr > th::before"] = new CSSObject()
                                        {
                                            BackgroundColor = "transparent !important",
                                        },
                                    },
                                    [">thead>tr,>tbody>tr,>tfoot>tr"] = new CSSObject()
                                    {
                                        [$"> {componentCls}-cell-fix-right-first::after"] = new CSSObject()
                                        {
                                            BorderInlineEnd = tableBorder,
                                        },
                                    },
                                    [">tbody>tr>th,>tbody>tr>td"] = new CSSObject()
                                    {
                                        [$"> {componentCls}-expanded-row-fixed"] = new CSSObject()
                                        {
                                            Margin = @$"-{token.TablePaddingVertical}px -{
                    token.TablePaddingHorizontal + token.LineWidth
                  }px",
                                            ["&::after"] = new CSSObject()
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
                        [$"&{componentCls}-scroll-horizontal"] = new CSSObject()
                        {
                            [$"> {componentCls}-container > {componentCls}-body"] = new CSSObject()
                            {
                                ["> table > tbody"] = new CSSObject()
                                {
                                    [$">tr{componentCls}-expanded-row,>tr{componentCls}-placeholder"] = new CSSObject()
                                    {
                                        ["> th, > td"] = new CSSObject()
                                        {
                                            BorderInlineEnd = 0,
                                        },
                                    },
                                },
                            },
                        },
                        ["..."] = getSizeBorderStyle("middle", token.TablePaddingVerticalMiddle, token.TablePaddingHorizontalMiddle),
                        ["..."] = getSizeBorderStyle("small", token.TablePaddingVerticalSmall, token.TablePaddingHorizontalSmall),
                        [$"> {componentCls}-footer"] = new CSSObject()
                        {
                            Border = tableBorder,
                            BorderTop = 0,
                        },
                    },
                    [$"{componentCls}-cell"] = new CSSObject()
                    {
                        [$"{componentCls}-container:first-child"] = new CSSObject()
                        {
                            BorderTop = 0,
                        },
                        ["&-scrollbar:not([rowspan])"] = new CSSObject()
                        {
                            BoxShadow = @$"0 {token.LineWidth}px 0 {token.LineWidth}px {token.TableHeaderBg}",
                        },
                    },
                    [$"{componentCls}-bordered {componentCls}-cell-scrollbar"] = new CSSObject()
                    {
                        BorderInlineEnd = tableBorder,
                    },
                },
            };
        }

        public CSSObject GenEllipsisStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-cell-ellipsis"] = new CSSObject()
                    {
                        ["..."] = TextEllipsis,
                        WordBreak = "keep-all",
                        [$"&{componentCls}-cell-fix-left-last,&{componentCls}-cell-fix-right-first"] = new CSSObject()
                        {
                            Overflow = "visible",
                            [$"{componentCls}-cell-content"] = new CSSObject()
                            {
                                Display = "block",
                                Overflow = "hidden",
                                TextOverflow = "ellipsis",
                            },
                        },
                        [$"{componentCls}-column-title"] = new CSSObject()
                        {
                            Overflow = "hidden",
                            TextOverflow = "ellipsis",
                            WordBreak = "keep-all",
                        },
                    },
                },
            };
        }

        public CSSObject GenEmptyStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-tbody > tr{componentCls}-placeholder"] = new CSSObject()
                    {
                        TextAlign = "center",
                        Color = token.ColorTextDisabled,
                        ["&:hover>th,&:hover>td,"] = new CSSObject()
                        {
                            Background = token.ColorBgContainer,
                        },
                    },
                },
            };
        }

        public CSSObject GenExpandStyle(TableToken token)
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
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-expand-icon-col"] = new CSSObject()
                    {
                        Width = tableExpandColumnWidth,
                    },
                    [$"{componentCls}-row-expand-icon-cell"] = new CSSObject()
                    {
                        TextAlign = "center",
                        [$"{componentCls}-row-expand-icon"] = new CSSObject()
                        {
                            Display = "inline-flex",
                            Float = "none",
                            VerticalAlign = "sub",
                        },
                    },
                    [$"{componentCls}-row-indent"] = new CSSObject()
                    {
                        Height = 1,
                        Float = "left",
                    },
                    [$"{componentCls}-row-expand-icon"] = new CSSObject()
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
                        ["&:focus, &:hover, &:active"] = new CSSObject()
                        {
                            BorderColor = "currentcolor",
                        },
                        ["&::before, &::after"] = new CSSObject()
                        {
                            Position = "absolute",
                            Background = "currentcolor",
                            Transition = @$"transform {motionDurationSlow} ease-out",
                            Content = "\"\"",
                        },
                        ["&::before"] = new CSSObject()
                        {
                            Top = halfInnerSize,
                            InsetInlineEnd = expandIconLineOffset,
                            InsetInlineStart = expandIconLineOffset,
                            Height = lineWidth,
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Top = expandIconLineOffset,
                            Bottom = expandIconLineOffset,
                            InsetInlineStart = halfInnerSize,
                            Width = lineWidth,
                            Transform = "rotate(90deg)",
                        },
                        ["&-collapsed::before"] = new CSSObject()
                        {
                            Transform = "rotate(-180deg)",
                        },
                        ["&-collapsed::after"] = new CSSObject()
                        {
                            Transform = "rotate(0deg)",
                        },
                        ["&-spaced"] = new CSSObject()
                        {
                            ["&::before, &::after"] = new CSSObject()
                            {
                                Display = "none",
                                Content = "none",
                            },
                            Background = "transparent",
                            Border = 0,
                            Visibility = "hidden",
                        },
                    },
                    [$"{componentCls}-row-indent + {componentCls}-row-expand-icon"] = new CSSObject()
                    {
                        MarginTop = (fontSize * lineHeight - lineWidth * 3) / 2 - Math.Ceiling((fontSizeSM * 1.4 - lineWidth * 3) / 2),
                        MarginInlineEnd = paddingXS,
                    },
                    [$"tr{componentCls}-expanded-row"] = new CSSObject()
                    {
                        ["&, &:hover"] = new CSSObject()
                        {
                            ["> th, > td"] = new CSSObject()
                            {
                                Background = tableExpandedRowBg,
                            },
                        },
                        [$"{antCls}-descriptions-view"] = new CSSObject()
                        {
                            Display = "flex",
                            ["table"] = new CSSObject()
                            {
                                Flex = "auto",
                                Width = "auto",
                            },
                        },
                    },
                    [$"{componentCls}-expanded-row-fixed"] = new CSSObject()
                    {
                        Position = "relative",
                        Margin = @$"-{tablePaddingVertical}px -{tablePaddingHorizontal}px",
                        Padding = @$"{tablePaddingVertical}px {tablePaddingHorizontal}px",
                    },
                },
            };
        }

        public CSSInterpolation GenFilterStyle(TableToken token)
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
            var filterDropdownMenuBg = token.FilterDropdownMenuBg;
            var dropdownPrefixCls = @$"{antCls}-dropdown";
            var tableFilterDropdownPrefixCls = @$"{componentCls}-filter-dropdown";
            var treePrefixCls = @$"{antCls}-tree";
            var tableBorder = @$"{lineWidth}px {lineType} {tableBorderColor}";
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [$"{componentCls}-wrapper"] = new CSSObject()
                    {
                        [$"{componentCls}-filter-column"] = new CSSObject()
                        {
                            Display = "flex",
                            JustifyContent = "space-between",
                        },
                        [$"{componentCls}-filter-trigger"] = new CSSObject()
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
                            ["&:hover"] = new CSSObject()
                            {
                                Color = colorTextDescription,
                                Background = tableHeaderFilterActiveBg,
                            },
                            ["&.active"] = new CSSObject()
                            {
                                Color = colorPrimary,
                            },
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{antCls}-dropdown"] = new CSSObject()
                    {
                        [tableFilterDropdownPrefixCls] = new CSSObject()
                        {
                            ["..."] = ResetComponent(token),
                            MinWidth = tableFilterDropdownWidth,
                            BackgroundColor = tableFilterDropdownBg,
                            BorderRadius = borderRadius,
                            BoxShadow = boxShadowSecondary,
                            Overflow = "hidden",
                            [$"{dropdownPrefixCls}-menu"] = new CSSObject()
                            {
                                MaxHeight = tableFilterDropdownHeight,
                                OverflowX = "hidden",
                                Border = 0,
                                BoxShadow = "none",
                                BorderRadius = "unset",
                                BackgroundColor = filterDropdownMenuBg,
                                ["&:empty::after"] = new CSSObject()
                                {
                                    Display = "block",
                                    Padding = @$"{paddingXS}px 0",
                                    Color = colorTextDisabled,
                                    FontSize = fontSizeSM,
                                    TextAlign = "center",
                                    Content = "\"Not Found\"",
                                },
                            },
                            [$"{tableFilterDropdownPrefixCls}-tree"] = new CSSObject()
                            {
                                PaddingBlock = @$"{paddingXS}px 0",
                                PaddingInline = paddingXS,
                                [treePrefixCls] = new CSSObject()
                                {
                                    Padding = 0,
                                },
                                [$"{treePrefixCls}-treenode {treePrefixCls}-node-content-wrapper:hover"] = new CSSObject()
                                {
                                    BackgroundColor = controlItemBgHover,
                                },
                                [$"{treePrefixCls}-treenode-checkbox-checked {treePrefixCls}-node-content-wrapper"] = new CSSObject()
                                {
                                    ["&, &:hover"] = new CSSObject()
                                    {
                                        BackgroundColor = controlItemBgActive,
                                    },
                                },
                            },
                            [$"{tableFilterDropdownPrefixCls}-search"] = new CSSObject()
                            {
                                Padding = paddingXS,
                                BorderBottom = tableBorder,
                                ["&-input"] = new CSSObject()
                                {
                                    ["input"] = new CSSObject()
                                    {
                                        MinWidth = tableFilterDropdownSearchWidth,
                                    },
                                    [iconCls] = new CSSObject()
                                    {
                                        Color = colorTextDisabled,
                                    },
                                },
                            },
                            [$"{tableFilterDropdownPrefixCls}-checkall"] = new CSSObject()
                            {
                                Width = "100%",
                                MarginBottom = paddingXXS,
                                MarginInlineStart = paddingXXS,
                            },
                            [$"{tableFilterDropdownPrefixCls}-btns"] = new CSSObject()
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
                new CSSObject()
                {
                    [$"{antCls}-dropdown {tableFilterDropdownPrefixCls}, {tableFilterDropdownPrefixCls}-submenu"] = new CSSObject()
                    {
                        [$"{antCls}-checkbox-wrapper + span"] = new CSSObject()
                        {
                            PaddingInlineStart = paddingXS,
                            Color = colorText,
                        },
                        ["> ul"] = new CSSObject()
                        {
                            MaxHeight = "calc(100vh - 130px)",
                            OverflowX = "hidden",
                            OverflowY = "auto",
                        },
                    },
                },
            };
        }

        public CSSObject GenFixedStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            var lineWidth = token.LineWidth;
            var colorSplit = token.ColorSplit;
            var motionDurationSlow = token.MotionDurationSlow;
            var zIndexTableFixed = token.ZIndexTableFixed;
            var tableBg = token.TableBg;
            var zIndexTableSticky = token.ZIndexTableSticky;
            var shadowColor = colorSplit;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-cell-fix-left,{componentCls}-cell-fix-right"] = new CSSObject()
                    {
                        Position = "sticky !important",
                        ZIndex = zIndexTableFixed,
                        Background = tableBg,
                    },
                    [$"{componentCls}-cell-fix-left-first::after,{componentCls}-cell-fix-left-last::after"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = 0,
                        Right = new PropertySkip()
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
                    [$"{componentCls}-cell-fix-left-all::after"] = new CSSObject()
                    {
                        Display = "none",
                    },
                    [$"{componentCls}-cell-fix-right-first::after,{componentCls}-cell-fix-right-last::after"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = 0,
                        Bottom = -lineWidth,
                        Left = new PropertySkip()
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
                    [$"{componentCls}-container"] = new CSSObject()
                    {
                        ["&::before, &::after"] = new CSSObject()
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
                        ["&::before"] = new CSSObject()
                        {
                            InsetInlineStart = 0,
                        },
                        ["&::after"] = new CSSObject()
                        {
                            InsetInlineEnd = 0,
                        },
                    },
                    [$"{componentCls}-ping-left"] = new CSSObject()
                    {
                        [$"&:not({componentCls}-has-fix-left) {componentCls}-container"] = new CSSObject()
                        {
                            Position = "relative",
                            ["&::before"] = new CSSObject()
                            {
                                BoxShadow = @$"inset 10px 0 8px -8px {shadowColor}",
                            },
                        },
                        [$"{componentCls}-cell-fix-left-first::after,{componentCls}-cell-fix-left-last::after"] = new CSSObject()
                        {
                            BoxShadow = @$"inset 10px 0 8px -8px {shadowColor}",
                        },
                        [$"{componentCls}-cell-fix-left-last::before"] = new CSSObject()
                        {
                            BackgroundColor = "transparent !important",
                        },
                    },
                    [$"{componentCls}-ping-right"] = new CSSObject()
                    {
                        [$"&:not({componentCls}-has-fix-right) {componentCls}-container"] = new CSSObject()
                        {
                            Position = "relative",
                            ["&::after"] = new CSSObject()
                            {
                                BoxShadow = @$"inset -10px 0 8px -8px {shadowColor}",
                            },
                        },
                        [$"{componentCls}-cell-fix-right-first::after,{componentCls}-cell-fix-right-last::after"] = new CSSObject()
                        {
                            BoxShadow = @$"inset -10px 0 8px -8px {shadowColor}",
                        },
                    },
                },
            };
        }

        public CSSObject GenPaginationStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-pagination{antCls}-pagination"] = new CSSObject()
                    {
                        Margin = @$"{token.Margin}px 0",
                    },
                    [$"{componentCls}-pagination"] = new CSSObject()
                    {
                        Display = "flex",
                        FlexWrap = "wrap",
                        RowGap = token.PaddingXS,
                        ["> *"] = new CSSObject()
                        {
                            Flex = "none",
                        },
                        ["&-left"] = new CSSObject()
                        {
                            JustifyContent = "flex-start",
                        },
                        ["&-center"] = new CSSObject()
                        {
                            JustifyContent = "center",
                        },
                        ["&-right"] = new CSSObject()
                        {
                            JustifyContent = "flex-end",
                        },
                    },
                },
            };
        }

        public CSSObject GenRadiusStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            var tableRadius = token.TableRadius;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        [$"{componentCls}-title, {componentCls}-header"] = new CSSObject()
                        {
                            BorderRadius = @$"{tableRadius}px {tableRadius}px 0 0",
                        },
                        [$"{componentCls}-title + {componentCls}-container"] = new CSSObject()
                        {
                            BorderStartStartRadius = 0,
                            BorderStartEndRadius = 0,
                            [$"{componentCls}-header, table"] = new CSSObject()
                            {
                                BorderRadius = 0,
                            },
                            ["table > thead > tr:first-child"] = new CSSObject()
                            {
                                ["th:first-child, th:last-child, td:first-child, td:last-child"] = new CSSObject()
                                {
                                    BorderRadius = 0,
                                },
                            },
                        },
                        ["&-container"] = new CSSObject()
                        {
                            BorderStartStartRadius = tableRadius,
                            BorderStartEndRadius = tableRadius,
                            ["table > thead > tr:first-child"] = new CSSObject()
                            {
                                ["> *:first-child"] = new CSSObject()
                                {
                                    BorderStartStartRadius = tableRadius,
                                },
                                ["> *:last-child"] = new CSSObject()
                                {
                                    BorderStartEndRadius = tableRadius,
                                },
                            },
                        },
                        ["&-footer"] = new CSSObject()
                        {
                            BorderRadius = @$"0 0 {tableRadius}px {tableRadius}px",
                        },
                    },
                },
            };
        }

        public CSSObject GenRtlStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper-rtl"] = new CSSObject()
                {
                    Direction = "rtl",
                    ["table"] = new CSSObject()
                    {
                        Direction = "rtl",
                    },
                    [$"{componentCls}-pagination-left"] = new CSSObject()
                    {
                        JustifyContent = "flex-end",
                    },
                    [$"{componentCls}-pagination-right"] = new CSSObject()
                    {
                        JustifyContent = "flex-start",
                    },
                    [$"{componentCls}-row-expand-icon"] = new CSSObject()
                    {
                        Float = "right",
                        ["&::after"] = new CSSObject()
                        {
                            Transform = "rotate(-90deg)",
                        },
                        ["&-collapsed::before"] = new CSSObject()
                        {
                            Transform = "rotate(180deg)",
                        },
                        ["&-collapsed::after"] = new CSSObject()
                        {
                            Transform = "rotate(0deg)",
                        },
                    },
                    [$"{componentCls}-container"] = new CSSObject()
                    {
                        ["&::before"] = new CSSObject()
                        {
                            InsetInlineStart = "unset",
                            InsetInlineEnd = 0,
                        },
                        ["&::after"] = new CSSObject()
                        {
                            InsetInlineStart = 0,
                            InsetInlineEnd = "unset",
                        },
                        [$"{componentCls}-row-indent"] = new CSSObject()
                        {
                            Float = "right",
                        },
                    },
                },
            };
        }

        public CSSObject GenSelectionStyle(TableToken token)
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
            var tableSelectedRowBg = token.TableSelectedRowBg;
            var tableSelectedRowHoverBg = token.TableSelectedRowHoverBg;
            var tableRowHoverBg = token.TableRowHoverBg;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-selection-col"] = new CSSObject()
                    {
                        Width = tableSelectionColumnWidth,
                        [$"&{componentCls}-selection-col-with-dropdown"] = new CSSObject()
                        {
                            Width = tableSelectionColumnWidth + fontSizeIcon + padding / 4,
                        },
                    },
                    [$"{componentCls}-bordered {componentCls}-selection-col"] = new CSSObject()
                    {
                        Width = tableSelectionColumnWidth + paddingXS * 2,
                        [$"&{componentCls}-selection-col-with-dropdown"] = new CSSObject()
                        {
                            Width = tableSelectionColumnWidth + fontSizeIcon + padding / 4 + paddingXS * 2,
                        },
                    },
                    [$"tabletrth{componentCls}-selection-column,tabletrtd{componentCls}-selection-column,{componentCls}-selection-column"] = new CSSObject()
                    {
                        PaddingInlineEnd = token.PaddingXS,
                        PaddingInlineStart = token.PaddingXS,
                        TextAlign = "center",
                        [$"{antCls}-radio-wrapper"] = new CSSObject()
                        {
                            MarginInlineEnd = 0,
                        },
                    },
                    [$"table tr th{componentCls}-selection-column{componentCls}-cell-fix-left"] = new CSSObject()
                    {
                        ZIndex = token.ZIndexTableFixed + 1,
                    },
                    [$"table tr th{componentCls}-selection-column::after"] = new CSSObject()
                    {
                        BackgroundColor = "transparent !important",
                    },
                    [$"{componentCls}-selection"] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "inline-flex",
                        FlexDirection = "column",
                    },
                    [$"{componentCls}-selection-extra"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = 0,
                        ZIndex = 1,
                        Cursor = "pointer",
                        Transition = @$"all {token.MotionDurationSlow}",
                        MarginInlineStart = "100%",
                        PaddingInlineStart = @$"{token.TablePaddingHorizontal / 4}px",
                        [iconCls] = new CSSObject()
                        {
                            Color = tableHeaderIconColor,
                            FontSize = fontSizeIcon,
                            VerticalAlign = "baseline",
                            ["&:hover"] = new CSSObject()
                            {
                                Color = tableHeaderIconColorHover,
                            },
                        },
                    },
                    [$"{componentCls}-tbody"] = new CSSObject()
                    {
                        [$"{componentCls}-row"] = new CSSObject()
                        {
                            [$"&{componentCls}-row-selected"] = new CSSObject()
                            {
                                [$"> {componentCls}-cell"] = new CSSObject()
                                {
                                    Background = tableSelectedRowBg,
                                    ["&-row-hover"] = new CSSObject()
                                    {
                                        Background = tableSelectedRowHoverBg,
                                    },
                                },
                            },
                            [$"> {componentCls}-cell-row-hover"] = new CSSObject()
                            {
                                Background = tableRowHoverBg,
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenSizeStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            var getSizeStyle = (string size, double paddingVertical, double paddingHorizontal, double fontSize) => {
                return new CSSObject()
                {
                    [$"{componentCls}{componentCls}-{size}"] = new CSSObject()
                    {
                        FontSize = fontSize,
                        [$"{componentCls}-title,{componentCls}-footer,{componentCls}-cell,{componentCls}-thead>tr>th,{componentCls}-tbody>tr>th,{componentCls}-tbody>tr>td,tfoot>tr>th,tfoot>tr>td"] = new CSSObject()
                        {
                            Padding = @$"{paddingVertical}px {paddingHorizontal}px",
                        },
                        [$"{componentCls}-filter-trigger"] = new CSSObject()
                        {
                            MarginInlineEnd = @$"-{paddingHorizontal / 2}px",
                        },
                        [$"{componentCls}-expanded-row-fixed"] = new CSSObject()
                        {
                            Margin = @$"-{paddingVertical}px -{paddingHorizontal}px",
                        },
                        [$"{componentCls}-tbody"] = new CSSObject()
                        {
                            [$"{componentCls}-wrapper:only-child {componentCls}"] = new CSSObject()
                            {
                                MarginBlock = @$"-{paddingVertical}px",
                                MarginInline = @$"{token.TableExpandColumnWidth - paddingHorizontal}px -{paddingHorizontal}px",
                            },
                        },
                        [$"{componentCls}-selection-extra"] = new CSSObject()
                        {
                            PaddingInlineStart = @$"{paddingHorizontal / 4}px",
                        },
                    },
                };
            };
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    ["..."] = getSizeStyle("middle", token.TablePaddingVerticalMiddle, token.TablePaddingHorizontalMiddle, token.TableFontSizeMiddle),
                    ["..."] = getSizeStyle("small", token.TablePaddingVerticalSmall, token.TablePaddingHorizontalSmall, token.TableFontSizeSmall)
                },
            };
        }

        public CSSObject GenSorterStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            var marginXXS = token.MarginXXS;
            var fontSizeIcon = token.FontSizeIcon;
            var tableHeaderIconColor = token.TableHeaderIconColor;
            var tableHeaderIconColorHover = token.TableHeaderIconColorHover;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-thead th{componentCls}-column-has-sorters"] = new CSSObject()
                    {
                        Outline = "none",
                        Cursor = "pointer",
                        Transition = @$"all {token.MotionDurationSlow}",
                        ["&:hover"] = new CSSObject()
                        {
                            Background = token.TableHeaderSortHoverBg,
                            ["&::before"] = new CSSObject()
                            {
                                BackgroundColor = "transparent !important",
                            },
                        },
                        ["&:focus-visible"] = new CSSObject()
                        {
                            Color = token.ColorPrimary,
                        },
                        [$"&{componentCls}-cell-fix-left:hover,&{componentCls}-cell-fix-right:hover"] = new CSSObject()
                        {
                            Background = token.TableFixedHeaderSortActiveBg,
                        },
                    },
                    [$"{componentCls}-thead th{componentCls}-column-sort"] = new CSSObject()
                    {
                        Background = token.TableHeaderSortBg,
                        ["&::before"] = new CSSObject()
                        {
                            BackgroundColor = "transparent !important",
                        },
                    },
                    [$"td{componentCls}-column-sort"] = new CSSObject()
                    {
                        Background = token.TableBodySortBg,
                    },
                    [$"{componentCls}-column-title"] = new CSSObject()
                    {
                        Position = "relative",
                        ZIndex = 1,
                        Flex = 1,
                    },
                    [$"{componentCls}-column-sorters"] = new CSSObject()
                    {
                        Display = "flex",
                        Flex = "auto",
                        AlignItems = "center",
                        JustifyContent = "space-between",
                        ["&::after"] = new CSSObject()
                        {
                            Position = "absolute",
                            Inset = 0,
                            Width = "100%",
                            Height = "100%",
                            Content = "\"\"",
                        },
                    },
                    [$"{componentCls}-column-sorter"] = new CSSObject()
                    {
                        MarginInlineStart = marginXXS,
                        Color = tableHeaderIconColor,
                        FontSize = 0,
                        Transition = @$"color {token.MotionDurationSlow}",
                        ["&-inner"] = new CSSObject()
                        {
                            Display = "inline-flex",
                            FlexDirection = "column",
                            AlignItems = "center",
                        },
                        ["&-up, &-down"] = new CSSObject()
                        {
                            FontSize = fontSizeIcon,
                            ["&.active"] = new CSSObject()
                            {
                                Color = token.ColorPrimary,
                            },
                        },
                        [$"{componentCls}-column-sorter-up + {componentCls}-column-sorter-down"] = new CSSObject()
                        {
                            MarginTop = "-0.3em",
                        },
                    },
                    [$"{componentCls}-column-sorters:hover {componentCls}-column-sorter"] = new CSSObject()
                    {
                        Color = tableHeaderIconColorHover,
                    },
                },
            };
        }

        public CSSObject GenStickyStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            var opacityLoading = token.OpacityLoading;
            var tableScrollThumbBg = token.TableScrollThumbBg;
            var tableScrollThumbBgHover = token.TableScrollThumbBgHover;
            var tableScrollThumbSize = token.TableScrollThumbSize;
            var tableScrollBg = token.TableScrollBg;
            var zIndexTableSticky = token.ZIndexTableSticky;
            var stickyScrollBarBorderRadius = token.StickyScrollBarBorderRadius;
            var tableBorder = @$"{token.LineWidth}px {token.LineType} {token.TableBorderColor}";
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-sticky"] = new CSSObject()
                    {
                        ["&-holder"] = new CSSObject()
                        {
                            Position = "sticky",
                            ZIndex = zIndexTableSticky,
                            Background = token.ColorBgContainer,
                        },
                        ["&-scroll"] = new CSSObject()
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
                            ["&:hover"] = new CSSObject()
                            {
                                TransformOrigin = "center bottom",
                            },
                            ["&-bar"] = new CSSObject()
                            {
                                Height = tableScrollThumbSize,
                                BackgroundColor = tableScrollThumbBg,
                                BorderRadius = stickyScrollBarBorderRadius,
                                Transition = @$"all {token.MotionDurationSlow}, transform none",
                                Position = "absolute",
                                Bottom = 0,
                                ["&:hover, &-active"] = new CSSObject()
                                {
                                    BackgroundColor = tableScrollThumbBgHover,
                                },
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenSummaryStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            var lineWidth = token.LineWidth;
            var tableBorderColor = token.TableBorderColor;
            var tableBorder = @$"{lineWidth}px {token.LineType} {tableBorderColor}";
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-summary"] = new CSSObject()
                    {
                        Position = "relative",
                        ZIndex = token.ZIndexTableFixed,
                        Background = token.TableBg,
                        ["> tr"] = new CSSObject()
                        {
                            ["> th, > td"] = new CSSObject()
                            {
                                BorderBottom = tableBorder,
                            },
                        },
                    },
                    [$"div{componentCls}-summary"] = new CSSObject()
                    {
                        BoxShadow = @$"0 -{lineWidth}px 0 {tableBorderColor}",
                    },
                },
            };
        }

        public CSSObject GenVirtualStyle(TableToken token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationMid = token.MotionDurationMid;
            var tableBorder = @$"{token.LineWidth}px {token.LineType} {token.TableBorderColor}";
            var rowCellCls = @$"{componentCls}-expanded-row-cell";
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-tbody-virtual"] = new CSSObject()
                    {
                        [$"{componentCls}-row"] = new CSSObject()
                        {
                            Display = "flex",
                            BoxSizing = "border-box",
                            Width = "100%",
                        },
                        [$"{componentCls}-cell"] = new CSSObject()
                        {
                            BorderBottom = tableBorder,
                            Transition = @$"background {motionDurationMid}",
                        },
                        [$"{componentCls}-expanded-row"] = new CSSObject()
                        {
                            [$"{rowCellCls}{rowCellCls}-fixed"] = new CSSObject()
                            {
                                Position = "sticky",
                                InsetInlineStart = 0,
                                Overflow = "hidden",
                                Width = @$"calc(var(--virtual-width) - {token.LineWidth}px)",
                                BorderInlineEnd = "none",
                            },
                        },
                    },
                    [$"{componentCls}-bordered"] = new CSSObject()
                    {
                        [$"{componentCls}-tbody-virtual"] = new CSSObject()
                        {
                            ["&:after"] = new CSSObject()
                            {
                                Content = "\"\"",
                                InsetInline = 0,
                                Bottom = 0,
                                BorderBottom = tableBorder,
                                Position = "absolute",
                            },
                            [$"{componentCls}-cell"] = new CSSObject()
                            {
                                BorderInlineEnd = tableBorder,
                                [$"&{componentCls}-cell-fix-right-first:before"] = new CSSObject()
                                {
                                    Content = "\"\"",
                                    Position = "absolute",
                                    InsetBlock = 0,
                                    InsetInlineStart = -token.LineWidth,
                                    BorderInlineStart = tableBorder,
                                },
                            },
                        },
                        [$"&{componentCls}-virtual"] = new CSSObject()
                        {
                            [$"{componentCls}-placeholder {componentCls}-cell"] = new CSSObject()
                            {
                                BorderInlineEnd = tableBorder,
                                BorderBottom = tableBorder,
                            },
                        },
                    },
                },
            };
        }
    }
}
