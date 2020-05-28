using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SmartEnum;

namespace AntDesign
{
    public sealed class ProgressSize : SmartEnum<ProgressSize>
    {
        public static readonly ProgressSize Small = new ProgressSize(nameof(Small), 1);
        public static readonly ProgressSize Default = new ProgressSize(nameof(Default), 2);

        private ProgressSize(string name, int value) : base(name.ToLowerInvariant(), value) { }
    }

    public sealed class ProgressType : SmartEnum<ProgressType>
    {
        public static readonly ProgressType Line = new ProgressType(nameof(Line), 1);
        public static readonly ProgressType Circle = new ProgressType(nameof(Circle), 2);
        public static readonly ProgressType Dashboard = new ProgressType(nameof(Dashboard), 3);

        private ProgressType(string name, int value) : base(name.ToLowerInvariant(), value) { }
    }

    public sealed class ProgressStatus : SmartEnum<ProgressStatus>
    {
        public static readonly ProgressStatus Success = new ProgressStatus(nameof(Success), 1);
        public static readonly ProgressStatus Exception = new ProgressStatus(nameof(Exception), 2);
        public static readonly ProgressStatus Normal = new ProgressStatus(nameof(Normal), 3);
        public static readonly ProgressStatus Active = new ProgressStatus(nameof(Active), 4);

        private ProgressStatus(string name, int value) : base(name.ToLowerInvariant(), value) { }
    }

    public sealed class ProgressStrokeLinecap : SmartEnum<ProgressStrokeLinecap>
    {
        public static readonly ProgressStrokeLinecap Round = new ProgressStrokeLinecap(nameof(Round), 1);
        public static readonly ProgressStrokeLinecap Square = new ProgressStrokeLinecap(nameof(Square), 2);

        private ProgressStrokeLinecap(string name, int value) : base(name.ToLowerInvariant(), value) { }
    }

    public sealed class ProgressGapPosition : SmartEnum<ProgressGapPosition>
    {
        public static readonly ProgressGapPosition Top = new ProgressGapPosition(nameof(Top), 1);
        public static readonly ProgressGapPosition Bottom = new ProgressGapPosition(nameof(Bottom), 2);
        public static readonly ProgressGapPosition Left = new ProgressGapPosition(nameof(Left), 3);
        public static readonly ProgressGapPosition Right = new ProgressGapPosition(nameof(Right), 4);

        private ProgressGapPosition(string name, int value) : base(name.ToLowerInvariant(), value) { }
    }
}
