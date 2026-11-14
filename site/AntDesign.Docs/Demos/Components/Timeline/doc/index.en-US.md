---
category: Components
type: Data Display
title: Timeline
cover: https://gw.alipayobjects.com/zos/antfincdn/vJmo00mmgR/Timeline.svg
---

Vertical display timeline.

## When To Use

- When a series of information needs to be ordered by time (ascending or descending).
- When you need a timeline to make a visual connection.


## API

Timeline

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| Pending | Set the last ghost node's existence or its content | RenderFragment        | -         |
| PendingDot   | Set the dot of the last ghost node when pending is true| RenderFragment  |-    |
| Reverse | reverse nodes or not | boolean         |-       |
| Mode |By sending `alternate` the timeline will distribute the nodes to the `left` and `right`.| string  | -  |

Timeline.Item
Node of timeline

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| Color | Set the circle's color blue, red, green, gray or other custom colors | string,         | -         |
| Dot   | Customize timeline dot| string         |
| Position | Customize node position `left`,`right`| string         |-       |
| Label |Set the label | RenderFragment  | -  |