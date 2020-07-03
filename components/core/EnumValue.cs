using System;

namespace AntDesign
{
    public abstract class EnumValue<TEnum, TValue> :
        IEquatable<EnumValue<TEnum, TValue>>,
        IComparable<EnumValue<TEnum, TValue>>
        where TEnum : EnumValue<TEnum, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        public string Name { get; set; }
        public TValue Value { get; set; }

        protected EnumValue(string name, TValue value)
        {
            this.Name = name;
            this.Value = value;
        }

        public bool Equals(EnumValue<TEnum, TValue> other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other is null)
                return false;

            return Value.Equals(other.Value);
        }

        public int CompareTo(EnumValue<TEnum, TValue> other)
        {
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public abstract class EnumValue<TEnum> :
        EnumValue<TEnum, int>
        where TEnum : EnumValue<TEnum, int>
    {
        protected EnumValue(string name, int value) :
            base(name, value)
        {
        }
    }
}
