---
category: Components
type: Navigation
title: Dropdown
cover: https://gw.alipayobjects.com/zos/alicdn/eedWN59yJ/Dropdown.svg
---

A dropdown list.

## When To Use

When there are more than a few options to choose from, you can wrap them in a `Dropdown`. By hovering or clicking on the trigger, a dropdown menu will appear, which allows you to choose an option and execute the relevant action.

## Two types

There are 2 rendering approaches for `Dropdown`:  
1. Wraps child element (content of the `Dropdown`) with a `<div>` (default approach).
2. Child element is not wrapped with anything. This approach requires usage of `<Unbound>` tag inside `<Dropdown>` and depending on the child element type (please refer to the first example):
   - html tag: has to have its `@ref` set to `@context.Current` 
   - `Ant Design Blazor` component: has to have its `RefBack` attribute set to `@context`.
