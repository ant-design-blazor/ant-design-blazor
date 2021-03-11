---
order: 1
title:
  zh-CN: 独立使用
  en-US: Standalone
---

## zh-CN

不包裹任何元素即是独立使用，可自定样式展现。

> 在右上角的 badge 则限定为红色。

## en-US

Used in standalone when children is empty.

<style>
.ant-badge-not-a-wrapper:not(.ant-badge-status) {
  margin-right: 8px;
}
.ant-badge.ant-badge-rtl:not(.ant-badge-not-a-wrapper) {
  margin-right: 0;
  margin-left: 20px;
}
[data-theme="dark"] .site-badge-count-4 .ant-badge-count {
  background-color: #141414;
  box-shadow: 0 0 0 1px #434343 inset;
}
</style>