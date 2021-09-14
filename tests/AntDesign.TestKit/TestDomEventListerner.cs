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

        public void AddExclusive<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false)
        {
            return;
        }

        public async ValueTask AddResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            return;
        }

        public void AddShared<T>(object dom, string eventName, Action<T> callback, bool preventDefault = false)
        {
            return;
        }

        public async ValueTask DisconnectResizeObserver(ElementReference dom)
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

        public async ValueTask DisposeResizeObserver(ElementReference dom)
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

        public async ValueTask RemoveResizeObserver(ElementReference dom, Action<List<ResizeObserverEntry>> callback)
        {
            return;
        }

        public void RemoveShared<T>(object dom, string eventName, Action<T> callback)
        {
            return;
        }
    }
}
