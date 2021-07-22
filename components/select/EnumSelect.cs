using System;
using System.Linq;

namespace AntDesign
{
    public class EnumSelect<TEnum> : Select<TEnum, EnumLookup<TEnum>>
    {
        public EnumSelect()
        {
            if (typeof(TEnum).IsEnum)
            {
                DataSource = Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(t => new EnumLookup<TEnum>
                {
                    Value = t,
                    Label = Enum.GetName(typeof(TEnum), t)
                })
                .OrderBy(t => t.Value)
                .ToList();
                ValueName = nameof(EnumLookup<TEnum>.Value);
                LabelName = nameof(EnumLookup<TEnum>.Label);
            }
        }
    }

    public class EnumLookup<TEnum>
    {
        public TEnum Value { get; set; }

        public string Label { get; set; }
    }
}
