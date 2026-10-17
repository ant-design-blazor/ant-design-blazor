---
title:
  en-US: Convert timestamp values
  zh-CN: 时间戳值转换
---

Keep the form model as a Unix timestamp while presenting a date-picker value. Changing the date normalizes it back to milliseconds; unlike React's `getValueProps`/`normalize` pair, Blazor performs the conversion in the value-change handler.
