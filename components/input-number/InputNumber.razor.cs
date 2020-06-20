using Microsoft.AspNetCore.Components;
using System;
using System.Globalization;
using System.Linq;

namespace AntDesign
{
    public partial class InputNumber<TValue> : AntInputComponentBase<TValue>
    {
        private string _format;
        protected const string PrefixCls = "ant-input-number";

        [Parameter]
        public Func<TValue, string> Formatter { get; set; }

        [Parameter]
        public Func<string, TValue> Parser { get; set; }

        private double _step = 1;

        [Parameter]
        public double Step
        {
            get
            {
                return _step;
            }
            set
            {
                _step = value;
                if (string.IsNullOrEmpty(_format))
                {
                    _format = string.Join('.', _step.ToString(CultureInfo.InvariantCulture).Split('.').Select(n => new string('0', n.Length)));
                }
            }
        }

        [Parameter]
        public TValue DefaultValue { get; set; }

        [Parameter]
        public TValue Max { get; set; }

        [Parameter]
        public TValue Min { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        private bool _isNullable;

        public InputNumber()
        {
            var t = typeof(TValue);
            _isNullable = t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>));

            if (typeof(TValue) == typeof(int) || typeof(TValue) == typeof(int?))
                SetMinMax(int.MinValue, int.MaxValue);
            else if (typeof(TValue) == typeof(decimal) || typeof(TValue) == typeof(decimal?))
                SetMinMax(decimal.MinValue, decimal.MaxValue);
            else if (typeof(TValue) == typeof(double) || typeof(TValue) == typeof(double?))
                SetMinMax(double.NegativeInfinity, double.PositiveInfinity);
            else if (typeof(TValue) == typeof(float) || typeof(TValue) == typeof(float?))
                SetMinMax(float.NegativeInfinity, float.PositiveInfinity);
        }

        private void SetMinMax(object min, object max)
        {
            var t = _isNullable ? Nullable.GetUnderlyingType(typeof(TValue)) : typeof(TValue);
            Max = (TValue)Convert.ChangeType(max, t);
            Min = (TValue)Convert.ChangeType(min, t);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Value = DefaultValue;
        }

        private void SetClass()
        {
            ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-lg", () => Size == InputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == InputSize.Small)
                .If($"{PrefixCls}-disabled", () => this.Disabled);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClass();
        }

        private void Increase()
        {
            object value = Value;

            if (typeof(TValue) == typeof(int) || typeof(TValue) == typeof(int?))
            {
                value = Convert.ToInt32(value) + Step;
            }
            else if (typeof(TValue) == typeof(decimal) || typeof(TValue) == typeof(decimal?))
            {
                value = Convert.ToDecimal(value) + Convert.ToDecimal(Step);
            }
            else if (typeof(TValue) == typeof(double) || typeof(TValue) == typeof(double?))
            {
                value = Convert.ToDouble(value) + Step;
            }
            else if (typeof(TValue) == typeof(float) || typeof(TValue) == typeof(float?))
            {
                value = Convert.ToSingle(value) + Step;
            }

            OnInput(new ChangeEventArgs() { Value = value });
        }

        private void Decrease()
        {
            object value = Value;
            if (typeof(TValue) == typeof(int) || typeof(TValue) == typeof(int?))
            {
                value = Convert.ToInt32(value) - Step;
            }
            else if (typeof(TValue) == typeof(decimal) || typeof(TValue) == typeof(decimal?))
            {
                value = Convert.ToDecimal(value) - Convert.ToDecimal(Step);
            }
            else if (typeof(TValue) == typeof(double) || typeof(TValue) == typeof(double?))
            {
                value = Convert.ToDouble(value) - Step;
            }
            else if (typeof(TValue) == typeof(float) || typeof(TValue) == typeof(float?))
            {
                value = Convert.ToSingle(value) - Step;
            }
            OnInput(new ChangeEventArgs() { Value = value });
        }

        private void OnInput(ChangeEventArgs args)
        {
            // TODO: handle non-number input, parser
            TValue num = default;
            if (Parser != null)
            {
                num = Parser(args.Value?.ToString());
            }
            else
            {
                try
                {
                    var value = args.Value;
                    if (_isNullable == false)
                    {
                        if (string.IsNullOrWhiteSpace(args.Value?.ToString())) value = default(TValue);
                        num = (TValue)Convert.ChangeType(value, typeof(TValue));
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(args.Value?.ToString()))
                        {
                            num = default(TValue);
                        }
                        else
                        {
                            num = (TValue)Convert.ChangeType(args.Value, Nullable.GetUnderlyingType(typeof(TValue)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    //TODO:还需补全非数字输入处理逻辑
                    return;
                }
            }

            if (MoreThan(num, Max))
                CurrentValue = Max;
            else if (MoreThan(Min, num))
                CurrentValue = Min;
            else
                CurrentValue = num;
        }

        private bool MoreThan(TValue left, TValue right)
        {
            if (typeof(TValue) == typeof(int) || typeof(TValue) == typeof(int?))
            {
                return (Convert.ToInt32(left) > Convert.ToInt32(right));
            }
            else if (typeof(TValue) == typeof(decimal) || typeof(TValue) == typeof(decimal?))
            {
                return (Convert.ToDecimal(left) > Convert.ToDecimal(right));
            }
            else if (typeof(TValue) == typeof(double) || typeof(TValue) == typeof(double?))
            {
                return (Convert.ToDouble(left) > Convert.ToDouble(right));
            }
            else if (typeof(TValue) == typeof(float) || typeof(TValue) == typeof(float?))
            {
                return (Convert.ToSingle(left) > Convert.ToSingle(right));
            }
            else
            {
                return false;
            }
        }

        private string GetIconClass(string direction)
        {
            string cls;
            if (direction == "up")
            {
                cls = $"ant-input-number-handler ant-input-number-handler-up " + (MoreThan(Value, Max) ? "ant-input-number-handler-up-disabled" : string.Empty);
            }
            else
            {
                cls = $"ant-input-number-handler ant-input-number-handler-down " + (MoreThan(Min, Value) ? "ant-input-number-handler-down-disabled" : string.Empty);
            }

            return cls;
        }

        protected override string FormatValueAsString(TValue value)
        {
            if (Formatter != null)
            {
                return Formatter(Value);
            }

            if (typeof(TValue) == typeof(int) || typeof(TValue) == typeof(int?))
            {
                return Convert.ToInt32(Value).ToString(_format, CultureInfo.InvariantCulture);
            }
            else if (typeof(TValue) == typeof(decimal) || typeof(TValue) == typeof(decimal?))
            {
                return Convert.ToDecimal(Value).ToString(_format, CultureInfo.InvariantCulture);
            }
            else if (typeof(TValue) == typeof(double) || typeof(TValue) == typeof(double?))
            {
                return Convert.ToDouble(Value).ToString(_format, CultureInfo.InvariantCulture);
            }
            else if (typeof(TValue) == typeof(float) || typeof(TValue) == typeof(float?))
            {
                return Convert.ToSingle(Value).ToString(_format, CultureInfo.InvariantCulture);
            }
            else
            {
                return Value?.ToString();
            }
        }
    }
}
