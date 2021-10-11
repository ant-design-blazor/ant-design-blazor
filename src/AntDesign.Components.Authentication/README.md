# AuthorizeReuseTabsRouteView 

A combination of `ReuseTabsRouteView` and `AuthorizeRouteView`.



# How to use

Almost the same as `AuthorizeRouteView`.



1. Modify the `App.razor` file, replace the `RouteView` or `ReuseTabsRouteView` with `AuthorizeReuseTabsRouteView`.

    ```diff
    +<CascadingAuthenticationState>
         <Router AppAssembly="@typeof(Program).Assembly" PreferExactMatches="@true">
             <Found Context="routeData">
    -            <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
    +            <AuthorizeReuseTabsRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)">
    +                <NotAuthorized>
    +                    <RedirectToLogin />
    +                </NotAuthorized>
    +                <Authorizing>
    +                    <p>Authorizing............</p>
    +                </Authorizing>
    +            </AuthorizeReuseTabsRouteView>
             </Found>
             <NotFound>
                 <LayoutView Layout="@typeof(MainLayout)">
                     <p>Sorry, there's nothing at this address.</p>
                 </LayoutView>
             </NotFound>
         </Router>
    +</CascadingAuthenticationState>
    ```

2. Then modify the `MainLayout.razor` file, add the `ReuseTabs` component. Note that `@Body` is **required** at this case, so you can perform redirect and other actions.

    ```diff
    @inherits LayoutComponentBase

    <div class="page">
        <div class="sidebar">
            <NavMenu />
        </div>

        <div class="main">
    -       <div class="top-row px-4">
    -           <a href="http://blazor.net" target="_blank" class="ml-md-auto">About</a>
    -       </div>
            <div class="content px-4">
                @Body
            </div>
    +       <ReuseTabs Class="top-row px-4" TabPaneClass="content px-4" / >
        </div>
    </div>

    ```

# Customize tab title

Same as `ReuseTabsRouteView`