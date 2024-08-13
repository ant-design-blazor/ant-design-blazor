using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.JsInterop.ObservableApi;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Tests
{
    class TestDomEventListerner : IDomEventListener
    {
        public void AddEventListenerToFirstChild(object dom, string eventName, Action<JsonElement> callback, bool preventDefault = false)
        {
            return;
        }

        public void AddEventListenerToFirstChild<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false)
        {
            return;
        }

        public void AddExclusive<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false, bool stopPropagation = false)
        {
            return;
        }

        public void AddShared<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false)
        {
            return;
        }

        public void Dispose()
        {
            return;
        }

        public void DisposeExclusive()
        {
            return;
        }

        public void DisposeShared()
        {
            return;
        }

        public void RemoveExclusive(object dom, string eventName)
        {
            return;
        }

        public void RemoveShared<T>(object dom, string eventName, Action<T> callback)
        {
            return;
        }

#if NET5_0_OR_GREATER
        public ValueTask AddResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask DisconnectResizeObserver(ElementReference dom)
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask DisposeResizeObserver(ElementReference dom)
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask RemoveResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            return ValueTask.CompletedTask;
        }
#else
        public async ValueTask AddResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            await Task.CompletedTask;
        }

        public async ValueTask DisconnectResizeObserver(ElementReference dom)
        {
            await Task.CompletedTask;
        }

        public async ValueTask DisposeResizeObserver(ElementReference dom)
        {
            await Task.CompletedTask;
        }

        public async ValueTask RemoveResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            await Task.CompletedTask;
        }
#endif
    }
}
