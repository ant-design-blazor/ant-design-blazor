// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    public partial class ReuseTabsService
    {
        internal event Action<string> OnClosePage;

        internal event Action<string> OnCloseOther;

        internal event Action<string> OnReloadPage;

        internal event Action OnCloseAll;

        internal event Action OnCloseCurrent;

        internal event Action OnUpdate;



        public void ClosePage(string key)
        {
            OnClosePage?.Invoke(key);
        }

        public void CloseOther(string key)
        {
            OnCloseOther?.Invoke(key);
        }

        public void CloseAll()
        {
            OnCloseAll?.Invoke();
        }

        public void CloseCurrent()
        {
            OnCloseCurrent?.Invoke();
        }

        public void Update()
        {
            OnUpdate?.Invoke();
        }

        public void ReloadPage(string url = null)
        {
            OnReloadPage?.Invoke(url);
        }
    }
}
