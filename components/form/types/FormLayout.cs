using Ardalis.SmartEnum;

namespace AntBlazor
{
    public sealed class FormLayout : SmartEnum<FormLayout>
    {
        public static readonly FormLayout Horizontal= new FormLayout(nameof(Horizontal), 0);
        public static readonly FormLayout Vertical = new FormLayout(nameof(Vertical), 1);
        public static readonly FormLayout Inline = new FormLayout(nameof(Inline), 2);

        private FormLayout(string name, int value) : base(name, value)
        {
        }
    }
}
