﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ReuseTabsPageItem
    {
        public string Url { get; set; }

        public DateTime CreatedAt { get; set; }

        public RenderFragment Title { get; set; }

        public RenderFragment Body { get; set; }

        public bool Ignore { get; set; }

        public bool Closable { get; set; } = true;

        public bool Pin { get; set; } = false;

        public bool KeepAlive { get; set; } = true;

        public int Order { get; set; } = 9999;
    }
}
