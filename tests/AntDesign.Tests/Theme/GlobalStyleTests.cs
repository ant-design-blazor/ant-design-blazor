// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Bunit;
using CssInCSharp;
using Xunit;

namespace AntDesign.Tests.Theme
{
    public class GlobalStyleTests : AntDesignTestBase
    {
        private readonly GlobalToken _token = new();

        [Fact]
        public void Gen_Link_Style()
        {
            var tokenHash = _token.GetTokenHash(StyleVersion);
            var cut = RenderComponent<Style>(parameters => parameters
                .Add(p => p.HashId, tokenHash.HashId)
                .Add(p => p.TokenKey, tokenHash.TokenKey)
                .Add(p => p.Path, $"{tokenHash.TokenKey}|Shared|ant")
                .Add(p => p.StyleFn, () => new CSSObject { ["&"] = GlobalStyle.GenLinkStyle(_token) }));
            cut.MarkupMatches(@"<style data-css-hash:ignore data-token-hash:ignore data-cache-path:ignore>:where(.css-dev-only-do-not-override-zcfrx9) a{color:#1677ff;text-decoration:none;background-color:transparent;outline:none;cursor:pointer;transition:color 0.3s;-webkit-text-decoration-skip:objects;}:where(.css-dev-only-do-not-override-zcfrx9) a:hover{color:#69b1ff;}:where(.css-dev-only-do-not-override-zcfrx9) a:active{color:#0958d9;}:where(.css-dev-only-do-not-override-zcfrx9) a:active,:where(.css-dev-only-do-not-override-zcfrx9) a:hover{text-decoration:none;outline:0;}:where(.css-dev-only-do-not-override-zcfrx9) a:focus{text-decoration:none;outline:0;}:where(.css-dev-only-do-not-override-zcfrx9) a[disabled]{color:rgba(0, 0, 0, 0.25);cursor:not-allowed;}</style>");
        }
    }
}
