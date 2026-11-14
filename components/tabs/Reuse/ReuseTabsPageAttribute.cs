// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    /// <summary>
    /// Attribute for ReuseTabsPage, used to set the page title and other properties.
    /// </summary>
    public class ReuseTabsPageAttribute : Attribute
    {
        /// <summary>
        /// Specifies the title of the tab.
        /// <para>
        ///  If you want to set a <see cref="Microsoft.AspNetCore.Components.RenderFragment"/>, you need implement <see cref="IReuseTabsPage.GetPageTitle()"/> in the page.
        /// </para>
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Weather the page wont be reused.
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Weather the tab can be closed.
        /// </summary>
        public bool Closable { get; set; } = true;

        /// <summary>
        /// Weather the tab can be pinned and opened at the first time.
        /// </summary>
        public bool Pin { get; set; } = false;

        /// <summary>
        /// The url of the pinned page. Because when the tab is clicked, it need to navigate to the page through the url.
        /// </summary>
        public string PinUrl { get; set; }

        /// <summary>
        /// Weather the page is keeping alive.
        /// <para>
        ///  If true, the page will be kept in memory, otherwise, the page will be destroyed when it is not active.
        ///  </para>
        /// </summary>
        public bool KeepAlive { get; set; } = true;

        /// <summary>
        /// The order of the page, the smaller the order, the earlier the page will be displayed.
        /// </summary>
        public int Order { get; set; } = 999;

        /// <summary>
        /// Weather the page is a singleton.
        /// <para>
        /// If true, the page will be reused although the parameters is different, otherwise, another tab will be created.
        /// </para>
        /// </summary>
        public bool Singleton { get; set; }
    }
}
