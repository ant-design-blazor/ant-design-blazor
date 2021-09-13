using System;
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
    public class DomEventListener : IDomEventListener
    {
        private Dictionary<string, IDisposable> _dotNetObjectStore = new();
        private bool? _isResizeObserverSupported = null;

        private readonly IJSRuntime _jsRuntime;
        private readonly DomEventSubscriptionStore _domEventSubscriptionsStore;
        private readonly string _id;

        public DomEventListener(IJSRuntime jsRuntime, DomEventSubscriptionStore domEventSubscriptionStore)
        {
            _jsRuntime = jsRuntime;
            _domEventSubscriptionsStore = domEventSubscriptionStore;
            _id = Guid.NewGuid().ToString();
        }

        private string FormatKey(object dom, string eventName)
        {
            var selector = dom is ElementReference eleRef ? eleRef.Id : dom.ToString();
            if (selector.IsIn("window", "document"))
            {
                return $"DEL-{selector}-{eventName}";
            }
            return $"DEL-{_id}-{selector}-{eventName}";
        }

        public void AddExclusive<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false)
        {
            var key = FormatKey(dom, eventName);
            if (_dotNetObjectStore.ContainsKey(key))
                return;

            var dotNetObject = DotNetObjectReference.Create(new Invoker<T>((p) =>
            {
                callback(p);
            }));
            _jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListener, dom, eventName, preventDefault, dotNetObject);
            _dotNetObjectStore.Add(key, dotNetObject);
        }

        public void RemoveExclusive(object dom, string eventName)
        {
            var key = FormatKey(dom, eventName);
            if (_dotNetObjectStore.TryGetValue(key, out IDisposable value))
            {
                value.Dispose();
            }
            _dotNetObjectStore.Remove(key);
        }

        public void DisposeExclusive()
        {
            foreach (var (k, v) in _dotNetObjectStore)
            {
                v.Dispose();
            }
            _dotNetObjectStore.Clear();
        }

        #region SharedEventListerner

        public virtual void AddShared<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false)
        {
            string key = FormatKey(dom, eventName);
            if (!_domEventSubscriptionsStore.ContainsKey(key))
            {
                _domEventSubscriptionsStore[key] = new List<DomEventSubscription>();

                var dotNetObject = DotNetObjectReference.Create(new Invoker<string>((p) =>
                {
                    for (var i = 0; i < _domEventSubscriptionsStore[key].Count; i++)
                    {
                        var subscription = _domEventSubscriptionsStore[key][i];
                        object tP = JsonSerializer.Deserialize(p, subscription.Type);
                        subscription.Delegate.DynamicInvoke(tP);
                    }
                }));

                _jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListener, dom, eventName, preventDefault, dotNetObject);
            }
            _domEventSubscriptionsStore[key].Add(new DomEventSubscription(callback, typeof(T), _id));
        }

        public void RemoveShared<T>(object dom, string eventName, Action<T> callback)
        {
            string key = FormatKey(dom, eventName);
            if (_domEventSubscriptionsStore.ContainsKey(key))
            {
                var subscription = _domEventSubscriptionsStore[key].SingleOrDefault(s => s.Delegate == (Delegate)callback);
                if (subscription != null && subscription.Id == _id)
                {
                    _domEventSubscriptionsStore[key].Remove(subscription);
                }
            }
        }

        public void DisposeShared()
        {
            bool find = true;

            while (find)
            {
                var (key, subscription) = _domEventSubscriptionsStore.FindDomEventSubscription(_id);
                if (!string.IsNullOrEmpty(key) && subscription != null)
                {
                    _domEventSubscriptionsStore[key].Remove(subscription);
                }
                else
                    find = false;
            }
        }

        #endregion

        #region ResizeObserver
        public async ValueTask AddResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            string key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
            if (!(await IsResizeObserverSupported()))
            {
                Action<JsonElement> action = (je) => callback.Invoke(new List<ResizeObserverEntry> { new ResizeObserverEntry() });
                AddShared<JsonElement>("window", "resize", action, false);
            }
            else
            {
                if (!_domEventSubscriptionsStore.ContainsKey(key))
                {
                    _domEventSubscriptionsStore[key] = new List<DomEventSubscription>();
                    await _jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Create, key, DotNetObjectReference.Create(new Invoker<string>((p) =>
                    {
                        for (var i = 0; i < _domEventSubscriptionsStore[key].Count; i++)
                        {
                            var subscription = _domEventSubscriptionsStore[key][i];
                            object tP = JsonSerializer.Deserialize(p, subscription.Type);
                            subscription.Delegate.DynamicInvoke(tP);
                        }
                    })));
                    await _jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Observe, key, dom);
                }
                _domEventSubscriptionsStore[key].Add(new DomEventSubscription(callback, typeof(List<ResizeObserverEntry>), _id));
            }
        }

        public async ValueTask RemoveResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            string key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
            if (_domEventSubscriptionsStore.ContainsKey(key))
            {
                var subscription = _domEventSubscriptionsStore[key].SingleOrDefault(s => s.Delegate == (Delegate)callback);
                if (subscription != null)
                {
                    _domEventSubscriptionsStore[key].Remove(subscription);
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
            _domEventSubscriptionsStore.TryRemove(key, out _);
        }

        public async ValueTask DisconnectResizeObserver(ElementReference dom)
        {
            string key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
            if (await IsResizeObserverSupported())
            {
                await _jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Disconnect, key);
            }
            if (_domEventSubscriptionsStore.ContainsKey(key))
            {
                _domEventSubscriptionsStore[key].Clear();
            }
        }

        private async ValueTask<bool> IsResizeObserverSupported() => _isResizeObserverSupported ??= await _jsRuntime.IsResizeObserverSupported();

        #endregion

        #region EventListenerToFirstChild

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

        private void AddEventListenerToFirstChildInternal<T>(object dom, string eventName, bool preventDefault, Action<T> callback)
        {
            var key = FormatKey(dom, eventName);
            if (!_dotNetObjectStore.ContainsKey(key))
            {
                var dotNetObject = DotNetObjectReference.Create(new Invoker<T>((p) =>
                {
                    callback?.Invoke(p);
                }));

                _jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListenerToFirstChild, dom, eventName, preventDefault, dotNetObject);
                _dotNetObjectStore.Add(key, dotNetObject);
            }
        }

        #endregion

        public void Dispose()
        {
            DisposeExclusive();
            DisposeShared();
        }
    }

    public class Invoker<T>
    {
        private Action<T> _action;

        public Invoker(Action<T> invoker)
        {
            _action = invoker;
        }

        [JSInvokable]
        public void Invoke(T param)
        {
            _action.Invoke(param);
        }
    }
}
