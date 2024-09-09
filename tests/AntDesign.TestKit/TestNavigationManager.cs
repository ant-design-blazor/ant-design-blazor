// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Tests
{
    [Obsolete("Use built in NavigationManager")]
    public class TestNavigationManager : NavigationManager
    {
        public delegate void NavigatedCallback(string uri, bool forceLoad);

        public TestNavigationManager()
        {
            Initialize("http://localhost/", "http://localhost/");
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            Navigated?.Invoke(uri, forceLoad);
        }

        public event NavigatedCallback Navigated;
    }
}
