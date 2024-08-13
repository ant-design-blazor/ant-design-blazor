namespace AntDesign
{
    public class TransferSearchArgs
    {
        public TransferDirection Direction { get; private set; }

        public string Value { get; private set; }

        public TransferSearchArgs(TransferDirection direction, string value)
        {
            Direction = direction;
            Value = value;
        }
    }
}
