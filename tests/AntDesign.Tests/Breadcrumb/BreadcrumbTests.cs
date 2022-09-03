using System.Threading.Tasks;
using AntDesign.JsInterop;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
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
