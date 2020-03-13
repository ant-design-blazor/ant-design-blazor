using System;

namespace AntBlazor.Docs.Routing
{
    public class Route
    {
        public string[] UriSegments { get; set; }
        public Type Handler { get; set; }

        public MatchResult Match(string[] segments)
        {
            if (segments.Length != UriSegments.Length)
            {
                return MatchResult.NoMatch();
            }

            for (var i = 0; i < UriSegments.Length; i++)
            {
                if (string.Compare(segments[i], UriSegments[i], StringComparison.OrdinalIgnoreCase) != 0)
                {
                    return MatchResult.NoMatch();
                }
            }

            return MatchResult.Match(this);
        }
    }
}