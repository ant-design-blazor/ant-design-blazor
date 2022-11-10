using static Util.Common.Delegate;

namespace AntDesign
{
    public sealed class GlobalThemeMode : EnumValue<GlobalThemeMode>
    {
        public static readonly GlobalThemeMode Light = new GlobalThemeMode(nameof(Light).ToLowerInvariant(), 1);
        public static readonly GlobalThemeMode Dark = new GlobalThemeMode(nameof(Dark).ToLowerInvariant(), 2);
        public static readonly GlobalThemeMode Compact = new GlobalThemeMode(nameof(Compact).ToLowerInvariant(), 3);
        public static readonly GlobalThemeMode Aliyun = new GlobalThemeMode(nameof(Aliyun).ToLowerInvariant(), 4);
        public static readonly GlobalThemeMode Custom = new GlobalThemeMode(nameof(Custom).ToLowerInvariant(), 5);

        private GlobalThemeMode(string name, int value) : base(name, value)
        {
        }
    }
}