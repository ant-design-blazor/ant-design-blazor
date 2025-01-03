### Why is `OnChange` called twice?

The Table component supports pre-rendering, where `OnChange` is called during the server-side rendering phase in order to query the data and render HTML to be returned as a request response. When the Blazor instance on the browser is started, `OnChange` is called again. The official documentation [Prerender ASP.NET Core Razor components](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/prerender?view=aspnetcore-9.0) has some ways to optimize. Alternatively, if you don't have SEO needs, consider [turn off prerendering](https://learn.microsoft.com/zh-cn/aspnet/core/blazor/state-management?view=aspnetcore-9.0&pivots=server#).

### How to avoid triggering `OnChange` on first load?

By default, `OnChange` is automatically triggered when the Table is initialized. We recommend using this instead of `OnInitialized{Async}` to load the data, because some information and state of the Table is passed in during the `OnChange` call, such as column names, sorting, filtering, paging, etc. If you want to avoid triggering `OnChange` when the Table is first loaded, you can set the PageIndex = 0 to indicate that you don't want the Table to be loaded.
If you want to avoid triggering `OnChange` on the first load, you can set PageIndex = 0 to indicate that you don't want the Table to load data.