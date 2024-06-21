using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class StepsToken
    {
        public double DescriptionMaxWidth
        {
            get => (double)_tokens["descriptionMaxWidth"];
            set => _tokens["descriptionMaxWidth"] = value;
        }

        public double CustomIconSize
        {
            get => (double)_tokens["customIconSize"];
            set => _tokens["customIconSize"] = value;
        }

        public double CustomIconTop
        {
            get => (double)_tokens["customIconTop"];
            set => _tokens["customIconTop"] = value;
        }

        public double CustomIconFontSize
        {
            get => (double)_tokens["customIconFontSize"];
            set => _tokens["customIconFontSize"] = value;
        }

        public double IconSize
        {
            get => (double)_tokens["iconSize"];
            set => _tokens["iconSize"] = value;
        }

        public double IconTop
        {
            get => (double)_tokens["iconTop"];
            set => _tokens["iconTop"] = value;
        }

        public double IconFontSize
        {
            get => (double)_tokens["iconFontSize"];
            set => _tokens["iconFontSize"] = value;
        }

        public double DotSize
        {
            get => (double)_tokens["dotSize"];
            set => _tokens["dotSize"] = value;
        }

        public double DotCurrentSize
        {
            get => (double)_tokens["dotCurrentSize"];
            set => _tokens["dotCurrentSize"] = value;
        }

        public string NavArrowColor
        {
            get => (string)_tokens["navArrowColor"];
            set => _tokens["navArrowColor"] = value;
        }

        public string NavContentMaxWidth
        {
            get => (string)_tokens["navContentMaxWidth"];
            set => _tokens["navContentMaxWidth"] = value;
        }

        public double IconSizeSM
        {
            get => (double)_tokens["iconSizeSM"];
            set => _tokens["iconSizeSM"] = value;
        }

        public double TitleLineHeight
        {
            get => (double)_tokens["titleLineHeight"];
            set => _tokens["titleLineHeight"] = value;
        }

    }

    public partial class StepsToken : TokenWithCommonCls
    {
        public string ProcessTailColor
        {
            get => (string)_tokens["processTailColor"];
            set => _tokens["processTailColor"] = value;
        }

        public string ProcessIconColor
        {
            get => (string)_tokens["processIconColor"];
            set => _tokens["processIconColor"] = value;
        }

        public string ProcessTitleColor
        {
            get => (string)_tokens["processTitleColor"];
            set => _tokens["processTitleColor"] = value;
        }

        public string ProcessDescriptionColor
        {
            get => (string)_tokens["processDescriptionColor"];
            set => _tokens["processDescriptionColor"] = value;
        }

        public string ProcessIconBgColor
        {
            get => (string)_tokens["processIconBgColor"];
            set => _tokens["processIconBgColor"] = value;
        }

        public string ProcessIconBorderColor
        {
            get => (string)_tokens["processIconBorderColor"];
            set => _tokens["processIconBorderColor"] = value;
        }

        public string ProcessDotColor
        {
            get => (string)_tokens["processDotColor"];
            set => _tokens["processDotColor"] = value;
        }

        public string WaitIconColor
        {
            get => (string)_tokens["waitIconColor"];
            set => _tokens["waitIconColor"] = value;
        }

        public string WaitTitleColor
        {
            get => (string)_tokens["waitTitleColor"];
            set => _tokens["waitTitleColor"] = value;
        }

        public string WaitDescriptionColor
        {
            get => (string)_tokens["waitDescriptionColor"];
            set => _tokens["waitDescriptionColor"] = value;
        }

        public string WaitTailColor
        {
            get => (string)_tokens["waitTailColor"];
            set => _tokens["waitTailColor"] = value;
        }

        public string WaitIconBgColor
        {
            get => (string)_tokens["waitIconBgColor"];
            set => _tokens["waitIconBgColor"] = value;
        }

        public string WaitIconBorderColor
        {
            get => (string)_tokens["waitIconBorderColor"];
            set => _tokens["waitIconBorderColor"] = value;
        }

        public string WaitDotColor
        {
            get => (string)_tokens["waitDotColor"];
            set => _tokens["waitDotColor"] = value;
        }

        public string FinishIconColor
        {
            get => (string)_tokens["finishIconColor"];
            set => _tokens["finishIconColor"] = value;
        }

        public string FinishTitleColor
        {
            get => (string)_tokens["finishTitleColor"];
            set => _tokens["finishTitleColor"] = value;
        }

        public string FinishDescriptionColor
        {
            get => (string)_tokens["finishDescriptionColor"];
            set => _tokens["finishDescriptionColor"] = value;
        }

        public string FinishTailColor
        {
            get => (string)_tokens["finishTailColor"];
            set => _tokens["finishTailColor"] = value;
        }

        public string FinishIconBgColor
        {
            get => (string)_tokens["finishIconBgColor"];
            set => _tokens["finishIconBgColor"] = value;
        }

        public string FinishIconBorderColor
        {
            get => (string)_tokens["finishIconBorderColor"];
            set => _tokens["finishIconBorderColor"] = value;
        }

        public string FinishDotColor
        {
            get => (string)_tokens["finishDotColor"];
            set => _tokens["finishDotColor"] = value;
        }

        public string ErrorIconColor
        {
            get => (string)_tokens["errorIconColor"];
            set => _tokens["errorIconColor"] = value;
        }

        public string ErrorTitleColor
        {
            get => (string)_tokens["errorTitleColor"];
            set => _tokens["errorTitleColor"] = value;
        }

        public string ErrorDescriptionColor
        {
            get => (string)_tokens["errorDescriptionColor"];
            set => _tokens["errorDescriptionColor"] = value;
        }

        public string ErrorTailColor
        {
            get => (string)_tokens["errorTailColor"];
            set => _tokens["errorTailColor"] = value;
        }

        public string ErrorIconBgColor
        {
            get => (string)_tokens["errorIconBgColor"];
            set => _tokens["errorIconBgColor"] = value;
        }

        public string ErrorIconBorderColor
        {
            get => (string)_tokens["errorIconBorderColor"];
            set => _tokens["errorIconBorderColor"] = value;
        }

        public string ErrorDotColor
        {
            get => (string)_tokens["errorDotColor"];
            set => _tokens["errorDotColor"] = value;
        }

        public string StepsNavActiveColor
        {
            get => (string)_tokens["stepsNavActiveColor"];
            set => _tokens["stepsNavActiveColor"] = value;
        }

        public double StepsProgressSize
        {
            get => (double)_tokens["stepsProgressSize"];
            set => _tokens["stepsProgressSize"] = value;
        }

        public double InlineDotSize
        {
            get => (double)_tokens["inlineDotSize"];
            set => _tokens["inlineDotSize"] = value;
        }

        public string InlineTitleColor
        {
            get => (string)_tokens["inlineTitleColor"];
            set => _tokens["inlineTitleColor"] = value;
        }

        public string InlineTailColor
        {
            get => (string)_tokens["inlineTailColor"];
            set => _tokens["inlineTailColor"] = value;
        }

    }

    public class StepItemStatusEnum
    {
        public const string Wait = "wait";
        public const string Process = "process";
        public const string Finish = "finish";
        public const string Error = "error";
    }

    public partial class Steps
    {
        public CSSObject GenStepsItemStatusStyle(string status, StepsToken token)
        {
            var prefix = @$"{token.ComponentCls}-item";
            var iconColorKey = @$"{status}IconColor";
            var titleColorKey = @$"{status}TitleColor";
            var descriptionColorKey = @$"{status}DescriptionColor";
            var tailColorKey = @$"{status}TailColor";
            var iconBgColorKey = @$"{status}IconBgColor";
            var iconBorderColorKey = @$"{status}IconBorderColor";
            var dotColorKey = @$"{status}DotColor";
            return new CSSObject()
            {
                [$"{prefix}-{status} {prefix}-icon"] = new CSSObject()
                {
                    BackgroundColor = token[iconBgColorKey].To<string>(),
                    BorderColor = token[iconBorderColorKey].To<string>(),
                    [$"> {token.ComponentCls}-icon"] = new CSSObject()
                    {
                        Color = token[iconColorKey].To<string>(),
                        [$"{token.ComponentCls}-icon-dot"] = new CSSObject()
                        {
                            Background = token[dotColorKey].To<string>(),
                        },
                    },
                },
                [$"{prefix}-{status}{prefix}-custom {prefix}-icon"] = new CSSObject()
                {
                    [$"> {token.ComponentCls}-icon"] = new CSSObject()
                    {
                        Color = token[dotColorKey].To<string>(),
                    },
                },
                [$"{prefix}-{status} > {prefix}-container > {prefix}-content > {prefix}-title"] = new CSSObject()
                {
                    Color = token[titleColorKey].To<string>(),
                    ["&::after"] = new CSSObject()
                    {
                        BackgroundColor = token[tailColorKey].To<string>(),
                    },
                },
                [$"{prefix}-{status} > {prefix}-container > {prefix}-content > {prefix}-description"] = new CSSObject()
                {
                    Color = token[descriptionColorKey].To<string>(),
                },
                [$"{prefix}-{status} > {prefix}-container > {prefix}-tail::after"] = new CSSObject()
                {
                    BackgroundColor = token[tailColorKey].To<string>(),
                },
            };
        }

        public CSSObject GenStepsItemStyle(StepsToken token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var stepsItemCls = @$"{componentCls}-item";
            var stepItemIconCls = @$"{stepsItemCls}-icon";
            return new CSSObject()
            {
                [stepsItemCls] = new CSSObject()
                {
                    Position = "relative",
                    Display = "inline-block",
                    Flex = 1,
                    Overflow = "hidden",
                    VerticalAlign = "top",
                    ["&:last-child"] = new CSSObject()
                    {
                        Flex = "none",
                        [$"> {stepsItemCls}-container > {stepsItemCls}-tail, > {stepsItemCls}-container >  {stepsItemCls}-content > {stepsItemCls}-title::after"] = new CSSObject()
                        {
                            Display = "none",
                        },
                    },
                },
                [$"{stepsItemCls}-container"] = new CSSObject()
                {
                    Outline = "none",
                    ["&:focus-visible"] = new CSSObject()
                    {
                        [stepItemIconCls] = new CSSObject()
                        {
                            ["..."] = GenFocusOutline(token)
                        },
                    },
                },
                [$"{stepItemIconCls}, {stepsItemCls}-content"] = new CSSObject()
                {
                    Display = "inline-block",
                    VerticalAlign = "top",
                },
                [stepItemIconCls] = new CSSObject()
                {
                    Width = token.IconSize,
                    Height = token.IconSize,
                    MarginTop = 0,
                    MarginBottom = 0,
                    MarginInlineStart = 0,
                    MarginInlineEnd = token.MarginXS,
                    FontSize = token.IconFontSize,
                    FontFamily = token.FontFamily,
                    LineHeight = @$"{token.IconSize}px",
                    TextAlign = "center",
                    BorderRadius = token.IconSize,
                    Border = @$"{token.LineWidth}px {token.LineType} transparent",
                    Transition = @$"background-color {motionDurationSlow}, border-color {motionDurationSlow}",
                    [$"{componentCls}-icon"] = new CSSObject()
                    {
                        Position = "relative",
                        Top = token.IconTop,
                        Color = token.ColorPrimary,
                        LineHeight = 1,
                    },
                },
                [$"{stepsItemCls}-tail"] = new CSSObject()
                {
                    Position = "absolute",
                    Top = token.IconSize / 2 - token.PaddingXXS,
                    InsetInlineStart = 0,
                    Width = "100%",
                    ["&::after"] = new CSSObject()
                    {
                        Display = "inline-block",
                        Width = "100%",
                        Height = token.LineWidth,
                        Background = token.ColorSplit,
                        BorderRadius = token.LineWidth,
                        Transition = @$"background {motionDurationSlow}",
                        Content = "\"\"",
                    },
                },
                [$"{stepsItemCls}-title"] = new CSSObject()
                {
                    Position = "relative",
                    Display = "inline-block",
                    PaddingInlineEnd = token.Padding,
                    Color = token.ColorText,
                    FontSize = token.FontSizeLG,
                    LineHeight = @$"{token.TitleLineHeight}px",
                    ["&::after"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = token.TitleLineHeight / 2,
                        InsetInlineStart = "100%",
                        Display = "block",
                        Width = 9999,
                        Height = token.LineWidth,
                        Background = token.ProcessTailColor,
                        Content = "\"\"",
                    },
                },
                [$"{stepsItemCls}-subtitle"] = new CSSObject()
                {
                    Display = "inline",
                    MarginInlineStart = token.MarginXS,
                    Color = token.ColorTextDescription,
                    FontWeight = "normal",
                    FontSize = token.FontSize,
                },
                [$"{stepsItemCls}-description"] = new CSSObject()
                {
                    Color = token.ColorTextDescription,
                    FontSize = token.FontSize,
                },
                ["..."] = GenStepsItemStatusStyle(StepItemStatusEnum.Wait, token),
                ["..."] = GenStepsItemStatusStyle(StepItemStatusEnum.Process, token),
                [$"{stepsItemCls}-process > {stepsItemCls}-container > {stepsItemCls}-title"] = new CSSObject()
                {
                    FontWeight = token.FontWeightStrong,
                },
                ["..."] = GenStepsItemStatusStyle(StepItemStatusEnum.Finish, token),
                ["..."] = GenStepsItemStatusStyle(StepItemStatusEnum.Error, token),
                [$"{stepsItemCls}{componentCls}-next-error > {componentCls}-item-title::after"] = new CSSObject()
                {
                    Background = token.ColorError,
                },
                [$"{stepsItemCls}-disabled"] = new CSSObject()
                {
                    Cursor = "not-allowed",
                },
            };
        }

        public CSSObject GenStepsClickableStyle(StepsToken token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            return new CSSObject()
            {
                [$"& {componentCls}-item"] = new CSSObject()
                {
                    [$"&:not({componentCls}-item-active)"] = new CSSObject()
                    {
                        [$"& > {componentCls}-item-container[role=\"button\"]"] = new CSSObject()
                        {
                            Cursor = "pointer",
                            [$"{componentCls}-item"] = new CSSObject()
                            {
                                [$"&-title, &-subtitle, &-description, &-icon {componentCls}-icon"] = new CSSObject()
                                {
                                    Transition = @$"color {motionDurationSlow}",
                                },
                            },
                            ["&:hover"] = new CSSObject()
                            {
                                [$"{componentCls}-item"] = new CSSObject()
                                {
                                    ["&-title, &-subtitle, &-description"] = new CSSObject()
                                    {
                                        Color = token.ColorPrimary,
                                    },
                                },
                            },
                        },
                        [$"&:not({componentCls}-item-process)"] = new CSSObject()
                        {
                            [$"& > {componentCls}-item-container[role=\"button\"]:hover"] = new CSSObject()
                            {
                                [$"{componentCls}-item"] = new CSSObject()
                                {
                                    ["&-icon"] = new CSSObject()
                                    {
                                        BorderColor = token.ColorPrimary,
                                        [$"{componentCls}-icon"] = new CSSObject()
                                        {
                                            Color = token.ColorPrimary,
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
                [$"&{componentCls}-horizontal:not({componentCls}-label-vertical)"] = new CSSObject()
                {
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        PaddingInlineStart = token.Padding,
                        WhiteSpace = "nowrap",
                        ["&:first-child"] = new CSSObject()
                        {
                            PaddingInlineStart = 0,
                        },
                        [$"&:last-child {componentCls}-item-title"] = new CSSObject()
                        {
                            PaddingInlineEnd = 0,
                        },
                        ["&-tail"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        ["&-description"] = new CSSObject()
                        {
                            MaxWidth = token.DescriptionMaxWidth,
                            WhiteSpace = "normal",
                        },
                    },
                },
            };
        }

        public CSSObject GenStepsStyle(StepsToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Display = "flex",
                    Width = "100%",
                    FontSize = 0,
                    TextAlign = "initial",
                    ["..."] = GenStepsItemStyle(token),
                    ["..."] = GenStepsClickableStyle(token),
                    ["..."] = GenStepsCustomIconStyle(token),
                    ["..."] = GenStepsSmallStyle(token),
                    ["..."] = GenStepsVerticalStyle(token),
                    ["..."] = GenStepsLabelPlacementStyle(token),
                    ["..."] = GenStepsProgressDotStyle(token),
                    ["..."] = GenStepsNavStyle(token),
                    ["..."] = GenStepsRTLStyle(token),
                    ["..."] = GenStepsProgressStyle(token),
                    ["..."] = GenStepsInlineStyle(token)
                },
            };
        }

        protected override UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Steps",
                (token) =>
                {
                    var wireframe = token.Wireframe;
                    var colorTextDisabled = token.ColorTextDisabled;
                    var controlHeightLG = token.ControlHeightLG;
                    var colorTextLightSolid = token.ColorTextLightSolid;
                    var colorText = token.ColorText;
                    var colorPrimary = token.ColorPrimary;
                    var colorTextLabel = token.ColorTextLabel;
                    var colorTextDescription = token.ColorTextDescription;
                    var colorTextQuaternary = token.ColorTextQuaternary;
                    var colorFillContent = token.ColorFillContent;
                    var controlItemBgActive = token.ControlItemBgActive;
                    var colorError = token.ColorError;
                    var colorBgContainer = token.ColorBgContainer;
                    var colorBorderSecondary = token.ColorBorderSecondary;
                    var colorSplit = token.ColorSplit;
                    var stepsToken = MergeToken(
                        token,
                        new StepsToken()
                        {
                            ProcessIconColor = colorTextLightSolid,
                            ProcessTitleColor = colorText,
                            ProcessDescriptionColor = colorText,
                            ProcessIconBgColor = colorPrimary,
                            ProcessIconBorderColor = colorPrimary,
                            ProcessDotColor = colorPrimary,
                            ProcessTailColor = colorSplit,
                            WaitIconColor = wireframe ? colorTextDisabled : colorTextLabel,
                            WaitTitleColor = colorTextDescription,
                            WaitDescriptionColor = colorTextDescription,
                            WaitTailColor = colorSplit,
                            WaitIconBgColor = wireframe ? colorBgContainer : colorFillContent,
                            WaitIconBorderColor = wireframe ? colorTextDisabled : "transparent",
                            WaitDotColor = colorTextDisabled,
                            FinishIconColor = colorPrimary,
                            FinishTitleColor = colorText,
                            FinishDescriptionColor = colorTextDescription,
                            FinishTailColor = colorPrimary,
                            FinishIconBgColor = wireframe ? colorBgContainer : controlItemBgActive,
                            FinishIconBorderColor = wireframe ? colorPrimary : controlItemBgActive,
                            FinishDotColor = colorPrimary,
                            ErrorIconColor = colorTextLightSolid,
                            ErrorTitleColor = colorError,
                            ErrorDescriptionColor = colorError,
                            ErrorTailColor = colorSplit,
                            ErrorIconBgColor = colorError,
                            ErrorIconBorderColor = colorError,
                            ErrorDotColor = colorError,
                            StepsNavActiveColor = colorPrimary,
                            StepsProgressSize = controlHeightLG,
                            InlineDotSize = 6,
                            InlineTitleColor = colorTextQuaternary,
                            InlineTailColor = colorBorderSecondary,
                        });
                    return new CSSInterpolation[]
                    {
                        GenStepsStyle(stepsToken),
                    };
                },
                (token) =>
                {
                    var colorTextDisabled = token.ColorTextDisabled;
                    var fontSize = token.FontSize;
                    var controlHeightSM = token.ControlHeightSM;
                    var controlHeight = token.ControlHeight;
                    var controlHeightLG = token.ControlHeightLG;
                    var fontSizeHeading3 = token.FontSizeHeading3;
                    return new StepsToken()
                    {
                        TitleLineHeight = controlHeight,
                        CustomIconSize = controlHeight,
                        CustomIconTop = 0,
                        CustomIconFontSize = controlHeightSM,
                        IconSize = controlHeight,
                        IconTop = -0.5f,
                        IconFontSize = fontSize,
                        IconSizeSM = fontSizeHeading3,
                        DotSize = controlHeight / 4,
                        DotCurrentSize = controlHeightLG / 4,
                        NavArrowColor = colorTextDisabled,
                        NavContentMaxWidth = "auto",
                        DescriptionMaxWidth = 140,
                    };
                });
        }

        public CSSObject GenStepsCustomIconStyle(StepsToken token)
        {
            var componentCls = token.ComponentCls;
            var customIconTop = token.CustomIconTop;
            var customIconSize = token.CustomIconSize;
            var customIconFontSize = token.CustomIconFontSize;
            return new CSSObject()
            {
                [$"{componentCls}-item-custom"] = new CSSObject()
                {
                    [$"> {componentCls}-item-container > {componentCls}-item-icon"] = new CSSObject()
                    {
                        Height = "auto",
                        Background = "none",
                        Border = 0,
                        [$"> {componentCls}-icon"] = new CSSObject()
                        {
                            Top = customIconTop,
                            Width = customIconSize,
                            Height = customIconSize,
                            FontSize = customIconFontSize,
                            LineHeight = @$"{customIconFontSize}px",
                        },
                    },
                },
                [$"&:not({componentCls}-vertical)"] = new CSSObject()
                {
                    [$"{componentCls}-item-custom"] = new CSSObject()
                    {
                        [$"{componentCls}-item-icon"] = new CSSObject()
                        {
                            Width = "auto",
                            Background = "none",
                        },
                    },
                },
            };
        }

        public CSSObject GenStepsInlineStyle(StepsToken token)
        {
            var componentCls = token.ComponentCls;
            var inlineDotSize = token.InlineDotSize;
            var inlineTitleColor = token.InlineTitleColor;
            var inlineTailColor = token.InlineTailColor;
            var containerPaddingTop = token.PaddingXS + token.LineWidth;
            var titleStyle = new CSSObject()
            {
                [$"{componentCls}-item-container {componentCls}-item-content {componentCls}-item-title"] = new CSSObject()
                {
                    Color = inlineTitleColor,
                },
            };
            return new CSSObject()
            {
                [$"&{componentCls}-inline"] = new CSSObject()
                {
                    Width = "auto",
                    Display = "inline-flex",
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        Flex = "none",
                        ["&-container"] = new CSSObject()
                        {
                            Padding = @$"{containerPaddingTop}px {token.PaddingXXS}px 0",
                            Margin = @$"0 {token.MarginXXS / 2}px",
                            BorderRadius = token.BorderRadiusSM,
                            Cursor = "pointer",
                            Transition = @$"background-color {token.MotionDurationMid}",
                            ["&:hover"] = new CSSObject()
                            {
                                Background = token.ControlItemBgHover,
                            },
                            ["&[role=\"button\"]:hover"] = new CSSObject()
                            {
                                Opacity = 1,
                            },
                        },
                        ["&-icon"] = new CSSObject()
                        {
                            Width = inlineDotSize,
                            Height = inlineDotSize,
                            MarginInlineStart = @$"calc(50% - {inlineDotSize / 2}px)",
                            [$"> {componentCls}-icon"] = new CSSObject()
                            {
                                Top = 0,
                            },
                            [$"{componentCls}-icon-dot"] = new CSSObject()
                            {
                                BorderRadius = token.FontSizeSM / 4,
                            },
                        },
                        ["&-content"] = new CSSObject()
                        {
                            Width = "auto",
                            MarginTop = token.MarginXS - token.LineWidth,
                        },
                        ["&-title"] = new CSSObject()
                        {
                            Color = inlineTitleColor,
                            FontSize = token.FontSizeSM,
                            LineHeight = token.LineHeightSM,
                            FontWeight = "normal",
                            MarginBottom = token.MarginXXS / 2,
                        },
                        ["&-description"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        ["&-tail"] = new CSSObject()
                        {
                            MarginInlineStart = 0,
                            Top = containerPaddingTop + inlineDotSize / 2,
                            Transform = @$"translateY(-50%)",
                            ["&:after"] = new CSSObject()
                            {
                                Width = "100%",
                                Height = token.LineWidth,
                                BorderRadius = 0,
                                MarginInlineStart = 0,
                                Background = inlineTailColor,
                            },
                        },
                        [$"&:first-child {componentCls}-item-tail"] = new CSSObject()
                        {
                            Width = "50%",
                            MarginInlineStart = "50%",
                        },
                        [$"&:last-child {componentCls}-item-tail"] = new CSSObject()
                        {
                            Display = "block",
                            Width = "50%",
                        },
                        ["&-wait"] = new CSSObject()
                        {
                            [$"{componentCls}-item-icon {componentCls}-icon {componentCls}-icon-dot"] = new CSSObject()
                            {
                                BackgroundColor = token.ColorBorderBg,
                                Border = @$"{token.LineWidth}px {token.LineType} {inlineTailColor}",
                            },
                            ["..."] = titleStyle,
                        },
                        ["&-finish"] = new CSSObject()
                        {
                            [$"{componentCls}-item-tail::after"] = new CSSObject()
                            {
                                BackgroundColor = inlineTailColor,
                            },
                            [$"{componentCls}-item-icon {componentCls}-icon {componentCls}-icon-dot"] = new CSSObject()
                            {
                                BackgroundColor = inlineTailColor,
                                Border = @$"{token.LineWidth}px {token.LineType} {inlineTailColor}",
                            },
                            ["..."] = titleStyle,
                        },
                        ["&-error"] = titleStyle,
                        ["&-active, &-process"] = new CSSObject()
                        {
                            [$"{componentCls}-item-icon"] = new CSSObject()
                            {
                                Width = inlineDotSize,
                                Height = inlineDotSize,
                                MarginInlineStart = @$"calc(50% - {inlineDotSize / 2}px)",
                                Top = 0,
                            },
                            ["..."] = titleStyle,
                        },
                        [$"&:not({componentCls}-item-active) > {componentCls}-item-container[role=\"button\"]:hover"] = new CSSObject()
                        {
                            [$"{componentCls}-item-title"] = new CSSObject()
                            {
                                Color = inlineTitleColor,
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenStepsLabelPlacementStyle(StepsToken token)
        {
            var componentCls = token.ComponentCls;
            var iconSize = token.IconSize;
            var lineHeight = token.LineHeight;
            var iconSizeSM = token.IconSizeSM;
            return new CSSObject()
            {
                [$"&{componentCls}-label-vertical"] = new CSSObject()
                {
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        Overflow = "visible",
                        ["&-tail"] = new CSSObject()
                        {
                            MarginInlineStart = iconSize / 2 + token.ControlHeightLG,
                            Padding = @$"{token.PaddingXXS}px {token.PaddingLG}px",
                        },
                        ["&-content"] = new CSSObject()
                        {
                            Display = "block",
                            Width = (iconSize / 2 + token.ControlHeightLG) * 2,
                            MarginTop = token.MarginSM,
                            TextAlign = "center",
                        },
                        ["&-icon"] = new CSSObject()
                        {
                            Display = "inline-block",
                            MarginInlineStart = token.ControlHeightLG,
                        },
                        ["&-title"] = new CSSObject()
                        {
                            PaddingInlineEnd = 0,
                            PaddingInlineStart = 0,
                            ["&::after"] = new CSSObject()
                            {
                                Display = "none",
                            },
                        },
                        ["&-subtitle"] = new CSSObject()
                        {
                            Display = "block",
                            MarginBottom = token.MarginXXS,
                            MarginInlineStart = 0,
                            LineHeight = lineHeight,
                        },
                    },
                    [$"&{componentCls}-small:not({componentCls}-dot)"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            ["&-icon"] = new CSSObject()
                            {
                                MarginInlineStart = token.ControlHeightLG + (iconSize - iconSizeSM) / 2,
                            },
                        },
                    },
                },
            };
        }

        public CSSObject GenStepsNavStyle(StepsToken token)
        {
            var componentCls = token.ComponentCls;
            var navContentMaxWidth = token.NavContentMaxWidth;
            var navArrowColor = token.NavArrowColor;
            var stepsNavActiveColor = token.StepsNavActiveColor;
            var motionDurationSlow = token.MotionDurationSlow;
            return new CSSObject()
            {
                [$"&{componentCls}-navigation"] = new CSSObject()
                {
                    PaddingTop = token.PaddingSM,
                    [$"&{componentCls}-small"] = new CSSObject()
                    {
                        [$"{componentCls}-item"] = new CSSObject()
                        {
                            ["&-container"] = new CSSObject()
                            {
                                MarginInlineStart = -token.MarginSM,
                            },
                        },
                    },
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        Overflow = "visible",
                        TextAlign = "center",
                        ["&-container"] = new CSSObject()
                        {
                            Display = "inline-block",
                            Height = "100%",
                            MarginInlineStart = -token.Margin,
                            PaddingBottom = token.PaddingSM,
                            TextAlign = "start",
                            Transition = @$"opacity {motionDurationSlow}",
                            [$"{componentCls}-item-content"] = new CSSObject()
                            {
                                MaxWidth = navContentMaxWidth,
                            },
                            [$"{componentCls}-item-title"] = new CSSObject()
                            {
                                MaxWidth = "100%",
                                PaddingInlineEnd = 0,
                                ["..."] = TextEllipsis,
                                ["&::after"] = new CSSObject()
                                {
                                    Display = "none",
                                },
                            },
                        },
                        [$"&:not({componentCls}-item-active)"] = new CSSObject()
                        {
                            [$"{componentCls}-item-container[role=\"button\"]"] = new CSSObject()
                            {
                                Cursor = "pointer",
                                ["&:hover"] = new CSSObject()
                                {
                                    Opacity = 0.85f,
                                },
                            },
                        },
                        ["&:last-child"] = new CSSObject()
                        {
                            Flex = 1,
                            ["&::after"] = new CSSObject()
                            {
                                Display = "none",
                            },
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = @$"calc(50% - {token.PaddingSM / 2}px)",
                            InsetInlineStart = "100%",
                            Display = "inline-block",
                            Width = token.FontSizeIcon,
                            Height = token.FontSizeIcon,
                            BorderTop = @$"{token.LineWidth}px {token.LineType} {navArrowColor}",
                            BorderBottom = "none",
                            BorderInlineStart = "none",
                            BorderInlineEnd = @$"{token.LineWidth}px {token.LineType} {navArrowColor}",
                            Transform = "translateY(-50%) translateX(-50%) rotate(45deg)",
                            Content = "\"\"",
                        },
                        ["&::before"] = new CSSObject()
                        {
                            Position = "absolute",
                            Bottom = 0,
                            InsetInlineStart = "50%",
                            Display = "inline-block",
                            Width = 0,
                            Height = token.LineWidthBold,
                            BackgroundColor = stepsNavActiveColor,
                            Transition = @$"width {motionDurationSlow}, inset-inline-start {motionDurationSlow}",
                            TransitionTimingFunction = "ease-out",
                            Content = "\"\"",
                        },
                    },
                    [$"{componentCls}-item{componentCls}-item-active::before"] = new CSSObject()
                    {
                        InsetInlineStart = 0,
                        Width = "100%",
                    },
                },
                [$"&{componentCls}-navigation{componentCls}-vertical"] = new CSSObject()
                {
                    [$"> {componentCls}-item"] = new CSSObject()
                    {
                        MarginInlineEnd = 0,
                        ["&::before"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        [$"&{componentCls}-item-active::before"] = new CSSObject()
                        {
                            Top = 0,
                            InsetInlineEnd = 0,
                            InsetInlineStart = "unset",
                            Display = "block",
                            Width = token.LineWidth * 3,
                            Height = @$"calc(100% - {token.MarginLG}px)",
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Position = "relative",
                            InsetInlineStart = "50%",
                            Display = "block",
                            Width = token.ControlHeight * 0.25,
                            Height = token.ControlHeight * 0.25,
                            MarginBottom = token.MarginXS,
                            TextAlign = "center",
                            Transform = "translateY(-50%) translateX(-50%) rotate(135deg)",
                        },
                        ["&:last-child"] = new CSSObject()
                        {
                            ["&::after"] = new CSSObject()
                            {
                                Display = "none",
                            },
                        },
                        [$"> {componentCls}-item-container > {componentCls}-item-tail"] = new CSSObject()
                        {
                            Visibility = "hidden",
                        },
                    },
                },
                [$"&{componentCls}-navigation{componentCls}-horizontal"] = new CSSObject()
                {
                    [$"> {componentCls}-item > {componentCls}-item-container > {componentCls}-item-tail"] = new CSSObject()
                    {
                        Visibility = "hidden",
                    },
                },
            };
        }

        public CSSObject GenStepsProgressDotStyle(StepsToken token)
        {
            var componentCls = token.ComponentCls;
            var descriptionMaxWidth = token.DescriptionMaxWidth;
            var lineHeight = token.LineHeight;
            var dotCurrentSize = token.DotCurrentSize;
            var dotSize = token.DotSize;
            var motionDurationSlow = token.MotionDurationSlow;
            return new CSSObject()
            {
                [$"&{componentCls}-dot, &{componentCls}-dot{componentCls}-small"] = new CSSObject()
                {
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        ["&-title"] = new CSSObject()
                        {
                            LineHeight = lineHeight,
                        },
                        ["&-tail"] = new CSSObject()
                        {
                            Top = token.IconSize / 2 - token.PaddingXXS,
                            Width = "100%",
                            MarginTop = 0,
                            MarginBottom = 0,
                            MarginInline = @$"{descriptionMaxWidth / 2}px 0",
                            Padding = 0,
                            ["&::after"] = new CSSObject()
                            {
                                Width = @$"calc(100% - {token.MarginSM * 2}px)",
                                Height = token.LineWidth * 3,
                                MarginInlineStart = token.MarginSM,
                            },
                        },
                        ["&-icon"] = new CSSObject()
                        {
                            Width = dotSize,
                            Height = dotSize,
                            MarginInlineStart = (token.DescriptionMaxWidth - dotSize) / 2,
                            PaddingInlineEnd = 0,
                            LineHeight = @$"{dotSize}px",
                            Background = "transparent",
                            Border = 0,
                            [$"{componentCls}-icon-dot"] = new CSSObject()
                            {
                                Position = "relative",
                                Float = "left",
                                Width = "100%",
                                Height = "100%",
                                BorderRadius = 100,
                                Transition = @$"all {motionDurationSlow}",
                                ["&::after"] = new CSSObject()
                                {
                                    Position = "absolute",
                                    Top = -token.MarginSM,
                                    InsetInlineStart = (dotSize - token.ControlHeightLG * 1.5) / 2,
                                    Width = token.ControlHeightLG * 1.5,
                                    Height = token.ControlHeight,
                                    Background = "transparent",
                                    Content = "\"\"",
                                },
                            },
                        },
                        ["&-content"] = new CSSObject()
                        {
                            Width = descriptionMaxWidth,
                        },
                        [$"&-process {componentCls}-item-icon"] = new CSSObject()
                        {
                            Position = "relative",
                            Top = (dotSize - dotCurrentSize) / 2,
                            Width = dotCurrentSize,
                            Height = dotCurrentSize,
                            LineHeight = @$"{dotCurrentSize}px",
                            Background = "none",
                            MarginInlineStart = (token.DescriptionMaxWidth - dotCurrentSize) / 2,
                        },
                        [$"&-process {componentCls}-icon"] = new CSSObject()
                        {
                            [$"&:first-child {componentCls}-icon-dot"] = new CSSObject()
                            {
                                InsetInlineStart = 0,
                            },
                        },
                    },
                },
                [$"&{componentCls}-vertical{componentCls}-dot"] = new CSSObject()
                {
                    [$"{componentCls}-item-icon"] = new CSSObject()
                    {
                        MarginTop = (token.ControlHeight - dotSize) / 2,
                        MarginInlineStart = 0,
                        Background = "none",
                    },
                    [$"{componentCls}-item-process {componentCls}-item-icon"] = new CSSObject()
                    {
                        MarginTop = (token.ControlHeight - dotCurrentSize) / 2,
                        Top = 0,
                        InsetInlineStart = (dotSize - dotCurrentSize) / 2,
                        MarginInlineStart = 0,
                    },
                    [$"{componentCls}-item > {componentCls}-item-container > {componentCls}-item-tail"] = new CSSObject()
                    {
                        Top = (token.ControlHeight - dotSize) / 2,
                        InsetInlineStart = 0,
                        Margin = 0,
                        Padding = @$"{dotSize + token.PaddingXS}px 0 {token.PaddingXS}px",
                        ["&::after"] = new CSSObject()
                        {
                            MarginInlineStart = (dotSize - token.LineWidth) / 2,
                        },
                    },
                    [$"&{componentCls}-small"] = new CSSObject()
                    {
                        [$"{componentCls}-item-icon"] = new CSSObject()
                        {
                            MarginTop = (token.ControlHeightSM - dotSize) / 2,
                        },
                        [$"{componentCls}-item-process {componentCls}-item-icon"] = new CSSObject()
                        {
                            MarginTop = (token.ControlHeightSM - dotCurrentSize) / 2,
                        },
                        [$"{componentCls}-item > {componentCls}-item-container > {componentCls}-item-tail"] = new CSSObject()
                        {
                            Top = (token.ControlHeightSM - dotSize) / 2,
                        },
                    },
                    [$"{componentCls}-item:first-child {componentCls}-icon-dot"] = new CSSObject()
                    {
                        InsetInlineStart = 0,
                    },
                    [$"{componentCls}-item-content"] = new CSSObject()
                    {
                        Width = "inherit",
                    },
                },
            };
        }

        public CSSObject GenStepsProgressStyle(StepsToken token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"&{componentCls}-with-progress"] = new CSSObject()
                {
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        PaddingTop = token.PaddingXXS,
                        [$"&-process {componentCls}-item-container {componentCls}-item-icon {componentCls}-icon"] = new CSSObject()
                        {
                            Color = token.ProcessIconColor,
                        },
                    },
                    [$"&{componentCls}-vertical > {componentCls}-item "] = new CSSObject()
                    {
                        PaddingInlineStart = token.PaddingXXS,
                        [$"> {componentCls}-item-container > {componentCls}-item-tail"] = new CSSObject()
                        {
                            Top = token.MarginXXS,
                            InsetInlineStart = token.IconSize / 2 - token.LineWidth + token.PaddingXXS,
                        },
                    },
                    [$"&, &{componentCls}-small"] = new CSSObject()
                    {
                        [$"&{componentCls}-horizontal {componentCls}-item:first-child"] = new CSSObject()
                        {
                            PaddingBottom = token.PaddingXXS,
                            PaddingInlineStart = token.PaddingXXS,
                        },
                    },
                    [$"&{componentCls}-small{componentCls}-vertical > {componentCls}-item > {componentCls}-item-container > {componentCls}-item-tail"] = new CSSObject()
                    {
                        InsetInlineStart = token.IconSizeSM / 2 - token.LineWidth + token.PaddingXXS,
                    },
                    [$"&{componentCls}-label-vertical"] = new CSSObject()
                    {
                        [$"{componentCls}-item {componentCls}-item-tail"] = new CSSObject()
                        {
                            Top = token.Margin - 2 * token.LineWidth,
                        },
                    },
                    [$"{componentCls}-item-icon"] = new CSSObject()
                    {
                        Position = "relative",
                        [$"{antCls}-progress"] = new CSSObject()
                        {
                            Position = "absolute",
                            InsetBlockStart = (token.IconSize - token.StepsProgressSize - token.LineWidth * 2) / 2,
                            InsetInlineStart = (token.IconSize - token.StepsProgressSize - token.LineWidth * 2) / 2,
                        },
                    },
                },
            };
        }

        public CSSObject GenStepsRTLStyle(StepsToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"&{componentCls}-rtl"] = new CSSObject()
                {
                    Direction = "rtl",
                    [$"{componentCls}-item"] = new CSSObject()
                    {
                        ["&-subtitle"] = new CSSObject()
                        {
                            Float = "left",
                        },
                    },
                    [$"&{componentCls}-navigation"] = new CSSObject()
                    {
                        [$"{componentCls}-item::after"] = new CSSObject()
                        {
                            Transform = "rotate(-45deg)",
                        },
                    },
                    [$"&{componentCls}-vertical"] = new CSSObject()
                    {
                        [$"> {componentCls}-item"] = new CSSObject()
                        {
                            ["&::after"] = new CSSObject()
                            {
                                Transform = "rotate(225deg)",
                            },
                            [$"{componentCls}-item-icon"] = new CSSObject()
                            {
                                Float = "right",
                            },
                        },
                    },
                    [$"&{componentCls}-dot"] = new CSSObject()
                    {
                        [$"{componentCls}-item-icon {componentCls}-icon-dot, &{componentCls}-small {componentCls}-item-icon {componentCls}-icon-dot"] = new CSSObject()
                        {
                            Float = "right",
                        },
                    },
                },
            };
        }

        public CSSObject GenStepsSmallStyle(StepsToken token)
        {
            var componentCls = token.ComponentCls;
            var iconSizeSM = token.IconSizeSM;
            var fontSizeSM = token.FontSizeSM;
            var fontSize = token.FontSize;
            var colorTextDescription = token.ColorTextDescription;
            return new CSSObject()
            {
                [$"&{componentCls}-small"] = new CSSObject()
                {
                    [$"&{componentCls}-horizontal:not({componentCls}-label-vertical) {componentCls}-item"] = new CSSObject()
                    {
                        PaddingInlineStart = token.PaddingSM,
                        ["&:first-child"] = new CSSObject()
                        {
                            PaddingInlineStart = 0,
                        },
                    },
                    [$"{componentCls}-item-icon"] = new CSSObject()
                    {
                        Width = iconSizeSM,
                        Height = iconSizeSM,
                        MarginTop = 0,
                        MarginBottom = 0,
                        MarginInline = @$"0 {token.MarginXS}px",
                        FontSize = fontSizeSM,
                        LineHeight = @$"{iconSizeSM}px",
                        TextAlign = "center",
                        BorderRadius = iconSizeSM,
                    },
                    [$"{componentCls}-item-title"] = new CSSObject()
                    {
                        PaddingInlineEnd = token.PaddingSM,
                        FontSize = fontSize,
                        LineHeight = @$"{iconSizeSM}px",
                        ["&::after"] = new CSSObject()
                        {
                            Top = iconSizeSM / 2,
                        },
                    },
                    [$"{componentCls}-item-description"] = new CSSObject()
                    {
                        Color = colorTextDescription,
                        FontSize = fontSize,
                    },
                    [$"{componentCls}-item-tail"] = new CSSObject()
                    {
                        Top = iconSizeSM / 2 - token.PaddingXXS,
                    },
                    [$"{componentCls}-item-custom {componentCls}-item-icon"] = new CSSObject()
                    {
                        Width = "inherit",
                        Height = "inherit",
                        LineHeight = "inherit",
                        Background = "none",
                        Border = 0,
                        BorderRadius = 0,
                        [$"> {componentCls}-icon"] = new CSSObject()
                        {
                            FontSize = iconSizeSM,
                            LineHeight = @$"{iconSizeSM}px",
                            Transform = "none",
                        },
                    },
                },
            };
        }

        public CSSObject GenStepsVerticalStyle(StepsToken token)
        {
            var componentCls = token.ComponentCls;
            var iconSizeSM = token.IconSizeSM;
            var iconSize = token.IconSize;
            return new CSSObject()
            {
                [$"&{componentCls}-vertical"] = new CSSObject()
                {
                    Display = "flex",
                    FlexDirection = "column",
                    [$"> {componentCls}-item"] = new CSSObject()
                    {
                        Display = "block",
                        Flex = "1 0 auto",
                        PaddingInlineStart = 0,
                        Overflow = "visible",
                        [$"{componentCls}-item-icon"] = new CSSObject()
                        {
                            Float = "left",
                            MarginInlineEnd = token.Margin,
                        },
                        [$"{componentCls}-item-content"] = new CSSObject()
                        {
                            Display = "block",
                            MinHeight = token.ControlHeight * 1.5,
                            Overflow = "hidden",
                        },
                        [$"{componentCls}-item-title"] = new CSSObject()
                        {
                            LineHeight = @$"{iconSize}px",
                        },
                        [$"{componentCls}-item-description"] = new CSSObject()
                        {
                            PaddingBottom = token.PaddingSM,
                        },
                    },
                    [$"> {componentCls}-item > {componentCls}-item-container > {componentCls}-item-tail"] = new CSSObject()
                    {
                        Position = "absolute",
                        Top = 0,
                        InsetInlineStart = iconSize / 2 - token.LineWidth,
                        Width = token.LineWidth,
                        Height = "100%",
                        Padding = @$"{iconSize + token.MarginXXS * 1.5}px 0 {token.MarginXXS * 1.5}px",
                        ["&::after"] = new CSSObject()
                        {
                            Width = token.LineWidth,
                            Height = "100%",
                        },
                    },
                    [$"> {componentCls}-item:not(:last-child) > {componentCls}-item-container > {componentCls}-item-tail"] = new CSSObject()
                    {
                        Display = "block",
                    },
                    [$" > {componentCls}-item > {componentCls}-item-container > {componentCls}-item-content > {componentCls}-item-title"] = new CSSObject()
                    {
                        ["&::after"] = new CSSObject()
                        {
                            Display = "none",
                        },
                    },
                    [$"&{componentCls}-small {componentCls}-item-container"] = new CSSObject()
                    {
                        [$"{componentCls}-item-tail"] = new CSSObject()
                        {
                            Position = "absolute",
                            Top = 0,
                            InsetInlineStart = iconSizeSM / 2 - token.LineWidth,
                            Padding = @$"{iconSizeSM + token.MarginXXS * 1.5}px 0 {token.MarginXXS * 1.5}px",
                        },
                        [$"{componentCls}-item-title"] = new CSSObject()
                        {
                            LineHeight = @$"{iconSizeSM}px",
                        },
                    },
                },
            };
        }

    }

}
