using System;
using System.Collections.Generic;
using Microsoft.JSInterop;

namespace AntBlazor.JsInterop
{
    public class DomEventService
    {
        private Dictionary<string, Func<object>> domEventListeners = new Dictionary<string, Func<object>>();

        private readonly IJSRuntime _jsRuntime;

        internal event Action onResize;

        public DomEventService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public void RegisterResizeListener()
        {
            if (!domEventListeners.ContainsKey("resize"))
            {
                _jsRuntime.InvokeAsync<string>(JSInteropConstants.addDomEventListener, "resize", DotNetObjectReference.Create(new Invoker(() =>
                 {
                     onResize?.Invoke();
                 })));
            }
        }
    }

    public class Invoker
    {
        private Action action;

        public Invoker(Action invoker)
        {
            this.action = invoker;
        }

        [JSInvokable]
        public void Invoke()
        {
            action.Invoke();
        }
    }
}