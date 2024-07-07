// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using CssInCSharp;
using static AntDesign.Motion;

namespace AntDesign
{
    internal class Fade
    {
        public static Keyframe FadeIn = new Keyframe("antFadeIn")
        {
            ["0%"] = new CSSObject
            {
                Opacity = 0,
            },
            ["100%"] = new CSSObject
            {
                Opacity = 1,
            }
        };

        public static Keyframe FadeOut = new Keyframe("antFadeOut")
        {
            ["0%"] = new CSSObject
            {
                Opacity = 1,
            },
            ["100%"] = new CSSObject
            {
                Opacity = 0,
            }
        };

        public static CSSInterpolation InitFadeMotion(TokenWithCommonCls token, bool sameLevel = false)
        {
            var antCls = token.AntCls;
            var motionCls = $"{antCls}-fade";
            var sameLevelPrefix = sameLevel ? "&" : "";
            return new CSSInterpolation[]
            {
                InitMotion(motionCls, FadeIn, FadeOut, token.MotionDurationMid, sameLevel),
                new CSSObject
                {
                    [$"{sameLevelPrefix}{motionCls}-enter,{sameLevelPrefix}{motionCls}-appear"] = new CSSObject
                    {
                        Opacity = 0,
                        AnimationTimingFunction = "linear",
                    },

                    [$"{sameLevelPrefix}{motionCls}-leave"] = new CSSObject
                    {
                        AnimationTimingFunction = "linear",
                    },

                }
            };
        }
    }
}
