---
order: 5
iframe: 360
link: /reuse/ignore
title:
  zh-CN: 指定忽略页面
  en-US: Ignore page
---

## zh-CN

ReuseTabs 会捕获页面组件，但不会渲染 Layout，因此可能会遇到希望某些页面（如登录页）不被捕获的场景，可使用 Ignore=true 禁用。如果需要打开全新页面，还需要配合不含 ReuseTabs 组件的 Layout。 

**注意: 必须级联传入 RouteData，否则无效。**

## en-US

ReuseTabs captures page components but does not render Layout, so you may encounter scenarios where you want certain pages, such as login pages, not to be captured, which can be disabled with Ignore=true. If you want to open a new page, you also need to match the Layout without the ReuseTabs component.

**Note: Must cascade RouteData, otherwise it will be invalid.**