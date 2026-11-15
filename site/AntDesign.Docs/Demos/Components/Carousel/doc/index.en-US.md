---
category: Components
type: Data Display
title: Carousel
cover: https://gw.alipayobjects.com/zos/antfincdn/%24C9tmj978R/Carousel.svg
---

A carousel component. Scales with its container.

## When To Use

- When there is a group of content on the same level.
- When there is insufficient content space, it can be used to save space in the form of a revolving door.
- Commonly used for a group of pictures/cards.


## API

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| AfterChange | (ToDo)Callback function called after the current index changes | function(current)        | -         |
| ChildContent | ChildContent | RenderFragment        | -         |
| Autoplay   | Whether to scroll automatically; pass `CarouselAutoplayOptions` to configure dot progress. A `TimeSpan` value is also accepted for compatibility with earlier Blazor versions. | bool or CarouselAutoplayOptions or TimeSpan | false |
| AutoplaySpeed | Autoplay interval in milliseconds | int | 3000 |
| Arrows | Whether to show switch arrows | bool | false |
| BeforeChange    | (ToDo)Callback function called before the current index changes | function(from, to)         |-       |
| DotPlacement | The placement of the dots: `top`, `bottom`, `start`, or `end` | CarouselDotPosition | bottom |
| DotPosition | Legacy alias for `DotPlacement` | CarouselDotPosition | bottom |
| DotDuration | Legacy convenience alias for `Autoplay.DotDuration` | bool | false |
| Dots | Whether to show the dots | bool | true |
| DotsClass | The class name of the dots | string        | -         |
| Easing | (ToDo)Transition interpolation function name   | string        | -         |
| Effect | Transition effect `scrollx` or `fade` | string        | -         |
| Infinite | Whether to infinitely wrap around at the start or end | bool | true |
