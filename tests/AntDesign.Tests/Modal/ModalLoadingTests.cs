// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Modal
{
    public class ModalLoadingTests : AntDesignTestBase
    {
        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public async Task ItShouldShowLoadingOnEveryConfirmationAfterCancelingClose(bool useSetConfirmLoading, bool okOnlyFooter)
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            var cut = RenderComponent<ModalContainer>();
            var service = Services.GetRequiredService<ModalService>();
            var options = new ModalOptions();
            if (okOnlyFooter)
            {
                options.Footer = ModalFooter.DefaultOkFooter;
            }

            var modalRef = await cut.InvokeAsync(() => service.CreateModal<LoadingFeedback, string>(options, ""));
            var feedback = cut.FindComponent<LoadingFeedback>().Instance;
            feedback.UseSetConfirmLoading = useSetConfirmLoading;

            for (var attempt = 0; attempt < 3; attempt++)
            {
                var confirmation = cut.Find(".ant-modal-footer .ant-btn-primary").ClickAsync(new MouseEventArgs());
                try
                {
                    cut.WaitForAssertion(() => cut.FindAll(".ant-modal-footer .ant-btn-loading").Should()
                        .HaveCount(1, "confirmation attempt {0} should show loading", attempt + 1));
                }
                finally
                {
                    await cut.InvokeAsync(feedback.CompleteConfirmation);
                    await confirmation;
                }

                cut.WaitForAssertion(() => cut.FindAll(".ant-modal-footer .ant-btn-loading").Should().BeEmpty());
                modalRef.Config.Visible.Should().BeTrue();
                modalRef.Config.ConfirmLoading.Should().BeFalse();
            }

            feedback.CancelClose = false;
            var finalConfirmation = cut.Find(".ant-modal-footer .ant-btn-primary").ClickAsync(new MouseEventArgs());
            await cut.InvokeAsync(feedback.CompleteConfirmation);
            await finalConfirmation;
            modalRef.Config.Visible.Should().BeFalse();
        }

        public class LoadingFeedback : FeedbackComponent<string>
        {
            private TaskCompletionSource<bool> _confirmation = new TaskCompletionSource<bool>();

            public bool UseSetConfirmLoading { get; set; }

            public bool CancelClose { get; set; } = true;

            public override async Task OnFeedbackOkAsync(ModalClosingEventArgs args)
            {
                var modalRef = (ModalRef)FeedbackRef;
                if (UseSetConfirmLoading)
                {
                    modalRef.SetConfirmLoading(true);
                }
                else
                {
                    modalRef.Config.ConfirmLoading = true;
                    await modalRef.UpdateConfigAsync();
                }

                await _confirmation.Task;
                _confirmation = new TaskCompletionSource<bool>();
                args.Cancel = CancelClose;
            }

            public void CompleteConfirmation() => _confirmation.TrySetResult(true);
        }
    }
}
