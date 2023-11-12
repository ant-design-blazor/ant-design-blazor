using System;
using System.Collections.Generic;

namespace AntDesign.Docs.Routing
{
    public class Route
    {
        public Type PageType { get; set; }

        internal RouteTemplate Template { get; set; }

        public string[] UnusedRouteParameterNames { get; set; }

        public Dictionary<string, object> Parameters { get; set; }

        public MatchResult Match(string[] segments, string relativeUri)
        {
            Dictionary<string, object> parameters = []; //ParseQueryString(relativeUri);

            if (Template.Segments.Length != segments.Length)
            {
                return MatchResult.NoMatch();
            }

            for (var i = 0; i < Template.Segments.Length; i++)
            {
                var segment = Template.Segments[i];
                var pathSegment = segments[i];
                if (!segment.Match(pathSegment, out var matchedParameterValue))
                {
                    return MatchResult.NoMatch();
                }
                else
                {
                    if (segment.IsParameter)
                    {
                        parameters[segment.Value] = matchedParameterValue;
                    }
                }
            }

            // In addition to extracting parameter values from the URL, each route entry
            // also knows which other parameters should be supplied with null values. These
            // are parameters supplied by other route entries matching the same handler.
            if (UnusedRouteParameterNames.Length > 0)
            {
                foreach (var name in UnusedRouteParameterNames)
                {
                    parameters[name] = null;
                }
            }

            this.Parameters = parameters;

            return MatchResult.Match(this);
        }

        private Dictionary<string, object> ParseQueryString(string uri)
        {
            // Parameters will be lazily initialized.
            Dictionary<string, object> querystring = null;

            foreach (string kvp in uri.Substring(uri.IndexOf("?", StringComparison.Ordinal) + 1).Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (kvp != "" && kvp.Contains("="))
                {
                    var pair = kvp.Split('=');
                    querystring ??= new Dictionary<string, object>(StringComparer.Ordinal);
                    querystring.Add(pair[0], pair[1]);
                }
            }

            return querystring;
        }
    }
}
