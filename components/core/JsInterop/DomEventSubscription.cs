using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AntDesign.JsInterop
{
    internal class DomEventSubscriptionStore : ConcurrentDictionary<string, List<DomEventSubscription>> {}

    internal class DomEventSubscription
    {
        internal Delegate Delegate { get; set; }
        internal Type Type { get; set; }

        public DomEventSubscription(Delegate @delegate, Type type)
        {
            Delegate = @delegate;
            Type = type;
        }
    }
}
