namespace AntDesign
{
    public class TransferSelectChangeArgs
    {
        public string[] SourceSelectedKeys { get; private set; }

        public string[] TargetSelectedKeys { get; private set; }

        public TransferSelectChangeArgs(string[] sourceSelectedKeys, string[] targetSelectedKeys)
        {
            SourceSelectedKeys = sourceSelectedKeys;
            TargetSelectedKeys = targetSelectedKeys;
        }
    }
}
