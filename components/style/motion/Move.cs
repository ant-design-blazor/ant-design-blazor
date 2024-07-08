// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using CssInCSharp;
using static AntDesign.Motion;

namespace AntDesign
{
    internal class MoveMotion
    {
        public Keyframe InKeyframes { get; set; }
        public Keyframe OutKeyframes { get; set; }
    }

    internal class Move
    {
        public static Keyframe MoveDownIn = new Keyframe("antMoveDownIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "translate3d(0, 100%, 0)",
                TransformOrigin = "0 0",
                Opacity = 0,
            },
            ["100%"] = new CSSObject
            {
                Transform = "translate3d(0, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 1,
            },
        };

        public static Keyframe MoveDownOut = new Keyframe("antMoveDownOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "translate3d(0, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 1,
            },
            ["100%"] = new CSSObject
            {
                Transform = "translate3d(0, 100%, 0)",
                TransformOrigin = "0 0",
                Opacity = 0,
            },
        };

        public static Keyframe MoveLeftIn = new Keyframe("antMoveLeftIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "translate3d(-100%, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 0,
            },
            ["100%"] = new CSSObject
            {
                Transform = "translate3d(0, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 1,
            },
        };

        public static Keyframe MoveLeftOut = new Keyframe("antMoveLeftOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "translate3d(0, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 1,
            },
            ["100%"] = new CSSObject
            {
                Transform = "translate3d(-100%, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 0,
            },
        };

        public static Keyframe MoveRightIn = new Keyframe("antMoveRightIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "translate3d(100%, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 0,
            },
            ["100%"] = new CSSObject
            {
                Transform = "translate3d(0, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 1,
            },
        };

        public static Keyframe MoveRightOut = new Keyframe("antMoveRightOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "translate3d(0, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 1,
            },
            ["100%"] = new CSSObject
            {
                Transform = "translate3d(100, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 0,
            },
        };

        public static Keyframe MoveUpIn = new Keyframe("antMoveUpIn")
        {
            ["0%"] = new CSSObject
            {
                Transform = "translate3d(0, -100%, 0)",
                TransformOrigin = "0 0",
                Opacity = 0,
            },
            ["100%"] = new CSSObject
            {
                Transform = "translate3d(0, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 1,
            },
        };

        public static Keyframe MoveUpOut = new Keyframe("antMoveUpOut")
        {
            ["0%"] = new CSSObject
            {
                Transform = "translate3d(0, 0, 0)",
                TransformOrigin = "0 0",
                Opacity = 1,
            },
            ["100%"] = new CSSObject
            {
                Transform = "translate3d(0, -100%, 0)",
                TransformOrigin = "0 0",
                Opacity = 0,
            },
        };

        private static Dictionary<string, MoveMotion> _moveMotion = new Dictionary<string, MoveMotion>()
        {
            { "move-up", new MoveMotion{ InKeyframes = MoveUpIn, OutKeyframes = MoveUpOut} },
            { "move-down", new MoveMotion{ InKeyframes = MoveDownIn, OutKeyframes = MoveDownOut} },
            { "move-left", new MoveMotion{ InKeyframes = MoveLeftIn, OutKeyframes = MoveLeftOut} },
            { "move-right", new MoveMotion{ InKeyframes = MoveRightIn, OutKeyframes = MoveRightOut} },
        };

        public static CSSInterpolation InitMoveMotion(TokenWithCommonCls token, string motionName)
        {
            var antCls = token.AntCls;
            var motionCls = $"{antCls}-{motionName}";
            var inKeyframes = _moveMotion[motionName].InKeyframes;
            var outKeyframes = _moveMotion[motionName].OutKeyframes;
            return new CSSInterpolation[]
            {
                InitMotion(motionCls, inKeyframes, outKeyframes, token.MotionDurationMid),
                new CSSObject()
                {
                    [$"{motionCls}-enter,{motionCls}-appear"] = new CSSObject()
                    {
                        Opacity = 0,
                        AnimationTimingFunction = token.MotionEaseOutCirc
                    },
                    [$"{motionCls}-leave"] = new CSSObject()
                    {
                        AnimationTimingFunction = token.MotionEaseInOutCirc
                    }
                }
            };
        }
    }
}
