namespace AntDesign
{
    public class TransferSearchArgs
    {
        public string Direction { get; private set; }

        public string Value { get; private set; }

        public TransferSearchArgs(string direction, string value)
        {
            Direction = direction;
            Value = value;
        }
    }
}
