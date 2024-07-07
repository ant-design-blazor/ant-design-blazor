// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign
{
    internal static class MenuHelper
    {
        /// <summary>
        /// Determines if the current URI should match the specified URI based on the given match criteria.
        /// </summary>
        /// <param name="match">The match criteria (Exact or Prefix).</param>
        /// <param name="currentUriAbsolute">The absolute URI of the current location.</param>
        /// <param name="hrefAbsolute">The absolute URI to match against.</param>
        /// <returns>True if the URIs match based on the criteria; otherwise, false.</returns>
        public static bool ShouldMatch(NavLinkMatch match, string currentUriAbsolute, string hrefAbsolute)
        {
            if (EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute, hrefAbsolute))
            {
                return true;
            }
            if (match == NavLinkMatch.Prefix
            && IsStrictlyPrefixWithSeparator(currentUriAbsolute, hrefAbsolute))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if a string value is strictly a prefix of another string, with a separator following the prefix.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <param name="prefix">The prefix to check for.</param>
        /// <returns>True if the value starts with the prefix followed by a separator; otherwise, false.</returns>
        public static bool IsStrictlyPrefixWithSeparator(string value, string prefix)
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

        /// <summary>
        /// Compares two URIs for equality, accounting for optional trailing slashes.
        /// </summary>
        /// <param name="currentUriAbsolute">The absolute URI of the current location.</param>
        /// <param name="hrefAbsolute">The absolute URI to compare against.</param>
        /// <returns>True if the URIs are equal, or if adding a trailing slash to the current URI makes them equal; otherwise, false.</returns>
        public static bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute, string hrefAbsolute)
        {
            if (string.Equals(currentUriAbsolute, hrefAbsolute, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (currentUriAbsolute.Length == hrefAbsolute.Length - 1)
            {
                // Special case: highlight links to http://host/path/ even if you're
                // at http://host/path (with no trailing slash)
                //
                // This is because the router accepts an absolute URI value of "same
                // as base URI but without trailing slash" as equivalent to "base URI",
                // which in turn is because it's common for servers to return the same page
                // for http://host/vdir as they do for host://host/vdir/ as it's no
                // good to display a blank page in that case.
                if (hrefAbsolute[^1] == '/'
                && hrefAbsolute.StartsWith(currentUriAbsolute, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
