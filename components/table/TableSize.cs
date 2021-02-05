namespace AntDesign
{
    public sealed class TableSize : EnumValue<TableSize>
    {
        public static readonly TableSize Middle = new TableSize(nameof(Middle).ToLowerInvariant(), 1);
        public static readonly TableSize Small = new TableSize(nameof(Small).ToLowerInvariant(), 2);

        private TableSize(string name, int value) : base(name, value)
        {
        }
    }
}
