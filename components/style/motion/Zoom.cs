// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using CssInCSharp;
using static AntDesign.Motion;
using static CssInCSharp.Property;

namespace AntDesign
{
    public class ZoomMotion
    {
        public Keyframe InKeyframes { get; set; }
        public Keyframe OutKeyframes { get; set; }
    }

    public class Zoom
    {
        public static Keyframe ZoomIn = new Keyframe("antZoomIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(0.2)",
                Opacity = 0,
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(1)",
                Opacity = 1,
            },
        };

        public static Keyframe ZoomOut = new Keyframe("antZoomOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(1)",
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(0.2)",
                Opacity = 0,
            },
        };

        public static Keyframe ZoomBigIn = new Keyframe("antZoomBigIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(0.8)",
                Opacity = 0,
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(1)",
                Opacity = 1,
            },
        };

        public static Keyframe ZoomBigOut = new Keyframe("antZoomBigOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(1)",
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(0.8)",
                Opacity = 0,
            },
        };

        public static Dictionary<string, ZoomMotion> ZoomMotion = new Dictionary<string, ZoomMotion>
        {
            { "zoom", new ZoomMotion { InKeyframes = ZoomIn, OutKeyframes = ZoomOut } },
            { "zoom-big", new ZoomMotion { InKeyframes = ZoomBigIn, OutKeyframes = ZoomBigOut } },
            { "zoom-big-fast", new ZoomMotion { InKeyframes = ZoomBigIn, OutKeyframes = ZoomBigOut } },
        };

        public static CSSInterpolation InitZoomMotion(TokenWithCommonCls token, string motionName)
        {
            var antCls = token.AntCls;
            var motionCls = $"{antCls}-{motionName}";
            var inKeyframes = ZoomMotion[motionName].InKeyframes;
            var outKeyframes = ZoomMotion[motionName].OutKeyframes;
            return new CSSInterpolation[] {
                InitMotion(motionCls, inKeyframes, outKeyframes, motionName == "zoom-big-fast" ? token.MotionDurationFast : token.MotionDurationMid),
                new CSSObject
                {
                    [$"{motionCls}-enter,{motionCls}-appear"] = new CSSObject
                    {
                        Transform = "scale(0)",
                        Opacity = 0,
                        AnimationTimingFunction = token.MotionEaseOutCirc,
                        ["&-prepare"] = new CSSObject
                        {
                            Transform = "none",
                        },
                    },

                    [$"{motionCls}-leave"] = new CSSObject
                    {
                        AnimationTimingFunction = token.MotionEaseInOutCirc,
                    }
                },
            };
        }
    }
}
