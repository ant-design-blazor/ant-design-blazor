using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// Small numerical value or status descriptor for UI elements.
    /// </summary>
    public partial class Badge : AntDomComponentBase
    {
        /// <summary>
        /// Customize Badge dot color
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Number to show in badge
        /// </summary>
        [Parameter]
        public int? Count { get; set; }

        [Parameter]
        public RenderFragment CountTemplate { get; set; }

        /// <summary>
        /// Whether to display a red dot instead of count
        /// </summary>
        [Parameter]
        public bool Dot { get; set; }

        /// <summary>
        /// Set offset of the badge dot, like[x, y]
        /// </summary>
        [Parameter]
        public ValueTuple<int, int> Offset { get; set; }

        /// <summary>
        /// Max count to show
        /// </summary>
        [Parameter]
        public int OverflowCount { get; set; } = 99;

        /// <summary>
        /// Whether to show badge when count is zero
        /// </summary>
        [Parameter]
        public bool ShowZero { get; set; } = false;

        /// <summary>
        /// Set Badge as a status dot
        /// </summary>
        [Parameter]
        public string Status { get; set; }

        /// <summary>
        /// If status is set, text sets the display text of the status dot
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// Text to show when hovering over the badge
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Size { get; set; }

        /// <summary>
        /// Wrapping this item.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private ClassMapper CountClassMapper { get; set; } = new ClassMapper();

        private int[] _countArray = Array.Empty<int>();

        private readonly int[] _countSingleArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        private char[] _maxNumberArray = Array.Empty<char>();

        private string StatusOrPresetColor => Status.IsIn(_badgeStatusTypes) ? Status : PresetColor;

        private string PresetColor => Color.IsIn(_badgePresetColors) ? Color : null;

        private bool HasStatusOrColor => !string.IsNullOrWhiteSpace(Status) || !string.IsNullOrWhiteSpace(Color);

        private string CountStyle => Offset == default ? null : $"{(Offset.Item1 > 0 ? $"right:-{Offset.Item1}px" : "")};{(Offset.Item2 > 0 ? $"margin-top:{Offset.Item2}px" : "")};";

        private string DotColorStyle => PresetColor == null && !string.IsNullOrWhiteSpace(Color) ? $"background:{Color};" : "";

        private bool ShowSup => (this.Dot && (!this.Count.HasValue || (this.Count > 0 || this.Count == 0 && this.ShowZero)))
                                || this.Count > 0
                                || (this.Count == 0 && this.ShowZero);

        private bool _dotEnter;

        private bool _dotLeave;

        private bool _showSup;

        /// <summary>
        /// Sets the default CSS classes.
        /// </summary>
        private void SetClassMap()
        {
            var prefixName = "ant-badge";
            ClassMapper.Clear()
                .Add(prefixName)
                .If($"{prefixName}-status", () => !string.IsNullOrEmpty(Status) || !string.IsNullOrEmpty(Color))
                .If($"{prefixName}-not-a-wrapper", () => ChildContent == null)
                ;

            CountClassMapper.Clear()
                .Add("ant-scroll-number")
                .If($"{prefixName}-count", () => !Dot)
                .If($"{prefixName}-dot", () => Dot)
                .If($"{prefixName}-count-sm", () => !string.IsNullOrWhiteSpace(Size) && Size.Equals("small", StringComparison.OrdinalIgnoreCase))
                .GetIf(() => $"ant-badge-status-{StatusOrPresetColor}", () => StatusOrPresetColor != null)
                .If($"{prefixName}-multiple-words", () => _countArray.Length >= 2)
                .If($"{prefixName}-zoom-enter {prefixName}-zoom-enter-active", () => _dotEnter)
                .If($"{prefixName}-zoom-leave {prefixName}-zoom-leave-active", () => _dotLeave)
                ;
        }

        /// <summary>
        /// Startup code
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (!string.IsNullOrEmpty(Color) && !string.IsNullOrEmpty(Status))
                throw new ArgumentException($"You cannot provide a {nameof(Status)} and a {nameof(Color)}, choose one.");

            _showSup = ShowSup;

            GenerateMaxNumberArray();

            SetClassMap();
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var showSupBefore = ShowSup;
            var beforeDot = Dot;
            var delayDot = false;
            await base.SetParametersAsync(parameters);

            if (showSupBefore != ShowSup)
            {
                if (ShowSup)
                {
                    _dotEnter = true;

                    if (!beforeDot && Dot)
                    {
                        Dot = true;
                    }

                    _showSup = true;

                    await Task.Delay(200);
                    _dotEnter = false;
                }
                else
                {
                    _dotLeave = true;

                    if (beforeDot && !Dot)
                    {
                        delayDot = true;
                        Dot = true;
                    }

                    await Task.Delay(200);
                    _dotLeave = false;
                    _showSup = false;

                    if (delayDot)
                    {
                        Dot = false;
                    }
                }
            }
        }

        /// <summary>
        /// Runs every time a parameter is set.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();

            if (Count.HasValue)
            {
                this._countArray = DigitsFromInteger(Count.Value);
            }

            if (OverflowCount > 0)
            {
                GenerateMaxNumberArray();
            }
        }

        private void GenerateMaxNumberArray()
        {
            this._maxNumberArray = OverflowCount.ToString(CultureInfo.InvariantCulture).ToCharArray();
        }

        private static int[] DigitsFromInteger(int number)
        {
            var n = Math.Abs(number);
            var length = (int)Math.Log10(n > 0 ? n : 1) + 1;
            var digits = new int[length];
            for (var i = 0; i < length; i++)
            {
                digits[(length - i) - 1] = n % 10;
                n /= 10;
            }

            if (n < 0)
                digits[0] *= -1;

            return digits;
        }

        private readonly string[] _badgePresetColors =
        {
            "pink",
            "red",
            "yellow",
            "orange",
            "cyan",
            "green",
            "blue",
            "purple",
            "geekblue",
            "magenta",
            "volcano",
            "gold",
            "lime"
        };

        private readonly string[] _badgeStatusTypes =
        {
            "success",
            "processing",
            "default",
            "error",
            "warning"
        };
    }
}
