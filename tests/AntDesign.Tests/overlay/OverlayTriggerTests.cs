// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using AntDesign;

namespace AntDesign.Tests.Overlay
{
    public class OverlayTriggerTests : AntDesignTestBase
    {
        [Fact]
        public void Renders_a_base_overlay_trigger()
        {
            var cut = Context.RenderComponent<AntDesign.Internal.OverlayTrigger>();
        }

        [Fact]
        public void Leave_animation_uses_the_reverse_of_the_adjusted_placement()
        {
            var cut = Context.RenderComponent<AntDesign.Internal.OverlayTrigger>();
            cut.Instance.ChangePlacementForShow(PlacementType.Create(Placement.TopLeft));

            Assert.Contains("ant-slide-down-enter", cut.Instance.GetOverlayEnterClass());
            Assert.Equal("ant-slide-up-leave ant-slide-up-leave-active ant-slide-up", cut.Instance.GetOverlayLeaveClass());

            cut.Instance.ChangePlacementForShow(PlacementType.Create(Placement.BottomLeft));
            Assert.Equal("ant-slide-up-enter ant-slide-up-enter-active ant-slide-up", cut.Instance.GetOverlayEnterClass());
            Assert.Equal("ant-slide-down-leave ant-slide-down-leave-active ant-slide-down", cut.Instance.GetOverlayLeaveClass());
        }
    }
}
