using Ardalis.SmartEnum;

namespace AntDesign
{
    public sealed class MenuTheme : SmartEnum<MenuTheme>
    {
        public static readonly MenuTheme Light = new MenuTheme(nameof(Light).ToLowerInvariant(), 1);
        public static readonly MenuTheme Dark = new MenuTheme(nameof(Dark).ToLowerInvariant(), 2);

        private MenuTheme(string name, int value) : base(name, value)
        {
        }
    }
}
