// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using AntDesign.Theming;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Button
{
    public partial class ButtonTests : AntDesignTestBase
    {
        [Fact]
        public void Renders_an_empty_button()
        {
            var cut = Context.RenderComponent<AntDesign.Button>();
            cut.MarkupMatches(@"
                <button class=""ant-btn ant-btn-default ant-btn-color-default ant-btn-variant-outlined"" id:ignore type=""button""></button>
            ");
        }

        [Fact]
        public void Renders_a_button_with_contents()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.AddChildContent("Save")
            );

            cut.MarkupMatches(@"
                <button class=""ant-btn ant-btn-default ant-btn-color-default ant-btn-variant-outlined"" id:ignore type=""button""><span>Save</span></button>
            ");
        }
        [Fact]
        public void Renders_a_button_with_contents_with_arialabel()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
            {
                p.AddChildContent("Save");
                p.Add(x => x.AriaLabel, "Save");
            });

            cut.MarkupMatches(@"
                <button class=""ant-btn ant-btn-default ant-btn-color-default ant-btn-variant-outlined"" id:ignore type=""button"" aria-label=""Save""><span>Save</span></button>
            ");
        }

        [Fact]
        public void Renders_a_disabled_the_button()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.Add(x => x.Disabled, true)
            );

            cut.MarkupMatches(@"
                <button class=""ant-btn ant-btn-default ant-btn-color-default ant-btn-variant-outlined"" id:ignore type=""button"" disabled></button>
            ");
        }

        [Theory]
        [InlineData(ButtonType.Dashed)]
        [InlineData(ButtonType.Default)]
        [InlineData(ButtonType.Primary)]
        [InlineData(ButtonType.Link)]
        public void Renders_buttons_of_different_types(ButtonType type)
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.Add(x => x.Type, type)
            );

            var legacyType = type.ToString().ToLowerInvariant();
            var color = type switch
            {
                ButtonType.Primary => "primary",
                ButtonType.Link => "link",
                _ => "default",
            };
            var variant = type switch
            {
                ButtonType.Primary => "solid",
                ButtonType.Dashed => "dashed",
                ButtonType.Link => "link",
                _ => "outlined",
            };

            cut.MarkupMatches($@"
                <button class=""ant-btn ant-btn-{legacyType} ant-btn-color-{color} ant-btn-variant-{variant}"" id:ignore type=""button""></button>
            ");
        }

        [Fact]
        public void Should_fire_OnClick_when_clicked()
        {
            var clicked = false;

            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.Add(x => x.OnClick, args => clicked = true)
            );

            var buttonEl = cut.Find("button");
            buttonEl.Click();

            Assert.True(clicked);
        }

        [Fact]
        public void Renders_loading_icon()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.Add(x => x.Loading, true)
            );

            cut.MarkupMatches(@"
                <button class=""ant-btn ant-btn-default ant-btn-color-default ant-btn-variant-outlined ant-btn-loading"" id:ignore type=""button"">
                  <span diff:ignore></span>
                </button>
            ");
        }


        [Fact]
        public void Renders_when_type_is_changed()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
                p.Add(x => x.Type, ButtonType.Default)
            );

            cut.SetParametersAndRender(p =>
                p.Add(x => x.Type, ButtonType.Dashed)
            );

            cut.MarkupMatches($@"
                <button class=""ant-btn ant-btn-dashed ant-btn-color-default ant-btn-variant-dashed"" id:ignore type=""button""></button>
            ");
        }

        [Fact]
        public void Renders_end_icon_placement_class()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
            {
                p.Add(x => x.Icon, "search");
                p.Add(x => x.IconPlacement, ButtonIconPlacement.End);
                p.AddChildContent("Search");
            });

            Assert.Contains("ant-btn-icon-end", cut.Find("button").ClassList);
        }

        [Fact]
        public void Renders_v6_color_and_variant_classes()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
            {
                p.Add(x => x.ButtonColor, ButtonColor.Pink);
                p.Add(x => x.Variant, ButtonVariant.Filled);
            });

            var button = cut.Find("button");
            Assert.Contains("ant-btn-color-pink", button.ClassList);
            Assert.Contains("ant-btn-variant-filled", button.ClassList);
        }

        [Fact]
        public void Renders_semantic_classes_and_styles_for_button_slots()
        {
            var cut = Context.RenderComponent<AntDesign.Button>(p =>
            {
                p.Add(x => x.Icon, "search");
                p.AddChildContent("Search");
                p.Add(x => x.ClassNames, new Dictionary<string, string>
                {
                    ["root"] = "custom-root",
                    ["icon"] = "custom-icon",
                    ["content"] = "custom-content",
                });
                p.Add(x => x.Styles, new Dictionary<string, string>
                {
                    ["root"] = "border-radius: 6px;",
                    ["icon"] = "color: red;",
                    ["content"] = "letter-spacing: .02em;",
                });
            });

            Assert.Contains("custom-root", cut.Find("button").ClassList);
            Assert.Contains("custom-icon", cut.Find(".ant-btn-icon").ClassList);
            Assert.Contains("custom-content", cut.Find("button > span:not(.ant-btn-icon)").ClassList);
            Assert.Contains("border-radius: 6px", cut.Find("button").GetAttribute("style"));
            Assert.Contains("color: red", cut.Find(".ant-btn-icon").GetAttribute("style"));
            Assert.Contains("letter-spacing: .02em", cut.Find("button > span:not(.ant-btn-icon)").GetAttribute("style"));
        }

        [Fact]
        public void Applies_configured_component_tokens_to_button_element()
        {
            var theme = new ThemeConfig();
            theme.Components["Button"] = new Dictionary<string, string>
            {
                ["defaultBgDisabled"] = "rgba(0,0,0,0.1)",
                ["dashedBgDisabled"] = "rgba(0,0,0,0.4)",
            };

            var cut = Context.RenderComponent<AntDesign.ConfigProvider>(parameters =>
            {
                parameters.Add(x => x.ThemeConfig, theme);
                parameters.AddChildContent<AntDesign.Button>(button => button
                    .Add(x => x.Type, ButtonType.Default)
                    .Add(x => x.Disabled, true));
                parameters.AddChildContent<AntDesign.Button>(button => button
                    .Add(x => x.Type, ButtonType.Dashed)
                    .Add(x => x.Disabled, true));
            });

            var buttons = cut.FindAll("button");
            Assert.Equal(2, buttons.Count);
            Assert.Contains("--ant-button-default-bg-disabled:rgba(0,0,0,0.1)", buttons[0].GetAttribute("style"));
            Assert.Contains("--ant-button-dashed-bg-disabled:rgba(0,0,0,0.4)", buttons[0].GetAttribute("style"));
            Assert.Contains("--ant-button-default-bg-disabled:rgba(0,0,0,0.1)", buttons[1].GetAttribute("style"));
            Assert.Contains("--ant-button-dashed-bg-disabled:rgba(0,0,0,0.4)", buttons[1].GetAttribute("style"));
        }

        [Fact]
        public void Does_not_emit_unset_generated_tokens_that_override_default_button_padding()
        {
            var theme = new ThemeConfig();

            var cut = Context.RenderComponent<AntDesign.ConfigProvider>(parameters =>
            {
                parameters.Add(x => x.ThemeConfig, theme);
                parameters.AddChildContent<AntDesign.Button>();
            });

            var style = cut.Find("button").GetAttribute("style");
            Assert.DoesNotContain("System.", style);
            Assert.DoesNotContain("--ant-button-padding-inline:", style);
        }
    }
}
