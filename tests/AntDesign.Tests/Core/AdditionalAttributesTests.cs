// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bunit;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace AntDesign.Tests.Core
{
    public class AdditionalAttributesTests : AntDesignTestBase
    {
        private sealed class AttributeProbe : AntDomComponentBase { }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, 0)]
        [InlineData(null, 1)]
        [InlineData(0, null)]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(1, null)]
        [InlineData(1, 0)]
        public void Empty_or_missing_input_reuses_the_other_dictionary(int? sharedCount, int? legacyCount)
        {
            IReadOnlyDictionary<string, object>? Create(int? count) => count == null ? null :
                new System.Collections.ObjectModel.ReadOnlyDictionary<string, object>(
                    Enumerable.Range(0, count.Value).ToDictionary(i => $"data-{i}", i => (object)i));
            var shared = Create(sharedCount);
            var legacy = Create(legacyCount);
            using var probe = new AttributeProbe { AdditionalAttributes = shared! };

            var result = probe.GetAdditionalAttributes(legacy!);

            var expected = shared == null ? legacy : legacy == null ? shared : sharedCount == 0 ? legacy : shared;
            Assert.Same(expected, result);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Merging_handles_disjoint_keys_and_different_input_comparers(bool readOnly, bool overlap)
        {
            var shared = Enumerable.Range(0, 8).ToDictionary(i => $"DATA-SHARED-{i}", i => (object)$"shared-{i}", StringComparer.OrdinalIgnoreCase);
            var legacy = Enumerable.Range(0, 8).ToDictionary(i => overlap ? $"data-shared-{i}" : $"data-legacy-{i}", i => (object)$"legacy-{i}");
            IReadOnlyDictionary<string, object> Wrap(Dictionary<string, object> source) => readOnly
                ? new System.Collections.ObjectModel.ReadOnlyDictionary<string, object>(source) : source;
            using var probe = new AttributeProbe { AdditionalAttributes = Wrap(shared) };

            var result = probe.GetAdditionalAttributes(Wrap(legacy));

            Assert.Equal(overlap ? 8 : 16, result.Count);
            for (var i = 0; i < 8; i++)
            {
                Assert.Equal(overlap ? $"legacy-{i}" : $"shared-{i}", result[$"data-shared-{i}"]);
                if (!overlap) Assert.Equal($"legacy-{i}", result[$"DATA-LEGACY-{i}"]);
                Assert.Equal($"shared-{i}", shared[$"DATA-SHARED-{i}"]);
            }
        }

        [Fact]
        public void Merging_preserves_precedence_and_observes_mutations_without_modifying_inputs()
        {
            var shared = new Dictionary<string, object> { ["DATA-SHARED"] = "shared", ["title"] = "shared" };
            var legacy = new Dictionary<string, object> { ["data-legacy"] = "legacy", ["TITLE"] = "legacy" };
            using var probe = new AttributeProbe { AdditionalAttributes = shared };
            var first = probe.GetAdditionalAttributes(legacy);
            Assert.Equal(3, first.Count);
            Assert.Equal("legacy", first["title"]);
            Assert.Equal("shared", first["data-shared"]);
            Assert.Equal(2, shared.Count);
            Assert.Equal(2, legacy.Count);
            Assert.Equal("shared", shared["title"]);

            shared["DATA-SHARED"] = "updated";
            legacy.Remove("TITLE");
            var second = probe.GetAdditionalAttributes(legacy);
            Assert.Equal("updated", second["data-shared"]);
            Assert.Equal("shared", second["title"]);
            Assert.Equal("shared", first["data-shared"]);
            Assert.Equal("legacy", first["title"]);
        }

        public AdditionalAttributesTests()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            JSInterop.Setup<DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true).SetResult(new DomRect());
            JSInterop.Setup<AntDesign.Internal.TextAreaInfo>(JSInteropConstants.InputComponentHelper.GetTextAreaInfo, _ => true)
                .SetResult(new AntDesign.Internal.TextAreaInfo { LineHeight = 10 });
        }

        [Fact]
        public void Components_have_at_most_one_catch_all_parameter()
        {
            foreach (var type in typeof(AntComponentBase).Assembly.GetTypes()
                .Where(t => typeof(IComponent).IsAssignableFrom(t)))
            {
                var parameters = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.GetCustomAttribute<ParameterAttribute>()?.CaptureUnmatchedValues == true);
                Assert.True(parameters.Count() <= 1, type.FullName);
            }
        }

        [Fact]
        public void Button_captures_unmatched_attributes_on_the_native_button()
        {
            var cut = RenderComponent<AntDesign.Button>(p => p
                .AddUnmatched("data-testid", "save-button")
                .AddUnmatched("aria-describedby", "save-help")
                .AddUnmatched("form", "editor")
                .AddUnmatched("name", "action")
                .AddUnmatched("value", "save")
                .AddUnmatched("hidden", true));

            var button = cut.Find("button");
            Assert.Equal("save-button", button.GetAttribute("data-testid"));
            Assert.Equal("save-help", button.GetAttribute("aria-describedby"));
            Assert.Equal("editor", button.GetAttribute("form"));
            Assert.Equal("action", button.GetAttribute("name"));
            Assert.Equal("save", button.GetAttribute("value"));
            Assert.True(button.HasAttribute("hidden"));
        }

        [Fact]
        public void Button_preserves_managed_attributes_and_click_handler()
        {
            var clicked = false;
            var replaced = false;
            var cut = RenderComponent<AntDesign.Button>(p => p
                .Add(x => x.Id, "save")
                .Add(x => x.Class, "custom")
                .Add(x => x.Style, "color: red")
                .Add(x => x.AriaLabel, "Save")
                .Add(x => x.OnClick, _ => clicked = true)
                .Add(x => x.AdditionalAttributes, new Dictionary<string, object>
                {
                    ["id"] = "wrong", ["class"] = "wrong", ["style"] = "wrong",
                    ["aria-label"] = "wrong", ["type"] = "submit",
                    ["onclick"] = EventCallback.Factory.Create<MouseEventArgs>(this, () => replaced = true)
                }));

            var button = cut.Find("button");
            Assert.Equal("save", button.Id);
            Assert.Contains("ant-btn", button.ClassList);
            Assert.Contains("custom", button.ClassList);
            Assert.Equal("color: red;", button.GetAttribute("style"));
            Assert.Equal("Save", button.GetAttribute("aria-label"));
            Assert.Equal("button", button.GetAttribute("type"));
            button.Click();
            Assert.True(clicked);
            Assert.False(replaced);

            cut.SetParametersAndRender(p => p.Add(x => x.Disabled, true)
                .Add(x => x.AdditionalAttributes, new Dictionary<string, object> { ["disabled"] = false }));
            Assert.True(cut.Find("button").HasAttribute("disabled"));
        }

        [Fact]
        public void Attributes_are_updated_and_removed_on_rerender()
        {
            var attributes = new Dictionary<string, object> { ["data-testid"] = "before", ["hidden"] = true };
            var cut = RenderComponent<AntDesign.Button>(p => p.Add(x => x.AdditionalAttributes, attributes));
            attributes["data-testid"] = "after";
            attributes["hidden"] = false;
            cut.SetParametersAndRender(p => p.Add(x => x.AdditionalAttributes, attributes));
            Assert.Equal("after", cut.Find("button").GetAttribute("data-testid"));
            Assert.False(cut.Find("button").HasAttribute("hidden"));

            cut.SetParametersAndRender(p => p.Add(x => x.AdditionalAttributes, null));
            Assert.False(cut.Find("button").HasAttribute("data-testid"));
        }

        private void AssertTarget<TComponent>(string selector) where TComponent : AntDomComponentBase
        {
            var cut = RenderComponent<TComponent>(p => p.AddUnmatched("data-testid", "control")
                .AddUnmatched("aria-describedby", "help"));
            var target = cut.Find(selector);
            Assert.Equal("control", target.GetAttribute("data-testid"));
            Assert.Equal("help", target.GetAttribute("aria-describedby"));
            Assert.Single(cut.FindAll("[data-testid='control']"));
        }

        [Fact]
        public void Input_components_forward_to_the_editable_control()
        {
            AssertTarget<Input<string>>("input");
            AssertTarget<TextArea>("textarea");
            AssertTarget<InputNumber<int>>("input");
            AssertTarget<AntDesign.Checkbox>("input");
            AssertTarget<Radio<string>>("input");
            AssertTarget<AntDesign.Switch>("button");
            AssertTarget<DownloadButton>("button");
            AssertTarget<DatePicker<DateTime>>("input");
            AssertTarget<Select<string, string>>("input");
        }

        [Fact]
        public void Ordinary_and_builder_components_forward_to_their_root()
        {
            AssertTarget<Row>("div");
            AssertTarget<Flex>("div");
            AssertTarget<AntDesign.Typography>("article");
            AssertTarget<AntDesign.Layout>("section");
            var icon = RenderComponent<AntDesign.Icon>(p => p.Add(x => x.Type, "save").AddUnmatched("data-testid", "icon"));
            Assert.Equal("icon", icon.Find("span").GetAttribute("data-testid"));
            AssertTarget<AntDesign.Mentions>("textarea");
        }

        [Fact]
        public void Wrapped_input_keeps_attributes_and_managed_change_handler_on_input()
        {
            var changed = false;
            var replaced = false;
            var cut = RenderComponent<Input<string>>(p => p
                .Add(x => x.AllowClear, true)
                .Add(x => x.OnChange, _ => changed = true)
                .Add(x => x.Disabled, true)
                .Add(x => x.AdditionalAttributes, new Dictionary<string, object>
                {
                    ["data-testid"] = "input", ["inputmode"] = "numeric",
                    ["disabled"] = false, ["class"] = "wrong",
                    ["onchange"] = EventCallback.Factory.Create<ChangeEventArgs>(this, () => replaced = true)
                }));
            var input = cut.Find("input");
            Assert.Equal("input", input.GetAttribute("data-testid"));
            Assert.Equal("numeric", input.GetAttribute("inputmode"));
            Assert.True(input.HasAttribute("disabled"));
            Assert.Contains("ant-input", input.ClassList);
            Assert.False(cut.Find("span.ant-input-affix-wrapper").HasAttribute("data-testid"));

            cut.SetParametersAndRender(p => p.Add(x => x.Disabled, false));
            cut.Find("input").Input("123");
            cut.Find("input").Change("123");
            Assert.True(changed);
            Assert.False(replaced);
        }

        [Fact]
        public void Composite_inputs_forward_in_range_and_multiple_modes()
        {
            var range = RenderComponent<RangePicker<DateTime[]>>(p => p.AddUnmatched("data-testid", "range"));
            Assert.Equal(2, range.FindAll("input[data-testid='range']").Count);

            var select = RenderComponent<Select<string, string>>(p => p
                .Add(x => x.Mode, SelectMode.Multiple)
                .AddUnmatched("data-testid", "select"));
            Assert.Single(select.FindAll("input[data-testid='select']"));
        }

        [Fact]
        public void Legacy_element_dictionary_remains_supported_alongside_shared_attributes()
        {
            var cut = RenderComponent<AntDesign.Internal.Element>(p => p
                .Add(x => x.HtmlTag, "section")
                .Add(x => x.Attributes, new Dictionary<string, object> { ["data-legacy"] = "old", ["title"] = "legacy" })
                .AddUnmatched("data-testid", "element")
                .AddUnmatched("title", "shared"));
            var element = cut.Find("section");
            Assert.Equal("element", element.GetAttribute("data-testid"));
            Assert.Equal("old", element.GetAttribute("data-legacy"));
            Assert.Equal("legacy", element.GetAttribute("title"));
        }

        [Fact]
        public void Pagination_preserves_legacy_attributes_and_accepts_arbitrary_attributes()
        {
            var cut = RenderComponent<Pagination>(p => p
                .Add(x => x.Total, 100)
                .Add(x => x.UnmatchedAttributes, new Dictionary<string, object> { ["data-legacy"] = "old" })
                .AddUnmatched("data-testid", "pager")
                .AddUnmatched("title", "Pages"));
            var root = cut.Find("ul.ant-pagination");
            Assert.Equal("pager", root.GetAttribute("data-testid"));
            Assert.Equal("old", root.GetAttribute("data-legacy"));
            Assert.Equal("Pages", root.GetAttribute("title"));
        }
    }
}
