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
using OneOf;

namespace AntDesign
{
    public partial class InputNumber<TValue> : AntInputComponentBase<TValue>
    {
        [Parameter]
        public int Precision { get; set; } = 0;

        [Parameter]
        public Func<TValue, string> Formatter { get; set; }

        [Parameter]
        public Func<string, string> Parser { get; set; }

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
                NumberFormatInfo nfi = CultureInfo.NumberFormat;
                var stepStr = Convert.ToDecimal(_step).ToString(nfi);
                if (stepStr.IndexOf(nfi.NumberDecimalSeparator) > 0)
                    _decimalPlaces = stepStr.Length - stepStr.IndexOf(nfi.NumberDecimalSeparator) - 1;
                else
                    _decimalPlaces = 0;
            }
        }

        [Parameter]
        public TValue DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                _hasDefaultValue = true;
            }
        }

        [Parameter]
        public TValue Max { get; set; }

        [Parameter]
        public TValue Min { get; set; }

        [Parameter]
        public int? MaxLength { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public EventCallback<TValue> OnChange { get; set; }

        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        [Parameter]
        public string PlaceHolder { get; set; }

        [Parameter]
        public bool Bordered { get; set; } = true;

        [Parameter]
        public OneOf<string, RenderFragment> Prefix { get; set; }

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
            { typeof(sbyte), sbyte.MaxValue },
            { typeof(byte), byte.MaxValue },

            { typeof(short), short.MaxValue },
            { typeof(ushort), ushort.MaxValue },

            { typeof(int), int.MaxValue },
            { typeof(uint), uint.MaxValue },

            { typeof(long), long.MaxValue },
            { typeof(ulong), ulong.MaxValue },

            { typeof(float), float.PositiveInfinity },
            { typeof(double), double.PositiveInfinity },
            { typeof(decimal), decimal.MaxValue },
        };

        private static readonly Dictionary<Type, object> _defaultMinimum = new Dictionary<Type, object>()
        {
            { typeof(sbyte), sbyte.MinValue },
            { typeof(byte), byte.MinValue },

            { typeof(short), short.MinValue },
            { typeof(ushort), ushort.MinValue },

            { typeof(int), int.MinValue },
            { typeof(uint), uint.MinValue },

            { typeof(long), long.MinValue },
            { typeof(ulong), ulong.MinValue },

            { typeof(float), float.NegativeInfinity},
            { typeof(double), double.NegativeInfinity },
            { typeof(decimal), decimal.MinValue },
        };

        private static Type[] _floatTypes = new Type[] { typeof(float), typeof(double), typeof(decimal) };

        private readonly bool _isNullable;
        private readonly int _interval = 200;

        private readonly Func<TValue, TValue, TValue> _increaseFunc;
        private readonly Func<TValue, TValue, TValue> _decreaseFunc;
        private readonly Func<TValue, TValue, bool> _greaterThanFunc;
        private readonly Func<TValue, TValue, bool> _equalToFunc;
        private readonly Func<TValue, int, TValue> _roundFunc;

        private CancellationTokenSource _increaseTokenSource;
        private CancellationTokenSource _decreaseTokenSource;

        private Func<string, TValue, TValue> _parseFunc;
        private Func<TValue, string, string> _toStringFunc;

        private string _inputString;
        private bool _focused;
        private bool _hasDefaultValue;
        private int? _decimalPlaces;
        private TValue _step;
        private TValue _defaultValue;

        private string _prefixCls = "ant-input-number";
        private string _inputNumberMode = "numeric";

        private ClassMapper _affixWarrperClass = new ClassMapper();

        private bool HasAffixWarrper => FormItem?.FeedbackIcon != null;

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
                _inputNumberMode = "decimal";
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

            if (_hasDefaultValue && EqualityComparer<TValue>.Default.Equals(Value, default))
            {
                CurrentValue = _defaultValue;
            }
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

            if (Parser is null && !Regex.IsMatch(value, @"^[+-]?\d*[.,]?\d*$"))
            {
                result = Value;
                return true;
            }

            if (value == "-" || value == "+")
            {
                value = "0";
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
            }
            else
            {
                if (Parser is not null)
                    result = _parseFunc(Parser(value), Value);
                else
                    result = _parseFunc(value, Value);
            }
            return true;
        }

        private void SetClass()
        {
            _affixWarrperClass
                .Add("ant-input-number-affix-wrapper")
                .If("ant-input-number-affix-wrapper-has-feedback", () => FormItem?.HasFeedback == true)
                .GetIf(() => $"ant-input-number-affix-wrapper-status-{FormItem?.ValidateStatus.ToString().ToLowerInvariant()}", () => FormItem is { ValidateStatus: not FormValidateStatus.Default });
            ;

            ClassMapper
                .Add(_prefixCls)
                .If($"{_prefixCls}-lg", () => Size == InputSize.Large)
                .If($"{_prefixCls}-sm", () => Size == InputSize.Small)
                .If($"{_prefixCls}-focused", () => _focused)
                .If($"{_prefixCls}-disabled", () => this.Disabled)
                .If($"{_prefixCls}-borderless", () => !Bordered)
                .GetIf(() => $"{_prefixCls}-status-{FormItem?.ValidateStatus.ToString().ToLowerInvariant()}", () => FormItem is { ValidateStatus: not FormValidateStatus.Default })
                .If($"{_prefixCls}-rtl", () => RTL);
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

            _increaseTokenSource?.Cancel();
            _increaseTokenSource = new CancellationTokenSource();

            _ = Increase(_increaseTokenSource.Token).ConfigureAwait(false);

            await SetFocus();
            var num = _increaseFunc(Value, _step);
            await ChangeValueAsync(num);
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

            _decreaseTokenSource?.Cancel();
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

        private async Task OnFocusAsync(FocusEventArgs args)
        {
            _focused = true;
            CurrentValue = Value;

            if (OnFocus.HasDelegate)
            {
                await OnFocus.InvokeAsync(args);
            }
        }

        private async Task OnBlurAsync()
        {
            _focused = false;
            if (_inputString != null)
            {
                await ConvertNumberAsync(_inputString);
                _inputString = null;
            }
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
                if (Precision > 0 || _decimalPlaces > 0)
                {
                    var round = Precision > 0 ? Precision : _decimalPlaces.Value;
                    CurrentValue = _roundFunc(value, round);
                }
                else
                {
                    CurrentValue = value;
                }
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

            if (EqualityComparer<TValue>.Default.Equals(value, default))
            {
                return default(TValue)?.ToString();
            }

            if (Precision > 0 || _decimalPlaces > 0)
            {
                var nN = "n" + (Precision > 0 ? Precision : _decimalPlaces.Value);
                return _toStringFunc(value, nN);
            }

            return _toStringFunc(value, null);
        }
    }
}
