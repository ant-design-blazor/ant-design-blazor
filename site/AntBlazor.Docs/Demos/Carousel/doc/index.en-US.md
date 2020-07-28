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
| Autoplay   | Whether to scroll automatically | TimeSpan         |
| BeforeChange    | (ToDo)Callback function called before the current index changes | function(from, to)         |-       |
| DotPosition |The position of the dots, which can be one of `top` `bottom` `left` `right` | string  | -  |
| Dots | (ToDo)Whether to show the dots at the bottom of the gallery, `object` for `dotsClass` and any others | string | -  |
| Easing | (ToDo)Transition interpolation function name   | string        | -         |
| Effect | Transition effect `scrollx` or `fade` | string        | -         |