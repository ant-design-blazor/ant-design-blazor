using System;
using System.Globalization;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace AntDesign.Tests
{
    public class AntDesignTestBase : TestContext, IDisposable
    {
        public TestContext Context => this;
        public NavigationManager NavigationManager => Services.GetRequiredService<NavigationManager>();

        public AntDesignTestBase()
        {            
            Services.AddAntDesign();

            //Needed for Tests using Overlay
            Services.AddScoped<AntDesign.JsInterop.DomEventService>(sp => new TestDomEventService(Context.JSInterop.JSRuntime));

            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
