namespace AntBlazor
{
    public class AntDropdownPlacement
    {
        public static readonly AntDropdownPlacement BottomLeft = new AntDropdownPlacement("bottomLeft", "down");
        public static readonly AntDropdownPlacement BottomCenter = new AntDropdownPlacement("bottomCenter", "down");
        public static readonly AntDropdownPlacement BottomRight = new AntDropdownPlacement("bottomRight", "down");

        public static readonly AntDropdownPlacement TopLeft = new AntDropdownPlacement("topLeft", "up");
        public static readonly AntDropdownPlacement TopCenter = new AntDropdownPlacement("topCenter", "up");
        public static readonly AntDropdownPlacement TopRight = new AntDropdownPlacement("topRight", "up");

        public string Name { get; private set; }
        public string SlideName { get; private set; }

        public AntDropdownPlacement(string name, string slideName)
        {
            Name = name;
            SlideName = slideName;
        }
    }
}
