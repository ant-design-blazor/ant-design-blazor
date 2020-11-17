using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Progress : AntDomComponentBase
    {
        private const string PrefixCls = "ant-progress";
        private const double CircleDash = 295.31;
        private string _bgStyle;
        private string _bgSuccessStyle;
        private string _circleTrailStyle;
        private string _circlePathStyle;
        private string _circleSuccessStyle;
        private bool _format = false;

        #region Parameters

        /// <summary>
        /// progress size
        /// </summary>
        [Parameter]
        public ProgressSize Size { get; set; } = ProgressSize.Default;

        /// <summary>
        /// to set the type, options: line circle dashboard
        /// </summary>
        [Parameter]
        public ProgressType Type { get; set; } = ProgressType.Line;

        /// <summary>
        /// template function of the content
        /// </summary>
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
        [Parameter]
        public bool ShowInfo { get; set; } = true;

        /// <summary>
        /// to set the status of the Progress, options: success exception normal active(line only)
        /// </summary>
        [Parameter]
        public ProgressStatus Status { get; set; } = ProgressStatus.Normal;

        /// <summary>
        /// to set the style of the progress linecap
        /// </summary>
        [Parameter]
        public ProgressStrokeLinecap StrokeLinecap { get; set; } = ProgressStrokeLinecap.Round;

        /// <summary>
        /// segmented success percent
        /// </summary>
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
        [Parameter]
        public int Steps { get; set; }

        /// <summary>
        /// to set the canvas width of the circular progress, unit: px
        /// to set the canvas width of the dashboard progress, unit: px
        /// </summary>
        [Parameter]
        public int Width { get; set; } = 120;

        /// <summary>
        /// the gap degree of half circle, 0 ~ 295
        /// </summary>
        [Parameter]
        public int GapDegree { get; set; } = 75;

        /// <summary>
        /// the gap position, options: top bottom left right
        /// </summary>
        [Parameter]
        public ProgressGapPosition GapPosition { get; set; } = ProgressGapPosition.Bottom;

        #endregion

        public async override Task SetParametersAsync(ParameterView parameters)
        {
            IReadOnlyDictionary<string, object> dict = parameters.ToDictionary();
            SetDefaultValues(dict);
            await base.SetParametersAsync(parameters);

            SetClasses();
            SetStyle();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private void SetDefaultValues(IReadOnlyDictionary<string, object> dict)
        {
            if (!dict.ContainsKey(nameof(Type)) || (ProgressType)dict[nameof(Type)] == ProgressType.Line)
            {
                StrokeWidth = 10;
            }
            else // Type is Circle or Dashboard
            {
                StrokeWidth = 6;
            }

            if (dict.TryGetValue(nameof(Percent), out object percent) && (double)percent == 100)
            {
                Status = ProgressStatus.Success;
            }
            else
            {
                Status = ProgressStatus.Normal;
            }

            _format = dict.ContainsKey(nameof(Format));
        }

        private void SetClasses()
        {
            ClassMapper.Clear()
                .Add(PrefixCls)
                .Add($"{PrefixCls}-{Size.Name}")
                .If($"{PrefixCls}-{Type.Name}", () => Type != ProgressType.Dashboard)
                .If($"{PrefixCls}-{ProgressType.Circle.Name}", () => Type == ProgressType.Dashboard)
                .If($"{PrefixCls}-status-{Status.Name}", () => Status != null)
                .If($"{PrefixCls}-show-info", () => ShowInfo)
                .If($"{PrefixCls}-steps", () => Steps > 0);
        }

        private void SetStyle()
        {
            if (Type == ProgressType.Line)
            {
                _bgStyle = GetLineBGStyle();
                if (SuccessPercent != 0)
                {
                    _bgSuccessStyle = $"width: {SuccessPercent}%; height: {StrokeWidth}px;";
                }
            }
            else if (Type == ProgressType.Circle)
            {
                _bgStyle = Size == ProgressSize.Default ? $"width: {Width}px; height: {Width}px; font-size: 24px;" : $"width: 80px; height: 80px; font-size: 18px;";
                _circleTrailStyle = $"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {CircleDash}px, {CircleDash}px; stroke-dashoffset: 0px;";
                if (SuccessPercent == 0)
                {
                    _circlePathStyle = $"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {CircleDash * Percent / 100}px, {CircleDash}px; stroke-dashoffset: 0px;";
                }
                else
                {
                    _circlePathStyle = $"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {CircleDash * (Percent - SuccessPercent) / 100}px, {CircleDash}px; stroke-dashoffset: 0px;";
                    _circleSuccessStyle = $"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {CircleDash * SuccessPercent / 100}px, {CircleDash}px; stroke-dashoffset: {-CircleDash * SuccessPercent / 100}px;";
                }
            }
            else
            {
                _bgStyle = Size == ProgressSize.Default ? $"width: 120px; height: 120px; font-size: 24px;" : $"width: 80px; height: 80px; font-size: 18px;";
                double circumference = CircleDash - GapDegree;
                double dashoffset = -GapDegree / 2.0;
                _circleTrailStyle = $"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {circumference}px, {CircleDash}px; stroke-dashoffset: {dashoffset}px;";
                if (SuccessPercent == 0)
                {
                    _circlePathStyle = $"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {circumference * Percent / 100}px, {CircleDash}px; stroke-dashoffset: {dashoffset}px;";
                }
                else
                {
                    _circlePathStyle = $"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {circumference * (Percent - SuccessPercent) / 100}px, {CircleDash}px; stroke-dashoffset: {dashoffset}px;";
                    _circleSuccessStyle = $"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {circumference * SuccessPercent / 100}px, {CircleDash}px; stroke-dashoffset: {dashoffset - circumference * SuccessPercent / 100}px;";
                }
            }
        }

        private string GetLineBGStyle()
        {
            string style = $"{(StrokeLinecap == ProgressStrokeLinecap.Round ? string.Empty : "border-radius: 0px; ")}width: {Percent}%; height: {StrokeWidth}px;";

            if (StrokeColor.Value == null)
            {
                return style;
            }

            // width: 99.9%; height: 8px; background-image: linear-gradient(to right, rgb(16, 142, 233) 0%, rgb(135, 208, 104) 100%);
            // width: 99.9%; height: 8px; background-image: linear-gradient(to right, rgb(16, 142, 233), rgb(135, 208, 104));
            // '0%': '#108ee9', '100%': '#87d068',
            if (StrokeColor.IsT1)
            {
                try
                {
                    StringBuilder gradientBuilder = new StringBuilder(" background-image: linear-gradient(to right,");
                    foreach (var pair in StrokeColor.AsT1)
                    {
                        gradientBuilder.Append($" {ToRGB(pair.Value)} {pair.Key},");
                    }
                    style += gradientBuilder.ToString().TrimEnd(',') + ");";
                }
                catch { }
            }
            else if (StrokeColor.IsT0)
            {
                style += $"background: {ToRGB(StrokeColor.AsT0)};";
            }

            return style;
        }

        private string GetCircleBGStyle()
        {
            throw new NotImplementedException();
        }

        private string ToRGB(string color)
        {
            string rgb = "rgb(";
            if (!color.StartsWith('#') && color.Length == 7)
            {
                throw new ArgumentOutOfRangeException(nameof(StrokeColor) + "'s value must be like \"#ffffff\"");
            }

            color = color.TrimStart('#');
            for (int i = 0; i < 3; i++)
            {
                int num = Convert.ToInt32(color.Substring(i * 2, 2), 16);
                rgb += num + ", ";
            }

            return rgb.TrimEnd(' ', ',') + ")";
        }
    }
}
