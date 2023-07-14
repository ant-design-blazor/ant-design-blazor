using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class UploadToken
    {
    }

    public partial class UploadToken : TokenWithCommonCls
    {
        public int UploadThumbnailSize { get; set; }

        public int UploadProgressOffset { get; set; }

        public int UploadPicCardSize { get; set; }

    }

    public partial class Upload
    {
        private Keyframes uploadAnimateInlineIn = new Keyframes("uploadAnimateInlineIn")
        {
            From = new Keyframes()
            {
                Width = 0,
                Height = 0,
                Margin = 0,
                Padding = 0,
                Opacity = 0,
            },
        };

        private Keyframes uploadAnimateInlineOut = new Keyframes("uploadAnimateInlineOut")
        {
            To = new Keyframes()
            {
                Width = 0,
                Height = 0,
                Margin = 0,
                Padding = 0,
                Opacity = 0,
            },
        };

        public Unknown_1 GenBaseStyle(Unknown_9 token)
        {
            var componentCls = token.ComponentCls;
            var colorTextDisabled = token.ColorTextDisabled;
            return new Unknown_10()
            {
                [$"{componentCls}-wrapper"] = new Unknown_11()
                {
                    ["..."] = ResetComponent(token),
                    [componentCls] = new Unknown_12()
                    {
                        Outline = 0,
                        ["input[type=\"file\"]"] = new Unknown_13()
                        {
                            Cursor = "pointer",
                        },
                    },
                    [$"{componentCls}-select"] = new Unknown_14()
                    {
                        Display = "inline-block",
                    },
                    [$"{componentCls}-disabled"] = new Unknown_15()
                    {
                        Color = colorTextDisabled,
                        Cursor = "not-allowed",
                    },
                },
            };
        }

        public Unknown_2 GenComponentStyleHook(Unknown_16 token)
        {
            var fontSizeHeading3 = token.FontSizeHeading3;
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var lineWidth = token.LineWidth;
            var controlHeightLG = token.ControlHeightLG;
            var listItemHeightSM = Math.Round(fontSize * lineHeight);
            var uploadToken = MergeToken(
                token,
                new Unknown_17()
                {
                    UploadThumbnailSize = fontSizeHeading3 * 2,
                    UploadProgressOffset = listItemHeightSM / 2 + lineWidth,
                    UploadPicCardSize = controlHeightLG * 2.55,
                });
            return new Unknown_18
            {
                GenBaseStyle(uploadToken),
                GenDraggerStyle(uploadToken),
                GenPictureStyle(uploadToken),
                GenPictureCardStyle(uploadToken),
                GenListStyle(uploadToken),
                GenMotionStyle(uploadToken),
                GenRtlStyle(uploadToken),
                GenCollapseMotion(uploadToken)
            };
        }

        public Unknown_3 GenDraggerStyle(Unknown_19 token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            return new Unknown_20()
            {
                [$"{componentCls}-wrapper"] = new Unknown_21()
                {
                    [$"{componentCls}-drag"] = new Unknown_22()
                    {
                        Position = "relative",
                        Width = "100%",
                        Height = "100%",
                        TextAlign = "center",
                        Background = token.ColorFillAlter,
                        Border = @$"{token.LineWidth}px dashed {token.ColorBorder}",
                        BorderRadius = token.BorderRadiusLG,
                        Cursor = "pointer",
                        Transition = @$"border-color {token.MotionDurationSlow}",
                        [componentCls] = new Unknown_23()
                        {
                            Padding = @$"{token.Padding}px 0",
                        },
                        [$"{componentCls}-btn"] = new Unknown_24()
                        {
                            Display = "table",
                            Width = "100%",
                            Height = "100%",
                            Outline = "none",
                        },
                        [$"{componentCls}-drag-container"] = new Unknown_25()
                        {
                            Display = "table-cell",
                            VerticalAlign = "middle",
                        },
                        [$"&:not({componentCls}-disabled):hover"] = new Unknown_26()
                        {
                            BorderColor = token.ColorPrimaryHover,
                        },
                        [$"p{componentCls}-drag-icon"] = new Unknown_27()
                        {
                            MarginBottom = token.Margin,
                            [iconCls] = new Unknown_28()
                            {
                                Color = token.ColorPrimary,
                                FontSize = token.UploadThumbnailSize,
                            },
                        },
                        [$"p{componentCls}-text"] = new Unknown_29()
                        {
                            Margin = @$"0 0 {token.MarginXXS}px",
                            Color = token.ColorTextHeading,
                            FontSize = token.FontSizeLG,
                        },
                        [$"p{componentCls}-hint"] = new Unknown_30()
                        {
                            Color = token.ColorTextDescription,
                            FontSize = token.FontSize,
                        },
                        [$"&{componentCls}-disabled"] = new Unknown_31()
                        {
                            Cursor = "not-allowed",
                            [$"p{componentCls}-drag-icon{iconCls},p{componentCls}-text,p{componentCls}-hint"] = new Unknown_32()
                            {
                                Color = token.ColorTextDisabled,
                            },
                        },
                    },
                },
            };
        }

        public Unknown_4 GenListStyle(Unknown_33 token)
        {
            var componentCls = token.ComponentCls;
            var antCls = token.AntCls;
            var iconCls = token.IconCls;
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var itemCls = @$"{componentCls}-list-item";
            var actionsCls = @$"{itemCls}-actions";
            var actionCls = @$"{itemCls}-action";
            var listItemHeightSM = Math.Round(fontSize * lineHeight);
            return new Unknown_34()
            {
                [$"{componentCls}-wrapper"] = new Unknown_35()
                {
                    [$"{componentCls}-list"] = new Unknown_36()
                    {
                        ["..."] = ClearFix(),
                        LineHeight = token.LineHeight,
                        [itemCls] = new Unknown_37()
                        {
                            Position = "relative",
                            Height = token.LineHeight * fontSize,
                            MarginTop = token.MarginXS,
                            FontSize = fontSize,
                            Display = "flex",
                            AlignItems = "center",
                            Transition = @$"background-color {token.MotionDurationSlow}",
                            ["&:hover"] = new Unknown_38()
                            {
                                BackgroundColor = token.ControlItemBgHover,
                            },
                            [$"{itemCls}-name"] = new Unknown_39()
                            {
                                ["..."] = textEllipsis,
                                Padding = @$"0 {token.PaddingXS}px",
                                LineHeight = lineHeight,
                                Flex = "auto",
                                Transition = @$"all {token.MotionDurationSlow}",
                            },
                            [actionsCls] = new Unknown_40()
                            {
                                [actionCls] = new Unknown_41()
                                {
                                    Opacity = 0,
                                },
                                [$"{actionCls}{antCls}-btn-sm"] = new Unknown_42()
                                {
                                    Height = listItemHeightSM,
                                    Border = 0,
                                    LineHeight = 1,
                                    ["> span"] = new Unknown_43()
                                    {
                                        Transform = "scale(1)",
                                    },
                                },
                                [$"{actionCls}:focus,&.picture{actionCls}"] = new Unknown_44()
                                {
                                    Opacity = 1,
                                },
                                [iconCls] = new Unknown_45()
                                {
                                    Color = token.ColorTextDescription,
                                    Transition = @$"all {token.MotionDurationSlow}",
                                },
                                [$"&:hover {iconCls}"] = new Unknown_46()
                                {
                                    Color = token.ColorText,
                                },
                            },
                            [$"{componentCls}-icon {iconCls}"] = new Unknown_47()
                            {
                                Color = token.ColorTextDescription,
                                FontSize = fontSize,
                            },
                            [$"{itemCls}-progress"] = new Unknown_48()
                            {
                                Position = "absolute",
                                Bottom = -token.UploadProgressOffset,
                                Width = "100%",
                                PaddingInlineStart = fontSize + token.PaddingXS,
                                FontSize = fontSize,
                                LineHeight = 0,
                                PointerEvents = "none",
                                ["> div"] = new Unknown_49()
                                {
                                    Margin = 0,
                                },
                            },
                        },
                        [$"{itemCls}:hover {actionCls}"] = new Unknown_50()
                        {
                            Opacity = 1,
                            Color = token.ColorText,
                        },
                        [$"{itemCls}-error"] = new Unknown_51()
                        {
                            Color = token.ColorError,
                            [$"{itemCls}-name, {componentCls}-icon {iconCls}"] = new Unknown_52()
                            {
                                Color = token.ColorError,
                            },
                            [actionsCls] = new Unknown_53()
                            {
                                [$"{iconCls}, {iconCls}:hover"] = new Unknown_54()
                                {
                                    Color = token.ColorError,
                                },
                                [actionCls] = new Unknown_55()
                                {
                                    Opacity = 1,
                                },
                            },
                        },
                        [$"{componentCls}-list-item-container"] = new Unknown_56()
                        {
                            Transition = @$"opacity {token.MotionDurationSlow}, height {token.MotionDurationSlow}",
                            ["&::before"] = new Unknown_57()
                            {
                                Display = "table",
                                Width = 0,
                                Height = 0,
                                Content = "\"\"",
                            },
                        },
                    },
                },
            };
        }

        public Unknown_5 GenMotionStyle(Unknown_58 token)
        {
            var componentCls = token.ComponentCls;
            var inlineCls = @$"{componentCls}-animate-inline";
            return new Unknown_59
            {
                new Unknown_60()
                {
                    [$"{componentCls}-wrapper"] = new Unknown_61()
                    {
                        [$"{inlineCls}-appear, {inlineCls}-enter, {inlineCls}-leave"] = new Unknown_62()
                        {
                            AnimationDuration = token.MotionDurationSlow,
                            AnimationTimingFunction = token.MotionEaseInOutCirc,
                            AnimationFillMode = "forwards",
                        },
                        [$"{inlineCls}-appear, {inlineCls}-enter"] = new Unknown_63()
                        {
                            AnimationName = uploadAnimateInlineIn,
                        },
                        [$"{inlineCls}-leave"] = new Unknown_64()
                        {
                            AnimationName = uploadAnimateInlineOut,
                        },
                    },
                },
                UploadAnimateInlineIn,
                UploadAnimateInlineOut
            };
        }

        public Unknown_6 GenPictureStyle(Unknown_65 token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var uploadThumbnailSize = token.UploadThumbnailSize;
            var uploadProgressOffset = token.UploadProgressOffset;
            var listCls = @$"{componentCls}-list";
            var itemCls = @$"{listCls}-item";
            return new Unknown_66()
            {
                [$"{componentCls}-wrapper"] = new Unknown_67()
                {
                    [$"{listCls}{listCls}-picture,{listCls}{listCls}-picture-card,{listCls}{listCls}-picture-circle"] = new Unknown_68()
                    {
                        [itemCls] = new Unknown_69()
                        {
                            Position = "relative",
                            Height = uploadThumbnailSize + token.LineWidth * 2 + token.PaddingXS * 2,
                            Padding = token.PaddingXS,
                            Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                            BorderRadius = token.BorderRadiusLG,
                            ["&:hover"] = new Unknown_70()
                            {
                                Background = "transparent",
                            },
                            [$"{itemCls}-thumbnail"] = new Unknown_71()
                            {
                                ["..."] = textEllipsis,
                                Width = uploadThumbnailSize,
                                Height = uploadThumbnailSize,
                                LineHeight = @$"{uploadThumbnailSize + token.PaddingSM}px",
                                TextAlign = "center",
                                Flex = "none",
                                [iconCls] = new Unknown_72()
                                {
                                    FontSize = token.FontSizeHeading2,
                                    Color = token.ColorPrimary,
                                },
                                ["img"] = new Unknown_73()
                                {
                                    Display = "block",
                                    Width = "100%",
                                    Height = "100%",
                                    Overflow = "hidden",
                                },
                            },
                            [$"{itemCls}-progress"] = new Unknown_74()
                            {
                                Bottom = uploadProgressOffset,
                                Width = @$"calc(100% - {token.PaddingSM * 2}px)",
                                MarginTop = 0,
                                PaddingInlineStart = uploadThumbnailSize + token.PaddingXS,
                            },
                        },
                        [$"{itemCls}-error"] = new Unknown_75()
                        {
                            BorderColor = token.ColorError,
                            [$"{itemCls}-thumbnail {iconCls}"] = new Unknown_76()
                            {
                                ["svg path[fill="#e6f7ff"]"] = new Unknown_77()
                                {
                                    Fill = token.ColorErrorBg,
                                },
                                ["svg path[fill="#1890ff"]"] = new Unknown_78()
                                {
                                    Fill = token.ColorError,
                                },
                            },
                        },
                        [$"{itemCls}-uploading"] = new Unknown_79()
                        {
                            BorderStyle = "dashed",
                            [$"{itemCls}-name"] = new Unknown_80()
                            {
                                MarginBottom = uploadProgressOffset,
                            },
                        },
                    },
                    [$"{listCls}{listCls}-picture-circle {itemCls}"] = new Unknown_81()
                    {
                        [$"&, &::before, {itemCls}-thumbnail"] = new Unknown_82()
                        {
                            BorderRadius = "50%",
                        },
                    },
                },
            };
        }

        public Unknown_7 GenPictureCardStyle(Unknown_83 token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var fontSizeLG = token.FontSizeLG;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var listCls = @$"{componentCls}-list";
            var itemCls = @$"{listCls}-item";
            var uploadPictureCardSize = token.UploadPicCardSize;
            return new Unknown_84()
            {
                [$"{componentCls}-wrapper{componentCls}-picture-card-wrapper,{componentCls}-wrapper{componentCls}-picture-circle-wrapper"] = new Unknown_85()
                {
                    ["..."] = ClearFix(),
                    Display = "inline-block",
                    Width = "100%",
                    [$"{componentCls}{componentCls}-select"] = new Unknown_86()
                    {
                        Width = uploadPictureCardSize,
                        Height = uploadPictureCardSize,
                        MarginInlineEnd = token.MarginXS,
                        MarginBottom = token.MarginXS,
                        TextAlign = "center",
                        VerticalAlign = "top",
                        BackgroundColor = token.ColorFillAlter,
                        Border = @$"{token.LineWidth}px dashed {token.ColorBorder}",
                        BorderRadius = token.BorderRadiusLG,
                        Cursor = "pointer",
                        Transition = @$"border-color {token.MotionDurationSlow}",
                        [$"> {componentCls}"] = new Unknown_87()
                        {
                            Display = "flex",
                            AlignItems = "center",
                            JustifyContent = "center",
                            Height = "100%",
                            TextAlign = "center",
                        },
                        [$"&:not({componentCls}-disabled):hover"] = new Unknown_88()
                        {
                            BorderColor = token.ColorPrimary,
                        },
                    },
                    [$"{listCls}{listCls}-picture-card, {listCls}{listCls}-picture-circle"] = new Unknown_89()
                    {
                        [$"{listCls}-item-container"] = new Unknown_90()
                        {
                            Display = "inline-block",
                            Width = uploadPictureCardSize,
                            Height = uploadPictureCardSize,
                            MarginBlock = @$"0 {token.MarginXS}px",
                            MarginInline = @$"0 {token.MarginXS}px",
                            VerticalAlign = "top",
                        },
                        ["&::after"] = new Unknown_91()
                        {
                            Display = "none",
                        },
                        [itemCls] = new Unknown_92()
                        {
                            Height = "100%",
                            Margin = 0,
                            ["&::before"] = new Unknown_93()
                            {
                                Position = "absolute",
                                ZIndex = 1,
                                Width = @$"calc(100% - {token.PaddingXS * 2}px)",
                                Height = @$"calc(100% - {token.PaddingXS * 2}px)",
                                BackgroundColor = token.ColorBgMask,
                                Opacity = 0,
                                Transition = @$"all {token.MotionDurationSlow}",
                                Content = "\" \"",
                            },
                        },
                        [$"{itemCls}:hover"] = new Unknown_94()
                        {
                            [$"&::before, {itemCls}-actions"] = new Unknown_95()
                            {
                                Opacity = 1,
                            },
                        },
                        [$"{itemCls}-actions"] = new Unknown_96()
                        {
                            Position = "absolute",
                            InsetInlineStart = 0,
                            ZIndex = 10,
                            Width = "100%",
                            WhiteSpace = "nowrap",
                            TextAlign = "center",
                            Opacity = 0,
                            Transition = @$"all {token.MotionDurationSlow}",
                            [$"{iconCls}-eye, {iconCls}-download, {iconCls}-delete"] = new Unknown_97()
                            {
                                ZIndex = 10,
                                Width = fontSizeLG,
                                Margin = @$"0 {token.MarginXXS}px",
                                FontSize = fontSizeLG,
                                Cursor = "pointer",
                                Transition = @$"all {token.MotionDurationSlow}",
                                ["svg"] = new Unknown_98()
                                {
                                    VerticalAlign = "baseline",
                                },
                            },
                        },
                        [$"{itemCls}-actions, {itemCls}-actions:hover"] = new Unknown_99()
                        {
                            [$"{iconCls}-eye, {iconCls}-download, {iconCls}-delete"] = new Unknown_100()
                            {
                                Color = New TinyColor(colorTextLightSolid).setAlpha(0.65).toRgbString(),
                                ["&:hover"] = new Unknown_101()
                                {
                                    Color = colorTextLightSolid,
                                },
                            },
                        },
                        [$"{itemCls}-thumbnail, {itemCls}-thumbnail img"] = new Unknown_102()
                        {
                            Position = "static",
                            Display = "block",
                            Width = "100%",
                            Height = "100%",
                            ObjectFit = "contain",
                        },
                        [$"{itemCls}-name"] = new Unknown_103()
                        {
                            Display = "none",
                            TextAlign = "center",
                        },
                        [$"{itemCls}-file + {itemCls}-name"] = new Unknown_104()
                        {
                            Position = "absolute",
                            Bottom = token.Margin,
                            Display = "block",
                            Width = @$"calc(100% - {token.PaddingXS * 2}px)",
                        },
                        [$"{itemCls}-uploading"] = new Unknown_105()
                        {
                            [$"&{itemCls}"] = new Unknown_106()
                            {
                                BackgroundColor = token.ColorFillAlter,
                            },
                            [$"&::before, {iconCls}-eye, {iconCls}-download, {iconCls}-delete"] = new Unknown_107()
                            {
                                Display = "none",
                            },
                        },
                        [$"{itemCls}-progress"] = new Unknown_108()
                        {
                            Bottom = token.MarginXL,
                            Width = @$"calc(100% - {token.PaddingXS * 2}px)",
                            PaddingInlineStart = 0,
                        },
                    },
                },
                [$"{componentCls}-wrapper{componentCls}-picture-circle-wrapper"] = new Unknown_109()
                {
                    [$"{componentCls}{componentCls}-select"] = new Unknown_110()
                    {
                        BorderRadius = "50%",
                    },
                },
            };
        }

        public Unknown_8 GenRtlStyle(Unknown_111 token)
        {
            var componentCls = token.ComponentCls;
            return new Unknown_112()
            {
                [$"{componentCls}-rtl"] = new Unknown_113()
                {
                    Direction = "rtl",
                },
            };
        }

    }

}