// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CssInCSharp;

namespace AntDesign
{
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

        public static CSSObject InitSlideMotion(TokenWithCommonCls token, string motionName)
        {
            return new CSSObject();
        }
    }
}
