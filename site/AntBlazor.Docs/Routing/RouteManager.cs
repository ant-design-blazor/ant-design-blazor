using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Docs.Routing
{
    public class RouteManager
    {
        public Route[] Routes { get; private set; }

        public void Initialise()
        {
            var pageComponentTypes = Assembly.GetExecutingAssembly()
                .ExportedTypes
                .Where(t => t.Namespace != null && (t.IsSubclassOf(typeof(ComponentBase))
                                                    && t.Namespace.Contains(".Pages")));

            var routesList = new List<Route>();
            foreach (var pageType in pageComponentTypes)
            {
                if (pageType.FullName == null)
                    continue;

                var newRoute = new Route
                {
                    UriSegments = pageType.FullName.Substring(pageType.FullName.IndexOf("Pages", StringComparison.Ordinal) + 6).Split('.'),
                    Handler = pageType
                };

                routesList.Add(newRoute);
            }

            Routes = routesList.ToArray();
        }

        public MatchResult Match(string[] segments)
        {
            if (segments.Length == 0)
            {
                var indexRoute = Routes.SingleOrDefault(x => x.Handler.FullName != null && x.Handler.FullName.ToLower().EndsWith("index"));
                return MatchResult.Match(indexRoute);
            }

            foreach (var route in Routes)
            {
                var matchResult = route.Match(segments);

                if (matchResult.IsMatch)
                {
                    return matchResult;
                }
            }

            return MatchResult.NoMatch();
        }
    }
}