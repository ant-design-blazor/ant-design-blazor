// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class ShowForeverPageItem
    {
        public ShowForeverPageItem(string url, Type pageType)
        {
            this.Url = url;
            this.PageType = pageType;
        }

        public string Url { get; set; }

        public Type PageType { get; set; }

    }
}
