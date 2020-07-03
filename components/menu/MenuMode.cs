namespace AntDesign
{
    public sealed class MenuMode : EnumValue<MenuMode>
    {
        public static readonly MenuMode Vertical = new MenuMode(nameof(Vertical).ToLowerInvariant(), 1);
        public static readonly MenuMode Horizontal = new MenuMode(nameof(Horizontal).ToLowerInvariant(), 2);
        public static readonly MenuMode Inline = new MenuMode(nameof(Inline).ToLowerInvariant(), 3);

        private MenuMode(string name, int value) : base(name, value)
        {
        }
    }
}
