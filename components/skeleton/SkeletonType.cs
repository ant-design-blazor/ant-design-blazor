using System;
using System.Collections.Generic;
using System.Text;
using OneOf;

namespace AntDesign
{
    public sealed class SkeletonElementSize : AntSizeLDSType
    {
    }

    public static class SkeletonAvatarShape
    {
        public const string Square = "square";
        public const string Circle = "circle";
    }

    public static class SkeletonButtonShape
    {
        public const string Default = "default";
        public const string Circle = "circle";
        public const string Round = "round";

    }

    public static class SkeletonElementType
    {
        public const string Button = "button";
        public const string Input = "input";
        public const string Avatar = "avatar";
    }
}
