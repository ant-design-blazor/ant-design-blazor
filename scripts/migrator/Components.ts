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

// 样式Token组件
const token: Component[] = [
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

// 已完成迁移
const completed: Component[] = [
    {
        name: 'Affix',
        src: ['components/affix/style/index.ts'],
        dist: 'components/affix/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Affix',
            propertyMap: '_tokens',
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
            propertyMap: '_tokens',
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
            propertyMap: '_tokens',
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
            propertyMap: '_tokens',
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
            propertyMap: '_tokens',
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
            propertyMap: '_tokens',
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
                { source: 'class ComponentToken', target: 'partial class BadgeToken' },
                { source: 'class BadgeToken', target: 'partial class BadgeToken : TokenWithCommonCls' },
                { source: 'class Badge', target: 'partial class Badge' },
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
            propertyMap: '_tokens',
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
                { source: 'var BreadcrumbToken', target: 'var breadcrumbToken' },
                { source: '(BreadcrumbToken)', target: '(breadcrumbToken)' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'FontWeight', to: 'double' },
                { from: 'PaddingInline<string | number>', to: 'double' },
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
    {
        name: 'Calendar',
        src: ['components/calendar/style/index.ts'],
        dist: 'components/calendar/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Calendar',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown2', to: 'CalendarToken', includes: [1, 3] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CalendarToken : TokenWithCommonCls' },
                { source: 'class CalendarToken', target: 'partial class CalendarToken' },
                { source: 'class Calendar', target: 'partial class Calendar' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
                // todo: 临时注释不可用代码
                { source: 'InitPickerPanelToken(token),', target: '// InitPickerPanelToken(token),' },
                { source: 'InitPanelComponentToken(token),', target: '// InitPanelComponentToken(token),' },
                { source: 'PickerCellInnerCls = @$"{token.ComponentCls}-cell-inner",', target: '// PickerCellInnerCls = @$"{token.ComponentCls}-cell-inner",' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CardToken', includes: [1] },
                { from: 'Unknown2', to: 'CardToken', includes: [1] },
                { from: 'Unknown3', to: 'CardToken', includes: [1] },
                { from: 'Unknown4', to: 'CardToken', includes: [1] },
                { from: 'Unknown5', to: 'CardToken', includes: [1] },
                { from: 'Unknown6', to: 'CardToken', includes: [1] },
                { from: 'Unknown7', to: 'CardToken', includes: [1] },
                { from: 'Unknown8', to: 'CardToken', includes: [1] },
                { from: 'Unknown9', to: 'CardToken', includes: [1, 3] },
                { from: 'Unknown9', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CardToken : TokenWithCommonCls' },
                { source: 'class CardToken', target: 'partial class CardToken' },
                { source: 'class Card', target: 'partial class Card' },
                { source: 'textEllipsis', target: 'TextEllipsis' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 41]] },
                { from: 'Unknown1', to: 'CarouselToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 10]] },
                { from: 'Unknown2', to: 'CarouselToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [1, 3] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[4, 11]] },
                { from: 'Unknown3', to: 'CarouselToken', includes: [2] },
                { from: 'Unknown4', to: 'CarouselToken', includes: [1, 3] },
                { from: 'Unknown4', to: 'CSSInterpolation[]', includes: [2] },
                { from: 'Unknown4', to: 'GenOptions', includes: [4] },
                { from: 'Unknown4', to: '()', includes: [5] },

            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CarouselToken : TokenWithCommonCls' },
                { source: 'class CarouselToken', target: 'partial class CarouselToken' },
                { source: 'class Carousel', target: 'partial class Carousel' },
                { source: '() =>', target: '(token) =>' },
                { source: '["dotWidthActive", "dotActiveWidth"]', target: 'new ("dotWidthActive", "dotActiveWidth")' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'FontWeight', to: 'double' },
                { from: 'Padding<string | number>', to: 'string' },
                { from: 'Unknown1', to: 'CSSInterpolation[]', includes: [1, 3, 7] },
                { from: 'Unknown1', to: 'CSSObject', ranges: [[4, 11]] },
                { from: 'Unknown1', to: 'CascaderToken', includes: [2] },
                { from: 'Unknown1', to: 'CSSInterpolation[]', includes: [7] },
                { from: 'Unknown2', to: 'CascaderToken', includes: [1, 2] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [1] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CascaderToken : TokenWithCommonCls' },
                { source: 'class CascaderToken', target: 'partial class CascaderToken' },
                { source: 'class Cascader', target: 'partial class Cascader' },
                { source: 'prepareComponentToken', target: 'PrepareComponentToken' },
                { source: 'string MenuPadding', target: 'double MenuPadding' },
                { source: '(string)_tokens["menuPadding"]', target: '(double)_tokens["menuPadding"]' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
                // todo: 临时注释不可用代码
                { source: 'GetColumnsStyle(token),', target: '// GetColumnsStyle(token),' },
            ]
        }
    },
    {
        name: 'Checkbox',
        src: ['components/checkbox/style/index.ts'],
        dist: 'components/checkbox/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Checkbox',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSInterpolation[]', includes: [1, 3] },
                { from: 'Unknown1', to: 'CSSObject', ranges: [[4, 43]] },
                { from: 'Unknown1', to: 'CheckboxToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [1, 3] },
                { from: 'Unknown2', to: 'CheckboxToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [1] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CheckboxToken : TokenWithCommonCls' },
                { source: 'class CheckboxToken', target: 'partial class CheckboxToken' },
                { source: 'class Checkbox', target: 'partial class Checkbox' },
                { source: '(token, args)', target: '(token)' },
                { source: 'args.PrefixCls', target: 'token.PrefixCls' },
                { source: `FullToken<'Checkbox'>`, target: 'TokenWithCommonCls' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Padding<string | number>', to: 'string' },
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 38]] },
                { from: 'Unknown1', to: 'CollapseToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 5]] },
                { from: 'Unknown2', to: 'CollapseToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 9]] },
                { from: 'Unknown3', to: 'CollapseToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 7]] },
                { from: 'Unknown4', to: 'CollapseToken', includes: [2] },
                { from: 'Unknown5', to: 'CollapseToken', includes: [1, 3] },
                { from: 'Unknown5', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class CollapseToken : TokenWithCommonCls' },
                { source: 'class CollapseToken', target: 'partial class CollapseToken' },
                { source: 'class Collapse', target: 'partial class Collapse' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
                // todo: 临时注释不可用代码
                { source: 'GenCollapseMotion(collapseToken),', target: '// GenCollapseMotion(collapseToken),' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 14]] },
                { from: 'Unknown4', to: 'DatePickerToken', includes: [2] },
                { from: 'Unknown5', to: 'CSSObject', ranges: [[1, 80]] },
                { from: 'Unknown5', to: 'CSSInterpolation[]', includes: [1, 3] },
                { from: 'Unknown5', to: 'DatePickerToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class DatePickerToken : TokenWithCommonCls' },
                { source: 'class PickerPanelToken', target: 'class PickerPanelToken : InputToken' },
                { source: 'class PickerToken', target: 'class PickerToken : PickerPanelToken' },
                { source: 'class SharedPickerToken', target: 'class SharedPickerToken : PickerToken' },
                { source: 'class DatePickerToken', target: 'partial class DatePickerToken' },
                { source: 'class DatePicker', target: 'partial class DatePicker' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 29]] },
                { from: 'Unknown2', to: 'DescriptionsToken', includes: [2] },
                { from: 'Unknown3', to: 'DescriptionsToken', includes: [1, 3] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class DescriptionsToken' },
                { source: 'class DescriptionsToken', target: 'partial class DescriptionsToken : TokenWithCommonCls' },
                { source: 'class Descriptions', target: 'partial class Descriptions' },
                { source: 'textEllipsis', target: 'TextEllipsis' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'MarginInline<string | number>', to: 'double' },
                { from: 'PaddingInline<string | number>', to: 'string' },
                { from: 'Unknown1', to: 'DividerToken', includes: [1] },
                { from: 'Unknown2', to: 'DividerToken', includes: [1, 3] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class DividerToken' },
                { source: 'class DividerToken', target: 'partial class DividerToken : TokenWithCommonCls' },
                { source: 'class Divider', target: 'partial class Divider' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 29]] },
                { from: 'Unknown1', to: 'PropertySkip', includes: [14, 16] },
                { from: 'Unknown2', to: 'DrawerToken', includes: [1, 3] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 43]] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [16, 23, 30, 37] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class DrawerToken' },
                { source: 'class DrawerToken', target: 'partial class DrawerToken : TokenWithCommonCls' },
                { source: 'class Drawer', target: 'partial class Drawer' },
                { source: 'SharedPanelMotion', target: 'sharedPanelMotion' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSInterpolation[]', includes: [1, 3] },
                { from: 'Unknown1', to: 'CSSObject', ranges: [[4, 50]] },
                { from: 'Unknown1', to: 'DropdownToken', includes: [2] },
                { from: 'Unknown1', to: 'PropertySkip', includes: [6] },
                { from: 'Unknown1', to: 'TokenWithCommonCls', includes: [20] },
                { from: 'Unknown2', to: 'arrowToken', includes: [1, 3] },
                { from: 'Unknown2', to: 'TokenWithCommonCls', includes: [2] },
                { from: 'Unknown2', to: 'DropdownToken', includes: [4, 6] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [5] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 7]] },
                { from: 'Unknown3', to: 'DropdownToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class DropdownToken' },
                { source: 'class DropdownToken', target: 'partial class DropdownToken : TokenWithCommonCls' },
                { source: 'class Dropdown', target: 'partial class Dropdown' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'EmptyToken', includes: [1] },
                { from: 'Unknown2', to: 'EmptyToken', includes: [1] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class EmptyToken' },
                { source: 'class EmptyToken', target: 'partial class EmptyToken : TokenWithCommonCls' },
                { source: 'class Empty', target: 'partial class Empty' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
    {
        name: 'Form',
        src: [
            'components/form/style/index.ts',
            'components/form/style/explain.ts',
            'components/form/style/fallbackCmp.ts',
        ],
        dist: 'components/form/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Form',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Padding<string | number>', to: 'string' },
                { from: 'Margin<string | number>', to: 'string' },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 7]] },
                { from: 'Unknown3', to: 'FormToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 39]] },
                { from: 'Unknown4', to: 'FormToken', includes: [2] },
                { from: 'Unknown5', to: 'CSSObject', ranges: [[1, 8]] },
                { from: 'Unknown5', to: 'FormToken', includes: [2] },
                { from: 'Unknown6', to: 'CSSObject', ranges: [[1, 10]] },
                { from: 'Unknown6', to: 'FormToken', includes: [2] },
                { from: 'Unknown9', to: 'CSSObject', ranges: [[1, 17]] },
                { from: 'Unknown9', to: 'CSSInterpolation[]', includes: [9] },
                { from: 'Unknown9', to: 'FormToken', includes: [2] },
                { from: 'Unknown12', to: 'CSSObject', ranges: [[1, 12]] },
                { from: 'Unknown12', to: 'FormToken', includes: [2] },
                { from: 'Unknown13', to: 'CSSObject', ranges: [[1, 5]] },
                { from: 'Unknown13', to: 'FormToken', includes: [2] },
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
            defaultClass: 'GridStyle',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'GridRowToken', includes: [1] },
                { from: 'Unknown2', to: 'GridColToken', includes: [1] },
                { from: 'Unknown3', to: 'CSSObject', includes: [1] },
            ],
            transforms: [
                { source: 'class GridRowToken', target: 'class GridRowToken : TokenWithCommonCls' },
                { source: 'class GridColToken', target: 'class GridColToken : TokenWithCommonCls' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown5', to: 'CSSInterpolation[]', includes: [1, 2] },
                { from: 'Unknown5', to: 'CSSObject', ranges: [[3, 19]] },
                { from: 'Unknown6', to: 'CSSObject', ranges: [[1, 8]] },
                { from: 'Unknown7', to: 'CSSObject', includes: [1, 3] },
                { from: 'Unknown7', to: 'ImageToken', includes: [2] },
                { from: 'Unknown8', to: 'ImageToken', includes: [1, 3, 4] },
                { from: 'Unknown8', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class ImageToken' },
                { source: 'class ImageToken', target: 'partial class ImageToken : TokenWithCommonCls' },
                { source: 'class Image', target: 'partial class Image' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown3', to: 'CSSObject', includes: [1, 2] },
                { from: 'Unknown4', to: 'InputToken', includes: [1] },
                { from: 'Unknown7', to: 'InputToken', includes: [1, 2] },
                { from: 'Unknown10', to: 'CSSObject', ranges: [[1, 7]] },
                { from: 'Unknown12', to: 'CSSObject', ranges: [[1, 21]] },
                { from: 'Unknown13', to: 'CSSObject', ranges: [[1, 22]] },
                { from: 'Unknown14', to: 'CSSObject', ranges: [[1, 29]] },
                { from: 'Unknown15', to: 'CSSObject', ranges: [[1, 18]] },
                { from: 'Unknown15', to: 'InputToken', includes: [2] },
                { from: 'Unknown16', to: 'CSSObject', ranges: [[1, 5]] },
                { from: 'Unknown16', to: 'InputToken', includes: [2] },
                { from: 'Unknown17', to: 'InputToken', includes: [1] },
                { from: 'Unknown19', to: 'CSSInterpolation[]', includes: [1] },
            ],
            transforms: [
                { source: 'class SharedComponentToken', target: 'partial class SharedComponentToken : TokenWithCommonCls' },
                { source: 'class SharedInputToken', target: 'partial class InputToken' },
                { source: 'class ComponentToken', target: 'partial class InputToken' },
                { source: 'class InputToken', target: 'partial class InputToken : SharedComponentToken' },
                { source: 'class Input', target: 'partial class Input<TValue>' },
                { source: 'initComponentToken', target: 'InitComponentToken' },
                { source: '"col-"', target: '\\"col-\\"' },
                { source: `'"\\\\a0"'`, target: `"'\\\\\\\\a0'"`, },
                { source: 'FIXED_CHROME_COLOR_HEIGHT', target: 'fixedChromeColorHeight' },
                { source: 'SharedInputToken', target: 'InputToken' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'true | "auto"', to: 'bool' },
                { from: `'lg' | 'sm'`, to: 'string' },
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 7]] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [1, 2] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[3, 46]] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 17]] },
                { from: 'Unknown4', to: 'CSSInterpolation[]', includes: [1] },
                { from: 'Unknown4', to: 'CSSObject', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class InputNumberToken' },
                { source: 'class InputNumberToken', target: 'partial class InputNumberToken : TokenWithCommonCls' },
                { source: 'class InputNumber', target: 'partial class InputNumber' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Padding<string | number>', to: 'string' },
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 25]] },
                { from: 'Unknown1', to: 'LayoutToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [1] },
                { from: 'Unknown2', to: 'LayoutToken', includes: [2] },
                { from: 'Unknown2', to: 'GenOptions', includes: [3] },
                { from: 'Unknown2', to: '()', includes: [4] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 6]] },
                { from: 'Unknown3', to: 'LayoutToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class LayoutToken' },
                { source: 'class LayoutToken', target: 'partial class LayoutToken : TokenWithCommonCls' },
                { source: 'class Layout', target: 'partial class Layout' },
                { source: '["colorBgBody", "bodyBg"],', target: 'new ("colorBgBody", "bodyBg"),' },
                { source: '["colorBgHeader", "headerBg"],', target: 'new ("colorBgHeader", "headerBg"),' },
                { source: '["colorBgTrigger", "triggerBg"]', target: 'new ("colorBgTrigger", "triggerBg")' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Padding<string | number>', to: 'string' },
                { from: 'MarginBottom<string | number>', to: 'string' },
                { from: 'MarginRight<string | number>', to: 'string' },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 48]] },
                { from: 'Unknown3', to: 'ListToken', includes: [2] },
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
            usings: defaultOptions.usings.concat(['using static AntDesign.InputStyle;']),
            defaultClass: 'Mentions',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 20]] },
                { from: 'Unknown1', to: 'MentionsToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [1] },
                { from: 'Unknown2', to: 'MentionsToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class MentionsToken' },
                { source: 'class MentionsToken', target: 'partial class MentionsToken : InputToken' },
                { source: 'class Mentions', target: 'partial class Mentions' },
                { source: 'textEllipsis', target: 'TextEllipsis' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'LineHeight<string | number>', to: 'double' },
                { from: 'MarginBlock<string | number>', to: 'double' },
                { from: 'PaddingInline<string | number>', to: 'double' },
                { from: 'MarginInlineEnd<string | number>', to: 'double' },
                { from: 'Unknown1', to: 'string[]', includes: [1, 2, 3] },
                { from: 'Unknown2', to: 'string[]', includes: [1] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 55]] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [1, 3] },
                { from: 'Unknown3', to: 'MenuToken', includes: [2] },
                { from: 'Unknown3', to: 'string[]', includes: [17, 19, 21] },
                { from: 'Unknown4', to: 'MenuToken', includes: [1, 2, 4] },
                { from: 'Unknown4', to: 'CSSInterpolation[]', includes: [3] },
                { from: 'Unknown4', to: '()', includes: [5] },
                { from: 'Unknown5', to: 'CSSObject', ranges: [[1, 10]] },
                { from: 'Unknown5', to: 'MenuToken', includes: [2] },
                { from: 'Unknown5', to: 'string[]', includes: [9] },
                { from: 'Unknown6', to: 'CSSObject', ranges: [[1, 7]] },
                { from: 'Unknown6', to: 'MenuToken', includes: [2] },
                { from: 'Unknown7', to: 'CSSObject', ranges: [[3, 36]] },
                { from: 'Unknown7', to: 'CSSInterpolation[]', includes: [1, 4] },
                { from: 'Unknown7', to: 'MenuToken', includes: [2] },
                { from: 'Unknown7', to: 'string[]', includes: [18] },
                { from: 'Unknown8', to: 'CSSObject', ranges: [[1, 9]] },
                { from: 'Unknown8', to: 'MenuToken', includes: [2] },
                { from: 'Unknown9', to: 'CSSObject', includes: [1, 2] },
                { from: 'CSSInterpolation', to: 'CSSObject' },
                { from: 'Unknown10', to: 'CSSObject', ranges: [[1, 4]] },
                { from: 'Unknown10', to: 'string[]', includes: [5, 6] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class MenuToken' },
                { source: 'class MenuToken', target: 'partial class MenuToken : TokenWithCommonCls' },
                { source: 'class Menu', target: 'partial class Menu' },
                { source: '"-active"', target: `'-active'` },
                { source: 'textEllipsis', target: 'TextEllipsis' },
                { source: 'double HorizontalLineHeight', target: 'string HorizontalLineHeight' },
                { source: '(double)_tokens["horizontalLineHeight"]', target: '(string)_tokens["horizontalLineHeight"]' },
                { source: 'UseComponentStyleResult()', target: 'GenOptions()' },
                { source: 'UseOriginHook(prefixCls)', target: 'useOriginHook' },
                { source: 'activeBarBorderWidth && activeBarWidth', target: 'activeBarBorderWidth != 0 && activeBarWidth != 0' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
    {
        name: 'Message',
        src: ['components/message/style/index.tsx'],
        dist: 'components/message/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat(['using Keyframes = CssInCSharp.Keyframe;']),
            defaultClass: 'Message',
            typeMap: [
                { from: 'Padding<string | number>', to: 'string' },
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 30]] },
                { from: 'Unknown1', to: 'MessageToken', includes: [2] },
                { from: 'Unknown1', to: 'CSSInterpolation[]', includes: [1, 16] },
                { from: 'Unknown2', to: 'MessageToken', includes: [1, 3] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class MessageToken' },
                { source: 'class MessageToken', target: 'partial class MessageToken : TokenWithCommonCls' },
                { source: 'class Message', target: 'partial class Message' },
                { source: 'CONTAINER_MAX_OFFSET', target: '1000' },
                { source: 'public CSSInterpolation[] GenComponentStyleHook', target: 'protected override CSSInterpolation[] UseStyle' },
            ]
        }
    },
    {
        name: 'Modal',
        src: ['components/modal/style/index.tsx'],
        dist: 'components/modal/style/Index.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Modal',
            propertyMap: '_tokens',
            usings: defaultOptions.usings.concat([
                'using static AntDesign.Fade;',
            ]),
            typeMap: [
                { from: 'React.CSSProperties', to: 'CSSObject' },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 12]] },
                { from: 'Unknown2', to: 'ModalToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSInterpolation', includes: [1] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [3] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 30]] },
                { from: 'Unknown3', to: 'ModalToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSInterpolation', includes: [1] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [3] },
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 12]] },
                { from: 'Unknown4', to: 'ModalToken', includes: [2] },
                { from: 'Unknown5', to: 'CSSObject', ranges: [[1, 6]] },
                { from: 'Unknown5', to: 'ModalToken', includes: [2] },
                { from: 'Unknown6', to: 'ModalToken', includes: [1, 2, 3] },
                { from: 'Unknown7', to: 'ModalToken', includes: [1, 2] },
            ],
            transforms: [
                { source: 'React.CSSProperties', target: 'CSSObject' },
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
            'components/notification/style/index.ts',
            'components/notification/style/placement.ts',
            'components/notification/style/pure-panel.ts',
            'components/notification/style/stack.ts',
        ],
        dist: 'components/notification/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Notification',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 20]] },
                { from: 'Unknown2', to: 'NotificationToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSInterpolation', includes: [1] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [6] },
                { from: 'Unknown6', to: 'CSSObject', ranges: [[1, 29]] },
                { from: 'Unknown6', to: 'NotificationToken', includes: [2] },
                { from: 'Unknown11', to: 'CSSObject', ranges: [[1, 14]] },
                { from: 'Unknown11', to: 'NotificationToken', includes: [2] },
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
        src: ['components/pagination/style/index.ts'],
        dist: 'components/pagination/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat(['using static AntDesign.InputStyle;']),
            defaultClass: 'Pagination',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 27]] },
                { from: 'Unknown1', to: 'PaginationToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 28]] },
                { from: 'Unknown2', to: 'PaginationToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 13]] },
                { from: 'Unknown3', to: 'PaginationToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 25]] },
                { from: 'Unknown4', to: 'PaginationToken', includes: [2] },
                { from: 'Unknown5', to: 'CSSObject', ranges: [[1, 13]] },
                { from: 'Unknown5', to: 'PaginationToken', includes: [2] },
                { from: 'Unknown6', to: 'CSSObject', ranges: [[1, 13]] },
                { from: 'Unknown6', to: 'PaginationToken', includes: [2] },
                { from: 'Unknown7', to: 'CSSObject', ranges: [[1, 26]] },
                { from: 'Unknown7', to: 'PaginationToken', includes: [2] },
                { from: 'Unknown8', to: 'CSSObject', ranges: [[1, 11]] },
                { from: 'Unknown8', to: 'PaginationToken', includes: [2] },
                { from: 'Unknown9', to: 'PaginationToken', includes: [1, 3] },
                { from: 'Unknown9', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class PaginationToken' },
                { source: 'class PaginationToken', target: 'partial class PaginationToken : InputToken' },
                { source: 'class Pagination', target: 'partial class Pagination' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 12]] },
                { from: 'Unknown1', to: 'PopconfirmToken', includes: [2] },
                { from: 'Unknown2', to: 'PopconfirmToken', includes: [1] },
                { from: 'Unknown2', to: 'GenOptions', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class PopconfirmToken' },
                { source: 'class PopconfirmToken', target: 'partial class PopconfirmToken : TokenWithCommonCls' },
                { source: 'class Popconfirm', target: 'partial class Popconfirm' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
    {
        name: 'Popover',
        src: ['components/popover/style/index.tsx'],
        dist: 'components/popover/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat([
                'using static AntDesign.Zoom;',
                'using static AntDesign.PlacementArrow;',
            ]),
            defaultClass: 'Popover',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'string | number', to: 'double' },
                { from: 'Unknown1', to: 'CSSInterpolation[]', includes: [1, 3] },
                { from: 'Unknown1', to: 'PopoverToken', includes: [2] },
                { from: 'Unknown1', to: 'CSSObject', ranges: [[4, 16]] },
                { from: 'Unknown1', to: 'PlacementArrowOptions', includes: [13] },
                { from: 'Unknown1', to: 'PropertySkip', includes: [6] },
                { from: 'Unknown2', to: 'CSSObject', includes: [1, 3] },
                { from: 'Unknown2', to: 'PopoverToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 7]] },
                { from: 'Unknown3', to: 'PopoverToken', includes: [2] },
                { from: 'Unknown4', to: 'PopoverToken', includes: [1, 3] },
                { from: 'Unknown4', to: 'CSSInterpolation[]', includes: [2] },
                { from: 'Unknown4', to: 'GenOptions', includes: [4] },
                { from: 'Unknown4', to: '()', includes: [5] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class PopoverToken' },
                { source: 'class PopoverToken', target: 'partial class PopoverToken : TokenWithCommonCls' },
                { source: 'class Popover', target: 'partial class Popover' },
                { source: 'ControlHeight -', target: 'controlHeight -' },
                { source: 'Wireframe && genWireframeStyle(popoverToken),', target: 'wireframe ? GenWireframeStyle(popoverToken) : null,' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
    {
        name: 'Progress',
        src: ['components/progress/style/index.tsx'],
        dist: 'components/progress/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat(['using Keyframes = CssInCSharp.Keyframe;']),
            defaultClass: 'Progress',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'Keyframes', includes: [1] },
                { from: 'Unknown1', to: 'CSSObject', ranges: [[2, 5]] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 30]] },
                { from: 'Unknown2', to: 'ProgressToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 14]] },
                { from: 'Unknown3', to: 'ProgressToken', includes: [2] },
                { from: 'Unknown6', to: 'ProgressToken', includes: [1, 3] },
                { from: 'Unknown6', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class ProgressToken' },
                { source: 'class ProgressToken', target: 'partial class ProgressToken : TokenWithCommonCls' },
                { source: 'class Progress', target: 'partial class Progress' },
                { source: 'isRtl ? "RTL" : "LTR"', target: '(isRtl ? "RTL" : "LTR")' },
                { source: 'GenAntProgressActive(bool isRtl)', target: 'GenAntProgressActive(bool isRtl = false)' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 7]] },
                { from: 'Unknown1', to: 'RadioToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 27]] },
                { from: 'Unknown2', to: 'RadioToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 34]] },
                { from: 'Unknown3', to: 'RadioToken', includes: [2] },
                { from: 'Unknown3', to: 'string[]', includes: [5] },
                { from: 'Unknown5', to: 'RadioToken', includes: [1, 3] },
                { from: 'Unknown5', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class RadioToken' },
                { source: 'class RadioToken', target: 'partial class RadioToken : TokenWithCommonCls' },
                { source: 'class Radio', target: 'partial class Radio<TValue>' },
                { source: `'"\\\\a0"'`, target: `"'\\\\\\\\a0'"`, },
                { source: 'getDotSize', target: 'GetDotSize' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Transform | string[] | Transform[] | { value: Transform | string[] | (Transform | string[])[]; _skip_check_?: boolean; _multi_value_?: boolean; }', to: 'string' },
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 13]] },
                { from: 'Unknown1', to: 'RateToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 6]] },
                { from: 'Unknown3', to: 'RateToken', includes: [2] },
                { from: 'Unknown4', to: 'RateToken', includes: [1, 3] },
                { from: 'Unknown4', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class RateToken' },
                { source: 'class RateToken', target: 'partial class RateToken : TokenWithCommonCls' },
                { source: 'class Rate', target: 'partial class Rate' },
                { source: 'token.Yellow6', target: 'token["yellow6"].ToString()' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Margin<string | number>', to: 'string', },
                { from: 'Unknown1', to: 'ResultToken', includes: [1] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 7]] },
                { from: 'Unknown2', to: 'ResultToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [1, 3] },
                { from: 'Unknown3', to: 'ResultToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSInterpolation[]', includes: [1] },
                { from: 'Unknown4', to: 'ResultToken', includes: [2] },
                { from: 'Unknown5', to: 'ResultToken', includes: [1, 3] },
                { from: 'Unknown5', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class ResultToken' },
                { source: 'class ResultToken', target: 'partial class ResultToken : TokenWithCommonCls' },
                { source: 'class Result', target: 'partial class Result' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 26]] },
                { from: 'Unknown4', to: 'SegmentedToken', includes: [1, 3] },
                { from: 'Unknown4', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SegmentedToken' },
                { source: 'class SegmentedToken', target: 'partial class SegmentedToken : TokenWithCommonCls' },
                { source: 'class Segmented', target: 'partial class Segmented<TValue>' },
                { source: 'segmentedTextEllipsisCss', target: 'SegmentedTextEllipsisCss' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
    {
        name: 'Skeleton',
        src: ['components/skeleton/style/index.tsx'],
        dist: 'components/skeleton/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat(['using Keyframes = CssInCSharp.Keyframe;']),
            defaultClass: 'Skeleton',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', includes: [1, 2, 3] },
                { from: 'Unknown13', to: 'CSSObject', ranges: [[1, 26]] },
                { from: 'Unknown14', to: 'SkeletonToken', includes: [1, 3] },
                { from: 'Unknown14', to: 'CSSInterpolation[]', includes: [2] },
                { from: 'Unknown14', to: 'GenOptions', includes: [4] },
                { from: 'Unknown14', to: '()', includes: [5] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SkeletonToken' },
                { source: 'class SkeletonToken', target: 'partial class SkeletonToken : TokenWithCommonCls' },
                { source: 'class Skeleton', target: 'partial class Skeleton' },
                { source: 'skeletonClsLoading', target: '_skeletonClsLoading' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 34]] },
                { from: 'Unknown1', to: 'SliderToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSObject', includes: [1, 2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 5]] },
                { from: 'Unknown3', to: 'SliderToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 4]] },
                { from: 'Unknown4', to: 'SliderToken', includes: [2] },
                { from: 'Unknown5', to: 'SliderToken', includes: [1, 3] },
                { from: 'Unknown5', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SliderToken' },
                { source: 'class SliderToken', target: 'partial class SliderToken : TokenWithCommonCls' },
                { source: 'class Slider', target: 'partial class Slider<TValue>' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 34]] },
                { from: 'Unknown1', to: 'SliderToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSObject', includes: [1, 2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 5]] },
                { from: 'Unknown3', to: 'SliderToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 4]] },
                { from: 'Unknown4', to: 'SliderToken', includes: [2] },
                { from: 'Unknown5', to: 'SliderToken', includes: [1, 3] },
                { from: 'Unknown5', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SliderToken' },
                { source: 'class SliderToken', target: 'partial class SliderToken : TokenWithCommonCls' },
                { source: 'class Slider', target: 'partial class Slider<TValue>' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
    {
        name: 'Space',
        src: [
            'components/space/style/index.ts',
            'components/space/style/compact.ts',
        ],
        dist: 'components/space/Style.cs',
        csOptions: {
            ...defaultOptions,
            defaultClass: 'Space',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 12]] },
                { from: 'Unknown1', to: 'SpaceToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 10]] },
                { from: 'Unknown2', to: 'SpaceToken', includes: [2] },
                { from: 'Unknown3', to: 'SpaceToken', includes: [1, 3] },
                { from: 'Unknown3', to: 'CSSInterpolation[]', includes: [2] },
                { from: 'Unknown3', to: 'GenOptions', includes: [4] },
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 6]] },
                { from: 'Unknown4', to: 'SpaceToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SpaceToken' },
                { source: 'class SpaceToken', target: 'partial class SpaceToken : TokenWithCommonCls' },
                { source: 'class Space', target: 'partial class Space' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
    {
        name: 'Spin',
        src: ['components/spin/style/index.ts'],
        dist: 'components/spin/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat(['using Keyframes = CssInCSharp.Keyframe;']),
            defaultClass: 'Spin',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', includes: [1, 2] },
                { from: 'Unknown2', to: 'CSSObject', includes: [1, 2] },
                { from: 'Unknown3', to: 'double', includes: [1] },
                { from: 'Unknown5', to: 'SpinToken', includes: [1, 3] },
                { from: 'Unknown5', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SpinToken' },
                { source: 'class SpinToken', target: 'partial class SpinToken : TokenWithCommonCls' },
                { source: 'class Spin', target: 'partial class Spin' },
                { source: 'antSpinMove', target: '_antSpinMove' },
                { source: 'antRotate', target: '_antRotate' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown2', to: 'StatisticToken', includes: [1, 3] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class StatisticToken' },
                { source: 'class StatisticToken', target: 'partial class StatisticToken : TokenWithCommonCls' },
                { source: 'class Statistic', target: 'partial class Statistic<TValue>' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'MaxWidth<string | number>', to: 'string' },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 21]] },
                { from: 'Unknown2', to: 'StepsToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 22]] },
                { from: 'Unknown3', to: 'StepsToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 4]] },
                { from: 'Unknown4', to: 'StepsToken', includes: [2] },
                { from: 'Unknown5', to: 'StepsToken', includes: [1, 3] },
                { from: 'Unknown5', to: 'CSSInterpolation[]', includes: [2] },
                { from: 'Unknown6', to: 'CSSObject', ranges: [[1, 9]] },
                { from: 'Unknown6', to: 'StepsToken', includes: [2] },
                { from: 'Unknown7', to: 'CSSObject', ranges: [[1, 29]] },
                { from: 'Unknown7', to: 'StepsToken', includes: [2] },
                { from: 'Unknown8', to: 'CSSObject', ranges: [[1, 14]] },
                { from: 'Unknown8', to: 'StepsToken', includes: [2] },
                { from: 'Unknown9', to: 'CSSObject', ranges: [[1, 30]] },
                { from: 'Unknown9', to: 'StepsToken', includes: [2] },
                { from: 'Unknown10', to: 'CSSObject', ranges: [[1, 26]] },
                { from: 'Unknown10', to: 'StepsToken', includes: [2] },
                { from: 'Unknown11', to: 'CSSObject', ranges: [[1, 15]] },
                { from: 'Unknown11', to: 'StepsToken', includes: [2] },
                { from: 'Unknown12', to: 'CSSObject', ranges: [[1, 14]] },
                { from: 'Unknown12', to: 'StepsToken', includes: [2] },
                { from: 'Unknown13', to: 'CSSObject', ranges: [[1, 13]] },
                { from: 'Unknown13', to: 'StepsToken', includes: [2] },
                { from: 'Unknown14', to: 'CSSObject', ranges: [[1, 17]] },
                { from: 'Unknown14', to: 'StepsToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class StepsToken' },
                { source: 'class StepsToken', target: 'partial class StepsToken : TokenWithCommonCls' },
                { source: 'class Steps', target: 'partial class Steps' },
                { source: 'StepItemStatusEnum status', target: 'string status' },
                { source: 'textEllipsis', target: 'TextEllipsis' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 20]] },
                { from: 'Unknown1', to: 'SwitchToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSObject', ranges: [[1, 6]] },
                { from: 'Unknown2', to: 'SwitchToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 11]] },
                { from: 'Unknown3', to: 'SwitchToken', includes: [2] },
                { from: 'Unknown4', to: 'CSSObject', ranges: [[1, 16]] },
                { from: 'Unknown4', to: 'SwitchToken', includes: [2] },
                { from: 'Unknown6', to: 'SwitchToken', includes: [1, 3] },
                { from: 'Unknown6', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class SwitchToken' },
                { source: 'class SwitchToken', target: 'partial class SwitchToken : TokenWithCommonCls' },
                { source: 'class Switch', target: 'partial class Switch' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            usings: defaultOptions.usings.concat(['using static AntDesign.Slide;']),
            defaultClass: 'Tabs',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown5', to: 'CSSObject', ranges: [[1, 19]] },
                { from: 'Unknown5', to: 'PropertySkip', includes: [8, 9, 18, 20] },
                { from: 'Unknown6', to: 'CSSObject', ranges: [[1, 30]] },
                { from: 'Unknown6', to: 'PropertySkip', includes: [6, 8, 10, 11, 13, 14, 25, 26, 30] },
                { from: 'Unknown8', to: 'TabsToken', includes: [1, 3] },
                { from: 'Unknown8', to: 'CSSInterpolation[]', includes: [2] },
                { from: 'Unknown9', to: 'CSSObject', ranges: [[4, 12]] },
                { from: 'Unknown9', to: 'CSSInterpolation[]', includes: [1, 3] },
                { from: 'Unknown9', to: 'TabsToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TabsToken' },
                { source: 'class TabsToken', target: 'partial class TabsToken : TokenWithCommonCls' },
                { source: 'class Tabs', target: 'partial class Tabs' },
                { source: 'textEllipsis', target: 'TextEllipsis' },
                { source: `"{componentCls}-nav-wrap-ping"`, target: `\\"{componentCls}-nav-wrap-ping\\"` },
                { source: `Token.CardPadding`, target: `token.CardPadding ?? $"{(token.CardHeight - Math.Round(token.FontSize * token.LineHeight)) / 2 - token.LineWidth}px {token.Padding}px"` },
                { source: `([object Object], [object Object])`, target: `InitSlideMotion(token, "slide-up"), InitSlideMotion(token, "slide-down")` },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'React.CSSProperties', to: 'string' },
                { from: 'CSSInterpolation', to: 'CSSObject' },
                { from: 'Unknown2', to: 'TagToken', includes: [1, 2, 3] },
                { from: 'Unknown3', to: 'TagToken', includes: [1, 3] },
                { from: 'Unknown3', to: 'GlobalToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TagToken' },
                { source: 'class TagToken', target: 'partial class TagToken : TokenWithCommonCls' },
                { source: 'class Tag', target: 'partial class Tag' },
                { source: 'prepareCommonToken', target: 'PrepareCommonToken' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 43]] },
                { from: 'Unknown1', to: 'TimelineToken', includes: [2] },
                { from: 'Unknown2', to: 'TimelineToken', includes: [1, 3] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TimelineToken' },
                { source: 'class TimelineToken', target: 'partial class TimelineToken : TokenWithCommonCls' },
                { source: 'class Timeline', target: 'partial class Timeline' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
    {
        name: 'Tooltip',
        src: ['components/tooltip/style/index.ts'],
        dist: 'components/tooltip/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat([
                'using static AntDesign.Zoom;',
                'using static AntDesign.PlacementArrow;',
            ]),
            defaultClass: 'Tooltip',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[4, 19]] },
                { from: 'Unknown1', to: 'CSSInterpolation[]', includes: [1, 3] },
                { from: 'Unknown1', to: 'TooltipToken', includes: [2, 16] },
                { from: 'Unknown1', to: 'PlacementArrowOptions', includes: [17] },
                { from: 'Unknown2', to: 'TooltipToken', includes: [1, 3] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TooltipToken' },
                { source: 'class TooltipToken', target: 'partial class TooltipToken : TokenWithCommonCls' },
                { source: 'class Tooltip', target: 'partial class Tooltip' },
                {
                    source: '["&-placement-left","&-placement-leftTop","&-placement-leftBottom","&-placement-right","&-placement-rightTop","&-placement-rightBottom",].join(",")',
                    target: 'new []{"&-placement-left","&-placement-leftTop","&-placement-leftBottom","&-placement-right","&-placement-rightTop","&-placement-rightBottom"}.Join(",")'
                },
                { source: 'new UseComponentStyleResult()', target: 'new GenOptions()' },
                { source: 'var TooltipToken', target: 'var tooltipToken' },
                { source: 'GenTooltipStyle(TooltipToken)', target: 'GenTooltipStyle(tooltipToken)' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Unknown7', to: 'TransferToken', includes: [1, 3] },
                { from: 'Unknown7', to: 'CSSInterpolation[]', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TransferToken' },
                { source: 'class TransferToken', target: 'partial class TransferToken : TokenWithCommonCls' },
                { source: 'class Transfer', target: 'partial class Transfer' },
                { source: 'textEllipsis', target: 'TextEllipsis' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
    {
        name: 'Tree',
        src: ['components/tree/style/index.ts'],
        dist: 'components/tree/Style.cs',
        csOptions: {
            ...defaultOptions,
            usings: defaultOptions.usings.concat([
                'using static AntDesign.CollapseMotion;',
                'using Keyframes = CssInCSharp.Keyframe;',
            ]),
            defaultClass: 'Tree',
            propertyMap: '_tokens',
            typeMap: [
                { from: 'DerivativeToken', to: 'GlobalToken' },
                { from: 'Unknown1', to: 'CSSObject', includes: [1, 2, 3] },
                { from: 'Unknown3', to: 'CSSObject', ranges: [[1, 4]] },
                { from: 'CSSInterpolation', to: 'CSSInterpolation[]' },
                { from: 'AliasToken & TreeSharedToken', to: 'TreeSharedToken' },
                { from: 'Unknown8', to: 'CSSInterpolation[]', includes: [1] },
                { from: 'Unknown8', to: 'CSSObject', includes: [2] },
                { from: 'Unknown8', to: 'TreeToken', includes: [3] },
                { from: 'AliasToken', to: 'GlobalToken' },
            ],
            transforms: [
                { source: 'class TreeSharedToken', target: 'partial class TreeSharedToken : TokenWithCommonCls' },
                { source: 'class ComponentToken', target: 'partial class TreeToken' },
                { source: 'class TreeToken', target: 'partial class TreeToken : TreeSharedToken' },
                { source: 'class Tree', target: 'partial class Tree<TItem>' },
                { source: 'treeNodeFX', target: '_treeNodeFX' },
                { source: 'new CSSInterpolation[]()', target: 'new TreeToken()' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
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
            usings: defaultOptions.usings.concat([
                'using static AntDesign.TreeStyle;',
            ]),
            typeMap: [
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 16]] },
                { from: 'Unknown1', to: 'CSSInterpolation[]', includes: [1, 3, 5] },
                { from: 'Unknown1', to: 'TreeSelectToken', includes: [2] },
                { from: 'Unknown1', to: 'TreeSharedToken', includes: [7] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TreeSelectToken' },
                { source: 'class TreeSelectToken', target: 'partial class TreeSelectToken : TokenWithCommonCls' },
                { source: 'class TreeSelect', target: 'partial class TreeSelect' },
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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'string | number', to: 'string' },
                { from: 'Unknown1', to: 'CSSObject', ranges: [[1, 17]] },
                { from: 'Unknown1', to: 'TypographyToken', includes: [2] },
                { from: 'Unknown2', to: 'CSSInterpolation[]', includes: [1] },
                { from: 'Unknown2', to: 'TypographyToken', includes: [2] },
                { from: 'Unknown3', to: 'CSSObject', includes: [1, 2] },
                { from: 'Unknown5', to: 'CSSObject', ranges: [[1, 8]] },
                { from: 'Unknown5', to: 'TypographyToken', includes: [2] },
                { from: 'Unknown6', to: 'TypographyToken', includes: [1] },
                { from: 'Unknown7', to: 'CSSObject', ranges: [[1, 7]] },
                { from: 'Unknown7', to: 'TypographyToken', includes: [2] },
                { from: 'Unknown8', to: 'CSSObject', ranges: [[1, 5]] },
                { from: 'Unknown8', to: 'TypographyToken', includes: [2] },
            ],
            transforms: [
                { source: 'class ComponentToken', target: 'partial class TypographyToken' },
                { source: 'class TypographyToken', target: 'partial class TypographyToken : TokenWithCommonCls' },
                { source: 'class Typography', target: 'partial class TypographyBase' },
                { source: 'WebkitBoxOrient', target: '["-web-kit-box-orient"]' },
                { source: 'public UseComponentStyleResult ExportDefault', target: 'protected override UseComponentStyleResult UseComponentStyle' },
            ]
        }
    },
]

// 未完成迁移
const uncompleted: Component[] = [

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
            propertyMap: '_tokens',
            typeMap: [
                { from: 'Padding<string | number>', to: 'string' },
                { from: 'LineHeight<string | number>', to: 'string' },
                { from: 'FontWeight', to: 'string' }
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

// 用于生成的实例，将需要生成的组件配置放到这里
export const components: Component[] = uncompleted.slice(0, 1);