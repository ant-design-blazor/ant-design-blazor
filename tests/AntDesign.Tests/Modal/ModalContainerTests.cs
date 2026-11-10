// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Modal
{
    public class ModalContainerTests : AntDesignTestBase
    {
        public ModalContainerTests()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
        }

        [Fact]
        public void ItShouldDestroyServiceModalOnRouteChangeByDefault()
        {
            var cut = RenderComponent<AntDesign.ModalContainer>();
            var modalService = Services.GetRequiredService<AntDesign.ModalService>();

            modalService.CreateModal(new AntDesign.ModalOptions
            {
                Content = builder => builder.AddContent(0, "modal content")
            });

            cut.WaitForAssertion(() => cut.FindAll(".ant-modal").Should().HaveCount(1));
            NavigationManager.NavigateTo("http://localhost/next");
            cut.WaitForAssertion(() => cut.FindAll(".ant-modal").Should().BeEmpty());
        }

        [Fact]
        public void ItShouldKeepServiceModalOnRouteChangeWhenKeepOnRouteChange()
        {
            var cut = RenderComponent<AntDesign.ModalContainer>();
            var modalService = Services.GetRequiredService<AntDesign.ModalService>();

            modalService.CreateModal(new AntDesign.ModalOptions
            {
                Content = builder => builder.AddContent(0, "modal content"),
                KeepOnRouteChange = true
            });

            cut.WaitForAssertion(() => cut.FindAll(".ant-modal").Should().HaveCount(1));
            NavigationManager.NavigateTo("http://localhost/next");
            cut.WaitForAssertion(() => cut.FindAll(".ant-modal").Should().HaveCount(1));
        }

        [Fact]
        public void ItShouldDestroyConfirmOnRouteChangeByDefault()
        {
            var cut = RenderComponent<AntDesign.ComfirmContainer>();
            var modalService = Services.GetRequiredService<AntDesign.ModalService>();

            modalService.Confirm(new AntDesign.ConfirmOptions
            {
                Content = "confirm content"
            });

            cut.WaitForAssertion(() => cut.FindAll(".ant-modal-confirm").Should().HaveCount(1));
            NavigationManager.NavigateTo("http://localhost/next");
            cut.WaitForAssertion(() => cut.FindAll(".ant-modal-confirm").Should().BeEmpty());
        }

        [Fact]
        public void ItShouldKeepConfirmOnRouteChangeWhenKeepOnRouteChange()
        {
            var cut = RenderComponent<AntDesign.ComfirmContainer>();
            var modalService = Services.GetRequiredService<AntDesign.ModalService>();

            modalService.Confirm(new AntDesign.ConfirmOptions
            {
                Content = "confirm content",
                KeepOnRouteChange = true
            });

            cut.WaitForAssertion(() => cut.FindAll(".ant-modal-confirm").Should().HaveCount(1));
            NavigationManager.NavigateTo("http://localhost/next");
            cut.WaitForAssertion(() => cut.FindAll(".ant-modal-confirm").Should().HaveCount(1));
        }
    }
}
