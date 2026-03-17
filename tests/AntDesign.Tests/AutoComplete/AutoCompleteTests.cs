// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.JsInterop;
using Bunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [Fact]
        public async Task Dropdown_refreshes_when_Options_change()
        {
            // Arrange: DOM info interop required by the component during render
            JSInterop.Setup<HtmlElement>("AntDesign.interop.domInfoHelper.getInfo", _ => true);
            JSInterop.Setup<AntDesign.JsInterop.DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
                .SetResult(new AntDesign.JsInterop.DomRect());
            // overlay helper is invoked when showing dropdowns; just return a dummy position
            JSInterop.Setup<object>("AntDesign.interop.overlayHelper.addOverlayToContainer", _ => true);


            var options = new List<string> { "one", "two", "three" };
            var cut = RenderComponent<AutoComplete<string>>(parameters => parameters
                .Add(p => p.Options, options)
                .Add(p => p.AllowFilter, false));

            // we don't need to open the overlay; just verify internal state updates
            // initial internal list should reflect the provided options
            var field0 = cut.Instance.GetType().GetField("_filteredOptions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var list0 = field0?.GetValue(cut.Instance) as System.Collections.IEnumerable;
            var initStrings = new List<string>();
            if (list0 != null)
            {
                foreach (var o in list0)
                {
                    if (o != null)
                    {
                        var prop = o.GetType().GetProperty("Label");
                        initStrings.Add(prop?.GetValue(o)?.ToString());
                    }
                }
            }
            initStrings.Should().Equal(new[] { "one", "two", "three" });

            // update the options collection while panel is open
            options = new List<string> { "alpha", "beta" };
            cut.SetParametersAndRender(parameters => parameters.Add(p => p.Options, options));

            // inspect internal _filteredOptions field to make sure it's updated
            var inst = cut.Instance;
            var field = inst.GetType().GetField("_filteredOptions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var internalList = field?.GetValue(inst) as System.Collections.IEnumerable;
            var internalStrings = new List<string>();
            if (internalList != null)
            {
                foreach (var o in internalList)
                {
                    if (o != null)
                    {
                        var prop = o.GetType().GetProperty("Label");
                        var val = prop?.GetValue(o)?.ToString();
                        internalStrings.Add(val);
                    }
                }
            }
            internalStrings.Should().Equal(new[] { "alpha", "beta" }, "internal options should update");

        }
    }
}
