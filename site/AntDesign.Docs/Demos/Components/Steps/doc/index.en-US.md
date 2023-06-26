---
category: Components
type: Navigation
title: Steps
cols: 1
cover: https://gw.alipayobjects.com/zos/antfincdn/UZYqMizXHaj/Steps.svg
---

`Steps` is a navigation bar that guides users through the steps of a task.

## When To Use

- When a given task is complicated or has a certain sequence in the series of subtasks, we can decompose it into several steps to make things easier.


## API

Steps
The whole of the step bar.

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| ClassName | Additional class to Steps | string         | -         |
| Type   | Type of steps, can be set to one of the following values: `default`, `navigation`| string         |`default`       |
| Current | To set the current step, counting from 0. You can overwrite this state by using status of Step | int         |0       |
| Direction | To specify the direction of the step bar, `horizontal` or `vertical`| string  | `horizontal`  |
| LabelPlacement | Place title and description with `horizontal` or `vertical` direction| string  | `horizontal`  |
| ProgressDot | Steps with progress dot style, customize the progress dot by setting it to a function. labelPlacement will be `vertical`| string  | `default`  |
| Size | To specify the size of the step bar, `default` and `small` are currently supported| string  | `default`  |
| Status | To specify the status of current step, can be set to one of the following values: `wait` `process` `finish` `error`| string  | `process`  |
| Initial | Set the initial step, counting from 0| int  | -  |
| OnChange | Trigger when Step is changed| function(e)  | -  |


Steps.Step
A single step in the step bar.

| Property | Description | Type | Default Value |
| --- | --- | --- | --- |
| Description | Description of the step, optional property | string         | -         |
| Icon | Icon of the step, optional property | string         | -         |
| Status | To specify the status. It will be automatically set by current of Steps if not configured. Optional values are: wait process finish error | string         | -         |
| Title | Title of the step | string         | -         |
| SubTitle | Subtitle of the step | string         | -         |
| Disabled | Disable click | boolean         | false         |
