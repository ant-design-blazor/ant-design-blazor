﻿using System;
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
        private ConcurrentDictionary<string, List<DomEventSubscription>> _domEventListeners = new();

        private readonly IJSRuntime _jsRuntime;
        private bool? _isResizeObserverSupported = null;

        public DomEventService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private void AddEventListenerToFirstChildInternal<T>(object dom, string eventName, bool preventDefault, Action<T> callback)
        {
            if (!_domEventListeners.ContainsKey(FormatKey(dom, eventName)))
            {
                _jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListenerToFirstChild, dom, eventName, preventDefault, DotNetObjectReference.Create(new Invoker<T>((p) =>
                {
                    callback?.Invoke(p);
                })));
            }
        }

        public void AddEventListener(object dom, string eventName, Action<JsonElement> callback, bool preventDefault = false)
        {
            AddEventListener<JsonElement>(dom, eventName, callback, preventDefault);
        }

        private Invoker<string> CreateSubscriptionInvoker(List<DomEventSubscription> domEventSubscriptions)
        {
            return new Invoker<string>((p) =>
            {
                for (var i = 0; i < domEventSubscriptions.Count; i++)
                {
                    var subscription = domEventSubscriptions[i];
                    object tP = JsonSerializer.Deserialize(p, subscription.Type);
                    subscription.Delegate.DynamicInvoke(tP);
                }
            });
        }

        public virtual void AddEventListener<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false)
        {
            string key = FormatKey(dom, eventName);
            if (!_domEventListeners.ContainsKey(key))
            {
                _domEventListeners[key] = new List<DomEventSubscription>();
                var invoker = CreateSubscriptionInvoker(_domEventListeners[key]);
                var dotNetObject = DotNetObjectReference.Create(invoker);

                _jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListener, dom, eventName, preventDefault, dotNetObject);
            }
            _domEventListeners[key].Add(new DomEventSubscription(callback, typeof(T)));
        }

        public void AddEventListenerToFirstChild(object dom, string eventName, Action<JsonElement> callback, bool preventDefault = false)
        {
            AddEventListenerToFirstChildInternal<string>(dom, eventName, preventDefault, (e) =>
            {
                JsonElement jsonElement = JsonDocument.Parse(e).RootElement;
                callback(jsonElement);
            });
        }

        public void AddEventListenerToFirstChild<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false)
        {
            AddEventListenerToFirstChildInternal<string>(dom, eventName, preventDefault, (e) =>
            {
                T obj = JsonSerializer.Deserialize<T>(e);
                callback(obj);
            });
        }

        public async ValueTask AddResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            string key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
            if (!(await IsResizeObserverSupported()))
            {
                Action<JsonElement> action = (je) => callback.Invoke(new List<ResizeObserverEntry> { new ResizeObserverEntry() });
                AddEventListener("window", "resize", action);
            }
            else
            {
                if (!_domEventListeners.ContainsKey(key))
                {
                    _domEventListeners[key] = new List<DomEventSubscription>();
                    var invoker = CreateSubscriptionInvoker(_domEventListeners[key]);
                    var dotNetObject = DotNetObjectReference.Create(invoker);
                    await _jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Create, key, dotNetObject);
                    await _jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Observe, key, dom);
                }
                _domEventListeners[key].Add(new DomEventSubscription(callback, typeof(List<ResizeObserverEntry>)));
            }
        }

        public async ValueTask RemoveResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            string key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
            if (_domEventListeners.ContainsKey(key))
            {
                var subscription = _domEventListeners[key].SingleOrDefault(s => s.Delegate == (Delegate)callback);
                if (subscription != null)
                {
                    _domEventListeners[key].Remove(subscription);
                }
            }
        }

        public async ValueTask DisposeResizeObserver(ElementReference dom)
        {
            string key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
            if (await IsResizeObserverSupported())
            {
                await _jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Dispose, key);
            }
            _domEventListeners.TryRemove(key, out _);
        }

        public async ValueTask DisconnectResizeObserver(ElementReference dom)
        {
            string key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
            if (await IsResizeObserverSupported())
            {
                await _jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Disconnect, key);
            }
            if (_domEventListeners.ContainsKey(key))
            {
                _domEventListeners[key].Clear();
            }
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

        private async ValueTask<bool> IsResizeObserverSupported() => _isResizeObserverSupported ??= await _jsRuntime.IsResizeObserverSupported();
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
