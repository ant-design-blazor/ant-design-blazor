using Ardalis.SmartEnum;

namespace AntDesign
{
    public sealed class BreakpointType : SmartEnum<BreakpointType>
    {
        public static readonly BreakpointType Xs = new BreakpointType(nameof(Xs).ToLowerInvariant(), 1, 480);
        public static readonly BreakpointType Sm = new BreakpointType(nameof(Sm).ToLowerInvariant(), 2, 576);
        public static readonly BreakpointType Md = new BreakpointType(nameof(Md).ToLowerInvariant(), 3, 768);
        public static readonly BreakpointType Lg = new BreakpointType(nameof(Lg).ToLowerInvariant(), 4, 992);
        public static readonly BreakpointType Xl = new BreakpointType(nameof(Xl).ToLowerInvariant(), 5, 1200);
        public static readonly BreakpointType Xxl = new BreakpointType(nameof(Xxl).ToLowerInvariant(), 6, 1600);

        public int Width { get; private set; }

        private BreakpointType(string name, int value, int width) : base(name, value)
        {
            Width = width;
        }
    }
}
