using Ardalis.SmartEnum;

namespace AntBlazor
{
    public sealed class PlacementType : SmartEnum<PlacementType>
    {
        public static readonly PlacementType Left = new PlacementType("left", "down", 0);
        public static readonly PlacementType Right = new PlacementType("right", "down", 1);

        public static readonly PlacementType BottomLeft = new PlacementType("bottomLeft", "down", 2);
        public static readonly PlacementType BottomCenter = new PlacementType("bottomCenter", "down", 3);
        public static readonly PlacementType BottomRight = new PlacementType("bottomRight", "down", 4);

        public static readonly PlacementType TopLeft = new PlacementType("topLeft", "up", 5);
        public static readonly PlacementType TopCenter = new PlacementType("topCenter", "up", 6);
        public static readonly PlacementType TopRight = new PlacementType("topRight", "up", 7);

        public string SlideName { get; private set; }

        public PlacementType(string name, string slideName, int value) : base(name, value)
        {
            SlideName = slideName;
        }
    }
}
