using System;
using System.Globalization;
using AntDesign.JsInterop;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AntDesign.Tests
{
    public class AntDesignTestBase : TestContext, IDisposable
    {
        public TestContext Context => this;
        public NavigationManager NavigationManager => Services.GetRequiredService<NavigationManager>();

        public Mock<IDomEventListener> MockedDomEventListener { get; set; } = new Mock<IDomEventListener>();

        public AntDesignTestBase(bool useMoq = false)
        {
            Services.AddAntDesign();

            //Needed for Tests using Overlay
            Services.AddScoped<AntDesign.JsInterop.DomEventService>(sp => new TestDomEventService(Context.JSInterop.JSRuntime, MockedDomEventListener));
            JSInterop.SetupVoid(JSInteropConstants.OverlayComponentHelper.DeleteOverlayFromContainer, _ => true);

            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        }

        public new void Dispose()
        {
            Context?.Dispose();
        }
    }
}
