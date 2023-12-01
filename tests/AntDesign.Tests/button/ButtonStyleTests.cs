// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit;

namespace AntDesign.Tests.button
{
    public class ButtonStyleTests : AntDesignTestBase
    {
        [Fact]
        public void Gen_Button_ComponentStyle()
        {
            var token = new TokenWithCommonCls();
            token.AntCls = "";
            token.PrefixCls = "";
            token.ComponentCls = ".ant-btn";
            var tokenHash = token.GetTokenHash(StyleVersion);
            var button = new AntDesign.Button();
        }
    }
}
