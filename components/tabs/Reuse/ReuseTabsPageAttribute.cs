// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ReuseTabsPageAttribute : Attribute
    {
        public string Title { get; set; }
        public RenderFragment Title1 { get; set; }
        public bool Ignore { get; set; }
        public bool Closable { get; set; } = true;
        /// <summary>
        /// 永久停留显示在tab上
        /// </summary>
        public bool ShowForever { get; set; } = false;

    }
}
