---
order: 1.3
title:
  en-US: Row-Click Select
  zh-CN: 点击行选择
---

## zh-CN

可根据需要设置如何通过点击行进行选择或取消选择当前行，包括单选、借助Shift或Ctrl键多选或跟随`Selection`列。
> 1、若存在Selection列：
>   - 当`Type`为`radio`时，`RowClickSelect`设置为`Single`和`BySelection`两者效果一样，均为单击行进行选择。
>   - 当`Type`为`checkbox`时，`RowClickSelect`设置为`Multiple`时需借助Shift或Ctrl键实现多选，设置为`BySelection`时点击行的处理逻辑和点击Checkbox一样。

> 2、若不存在Selection列：
>   - `RowClickSelect`设置为`Single`支持单选行。
>   - `RowClickSelect`设置为`Multiple`支持通过Shift或Ctrl实现多选。
>   - `RowClickSelect`设置为`BySelection`不作响应。

## en-US

You can configure how to select or deselect the current row by clicking on it based on your needs, including single selection, multi-selection using Shift or Ctrl keys, or following the `Selection` column.
>1. If the `Selection` column exists:
>   - When the `Type` is `radio`, setting `RowClickSelect` to either `Single` or `BySelection` will have the same effect, allowing users to select a row by clicking on it.
>   - When the `Type` is `checkbox`, setting `RowClickSelect` to `Multiple` requires the use of Shift or Ctrl keys to achieve multiple selections. Setting it to `BySelection` will make clicking on rows behave similarly to clicking on checkboxes.

>2. If the Selection column does not exist:
>   - Setting `RowClickSelect` to `Single` allows for single-row selection by clicking.
>   - Setting `RowClickSelect` to `Multiple` enables multiple selections via Shift or Ctrl keys.
>   - Setting `RowClickSelect` to `BySelection` will have no response, as there is no Selection column present for users to interact with through clicking.