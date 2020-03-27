using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AntBlazor.core.Internal;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Docs.Routing
{
    public class RouteManager
    {
        public Route[] Routes { get; private set; }

        public void Initialise(Assembly appAssembly)
        {
            var pageComponentTypes = appAssembly
                .ExportedTypes
                .Where(t => t.Namespace != null && (t.IsSubclassOf(typeof(ComponentBase))
                                                    && t.Namespace.Contains(".Pages")));

            var routesList = new List<Route>();
            foreach (var pageType in pageComponentTypes)
            {
                if (pageType.FullName == null)
                    continue;

                var uriSegments = pageType.FullName.Substring(pageType.FullName.IndexOf("Pages", StringComparison.Ordinal) + 6).Split('.');

                var routeAttributes = pageType.GetCustomAttributes<RouteAttribute>(inherit: false);

                if (!routeAttributes.Any())
                {
                    var newRoute = new Route
                    {
                        UriSegments = uriSegments,
                        PageType = pageType,
                    };
                    routesList.Add(newRoute);
                    continue;
                }

                var templates = routeAttributes.Select(t => t.Template).ToArray();
                var parsedTemplates = templates.Select(TemplateParser.ParseTemplate).ToArray();
                var allRouteParameterNames = parsedTemplates
                    .SelectMany(GetParameterNames)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                foreach (var parsedTemplate in parsedTemplates)
                {
                    var unusedRouteParameterNames = allRouteParameterNames
                        .Except(GetParameterNames(parsedTemplate), StringComparer.OrdinalIgnoreCase)
                        .ToArray();

                    var newRoute = new Route
                    {
                        UriSegments = uriSegments,
                        PageType = pageType,
                        Template = parsedTemplate,
                        UnusedRouteParameterNames = unusedRouteParameterNames
                    };

                    routesList.Add(newRoute);
                }
            }

            Routes = routesList.ToArray();
        }

        public MatchResult Match(string relativeUri)
        {
            var segments = relativeUri.Trim().Split('/', StringSplitOptions.RemoveEmptyEntries).Select(Uri.UnescapeDataString).ToArray();
            if (segments.Length == 0)
            {
                var indexRoute = Routes.SingleOrDefault(x => x.PageType.FullName != null && x.PageType.FullName.ToLower().EndsWith("index"));

                if (indexRoute != null)
                {
                    return MatchResult.Match(indexRoute);
                }
            }

            foreach (var route in Routes)
            {
                var matchResult = route.Match(segments, relativeUri);

                if (matchResult.IsMatch)
                {
                    return matchResult;
                }
            }

            return MatchResult.NoMatch();
        }

        private static string[] GetParameterNames(RouteTemplate routeTemplate)
        {
            return routeTemplate.Segments
                .Where(s => s.IsParameter)
                .Select(s => s.Value)
                .ToArray();
        }
    }
}