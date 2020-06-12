using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign.Docs.Routing
{
    public class ConventionRouter : IComponent, IHandleAfterRender, IDisposable
    {
        private RenderHandle _renderHandle;
        private bool _navigationInterceptionEnabled;
        private string _location;

        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private INavigationInterception NavigationInterception { get; set; }
        [Inject] private RouteManager RouteManager { get; set; }
        [Inject] private ILanguageService LanguageService { get; set; }
        [Parameter] public RenderFragment NotFound { get; set; }
        [Parameter] public RenderFragment<RouteData> Found { get; set; }

        [Parameter] public Assembly AppAssembly { get; set; }

        [Parameter] public string DefaultUrl { get; set; }

        private static CultureInfo[] AllCultureInfos => CultureInfo.GetCultures(CultureTypes.AllCultures);

        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
            _location = NavigationManager.Uri;
            NavigationManager.LocationChanged += HandleLocationChanged;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (Found == null)
            {
                throw new InvalidOperationException($"The {nameof(ConventionRouter)} component requires a value for the parameter {nameof(Found)}.");
            }

            if (NotFound == null)
            {
                throw new InvalidOperationException($"The {nameof(ConventionRouter)} component requires a value for the parameter {nameof(NotFound)}.");
            }

            RouteManager.Initialise(AppAssembly);
            Refresh();

            return Task.CompletedTask;
        }

        public Task OnAfterRenderAsync()
        {
            if (!_navigationInterceptionEnabled)
            {
                _navigationInterceptionEnabled = true;
                return NavigationInterception.EnableNavigationInterceptionAsync();
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
        }

        private void HandleLocationChanged(object sender, LocationChangedEventArgs args)
        {
            _location = args.Location;
            Refresh();
        }

        private void Refresh()
        {
            var relativeUri = NavigationManager.ToBaseRelativePath(_location);

            var currentCulture = LanguageService.CurrentCulture;

            var segment = relativeUri.IndexOf('/') > 0 ? relativeUri.Substring(0, relativeUri.IndexOf('/')) : null;

            if (segment == null)
            {
                NavigationManager.NavigateTo($"{currentCulture.Name}/{relativeUri}", true);
                return;
            }
            else
            {
                if (AllCultureInfos.Any(x => x.Name == segment))
                {
                    LanguageService.SetLanguage(CultureInfo.GetCultureInfo(segment));
                }
                else
                {
                    NavigationManager.NavigateTo($"en-US/{relativeUri}", true);
                    return;
                }
            }

            var matchResult = RouteManager.Match(relativeUri);

            if (matchResult.IsMatch)
            {
                var routeData = new RouteData(matchResult.MatchedRoute.PageType, matchResult.MatchedRoute.Parameters);

                _renderHandle.Render(Found(routeData));
            }
            else
            {
                if (!string.IsNullOrEmpty(DefaultUrl))
                {
                    NavigationManager.NavigateTo($"{currentCulture}/{DefaultUrl}", true);
                }

                _renderHandle.Render(NotFound);
            }
        }
    }
}
