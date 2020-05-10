using Ardalis.SmartEnum;

namespace AntBlazor
{
    public sealed class AntMenuTheme : SmartEnum<AntMenuTheme>
    {
        public static readonly AntMenuTheme Light = new AntMenuTheme(nameof(Light).ToLowerInvariant(), 1);
        public static readonly AntMenuTheme Dark = new AntMenuTheme(nameof(Dark).ToLowerInvariant(), 2);

        private AntMenuTheme(string name, int value) : base(name, value)
        {
        }
    }
}
