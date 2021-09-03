using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign.Components.Authentication
{
    public sealed class AuthorizeReuseTabsRouteView : ReuseTabsRouteView
    {
        [Parameter]
        public RenderFragment<AuthenticationState> NotAuthorized { get; set; }

        [Parameter]
        public RenderFragment Authorizing { get; set; }

        private static readonly RenderFragment<AuthenticationState> _defaultNotAuthorizedContent
            = state => builder => builder.AddContent(0, "Not authorized");

        private static readonly RenderFragment _defaultAuthorizingContent
            = builder => builder.AddContent(0, "Authorizing...");

        private readonly RenderFragment _renderAuthorizeRouteViewCoreDelegate;
        private readonly RenderFragment<AuthenticationState> _renderAuthorizedDelegate;
        private readonly RenderFragment<AuthenticationState> _renderNotAuthorizedDelegate;
        private readonly RenderFragment _renderAuthorizingDelegate;

        public AuthorizeReuseTabsRouteView()
        {
            _renderAuthorizedDelegate = authenticateState => builder => base.Render(builder);

            _renderNotAuthorizedDelegate = authenticationState => builder => RenderNotAuthorizedInDefaultLayout(builder, authenticationState);

            _renderAuthorizingDelegate = RenderAuthorizingInDefaultLayout;

            _renderAuthorizeRouteViewCoreDelegate = RenderAuthorizeRouteViewCore;
        }


        [Parameter]
        public object Resource { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> ExistingCascadedAuthenticationState { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            if (ExistingCascadedAuthenticationState != null)
            {
                _renderAuthorizeRouteViewCoreDelegate(builder);
            }
            else
            {
                builder.OpenComponent<CascadingAuthenticationState>(0);
                builder.AddAttribute(1, nameof(CascadingAuthenticationState.ChildContent), _renderAuthorizeRouteViewCoreDelegate);
                builder.CloseComponent();
            }
        }

        //委托
        private void RenderAuthorizeRouteViewCore(RenderTreeBuilder builder)
        {
            builder.OpenComponent<AuthorizeRouteViewCore>(0);
            builder.AddAttribute(1, nameof(AuthorizeRouteViewCore.RouteData), RouteData);
            builder.AddAttribute(2, nameof(AuthorizeRouteViewCore.Authorized), _renderAuthorizedDelegate);
            builder.AddAttribute(3, nameof(AuthorizeRouteViewCore.Authorizing), _renderAuthorizingDelegate);
            builder.AddAttribute(4, nameof(AuthorizeRouteViewCore.NotAuthorized), _renderNotAuthorizedDelegate);
            builder.AddAttribute(5, nameof(AuthorizeRouteViewCore.Resource), Resource);
            builder.CloseComponent();
        }

        //未授权委托
        private void RenderNotAuthorizedInDefaultLayout(RenderTreeBuilder builder, AuthenticationState authenticationState)
        {
            var content = NotAuthorized ?? _defaultNotAuthorizedContent;
            RenderContentInDefaultLayout(builder, content(authenticationState));
        }

        //已授权委托
        private void RenderAuthorizingInDefaultLayout(RenderTreeBuilder builder)
        {
            var content = Authorizing ?? _defaultAuthorizingContent;
            RenderContentInDefaultLayout(builder, content);
        }

        private void RenderContentInDefaultLayout(RenderTreeBuilder builder, RenderFragment content)
        {
            builder.OpenComponent<LayoutView>(0);
            builder.AddAttribute(1, nameof(LayoutView.Layout), DefaultLayout);
            builder.AddAttribute(2, nameof(LayoutView.ChildContent), content);
            builder.CloseComponent();
        }

        private sealed class AuthorizeRouteViewCore : AuthorizeViewCore
        {
            [Parameter]
            public RouteData RouteData { get; set; } = default!;

            protected override IAuthorizeData[] GetAuthorizeData()
                => AttributeAuthorizeDataCache.GetAuthorizeDataForType(RouteData.PageType);
        }
    }



    internal static class AttributeAuthorizeDataCache
    {
        private static readonly ConcurrentDictionary<Type, IAuthorizeData[]> _cache = new ConcurrentDictionary<Type, IAuthorizeData[]>();

        public static IAuthorizeData[] GetAuthorizeDataForType(Type type)
        {
            if (!_cache.TryGetValue(type, out var result))
            {
                result = ComputeAuthorizeDataForType(type);
                _cache[type] = result;
            }

            return result;
        }

        private static IAuthorizeData[] ComputeAuthorizeDataForType(Type type)
        {
            var allAttributes = type.GetCustomAttributes(inherit: true);
            List<IAuthorizeData> authorizeDatas = null;
            for (var i = 0; i < allAttributes.Length; i++)
            {
                if (allAttributes[i] is IAllowAnonymous)
                {
                    return null;
                }

                if (allAttributes[i] is IAuthorizeData authorizeData)
                {
                    authorizeDatas ??= new List<IAuthorizeData>();
                    authorizeDatas.Add(authorizeData);
                }
            }

            return authorizeDatas?.ToArray();
        }
    }
}
