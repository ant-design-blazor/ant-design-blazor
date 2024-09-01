// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.JSInterop;

namespace AntDesign.JsInterop
{
    public class DomEventService
    {
        private DomEventSubscriptionStore _domEventSubscriptionStore = new();

        private readonly IJSRuntime _jsRuntime;

        public DomEventService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public virtual IDomEventListener CreateDomEventListerner()
        {
            return new DomEventListener(_jsRuntime, _domEventSubscriptionStore);
        }
    }
}
