// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
