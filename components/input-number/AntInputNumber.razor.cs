using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace AntBlazor
{
    /// <summary>
    /// base class for InputNumber component
    /// https://ant.design/components/input-number/
    /// </summary>
    public partial class AntInputNumber : AntInputComponentBase<double>
    {
        //<div class="ant-input-number">
        //    <div class="ant-input-number-handler-wrap">
        //        <span class="ant-input-number-handler ant-input-number-handler-up " role="button" aria-disabled="false" aria-label="Increase Value" unselectable="unselectable">
        //            <span class="anticon anticon-up ant-input-number-handler-up-inner" role="img" aria-label="up">
        //                <svg xmlns = "http://www.w3.org/2000/svg" class="" aria-hidden="true" fill="currentColor" viewBox="64 64 896 896" focusable="false" width="1em" height="1em" data-icon="up">
        //                    <path d = "M 890.5 755.3 L 537.9 269.2 c -12.8 -17.6 -39 -17.6 -51.7 0 L 133.5 755.3 A 8 8 0 0 0 140 768 h 75 c 5.1 0 9.9 -2.5 12.9 -6.6 L 512 369.8 l 284.1 391.6 c 3 4.1 7.8 6.6 12.9 6.6 h 75 c 6.5 0 10.3 -7.4 6.5 -12.7 Z" />
        //                </ svg >
        //            </ span >
        //        </ span >
        //        < span class="ant-input-number-handler ant-input-number-handler-down " role="button" aria-disabled="false" aria-label="Decrease Value" unselectable="unselectable">
        //            <span class="anticon anticon-down ant-input-number-handler-down-inner" role="img" aria-label="down">
        //                <svg xmlns = "http://www.w3.org/2000/svg" class="" aria-hidden="true" fill="currentColor" viewBox="64 64 896 896" focusable="false" width="1em" height="1em" data-icon="down">
        //                    <path d = "M 884 256 h -75 c -5.1 0 -9.9 2.5 -12.9 6.6 L 512 654.2 L 227.9 262.6 c -3 -4.1 -7.8 -6.6 -12.9 -6.6 h -75 c -6.5 0 -10.3 7.4 -6.5 12.7 l 352.6 486.1 c 12.8 17.6 39 17.6 51.7 0 l 352.6 -486.1 c 3.9 -5.3 0.1 -12.7 -6.4 -12.7 Z" />
        //                </ svg >
        //            </ span >
        //        </ span >
        //    </ div >
        //    < div class="ant-input-number-input-wrap">
        //        <input class="ant-input-number-input" role="spinbutton" aria-valuenow="3" aria-valuemin="1" aria-valuemax="10" min="1" max="10" step="1" value="3" autocomplete="off">
        //    </div>
        //</div>
        private string _format;
        protected const string PrefixCls = "ant-input-number";

        [Parameter]
        public Func<string, string> formatter { get; set; }

        private double _step = 1;
        [Parameter]
        public double step
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
                    _format = string.Join('.', _step.ToString().Split('.').Select(n => new string('0', n.Length)));
                }
            }
        }

        [Parameter]
        public double? defaultValue { get; set; }

        [Parameter]
        public double max { get; set; } = double.PositiveInfinity;

        [Parameter]
        public double min { get; set; } = double.NegativeInfinity;

        [Parameter]
        public string size { get; set; } = AntInputSize.Default;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (defaultValue.HasValue)
            {
                Value = defaultValue.Value;
            }
        }

        private void SetClass()
        {
            ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-lg", () => size == AntInputSize.Large)
                .If($"{PrefixCls}-sm", () => size == AntInputSize.Small)
                .If($"{PrefixCls}-disabled", () => Attributes != null && Attributes["disabled"].ToString() == true.ToString());
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClass();
        }

        private void Increase()
        {
            OnInput(new ChangeEventArgs() { Value = Value + step });
        }

        private void Decrease()
        {
            OnInput(new ChangeEventArgs() { Value = Value - step });
        }

        private void OnInput(ChangeEventArgs args)
        {
            // TODO: handle non-number input, parser
            if (double.TryParse(args.Value.ToString(), out double num) && num >= min && num <= max)
            {
                Value = num;
            }
        }

        private string GetIconClass(string direction)
        {
            string cls = string.Empty;
            if (direction == "up")
            {
                cls = $"ant-input-number-handler ant-input-number-handler-up " + (Value >= max ? "ant-input-number-handler-up-disabled" : string.Empty);
            }
            else
            {
                cls = $"ant-input-number-handler ant-input-number-handler-down " + (Value <= min ? "ant-input-number-handler-down-disabled" : string.Empty);
            }

            return cls;
        }

        private string DisplayValue()
        {
            if (formatter != null)
            {
                return formatter(Value.ToString());
            }

            return Value.ToString(_format);
        }
    }
}