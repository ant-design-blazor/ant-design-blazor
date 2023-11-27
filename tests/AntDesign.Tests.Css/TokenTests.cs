// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign.Tests.Css
{
    public class TokenTests
    {
        [Fact]
        public void Gen_Token_Key()
        {
            var token = new GlobalToken();
            var version = "5.11.4";
            var salt = $"{version}-true";
            var tokens = token.GetToken();
            var tokenStr = token.FlattenToken(tokens);
            var tokenKey = token.Hash($"{salt}_{tokenStr}");
            Assert.Equal(tokenKey, "1iw360o");
        }
    }
}
