// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Display the current progress of an operation flow.</para>

    <h2>When To Use</h2>

    <para>If it will take a long time to complete an operation, you can use `Progress` to show the current progress and status.</para>

    <list type="bullet">
        <item>When an operation will interrupt the current interface, or it needs to run in the background for more than 2 seconds.</item>
        <item>When you need to display the completion percentage of an operation.</item>
    </list>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Feedback, "https://gw.alipayobjects.com/zos/alicdn/xqsDu4ZyR/Progress.svg", Title = "Progress", SubTitle = "进度条")]
    public partial class Progress : AntDomComponentBase
    {
        private const string PrefixCls = "ant-progress";
        private const double CircleDash = 295.31;
        private string _bgStyle;
        private string _bgSuccessStyle;
        private string _circleTrailStyle;
        private string _circlePathStyle;
        private string _circleSuccessStyle;

        #region Parameters

        /// <summary>
        /// progress size
        /// </summary>
        /// <default value="ProgressSize.Default" />
        [Parameter]
        public ProgressSize Size { get; set; } = ProgressSize.Default;

        /// <summary>
        /// to set the type, options: line circle dashboard
        /// </summary>
        /// <default value="ProgressType.Line" />
        [Parameter]
        public ProgressType Type { get; set; } = ProgressType.Line;

        /// <summary>
        /// template function of the content
        /// </summary>
        /// <default value="i%" />
        [Parameter]
        public Func<double, string> Format { get; set; } = (i) => i + "%";

        /// <summary>
        /// to set the completion percentage
        /// </summary>
        [Parameter]
        public double Percent { get; set; }

        /// <summary>
        /// whether to display the progress value and the status icon
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool ShowInfo { get; set; } = true;

        /// <summary>
        /// to set the status of the Progress, options: success exception normal active(line only)
        /// </summary>
        /// <default value="ProgressStatus.Normal" />
        [Parameter]
        public ProgressStatus? Status { get; set; }

        /// <summary>
        /// to set the style of the progress linecap
        /// </summary>
        /// <default value="ProgressStrokeLinecap.Round" />
        [Parameter]
        public ProgressStrokeLinecap StrokeLinecap { get; set; } = ProgressStrokeLinecap.Round;

        /// <summary>
        /// segmented success percent
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public double SuccessPercent { get; set; }

        /// <summary>
        /// color of unfilled part
        /// </summary>
        [Parameter]
        public string TrailColor { get; set; }

        /// <summary>
        /// to set the width of the progress bar, unit: px
        /// to set the width of the circular progress, unit: percentage of the canvas width
        /// to set the width of the dashboard progress, unit: percentage of the canvas width
        /// </summary>
        /// <default value="Type = line: 10, Type = circle or dashboard: 6" />
        [Parameter]
        public int StrokeWidth { get; set; }

        /// <summary>
        /// color of progress bar, render linear-gradient when passing an object
        /// color of circular progress, render linear-gradient when passing an object
        /// </summary>
        [Parameter]
        public OneOf<string, Dictionary<string, string>> StrokeColor { get; set; }

        /// <summary>
        /// the total step count
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public int Steps { get; set; }

        /// <summary>
        /// to set the canvas width of the circular progress, unit: px
        /// to set the canvas width of the dashboard progress, unit: px
        /// </summary>
        /// <default value="120" />
        [Parameter]
        public int Width { get; set; } = 120;

        /// <summary>
        /// the gap degree of half circle, 0 ~ 295
        /// </summary>
        /// <default value="75" />
        [Parameter]
        public int GapDegree { get; set; } = 75;

        /// <summary>
        /// the gap position, options: top bottom left right
        /// </summary>
        /// <default value="ProgressGapPosition.Bottom" />
        [Parameter]
        public ProgressGapPosition GapPosition { get; set; } = ProgressGapPosition.Bottom;

        #endregion Parameters

        private static readonly Dictionary<ProgressSize, int> _sizeMap = new()
        {
            [ProgressSize.Small] = 6,
            [ProgressSize.Default] = 8,
        };

        protected override void OnInitialized()
        {
            SetClasses();
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (StrokeWidth <= 0)
            {
                var size = Type switch
                {
                    ProgressType.Line => Size,
                    _ => ProgressSize.Small // Type is Circle or Dashboard
                };
                StrokeWidth = _sizeMap[size];
            }

            if (Status is null && Percent is double percent && percent == 100)
            {
                Status = ProgressStatus.Success;
            }

            Status ??= ProgressStatus.Normal;

            SetStyle();
        }

        private void SetClasses()
        {
            ClassMapper
                .Add(PrefixCls)
                .Get(() => $"{PrefixCls}-status-{Status.ToString().ToLowerInvariant()}")
                .GetIf(() => $"{PrefixCls}-small", () => Size == ProgressSize.Small)
                .GetIf(() => $"{PrefixCls}-default", () => Size == ProgressSize.Default)
                .GetIf(() => $"{PrefixCls}-{Type.ToString().ToLowerInvariant()}", () => Type != ProgressType.Dashboard)
                .GetIf(() => $"{PrefixCls}-circle", () => Type == ProgressType.Dashboard)
                .GetIf(() => $"{PrefixCls}-show-info", () => ShowInfo)
                .GetIf(() => $"{PrefixCls}-steps", () => Steps > 0)
                .GetIf(() => $"{PrefixCls}-rtl", () => RTL);
        }

        private void SetStyle()
        {
            if (Type == ProgressType.Line)
            {
                _bgStyle = GetLineBGStyle();
                if (SuccessPercent != 0)
                {
                    _bgSuccessStyle = FormattableString.Invariant($" width: {SuccessPercent}%; height: {StrokeWidth}px;");
                }
            }
            else if (Type == ProgressType.Circle)
            {
                _bgStyle = Size == ProgressSize.Default ? $"width: {Width}px; height: {Width}px; font-size: 24px;" : "width: 80px; height: 80px; font-size: 18px;";
                _circleTrailStyle = FormattableString.Invariant($"stroke:{TrailColor}; transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {CircleDash}px, {CircleDash}px; stroke-dashoffset: 0px;");
                if (SuccessPercent == 0)
                {
                    _circlePathStyle = FormattableString.Invariant($"{GetCircleColor()};transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {CircleDash * Percent / 100}px, {CircleDash}px; stroke-dashoffset: 0px;");
                }
                else
                {
                    _circlePathStyle = FormattableString.Invariant($"{GetCircleColor()};transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {CircleDash * (Percent - SuccessPercent) / 100}px, {CircleDash}px; stroke-dashoffset: 0px;");
                    _circleSuccessStyle = FormattableString.Invariant($"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {CircleDash * SuccessPercent / 100}px, {CircleDash}px; stroke-dashoffset: {-CircleDash * SuccessPercent / 100}px;");
                }
            }
            else
            {
                _bgStyle = Size == ProgressSize.Default ? $"width: 120px; height: 120px; font-size: 24px;" : $"width: 80px; height: 80px; font-size: 18px;";
                double circumference = CircleDash - GapDegree;
                double dashoffset = -GapDegree / 2.0;
                _circleTrailStyle = FormattableString.Invariant($"stroke:{TrailColor}; transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {circumference}px, {CircleDash}px; stroke-dashoffset: {dashoffset}px;");
                if (SuccessPercent == 0)
                {
                    _circlePathStyle = FormattableString.Invariant($"{GetCircleColor()};transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {circumference * Percent / 100}px, {CircleDash}px; stroke-dashoffset: {dashoffset}px;");
                }
                else
                {
                    _circlePathStyle = FormattableString.Invariant($"{GetCircleColor()};transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {circumference * (Percent - SuccessPercent) / 100}px, {CircleDash}px; stroke-dashoffset: {dashoffset}px;");
                    _circleSuccessStyle = FormattableString.Invariant($"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {circumference * SuccessPercent / 100}px, {CircleDash}px; stroke-dashoffset: {dashoffset - circumference * SuccessPercent / 100}px;");
                }
            }
        }

        private string GetCircleColor()
        {
            var baseColor = string.Empty;
            if (StrokeColor.Value == null)
            {
                return baseColor;
            }

            var style = new StringBuilder(baseColor);

            if (StrokeColor.IsT1)
            {
                try
                {
                    var gradientPoints = string.Join(", ", StrokeColor.AsT1.Select(pair => $"{ToRGB(pair.Value)} {pair.Key}"));
                    style.Append($" stroke: linear-gradient(to right, {gradientPoints})");
                }
                catch
                {
                    throw new ArgumentOutOfRangeException($"{nameof(StrokeColor)}'s value must be dictionary like \"0%\": \"#108ee9\", \"100%\": \"#87d068\"");
                }
            }
            else if (StrokeColor.IsT0)
            {
                style.Append("stroke: ");
                style.Append(ToRGB(StrokeColor.AsT0));
                style.Append(';');
            }

            return style.ToString();
        }

        private string GetLineBGStyle()
        {
            var baseStyle = FormattableString.Invariant($"{(StrokeLinecap == ProgressStrokeLinecap.Round ? string.Empty : "border-radius: 0px; ")}width: {Percent}%; height: {StrokeWidth}px;");

            if (StrokeColor.Value == null)
            {
                return baseStyle;
            }

            var style = new StringBuilder(baseStyle);

            // width: 99.9%; height: 8px; background-image: linear-gradient(to right, rgb(16, 142, 233) 0%, rgb(135, 208, 104) 100%);
            // width: 99.9%; height: 8px; background-image: linear-gradient(to right, rgb(16, 142, 233), rgb(135, 208, 104));
            // '0%': '#108ee9', '100%': '#87d068',
            if (StrokeColor.IsT1)
            {
                try
                {
                    var gradientPoints = string.Join(", ", StrokeColor.AsT1.Select(pair => $"{ToRGB(pair.Value)} {pair.Key}"));
                    style.Append($" background-image: linear-gradient(to right, {gradientPoints})");
                }
                catch
                {
                    throw new ArgumentOutOfRangeException($"{nameof(StrokeColor)}'s value must be dictionary like \"0%\": \"#108ee9\", \"100%\": \"#87d068\"");
                }
            }
            else if (StrokeColor.IsT0)
            {
                style.Append("background: ");
                style.Append(ToRGB(StrokeColor.AsT0));
                style.Append(';');
            }

            return style.ToString();
        }

#if NET7_0_OR_GREATER
        [GeneratedRegex(@"^#(?<r>[a-fA-F0-9]{2})(?<g>[a-fA-F0-9]{2})(?<b>[a-fA-F0-9]{2})$")]
        private static partial Regex HexColor();
#else
        private static readonly Regex _hexColor = new(@"^#(?<r>[a-fA-F0-9]{2})(?<g>[a-fA-F0-9]{2})(?<b>[a-fA-F0-9]{2})$");
#endif

        private string ToRGB(string color)
        {
#if NET7_0_OR_GREATER
            var hexMatch = HexColor().Match(color);
#else
            var hexMatch = _hexColor.Match(color);
#endif
            if (!hexMatch.Success)
            {
                throw new ArgumentOutOfRangeException($"{nameof(StrokeColor)}'s value must be like \"#ffffff\"");
            }
            int ColFromHex(string groupName)
            {
                return Convert.ToInt32(hexMatch.Groups[groupName].Value, 16);
            }
            return $"rgb({ColFromHex("r")}, {ColFromHex("g")}, {ColFromHex("b")})";
        }
    }
}
