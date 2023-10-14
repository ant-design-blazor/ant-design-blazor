﻿using System;
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
        protected string DateFormat => "dd/MM/yyyy";
        protected string TimeFormat = "yyyy-MM-dd HH:mm:ss";
        protected readonly CultureInfo Culture = CultureInfo.InvariantCulture;
        
        public TestContext Context => this;
        public NavigationManager NavigationManager => Services.GetRequiredService<NavigationManager>();

        public Mock<IDomEventListener> MockedDomEventListener { get; set; } = new Mock<IDomEventListener>();

        public AntDesignTestBase(bool useMoq = false)
        {
            Services.AddAntDesign();

            //Needed for Tests using Overlay
            Services.AddScoped<AntDesign.JsInterop.DomEventService>(sp => new TestDomEventService(Context.JSInterop.JSRuntime, MockedDomEventListener));
            JSInterop.SetupVoid(JSInteropConstants.OverlayComponentHelper.DeleteOverlayFromContainer, _ => true);
            JSInterop.Mode = JSRuntimeMode.Strict;

            LocaleProvider.SetLocale("en-US");
        }

        public new void Dispose()
        {
            Context?.Dispose();
        }
    }
}
