using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class StepsToken
    {
        public int DescriptionMaxWidth { get; set; }

        public int CustomIconSize { get; set; }

        public int CustomIconTop { get; set; }

        public int CustomIconFontSize { get; set; }

        public int IconSize { get; set; }

        public int IconTop { get; set; }

        public int IconFontSize { get; set; }

        public int DotSize { get; set; }

        public int DotCurrentSize { get; set; }

        public string NavArrowColor { get; set; }

        public CSSProperties NavContentMaxWidth { get; set; }

        public int IconSizeSM { get; set; }

        public int TitleLineHeight { get; set; }

    }

    public partial class StepsToken : TokenWithCommonCls
    {
        public string ProcessTailColor { get; set; }

        public string ProcessIconColor { get; set; }

        public string ProcessTitleColor { get; set; }

        public string ProcessDescriptionColor { get; set; }

        public string ProcessIconBgColor { get; set; }

        public string ProcessIconBorderColor { get; set; }

        public string ProcessDotColor { get; set; }

        public string WaitIconColor { get; set; }

        public string WaitTitleColor { get; set; }

        public string WaitDescriptionColor { get; set; }

        public string WaitTailColor { get; set; }

        public string WaitIconBgColor { get; set; }

        public string WaitIconBorderColor { get; set; }

        public string WaitDotColor { get; set; }

        public string FinishIconColor { get; set; }

        public string FinishTitleColor { get; set; }

        public string FinishDescriptionColor { get; set; }

        public string FinishTailColor { get; set; }

        public string FinishIconBgColor { get; set; }

        public string FinishIconBorderColor { get; set; }

        public string FinishDotColor { get; set; }

        public string ErrorIconColor { get; set; }

        public string ErrorTitleColor { get; set; }

        public string ErrorDescriptionColor { get; set; }

        public string ErrorTailColor { get; set; }

        public string ErrorIconBgColor { get; set; }

        public string ErrorIconBorderColor { get; set; }

        public string ErrorDotColor { get; set; }

        public string StepsNavActiveColor { get; set; }

        public int StepsProgressSize { get; set; }

        public int InlineDotSize { get; set; }

        public string InlineTitleColor { get; set; }

        public string InlineTailColor { get; set; }

    }

    public partial class Steps
    {
        public CSSObject GenStepsItemStatusStyle(StepItemStatusEnum status, StepsToken token)
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
                    BackgroundColor = token[iconBgColorKey],
                    BorderColor = token[iconBorderColorKey],
                    [$"> {token.ComponentCls}-icon"] = new CSSObject()
                    {
                        Color = token[iconColorKey],
                        [$"{token.ComponentCls}-icon-dot"] = new CSSObject()
                        {
                            Background = token[dotColorKey],
                        },
                    },
                },
                [$"{prefix}-{status}{prefix}-custom {prefix}-icon"] = new CSSObject()
                {
                    [$"> {token.ComponentCls}-icon"] = new CSSObject()
                    {
                        Color = token[dotColorKey],
                    },
                },
                [$"{prefix}-{status} > {prefix}-container > {prefix}-content > {prefix}-title"] = new CSSObject()
                {
                    Color = token[titleColorKey],
                    ["&::after"] = new CSSObject()
                    {
                        BackgroundColor = token[tailColorKey],
                    },
                },
                [$"{prefix}-{status} > {prefix}-container > {prefix}-content > {prefix}-description"] = new CSSObject()
                {
                    Color = token[descriptionColorKey],
                },
                [$"{prefix}-{status} > {prefix}-container > {prefix}-tail::after"] = new CSSObject()
                {
                    BackgroundColor = token[tailColorKey],
                },
            };
        }

        public Unknown_1 GenStepsItemStyle(Unknown_15 token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var stepsItemCls = @$"{componentCls}-item";
            return new Unknown_16()
            {
                [stepsItemCls] = new Unknown_17()
                {
                    Position = "relative",
                    Display = "inline-block",
                    Flex = 1,
                    Overflow = "hidden",
                    VerticalAlign = "top",
                    ["&:last-child"] = new Unknown_18()
                    {
                        Flex = "none",
                        [$"> {stepsItemCls}-container > {stepsItemCls}-tail, > {stepsItemCls}-container >  {stepsItemCls}-content > {stepsItemCls}-title::after"] = new Unknown_19()
                        {
                            Display = "none",
                        },
                    },
                },
                [$"{stepsItemCls}-container"] = new Unknown_20()
                {
                    Outline = "none",
                },
                [$"{stepsItemCls}-icon, {stepsItemCls}-content"] = new Unknown_21()
                {
                    Display = "inline-block",
                    VerticalAlign = "top",
                },
                [$"{stepsItemCls}-icon"] = new Unknown_22()
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
                    [$"{componentCls}-icon"] = new Unknown_23()
                    {
                        Position = "relative",
                        Top = token.IconTop,
                        Color = token.ColorPrimary,
                        LineHeight = 1,
                    },
                },
                [$"{stepsItemCls}-tail"] = new Unknown_24()
                {
                    Position = "absolute",
                    Top = token.IconSize / 2 - token.PaddingXXS,
                    InsetInlineStart = 0,
                    Width = "100%",
                    ["&::after"] = new Unknown_25()
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
                [$"{stepsItemCls}-title"] = new Unknown_26()
                {
                    Position = "relative",
                    Display = "inline-block",
                    PaddingInlineEnd = token.Padding,
                    Color = token.ColorText,
                    FontSize = token.FontSizeLG,
                    LineHeight = @$"{token.TitleLineHeight}px",
                    ["&::after"] = new Unknown_27()
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
                [$"{stepsItemCls}-subtitle"] = new Unknown_28()
                {
                    Display = "inline",
                    MarginInlineStart = token.MarginXS,
                    Color = token.ColorTextDescription,
                    FontWeight = "normal",
                    FontSize = token.FontSize,
                },
                [$"{stepsItemCls}-description"] = new Unknown_29()
                {
                    Color = token.ColorTextDescription,
                    FontSize = token.FontSize,
                },
                ["..."] = GenStepsItemStatusStyle(StepItemStatusEnum.Wait, token),
                ["..."] = GenStepsItemStatusStyle(StepItemStatusEnum.Process, token),
                [$"{stepsItemCls}-process > {stepsItemCls}-container > {stepsItemCls}-title"] = new Unknown_30()
                {
                    FontWeight = token.FontWeightStrong,
                },
                ["..."] = GenStepsItemStatusStyle(StepItemStatusEnum.Finish, token),
                ["..."] = GenStepsItemStatusStyle(StepItemStatusEnum.Error, token),
                [$"{stepsItemCls}{componentCls}-next-error > {componentCls}-item-title::after"] = new Unknown_31()
                {
                    Background = token.ColorError,
                },
                [$"{stepsItemCls}-disabled"] = new Unknown_32()
                {
                    Cursor = "not-allowed",
                },
            };
        }

        public Unknown_2 GenStepsClickableStyle(Unknown_33 token)
        {
            var componentCls = token.ComponentCls;
            var motionDurationSlow = token.MotionDurationSlow;
            return new Unknown_34()
            {
                [$"& {componentCls}-item"] = new Unknown_35()
                {
                    [$"&:not({componentCls}-item-active)"] = new Unknown_36()
                    {
                        [$"& > {componentCls}-item-container[role=\"button\"]"] = new Unknown_37()
                        {
                            Cursor = "pointer",
                            [$"{componentCls}-item"] = new Unknown_38()
                            {
                                [$"&-title, &-subtitle, &-description, &-icon {componentCls}-icon"] = new Unknown_39()
                                {
                                    Transition = @$"color {motionDurationSlow}",
                                },
                            },
                            ["&:hover"] = new Unknown_40()
                            {
                                [$"{componentCls}-item"] = new Unknown_41()
                                {
                                    ["&-title, &-subtitle, &-description"] = new Unknown_42()
                                    {
                                        Color = token.ColorPrimary,
                                    },
                                },
                            },
                        },
                        [$"&:not({componentCls}-item-process)"] = new Unknown_43()
                        {
                            [$"& > {componentCls}-item-container[role=\"button\"]:hover"] = new Unknown_44()
                            {
                                [$"{componentCls}-item"] = new Unknown_45()
                                {
                                    ["&-icon"] = new Unknown_46()
                                    {
                                        BorderColor = token.ColorPrimary,
                                        [$"{componentCls}-icon"] = new Unknown_47()
                                        {
                                            Color = token.ColorPrimary,
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
                [$"&{componentCls}-horizontal:not({componentCls}-label-vertical)"] = new Unknown_48()
                {
                    [$"{componentCls}-item"] = new Unknown_49()
                    {
                        PaddingInlineStart = token.Padding,
                        WhiteSpace = "nowrap",
                        ["&:first-child"] = new Unknown_50()
                        {
                            PaddingInlineStart = 0,
                        },
                        [$"&:last-child {componentCls}-item-title"] = new Unknown_51()
                        {
                            PaddingInlineEnd = 0,
                        },
                        ["&-tail"] = new Unknown_52()
                        {
                            Display = "none",
                        },
                        ["&-description"] = new Unknown_53()
                        {
                            MaxWidth = token.DescriptionMaxWidth,
                            WhiteSpace = "normal",
                        },
                    },
                },
            };
        }

        public Unknown_3 GenStepsStyle(Unknown_54 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_55()
            {
                [componentCls] = new Unknown_56()
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

        public Unknown_4 GenComponentStyleHook(Unknown_57 token)
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
                new Unknown_58()
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
            return new Unknown_59 { GenStepsStyle(stepsToken) };
        }

        public Unknown_5 GenComponentStyleHook(Unknown_60 token)
        {
            var colorTextDisabled = token.ColorTextDisabled;
            var fontSize = token.FontSize;
            var controlHeightSM = token.ControlHeightSM;
            var controlHeight = token.ControlHeight;
            var controlHeightLG = token.ControlHeightLG;
            var fontSizeHeading3 = token.FontSizeHeading3;
            return new Unknown_61()
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
        }

        public Unknown_6 GenStepsCustomIconStyle(Unknown_62 token)
        {
            var componentCls = token.ComponentCls;
            var customIconTop = token.CustomIconTop;
            var customIconSize = token.CustomIconSize;
            var customIconFontSize = token.CustomIconFontSize;
            return new Unknown_63()
            {
                [$"{componentCls}-item-custom"] = new Unknown_64()
                {
                    [$"> {componentCls}-item-container > {componentCls}-item-icon"] = new Unknown_65()
                    {
                        Height = "auto",
                        Background = "none",
                        Border = 0,
                        [$"> {componentCls}-icon"] = new Unknown_66()
                        {
                            Top = customIconTop,
                            Width = customIconSize,
                            Height = customIconSize,
                            FontSize = customIconFontSize,
                            LineHeight = @$"{customIconFontSize}px",
                        },
                    },
                },
                [$"&:not({componentCls}-vertical)"] = new Unknown_67()
                {
                    [$"{componentCls}-item-custom"] = new Unknown_68()
                    {
                        [$"{componentCls}-item-icon"] = new Unknown_69()
                        {
                            Width = "auto",
                            Background = "none",
                        },
                    },
                },
            };
        }

        public Unknown_7 GenStepsInlineStyle(Unknown_70 token)
        {
            var componentCls = token.ComponentCls;
            var inlineDotSize = token.InlineDotSize;
            var inlineTitleColor = token.InlineTitleColor;
            var inlineTailColor = token.InlineTailColor;
            var containerPaddingTop = token.PaddingXS + token.LineWidth;
            var titleStyle = new Unknown_71()
            {
                [$"{componentCls}-item-container {componentCls}-item-content {componentCls}-item-title"] = new Unknown_72()
                {
                    Color = inlineTitleColor,
                },
            };
            return new Unknown_73()
            {
                [$"&{componentCls}-inline"] = new Unknown_74()
                {
                    Width = "auto",
                    Display = "inline-flex",
                    [$"{componentCls}-item"] = new Unknown_75()
                    {
                        Flex = "none",
                        ["&-container"] = new Unknown_76()
                        {
                            Padding = @$"{containerPaddingTop}px {token.PaddingXXS}px 0",
                            Margin = @$"0 {token.MarginXXS / 2}px",
                            BorderRadius = token.BorderRadiusSM,
                            Cursor = "pointer",
                            Transition = @$"background-color {token.MotionDurationMid}",
                            ["&:hover"] = new Unknown_77()
                            {
                                Background = token.ControlItemBgHover,
                            },
                            ["&[role=\"button\"]:hover"] = new Unknown_78()
                            {
                                Opacity = 1,
                            },
                        },
                        ["&-icon"] = new Unknown_79()
                        {
                            Width = inlineDotSize,
                            Height = inlineDotSize,
                            MarginInlineStart = @$"calc(50% - {inlineDotSize / 2}px)",
                            [$"> {componentCls}-icon"] = new Unknown_80()
                            {
                                Top = 0,
                            },
                            [$"{componentCls}-icon-dot"] = new Unknown_81()
                            {
                                BorderRadius = token.FontSizeSM / 4,
                            },
                        },
                        ["&-content"] = new Unknown_82()
                        {
                            Width = "auto",
                            MarginTop = token.MarginXS - token.LineWidth,
                        },
                        ["&-title"] = new Unknown_83()
                        {
                            Color = inlineTitleColor,
                            FontSize = token.FontSizeSM,
                            LineHeight = token.LineHeightSM,
                            FontWeight = "normal",
                            MarginBottom = token.MarginXXS / 2,
                        },
                        ["&-description"] = new Unknown_84()
                        {
                            Display = "none",
                        },
                        ["&-tail"] = new Unknown_85()
                        {
                            MarginInlineStart = 0,
                            Top = containerPaddingTop + inlineDotSize / 2,
                            Transform = @$"translateY(-50%)",
                            ["&:after"] = new Unknown_86()
                            {
                                Width = "100%",
                                Height = token.LineWidth,
                                BorderRadius = 0,
                                MarginInlineStart = 0,
                                Background = inlineTailColor,
                            },
                        },
                        [$"&:first-child {componentCls}-item-tail"] = new Unknown_87()
                        {
                            Width = "50%",
                            MarginInlineStart = "50%",
                        },
                        [$"&:last-child {componentCls}-item-tail"] = new Unknown_88()
                        {
                            Display = "block",
                            Width = "50%",
                        },
                        ["&-wait"] = new Unknown_89()
                        {
                            [$"{componentCls}-item-icon {componentCls}-icon {componentCls}-icon-dot"] = new Unknown_90()
                            {
                                BackgroundColor = token.ColorBorderBg,
                                Border = @$"{token.LineWidth}px {token.LineType} {inlineTailColor}",
                            },
                            ["..."] = titleStyle,
                        },
                        ["&-finish"] = new Unknown_91()
                        {
                            [$"{componentCls}-item-tail::after"] = new Unknown_92()
                            {
                                BackgroundColor = inlineTailColor,
                            },
                            [$"{componentCls}-item-icon {componentCls}-icon {componentCls}-icon-dot"] = new Unknown_93()
                            {
                                BackgroundColor = inlineTailColor,
                                Border = @$"{token.LineWidth}px {token.LineType} {inlineTailColor}",
                            },
                            ["..."] = titleStyle,
                        },
                        ["&-error"] = titleStyle,
                        ["&-active, &-process"] = new Unknown_94()
                        {
                            [$"{componentCls}-item-icon"] = new Unknown_95()
                            {
                                Width = inlineDotSize,
                                Height = inlineDotSize,
                                MarginInlineStart = @$"calc(50% - {inlineDotSize / 2}px)",
                                Top = 0,
                            },
                            ["..."] = titleStyle,
                        },
                        [$"&:not({componentCls}-item-active) > {componentCls}-item-container[role=\"button\"]:hover"] = new Unknown_96()
                        {
                            [$"{componentCls}-item-title"] = new Unknown_97()
                            {
                                Color = inlineTitleColor,
                            },
                        },
                    },
                },
            };
        }

        public Unknown_8 GenStepsLabelPlacementStyle(Unknown_98 token)
        {
            var componentCls = token.ComponentCls;
            var iconSize = token.IconSize;
            var lineHeight = token.LineHeight;
            var iconSizeSM = token.IconSizeSM;
            return new Unknown_99()
            {
                [$"&{componentCls}-label-vertical"] = new Unknown_100()
                {
                    [$"{componentCls}-item"] = new Unknown_101()
                    {
                        Overflow = "visible",
                        ["&-tail"] = new Unknown_102()
                        {
                            MarginInlineStart = iconSize / 2 + token.ControlHeightLG,
                            Padding = @$"{token.PaddingXXS}px {token.PaddingLG}px",
                        },
                        ["&-content"] = new Unknown_103()
                        {
                            Display = "block",
                            Width = (iconSize / 2 + token.ControlHeightLG) * 2,
                            MarginTop = token.MarginSM,
                            TextAlign = "center",
                        },
                        ["&-icon"] = new Unknown_104()
                        {
                            Display = "inline-block",
                            MarginInlineStart = token.ControlHeightLG,
                        },
                        ["&-title"] = new Unknown_105()
                        {
                            PaddingInlineEnd = 0,
                            PaddingInlineStart = 0,
                            ["&::after"] = new Unknown_106()
                            {
                                Display = "none",
                            },
                        },
                        ["&-subtitle"] = new Unknown_107()
                        {
                            Display = "block",
                            MarginBottom = token.MarginXXS,
                            MarginInlineStart = 0,
                            LineHeight = lineHeight,
                        },
                    },
                    [$"&{componentCls}-small:not({componentCls}-dot)"] = new Unknown_108()
                    {
                        [$"{componentCls}-item"] = new Unknown_109()
                        {
                            ["&-icon"] = new Unknown_110()
                            {
                                MarginInlineStart = token.ControlHeightLG + (iconSize - iconSizeSM) / 2,
                            },
                        },
                    },
                },
            };
        }

        public Unknown_9 GenStepsNavStyle(Unknown_111 token)
        {
            var componentCls = token.ComponentCls;
            var navContentMaxWidth = token.NavContentMaxWidth;
            var navArrowColor = token.NavArrowColor;
            var stepsNavActiveColor = token.StepsNavActiveColor;
            var motionDurationSlow = token.MotionDurationSlow;
            return new Unknown_112()
            {
                [$"&{componentCls}-navigation"] = new Unknown_113()
                {
                    PaddingTop = token.PaddingSM,
                    [$"&{componentCls}-small"] = new Unknown_114()
                    {
                        [$"{componentCls}-item"] = new Unknown_115()
                        {
                            ["&-container"] = new Unknown_116()
                            {
                                MarginInlineStart = -token.MarginSM,
                            },
                        },
                    },
                    [$"{componentCls}-item"] = new Unknown_117()
                    {
                        Overflow = "visible",
                        TextAlign = "center",
                        ["&-container"] = new Unknown_118()
                        {
                            Display = "inline-block",
                            Height = "100%",
                            MarginInlineStart = -token.Margin,
                            PaddingBottom = token.PaddingSM,
                            TextAlign = "start",
                            Transition = @$"opacity {motionDurationSlow}",
                            [$"{componentCls}-item-content"] = new Unknown_119()
                            {
                                MaxWidth = navContentMaxWidth,
                            },
                            [$"{componentCls}-item-title"] = new Unknown_120()
                            {
                                MaxWidth = "100%",
                                PaddingInlineEnd = 0,
                                ["..."] = textEllipsis,
                                ["&::after"] = new Unknown_121()
                                {
                                    Display = "none",
                                },
                            },
                        },
                        [$"&:not({componentCls}-item-active)"] = new Unknown_122()
                        {
                            [$"{componentCls}-item-container[role=\"button\"]"] = new Unknown_123()
                            {
                                Cursor = "pointer",
                                ["&:hover"] = new Unknown_124()
                                {
                                    Opacity = 0.85f,
                                },
                            },
                        },
                        ["&:last-child"] = new Unknown_125()
                        {
                            Flex = 1,
                            ["&::after"] = new Unknown_126()
                            {
                                Display = "none",
                            },
                        },
                        ["&::after"] = new Unknown_127()
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
                        ["&::before"] = new Unknown_128()
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
                    [$"{componentCls}-item{componentCls}-item-active::before"] = new Unknown_129()
                    {
                        InsetInlineStart = 0,
                        Width = "100%",
                    },
                },
                [$"&{componentCls}-navigation{componentCls}-vertical"] = new Unknown_130()
                {
                    [$"> {componentCls}-item"] = new Unknown_131()
                    {
                        MarginInlineEnd = 0,
                        ["&::before"] = new Unknown_132()
                        {
                            Display = "none",
                        },
                        [$"&{componentCls}-item-active::before"] = new Unknown_133()
                        {
                            Top = 0,
                            InsetInlineEnd = 0,
                            InsetInlineStart = "unset",
                            Display = "block",
                            Width = token.LineWidth * 3,
                            Height = @$"calc(100% - {token.MarginLG}px)",
                        },
                        ["&::after"] = new Unknown_134()
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
                        [$"> {componentCls}-item-container > {componentCls}-item-tail"] = new Unknown_135()
                        {
                            Visibility = "hidden",
                        },
                    },
                },
                [$"&{componentCls}-navigation{componentCls}-horizontal"] = new Unknown_136()
                {
                    [$"> {componentCls}-item > {componentCls}-item-container > {componentCls}-item-tail"] = new Unknown_137()
                    {
                        Visibility = "hidden",
                    },
                },
            };
        }

        public Unknown_10 GenStepsProgressDotStyle(Unknown_138 token)
        {
            var componentCls = token.ComponentCls;
            var descriptionMaxWidth = token.DescriptionMaxWidth;
            var lineHeight = token.LineHeight;
            var dotCurrentSize = token.DotCurrentSize;
            var dotSize = token.DotSize;
            var motionDurationSlow = token.MotionDurationSlow;
            return new Unknown_139()
            {
                [$"&{componentCls}-dot, &{componentCls}-dot{componentCls}-small"] = new Unknown_140()
                {
                    [$"{componentCls}-item"] = new Unknown_141()
                    {
                        ["&-title"] = new Unknown_142()
                        {
                            LineHeight = lineHeight,
                        },
                        ["&-tail"] = new Unknown_143()
                        {
                            Top = Math.Floor((token.DotSize - token.LineWidth * 3) / 2),
                            Width = "100%",
                            MarginTop = 0,
                            MarginBottom = 0,
                            MarginInline = @$"{descriptionMaxWidth / 2}px 0",
                            Padding = 0,
                            ["&::after"] = new Unknown_144()
                            {
                                Width = @$"calc(100% - {token.MarginSM * 2}px)",
                                Height = token.LineWidth * 3,
                                MarginInlineStart = token.MarginSM,
                            },
                        },
                        ["&-icon"] = new Unknown_145()
                        {
                            Width = dotSize,
                            Height = dotSize,
                            MarginInlineStart = (token.DescriptionMaxWidth - dotSize) / 2,
                            PaddingInlineEnd = 0,
                            LineHeight = @$"{dotSize}px",
                            Background = "transparent",
                            Border = 0,
                            [$"{componentCls}-icon-dot"] = new Unknown_146()
                            {
                                Position = "relative",
                                Float = "left",
                                Width = "100%",
                                Height = "100%",
                                BorderRadius = 100,
                                Transition = @$"all {motionDurationSlow}",
                                ["&::after"] = new Unknown_147()
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
                        ["&-content"] = new Unknown_148()
                        {
                            Width = descriptionMaxWidth,
                        },
                        [$"&-process {componentCls}-item-icon"] = new Unknown_149()
                        {
                            Position = "relative",
                            Top = (dotSize - dotCurrentSize) / 2,
                            Width = dotCurrentSize,
                            Height = dotCurrentSize,
                            LineHeight = @$"{dotCurrentSize}px",
                            Background = "none",
                            MarginInlineStart = (token.DescriptionMaxWidth - dotCurrentSize) / 2,
                        },
                        [$"&-process {componentCls}-icon"] = new Unknown_150()
                        {
                            [$"&:first-child {componentCls}-icon-dot"] = new Unknown_151()
                            {
                                InsetInlineStart = 0,
                            },
                        },
                    },
                },
                [$"&{componentCls}-vertical{componentCls}-dot"] = new Unknown_152()
                {
                    [$"{componentCls}-item-icon"] = new Unknown_153()
                    {
                        MarginTop = (token.ControlHeight - dotSize) / 2,
                        MarginInlineStart = 0,
                        Background = "none",
                    },
                    [$"{componentCls}-item-process {componentCls}-item-icon"] = new Unknown_154()
                    {
                        MarginTop = (token.ControlHeight - dotCurrentSize) / 2,
                        Top = 0,
                        InsetInlineStart = (dotSize - dotCurrentSize) / 2,
                        MarginInlineStart = 0,
                    },
                    [$"{componentCls}-item > {componentCls}-item-container > {componentCls}-item-tail"] = new Unknown_155()
                    {
                        Top = (token.ControlHeight - dotSize) / 2,
                        InsetInlineStart = 0,
                        Margin = 0,
                        Padding = @$"{dotSize + token.PaddingXS}px 0 {token.PaddingXS}px",
                        ["&::after"] = new Unknown_156()
                        {
                            MarginInlineStart = (dotSize - token.LineWidth) / 2,
                        },
                    },
                    [$"&{componentCls}-small"] = new Unknown_157()
                    {
                        [$"{componentCls}-item-icon"] = new Unknown_158()
                        {
                            MarginTop = (token.ControlHeightSM - dotSize) / 2,
                        },
                        [$"{componentCls}-item-process {componentCls}-item-icon"] = new Unknown_159()
                        {
                            MarginTop = (token.ControlHeightSM - dotCurrentSize) / 2,
                        },
                        [$"{componentCls}-item > {componentCls}-item-container > {componentCls}-item-tail"] = new Unknown_160()
                        {
                            Top = (token.ControlHeightSM - dotSize) / 2,
                        },
                    },
                    [$"{componentCls}-item:first-child {componentCls}-icon-dot"] = new Unknown_161()
                    {
                        InsetInlineStart = 0,
                    },
                    [$"{componentCls}-item-content"] = new Unknown_162()
                    {
                        Width = "inherit",
                    },
                },
            };
        }

        public Unknown_11 GenStepsProgressStyle(Unknown_163 token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            return new Unknown_164()
            {
                [$"&{componentCls}-with-progress"] = new Unknown_165()
                {
                    [$"{componentCls}-item"] = new Unknown_166()
                    {
                        PaddingTop = token.PaddingXXS,
                        [$"&-process {componentCls}-item-container {componentCls}-item-icon {componentCls}-icon"] = new Unknown_167()
                        {
                            Color = token.ProcessIconColor,
                        },
                    },
                    [$"&{componentCls}-vertical > {componentCls}-item "] = new Unknown_168()
                    {
                        PaddingInlineStart = token.PaddingXXS,
                        [$"> {componentCls}-item-container > {componentCls}-item-tail"] = new Unknown_169()
                        {
                            Top = token.MarginXXS,
                            InsetInlineStart = token.IconSize / 2 - token.LineWidth + token.PaddingXXS,
                        },
                    },
                    [$"&, &{componentCls}-small"] = new Unknown_170()
                    {
                        [$"&{componentCls}-horizontal {componentCls}-item:first-child"] = new Unknown_171()
                        {
                            PaddingBottom = token.PaddingXXS,
                            PaddingInlineStart = token.PaddingXXS,
                        },
                    },
                    [$"&{componentCls}-small{componentCls}-vertical > {componentCls}-item > {componentCls}-item-container > {componentCls}-item-tail"] = new Unknown_172()
                    {
                        InsetInlineStart = token.IconSizeSM / 2 - token.LineWidth + token.PaddingXXS,
                    },
                    [$"&{componentCls}-label-vertical"] = new Unknown_173()
                    {
                        [$"{componentCls}-item {componentCls}-item-tail"] = new Unknown_174()
                        {
                            Top = token.Margin - 2 * token.LineWidth,
                        },
                    },
                    [$"{componentCls}-item-icon"] = new Unknown_175()
                    {
                        Position = "relative",
                        [$"{antCls}-progress"] = new Unknown_176()
                        {
                            Position = "absolute",
                            InsetBlockStart = (token.IconSize - token.StepsProgressSize - token.LineWidth * 2) / 2,
                            InsetInlineStart = (token.IconSize - token.StepsProgressSize - token.LineWidth * 2) / 2,
                        },
                    },
                },
            };
        }

        public Unknown_12 GenStepsRTLStyle(Unknown_177 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_178()
            {
                [$"&{componentCls}-rtl"] = new Unknown_179()
                {
                    Direction = "rtl",
                    [$"{componentCls}-item"] = new Unknown_180()
                    {
                        ["&-subtitle"] = new Unknown_181()
                        {
                            Float = "left",
                        },
                    },
                    [$"&{componentCls}-navigation"] = new Unknown_182()
                    {
                        [$"{componentCls}-item::after"] = new Unknown_183()
                        {
                            Transform = "rotate(-45deg)",
                        },
                    },
                    [$"&{componentCls}-vertical"] = new Unknown_184()
                    {
                        [$"> {componentCls}-item"] = new Unknown_185()
                        {
                            ["&::after"] = new Unknown_186()
                            {
                                Transform = "rotate(225deg)",
                            },
                            [$"{componentCls}-item-icon"] = new Unknown_187()
                            {
                                Float = "right",
                            },
                        },
                    },
                    [$"&{componentCls}-dot"] = new Unknown_188()
                    {
                        [$"{componentCls}-item-icon {componentCls}-icon-dot, &{componentCls}-small {componentCls}-item-icon {componentCls}-icon-dot"] = new Unknown_189()
                        {
                            Float = "right",
                        },
                    },
                },
            };
        }

        public Unknown_13 GenStepsSmallStyle(Unknown_190 token)
        {
            var componentCls = token.ComponentCls;
            var iconSizeSM = token.IconSizeSM;
            var fontSizeSM = token.FontSizeSM;
            var fontSize = token.FontSize;
            var colorTextDescription = token.ColorTextDescription;
            return new Unknown_191()
            {
                [$"&{componentCls}-small"] = new Unknown_192()
                {
                    [$"&{componentCls}-horizontal:not({componentCls}-label-vertical) {componentCls}-item"] = new Unknown_193()
                    {
                        PaddingInlineStart = token.PaddingSM,
                        ["&:first-child"] = new Unknown_194()
                        {
                            PaddingInlineStart = 0,
                        },
                    },
                    [$"{componentCls}-item-icon"] = new Unknown_195()
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
                    [$"{componentCls}-item-title"] = new Unknown_196()
                    {
                        PaddingInlineEnd = token.PaddingSM,
                        FontSize = fontSize,
                        LineHeight = @$"{iconSizeSM}px",
                        ["&::after"] = new Unknown_197()
                        {
                            Top = iconSizeSM / 2,
                        },
                    },
                    [$"{componentCls}-item-description"] = new Unknown_198()
                    {
                        Color = colorTextDescription,
                        FontSize = fontSize,
                    },
                    [$"{componentCls}-item-tail"] = new Unknown_199()
                    {
                        Top = iconSizeSM / 2 - token.PaddingXXS,
                    },
                    [$"{componentCls}-item-custom {componentCls}-item-icon"] = new Unknown_200()
                    {
                        Width = "inherit",
                        Height = "inherit",
                        LineHeight = "inherit",
                        Background = "none",
                        Border = 0,
                        BorderRadius = 0,
                        [$"> {componentCls}-icon"] = new Unknown_201()
                        {
                            FontSize = iconSizeSM,
                            LineHeight = @$"{iconSizeSM}px",
                            Transform = "none",
                        },
                    },
                },
            };
        }

        public Unknown_14 GenStepsVerticalStyle(Unknown_202 token)
        {
            var componentCls = token.ComponentCls;
            var iconSizeSM = token.IconSizeSM;
            var iconSize = token.IconSize;
            return new Unknown_203()
            {
                [$"&{componentCls}-vertical"] = new Unknown_204()
                {
                    Display = "flex",
                    FlexDirection = "column",
                    [$"> {componentCls}-item"] = new Unknown_205()
                    {
                        Display = "block",
                        Flex = "1 0 auto",
                        PaddingInlineStart = 0,
                        Overflow = "visible",
                        [$"{componentCls}-item-icon"] = new Unknown_206()
                        {
                            Float = "left",
                            MarginInlineEnd = token.Margin,
                        },
                        [$"{componentCls}-item-content"] = new Unknown_207()
                        {
                            Display = "block",
                            MinHeight = token.ControlHeight * 1.5,
                            Overflow = "hidden",
                        },
                        [$"{componentCls}-item-title"] = new Unknown_208()
                        {
                            LineHeight = @$"{iconSize}px",
                        },
                        [$"{componentCls}-item-description"] = new Unknown_209()
                        {
                            PaddingBottom = token.PaddingSM,
                        },
                    },
                    [$"> {componentCls}-item > {componentCls}-item-container > {componentCls}-item-tail"] = new Unknown_210()
                    {
                        Position = "absolute",
                        Top = 0,
                        InsetInlineStart = iconSize / 2 - token.LineWidth,
                        Width = token.LineWidth,
                        Height = "100%",
                        Padding = @$"{iconSize + token.MarginXXS * 1.5}px 0 {token.MarginXXS * 1.5}px",
                        ["&::after"] = new Unknown_211()
                        {
                            Width = token.LineWidth,
                            Height = "100%",
                        },
                    },
                    [$"> {componentCls}-item:not(:last-child) > {componentCls}-item-container > {componentCls}-item-tail"] = new Unknown_212()
                    {
                        Display = "block",
                    },
                    [$" > {componentCls}-item > {componentCls}-item-container > {componentCls}-item-content > {componentCls}-item-title"] = new Unknown_213()
                    {
                        ["&::after"] = new Unknown_214()
                        {
                            Display = "none",
                        },
                    },
                    [$"&{componentCls}-small {componentCls}-item-container"] = new Unknown_215()
                    {
                        [$"{componentCls}-item-tail"] = new Unknown_216()
                        {
                            Position = "absolute",
                            Top = 0,
                            InsetInlineStart = iconSizeSM / 2 - token.LineWidth,
                            Padding = @$"{iconSizeSM + token.MarginXXS * 1.5}px 0 {token.MarginXXS * 1.5}px",
                        },
                        [$"{componentCls}-item-title"] = new Unknown_217()
                        {
                            LineHeight = @$"{iconSizeSM}px",
                        },
                    },
                },
            };
        }

    }

}