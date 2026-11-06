// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Tabs
{
    public class ReuseTabsServiceTests : AntDesignTestBase
    {
        private ReuseTabsService CreateService()
        {
            return Services.GetRequiredService<ReuseTabsService>();
        }

        /// <summary>
        /// Regression test for https://github.com/ant-design-blazor/ant-design-blazor/issues/4796
        /// ScanPinnedPageAttribute should not throw when dynamic assemblies are present in AppDomain.
        /// </summary>
        [Fact]
        public void Init_WithDynamicAssemblyInAppDomain_DoesNotThrow()
        {
            // Arrange: create a dynamic assembly so it appears in AppDomain.CurrentDomain.GetAssemblies()
            var dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("TestDynamicAssembly_" + System.Guid.NewGuid()),
                AssemblyBuilderAccess.Run);

            dynamicAssembly.IsDynamic.Should().BeTrue("the assembly should be dynamic");

            var service = CreateService();

            // Act & Assert: Init(true) calls ScanPinnedPageAttribute which iterates all assemblies
            var act = () => service.Init(true);
            act.Should().NotThrow<NotSupportedException>();
            act.Should().NotThrow();
        }

        /// <summary>
        /// ScanPinnedPageAttribute should not scan when no assembly list is provided.
        /// </summary>
        [Fact]
        public void Init_WithNullScanAssemblies_DoesNotScanPinnedPages()
        {
            var service = CreateService();

            service.Init(true);

            service.Pages.Should().BeEmpty();
        }

        [Fact]
        public void ReuseTabs_DefaultScanAssemblies_UsesPageAssembly()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;

            var routeData = new RouteData(typeof(FakePinnedPage), new Dictionary<string, object?>());
            RenderComponent<ReuseTabs>(parameters => parameters.AddCascadingValue(routeData));

            var service = CreateService();

            service.Pages.Should().Contain(p => p.Url == "/fake-pinned");
        }

        [Fact]
        public void Init_WithEmptyScanAssemblies_DoesNotScanPinnedPages()
        {
            var service = CreateService();

            // Act
            service.Init(true, Array.Empty<Assembly>());

            // Assert
            service.Pages.Should().BeEmpty();
        }

        [Fact]
        public void Init_WithSpecifiedScanAssemblies_UsesProvidedAssemblyList()
        {
            var service = CreateService();
            var targetAssembly = Assembly.GetExecutingAssembly();

            // Act
            service.Init(true, new[] { targetAssembly });

            // Assert
            service.Pages.Should().Contain(p => p.Url == "/fake-pinned");
        }

        /// <summary>
        /// Regression test for a missing referenced assembly during ExportedTypes enumeration.
        /// </summary>
        [Fact]
        public void Init_WithExportedTypesThrowFileNotFoundException_DoesNotThrow()
        {
            var service = new ReuseTabsServiceWithMissingDependencyExport(
                Services.GetRequiredService<NavigationManager>(),
                Services.GetRequiredService<MenuService>());

            var act = () => service.Init(true, new[] { Assembly.GetExecutingAssembly() });
            act.Should().NotThrow();
        }

        private class ReuseTabsServiceWithMissingDependencyExport : ReuseTabsService
        {
            public ReuseTabsServiceWithMissingDependencyExport(NavigationManager navmgr, MenuService menusService)
                : base(navmgr, menusService)
            {
            }

            protected override Type[] GetExportedTypes(Assembly assembly)
            {
                throw new FileNotFoundException("Could not load file or assembly 'SkiaSharp'.", "SkiaSharp, Version=3.119.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
            }
        }
    }

    [Route("/fake-pinned")]
    [ReuseTabsPage(Pin = true, PinUrl = "/fake-pinned")]
    public class FakePinnedPage : ComponentBase
    {
    }
}
