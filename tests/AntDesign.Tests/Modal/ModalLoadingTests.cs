// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Modal
{
    public class ModalLoadingTests : AntDesignTestBase
    {
        [Theory]
        [InlineData(false, 0)]
        [InlineData(false, 1)]
        [InlineData(false, 2)]
        [InlineData(true, 0)]
        [InlineData(true, 1)]
        [InlineData(true, 2)]
        public async Task ItShouldShowLoadingOnEveryConfirmationAfterCancelingClose(bool useSetConfirmLoading, int footerKind)
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            var cut = RenderComponent<ModalContainer>();
            var service = Services.GetRequiredService<ModalService>();
            var options = new ModalOptions();
            if (footerKind == 1)
            {
                options.Footer = ModalFooter.DefaultOkFooter;
            }
            else if (footerKind == 2)
            {
                options.Footer = (RenderFragment)(builder =>
                {
                    builder.OpenComponent<PropsLoadingFooter>(0);
                    builder.CloseComponent();
                });
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
                    await cut.Find(".ant-modal-footer .ant-btn-primary").ClickAsync(new MouseEventArgs());
                    feedback.ConfirmationCount.Should().Be(attempt + 1, "loading should prevent duplicate confirmations");
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

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ItShouldKeepButtonPropsLoadingSynchronizedForCustomFooterButtons(bool okOnlyFooter)
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            var buttonProps = new ButtonProps();
            RenderFragment footer = builder =>
            {
                builder.OpenComponent<CompositeLoadingFooter>(0);
                builder.AddAttribute(1, nameof(CompositeLoadingFooter.OkOnlyFooter), okOnlyFooter);
                builder.CloseComponent();
            };
            var cut = RenderComponent<AntDesign.Modal>(parameters => parameters
                .Add(x => x.Visible, true)
                .Add(x => x.ConfirmLoading, true)
                .Add(x => x.OkButtonProps, buttonProps)
                .Add(x => x.Footer, footer));

            cut.FindAll(".ant-modal-footer .ant-btn-primary.ant-btn-loading").Should().HaveCount(2);
            buttonProps.Loading.Should().BeTrue();

            cut.SetParametersAndRender(parameters => parameters.Add(x => x.ConfirmLoading, false));
            cut.FindAll(".ant-modal-footer .ant-btn-loading").Should().BeEmpty();
            buttonProps.Loading.Should().BeFalse();

            cut.SetParametersAndRender(parameters => parameters.Add(x => x.ConfirmLoading, true));
            cut.FindAll(".ant-modal-footer .ant-btn-primary.ant-btn-loading").Should().HaveCount(2);
            buttonProps.Loading.Should().BeTrue();
        }

        public class LoadingFeedback : FeedbackComponent<string>
        {
            private TaskCompletionSource<bool> _confirmation = new TaskCompletionSource<bool>();

            public bool UseSetConfirmLoading { get; set; }

            public bool CancelClose { get; set; } = true;

            public int ConfirmationCount { get; private set; }

            public override async Task OnFeedbackOkAsync(ModalClosingEventArgs args)
            {
                ConfirmationCount++;
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
