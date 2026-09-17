---
order: 1
title:
  zh-CN: 按 Id 覆写
  en-US: Override by Id
---

## zh-CN

全局 `ButtonProps` 先将所有按钮设为主按钮；`["delete"]` 和 `["publish"]` 仅修改对应 `Id` 的 Button。局部 Props 只会覆盖显式赋值的字段，因此仍会保留全局的 `Type`。

## en-US

The global `ButtonProps` makes every button primary. `["delete"]` and `["publish"]` only change the Button with the matching `Id`. Local props override only their explicitly assigned properties, so the global `Type` is retained.
