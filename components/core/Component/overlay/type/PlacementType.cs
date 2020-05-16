namespace AntBlazor
{
    public class PlacementType
    {
        public static readonly PlacementType Left = new PlacementType("left", "down");
        public static readonly PlacementType Right = new PlacementType("right", "down");

        public static readonly PlacementType BottomLeft = new PlacementType("bottomLeft", "down");
        public static readonly PlacementType BottomCenter = new PlacementType("bottomCenter", "down");
        public static readonly PlacementType BottomRight = new PlacementType("bottomRight", "down");

        public static readonly PlacementType TopLeft = new PlacementType("topLeft", "up");
        public static readonly PlacementType TopCenter = new PlacementType("topCenter", "up");
        public static readonly PlacementType TopRight = new PlacementType("topRight", "up");

        public string Name { get; private set; }
        public string SlideName { get; private set; }

        public PlacementType(string name, string slideName)
        {
            Name = name;
            SlideName = slideName;
        }
    }
}
