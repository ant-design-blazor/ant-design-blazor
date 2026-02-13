using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AntDesign.Tests.Dropdown
{
    public class ContextMenuDemoTests : AntDesignTestBase
    {
        [Fact]
        public async Task RightClick_calls_OverlayService_OpenAsync_with_mouse_coordinates()
        {
            var overlayMock = new Mock<IOverlayService>();
            overlayMock
                .Setup(s => s.OpenAsync(It.IsAny<RenderFragment>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<string>()))
                .ReturnsAsync((OverlayReference)null);

            Context.Services.AddScoped(_ => overlayMock.Object);

            var cut = Context.RenderComponent<ContextMenuDemo>();

            var area = cut.Find(".context-menu-demo-area");

            var mouseArgs = new MouseEventArgs { ClientX = 123, ClientY = 456 };

            area.TriggerEvent("oncontextmenu", mouseArgs);

            overlayMock.Verify(
                s => s.OpenAsync(
                    It.IsAny<RenderFragment>(),
                    123,
                    456,
                    It.IsAny<string>()),
                Times.Once);
        }
    }
}
