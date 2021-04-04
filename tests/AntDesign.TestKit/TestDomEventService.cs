// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace AntDesign.Tests
{
    public class TestDomEventService : AntDesign.JsInterop.DomEventService
    {
        public TestDomEventService(IJSRuntime js) : base(js)
        {

        }

        public override void AddEventListener<T>(object dom, string eventName, Action<T> callback, bool exclusive = true, bool preventDefault = false)
        {
            return;
        }
    }
}
