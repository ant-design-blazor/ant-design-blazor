using System;
using CssInCSharp;
using CssInCSharp.Colors;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;
using static AntDesign.Fade;
using static AntDesign.CollapseMotion;

namespace AntDesign
{
    public partial class UploadToken
    {
        public string ActionsColor
        {
            get => (string)_tokens["actionsColor"];
            set => _tokens["actionsColor"] = value;
        }

    }

    public partial class UploadToken : TokenWithCommonCls
    {
        public double UploadThumbnailSize
        {
            get => (double)_tokens["uploadThumbnailSize"];
            set => _tokens["uploadThumbnailSize"] = value;
        }

        public double UploadProgressOffset
        {
            get => (double)_tokens["uploadProgressOffset"];
            set => _tokens["uploadProgressOffset"] = value;
        }

        public double UploadPicCardSize
        {
            get => (double)_tokens["uploadPicCardSize"];
            set => _tokens["uploadPicCardSize"] = value;
        }

    }

    public partial class UploadStyle
    {
        private static readonly Keyframe _uploadAnimateInlineIn = new Keyframe("uploadAnimateInlineIn",
            new CSSObject()
            {
                ["from"] = new CSSObject()
                {
                    Width = 0,
                    Height = 0,
                    Margin = 0,
                    Padding = 0,
                    Opacity = 0,
                },
            });

        private static readonly Keyframe _uploadAnimateInlineOut = new Keyframe("uploadAnimateInlineOut",
            new CSSObject()
            {
                ["to"] = new CSSObject()
                {
                    Width = 0,
                    Height = 0,
                    Margin = 0,
                    Padding = 0,
                    Opacity = 0,
                },
            });

        public static CSSObject GenBaseStyle(UploadToken token)
        {
            var componentCls = token.ComponentCls;
            var colorTextDisabled = token.ColorTextDisabled;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    [componentCls] = new CSSObject()
                    {
                        Outline = 0,
                        ["input[type=\"file\"]"] = new CSSObject()
                        {
                            Cursor = "pointer",
                        },
                    },
                    [$"{componentCls}-select"] = new CSSObject()
                    {
                        Display = "inline-block",
                    },
                    [$"{componentCls}-disabled"] = new CSSObject()
                    {
                        Color = colorTextDisabled,
                        Cursor = "not-allowed",
                    },
                },
            };
        }

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Upload",
                (token) =>
                {
                    var fontSizeHeading3 = token.FontSizeHeading3;
                    var fontSize = token.FontSize;
                    var lineHeight = token.LineHeight;
                    var lineWidth = token.LineWidth;
                    var controlHeightLG = token.ControlHeightLG;
                    var listItemHeightSM = Math.Round(fontSize * lineHeight);
                    var uploadToken = MergeToken(
                        token,
                        new UploadToken()
                        {
                            UploadThumbnailSize = fontSizeHeading3 * 2,
                            UploadProgressOffset = listItemHeightSM / 2 + lineWidth,
                            UploadPicCardSize = controlHeightLG * 2.55,
                        });
                    return new CSSInterpolation[]
                    {
                        GenBaseStyle(uploadToken),
                        GenDraggerStyle(uploadToken),
                        GenPictureStyle(uploadToken),
                        GenPictureCardStyle(uploadToken),
                        GenListStyle(uploadToken),
                        GenMotionStyle(uploadToken),
                        GenRtlStyle(uploadToken),
                        GenCollapseMotion(uploadToken),
                    };
                },
                (token) =>
                {
                    return new UploadToken()
                    {
                        ActionsColor = token.ColorTextDescription,
                    };
                });
        }

        public static CSSObject GenDraggerStyle(UploadToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-drag"] = new CSSObject()
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
                        [componentCls] = new CSSObject()
                        {
                            Padding = @$"{token.Padding}px 0",
                        },
                        [$"{componentCls}-btn"] = new CSSObject()
                        {
                            Display = "table",
                            Width = "100%",
                            Height = "100%",
                            Outline = "none",
                        },
                        [$"{componentCls}-drag-container"] = new CSSObject()
                        {
                            Display = "table-cell",
                            VerticalAlign = "middle",
                        },
                        [$"&:not({componentCls}-disabled):hover,&-hover:not({componentCls}-disabled)"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimaryHover,
                        },
                        [$"p{componentCls}-drag-icon"] = new CSSObject()
                        {
                            MarginBottom = token.Margin,
                            [iconCls] = new CSSObject()
                            {
                                Color = token.ColorPrimary,
                                FontSize = token.UploadThumbnailSize,
                            },
                        },
                        [$"p{componentCls}-text"] = new CSSObject()
                        {
                            Margin = @$"0 0 {token.MarginXXS}px",
                            Color = token.ColorTextHeading,
                            FontSize = token.FontSizeLG,
                        },
                        [$"p{componentCls}-hint"] = new CSSObject()
                        {
                            Color = token.ColorTextDescription,
                            FontSize = token.FontSize,
                        },
                        [$"&{componentCls}-disabled"] = new CSSObject()
                        {
                            [@$"p{componentCls}-drag-icon {iconCls},
                                p{componentCls}-text,
                                p{componentCls}-hint"
                            ] = new CSSObject()
                            {
                                Color = token.ColorTextDisabled,
                            },
                        },
                    },
                },
            };
        }

        public static CSSObject GenListStyle(UploadToken token)
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
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}-list"] = new CSSObject()
                    {
                        ["..."] = ClearFix(),
                        LineHeight = token.LineHeight,
                        [itemCls] = new CSSObject()
                        {
                            Position = "relative",
                            Height = token.LineHeight * fontSize,
                            MarginTop = token.MarginXS,
                            FontSize = fontSize,
                            Display = "flex",
                            AlignItems = "center",
                            Transition = @$"background-color {token.MotionDurationSlow}",
                            ["&:hover"] = new CSSObject()
                            {
                                BackgroundColor = token.ControlItemBgHover,
                            },
                            [$"{itemCls}-name"] = new CSSObject()
                            {
                                ["..."] = TextEllipsis,
                                Padding = @$"0 {token.PaddingXS}px",
                                LineHeight = lineHeight,
                                Flex = "auto",
                                Transition = @$"all {token.MotionDurationSlow}",
                            },
                            [actionsCls] = new CSSObject()
                            {
                                [actionCls] = new CSSObject()
                                {
                                    Opacity = 0,
                                },
                                [$"{actionCls}{antCls}-btn-sm"] = new CSSObject()
                                {
                                    Height = listItemHeightSM,
                                    Border = 0,
                                    LineHeight = 1,
                                    ["> span"] = new CSSObject()
                                    {
                                        Transform = "scale(1)",
                                    },
                                },
                                [@$"
                                    {actionCls}:focus-visible,
                                    &.picture {actionCls}"] = new CSSObject()
                                {
                                    Opacity = 1,
                                },
                                [iconCls] = new CSSObject()
                                {
                                    Color = token.ActionsColor,
                                    Transition = @$"all {token.MotionDurationSlow}",
                                },
                                [$"&:hover {iconCls}"] = new CSSObject()
                                {
                                    Color = token.ColorText,
                                },
                            },
                            [$"{componentCls}-icon {iconCls}"] = new CSSObject()
                            {
                                Color = token.ColorTextDescription,
                                FontSize = fontSize,
                            },
                            [$"{itemCls}-progress"] = new CSSObject()
                            {
                                Position = "absolute",
                                Bottom = -token.UploadProgressOffset,
                                Width = "100%",
                                PaddingInlineStart = fontSize + token.PaddingXS,
                                FontSize = fontSize,
                                LineHeight = 0,
                                PointerEvents = "none",
                                ["> div"] = new CSSObject()
                                {
                                    Margin = 0,
                                },
                            },
                        },
                        [$"{itemCls}:hover {actionCls}"] = new CSSObject()
                        {
                            Opacity = 1,
                            Color = token.ColorText,
                        },
                        [$"{itemCls}-error"] = new CSSObject()
                        {
                            Color = token.ColorError,
                            [$"{itemCls}-name, {componentCls}-icon {iconCls}"] = new CSSObject()
                            {
                                Color = token.ColorError,
                            },
                            [actionsCls] = new CSSObject()
                            {
                                [$"{iconCls}, {iconCls}:hover"] = new CSSObject()
                                {
                                    Color = token.ColorError,
                                },
                                [actionCls] = new CSSObject()
                                {
                                    Opacity = 1,
                                },
                            },
                        },
                        [$"{componentCls}-list-item-container"] = new CSSObject()
                        {
                            Transition = @$"opacity {token.MotionDurationSlow}, height {token.MotionDurationSlow}",
                            ["&::before"] = new CSSObject()
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

        public static CSSInterpolation GenMotionStyle(UploadToken token)
        {
            var componentCls = token.ComponentCls;
            var inlineCls = @$"{componentCls}-animate-inline";
            return new CSSInterpolation[]
            {
                new CSSObject()
                {
                    [$"{componentCls}-wrapper"] = new CSSObject()
                    {
                        [$"{inlineCls}-appear, {inlineCls}-enter, {inlineCls}-leave"] = new CSSObject()
                        {
                            AnimationDuration = token.MotionDurationSlow,
                            AnimationTimingFunction = token.MotionEaseInOutCirc,
                            AnimationFillMode = "forwards",
                        },
                        [$"{inlineCls}-appear, {inlineCls}-enter"] = new CSSObject()
                        {
                            AnimationName = _uploadAnimateInlineIn,
                        },
                        [$"{inlineCls}-leave"] = new CSSObject()
                        {
                            AnimationName = _uploadAnimateInlineOut,
                        },
                    },
                },
                new CSSObject()
                {
                    [$"{componentCls}-wrapper"] = InitFadeMotion(token)
                },
                _uploadAnimateInlineIn,
                _uploadAnimateInlineOut
            };
        }

        public static CSSObject GenPictureStyle(UploadToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var uploadThumbnailSize = token.UploadThumbnailSize;
            var uploadProgressOffset = token.UploadProgressOffset;
            var listCls = @$"{componentCls}-list";
            var itemCls = @$"{listCls}-item";
            var blue = AntDesignColorPalettes.PresetPalettes["blue"];
            return new CSSObject()
            {
                [$"{componentCls}-wrapper"] = new CSSObject()
                {
                    [$"{listCls}{listCls}-picture,{listCls}{listCls}-picture-card,{listCls}{listCls}-picture-circle"] = new CSSObject()
                    {
                        [itemCls] = new CSSObject()
                        {
                            Position = "relative",
                            Height = uploadThumbnailSize + token.LineWidth * 2 + token.PaddingXS * 2,
                            Padding = token.PaddingXS,
                            Border = @$"{token.LineWidth}px {token.LineType} {token.ColorBorder}",
                            BorderRadius = token.BorderRadiusLG,
                            ["&:hover"] = new CSSObject()
                            {
                                Background = "transparent",
                            },
                            [$"{itemCls}-thumbnail"] = new CSSObject()
                            {
                                ["..."] = TextEllipsis,
                                Width = uploadThumbnailSize,
                                Height = uploadThumbnailSize,
                                LineHeight = @$"{uploadThumbnailSize + token.PaddingSM}px",
                                TextAlign = "center",
                                Flex = "none",
                                [iconCls] = new CSSObject()
                                {
                                    FontSize = token.FontSizeHeading2,
                                    Color = token.ColorPrimary,
                                },
                                ["img"] = new CSSObject()
                                {
                                    Display = "block",
                                    Width = "100%",
                                    Height = "100%",
                                    Overflow = "hidden",
                                },
                            },
                            [$"{itemCls}-progress"] = new CSSObject()
                            {
                                Bottom = uploadProgressOffset,
                                Width = @$"calc(100% - {token.PaddingSM * 2}px)",
                                MarginTop = 0,
                                PaddingInlineStart = uploadThumbnailSize + token.PaddingXS,
                            },
                        },
                        [$"{itemCls}-error"] = new CSSObject()
                        {
                            BorderColor = token.ColorError,
                            [$"{itemCls}-thumbnail {iconCls}"] = new CSSObject()
                            {
                                [$"svg path[fill=\"{blue[0]}\"]"] = new CSSObject()
                                {
                                    ["fill"] = token.ColorErrorBg,
                                },
                                [$"svg path[fill=\"{blue[5]}\"]"] = new CSSObject()
                                {
                                    ["fill"] = token.ColorError,
                                },
                            },
                        },
                        [$"{itemCls}-uploading"] = new CSSObject()
                        {
                            BorderStyle = "dashed",
                            [$"{itemCls}-name"] = new CSSObject()
                            {
                                MarginBottom = uploadProgressOffset,
                            },
                        },
                    },
                    [$"{listCls}{listCls}-picture-circle {itemCls}"] = new CSSObject()
                    {
                        [$"&, &::before, {itemCls}-thumbnail"] = new CSSObject()
                        {
                            BorderRadius = "50%",
                        },
                    },
                },
            };
        }

        public static CSSObject GenPictureCardStyle(UploadToken token)
        {
            var componentCls = token.ComponentCls;
            var iconCls = token.IconCls;
            var fontSizeLG = token.FontSizeLG;
            var colorTextLightSolid = token.ColorTextLightSolid;
            var listCls = @$"{componentCls}-list";
            var itemCls = @$"{listCls}-item";
            var uploadPictureCardSize = token.UploadPicCardSize;
            return new CSSObject()
            {
                [$"{componentCls}-wrapper{componentCls}-picture-card-wrapper,{componentCls}-wrapper{componentCls}-picture-circle-wrapper"] = new CSSObject()
                {
                    ["..."] = ClearFix(),
                    Display = "inline-block",
                    Width = "100%",
                    [$"{componentCls}{componentCls}-select"] = new CSSObject()
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
                        [$"> {componentCls}"] = new CSSObject()
                        {
                            Display = "flex",
                            AlignItems = "center",
                            JustifyContent = "center",
                            Height = "100%",
                            TextAlign = "center",
                        },
                        [$"&:not({componentCls}-disabled):hover"] = new CSSObject()
                        {
                            BorderColor = token.ColorPrimary,
                        },
                    },
                    [$"{listCls}{listCls}-picture-card, {listCls}{listCls}-picture-circle"] = new CSSObject()
                    {
                        [$"{listCls}-item-container"] = new CSSObject()
                        {
                            Display = "inline-block",
                            Width = uploadPictureCardSize,
                            Height = uploadPictureCardSize,
                            MarginBlock = @$"0 {token.MarginXS}px",
                            MarginInline = @$"0 {token.MarginXS}px",
                            VerticalAlign = "top",
                        },
                        ["&::after"] = new CSSObject()
                        {
                            Display = "none",
                        },
                        [itemCls] = new CSSObject()
                        {
                            Height = "100%",
                            Margin = 0,
                            ["&::before"] = new CSSObject()
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
                        [$"{itemCls}:hover"] = new CSSObject()
                        {
                            [$"&::before, {itemCls}-actions"] = new CSSObject()
                            {
                                Opacity = 1,
                            },
                        },
                        [$"{itemCls}-actions"] = new CSSObject()
                        {
                            Position = "absolute",
                            InsetInlineStart = 0,
                            ZIndex = 10,
                            Width = "100%",
                            WhiteSpace = "nowrap",
                            TextAlign = "center",
                            Opacity = 0,
                            Transition = @$"all {token.MotionDurationSlow}",
                            [$"{iconCls}-eye, {iconCls}-download, {iconCls}-delete"] = new CSSObject()
                            {
                                ZIndex = 10,
                                Width = fontSizeLG,
                                Margin = @$"0 {token.MarginXXS}px",
                                FontSize = fontSizeLG,
                                Cursor = "pointer",
                                Transition = @$"all {token.MotionDurationSlow}",
                                ["svg"] = new CSSObject()
                                {
                                    VerticalAlign = "baseline",
                                },
                            },
                        },
                        [$"{itemCls}-actions, {itemCls}-actions:hover"] = new CSSObject()
                        {
                            [$"{iconCls}-eye, {iconCls}-download, {iconCls}-delete"] = new CSSObject()
                            {
                                Color = new TinyColor(colorTextLightSolid).SetAlpha(0.65).ToRgbString(),
                                ["&:hover"] = new CSSObject()
                                {
                                    Color = colorTextLightSolid,
                                },
                            },
                        },
                        [$"{itemCls}-thumbnail, {itemCls}-thumbnail img"] = new CSSObject()
                        {
                            Position = "static",
                            Display = "block",
                            Width = "100%",
                            Height = "100%",
                            ObjectFit = "contain",
                        },
                        [$"{itemCls}-name"] = new CSSObject()
                        {
                            Display = "none",
                            TextAlign = "center",
                        },
                        [$"{itemCls}-file + {itemCls}-name"] = new CSSObject()
                        {
                            Position = "absolute",
                            Bottom = token.Margin,
                            Display = "block",
                            Width = @$"calc(100% - {token.PaddingXS * 2}px)",
                        },
                        [$"{itemCls}-uploading"] = new CSSObject()
                        {
                            [$"&{itemCls}"] = new CSSObject()
                            {
                                BackgroundColor = token.ColorFillAlter,
                            },
                            [$"&::before, {iconCls}-eye, {iconCls}-download, {iconCls}-delete"] = new CSSObject()
                            {
                                Display = "none",
                            },
                        },
                        [$"{itemCls}-progress"] = new CSSObject()
                        {
                            Bottom = token.MarginXL,
                            Width = @$"calc(100% - {token.PaddingXS * 2}px)",
                            PaddingInlineStart = 0,
                        },
                    },
                },
                [$"{componentCls}-wrapper{componentCls}-picture-circle-wrapper"] = new CSSObject()
                {
                    [$"{componentCls}{componentCls}-select"] = new CSSObject()
                    {
                        BorderRadius = "50%",
                    },
                },
            };
        }

        public static CSSObject GenRtlStyle(UploadToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-rtl"] = new CSSObject()
                {
                    Direction = "rtl",
                },
            };
        }

    }

}
