// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public sealed class ProgressSize : EnumValue<ProgressSize>
    {
        public static readonly ProgressSize Small = new ProgressSize(nameof(Small), 6);
        public static readonly ProgressSize Default = new ProgressSize(nameof(Default), 8);

        private ProgressSize(string name, int value) : base(name.ToLowerInvariant(), value)
        {
        }
    }

    public sealed class ProgressType : EnumValue<ProgressType>
    {
        public static readonly ProgressType Line = new ProgressType(nameof(Line), 1);
        public static readonly ProgressType Circle = new ProgressType(nameof(Circle), 2);
        public static readonly ProgressType Dashboard = new ProgressType(nameof(Dashboard), 3);

        private ProgressType(string name, int value) : base(name.ToLowerInvariant(), value)
        {
        }
    }

    public sealed class ProgressStatus : EnumValue<ProgressStatus>
    {
        public static readonly ProgressStatus Success = new ProgressStatus(nameof(Success), 1);
        public static readonly ProgressStatus Exception = new ProgressStatus(nameof(Exception), 2);
        public static readonly ProgressStatus Normal = new ProgressStatus(nameof(Normal), 3);
        public static readonly ProgressStatus Active = new ProgressStatus(nameof(Active), 4);

        private ProgressStatus(string name, int value) : base(name.ToLowerInvariant(), value)
        {
        }
    }

    public sealed class ProgressStrokeLinecap : EnumValue<ProgressStrokeLinecap>
    {
        public static readonly ProgressStrokeLinecap Round = new ProgressStrokeLinecap(nameof(Round), 1);
        public static readonly ProgressStrokeLinecap Square = new ProgressStrokeLinecap(nameof(Square), 2);

        private ProgressStrokeLinecap(string name, int value) : base(name.ToLowerInvariant(), value)
        {
        }
    }

    public sealed class ProgressGapPosition : EnumValue<ProgressGapPosition>
    {
        public static readonly ProgressGapPosition Top = new ProgressGapPosition(nameof(Top), 1);
        public static readonly ProgressGapPosition Bottom = new ProgressGapPosition(nameof(Bottom), 2);
        public static readonly ProgressGapPosition Left = new ProgressGapPosition(nameof(Left), 3);
        public static readonly ProgressGapPosition Right = new ProgressGapPosition(nameof(Right), 4);

        private ProgressGapPosition(string name, int value) : base(name.ToLowerInvariant(), value)
        {
        }
    }
}
