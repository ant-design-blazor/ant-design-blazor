---
category: Components
subtitle: Color Picker
type: Data Entry
title: ColorPicker
---

ColorPicker lets users select a color and control the panel state.

## When to use

Use it when a color value needs to be selected or previewed.

## API

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| Value | Current color value | string | `#1677ff` |
| ValueChanged | Callback raised when the color changes; use `@bind-Value` for two-way binding | `EventCallback<string>` | — |
| ChildContent | Optional content used as a custom trigger instead of the built-in swatch | `RenderFragment` | — |
| Size | Trigger size: `small`, `middle`, or `large` | string | `middle` |
| Open | Whether the panel is open | bool | false |
| ShowText | Whether to show the color text | bool | false |
| Disabled | Whether the control is disabled | bool | false |
| AllowClear | Whether the selected color can be cleared | bool | false |
| Presets | Labeled color preset groups | `IReadOnlyList<ColorPickerPreset>` | empty |
| OnClear | Callback invoked when the color is cleared | `EventCallback` | — |
| Format | Color input format: `hex`, `rgb`, or `hsb` | string | `hex` |
| FormatChanged | Raised when the input format changes | `EventCallback<string>` | — |
| DisabledAlpha | Hide the opacity slider | bool | false |
| DisabledFormat | Hide the format selector | bool | false |
| AriaLabel | Accessible label for the color button | string | Choose color |
