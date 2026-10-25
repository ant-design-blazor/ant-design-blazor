# AntDesign.Colors

Shared C# color calculations for Ant Design Blazor and AntDesign.Icons. The
palette generator is ported from `@ant-design/colors` 8.0.1, using conversion,
mixing and rounding from `@ant-design/fast-color` 3.0.1 in the pinned React v6
checkout. Theme neutral/alias/shadow calculations use the same implementation.
There is no Blazor, Icons, JavaScript or System.Drawing dependency.

```csharp
using AntDesign.Colors;

var light = ColorPalette.Generate("#1677ff");
var dark = ColorPalette.Generate("#1677ff", dark: true, backgroundColor: "#141414");
var secondary = light[0];
```

`ThemeColorMath` provides the neutral/alias operations used by the theme compiler.
The Icon compatibility helpers delegate to this package instead of maintaining
an independent approximation of the first swatch. The palette reference script
in `tools/AntDesign.StyleGen/src/color-palette-reference.mjs` records actual
upstream results; `ColorPaletteParityTests` compares complete palettes and
color operations, and `ReactThemeAlgorithmParityTests` checks complete token maps.

Publishing order: publish `AntDesign.Colors` first, then update and publish
`AntDesign.Icons`, then update AntDesign's Icons dependency. Local prerelease
packages are currently used for development; they have not been published.
See `THIRD-PARTY-NOTICES.txt` for upstream licenses.
