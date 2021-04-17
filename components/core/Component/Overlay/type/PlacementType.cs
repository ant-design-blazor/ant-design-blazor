namespace AntDesign
{
    public sealed class PlacementType : EnumValue<PlacementType>
    {
        public static readonly PlacementType TopLeft = new PlacementType("topLeft", "down", "33% 100%", 0);
        public static readonly PlacementType TopCenter = new PlacementType("topCenter", "down", "50% 100%", 1);
        public static readonly PlacementType Top = new PlacementType("top", "down", "50% 100%", 1);
        public static readonly PlacementType TopRight = new PlacementType("topRight", "down", "66% 100%", 2);

        public static readonly PlacementType Left = new PlacementType("left", "up", "100% 50%%", 3);
        public static readonly PlacementType LeftTop = new PlacementType("leftTop", "down", "100% 33%", 4);
        public static readonly PlacementType LeftBottom = new PlacementType("leftBottom", "up", "100% 66%", 5);

        public static readonly PlacementType Right = new PlacementType("right", "up", "0 50%", 6);
        public static readonly PlacementType RightTop = new PlacementType("rightTop", "down", "0 33%", 7);
        public static readonly PlacementType RightBottom = new PlacementType("rightBottom", "up", "0 66%", 8);

        public static readonly PlacementType BottomLeft = new PlacementType("bottomLeft", "up", "33% 0", 9);
        public static readonly PlacementType BottomCenter = new PlacementType("bottomCenter", "up", "50% 0", 10);
        public static readonly PlacementType Bottom = new PlacementType("bottom", "up", "50% 0", 10);
        public static readonly PlacementType BottomRight = new PlacementType("bottomRight", "up", "66% 0", 11);

        public string SlideName { get; private set; }
        public string TranformOrigin { get; private set; }

        public PlacementType(string name, string slideName, string transformOrigin, int value) : base(name, value)
        {
            SlideName = slideName;
            TranformOrigin = transformOrigin;
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
