// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Threading.Tasks;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Notification
{
    public class NotificationCultureTests : AntDesignTestBase
    {
        [Theory]
        [InlineData(NotificationPlacement.TopRight, "top:24.5px")]
        [InlineData(NotificationPlacement.Top, "inset: 24.5px auto auto 50%")]
        [InlineData(NotificationPlacement.BottomLeft, "bottom: 16.25px")]
        public async Task Offsets_are_written_with_a_dot_under_a_comma_culture(NotificationPlacement placement, string expectedStyle)
        {
            var culture = CultureInfo.CurrentCulture;
            var uiCulture = CultureInfo.CurrentUICulture;
            try
            {
                // tr-TR writes 24.5 as "24,5", which a browser rejects as a CSS length
                CultureInfo.CurrentCulture = new CultureInfo("tr-TR");
                CultureInfo.CurrentUICulture = new CultureInfo("tr-TR");
                JSInterop.Mode = JSRuntimeMode.Loose;

                var cut = RenderComponent<AntDesign.Notification>();
                var service = Services.GetRequiredService<INotificationService>();
                service.Config(new NotificationGlobalConfig { Top = 24.5, Bottom = 16.25 });

                // Open completes only when the notification closes, so don't wait for it
                await cut.InvokeAsync(() =>
                {
                    _ = service.Open(new NotificationConfig
                    {
                        Message = "Test",
                        Placement = placement,
                        Duration = 0
                    });
                });

                cut.WaitForAssertion(() => cut.Markup.Should().Contain(expectedStyle));
            }
            finally
            {
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = uiCulture;
            }
        }
    }
}
