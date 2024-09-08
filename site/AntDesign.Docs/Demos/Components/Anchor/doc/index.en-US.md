---
category: Components
type: Other
cols: 2
title: Anchor
cover: https://gw.alipayobjects.com/zos/alicdn/_1-C1JwsC/Anchor.svg
---

Hyperlinks to scroll on one page.

## When To Use

For displaying anchor hyperlinks on page and jumping between them.

## API

### Anchor Props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Affix | Fixed mode of Anchor | boolean | true |  |
| Bounds | Bounding distance of anchor area | number | 5(px) |  |
| GetContainer | Scrolling container | () => HTMLElement | () => window |  |
| OffsetBottom | Pixels to offset from bottom when calculating position of scroll | number | - |  |
| OffsetTop | Pixels to offset from top when calculating position of scroll | number | 0 |  |
| ShowInkInFixed | Whether show ink-balls in Fixed mode | boolean | false |  |
| OnClick | set the handler to handle `click` event | Function(e: Event, link: Object) | - |  |
| GetCurrentAnchor | Customize the anchor highlight | () => string | - |  |
| TargetOffset | Anchor scroll offset, default as `offsetTop`, [example](#components-anchor-demo-targetOffset) | number | `offsetTop` |  |
| OnChange | Listening for anchor link change | (currentActiveLink: string) => void |  |  |
| Key | used to refresh links list when the key changed | string |  |  |
| ChildContent | Additional Content | RenderFragment |  |  |

### Link Props

| Property | Description                               | Type              | Default | Version |
| -------- | ----------------------------------------- | ----------------- | ------- | ------- |
| Href     | target of hyperlink                       | string            |         |         |
| Title    | content of hyperlink                      | string\|ReactNode |         |         |
| Target   | Specifies where to display the linked URL | string            |         |         |
