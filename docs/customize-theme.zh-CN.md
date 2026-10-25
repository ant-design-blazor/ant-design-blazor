---
order: 7
title: 定制主题
---

Ant Design Blazor v6 使用 Ant Design v6 的 Design Token 体系。可以通过全局 token 定制应用主题、选择内置算法，或只覆盖某个组件的 token。这些设置由 `ConfigProvider` 作用域管理，不需要重新编译 Less。

## 基本使用

通过 `ThemeConfig.Token` 设置全局 Seed、Map 或 Alias token。token 名称与 React v6 中的 camelCase 名称一致。值使用字符串表示，因此颜色、CSS 长度、数字和布尔值均可配置。

```csharp
using AntDesign.Theming;

private static readonly ThemeConfig BrandTheme = new()
{
    Token = new ThemeToken
    {
        ["colorPrimary"] = "#722ed1",
        ["colorSuccess"] = "#389e0d",
        ["borderRadius"] = "8",
        ["controlHeight"] = "36",
    }
};
```

```razor
<ConfigProvider ThemeConfig="@BrandTheme">
    <Button Type="ButtonType.Primary">主题按钮</Button>
    <Input Placeholder="主题输入框" />
</ConfigProvider>
```

`ThemeConfig.Token` 还提供生成的强类型快捷属性，例如 `Token.ColorPrimary`；动态配置 token 时也可以使用字符串索引器。全局 token 属性和来源接口由 Ant Design v6 源码生成，C# 定义位于 `components/theming/GeneratedThemeTokens.cs` 和 `components/theming/ThemeTokenProperties.cs`；组件级 token 定义位于 `components/theming/GeneratedComponentTokens.cs`。

## 预设算法

主题颜色与双色图标共用独立的 `AntDesign.Colors` 库，迁移自当前 Ant v6 依赖中的
`@ant-design/colors` 8.0.1 和所需的 `@ant-design/fast-color` 3.0.1 运算，
不再维护 Icon 专用色板算法，也不通过 JavaScript 计算颜色。

```csharp
using AntDesign.Colors;

string[] light = ColorPalette.Generate("#1677ff"); // 完整十档色板
string[] dark = ColorPalette.Generate("#1677ff", dark: true, backgroundColor: "#141414");
string secondary = light[0]; // 双色图标也取同一色板的第一档
```

默认算法会根据 Seed token 推导 Map 和 Alias token。还提供 `dark` 和 `compact` 算法。Seed 输入参与算法推导（因此 compact 会缩小 `controlHeight`，dark 会转换 `colorPrimary`）；显式 Map 覆盖在 Alias 推导前合并，显式 Alias 覆盖拥有最终优先级。

```csharp
private static readonly ThemeConfig DarkTheme = new()
{
    Algorithm = "dark",
    Token = new ThemeToken { ["colorPrimary"] = "#1677ff" }
};

private static readonly ThemeConfig CompactTheme = new()
{
    Algorithm = "compact",
    Token = new ThemeToken { ["controlHeight"] = "28" }
};
```

可用算法也会列在 `GeneratedThemeTokens.cs` 中。Ant Design Blazor 当前每个 `ThemeConfig` 选择一个算法（`default`、`dark` 或 `compact`）；React 的算法组合数组尚未作为 Blazor API 暴露。

## 组件级 Token

使用 `SetComponentToken` 将定制限制在单个组件。组件 token 类型和 CSS 变量名均从 React v6 的样式 token 源生成。

```csharp
private static readonly ThemeConfig SelectTheme = new ThemeConfig()
    .SetComponentToken("Select", new SelectComponentToken
    {
        OptionSelectedBg = "#f9f0ff",
        OptionSelectedColor = "#531dab",
        MultipleItemBg = "#efdbff",
    });
```

```razor
<ConfigProvider ThemeConfig="@SelectTheme">
    <Select TItemValue="string" TItem="string" Placeholder="请选择">
        <SelectOptions>
            <SelectOption TItemValue="string" TItem="string" Value='@("one")' Label="选项一" />
            <SelectOption TItemValue="string" TItem="string" Value='@("two")' Label="选项二" />
        </SelectOptions>
    </Select>
</ConfigProvider>
```

每个组件文档下方的 Design Token 章节会说明该组件支持的 token。没有专属 token 的组件会注明继承全局 token 与状态样式。

## 嵌套主题与 CSS 变量

嵌套的 `ConfigProvider` 会创建局部主题作用域。未指定的全局值从父级继承，覆盖值只影响该 Provider 的后代。默认启用 `CssVar`，Provider 会在作用域元素上输出解析后的 `--ant-*` CSS 自定义属性。组件 token 的变量示例为 `--ant-select-option-selected-bg`。

继承时合并的是显式配置，而不是父级已派生的色板。子级未设置 `Algorithm` 时继承父级算法；显式设置 `Algorithm = "default"` 可切回默认算法。设置 `Inherit = false` 可创建全新的默认主题，不继承父级 token 和组件覆盖。

```razor
<ConfigProvider ThemeConfig="@BrandTheme">
    <Button Type="ButtonType.Primary">品牌主题</Button>

    <ConfigProvider ThemeConfig="@DarkTheme">
        <Card>局部主题区域</Card>
    </ConfigProvider>
</ConfigProvider>
```

如果只需通过 CSS 微调，请针对 Provider 作用域设置变量，而不是设置 `:root`，避免意外影响页面上的其他主题区域：

```css
.ant-theme {
  --ant-color-primary: #722ed1;
}
```

对于参与派生的 token（例如 `colorPrimary`），优先使用 `ThemeConfig`；CSS 覆盖不会重新计算相关派生 token。只有明确不需要 Provider 输出运行时变量时才设置 `CssVar = false`。

## Token 分层

Ant Design v6 全局 token 按推导关系组织：Seed token 表达设计意图，选定的算法派生 Map token，Alias token 提供更适合组件样式使用的名称。需要联动调整颜色、尺寸或状态时，优先修改 Seed token；也可以显式覆盖某个派生 token。组件 token 则属于独立的组件作用域。

当前 C# 全局 token API 从 Ant Design 仓库的 `components/theme/interface/seeds.ts`、`components/theme/interface/maps/` 和 `components/theme/interface/alias.ts` 生成，使用 `generate-csharp-theme-tokens` 同步。同步报告会验证 Seed/Alias 属性和快捷属性是否缺漏；Map token 分组可通过 `ThemeConfig.Map` 和全局 token 索引器访问。

## 迁移说明

旧版 v4 Less 变量（例如 `@primary-color`、`modifyVars` 或 `ant-design-blazor.variable.css`）不适用于 v6 token 工作流。v6 样式使用 `--ant-*` 变量和 `ThemeConfig`，组件样式由 NuGet 包提供。请使用上面的生成 token API，不要继续依赖 v4 变量名。

<!-- BEGIN GENERATED GLOBAL TOKEN REFERENCE -->
<h2 id="global-token-reference">全局 Token 参考</h2>
此表由 Ant Design React v6 的 `components/version/token-meta.json` 生成，共 234 项。CSS 变量名按 Blazor v6 的变量格式展示。

### Seed（基础）

| Token | 说明 | CSS 变量 | 类型 |
| --- | --- | --- | --- |
| `blue` | 预设blue颜色 | `--ant-blue` | `color` |
| `borderRadius` | 基础组件的圆角大小，例如按钮、输入框、卡片等 | `--ant-border-radius` | `number` |
| `colorBgBase` | 用于派生背景色梯度的基础变量，v5 中我们添加了一层背景色的派生算法可以产出梯度明确的背景色的梯度变量。但请不要在代码中直接使用该 Seed Token ！ | `--ant-color-bg-base` | `string` |
| `colorError` | 用于表示操作失败的 Token 序列，如失败按钮、错误状态提示（Result）组件等。 | `--ant-color-error` | `string` |
| `colorInfo` | 用于表示操作信息的 Token 序列，如 Alert 、Tag、 Progress 等组件都有用到该组梯度变量。 | `--ant-color-info` | `string` |
| `colorLink` | 控制超链接的颜色。 | `--ant-color-link` | `string` |
| `colorPrimary` | 品牌色是体现产品特性和传播理念最直观的视觉元素之一。在你完成品牌主色的选取之后，我们会自动帮你生成一套完整的色板，并赋予它们有效的设计语义 | `--ant-color-primary` | `string` |
| `colorSuccess` | 用于表示操作成功的 Token 序列，如 Result、Progress 等组件会使用该组梯度变量。 | `--ant-color-success` | `string` |
| `colorTextBase` | 用于派生文本色梯度的基础变量，v5 中我们添加了一层文本色的派生算法可以产出梯度明确的文本色的梯度变量。但请不要在代码中直接使用该 Seed Token ！ | `--ant-color-text-base` | `string` |
| `colorWarning` | 用于表示操作警告的 Token 序列，如 Notification、 Alert等警告类组件或 Input 输入类等组件会使用该组梯度变量。 | `--ant-color-warning` | `string` |
| `controlHeight` | Ant Design 中按钮和输入框等基础控件的高度 | `--ant-control-height` | `number` |
| `cyan` | 预设cyan颜色 | `--ant-cyan` | `color` |
| `focusOutline` | 用于配置组件聚焦时是否显示可见描边 | `--ant-focus-outline` | `boolean` |
| `fontFamily` | Ant Design 的字体家族中优先使用系统默认的界面字体，同时提供了一套利于屏显的备用字体库，来维护在不同平台以及浏览器的显示下，字体始终保持良好的易读性和可读性，体现了友好、稳定和专业的特性。 | `--ant-font-family` | `string` |
| `fontFamilyCode` | 代码字体，用于 Typography 内的 code、pre 和 kbd 类型的元素 | `--ant-font-family-code` | `string` |
| `fontSize` | 设计系统中使用最广泛的字体大小，文本梯度也将基于该字号进行派生。 | `--ant-font-size` | `number` |
| `geekblue` | 预设geekblue颜色 | `--ant-geekblue` | `color` |
| `gold` | 预设gold颜色 | `--ant-gold` | `color` |
| `green` | 预设green颜色 | `--ant-green` | `color` |
| `lime` | 预设lime颜色 | `--ant-lime` | `color` |
| `lineType` | 用于控制组件边框、分割线等的样式，默认是实线 | `--ant-line-type` | `string` |
| `lineWidth` | 用于控制组件边框、分割线等的宽度 | `--ant-line-width` | `number` |
| `magenta` | 预设magenta颜色 | `--ant-magenta` | `color` |
| `motion` | 用于配置动画效果，为 `false` 时则关闭动画 | `--ant-motion` | `boolean` |
| `motionBase` | 动画基础时长。 | `--ant-motion-base` | `number` |
| `motionEaseInBack` | 预设动效曲率 | `--ant-motion-ease-in-back` | `string` |
| `motionEaseInOut` | 预设动效曲率 | `--ant-motion-ease-in-out` | `string` |
| `motionEaseInOutCirc` | 预设动效曲率 | `--ant-motion-ease-in-out-circ` | `string` |
| `motionEaseInQuint` | 预设动效曲率 | `--ant-motion-ease-in-quint` | `string` |
| `motionEaseOut` | 预设动效曲率 | `--ant-motion-ease-out` | `string` |
| `motionEaseOutBack` | 预设动效曲率 | `--ant-motion-ease-out-back` | `string` |
| `motionEaseOutCirc` | 预设动效曲率 | `--ant-motion-ease-out-circ` | `string` |
| `motionEaseOutQuint` | 预设动效曲率 | `--ant-motion-ease-out-quint` | `string` |
| `motionUnit` | 用于控制动画时长的变化单位 | `--ant-motion-unit` | `number` |
| `opacityImage` | 控制图片不透明度 | `--ant-opacity-image` | `number` |
| `orange` | 预设orange颜色 | `--ant-orange` | `color` |
| `pink` | 预设pink颜色 | `--ant-pink` | `color` |
| `purple` | 预设purple颜色 | `--ant-purple` | `color` |
| `red` | 预设red颜色 | `--ant-red` | `color` |
| `sizePopupArrow` | 组件箭头的尺寸 | `--ant-size-popup-arrow` | `number` |
| `sizeStep` | 用于控制组件尺寸的基础步长，尺寸步长结合尺寸变化单位，就可以派生各种尺寸梯度。通过调整步长即可得到不同的布局模式，例如 V5 紧凑模式下的尺寸步长为 2 | `--ant-size-step` | `number` |
| `sizeUnit` | 用于控制组件尺寸的变化单位，在 Ant Design 中我们的基础单位为 4 ，便于更加细致地控制尺寸梯度 | `--ant-size-unit` | `number` |
| `volcano` | 预设volcano颜色 | `--ant-volcano` | `color` |
| `wireframe` | 用于将组件的视觉效果变为线框化，如果需要使用 V4 的效果，需要开启配置项 | `--ant-wireframe` | `boolean` |
| `yellow` | 预设yellow颜色 | `--ant-yellow` | `color` |
| `zIndexBase` | 所有组件的基础 Z 轴值，用于一些悬浮类的组件的可以基于该值 Z 轴控制层级，例如 BackTop、 Affix 等 | `--ant-z-index-base` | `number` |
| `zIndexPopupBase` | 浮层类组件的基础 Z 轴值，用于一些悬浮类的组件的可以基于该值 Z 轴控制层级，例如 FloatButton、 Affix、Modal 等 | `--ant-z-index-popup-base` | `number` |

### Map（派生）

| Token | 说明 | CSS 变量 | 类型 |
| --- | --- | --- | --- |
| `borderRadiusLG` | LG号圆角，用于组件中的一些大圆角，如 Card、Modal 等一些组件样式。 | `--ant-border-radius-lg` | `number` |
| `borderRadiusOuter` | 外部圆角 | `--ant-border-radius-outer` | `number` |
| `borderRadiusSM` | SM号圆角，用于组件小尺寸下的圆角，如 Button、Input、Select 等输入类控件在 small size 下的圆角 | `--ant-border-radius-sm` | `number` |
| `borderRadiusXS` | XS号圆角，用于组件中的一些小圆角，如 Segmented 、Arrow 等一些内部圆角的组件样式中。 | `--ant-border-radius-xs` | `number` |
| `colorBgBlur` | 控制毛玻璃容器的背景色，通常为透明色。 | `--ant-color-bg-blur` | `string` |
| `colorBgContainer` | 组件的容器背景色，例如：默认按钮、输入框等。务必不要将其与 `colorBgElevated` 混淆。 | `--ant-color-bg-container` | `string` |
| `colorBgElevated` | 浮层容器背景色，在暗色模式下该 token 的色值会比 `colorBgContainer` 要亮一些。例如：模态框、弹出框、菜单等。 | `--ant-color-bg-elevated` | `string` |
| `colorBgLayout` | 该色用于页面整体布局的背景色，只有需要在页面中处于 B1 的视觉层级时才会使用该 token，其他用法都是错误的 | `--ant-color-bg-layout` | `string` |
| `colorBgMask` | 浮层的背景蒙层颜色，用于遮罩浮层下面的内容，Modal、Drawer、Image 等组件的蒙层使用的是该 token | `--ant-color-bg-mask` | `string` |
| `colorBgSolid` | 实心的背景颜色，目前只用在默认实心按钮背景色上。 | `--ant-color-bg-solid` | `string` |
| `colorBgSolidActive` | 实心的背景颜色激活态，目前只用在默认实心按钮的 active 效果。 | `--ant-color-bg-solid-active` | `string` |
| `colorBgSolidHover` | 实心的背景颜色悬浮态，目前只用在默认实心按钮的 hover 效果。 | `--ant-color-bg-solid-hover` | `string` |
| `colorBgSpotlight` | 该色用于引起用户强烈关注注意的背景色，目前只用在 Tooltip 的背景色上。 | `--ant-color-bg-spotlight` | `string` |
| `colorBorder` | 默认使用的边框颜色, 用于分割不同的元素，例如：表单的分割线、卡片的分割线等。 | `--ant-color-border` | `string` |
| `colorBorderDisabled` | 控制元素在禁用状态下的边框颜色。 | `--ant-color-border-disabled` | `string` |
| `colorBorderSecondary` | 比默认使用的边框色要浅一级，此颜色和 colorSplit 的颜色一致。使用的是实色。 | `--ant-color-border-secondary` | `string` |
| `colorErrorActive` | 错误色的深色激活态 | `--ant-color-error-active` | `string` |
| `colorErrorBg` | 错误色的浅色背景颜色 | `--ant-color-error-bg` | `string` |
| `colorErrorBgActive` | 错误色的浅色背景色激活态 | `--ant-color-error-bg-active` | `string` |
| `colorErrorBgFilledHover` | 错误色的浅色填充背景色悬浮态，目前只用在危险填充按钮的 hover 效果。 | `--ant-color-error-bg-filled-hover` | `string` |
| `colorErrorBgHover` | 错误色的浅色背景色悬浮态 | `--ant-color-error-bg-hover` | `string` |
| `colorErrorBorder` | 错误色的描边色 | `--ant-color-error-border` | `string` |
| `colorErrorBorderHover` | 错误色的描边色悬浮态 | `--ant-color-error-border-hover` | `string` |
| `colorErrorHover` | 错误色的深色悬浮态 | `--ant-color-error-hover` | `string` |
| `colorErrorText` | 错误色的文本默认态 | `--ant-color-error-text` | `string` |
| `colorErrorTextActive` | 错误色的文本激活态 | `--ant-color-error-text-active` | `string` |
| `colorErrorTextHover` | 错误色的文本悬浮态 | `--ant-color-error-text-hover` | `string` |
| `colorFill` | 最深的填充色，用于拉开与二、三级填充色的区分度，目前只用在 Slider 的 hover 效果。 | `--ant-color-fill` | `string` |
| `colorFillQuaternary` | 最弱一级的填充色，适用于不易引起注意的色块，例如斑马纹、区分边界的色块等。 | `--ant-color-fill-quaternary` | `string` |
| `colorFillSecondary` | 二级填充色可以较为明显地勾勒出元素形体，如 Rate、Skeleton 等。也可以作为三级填充色的 Hover 状态，如 Table 等。 | `--ant-color-fill-secondary` | `string` |
| `colorFillTertiary` | 三级填充色用于勾勒出元素形体的场景，如 Slider、Segmented 等。如无强调需求的情况下，建议使用三级填色作为默认填色。 | `--ant-color-fill-tertiary` | `string` |
| `colorInfoActive` | 信息色的深色激活态。 | `--ant-color-info-active` | `string` |
| `colorInfoBg` | 信息色的浅色背景颜色。 | `--ant-color-info-bg` | `string` |
| `colorInfoBgHover` | 信息色的浅色背景色悬浮态。 | `--ant-color-info-bg-hover` | `string` |
| `colorInfoBorder` | 信息色的描边色。 | `--ant-color-info-border` | `string` |
| `colorInfoBorderHover` | 信息色的描边色悬浮态。 | `--ant-color-info-border-hover` | `string` |
| `colorInfoHover` | 信息色的深色悬浮态。 | `--ant-color-info-hover` | `string` |
| `colorInfoText` | 信息色的文本默认态。 | `--ant-color-info-text` | `string` |
| `colorInfoTextActive` | 信息色的文本激活态。 | `--ant-color-info-text-active` | `string` |
| `colorInfoTextHover` | 信息色的文本悬浮态。 | `--ant-color-info-text-hover` | `string` |
| `colorLinkActive` | 控制超链接被点击时的颜色。 | `--ant-color-link-active` | `string` |
| `colorLinkHover` | 控制超链接悬浮时的颜色。 | `--ant-color-link-hover` | `string` |
| `colorPrimaryActive` | 主色梯度下的深色激活态。 | `--ant-color-primary-active` | `string` |
| `colorPrimaryBg` | 主色浅色背景颜色，一般用于视觉层级较弱的选中状态。 | `--ant-color-primary-bg` | `string` |
| `colorPrimaryBgHover` | 与主色浅色背景颜色相对应的悬浮态颜色。 | `--ant-color-primary-bg-hover` | `string` |
| `colorPrimaryBorder` | 主色梯度下的描边用色，用在 Slider 等组件的描边上。 | `--ant-color-primary-border` | `string` |
| `colorPrimaryBorderHover` | 主色梯度下的描边用色的悬浮态，Slider 、Button 等组件的描边 Hover 时会使用。 | `--ant-color-primary-border-hover` | `string` |
| `colorPrimaryHover` | 主色梯度下的悬浮态。 | `--ant-color-primary-hover` | `string` |
| `colorPrimaryText` | 主色梯度下的文本颜色。 | `--ant-color-primary-text` | `string` |
| `colorPrimaryTextActive` | 主色梯度下的文本激活态。 | `--ant-color-primary-text-active` | `string` |
| `colorPrimaryTextHover` | 主色梯度下的文本悬浮态。 | `--ant-color-primary-text-hover` | `string` |
| `colorSuccessActive` | 成功色的深色激活态 | `--ant-color-success-active` | `string` |
| `colorSuccessBg` | 成功色的浅色背景颜色，用于 Tag 和 Alert 的成功态背景色 | `--ant-color-success-bg` | `string` |
| `colorSuccessBgHover` | 成功色浅色背景颜色，一般用于视觉层级较弱的选中状态，不过 antd 目前没有使用到该 token | `--ant-color-success-bg-hover` | `string` |
| `colorSuccessBorder` | 成功色的描边色，用于 Tag 和 Alert 的成功态描边色 | `--ant-color-success-border` | `string` |
| `colorSuccessBorderHover` | 成功色的描边色悬浮态 | `--ant-color-success-border-hover` | `string` |
| `colorSuccessHover` | 成功色的深色悬浮态 | `--ant-color-success-hover` | `string` |
| `colorSuccessText` | 成功色的文本默认态 | `--ant-color-success-text` | `string` |
| `colorSuccessTextActive` | 成功色的文本激活态 | `--ant-color-success-text-active` | `string` |
| `colorSuccessTextHover` | 成功色的文本悬浮态 | `--ant-color-success-text-hover` | `string` |
| `colorText` | 最深的文本色。为了符合W3C标准，默认的文本颜色使用了该色，同时这个颜色也是最深的中性色。 | `--ant-color-text` | `string` |
| `colorTextQuaternary` | 第四级文本色是最浅的文本色，例如表单的输入提示文本、禁用色文本等。 | `--ant-color-text-quaternary` | `string` |
| `colorTextSecondary` | 作为第二梯度的文本色，一般用在不那么需要强化文本颜色的场景，例如 Label 文本、Menu 的文本选中态等场景。 | `--ant-color-text-secondary` | `string` |
| `colorTextTertiary` | 第三级文本色一般用于描述性文本，例如表单的中的补充说明文本、列表的描述性文本等场景。 | `--ant-color-text-tertiary` | `string` |
| `colorWarningActive` | 警戒色的深色激活态 | `--ant-color-warning-active` | `string` |
| `colorWarningBg` | 警戒色的浅色背景颜色 | `--ant-color-warning-bg` | `string` |
| `colorWarningBgHover` | 警戒色的浅色背景色悬浮态 | `--ant-color-warning-bg-hover` | `string` |
| `colorWarningBorder` | 警戒色的描边色 | `--ant-color-warning-border` | `string` |
| `colorWarningBorderHover` | 警戒色的描边色悬浮态 | `--ant-color-warning-border-hover` | `string` |
| `colorWarningHover` | 警戒色的深色悬浮态 | `--ant-color-warning-hover` | `string` |
| `colorWarningText` | 警戒色的文本默认态 | `--ant-color-warning-text` | `string` |
| `colorWarningTextActive` | 警戒色的文本激活态 | `--ant-color-warning-text-active` | `string` |
| `colorWarningTextHover` | 警戒色的文本悬浮态 | `--ant-color-warning-text-hover` | `string` |
| `colorWhite` | 不随主题变化的纯白色 | `--ant-color-white` | `string` |
| `controlHeightLG` | 较高的组件高度 | `--ant-control-height-lg` | `number` |
| `controlHeightSM` | 较小的组件高度 | `--ant-control-height-sm` | `number` |
| `controlHeightXS` | 更小的组件高度 | `--ant-control-height-xs` | `number` |
| `fontSizeHeading1` | H1 标签所使用的字号 | `--ant-font-size-heading1` | `string \| number` |
| `fontSizeHeading2` | h2 标签所使用的字号 | `--ant-font-size-heading2` | `string \| number` |
| `fontSizeHeading3` | h3 标签使用的字号 | `--ant-font-size-heading3` | `string \| number` |
| `fontSizeHeading4` | h4 标签使用的字号 | `--ant-font-size-heading4` | `string \| number` |
| `fontSizeHeading5` | h5 标签使用的字号 | `--ant-font-size-heading5` | `string \| number` |
| `fontSizeLG` | 大号字体大小 | `--ant-font-size-lg` | `number` |
| `fontSizeSM` | 小号字体大小 | `--ant-font-size-sm` | `number` |
| `fontSizeXL` | 超大号字体大小 | `--ant-font-size-xl` | `number` |
| `lineHeight` | 文本行高 | `--ant-line-height` | `number` |
| `lineHeightHeading1` | H1 标签所使用的行高 | `--ant-line-height-heading1` | `number` |
| `lineHeightHeading2` | h2 标签所使用的行高 | `--ant-line-height-heading2` | `number` |
| `lineHeightHeading3` | h3 标签所使用的行高 | `--ant-line-height-heading3` | `number` |
| `lineHeightHeading4` | h4 标签所使用的行高 | `--ant-line-height-heading4` | `number` |
| `lineHeightHeading5` | h5 标签所使用的行高 | `--ant-line-height-heading5` | `number` |
| `lineHeightLG` | 大型文本行高 | `--ant-line-height-lg` | `number` |
| `lineHeightSM` | 小型文本行高 | `--ant-line-height-sm` | `number` |
| `lineWidthBold` | 描边类组件的默认线宽，如 Button、Input、Select 等输入类控件。 | `--ant-line-width-bold` | `number` |
| `motionDurationFast` | 动效播放速度，快速。用于小型元素动画交互 | `--ant-motion-duration-fast` | `string` |
| `motionDurationMid` | 动效播放速度，中速。用于中型元素动画交互 | `--ant-motion-duration-mid` | `string` |
| `motionDurationSlow` | 动效播放速度，慢速。用于大型元素如面板动画交互 | `--ant-motion-duration-slow` | `string` |
| `size` | 中等尺寸 | `--ant-size` | `number` |
| `sizeLG` | 大尺寸 | `--ant-size-lg` | `number` |
| `sizeMD` | 中大尺寸 | `--ant-size-md` | `number` |
| `sizeMS` | 与size大小相同，但在紧凑模式下可能更大 | `--ant-size-ms` | `number` |
| `sizeSM` | 中小尺寸 | `--ant-size-sm` | `number` |
| `sizeXL` | 超大尺寸 | `--ant-size-xl` | `number` |
| `sizeXS` | 小尺寸 | `--ant-size-xs` | `number` |
| `sizeXXL` | 最大尺寸 | `--ant-size-xxl` | `number` |
| `sizeXXS` | 最小尺寸 | `--ant-size-xxs` | `number` |

### Alias（语义别名）

| Token | 说明 | CSS 变量 | 类型 |
| --- | --- | --- | --- |
| `boxShadow` | 控制元素阴影样式。 | `--ant-box-shadow` | `string` |
| `boxShadowSecondary` | 控制元素二级阴影样式。 | `--ant-box-shadow-secondary` | `string` |
| `boxShadowTertiary` | 控制元素三级盒子阴影样式。 | `--ant-box-shadow-tertiary` | `string` |
| `colorBgContainerDisabled` | 控制容器在禁用状态下的背景色。 | `--ant-color-bg-container-disabled` | `string` |
| `colorBgTextActive` | 控制文本在激活状态下的背景色。 | `--ant-color-bg-text-active` | `string` |
| `colorBgTextHover` | 控制文本在悬停状态下的背景色。 | `--ant-color-bg-text-hover` | `string` |
| `colorBorderBg` | 控制元素背景边框的颜色。 | `--ant-color-border-bg` | `string` |
| `colorErrorAffix` | 控制表单控件前后缀在错误状态下的颜色。 | `--ant-color-error-affix` | `string` |
| `colorErrorOutline` | 控制输入组件错误状态下的外轮廓线颜色。 | `--ant-color-error-outline` | `string` |
| `colorFillAlter` | 控制元素替代背景色。 | `--ant-color-fill-alter` | `string` |
| `colorFillContent` | 控制内容区域的背景色。 | `--ant-color-fill-content` | `string` |
| `colorFillContentHover` | 控制内容区域背景色在鼠标悬停时的样式。 | `--ant-color-fill-content-hover` | `string` |
| `colorHighlight` | 控制页面元素高亮时的颜色。 | `--ant-color-highlight` | `string` |
| `colorIcon` | 控制弱操作图标的颜色，例如 allowClear 或 Alert 关闭按钮。  * | `--ant-color-icon` | `string` |
| `colorIconHover` | 控制弱操作图标在悬浮状态下的颜色，例如 allowClear 或 Alert 关闭按钮。 | `--ant-color-icon-hover` | `string` |
| `colorSplit` | 用于作为分割线的颜色，此颜色和 colorBorderSecondary 的颜色一致，但是用的是透明色。 | `--ant-color-split` | `string` |
| `colorTextDescription` | 控制文本描述字体颜色。 | `--ant-color-text-description` | `string` |
| `colorTextDisabled` | 控制禁用状态下的字体颜色。 | `--ant-color-text-disabled` | `string` |
| `colorTextHeading` | 控制标题字体颜色。 | `--ant-color-text-heading` | `string` |
| `colorTextLabel` | 控制文本标签字体颜色。 | `--ant-color-text-label` | `string` |
| `colorTextLightSolid` | 控制带背景色的文本，例如 Primary Button 组件中的文本高亮颜色。 | `--ant-color-text-light-solid` | `string` |
| `colorTextPlaceholder` | 控制占位文本的颜色。 | `--ant-color-text-placeholder` | `string` |
| `colorWarningAffix` | 控制表单控件前后缀在警告状态下的颜色。 | `--ant-color-warning-affix` | `string` |
| `colorWarningOutline` | 控制输入组件警告状态下的外轮廓线颜色。 | `--ant-color-warning-outline` | `string` |
| `controlInteractiveSize` | 控制组件的交互大小。 | `--ant-control-interactive-size` | `number` |
| `controlItemBgActive` | 控制组件项在激活状态下的背景颜色。 | `--ant-control-item-bg-active` | `string` |
| `controlItemBgActiveDisabled` | 控制组件项在禁用状态下的激活背景颜色。 | `--ant-control-item-bg-active-disabled` | `string` |
| `controlItemBgActiveHover` | 控制组件项在鼠标悬浮且激活状态下的背景颜色。 | `--ant-control-item-bg-active-hover` | `string` |
| `controlItemBgHover` | 控制组件项在鼠标悬浮时的背景颜色。 | `--ant-control-item-bg-hover` | `string` |
| `controlOutline` | 控制输入组件的外轮廓线颜色。 | `--ant-control-outline` | `string` |
| `controlOutlineWidth` | 控制输入组件的外轮廓线宽度。 | `--ant-control-outline-width` | `number` |
| `controlPaddingHorizontal` | 控制元素水平内间距。 | `--ant-control-padding-horizontal` | `number` |
| `controlPaddingHorizontalSM` | 控制元素中小尺寸水平内间距。 | `--ant-control-padding-horizontal-sm` | `number` |
| `fontSizeIcon` | 控制选择器、级联选择器等中的操作图标字体大小。正常情况下与 fontSizeSM 相同。 | `--ant-font-size-icon` | `number` |
| `fontWeightStrong` | 控制标题类组件（如 h1、h2、h3）或选中项的字体粗细。 | `--ant-font-weight-strong` | `number` |
| `lineWidthFocus` | 控制线条的宽度，当组件处于聚焦态时。 | `--ant-line-width-focus` | `number` |
| `linkDecoration` | 控制链接文本的装饰样式。 | `--ant-link-decoration` | `TextDecoration<string \| number> \| undefined` |
| `linkFocusDecoration` | 控制链接聚焦时文本的装饰样式。 | `--ant-link-focus-decoration` | `TextDecoration<string \| number> \| undefined` |
| `linkHoverDecoration` | 控制链接鼠标悬浮时文本的装饰样式。 | `--ant-link-hover-decoration` | `TextDecoration<string \| number> \| undefined` |
| `margin` | 控制元素外边距，中等尺寸。 | `--ant-margin` | `number` |
| `marginLG` | 控制元素外边距，大尺寸。 | `--ant-margin-lg` | `number` |
| `marginMD` | 控制元素外边距，中大尺寸。 | `--ant-margin-md` | `number` |
| `marginSM` | 控制元素外边距，中小尺寸。 | `--ant-margin-sm` | `number` |
| `marginXL` | 控制元素外边距，超大尺寸。 | `--ant-margin-xl` | `number` |
| `marginXS` | 控制元素外边距，小尺寸。 | `--ant-margin-xs` | `number` |
| `marginXXL` | 控制元素外边距，最大尺寸。 | `--ant-margin-xxl` | `number` |
| `marginXXS` | 控制元素外边距，最小尺寸。 | `--ant-margin-xxs` | `number` |
| `opacityLoading` | 控制加载状态的透明度。 | `--ant-opacity-loading` | `number` |
| `padding` | 控制元素的内间距。 | `--ant-padding` | `number` |
| `paddingContentHorizontal` | 控制内容元素水平内间距。 | `--ant-padding-content-horizontal` | `number` |
| `paddingContentHorizontalLG` | 控制内容元素水平内间距，适用于大屏幕设备。 | `--ant-padding-content-horizontal-lg` | `number` |
| `paddingContentHorizontalSM` | 控制内容元素水平内间距，适用于小屏幕设备。 | `--ant-padding-content-horizontal-sm` | `number` |
| `paddingContentVertical` | 控制内容元素垂直内间距。 | `--ant-padding-content-vertical` | `number` |
| `paddingContentVerticalLG` | 控制内容元素垂直内间距，适用于大屏幕设备。 | `--ant-padding-content-vertical-lg` | `number` |
| `paddingContentVerticalSM` | 控制内容元素垂直内间距，适用于小屏幕设备。 | `--ant-padding-content-vertical-sm` | `number` |
| `paddingLG` | 控制元素的大内间距。 | `--ant-padding-lg` | `number` |
| `paddingMD` | 控制元素的中等内间距。 | `--ant-padding-md` | `number` |
| `paddingSM` | 控制元素的小内间距。 | `--ant-padding-sm` | `number` |
| `paddingXL` | 控制元素的特大内间距。 | `--ant-padding-xl` | `number` |
| `paddingXS` | 控制元素的特小内间距。 | `--ant-padding-xs` | `number` |
| `paddingXXS` | 控制元素的极小内间距。 | `--ant-padding-xxs` | `number` |
| `screenLG` | 控制大屏幕的屏幕宽度。 | `--ant-screen-lg` | `number` |
| `screenLGMax` | 控制大屏幕的最大宽度。 | `--ant-screen-lg-max` | `number` |
| `screenLGMin` | 控制大屏幕的最小宽度。 | `--ant-screen-lg-min` | `number` |
| `screenMD` | 控制中等屏幕的屏幕宽度。 | `--ant-screen-md` | `number` |
| `screenMDMax` | 控制中等屏幕的最大宽度。 | `--ant-screen-md-max` | `number` |
| `screenMDMin` | 控制中等屏幕的最小宽度。 | `--ant-screen-md-min` | `number` |
| `screenSM` | 控制小屏幕的屏幕宽度。 | `--ant-screen-sm` | `number` |
| `screenSMMax` | 控制小屏幕的最大宽度。 | `--ant-screen-sm-max` | `number` |
| `screenSMMin` | 控制小屏幕的最小宽度。 | `--ant-screen-sm-min` | `number` |
| `screenXL` | 控制超大屏幕的屏幕宽度。 | `--ant-screen-xl` | `number` |
| `screenXLMax` | 控制超大屏幕的最大宽度。 | `--ant-screen-xl-max` | `number` |
| `screenXLMin` | 控制超大屏幕的最小宽度。 | `--ant-screen-xl-min` | `number` |
| `screenXS` | 控制超小屏幕的屏幕宽度。 | `--ant-screen-xs` | `number` |
| `screenXSMax` | 控制超小屏幕的最大宽度。 | `--ant-screen-xs-max` | `number` |
| `screenXSMin` | 控制超小屏幕的最小宽度。 | `--ant-screen-xs-min` | `number` |
| `screenXXL` | 控制超超大屏幕的屏幕宽度。 | `--ant-screen-xxl` | `number` |
| `screenXXLMax` | 控制超超大屏幕的最大宽度。 | `--ant-screen-xxl-max` | `number` |
| `screenXXLMin` | 控制超超大屏幕的最小宽度。 | `--ant-screen-xxl-min` | `number` |
| `screenXXXL` | 控制超超超大屏幕的屏幕宽度。 | `--ant-screen-xxxl` | `number` |
| `screenXXXLMin` | 控制超超超大屏幕的最小宽度。 | `--ant-screen-xxxl-min` | `number` |
<!-- END GENERATED GLOBAL TOKEN REFERENCE -->
