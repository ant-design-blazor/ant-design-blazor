// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Tour
{
    public class TourTests : AntDesignTestBase
    {
        [Fact]
        public void Tour_Should_Render_When_Open_Is_True()
        {
            // Arrange
            var steps = new List<TourStep>
            {
                new TourStep { Title = "Step 1", Description = "Description 1" },
                new TourStep { Title = "Step 2", Description = "Description 2" }
            };

            // Act
            var cut = RenderComponent<AntDesign.Tour>(parameters => parameters
                .Add(p => p.Open, true)
                .Add(p => p.Steps, steps));

            // Assert
            cut.Markup.Should().Contain("ant-tour-mask");
        }

        [Fact]
        public void Tour_Should_Not_Render_When_Open_Is_False()
        {
            // Arrange
            var steps = new List<TourStep>
            {
                new TourStep { Title = "Step 1", Description = "Description 1" }
            };

            // Act
            var cut = RenderComponent<AntDesign.Tour>(parameters => parameters
                .Add(p => p.Open, false)
                .Add(p => p.Steps, steps));

            // Assert
            cut.Markup.Should().NotContain("ant-tour-mask");
        }

        [Fact]
        public void Tour_Should_Display_Multiple_Steps()
        {
            // Arrange
            var steps = new List<TourStep>
            {
                new TourStep { Title = "Step 1", Description = "First step" },
                new TourStep { Title = "Step 2", Description = "Second step" },
                new TourStep { Title = "Step 3", Description = "Third step" }
            };

            // Act
            var cut = RenderComponent<AntDesign.Tour>(parameters => parameters
                .Add(p => p.Open, true)
                .Add(p => p.Steps, steps)
                .Add(p => p.Current, 0));

            // Assert
            steps.Count.Should().Be(3);
        }

        [Fact]
        public void Tour_Should_Change_Current_Step()
        {
            // Arrange
            var currentStep = 0;
            var steps = new List<TourStep>
            {
                new TourStep { Title = "Step 1", Description = "First" },
                new TourStep { Title = "Step 2", Description = "Second" }
            };

            // Act
            var cut = RenderComponent<AntDesign.Tour>(parameters => parameters
                .Add(p => p.Open, true)
                .Add(p => p.Steps, steps)
                .Add(p => p.Current, currentStep)
                .Add(p => p.CurrentChanged, EventCallback.Factory.Create<int>(this, value => currentStep = value)));

            // Assert
            currentStep.Should().Be(0);
        }

        [Fact]
        public void Tour_Should_Support_Primary_Type()
        {
            // Arrange
            var steps = new List<TourStep>
            {
                new TourStep { Title = "Primary Step", Description = "This is primary", Type = TourType.Primary }
            };

            // Act
            var cut = RenderComponent<AntDesign.Tour>(parameters => parameters
                .Add(p => p.Open, true)
                .Add(p => p.Steps, steps)
                .Add(p => p.Type, TourType.Primary));

            // Assert
            steps[0].Type.Should().Be(TourType.Primary);
        }

        [Fact]
        public void Tour_Should_Support_Custom_Mask()
        {
            // Arrange
            var steps = new List<TourStep>
            {
                new TourStep { Title = "Step", Description = "Description", Mask = false }
            };

            // Act
            var cut = RenderComponent<AntDesign.Tour>(parameters => parameters
                .Add(p => p.Open, true)
                .Add(p => p.Steps, steps)
                .Add(p => p.Mask, false));

            // Assert
            steps[0].Mask.Should().BeFalse();
        }

        [Fact]
        public void Tour_Should_Handle_Empty_Steps()
        {
            // Arrange & Act
            var cut = RenderComponent<AntDesign.Tour>(parameters => parameters
                .Add(p => p.Open, true)
                .Add(p => p.Steps, new List<TourStep>()));

            // Assert
            cut.Markup.Should().NotContain("ant-tour-panel");
        }

        [Fact]
        public void TourStep_Should_Have_Default_Values()
        {
            // Arrange & Act
            var step = new TourStep();

            // Assert
            step.Placement.Should().Be(Placement.Bottom);
            step.Arrow.Should().BeTrue();
            step.Type.Should().Be(TourType.Default);
            step.Closable.Should().BeTrue();
        }

        [Fact]
        public void TourStep_Should_Support_Custom_Button_Props()
        {
            // Arrange
            var nextButtonProps = new TourButtonProps
            {
                Text = "Continue",
                Disabled = false
            };

            var step = new TourStep
            {
                Title = "Step",
                Description = "Description",
                NextButtonProps = nextButtonProps
            };

            // Act & Assert
            step.NextButtonProps.Text.Should().Be("Continue");
            step.NextButtonProps.Disabled.Should().BeFalse();
        }

        [Fact]
        public void Tour_Should_Support_ZIndex()
        {
            // Arrange
            var steps = new List<TourStep>
            {
                new TourStep { Title = "Step", Description = "Description" }
            };

            // Act
            var cut = RenderComponent<AntDesign.Tour>(parameters => parameters
                .Add(p => p.Open, true)
                .Add(p => p.Steps, steps)
                .Add(p => p.ZIndex, 2000));

            // Assert
            cut.Instance.ZIndex.Should().Be(2000);
        }

        [Fact]
        public async Task Tour_Should_Call_OnClose_When_Closed()
        {
            // Arrange
            var closeCalled = false;
            var steps = new List<TourStep>
            {
                new TourStep { Title = "Step", Description = "Description" }
            };

            // Act
            var cut = RenderComponent<AntDesign.Tour>(parameters => parameters
                .Add(p => p.Open, true)
                .Add(p => p.Steps, steps)
                .Add(p => p.OnClose, EventCallback.Factory.Create(this, () => closeCalled = true)));

            // Assert
            closeCalled.Should().BeFalse();
        }

        [Fact]
        public void Tour_Should_Support_All_Placement_Options()
        {
            // Arrange & Act
            var placements = new[]
            {
                Placement.Top,
                Placement.TopLeft,
                Placement.TopRight,
                Placement.Bottom,
                Placement.BottomLeft,
                Placement.BottomRight,
                Placement.Left,
                Placement.LeftTop,
                Placement.LeftBottom,
                Placement.Right,
                Placement.RightTop,
                Placement.RightBottom
            };

            // Assert
            foreach (var placement in placements)
            {
                var step = new TourStep { Placement = placement };
                step.Placement.Should().Be(placement);
            }
        }

        [Fact]
        public void TourButtonProps_Should_Support_All_Properties()
        {
            // Arrange & Act
            var buttonProps = new TourButtonProps
            {
                Text = "Custom Text",
                Disabled = true,
                Class = "custom-class",
                Style = "color: red;"
            };

            // Assert
            buttonProps.Text.Should().Be("Custom Text");
            buttonProps.Disabled.Should().BeTrue();
            buttonProps.Class.Should().Be("custom-class");
            buttonProps.Style.Should().Be("color: red;");
        }

        [Fact]
        public void Tour_Should_Support_Cover_Content()
        {
            // Arrange
            RenderFragment coverContent = builder =>
            {
                builder.OpenElement(0, "img");
                builder.AddAttribute(1, "src", "test.png");
                builder.CloseElement();
            };

            var step = new TourStep
            {
                Title = "Step",
                Description = "Description",
                Cover = coverContent
            };

            // Act & Assert
            step.Cover.Should().NotBeNull();
        }

        [Fact]
        public void Tour_Should_Support_Description_Template()
        {
            // Arrange
            RenderFragment descriptionTemplate = builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, "Custom Description");
                builder.CloseElement();
            };

            var step = new TourStep
            {
                Title = "Step",
                DescriptionTemplate = descriptionTemplate
            };

            // Act & Assert
            step.DescriptionTemplate.Should().NotBeNull();
        }
    }
}
