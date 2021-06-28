---
order: 5
title:
  zh-CN: 异步确认对话框
  en-US: Promise Confirmation modal dialog
---

## zh-CN

使用 `Confirm()` 可以快捷地弹出确认框。OnCancel/OnOk 异步事件 可以延迟关闭。

使用`ModalClosingEventArgs.Cancel`决定窗体是否关闭。返回结果: true 如果应取消事件;否则为 false。

## en-US

Use `Confirm()` to show a confirmation modal dialog. Asynchronous event OnCancel/OnOk can delay closing the dialog.

