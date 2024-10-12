﻿---
category: Experimental
type: Layout
title: ReuseTabs
cols: 1
cover: https://gw.alipayobjects.com/zos/antfincdn/lkI2hNEDr2V/Tabs.svg
---

Used to implement in-application page tabs and page caching.

## When to use

- You need to use the in-app page tab to keep opened pages.
- When the page state needs to be hold, the state will not be lost when navigating back to the opened page.

## How to use

Modify the `MainLayout.razor` file, add the `ReuseTabs` component. Note that `@Body` is not required at this case.

   ```html
   @inherits LayoutComponentBase

   <div class="page">
       <div class="sidebar">
           <NavMenu />
       </div>

       <div class="main">
         <ReuseTabs Class="top-row px-4" TabPaneClass="content px-4" Body="Body" / >
       </div>
   </div>

   ```

With just this step, you can easily implement a simple multi-tab feature. 

But if you still need more powerful page configuration capabilities, 
such as the `ReuseTabsPageAttribute` or `IReuseTabsPage`  in the following examples, 
you also need to modify the `Routes.razor` file, warp the `RouteView` or `AuthorizeRouteView` with `<CascadingValue Value="routeData">...</CascadingValue>`.

   ```html
   <Router AppAssembly="@typeof(Program).Assembly">
       <Found Context="routeData">
           <CascadingValue Value="routeData">
               <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" / >
           </CascadingValue>
      </Found>
       ...
   </Router>
   ```

## API

### ReuseTabs

| Property | Description | Type | Default | 
| --- | --- | --- | --- |
| Body | Used to set the @Body of Layout component. This parameter must be set in non-cascading RouteData mode. | RenderFragment | - |
| TabPaneClass | the class names of the Pane container | string | --- |
| Draggable | Whether you can drag and drop to adjust the order | bool | false |
| Size | Tabs size | TabSize | - |
| TabPaneTemplate | A template for a rendering class that adds styles around the page in the TabPane, passing in a context called `ReuseTabsPageItem`, where the Body is the page content | `RenderFragment<ReuseTabsPageItem>` | context => context.Body |
| Locale | Localized object | - | - |
| HidePages | Whether pages hidden in Tabs are used with ReusePages | bool | false |
| ReuseTabsRouteData | The routing information for the current page, which is a serializable version of RouteData | RouteData | - |

Other properties inherit from [Tabs](/components/tabs#API)

### ReuseTabsPageAttribute attribute


**Note: Must cascade RouteData, otherwise it will be invalid.**

| Property | Description | Type | Default | 
| --- | --- | --- | --- |
| Title | The fixed title show on tab. | string | current path |
| Ignore | If `Ignore=true`, the page is not displayed in tab, but in the entire page. | boolean | false |
| Closable | Whether the delete button can be displayed. | boolean | false |
| Pin | Whether the page is fixed to load and avoid closing, usually used on the home page or default page. | boolean | false |
| PinUrl | Specify the Url of the loaded page, and then open the page with a route parameter, such as `/order/1` | string | - |
| KeepAlive| Whether to cache the page state | bool | true |
| Order | The sequence number | int | 999 |
| TypeName | The page's classsname | string | - |
| Key | The page's key | string | - |
| Singleton | Whether to create a new page for route with different params | bool | false |

### IReuseTabsPage interface

**Note: Must cascade RouteData, otherwise it will be invalid.**

| Method | Description |
| --- | --- | 
| RenderFragment GetPageTitle() | Sets dynamic titles for titles that need to be determined when using templates or when loading |


### ReuseTabsService

Used to control ReuseTabs in pages

| Method | Description | 
| --- | --- | 
| Pages | The information list of the currently opened pages can be used for caching and recovery | 
| CreateTab(string pageUrl, RenderFragment? title = null) | Create a tab, but do not navigate to the page, and initialize the page when you navigate to the page. |
| ClosePage(string url) | Close the page with the specified url. |
| CloseOther(string url) | Close all pages except those that specify url, `Cloasable=false`, or `Pin=true`. |
| CloseAll() | Close all pages except those that `Cloasable=false` or `Pin=true`.|
| CloseCurrent() | Close current page. |
| Update() | Update the state of current tab. When the variable referenced in `GetPageTitle()` changes, `Update()` needs to be called to update the tab display. |
| ReloadPage(url) | Reload the page for the specified label, allowing the page components to be reinitialized without refreshing the browser. If no url is passed, reload the current page . |
