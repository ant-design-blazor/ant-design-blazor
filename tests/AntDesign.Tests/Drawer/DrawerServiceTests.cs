// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Drawer
{
    public class DrawerServiceTests : AntDesignTestBase
    {
        public DrawerServiceTests()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
        }

        [Theory]
        [InlineData(".ant-drawer-close", false)]
        [InlineData(".ant-drawer-mask", false)]
        [InlineData(".ant-drawer-close", true)]
        [InlineData(".ant-drawer-mask", true)]
        public async Task ItShouldCloseOnceAndHonorCancellation(string selector, bool cancel)
        {
            var cut = RenderComponent<DrawerContainer>();
            var service = Services.GetRequiredService<DrawerService>();
            var drawer = await cut.InvokeAsync(() => service.CreateAsync(new DrawerOptions()));
            var closing = 0;
            var closed = 0;
            drawer.OnClosing = args =>
            {
                closing++;
                args.Cancel = cancel;
                return Task.CompletedTask;
            };
            drawer.OnClose = () =>
            {
                closed++;
                return Task.CompletedTask;
            };

            await cut.Find(selector).ClickAsync(new());

            cut.WaitForAssertion(() =>
            {
                closing.Should().Be(1);
                closed.Should().Be(cancel ? 0 : 1);
                cut.FindAll(".ant-drawer-open").Should().HaveCount(cancel ? 1 : 0);
                drawer.Config.Visible.Should().Be(cancel);
                if (!cancel)
                    cut.FindAll(".ant-drawer").Should().BeEmpty();
            });

            if (cancel)
            {
                cancel = false;
                await cut.Find(selector).ClickAsync(new());
                cut.WaitForAssertion(() => closed.Should().Be(1));
                closing.Should().Be(2);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ItShouldNotifyOnClosedForBothCloseOverloads(bool withResult)
        {
            var cut = RenderComponent<DrawerContainer>();
            var service = Services.GetRequiredService<DrawerService>();
            var drawer = await cut.InvokeAsync(() => service.CreateAsync<TestFeedback, string, string>(new DrawerOptions(), "initial"));
            var feedback = cut.FindComponent<TestFeedback>().Instance;
            var closed = 0;
            var closing = 0;
            var onClose = 0;
            string? result = "not closed";
            drawer.OnClosing = _ =>
            {
                closing++;
                return Task.CompletedTask;
            };
            drawer.OnClose = () =>
            {
                onClose++;
                return Task.CompletedTask;
            };
            drawer.OnClosed = value =>
            {
                closed++;
                result = value;
                return Task.CompletedTask;
            };

            await cut.InvokeAsync(() => withResult ? drawer.CloseAsync("updated") : drawer.CloseAsync());

            closed.Should().Be(1);
            closing.Should().Be(1);
            onClose.Should().Be(1);
            result.Should().Be(withResult ? "updated" : null);
            feedback.CancelCalls.Should().Be(0);
        }

        [Theory]
        [InlineData(".ant-drawer-close")]
        [InlineData(".ant-drawer-mask")]
        public async Task ItShouldInvokeFeedbackCancellationAndAllowRejection(string selector)
        {
            var cut = RenderComponent<DrawerContainer>();
            var service = Services.GetRequiredService<DrawerService>();
            var drawer = await cut.InvokeAsync(() => service.CreateAsync<TestFeedback, string, string>(new DrawerOptions(), "initial"));
            var feedback = cut.FindComponent<TestFeedback>().Instance;
            feedback.RejectClose = true;

            await cut.Find(selector).ClickAsync(new());

            cut.WaitForAssertion(() =>
            {
                feedback.CancelCalls.Should().Be(1);
                cut.FindAll(".ant-drawer-open").Should().HaveCount(1);
                drawer.Config.Visible.Should().BeTrue();
            });

            feedback.RejectClose = false;
            await cut.Find(selector).ClickAsync(new());
            cut.WaitForAssertion(() => cut.FindAll(".ant-drawer").Should().BeEmpty());
            feedback.CancelCalls.Should().Be(2);
        }

        [Fact]
        public async Task ItShouldCompleteDialogWhenDismissedWithoutAResult()
        {
            var cut = RenderComponent<DrawerContainer>();
            var service = Services.GetRequiredService<DrawerService>();
            Task<string>? dialog = null;
            await cut.InvokeAsync(() =>
            {
                dialog = service.CreateDialogAsync<TestFeedback, string, string>(new DrawerOptions(), "initial");
            });
            cut.WaitForAssertion(() => cut.FindAll(".ant-drawer-open").Should().HaveCount(1));

            await cut.Find(".ant-drawer-close").ClickAsync(new());

            cut.WaitForAssertion(() => dialog!.IsCompletedSuccessfully.Should().BeTrue());
            (await dialog!).Should().BeNull();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ItShouldKeepDrawerOpenWhileAwaitingClosingConfirmation(bool reject)
        {
            var cut = RenderComponent<DrawerContainer>();
            var service = Services.GetRequiredService<DrawerService>();
            var drawer = await cut.InvokeAsync(() => service.CreateAsync(new DrawerOptions()));
            var confirmation = new TaskCompletionSource<bool>();
            var closing = 0;
            drawer.OnClosing = async args =>
            {
                closing++;
                args.Cancel = await confirmation.Task;
            };

            var close = cut.Find(".ant-drawer-close").ClickAsync(new());
            cut.WaitForAssertion(() => closing.Should().Be(1));
            cut.FindAll(".ant-drawer-open").Should().HaveCount(1);
            drawer.Config.Visible.Should().BeTrue();

            // A second click must not open another confirmation while the first is pending.
            await cut.Find(".ant-drawer-close").ClickAsync(new());
            closing.Should().Be(1);
            await cut.InvokeAsync(() => confirmation.SetResult(reject));
            await close;

            cut.WaitForAssertion(() => cut.FindAll(".ant-drawer-open").Should().HaveCount(reject ? 1 : 0));
            drawer.Config.Visible.Should().Be(reject);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ItShouldHonorCancellationForProgrammaticClose(bool withResult)
        {
            var cut = RenderComponent<DrawerContainer>();
            var service = Services.GetRequiredService<DrawerService>();
            var drawer = await cut.InvokeAsync(() => service.CreateAsync<TestFeedback, string, string>(new DrawerOptions(), "initial"));
            var closed = 0;
            drawer.OnClosing = args =>
            {
                args.Reject();
                return Task.CompletedTask;
            };
            drawer.OnClosed = _ =>
            {
                closed++;
                return Task.CompletedTask;
            };

            await cut.InvokeAsync(() => withResult ? drawer.CloseAsync("updated") : drawer.CloseAsync());

            closed.Should().Be(0);
            cut.FindAll(".ant-drawer-open").Should().HaveCount(1);
            drawer.OnClosing = null;
            await cut.InvokeAsync(() => withResult ? drawer.CloseAsync("updated") : drawer.CloseAsync());
            closed.Should().Be(1);
        }

        [Fact]
        public async Task ItShouldNotifyOncePerCloseAndAllowReopening()
        {
            var cut = RenderComponent<DrawerContainer>();
            var service = Services.GetRequiredService<DrawerService>();
            var drawer = await cut.InvokeAsync(() => service.CreateAsync<TestFeedback, string, string>(new DrawerOptions(), "initial"));
            var closed = 0;
            drawer.OnClosed = _ =>
            {
                closed++;
                return Task.CompletedTask;
            };

            await cut.InvokeAsync(() => drawer.CloseAsync("first"));
            await cut.InvokeAsync(() => drawer.CloseAsync("duplicate"));
            closed.Should().Be(1);
            await cut.InvokeAsync(() => drawer.OpenAsync());
            cut.FindAll(".ant-drawer-open").Should().HaveCount(1);
            await cut.InvokeAsync(() => drawer.CloseAsync("second"));
            closed.Should().Be(2);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ItShouldInvokeTheCurrentFeedbackComponentAfterReopening(bool afterNavigation)
        {
            var cut = RenderComponent<DrawerContainer>();
            var service = Services.GetRequiredService<DrawerService>();
            var drawer = await cut.InvokeAsync(() => service.CreateAsync<TestFeedback, string, string>(new DrawerOptions(), "initial"));
            var previousFeedback = cut.FindComponent<TestFeedback>().Instance;

            if (afterNavigation)
            {
                await cut.InvokeAsync(() => NavigationManager.NavigateTo("http://localhost/reopened"));
                cut.WaitForAssertion(() => cut.FindAll(".ant-drawer").Should().BeEmpty());
            }
            else
            {
                await cut.InvokeAsync(() => drawer.CloseAsync());
            }
            await cut.InvokeAsync(() => drawer.OpenAsync());
            var currentFeedback = cut.FindComponent<TestFeedback>().Instance;
            currentFeedback.Should().NotBeSameAs(previousFeedback);
            currentFeedback.RejectClose = true;

            await cut.Find(".ant-drawer-close").ClickAsync(new());

            currentFeedback.CancelCalls.Should().Be(1);
            previousFeedback.CancelCalls.Should().Be(0);
            cut.FindAll(".ant-drawer-open").Should().HaveCount(1);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ItShouldAwaitFeedbackCancellationOnce(bool reject)
        {
            var cut = RenderComponent<DrawerContainer>();
            var service = Services.GetRequiredService<DrawerService>();
            var drawer = await cut.InvokeAsync(() => service.CreateAsync<TestFeedback, string, string>(new DrawerOptions(), "initial"));
            var feedback = cut.FindComponent<TestFeedback>().Instance;
            feedback.Cancellation = new TaskCompletionSource<bool>();

            var close = cut.Find(".ant-drawer-close").ClickAsync(new());
            cut.WaitForAssertion(() => feedback.CancelCalls.Should().Be(1));
            await cut.Find(".ant-drawer-close").ClickAsync(new());
            feedback.CancelCalls.Should().Be(1);
            cut.FindAll(".ant-drawer-open").Should().HaveCount(1);

            await cut.InvokeAsync(() => feedback.Cancellation.SetResult(reject));
            await close;

            drawer.Config.Visible.Should().Be(reject);
            cut.FindAll(".ant-drawer-open").Should().HaveCount(reject ? 1 : 0);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ItShouldAllowClosingAReopenedDrawerWhileThePreviousNotificationIsPending(bool closeByClick)
        {
            var cut = RenderComponent<DrawerContainer>();
            var service = Services.GetRequiredService<DrawerService>();
            var drawer = await cut.InvokeAsync(() => service.CreateAsync<TestFeedback, string, string>(new DrawerOptions(), "initial"));
            var notification = new TaskCompletionSource<bool>();
            var closed = 0;
            drawer.OnClosed = async _ =>
            {
                closed++;
                if (closed == 1)
                {
                    await drawer.OpenAsync();
                    await notification.Task;
                }
            };

            var firstClose = closeByClick
                ? cut.Find(".ant-drawer-close").ClickAsync(new())
                : cut.InvokeAsync(() => drawer.CloseAsync("first"));
            try
            {
                cut.WaitForAssertion(() =>
                {
                    closed.Should().Be(1);
                    cut.FindAll(".ant-drawer-open").Should().HaveCount(1);
                });

                await cut.Find(".ant-drawer-close").ClickAsync(new());

                closed.Should().Be(2);
                drawer.Config.Visible.Should().BeFalse();
            }
            finally
            {
                await cut.InvokeAsync(() => notification.TrySetResult(true));
                await firstClose;
            }
        }

        [Fact]
        public async Task ItShouldNotifyOnceWhileTheClosedNotificationIsPending()
        {
            var cut = RenderComponent<DrawerContainer>();
            var service = Services.GetRequiredService<DrawerService>();
            var drawer = await cut.InvokeAsync(() => service.CreateAsync<TestFeedback, string, string>(new DrawerOptions(), "initial"));
            var notification = new TaskCompletionSource<bool>();
            var notificationStarted = new TaskCompletionSource<bool>();
            var closed = 0;
            drawer.OnClosed = async _ =>
            {
                closed++;
                notificationStarted.TrySetResult(true);
                await notification.Task;
            };

            var close = cut.InvokeAsync(() => drawer.CloseAsync("first"));
            try
            {
                // This callback does not render, so wait for its explicit signal.
                await Task.WhenAny(notificationStarted.Task, Task.Delay(5000));
                notificationStarted.Task.IsCompletedSuccessfully.Should().BeTrue();

                await cut.InvokeAsync(() => drawer.CloseAsync("duplicate"));

                closed.Should().Be(1);
                cut.FindAll(".ant-drawer").Should().BeEmpty();
            }
            finally
            {
                await cut.InvokeAsync(() => notification.TrySetResult(true));
                await close;
            }
        }

        public class TestFeedback : FeedbackComponent<string, string>
        {
            public int CancelCalls { get; private set; }
            public bool RejectClose { get; set; }
            public TaskCompletionSource<bool>? Cancellation { get; set; }

            public override async Task OnFeedbackCancelAsync(ModalClosingEventArgs args)
            {
                CancelCalls++;
                args.Cancel = Cancellation == null ? RejectClose : await Cancellation.Task;
            }
        }
    }
}
