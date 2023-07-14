namespace AntDesign
{
    public partial class GlobalToken
    {
        /**
        * @internal
        */
        public string ColorTextBase { get; set; }

        /**
        * @internal
        */
        public string ColorBgBase { get; set; }

        // ----------   Text   ---------- //

        /**
        * @nameZH 一级文本色
        * @desc 最深的文本色。为了符合W3C标准，默认的文本颜色使用了该色，同时这个颜色也是最深的中性色。
        * @descEN Default text color which comply with W3C standards, and this color is also the darkest neutral color.
        */
        public string ColorText { get; set; }

        /**
        * @nameZH 二级文本色
        * @desc 作为第二梯度的文本色，一般用在不那么需要强化文本颜色的场景，例如 Label 文本、Menu 的文本选中态等场景。
        * @descEN The second level of text color is generally used in scenarios where text color is not emphasized, such as label text, menu text selection state, etc.
        */
        public string ColorTextSecondary { get; set; }

        /**
        * @nameZH 三级文本色
        * @desc 第三级文本色一般用于描述性文本，例如表单的中的补充说明文本、列表的描述性文本等场景。
        * @descEN The third level of text color is generally used for descriptive text, such as form supplementary explanation text, list descriptive text, etc.
        */
        public string ColorTextTertiary { get; set; }

        /**
        * @nameZH 四级文本色
        * @desc 第四级文本色是最浅的文本色，例如表单的输入提示文本、禁用色文本等。
        * @descEN The fourth level of text color is the lightest text color, such as form input prompt text, disabled color text, etc.
        */
        public string ColorTextQuaternary { get; set; }

        // ----------   Border   ---------- //

        /**
        * @nameZH 一级边框色
        * @nameEN Default Border Color
        * @desc 默认使用的边框颜色, 用于分割不同的元素，例如：表单的分割线、卡片的分割线等。
        * @descEN Default border color, used to separate different elements, such public form separator, card separator As { get; set; } etc.
        */
        public string ColorBorder { get; set; }

        /**
        * @nameZH 二级边框色
        * @nameEN Secondary Border Color
        * @desc 比默认使用的边框色要浅一级，此颜色和 colorSplit 的颜色一致。使用的是实色。
        * @descEN Slightly lighter than the default border color, this color is the same as `colorSplit`. Solid color is used.
        */
        public string ColorBorderSecondary { get; set; }

        // ----------   Fill   ---------- //

        /**
        * @nameZH 一级填充色
        * @desc 最深的填充色，用于拉开与二、三级填充色的区分度，目前只用在 Slider 的 hover 效果。
        * @descEN The darkest fill color is used to distinguish between the second and third level of fill color, and is currently only used in the hover effect of Slider.
        */
        public string ColorFill { get; set; }

        /**
        * @nameZH 二级填充色
        * @desc 二级填充色可以较为明显地勾勒出元素形体，如 Rate、Skeleton 等。也可以作为三级填充色的 Hover 状态，如 Table 等。
        * @descEN The second level of fill color can outline the shape of the element more clearly, such as Rate, Skeleton, etc. It can also be used as the Hover state of the third level of fill color, such as Table, etc.
        */
        public string ColorFillSecondary { get; set; }

        /**
        * @nameZH 三级填充色
        * @desc 三级填充色用于勾勒出元素形体的场景，如 Slider、Segmented 等。如无强调需求的情况下，建议使用三级填色作为默认填色。
        * @descEN The third level of fill color is used to outline the shape of the element, such as Slider, Segmented, etc. If there is no emphasis requirement, it is recommended to use the third level of fill color as the default fill color.
        */
        public string ColorFillTertiary { get; set; }

        /**
        * @nameZH 四级填充色
        * @desc 最弱一级的填充色，适用于不易引起注意的色块，例如斑马纹、区分边界的色块等。
        * @descEN The weakest level of fill color is suitable for color blocks that are not easy to attract attention, such as zebra stripes, color blocks that distinguish boundaries, etc.
        */
        public string ColorFillQuaternary { get; set; }

        // ----------   Surface   ---------- //

        /**
        * @nameZH 布局背景色
        * @desc 该色用于页面整体布局的背景色，只有需要在页面中处于 B1 的视觉层级时才会使用该 token，其他用法都是错误的
        * @descEN This color is used for the background color of the overall layout of the page. This token will only be used when it is necessary to be at the B1 visual level in the page. Other usages are wrong.
        */
        public string ColorBgLayout { get; set; }

        /**
        * @nameZH 组件容器背景色
        * @desc 组件的容器背景色，例如：默认按钮、输入框等。务必不要将其与 `colorBgElevated` 混淆。
        * @descEN Container background color, e.public default button, input box G { get; set; } etc. Be sure not to confuse this with `colorBgElevated`.
        */
        public string ColorBgContainer { get; set; }

        /**
        * @nameZH 浮层容器背景色
        * @desc 浮层容器背景色，在暗色模式下该 token 的色值会比 `colorBgContainer` 要亮一些。例如：模态框、弹出框、菜单等。
        * @descEN Container background color of the popup layer, in dark mode the color value of this token will be a little brighter than `colorBgContainer`. E.public modal, pop-up, menu G { get; set; } etc.
        */
        public string ColorBgElevated { get; set; }

        /**
        * @nameZH 引起注意的背景色
        * @desc 该色用于引起用户强烈关注注意的背景色，目前只用在 Tooltip 的背景色上。
        * @descEN This color is used to draw the user's strong attention to the background color, and is currently only used in the background color of Tooltip.
        */
        public string ColorBgSpotlight { get; set; }
    }

    /**
        * 品牌色梯度变量
        */
    public partial class GlobalToken
    {
        /**
        * @nameZH 品牌主色
        * @nameEN Primary color of the brand
        * @desc 品牌色是体现产品特性和传播理念最直观的视觉元素之一，用于产品的主色调、主按钮、主图标、主文本等
        * @descEN The brand color is one of the most intuitive visual elements that reflects product characteristics and communication concepts, and is used for the main color tone, main buttons, main icons, main text, etc. of the product.
        */
        public string ColorPrimary { get; set; } // 6

        /**
        * @nameZH 主色浅色背景色
        * @nameEN Light background color of primary color
        * @desc 主色浅色背景颜色，一般用于视觉层级较弱的选中状态。
        * @descEN Light background color of primary color, usually used for weak visual level selection state.
        */
        public string ColorPrimaryBg { get; set; } // 1

        /**
        * @nameZH 主色浅色背景悬浮态
        * @nameEN Hover state of light background color of primary color
        * @desc 与主色浅色背景颜色相对应的悬浮态颜色。
        * @descEN The hover state color corresponding to the light background color of the primary color.
        */
        public string ColorPrimaryBgHover { get; set; } // 2

        /**
        * @nameZH 主色描边色
        * @nameEN Border color of primary color
        * @desc 主色梯度下的描边用色，用在 Slider 等组件的描边上。
        * @descEN The stroke color under the main color gradient, used on the stroke of components such as Slider.
        */
        public string ColorPrimaryBorder { get; set; } // 3

        /**
        * @nameZH 主色描边色悬浮态
        * @nameEN Hover state of border color of primary color
        * @desc 主色梯度下的描边用色的悬浮态，Slider 、Button 等组件的描边 Hover 时会使用。
        * @descEN The hover state of the stroke color under the main color gradient, which will be used when the stroke Hover of components such as Slider and Button.
        */
        public string ColorPrimaryBorderHover { get; set; } // 4

        /**
        * @nameZH 主色悬浮态
        * @nameEN Hover state of primary color
        * @desc 主色梯度下的悬浮态。
        * @descEN Hover state under the main color gradient.
        */
        public string ColorPrimaryHover { get; set; } // 5

        /**
        * @nameZH 主色激活态
        * @nameEN Active state of primary color
        * @desc 主色梯度下的深色激活态。
        * @descEN Dark active state under the main color gradient.
        */
        public string ColorPrimaryActive { get; set; } // 7

        /**
        * @nameZH 主色文本悬浮态
        * @nameEN Hover state of text color of primary color
        * @desc 主色梯度下的文本悬浮态。
        * @descEN Hover state of text color under the main color gradient.
        */
        public string ColorPrimaryTextHover { get; set; } // 8

        /**
        * @nameZH 主色文本
        * @nameEN Text color of primary color
        * @desc 主色梯度下的文本颜色。
        * @descEN Text color under the main color gradient.
        */
        public string ColorPrimaryText { get; set; } // 9

        /**
        * @nameZH 主色文本激活态
        * @nameEN Active state of text color of primary color
        * @desc 主色梯度下的文本激活态。
        * @descEN Active state of text color under the main color gradient.
        */
        public string ColorPrimaryTextActive { get; set; } // 10
    }

    public partial class GlobalToken
    {
        /**
        * @nameZH 成功色的浅色背景颜色
        * @nameEN Light Background Color of Success Color
        * @desc 成功色的浅色背景颜色，用于 Tag 和 Alert 的成功态背景色
        * @descEN Light background color of success color, used for Tag and Alert success state background color
        */
        public string ColorSuccessBg { get; set; } // 1

        /**
        * @nameZH 成功色的浅色背景色悬浮态
        * @nameEN Hover State Color of Light Success Background
        * @desc 成功色浅色背景颜色，一般用于视觉层级较弱的选中状态，不过 antd 目前没有使用到该 token
        * @descEN Light background color of success color, but antd does not use this token currently
        */
        public string ColorSuccessBgHover { get; set; } // 2

        /**
        * @nameZH 成功色的描边色
        * @nameEN Border Color of Success Color
        * @desc 成功色的描边色，用于 Tag 和 Alert 的成功态描边色
        * @descEN Border color of success color, used for Tag and Alert success state border color
        */
        public string ColorSuccessBorder { get; set; } // 3

        /**
        * @nameZH 成功色的描边色悬浮态
        * @nameEN Hover State Color of Success Border
        * @desc 成功色的描边色悬浮态
        * @descEN Hover state color of success color border
        */
        public string ColorSuccessBorderHover { get; set; } // 4

        /**
        * @nameZH 成功色的深色悬浮态
        * @nameEN Hover State Color of Dark Success
        * @desc 成功色的深色悬浮态
        * @descEN Hover state color of dark success color
        */
        public string ColorSuccessHover { get; set; } // 5

        /**
        * @nameZH 成功色
        * @nameEN Success Color
        * @desc 默认的成功色，如 Result、Progress 等组件中都有使用该颜色
        * @descEN Default success color, used in components such as Result and Progress
        */
        public string ColorSuccess { get; set; } // 6

        /**
        * @nameZH 成功色的深色激活态
        * @nameEN Active State Color of Dark Success
        * @desc 成功色的深色激活态
        * @descEN Active state color of dark success color
        */
        public string ColorSuccessActive { get; set; } // 7

        /**
        * @nameZH 成功色的文本悬浮态
        * @nameEN Hover State Color of Success Text
        * @desc 成功色的文本悬浮态
        * @descEN Hover state color of success color text
        */
        public string ColorSuccessTextHover { get; set; } // 8

        /**
        * @nameZH 成功色的文本默认态
        * @nameEN Default State Color of Success Text
        * @desc 成功色的文本默认态
        * @descEN Default state color of success color text
        */
        public string ColorSuccessText { get; set; } // 9

        /**
        * @nameZH 成功色的文本激活态
        * @nameEN Active State Color of Success Text
        * @desc 成功色的文本激活态
        * @descEN Active state color of success color text
        */
        public string ColorSuccessTextActive { get; set; } // 10
    }

    public partial class GlobalToken
    {
        /**
        * @nameZH 警戒色的浅色背景颜色
        * @nameEN Warning background color
        * @desc 警戒色的浅色背景颜色
        * @descEN The background color of the warning state.
        */
        public string ColorWarningBg { get; set; } // 1

        /**
        * @nameZH 警戒色的浅色背景色悬浮态
        * @nameEN Warning background color hover state
        * @desc 警戒色的浅色背景色悬浮态
        * @descEN The hover state background color of the warning state.
        */
        public string ColorWarningBgHover { get; set; } // 2

        /**
        * @nameZH 警戒色的描边色
        * @nameEN Warning border color
        * @desc 警戒色的描边色
        * @descEN The border color of the warning state.
        */
        public string ColorWarningBorder { get; set; } // 3

        /**
        * @nameZH 警戒色的描边色悬浮态
        * @nameEN Warning border color hover state
        * @desc 警戒色的描边色悬浮态
        * @descEN The hover state border color of the warning state.
        */
        public string ColorWarningBorderHover { get; set; } // 4

        /**
        * @nameZH 警戒色的深色悬浮态
        * @nameEN Warning hover color
        * @desc 警戒色的深色悬浮态
        * @descEN The hover state of the warning color.
        */
        public string ColorWarningHover { get; set; } // 5

        /**
        * @nameZH 警戒色
        * @nameEN Warning color
        * @desc 最常用的警戒色，例如 Notification、 Alert等警告类组件或 Input 输入类等组件会使用该颜色
        * @descEN The most commonly used warning color, used for warning components such as Notification, Alert, or input components.
        */
        public string ColorWarning { get; set; } // 6

        /**
        * @nameZH 警戒色的深色激活态
        * @nameEN Warning active color
        * @desc 警戒色的深色激活态
        * @descEN The active state of the warning color.
        */
        public string ColorWarningActive { get; set; } // 7

        /**
        * @nameZH 警戒色的文本悬浮态
        * @nameEN Warning text hover state
        * @desc 警戒色的文本悬浮态
        * @descEN The hover state of the text in the warning color.
        */
        public string ColorWarningTextHover { get; set; } // 8

        /**
        * @nameZH 警戒色的文本默认态
        * @nameEN Warning text default state
        * @desc 警戒色的文本默认态
        * @descEN The default state of the text in the warning color.
        */
        public string ColorWarningText { get; set; } // 9

        /**
        * @nameZH 警戒色的文本激活态
        * @nameEN Warning text active state
        * @desc 警戒色的文本激活态
        * @descEN The active state of the text in the warning color.
        */
        public string ColorWarningTextActive { get; set; } // 10
    }

    public partial class GlobalToken
    {
        /**
        * @nameZH 信息色的浅色背景颜色
        * @nameEN Light background color of information color
        * @desc 信息色的浅色背景颜色。
        * @descEN Light background color of information color.
        */
        public string ColorInfoBg { get; set; } // 1

        /**
        * @nameZH 信息色的浅色背景色悬浮态
        * @nameEN Hover state of light background color of information color
        * @desc 信息色的浅色背景色悬浮态。
        * @descEN Hover state of light background color of information color.
        */
        public string ColorInfoBgHover { get; set; } // 2

        /**
        * @nameZH 信息色的描边色
        * @nameEN Border color of information color
        * @desc 信息色的描边色。
        * @descEN Border color of information color.
        */
        public string ColorInfoBorder { get; set; } // 3

        /**
        * @nameZH 信息色的描边色悬浮态
        * @nameEN Hover state of border color of information color
        * @desc 信息色的描边色悬浮态。
        * @descEN Hover state of border color of information color.
        */
        public string ColorInfoBorderHover { get; set; } // 4

        /**
        * @nameZH 信息色的深色悬浮态
        * @nameEN Hover state of dark color of information color
        * @desc 信息色的深色悬浮态。
        * @descEN Hover state of dark color of information color.
        */
        public string ColorInfoHover { get; set; } // 5

        /**
        * @nameZH 信息色
        * @nameEN Information color
        * @desc 信息色。
        * @descEN Information color.
        */
        public string ColorInfo { get; set; } // 6

        /**
        * @nameZH 信息色的深色激活态
        * @nameEN Active state of dark color of information color
        * @desc 信息色的深色激活态。
        * @descEN Active state of dark color of information color.
        */
        public string ColorInfoActive { get; set; } // 7

        /**
        * @nameZH 信息色的文本悬浮态
        * @nameEN Hover state of text color of information color
        * @desc 信息色的文本悬浮态。
        * @descEN Hover state of text color of information color.
        */
        public string ColorInfoTextHover { get; set; } // 8

        /**
        * @nameZH 信息色的文本默认态
        * @nameEN Default state of text color of information color
        * @desc 信息色的文本默认态。
        * @descEN Default state of text color of information color.
        */
        public string ColorInfoText { get; set; } // 9

        /**
        * @nameZH 信息色的文本激活态
        * @nameEN Active state of text color of information color
        * @desc 信息色的文本激活态。
        * @descEN Active state of text color of information color.
        */
        public string ColorInfoTextActive { get; set; } // 10
    }

    public partial class GlobalToken
    {
        /**
        * @nameZH 错误色的浅色背景颜色
        * @nameEN Error background color
        * @desc 错误色的浅色背景颜色
        * @descEN The background color of the error state.
        */
        public string ColorErrorBg { get; set; } // 1

        /**
        * @nameZH 错误色的浅色背景色悬浮态
        * @nameEN Error background color hover state
        * @desc 错误色的浅色背景色悬浮态
        * @descEN The hover state background color of the error state.
        */
        public string ColorErrorBgHover { get; set; } // 2

        /**
        * @nameZH 错误色的描边色
        * @nameEN Error border color
        * @desc 错误色的描边色
        * @descEN The border color of the error state.
        */
        public string ColorErrorBorder { get; set; } // 3

        /**
        * @nameZH 错误色的描边色悬浮态
        * @nameEN Error border color hover state
        * @desc 错误色的描边色悬浮态
        * @descEN The hover state border color of the error state.
        */
        public string ColorErrorBorderHover { get; set; } // 4

        /**
        * @nameZH 错误色的深色悬浮态
        * @nameEN Error hover color
        * @desc 错误色的深色悬浮态
        * @descEN The hover state of the error color.
        */
        public string ColorErrorHover { get; set; } // 5

        /**
        * @nameZH 错误色
        * @nameEN Error color
        * @desc 错误色
        * @descEN The color of the error state.
        */
        public string ColorError { get; set; } // 6

        /**
        * @nameZH 错误色的深色激活态
        * @nameEN Error active color
        * @desc 错误色的深色激活态
        * @descEN The active state of the error color.
        */
        public string ColorErrorActive { get; set; } // 7

        /**
        * @nameZH 错误色的文本悬浮态
        * @nameEN Error text hover state
        * @desc 错误色的文本悬浮态
        * @descEN The hover state of the text in the error color.
        */
        public string ColorErrorTextHover { get; set; } // 8

        /**
        * @nameZH 错误色的文本默认态
        * @nameEN Error text default state
        * @desc 错误色的文本默认态
        * @descEN The default state of the text in the error color.
        */
        public string ColorErrorText { get; set; } // 9

        /**
        * @nameZH 错误色的文本激活态
        * @nameEN Error text active state
        * @desc 错误色的文本激活态
        * @descEN The active state of the text in the error color.
        */
        public string ColorErrorTextActive { get; set; } // 10
    }

    public partial class GlobalToken
    {
        /**
        * @nameZH 纯白色
        * @desc 不随主题变化的纯白色
        * @descEN Pure white color don't changed by theme
        * @default #FFFFFF
        */
        public string ColorWhite { get; set; }

        /**
        * @nameZH 浮层的背景蒙层颜色
        * @nameEN Background color of the mask
        * @desc 浮层的背景蒙层颜色，用于遮罩浮层下面的内容，Modal、Drawer 等组件的蒙层使用的是该 token
        * @descEN The background color of the mask, used to cover the content below the mask, Modal, Drawer and other components use this token
        */
        public string ColorBgMask { get; set; }

        /**
        * @nameZH 纯黑色
        * @desc 不随主题变化的纯黑色
        * @default #0000
        */
        // public string ColorBlack { get; set; }
    }

}