---
category: Components
type: Data Entry
title: Slider
cover: https://gw.alipayobjects.com/zos/alicdn/HZ3meFc6W/Silder.svg
---

A Slider component for displaying current value and intervals in range.

## When To Use

- To input a value in a range.



## API

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| defaultValue            | The default value of slider. When range is false, use number, otherwise, use [number, number]      | number         |
| disabled            | If true, the slider will not be interactable.         | boolean         |-       |
| dots |	Whether the thumb can drag over tick only.   | boolean        | -         |
| included | Make effect when marks not null, true means containment and false means coordinative                         | boolean        | -         |
| marks | Tick mark of Slider, type of key must be number, and must in closed interval [min, max], each mark can declare its own style.      | object         |
| max | The maximum value the slider can slide to                        | number        | -         |
| min |The minimum value the slider can slide to.                     | number       | -         |
| range |dual thumb mode                        | boolean        | -         |
| reverse | reverse the component                         | boolean       | -         |
| step | The granularity the slider can step through values. Must greater than 0, and be divided by (max - min) . When marks no null, step can be null.   | number        | -         |
| value | The value of slider. When range is false, use number, otherwise, use [number, number]         | number        | -         |
| vertical | If true, the slider will be vertical.                   | boolean        | -         |
| onAfterChange |Fire when onmouseup is fired.                        | function(e)        | -         |
| onChange |Callback function that is fired when the user changes the slider's value.                          | function(e)        | -         |
| tooltipPlacement | 	Set Tooltip display position. Ref Tooltip.                          | string        | -         |
| tooltipVisible |If true, Tooltip will show always, or it will not show anyway, even if dragging or hovering.                           | boolean        | -         |
| getTooltipPopupContainer |The DOM container of the Tooltip, the default behavior is to create a div element in body.                         | Rendfragment        | -         |
