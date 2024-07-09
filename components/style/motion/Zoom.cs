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

        public static Keyframe ZoomUpIn = new Keyframe("antZoomUpIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(0.8)",
                TransformOrigin = "50% 0%",
                Opacity = 0,
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(1)",
                TransformOrigin = "50% 0%",
            },
        };

        public static Keyframe ZoomUpOut = new Keyframe("antZoomUpOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(1)",
                TransformOrigin = "50% 0%",
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(0.8)",
                TransformOrigin = "50% 0%",
                Opacity = 0,
            },
        };

        public static Keyframe ZoomLeftIn = new Keyframe("antZoomLeftIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(0.8)",
                TransformOrigin = "0% 50%",
                Opacity = 0,
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(1)",
                TransformOrigin = "0% 50%",
            },
        };

        public static Keyframe ZoomLeftOut = new Keyframe("antZoomLeftOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(1)",
                TransformOrigin = "0% 50%",
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(0.8)",
                TransformOrigin = "0% 50%",
                Opacity = 0,
            },
        };

        public static Keyframe ZoomRightIn = new Keyframe("antZoomRightIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(0.8)",
                TransformOrigin = "100% 50%",
                Opacity = 0,
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(1)",
                TransformOrigin = "100% 50%",
            },
        };

        public static Keyframe ZoomRightOut = new Keyframe("antZoomRightOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(1)",
                TransformOrigin = "100% 50%",
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(0.8)",
                TransformOrigin = "100% 50%",
                Opacity = 0,
            },
        };

        public static Keyframe ZoomDownIn = new Keyframe("antZoomDownIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(0.8)",
                TransformOrigin = "50% 100%",
                Opacity = 0,
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(1)",
                TransformOrigin = "50% 100%",
            },
        };

        public static Keyframe ZoomDownOut = new Keyframe("antZoomDownOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scale(1)",
                TransformOrigin = "50% 100%",
            },
            ["100%"] = new CSSObject
            {
                Transform = "scale(0.8)",
                TransformOrigin = "50% 100%",
                Opacity = 0,
            },
        };

        public static Dictionary<string, ZoomMotion> ZoomMotion = new Dictionary<string, ZoomMotion>
        {
            { "zoom", new ZoomMotion { InKeyframes = ZoomIn, OutKeyframes = ZoomOut } },
            { "zoom-big", new ZoomMotion { InKeyframes = ZoomBigIn, OutKeyframes = ZoomBigOut } },
            { "zoom-big-fast", new ZoomMotion { InKeyframes = ZoomBigIn, OutKeyframes = ZoomBigOut } },
            { "zoom-left", new ZoomMotion { InKeyframes = ZoomLeftIn, OutKeyframes = ZoomLeftOut } },
            { "zoom-right", new ZoomMotion { InKeyframes = ZoomRightIn, OutKeyframes = ZoomRightOut } },
            { "zoom-up", new ZoomMotion { InKeyframes = ZoomUpIn, OutKeyframes = ZoomUpOut } },
            { "zoom-down", new ZoomMotion { InKeyframes = ZoomDownIn, OutKeyframes = ZoomDownOut } },
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
