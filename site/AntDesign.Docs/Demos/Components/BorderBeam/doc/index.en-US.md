---
category: Components
group: Other
type: Other
title: BorderBeam
description: Animated decorative beam along a container border.
---

## When To Use

Use BorderBeam to add a decorative moving highlight to a container, such as a feature card or an emphasized panel. It is decorative and should not replace focus, validation, or status indicators.

## Examples

<code src="./demo/Basic.razor">Basic</code>
<code src="./demo/Hover.razor">Show on hover</code>
<code src="./demo/Count.razor">Multiple beams</code>
<code src="./demo/CustomContainer.razor">Custom container</code>
<code src="./demo/CustomizedColor.razor">Gradients</code>
<code src="./demo/Duration.razor">Duration</code>
<code src="./demo/Size.razor">Size</code>
<code src="./demo/LineWidth.razor">Line width</code>
<code src="./demo/NonUniformRadius.razor" debug>Non-uniform radius</code>
<code src="./demo/ComponentToken.razor" debug>Component token</code>

## API

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| Count | Number of evenly spaced beams | int | 1 |
| Color | Beam color or gradient stops; the trailing segment fades to transparent | `OneOf<string, IReadOnlyList<BorderBeamGradientStop>>` | - |
| Duration | Seconds for one circuit | double? | 6 |
| LineWidth | Beam thickness, such as `2px` | string | 1px |
| Outset | Outward offset from the host border | string | computed from host border |
| Size | Length of the visible beam segment | string | 100px |

Place `<BorderBeam />` inside the positioned container that it decorates. The host needs a positioning context, usually `position: relative`.

```razor
<div style="position:relative; overflow:hidden; border:1px solid #eee; border-radius:12px; padding:24px">
    <BorderBeam />
    <h3>Feature panel</h3>
    <p>Decorative border highlight.</p>
</div>
```

## FAQ

### How does BorderBeam behave when reduced motion is enabled?

The decorative beam is hidden when the browser requests reduced motion.

### What does percent mean in color?

The Blazor `Color` parameter accepts a CSS color string or a list of `BorderBeamGradientStop` values. Each stop has a CSS `Color` and a `Percent` from 0 to 100; the beam fades to transparent after the last stop.

### Why is BorderBeam not working?

Place it inside the DOM container to decorate and give that container a positioning context, usually `position: relative`.
