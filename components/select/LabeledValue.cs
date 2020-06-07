namespace AntDesign
{
    public class LabeledValue
    {
        public string Key { get; }

        public object Label { get; }

        public bool Disabled { get; }

        public LabeledValue(string key, object label, bool disabled = false)
        {
            Key = key;
            Label = label;
            Disabled = disabled;
        }
    }
}
