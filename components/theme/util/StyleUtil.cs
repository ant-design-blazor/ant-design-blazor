// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using CssInCSharp;
using Microsoft.AspNetCore.Components;
using static CssInCSharp.StyleHelper;

namespace AntDesign
{
    internal class StyleUtil
    {
        internal static RenderFragment GenComponentStyleHook(string componentName, TokenWithCommonCls token, Func<CSSInterpolation> func)
        {
            return GenComponentStyleHook(new[] { componentName, componentName }, token, func);
        }

        /*
         * 注：
         * 样式渲染一定只能传样式渲染的Func，不要传CSSObject对象。
         * CSSObject构建会消耗内存，而Func只有一个引用外加闭包参数，内存开销少几乎无性能损耗。
         * 只有当缓存未命中，渲染组件才会调用Func去创建CSSObject对象并编译生成样式内容。
         */
        internal static RenderFragment GenComponentStyleHook(string[] componentNames, TokenWithCommonCls token, Func<CSSInterpolation> func)
        {
            var concatComponent = string.Join("-", componentNames);
            var hash = token.GetTokenHash();
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

                // Generate current component style
                new ()
                {
                    HashId = hash.HashId,
                    TokenKey = hash.TokenKey,
                    Path = new[] { concatComponent, token.PrefixCls, token.IconCls },
                    StyleFn = func,
                }
            });
        }
    }
}
