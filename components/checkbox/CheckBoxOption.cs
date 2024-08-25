namespace AntDesign
{
    public class CheckboxOption<TValue>
    {
        public string Label { get; set; }

        public TValue Value { get; set; }

        public bool Checked { get; set; }

        public bool Disabled { get; set; }
    }
}
