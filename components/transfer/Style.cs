using System;
using CssInCs;
using static AntDesign.GlobalStyle;
using static AntDesign.Theme;

namespace AntDesign
{
    public partial class TransferToken
    {
        public int ListWidth { get; set; }

        public int ListWidthLG { get; set; }

        public int ListHeight { get; set; }

    }

    public partial class TransferToken : TokenWithCommonCls
    {
        public int TransferItemHeight { get; set; }

        public int TransferHeaderVerticalPadding { get; set; }

        public int TransferItemPaddingVertical { get; set; }

        public int TransferHeaderHeight { get; set; }

    }

    public partial class Transfer
    {
        public CSSObject GenTransferCustomizeStyle(TransferToken token)
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

        public CSSObject GenTransferStatusColor(TransferToken token, string color)
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

        public CSSObject GenTransferStatusStyle(TransferToken token)
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

        public CSSObject GenTransferListStyle(TransferToken token)
        {
            var componentCls = token.ComponentCls;
            var colorBorder = token.ColorBorder;
            var colorSplit = token.ColorSplit;
            var lineWidth = token.LineWidth;
            var transferItemHeight = token.TransferItemHeight;
            var transferHeaderHeight = token.TransferHeaderHeight;
            var transferHeaderVerticalPadding = token.TransferHeaderVerticalPadding;
            var transferItemPaddingVertical = token.TransferItemPaddingVertical;
            var controlItemBgActive = token.ControlItemBgActive;
            var controlItemBgActiveHover = token.ControlItemBgActiveHover;
            var colorTextDisabled = token.ColorTextDisabled;
            var listHeight = token.ListHeight;
            var listWidth = token.ListWidth;
            var listWidthLG = token.ListWidthLG;
            var fontSizeIcon = token.FontSizeIcon;
            var marginXS = token.MarginXS;
            var paddingSM = token.PaddingSM;
            var lineType = token.LineType;
            var iconCls = token.IconCls;
            var motionDurationSlow = token.MotionDurationSlow;
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
                    Height = transferHeaderHeight,
                    Padding = @$"{
        transferHeaderVerticalPadding - lineWidth
      }px {paddingSM}px {transferHeaderVerticalPadding}px",
                    Color = token.ColorText,
                    Background = token.ColorBgContainer,
                    BorderBottom = @$"{lineWidth}px {lineType} {colorSplit}",
                    BorderRadius = @$"{token.BorderRadiusLG}px {token.BorderRadiusLG}px 0 0",
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
                        ["..."] = textEllipsis,
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
                    Overflow = "hidden",
                    FontSize = token.FontSize,
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
                        MinHeight = transferItemHeight,
                        Padding = @$"{transferItemPaddingVertical}px {paddingSM}px",
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
                            ["..."] = textEllipsis,
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
                                Insert = @$"-{transferItemPaddingVertical}px -50%",
                                Content = "\"\"",
                            },
                        },
                        [$"&:not({componentCls}-list-content-item-disabled)"] = new CSSObject()
                        {
                            ["&:hover"] = new CSSObject()
                            {
                                BackgroundColor = token.ControlItemBgHover,
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
            };
        }

        public CSSObject GenTransferStyle(TransferToken token)
        {
            var antCls = token.AntCls;
            var iconCls = token.IconCls;
            var componentCls = token.ComponentCls;
            var transferHeaderHeight = token.TransferHeaderHeight;
            var marginXS = token.MarginXS;
            var marginXXS = token.MarginXXS;
            var fontSizeIcon = token.FontSizeIcon;
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
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
                            Background = token.ColorBgContainerDisabled,
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
                        MaxHeight = TransferHeaderHeight / 2 - Math.Round(fontSize * lineHeight),
                    },
                },
            };
        }

        public CSSObject GenTransferRTLStyle(TransferToken token)
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

        public Unknown_1 GenComponentStyleHook(Unknown_2 token)
        {
            var fontSize = token.FontSize;
            var lineHeight = token.LineHeight;
            var lineWidth = token.LineWidth;
            var controlHeightLG = token.ControlHeightLG;
            var controlHeight = token.ControlHeight;
            var fontHeight = Math.Round(fontSize * lineHeight);
            var transferHeaderHeight = controlHeightLG;
            var transferItemHeight = controlHeight;
            var transferToken = MergeToken(
                token,
                new Unknown_3()
                {
                    TransferItemHeight = transferItemHeight,
                    TransferHeaderHeight = transferHeaderHeight,
                    TransferHeaderVerticalPadding = Math.Ceil((transferHeaderHeight - lineWidth - fontHeight) / 2),
                    TransferItemPaddingVertical = (transferItemHeight - fontHeight) / 2,
                });
            return new Unknown_4
            {
                GenTransferStyle(transferToken),
                GenTransferCustomizeStyle(transferToken),
                GenTransferStatusStyle(transferToken),
                GenTransferRTLStyle(transferToken)
            };
        }

    }

}