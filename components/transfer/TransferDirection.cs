namespace AntDesign
{
    public class TransferDirection : EnumValue<TransferDirection>
    {
        public static readonly TransferDirection Left = new("left", 0);
        public static readonly TransferDirection Right = new("right", 1);

        private TransferDirection(string name, int value) : base(name, value)
        {
        }
    }
}
