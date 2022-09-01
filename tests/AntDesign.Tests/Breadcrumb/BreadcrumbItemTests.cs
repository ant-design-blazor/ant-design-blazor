using Bunit;
using FluentAssertions;
using Xunit;

namespace AntDesign.Tests.Breadcrumb
{
    public class BreadcrumbItemTests : AntDesignTestBase
    {
        [Fact]
        public void ItShouldRenderLinkWhenHrefGiven()
        {
            const string Href = "#SomeAnchor";

            var systemUnderTest = RenderComponent<BreadcrumbItem>(parameters => parameters
                .Add(x => x.Href, Href)
                .Add(x => x.ChildContent, "Child Content")
            );

            systemUnderTest.MarkupMatches($@"<span>
                <span class=""ant-breadcrumb-link"">
                    <a href=""{Href}"">Child Content</a>
                </span>
            </span>");
        }

        [Fact]
        public void ItShouldNotRenderLInkWhenHrefNotGiven()
        {
            var systemUnderTest = RenderComponent<BreadcrumbItem>(parameters => parameters
                .Add(x => x.ChildContent, "Child Content")
            );

            systemUnderTest.MarkupMatches($@"<span>
                <span class=""ant-breadcrumb-link"">Child Content</span>
            </span>");
        }

        [Fact]
        public void ItemShouldGetBreadcrumbSeparatorAndRenderProperly()
        {
            var systemUnderTest = RenderComponent<AntDesign.Breadcrumb>(parameters => parameters
                .Add(x => x.Separator, "-")
                .AddChildContent<BreadcrumbItem>(itemParameters => itemParameters.Add(x => x.ChildContent, "Link Item"))
            );

            systemUnderTest.Find("div > span").MarkupMatches($@"<span>
                <span class=""ant-breadcrumb-link"">Link Item</span>
		        <span class=""ant-breadcrumb-separator"">-</span>
            </span>");
        }

        [Fact]
        public void ItShouldCallOnClickWhenProvidedAndClicked()
        {
            var onClickCalled = false;

            var systemUnderTest = RenderComponent<BreadcrumbItem>(parameters => parameters
                .Add(x => x.ChildContent, "Child Content")
                .Add(x => x.OnClick, () => { onClickCalled = true; })
            );

            systemUnderTest.Find("span").Click();

            onClickCalled.Should().BeTrue();
        }

        [Fact]
        public void ItShouldRenderDropdownWhenOverlayProvided()
        {
            JSInterop.SetupVoid("AntDesign.interop.styleHelper.addCls", _ => true);

            var systemUnderTest = RenderComponent<BreadcrumbItem>(parameters => parameters
                .Add(x => x.ChildContent, "Child Content")
                .Add(x => x.Overlay, "")
            );

            systemUnderTest.MarkupMatches($@"<span>
                <span class=""ant-breadcrumb-overlay-link"">
	                <span class=""ant-breadcrumb-link"">Child Content</span>
                    <span role=""img"" class="" anticon anticon-down"" id:ignore>
                      <svg focusable=""false"" width=""1em"" height=""1em"" fill=""currentColor"" style=""pointer-events: none;"" xmlns=""http://www.w3.org/2000/svg"" class=""icon"" viewBox=""0 0 1024 1024"">
                        <path d=""M884 256h-75c-5.1 0-9.9 2.5-12.9 6.6L512 654.2 227.9 262.6c-3-4.1-7.8-6.6-12.9-6.6h-75c-6.5 0-10.3 7.4-6.5 12.7l352.6 486.1c12.8 17.6 39 17.6 51.7 0l352.6-486.1c3.9-5.3.1-12.7-6.4-12.7z""></path>
                      </svg>
                    </span>
                </span>
            </span>");
        }
    }
}
