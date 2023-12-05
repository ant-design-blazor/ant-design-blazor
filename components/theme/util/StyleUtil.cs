// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using CssInCSharp;
using static CssInCSharp.StyleHelper;

namespace AntDesign
{
    public delegate string UseComponentStyleResult(string prefixCls);

    internal class StyleUtil
    {
        internal static UseComponentStyleResult GenComponentStyleHook(string componentName, Func<TokenWithCommonCls, CSSInterpolation> styleFn, Func<IToken> getDefaultToken = null)
        {
            return GenComponentStyleHook(new[] { componentName, componentName }, styleFn, getDefaultToken);
        }

        internal static UseComponentStyleResult GenComponentStyleHook(string[] componentNames, Func<TokenWithCommonCls, CSSInterpolation> styleFn, Func<IToken> getDefaultToken = null)
        {
            var concatComponent = string.Join("-", componentNames);
            return (prefixCls) =>
            {
                var token = Seed.DefaultSeedToken;
                var hash = token.GetTokenHash();
                var componentToken = new TokenWithCommonCls();
                componentToken.Merge(token);
                componentToken.PrefixCls = prefixCls;
                componentToken.ComponentCls = $".{prefixCls}";
                if (getDefaultToken != null)
                {
                    var defaultToken = getDefaultToken();
                    componentToken.Merge(defaultToken);
                }

                // Generate style for all a tags in antd component.
                UseStyleRegister(new StyleInfo()
                {
                    HashId = hash.HashId,
                    TokenKey = hash.TokenKey,
                    Path = new[] { "Shared", componentToken.AntCls },
                    StyleFn = () => new CSSObject { ["&"] = GlobalStyle.GenLinkStyle(token) },
                });

                // Generate style for icons
                UseStyleRegister(new StyleInfo()
                {
                    HashId = hash.HashId,
                    TokenKey = hash.TokenKey,
                    Path = new[] { "ant-design-icons", componentToken.IconCls },
                    StyleFn = () => new CSSObject
                    {
                        [$".{componentToken.IconCls}"] = new CSSObject
                        {
                            ["..."] = GlobalStyle.ResetIcon(),
                            [$".{componentToken.IconCls} .{componentToken.IconCls}-icon"] = new CSSObject
                            {
                                Display = "block"
                            }
                        }
                    },
                });

                // Generate current component style
                UseStyleRegister(new StyleInfo()
                {
                    HashId = hash.HashId,
                    TokenKey = hash.TokenKey,
                    Path = new[] { concatComponent, componentToken.PrefixCls, componentToken.IconCls },
                    StyleFn = () => styleFn(componentToken),
                });

                return hash.HashId;
            };
        }
    }
}
