---
order: 5
iframe: 360
link: /reuse/ignore
title:
  zh-CN: 指定忽略页面
  en-US: Ignore page
---

## zh-CN

当希望某些页面不被捕获的时候，可使用 Ignore=true 禁用。如果需要打开全新页面，还需要配合不含 ReuseTabs 组件的 Layout。 

**注意: 必须级联传入 RouteData，否则无效。**

## en-US

If the page is Singleton, it will be reused although the parameters is different, otherwise, another tab will be created.

The singleton page is not re-instantiated, nor is the `OnInitialized{Async}` method performed, so it needs to listen for location navigation events to update the UI.

**Note: Must cascade RouteData, otherwise it will be invalid.**