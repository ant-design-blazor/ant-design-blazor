using System;
using CssInCSharp;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;
using static AntDesign.StyleUtil;

namespace AntDesign
{
    public partial class TransferToken
    {
        public double ListWidth
        {
            get => (double)_tokens["listWidth"];
            set => _tokens["listWidth"] = value;
        }

        public double ListWidthLG
        {
            get => (double)_tokens["listWidthLG"];
            set => _tokens["listWidthLG"] = value;
        }

        public double ListHeight
        {
            get => (double)_tokens["listHeight"];
            set => _tokens["listHeight"] = value;
        }

        public double ItemHeight
        {
            get => (double)_tokens["itemHeight"];
            set => _tokens["itemHeight"] = value;
        }

        public double ItemPaddingBlock
        {
            get => (double)_tokens["itemPaddingBlock"];
            set => _tokens["itemPaddingBlock"] = value;
        }

        public double HeaderHeight
        {
            get => (double)_tokens["headerHeight"];
            set => _tokens["headerHeight"] = value;
        }

    }

    public partial class TransferToken : TokenWithCommonCls
    {
        public double TransferHeaderVerticalPadding
        {
            get => (double)_tokens["transferHeaderVerticalPadding"];
            set => _tokens["transferHeaderVerticalPadding"] = value;
        }

    }

    public partial class TransferStyle
    {
        public static CSSObject GenTransferCustomizeStyle(TransferToken token)
        {
            var antCls = token.AntCls;
            var componentCls = token.ComponentCls;
            var listHeight = token.ListHeight;
            var controlHeightLG = token.ControlHeightLG;
            var marginXXS = token.MarginXXS;
            var margin = token.Margin;
            var tableCls = @$"{antCls}-table";
            var inputCls = @$"{antCls}-input";
            return new CSSObject()
            {
                [$"{componentCls}-customize-list"] = new CSSObject()
                {
                    [$"{componentCls}-list"] = new CSSObject()
                    {
                        Flex = "1 1 50%",
                        Width = "auto",
                        Height = "auto",
                        MinHeight = listHeight,
                    },
                    [$"{tableCls}-wrapper"] = new CSSObject()
                    {
                        [$"{tableCls}-small"] = new CSSObject()
                        {
                            Border = 0,
                            BorderRadius = 0,
                            [$"{tableCls}-selection-column"] = new CSSObject()
                            {
                                Width = controlHeightLG,
                                MinWidth = controlHeightLG,
                            },
                        },
                        [$"{tableCls}-pagination{tableCls}-pagination"] = new CSSObject()
                        {
                            Margin = @$"{margin}px 0 {marginXXS}px",
                        },
                    },
                    [$"{inputCls}[disabled]"] = new CSSObject()
                    {
                        BackgroundColor = "transparent",
                    },
                },
            };
        }

        public static CSSObject GenTransferStatusColor(TransferToken token, string color)
        {
            var componentCls = token.ComponentCls;
            var colorBorder = token.ColorBorder;
            return new CSSObject()
            {
                [$"{componentCls}-list"] = new CSSObject()
                {
                    BorderColor = color,
                    ["&-search:not([disabled])"] = new CSSObject()
                    {
                        BorderColor = colorBorder,
                    },
                },
            };
        }

        public static CSSObject GenTransferStatusStyle(TransferToken token)
        {
            var componentCls = token.ComponentCls;
            return new CSSObject()
            {
                [$"{componentCls}-status-error"] = new CSSObject()
                {
                    ["..."] = GenTransferStatusColor(token, token.ColorError)
                },
                [$"{componentCls}-status-warning"] = new CSSObject()
                {
                    ["..."] = GenTransferStatusColor(token, token.ColorWarning)
                },
            };
        }

        public static CSSObject GenTransferListStyle(TransferToken token)
        {
            var componentCls = token.ComponentCls;
            var colorBorder = token.ColorBorder;
            var colorSplit = token.ColorSplit;
            var lineWidth = token.LineWidth;
            var itemHeight = token.ItemHeight;
            var headerHeight = token.HeaderHeight;
            var transferHeaderVerticalPadding = token.TransferHeaderVerticalPadding;
            var itemPaddingBlock = token.ItemPaddingBlock;
            var controlItemBgActive = token.ControlItemBgActive;
            var colorTextDisabled = token.ColorTextDisabled;
            var listHeight = token.ListHeight;
            var listWidth = token.ListWidth;
            var listWidthLG = token.ListWidthLG;
            var fontSizeIcon = token.FontSizeIcon;
            var marginXS = token.MarginXS;
            var paddingSM = token.PaddingSM;
            var lineType = token.LineType;
            var antCls = token.AntCls;
            var iconCls = token.IconCls;
            var motionDurationSlow = token.MotionDurationSlow;
            var controlItemBgHover = token.ControlItemBgHover;
            var borderRadiusLG = token.BorderRadiusLG;
            var colorBgContainer = token.ColorBgContainer;
            var colorText = token.ColorText;
            var controlItemBgActiveHover = token.ControlItemBgActiveHover;
            return new CSSObject()
            {
                Display = "flex",
                FlexDirection = "column",
                Width = listWidth,
                Height = listHeight,
                Border = @$"{lineWidth}px {lineType} {colorBorder}",
                BorderRadius = token.BorderRadiusLG,
                ["&-with-pagination"] = new CSSObject()
                {
                    Width = listWidthLG,
                    Height = "auto",
                },
                ["&-search"] = new CSSObject()
                {
                    [$"{iconCls}-search"] = new CSSObject()
                    {
                        Color = colorTextDisabled,
                    },
                },
                ["&-header"] = new CSSObject()
                {
                    Display = "flex",
                    Flex = "none",
                    AlignItems = "center",
                    Height = headerHeight,
                    Padding = @$"{
        transferHeaderVerticalPadding - lineWidth
      }px {paddingSM}px {transferHeaderVerticalPadding}px",
                    Color = colorText,
                    Background = colorBgContainer,
                    BorderBottom = @$"{lineWidth}px {lineType} {colorSplit}",
                    BorderRadius = @$"{borderRadiusLG}px {borderRadiusLG}px 0 0",
                    ["> *:not(:last-child)"] = new CSSObject()
                    {
                        MarginInlineEnd = 4,
                    },
                    ["> *"] = new CSSObject()
                    {
                        Flex = "none",
                    },
                    ["&-title"] = new CSSObject()
                    {
                        ["..."] = TextEllipsis,
                        Flex = "auto",
                        TextAlign = "end",
                    },
                    ["&-dropdown"] = new CSSObject()
                    {
                        ["..."] = ResetIcon(),
                        FontSize = fontSizeIcon,
                        Transform = "translateY(10%)",
                        Cursor = "pointer",
                        ["&[disabled]"] = new CSSObject()
                        {
                            Cursor = "not-allowed",
                        },
                    },
                },
                ["&-body"] = new CSSObject()
                {
                    Display = "flex",
                    Flex = "auto",
                    FlexDirection = "column",
                    FontSize = token.FontSize,
                    MinHeight = 0,
                    ["&-search-wrapper"] = new CSSObject()
                    {
                        Position = "relative",
                        Flex = "none",
                        Padding = paddingSM,
                    },
                },
                ["&-content"] = new CSSObject()
                {
                    Flex = "auto",
                    Margin = 0,
                    Padding = 0,
                    Overflow = "auto",
                    ListStyle = "none",
                    ["&-item"] = new CSSObject()
                    {
                        Display = "flex",
                        AlignItems = "center",
                        MinHeight = itemHeight,
                        Padding = @$"{itemPaddingBlock}px {paddingSM}px",
                        Transition = @$"all {motionDurationSlow}",
                        ["> *:not(:last-child)"] = new CSSObject()
                        {
                            MarginInlineEnd = marginXS,
                        },
                        ["> *"] = new CSSObject()
                        {
                            Flex = "none",
                        },
                        ["&-text"] = new CSSObject()
                        {
                            ["..."] = TextEllipsis,
                            Flex = "auto",
                        },
                        ["&-remove"] = new CSSObject()
                        {
                            Position = "relative",
                            Color = colorBorder,
                            Cursor = "pointer",
                            Transition = @$"all {motionDurationSlow}",
                            ["&:hover"] = new CSSObject()
                            {
                                Color = token.ColorLinkHover,
                            },
                            ["&::after"] = new CSSObject()
                            {
                                Position = "absolute",
                                Inset = @$"-{itemPaddingBlock}px -50%",
                                Content = "\"\"",
                            },
                        },
                        [$"&:not({componentCls}-list-content-item-disabled)"] = new CSSObject()
                        {
                            ["&:hover"] = new CSSObject()
                            {
                                BackgroundColor = controlItemBgHover,
                                Cursor = "pointer",
                            },
                            [$"&{componentCls}-list-content-item-checked:hover"] = new CSSObject()
                            {
                                BackgroundColor = controlItemBgActiveHover,
                            },
                        },
                        ["&-checked"] = new CSSObject()
                        {
                            BackgroundColor = controlItemBgActive,
                        },
                        ["&-disabled"] = new CSSObject()
                        {
                            Color = colorTextDisabled,
                            Cursor = "not-allowed",
                        },
                    },
                    [$"&-show-remove {componentCls}-list-content-item:not({componentCls}-list-content-item-disabled):hover"] = new CSSObject()
                    {
                        Background = "transparent",
                        Cursor = "default",
                    },
                },
                ["&-pagination"] = new CSSObject()
                {
                    Padding = @$"{token.PaddingXS}px 0",
                    TextAlign = "end",
                    BorderTop = @$"{lineWidth}px {lineType} {colorSplit}",
                    [$"{antCls}-pagination-options"] = new CSSObject()
                    {
                        PaddingInlineEnd = token.PaddingXS,
                    },
                },
                ["&-body-not-found"] = new CSSObject()
                {
                    Flex = "none",
                    Width = "100%",
                    Margin = "auto 0",
                    Color = colorTextDisabled,
                    TextAlign = "center",
                },
                ["&-footer"] = new CSSObject()
                {
                    BorderTop = @$"{lineWidth}px {lineType} {colorSplit}",
                },
                ["&-checkbox"] = new CSSObject()
                {
                    LineHeight = 1,
                },
            };
        }

        public static CSSObject GenTransferStyle(TransferToken token)
        {
            var antCls = token.AntCls;
            var iconCls = token.IconCls;
            var componentCls = token.ComponentCls;
            var headerHeight = token.HeaderHeight;
            var marginXS = token.MarginXS;
            var marginXXS = token.MarginXXS;
            var fontSizeIcon = token.FontSizeIcon;
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var colorBgContainerDisabled = token.ColorBgContainerDisabled;
            return new CSSObject()
            {
                [componentCls] = new CSSObject()
                {
                    ["..."] = ResetComponent(token),
                    Position = "relative",
                    Display = "flex",
                    AlignItems = "stretch",
                    [$"{componentCls}-disabled"] = new CSSObject()
                    {
                        [$"{componentCls}-list"] = new CSSObject()
                        {
                            Background = colorBgContainerDisabled,
                        },
                    },
                    [$"{componentCls}-list"] = GenTransferListStyle(token),
                    [$"{componentCls}-operation"] = new CSSObject()
                    {
                        Display = "flex",
                        Flex = "none",
                        FlexDirection = "column",
                        AlignSelf = "center",
                        Margin = @$"0 {marginXS}px",
                        VerticalAlign = "middle",
                        [$"{antCls}-btn"] = new CSSObject()
                        {
                            Display = "block",
                            ["&:first-child"] = new CSSObject()
                            {
                                MarginBottom = marginXXS,
                            },
                            [iconCls] = new CSSObject()
                            {
                                FontSize = fontSizeIcon,
                            },
                        },
                    },
                    [$"{antCls}-empty-image"] = new CSSObject()
                    {
                        MaxHeight = headerHeight / 2 - Math.Round(fontSize * lineHeight),
                    },
                },
            };
        }

        public static CSSObject GenTransferRTLStyle(TransferToken token)
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

        public static UseComponentStyleResult UseComponentStyle()
        {
            return GenComponentStyleHook(
                "Transfer",
                (token) =>
                {
                    var fontSize = token.FontSize;
                    var lineHeight = token.LineHeight;
                    var lineWidth = token.LineWidth;
                    var controlHeightLG = token.ControlHeightLG;
                    var fontHeight = Math.Round(fontSize * lineHeight);
                    var transferToken = MergeToken(
                        token,
                        new TransferToken()
                        {
                            TransferHeaderVerticalPadding = Math.Ceiling((controlHeightLG - lineWidth - fontHeight) / 2),
                        });
                    return new CSSInterpolation[]
                    {
                        GenTransferStyle(transferToken),
                        GenTransferCustomizeStyle(transferToken),
                        GenTransferStatusStyle(transferToken),
                        GenTransferRTLStyle(transferToken),
                    };
                },
                (token) =>
                {
                    var fontSize = token.FontSize;
                    var lineHeight = token.LineHeight;
                    var controlHeight = token.ControlHeight;
                    var controlHeightLG = token.ControlHeightLG;
                    var fontHeight = Math.Round(fontSize * lineHeight);
                    return new TransferToken()
                    {
                        ListWidth = 180,
                        ListHeight = 200,
                        ListWidthLG = 250,
                        HeaderHeight = controlHeightLG,
                        ItemHeight = controlHeight,
                        ItemPaddingBlock = (controlHeight - fontHeight) / 2,
                    };
                });
        }

    }

}
