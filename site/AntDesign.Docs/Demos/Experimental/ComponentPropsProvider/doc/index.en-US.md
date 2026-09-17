---
category: Experimental
type: Other
title: ComponentPropsProvider
subtitle: Cascaded component props
cols: 1
---

## When To Use

Use `ComponentPropsProvider` when a component renders child components internally and their source-generated `XxxProps` should be configured from outside, without exposing every child parameter on the parent component.

## Override rules

- Passing `XxxProps` to the `ComponentPropsCollection` constructor supplies default props to every child of the matching type.
- `[id] = props` only overrides the child with the matching `Id`.
- Props are merged in the order: outer provider, inner provider, then the id-specific entry. A later props object changes only the properties explicitly assigned to it.

One provider can contain multiple props types, while every `XxxProps` object remains strongly typed.
