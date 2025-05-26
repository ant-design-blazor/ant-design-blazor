// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.JsInterop.ObservableApi;
using Microsoft.AspNetCore.Components;

namespace AntDesign.JsInterop
{
    public interface IDomEventListener : IDisposable
    {
        void AddEventListenerToFirstChild(object dom, string eventName, Action<JsonElement> callback, bool preventDefault = false);

        void AddEventListenerToFirstChild<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false);

        void AddExclusive<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false, bool stopPropagation = false);

        void AddExclusive<T>(object dom, string eventName, Func<T, Task> callback, bool preventDefault = false, bool stopPropagation = false);

        ValueTask AddResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback);

        ValueTask AddResizeObserver(ElementReference dom, Func<List<ResizeObserverEntry>, Task> callback);

        void AddShared<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false);

        void AddShared<T>(object dom, string eventName, Func<T, Task> callback, bool preventDefault = false);

        ValueTask DisconnectResizeObserver(ElementReference dom);

        void DisposeExclusive();

        ValueTask DisposeResizeObserver(ElementReference dom);

        void RemoveExclusive(object dom, string eventName);

        ValueTask RemoveResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback);

        ValueTask RemoveResizeObserver(ElementReference dom, Func<List<ResizeObserverEntry>, Task> callback);

        void RemoveShared<T>(object dom, string eventName, Action<T> callback);

        void RemoveShared<T>(object dom, string eventName, Func<T, Task> callback);

        void DisposeShared();
    }
}
