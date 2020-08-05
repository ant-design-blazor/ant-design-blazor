---
order: 5
title:
  zh-CN: Task 接口
  en-US: Task interface
---

## zh-CN

可以通过 Task 接口在关闭后运行 callback 。以上用例将在每个 message 将要结束时通过 ContinueWith 显示新的 message 。

## en-US

`Message` provides a Task interface for `onClose`. The above example will display a new message when the old message is about to close.
