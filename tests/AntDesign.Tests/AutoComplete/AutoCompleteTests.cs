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
        public void AutoComplete_renders_the_v6_semantic_root_around_its_input()
        {
            JSInterop.Setup<HtmlElement>("AntDesign.interop.domInfoHelper.getInfo", _ => true);

            var component = RenderComponent<AutoComplete<string>>();

            component.Find(".ant-select-auto-complete").QuerySelector("input").Should().NotBeNull();
        }

        [Fact]
        public void Semantic_popup_list_slots_apply_to_parent_and_option_nodes()
        {
            var autocomplete = new AntDesign.AutoComplete<string>
            {
                ClassNames = new Dictionary<string, string>
                {
                    ["popup.list"] = "custom-list",
                    ["popup.listItem"] = "custom-option"
                },
                Styles = new Dictionary<string, string>
                {
                    ["popup.list"] = "background-color: rgb(245, 250, 255)",
                    ["popup.listItem"] = "padding-inline-start: 20px"
                }
            };

            var autocompleteRef = (IAutoCompleteRef)autocomplete;
            autocompleteRef.GetSemanticClass("popup.list", null).Should().Be("custom-list");
            autocompleteRef.GetSemanticStyle("popup.list", "max-height:256px").Should()
                .Contain("max-height:256px").And.Contain("background-color: rgb(245, 250, 255)");

            var option = RenderComponent<AutoCompleteOption>(parameters => parameters
                .AddCascadingValue(autocompleteRef)
                .Add(x => x.Value, "Seattle")
                .Add(x => x.Class, "option-base")
                .Add(x => x.Style, "color: navy"));

            option.Find(".custom-option").ClassList.Should().Contain("ant-select-item").And.Contain("option-base");
            option.Find(".custom-option").GetAttribute("style").Should()
                .Contain("color: navy").And.Contain("padding-inline-start: 20px");
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
