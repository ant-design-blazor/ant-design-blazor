// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using CssInCSharp;

namespace AntDesign
{
    internal class CollapseMotion
    {
        public static CSSObject GenCollapseMotion(TokenWithCommonCls token)
        {
            return new CSSObject()
            {
                [token.ComponentCls] = new CSSObject()
                {
                    [$"{token.AntCls}-motion-collapse-legacy"] = new CSSObject()
                    {
                        Overflow = "hidden",
                        ["&-active"] = new CSSObject()
                        {
                            Transition = $"height {token.MotionDurationMid} {token.MotionEaseInOut},opacity {token.MotionDurationMid} {token.MotionEaseInOut} !important"
                        }
                    },
                    [$"{token.AntCls}-motion-collapse"] = new CSSObject()
                    {
                        Overflow = "hidden",
                        Transition = $"height {token.MotionDurationMid} ${token.MotionEaseInOut},opacity {token.MotionDurationMid} {token.MotionEaseInOut} !important"
                    }
                }
            };
        }
    }
}
