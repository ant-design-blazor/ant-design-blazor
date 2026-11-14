### `OnChange` 为什么会被调用两次？

Table 组件支持预渲染，即在服务端渲染阶段会调用 `OnChange` 以便查询数据，并渲染出HTML作为请求响应返回。当浏览器上的 Blazor 实例启动时，会再次调用一次 `OnChange`。官方文档 [预呈现 ASP.NET Core Razor 组件](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/prerender?view=aspnetcore-9.0) 中有一些优化方式。另外，如果没有 SEO 需求，可考虑[关闭预呈现](https://learn.microsoft.com/zh-cn/aspnet/core/blazor/state-management?view=aspnetcore-9.0&pivots=server#handle-prerendering)

### 如何避免在首次加载时触发 `OnChange`？

默认情况下会在 Table 初始化完毕时自动触发 `OnChange`。我们推荐用它来替代 `OnInitialized{Async}`去加载数据，因为在 `OnChange` 调用时会传入 Table 的一些信息和状态，比如列名、排序、筛选、分页等。
如果希望避免在首次加载时触发 `OnChange`，可以设置 PageIndex = 0，以表示您不希望 Table 加载数据。