namespace AntDesign
{
    public class SortDirection : EnumValue<SortDirection>
    {
        public static readonly SortDirection None;

        public static readonly SortDirection Ascending = new SortDirection("ascend", 1);

        public static readonly SortDirection Descending = new SortDirection("descend", 2);

        public SortDirection(string name, int value) : base(name, value)
        {
        }

        public static SortDirection Parse(string typeName)
        {
            return typeName switch
            {
                "ascend" => Ascending,
                "descend" => Descending,
                _ => None
            };
        }

        public static class Preset
        {
            public static readonly SortDirection[] Default = new[] { Ascending, Descending, None };

            public static readonly SortDirection[] TwoWay = new[] { Ascending, Descending, Ascending };
        }
    }
}
