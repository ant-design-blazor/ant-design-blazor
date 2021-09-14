namespace AntDesign
{
    public enum Placement
    {
        TopLeft,
        TopCenter,
        Top,
        TopRight,
        Left,
        LeftTop,
        LeftBottom,
        Right,
        RightTop,
        RightBottom,
        BottomLeft,
        BottomCenter,
        Bottom,
        BottomRight
    }

    public sealed class PlacementType : EnumValue<PlacementType>
    {
        public static readonly PlacementType TopLeft = new PlacementType("topLeft", "down", "33% 100%", 0, Placement.TopLeft);
        public static readonly PlacementType TopCenter = new PlacementType("topCenter", "down", "50% 100%", 1, Placement.TopCenter);
        public static readonly PlacementType Top = new PlacementType("top", "down", "50% 100%", 1, Placement.Top);
        public static readonly PlacementType TopRight = new PlacementType("topRight", "down", "66% 100%", 2, Placement.TopRight);

        public static readonly PlacementType Left = new PlacementType("left", "up", "100% 50%%", 3, Placement.Left);
        public static readonly PlacementType LeftTop = new PlacementType("leftTop", "down", "100% 33%", 4, Placement.LeftTop);
        public static readonly PlacementType LeftBottom = new PlacementType("leftBottom", "up", "100% 66%", 5, Placement.LeftBottom);

        public static readonly PlacementType Right = new PlacementType("right", "up", "0 50%", 6, Placement.Right);
        public static readonly PlacementType RightTop = new PlacementType("rightTop", "down", "0 33%", 7, Placement.RightTop);
        public static readonly PlacementType RightBottom = new PlacementType("rightBottom", "up", "0 66%", 8, Placement.RightBottom);

        public static readonly PlacementType BottomLeft = new PlacementType("bottomLeft", "up", "33% 0", 9, Placement.BottomLeft);
        public static readonly PlacementType BottomCenter = new PlacementType("bottomCenter", "up", "50% 0", 10, Placement.BottomCenter);
        public static readonly PlacementType Bottom = new PlacementType("bottom", "up", "50% 0", 10, Placement.Bottom);
        public static readonly PlacementType BottomRight = new PlacementType("bottomRight", "up", "66% 0", 11, Placement.BottomRight);

        public static PlacementType Create(Placement placement) => placement switch
        {
            Placement.TopLeft => PlacementType.TopLeft,
            Placement.TopCenter => PlacementType.TopCenter,
            Placement.Top => PlacementType.Top,
            Placement.TopRight => PlacementType.TopRight,
            Placement.Left => PlacementType.Left,
            Placement.LeftTop => PlacementType.LeftTop,
            Placement.LeftBottom => PlacementType.LeftBottom,
            Placement.Right => PlacementType.Right,
            Placement.RightTop => PlacementType.RightTop,
            Placement.RightBottom => PlacementType.RightBottom,
            Placement.BottomLeft => PlacementType.BottomLeft,
            Placement.BottomCenter => PlacementType.BottomCenter,
            Placement.Bottom => PlacementType.Bottom,
            Placement.BottomRight => PlacementType.BottomRight,
            _ => PlacementType.BottomLeft
        };

        public string SlideName { get; private set; }
        public string TranformOrigin { get; private set; }

        public Placement Placement { get; private set; } = Placement.BottomLeft;

        private PlacementType(string name, string slideName, string transformOrigin, int value, Placement placement) : base(name, value)
        {
            SlideName = slideName;
            TranformOrigin = transformOrigin;
            Placement = placement;
        }

        internal PlacementType GetReverseType()
        {
            if (this == TopLeft) return BottomLeft;
            if (this == TopCenter) return BottomCenter;
            if (this == Top) return Bottom;
            if (this == TopRight) return BottomRight;

            if (this == Left) return Right;
            if (this == LeftTop) return RightTop;
            if (this == LeftBottom) return RightBottom;

            if (this == Right) return Left;
            if (this == RightTop) return LeftTop;
            if (this == RightBottom) return LeftBottom;

            if (this == BottomLeft) return TopLeft;
            if (this == BottomCenter) return TopCenter;
            if (this == Bottom) return Top;
            if (this == BottomRight) return TopRight;

            return this;
        }

        internal PlacementDirection GetDirection()
        {
            if (this.IsIn(TopLeft, TopCenter, Top, TopRight))
            {
                return PlacementDirection.Top;
            }

            if (this.IsIn(Left, LeftTop, LeftBottom))
            {
                return PlacementDirection.Left;
            }

            if (this.IsIn(Right, RightTop, RightBottom))
            {
                return PlacementDirection.Right;
            }

            if (this.IsIn(BottomLeft, BottomCenter, Bottom, BottomRight))
            {
                return PlacementDirection.Bottom;
            }

            return PlacementDirection.Bottom;
        }

        internal PlacementType GetRTLPlacement()
        {
            if (this == TopLeft) return TopRight;
            if (this == TopRight) return TopLeft;

            if (this == Left) return Right;
            if (this == LeftTop) return RightTop;
            if (this == LeftBottom) return RightBottom;

            if (this == Right) return Left;
            if (this == RightTop) return LeftTop;
            if (this == RightBottom) return LeftBottom;

            if (this == BottomLeft) return BottomRight;
            if (this == BottomRight) return BottomLeft;

            return this;
        }
    }
}
