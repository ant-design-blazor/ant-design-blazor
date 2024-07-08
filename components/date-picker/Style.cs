using System;
using CssInCSharp;
using CssInCSharp.Colors;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.Slide;
using static AntDesign.Move;
using static AntDesign.InputStyle;

namespace AntDesign
{
    public class PanelComponentToken : InputToken
    {
        public string CellHoverBg
        {
            get => (string)_tokens["cellHoverBg"];
            set => _tokens["cellHoverBg"] = value;
        }

        public string CellActiveWithRangeBg
        {
            get => (string)_tokens["cellActiveWithRangeBg"];
            set => _tokens["cellActiveWithRangeBg"] = value;
        }

        public string CellHoverWithRangeBg
        {
            get => (string)_tokens["cellHoverWithRangeBg"];
            set => _tokens["cellHoverWithRangeBg"] = value;
        }

        public string CellBgDisabled
        {
            get => (string)_tokens["cellBgDisabled"];
            set => _tokens["cellBgDisabled"] = value;
        }

        public string CellRangeBorderColor
        {
            get => (string)_tokens["cellRangeBorderColor"];
            set => _tokens["cellRangeBorderColor"] = value;
        }

        public double TimeColumnWidth
        {
            get => (double)_tokens["timeColumnWidth"];
            set => _tokens["timeColumnWidth"] = value;
        }

        public double TimeColumnHeight
        {
            get => (double)_tokens["timeColumnHeight"];
            set => _tokens["timeColumnHeight"] = value;
        }

        public double TimeCellHeight
        {
            get => (double)_tokens["timeCellHeight"];
            set => _tokens["timeCellHeight"] = value;
        }

        public double CellHeight
        {
            get => (double)_tokens["cellHeight"];
            set => _tokens["cellHeight"] = value;
        }

        public double CellWidth
        {
            get => (double)_tokens["cellWidth"];
            set => _tokens["cellWidth"] = value;
        }

        public double TextHeight
        {
            get => (double)_tokens["textHeight"];
            set => _tokens["textHeight"] = value;
        }

        public double WithoutTimeCellHeight
        {
            get => (double)_tokens["withoutTimeCellHeight"];
            set => _tokens["withoutTimeCellHeight"] = value;
        }
    }

    public class PickerPanelToken : PanelComponentToken
    {
        public string PickerCellCls
        {
            get => (string)_tokens["pickerCellCls"];
            set => _tokens["pickerCellCls"] = value;
        }

        public string PickerCellInnerCls
        {
            get => (string)_tokens["pickerCellInnerCls"];
            set => _tokens["pickerCellInnerCls"] = value;
        }

        public double PickerDatePanelPaddingHorizontal
        {
            get => (double)_tokens["pickerDatePanelPaddingHorizontal"];
            set => _tokens["pickerDatePanelPaddingHorizontal"] = value;
        }

        public double PickerYearMonthCellWidth
        {
            get => (double)_tokens["pickerYearMonthCellWidth"];
            set => _tokens["pickerYearMonthCellWidth"] = value;
        }

        public double PickerCellPaddingVertical
        {
            get => (double)_tokens["pickerCellPaddingVertical"];
            set => _tokens["pickerCellPaddingVertical"] = value;
        }

        public double PickerQuarterPanelContentHeight
        {
            get => (double)_tokens["pickerQuarterPanelContentHeight"];
            set => _tokens["pickerQuarterPanelContentHeight"] = value;
        }

        public double PickerCellBorderGap
        {
            get => (double)_tokens["pickerCellBorderGap"];
            set => _tokens["pickerCellBorderGap"] = value;
        }

        public double PickerControlIconSize
        {
            get => (double)_tokens["pickerControlIconSize"];
            set => _tokens["pickerControlIconSize"] = value;
        }

        public double PickerControlIconBorderWidth
        {
            get => (double)_tokens["pickerControlIconBorderWidth"];
            set => _tokens["pickerControlIconBorderWidth"] = value;
        }

    }

    public partial class PickerToken : PickerPanelToken
    {
        public double PresetsWidth
        {
            get => (double)_tokens["presetsWidth"];
            set => _tokens["presetsWidth"] = value;
        }

        public double PresetsMaxWidth
        {
            get => (double)_tokens["presetsMaxWidth"];
            set => _tokens["presetsMaxWidth"] = value;
        }

        public double ZIndexPopup
        {
            get => (double)_tokens["zIndexPopup"];
            set => _tokens["zIndexPopup"] = value;
        }
    }

    public class DatePickerStyle
    {
        public static CSSObject GenPikerPadding(PickerToken token, double inputHeight, double fontSize, double paddingHorizontal)
        {
            var lineHeight = token.LineHeight;
            var fontHeight = Math.Floor(fontSize * lineHeight) + 2;
            var paddingTop = Math.Max((inputHeight - fontHeight) / 2, 0);
            var paddingBottom = Math.Max(inputHeight - fontHeight - paddingTop, 0);
            return new CSSObject()
            {
                Padding = @$"{paddingTop}px {paddingHorizontal}px {paddingBottom}px",
            };
        }

        public static CSSObject GenPickerCellInnerStyle(PickerPanelToken token)
        {
            var componentCls = token.ComponentCls;
            var pickerCellCls = token.PickerCellCls;
            var pickerCellInnerCls = token.PickerCellInnerCls;
            var cellHeight = token.CellHeight;
            var motionDurationSlow = token.MotionDurationSlow;
            var borderRadiusSM = token.BorderRadiusSM;
            var motionDurationMid = token.MotionDurationMid;
            var cellHoverBg = token.CellHoverBg;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var colorPrimary = token.ColorPrimary;
            var cellActiveWithRangeBg = token.CellActiveWithRangeBg;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var controlHeightSM = token.ControlHeightSM;
            var cellRangeBorderColor = token.CellRangeBorderColor;
            var pickerCellBorderGap = token.PickerCellBorderGap;
            var cellHoverWithRangeBg = token.CellHoverWithRangeBg;
            var cellWidth = token.CellWidth;
            var colorTextDisabled = token.ColorTextDisabled;
            var cellBgDisabled = token.CellBgDisabled;
            return new CSSObject()
            {
                ["&::before"] = new CSSObject()
                {
                    Position = "absolute",
                    Top = "50%",
                    InsetInlineStart = 0,
                    InsetInlineEnd = 0,
                    ZIndex = 1,
                    Height = cellHeight,
                    Transform = "translateY(-50%)",
                    Transition = @$"all {motionDurationSlow}",
                    Content = "\"\"",
                },
                [pickerCellInnerCls] = new CSSObject()
                {
                    Position = "relative",
                    ZIndex = 2,
                    Display = "inline-block",
                    MinWidth = cellHeight,
                    Height = cellHeight,
                    LineHeight = @$"{cellHeight}px",
                    BorderRadius = borderRadiusSM,
                    Transition = @$"background {motionDurationMid}, border {motionDurationMid}",
                },
                ["&-range-hover-start, &-range-hover-end"] = new CSSObject()
                {
                    [pickerCellInnerCls] = new CSSObject()
                    {
                        BorderStartEndRadius = 0,
                        BorderEndEndRadius = 0,
                    },
                },
                [$"&:hover:not({pickerCellCls}-in-view),&:hover:not({pickerCellCls}-selected):not({pickerCellCls}-range-start):not({pickerCellCls}-range-end):not({pickerCellCls}-range-hover-start):not({pickerCellCls}-range-hover-end)"] = new CSSObject()
                {
                    [pickerCellInnerCls] = new CSSObject()
                    {
                        Background = cellHoverBg,
                    },
                },
                [$"&-in-view{pickerCellCls}-today {pickerCellInnerCls}"] = new CSSObject()
                {
                    ["&::before"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = 0,
                        InsetInlineEnd = 0,
                        Bottom = 0,
                        InsetInlineStart = 0,
                        ZIndex = 1,
                        Border = @$"{lineWidth}px {lineType} {colorPrimary}",
                        BorderRadius = borderRadiusSM,
                        Content = "\"\"",
                    },
                },
                [$"&-in-view{pickerCellCls}-in-range"] = new CSSObject()
                {
                    Position = "relative",
                    ["&::before"] = new CSSObject()
                    {
                        Background = cellActiveWithRangeBg,
                    },
                },
                [$"&-in-view{pickerCellCls}-selected{pickerCellInnerCls},&-in-view{pickerCellCls}-range-start{pickerCellInnerCls},&-in-view{pickerCellCls}-range-end{pickerCellInnerCls}"] = new CSSObject()
                {
                    Color = colorTextLightSolid,
                    Background = colorPrimary,
                },
                [$"&-in-view{pickerCellCls}-range-start:not({pickerCellCls}-range-start-single),&-in-view{pickerCellCls}-range-end:not({pickerCellCls}-range-end-single)"] = new CSSObject()
                {
                    ["&::before"] = new CSSObject()
                    {
                        Background = cellActiveWithRangeBg,
                    },
                },
                [$"&-in-view{pickerCellCls}-range-start::before"] = new CSSObject()
                {
                    InsetInlineStart = "50%",
                },
                [$"&-in-view{pickerCellCls}-range-end::before"] = new CSSObject()
                {
                    InsetInlineEnd = "50%",
                },
                [$"&-in-view{pickerCellCls}-range-hover-start:not({pickerCellCls}-in-range):not({pickerCellCls}-range-start):not({pickerCellCls}-range-end),&-in-view{pickerCellCls}-range-hover-end:not({pickerCellCls}-in-range):not({pickerCellCls}-range-start):not({pickerCellCls}-range-end),&-in-view{pickerCellCls}-range-hover-start{pickerCellCls}-range-start-single,&-in-view{pickerCellCls}-range-hover-start{pickerCellCls}-range-start{pickerCellCls}-range-end{pickerCellCls}-range-end-near-hover,&-in-view{pickerCellCls}-range-hover-end{pickerCellCls}-range-start{pickerCellCls}-range-end{pickerCellCls}-range-start-near-hover,&-in-view{pickerCellCls}-range-hover-end{pickerCellCls}-range-end-single,&-in-view{pickerCellCls}-range-hover:not({pickerCellCls}-in-range)"] = new CSSObject()
                {
                    ["&::after"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = "50%",
                        ZIndex = 0,
                        Height = controlHeightSM,
                        BorderTop = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                        BorderBottom = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                        Transform = "translateY(-50%)",
                        Transition = @$"all {motionDurationSlow}",
                        Content = "\"\"",
                    },
                },
                ["&-range-hover-start::after,&-range-hover-end::after,&-range-hover::after"] = new CSSObject()
                {
                    InsetInlineEnd = 0,
                    InsetInlineStart = pickerCellBorderGap,
                },
                [$"&-in-view{pickerCellCls}-in-range{pickerCellCls}-range-hover::before,&-in-view{pickerCellCls}-in-range{pickerCellCls}-range-hover-start::before,&-in-view{pickerCellCls}-in-range{pickerCellCls}-range-hover-end::before,&-in-view{pickerCellCls}-range-start{pickerCellCls}-range-hover::before,&-in-view{pickerCellCls}-range-end{pickerCellCls}-range-hover::before,&-in-view{pickerCellCls}-range-start:not({pickerCellCls}-range-start-single){pickerCellCls}-range-hover-start::before,&-in-view{pickerCellCls}-range-end:not({pickerCellCls}-range-end-single){pickerCellCls}-range-hover-end::before,{componentCls}-panel>:not({componentCls}-date-panel)&-in-view{pickerCellCls}-in-range{pickerCellCls}-range-hover-start::before,{componentCls}-panel>:not({componentCls}-date-panel)&-in-view{pickerCellCls}-in-range{pickerCellCls}-range-hover-end::before"] = new CSSObject()
                {
                    Background = cellHoverWithRangeBg,
                },
                [$"&-in-view{pickerCellCls}-range-start:not({pickerCellCls}-range-start-single):not({pickerCellCls}-range-end) {pickerCellInnerCls}"] = new CSSObject()
                {
                    BorderStartStartRadius = borderRadiusSM,
                    BorderEndStartRadius = borderRadiusSM,
                    BorderStartEndRadius = 0,
                    BorderEndEndRadius = 0,
                },
                [$"&-in-view{pickerCellCls}-range-end:not({pickerCellCls}-range-end-single):not({pickerCellCls}-range-start) {pickerCellInnerCls}"] = new CSSObject()
                {
                    BorderStartStartRadius = 0,
                    BorderEndStartRadius = 0,
                    BorderStartEndRadius = borderRadiusSM,
                    BorderEndEndRadius = borderRadiusSM,
                },
                [$"&-range-hover{pickerCellCls}-range-end::after"] = new CSSObject()
                {
                    InsetInlineStart = "50%",
                },
                [$"tr>&-in-view{pickerCellCls}-range-hover:first-child::after,tr>&-in-view{pickerCellCls}-range-hover-end:first-child::after,&-in-view{pickerCellCls}-start{pickerCellCls}-range-hover-edge-start{pickerCellCls}-range-hover-edge-start-near-range::after,&-in-view{pickerCellCls}-range-hover-edge-start:not({pickerCellCls}-range-hover-edge-start-near-range)::after,&-in-view{pickerCellCls}-range-hover-start::after"] = new CSSObject()
                {
                    InsetInlineStart = (cellWidth - cellHeight) / 2,
                    BorderInlineStart = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                    BorderStartStartRadius = borderRadiusSM,
                    BorderEndStartRadius = borderRadiusSM,
                },
                [$"tr>&-in-view{pickerCellCls}-range-hover:last-child::after,tr>&-in-view{pickerCellCls}-range-hover-start:last-child::after,&-in-view{pickerCellCls}-end{pickerCellCls}-range-hover-edge-end{pickerCellCls}-range-hover-edge-end-near-range::after,&-in-view{pickerCellCls}-range-hover-edge-end:not({pickerCellCls}-range-hover-edge-end-near-range)::after,&-in-view{pickerCellCls}-range-hover-end::after"] = new CSSObject()
                {
                    InsetInlineEnd = (cellWidth - cellHeight) / 2,
                    BorderInlineEnd = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                    BorderStartEndRadius = borderRadiusSM,
                    BorderEndEndRadius = borderRadiusSM,
                },
                ["&-disabled"] = new CSSObject()
                {
                    Color = colorTextDisabled,
                    PointerEvents = "none",
                    [pickerCellInnerCls] = new CSSObject()
                    {
                        Background = "transparent",
                    },
                    ["&::before"] = new CSSObject()
                    {
                        Background = cellBgDisabled,
                    },
                },
                [$"&-disabled{pickerCellCls}-today {pickerCellInnerCls}::before"] = new CSSObject()
                {
                    BorderColor = colorTextDisabled,
                },
            };
        }

        public static CSSObject GenPanelStyle(PickerPanelToken token)
        {
            var componentCls = token.ComponentCls;
            var pickerCellCls = token.PickerCellCls;
            var pickerCellInnerCls = token.PickerCellInnerCls;
            var pickerYearMonthCellWidth = token.PickerYearMonthCellWidth;
            var pickerControlIconSize = token.PickerControlIconSize;
            var cellWidth = token.CellWidth;
            var paddingSM = token.PaddingSM;
            var paddingXS = token.PaddingXS;
            var paddingXXS = token.PaddingXXS;
            var colorBgContainer = token.ColorBgContainer;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var borderRadiusLG = token.BorderRadiusLG;
            var colorPrimary = token.ColorPrimary;
            var colorTextHeading = token.ColorTextHeading;
            var colorSplit = token.ColorSplit;
            var pickerControlIconBorderWidth = token.PickerControlIconBorderWidth;
            var colorIcon = token.ColorIcon;
            var textHeight = token.TextHeight;
            var motionDurationMid = token.MotionDurationMid;
            var colorIconHover = token.ColorIconHover;
            var fontWeightStrong = token.FontWeightStrong;
            var cellHeight = token.CellHeight;
            var pickerCellPaddingVertical = token.PickerCellPaddingVertical;
            var colorTextDisabled = token.ColorTextDisabled;
            var colorText = token.ColorText;
            var fontSize = token.FontSize;
            var cellHoverWithRangeBg = token.CellHoverWithRangeBg;
            var motionDurationSlow = token.MotionDurationSlow;
            var withoutTimeCellHeight = token.WithoutTimeCellHeight;
            var pickerQuarterPanelContentHeight = token.PickerQuarterPanelContentHeight;
            var colorLink = token.ColorLink;
            var colorLinkActive = token.ColorLinkActive;
            var colorLinkHover = token.ColorLinkHover;
            var cellRangeBorderColor = token.CellRangeBorderColor;
            var borderRadiusSM = token.BorderRadiusSM;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var cellHoverBg = token.CellHoverBg;
            var timeColumnHeight = token.TimeColumnHeight;
            var timeColumnWidth = token.TimeColumnWidth;
            var timeCellHeight = token.TimeCellHeight;
            var controlItemBgActive = token.ControlItemBgActive;
            var marginXXS = token.MarginXXS;
            var pickerDatePanelPaddingHorizontal = token.PickerDatePanelPaddingHorizontal;
            var pickerPanelWidth = cellWidth * 7 + pickerDatePanelPaddingHorizontal * 2;
            var commonHoverCellFixedDistance = (pickerPanelWidth - paddingXS * 2) / 3 - pickerYearMonthCellWidth - paddingSM;
            var quarterHoverCellFixedDistance = (pickerPanelWidth - paddingXS * 2) / 4 - pickerYearMonthCellWidth;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["&-panel"] = new CSSObject()
                    {
                        Display = "inline-flex",
                        FlexDirection = "column",
                        TextAlign = "center",
                        Background = colorBgContainer,
                        Border = @$"{lineWidth}px {lineType} {colorSplit}",
                        BorderRadius = borderRadiusLG,
                        Outline = "none",
                        ["&-focused"] = new CSSObject()
                        {
                            BorderColor = colorPrimary,
                        },
                        ["&-rtl"] = new CSSObject()
                        {
                            Direction = "rtl",
                            [$"{componentCls}-prev-icon,{componentCls}-super-prev-icon"] = new CSSObject()
                            {
                                Transform = "rotate(45deg)",
                            },
                            [$"{componentCls}-next-icon,{componentCls}-super-next-icon"] = new CSSObject()
                            {
                                Transform = "rotate(-135deg)",
                            },
                        },
                    },
                    ["&-decade-panel,&-year-panel,&-quarter-panel,&-month-panel,&-week-panel,&-date-panel,&-time-panel"] = new CSSObject()
                    {
                        Display = "flex",
                        FlexDirection = "column",
                        Width = pickerPanelWidth,
                    },
                    ["&-header"] = new CSSObject()
                    {
                        Display = "flex",
                        Padding = @$"0 {paddingXS}px",
                        Color = colorTextHeading,
                        BorderBottom = @$"{lineWidth}px {lineType} {colorSplit}",
                        ["> *"] = new CSSObject()
                        {
                            Flex = "none",
                        },
                        ["button"] = new CSSObject()
                        {
                            Padding = 0,
                            Color = colorIcon,
                            LineHeight = @$"{textHeight}px",
                            Background = "transparent",
                            Border = 0,
                            Cursor = "pointer",
                            Transition = @$"color {motionDurationMid}",
                            FontSize = "inherit",
                        },
                        ["> button"] = new CSSObject()
                        {
                            MinWidth = "1.6em",
                            FontSize = fontSize,
                            ["&:hover"] = new CSSObject()
                            {
                                Color = colorIconHover,
                            },
                        },
                        ["&-view"] = new CSSObject()
                        {
                            Flex = "auto",
                            FontWeight = fontWeightStrong,
                            LineHeight = @$"{textHeight}px",
                            ["button"] = new CSSObject()
                            {
                                Color = "inherit",
                                FontWeight = "inherit",
                                VerticalAlign = "top",
                                ["&:not(:first-child)"] = new CSSObject()
                                {
                                    MarginInlineStart = paddingXS,
                                },
                                ["&:hover"] = new CSSObject()
                                {
                                    Color = colorPrimary,
                                },
                            },
                        },
                    },
                    ["&-prev-icon,&-next-icon,&-super-prev-icon,&-super-next-icon"] = new CSSObject()
                    {
                        Position = "relative",
                        Display = "inline-block",
                        Width = pickerControlIconSize,
                        Height = pickerControlIconSize,
                        ["&::before"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = 0,
                            InsetInlineStart = 0,
                            Display = "inline-block",
                            Width = pickerControlIconSize,
                            Height = pickerControlIconSize,
                            Border = @$"0 solid currentcolor",
                            BorderBlockStartWidth = pickerControlIconBorderWidth,
                            BorderBlockEndWidth = 0,
                            BorderInlineStartWidth = pickerControlIconBorderWidth,
                            BorderInlineEndWidth = 0,
                            Content = "\"\"",
                        },
                    },
                    ["&-super-prev-icon,&-super-next-icon"] = new CSSObject()
                    {
                        ["&::after"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = Math.Ceiling(pickerControlIconSize / 2),
                            InsetInlineStart = Math.Ceiling(pickerControlIconSize / 2),
                            Display = "inline-block",
                            Width = pickerControlIconSize,
                            Height = pickerControlIconSize,
                            Border = "0 solid currentcolor",
                            BorderBlockStartWidth = pickerControlIconBorderWidth,
                            BorderBlockEndWidth = 0,
                            BorderInlineStartWidth = pickerControlIconBorderWidth,
                            BorderInlineEndWidth = 0,
                            Content = "\"\"",
                        },
                    },
                    ["&-prev-icon,&-super-prev-icon"] = new CSSObject()
                    {
                        Transform = "rotate(-45deg)",
                    },
                    ["&-next-icon,&-super-next-icon"] = new CSSObject()
                    {
                        Transform = "rotate(135deg)",
                    },
                    ["&-content"] = new CSSObject()
                    {
                        Width = "100%",
                        TableLayout = "fixed",
                        BorderCollapse = "collapse",
                        ["th, td"] = new CSSObject()
                        {
                            Position = "relative",
                            MinWidth = cellHeight,
                            FontWeight = "normal",
                        },
                        ["th"] = new CSSObject()
                        {
                            Height = cellHeight + pickerCellPaddingVertical * 2,
                            Color = colorText,
                            VerticalAlign = "middle",
                        },
                    },
                    ["&-cell"] = new CSSObject()
                    {
                        Padding = @$"{pickerCellPaddingVertical}px 0",
                        Color = colorTextDisabled,
                        Cursor = "pointer",
                        ["&-in-view"] = new CSSObject()
                        {
                            Color = colorText,
                        },
                        ["..."] = GenPickerCellInnerStyle(token)
                    },
                    [$"&-date-panel{componentCls}-cell-in-view{componentCls}-cell-in-range{componentCls}-cell-range-hover-start{pickerCellInnerCls},&-date-panel{componentCls}-cell-in-view{componentCls}-cell-in-range{componentCls}-cell-range-hover-end{pickerCellInnerCls}"] = new CSSObject()
                    {
                        ["&::after"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = 0,
                            Bottom = 0,
                            ZIndex = -1,
                            Background = cellHoverWithRangeBg,
                            Transition = @$"all {motionDurationSlow}",
                            Content = "\"\"",
                        },
                    },
                    [$"&-date-panel{componentCls}-cell-in-view{componentCls}-cell-in-range{componentCls}-cell-range-hover-start{pickerCellInnerCls}::after"] = new CSSObject()
                    {
                        InsetInlineEnd = -(cellWidth - cellHeight) / 2,
                        InsetInlineStart = 0,
                    },
                    [$"&-date-panel {componentCls}-cell-in-view{componentCls}-cell-in-range{componentCls}-cell-range-hover-end {pickerCellInnerCls}::after"] = new CSSObject()
                    {
                        InsetInlineEnd = 0,
                        InsetInlineStart = -(cellWidth - cellHeight) / 2,
                    },
                    [$"&-range-hover{componentCls}-range-start::after"] = new CSSObject()
                    {
                        InsetInlineEnd = "50%",
                    },
                    ["&-decade-panel,&-year-panel,&-quarter-panel,&-month-panel"] = new CSSObject()
                    {
                        [$"{componentCls}-content"] = new CSSObject()
                        {
                            Height = withoutTimeCellHeight * 4,
                        },
                        [pickerCellInnerCls] = new CSSObject()
                        {
                            Padding = @$"0 {paddingXS}px",
                        },
                    },
                    ["&-quarter-panel"] = new CSSObject()
                    {
                        [$"{componentCls}-content"] = new CSSObject()
                        {
                            Height = pickerQuarterPanelContentHeight,
                        },
                        [$"{componentCls}-cell-range-hover-start::after"] = new CSSObject()
                        {
                            InsetInlineStart = quarterHoverCellFixedDistance,
                            BorderInlineStart = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                            [$"{componentCls}-panel-rtl &"] = new CSSObject()
                            {
                                InsetInlineEnd = quarterHoverCellFixedDistance,
                                BorderInlineEnd = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                            },
                        },
                        [$"{componentCls}-cell-range-hover-end::after"] = new CSSObject()
                        {
                            InsetInlineEnd = quarterHoverCellFixedDistance,
                            BorderInlineEnd = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                            [$"{componentCls}-panel-rtl &"] = new CSSObject()
                            {
                                InsetInlineStart = quarterHoverCellFixedDistance,
                                BorderInlineStart = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                            },
                        },
                    },
                    [$"&-panel {componentCls}-footer"] = new CSSObject()
                    {
                        BorderTop = @$"{lineWidth}px {lineType} {colorSplit}",
                    },
                    ["&-footer"] = new CSSObject()
                    {
                        Width = "min-content",
                        MinWidth = "100%",
                        LineHeight = @$"{textHeight - 2 * lineWidth}px",
                        TextAlign = "center",
                        ["&-extra"] = new CSSObject()
                        {
                            Padding = @$"0 {paddingSM}px",
                            LineHeight = @$"{textHeight - 2 * lineWidth}px",
                            TextAlign = "start",
                            ["&:not(:last-child)"] = new CSSObject()
                            {
                                BorderBottom = @$"{lineWidth}px {lineType} {colorSplit}",
                            },
                        },
                    },
                    ["&-now"] = new CSSObject()
                    {
                        TextAlign = "start",
                    },
                    ["&-today-btn"] = new CSSObject()
                    {
                        Color = colorLink,
                        ["&:hover"] = new CSSObject()
                        {
                            Color = colorLinkHover,
                        },
                        ["&:active"] = new CSSObject()
                        {
                            Color = colorLinkActive,
                        },
                        [$"&{componentCls}-today-btn-disabled"] = new CSSObject()
                        {
                            Color = colorTextDisabled,
                            Cursor = "not-allowed",
                        },
                    },
                    ["&-decade-panel"] = new CSSObject()
                    {
                        [pickerCellInnerCls] = new CSSObject()
                        {
                            Padding = @$"0 {paddingXS / 2}px",
                        },
                        [$"{componentCls}-cell::before"] = new CSSObject()
                        {
                            Display = "none",
                        },
                    },
                    ["&-year-panel,&-quarter-panel,&-month-panel"] = new CSSObject()
                    {
                        [$"{componentCls}-body"] = new CSSObject()
                        {
                            Padding = @$"0 {paddingXS}px",
                        },
                        [pickerCellInnerCls] = new CSSObject()
                        {
                            Width = pickerYearMonthCellWidth,
                        },
                        [$"{componentCls}-cell-range-hover-start::after"] = new CSSObject()
                        {
                            BorderStartStartRadius = borderRadiusSM,
                            BorderEndStartRadius = borderRadiusSM,
                            BorderStartEndRadius = 0,
                            BorderEndEndRadius = 0,
                            [$"{componentCls}-panel-rtl &"] = new CSSObject()
                            {
                                BorderStartStartRadius = 0,
                                BorderEndStartRadius = 0,
                                BorderStartEndRadius = borderRadiusSM,
                                BorderEndEndRadius = borderRadiusSM,
                            },
                        },
                        [$"{componentCls}-cell-range-hover-end::after"] = new CSSObject()
                        {
                            BorderStartStartRadius = 0,
                            BorderEndStartRadius = 0,
                            BorderStartEndRadius = borderRadiusSM,
                            BorderEndEndRadius = borderRadiusSM,
                            [$"{componentCls}-panel-rtl &"] = new CSSObject()
                            {
                                BorderStartStartRadius = borderRadiusSM,
                                BorderEndStartRadius = borderRadiusSM,
                                BorderStartEndRadius = 0,
                                BorderEndEndRadius = 0,
                            },
                        },
                    },
                    ["&-year-panel,&-month-panel"] = new CSSObject()
                    {
                        [$"{componentCls}-cell-range-hover-start::after"] = new CSSObject()
                        {
                            InsetInlineStart = commonHoverCellFixedDistance,
                            BorderInlineStart = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                            [$"{componentCls}-panel-rtl &"] = new CSSObject()
                            {
                                InsetInlineEnd = commonHoverCellFixedDistance,
                                BorderInlineEnd = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                            },
                        },
                        [$"{componentCls}-cell-range-hover-end::after"] = new CSSObject()
                        {
                            InsetInlineEnd = commonHoverCellFixedDistance,
                            BorderInlineEnd = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                            [$"{componentCls}-panel-rtl &"] = new CSSObject()
                            {
                                InsetInlineStart = commonHoverCellFixedDistance,
                                BorderInlineStart = @$"{lineWidth}px dashed {cellRangeBorderColor}",
                            },
                        },
                    },
                    ["&-week-panel"] = new CSSObject()
                    {
                        [$"{componentCls}-body"] = new CSSObject()
                        {
                            Padding = @$"{paddingXS}px {paddingSM}px",
                        },
                        [$"{componentCls}-cell"] = new CSSObject()
                        {
                            [$"&:hover{pickerCellInnerCls},&-selected{pickerCellInnerCls},{pickerCellInnerCls}"] = new CSSObject()
                            {
                                Background = "transparent !important",
                            },
                        },
                        ["&-row"] = new CSSObject()
                        {
                            ["td"] = new CSSObject()
                            {
                                ["&:before"] = new CSSObject()
                                {
                                    Transition = @$"background {motionDurationMid}",
                                },
                                ["&:first-child:before"] = new CSSObject()
                                {
                                    BorderStartStartRadius = borderRadiusSM,
                                    BorderEndStartRadius = borderRadiusSM,
                                },
                                ["&:last-child:before"] = new CSSObject()
                                {
                                    BorderStartEndRadius = borderRadiusSM,
                                    BorderEndEndRadius = borderRadiusSM,
                                },
                            },
                            ["&:hover td"] = new CSSObject()
                            {
                                ["&:before"] = new CSSObject()
                                {
                                    Background = cellHoverBg,
                                },
                            },
                            ["&-range-starttd,&-range-endtd,&-selectedtd"] = new CSSObject()
                            {
                                [$"&{pickerCellCls}"] = new CSSObject()
                                {
                                    ["&:before"] = new CSSObject()
                                    {
                                        Background = colorPrimary,
                                    },
                                    [$"&{componentCls}-cell-week"] = new CSSObject()
                                    {
                                        Color = new TinyColor(colorTextLightSolid).SetAlpha(0.5).ToHexString(),
                                    },
                                    [pickerCellInnerCls] = new CSSObject()
                                    {
                                        Color = colorTextLightSolid,
                                    },
                                },
                            },
                            ["&-range-hover td:before"] = new CSSObject()
                            {
                                Background = controlItemBgActive,
                            },
                        },
                    },
                    ["&-date-panel"] = new CSSObject()
                    {
                        [$"{componentCls}-body"] = new CSSObject()
                        {
                            Padding = @$"{paddingXS}px {pickerDatePanelPaddingHorizontal}px",
                        },
                        [$"{componentCls}-content"] = new CSSObject()
                        {
                            Width = cellWidth * 7,
                            ["th"] = new CSSObject()
                            {
                                Width = cellWidth,
                                BoxSizing = "border-box",
                                Padding = 0,
                            },
                        },
                    },
                    ["&-datetime-panel"] = new CSSObject()
                    {
                        Display = "flex",
                        [$"{componentCls}-time-panel"] = new CSSObject()
                        {
                            BorderInlineStart = @$"{lineWidth}px {lineType} {colorSplit}",
                        },
                        [$"{componentCls}-date-panel,{componentCls}-time-panel"] = new CSSObject()
                        {
                            Transition = @$"opacity {motionDurationSlow}",
                        },
                        ["&-active"] = new CSSObject()
                        {
                            [$"{componentCls}-date-panel,{componentCls}-time-panel"] = new CSSObject()
                            {
                                Opacity = 0.3f,
                                ["&-active"] = new CSSObject()
                                {
                                    Opacity = 1,
                                },
                            },
                        },
                    },
                    ["&-time-panel"] = new CSSObject()
                    {
                        Width = "auto",
                        MinWidth = "auto",
                        Direction = "ltr",
                        [$"{componentCls}-content"] = new CSSObject()
                        {
                            Display = "flex",
                            Flex = "auto",
                            Height = timeColumnHeight,
                        },
                        ["&-column"] = new CSSObject()
                        {
                            Flex = "1 0 auto",
                            Width = timeColumnWidth,
                            Margin = @$"{paddingXXS}px 0",
                            Padding = 0,
                            OverflowY = "hidden",
                            TextAlign = "start",
                            ListStyle = "none",
                            Transition = @$"background {motionDurationMid}",
                            OverflowX = "hidden",
                            ["&::-webkit-scrollbar"] = new CSSObject()
                            {
                                Width = 8,
                                BackgroundColor = "transparent",
                            },
                            ["&::-webkit-scrollbar-thumb"] = new CSSObject()
                            {
                                BackgroundColor = token.ColorTextTertiary,
                                BorderRadius = 4,
                            },
                            ["&"] = new CSSObject()
                            {
                                ScrollbarWidth = "thin",
                                ScrollbarColor = @$"{token.ColorTextTertiary} transparent",
                            },
                            ["&::after"] = new CSSObject()
                            {
                                Display = "block",
                                Height = timeColumnHeight - timeCellHeight,
                                Content = "\"\"",
                            },
                            ["&:not(:first-child)"] = new CSSObject()
                            {
                                BorderInlineStart = @$"{lineWidth}px {lineType} {colorSplit}",
                            },
                            ["&-active"] = new CSSObject()
                            {
                                Background = new TinyColor(controlItemBgActive).SetAlpha(0.2).ToHexString(),
                            },
                            ["&:hover"] = new CSSObject()
                            {
                                OverflowY = "auto",
                            },
                            ["> li"] = new CSSObject()
                            {
                                Margin = 0,
                                Padding = 0,
                                [$"&{componentCls}-time-panel-cell"] = new CSSObject()
                                {
                                    MarginInline = marginXXS,
                                    [$"{componentCls}-time-panel-cell-inner"] = new CSSObject()
                                    {
                                        Display = "block",
                                        Width = timeColumnWidth - 2 * marginXXS,
                                        Height = timeCellHeight,
                                        Margin = 0,
                                        PaddingBlock = 0,
                                        PaddingInlineEnd = 0,
                                        PaddingInlineStart = (timeColumnWidth - timeCellHeight) / 2,
                                        Color = colorText,
                                        LineHeight = @$"{timeCellHeight}px",
                                        BorderRadius = borderRadiusSM,
                                        Cursor = "pointer",
                                        Transition = @$"background {motionDurationMid}",
                                        ["&:hover"] = new CSSObject()
                                        {
                                            Background = cellHoverBg,
                                        },
                                    },
                                    ["&-selected"] = new CSSObject()
                                    {
                                        [$"{componentCls}-time-panel-cell-inner"] = new CSSObject()
                                        {
                                            Background = controlItemBgActive,
                                        },
                                    },
                                    ["&-disabled"] = new CSSObject()
                                    {
                                        [$"{componentCls}-time-panel-cell-inner"] = new CSSObject()
                                        {
                                            Color = colorTextDisabled,
                                            Background = "transparent",
                                            Cursor = "not-allowed",
                                        },
                                    },
                                },
                            },
                        },
                    },
                    [$"&-datetime-panel {componentCls}-time-panel-column:after"] = new CSSObject()
                    {
                        Height = timeColumnHeight - timeCellHeight + paddingXXS * 2,
                    },
                },
            };
        }

        public static CSSObject GenPickerStatusStyle(PickerToken token)
        {
            var componentCls = token.ComponentCls;
            var colorBgContainer = token.ColorBgContainer;
            var colorError = token.ColorError;
            var errorActiveShadow = token.ErrorActiveShadow;
            var colorWarning = token.ColorWarning;
            var warningActiveShadow = token.WarningActiveShadow;
            var colorErrorHover = token.ColorErrorHover;
            var colorWarningHover = token.ColorWarningHover;
            return new CSSObject()
            {
                [$"{componentCls}:not({componentCls}-disabled):not([disabled])"] = new CSSObject()
                {
                    [$"&{componentCls}-status-error"] = new CSSObject()
                    {
                        BackgroundColor = colorBgContainer,
                        BorderColor = colorError,
                        ["&:hover"] = new CSSObject()
                        {
                            BorderColor = colorErrorHover,
                        },
                        [$"&{componentCls}-focused, &:focus"] = new CSSObject()
                        {
                            ["..."] = GenActiveStyle(
                                MergeToken(
                                    token,
                                    new PickerToken()
                                    {
                                        ActiveBorderColor = colorError,
                                        ActiveShadow = errorActiveShadow,
                                    }))
                        },
                        [$"{componentCls}-active-bar"] = new CSSObject()
                        {
                            Background = colorError,
                        },
                    },
                    [$"&{componentCls}-status-warning"] = new CSSObject()
                    {
                        BackgroundColor = colorBgContainer,
                        BorderColor = colorWarning,
                        ["&:hover"] = new CSSObject()
                        {
                            BorderColor = colorWarningHover,
                        },
                        [$"&{componentCls}-focused, &:focus"] = new CSSObject()
                        {
                            ["..."] = GenActiveStyle(
                                MergeToken(
                                    token,
                                    new PickerToken()
                                    {
                                        ActiveBorderColor = colorWarning,
                                        ActiveShadow = warningActiveShadow,
                                    }))
                        },
                        [$"{componentCls}-active-bar"] = new CSSObject()
                        {
                            Background = colorWarning,
                        },
                    },
                },
            };
        }

        public static CSSInterpolation[] GenPickerStyle(PickerToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var controlHeight = token.ControlHeight;
            var fontSize = token.FontSize;
            var paddingInline = token.PaddingInline;
            var colorBgContainer = token.ColorBgContainer;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var colorBorder = token.ColorBorder;
            var borderRadius = token.BorderRadius;
            var motionDurationMid = token.MotionDurationMid;
            var colorBgContainerDisabled = token.ColorBgContainerDisabled;
            var colorTextDisabled = token.ColorTextDisabled;
            var colorTextPlaceholder = token.ColorTextPlaceholder;
            var controlHeightLG = token.ControlHeightLG;
            var fontSizeLG = token.FontSizeLG;
            var controlHeightSM = token.ControlHeightSM;
            var paddingInlineSM = token.PaddingInlineSM;
            var paddingXS = token.PaddingXS;
            var marginXS = token.MarginXS;
            var colorTextDescription = token.ColorTextDescription;
            var lineWidthBold = token.LineWidthBold;
            var lineHeight = token.LineHeight;
            var colorPrimary = token.ColorPrimary;
            var motionDurationSlow = token.MotionDurationSlow;
            var zIndexPopup = token.ZIndexPopup;
            var paddingXXS = token.PaddingXXS;
            var paddingSM = token.PaddingSM;
            var textHeight = token.TextHeight;
            var cellActiveWithRangeBg = token.CellActiveWithRangeBg;
            var colorPrimaryBorder = token.ColorPrimaryBorder;
            var sizePopupArrow = token.SizePopupArrow;
            var borderRadiusXS = token.BorderRadiusXS;
            var borderRadiusOuter = token.BorderRadiusOuter;
            var colorBgElevated = token.ColorBgElevated;
            var borderRadiusLG = token.BorderRadiusLG;
            var boxShadowSecondary = token.BoxShadowSecondary;
            var borderRadiusSM = token.BorderRadiusSM;
            var colorSplit = token.ColorSplit;
            var cellHoverBg = token.CellHoverBg;
            var presetsWidth = token.PresetsWidth;
            var presetsMaxWidth = token.PresetsMaxWidth;
            var boxShadowPopoverArrow = token.BoxShadowPopoverArrow;
            var colorTextQuaternary = token.ColorTextQuaternary;
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        ["..."] = GenPikerPadding(token, controlHeight, fontSize, paddingInline),
                        Position = "relative",
                        Display = "inline-flex",
                        AlignItems = "center",
                        Background = colorBgContainer,
                        LineHeight = 1,
                        Border = @$"{lineWidth}px {lineType} {colorBorder}",
                        BorderRadius = borderRadius,
                        Transition = @$"border {motionDurationMid}, box-shadow {motionDurationMid}",
                        ["&:hover"] = new CSSObject()
                        {
                            ["..."] = GenHoverStyle(token)
                        },
                        [$"&-focused{componentCls}"] = new CSSObject()
                        {
                            ["..."] = GenActiveStyle(token)
                        },
                        [$"&{componentCls}-disabled"] = new CSSObject()
                        {
                            Background = colorBgContainerDisabled,
                            BorderColor = colorBorder,
                            Cursor = "not-allowed",
                            [$"{componentCls}-suffix"] = new CSSObject()
                            {
                                Color = colorTextQuaternary,
                            },
                        },
                        [$"&{componentCls}-borderless"] = new CSSObject()
                        {
                            BackgroundColor = "transparent !important",
                            BorderColor = "transparent !important",
                            BoxShadow = "none !important",
                        },
                        [$"{componentCls}-input"] = new CSSObject()
                        {
                            Position = "relative",
                            Display = "inline-flex",
                            AlignItems = "center",
                            Width = "100%",
                            ["> input"] = new CSSObject()
                            {
                                ["..."] = GenBasicInputStyle(token),
                                Flex = "auto",
                                MinWidth = 1,
                                Height = "auto",
                                Padding = 0,
                                Background = "transparent",
                                Border = 0,
                                BorderRadius = 0,
                                FontFamily = "inherit",
                                ["&:focus"] = new CSSObject()
                                {
                                    BoxShadow = "none",
                                },
                                ["&[disabled]"] = new CSSObject()
                                {
                                    Background = "transparent",
                                },
                            },
                            ["&:hover"] = new CSSObject()
                            {
                                [$"{componentCls}-clear"] = new CSSObject()
                                {
                                    Opacity = 1,
                                },
                            },
                            ["&-placeholder"] = new CSSObject()
                            {
                                ["> input"] = new CSSObject()
                                {
                                    Color = colorTextPlaceholder,
                                },
                            },
                        },
                        ["&-large"] = new CSSObject()
                        {
                            ["..."] = GenPikerPadding(token, controlHeightLG, fontSizeLG, paddingInline),
                            [$"{componentCls}-input > input"] = new CSSObject()
                            {
                                FontSize = fontSizeLG,
                            },
                        },
                        ["&-small"] = new CSSObject()
                        {
                            ["..."] = GenPikerPadding(token, controlHeightSM, fontSize, paddingInlineSM)
                        },
                        [$"{componentCls}-suffix"] = new CSSObject()
                        {
                            Display = "flex",
                            Flex = "none",
                            AlignSelf = "center",
                            MarginInlineStart = paddingXS / 2,
                            Color = colorTextDisabled,
                            LineHeight = 1,
                            PointerEvents = "none",
                            ["> *"] = new CSSObject()
                            {
                                VerticalAlign = "top",
                                ["&:not(:last-child)"] = new CSSObject()
                                {
                                    MarginInlineEnd = marginXS,
                                },
                            },
                        },
                        [$"{componentCls}-clear"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = "50%",
                            InsetInlineEnd = 0,
                            Color = colorTextDisabled,
                            LineHeight = 1,
                            Background = colorBgContainer,
                            Transform = "translateY(-50%)",
                            Cursor = "pointer",
                            Opacity = 0,
                            Transition = @$"opacity {motionDurationMid}, color {motionDurationMid}",
                            ["> *"] = new CSSObject()
                            {
                                VerticalAlign = "top",
                            },
                            ["&:hover"] = new CSSObject()
                            {
                                Color = colorTextDescription,
                            },
                        },
                        [$"{componentCls}-separator"] = new CSSObject()
                        {
                            Position = "relative",
                            Display = "inline-block",
                            Width = "1em",
                            Height = fontSizeLG,
                            Color = colorTextDisabled,
                            FontSize = fontSizeLG,
                            VerticalAlign = "top",
                            Cursor = "default",
                            [$"{componentCls}-focused &"] = new CSSObject()
                            {
                                Color = colorTextDescription,
                            },
                            [$"{componentCls}-range-separator &"] = new CSSObject()
                            {
                                [$"{componentCls}-disabled &"] = new CSSObject()
                                {
                                    Cursor = "not-allowed",
                                },
                            },
                        },
                        ["&-range"] = new CSSObject()
                        {
                            Position = "relative",
                            Display = "inline-flex",
                            [$"{componentCls}-clear"] = new CSSObject()
                            {
                                InsetInlineEnd = paddingInline,
                            },
                            ["&:hover"] = new CSSObject()
                            {
                                [$"{componentCls}-clear"] = new CSSObject()
                                {
                                    Opacity = 1,
                                },
                            },
                            [$"{componentCls}-active-bar"] = new CSSObject()
                            {
                                Bottom = -lineWidth,
                                Height = lineWidthBold,
                                MarginInlineStart = paddingInline,
                                Background = colorPrimary,
                                Opacity = 0,
                                Transition = @$"all {motionDurationSlow} ease-out",
                                PointerEvents = "none",
                            },
                            [$"&{componentCls}-focused"] = new CSSObject()
                            {
                                [$"{componentCls}-active-bar"] = new CSSObject()
                                {
                                    Opacity = 1,
                                },
                            },
                            [$"{componentCls}-range-separator"] = new CSSObject()
                            {
                                AlignItems = "center",
                                Padding = @$"0 {paddingXS}px",
                                LineHeight = 1,
                            },
                            [$"&{componentCls}-small"] = new CSSObject()
                            {
                                [$"{componentCls}-clear"] = new CSSObject()
                                {
                                    InsetInlineEnd = paddingInlineSM,
                                },
                                [$"{componentCls}-active-bar"] = new CSSObject()
                                {
                                    MarginInlineStart = paddingInlineSM,
                                },
                            },
                        },
                        ["&-dropdown"] = new CSSObject()
                        {
                            ["..."] = ResetComponent(token),
                            ["..."] = GenPanelStyle(token),
                            Position = "absolute",
                            Top = -9999,
                            Left = new PropertySkip()
                            {
                                SkipCheck = true,
                                Value = -9999,
                            },
                            ZIndex = zIndexPopup,
                            [$"&{componentCls}-dropdown-hidden"] = new CSSObject()
                            {
                                Display = "none",
                            },
                            [$"&{componentCls}-dropdown-placement-bottomLeft"] = new CSSObject()
                            {
                                [$"{componentCls}-range-arrow"] = new CSSObject()
                                {
                                    Top = 0,
                                    Display = "block",
                                    Transform = "translateY(-100%)",
                                },
                            },
                            [$"&{componentCls}-dropdown-placement-topLeft"] = new CSSObject()
                            {
                                [$"{componentCls}-range-arrow"] = new CSSObject()
                                {
                                    Bottom = 0,
                                    Display = "block",
                                    Transform = "translateY(100%) rotate(180deg)",
                                },
                            },
                            [$"&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-dropdown-placement-topLeft,&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-dropdown-placement-topRight,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-dropdown-placement-topLeft,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-dropdown-placement-topRight"] = new CSSObject()
                            {
                                AnimationName = SlideDownIn,
                            },
                            [$"&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-dropdown-placement-bottomLeft,&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-dropdown-placement-bottomRight,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-dropdown-placement-bottomLeft,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-dropdown-placement-bottomRight"] = new CSSObject()
                            {
                                AnimationName = SlideUpIn,
                            },
                            [$"&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-dropdown-placement-topLeft,&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-dropdown-placement-topRight"] = new CSSObject()
                            {
                                AnimationName = SlideDownOut,
                            },
                            [$"&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-dropdown-placement-bottomLeft,&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-dropdown-placement-bottomRight"] = new CSSObject()
                            {
                                AnimationName = SlideUpOut,
                            },
                            [$"{componentCls}-panel > {componentCls}-time-panel"] = new CSSObject()
                            {
                                PaddingTop = paddingXXS,
                            },
                            [$"{componentCls}-ranges"] = new CSSObject()
                            {
                                MarginBottom = 0,
                                Padding = @$"{paddingXXS}px {paddingSM}px",
                                Overflow = "hidden",
                                LineHeight = @$"{textHeight - 2 * lineWidth - paddingXS / 2}px",
                                TextAlign = "start",
                                ListStyle = "none",
                                Display = "flex",
                                JustifyContent = "space-between",
                                ["> li"] = new CSSObject()
                                {
                                    Display = "inline-block",
                                },
                                [$"{componentCls}-preset > {antCls}-tag-blue"] = new CSSObject()
                                {
                                    Color = colorPrimary,
                                    Background = cellActiveWithRangeBg,
                                    BorderColor = colorPrimaryBorder,
                                    Cursor = "pointer",
                                },
                                [$"{componentCls}-ok"] = new CSSObject()
                                {
                                    MarginInlineStart = "auto",
                                },
                            },
                            [$"{componentCls}-range-wrapper"] = new CSSObject()
                            {
                                Display = "flex",
                                Position = "relative",
                            },
                            [$"{componentCls}-range-arrow"] = new CSSObject()
                            {
                                Position = "absolute",
                                ZIndex = 1,
                                Display = "none",
                                MarginInlineStart = paddingInline * 1.5,
                                Transition = @$"left {motionDurationSlow} ease-out",
                                ["..."] = RoundedArrow(sizePopupArrow, borderRadiusXS, borderRadiusOuter, colorBgElevated, boxShadowPopoverArrow)
                            },
                            [$"{componentCls}-panel-container"] = new CSSObject()
                            {
                                Overflow = "hidden",
                                VerticalAlign = "top",
                                Background = colorBgElevated,
                                BorderRadius = borderRadiusLG,
                                BoxShadow = boxShadowSecondary,
                                Transition = @$"margin {motionDurationSlow}",
                                [$"{componentCls}-panel-layout"] = new CSSObject()
                                {
                                    Display = "flex",
                                    FlexWrap = "nowrap",
                                    AlignItems = "stretch",
                                },
                                [$"{componentCls}-presets"] = new CSSObject()
                                {
                                    Display = "flex",
                                    FlexDirection = "column",
                                    MinWidth = presetsWidth,
                                    MaxWidth = presetsMaxWidth,
                                    ["ul"] = new CSSObject()
                                    {
                                        Height = 0,
                                        Flex = "auto",
                                        ListStyle = "none",
                                        Overflow = "auto",
                                        Margin = 0,
                                        Padding = paddingXS,
                                        BorderInlineEnd = @$"{lineWidth}px {lineType} {colorSplit}",
                                        ["li"] = new CSSObject()
                                        {
                                            ["..."] = TextEllipsis,
                                            BorderRadius = borderRadiusSM,
                                            PaddingInline = paddingXS,
                                            PaddingBlock = (controlHeightSM - Math.Round(fontSize * lineHeight)) / 2,
                                            Cursor = "pointer",
                                            Transition = @$"all {motionDurationSlow}",
                                            ["+ li"] = new CSSObject()
                                            {
                                                MarginTop = marginXS,
                                            },
                                            ["&:hover"] = new CSSObject()
                                            {
                                                Background = cellHoverBg,
                                            },
                                        },
                                    },
                                },
                                [$"{componentCls}-panels"] = new CSSObject()
                                {
                                    Display = "inline-flex",
                                    FlexWrap = "nowrap",
                                    Direction = "ltr",
                                    [$"{componentCls}-panel"] = new CSSObject()
                                    {
                                        BorderWidth = @$"0 0 {lineWidth}px",
                                    },
                                    ["&:last-child"] = new CSSObject()
                                    {
                                        [$"{componentCls}-panel"] = new CSSObject()
                                        {
                                            BorderWidth = 0,
                                        },
                                    },
                                },
                                [$"{componentCls}-panel"] = new CSSObject()
                                {
                                    VerticalAlign = "top",
                                    Background = "transparent",
                                    BorderRadius = 0,
                                    BorderWidth = 0,
                                    [$"{componentCls}-content,table"] = new CSSObject()
                                    {
                                        TextAlign = "center",
                                    },
                                    ["&-focused"] = new CSSObject()
                                    {
                                        BorderColor = colorBorder,
                                    },
                                },
                            },
                        },
                        ["&-dropdown-range"] = new CSSObject()
                        {
                            Padding = @$"{(sizePopupArrow * 2) / 3}px 0",
                            ["&-hidden"] = new CSSObject()
                            {
                                Display = "none",
                            },
                        },
                        ["&-rtl"] = new CSSObject()
                        {
                            Direction = "rtl",
                            [$"{componentCls}-separator"] = new CSSObject()
                            {
                                Transform = "rotate(180deg)",
                            },
                            [$"{componentCls}-footer"] = new CSSObject()
                            {
                                ["&-extra"] = new CSSObject()
                                {
                                    Direction = "rtl",
                                },
                            },
                        },
                    },
                },
                InitSlideMotion(token, "slide-up"),
                InitSlideMotion(token, "slide-down"),
                InitMoveMotion(token, "move-up"),
                InitMoveMotion(token, "move-down"),
            };
        }

        public static PickerToken InitPickerPanelToken(TokenWithCommonCls token)
        {
            var componentCls = token.ComponentCls;
            var controlHeightLG = token.ControlHeightLG;
            var paddingXXS = token.PaddingXXS;
            var padding = token.Padding;
            return new PickerToken()
            {
                PickerCellCls = @$"{componentCls}-cell",
                PickerCellInnerCls = @$"{componentCls}-cell-inner",
                PickerYearMonthCellWidth = controlHeightLG * 1.5,
                PickerQuarterPanelContentHeight = controlHeightLG * 1.4,
                PickerCellPaddingVertical = paddingXXS + paddingXXS / 2,
                PickerCellBorderGap = 2,
                PickerControlIconSize = 7,
                PickerControlIconBorderWidth = 1.5f,
                PickerDatePanelPaddingHorizontal = padding + paddingXXS / 2,
            };
        }

        public static PickerToken InitPanelComponentToken(GlobalToken token)
        {
            return new PickerToken()
            {
                CellHoverBg = token.ControlItemBgHover,
                CellActiveWithRangeBg = token.ControlItemBgActive,
                CellHoverWithRangeBg = new TinyColor(token.ColorPrimary).Lighten(35).ToHexString(),
                CellRangeBorderColor = new TinyColor(token.ColorPrimary).Lighten(20).ToHexString(),
                CellBgDisabled = token.ColorBgContainerDisabled,
                TimeColumnWidth = token.ControlHeightLG * 1.4,
                TimeColumnHeight = 28 * 8,
                TimeCellHeight = 28,
                CellWidth = token.ControlHeightSM * 1.5,
                CellHeight = token.ControlHeightSM,
                TextHeight = token.ControlHeightLG,
                WithoutTimeCellHeight = token.ControlHeightLG * 1.65,
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "DatePicker",
                (token) =>
                {
                    var pickerToken = MergeToken(
                        InitInputToken(token),
                        InitPickerPanelToken(token));
                    return new CSSInterpolation[]
                    {
                        GenPickerStyle(pickerToken),
                        GenPickerStatusStyle(pickerToken),
                        GenCompactItemStyle(
                            token,
                            new CompactItemOptions()
                            {
                                FocusElCls = @$"{token.ComponentCls}-focused",
                            }),
                    };
                },
                (token) =>
                {
                    return new PickerToken()
                    {
                        ["..."] = InitComponentToken(token),
                        ["..."] = InitPanelComponentToken(token),
                        PresetsWidth = 120,
                        PresetsMaxWidth = 200,
                        ZIndexPopup = token.ZIndexPopupBase + 50,
                    };
                });
        }
    }
}
