﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AntDesign.JsInterop
{
    public class DomEventSubscriptionStore : ConcurrentDictionary<DomEventKey, List<DomEventSubscription>>
    {
        public (DomEventKey key, DomEventSubscription subscription) FindDomEventSubscription(string id)
        {
            DomEventKey key = null;
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

    public record DomEventKey(string Selector, string EventName, string ListenerId)
    {
        public override string ToString()
        {
            return $"{Selector}-{EventName}-{ListenerId}";
        }
    }
}
