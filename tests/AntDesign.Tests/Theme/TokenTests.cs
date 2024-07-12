// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Shouldly;
using Xunit;

namespace AntDesign.Tests.Theme
{
    public class TokenTests : AntDesignTestBase
    {
        [Fact]
        public void Gen_Token_Hash()
        {
            var token = Seed.DefaultSeedToken;
            var tokenHash = token.GetTokenHash(StyleVersion);
            tokenHash.ShouldNotBeNull();
            tokenHash.TokenKey.ShouldBe("18wx8kn");
        }
    }
}
