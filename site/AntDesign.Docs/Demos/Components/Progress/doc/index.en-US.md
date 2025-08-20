---
category: Components
type: Feedback
title: Progress
cover: https://gw.alipayobjects.com/zos/alicdn/xqsDu4ZyR/Progress.svg
---

Display the current progress of an operation flow.

## When To Use

If it will take a long time to complete an operation, you can use `Progress` to show the current progress and status.

- When an operation will interrupt the current interface, or it needs to run in the background for more than 2 seconds.
- When you need to display the completion percentage of an operation.

## API

Properties that shared by all types.

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Size | Size of the progress bar | ProgressSize | `ProgressSize.Default` |
| Type | Set the type of progress | ProgressType | `ProgressType.Line` |
| Format | Template expression for content | Func<double, string> | `percent => percent + '%'` |
| Percent | Completion percentage | number | 0 |
| ShowInfo | Display the progress value and the status icon | boolean | true |
| Status | Status of the progress | ProgressStatus | `ProgressStatus.Normal` |
| StrokeLinecap | Set the style of the progress linecap | ProgressStrokeLinecap | `ProgressStrokeLinecap.Round` |
| StrokeColor | Color of progress bar | string | - |
| SuccessPercent | Segmented success percent | number | 0 |
| TrailColor | Color of unfilled part as hex string. | string | - |

### `Type="ProgressType.Line"`

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| StrokeWidth | to set the width of the progress bar, unit: `px` | number | 10 |
| StrokeColor | color of progress bar, render `linear-gradient` when passing an object | string \| { from: string; to: string; direction: string } | - |
| Steps | the total step count | number | - |

### `Type="ProgressType.Circle"`

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Width | to set the canvas width of the circular progress, unit: `px` | number | 132 |
| WtrokeWidth | to set the width of the circular progress, unit: percentage of the canvas width | number | 6 |
| StrokeColor | color of circular progress, render `linear-gradient` when passing an object | string \| object | - |

### `Type="ProgressType.Dashboard"`

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Width | Set the canvas width of the dashboard progress, unit: `px` | number | 132 |
| StrokeWidth | Set the width of the dashboard progress, unit: percentage of the canvas width | number | 6 |
| MapDegree | The gap degree of half circle, 0 ~ 360 | number | 0 |
| GapPosition | Set the gap position | ProgressGapPosition | `ProgressGapPosition.Top` |
