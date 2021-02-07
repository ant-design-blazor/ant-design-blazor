namespace AntDesign
{
    public sealed class PlacementType : EnumValue<PlacementType>
    {
        public static readonly PlacementType TopLeft = new PlacementType("topLeft", "up", "33% 100%", 0);
        public static readonly PlacementType TopCenter = new PlacementType("topCenter", "up", "50% 100%", 1);
        public static readonly PlacementType Top = new PlacementType("top", "up", "50% 100%", 1);
        public static readonly PlacementType TopRight = new PlacementType("topRight", "up", "66% 100%", 2);

        public static readonly PlacementType Left = new PlacementType("left", "down", "100% 50%%", 3);
        public static readonly PlacementType LeftTop = new PlacementType("leftTop", "up", "100% 33%", 4);
        public static readonly PlacementType LeftBottom = new PlacementType("leftBottom", "down", "100% 66%", 5);

        public static readonly PlacementType Right = new PlacementType("right", "down", "0 50%", 6);
        public static readonly PlacementType RightTop = new PlacementType("rightTop", "up", "0 33%", 7);
        public static readonly PlacementType RightBottom = new PlacementType("rightBottom", "down", "0 66%", 8);

        public static readonly PlacementType BottomLeft = new PlacementType("bottomLeft", "down", "33% 0", 9);
        public static readonly PlacementType BottomCenter = new PlacementType("bottomCenter", "down", "50% 0", 10);
        public static readonly PlacementType Bottom = new PlacementType("bottom", "down", "50% 0", 10);
        public static readonly PlacementType BottomRight = new PlacementType("bottomRight", "down", "66% 0", 11);

        public string SlideName { get; private set; }
        public string TranformOrigin { get; private set; }

        public PlacementType(string name, string slideName, string transformOrigin, int value) : base(name, value)
        {
            SlideName = slideName;
            TranformOrigin = transformOrigin;
        }

        public PlacementType GetReverseType()
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
    }
}
