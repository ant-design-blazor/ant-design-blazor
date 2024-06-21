// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using CssInCSharp;
using Microsoft.AspNetCore.Components;
using static CssInCSharp.StyleHelper;

namespace AntDesign
{
    public delegate (RenderFragment, string) UseComponentStyleResult(string prefixCls);

    internal class GenOptions
    {
        public bool ResetStyle { get; set; }
        public List<(string, string)> DeprecatedTokens { get; set; }
    }

    internal class StyleUtil
    {
        internal static UseComponentStyleResult GenComponentStyleHook(
            string componentName,
            Func<TokenWithCommonCls, CSSInterpolation> styleFn)
        {
            return GenComponentStyleHook<TokenWithCommonCls>(componentName, styleFn);
        }

        internal static UseComponentStyleResult GenComponentStyleHook<T>(
            string componentName,
            Func<T, CSSInterpolation> styleFn,
            Func<GlobalToken, T> getDefaultToken = null,
            GenOptions options = null) where T : IToken, new()

        {
            return GenComponentStyleHook(new[] { componentName, componentName }, styleFn, getDefaultToken);
        }

        internal static UseComponentStyleResult GenComponentStyleHook<T>(
            string[] componentNames,
            Func<T, CSSInterpolation> styleFn,
            Func<GlobalToken, T> getDefaultToken = null) where T : IToken, new()
        {
            var concatComponent = string.Join("-", componentNames);
            return (prefixCls) =>
            {
                var token = Seed.DefaultSeedToken;
                var hash = token.GetTokenHash();
                var mergedToken = getDefaultToken != null ? getDefaultToken(token) : new T();
                var componentToken = new TokenWithCommonCls() { PrefixCls = prefixCls, ComponentCls = $".{prefixCls}" };
                mergedToken.Merge(token, componentToken);

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
                var render = UseStyleRegister(new StyleInfo()
                {
                    HashId = hash.HashId,
                    TokenKey = hash.TokenKey,
                    Path = new[] { concatComponent, componentToken.PrefixCls, componentToken.IconCls },
                    StyleFn = () => styleFn(mergedToken),
                });

                return (render, hash.HashId);
            };
        }
    }
}
