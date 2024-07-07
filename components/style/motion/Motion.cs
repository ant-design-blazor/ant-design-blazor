// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using CssInCSharp;

namespace AntDesign
{
    internal class Motion
    {
        public static CSSObject InitMotion(string motionCls, Keyframe inKeyframes, Keyframe outKeyframes, string duration, bool sameLevel = false)
        {
            var sameLevelPrefix = sameLevel ? "&" : "";
            return new CSSObject
            {
                [$"{sameLevelPrefix}{motionCls}-enter,{sameLevelPrefix}{motionCls}-appear"] = new CSSObject
                {
                    ["..."] = InitMotionCommon(duration),
                    AnimationPlayState = "paused",
                },

                [$"{sameLevelPrefix}{motionCls}-leave"] = new CSSObject
                {
                    ["..."] = InitMotionCommon(duration),
                    AnimationPlayState = "paused",
                },

                [$"{sameLevelPrefix}{motionCls}-enter{motionCls}-enter-active,{sameLevelPrefix}{motionCls}-appear{motionCls}-appear-active"] = new CSSObject
                {
                    AnimationName = inKeyframes,
                    AnimationPlayState = "running",
                },

                [$"{sameLevelPrefix}{motionCls}-leave{motionCls}-leave-active"] = new CSSObject
                {
                    AnimationName = outKeyframes,
                    AnimationPlayState = "running",
                    PointerEvents = "none"
                },
            };
        }

        public static CSSObject InitMotionCommon(string duration)
        {
            return new CSSObject
            {
                AnimationDuration = duration,
                AnimationFillMode = "both",
            };
        }
    }
}
