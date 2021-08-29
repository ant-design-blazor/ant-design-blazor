using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.JsInterop
{
    public class DomEventListener<T>
    {
        private readonly IJSRuntime _jsRuntime;
        private Dictionary<string, DotNetObjectReference<Invoker<T>>> _dotNetObjectStore = new();

        public DomEventListener(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public void Add(object dom, string eventName, Action<T> callback, bool preventDefault = false)
        {
            var dotNetObject = DotNetObjectReference.Create(new Invoker<T>((p) =>
            {
                callback(p);
            }));
            _jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListener, dom, eventName, preventDefault, dotNetObject);

            var key = DomEventService.FormatKey(dom, eventName);
            _dotNetObjectStore.Add(key, dotNetObject);
        }

        public void Remove(object dom, string eventName)
        {
            var key = DomEventService.FormatKey(dom, eventName);
            foreach (var (k, v) in _dotNetObjectStore)
            {
                if (k == key)
                {
                    v.Dispose();
                }
            }
        }

        public void Dispose()
        {
            foreach (var (k, v) in _dotNetObjectStore)
            {
                v.Dispose();
            }
        }
    }
}
