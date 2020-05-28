using Ardalis.SmartEnum;

namespace AntDesign
{
    public sealed class TriggerType : SmartEnum<TriggerType>
    {
        public static readonly TriggerType Click = new TriggerType(nameof(Click), 0);
        public static readonly TriggerType Hover = new TriggerType(nameof(Hover), 1);
        public static readonly TriggerType Focus = new TriggerType(nameof(Focus), 2);
        public static readonly TriggerType ContextMenu = new TriggerType(nameof(ContextMenu), 3);

        private TriggerType(string name, int value) : base(name, value)
        {
        }
    }
}
