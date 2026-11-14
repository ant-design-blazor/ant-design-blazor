// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
        <para>Small numerical value or status descriptor for UI elements.</para>

        <h2>When To Use</h2>

        <para>Badge normally appears in proximity to notifications or user avatars with eye-catching appeal, typically displaying unread messages count.</para>
    </summary>
    <seealso cref="AntDesign.BadgeRibbon" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/antfincdn/6%26GF9WHwvY/Badge.svg", Title = "Badge", SubTitle = "徽标数")]
    public partial class Badge : AntDomComponentBase
    {
        /// <summary>
        /// Customize Badge status dot color. Usage of this parameter will make the badge a status dot.
        /// </summary>
        [Parameter]
        public OneOf<BadgeColor?, string> Color { get; set; }

        /// <summary>
        /// Set Badge status dot to a preset color. Usage of this parameter will make the badge a status dot.
        /// </summary>
        [Parameter]
        [Obsolete("Use Color instead")]
        public PresetColor? PresetColor { get; set; }

        /// <summary>
        /// Number to show in badge
        /// </summary>
        [Parameter]
        public int? Count
        {
            get => _count;
            set
            {
                if (_count == value)
                {
                    return;
                }

                _count = value;

                if (_count > 0)
                {
                    _countArray = DigitsFromInteger(_count.Value);
                }
            }
        }

        /// <summary>
        /// Template to show in place of Count
        /// </summary>
        [Parameter]
        public RenderFragment CountTemplate { get; set; }

        /// <summary>
        /// Whether to display a dot instead of count
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Dot { get; set; }

        /// <summary>
        /// Set offset of the badge dot, like (left, top)
        /// </summary>
        [Parameter]
        public (int Left, int Top) Offset { get; set; }

        /// <summary>
        /// Max count to show
        /// </summary>
        /// <default value="99"/>
        [Parameter]
        public int OverflowCount
        {
            get => _overflowCount;
            set
            {
                if (_overflowCount == value)
                {
                    return;
                }

                _overflowCount = value;

                if (_overflowCount > 0)
                {
                    GenerateMaxNumberArray();
                }
            }
        }

        /// <summary>
        /// Whether to show badge when count is zero
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool ShowZero { get; set; } = false;

        /// <summary>
        /// Set Badge dot to a status color. Usage of this parameter will make the badge a status dot.
        /// </summary>
        [Parameter]
        public BadgeStatus? Status { get; set; }

        /// <summary>
        /// The display text next to the status dot
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// Text to show when hovering over the badge. Defaults to the value of Count
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Size of the badge
        /// </summary>
        [Parameter]
        public BadgeSize? Size { get; set; }

        /// <summary>
        /// Wrapping this item.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private ClassMapper CountClassMapper { get; set; } = new();

        private ClassMapper DotClassMapper { get; set; } = new();

        private int[] _countArray = [];

        private char[] _maxNumberArray = [];

        private static readonly int[] _countSingleArray = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

        private static readonly Dictionary<BadgeStatus, string> _statusMap = new()
        {
            [BadgeStatus.Default] = "default",
            [BadgeStatus.Success] = "success",
            [BadgeStatus.Processing] = "processing",
            [BadgeStatus.Error] = "error",
            [BadgeStatus.Warning] = "warning",
        };

        private string StatusOrPresetColor => Status.HasValue
            ? _statusMap[Status.Value]
            : (Color.IsT0 && Color.AsT0.HasValue
                ? Enum.GetName(typeof(BadgeColor), Color.AsT0)
                : string.Empty);

        private bool HasStatusOrColor => Status.HasValue || ((Color.IsT0 && Color.AsT0.HasValue) || (Color.IsT1 && !string.IsNullOrWhiteSpace(Color.AsT1)));

        private string CountStyle => Offset == default ? string.Empty : $"{$"right:{-Offset.Left}px"};{$"margin-top:{Offset.Top}px"};";

        private string DotColorStyle => Color.IsT1 ? $"background:{Color.AsT1};" : string.Empty;

        private bool RealShowSup => (Dot && (!Count.HasValue || (Count > 0 || Count == 0 && ShowZero)))
                                || Count > 0
                                || (Count == 0 && ShowZero);

        private bool _dotEnter;

        private bool _dotLeave;

        private bool _finalShowSup;

        private int? _count;

        private int _overflowCount = 99;

        private bool _showOverflowCount = false;

        /// <summary>
        /// Sets the default CSS classes.
        /// </summary>
        private void SetClassMap()
        {
            var prefixName = "ant-badge";
            ClassMapper.Clear()
                .Add(prefixName)
                .If($"{prefixName}-status", () => HasStatusOrColor)
                .If($"{prefixName}-not-a-wrapper", () => ChildContent == null)
                .If($"{prefixName}-rtl", () => RTL)
                ;

            CountClassMapper.Clear()
                .Add("ant-scroll-number")
                .If($"{prefixName}-count", () => !Dot && !HasStatusOrColor)
                .If($"{prefixName}-dot", () => Dot || HasStatusOrColor)
                .If($"{prefixName}-count-sm", () => Size == BadgeSize.Small)
                .GetIf(() => $"ant-badge-status-{StatusOrPresetColor}", () => !string.IsNullOrWhiteSpace(StatusOrPresetColor))
                .If($"{prefixName}-multiple-words", () => _countArray.Length >= 2)
                .If($"{prefixName}-zoom-enter {prefixName}-zoom-enter-active", () => _dotEnter)
                .If($"{prefixName}-zoom-leave {prefixName}-zoom-leave-active", () => _dotLeave)
                .If($"{prefixName}-count-overflow", () => _showOverflowCount)
                ;

            DotClassMapper.Clear()
                .Add("ant-badge-status-dot")
                .GetIf(() => $"ant-badge-status-{StatusOrPresetColor.ToLowerInvariant()}", () => !string.IsNullOrWhiteSpace(StatusOrPresetColor))
                ;
        }

        /// <summary>
        /// Startup code
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            _finalShowSup = RealShowSup;

            GenerateMaxNumberArray();

            SetClassMap();
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var showSupBefore = RealShowSup;
            var beforeDot = Dot;
            var delayDot = false;
            var beforeCount = _count;

            await base.SetParametersAsync(parameters);

            // if count is changing to 0 and it was overflow, hold the overflow display.
            if (_count == 0 && beforeCount > _overflowCount)
            {
                _showOverflowCount = true;
            }
            else
            {
                _showOverflowCount = _count > _overflowCount;
            }

            if (showSupBefore == RealShowSup)
            {
                return;
            }

            if (RealShowSup)
            {
                _dotEnter = true;

                if (!beforeDot && Dot)
                {
                    Dot = true;
                }

                _finalShowSup = true;

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
                _finalShowSup = false;

                if (delayDot)
                {
                    Dot = false;
                }
            }

            StateHasChanged();
        }

        private void GenerateMaxNumberArray()
        {
            _maxNumberArray = _overflowCount.ToString(CultureInfo.InvariantCulture).ToCharArray();
        }

        private static int[] DigitsFromInteger(int number)
        {
            var n = Math.Abs(number);
            var length = (int)Math.Log10(n > 0 ? n : 1) + 1;
            var digits = new int[length];
            for (var i = 0; i < length; i++)
            {
                digits[length - i - 1] = n % 10;
                n /= 10;
            }

            if (n < 0)
            {
                digits[0] *= -1;
            }

            return digits;
        }
    }
}
