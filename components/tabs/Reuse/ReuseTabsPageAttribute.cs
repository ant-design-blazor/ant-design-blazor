// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    public class ReuseTabsPageAttribute : Attribute
    {
        public string Title { get; set; }

        public bool Ignore { get; set; }

        public bool Closable { get; set; } = true;

        public bool Pin { get; set; } = false;
    }
}
