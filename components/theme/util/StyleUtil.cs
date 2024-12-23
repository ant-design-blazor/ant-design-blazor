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

    public class StyleUtil
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
                var (hash, token) = UseTheme();
                var rootPrefixCls = GetPrefixCls();
                var iconPrefixCls = DefaultIconPrefixCls;
                var mergedToken = new T();
                var componentToken = new TokenWithCommonCls()
                {
                    ComponentCls = $".{prefixCls}",
                    PrefixCls = prefixCls,
                    IconCls = $".{iconPrefixCls}",
                    AntCls = $".{rootPrefixCls}",
                    ["rootPrefixCls"] = rootPrefixCls,
                };
                mergedToken.Merge(token, componentToken);
                if (getDefaultToken != null)
                {
                    mergedToken.Merge(getDefaultToken(token));
                }

                // COMPONENT, HEAD
                var renderingMode = Environment.GetEnvironmentVariable("RENDERING_MODE");
                if (renderingMode == "COMPONENT")
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

        internal static UseComponentStyleResult GenSubStyleComponent<T>(
            string[] componentName,
            Func<T, CSSInterpolation> styleFn,
            Func<GlobalToken, T> getDefaultToken,
            GenOptions options) where T : IToken, new()
        {
            options.ResetStyle = false;
            return GenComponentStyleHook<T>(componentName, styleFn, getDefaultToken, options);
        }

        internal static string GetPrefixCls(string suffixCls = null, string customizePrefixCls = null)
        {
            if (!string.IsNullOrEmpty(customizePrefixCls))
            {
                return customizePrefixCls;
            }

            return !string.IsNullOrEmpty(suffixCls) ? $"ant-{suffixCls}" : "ant";
        }

        public static (string, GlobalToken) UseToken()
        {
            var (hash, token) = UseTheme();
            return (hash.HashId, token);
        }

        public static (TokenHash, GlobalToken) UseTheme()
        {
            /*
             * 这里依据配置调用不同主题
             * Themes.Default.Derivative();
             * Themes.Dark.Derivative();
             * Themes.Compact.Derivative();
             */
            var token = Themes.Default.Derivative();
            var hash = token.GetTokenHash();
            return (hash, token);
        }
    }
}
