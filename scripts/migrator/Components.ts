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
        'using CssInCSharp;',
        'using static AntDesign.GlobalStyle;',
        'using static AntDesign.Theme;',
        'using static AntDesign.StyleUtil;',
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

// 主题有关代码的生成参数
export const data0: Component[] = [
    {
        name: 'ColorNeutralMapToken',
        src: [
            'components/theme/interface/maps/colors.ts'
        ],
        dist: 'components/theme/interface/maps/Colors.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'GlobalToken',
            propertyMap: '_tokens',
            transforms: [
                { source: 'class ColorNeutralMapToken', target: 'partial class GlobalToken' },
                { source: 'class ColorPrimaryMapToken', target: 'partial class GlobalToken' },
                { source: 'class ColorSuccessMapToken', target: 'partial class GlobalToken' },
                { source: 'class ColorWarningMapToken', target: 'partial class GlobalToken' },
                { source: 'class ColorInfoMapToken', target: 'partial class GlobalToken' },
                { source: 'class ColorErrorMapToken', target: 'partial class GlobalToken' },
                { source: 'class ColorLinkMapToken', target: 'partial class GlobalToken' },
                { source: 'class ColorMapToken', target: 'partial class GlobalToken' },
            ]
        }
    },
    {
        name: 'FontMapToken',
        src: [
            'components/theme/interface/maps/font.ts'
        ],
        dist: 'components/theme/interface/maps/Font.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'GlobalToken',
            propertyMap: '_tokens',
            transforms: [
                { source: 'class FontMapToken', target: 'partial class GlobalToken' },
            ]
        }
    },
    {
        name: 'SizeMapToken',
        src: [
            'components/theme/interface/maps/size.ts'
        ],
        dist: 'components/theme/interface/maps/Size.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'GlobalToken',
            propertyMap: '_tokens',
            transforms: [
                { source: 'class SizeMapToken', target: 'partial class GlobalToken' },
                { source: 'class HeightMapToken', target: 'partial class GlobalToken' },
            ]
        }
    },
    {
        name: 'StyleMapToken',
        src: [
            'components/theme/interface/maps/style.ts'
        ],
        dist: 'components/theme/interface/maps/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'GlobalToken',
            propertyMap: '_tokens',
            transforms: [
                { source: 'class StyleMapToken', target: 'partial class GlobalToken' },
            ]
        }
    },
    {
        name: 'AliasToken',
        src: [
            'components/theme/interface/alias.ts'
        ],
        dist: 'components/theme/interface/Alias.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'GlobalToken',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'TextDecoration<string | number>', to: 'string' },
            ],
            transforms: [
                { source: 'class AliasToken', target: 'partial class GlobalToken' },
            ]
        }
    },
    {
        name: 'SeedToken',
        src: [
            'components/theme/interface/seeds.ts'
        ],
        dist: 'components/theme/interface/Seeds.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'GlobalToken',
            propertyMap: '_tokens',
            transforms: [
                { source: 'class SeedToken', target: 'partial class GlobalToken' },
            ]
        }
    }
]

// 未完成测试的组件
export const data1: Component[] = [
    
    
    {
        name: 'Calendar',
        src: ['components/calendar/style/index.ts'],
        dist: 'components/calendar/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Calendar',
            typeMap: [
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [1, 4] },
                { from: 'Unknown2', to: 'GlobalToken', includes: [2] },
                { from: 'Unknown2', to: 'CalendarToken', includes: [3] },
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
                { from: 'Unknown1', to: 'CardToken', includes: [1] },
                { from: 'Unknown2', to: 'CardToken', includes: [1] },
                { from: 'Unknown3', to: 'CardToken', includes: [1] },
                { from: 'Unknown4', to: 'CardToken', includes: [1] },
                { from: 'Unknown5', to: 'CardToken', includes: [1] },
                { from: 'Unknown6', to: 'CardToken', includes: [1] },
                { from: 'Unknown7', to: 'CardToken', includes: [1] },
                { from: 'Unknown8', to: 'CardToken', includes: [1] },
                { from: 'Unknown9', to: 'CSSInterpolation[]', includes: [1, 4] },
                { from: 'Unknown9', to: 'GlobalToken', includes: [2] },
                { from: 'Unknown9', to: 'CardToken', includes: [3] },
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
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 41]] },
                { from: 'Unknown1', to: 'CarouselToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 10]] },
                { from: 'Unknown2', to: 'CarouselToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject[]', includes: [1, 3] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[4, 11]] },
                { from: 'Unknown3', to: 'CarouselToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSInterpolation[]', includes: [1, 4] },
                { from: 'Unknown4', to: 'GlobalToken', includes: [2] },
                { from: 'Unknown4', to: 'CarouselToken', includes: [3] },
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
                { from: 'Unknown1', to: 'CSSObject', ranges: [[4, 30]] },
                { from: 'Unknown1', to: 'CSSObject[]', includes: [1, 3, 7] },
                { from: 'Unknown1', to: 'CascaderToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [1, 3] },
                { from: 'Unknown2', to: 'GlobalToken', includes: [2] }
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
                { from: 'Unknown2', to: 'CSSObject[]', includes: [1, 3] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[4, 46]] },
                { from: 'Unknown2', to: 'CheckboxToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject[]', includes: [1, 3] },
                { from: 'Unknown3', to: 'CheckboxToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSInterpolation[]', includes: [1, 4] },
                { from: 'Unknown4', to: 'GlobalToken', includes: [2] },
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
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 41]] },
                { from: 'Unknown1', to: 'CollapseToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 5]] },
                { from: 'Unknown2', to: 'CollapseToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 9]] },
                { from: 'Unknown3', to: 'CollapseToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 7]] },
                { from: 'Unknown4', to: 'CollapseToken', includes: [2] },
                { from: 'Unknown5', to: 'CSSInterpolation[]', includes: [1, 4] },
                { from: 'Unknown5', to: 'GlobalToken', includes: [2] },
                { from: 'Unknown5', to: 'CollapseToken', includes: [3] },
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
        name: 'Drawer',
        src: [
            'components/drawer/style/index.ts',
            'components/drawer/style/motion.ts',
        ],
        dist: 'components/drawer/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Drawer',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 29]] },
                { from: 'Unknown1', to: 'PropertySkip', includes: [14, 16] },
                { from: 'Unknown2', to: 'GlobalToken', includes: [2] },
                { from: 'Unknown2', to: 'DrawerToken', includes: [3] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [1, 4] },
                { from: 'Unknown3', to: 'GlobalToken', includes: [2] },
                { from: 'Unknown3', to: 'DrawerToken', includes: [1, 3] },
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 43]] },
                { from: 'Unknown4', to: 'CSSObject[]', includes: [16, 23, 30, 37] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class DrawerToken' },
                { source: 'class DrawerToken', target: 'partial class DrawerToken : TokenWithCommonCls' },
                { source: 'class Drawer', target: 'partial class Drawer' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
                { source: 'SharedPanelMotion', target: 'sharedPanelMotion' }
            ]
        }
    },
    {
        name: 'DatePicker',
        src: ['components/date-picker/style/index.ts'],
        dist: 'components/date-picker/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat([
                'using static AntDesign.Slide;',
                'using static AntDesign.Move;',
            ]),
            defaultClass: 'DatePicker',
            typeMap: [
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 12]] },
                { from: 'Unknown4', to: 'DatePickerToken', includes: [2] },
                { from: 'Unknown5', to: 'CSSObject', ranges: [[1, 80]] },
                { from: 'Unknown5', to: 'CSSObject[]', includes: [1, 3] },
                { from: 'Unknown5', to: 'DatePickerToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class DatePickerToken : TokenWithCommonCls' },
                { source: 'class PickerPanelToken', target: 'class PickerPanelToken : InputToken' },
                { source: 'class PickerToken', target: 'class PickerToken : PickerPanelToken' },
                { source: 'class SharedPickerToken', target: 'class SharedPickerToken : PickerToken' },
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
                { from: 'Unknown', to: 'DividerToken', includes: [2, 4] },
                { from: 'Unknown', to: 'CSSInterpolation[]', includes: [1, 5] },
                { from: 'Unknown', to: 'GlobalToken', includes: [3] },
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

// 已经可以生成代码的组件
export const data2: Component[] = [
    {
        name: 'Affix',
        src: ['components/affix/style/index.ts'],
        dist: 'components/affix/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Affix',
            typeMap: [
                { from: 'Unknown1', to: 'AffixToken', includes: [1] },
                { from: 'Unknown2', to: 'AffixToken', includes: [1] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class AffixToken', target: 'class AffixToken : TokenWithCommonCls' },
                { source: 'class Affix', target: 'partial class Affix' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
                { from: 'Padding<string | number>', to: 'string' },
                { from: 'Unknown6', to: 'CSSInterpolation', includes: [1] },
                { from: 'Unknown6', to: 'AlertToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class AlertToken : TokenWithCommonCls' },
                { source: 'class AlertToken', target: 'partial class AlertToken' },
                { source: 'class Alert', target: 'partial class Alert' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
                { source: 'new CSSInterpolation', target: 'new CSSInterpolation[]' }
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
                { from: 'Unknown1', to: 'AnchorToken', includes: [1] },
                { from: 'Unknown2', to: 'AnchorToken', includes: [1] },
                { from: 'Unknown3', to: 'AnchorToken', includes: [1] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [2] },
                { from: 'Unknown3', to: 'AnchorToken', includes: [3] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class AnchorToken : TokenWithCommonCls' },
                { source: 'class AnchorToken', target: 'partial class AnchorToken' },
                { source: 'class Anchor', target: 'partial class Anchor' },
                { source: 'textEllipsis', target: 'TextEllipsis' }, // 无法推导这个是局部变量还是全局变量，手动映射
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' }
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
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 9]] },
                { from: 'Unknown1', to: 'AvatarToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 8]] },
                { from: 'Unknown2', to: 'AvatarToken', includes: [2] },
                { from: 'Unknown3', to: 'AvatarToken', includes: [1] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [2] },
                { from: 'Unknown3', to: 'AvatarToken', includes: [3] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class AvatarToken : TokenWithCommonCls' },
                { source: 'class AvatarToken', target: 'partial class AvatarToken' },
                { source: 'class Avatar', target: 'partial class Avatar' },
                { source: 'AvatarSizeStyle', target: 'avatarSizeStyle' },
                { source: 'Math.Round(', target: '(int)Math.Round((double)' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' }
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
                { from: 'Unknown1', to: 'BackTopToken', includes: [1] },
                { from: 'Unknown2', to: 'BackTopToken', includes: [1] },
                { from: 'Unknown3', to: 'BackTopToken', includes: [1] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [2] },
                { from: 'Unknown3', to: 'BackTopToken', includes: [3] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class BackTopToken : TokenWithCommonCls' },
                { source: 'class BackTopToken', target: 'partial class BackTopToken' },
                { source: 'class BackTop', target: 'partial class BackTop' },
                { source: 'controlHeightLG * 1.25', target: '(int)(controlHeightLG * 1.25)' },
                { source: 'controlHeightLG * 2.5', target: '(int)(controlHeightLG * 2.5)' },
                { source: 'controlHeightLG * 1.5', target: '(int)(controlHeightLG * 1.5)' },
                { source: 'controlHeightLG * 0.5', target: '(int)(controlHeightLG * 0.5)' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' }
            ]
        }
    },
    {
        name: 'Badge',
        src: ['components/badge/style/index.ts'],
        dist: 'components/badge/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat(['using Keyframes = CssInCSharp.Keyframe;']),
            defaultClass: 'Badge',
            typeMap: [
                { from: 'Keyframes', to: 'CSSObject' },
                { from: 'Unknown7', to: 'CSSObject', includes: [1, 2, 3] },
                { from: 'Unknown8', to: 'BadgeToken', includes: [1, 2, 3] },
                { from: 'Unknown9', to: 'BadgeToken', includes: [1, 2] },
                { from: 'Unknown10', to: 'CSSInterpolation[]', includes: [1] },
            ],
            transforms: [
                { source: 'antStatusProcessing', target: '_antStatusProcessing' },
                { source: 'antZoomBadgeIn', target: '_antZoomBadgeIn' },
                { source: 'antZoomBadgeOut', target: '_antZoomBadgeOut' },
                { source: 'antNoWrapperZoomBadgeIn', target: '_antNoWrapperZoomBadgeIn' },
                { source: 'antNoWrapperZoomBadgeOut', target: '_antNoWrapperZoomBadgeOut' },
                { source: 'antBadgeLoadingCircle', target: '_antBadgeLoadingCircle' },
                { source: 'Bdi', target: '["bdi"]' },
                { source: 'class ComponentToken', target: 'partial class BadgeToken' },
                { source: 'class BadgeToken', target: 'partial class BadgeToken : TokenWithCommonCls' },
                { source: 'class Badge', target: 'partial class Badge' },
                { source: 'Math.Round(fontSize * lineHeight)', target: '(int)Math.Round((double)fontSize * lineHeight)' },
                { source: 'prepareComponentToken', target: 'PrepareComponentToken' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' }
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
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 19]] },
                { from: 'Unknown1', to: 'BreadcrumbToken', includes: [2] },
                { from: 'Unknown2', to: 'BreadcrumbToken', includes: [1, 3] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class BreadcrumbToken' },
                { source: 'class BreadcrumbToken', target: 'partial class BreadcrumbToken : TokenWithCommonCls' },
                { source: 'class Breadcrumb', target: 'partial class Breadcrumb' },
                { source: 'token.LineHeight * token.FontSize', target: '(int)(token.LineHeight * token.FontSize)' },
                { source: 'var BreadcrumbToken', target: 'var breadcrumbToken' },
                { source: '(BreadcrumbToken)', target: '(breadcrumbToken)' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
];

// 用于生成的实例，将需要生成的组件配置放到这里
export const components: Component[] = [
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
                { from: 'FontWeight', to: 'int' },
                { from: 'PaddingInline<string | number>', to: 'int' },
                { from: 'Unknown1', to: 'ButtonToken', includes: [1] },
                { from: 'Unknown3', to: 'CSSObject', includes: [1, 3] },
                { from: 'Unknown3', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSObject', includes: [1, 3] },
                { from: 'Unknown4', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown5', to: 'CSSObject', includes: [1, 3] },
                { from: 'Unknown5', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown6', to: 'CSSObject', includes: [1, 2] },
                { from: 'Unknown7', to: 'CSSObject', includes: [1, 3, 4] },
                { from: 'Unknown7', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown8', to: 'CSSObject', includes: [1, 3] },
                { from: 'Unknown8', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown9', to: 'CSSObject', includes: [1, 3, 4] },
                { from: 'Unknown9', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown10', to: 'CSSObject', ranges: [[1, 8]] },
                { from: 'Unknown10', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown11', to: 'CSSObject', ranges: [[1, 12]] },
                { from: 'Unknown11', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown12', to: 'CSSObject', includes: [1, 3] },
                { from: 'Unknown12', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown13', to: 'CSSObject', ranges: [[1, 8]] },
                { from: 'Unknown13', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown14', to: 'CSSObject', ranges: [[1, 8]] },
                { from: 'Unknown14', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown15', to: 'CSSObject', ranges: [[1, 4]] },
                { from: 'Unknown15', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown16', to: 'CSSObject', ranges: [[1, 9]] },
                { from: 'CSSInterpolation', to: 'CSSInterpolation[]' },
                { from: 'Unknown17', to: 'CSSInterpolation', includes: [1] },
                { from: 'Unknown17', to: 'ButtonToken', includes: [2, 3] },
                { from: 'Unknown18', to: 'CSSInterpolation', includes: [1] },
                { from: 'Unknown18', to: 'ButtonToken', includes: [2, 3] },
                { from: 'Unknown19', to: 'CSSInterpolation', includes: [1] },
                { from: 'Unknown19', to: 'ButtonToken', includes: [2, 3] },
                { from: 'Unknown20', to: 'CSSObject', includes: [1, 3, 4, 5] },
                { from: 'Unknown20', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown21', to: 'ButtonToken', includes: [1, 2, 3] },
                { from: 'Unknown22', to: 'ButtonToken', includes: [1, 2] },
                { from: 'Unknown23', to: 'CSSInterpolation[]', includes: [1] },
                { from: 'Unknown24', to: 'CSSObject', ranges: [[1, 9]] },
                { from: 'Unknown25', to: 'CSSObject', ranges: [[1, 14]] },
                { from: 'Unknown25', to: 'ButtonToken', includes: [2] },
                { from: 'Unknown25', to: 'CSSInterpolation[]', includes: [4] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class ButtonToken' },
                { source: 'class ButtonToken', target: 'partial class ButtonToken : TokenWithCommonCls' },
                { source: 'class Button', target: 'partial class Button' },
                { source: 'CSSObject hoverStyle, CSSObject activeStyle', target: 'CSSObject hoverStyle = default, CSSObject activeStyle = default' },
                { source: 'prepareComponentToken', target: 'PrepareComponentToken' },
                // 实际没有该属性，先注释掉
                { source: 'DefaultBorderColorDisabled = token.ColorBorder,', target: '// DefaultBorderColorDisabled = token.ColorBorder,' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
]