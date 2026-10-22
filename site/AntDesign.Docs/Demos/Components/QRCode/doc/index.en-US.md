---
category: Components
subtitle: QR Code
type: Data Display
title: QRCode
---

QRCode renders text as a QR code.

## API

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| Value | QR code content | string | - |
| Type | Rendering format: `canvas` or `svg` | string | `canvas` |
| Size | QR code size | int | 160 |
| Icon | URL of the central image | string | - |
| IconSize | Square size or separate image width and height, in pixels | OneOf\<int, QRCodeIconSize\> | 40 |
| ErrorLevel | Minimum error correction level: `L`, `M`, `Q`, or `H` | string | `M` |
| BoostLevel | Increase the error correction level when it fits in the same QR version | bool | true |
| MarginSize | Quiet-zone width in modules; zero removes the margin | int | 0 |
| Status | Current status | string | active |
| Color | Foreground CSS color | string | Theme text color |
| BackgroundColor | Background CSS color | string | `transparent` |
| StatusRender | Custom content rendered over a non-active QR code | RenderFragment | — |
| StatusTemplate | Custom status content with status, locale, and refresh callback | RenderFragment\<QRCodeStatusRenderInfo\> | - |
| Bordered | Whether to show a border | bool | true |
| OnRefresh | Called when the refresh button is clicked; providing it displays the button | EventCallback | - |
| Locale | Localized status text | QRCodeLocale | Current locale |
| ClassNames | Custom classes keyed by `root` and `cover` | IReadOnlyDictionary\<string, string\> | - |
| Styles | Custom inline styles keyed by `root` and `cover` | IReadOnlyDictionary\<string, string\> | - |

### Methods

`DownloadAsync(string fileName = null)` downloads the current Canvas as PNG or the current SVG as SVG. The default file names are `QRCode.png` and `QRCode.svg`. A custom `StatusTemplate` takes precedence over `StatusRender`; the QR code remains rendered beneath either template.
