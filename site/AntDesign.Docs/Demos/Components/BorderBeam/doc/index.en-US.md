---
category: Components
type: Other
title: BorderBeam
description: Decorative component that renders a moving beam along a container border.
cover: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*uae3QbkNCm8AAAAAAAAAAAAADrJ8AQ/original
coverDark: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*VcjGQLSrYdcAAAAAAAAAAAAADrJ8AQ/original
cols: 2
---

## When To Use

- Use when a container needs stronger visual emphasis without introducing business state semantics.
- Suitable for login panels, recommendation cards, AI modules, and key CTA blocks.
- As a decorative effect, it should not replace focus rings, validation borders, or status feedback.

## API

### BorderBeam

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| ChildContent | Decorated content | `RenderFragment` | - |
| Color | Beam color configuration: a solid color or gradient stops | `OneOf<string, BorderBeamGradient[]>` | - |
| Count | Number of beams | `double` | 1 |
| Duration | Time in seconds for a beam to complete one loop | `double` | 6 |
| LineWidth | Beam line width. Numbers are treated as pixels | `object` | `1px` |
| Outset | Outset distance of the beam layer from the container edge | `object` | - |
| Size | Size of the visible beam segment. Numbers are treated as pixels | `object` | `100px` |

## FAQ

### How does BorderBeam behave when reduced motion is enabled?

When `prefers-reduced-motion: reduce` is active, the beam effect is hidden.

### What does `Percent` mean in `Color`?

`Percent` is the authored gradient-stop position in the `0 ~ 100` range. BorderBeam maps the stops into the visible beam segment and reserves trailing space for transparent fade-out.

### Why is BorderBeam not working?

BorderBeam inserts the beam layer into the first rendered child. Make sure the child resolves to an actual HTML container and provides a positioning context, usually with `position: relative`.

### How do I keep the beam radius aligned with my container?

The host element's computed `border-radius` is read during initialization. For non-uniform corners, set a multi-value radius such as `20px 20px 0 0` on the host and use `Outset="0"`.
