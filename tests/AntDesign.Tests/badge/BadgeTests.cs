// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Bunit;
using FluentAssertions;
using Xunit;

namespace AntDesign.Tests.Badge
{
    public class BadgeTests : AntDesignTestBase
    {
        [Fact]
        public void ItShouldRenderNumberProperly()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters.Add(x => x.Count, 25));

            systemUnderTest.MarkupMatches(@"<span class=""ant-badge ant-badge-not-a-wrapper"" id:ignore>
                <sup class=""ant-scroll-number ant-badge-count ant-badge-multiple-words ant-badge-zoom-enter ant-badge-zoom-enter-active""
                    style="""" title=""25"">
                    <span class=""ant-scroll-number-only"" style=""transform: translateY(-200%)"">
                        <p class=""ant-scroll-number-only-unit"">0</p>
                        <p class=""ant-scroll-number-only-unit"">1</p>
                        <p class=""ant-scroll-number-only-unit current"">2</p>
                        <p class=""ant-scroll-number-only-unit"">3</p>
                        <p class=""ant-scroll-number-only-unit"">4</p>
                        <p class=""ant-scroll-number-only-unit"">5</p>
                        <p class=""ant-scroll-number-only-unit"">6</p>
                        <p class=""ant-scroll-number-only-unit"">7</p>
                        <p class=""ant-scroll-number-only-unit"">8</p>
                        <p class=""ant-scroll-number-only-unit"">9</p>
                    </span>
                    <span class=""ant-scroll-number-only"" style=""transform: translateY(-500%)"">
                        <p class=""ant-scroll-number-only-unit"">0</p>
                        <p class=""ant-scroll-number-only-unit"">1</p>
                        <p class=""ant-scroll-number-only-unit"">2</p>
                        <p class=""ant-scroll-number-only-unit"">3</p>
                        <p class=""ant-scroll-number-only-unit"">4</p>
                        <p class=""ant-scroll-number-only-unit current"">5</p>
                        <p class=""ant-scroll-number-only-unit"">6</p>
                        <p class=""ant-scroll-number-only-unit"">7</p>
                        <p class=""ant-scroll-number-only-unit"">8</p>
                        <p class=""ant-scroll-number-only-unit"">9</p>
                    </span>
                </sup>
            </span>");
        }

        [Fact]
        public void ItShouldDisplayCustomTitleForCountBadge()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 25)
                .Add(x => x.Title, "Test Title"));

            systemUnderTest.Find("sup").Attributes.Single(x => x.Name == "title").Value.Should().BeEquivalentTo("Test Title");
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void ItShouldDisplayCountForTitleWhenInvalidParameterValuePassed(string value)
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 25)
                .Add(x => x.Title, value));

            systemUnderTest.Find("sup").Attributes.Single(x => x.Name == "title").Value.Should().BeEquivalentTo("25");
        }

        [Fact]
        public void ItShouldDisplayCustomTitleForDotBadge()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 25)
                .Add(x => x.Dot, true)
                .Add(x => x.Title, "Test Title"));

            systemUnderTest.Find("sup").Attributes.Single(x => x.Name == "title").Value.Should().BeEquivalentTo("Test Title");
        }

        [Fact]
        public void ItShouldNotRenderZeroByDefault()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters.Add(x => x.Count, 0));

            systemUnderTest.MarkupMatches(@"<span class=""ant-badge ant-badge-not-a-wrapper"" id:ignore></span>");
        }

        [Fact]
        public void ItShouldRenderZeroWhenToldTo()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 0)
                .Add(x => x.ShowZero, true)
            );

            systemUnderTest.MarkupMatches(@"<span class=""ant-badge ant-badge-not-a-wrapper"" id:ignore>
                <sup class=""ant-scroll-number ant-badge-count ant-badge-zoom-enter ant-badge-zoom-enter-active""
                    style="""" title=""0"">
                    0
                </sup>
            </span>");
        }

        [Fact]
        public void ItShouldRenderOverflowOver99ByDefault()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters.Add(x => x.Count, 100));

            systemUnderTest.MarkupMatches(@"<span class=""ant-badge ant-badge-not-a-wrapper"" id:ignore>
                <sup class=""ant-scroll-number ant-badge-count ant-badge-multiple-words ant-badge-zoom-enter ant-badge-zoom-enter-active ant-badge-count-overflow""
                    style="""" title=""100"">
                    99+
                </sup>
            </span>");
        }

        [Fact]
        public void ItShouldRenderOverflowOverGivenNumber()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 500)
                .Add(x => x.OverflowCount, 475));

            systemUnderTest.MarkupMatches(@"<span class=""ant-badge ant-badge-not-a-wrapper"" id:ignore>
                <sup class=""ant-scroll-number ant-badge-count ant-badge-multiple-words ant-badge-zoom-enter ant-badge-zoom-enter-active ant-badge-count-overflow""
                    style="""" title=""500"">
                    475+
                </sup>
            </span>");
        }

        [Fact]
        public void ItShouldRenderNumberBelowOverflowOverGivenNumber()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 474)
                .Add(x => x.OverflowCount, 475));

            systemUnderTest.MarkupMatches(@"<span class=""ant-badge ant-badge-not-a-wrapper"" id:ignore>
                <sup class=""ant-scroll-number ant-badge-count ant-badge-multiple-words ant-badge-zoom-enter ant-badge-zoom-enter-active""
                    style="""" title=""474"">
                    <span class=""ant-scroll-number-only"" style=""transform: translateY(-400%)"">
                        <p class=""ant-scroll-number-only-unit"">0</p>
                        <p class=""ant-scroll-number-only-unit"">1</p>
                        <p class=""ant-scroll-number-only-unit"">2</p>
                        <p class=""ant-scroll-number-only-unit"">3</p>
                        <p class=""ant-scroll-number-only-unit current"">4</p>
                        <p class=""ant-scroll-number-only-unit"">5</p>
                        <p class=""ant-scroll-number-only-unit"">6</p>
                        <p class=""ant-scroll-number-only-unit"">7</p>
                        <p class=""ant-scroll-number-only-unit"">8</p>
                        <p class=""ant-scroll-number-only-unit"">9</p>
                    </span>
                    <span class=""ant-scroll-number-only"" style=""transform: translateY(-700%)"">
                        <p class=""ant-scroll-number-only-unit"">0</p>
                        <p class=""ant-scroll-number-only-unit"">1</p>
                        <p class=""ant-scroll-number-only-unit"">2</p>
                        <p class=""ant-scroll-number-only-unit"">3</p>
                        <p class=""ant-scroll-number-only-unit"">4</p>
                        <p class=""ant-scroll-number-only-unit"">5</p>
                        <p class=""ant-scroll-number-only-unit"">6</p>
                        <p class=""ant-scroll-number-only-unit current"">7</p>
                        <p class=""ant-scroll-number-only-unit"">8</p>
                        <p class=""ant-scroll-number-only-unit"">9</p>
                    </span>
                    <span class=""ant-scroll-number-only"" style=""transform: translateY(-400%)"">
                        <p class=""ant-scroll-number-only-unit"">0</p>
                        <p class=""ant-scroll-number-only-unit"">1</p>
                        <p class=""ant-scroll-number-only-unit"">2</p>
                        <p class=""ant-scroll-number-only-unit"">3</p>
                        <p class=""ant-scroll-number-only-unit current"">4</p>
                        <p class=""ant-scroll-number-only-unit"">5</p>
                        <p class=""ant-scroll-number-only-unit"">6</p>
                        <p class=""ant-scroll-number-only-unit"">7</p>
                        <p class=""ant-scroll-number-only-unit"">8</p>
                        <p class=""ant-scroll-number-only-unit"">9</p>
                    </span>
                </sup>
            </span>");
        }

        [Fact]
        public void ItShouldRenderJustDotWhenToldTo()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 25)
                .Add(x => x.Dot, true));

            systemUnderTest.MarkupMatches(@"<span class=""ant-badge ant-badge-not-a-wrapper"" id:ignore>
                <sup class=""ant-scroll-number ant-badge-dot ant-badge-multiple-words ant-badge-zoom-enter ant-badge-zoom-enter-active""
                    style="""" title=""25"">
                </sup>
            </span>");
        }

        [Fact]
        public void ItShouldNotRenderDotAtZero()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 0)
                .Add(x => x.Dot, true));

            systemUnderTest.MarkupMatches(@"<span class=""ant-badge ant-badge-not-a-wrapper"" id:ignore></span>");
        }

        [Fact]
        public void ItShouldRenderDotAtZeroWhenToldTo()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 0)
                .Add(x => x.Dot, true)
                .Add(x => x.ShowZero, true));

            systemUnderTest.MarkupMatches(@"<span class=""ant-badge ant-badge-not-a-wrapper"" id:ignore>
                <sup class=""ant-scroll-number ant-badge-dot ant-badge-zoom-enter ant-badge-zoom-enter-active""
                    style="""" title=""0"">
                </sup></span>");
        }

        [Fact]
        public void ItShouldRenderStatusWithText()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Status, BadgeStatus.Default)
                .Add(x => x.Text, "Llama"));

            systemUnderTest.MarkupMatches($@"<span class=""ant-badge ant-badge-status ant-badge-not-a-wrapper"" id:ignore>
                <span class=""ant-badge-status-dot ant-badge-status-default"" style=""""></span>
                <span class=""ant-badge-status-text"">Llama</span>
            </span>");
        }

        [Theory]
        [InlineData(PresetColor.Red, "ant-badge-status-red")]
        [InlineData(PresetColor.Pink, "ant-badge-status-pink")]
        public void ItShouldRenderPresetColor(PresetColor color, string expectedClass)
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 10)
                .Add(x => x.PresetColor, color)
                .Add(x => x.Dot, true));

            systemUnderTest.MarkupMatches($@"<span class=""ant-badge ant-badge-status ant-badge-not-a-wrapper"" id:ignore>
                <span class=""ant-badge-status-dot {expectedClass}"" style=""""></span>
            </span>");
        }

        [Theory]
        [InlineData(BadgeStatus.Success, "ant-badge-status-success")]
        [InlineData(BadgeStatus.Processing, "ant-badge-status-processing")]
        public void ItShouldRenderStatusColor(string status, string expectedClass)
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 10)
                .Add(x => x.Status, status)
                .Add(x => x.Dot, true));

            systemUnderTest.MarkupMatches($@"<span class=""ant-badge ant-badge-status ant-badge-not-a-wrapper"" id:ignore>
                <span class=""ant-badge-status-dot {expectedClass}"" style=""""></span>
            </span>");
        }

        [Theory]
        [InlineData("red", "background:red;")]
        [InlineData("pink", "background:pink;")]
        [InlineData("#2db7f5", "background:#2db7f5;")]
        public void ItShouldRenderCustomColor(string color, string expectedStyle)
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 10)
                .Add(x => x.Color, color)
                .Add(x => x.Dot, true));

            systemUnderTest.MarkupMatches($@"<span class=""ant-badge ant-badge-status ant-badge-not-a-wrapper"" id:ignore>
                <span class=""ant-badge-status-dot"" style=""{expectedStyle}""></span>
            </span>");
        }

        [Fact]
        public void ItShouldProperlyWrapChildContent()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters
                .Add(x => x.Count, 25)
                .AddChildContent("<button>Test Button</button>"));

            systemUnderTest.MarkupMatches(@"<span class=""ant-badge"" id:ignore>
                <button>Test Button</button>
                <sup class=""ant-scroll-number ant-badge-count ant-badge-multiple-words ant-badge-zoom-enter ant-badge-zoom-enter-active""
                    style="""" title=""25"">
                    <span class=""ant-scroll-number-only"" style=""transform: translateY(-200%)"">
                        <p class=""ant-scroll-number-only-unit"">0</p>
                        <p class=""ant-scroll-number-only-unit"">1</p>
                        <p class=""ant-scroll-number-only-unit current"">2</p>
                        <p class=""ant-scroll-number-only-unit"">3</p>
                        <p class=""ant-scroll-number-only-unit"">4</p>
                        <p class=""ant-scroll-number-only-unit"">5</p>
                        <p class=""ant-scroll-number-only-unit"">6</p>
                        <p class=""ant-scroll-number-only-unit"">7</p>
                        <p class=""ant-scroll-number-only-unit"">8</p>
                        <p class=""ant-scroll-number-only-unit"">9</p>
                    </span>
                    <span class=""ant-scroll-number-only"" style=""transform: translateY(-500%)"">
                        <p class=""ant-scroll-number-only-unit"">0</p>
                        <p class=""ant-scroll-number-only-unit"">1</p>
                        <p class=""ant-scroll-number-only-unit"">2</p>
                        <p class=""ant-scroll-number-only-unit"">3</p>
                        <p class=""ant-scroll-number-only-unit"">4</p>
                        <p class=""ant-scroll-number-only-unit current"">5</p>
                        <p class=""ant-scroll-number-only-unit"">6</p>
                        <p class=""ant-scroll-number-only-unit"">7</p>
                        <p class=""ant-scroll-number-only-unit"">8</p>
                        <p class=""ant-scroll-number-only-unit"">9</p>
                    </span>
                </sup>
            </span>");
        }

        [Fact]
        public void ItShouldRenderCustomCountTemplate()
        {
            var systemUnderTest = RenderComponent<AntDesign.Badge>(parameters => parameters.Add(x => x.CountTemplate, "<span>Test</span>"));

            systemUnderTest.MarkupMatches(@"<span class=""ant-badge ant-badge-not-a-wrapper"" id:ignore>
                <span role=""img"" class=""ant-scroll-number-custom-component"">
                    <span>Test</span>
                </span>
            </span>");
        }
    }
}
