using System;
using System.ComponentModel;

namespace AntDesign
{
    [Flags]
    public enum Trigger
    {
        Click = 1 << 0,
        Hover = 1 << 1,
        Focus = 1 << 2,
        ContextMenu = 1 << 3
    }

    public sealed class TriggerType : EnumValue<TriggerType>
    {
        public static readonly TriggerType Click = new TriggerType(nameof(Click), 0, Trigger.Click);
        public static readonly TriggerType Hover = new TriggerType(nameof(Hover), 1, Trigger.Hover);
        public static readonly TriggerType Focus = new TriggerType(nameof(Focus), 2, Trigger.Focus);
        public static readonly TriggerType ContextMenu = new TriggerType(nameof(ContextMenu), 3, Trigger.ContextMenu);

        public static TriggerType Create(Trigger trigger)
        {
            return trigger switch
            {
                Trigger.Click => Click,
                Trigger.Hover => Hover,
                Trigger.Focus => Focus,
                Trigger.ContextMenu => ContextMenu,
                _ => throw new InvalidEnumArgumentException($"Unrecognized value of Trigger enum ({trigger}).")
            };
        }

        private TriggerType(string name, int value, Trigger trigger) : base(name, value)
        {
            Trigger = trigger;
        }

        public Trigger Trigger { get; private set; }
    }
}
