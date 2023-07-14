using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class DatePickerToken : TokenWithCommonCls
    {
        public int PresetsWidth { get; set; }

        public int PresetsMaxWidth { get; set; }

        public int ZIndexPopup { get; set; }

    }

    public class PickerPanelToken
    {
    }

    public class PickerToken
    {
    }

    public class SharedPickerToken
    {
    }

    public partial class DatePicker
    {
        public CSSObject GenPikerPadding(PickerToken token, int inputHeight, int fontSize, int paddingHorizontal)
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

        public CSSObject GenPickerCellInnerStyle(SharedPickerToken token)
        {
            var componentCls = token.ComponentCls;
            var pickerCellCls = token.PickerCellCls;
            var pickerCellInnerCls = token.PickerCellInnerCls;
            var pickerPanelCellHeight = token.PickerPanelCellHeight;
            var motionDurationSlow = token.MotionDurationSlow;
            var borderRadiusSM = token.BorderRadiusSM;
            var motionDurationMid = token.MotionDurationMid;
            var controlItemBgHover = token.ControlItemBgHover;
            var lineWidth = token.LineWidth;
            var lineType = token.LineType;
            var colorPrimary = token.ColorPrimary;
            var controlItemBgActive = token.ControlItemBgActive;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var controlHeightSM = token.ControlHeightSM;
            var pickerDateHoverRangeBorderColor = token.PickerDateHoverRangeBorderColor;
            var pickerCellBorderGap = token.PickerCellBorderGap;
            var pickerBasicCellHoverWithRangeColor = token.PickerBasicCellHoverWithRangeColor;
            var pickerPanelCellWidth = token.PickerPanelCellWidth;
            var colorTextDisabled = token.ColorTextDisabled;
            var colorBgContainerDisabled = token.ColorBgContainerDisabled;
            return new CSSObject()
            {
                ["&::before"] = new CSSObject()
                {
                    Position = "absolute",
                    Top = "50%",
                    InsetInlineStart = 0,
                    InsetInlineEnd = 0,
                    ZIndex = 1,
                    Height = pickerPanelCellHeight,
                    Transform = "translateY(-50%)",
                    Transition = @$"all {motionDurationSlow}",
                    Content = "\"\"",
                },
                [pickerCellInnerCls] = new CSSObject()
                {
                    Position = "relative",
                    ZIndex = 2,
                    Display = "inline-block",
                    MinWidth = pickerPanelCellHeight,
                    Height = pickerPanelCellHeight,
                    LineHeight = @$"{pickerPanelCellHeight}px",
                    BorderRadius = borderRadiusSM,
                    Transition = @$"background {motionDurationMid}, border {motionDurationMid}",
                },
                [$"&:hover:not({pickerCellCls}-in-view),&:hover:not({pickerCellCls}-selected):not({pickerCellCls}-range-start):not({pickerCellCls}-range-end):not({pickerCellCls}-range-hover-start):not({pickerCellCls}-range-hover-end)"] = new CSSObject()
                {
                    [pickerCellInnerCls] = new CSSObject()
                    {
                        Background = controlItemBgHover,
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
                        Background = controlItemBgActive,
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
                        Background = controlItemBgActive,
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
                        BorderTop = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
                        BorderBottom = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
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
                    Background = pickerBasicCellHoverWithRangeColor,
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
                    InsetInlineStart = (pickerPanelCellWidth - pickerPanelCellHeight) / 2,
                    BorderInlineStart = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
                    BorderStartStartRadius = lineWidth,
                    BorderEndStartRadius = lineWidth,
                },
                [$"tr>&-in-view{pickerCellCls}-range-hover:last-child::after,tr>&-in-view{pickerCellCls}-range-hover-start:last-child::after,&-in-view{pickerCellCls}-end{pickerCellCls}-range-hover-edge-end{pickerCellCls}-range-hover-edge-end-near-range::after,&-in-view{pickerCellCls}-range-hover-edge-end:not({pickerCellCls}-range-hover-edge-end-near-range)::after,&-in-view{pickerCellCls}-range-hover-end::after"] = new CSSObject()
                {
                    InsetInlineEnd = (pickerPanelCellWidth - pickerPanelCellHeight) / 2,
                    BorderInlineEnd = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
                    BorderStartEndRadius = lineWidth,
                    BorderEndEndRadius = lineWidth,
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
                        Background = colorBgContainerDisabled,
                    },
                },
                [$"&-disabled{pickerCellCls}-today {pickerCellInnerCls}::before"] = new CSSObject()
                {
                    BorderColor = colorTextDisabled,
                },
            };
        }

        public CSSObject GenPanelStyle(SharedPickerToken token)
        {
            var componentCls = token.ComponentCls;
            var pickerCellCls = token.PickerCellCls;
            var pickerCellInnerCls = token.PickerCellInnerCls;
            var pickerYearMonthCellWidth = token.PickerYearMonthCellWidth;
            var pickerControlIconSize = token.PickerControlIconSize;
            var pickerPanelCellWidth = token.PickerPanelCellWidth;
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
            var pickerTextHeight = token.PickerTextHeight;
            var motionDurationMid = token.MotionDurationMid;
            var colorIconHover = token.ColorIconHover;
            var fontWeightStrong = token.FontWeightStrong;
            var pickerPanelCellHeight = token.PickerPanelCellHeight;
            var pickerCellPaddingVertical = token.PickerCellPaddingVertical;
            var colorTextDisabled = token.ColorTextDisabled;
            var colorText = token.ColorText;
            var fontSize = token.FontSize;
            var pickerBasicCellHoverWithRangeColor = token.PickerBasicCellHoverWithRangeColor;
            var motionDurationSlow = token.MotionDurationSlow;
            var pickerPanelWithoutTimeCellHeight = token.PickerPanelWithoutTimeCellHeight;
            var pickerQuarterPanelContentHeight = token.PickerQuarterPanelContentHeight;
            var colorLink = token.ColorLink;
            var colorLinkActive = token.ColorLinkActive;
            var colorLinkHover = token.ColorLinkHover;
            var pickerDateHoverRangeBorderColor = token.PickerDateHoverRangeBorderColor;
            var borderRadiusSM = token.BorderRadiusSM;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var controlItemBgHover = token.ControlItemBgHover;
            var pickerTimePanelColumnHeight = token.PickerTimePanelColumnHeight;
            var pickerTimePanelColumnWidth = token.PickerTimePanelColumnWidth;
            var pickerTimePanelCellHeight = token.PickerTimePanelCellHeight;
            var controlItemBgActive = token.ControlItemBgActive;
            var marginXXS = token.MarginXXS;
            var pickerDatePanelPaddingHorizontal = token.PickerDatePanelPaddingHorizontal;
            var pickerPanelWidth = pickerPanelCellWidth * 7 + pickerDatePanelPaddingHorizontal * 2;
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
                            LineHeight = @$"{pickerTextHeight}px",
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
                            LineHeight = @$"{pickerTextHeight}px",
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
                            Top = Math.Ceil(pickerControlIconSize / 2),
                            InsetInlineStart = Math.Ceil(pickerControlIconSize / 2),
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
                            MinWidth = pickerPanelCellHeight,
                            FontWeight = "normal",
                        },
                        ["th"] = new CSSObject()
                        {
                            Height = pickerPanelCellHeight + pickerCellPaddingVertical * 2,
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
                            Background = pickerBasicCellHoverWithRangeColor,
                            Transition = @$"all {motionDurationSlow}",
                            Content = "\"\"",
                        },
                    },
                    [$"&-date-panel{componentCls}-cell-in-view{componentCls}-cell-in-range{componentCls}-cell-range-hover-start{pickerCellInnerCls}::after"] = new CSSObject()
                    {
                        InsetInlineEnd = -(pickerPanelCellWidth - pickerPanelCellHeight) / 2,
                        InsetInlineStart = 0,
                    },
                    [$"&-date-panel {componentCls}-cell-in-view{componentCls}-cell-in-range{componentCls}-cell-range-hover-end {pickerCellInnerCls}::after"] = new CSSObject()
                    {
                        InsetInlineEnd = 0,
                        InsetInlineStart = -(pickerPanelCellWidth - pickerPanelCellHeight) / 2,
                    },
                    [$"&-range-hover{componentCls}-range-start::after"] = new CSSObject()
                    {
                        InsetInlineEnd = "50%",
                    },
                    ["&-decade-panel,&-year-panel,&-quarter-panel,&-month-panel"] = new CSSObject()
                    {
                        [$"{componentCls}-content"] = new CSSObject()
                        {
                            Height = pickerPanelWithoutTimeCellHeight * 4,
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
                            BorderInlineStart = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
                            [$"{componentCls}-panel-rtl &"] = new CSSObject()
                            {
                                InsetInlineEnd = quarterHoverCellFixedDistance,
                                BorderInlineEnd = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
                            },
                        },
                        [$"{componentCls}-cell-range-hover-end::after"] = new CSSObject()
                        {
                            InsetInlineEnd = quarterHoverCellFixedDistance,
                            BorderInlineEnd = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
                            [$"{componentCls}-panel-rtl &"] = new CSSObject()
                            {
                                InsetInlineStart = quarterHoverCellFixedDistance,
                                BorderInlineStart = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
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
                        LineHeight = @$"{pickerTextHeight - 2 * lineWidth}px",
                        TextAlign = "center",
                        ["&-extra"] = new CSSObject()
                        {
                            Padding = @$"0 {paddingSM}",
                            LineHeight = @$"{pickerTextHeight - 2 * lineWidth}px",
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
                            BorderInlineStart = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
                            [$"{componentCls}-panel-rtl &"] = new CSSObject()
                            {
                                InsetInlineEnd = commonHoverCellFixedDistance,
                                BorderInlineEnd = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
                            },
                        },
                        [$"{componentCls}-cell-range-hover-end::after"] = new CSSObject()
                        {
                            InsetInlineEnd = commonHoverCellFixedDistance,
                            BorderInlineEnd = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
                            [$"{componentCls}-panel-rtl &"] = new CSSObject()
                            {
                                InsetInlineStart = commonHoverCellFixedDistance,
                                BorderInlineStart = @$"{lineWidth}px dashed {pickerDateHoverRangeBorderColor}",
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
                                    Background = controlItemBgHover,
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
                                        Color = New TinyColor(colorTextLightSolid).setAlpha(0.5).toHexString()
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
                            Width = pickerPanelCellWidth * 7,
                            ["th"] = new CSSObject()
                            {
                                Width = pickerPanelCellWidth,
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
                            Height = pickerTimePanelColumnHeight,
                        },
                        ["&-column"] = new CSSObject()
                        {
                            Flex = "1 0 auto",
                            Width = pickerTimePanelColumnWidth,
                            Margin = @$"{paddingXXS}px 0",
                            Padding = 0,
                            OverflowY = "hidden",
                            TextAlign = "start",
                            ListStyle = "none",
                            Transition = @$"background {motionDurationMid}",
                            OverflowX = "hidden",
                            ["&::after"] = new CSSObject()
                            {
                                Display = "block",
                                Height = pickerTimePanelColumnHeight - pickerTimePanelCellHeight,
                                Content = "\"\"",
                            },
                            ["&:not(:first-child)"] = new CSSObject()
                            {
                                BorderInlineStart = @$"{lineWidth}px {lineType} {colorSplit}",
                            },
                            ["&-active"] = new CSSObject()
                            {
                                Background = New TinyColor(controlItemBgActive).setAlpha(0.2).toHexString()
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
                                        Width = pickerTimePanelColumnWidth - 2 * marginXXS,
                                        Height = pickerTimePanelCellHeight,
                                        Margin = 0,
                                        PaddingBlock = 0,
                                        PaddingInlineEnd = 0,
                                        PaddingInlineStart = (pickerTimePanelColumnWidth - pickerTimePanelCellHeight) / 2,
                                        Color = colorText,
                                        LineHeight = @$"{pickerTimePanelCellHeight}px",
                                        BorderRadius = borderRadiusSM,
                                        Cursor = "pointer",
                                        Transition = @$"background {motionDurationMid}",
                                        ["&:hover"] = new CSSObject()
                                        {
                                            Background = controlItemBgHover,
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
                        Height = pickerTimePanelColumnHeight - pickerTimePanelCellHeight + paddingXXS * 2,
                    },
                },
            };
        }

        public CSSObject GenPickerStatusStyle(DatePickerToken token)
        {
            var componentCls = token.ComponentCls;
            var colorBgContainer = token.ColorBgContainer;
            var colorError = token.ColorError;
            var colorErrorOutline = token.ColorErrorOutline;
            var colorWarning = token.ColorWarning;
            var colorWarningOutline = token.ColorWarningOutline;
            return new CSSObject()
            {
                [$"{componentCls}:not({componentCls}-disabled)"] = new CSSObject()
                {
                    [$"&{componentCls}-status-error"] = new CSSObject()
                    {
                        ["&, &:not([disabled]):hover"] = new CSSObject()
                        {
                            BackgroundColor = colorBgContainer,
                            BorderColor = colorError,
                        },
                        [$"&{componentCls}-focused, &:focus"] = new CSSObject()
                        {
                            ["..."] = GenActiveStyle(mergeToken<PickerToken>(token, {
              inputBorderActiveColor: colorError,
              inputBorderHoverColor: colorError,
              controlOutline: colorErrorOutline,
            }))
                        },
                        [$"{componentCls}-active-bar"] = new CSSObject()
                        {
                            Background = colorError,
                        },
                    },
                    [$"&{componentCls}-status-warning"] = new CSSObject()
                    {
                        ["&, &:not([disabled]):hover"] = new CSSObject()
                        {
                            BackgroundColor = colorBgContainer,
                            BorderColor = colorWarning,
                        },
                        [$"&{componentCls}-focused, &:focus"] = new CSSObject()
                        {
                            ["..."] = GenActiveStyle(mergeToken<PickerToken>(token, {
              inputBorderActiveColor: colorWarning,
              inputBorderHoverColor: colorWarning,
              controlOutline: colorWarningOutline,
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

        public CSSObject[] GenPickerStyle(DatePickerToken token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var controlHeight = token.ControlHeight;
            var fontSize = token.FontSize;
            var inputPaddingHorizontal = token.InputPaddingHorizontal;
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
            var inputPaddingHorizontalSM = token.InputPaddingHorizontalSM;
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
            var pickerTextHeight = token.PickerTextHeight;
            var controlItemBgActive = token.ControlItemBgActive;
            var colorPrimaryBorder = token.ColorPrimaryBorder;
            var sizePopupArrow = token.SizePopupArrow;
            var borderRadiusXS = token.BorderRadiusXS;
            var borderRadiusOuter = token.BorderRadiusOuter;
            var colorBgElevated = token.ColorBgElevated;
            var borderRadiusLG = token.BorderRadiusLG;
            var boxShadowSecondary = token.BoxShadowSecondary;
            var borderRadiusSM = token.BorderRadiusSM;
            var colorSplit = token.ColorSplit;
            var controlItemBgHover = token.ControlItemBgHover;
            var presetsWidth = token.PresetsWidth;
            var presetsMaxWidth = token.PresetsMaxWidth;
            var boxShadowPopoverArrow = token.BoxShadowPopoverArrow;
            return new CSSObject[]
            {
                new CSSObject()
                {
                    [componentCls] = new CSSObject()
                    {
                        ["..."] = ResetComponent(token),
                        ["..."] = GenPikerPadding(token, controlHeight, fontSize, inputPaddingHorizontal),
                        Position = "relative",
                        Display = "inline-flex",
                        AlignItems = "center",
                        Background = colorBgContainer,
                        LineHeight = 1,
                        Border = @$"{lineWidth}px {lineType} {colorBorder}",
                        BorderRadius = borderRadius,
                        Transition = @$"border {motionDurationMid}, box-shadow {motionDurationMid}",
                        ["&:hover, &-focused"] = new CSSObject()
                        {
                            ["..."] = GenHoverStyle(token)
                        },
                        ["&-focused"] = new CSSObject()
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
                                Color = colorTextDisabled,
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
                            ["..."] = GenPikerPadding(token, controlHeightLG, fontSizeLG, inputPaddingHorizontal),
                            [$"{componentCls}-input > input"] = new CSSObject()
                            {
                                FontSize = fontSizeLG,
                            },
                        },
                        ["&-small"] = new CSSObject()
                        {
                            ["..."] = GenPikerPadding(token, controlHeightSM, fontSize, inputPaddingHorizontalSM)
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
                                InsetInlineEnd = inputPaddingHorizontal,
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
                                MarginInlineStart = inputPaddingHorizontal,
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
                                    InsetInlineEnd = inputPaddingHorizontalSM,
                                },
                                [$"{componentCls}-active-bar"] = new CSSObject()
                                {
                                    MarginInlineStart = inputPaddingHorizontalSM,
                                },
                            },
                        },
                        ["&-dropdown"] = new CSSObject()
                        {
                            ["..."] = ResetComponent(token),
                            ["..."] = GenPanelStyle(token),
                            Position = "absolute",
                            Top = -9999,
                            Left = new CSSObject()
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
                                AnimationName = slideDownIn,
                            },
                            [$"&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-dropdown-placement-bottomLeft,&{antCls}-slide-up-enter{antCls}-slide-up-enter-active{componentCls}-dropdown-placement-bottomRight,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-dropdown-placement-bottomLeft,&{antCls}-slide-up-appear{antCls}-slide-up-appear-active{componentCls}-dropdown-placement-bottomRight"] = new CSSObject()
                            {
                                AnimationName = slideUpIn,
                            },
                            [$"&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-dropdown-placement-topLeft,&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-dropdown-placement-topRight"] = new CSSObject()
                            {
                                AnimationName = slideDownOut,
                            },
                            [$"&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-dropdown-placement-bottomLeft,&{antCls}-slide-up-leave{antCls}-slide-up-leave-active{componentCls}-dropdown-placement-bottomRight"] = new CSSObject()
                            {
                                AnimationName = slideUpOut,
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
                                LineHeight = @$"{pickerTextHeight - 2 * lineWidth - paddingXS / 2}px",
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
                                    Background = controlItemBgActive,
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
                                MarginInlineStart = inputPaddingHorizontal * 1.5,
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
                                                Background = controlItemBgHover,
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
                InitMoveMotion(token, "move-down")
            };
        }

        public PickerPanelToken InitPickerPanelToken(TokenWithCommonCls<GlobalToken> token)
        {
            var pickerTimePanelCellHeight = 28;
            var componentCls = token.ComponentCls;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeightSM = token.ControlHeightSM;
            var colorPrimary = token.ColorPrimary;
            var paddingXXS = token.PaddingXXS;
            var padding = token.Padding;
            return new PickerPanelToken()
            {
                PickerCellCls = @$"{componentCls}-cell",
                PickerCellInnerCls = @$"{componentCls}-cell-inner",
                PickerTextHeight = controlHeightLG,
                PickerPanelCellWidth = controlHeightSM * 1.5,
                PickerPanelCellHeight = controlHeightSM,
                PickerDateHoverRangeBorderColor = New TinyColor(colorPrimary).lighten(20).toHexString(),
                PickerBasicCellHoverWithRangeColor = New TinyColor(colorPrimary).lighten(35).toHexString(),
                PickerPanelWithoutTimeCellHeight = controlHeightLG * 1.65,
                PickerYearMonthCellWidth = controlHeightLG * 1.5,
                PickerTimePanelColumnHeight = pickerTimePanelCellHeight * 8,
                PickerTimePanelColumnWidth = controlHeightLG * 1.4,
                PickerTimePanelCellHeight = pickerTimePanelCellHeight,
                PickerQuarterPanelContentHeight = controlHeightLG * 1.4,
                PickerCellPaddingVertical = paddingXXS + paddingXXS / 2,
                PickerCellBorderGap = 2,
                PickerControlIconSize = 7,
                PickerControlIconBorderWidth = 1.5f,
                PickerDatePanelPaddingHorizontal = padding + paddingXXS / 2,
            };
        }

        protected override CSSInterpolation[] UseStyle(GlobalToken token)
        {
            var pickerToken = MergeToken(initInputToken<FullToken<"DatePicker">>(token), initPickerPanelToken(token));
            return new CSSInterpolation[]
            {
                GenPickerStyle(pickerToken),
                GenPickerStatusStyle(pickerToken),
                GenCompactItemStyle(token, {
        focusElCls: $"{token.ComponentCls}-focused",
      })
            };
        }

        public DatePickerToken GenComponentStyleHook(GlobalToken token)
        {
            return new DatePickerToken()
            {
                PresetsWidth = 120,
                PresetsMaxWidth = 200,
                ZIndexPopup = token.ZIndexPopupBase + 50,
            };
        }

    }

}