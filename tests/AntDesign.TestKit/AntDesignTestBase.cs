// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using AntDesign.JsInterop;
using AntDesign.Core.JsInterop.Modules.Components;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AntDesign.Tests
{
    public class AntDesignTestBase : TestContext, IDisposable
    {
        protected const string DateFormat = "dd/MM/yyyy";
        protected const string TimeFormat = "yyyy-MM-dd HH:mm:ss";
        protected readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        public TestContext Context => this;
        public NavigationManager NavigationManager => Services.GetRequiredService<NavigationManager>();

        public Mock<IDomEventListener> MockedDomEventListener { get; set; } = new Mock<IDomEventListener>();

        public AntDesignTestBase(bool useMoq = false)
        {
            Services.AddAntDesign();

            //Needed for Tests using Overlay
            Services.AddScoped<DomEventService>(sp => new TestDomEventService(Context.JSInterop.JSRuntime, MockedDomEventListener));
            JSInterop.SetupVoid(JSInteropConstants.OverlayComponentHelper.DeleteOverlayFromContainer, _ => true);
            JSInterop.SetupVoid(JSInteropConstants.OverlayComponentHelper.BindImmediateOpen, _ => true);
            // Overlay width is prepared in JavaScript before an animated popup
            // is revealed. Most bUnit tests do not model layout, so return no
            // measured width unless a test supplies a more specific result.
            JSInterop.Setup<string>(JSInteropConstants.OverlayComponentHelper.PrepareOverlayWidthForShow, _ => true)
                .SetResult(string.Empty);
            JSInterop.Setup<string>(JSInteropConstants.OverlayComponentHelper.GetInheritedAntDesignTokenStyle, _ => true)
                .SetResult(string.Empty);
            JSInterop.SetupVoid(JSInteropConstants.OverlayComponentHelper.RevealOverlayForShow, _ => true);
            JSInterop.Setup<OverlayPosition>(JSInteropConstants.OverlayComponentHelper.UpdateOverlayPosition, _ => true)
                .SetResult(new OverlayPosition { Placement = Placement.BottomLeft, ZIndex = 5000 });
            JSInterop.SetupVoid(JSInteropConstants.AddPreventKeys, _ => true);
            JSInterop.SetupVoid(JSInteropConstants.AddPreventEnterOnOverlayVisible, _ => true);
            JSInterop.SetupVoid(JSInteropConstants.SetDomAttribute, _ => true);
            JSInterop.SetupVoid("AntDesign.interop.tableHelper.bindTableScroll", _ => true);
            JSInterop.SetupVoid("AntDesign.interop.domManipulationHelper.blur", _ => true);
            JSInterop.SetupVoid("AntDesign.interop.domManipulationHelper.smoothScrollTo", _ => true);

            // Provide a default getWindow stub so layout components calling into domInfoHelper.getWindow do not fail in tests
            JSInterop.Setup<Window>(JSInteropConstants.GetWindow, _ => true).SetResult(new Window());
            JSInterop.SetupVoid("AntDesign.interop.menuOverflowHelper.observe", _ => true).SetVoidResult();
            JSInterop.SetupVoid("AntDesign.interop.menuOverflowHelper.unobserve", _ => true).SetVoidResult();

            JSInterop.Mode = JSRuntimeMode.Strict;

            LocaleProvider.SetLocale("en-US");
        }

        public new void Dispose()
        {
            Context?.Dispose();
        }
    }
}
