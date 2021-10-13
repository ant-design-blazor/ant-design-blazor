---
order: 10
title:
  zh-CN: 卡片式页签容器
  en-US: Container of card type Tab
---

## zh-CN

用于容器顶部，需要一点额外的样式覆盖。

## en-US

Should be used at the top of container, needs to override styles.

<style>
#components-tabs-demo-card-top .code-box-demo {
  background: #F5F5F5;
  overflow: hidden;
  padding: 24px;
}
[data-theme="dark"] .card-container > .ant-tabs-card > .ant-tabs-bar .ant-tabs-tab {
  border-color: transparent;
  background: transparent;
}
[data-theme="dark"] #components-tabs-demo-card-top .code-box-demo {
  background: #000;
}
[data-theme="dark"] .card-container > .ant-tabs-card > .ant-tabs-content > .ant-tabs-tabpane {
  background: #141414;
}
[data-theme="dark"] .card-container > .ant-tabs-card > .ant-tabs-bar {
  border-color: #141414;
}
[data-theme="dark"] .card-container > .ant-tabs-card > .ant-tabs-bar .ant-tabs-tab-active {
  border-color: #141414;
  background: #141414;
}
</style>
