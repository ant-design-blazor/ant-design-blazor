using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace AntDesign
{
    public partial class InputNumber<TValue> : AntInputComponentBase<TValue>
    {
        private string _format;
        protected const string PrefixCls = "ant-input-number";

        [Parameter]
        public Func<TValue, string> Formatter { get; set; }

        [Parameter]
        public Func<string, string> Parser { get; set; }

        private TValue _step;

        [Parameter]
        public TValue Step
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
        public TValue DefaultValue { get; set; }

        [Parameter]
        public TValue Max { get; set; }

        [Parameter]
        public TValue Min { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        private bool _isNullable;
        private string _inputString;
        private bool _focused;

        private Func<TValue, TValue, TValue> _increaseFunc;
        private Func<TValue, TValue, TValue> _decreaseFunc;
        private Func<TValue, TValue, bool> _greaterThanFunc;
        private Func<TValue, TValue, bool> _greaterThanOrEqualFunc;
        private Func<TValue, string, string> _toStringFunc;

        public InputNumber()
        {
            var surfaceType = typeof(TValue);
            _isNullable = surfaceType.IsGenericType && surfaceType.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (surfaceType == typeof(int) || surfaceType == typeof(int?))
                SetMinMax(int.MinValue, int.MaxValue);
            else if (surfaceType == typeof(decimal) || surfaceType == typeof(decimal?))
                SetMinMax(decimal.MinValue, decimal.MaxValue);
            else if (surfaceType == typeof(double) || surfaceType == typeof(double?))
                SetMinMax(double.NegativeInfinity, double.PositiveInfinity);
            else if (surfaceType == typeof(float) || surfaceType == typeof(float?))
                SetMinMax(float.NegativeInfinity, float.PositiveInfinity);

            ParameterExpression piValue = Expression.Parameter(surfaceType, "value");
            ParameterExpression piStep = Expression.Parameter(surfaceType, "step");
            var fexpAdd = Expression.Lambda<Func<TValue, TValue, TValue>>(Expression.Add(piValue, piStep), piValue, piStep);
            _increaseFunc = fexpAdd.Compile();
            var fexpSubtract = Expression.Lambda<Func<TValue, TValue, TValue>>(Expression.Subtract(piValue, piStep), piValue, piStep);
            _decreaseFunc = fexpSubtract.Compile();

            ParameterExpression piLeft = Expression.Parameter(surfaceType, "left");
            ParameterExpression piRight = Expression.Parameter(surfaceType, "right");
            var fexpGreaterThan = Expression.Lambda<Func<TValue, TValue, bool>>(Expression.GreaterThan(piLeft, piRight), piLeft, piRight);
            _greaterThanFunc = fexpGreaterThan.Compile();
            var fexpGreaterThanOrEqual = Expression.Lambda<Func<TValue, TValue, bool>>(Expression.GreaterThanOrEqual(piLeft, piRight), piLeft, piRight);
            _greaterThanOrEqualFunc = fexpGreaterThanOrEqual.Compile();

            ParameterExpression format = Expression.Parameter(typeof(string), "format");
            ParameterExpression value = Expression.Parameter(surfaceType, "value");
            Expression expValue;
            if (_isNullable == true)
                expValue = Expression.Property(value, "Value");
            else
                expValue = value;
            MethodCallExpression expToString = Expression.Call(expValue, expValue.Type.GetMethod("ToString", new Type[] { typeof(string), typeof(IFormatProvider) }), format, Expression.Constant(CultureInfo.InvariantCulture));
            var lambdaToString = Expression.Lambda<Func<TValue, string, string>>(expToString, value, format);
            _toStringFunc = lambdaToString.Compile();

            Console.WriteLine(_toStringFunc.ToString());

            var underlyingType = _isNullable ? Nullable.GetUnderlyingType(surfaceType) : surfaceType;
            _step = (TValue)Convert.ChangeType(1, underlyingType);
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
            CurrentValue = DefaultValue;
            _inputString = CurrentValueAsString;
        }

        private void SetClass()
        {
            ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-lg", () => Size == InputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == InputSize.Small)
                .If($"{PrefixCls}-focused", () => _focused)
                .If($"{PrefixCls}-disabled", () => this.Disabled);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClass();
        }

        private void Increase()
        {
            _inputString = _increaseFunc(Value, Step).ToString();
            ConvertNumber(_inputString);
        }

        private void Decrease()
        {
            _inputString = _decreaseFunc(Value, Step).ToString();
            ConvertNumber(_inputString);
        }

        private void OnInput(ChangeEventArgs args)
        {
            _inputString = args.Value?.ToString();
            ConvertNumber(_inputString);
        }

        private void OnFocus()
        {
            _focused = true;
        }

        private void OnBlur()
        {
            _focused = false;
            _inputString = Regex.Replace(_inputString, @"[^\d.\d]", "");
            ConvertNumber(_inputString);
        }

        private void ConvertNumber(string inputString)
        {
            if (Parser != null)
            {
                _inputString = Parser(inputString);
            }

            if (Regex.IsMatch(inputString, @"^[+-]?\d*[.]?\d*$"))
            {
                TValue num;
                if (!_isNullable)
                {
                    if (string.IsNullOrWhiteSpace(inputString))
                    {
                        num = default;
                    }
                    else
                    {
                        num = (TValue)Convert.ChangeType(inputString, typeof(TValue));
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(inputString))
                    {
                        num = default;
                    }
                    else
                    {
                        num = (TValue)Convert.ChangeType(inputString, Nullable.GetUnderlyingType(typeof(TValue)));
                    }
                }
                ChangeValue(num);
            }
        }

        private void ChangeValue(TValue value)
        {
            if (_greaterThanFunc(value, Max))
                CurrentValue = Max;
            else if (_greaterThanFunc(Min, value))
                CurrentValue = Min;
            else
                CurrentValue = value;
        }

        private string GetIconClass(string direction)
        {
            string cls;
            if (direction == "up")
            {
                cls = $"ant-input-number-handler ant-input-number-handler-up " + (_greaterThanOrEqualFunc(Value, Max) ? "ant-input-number-handler-up-disabled" : string.Empty);
            }
            else
            {
                cls = $"ant-input-number-handler ant-input-number-handler-down " + (_greaterThanOrEqualFunc(Min, Value) ? "ant-input-number-handler-down-disabled" : string.Empty);
            }

            return cls;
        }

        protected override string FormatValueAsString(TValue value)
        {
            if (Formatter != null)
            {
                return Formatter(Value);
            }

            if (EqualityComparer<TValue>.Default.Equals(value, default(TValue)) == false)
                return _toStringFunc(value, _format);
            else
                return default(TValue).ToString();
        }
    }
}
