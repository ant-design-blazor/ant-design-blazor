import { CsOptions } from "./CsBuilder";

export type Component = {
    name: string;
    src: string[];
    dist: string;
    csOptions: CsOptions;
}

const defaultOptions = {
    usings: [
        'using System;',
        'using CssInCs;',
        'using static AntDesign.GlobalStyle;',
        'using static AntDesign.Theme;',
    ],
    namespace: 'AntDesign',
    defaultTab: '    '
}

/**
 * 使用说明：
 * typeMap：用于ts和cs类型的映射，无法推导的类型默认会以Unknown_{index}方式生成，
 *          对Unknown类型添加手动映射后，会转成手动映射类型。typeMap是在生成过程中进行替换。
 * transform：是在cs代码生成后，对生成的cs代码进行局部调整。
 */
export const components: Component[] = [
    {
        name: 'Affix',
        src: ['components/affix/style/index.ts'],
        dist: 'components/affix/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Affix',
            typeMap: [
                { from: 'Unknown', to: 'AffixToken', includes: [2, 4] },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [1, 5] },
                { from: 'Unknown', to: 'GlobalToken', includes: [3] },
            ],
            transforms: [
                { source: 'class AffixToken', target: 'class AffixToken : TokenWithCommonCls' },
                { source: 'class Affix', target: 'partial class Affix' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Alert',
        src: ['components/alert/style/index.ts'],
        dist: 'components/alert/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Alert',
            typeMap: [
                { from: 'CSSInterpolation', to: 'CSSObject[]' },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [1, 4] },
                { from: 'Unknown', to: 'GlobalToken', includes: [2] },
                { from: 'Unknown', to: 'AlertToken', includes: [3] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class AlertToken : TokenWithCommonCls' },
                { source: 'class AlertToken', target: 'partial class AlertToken' },
                { source: 'class Alert', target: 'partial class Alert' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Anchor',
        src: ['components/anchor/style/index.ts'],
        dist: 'components/anchor/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Anchor',
            typeMap: [
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [1, 6] },
                { from: 'Unknown', to: 'AnchorToken', includes: [2, 3, 5] },
                { from: 'Unknown', to: 'GlobalToken', includes: [4] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class AnchorToken : TokenWithCommonCls' },
                { source: 'class AnchorToken', target: 'partial class AnchorToken' },
                { source: 'class Anchor', target: 'partial class Anchor' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
                { source: 'textEllipsis', target: 'TextEllipsis' }, // 无法推导这个是局部变量还是全局变量，手动映射
            ]
        },
    },
    {
        name: 'Avatar',
        src: ['components/avatar/style/index.ts'],
        dist: 'components/avatar/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Avatar',
            typeMap: [
                { from: 'Unknown', to: 'CSSObject', includes: [1, 2, 5, 6, 7, 8, 9, 10, 11, 13, 14, 15, 16] },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [3, 19] },
                { from: 'Unknown', to: 'AvatarToken', includes: [4, 12, 18] },
                { from: 'Unknown', to: 'GlobalToken', includes: [17] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class AvatarToken : TokenWithCommonCls' },
                { source: 'class AvatarToken', target: 'partial class AvatarToken' },
                { source: 'class Avatar', target: 'partial class Avatar' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
                { source: 'AvatarSizeStyle', target: 'avatarSizeStyle' },
                { source: 'Math.Round(', target: '(int)Math.Round((double)' }
            ]
        }
    },
    {
        name: 'BackTop',
        src: ['components/back-top/style/index.ts'],
        dist: 'components/back-top/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'BackTop',
            typeMap: [
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [1, 7] },
                { from: 'Unknown', to: 'BackTopToken', includes: [2, 3, 4, 6, 8, 9] },
                { from: 'Unknown', to: 'GlobalToken', includes: [5] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class BackTopToken : TokenWithCommonCls' },
                { source: 'class BackTopToken', target: 'partial class BackTopToken' },
                { source: 'class BackTop', target: 'partial class BackTop' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook(GlobalToken token)', target: 'protected override CSSInterpolation[] UseStyle(GlobalToken token)' },
                { source: 'controlHeightLG * 1.25', target: '(int)(controlHeightLG * 1.25)' },
                { source: 'controlHeightLG * 2.5', target: '(int)(controlHeightLG * 2.5)' },
                { source: 'controlHeightLG * 1.5', target: '(int)(controlHeightLG * 1.5)' },
                { source: 'controlHeightLG * 0.5', target: '(int)(controlHeightLG * 0.5)' },
            ]
        }
    },
    {
        name: 'Badge',
        src: ['components/badge/style/index.ts'],
        dist: 'components/badge/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat(['using Keyframes = CssInCs.Keyframe;']),
            defaultClass: 'Badge',
            typeMap: [
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [3, 10] },
                { from: 'Unknown', to: 'CSSObject', includes: [4, 5, 6, 7] },
                { from: 'Unknown', to: 'GlobalToken', includes: [8] },
                { from: 'Unknown', to: 'BadgeToken', includes: [9] },
                { from: 'Keyframes', to: 'CSSObject' },
            ],
            transforms: [
                { source: 'class BadgeToken', target: 'partial class BadgeToken : TokenWithCommonCls' },
                { source: 'class Badge', target: 'partial class Badge' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
                { source: 'Math.Round(fontSize * lineHeight)', target: '(int)Math.Round((double)fontSize * lineHeight)' }
            ]
        }
    },
    {
        name: 'Breadcrumb',
        src: ['components/breadcrumb/style/index.ts'],
        dist: 'components/breadcrumb/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Breadcrumb',
            typeMap: [
                { from: 'Unknown', to: 'CSSObject', includes: [1], ranges: [[4, 20]] },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [2, 23] },
                { from: 'Unknown', to: 'BreadcrumbToken', includes: [3, 22] },
                { from: 'Unknown', to: 'GlobalToken', includes: [21] },
            ],
            transforms: [
                { source: 'class BreadcrumbToken', target: 'partial class BreadcrumbToken : TokenWithCommonCls' },
                { source: 'class Breadcrumb', target: 'partial class Breadcrumb' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' }
            ]
        }
    },
    {
        name: 'Button',
        src: [
            'components/button/style/index.ts',
            'components/button/style/group.ts'
        ],
        dist: 'components/button/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Button',
            typeMap: [
                { from: 'CSSInterpolation', to: 'CSSObject[]' },
                { from: 'Unknown', to: 'CSSObject', includes: [17, 19, 20, 23, 25, 27, 28, 29, 31, 32, 34, 36, 37, 57, 73, 74, 76, 107], ranges: [[1, 13], [39, 44], [46, 55], [59, 64], [66, 71], [77, 85], [92, 94], [98, 105], [109, 118]] },
                { from: 'Unknown', to: 'CSSObject[]', includes: [14, 15, 16, 108] },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [18, 97] },
                { from: 'Unknown', to: 'ButtonToken', includes: [21, 22, 24, 26, 30, 33, 35, 38, 45, 56, 58, 65, 72, 75, 96, 106], ranges: [[86, 91]] },
                { from: 'Unknown', to: 'GlobalToken', includes: [95] },
            ],
            transforms: [
                { source: 'class ButtonToken', target: 'partial class ButtonToken : TokenWithCommonCls' },
                { source: 'class Button', target: 'partial class Button' },
                { source: 'public CSSObject[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
                { source: 'CSSObject hoverStyle, CSSObject activeStyle', target: 'CSSObject hoverStyle = default, CSSObject activeStyle = default' }
            ]
        }
    },
    {
        name: 'Calendar',
        src: ['components/calendar/style/index.ts'],
        dist: 'components/calendar/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Calendar',
            typeMap: [
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [1, 4] },
                { from: 'Unknown', to: 'CalendarToken', includes: [3] },
                { from: 'Unknown', to: 'GlobalToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CalendarToken : TokenWithCommonCls' },
                { source: 'class CalendarToken', target: 'partial class CalendarToken' },
                { source: 'class Calendar', target: 'partial class Calendar' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' }
            ]
        }
    },
    {
        name: 'Card',
        src: ['components/card/style/index.ts'],
        dist: 'components/card/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Card',
            typeMap: [
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [1, 12] },
                { from: 'Unknown', to: 'CardToken', includes: [11], ranges: [[2, 9]] },
                { from: 'Unknown', to: 'GlobalToken', includes: [10] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CardToken : TokenWithCommonCls' },
                { source: 'class CardToken', target: 'partial class CardToken' },
                { source: 'class Card', target: 'partial class Card' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
                { source: 'textEllipsis', target: 'TextEllipsis' }
            ]
        }
    },
    {
        name: 'Carousel',
        src: ['components/carousel/style/index.ts'],
        dist: 'components/carousel/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Carousel',
            typeMap: [
                { from: 'Unknown', to: 'CSSObject', includes: [1, 2], ranges: [[6, 44], [46, 53], [56, 63]] },
                { from: 'Unknown', to: 'CarouselToken', includes: [5, 45, 54, 65] },
                { from: 'Unknown', to: 'CSSObject[]', includes: [3, 55] },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [4, 66] },
                { from: 'Unknown', to: 'GlobalToken', includes: [64] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CarouselToken : TokenWithCommonCls' },
                { source: 'class CarouselToken', target: 'partial class CarouselToken' },
                { source: 'class Carousel', target: 'partial class Carousel' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
                { source: '-carouselArrowSize * 1.25', target: '(int)(-carouselArrowSize * 1.25)' },
            ]
        }
    },
    {
        name: 'Cascader',
        src: ['components/cascader/style/index.ts'],
        dist: 'components/cascader/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Cascader',
            typeMap: [
                { from: 'Unknown', to: 'CSSObject', ranges: [[5, 31]] },
                { from: 'Unknown', to: 'CSSObject[]', includes: [1, 4, 8] },
                { from: 'Unknown', to: 'CascaderToken', includes: [3] },
                { from: 'Unknown', to: 'GlobalToken', includes: [32] },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [2, 33] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CascaderToken : TokenWithCommonCls' },
                { source: 'class CascaderToken', target: 'partial class CascaderToken' },
                { source: 'class Cascader', target: 'partial class Cascader' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
                { source: '["-ms-overflow-style"]', target: 'MsOverflowStyle' },
                { source: 'textEllipsis', target: 'TextEllipsis' },
                { source: 'GenBaseStyle(token)', target: 'GenBaseStyle(token as CascaderToken)' },
                { source: 'Math.Round(', target: 'Math.Round((double)' },
            ]
        }
    },
    {
        name: 'Checkbox',
        src: ['components/checkbox/style/index.ts'],
        dist: 'components/checkbox/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat(['using Keyframes = CssInCs.Keyframe;']),
            defaultClass: 'Checkbox',
            typeMap: [
                { from: 'Keyframes', to: 'CSSObject' },
                { from: 'Unknown', to: 'CSSObject[]', includes: [1, 2, 5, 50] },
                { from: 'Unknown', to: 'CSSObject', ranges: [[6, 48]] },
                { from: 'Unknown', to: 'CheckboxToken', includes: [4, 49] },
                { from: 'Unknown', to: 'GlobalToken', includes: [51] },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [3, 53] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CheckboxToken : TokenWithCommonCls' },
                { source: 'class CheckboxToken', target: 'partial class CheckboxToken' },
                { source: 'class Checkbox', target: 'partial class Checkbox' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
                { source: `FullToken<'Checkbox'>`, target: 'GlobalToken' },
                { source: 'new CSSObject[] { GenCheckboxStyle(checkboxToken) }', target: 'GenCheckboxStyle(checkboxToken)' },
            ]
        }
    },
    {
        name: 'Collapse',
        src: ['components/collapse/style/index.ts'],
        dist: 'components/collapse/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Collapse',
            typeMap: [
                { from: 'Unknown', to: 'CSSObject', includes: [1, 2, 3, 4], ranges: [[7, 45], [47, 49], [51, 57], [59, 63]] },
                { from: 'Unknown', to: 'CollapseToken', includes: [6, 46, 50, 58, 65] },
                { from: 'Unknown', to: 'GlobalToken', includes: [64] },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [5, 66] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CollapseToken : TokenWithCommonCls' },
                { source: 'class CollapseToken', target: 'partial class CollapseToken' },
                { source: 'class Collapse', target: 'partial class Collapse' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' }
            ]
        }
    },
    {
        name: 'DatePicker',
        src: ['components/date-picker/style/index.ts'],
        dist: 'components/date-picker/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'DatePicker',
            typeMap: [
                { from: 'Unknown', to: 'CSSObject', includes: [1], ranges: [[6, 15], [18, 94]] },
                { from: 'Unknown', to: 'DatePickerToken', includes: [4, 5, 16, 98] },
                { from: 'Unknown', to: 'CSSObject[]', includes: [2, 17] },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [3, 96] },
                { from: 'Unknown', to: 'GlobalToken', includes: [95, 97] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class DatePickerToken : TokenWithCommonCls' },
                { source: 'class DatePickerToken', target: 'partial class DatePickerToken' },
                { source: 'class DatePicker', target: 'partial class DatePicker' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
                { source: 'textEllipsis', target: 'TextEllipsis' }
            ]
        }
    },
    {
        name: 'Descriptions',
        src: ['components/descriptions/style/index.ts'],
        dist: 'components/descriptions/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Descriptions',
            typeMap: [
                { from: 'Unknown', to: 'CSSObject', includes: [1], ranges: [[3, 29]] },
                { from: 'Unknown', to: 'DescriptionsToken', includes: [31] },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [2, 32] },
                { from: 'Unknown', to: 'GlobalToken', includes: [30] },
            ],
            transforms: [
                { source: 'class DescriptionsToken', target: 'partial class DescriptionsToken : TokenWithCommonCls' },
                { source: 'class Descriptions', target: 'partial class Descriptions' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
                { source: 'textEllipsis', target: 'TextEllipsis' }
            ]
        }
    },
    {
        name: 'Divider',
        src: ['components/divider/style/index.ts'],
        dist: 'components/divider/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Divider',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class DividerToken' },
                { source: 'class DividerToken', target: 'partial class DividerToken : TokenWithCommonCls' },
                { source: 'class Divider', target: 'partial class Divider' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Drawer',
        src: ['components/drawer/style/index.ts'],
        dist: 'components/drawer/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Drawer',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class DrawerToken' },
                { source: 'class DrawerToken', target: 'partial class DrawerToken : TokenWithCommonCls' },
                { source: 'class Drawer', target: 'partial class Drawer' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Dropdown',
        src: [
            'components/dropdown/style/index.ts',
            'components/dropdown/style/status.ts',
        ],
        dist: 'components/dropdown/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Dropdown',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class DropdownToken' },
                { source: 'class DropdownToken', target: 'partial class DropdownToken : TokenWithCommonCls' },
                { source: 'class Dropdown', target: 'partial class Dropdown' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Empty',
        src: ['components/empty/style/index.ts'],
        dist: 'components/empty/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Empty',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class EmptyToken' },
                { source: 'class EmptyToken', target: 'partial class EmptyToken : TokenWithCommonCls' },
                { source: 'class Empty', target: 'partial class Empty' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Form',
        src: [
            'components/form/style/index.ts',
            'components/form/style/explain.ts',
        ],
        dist: 'components/form/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Form',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class FormToken' },
                { source: 'class FormToken', target: 'partial class FormToken : TokenWithCommonCls' },
                { source: 'class Form', target: 'partial class Form' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Grid',
        src: ['components/grid/style/index.ts'],
        dist: 'components/grid/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Grid',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class GridToken' },
                { source: 'class GridToken', target: 'partial class GridToken : TokenWithCommonCls' },
                { source: 'class Grid', target: 'partial class Grid' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Image',
        src: ['components/image/style/index.ts'],
        dist: 'components/image/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Image',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class ImageToken' },
                { source: 'class ImageToken', target: 'partial class ImageToken : TokenWithCommonCls' },
                { source: 'class Image', target: 'partial class Image' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Input',
        src: ['components/input/style/index.ts'],
        dist: 'components/input/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Input',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class InputToken' },
                { source: 'class InputToken', target: 'partial class InputToken : TokenWithCommonCls' },
                { source: 'class Input', target: 'partial class Input' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'InputNumber',
        src: ['components/input-number/style/index.ts'],
        dist: 'components/input-number/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'InputNumber',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class InputNumberToken' },
                { source: 'class InputNumberToken', target: 'partial class InputNumberToken : TokenWithCommonCls' },
                { source: 'class InputNumber', target: 'partial class InputNumber' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Layout',
        src: [
            'components/layout/style/index.ts',
            'components/layout/style/light.ts',
        ],
        dist: 'components/layout/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Layout',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class LayoutToken' },
                { source: 'class LayoutToken', target: 'partial class LayoutToken : TokenWithCommonCls' },
                { source: 'class Layout', target: 'partial class Layout' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'List',
        src: ['components/list/style/index.ts'],
        dist: 'components/list/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'List',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class ListToken' },
                { source: 'class ListToken', target: 'partial class ListToken : TokenWithCommonCls' },
                { source: 'class List', target: 'partial class List' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Mentions',
        src: ['components/mentions/style/index.ts'],
        dist: 'components/mentions/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Mentions',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class MentionsToken' },
                { source: 'class MentionsToken', target: 'partial class MentionsToken : TokenWithCommonCls' },
                { source: 'class Mentions', target: 'partial class Mentions' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Menu',
        src: [
            'components/menu/style/index.tsx',
            'components/menu/style/horizontal.tsx',
            'components/menu/style/vertical.tsx',
            'components/menu/style/rtl.tsx',
            'components/menu/style/theme.tsx',
        ],
        dist: 'components/menu/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Menu',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class MenuToken' },
                { source: 'class MenuToken', target: 'partial class MenuToken : TokenWithCommonCls' },
                { source: 'class Menu', target: 'partial class Menu' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Message',
        src: ['components/message/style/index.tsx'],
        dist: 'components/message/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Message',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class MessageToken' },
                { source: 'class MessageToken', target: 'partial class MessageToken : TokenWithCommonCls' },
                { source: 'class Message', target: 'partial class Message' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Modal',
        src: ['components/modal/style/index.tsx'],
        dist: 'components/modal/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Modal',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class ModalToken' },
                { source: 'class ModalToken', target: 'partial class ModalToken : TokenWithCommonCls' },
                { source: 'class Modal', target: 'partial class Modal' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Notification',
        src: [
            'components/notification/style/index.tsx',
            'components/notification/style/placement.ts',
        ],
        dist: 'components/notification/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Notification',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class NotificationToken' },
                { source: 'class NotificationToken', target: 'partial class NotificationToken : TokenWithCommonCls' },
                { source: 'class Notification', target: 'partial class Notification' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Pagination',
        src: ['components/pagination/style/index.tsx'],
        dist: 'components/pagination/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Pagination',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class PaginationToken' },
                { source: 'class PaginationToken', target: 'partial class PaginationToken : TokenWithCommonCls' },
                { source: 'class Pagination', target: 'partial class Pagination' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Popconfirm',
        src: ['components/popconfirm/style/index.tsx'],
        dist: 'components/popconfirm/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Popconfirm',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class PopconfirmToken' },
                { source: 'class PopconfirmToken', target: 'partial class PopconfirmToken : TokenWithCommonCls' },
                { source: 'class Popconfirm', target: 'partial class Popconfirm' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Popover',
        src: ['components/popover/style/index.tsx'],
        dist: 'components/popover/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Popover',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class PopoverToken' },
                { source: 'class PopoverToken', target: 'partial class PopoverToken : TokenWithCommonCls' },
                { source: 'class Popover', target: 'partial class Popover' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Progress',
        src: ['components/progress/style/index.tsx'],
        dist: 'components/progress/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Progress',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class ProgressToken' },
                { source: 'class ProgressToken', target: 'partial class ProgressToken : TokenWithCommonCls' },
                { source: 'class Progress', target: 'partial class Progress' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Radio',
        src: ['components/radio/style/index.tsx'],
        dist: 'components/radio/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Radio',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class RadioToken' },
                { source: 'class RadioToken', target: 'partial class RadioToken : TokenWithCommonCls' },
                { source: 'class Radio', target: 'partial class Radio' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Rate',
        src: ['components/rate/style/index.tsx'],
        dist: 'components/rate/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Rate',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class RateToken' },
                { source: 'class RateToken', target: 'partial class RateToken : TokenWithCommonCls' },
                { source: 'class Rate', target: 'partial class Rate' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Result',
        src: ['components/result/style/index.tsx'],
        dist: 'components/result/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Result',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class ResultToken' },
                { source: 'class ResultToken', target: 'partial class ResultToken : TokenWithCommonCls' },
                { source: 'class Result', target: 'partial class Result' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Segmented',
        src: ['components/segmented/style/index.tsx'],
        dist: 'components/segmented/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Segmented',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SegmentedToken' },
                { source: 'class SegmentedToken', target: 'partial class SegmentedToken : TokenWithCommonCls' },
                { source: 'class Segmented', target: 'partial class Segmented' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Select',
        src: ['components/select/style/index.tsx'],
        dist: 'components/select/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Select',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SelectToken' },
                { source: 'class SelectToken', target: 'partial class SelectToken : TokenWithCommonCls' },
                { source: 'class Select', target: 'partial class Select' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Select',
        src: [
            'components/select/style/index.tsx',
            'components/select/style/dropdown.tsx',
            'components/select/style/multiple.tsx',
            'components/select/style/single.tsx',
        ],
        dist: 'components/select/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Select',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SelectToken' },
                { source: 'class SelectToken', target: 'partial class SelectToken : TokenWithCommonCls' },
                { source: 'class Select', target: 'partial class Select' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Skeleton',
        src: ['components/skeleton/style/index.tsx'],
        dist: 'components/skeleton/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Skeleton',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SkeletonToken' },
                { source: 'class SkeletonToken', target: 'partial class SkeletonToken : TokenWithCommonCls' },
                { source: 'class Skeleton', target: 'partial class Skeleton' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Slider',
        src: ['components/slider/style/index.tsx'],
        dist: 'components/slider/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Slider',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SliderToken' },
                { source: 'class SliderToken', target: 'partial class SliderToken : TokenWithCommonCls' },
                { source: 'class Slider', target: 'partial class Slider' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Space',
        src: [
            'components/space/style/index.tsx',
            'components/space/style/compact.tsx',
        ],
        dist: 'components/space/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Space',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SpaceToken' },
                { source: 'class SpaceToken', target: 'partial class SpaceToken : TokenWithCommonCls' },
                { source: 'class Space', target: 'partial class Space' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Spin',
        src: ['components/spin/style/index.tsx'],
        dist: 'components/spin/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Spin',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SpinToken' },
                { source: 'class SpinToken', target: 'partial class SpinToken : TokenWithCommonCls' },
                { source: 'class Spin', target: 'partial class Spin' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Statistic',
        src: ['components/statistic/style/index.tsx'],
        dist: 'components/statistic/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Statistic',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class StatisticToken' },
                { source: 'class StatisticToken', target: 'partial class StatisticToken : TokenWithCommonCls' },
                { source: 'class Statistic', target: 'partial class Statistic' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Steps',
        src: [
            'components/steps/style/index.tsx',
            'components/steps/style/custom-icon.ts',
            'components/steps/style/inline.ts',
            'components/steps/style/label-placement.ts',
            'components/steps/style/nav.ts',
            'components/steps/style/progress-dot.ts',
            'components/steps/style/progress.ts',
            'components/steps/style/rtl.ts',
            'components/steps/style/small.ts',
            'components/steps/style/vertical.ts',
        ],
        dist: 'components/steps/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Steps',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class StepsToken' },
                { source: 'class StepsToken', target: 'partial class StepsToken : TokenWithCommonCls' },
                { source: 'class Steps', target: 'partial class Steps' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Switch',
        src: ['components/switch/style/index.ts'],
        dist: 'components/switch/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Switch',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SwitchToken' },
                { source: 'class SwitchToken', target: 'partial class SwitchToken : TokenWithCommonCls' },
                { source: 'class Switch', target: 'partial class Switch' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Table',
        src: [
            'components/table/style/index.ts',
            'components/table/style/bordered.ts',
            'components/table/style/ellipsis.ts',
            'components/table/style/empty.ts',
            'components/table/style/expand.ts',
            'components/table/style/filter.ts',
            'components/table/style/fixed.ts',
            'components/table/style/pagination.ts',
            'components/table/style/radius.ts',
            'components/table/style/rtl.ts',
            'components/table/style/selection.ts',
            'components/table/style/size.ts',
            'components/table/style/sorter.ts',
            'components/table/style/sticky.ts',
            'components/table/style/summary.ts',
        ],
        dist: 'components/table/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Table',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TableToken' },
                { source: 'class TableToken', target: 'partial class TableToken : TokenWithCommonCls' },
                { source: 'class Table', target: 'partial class Table' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Tabs',
        src: [
            'components/tabs/style/index.ts',
            'components/tabs/style/motion.ts',
        ],
        dist: 'components/tabs/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Tabs',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TabsToken' },
                { source: 'class TabsToken', target: 'partial class TabsToken : TokenWithCommonCls' },
                { source: 'class Tabs', target: 'partial class Tabs' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Tag',
        src: ['components/tag/style/index.ts'],
        dist: 'components/tag/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Tag',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TagToken' },
                { source: 'class TagToken', target: 'partial class TagToken : TokenWithCommonCls' },
                { source: 'class Tag', target: 'partial class Tag' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Timeline',
        src: ['components/timeline/style/index.ts'],
        dist: 'components/timeline/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Timeline',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TimelineToken' },
                { source: 'class TimelineToken', target: 'partial class TimelineToken : TokenWithCommonCls' },
                { source: 'class Timeline', target: 'partial class Timeline' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Tooltip',
        src: ['components/tooltip/style/index.ts'],
        dist: 'components/tooltip/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Tooltip',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TooltipToken' },
                { source: 'class TooltipToken', target: 'partial class TooltipToken : TokenWithCommonCls' },
                { source: 'class Tooltip', target: 'partial class Tooltip' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Transfer',
        src: ['components/transfer/style/index.ts'],
        dist: 'components/transfer/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Transfer',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TransferToken' },
                { source: 'class TransferToken', target: 'partial class TransferToken : TokenWithCommonCls' },
                { source: 'class Transfer', target: 'partial class Transfer' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Tree',
        src: ['components/tree/style/index.ts'],
        dist: 'components/tree/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Tree',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TreeToken' },
                { source: 'class TreeToken', target: 'partial class TreeToken : TokenWithCommonCls' },
                { source: 'class Tree', target: 'partial class Tree' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'TreeSelect',
        src: ['components/tree-select/style/index.ts'],
        dist: 'components/tree-select/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'TreeSelect',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TreeSelectToken' },
                { source: 'class TreeSelectToken', target: 'partial class TreeSelectToken : TokenWithCommonCls' },
                { source: 'class TreeSelect', target: 'partial class TreeSelect' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Typography',
        src: [
            'components/typography/style/index.ts',
            'components/typography/style/mixins.ts',
        ],
        dist: 'components/typography/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Typography',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TypographyToken' },
                { source: 'class TypographyToken', target: 'partial class TypographyToken : TokenWithCommonCls' },
                { source: 'class Typography', target: 'partial class Typography' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Upload',
        src: [
            'components/upload/style/index.ts',
            'components/upload/style/dragger.ts',
            'components/upload/style/list.ts',
            'components/upload/style/motion.ts',
            'components/upload/style/picture.ts',
            'components/upload/style/rtl.ts',
        ],
        dist: 'components/upload/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Upload',
            typeMap: [
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class UploadToken' },
                { source: 'class UploadToken', target: 'partial class UploadToken : TokenWithCommonCls' },
                { source: 'class Upload', target: 'partial class Upload' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    }
];