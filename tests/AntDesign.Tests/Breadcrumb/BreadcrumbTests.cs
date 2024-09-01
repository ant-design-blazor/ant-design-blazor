// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Bunit;
using Xunit;

namespace AntDesign.Tests.Breadcrumb
{
    public class BreadcrumbTests : AntDesignTestBase
    {
        [Fact]
        public void ItShouldRenderWrapperProperly()
        {
            var systemUnderTest = RenderComponent<AntDesign.Breadcrumb>();

            systemUnderTest.MarkupMatches("<nav class=\"ant-breadcrumb\" id:ignore><ol></ol></nav>");
        }

        [Fact]
        public void ItShouldRenderChildContent()
        {
            var systemUnderTest = RenderComponent<AntDesign.Breadcrumb>(p => p.AddChildContent("<span>Test</span>"));

            systemUnderTest.Find("span").MarkupMatches("<span>Test</span>");
        }
    }
}
