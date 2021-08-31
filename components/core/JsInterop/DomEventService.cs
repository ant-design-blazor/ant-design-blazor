using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using AntDesign.Core.JsInterop.ObservableApi;
using Microsoft.AspNetCore.Components;
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

        public DomEventListener CreateDomEventListerner()
        {
            return new DomEventListener(_jsRuntime, _domEventSubscriptionStore);
        }
    }
}
