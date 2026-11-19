---
category: Components
type: Layout
title: Masonry
cover: https://mdn.alipayobjects.com/huamei_iwk9zp/afts/img/A*cELTRrM5HpAAAAAAOGAAAAgAegCCAQ/original
coverDark: https://mdn.alipayobjects.com/huamei_iwk9zp/afts/img/A*2CxJRYJmfbIAAAAAPqAAAAgAegCCAQ/original
cols: 1
tag: 6.0.0
---

A masonry layout component for displaying content with different heights.

## When To Use

- When displaying images or cards with irregular heights
- When content needs to be evenly distributed in columns
- When column count needs to be responsive

## Examples

## API

### Masonry

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ClassNames | Customize class for each semantic structure inside the component. Supports object or function. | Record<[SemanticDOM](#semantic-dom), string> \| (info: { props })=> Record<[SemanticDOM](#semantic-dom), string> | - | 6.0.0 |
| Columns | Number of columns, can be a fixed value or a responsive configuration | `number \| { xs?: number; sm?: number; md?: number }` | `3` |  |
| Fresh | Whether to continuously monitor the size changes of child items | `boolean` | `false` |  |
| Gutter | Spacing, can be a fixed value, responsive configuration, or a configuration for horizontal and vertical spacing | [Gap](#gap) \| \[[Gap](#gap), [Gap](#gap)\] | `0` |  |
| Items | Masonry items | [MasonryItem](#masonryitem)[] | - |  |
| ItemRender | Custom item rendering function | `(item: MasonryItem) => React.ReactNode` | - |  |
| Styles | Customize inline style for each semantic structure inside the component. Supports object or function. | Record<[SemanticDOM](#semantic-dom), CSSProperties> \| (info: { props })=> Record<[SemanticDOM](#semantic-dom), CSSProperties> | - | 6.0.0 |
| OnLayoutChange | Callback for column sorting changes | `({ key: React.Key; column: number }[]) => void` | - |  |

### MasonryItem

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| ChildContent | Custom display content, takes precedence over `itemRender` | `React.ReactNode` | - |
| Column | Specifies the column to which the item belongs | `number` | - |
| Data | Custom data storage | `T` | - |
| Height | Height of the item | `number` | - |
| Key | Unique identifier for the item | `string` \| `number` | - |

### Gap

`Gap` represents the spacing between items. It can either be a fixed value or a responsive configuration.

```ts
type Gap = undefined | number | Partial<Record<'xs' | 'sm' | 'md' | 'lg' | 'xl' | 'xxl', number>>;
```

## Semantic DOM

| Semantic DOM | Description |
| --- | --- |
| root | Root element, sets relative positioning, flex layout and masonry container styles |
| item | Item element, sets absolute positioning, width calculation, transition animation and masonry item styles |

## Design Token

Masonry has no component-specific design tokens.
