---
category: Components
type: Data Display
title: Card
cols: 1
cover: https://gw.alipayobjects.com/zos/antfincdn/NqXt8DJhky/Card.svg
---

Simple rectangular container.

## When To Use

- A card can be used to display content related to a single subject. The content can consist of multiple elements of varying types and sizes.


## API


Card

| Property             | Description                                         | Type          | Default    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Actions |The action list, shows at the bottom of the Card.   | Array(RenderFragment) |-        |
| ActionTemplate | The template placeholder for CardAction | RenderFragment | -
| Body |Body area on card | RenderFragment |-        |
| Extra |Content to render in the top-right corner of the card | RenderFragment |-        |
| Bordered |Toggles rendering of the border around the card | boolean |-        |
| BodyStyle |Inline style to apply to the card content | Css Properties |-        |
| CardAction | A component for actions, and it should be put in ActionTemplate | CardAction | -
| Cover |Card cover | RenderFragment |-        |
| Loading |Shows a loading indicator while the contents of the card are being fetched | boolean |-        |
| Size |	Size of card | RenderFragment |-        |
| Title |	Card title | String or RenderFragement |-        |
| Type |Card style type, can be set to inner or not set | string |-        |

Card.Grid

| Property             | Description                                         | Type          | Default    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| ChildContent | Child container | RenderFragment |-        |
| Hoverable | Lift up when hovering card grid | boolean |-        |
| Style | style object of container | CSS Properties |-        |

Card.Meta

| Property             | Description                                         | Type          | Default    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Avatar | 	avatar or icon | RenderFragment |-        |
| ChildContent | Child  container | RenderFragment |-        |
| Description | description content | boolean |-        |
| Style | style object of container| CSS Properties |-        |
| Title |	title content | String or RenderFragement |-        |
