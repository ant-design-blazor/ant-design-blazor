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

        ValueTask AddResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback);

        void AddShared<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false);

        ValueTask DisconnectResizeObserver(ElementReference dom);

        void DisposeExclusive();

        ValueTask DisposeResizeObserver(ElementReference dom);

        void RemoveExclusive(object dom, string eventName);

        ValueTask RemoveResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback);

        void RemoveShared<T>(object dom, string eventName, Action<T> callback);

        void DisposeShared();
    }
}
