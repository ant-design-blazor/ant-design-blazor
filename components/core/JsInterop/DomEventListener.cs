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
        private Dictionary<DomEventKey, IDisposable> _exclusiveDotNetObjectStore = new();
        private Dictionary<DomEventKey, IDisposable> _sharedDotNetObjectStore = new();
        private bool? _isResizeObserverSupported = null;

        private readonly IJSRuntime _jsRuntime;
        private readonly DomEventSubscriptionStore _domEventSubscriptionsStore;
        private readonly string _id;
        private bool _isDisposed;

        public DomEventListener(IJSRuntime jsRuntime, DomEventSubscriptionStore domEventSubscriptionStore)
        {
            _jsRuntime = jsRuntime;
            _domEventSubscriptionsStore = domEventSubscriptionStore;
            _id = Guid.NewGuid().ToString();
        }

        private DomEventKey FormatKey(object dom, string eventName)
        {
            var selector = dom is ElementReference eleRef ? eleRef.GetSelector() : dom.ToString();
            if (selector.IsIn("window", "document"))
            {
                return new DomEventKey(selector, eventName, "");
            }
            return new DomEventKey(selector, eventName, _id);
        }

        public void AddExclusive<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false, bool stopPropagation = false)
        {
            var key = FormatKey(dom, eventName);
            if (_exclusiveDotNetObjectStore.ContainsKey(key))
                return;

            var dotNetObject = DotNetObjectReference.Create(new Invoker<T>((p) =>
            {
                callback(p);
            }));
            _jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListener, dom, eventName, preventDefault, dotNetObject, stopPropagation);
            _exclusiveDotNetObjectStore.Add(key, dotNetObject);
        }

        public void RemoveExclusive(object dom, string eventName)
        {
            var key = FormatKey(dom, eventName);
            if (_exclusiveDotNetObjectStore.TryGetValue(key, out IDisposable dotNetObject))
            {
                _jsRuntime.InvokeVoidAsync(JSInteropConstants.RemoveDomEventListener, dom, eventName, dotNetObject);
            }
            _exclusiveDotNetObjectStore.Remove(key);
        }

        public void DisposeExclusive()
        {
            foreach (var (key, dotNetObject) in _exclusiveDotNetObjectStore)
            {
                _jsRuntime.InvokeVoidAsync(JSInteropConstants.RemoveDomEventListener, key.Selector, key.EventName, dotNetObject);
            }
            _exclusiveDotNetObjectStore.Clear();
        }

        #region SharedEventListerner

        public virtual void AddShared<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false)
        {
            var key = FormatKey(dom, eventName);
            if (!_domEventSubscriptionsStore.ContainsKey(key))
            {
                _domEventSubscriptionsStore[key] = new List<DomEventSubscription>();

                var dotNetObject = DotNetObjectReference.Create(new Invoker<string>((p) =>
                {
                    if (!_domEventSubscriptionsStore.ContainsKey(key))
                        return;

                    for (var i = 0; i < _domEventSubscriptionsStore[key].Count; i++)
                    {
                        var subscription = _domEventSubscriptionsStore[key][i];
                        var tP = JsonSerializer.Deserialize(p, subscription.Type, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                        subscription.Delegate.DynamicInvoke(tP);
                    }
                }));

                _sharedDotNetObjectStore.Add(key, dotNetObject);

                _jsRuntime.InvokeVoidAsync(JSInteropConstants.AddDomEventListener, dom, eventName, preventDefault, dotNetObject);
            }
            _domEventSubscriptionsStore[key].Add(new DomEventSubscription(callback, typeof(T), _id));
        }

        public void RemoveShared<T>(object dom, string eventName, Action<T> callback)
        {
            var key = FormatKey(dom, eventName);
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
                if (key != null && subscription != null)
                {
                    _domEventSubscriptionsStore[key].Remove(subscription);

                    if (_domEventSubscriptionsStore[key].Count == 0 && _sharedDotNetObjectStore.ContainsKey(key))
                    {
                        var dotNetObject = _sharedDotNetObjectStore[key];

                        _jsRuntime.InvokeVoidAsync(JSInteropConstants.RemoveDomEventListener, key.Selector, key.EventName, dotNetObject);

                        _domEventSubscriptionsStore.Remove(key, out var _);
                    }
                }
                else
                    find = false;
            }
        }

        #endregion SharedEventListerner

        #region ResizeObserver

        public async ValueTask AddResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            var key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
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
                    await _jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Create, key.ToString(), DotNetObjectReference.Create(new Invoker<string>((p) =>
                    {
                        for (var i = 0; i < _domEventSubscriptionsStore[key].Count; i++)
                        {
                            var subscription = _domEventSubscriptionsStore[key][i];
                            object tP = JsonSerializer.Deserialize(p, subscription.Type);
                            subscription.Delegate.DynamicInvoke(tP);
                        }
                    })));
                    await _jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Observe, key.ToString(), dom);
                }
                _domEventSubscriptionsStore[key].Add(new DomEventSubscription(callback, typeof(List<ResizeObserverEntry>), _id));
            }
        }

        public async ValueTask RemoveResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            var key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
            if (_domEventSubscriptionsStore.ContainsKey(key))
            {
                var subscription = _domEventSubscriptionsStore[key].SingleOrDefault(s => s.Delegate == (Delegate)callback);
                if (subscription != null)
                {
                    _domEventSubscriptionsStore[key].Remove(subscription);
                }
            }

            await Task.CompletedTask;
        }

        public async ValueTask DisposeResizeObserver(ElementReference dom)
        {
            var key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
            if (await IsResizeObserverSupported())
            {
                await _jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Dispose, key.ToString());
            }
            _domEventSubscriptionsStore.TryRemove(key, out _);
        }

        public async ValueTask DisconnectResizeObserver(ElementReference dom)
        {
            var key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
            if (await IsResizeObserverSupported())
            {
                await _jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Disconnect, key.ToString());
            }
            if (_domEventSubscriptionsStore.ContainsKey(key))
            {
                _domEventSubscriptionsStore[key].Clear();
            }
        }

        private async ValueTask<bool> IsResizeObserverSupported() => _isResizeObserverSupported ??= await _jsRuntime.IsResizeObserverSupported();

        #endregion ResizeObserver

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
            if (!_exclusiveDotNetObjectStore.ContainsKey(key))
            {
                var dotNetObject = DotNetObjectReference.Create(new Invoker<T>((p) =>
                {
                    callback?.Invoke(p);
                }));

                _jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListenerToFirstChild, dom, eventName, preventDefault, dotNetObject);
                _exclusiveDotNetObjectStore.Add(key, dotNetObject);
            }
        }

        #endregion EventListenerToFirstChild

        public void Dispose()
        {
            if (!_isDisposed)
            {
                DisposeExclusive();
                DisposeShared();
                _isDisposed = true;
            }
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
