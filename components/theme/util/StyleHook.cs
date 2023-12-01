// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using CssInCs;
using Microsoft.AspNetCore.Components;
using static CssInCs.StyleHelper;

namespace AntDesign
{
    internal static class StyleHook
    {
        internal static RenderFragment GenComponentStyleHook(string componentName, Func<TokenWithCommonCls, CSSInterpolation> func)
        {

            return GenComponentStyleHook(new[] { componentName, componentName }, func);
        }

        internal static RenderFragment GenComponentStyleHook(string[] componentNames, Func<TokenWithCommonCls, CSSInterpolation> func)
        {
            var token = new TokenWithCommonCls();
            var hash = token.GetTokenHash("5.11.4");
            var concatComponent = string.Join("-", componentNames);
            return UseStyleRegister(new StyleInfo[]
            {
                // Generate style for all a tags in antd component.
                new ()
                {
                    HashId = hash.HashId,
                    TokenKey = hash.TokenKey,
                    Path = new[] { "Shared", token.AntCls },
                    StyleFn = () => new CSSObject
                    {
                        ["&"] = GlobalStyle.GenLinkStyle(token)
                    },
                },

                // Generate style for icons
                new ()
                {
                    HashId = hash.HashId,
                    TokenKey = hash.TokenKey,
                    Path = new[] { "ant-design-icons", token.IconCls },
                    StyleFn = () => new CSSObject
                    {
                        [$".{token.IconCls}"] = new CSSObject
                        {
                            ["..."] = GlobalStyle.ResetIcon(),
                            [$".{token.IconCls} .{token.IconCls}-icon"] = new CSSObject
                            {
                                Display = "block"
                            }
                        }
                    },
                },

                // Generate component style
                new ()
                {
                    HashId = hash.HashId,
                    TokenKey = hash.TokenKey,
                    Path = new[] { concatComponent, token.PrefixCls, token.ComponentCls, token.IconCls },
                    StyleFn = () => func(token),
                }
            });
        }
    }
}
