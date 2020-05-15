using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SmartEnum;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class Progress : AntDomComponentBase
    {
        private const string PrefixCls = "ant-progress";
        private const double CircleDash = 295.31;
        private string _bgStyle;
        private string _circleTrailStyle;
        private string _circlePathStyle;

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
        public Func<int, string> Format { get; set; } = (i) => i + "%";

        /// <summary>
        /// to set the completion percentage
        /// </summary>
        [Parameter]
        public int Percent { get; set; }

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
        /// color of progress bar
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// segmented success percent
        /// </summary>
        [Parameter]
        public int SuccessPercent { get; set; }

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
        public string StrokeColor { get; set; }

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
        public int Width { get; set; }

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
            SetDefaultValues(parameters.ToDictionary());
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
                Width = 132;
                StrokeWidth = 6;
            }

            if (dict.TryGetValue(nameof(Percent), out object percent) && (int)percent == 100)
            {
                Status = ProgressStatus.Success;
            }
        }

        private void SetClasses()
        {
            ClassMapper.Clear()
                .Add(PrefixCls)
                .Add($"{PrefixCls}-{Size.Name}")
                .Add($"{PrefixCls}-{Type.Name}")
                .If($"{PrefixCls}-status-{Status.Name}", () => Status != null)
                .If($"{PrefixCls}-show-info", () => ShowInfo);
        }

        private void SetStyle()
        {
            if (Type == ProgressType.Line)
            {
                _bgStyle = $"width: {Percent}%; height: {(Size == ProgressSize.Default ? 8 : 6)}px;";
            }
            else if (Type == ProgressType.Circle)
            {
                _bgStyle = Size == ProgressSize.Default ? $"width: 120px; height: 120px; font-size: 24px;" : $"width: 80px; height: 80px; font-size: 18px;";
                _circleTrailStyle = $"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {CircleDash}px, {CircleDash}px; stroke-dashoffset: 0px;";
                _circlePathStyle = $"transition:stroke-dashoffset 0.3s, stroke-dasharray 0.3s, stroke 0.3s, stroke-width 0.06s 0.3s; stroke-dasharray: {CircleDash * Percent / 100}px, {CircleDash}px; stroke-dashoffset: 0px;";
            }
        }
    }
}
