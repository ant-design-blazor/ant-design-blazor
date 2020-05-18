using Ardalis.SmartEnum;

namespace AntBlazor
{
    public sealed class TriggerType : SmartEnum<TriggerType>
    {
        public static readonly TriggerType Click = new TriggerType(nameof(Click), 0);
        public static readonly TriggerType Hover = new TriggerType(nameof(Hover), 1);
        public static readonly TriggerType ContextMenu = new TriggerType(nameof(ContextMenu), 2);

        private TriggerType(string name, int value) : base(name, value)
        {
        }
    }
}
