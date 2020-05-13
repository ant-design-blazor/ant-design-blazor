using System.Globalization;
using Ardalis.SmartEnum;

namespace AntBlazor
{
    public sealed class AntMenuTheme : SmartEnum<AntMenuTheme>
    {
        public static readonly AntMenuTheme Light = new AntMenuTheme(nameof(Light).ToLower(CultureInfo.CurrentCulture), 1);
        public static readonly AntMenuTheme Dark = new AntMenuTheme(nameof(Dark).ToLower(CultureInfo.CurrentCulture), 2);

        private AntMenuTheme(string name, int value) : base(name, value)
        {
        }
    }
}
