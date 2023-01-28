---
order: 7
title:
  zh-CN: 滚动加载无限长列表
  en-US: Infinite & virtualized
---

## zh-CN

结合 [react-virtualized](https://github.com/bvaughn/react-virtualized) 实现滚动加载无限长列表，带有虚拟化（[virtualization](https://blog.jscrambler.com/optimizing-react-rendering-through-virtualization/)）功能，能够提高数据量大时候长列表的性能。

`virtualized` 是在大数据列表中应用的一种技术，主要是为了减少不可见区域不必要的渲染从而提高性能，特别是数据量在成千上万条效果尤为明显。[了解更多](https://blog.jscrambler.com/optimizing-react-rendering-through-virtualization/)

## en-US

An example of infinite list & virtualized loading using [react-virtualized](https://github.com/bvaughn/react-virtualized). [Learn more](https://blog.jscrambler.com/optimizing-react-rendering-through-virtualization/).

`Virtualized` rendering is a technique to mount big sets of data. It reduces the amount of rendered DOM nodes by tracking and hiding whatever isn't currently visible.
