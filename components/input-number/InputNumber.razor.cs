using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
                var stepStr = _step.ToString();
                if (string.IsNullOrEmpty(_format))
                {
                    _format = string.Join('.', stepStr.Split('.').Select(n => new string('0', n.Length)));
                }
                else
                {
                    if (stepStr.IndexOf('.') > 0)
                        DecimalPlaces = stepStr.Length - stepStr.IndexOf('.') - 1;
                    else
                        DecimalPlaces = 0;
                }
            }
        }


        public int? DecimalPlaces { get; set; }

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
        private Func<TValue, int, TValue> _roundFunc;

        private static Type _surfaceType = typeof(TValue);

        private static Dictionary<Type, object> _defaultMaximum = new Dictionary<Type, object>()
        {
            { typeof(int),int.MaxValue },
            { typeof(decimal),decimal.MaxValue },
            { typeof(double),double.PositiveInfinity },
            { typeof(float),float.PositiveInfinity },
        };

        private static Dictionary<Type, object> _defaultMinimum = new Dictionary<Type, object>()
        {
            { typeof(int),int.MinValue },
            { typeof(decimal),decimal.MinValue },
            { typeof(double),double.NegativeInfinity },
            { typeof(float),float.NegativeInfinity},
        };

        public InputNumber()
        {
            _isNullable = _surfaceType.IsGenericType && _surfaceType.GetGenericTypeDefinition() == typeof(Nullable<>);

            //递增与递减
            ParameterExpression piValue = Expression.Parameter(_surfaceType, "value");
            ParameterExpression piStep = Expression.Parameter(_surfaceType, "step");
            var fexpAdd = Expression.Lambda<Func<TValue, TValue, TValue>>(Expression.Add(piValue, piStep), piValue, piStep);
            _increaseFunc = fexpAdd.Compile();
            var fexpSubtract = Expression.Lambda<Func<TValue, TValue, TValue>>(Expression.Subtract(piValue, piStep), piValue, piStep);
            _decreaseFunc = fexpSubtract.Compile();

            //数字比较
            ParameterExpression piLeft = Expression.Parameter(_surfaceType, "left");
            ParameterExpression piRight = Expression.Parameter(_surfaceType, "right");
            var fexpGreaterThan = Expression.Lambda<Func<TValue, TValue, bool>>(Expression.GreaterThan(piLeft, piRight), piLeft, piRight);
            _greaterThanFunc = fexpGreaterThan.Compile();
            var fexpGreaterThanOrEqual = Expression.Lambda<Func<TValue, TValue, bool>>(Expression.GreaterThanOrEqual(piLeft, piRight), piLeft, piRight);
            _greaterThanOrEqualFunc = fexpGreaterThanOrEqual.Compile();

            //格式化
            ParameterExpression format = Expression.Parameter(typeof(string), "format");
            ParameterExpression value = Expression.Parameter(_surfaceType, "value");
            Expression expValue;
            if (_isNullable == true)
                expValue = Expression.Property(value, "Value");
            else
                expValue = value;
            MethodCallExpression expToString = Expression.Call(expValue, expValue.Type.GetMethod("ToString", new Type[] { typeof(string), typeof(IFormatProvider) }), format, Expression.Constant(CultureInfo.InvariantCulture));
            var lambdaToString = Expression.Lambda<Func<TValue, string, string>>(expToString, value, format);
            _toStringFunc = lambdaToString.Compile();

            //四舍五入
            ParameterExpression num = Expression.Parameter(_surfaceType, "num");
            ParameterExpression decimalPlaces = Expression.Parameter(typeof(int), "decimalPlaces");
            MethodCallExpression expRound = Expression.Call(null, typeof(InputNumberMath).GetMethod("Round", new Type[] { _surfaceType, typeof(int) }), num, decimalPlaces);
            var lambdaRound = Expression.Lambda<Func<TValue, int, TValue>>(expRound, num, decimalPlaces);
            _roundFunc = lambdaRound.Compile();


            var underlyingType = _isNullable ? Nullable.GetUnderlyingType(_surfaceType) : _surfaceType;

            if (_defaultMaximum.ContainsKey(underlyingType)) Max = (TValue)Convert.ChangeType(_defaultMaximum[underlyingType], underlyingType);
            if (_defaultMinimum.ContainsKey(underlyingType)) Min = (TValue)Convert.ChangeType(_defaultMinimum[underlyingType], underlyingType);

            _step = (TValue)Convert.ChangeType(1, underlyingType);
        }

        private void SetMinMax(object min, object max)
        {
            var t = _isNullable ? Nullable.GetUnderlyingType(_surfaceType) : _surfaceType;
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
        }

        private void OnFocus()
        {
            _focused = true;
            CurrentValue = Value;
        }

        private void OnBlur()
        {
            _focused = false;
            if (_inputString == null) _inputString = Value.ToString();
            _inputString = Regex.Replace(_inputString, @"[^\+\-\d.\d]", "");
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
                if (inputString == "-" || inputString == "+") inputString = "0";
                TValue num;
                if (!_isNullable)
                {
                    if (string.IsNullOrWhiteSpace(inputString))
                    {
                        num = default;
                    }
                    else
                    {
                        num = (TValue)Convert.ChangeType(inputString, _surfaceType);
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
                        num = (TValue)Convert.ChangeType(inputString, Nullable.GetUnderlyingType(_surfaceType));
                    }
                }
                if (DecimalPlaces.HasValue)
                    num = _roundFunc(num, DecimalPlaces.Value);
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

            if (EqualityComparer<TValue>.Default.Equals(value, default) == false)
                return _toStringFunc(value, _format);
            else
                return default(TValue)?.ToString();
        }
    }
}
