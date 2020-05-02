using Microsoft.AspNetCore.Components;
using System;
using System.Globalization;
using System.Linq;

namespace AntBlazor
{
    public partial class AntInputNumber : AntInputComponentBase<double>
    {
        private string _format;
        protected const string PrefixCls = "ant-input-number";

        [Parameter]
        public Func<double, string> Formatter { get; set; }

        [Parameter]
        public Func<string, double> Parser { get; set; }

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
        public double? DefaultValue { get; set; }

        [Parameter]
        public double Max { get; set; } = double.PositiveInfinity;

        [Parameter]
        public double Min { get; set; } = double.NegativeInfinity;

        [Parameter]
        public string Size { get; set; } = AntInputSize.Default;

        [Parameter]
        public bool Disabled { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (DefaultValue.HasValue)
            {
                Value = DefaultValue.Value;
            }
        }

        private void SetClass()
        {
            ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-lg", () => Size == AntInputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == AntInputSize.Small)
                .If($"{PrefixCls}-disabled", () => this.Disabled);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClass();
        }

        private void Increase()
        {
            OnInput(new ChangeEventArgs() { Value = Value + Step });
        }

        private void Decrease()
        {
            OnInput(new ChangeEventArgs() { Value = Value - Step });
        }

        private void OnInput(ChangeEventArgs args)
        {
            // TODO: handle non-number input, parser
            double num;
            if (Parser != null)
            {
                num = Parser(args.Value.ToString());
            }
            else
            {
                _ = double.TryParse(args.Value.ToString(), out num);
            }

            if (num >= Min && num <= Max)
            {
                Value = num;
            }
        }

        private string GetIconClass(string direction)
        {
            string cls;
            if (direction == "up")
            {
                cls = $"ant-input-number-handler ant-input-number-handler-up " + (Value >= Max ? "ant-input-number-handler-up-disabled" : string.Empty);
            }
            else
            {
                cls = $"ant-input-number-handler ant-input-number-handler-down " + (Value <= Min ? "ant-input-number-handler-down-disabled" : string.Empty);
            }

            return cls;
        }

        private string DisplayValue()
        {
            if (Formatter != null)
            {
                return Formatter(Value);
            }

            return Value.ToString(_format, CultureInfo.InvariantCulture);
        }
    }
}
