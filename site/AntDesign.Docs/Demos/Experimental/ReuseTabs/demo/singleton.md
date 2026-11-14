---
order: 4
iframe: 360
link: /reuse/singleton
title:
  zh-CN: 单例页面
  en-US: Singleton Page
---

## zh-CN

如果页面是单例的，它会被不同参数复用，否则，当参数不同时会打开另一个页面。默认不是单例。

单例页面不会重新实例化，也不再执行初始化方法，因此需要监听页面导航事件来更新界面。

**注意: 必须级联传入 RouteData，否则无效。**

## en-US

If the page is Singleton, it will be reused although the parameters is different, otherwise, another tab will be created.

The singleton page is not re-instantiated, nor is the `OnInitialized{Async}` method performed, so it needs to listen for location navigation events to update the UI.

**Note: Must cascade RouteData, otherwise it will be invalid.**