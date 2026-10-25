---
order: 4
title:
  zh-CN: 使用 iconfont.cn
  en-US: Use iconfont.cn
---

## zh-CN

对于使用 [iconfont.cn](https://iconfont.cn/) 的用户，将 “Symbol 引用” 中的 JS 地址传给 `IconFont.ScriptUrl` 即可使用项目中的图标。组件会在交互式渲染后加载资源，并在多个图标实例之间复用同一请求，无需在 Razor 中直接插入 `<script>`。

JS 获取方法：创建项目 -> 导入图标 -> 选择 “Symbol” -> 查看在线链接

## en-US

For [iconfont.cn](https://iconfont.cn/), pass the “Symbol” JavaScript URL to `IconFont.ScriptUrl`. Resources load after interactive rendering and requests are shared across icon instances; no inline Razor `<script>` is needed.

JS to get: Create project -> Import icon -> Select 'Symbol' -> View online links
