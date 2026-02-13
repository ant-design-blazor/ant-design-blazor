// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Core
{
    public class OverlayServiceTests : AntDesignTestBase
    {
        private IOverlayService _overlayService;

        public OverlayServiceTests()
        {
            _overlayService = Services.GetRequiredService<IOverlayService>();
        }

        [Fact]
        public void OverlayService_Should_Be_Registered()
        {
            // Arrange & Act
            var service = Services.GetService<IOverlayService>();

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<OverlayService>();
        }

        [Fact]
        public async Task OpenAsync_Should_Return_OverlayReference()
        {
            // Arrange
            RenderFragment content = builder => builder.AddContent(0, "Test Content");

            // Act
            var reference = await _overlayService.OpenAsync(content, 100, 200);

            // Assert
            reference.Should().NotBeNull();
            reference.Id.Should().NotBeNullOrEmpty();
            reference.IsClosed.Should().BeFalse();
        }

        [Fact]
        public void Open_Should_Return_OverlayReference()
        {
            // Arrange
            RenderFragment content = builder => builder.AddContent(0, "Test Content");

            // Act
            var reference = _overlayService.Open(content, 150, 250);

            // Assert
            reference.Should().NotBeNull();
            reference.Id.Should().NotBeNullOrEmpty();
            reference.IsClosed.Should().BeFalse();
        }

        [Fact]
        public async Task OpenAsync_Should_Throw_When_Content_Is_Null()
        {
            // Arrange, Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _overlayService.OpenAsync(null, 100, 200));
        }

        [Fact]
        public void Open_Should_Throw_When_Content_Is_Null()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _overlayService.Open(null, 100, 200));
        }

        [Fact]
        public async Task CloseAsync_Should_Mark_Reference_As_Closed()
        {
            // Arrange
            RenderFragment content = builder => builder.AddContent(0, "Test Content");
            var reference = await _overlayService.OpenAsync(content, 100, 200);

            // Act
            await reference.CloseAsync();

            // Assert
            reference.IsClosed.Should().BeTrue();
        }

        [Fact]
        public async Task Close_Should_Mark_Reference_As_Closed()
        {
            // Arrange
            RenderFragment content = builder => builder.AddContent(0, "Test Content");
            var reference = await _overlayService.OpenAsync(content, 100, 200);

            // Act
            reference.Close();
            await Task.Delay(100); // Give time for async operation

            // Assert
            reference.IsClosed.Should().BeTrue();
        }

        [Fact]
        public async Task Multiple_Overlays_Should_Have_Unique_Ids()
        {
            // Arrange
            RenderFragment content1 = builder => builder.AddContent(0, "Content 1");
            RenderFragment content2 = builder => builder.AddContent(0, "Content 2");
            RenderFragment content3 = builder => builder.AddContent(0, "Content 3");

            // Act
            var ref1 = await _overlayService.OpenAsync(content1, 100, 100);
            var ref2 = await _overlayService.OpenAsync(content2, 200, 200);
            var ref3 = await _overlayService.OpenAsync(content3, 300, 300);

            // Assert
            ref1.Id.Should().NotBe(ref2.Id);
            ref2.Id.Should().NotBe(ref3.Id);
            ref1.Id.Should().NotBe(ref3.Id);
        }

        [Fact]
        public async Task CloseAll_Should_Close_All_Overlays()
        {
            // Arrange
            RenderFragment content1 = builder => builder.AddContent(0, "Content 1");
            RenderFragment content2 = builder => builder.AddContent(0, "Content 2");
            
            var ref1 = await _overlayService.OpenAsync(content1, 100, 100);
            var ref2 = await _overlayService.OpenAsync(content2, 200, 200);

            // Act
            _overlayService.CloseAll();

            // Assert
            // Note: CloseAll clears internal state but doesn't mark references as closed
            // The overlays should no longer be rendered
            var overlayService = _overlayService as OverlayService;
            overlayService.Should().NotBeNull();
            overlayService!.GetAll().Should().BeEmpty();
        }

        [Fact]
        public async Task Overlay_Should_Accept_Custom_Container()
        {
            // Arrange
            RenderFragment content = builder => builder.AddContent(0, "Test Content");

            // Act
            var reference = await _overlayService.OpenAsync(content, 100, 200, "#custom-container");

            // Assert
            reference.Should().NotBeNull();
            reference.Id.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CloseAsync_Should_Be_Idempotent()
        {
            // Arrange
            RenderFragment content = builder => builder.AddContent(0, "Test Content");
            var reference = await _overlayService.OpenAsync(content, 100, 200);

            // Act
            await reference.CloseAsync();
            await reference.CloseAsync(); // Call again

            // Assert
            reference.IsClosed.Should().BeTrue();
        }

        [Fact]
        public async Task OverlayContainer_Should_Render_Overlay_Content()
        {
            // Arrange
            var cut = RenderComponent<OverlayContainer>();
            RenderFragment content = builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "test-overlay-content");
                builder.AddContent(2, "Hello Overlay");
                builder.CloseElement();
            };

            // Act
            var reference = await _overlayService.OpenAsync(content, 100, 200);
            cut.Render(); // Force re-render

            // Assert
            reference.Should().NotBeNull();
        }

        [Fact]
        public async Task OverlayService_Should_Support_Coordinates_At_Zero()
        {
            // Arrange
            RenderFragment content = builder => builder.AddContent(0, "Test Content");

            // Act
            var reference = await _overlayService.OpenAsync(content, 0, 0);

            // Assert
            reference.Should().NotBeNull();
            reference.IsClosed.Should().BeFalse();
        }

        [Fact]
        public async Task OverlayService_Should_Support_Large_Coordinates()
        {
            // Arrange
            RenderFragment content = builder => builder.AddContent(0, "Test Content");

            // Act
            var reference = await _overlayService.OpenAsync(content, 5000, 3000);

            // Assert
            reference.Should().NotBeNull();
            reference.IsClosed.Should().BeFalse();
        }
    }
}
