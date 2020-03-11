using System;
using System.Collections.Generic;
using Microsoft.JSInterop;

namespace AntBlazor.JsInterop
{
    public class DomEventService
    {
        private Dictionary<string, Func<object>> domEventListeners = new Dictionary<string, Func<object>>();

        private readonly IJSRuntime _jsRuntime;

        public DomEventService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public void AddEventListener<T>(string dom, string eventName, Action<T> callback)
        {
            if (!domEventListeners.ContainsKey($"{dom}-{eventName}"))
            {
                _jsRuntime.InvokeAsync<string>(JSInteropConstants.addDomEventListener, dom, eventName, DotNetObjectReference.Create(new Invoker<T>((p) =>
                 {
                     callback?.Invoke(p);
                 })));
            }
        }
    }

    public class Invoker<T>
    {
        private Action<T> action;

        public Invoker(Action<T> invoker)
        {
            this.action = invoker;
        }

        [JSInvokable]
        public void Invoke(T param)
        {
            action.Invoke(param);
        }
    }
}