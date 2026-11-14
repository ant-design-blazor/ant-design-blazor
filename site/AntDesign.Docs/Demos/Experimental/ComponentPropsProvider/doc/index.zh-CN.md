---
category: Experimental
type: 其他
title: ComponentPropsProvider
subtitle: 级联组件属性
cols: 1
---

## 何时使用

当一个组件内部渲染多个子组件时，使用 `ComponentPropsProvider` 从外层统一设置由源生成器生成的 `XxxProps`，而不需要在父组件上重复暴露子组件的每个参数。

## 覆写规则

- 在 `ComponentPropsCollection` 构造函数中传入 `XxxProps`，为对应类型的所有子组件提供默认属性。
- 使用 `[id] = props` 仅覆写 `Id` 匹配的子组件。
- 属性按“外层 Provider → 内层 Provider → Id 指定项”合并；后面的对象只覆盖其显式设置的属性。

一个 Provider 可以同时传入多个不同的 Props 类型，且每个 `XxxProps` 的属性仍由编译器进行类型检查。
