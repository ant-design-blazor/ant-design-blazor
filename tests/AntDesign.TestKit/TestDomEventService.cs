// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.JsInterop;
using Microsoft.JSInterop;
using Moq;

namespace AntDesign.Tests
{
    public class TestDomEventService : AntDesign.JsInterop.DomEventService
    {
        public Mock<IDomEventListener> MockedDomEventListener { get; set; }
        public TestDomEventService(IJSRuntime js, Mock<IDomEventListener> mock = null) : base(js)
        {
            if (mock is not null)
            {
                MockedDomEventListener = mock;
            }
        }

        public override IDomEventListener CreateDomEventListerner()
        {
            return MockedDomEventListener?.Object ?? new TestDomEventListerner();
        }
    }
}
