namespace AntDesign
{

    // ======================================================================
    // ==                           Alias Token                            ==
    // ======================================================================
    // 🔥🔥🔥🔥🔥🔥🔥 DO NOT MODIFY THIS. PLEASE CONTACT DESIGNER. 🔥🔥🔥🔥🔥🔥🔥

    public partial class GlobalToken
    {
        // Background
        /**
        * @nameZH 内容区域背景色（悬停）
        * @nameEN Background color of content area (hover)
        * @desc 控制内容区域背景色在鼠标悬停时的样式。
        * @descEN Control the style of background color of content area when mouse hovers over it.
        */
        public string ColorFillContentHover { get; set; }
        /**
        * @nameZH 替代背景色
        * @nameEN Alternative background color
        * @desc 控制元素替代背景色。
        * @descEN Control the alternative background color of element.
        */
        public string ColorFillAlter { get; set; }
        /**
        * @nameZH 内容区域背景色
        * @nameEN Background color of content area
        * @desc 控制内容区域的背景色。
        * @descEN Control the background color of content area.
        */
        public string ColorFillContent { get; set; }
        /**
        * @nameZH 容器禁用态下的背景色
        * @nameEN Disabled container background color
        * @desc 控制容器在禁用状态下的背景色。
        * @descEN Control the background color of container in disabled state.
        */
        public string ColorBgContainerDisabled { get; set; }
        /**
        * @nameZH 文本悬停态背景色
        * @nameEN Text hover background color
        * @desc 控制文本在悬停状态下的背景色。
        * @descEN Control the background color of text in hover state.
        */
        public string ColorBgTextHover { get; set; }
        /**
        * @nameZH 文本激活态背景色
        * @nameEN Text active background color
        * @desc 控制文本在激活状态下的背景色。
        * @descEN Control the background color of text in active state.
        */
        public string ColorBgTextActive { get; set; }

        // Border
        /**
        * @nameZH 背景边框颜色
        * @nameEN Background border color
        * @desc 控制元素背景边框的颜色。
        * @descEN Control the color of background border of element.
        */
        public string ColorBorderBg { get; set; }
        /**
        * @nameZH 分割线颜色
        * @nameEN Separator color
        * @desc 用于作为分割线的颜色，此颜色和 colorBorderSecondary 的颜色一致，但是用的是透明色。
        * @descEN Used as the color of separator, this color is the same as colorBorderSecondary but with transparency.
        */
        public string ColorSplit { get; set; }

        // Text
        /**
        * @nameZH 占位文本颜色
        * @nameEN Placeholder text color
        * @desc 控制占位文本的颜色。
        * @descEN Control the color of placeholder text.
        */
        public string ColorTextPlaceholder { get; set; }
        /**
        * @nameZH 禁用字体颜色
        * @nameEN Disabled text color
        * @desc 控制禁用状态下的字体颜色。
        * @descEN Control the color of text in disabled state.
        */
        public string ColorTextDisabled { get; set; }
        /**
        * @nameZH 标题字体颜色
        * @nameEN Heading font color
        * @desc 控制标题字体颜色。
        * @descEN Control the font color of heading.
        */
        public string ColorTextHeading { get; set; }
        /**
        * @nameZH 文本标签字体颜色
        * @nameEN Text label font color
        * @desc 控制文本标签字体颜色。
        * @descEN Control the font color of text label.
        */
        public string ColorTextLabel { get; set; }
        /**
        * @nameZH 文本描述字体颜色
        * @nameEN Text description font color
        * @desc 控制文本描述字体颜色。
        * @descEN Control the font color of text description.
        */
        public string ColorTextDescription { get; set; }
        /**
        * @nameZH 固定文本高亮颜色
        * @nameEN Fixed text highlight color
        * @desc 控制带背景色的文本，例如 Primary Button 组件中的文本高亮颜色。
        * @descEN Control the highlight color of text with background color, such as the text in Primary Button components.
        */
        public string ColorTextLightSolid { get; set; }

        /**
        /**
        * @nameZH 弱操作图标颜色
        * @nameEN Weak action icon color
        * @desc 控制弱操作图标的颜色，例如 allowClear 或 Alert 关闭按钮。
        * @descEN Weak action. Such as `allowClear` or Alert close button
        */
        public string ColorIcon { get; set; }
        /**  */
        /**
        * @nameZH 弱操作图标悬浮态颜色
        * @nameEN Weak action icon hover color
        * @desc 控制弱操作图标在悬浮状态下的颜色，例如 allowClear 或 Alert 关闭按钮。
        * @descEN Weak action hover color. Such as `allowClear` or Alert close button
        */
        public string ColorIconHover { get; set; }

        /**
        * @nameZH 超链接颜色
        * @nameEN Hyperlink color
        * @desc 控制超链接的颜色。
        * @descEN Control the color of hyperlink.
        */
        public string ColorLink { get; set; }
        /**
        * @nameZH 超链接悬浮颜色
        * @nameEN Hyperlink hover color
        * @desc 控制超链接悬浮时的颜色。
        * @descEN Control the color of hyperlink when hovering.
        */
        public string ColorLinkHover { get; set; }
        /**
        * @nameZH 超链接激活颜色
        * @nameEN Hyperlink active color
        * @desc 控制超链接被点击时的颜色。
        * @descEN Control the color of hyperlink when clicked.
        */
        public string ColorLinkActive { get; set; }

        /**
        * @nameZH 高亮颜色
        * @nameEN Highlight color
        * @desc 控制页面元素高亮时的颜色。
        * @descEN Control the color of page element when highlighted.
        */
        public string ColorHighlight { get; set; }

        /**
        * @nameZH 输入组件的 Outline 颜色
        * @nameEN Input component outline color
        * @desc 控制输入组件的外轮廓线颜色。
        * @descEN Control the outline color of input component.
        */
        public string ControlOutline { get; set; }

        /**
        * @nameZH 警告状态下的 Outline 颜色
        * @nameEN Warning outline color
        * @desc 控制输入组件警告状态下的外轮廓线颜色。
        * @descEN Control the outline color of input component in warning state.
        */
        public string ColorWarningOutline { get; set; }

        /**
        * @nameZH 错误状态下的 Outline 颜色
        * @nameEN Error outline color
        * @desc 控制输入组件错误状态下的外轮廓线颜色。
        * @descEN Control the outline color of input component in error state.
        */
        public string ColorErrorOutline { get; set; }

        // Font
        /**
        * @nameZH 选择器、级联选择器等中的操作图标字体大小
        * @nameEN Operation icon font size in Select, Cascader, etc.
        * @desc 控制选择器、级联选择器等中的操作图标字体大小。正常情况下与 fontSizeSM 相同。
        * @descEN Control the font size of operation icon in Select, Cascader, etc. Normally same as fontSizeSM.
        */
        public int FontSizeIcon { get; set; }

        /**
        * @nameZH 标题类组件（如 h1、h2、h3）或选中项的字体粗细
        * @nameEN Font weight for heading components (such as h1, h2, h3) or selected item
        * @desc 控制标题类组件（如 h1、h2、h3）或选中项的字体粗细。
        * @descEN Control the font weight of heading components (such as h1, h2, h3) or selected item.
        */
        public int FontWeightStrong { get; set; }

        // Control

        /**
        * @nameZH 输入组件的外轮廓线宽度
        * @nameEN Input component outline width
        * @desc 控制输入组件的外轮廓线宽度。
        * @descEN Control the outline width of input component.
        */
        public int ControlOutlineWidth { get; set; }

        /**
        * @nameZH 控制组件项在鼠标悬浮时的背景颜色
        * @nameEN Background color of control component item when hovering
        * @desc 控制组件项在鼠标悬浮时的背景颜色。
        * @descEN Control the background color of control component item when hovering.
        */
        public string ControlItemBgHover { get; set; } // Note. It also is a color

        /**
        * @nameZH 控制组件项在激活状态下的背景颜色
        * @nameEN Background color of control component item when active
        * @desc 控制组件项在激活状态下的背景颜色。
        * @descEN Control the background color of control component item when active.
        */
        public string ControlItemBgActive { get; set; } // Note. It also is a color

        /**
        * @nameZH 控制组件项在鼠标悬浮且激活状态下的背景颜色
        * @nameEN Background color of control component item when hovering and active
        * @desc 控制组件项在鼠标悬浮且激活状态下的背景颜色。
        * @descEN Control the background color of control component item when hovering and active.
        */
        public string ControlItemBgActiveHover { get; set; } // Note. It also is a color

        /**
        * @nameZH 控制组件的交互大小
        * @nameEN Interactive size of control component
        * @desc 控制组件的交互大小。
        * @descEN Control the interactive size of control component.
        */
        public int ControlInteractiveSize { get; set; }

        /**
        * @nameZH 控制组件项在禁用状态下的激活背景颜色
        * @nameEN Background color of control component item when active and disabled
        * @desc 控制组件项在禁用状态下的激活背景颜色。
        * @descEN Control the background color of control component item when active and disabled.
        */
        public string ControlItemBgActiveDisabled { get; set; } // Note. It also is a color

        // Line
        /**
        * @nameZH 线条宽度(聚焦态)
        * @nameEN Line width(focus state)
        * @desc 控制线条的宽度，当组件处于聚焦态时。
        * @descEN Control the width of the line when the component is in focus state.
        */
        public int LineWidthFocus { get; set; }

        // Padding
        /**
        * @nameZH 极小内间距
        * @nameEN Extra extra small padding
        * @desc 控制元素的极小内间距。
        * @descEN Control the extra extra small padding of the element.
        */
        public int PaddingXXS { get; set; }
        /**
        * @nameZH 特小内间距
        * @nameEN Extra small padding
        * @desc 控制元素的特小内间距。
        * @descEN Control the extra small padding of the element.
        */
        public int PaddingXS { get; set; }
        /**
        * @nameZH 小内间距
        * @nameEN Small padding
        * @desc 控制元素的小内间距。
        * @descEN Control the small padding of the element.
        */
        public int PaddingSM { get; set; }
        /**
        * @nameZH 内间距
        * @nameEN Padding
        * @desc 控制元素的内间距。
        * @descEN Control the padding of the element.
        */
        public int Padding { get; set; }
        /**
        * @nameZH 中等内间距
        * @nameEN Medium padding
        * @desc 控制元素的中等内间距。
        * @descEN Control the medium padding of the element.
        */
        public int PaddingMD { get; set; }
        /**
        * @nameZH 大内间距
        * @nameEN Large padding
        * @desc 控制元素的大内间距。
        * @descEN Control the large padding of the element.
        */
        public int PaddingLG { get; set; }
        /**
        * @nameZH 特大内间距
        * @nameEN Extra large padding
        * @desc 控制元素的特大内间距。
        * @descEN Control the extra large padding of the element.
        */
        public int PaddingXL { get; set; }

        // Padding Content
        /**
        * @nameZH 内容水平内间距（LG）
        * @nameEN Content horizontal padding (LG)
        * @desc 控制内容元素水平内间距，适用于大屏幕设备。
        * @descEN Control the horizontal padding of content element, suitable for large screen devices.
        */
        public int PaddingContentHorizontalLG { get; set; }
        /**
        * @nameZH 内容水平内间距
        * @nameEN Content horizontal padding
        * @desc 控制内容元素水平内间距。
        * @descEN Control the horizontal padding of content element.
        */
        public int PaddingContentHorizontal { get; set; }
        /**
        * @nameZH 内容水平内间距（SM）
        * @nameEN Content horizontal padding (SM)
        * @desc 控制内容元素水平内间距，适用于小屏幕设备。
        * @descEN Control the horizontal padding of content element, suitable for small screen devices.
        */
        public int PaddingContentHorizontalSM { get; set; }
        /**
        * @nameZH 内容垂直内间距（LG）
        * @nameEN Content vertical padding (LG)
        * @desc 控制内容元素垂直内间距，适用于大屏幕设备。
        * @descEN Control the vertical padding of content element, suitable for large screen devices.
        */
        public int PaddingContentVerticalLG { get; set; }
        /**
        * @nameZH 内容垂直内间距
        * @nameEN Content vertical padding
        * @desc 控制内容元素垂直内间距。
        * @descEN Control the vertical padding of content element.
        */
        public int PaddingContentVertical { get; set; }
        /**
        * @nameZH 内容垂直内间距（SM）
        * @nameEN Content vertical padding (SM)
        * @desc 控制内容元素垂直内间距，适用于小屏幕设备。
        * @descEN Control the vertical padding of content element, suitable for small screen devices.
        */
        public int PaddingContentVerticalSM { get; set; }

        // Margin
        /**
        * @nameZH 外边距 XXS
        * @nameEN Margin XXS
        * @desc 控制元素外边距，最小尺寸。
        * @descEN Control the margin of an element, with the smallest size.
        */
        public int MarginXXS { get; set; }
        /**
        * @nameZH 外边距 XS
        * @nameEN Margin XS
        * @desc 控制元素外边距，小尺寸。
        * @descEN Control the margin of an element, with a small size.
        */
        public int MarginXS { get; set; }
        /**
        * @nameZH 外边距 SM
        * @nameEN Margin SM
        * @desc 控制元素外边距，中小尺寸。
        * @descEN Control the margin of an element, with a medium-small size.
        */
        public int MarginSM { get; set; }
        /**
        * @nameZH 外边距
        * @nameEN Margin
        * @desc 控制元素外边距，中等尺寸。
        * @descEN Control the margin of an element, with a medium size.
        */
        public int Margin { get; set; }
        /**
        * @nameZH 外边距 MD
        * @nameEN Margin MD
        * @desc 控制元素外边距，中大尺寸。
        * @descEN Control the margin of an element, with a medium-large size.
        */
        public int MarginMD { get; set; }
        /**
        * @nameZH 外边距 LG
        * @nameEN Margin LG
        * @desc 控制元素外边距，大尺寸。
        * @descEN Control the margin of an element, with a large size.
        */
        public int MarginLG { get; set; }
        /**
        * @nameZH 外边距 XL
        * @nameEN Margin XL
        * @desc 控制元素外边距，超大尺寸。
        * @descEN Control the margin of an element, with an extra-large size.
        */
        public int MarginXL { get; set; }
        /**
        * @nameZH 外边距 XXL
        * @nameEN Margin XXL
        * @desc 控制元素外边距，最大尺寸。
        * @descEN Control the margin of an element, with the largest size.
        */
        public int MarginXXL { get; set; }

        // =============== Legacy: should be remove ===============
        /**
        * @nameZH 加载状态透明度
        * @nameEN Loading opacity
        * @desc 控制加载状态的透明度。
        * @descEN Control the opacity of the loading state.
        */
        public int OpacityLoading { get; set; }

        /**
        * @nameZH 一级阴影
        * @nameEN Box shadow
        * @desc 控制元素阴影样式。
        * @descEN Control the box shadow style of an element.
        */
        public string BoxShadow { get; set; }
        /**
        * @nameZH 二级阴影
        * @nameEN Secondary box shadow
        * @desc 控制元素二级阴影样式。
        * @descEN Control the secondary box shadow style of an element.
        */
        public string BoxShadowSecondary { get; set; }
        /**
        * @nameZH 三级阴影
        * @nameEN Tertiary box shadow
        * @desc 控制元素三级盒子阴影样式。
        * @descEN Control the tertiary box shadow style of an element.
        */
        public string BoxShadowTertiary { get; set; }

        /**
        * @nameZH 链接文本装饰
        * @nameEN Link text decoration
        * @desc 控制链接文本的装饰样式。
        * @descEN Control the text decoration style of a link.
        */
        public string LinkDecoration { get; set; }
        /**
        * @nameZH 链接鼠标悬浮时文本装饰
        * @nameEN Link text decoration on mouse hover
        * @desc 控制链接鼠标悬浮时文本的装饰样式。
        * @descEN Control the text decoration style of a link on mouse hover.
        */
        public string LinkHoverDecoration { get; set; }
        /**
        * @nameZH 链接聚焦时文本装饰
        * @nameEN Link text decoration on focus
        * @desc 控制链接聚焦时文本的装饰样式。
        * @descEN Control the text decoration style of a link on focus.
        */
        public string LinkFocusDecoration { get; set; }

        /**
        * @nameZH 控制水平内间距
        * @nameEN Control horizontal padding
        * @desc 控制元素水平内间距。
        * @descEN Control the horizontal padding of an element.
        */
        public int ControlPaddingHorizontal { get; set; }
        /**
        * @nameZH 控制中小尺寸水平内间距
        * @nameEN Control horizontal padding with a small-medium size
        * @desc 控制元素中小尺寸水平内间距。
        * @descEN Control the horizontal padding of an element with a small-medium size.
        */
        public int ControlPaddingHorizontalSM { get; set; }

        // Media queries breakpoints
        /**
        * @nameZH 屏幕宽度（像素） - 超小屏幕
        * @nameEN Screen width (pixels) - Extra small screens
        * @desc 控制超小屏幕的屏幕宽度。
        * @descEN Control the screen width of extra small screens.
        */
        public int ScreenXS { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 超小屏幕最小值
        * @nameEN Screen width (pixels) - Extra small screens minimum value
        * @desc 控制超小屏幕的最小宽度。
        * @descEN Control the minimum width of extra small screens.
        */
        public int ScreenXSMin { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 超小屏幕最大值
        * @nameEN Screen width (pixels) - Extra small screens maximum value
        * @desc 控制超小屏幕的最大宽度。
        * @descEN Control the maximum width of extra small screens.
        */
        public int ScreenXSMax { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 小屏幕
        * @nameEN Screen width (pixels) - Small screens
        * @desc 控制小屏幕的屏幕宽度。
        * @descEN Control the screen width of small screens.
        */
        public int ScreenSM { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 小屏幕最小值
        * @nameEN Screen width (pixels) - Small screens minimum value
        * @desc 控制小屏幕的最小宽度。
        * @descEN Control the minimum width of small screens.
        */
        public int ScreenSMMin { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 小屏幕最大值
        * @nameEN Screen width (pixels) - Small screens maximum value
        * @desc 控制小屏幕的最大宽度。
        * @descEN Control the maximum width of small screens.
        */
        public int ScreenSMMax { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 中等屏幕
        * @nameEN Screen width (pixels) - Medium screens
        * @desc 控制中等屏幕的屏幕宽度。
        * @descEN Control the screen width of medium screens.
        */
        public int ScreenMD { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 中等屏幕最小值
        * @nameEN Screen width (pixels) - Medium screens minimum value
        * @desc 控制中等屏幕的最小宽度。
        * @descEN Control the minimum width of medium screens.
        */
        public int ScreenMDMin { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 中等屏幕最大值
        * @nameEN Screen width (pixels) - Medium screens maximum value
        * @desc 控制中等屏幕的最大宽度。
        * @descEN Control the maximum width of medium screens.
        */
        public int ScreenMDMax { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 大屏幕
        * @nameEN Screen width (pixels) - Large screens
        * @desc 控制大屏幕的屏幕宽度。
        * @descEN Control the screen width of large screens.
        */
        public int ScreenLG { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 大屏幕最小值
        * @nameEN Screen width (pixels) - Large screens minimum value
        * @desc 控制大屏幕的最小宽度。
        * @descEN Control the minimum width of large screens.
        */
        public int ScreenLGMin { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 大屏幕最大值
        * @nameEN Screen width (pixels) - Large screens maximum value
        * @desc 控制大屏幕的最大宽度。
        * @descEN Control the maximum width of large screens.
        */
        public int ScreenLGMax { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 超大屏幕
        * @nameEN Screen width (pixels) - Extra large screens
        * @desc 控制超大屏幕的屏幕宽度。
        * @descEN Control the screen width of extra large screens.
        */
        public int ScreenXL { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 超大屏幕最小值
        * @nameEN Screen width (pixels) - Extra large screens minimum value
        * @desc 控制超大屏幕的最小宽度。
        * @descEN Control the minimum width of extra large screens.
        */
        public int ScreenXLMin { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 超大屏幕最大值
        * @nameEN Screen width (pixels) - Extra large screens maximum value
        * @desc 控制超大屏幕的最大宽度。
        * @descEN Control the maximum width of extra large screens.
        */
        public int ScreenXLMax { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 超超大屏幕
        * @nameEN Screen width (pixels) - Extra extra large screens
        * @desc 控制超超大屏幕的屏幕宽度。
        * @descEN Control the screen width of extra extra large screens.
        */
        public int ScreenXXL { get; set; }
        /**
        * @nameZH 屏幕宽度（像素） - 超超大屏幕最小值
        * @nameEN Screen width (pixels) - Extra extra large screens minimum value
        * @desc 控制超超大屏幕的最小宽度。
        * @descEN Control the minimum width of extra extra large screens.
        */
        public int ScreenXXLMin { get; set; }

        /**
        * Used for DefaultButton, Switch which has default outline
        * @desc 默认样式的 Outline 颜色
        * @descEN Default style outline color.
        */
        public string ControlTmpOutline { get; set; }

        // public component box-shadow FIXME { get; set; } should be removed
        /** @internal */
        public string BoxShadowPopoverArrow { get; set; }
        /** @internal */
        public string BoxShadowCard { get; set; }
        /** @internal */
        public string BoxShadowDrawerRight { get; set; }
        /** @internal */
        public string BoxShadowDrawerLeft { get; set; }
        /** @internal */
        public string BoxShadowDrawerUp { get; set; }
        /** @internal */
        public string BoxShadowDrawerDown { get; set; }
        /** @internal */
        public string BoxShadowTabsOverflowLeft { get; set; }
        /** @internal */
        public string BoxShadowTabsOverflowRight { get; set; }
        /** @internal */
        public string BoxShadowTabsOverflowTop { get; set; }
        /** @internal */
        public string BoxShadowTabsOverflowBottom { get; set; }
    }

}