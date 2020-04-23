using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.JSInterop;

namespace AntBlazor.JsInterop
{
    public class DomEventService
    {
        private Dictionary<string, Func<object>> _domEventListeners = new Dictionary<string, Func<object>>();

        private readonly IJSRuntime _jsRuntime;

        public DomEventService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private void AddEventListenerInternal<T>(string dom, string eventName, Action<T> callback)
        {
            if (!_domEventListeners.ContainsKey($"{dom}-{eventName}"))
            {
                _jsRuntime.InvokeAsync<string>(JSInteropConstants.addDomEventListener, dom, eventName, DotNetObjectReference.Create(new Invoker<T>((p) =>
                {
                    callback?.Invoke(p);
                })));
            }
        }

        public void AddEventListener(string dom, string eventName, Action<JsonElement> callback)
        {
            AddEventListenerInternal<string>(dom, eventName, (e) =>
            {
                JsonElement jsonElement = JsonDocument.Parse(e).RootElement;
                callback(jsonElement);
            });
        }

        public void AddEventListener<T>(string dom, string eventName, Action<T> callback)
        {
            AddEventListenerInternal<string>(dom, eventName, (e) =>
            {
                T obj = JsonSerializer.Deserialize<T>(e);
                callback(obj);
            });
        }
    }

    public class Invoker<T>
    {
        private Action<T> _action;

        public Invoker(Action<T> invoker)
        {
            this._action = invoker;
        }

        [JSInvokable]
        public void Invoke(T param)
        {
            _action.Invoke(param);
        }
    }
}
