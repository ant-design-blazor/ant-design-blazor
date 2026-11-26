// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace AntDesign.Tests.Modal
{
    public class ModalConfirmationTests : AntDesignTestBase
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ManualConfirmationWaitsForValidationBeforeClosing(bool isValid)
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            var validationResult = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var validationStarted = false;
            var visibilityChanges = new List<bool>();
            var cancelCount = 0;
            AntDesign.Modal modal = null!;
            var cut = RenderComponent<AntDesign.Modal>(parameters => parameters
                .Add(x => x.Visible, true)
                .Add(x => x.VisibleChanged, value => visibilityChanges.Add(value))
                .Add(x => x.CloseOnOk, false)
                .Add(x => x.OnCancel, _ => { cancelCount++; })
                .Add(x => x.OnOk, async () =>
                {
                    validationStarted = true;
                    if (await validationResult.Task)
                    {
                        await modal.CloseAsync();
                    }
                }));
            modal = cut.Instance;

            var confirmation = cut.Find(".ant-modal-footer .ant-btn-primary").ClickAsync(new MouseEventArgs());
            try
            {
                cut.WaitForAssertion(() => Assert.True(validationStarted));
                Assert.Empty(visibilityChanges);
            }
            finally
            {
                validationResult.SetResult(isValid);
                await confirmation;
            }

            Assert.Equal(isValid ? new[] { false } : System.Array.Empty<bool>(), visibilityChanges);
            Assert.Equal(0, cancelCount);
        }

        [Fact]
        public async Task DefaultConfirmationStillRequestsClosingBeforeInvokingOnOk()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            var events = new List<string>();
            var cut = RenderComponent<AntDesign.Modal>(parameters => parameters
                .Add(x => x.Visible, true)
                .Add(x => x.VisibleChanged, value => events.Add($"Visible: {value}"))
                .Add(x => x.OnOk, _ => events.Add("OnOk")));

            await cut.Find(".ant-modal-footer .ant-btn-primary").ClickAsync(new MouseEventArgs());

            Assert.Equal(new[] { "Visible: False", "OnOk" }, events);
        }

        [Fact]
        public async Task ManualConfirmationStillAllowsCanceling()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            var visibilityChanges = new List<bool>();
            var cut = RenderComponent<AntDesign.Modal>(parameters => parameters
                .Add(x => x.Visible, true)
                .Add(x => x.VisibleChanged, value => visibilityChanges.Add(value))
                .Add(x => x.CloseOnOk, false));

            await cut.Find(".ant-modal-footer button:not(.ant-btn-primary)").ClickAsync(new MouseEventArgs());

            Assert.Equal(new[] { false }, visibilityChanges);
        }

        [Fact]
        public async Task CloseAsyncKeepsAnUnboundModalClosedUntilVisibilityChanges()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            var cut = RenderComponent<AntDesign.Modal>(parameters => parameters
                .Add(x => x.Visible, true)
                .Add(x => x.Title, "Form"));

            await cut.Instance.CloseAsync();
            Assert.False(cut.FindComponent<DialogWrapper>().Instance.Visible);

            cut.SetParametersAndRender(parameters => parameters
                .Add(x => x.Visible, true)
                .Add(x => x.Title, "Updated form"));
            Assert.False(cut.FindComponent<DialogWrapper>().Instance.Visible);

            cut.SetParametersAndRender(parameters => parameters.Add(x => x.Visible, false));
            cut.SetParametersAndRender(parameters => parameters.Add(x => x.Visible, true));
            Assert.True(cut.FindComponent<DialogWrapper>().Instance.Visible);
        }
    }
}
