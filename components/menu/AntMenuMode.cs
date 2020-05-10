using Ardalis.SmartEnum;

namespace AntBlazor
{
    public sealed class AntMenuMode : SmartEnum<AntMenuMode>
    {
        public static readonly AntMenuMode Vertical = new AntMenuMode(nameof(Vertical).ToLower(), 1);
        public static readonly AntMenuMode Horizontal = new AntMenuMode(nameof(Horizontal).ToLower(), 2);
        public static readonly AntMenuMode Inline = new AntMenuMode(nameof(Inline).ToLower(), 3);

        private AntMenuMode(string name, int value) : base(name, value)
        {
        }
    }
}
