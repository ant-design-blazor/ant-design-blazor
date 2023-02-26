namespace AntDesign
{
    public class TransferChangeArgs
    {
        public string[] TargetKeys { get; private set; }

        public TransferDirection Direction { get; private set; }

        public string[] MoveKeys { get; private set; }

        public TransferChangeArgs(string[] targetKeys, TransferDirection direction, string[] moveKeys)
        {
            TargetKeys = targetKeys;
            Direction = direction;
            MoveKeys = moveKeys;
        }
    }
}
