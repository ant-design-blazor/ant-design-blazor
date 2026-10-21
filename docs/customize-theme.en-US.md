---
order: 7
title: Customize Theme
---

Ant Design Blazor v6 uses the Ant Design v6 Design Token system. Customize the application theme with global tokens, choose a built-in algorithm, or override tokens for one component. These settings are scoped by `ConfigProvider`; they do not require recompiling Less.

## Basic usage

Set global Seed, Map, or Alias tokens through `ThemeConfig.Token`. Token names use the React v6 camelCase names. Values are strings so colors, CSS lengths, numbers, and booleans can all be represented.

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
    <Button Type="ButtonType.Primary">Themed action</Button>
    <Input Placeholder="Themed input" />
</ConfigProvider>
```

`ThemeConfig.Token` also provides generated typed shortcuts, such as `Token.ColorPrimary`; the string indexer is convenient for object initializers and dynamically supplied token names. Global token properties and their source interfaces are generated from the checked-out Ant Design v6 source into `components/theming/GeneratedThemeTokens.cs` and `components/theming/ThemeTokenProperties.cs`. The generated component-token contract is in `components/theming/GeneratedComponentTokens.cs`.

## Preset algorithms

The default algorithm derives Map and Alias tokens from the supplied Seed tokens. `dark` and `compact` are also available. Global explicit tokens are applied after the algorithm, so a supplied value takes precedence over its derived value.

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

The available algorithms are also listed in `GeneratedThemeTokens.cs`. Ant Design Blazor currently selects one algorithm per `ThemeConfig` (`default`, `dark`, or `compact`); React's algorithm-composition array is not exposed as a Blazor API.

## Component tokens

Use `SetComponentToken` to scope design changes to a single component. Component token types and CSS variable names are generated from React's v6 style token sources.

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
    <Select TItemValue="string" TItem="string" Placeholder="Choose a value">
        <SelectOptions>
            <SelectOption TItemValue="string" TItem="string" Value='@("one")' Label="One" />
            <SelectOption TItemValue="string" TItem="string" Value='@("two")' Label="Two" />
        </SelectOptions>
    </Select>
</ConfigProvider>
```

Every component page's Design Token section documents that component's available tokens. Components without dedicated tokens inherit global tokens and state so explicitly when applicable.

## Nested themes and CSS variables

Nested `ConfigProvider` instances create a local theme scope. Unspecified global values inherit from the parent; overrides only affect descendants of that provider. By default `CssVar` is enabled, and the provider emits resolved tokens as `--ant-*` CSS custom properties on its scope element. Component tokens use names such as `--ant-select-option-selected-bg`.

```razor
<ConfigProvider ThemeConfig="@BrandTheme">
    <Button Type="ButtonType.Primary">Brand theme</Button>

    <ConfigProvider ThemeConfig="@DarkTheme">
        <Card>Locally themed area</Card>
    </ConfigProvider>
</ConfigProvider>
```

For a CSS-only adjustment, target the provider scope instead of `:root` so the override does not unintentionally affect other themed regions:

```css
.ant-theme {
  --ant-color-primary: #722ed1;
}
```

Prefer `ThemeConfig` for tokens that participate in derivation (for example `colorPrimary`); CSS overrides do not recalculate dependent tokens. Set `CssVar = false` only when you intentionally do not want the provider to emit runtime variables.

## Token model

Ant Design v6 organizes global tokens by derivation: Seed tokens express design intent, the selected algorithm derives Map tokens, and Alias tokens provide names convenient for component styles. A global override may replace a derived token, but changing a Seed token is preferred when related colors, dimensions, or states should remain coherent. Component tokens form a separate, component-scoped layer.

The full current C# surface is generated from `components/theme/interface/seeds.ts`, `components/theme/interface/maps/`, and `components/theme/interface/alias.ts` in the Ant Design repository by `generate-csharp-theme-tokens`. The repository's synchronization report validates that Seed/Alias fields and typed shortcuts remain present; the map token groups are available through `ThemeConfig.Map` and the common token indexer.

## Migration note

Older examples based on v4 Less variables (`@primary-color`, `modifyVars`, or `ant-design-blazor.variable.css`) do not describe the v6 token workflow. The v6 styles use `--ant-*` variables and `ThemeConfig`; component styles are bundled with the package. Use the generated token APIs above rather than relying on the old v4 variable names.

<!-- BEGIN GENERATED GLOBAL TOKEN REFERENCE -->
<h2 id="global-token-reference">Global Token Reference</h2>
Generated from Ant Design React v6 `components/version/token-meta.json`; 234 tokens in total. CSS custom property names follow the Blazor v6 variable format.

### Seed (foundation)

| Token | Description | CSS variable | Type |
| --- | --- | --- | --- |
| `blue` | Preset blue color | `--ant-blue` | `color` |
| `borderRadius` | Border radius of base components | `--ant-border-radius` | `number` |
| `colorBgBase` | Used to derive the base variable of the background color gradient. In v5, we added a layer of background color derivation algorithm to produce map token of background color. But PLEASE DO NOT USE this Seed Token directly in the code! | `--ant-color-bg-base` | `string` |
| `colorError` | Used to represent the visual elements of the operation failure, such as the error Button, error Result component, etc. | `--ant-color-error` | `string` |
| `colorInfo` | Used to represent the operation information of the Token sequence, such as Alert, Tag, Progress, and other components use these map tokens. | `--ant-color-info` | `string` |
| `colorLink` | Control the color of hyperlink. | `--ant-color-link` | `string` |
| `colorPrimary` | Brand color is one of the most direct visual elements to reflect the characteristics and communication of the product. After you have selected the brand color, we will automatically generate a complete color palette and assign it effective design semantics. | `--ant-color-primary` | `string` |
| `colorSuccess` | Used to represent the token sequence of operation success, such as Result, Progress and other components will use these map tokens. | `--ant-color-success` | `string` |
| `colorTextBase` | Used to derive the base variable of the text color gradient. In v5, we added a layer of text color derivation algorithm to produce gradient variables of text color gradient. But please do not use this Seed Token directly in the code! | `--ant-color-text-base` | `string` |
| `colorWarning` | Used to represent the warning map token, such as Notification, Alert, etc. Alert or Control component(like Input) will use these map tokens. | `--ant-color-warning` | `string` |
| `controlHeight` | The height of the basic controls such as buttons and input boxes in Ant Design | `--ant-control-height` | `number` |
| `cyan` | Preset cyan color | `--ant-cyan` | `color` |
| `focusOutline` | Used to configure whether components show a visible outline when focused | `--ant-focus-outline` | `boolean` |
| `fontFamily` | The font family of Ant Design prioritizes the default interface font of the system, and provides a set of alternative font libraries that are suitable for screen display to maintain the readability and readability of the font under different platforms and browsers, reflecting the friendly, stable and professional characteristics. | `--ant-font-family` | `string` |
| `fontFamilyCode` | Code font, used for code, pre and kbd elements in Typography | `--ant-font-family-code` | `string` |
| `fontSize` | The most widely used font size in the design system, from which the text gradient will be derived. | `--ant-font-size` | `number` |
| `geekblue` | Preset geekblue color | `--ant-geekblue` | `color` |
| `gold` | Preset gold color | `--ant-gold` | `color` |
| `green` | Preset green color | `--ant-green` | `color` |
| `lime` | Preset lime color | `--ant-lime` | `color` |
| `lineType` | Border style of base components | `--ant-line-type` | `string` |
| `lineWidth` | Border width of base components | `--ant-line-width` | `number` |
| `magenta` | Preset magenta color | `--ant-magenta` | `color` |
| `motion` | Used to configure the motion effect, when it is `false`, the motion is turned off | `--ant-motion` | `boolean` |
| `motionBase` | Animation Base Duration. | `--ant-motion-base` | `number` |
| `motionEaseInBack` | Preset motion curve. | `--ant-motion-ease-in-back` | `string` |
| `motionEaseInOut` | Preset motion curve. | `--ant-motion-ease-in-out` | `string` |
| `motionEaseInOutCirc` | Preset motion curve. | `--ant-motion-ease-in-out-circ` | `string` |
| `motionEaseInQuint` | Preset motion curve. | `--ant-motion-ease-in-quint` | `string` |
| `motionEaseOut` | Preset motion curve. | `--ant-motion-ease-out` | `string` |
| `motionEaseOutBack` | Preset motion curve. | `--ant-motion-ease-out-back` | `string` |
| `motionEaseOutCirc` | Preset motion curve. | `--ant-motion-ease-out-circ` | `string` |
| `motionEaseOutQuint` | Preset motion curve. | `--ant-motion-ease-out-quint` | `string` |
| `motionUnit` | The unit of animation duration change | `--ant-motion-unit` | `number` |
| `opacityImage` | Control image opacity | `--ant-opacity-image` | `number` |
| `orange` | Preset orange color | `--ant-orange` | `color` |
| `pink` | Preset pink color | `--ant-pink` | `color` |
| `purple` | Preset purple color | `--ant-purple` | `color` |
| `red` | Preset red color | `--ant-red` | `color` |
| `sizePopupArrow` | The size of the component arrow | `--ant-size-popup-arrow` | `number` |
| `sizeStep` | The base step of size change, the size step combined with the size change unit, can derive various size steps. By adjusting the step, you can get different layout modes, such as the size step of the compact mode of V5 is 2 | `--ant-size-step` | `number` |
| `sizeUnit` | The unit of size change, in Ant Design, our base unit is 4, which is more fine-grained control of the size step | `--ant-size-unit` | `number` |
| `volcano` | Preset volcano color | `--ant-volcano` | `color` |
| `wireframe` | Used to change the visual effect of the component to wireframe, if you need to use the V4 effect, you need to enable the configuration item | `--ant-wireframe` | `boolean` |
| `yellow` | Preset yellow color | `--ant-yellow` | `color` |
| `zIndexBase` | The base Z axis value of all components, which can be used to control the level of some floating components based on the Z axis value, such as BackTop, Affix, etc. | `--ant-z-index-base` | `number` |
| `zIndexPopupBase` | Base zIndex of component like FloatButton, Affix which can be cover by large popup | `--ant-z-index-popup-base` | `number` |

### Map (derived)

| Token | Description | CSS variable | Type |
| --- | --- | --- | --- |
| `borderRadiusLG` | LG size border radius, used in some large border radius components, such as Card, Modal and other components. | `--ant-border-radius-lg` | `number` |
| `borderRadiusOuter` | Outer border radius | `--ant-border-radius-outer` | `number` |
| `borderRadiusSM` | SM size border radius, used in small size components, such as Button, Input, Select and other input components in small size | `--ant-border-radius-sm` | `number` |
| `borderRadiusXS` | XS size border radius, used in some small border radius components, such as Segmented, Arrow and other components with small border radius. | `--ant-border-radius-xs` | `number` |
| `colorBgBlur` | Control the background color of frosted glass container, usually transparent. | `--ant-color-bg-blur` | `string` |
| `colorBgContainer` | Container background color, e.g: default button, input box, etc. Be sure not to confuse this with `colorBgElevated`. | `--ant-color-bg-container` | `string` |
| `colorBgElevated` | Container background color of the popup layer, in dark mode the color value of this token will be a little brighter than `colorBgContainer`. E.g: modal, pop-up, menu, etc. | `--ant-color-bg-elevated` | `string` |
| `colorBgLayout` | This color is used for the background color of the overall layout of the page. This token will only be used when it is necessary to be at the B1 visual level in the page. Other usages are wrong. | `--ant-color-bg-layout` | `string` |
| `colorBgMask` | The background color of the mask, used to cover the content below the mask, Modal, Drawer, Image and other components use this token | `--ant-color-bg-mask` | `string` |
| `colorBgSolid` | Solid background color, currently only used for the default solid button background color. | `--ant-color-bg-solid` | `string` |
| `colorBgSolidActive` | Solid background color active state, currently only used in the active effect of the default solid button. | `--ant-color-bg-solid-active` | `string` |
| `colorBgSolidHover` | Solid background color hover state, currently only used in the hover effect of the default solid button. | `--ant-color-bg-solid-hover` | `string` |
| `colorBgSpotlight` | This color is used to draw the user's strong attention to the background color, and is currently only used in the background color of Tooltip. | `--ant-color-bg-spotlight` | `string` |
| `colorBorder` | Default border color, used to separate different elements, such as: form separator, card separator, etc. | `--ant-color-border` | `string` |
| `colorBorderDisabled` | Control the border color of the element in the disabled state. | `--ant-color-border-disabled` | `string` |
| `colorBorderSecondary` | Slightly lighter than the default border color, this color is the same as `colorSplit`. Solid color is used. | `--ant-color-border-secondary` | `string` |
| `colorErrorActive` | The active state of the error color. | `--ant-color-error-active` | `string` |
| `colorErrorBg` | The background color of the error state. | `--ant-color-error-bg` | `string` |
| `colorErrorBgActive` | The active state background color of the error state. | `--ant-color-error-bg-active` | `string` |
| `colorErrorBgFilledHover` | The wrong color fills the background color of the suspension state, which is currently only used in the hover effect of the dangerous filled button. | `--ant-color-error-bg-filled-hover` | `string` |
| `colorErrorBgHover` | The hover state background color of the error state. | `--ant-color-error-bg-hover` | `string` |
| `colorErrorBorder` | The border color of the error state. | `--ant-color-error-border` | `string` |
| `colorErrorBorderHover` | The hover state border color of the error state. | `--ant-color-error-border-hover` | `string` |
| `colorErrorHover` | The hover state of the error color. | `--ant-color-error-hover` | `string` |
| `colorErrorText` | The default state of the text in the error color. | `--ant-color-error-text` | `string` |
| `colorErrorTextActive` | The active state of the text in the error color. | `--ant-color-error-text-active` | `string` |
| `colorErrorTextHover` | The hover state of the text in the error color. | `--ant-color-error-text-hover` | `string` |
| `colorFill` | The darkest fill color is used to distinguish between the second and third level of fill color, and is currently only used in the hover effect of Slider. | `--ant-color-fill` | `string` |
| `colorFillQuaternary` | The weakest level of fill color is suitable for color blocks that are not easy to attract attention, such as zebra stripes, color blocks that distinguish boundaries, etc. | `--ant-color-fill-quaternary` | `string` |
| `colorFillSecondary` | The second level of fill color can outline the shape of the element more clearly, such as Rate, Skeleton, etc. It can also be used as the Hover state of the third level of fill color, such as Table, etc. | `--ant-color-fill-secondary` | `string` |
| `colorFillTertiary` | The third level of fill color is used to outline the shape of the element, such as Slider, Segmented, etc. If there is no emphasis requirement, it is recommended to use the third level of fill color as the default fill color. | `--ant-color-fill-tertiary` | `string` |
| `colorInfoActive` | Active state of dark color of information color. | `--ant-color-info-active` | `string` |
| `colorInfoBg` | Light background color of information color. | `--ant-color-info-bg` | `string` |
| `colorInfoBgHover` | Hover state of light background color of information color. | `--ant-color-info-bg-hover` | `string` |
| `colorInfoBorder` | Border color of information color. | `--ant-color-info-border` | `string` |
| `colorInfoBorderHover` | Hover state of border color of information color. | `--ant-color-info-border-hover` | `string` |
| `colorInfoHover` | Hover state of dark color of information color. | `--ant-color-info-hover` | `string` |
| `colorInfoText` | Default state of text color of information color. | `--ant-color-info-text` | `string` |
| `colorInfoTextActive` | Active state of text color of information color. | `--ant-color-info-text-active` | `string` |
| `colorInfoTextHover` | Hover state of text color of information color. | `--ant-color-info-text-hover` | `string` |
| `colorLinkActive` | Control the color of hyperlink when clicked. | `--ant-color-link-active` | `string` |
| `colorLinkHover` | Control the color of hyperlink when hovering. | `--ant-color-link-hover` | `string` |
| `colorPrimaryActive` | Dark active state under the main color gradient. | `--ant-color-primary-active` | `string` |
| `colorPrimaryBg` | Light background color of primary color, usually used for weak visual level selection state. | `--ant-color-primary-bg` | `string` |
| `colorPrimaryBgHover` | The hover state color corresponding to the light background color of the primary color. | `--ant-color-primary-bg-hover` | `string` |
| `colorPrimaryBorder` | The stroke color under the main color gradient, used on the stroke of components such as Slider. | `--ant-color-primary-border` | `string` |
| `colorPrimaryBorderHover` | The hover state of the stroke color under the main color gradient, which will be used when the stroke Hover of components such as Slider and Button. | `--ant-color-primary-border-hover` | `string` |
| `colorPrimaryHover` | Hover state under the main color gradient. | `--ant-color-primary-hover` | `string` |
| `colorPrimaryText` | Text color under the main color gradient. | `--ant-color-primary-text` | `string` |
| `colorPrimaryTextActive` | Active state of text color under the main color gradient. | `--ant-color-primary-text-active` | `string` |
| `colorPrimaryTextHover` | Hover state of text color under the main color gradient. | `--ant-color-primary-text-hover` | `string` |
| `colorSuccessActive` | Active state color of dark success color | `--ant-color-success-active` | `string` |
| `colorSuccessBg` | Light background color of success color, used for Tag and Alert success state background color | `--ant-color-success-bg` | `string` |
| `colorSuccessBgHover` | Light background color of success color, but antd does not use this token currently | `--ant-color-success-bg-hover` | `string` |
| `colorSuccessBorder` | Border color of success color, used for Tag and Alert success state border color | `--ant-color-success-border` | `string` |
| `colorSuccessBorderHover` | Hover state color of success color border | `--ant-color-success-border-hover` | `string` |
| `colorSuccessHover` | Hover state color of dark success color | `--ant-color-success-hover` | `string` |
| `colorSuccessText` | Default state color of success color text | `--ant-color-success-text` | `string` |
| `colorSuccessTextActive` | Active state color of success color text | `--ant-color-success-text-active` | `string` |
| `colorSuccessTextHover` | Hover state color of success color text | `--ant-color-success-text-hover` | `string` |
| `colorText` | Default text color which comply with W3C standards, and this color is also the darkest neutral color. | `--ant-color-text` | `string` |
| `colorTextQuaternary` | The fourth level of text color is the lightest text color, such as form input prompt text, disabled color text, etc. | `--ant-color-text-quaternary` | `string` |
| `colorTextSecondary` | The second level of text color is generally used in scenarios where text color is not emphasized, such as label text, menu text selection state, etc. | `--ant-color-text-secondary` | `string` |
| `colorTextTertiary` | The third level of text color is generally used for descriptive text, such as form supplementary explanation text, list descriptive text, etc. | `--ant-color-text-tertiary` | `string` |
| `colorWarningActive` | The active state of the warning color. | `--ant-color-warning-active` | `string` |
| `colorWarningBg` | The background color of the warning state. | `--ant-color-warning-bg` | `string` |
| `colorWarningBgHover` | The hover state background color of the warning state. | `--ant-color-warning-bg-hover` | `string` |
| `colorWarningBorder` | The border color of the warning state. | `--ant-color-warning-border` | `string` |
| `colorWarningBorderHover` | The hover state border color of the warning state. | `--ant-color-warning-border-hover` | `string` |
| `colorWarningHover` | The hover state of the warning color. | `--ant-color-warning-hover` | `string` |
| `colorWarningText` | The default state of the text in the warning color. | `--ant-color-warning-text` | `string` |
| `colorWarningTextActive` | The active state of the text in the warning color. | `--ant-color-warning-text-active` | `string` |
| `colorWarningTextHover` | The hover state of the text in the warning color. | `--ant-color-warning-text-hover` | `string` |
| `colorWhite` | Pure white color don't changed by theme | `--ant-color-white` | `string` |
| `controlHeightLG` | LG component height | `--ant-control-height-lg` | `number` |
| `controlHeightSM` | SM component height | `--ant-control-height-sm` | `number` |
| `controlHeightXS` | XS component height | `--ant-control-height-xs` | `number` |
| `fontSizeHeading1` | Font size of h1 tag. | `--ant-font-size-heading1` | `string \| number` |
| `fontSizeHeading2` | Font size of h2 tag. | `--ant-font-size-heading2` | `string \| number` |
| `fontSizeHeading3` | Font size of h3 tag. | `--ant-font-size-heading3` | `string \| number` |
| `fontSizeHeading4` | Font size of h4 tag. | `--ant-font-size-heading4` | `string \| number` |
| `fontSizeHeading5` | Font size of h5 tag. | `--ant-font-size-heading5` | `string \| number` |
| `fontSizeLG` | Large font size | `--ant-font-size-lg` | `number` |
| `fontSizeSM` | Small font size | `--ant-font-size-sm` | `number` |
| `fontSizeXL` | Super large font size | `--ant-font-size-xl` | `number` |
| `lineHeight` | Line height of text. | `--ant-line-height` | `number` |
| `lineHeightHeading1` | Line height of h1 tag. | `--ant-line-height-heading1` | `number` |
| `lineHeightHeading2` | Line height of h2 tag. | `--ant-line-height-heading2` | `number` |
| `lineHeightHeading3` | Line height of h3 tag. | `--ant-line-height-heading3` | `number` |
| `lineHeightHeading4` | Line height of h4 tag. | `--ant-line-height-heading4` | `number` |
| `lineHeightHeading5` | Line height of h5 tag. | `--ant-line-height-heading5` | `number` |
| `lineHeightLG` | Line height of large text. | `--ant-line-height-lg` | `number` |
| `lineHeightSM` | Line height of small text. | `--ant-line-height-sm` | `number` |
| `lineWidthBold` | The default line width of the outline class components, such as Button, Input, Select, etc. | `--ant-line-width-bold` | `number` |
| `motionDurationFast` | Motion speed, fast speed. Used for small element animation interaction. | `--ant-motion-duration-fast` | `string` |
| `motionDurationMid` | Motion speed, medium speed. Used for medium element animation interaction. | `--ant-motion-duration-mid` | `string` |
| `motionDurationSlow` | Motion speed, slow speed. Used for large element animation interaction. | `--ant-motion-duration-slow` | `string` |
| `size` | Medium size | `--ant-size` | `number` |
| `sizeLG` | Large size | `--ant-size-lg` | `number` |
| `sizeMD` | Medium-large size | `--ant-size-md` | `number` |
| `sizeMS` | Same as `size`, but could be larger in compact mode | `--ant-size-ms` | `number` |
| `sizeSM` | Medium-small size | `--ant-size-sm` | `number` |
| `sizeXL` | Extra-large size | `--ant-size-xl` | `number` |
| `sizeXS` | Small size | `--ant-size-xs` | `number` |
| `sizeXXL` | Largest size | `--ant-size-xxl` | `number` |
| `sizeXXS` | Smallest size | `--ant-size-xxs` | `number` |

### Alias (semantic aliases)

| Token | Description | CSS variable | Type |
| --- | --- | --- | --- |
| `boxShadow` | Control the box shadow style of an element. | `--ant-box-shadow` | `string` |
| `boxShadowSecondary` | Control the secondary box shadow style of an element. | `--ant-box-shadow-secondary` | `string` |
| `boxShadowTertiary` | Control the tertiary box shadow style of an element. | `--ant-box-shadow-tertiary` | `string` |
| `colorBgContainerDisabled` | Control the background color of container in disabled state. | `--ant-color-bg-container-disabled` | `string` |
| `colorBgTextActive` | Control the background color of text in active state. | `--ant-color-bg-text-active` | `string` |
| `colorBgTextHover` | Control the background color of text in hover state. | `--ant-color-bg-text-hover` | `string` |
| `colorBorderBg` | Control the color of background border of element. | `--ant-color-border-bg` | `string` |
| `colorErrorAffix` | Control the color of form control prefix/suffix in error state. | `--ant-color-error-affix` | `string` |
| `colorErrorOutline` | Control the outline color of input component in error state. | `--ant-color-error-outline` | `string` |
| `colorFillAlter` | Control the alternative background color of element. | `--ant-color-fill-alter` | `string` |
| `colorFillContent` | Control the background color of content area. | `--ant-color-fill-content` | `string` |
| `colorFillContentHover` | Control the style of background color of content area when mouse hovers over it. | `--ant-color-fill-content-hover` | `string` |
| `colorHighlight` | Control the color of page element when highlighted. | `--ant-color-highlight` | `string` |
| `colorIcon` | Weak action. Such as `allowClear` or Alert close button | `--ant-color-icon` | `string` |
| `colorIconHover` | Weak action hover color. Such as `allowClear` or Alert close button | `--ant-color-icon-hover` | `string` |
| `colorSplit` | Used as the color of separator, this color is the same as colorBorderSecondary but with transparency. | `--ant-color-split` | `string` |
| `colorTextDescription` | Control the font color of text description. | `--ant-color-text-description` | `string` |
| `colorTextDisabled` | Control the color of text in disabled state. | `--ant-color-text-disabled` | `string` |
| `colorTextHeading` | Control the font color of heading. | `--ant-color-text-heading` | `string` |
| `colorTextLabel` | Control the font color of text label. | `--ant-color-text-label` | `string` |
| `colorTextLightSolid` | Control the highlight color of text with background color, such as the text in Primary Button components. | `--ant-color-text-light-solid` | `string` |
| `colorTextPlaceholder` | Control the color of placeholder text. | `--ant-color-text-placeholder` | `string` |
| `colorWarningAffix` | Control the color of form control prefix/suffix in warning state. | `--ant-color-warning-affix` | `string` |
| `colorWarningOutline` | Control the outline color of input component in warning state. | `--ant-color-warning-outline` | `string` |
| `controlInteractiveSize` | Control the interactive size of control component. | `--ant-control-interactive-size` | `number` |
| `controlItemBgActive` | Control the background color of control component item when active. | `--ant-control-item-bg-active` | `string` |
| `controlItemBgActiveDisabled` | Control the background color of control component item when active and disabled. | `--ant-control-item-bg-active-disabled` | `string` |
| `controlItemBgActiveHover` | Control the background color of control component item when hovering and active. | `--ant-control-item-bg-active-hover` | `string` |
| `controlItemBgHover` | Control the background color of control component item when hovering. | `--ant-control-item-bg-hover` | `string` |
| `controlOutline` | Control the outline color of input component. | `--ant-control-outline` | `string` |
| `controlOutlineWidth` | Control the outline width of input component. | `--ant-control-outline-width` | `number` |
| `controlPaddingHorizontal` | Control the horizontal padding of an element. | `--ant-control-padding-horizontal` | `number` |
| `controlPaddingHorizontalSM` | Control the horizontal padding of an element with a small-medium size. | `--ant-control-padding-horizontal-sm` | `number` |
| `fontSizeIcon` | Control the font size of operation icon in Select, Cascader, etc. Normally same as fontSizeSM. | `--ant-font-size-icon` | `number` |
| `fontWeightStrong` | Control the font weight of heading components (such as h1, h2, h3) or selected item. | `--ant-font-weight-strong` | `number` |
| `lineWidthFocus` | Control the width of the line when the component is in focus state. | `--ant-line-width-focus` | `number` |
| `linkDecoration` | Control the text decoration style of a link. | `--ant-link-decoration` | `TextDecoration<string \| number> \| undefined` |
| `linkFocusDecoration` | Control the text decoration style of a link on focus. | `--ant-link-focus-decoration` | `TextDecoration<string \| number> \| undefined` |
| `linkHoverDecoration` | Control the text decoration style of a link on mouse hover. | `--ant-link-hover-decoration` | `TextDecoration<string \| number> \| undefined` |
| `margin` | Control the margin of an element, with a medium size. | `--ant-margin` | `number` |
| `marginLG` | Control the margin of an element, with a large size. | `--ant-margin-lg` | `number` |
| `marginMD` | Control the margin of an element, with a medium-large size. | `--ant-margin-md` | `number` |
| `marginSM` | Control the margin of an element, with a medium-small size. | `--ant-margin-sm` | `number` |
| `marginXL` | Control the margin of an element, with an extra-large size. | `--ant-margin-xl` | `number` |
| `marginXS` | Control the margin of an element, with a small size. | `--ant-margin-xs` | `number` |
| `marginXXL` | Control the margin of an element, with the largest size. | `--ant-margin-xxl` | `number` |
| `marginXXS` | Control the margin of an element, with the smallest size. | `--ant-margin-xxs` | `number` |
| `opacityLoading` | Control the opacity of the loading state. | `--ant-opacity-loading` | `number` |
| `padding` | Control the padding of the element. | `--ant-padding` | `number` |
| `paddingContentHorizontal` | Control the horizontal padding of content element. | `--ant-padding-content-horizontal` | `number` |
| `paddingContentHorizontalLG` | Control the horizontal padding of content element, suitable for large screen devices. | `--ant-padding-content-horizontal-lg` | `number` |
| `paddingContentHorizontalSM` | Control the horizontal padding of content element, suitable for small screen devices. | `--ant-padding-content-horizontal-sm` | `number` |
| `paddingContentVertical` | Control the vertical padding of content element. | `--ant-padding-content-vertical` | `number` |
| `paddingContentVerticalLG` | Control the vertical padding of content element, suitable for large screen devices. | `--ant-padding-content-vertical-lg` | `number` |
| `paddingContentVerticalSM` | Control the vertical padding of content element, suitable for small screen devices. | `--ant-padding-content-vertical-sm` | `number` |
| `paddingLG` | Control the large padding of the element. | `--ant-padding-lg` | `number` |
| `paddingMD` | Control the medium padding of the element. | `--ant-padding-md` | `number` |
| `paddingSM` | Control the small padding of the element. | `--ant-padding-sm` | `number` |
| `paddingXL` | Control the extra large padding of the element. | `--ant-padding-xl` | `number` |
| `paddingXS` | Control the extra small padding of the element. | `--ant-padding-xs` | `number` |
| `paddingXXS` | Control the extra extra small padding of the element. | `--ant-padding-xxs` | `number` |
| `screenLG` | Control the screen width of large screens. | `--ant-screen-lg` | `number` |
| `screenLGMax` | Control the maximum width of large screens. | `--ant-screen-lg-max` | `number` |
| `screenLGMin` | Control the minimum width of large screens. | `--ant-screen-lg-min` | `number` |
| `screenMD` | Control the screen width of medium screens. | `--ant-screen-md` | `number` |
| `screenMDMax` | Control the maximum width of medium screens. | `--ant-screen-md-max` | `number` |
| `screenMDMin` | Control the minimum width of medium screens. | `--ant-screen-md-min` | `number` |
| `screenSM` | Control the screen width of small screens. | `--ant-screen-sm` | `number` |
| `screenSMMax` | Control the maximum width of small screens. | `--ant-screen-sm-max` | `number` |
| `screenSMMin` | Control the minimum width of small screens. | `--ant-screen-sm-min` | `number` |
| `screenXL` | Control the screen width of extra large screens. | `--ant-screen-xl` | `number` |
| `screenXLMax` | Control the maximum width of extra large screens. | `--ant-screen-xl-max` | `number` |
| `screenXLMin` | Control the minimum width of extra large screens. | `--ant-screen-xl-min` | `number` |
| `screenXS` | Control the screen width of extra small screens. | `--ant-screen-xs` | `number` |
| `screenXSMax` | Control the maximum width of extra small screens. | `--ant-screen-xs-max` | `number` |
| `screenXSMin` | Control the minimum width of extra small screens. | `--ant-screen-xs-min` | `number` |
| `screenXXL` | Control the screen width of extra extra large screens. | `--ant-screen-xxl` | `number` |
| `screenXXLMax` | Control the maximum width of extra extra large screens. | `--ant-screen-xxl-max` | `number` |
| `screenXXLMin` | Control the minimum width of extra extra large screens. | `--ant-screen-xxl-min` | `number` |
| `screenXXXL` | Control the screen width of XXXL screens. | `--ant-screen-xxxl` | `number` |
| `screenXXXLMin` | Control the minimum width of XXXL screens. | `--ant-screen-xxxl-min` | `number` |
<!-- END GENERATED GLOBAL TOKEN REFERENCE -->
