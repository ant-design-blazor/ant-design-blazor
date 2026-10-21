// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    /// <summary>An option rendered by <see cref="CheckableTagGroup"/>.</summary>
    public sealed class CheckableTagOption
    {
        /// <summary>The option value.</summary>
        public string Value { get; set; }

        /// <summary>The content displayed in the tag.</summary>
        public string Label { get; set; }

        /// <summary>Optional class applied to this option.</summary>
        public string Class { get; set; }

        /// <summary>Optional inline style applied to this option.</summary>
        public string Style { get; set; }
    }
}
