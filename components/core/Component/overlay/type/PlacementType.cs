using Ardalis.SmartEnum;

namespace AntBlazor
{
    public sealed class PlacementType : SmartEnum<PlacementType>
    {
        public static readonly PlacementType TopLeft = new PlacementType("topLeft", "up", 0);
        public static readonly PlacementType TopCenter = new PlacementType("topCenter", "up", 1);
        public static readonly PlacementType TopRight = new PlacementType("topRight", "up", 2);

        public static readonly PlacementType Left = new PlacementType("left", "down", 3);
        public static readonly PlacementType LeftTop = new PlacementType("leftTop", "up", 4);
        public static readonly PlacementType LeftBottom = new PlacementType("leftBottom", "down",5);

        public static readonly PlacementType Right = new PlacementType("right", "down", 6);
        public static readonly PlacementType RightTop = new PlacementType("rightTop", "up", 7);
        public static readonly PlacementType RightBottom = new PlacementType("rightBottom", "down", 8);

        public static readonly PlacementType BottomLeft = new PlacementType("bottomLeft", "down", 9);
        public static readonly PlacementType BottomCenter = new PlacementType("bottomCenter", "down", 10);
        public static readonly PlacementType BottomRight = new PlacementType("bottomRight", "down", 11);



        public string SlideName { get; private set; }

        public PlacementType(string name, string slideName, int value) : base(name, value)
        {
            SlideName = slideName;
        }
    }
}
