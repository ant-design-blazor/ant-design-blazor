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
| DefaultValue            | The default value of slider. When range is false, use number, otherwise, use [number, number]      | number         |
| Disabled            | If true, the slider will not be interactable.         | boolean         |-       |
| Dots |	Whether the thumb can drag over tick only.   | boolean        | -         |
| Included | Make effect when marks not null, true means containment and false means coordinative                         | boolean        | -         |
| Marks | Tick mark of Slider, type of key must be number, and must in closed interval [min, max], each mark can declare its own style.      | object         |
| Max | The maximum value the slider can slide to                        | number        | -         |
| Min |The minimum value the slider can slide to.                     | number       | -         |
| Range |dual thumb mode                        | boolean        | -         |
| Reverse | reverse the component                         | boolean       | -         |
| Step | The granularity the slider can step through values. Must greater than 0, and be divided by (max - min) . When marks no null, step can be null.   | number        | -         |
| Value | The value of slider. When range is false, use number, otherwise, use [number, number]         | number        | -         |
| Vertical | If true, the slider will be vertical.                   | boolean        | -         |
| OnAfterChange |Fire when onmouseup is fired.                        | function(e)        | -         |
| OnChange |Callback function that is fired when the user changes the slider's value.                          | function(e)        | -         |
| HasTooltip |Will not render `Tooltip` if set to false | boolean | true |
| TipFormatter |Slider will pass its value to `TipFormatter`, and display its value in `Tooltip`. | Func<double, string> | (d) => d.ToString() |
| TooltipPlacement | 	Set `Tooltip` display position. Ref [`Tooltip`](/components/tooltip). | PlacementType | `PlacementType.Top` for horizontal slider & `PlacementType.Right` for vertical slider|
| TooltipVisible |If true, `Tooltip` will show always, or it will not show anyway, even if dragging or hovering.                           | boolean        | false        |
| GetTooltipPopupContainer |The DOM container of the Tooltip, the default behavior is to create a div element in body.                         | Rendfragment        | -         |
