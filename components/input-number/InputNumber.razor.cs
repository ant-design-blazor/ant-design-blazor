using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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
                        _decimalPlaces = stepStr.Length - stepStr.IndexOf('.') - 1;
                    else
                        _decimalPlaces = 0;
                }
            }
        }

        private int? _decimalPlaces;

        [Parameter]
        public TValue DefaultValue { get; set; }

        [Parameter]
        public TValue Max { get; set; }

        [Parameter]
        public TValue Min { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public EventCallback<TValue> OnChange { get; set; }

        private readonly bool _isNullable;


        private readonly Func<TValue, TValue, TValue> _increaseFunc;
        private readonly Func<TValue, TValue, TValue> _decreaseFunc;
        private readonly Func<TValue, TValue, bool> _greaterThanFunc;
        private readonly Func<TValue, TValue, bool> _equalToFunc;
        private Func<TValue, string, string> _toStringFunc;
        private readonly Func<TValue, int, TValue> _roundFunc;
        private Func<string, TValue, TValue> _parseFunc;

        private static readonly Type _surfaceType = typeof(TValue);

        private static readonly Type[] _smallIntegerType = new Type[]
        {
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
        };

        private static readonly Type[] _supportTypes = new Type[] {
             typeof(sbyte),
             typeof(byte),

             typeof(short),
             typeof(ushort),

             typeof(int),
             typeof(uint),

             typeof(long),
             typeof(ulong),

             typeof(float),
             typeof(double),
             typeof(decimal)
        };

        private static readonly Dictionary<Type, object> _defaultMaximum = new Dictionary<Type, object>()
        {
            { typeof(sbyte),sbyte.MaxValue },
            { typeof(byte), byte.MaxValue },

            { typeof(short),short.MaxValue },
            { typeof(ushort),ushort.MaxValue },

            { typeof(int),int.MaxValue },
            { typeof(uint),uint.MaxValue },

            { typeof(long),long.MaxValue },
            { typeof(ulong),ulong.MaxValue },

            { typeof(float),float.PositiveInfinity },
            { typeof(double),double.PositiveInfinity },
            { typeof(decimal),decimal.MaxValue },
        };

        private static readonly Dictionary<Type, object> _defaultMinimum = new Dictionary<Type, object>()
        {
            { typeof(sbyte),sbyte.MinValue },
            { typeof(byte), byte.MinValue },

            { typeof(short),short.MinValue },
            { typeof(ushort),ushort.MinValue },

            { typeof(int),int.MinValue },
            { typeof(uint),uint.MinValue },

            { typeof(long),long.MinValue },
            { typeof(ulong),ulong.MinValue },

            { typeof(float),float.NegativeInfinity},
            { typeof(double),double.NegativeInfinity },
            { typeof(decimal),decimal.MinValue },
        };

        private static Type[] _floatTypes = new Type[] { typeof(float), typeof(double), typeof(decimal) };
        private string _inputString;
        private bool _focused;

        private readonly int _interval = 200;
        private CancellationTokenSource _increaseTokenSource;
        private CancellationTokenSource _decreaseTokenSource;

        public InputNumber()
        {
            _isNullable = _surfaceType.IsGenericType && _surfaceType.GetGenericTypeDefinition() == typeof(Nullable<>);
            var underlyingType = _isNullable ? Nullable.GetUnderlyingType(_surfaceType) : _surfaceType;
            if (!_supportTypes.Contains(underlyingType))
            {
                throw new NotSupportedException("InputNumber supports only numeric types.");
            }

            //递增与递减 Increment and decrement
            ParameterExpression piValue = Expression.Parameter(_surfaceType, "value");
            ParameterExpression piStep = Expression.Parameter(_surfaceType, "step");
            Expression<Func<TValue, TValue, TValue>> fexpAdd;
            Expression<Func<TValue, TValue, TValue>> fexpSubtract;
            if (_smallIntegerType.Contains(underlyingType))
            {
                fexpAdd = Expression.Lambda<Func<TValue, TValue, TValue>>(Expression.Convert(Expression.Add(Expression.Convert(piValue, typeof(int)), Expression.Convert(piStep, typeof(int))), _surfaceType), piValue, piStep);
                fexpSubtract = Expression.Lambda<Func<TValue, TValue, TValue>>(Expression.Convert(Expression.Subtract(Expression.Convert(piValue, typeof(int)), Expression.Convert(piStep, typeof(int))), _surfaceType), piValue, piStep);
            }
            else
            {
                fexpAdd = Expression.Lambda<Func<TValue, TValue, TValue>>(Expression.Add(piValue, piStep), piValue, piStep);
                fexpSubtract = Expression.Lambda<Func<TValue, TValue, TValue>>(Expression.Subtract(piValue, piStep), piValue, piStep);
            }
            _increaseFunc = fexpAdd.Compile();
            _decreaseFunc = fexpSubtract.Compile();

            //数字比较 Digital comparison
            ParameterExpression piLeft = Expression.Parameter(_surfaceType, "left");
            ParameterExpression piRight = Expression.Parameter(_surfaceType, "right");
            var fexpGreaterThan = Expression.Lambda<Func<TValue, TValue, bool>>(Expression.GreaterThan(piLeft, piRight), piLeft, piRight);
            _greaterThanFunc = fexpGreaterThan.Compile();
            var fexpEqualTo = Expression.Lambda<Func<TValue, TValue, bool>>(Expression.Equal(piLeft, piRight), piLeft, piRight);
            _equalToFunc = fexpEqualTo.Compile();

            //四舍五入 rounding
            if (_floatTypes.Contains(_surfaceType))
            {
                ParameterExpression num = Expression.Parameter(_surfaceType, "num");
                ParameterExpression decimalPlaces = Expression.Parameter(typeof(int), "decimalPlaces");
                MethodCallExpression expRound = Expression.Call(null, typeof(InputNumberMath).GetMethod(nameof(InputNumberMath.Round), new Type[] { _surfaceType, typeof(int) }), num, decimalPlaces);
                var lambdaRound = Expression.Lambda<Func<TValue, int, TValue>>(expRound, num, decimalPlaces);
                _roundFunc = lambdaRound.Compile();
            }

            if (_defaultMaximum.ContainsKey(underlyingType)) Max = (TValue)_defaultMaximum[underlyingType];
            if (_defaultMinimum.ContainsKey(underlyingType)) Min = (TValue)_defaultMinimum[underlyingType];

            _step = (TValue)Convert.ChangeType(1, underlyingType);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // 数字解析 Digital analysis
            ParameterExpression input = Expression.Parameter(typeof(string), "input");
            ParameterExpression defaultValue = Expression.Parameter(typeof(TValue), "defaultValue");
            MethodCallExpression inputParse = Expression.Call(null, typeof(InputNumberMath).GetMethod(nameof(InputNumberMath.Parse), new Type[] { typeof(string), typeof(TValue), typeof(CultureInfo) }), input, defaultValue, Expression.Constant(CultureInfo));
            var lambdaParse = Expression.Lambda<Func<string, TValue, TValue>>(inputParse, input, defaultValue);
            _parseFunc = lambdaParse.Compile();

            //格式化 format
            ParameterExpression format = Expression.Parameter(typeof(string), "format");
            ParameterExpression value = Expression.Parameter(_surfaceType, "value");
            Expression expValue;
            if (_isNullable)
                expValue = Expression.Property(value, "Value");
            else
                expValue = value;
            MethodCallExpression expToString = Expression.Call(expValue, expValue.Type.GetMethod("ToString", new Type[] { typeof(string), typeof(IFormatProvider) }), format, Expression.Constant(CultureInfo));
            var lambdaToString = Expression.Lambda<Func<TValue, string, string>>(expToString, value, format);
            _toStringFunc = lambdaToString.Compile();


            SetClass();
            CurrentValue = Value ?? DefaultValue;
        }

        /// <summary>
        /// Always return true, if input string is invalid, result = default, if input string is null or empty, result = DefaultValue
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="validationErrorMessage"></param>
        /// <returns></returns>
        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            validationErrorMessage = null;
            if (!Regex.IsMatch(value, @"^[+-]?\d*[.,]?\d*$"))
            {
                result = Value;
                return true;
            }

            if (value == "-" || value == "+")
            {
                value = "0";
            }
            if (!_isNullable)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = default;
                }
                else
                {
                    result = _parseFunc(value, Value);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = default;
                }
                else
                {
                    result = _parseFunc(value, Value);
                }
            }
            return true;
        }

        private void SetClass()
        {
            ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-lg", () => Size == InputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == InputSize.Small)
                .If($"{PrefixCls}-focused", () => _focused)
                .If($"{PrefixCls}-disabled", () => this.Disabled)
                .If($"{PrefixCls}-rtl", () => RTL);
        }

        #region Value Increase and Decrease Methods

        private async Task IncreaseDown()
        {
            if (_isNullable && Value == null)
            {
                return;
            }
            if (_equalToFunc(Value, Max))
            {
                return;
            }
            await SetFocus();
            var num = _increaseFunc(Value, _step);
            await ChangeValueAsync(num);

            _increaseTokenSource = new CancellationTokenSource();
            _ = Increase(_increaseTokenSource.Token).ConfigureAwait(false);
        }

        private void IncreaseUp() => _increaseTokenSource?.Cancel();

        private async Task Increase(CancellationToken cancellationToken)
        {
            await Task.Delay(600, CancellationToken.None);
            while (true)
            {
                if (_equalToFunc(Value, Max) || cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                var num = _increaseFunc(Value, _step);
                await ChangeValueAsync(num);
                StateHasChanged();
                await Task.Delay(_interval, CancellationToken.None);
            }
        }

        private async Task DecreaseDown()
        {
            if (_isNullable && Value == null)
            {
                return;
            }
            if (_equalToFunc(Value, Min))
            {
                return;
            }
            await SetFocus();
            var num = _decreaseFunc(Value, _step);
            await ChangeValueAsync(num);

            _decreaseTokenSource = new CancellationTokenSource();
            _ = Decrease(_decreaseTokenSource.Token).ConfigureAwait(false);
        }

        private void DecreaseUp() => _decreaseTokenSource?.Cancel();

        private async Task Decrease(CancellationToken cancellationToken)
        {
            await Task.Delay(600, CancellationToken.None);
            while (true)
            {
                if (_equalToFunc(Value, Min) || cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                var num = _decreaseFunc(Value, _step);
                await ChangeValueAsync(num);
                StateHasChanged();
                await Task.Delay(_interval, CancellationToken.None);
            }
        }

        private async Task OnKeyDown(KeyboardEventArgs e)
        {
            if (_isNullable && Value == null)
            {
                return;
            }
            if (e.Key == "ArrowUp")
            {
                if (_equalToFunc(Value, Max))
                {
                    return;
                }
                var num = _increaseFunc(Value, _step);
                await ChangeValueAsync(num);
                StateHasChanged();
            }
            else if (e.Key == "ArrowDown")
            {
                if (_equalToFunc(Value, Min))
                {
                    return;
                }
                var num = _decreaseFunc(Value, _step);
                await ChangeValueAsync(num);
                StateHasChanged();
            }
        }

        #endregion Value Increase and Decrease Methods

        private async Task SetFocus()
        {
            _focused = true;
            await FocusAsync(Ref);
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

        private async Task OnBlurAsync()
        {
            _focused = false;
            if (_inputString == null)
            {
                await ChangeValueAsync(Value);
                return;
            }

            if (!CurrentValueAsString.Equals(_inputString))
                CurrentValueAsString = _inputString;

            _inputString = null;
        }

        private async Task ConvertNumberAsync(string inputString)
        {
            _ = TryParseValueFromString(inputString, out TValue num, out _);
            await ChangeValueAsync(num);
        }

        private async Task ChangeValueAsync(TValue value)
        {
            if (_greaterThanFunc(value, Max))
                value = Max;
            else if (_greaterThanFunc(Min, value))
                value = Min;
            if (_roundFunc == null)
            {
                CurrentValue = value;
            }
            else
            {
                CurrentValue = _decimalPlaces.HasValue ? _roundFunc(value, _decimalPlaces.Value) : value;
            }
            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(value);
            }
        }

        private string GetIconClass(string direction)
        {
            string cls;
            if (direction == "up")
            {
                cls = $"ant-input-number-handler ant-input-number-handler-up " + (!_greaterThanFunc(Max, Value) ? "ant-input-number-handler-up-disabled" : string.Empty);
            }
            else
            {
                cls = $"ant-input-number-handler ant-input-number-handler-down " + (!_greaterThanFunc(Value, Min) ? "ant-input-number-handler-down-disabled" : string.Empty);
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
