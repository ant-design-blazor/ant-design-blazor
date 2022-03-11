﻿using System;
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
        public MenuItem MenuItem { get; set; }

        [CascadingParameter]
        public Menu Menu { get; set; }

        [CascadingParameter]
        public Button Button { get; set; }

        [Inject] private NavigationManager NavigationManger { get; set; }

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
                _isActive = ShouldMatch(NavigationManger.Uri);
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
            bool shouldBeActiveNow = ShouldMatch(args.Location);
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

        private bool ShouldMatch(string currentUriAbsolute)
        {
            if (EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute))
            {
                return true;
            }
            if (Match == NavLinkMatch.Prefix
            && IsStrictlyPrefixWithSeparator(currentUriAbsolute, _hrefAbsolute))
            {
                return true;
            }
            return false;
        }

        private bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute)
        {
            if (string.Equals(currentUriAbsolute, _hrefAbsolute, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (currentUriAbsolute.Length == _hrefAbsolute.Length - 1)
            {
                // Special case: highlight links to http://host/path/ even if you're
                // at http://host/path (with no trailing slash)
                //
                // This is because the router accepts an absolute URI value of "same
                // as base URI but without trailing slash" as equivalent to "base URI",
                // which in turn is because it's common for servers to return the same page
                // for http://host/vdir as they do for host://host/vdir/ as it's no
                // good to display a blank page in that case.
                if (_hrefAbsolute[^1] == '/'
                && _hrefAbsolute.StartsWith(currentUriAbsolute, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder != null)
            {
                builder.OpenElement(0, "a");
                builder.AddAttribute(1, "href", Href);
                builder.AddAttribute(2, "class", ClassMapper.Class);
                builder.AddAttribute(3, "style", Style);
                builder.SetKey(MenuItem.Key);
                builder.AddMultipleAttributes(5, Attributes);
                builder.AddContent(6, ChildContent);
                builder.CloseElement();
            }
        }

        private static bool IsStrictlyPrefixWithSeparator(string value, string prefix)
        {
            int prefixLength = prefix.Length;
            if (value.Length > prefixLength)
            {
                return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                && (
                // Only match when there's a separator character either at the end of the
                // prefix or right after it.
                // Example: "/abc" is treated as a prefix of "/abc/def" but not "/abcdef"
                // Example: "/abc/" is treated as a prefix of "/abc/def" but not "/abcdef"
                prefixLength == 0
                || !char.IsLetterOrDigit(prefix[prefixLength - 1])
                || !char.IsLetterOrDigit(value[prefixLength])
                );
            }
            else
            {
                return false;
            }
        }
    }
}
