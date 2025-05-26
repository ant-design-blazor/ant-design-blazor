// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using AntDesign.Core.Helpers;
using AntDesign.Core.JsInterop.ObservableApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.JsInterop;

public class DomEventListener(IJSRuntime jsRuntime, DomEventSubscriptionStore domEventSubscriptionStore) : IDomEventListener
{
    private readonly string _id = Guid.NewGuid().ToString();
    private readonly Dictionary<DomEventKey, IDisposable> _exclusiveDotNetObjectStore = [];
    private readonly Dictionary<DomEventKey, IDisposable> _sharedDotNetObjectStore = [];

    private bool? _isResizeObserverSupported = null;
    private bool _isDisposed;

    private DomEventKey FormatKey(object dom, string eventName)
    {
        var selector = dom is ElementReference eleRef ? eleRef.GetSelector() : dom.ToString();
        if (selector.IsIn("window", "document"))
        {
            return new(selector, eventName, string.Empty);
        }
        return new(selector, eventName, _id);
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
        jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListener, dom, eventName, preventDefault, dotNetObject, stopPropagation);
        _exclusiveDotNetObjectStore.Add(key, dotNetObject);
    }

    public void AddExclusive<T>(object dom, string eventName, Func<T, Task> callback, bool preventDefault = false, bool stopPropagation = false)
    {
        var key = FormatKey(dom, eventName);
        if (_exclusiveDotNetObjectStore.ContainsKey(key))
            return;

        var dotNetObject = DotNetObjectReference.Create(new AsyncInvoker<T>((p) =>
        {
            return callback(p);
        }));
        jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListener, dom, eventName, preventDefault, dotNetObject, stopPropagation);
        _exclusiveDotNetObjectStore.Add(key, dotNetObject);
    }

    public void RemoveExclusive(object dom, string eventName)
    {
        var key = FormatKey(dom, eventName);
        if (_exclusiveDotNetObjectStore.TryGetValue(key, out var dotNetObject))
        {
            jsRuntime.InvokeVoidAsync(JSInteropConstants.RemoveDomEventListener, dom, eventName, dotNetObject);
        }
        _exclusiveDotNetObjectStore.Remove(key);
    }

    public void DisposeExclusive()
    {
        foreach (var (key, dotNetObject) in _exclusiveDotNetObjectStore)
        {
            jsRuntime.InvokeVoidAsync(JSInteropConstants.RemoveDomEventListener, key.Selector, key.EventName, dotNetObject);
        }
        _exclusiveDotNetObjectStore.Clear();
    }

    #region SharedEventListerner

    public virtual void AddShared<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false)
    {
        var key = FormatKey(dom, eventName);
        if (!domEventSubscriptionStore.TryGetValue(key, out var list))
        {
            list = [];
            if (domEventSubscriptionStore.TryAdd(key, list))
            {
                var dotNetObject = DotNetObjectReference.Create(new Invoker<string>((p) =>
                {
                    if (!domEventSubscriptionStore.TryGetValue(key, out var tempList))
                        return;

                    var jsonOpt = JsonSerializerHelper.DefaultOptions;
                    foreach (var subscription in tempList)
                    {
                        var tP = JsonSerializer.Deserialize(p, subscription.Type, jsonOpt);
                        subscription.Delegate.DynamicInvoke(tP);
                    }
                }));

                _sharedDotNetObjectStore.Add(key, dotNetObject);

                jsRuntime.InvokeVoidAsync(JSInteropConstants.AddDomEventListener, dom, eventName, preventDefault, dotNetObject);
            }
            else
            {
                _ = domEventSubscriptionStore.TryGetValue(key, out list);
            }
        }
        list.Add(new DomEventSubscription(callback, typeof(T), _id));
    }

    public virtual void AddShared<T>(object dom, string eventName, Func<T, Task> callback, bool preventDefault = false)
    {
        var key = FormatKey(dom, eventName);
        if (!domEventSubscriptionStore.TryGetValue(key, out var list))
        {
            list = [];
            if (domEventSubscriptionStore.TryAdd(key, list))
            {
                var dotNetObject = DotNetObjectReference.Create(new AsyncInvoker<string>(async (p) =>
                {
                    if (!domEventSubscriptionStore.TryGetValue(key, out var tempList))
                        return;

                    var jsonOpt = JsonSerializerHelper.DefaultOptions;
                    foreach (var subscription in tempList)
                    {
                        var tP = JsonSerializer.Deserialize(p, subscription.Type, jsonOpt);
                        if (subscription.IsAsync)
                        {
                            await (Task)subscription.Delegate.DynamicInvoke(tP);
                        }
                        else
                        {
                            subscription.Delegate.DynamicInvoke(tP);
                        }
                    }
                }));

                _sharedDotNetObjectStore.Add(key, dotNetObject);

                jsRuntime.InvokeVoidAsync(JSInteropConstants.AddDomEventListener, dom, eventName, preventDefault, dotNetObject);
            }
            else
            {
                _ = domEventSubscriptionStore.TryGetValue(key, out list);
            }
        }
        list.Add(new DomEventSubscription(callback, typeof(T), _id, true));
    }

    public void RemoveShared<T>(object dom, string eventName, Action<T> callback)
    {
        var key = FormatKey(dom, eventName);
        if (domEventSubscriptionStore.TryGetValue(key, out var subscriptions))
        {
            var index = subscriptions.FindIndex(s => s.Id == _id && s.Delegate == (Delegate)callback);
            if (index >= 0)
            {
                subscriptions.RemoveAt(index);
            }
        }
    }

    public void RemoveShared<T>(object dom, string eventName, Func<T, Task> callback)
    {
        var key = FormatKey(dom, eventName);
        if (domEventSubscriptionStore.TryGetValue(key, out var subscriptions))
        {
            var index = subscriptions.FindIndex(s => s.Id == _id && s.Delegate == (Delegate)callback);
            if (index >= 0)
            {
                subscriptions.RemoveAt(index);
            }
        }
    }

    public void DisposeShared()
    {
        var find = true;

        while (find)
        {
            var (key, subscription) = domEventSubscriptionStore.FindDomEventSubscription(_id);
            if (key != null && subscription != null)
            {
                var tempList = domEventSubscriptionStore[key];
                tempList.Remove(subscription);

                if (tempList.Count == 0 && _sharedDotNetObjectStore.TryGetValue(key, out var dotNetObject))
                {
                    jsRuntime.InvokeVoidAsync(JSInteropConstants.RemoveDomEventListener, key.Selector, key.EventName, dotNetObject);

                    domEventSubscriptionStore.Remove(key, out var _);
                }
            }
            else
            {
                find = false;
            }
        }
    }

    #endregion SharedEventListerner

    #region ResizeObserver

    public async ValueTask AddResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
    {
        var key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
        if (!await IsResizeObserverSupported())
        {
            AddShared<JsonElement>("window", "resize", Callback, false);

            void Callback(JsonElement _) => callback.Invoke([new ResizeObserverEntry()]);
        }
        else
        {
            if (!domEventSubscriptionStore.TryGetValue(key, out var subscriptions))
            {
                subscriptions = [];
                if (domEventSubscriptionStore.TryAdd(key, subscriptions))
                {
                    await jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Create, key.ToString(), DotNetObjectReference.Create(new Invoker<string>((p) =>
                    {
                        foreach (var subscription in subscriptions)
                        {
                            var tP = JsonSerializer.Deserialize(p, subscription.Type);
                            subscription.Delegate.DynamicInvoke(tP);
                        }
                    })));
                    await jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Observe, key.ToString(), dom);
                }
                else
                {
                    _ = domEventSubscriptionStore.TryGetValue(key, out subscriptions);
                }
            }

            subscriptions.Add(new DomEventSubscription(callback, typeof(List<ResizeObserverEntry>), _id));
        }
    }

    public async ValueTask AddResizeObserver(ElementReference dom, Func<List<ResizeObserverEntry>, Task> callback)
    {
        var key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
        if (!await IsResizeObserverSupported())
        {
            AddShared<JsonElement>("window", "resize", AsyncCallback, false);

            async void AsyncCallback(JsonElement _) => await callback.Invoke([new ResizeObserverEntry()]);
        }
        else
        {
            if (!domEventSubscriptionStore.TryGetValue(key, out var subscriptions))
            {
                subscriptions = [];
                if (domEventSubscriptionStore.TryAdd(key, subscriptions))
                {
                    await jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Create, key.ToString(), DotNetObjectReference.Create(new AsyncInvoker<string>(async (p) =>
                    {
                        foreach (var subscription in subscriptions)
                        {
                            var tP = JsonSerializer.Deserialize(p, subscription.Type);
                            if (subscription.IsAsync)
                            {
                                await (Task)subscription.Delegate.DynamicInvoke(tP);
                            }
                            else
                            {
                                subscription.Delegate.DynamicInvoke(tP);
                            }
                        }
                    })));
                    await jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Observe, key.ToString(), dom);
                }
                else
                {
                    _ = domEventSubscriptionStore.TryGetValue(key, out subscriptions);
                }
            }

            subscriptions.Add(new DomEventSubscription(callback, typeof(List<ResizeObserverEntry>), _id, true));
        }
    }

    public async ValueTask RemoveResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
    {
        var key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
        if (domEventSubscriptionStore.TryGetValue(key, out var list))
        {
            var index = list.FindIndex(s => s.Delegate == (Delegate)callback);
            if (index >= 0)
            {
                list.RemoveAt(index);
            }
        }

        await Task.CompletedTask;
    }

    public async ValueTask RemoveResizeObserver(ElementReference dom, Func<List<ResizeObserverEntry>, Task> callback)
    {
        var key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
        if (domEventSubscriptionStore.TryGetValue(key, out var list))
        {
            var index = list.FindIndex(s => s.Delegate == (Delegate)callback && s.IsAsync);
            if (index >= 0)
            {
                list.RemoveAt(index);
            }
        }

        await Task.CompletedTask;
    }

    public async ValueTask DisposeResizeObserver(ElementReference dom)
    {
        var key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
        if (await IsResizeObserverSupported())
        {
            await jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Dispose, key.ToString());
        }
        domEventSubscriptionStore.TryRemove(key, out _);
    }

    public async ValueTask DisconnectResizeObserver(ElementReference dom)
    {
        var key = FormatKey(dom.Id, nameof(JSInteropConstants.ObserverConstants.Resize));
        if (await IsResizeObserverSupported())
        {
            await jsRuntime.InvokeVoidAsync(JSInteropConstants.ObserverConstants.Resize.Disconnect, key.ToString());
        }
        if (domEventSubscriptionStore.TryGetValue(key, out var list))
        {
            list.Clear();
        }
    }

    private async ValueTask<bool> IsResizeObserverSupported() => _isResizeObserverSupported ??= await jsRuntime.IsResizeObserverSupported();

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

            jsRuntime.InvokeAsync<string>(JSInteropConstants.AddDomEventListenerToFirstChild, dom, eventName, preventDefault, dotNetObject);
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

public class Invoker<T>(Action<T> invoker)
{
    [JSInvokable]
    public void Invoke(T param)
    {
        invoker.Invoke(param);
    }
}

public class AsyncInvoker<T>
{
    private readonly Func<T, Task> _invoker;

    public AsyncInvoker(Func<T, Task> invoker)
    {
        _invoker = invoker;
    }

    [JSInvokable]
    public Task Invoke(T param)
    {
        return _invoker(param);
    }
}
