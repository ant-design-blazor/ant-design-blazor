// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Bunit;
using CssInCs;

namespace AntDesign.Tests.Css
{
    public class GlobalStyleTests : TestContext
    {
        [Fact]
        public void Gen_Link_Style()
        {
            var token = new GlobalToken();
            var render = new Func<CSSInterpolation>(() => GlobalStyle.GenLinkStyle(token));
            var cut = RenderComponent<Style>(parameters => parameters
                .Add(p => p.HashId, "")
                .Add(p => p.Token, "1iw360o")
                .Add(p => p.Path, new[] { "Shared", "ant" })
                .Add(p => p.StyleFn, render));
            cut.MarkupMatches(@"<style data-css-hash="""" data-token-hash=""1iw360o"" data-cache-path=""1iw360o|Shared|ant"">a{color:#1677ff;text-decoration:none;background-color:transparent;outline:none;cursor:pointer;transition:color 0.3s;-webkit-text-decoration-skip:objects;}a:hover{color:#69b1ff;}a:active{color:#0958d9;}a:active,a:hover{text-decoration:none;outline:0;}a:focus{text-decoration:none;outline:0;}a[disabled]{color:rgba(0, 0, 0, 0.25);cursor:not-allowed;}</style>");
        }
    }
}
