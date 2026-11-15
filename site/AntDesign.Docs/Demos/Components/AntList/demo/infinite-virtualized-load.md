---
order: 7
title:
  zh-CN: 滚动加载无限长列表
  en-US: Infinite & virtualized
---

## zh-CN

滚动到列表底部时自动加载下一页，并通过虚拟滚动只渲染可视区域附近的列表项，降低长列表的 DOM 数量。

虚拟滚动会用占位区域保持列表的整体高度，同时复用有限数量的可见列表项。随着滚动位置变化，列表只更新当前视口附近的数据。

## en-US

Scroll to the bottom to append the next page. Virtual scrolling renders only the items near the viewport to keep the DOM small.

Spacer elements preserve the full list height while a small window of visible items is updated as you scroll.
