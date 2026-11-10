// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using Xunit;

namespace AntDesign.Tests.Slider
{
    public class SliderTests : AntDesignTestBase
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public Task ItShouldRenderAriaPropertiesWithCorrectValues(bool hasTooltip, bool disabled)
        {
            var systemUnderTest = RenderComponent<Slider<double>>(parameters => parameters
                .Add(x => x.HasTooltip, hasTooltip)
                .Add(x => x.DefaultValue, 15.2)
                .Add(x => x.Min, 12.5)
                .Add(x => x.Max, 50.2)
                .Add(x => x.Step, 0.1)
                .Add(x => x.Disabled, disabled)
            );

            var handle = systemUnderTest.Find(".ant-slider-handle");

            var attributeValues = handle.Attributes
                .Where(x => x.Name.StartsWith("aria-"))
                .Select(x => (x.Name, x.Value));

            attributeValues.Should().BeEquivalentTo(new[]
            {
                ("aria-disabled", disabled.ToString()),
                ("aria-valuemax", "50.2"),
                ("aria-valuemin", "12.5"),
                ("aria-valuenow", "15.2"),
            }, options => options.ExcludingMissingMembers());

            return Task.CompletedTask;
        }

        [Fact]
        public Task ItShouldThrowWhenNeitherStepOrMarksAreGiven()
        {
            Action systemUnderTest = () =>
            {
                RenderComponent<Slider<double>>(parameters => parameters
                    .Add(x => x.Step, null)
                    .Add(x => x.Marks, null)
                );
            };

            systemUnderTest.Should().Throw<ArgumentException>().WithMessage($"Step can only be null when Marks is not null. (Parameter 'Step')");

            return Task.CompletedTask;
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-0.000000000000000000001)]
        public Task ItShouldThrowWhenStepInvalid(double step)
        {
            Action systemUnderTest = () =>
            {
                RenderComponent<Slider<double>>(parameters => parameters.Add(x => x.Step, step));
            };

            systemUnderTest.Should().Throw<ArgumentException>().WithMessage("Must greater than 0. (Parameter 'Step')");

            return Task.CompletedTask;
        }

        [Theory]
        [InlineData(0, 100, 300)]
        [InlineData(12.5, 50.2, 10)]
        public Task ItShouldThrowWhenInvalidStepMinMaxCombination(double min, double max, double step)
        {
            Action systemUnderTest = () =>
            {
                RenderComponent<Slider<double>>(parameters => parameters
                    .Add(x => x.Step, step)
                    .Add(x => x.Min, min)
                    .Add(x => x.Max, max)
                );
            };

            systemUnderTest.Should().Throw<ArgumentException>().WithMessage($"Must be divided by ({max} - {min}). (Parameter 'Step')");

            return Task.CompletedTask;
        }

        [Theory]
        [InlineData(0, 100, 1)]
        [InlineData(0, 100, 10)]
        [InlineData(12.5, 50.2, 0.1)]
        public Task ItShouldNotThrowWhenValidStep(double min, double max, double step)
        {
            Action systemUnderTest = () =>
            {
                RenderComponent<Slider<double>>(parameters => parameters
                    .Add(x => x.Step, step)
                    .Add(x => x.Min, min)
                    .Add(x => x.Max, max)
                );
            };

            systemUnderTest.Should().NotThrow<ArgumentException>();

            return Task.CompletedTask;
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public Task ItShouldRenderAriaPropertiesWithCorrectValues_ForRange(bool hasTooltip, bool disabled)
        {
            var systemUnderTest = RenderComponent<Slider<(double, double)>>(parameters => parameters
                .Add(x => x.HasTooltip, hasTooltip)
                .Add(x => x.DefaultValue, (15.5, 25.1))
                .Add(x => x.Min, 12.5)
                .Add(x => x.Max, 50.2)
                .Add(x => x.Step, 0.1)
                .Add(x => x.Disabled, disabled)
            );

            var leftHandle = systemUnderTest.Find(".ant-slider-handle-1");
            var rightHandle = systemUnderTest.Find(".ant-slider-handle-2");

            var attributes = new
            {
                LeftAttributes = leftHandle.Attributes
                    .Where(x => x.Name.StartsWith("aria-"))
                    .Select(x => (x.Name, x.Value)),
                RightAttributes = rightHandle.Attributes
                    .Where(x => x.Name.StartsWith("aria-"))
                    .Select(x => (x.Name, x.Value))
            };

            attributes.Should().BeEquivalentTo(new
            {
                LeftAttributes = new[]
                {
                    ("aria-disabled", disabled.ToString()),
                    ("aria-valuemax", "50.2"),
                    ("aria-valuemin", "12.5"),
                    ("aria-valuenow", "15.5"),
                },
                RightAttributes = new[]
                {
                    ("aria-disabled", disabled.ToString()),
                    ("aria-valuemax", "50.2"),
                    ("aria-valuemin", "12.5"),
                    ("aria-valuenow", "25.1"),
                }
            });

            return Task.CompletedTask;
        }

        [Fact]
        public Task ItShouldRenderProperMarkup()
        {
            var systemUnderTest = RenderComponent<Slider<double>>(parameters => parameters
                .Add(x => x.DefaultValue, 30)
            );

            systemUnderTest.MarkupMatches(@"<div class=""ant-slider ant-slider-horizontal"" style=""user-select:none;"" id:ignore>
                <div class=""ant-slider-rail""></div>
                <div class=""ant-slider-track"" style=""left: 0%; width: 30%; right: auto;""></div>
                <div class=""ant-slider-step""></div>
                <div class=""ant-slider-handle"" tabindex=""0"" role=""slider"" aria-valuemin=""0"" aria-valuemax=""100"" aria-valuenow=""30"" aria-disabled=""False"" style=""left: 30%; right: auto; transform: translateX(-50%);""></div>
                <div class=""ant-slider-mark""></div>
            </div>");

            return Task.CompletedTask;
        }

        [Fact]
        public Task ItShouldShowTooltipWhenRightHandleClicked()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;

            var systemUnderTest = RenderComponent<Slider<double>>(parameters => parameters
                .Add(x => x.DefaultValue, 30)
                .Add(x => x.HasTooltip, true)
            );

            systemUnderTest.Find(".ant-slider-handle").PointerDown(CreateTouchPointerEvent(1, 30));

            systemUnderTest.WaitForAssertion(() =>
            {
                systemUnderTest.FindComponent<Tooltip>().Instance.Visible.Should().BeTrue();
            });

            return Task.CompletedTask;
        }

        [Fact]
        public Task ItShouldShowTooltipWhenRangeLeftHandleClicked()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;

            var systemUnderTest = RenderComponent<Slider<(double, double)>>(parameters => parameters
                .Add(x => x.DefaultValue, (10, 30))
                .Add(x => x.HasTooltip, true)
            );

            systemUnderTest.Find(".ant-slider-handle-1").PointerDown(CreateTouchPointerEvent(1, 10));

            systemUnderTest.WaitForAssertion(() =>
            {
                var tooltips = systemUnderTest.FindComponents<Tooltip>();
                tooltips.First().Instance.Visible.Should().BeTrue();
            });

            return Task.CompletedTask;
        }

        [Fact]
        public Task ItShouldShowTooltipWhenRangeRightHandleClicked()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;

            var systemUnderTest = RenderComponent<Slider<(double, double)>>(parameters => parameters
                .Add(x => x.DefaultValue, (10, 30))
                .Add(x => x.HasTooltip, true)
            );

            systemUnderTest.Find(".ant-slider-handle-2").PointerDown(CreateTouchPointerEvent(1, 30));

            systemUnderTest.WaitForAssertion(() =>
            {
                var tooltips = systemUnderTest.FindComponents<Tooltip>();
                tooltips.Last().Instance.Visible.Should().BeTrue();
            });

            return Task.CompletedTask;
        }

        [Fact]
        public Task ItShouldRenderProperMarkup_ForMarkers()
        {
            var systemUnderTest = RenderComponent<Slider<double>>(parameters => parameters
                .Add(x => x.DefaultValue, 30)
                .Add(x => x.Marks, new[]
                {
                    new SliderMark(0, "0 C"),
                    new SliderMark(26, "26 C"),
                    new SliderMark(37, "37 C"),
                    new SliderMark(100, (builder) =>
                    {
                        builder.OpenElement(0, "strong");
                        builder.AddContent(1, "100 C");
                        builder.CloseElement();
                    }, "color: red")
                })
            );

            systemUnderTest.Find(".ant-slider-mark").MarkupMatches(@"<div class=""ant-slider-mark"">
                <span class:ignore style=""left: 0%; transform: translateX(-50%);"">0 C</span>
                <span class:ignore style=""left: 26%; transform: translateX(-50%);"">26 C</span>
                <span class:ignore style=""left: 37%; transform: translateX(-50%);"">37 C</span>
                <span class:ignore style=""left: 100%; transform: translateX(-50%); color: red;""><strong>100 C</strong></span></div>
            </div>");

            return Task.CompletedTask;
        }

        public static IEnumerable<object[]> MarkersActiveTestData()
        {
            yield return new object[]
            {
                0,
                new[]
                {
                    "ant-slider-mark-text ant-slider-mark-text-active",
                    "ant-slider-mark-text",
                    "ant-slider-mark-text",
                    "ant-slider-mark-text",
                }
            };

            yield return new object[]
            {
                27,
                new[]
                {
                    "ant-slider-mark-text ant-slider-mark-text-active",
                    "ant-slider-mark-text ant-slider-mark-text-active",
                    "ant-slider-mark-text",
                    "ant-slider-mark-text",
                }
            };

            yield return new object[]
            {
                40,
                new[]
                {
                    "ant-slider-mark-text ant-slider-mark-text-active",
                    "ant-slider-mark-text ant-slider-mark-text-active",
                    "ant-slider-mark-text ant-slider-mark-text-active",
                    "ant-slider-mark-text",
                }
            };

            yield return new object[]
            {
                100,
                new[]
                {
                    "ant-slider-mark-text ant-slider-mark-text-active",
                    "ant-slider-mark-text ant-slider-mark-text-active",
                    "ant-slider-mark-text ant-slider-mark-text-active",
                    "ant-slider-mark-text ant-slider-mark-text-active",
                }
            };
        }

        [Theory]
        [MemberData(nameof(MarkersActiveTestData))]
        public Task ItShouldRenderProperClassForMarkersActive(int value, string[] expectedClassesInOrder)
        {
            var systemUnderTest = RenderComponent<Slider<double>>(parameters => parameters
                .Add(x => x.DefaultValue, value)
                .Add(x => x.Marks, new[]
                {
                    new SliderMark(0, "0 C"),
                    new SliderMark(26, "26 C"),
                    new SliderMark(37, "37 C"),
                    new SliderMark(100, (builder) =>
                    {
                        builder.OpenElement(0, "strong");
                        builder.AddContent(1, "100 C");
                        builder.CloseElement();
                    }, "color: red")
                })
            );

            systemUnderTest.FindAll(".ant-slider-mark-text").Select(x => x.ClassName.Trim()).Should().BeEquivalentTo(expectedClassesInOrder, options => options.WithStrictOrdering());

            return Task.CompletedTask;
        }

        [Fact]
        public async Task ItShouldDragWithTouchPointer()
        {
            Func<JsonElement, Task> pointerMove = null!;
            Func<JsonElement, Task> pointerUp = null!;
            Func<JsonElement, Task> pointerCancel = null!;

            MockedDomEventListener
                .Setup(x => x.AddShared<JsonElement>("window", "pointermove", It.IsAny<Func<JsonElement, Task>>(), false))
                .Callback<object, string, Func<JsonElement, Task>, bool>((_, _, callback, _) => pointerMove = callback);
            MockedDomEventListener
                .Setup(x => x.AddShared<JsonElement>("window", "pointerup", It.IsAny<Func<JsonElement, Task>>(), false))
                .Callback<object, string, Func<JsonElement, Task>, bool>((_, _, callback, _) => pointerUp = callback);
            MockedDomEventListener
                .Setup(x => x.AddShared<JsonElement>("window", "pointercancel", It.IsAny<Func<JsonElement, Task>>(), false))
                .Callback<object, string, Func<JsonElement, Task>, bool>((_, _, callback, _) => pointerCancel = callback);

            JSInterop.Setup<HtmlElement>(JSInteropConstants.GetDomInfo, _ => true)
                .SetResult(new HtmlElement { AbsoluteLeft = 0, ClientWidth = 100 });

            double changedValue = -1;
            double committedValue = -1;
            int commitCount = 0;
            var systemUnderTest = RenderComponent<Slider<double>>(parameters => parameters
                .Add(x => x.DefaultValue, 30)
                .Add(x => x.HasTooltip, false)
                .Add(x => x.OnChange, value => changedValue = value)
                .Add(x => x.OnAfterChange, value =>
                {
                    committedValue = value;
                    commitCount++;
                })
            );

            pointerMove.Should().NotBeNull();
            pointerUp.Should().NotBeNull();
            pointerCancel.Should().NotBeNull();

            systemUnderTest.Find(".ant-slider-handle").PointerDown(CreateTouchPointerEvent(1, 30));

            await systemUnderTest.InvokeAsync(() => pointerMove(CreateTouchPointerJson(2, 80)));
            changedValue.Should().Be(-1);
            await systemUnderTest.InvokeAsync(() => pointerUp(CreateTouchPointerJson(2, 80)));
            committedValue.Should().Be(-1);

            await systemUnderTest.InvokeAsync(() => pointerMove(CreateTouchPointerJson(1, 60)));
            changedValue.Should().Be(60);
            systemUnderTest.Find(".ant-slider-handle").GetAttribute("aria-valuenow").Should().Be("60");

            await systemUnderTest.InvokeAsync(() => pointerUp(CreateTouchPointerJson(1, 60)));
            committedValue.Should().Be(60);
            commitCount.Should().Be(1);

            systemUnderTest.Find(".ant-slider-handle").PointerDown(CreateTouchPointerEvent(3, 60));
            await systemUnderTest.InvokeAsync(() => pointerCancel(CreateTouchPointerJson(3, 60)));
            await systemUnderTest.InvokeAsync(() => pointerMove(CreateTouchPointerJson(3, 80)));

            changedValue.Should().Be(60);
            committedValue.Should().Be(60);
            commitCount.Should().Be(1);
        }

        private static PointerEventArgs CreateTouchPointerEvent(long pointerId, double position)
        {
            return new PointerEventArgs
            {
                PointerId = pointerId,
                PointerType = "touch",
                IsPrimary = true,
                Button = 0,
                Buttons = 1,
                ClientX = position,
            };
        }

        private static JsonElement CreateTouchPointerJson(long pointerId, double position)
        {
            return JsonSerializer.SerializeToElement(new
            {
                pointerId,
                pointerType = "touch",
                isPrimary = true,
                button = 0,
                buttons = 1,
                clientX = position,
                clientY = 0,
                pageX = position,
                pageY = 0,
            });
        }
    }
}
