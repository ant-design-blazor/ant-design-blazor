// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign
{
    public class MenuLink : AntDomComponentBase
    {
        private const string DefaultActiveClass = "active";
        private bool _isActive;
        private string _hrefAbsolute;
        private string _class;

        /// <summary>
        /// Gets or sets the CSS class name applied to the NavLink when the
        /// current route matches the NavLink href.
        /// </summary>
        [Parameter]
        public string ActiveClass { get; set; }

        [Parameter]
        public string Href { get; set; }

        [Parameter]
        public MenuTarget? Target { get; set; }

        /// <summary>
        /// Gets or sets the child content of the component.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        /// <summary>
        /// Gets or sets a value representing the URL matching behavior.
        /// </summary>
        [Parameter]
        public NavLinkMatch Match { get; set; } = NavLinkMatch.All;

        [CascadingParameter]
        internal MenuItem MenuItem { get; set; }

        [CascadingParameter]
        internal Menu Menu { get; set; }

        [CascadingParameter]
        internal Button Button { get; set; }

        [Inject] private NavigationManager NavigationManger { get; set; }

        private readonly static Dictionary<MenuTarget, string> _targetMap = new()
        {
            [MenuTarget.Self] = "_self",
            [MenuTarget.Blank] = "_blank",
            [MenuTarget.Parent] = "_parent",
            [MenuTarget.Top] = "_top",
        };

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            // We'll consider re-rendering on each location change
            NavigationManger.LocationChanged += OnLocationChanged;

            ClassMapper.If(ActiveClass, () => _isActive)
                .If(DefaultActiveClass, () => _isActive && string.IsNullOrWhiteSpace(ActiveClass));
        }

        /// <inheritdoc />
        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            if (Match != NavLinkMatch.All && Href == "/")
            {
                Match = NavLinkMatch.All;
            }

            // Update computed state
            _hrefAbsolute = Href == null ? null : NavigationManger.ToAbsoluteUri(Href).AbsoluteUri;

            if (MenuItem.FirstRun)
            {
                _isActive = MenuHelper.ShouldMatch(Match, NavigationManger.Uri, _hrefAbsolute);
                if (MenuItem != null && _isActive && !MenuItem.IsSelected)
                {
                    Menu?.SelectItem(MenuItem);
                    Menu?.SelectSubmenu(MenuItem.ParentMenu);
                }
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            // To avoid leaking memory, it's important to detach any event handlers in Dispose()
            NavigationManger.LocationChanged -= OnLocationChanged;
            base.Dispose(disposing);
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs args)
        {
            // We could just re-render always, but for this component we know the
            // only relevant state change is to the _isActive property.
            bool shouldBeActiveNow = MenuHelper.ShouldMatch(Match, args.Location, _hrefAbsolute);
            if (shouldBeActiveNow != _isActive)
            {
                _isActive = shouldBeActiveNow;

                if (MenuItem != null)
                {
                    if (_isActive && !MenuItem.IsSelected)
                    {
                        Menu.SelectItem(MenuItem);
                    }
                    else if (!_isActive && MenuItem.IsSelected)
                    {
                        MenuItem.Deselect();
                    }
                }

                Menu.MarkStateHasChanged();
            }
        }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder != null)
            {
                builder.OpenElement(0, "a");
                builder.AddAttribute(1, "href", Href);
                builder.AddAttribute(2, "class", ClassMapper.Class);
                builder.AddAttribute(3, "style", Style);

                if (Target.HasValue)
                    builder.AddAttribute(4, "target", _targetMap[Target.Value]);

                builder.SetKey(MenuItem.Key);
                builder.AddMultipleAttributes(5, Attributes);
                builder.AddContent(6, ChildContent);
                builder.CloseElement();
            }
        }
    }
}
