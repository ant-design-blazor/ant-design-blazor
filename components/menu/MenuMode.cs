using Ardalis.SmartEnum;

namespace AntDesign
{
    public sealed class MenuMode : SmartEnum<MenuMode>
    {
        public static readonly MenuMode Vertical = new MenuMode(nameof(Vertical).ToLower(), 1);
        public static readonly MenuMode Horizontal = new MenuMode(nameof(Horizontal).ToLower(), 2);
        public static readonly MenuMode Inline = new MenuMode(nameof(Inline).ToLower(), 3);

        private MenuMode(string name, int value) : base(name, value)
        {
        }
    }
}
