namespace AntDesign
{
    public sealed class GlobalThemeMode : EnumValue<GlobalThemeMode>
    {
        public static readonly GlobalThemeMode Light = new GlobalThemeMode(nameof(Light).ToLowerInvariant(), "/_content/AntDesign/css/ant-design-blazor.css");
        public static readonly GlobalThemeMode Dark = new GlobalThemeMode(nameof(Dark).ToLowerInvariant(), "/_content/AntDesign/css/ant-design-blazor.dark.css");
        public static readonly GlobalThemeMode Compact = new GlobalThemeMode(nameof(Compact).ToLowerInvariant(), "/_content/AntDesign/css/ant-design-blazor.compact.css");
        public static readonly GlobalThemeMode Aliyun = new GlobalThemeMode(nameof(Aliyun).ToLowerInvariant(), "/_content/AntDesign/css/ant-design-blazor.aliyun.css");

        private GlobalThemeMode(string name, string value) : base(name, value)
        {
        }
    }
}
