using System.Globalization;
using Ardalis.SmartEnum;

namespace AntBlazor
{
    public sealed class AntMenuMode : SmartEnum<AntMenuMode>
    {
        public static readonly AntMenuMode Vertical = new AntMenuMode(nameof(Vertical).ToLower(CultureInfo.CurrentCulture), 1);
        public static readonly AntMenuMode Horizontal = new AntMenuMode(nameof(Horizontal).ToLower(CultureInfo.CurrentCulture), 2);
        public static readonly AntMenuMode Inline = new AntMenuMode(nameof(Inline).ToLower(CultureInfo.CurrentCulture), 3);

        private AntMenuMode(string name, int value) : base(name, value)
        {
        }
    }
}
