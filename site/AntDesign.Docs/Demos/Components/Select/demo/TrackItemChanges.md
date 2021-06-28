---
order: 5
title:
  zh-CN: Track Item Changes
  en-US: Track Item Changes
---

## zh-CN

When an `Item` is initialized, the label value (e.g. Lucy), group name, disabled value  is set by reflection. To improve the performance and to avoid having to do the reflection again on every render cycle there is the parameter `IgnoreItemChanges`. The default value for this parameter is `True`. If you want that the values are updated at runtime set the parameter to `False`.

## en-US

When an `Item` is initialized, the label value (e.g. Lucy), group name, disabled value  is set by reflection. To improve the performance and to avoid having to do the reflection again on every render cycle there is the parameter `IgnoreItemChanges`. The default value for this parameter is `True`. If you want that the values are updated at runtime set the parameter to `False`.