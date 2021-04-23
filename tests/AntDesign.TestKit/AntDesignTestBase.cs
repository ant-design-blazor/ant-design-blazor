using System;
using System.Globalization;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace AntDesign.Tests
{
    public class AntDesignTestBase : TestContext, IDisposable
    {
        public TestNavigationManager NavigationManager { get; }

        protected TestContext Context => this;

        public AntDesignTestBase()
        {
            NavigationManager = new TestNavigationManager();

            Context.Services.AddScoped<NavigationManager>(sp => NavigationManager);
            Context.Services.AddAntDesign();

            //Needed for Tests using Overlay
            Context.Services.AddScoped<AntDesign.JsInterop.DomEventService>(sp => new TestDomEventService(Context.JSInterop.JSRuntime));

            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
