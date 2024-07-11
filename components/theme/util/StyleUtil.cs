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

    public class GenOptions
    {
        public bool ResetStyle { get; set; } = true;
        public int Order { get; set; } = -999;
        public List<(string, string)> DeprecatedTokens { get; set; }
    }

    internal class StyleUtil
    {
        private const string DefaultIconPrefixCls = "anticon";

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
            return GenComponentStyleHook(new[] { componentName, componentName }, styleFn, getDefaultToken, options);
        }

        private static UseComponentStyleResult GenComponentStyleHook<T>(
            string[] componentNames,
            Func<T, CSSInterpolation> styleFn,
            Func<GlobalToken, T> getDefaultToken,
            GenOptions options) where T : IToken, new()
        {
            var concatComponent = string.Join("-", componentNames);
            options ??= new GenOptions();
            return (prefixCls) =>
            {
                var token = Seed.DefaultSeedToken;
                var hash = token.GetTokenHash();
                var rootPrefixCls = GetPrefixCls();
                var iconPrefixCls = DefaultIconPrefixCls;
                var mergedToken = getDefaultToken != null ? getDefaultToken(token) : new T();
                var componentToken = new TokenWithCommonCls()
                {
                    ComponentCls = $".{prefixCls}",
                    PrefixCls = prefixCls,
                    IconCls = $".{iconPrefixCls}",
                    AntCls = $".{rootPrefixCls}",
                    ["rootPrefixCls"] = rootPrefixCls,
                };
                mergedToken.Merge(token, componentToken);

                var renderMode = Environment.GetEnvironmentVariable("RENDER_MODE");
                if (renderMode == "testing")
                {
                    var styleFunc = (Func<CSSInterpolation>)(() => new CSSInterpolation[]
                    {
                        options.ResetStyle == false ? null : GlobalStyle.GenCommonStyle(token, prefixCls),
                        styleFn(mergedToken),
                    });
                    var testingRender = (RenderFragment)((builder) =>
                    {
                        builder.OpenComponent<Style>(0);
                        builder.AddAttribute(1, "HashId", hash.HashId);
                        builder.AddAttribute(2, "TokenKey", hash.TokenKey);
                        builder.AddAttribute(3, "Path", $"{concatComponent}|{prefixCls}|{iconPrefixCls}");
                        builder.AddAttribute(4, "StyleFn", styleFunc);
                        builder.CloseComponent();
                    });
                    return (testingRender, hash.HashId);
                }

                // Generate style for all a tags in antd component.
                UseStyleRegister(new StyleInfo()
                {
                    HashId = hash.HashId,
                    TokenKey = hash.TokenKey,
                    Path = new[] { "Shared", rootPrefixCls },
                    StyleFn = () => new CSSObject { ["&"] = GlobalStyle.GenLinkStyle(token) },
                });

                // Generate style for icons
                UseStyleRegister(new StyleInfo()
                {
                    HashId = hash.HashId,
                    TokenKey = hash.TokenKey,
                    Path = new[] { "ant-design-icons", iconPrefixCls },
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
                    Path = new[] { concatComponent, prefixCls, iconPrefixCls },
                    StyleFn = () => new CSSInterpolation[]
                    {
                        options.ResetStyle == false ? null : GlobalStyle.GenCommonStyle(token, prefixCls),
                        styleFn(mergedToken),
                    },
                });

                return (render, hash.HashId);
            };
        }

        internal static UseComponentStyleResult GenSubStyleComponent(string[] componentName)
        {
            return (prefixCls) =>
            {
                var render = UseStyleRegister(new StyleInfo()
                {
                    HashId = "",
                    TokenKey = "",
                });
                return (render, "");
            };
        }

        private static string GetPrefixCls(string suffixCls = null, string customizePrefixCls = null)
        {
            if (!string.IsNullOrEmpty(customizePrefixCls))
            {
                return customizePrefixCls;
            }

            return !string.IsNullOrEmpty(suffixCls) ? $"ant-{suffixCls}" : "ant";
        }
    }
}
