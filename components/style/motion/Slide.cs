// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using CssInCSharp;
using static AntDesign.Motion;

namespace AntDesign
{
    internal class SlideMotion
    {
        public Keyframe InKeyframes { get; set; }
        public Keyframe OutKeyframes { get; set; }
    }

    internal class Slide
    {
        public static Keyframe SlideUpIn = new Keyframe("antSlideUpIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scaleY(0.8)",
                TransformOrigin = "0% 0%",
                Opacity = 0
            },
            ["100%"] = new CSSObject
            {
                Transform = "scaleY(1)",
                TransformOrigin = "0% 0%",
                Opacity = 1
            },
        };

        public static Keyframe SlideUpOut = new Keyframe("antSlideUpOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scaleY(1)",
                TransformOrigin = "0% 0%",
                Opacity = 1
            },
            ["100%"] = new CSSObject
            {
                Transform = "scaleY(0.8)",
                TransformOrigin = "0% 0%",
                Opacity = 0
            },
        };

        public static Keyframe SlideDownIn = new Keyframe("antSlideDownIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scaleY(0.8)",
                TransformOrigin = "100% 100%",
                Opacity = 0
            },
            ["100%"] = new CSSObject
            {
                Transform = "scaleY(1)",
                TransformOrigin = "100% 100%",
                Opacity = 1
            },
        };

        public static Keyframe SlideDownOut = new Keyframe("antSlideDownOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scaleY(1)",
                TransformOrigin = "100% 100%",
                Opacity = 1
            },
            ["100%"] = new CSSObject
            {
                Transform = "scaleY(0.8)",
                TransformOrigin = "100% 100%",
                Opacity = 0
            },
        };

        public static Keyframe SlideLeftIn = new Keyframe("antSlideLeftIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scaleX(0.8)",
                TransformOrigin = "0% 0%",
                Opacity = 0
            },
            ["100%"] = new CSSObject
            {
                Transform = "scaleX(1)",
                TransformOrigin = "0% 0%",
                Opacity = 1
            },
        };

        public static Keyframe SlideLeftOut = new Keyframe("antSlideLeftOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scaleX(1)",
                TransformOrigin = "0% 0%",
                Opacity = 1
            },
            ["100%"] = new CSSObject
            {
                Transform = "scaleX(0.8)",
                TransformOrigin = "0% 0%",
                Opacity = 0
            },
        };

        public static Keyframe SlideRightIn = new Keyframe("antSlideRightIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scaleX(0.8)",
                TransformOrigin = "100% 0%",
                Opacity = 0
            },
            ["100%"] = new CSSObject
            {
                Transform = "scaleX(1)",
                TransformOrigin = "100% 0%",
                Opacity = 1
            },
        };

        public static Keyframe SlideRightOut = new Keyframe("antSlideRightOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "scaleX(1)",
                TransformOrigin = "100% 0%",
                Opacity = 1
            },
            ["100%"] = new CSSObject
            {
                Transform = "scaleX(0.8)",
                TransformOrigin = "100% 0%",
                Opacity = 0
            },
        };

        private static Dictionary<string, SlideMotion> _slideMotion = new Dictionary<string, SlideMotion>()
        {
            { "slide-up", new SlideMotion() { InKeyframes = SlideUpIn, OutKeyframes = SlideUpOut } },
            { "slide-down", new SlideMotion() { InKeyframes = SlideDownIn, OutKeyframes = SlideDownOut } },
            { "slide-left", new SlideMotion() { InKeyframes = SlideLeftIn, OutKeyframes = SlideLeftOut } },
            { "slide-right", new SlideMotion() { InKeyframes = SlideRightIn, OutKeyframes = SlideRightOut } },
        };

        public static CSSInterpolation InitSlideMotion(TokenWithCommonCls token, string motionName)
        {
            var antCls = token.AntCls;
            var motionCls = $"{antCls}-{motionName}";
            var inKeyframes = _slideMotion[motionName].InKeyframes;
            var outKeyframes = _slideMotion[motionName].OutKeyframes;
            return new CSSInterpolation[]
            {
                InitMotion(motionCls, inKeyframes, outKeyframes, token.MotionDurationMid),
                new CSSObject()
                {
                    [$"{motionCls}-enter,{motionCls}-appear"] = new CSSObject()
                    {
                        Transform = "scale(0)",
                        TransformOrigin = "0% 0%",
                        Opacity = 0,
                        AnimationTimingFunction = token.MotionEaseOutQuint,
                        ["&-prepare"] = new CSSObject()
                        {
                            Transform = "scale(1)",
                        }
                    },
                    ["&-leave"] = new CSSObject()
                    {
                        AnimationTimingFunction = token.MotionEaseInQuint,
                    }
                }
            };
        }
    }
}
