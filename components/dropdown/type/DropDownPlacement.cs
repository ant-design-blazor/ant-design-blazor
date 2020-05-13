namespace AntBlazor
{
    public class DropdownPlacement
    {
        public static readonly DropdownPlacement BottomLeft = new DropdownPlacement("bottomLeft", "down");
        public static readonly DropdownPlacement BottomCenter = new DropdownPlacement("bottomCenter", "down");
        public static readonly DropdownPlacement BottomRight = new DropdownPlacement("bottomRight", "down");

        public static readonly DropdownPlacement TopLeft = new DropdownPlacement("topLeft", "up");
        public static readonly DropdownPlacement TopCenter = new DropdownPlacement("topCenter", "up");
        public static readonly DropdownPlacement TopRight = new DropdownPlacement("topRight", "up");

        public string Name { get; private set; }
        public string SlideName { get; private set; }

        public DropdownPlacement(string name, string slideName)
        {
            Name = name;
            SlideName = slideName;
        }
    }
}
