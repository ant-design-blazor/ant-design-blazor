using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.JSInterop;

namespace AntDesign.JsInterop
{
    public class DomEventService
    {
        private ConcurrentDictionary<string, List<DomEventSubscription>> _domEventListeners = new ConcurrentDictionary<string, List<DomEventSubscription>>();

        private readonly IJSRuntime _jsRuntime;

        public DomEventService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private void AddEventListenerToFirstChildInternal<T>(object dom, string eventName, Action<T> callback)
        {
            if (!_domEventListeners.ContainsKey(FormatKey(dom, eventName)))
            {
                _jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListenerToFirstChild, dom, eventName, DotNetObjectReference.Create(new Invoker<T>((p) =>
                {
                    callback?.Invoke(p);
                })));
            }
        }

        public void AddEventListener(object dom, string eventName, Action<JsonElement> callback, bool exclusive = true)
        {
            AddEventListener<JsonElement>(dom, eventName, callback, exclusive);
        }

        public void AddEventListener<T>(object dom, string eventName, Action<T> callback, bool exclusive = true)
        {
            if (exclusive)
            {
                _jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListener, dom, eventName, DotNetObjectReference.Create(new Invoker<T>((p) =>
                {
                    callback(p);
                })));
            }
            else
            {
                string key = FormatKey(dom, eventName);
                if (!_domEventListeners.ContainsKey(key))
                {
                    _domEventListeners[key] = new List<DomEventSubscription>();

                    _jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListener, dom, eventName, DotNetObjectReference.Create(new Invoker<T>((p) =>
                    {
                        foreach (var subscription in _domEventListeners[key])
                        {
                            subscription.Delegate.DynamicInvoke(p);
                        }
                    })));
                }
                _domEventListeners[key].Add(new DomEventSubscription(callback));
            }
        }

        public void AddEventListenerToFirstChild(object dom, string eventName, Action<JsonElement> callback)
        {
            AddEventListenerToFirstChildInternal<string>(dom, eventName, (e) =>
            {
                JsonElement jsonElement = JsonDocument.Parse(e).RootElement;
                callback(jsonElement);
            });
        }

        public void AddEventListenerToFirstChild<T>(object dom, string eventName, Action<T> callback)
        {
            AddEventListenerToFirstChildInternal<string>(dom, eventName, (e) =>
            {
                T obj = JsonSerializer.Deserialize<T>(e);
                callback(obj);
            });
        }

        private static string FormatKey(object dom, string eventName) => $"{dom}-{eventName}";

        public void RemoveEventListerner<T>(object dom, string eventName, Action<T> callback)
        {
            string key = FormatKey(dom, eventName);
            if (_domEventListeners.ContainsKey(key))
            {
                var subscription = _domEventListeners[key].SingleOrDefault(s => s.Delegate == (Delegate)callback);
                if (subscription != null)
                {
                    _domEventListeners[key].Remove(subscription);
                }
            }
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
