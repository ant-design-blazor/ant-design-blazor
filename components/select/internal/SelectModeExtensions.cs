// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

#pragma warning disable 1591 // Disable missing XML comment

namespace AntDesign.Select.Internal
{
    internal static class SelectModeExtensions
    {
        internal const string Tags = "tags";
        internal const string Multiple = "multiple";
        private const StringComparison Comparison = StringComparison.OrdinalIgnoreCase;

        internal static SelectMode ToSelectMode(this string mode)
        {
            if (Tags.Equals(mode, Comparison))
            {
                return SelectMode.Tags;
            }

            return Multiple.Equals(mode, Comparison) ? SelectMode.Multiple : SelectMode.Default;
        }
    }
}
