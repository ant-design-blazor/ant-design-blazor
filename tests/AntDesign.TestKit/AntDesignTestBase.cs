using System;
using System.Globalization;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AntDesign.Tests
{
    public class AntDesignTestBase : IDisposable
    {
        public TestContext Context { get; }
        public TestNavigationManager NavigationManager { get; }

        public AntDesignTestBase()
        {
            Context = new TestContext();
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
