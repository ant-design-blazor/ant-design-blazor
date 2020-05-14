namespace AntBlazor
{
    public class Placement
    {
        public static readonly Placement Left = new Placement("left", "down");
        public static readonly Placement Right = new Placement("right", "down");

        public static readonly Placement BottomLeft = new Placement("bottomLeft", "down");
        public static readonly Placement BottomCenter = new Placement("bottomCenter", "down");
        public static readonly Placement BottomRight = new Placement("bottomRight", "down");

        public static readonly Placement TopLeft = new Placement("topLeft", "up");
        public static readonly Placement TopCenter = new Placement("topCenter", "up");
        public static readonly Placement TopRight = new Placement("topRight", "up");

        public string Name { get; private set; }
        public string SlideName { get; private set; }

        public Placement(string name, string slideName)
        {
            Name = name;
            SlideName = slideName;
        }
    }
}
