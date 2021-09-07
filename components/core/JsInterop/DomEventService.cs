using Microsoft.JSInterop;

namespace AntDesign.JsInterop
{
    public class DomEventService
    {
        private DomEventSubscriptionStore _domEventSubscriptionStore = new();

        private readonly IJSRuntime _jsRuntime;

        public DomEventService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public virtual IDomEventListener CreateDomEventListerner()
        {
            return new DomEventListener(_jsRuntime, _domEventSubscriptionStore);
        }
    }
}
