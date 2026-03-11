---
order: 1
title:
  zh-CN: 表单集成
  en-US: Form Integration
---

## zh-CN

使用 `DraftMonitor` 组件与 `Form` 组合，实现表单草稿自动保存功能。当表单数据发生变化时，会自动延迟保存到本地存储。当用户重新打开页面时，会提示是否恢复未完成的草稿。

通过组合而非继承的方式，可以灵活地为任何表单添加草稿功能。

特性：
- 自动检测表单变更并延迟保存
- 支持自定义延迟时长（默认 1000ms）
- 支持版本控制，只恢复匹配版本的草稿
- 支持三种恢复模式：确认弹窗、自动恢复、手动恢复
- 组件化设计，易于集成

## en-US

Use `DraftMonitor` component combined with `Form` to implement form draft auto-save feature. When form data changes, it will automatically save to local storage with delay. When user reopens the page, it will prompt whether to recover the unfinished draft.

Through composition rather than inheritance, you can flexibly add draft functionality to any form.

Features:
- Auto-detect form changes and save with delay
- Customizable delay duration (default 1000ms)
- Version control, only recover drafts with matching version
- Three recovery modes: confirm dialog, auto-recover, manual
- Component-based design, easy to integrate
