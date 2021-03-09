using Bunit;
using Xunit;

namespace AntDesign.Tests.Overlay
{
    public class OverlayTriggerTests : AntDesignTestBase
    {
        [Fact]
        public void Renders_a_base_overlay_trigger()
        {
            var cut = Context.RenderComponent<AntDesign.Internal.OverlayTrigger>();
        }
    }
}
