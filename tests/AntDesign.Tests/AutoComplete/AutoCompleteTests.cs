// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.JsInterop;
using Bunit;
using FluentAssertions;
using Xunit;

namespace AntDesign.Tests.AutoComplete
{
    public class AutoCompleteTests : AntDesignTestBase
    {
        [Fact]
        public void AutoComplete_apply_class()
        {
            JSInterop.Setup<HtmlElement>("AntDesign.interop.domInfoHelper.getInfo", _ => true);
            var component = RenderComponent<AutoComplete<string>>(parameters => parameters
                .Add(x => x.Class, "test"));

            component.Find("input").ClassList.Contains("test").Should().BeTrue();
        }
    }
}
