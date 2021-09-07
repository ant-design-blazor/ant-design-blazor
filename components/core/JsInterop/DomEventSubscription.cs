using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AntDesign.JsInterop
{
    public class DomEventSubscriptionStore : ConcurrentDictionary<string, List<DomEventSubscription>>
    {
        public (string key, DomEventSubscription subscription) FindDomEventSubscription(string id)
        {
            string key = string.Empty;
            DomEventSubscription subscription = null;

            foreach (var (k, subscriptionList) in this)
            {
                key = k;

                foreach (var item in subscriptionList)
                {
                    if (item.Id == id)
                    {
                        subscription = item;
                        break;
                    }
                }

                if (subscription != null)
                    break;
            }
            return (key, subscription);
        }
    }

    public class DomEventSubscription
    {
        internal Delegate Delegate { get; set; }
        internal Type Type { get; set; }
        internal string Id { get; set; }

        public DomEventSubscription(Delegate @delegate, Type type, string id)
        {
            Delegate = @delegate;
            Type = type;
            Id = id;
        }
    }
}
