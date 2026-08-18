// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// Adds a decorative beam that continuously moves around a container border.
    /// </summary>
    /// <remarks>
    /// BorderBeam is a visual decoration for cards, panels, call-to-action areas,
    /// and other content containers. It should not be used as a replacement for
    /// focus, validation, or status indication styles.
    /// </remarks>
    [Documentation(DocumentationCategory.Components, DocumentationType.Other,
        "https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*uae3QbkNCm8AAAAAAAAAAAAADrJ8AQ/original",
        Columns = 2, Title = "BorderBeam", SubTitle = "边框流光")]
    public partial class BorderBeam : AntDomComponentBase
    {
        internal const double DefaultDuration = 6;
        internal const double MaxColorStopPercent = 70;

        private BorderInfo _borderInfo = new BorderInfo();
        private bool _needsMount;
        private bool _hasMounted;

        /// <summary>Custom prefix class for the beam effect.</summary>
        /// <default value="ant-border-beam" />
        [Parameter] public string PrefixCls { get; set; } = "ant-border-beam";

        /// <summary>Additional class name for the beam effect.</summary>
        [Parameter] public string ClassName { get; set; }

        /// <summary>
        /// Beam color, either a CSS color or a collection of gradient stops.
        /// </summary>
        [Parameter] public OneOf<string, BorderBeamGradient[]> Color { get; set; }

        /// <summary>Number of beams.</summary>
        /// <default value="1" />
        [Parameter] public double Count { get; set; } = 1;

        /// <summary>Time in seconds for a beam to complete one loop.</summary>
        /// <default value="6" />
        [Parameter] public double Duration { get; set; }

        /// <summary>Beam line width. Numeric values are treated as pixels.</summary>
        /// <default value="1px" />
        [Parameter] public object LineWidth { get; set; }

        /// <summary>Distance to outset the beam layer from the container edge.</summary>
        [Parameter] public object Outset { get; set; }

        /// <summary>Size of the visible beam segment. Numeric values are treated as pixels.</summary>
        /// <default value="100px" />
        [Parameter] public object Size { get; set; }

        /// <summary>Content that hosts the beam effect.</summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        private int MergedCount => IsFinite(Count) && Count >= 1 ? Math.Max(1, (int)Math.Floor(Count)) : 1;

        private double MergedDuration => IsFinite(Duration) && Duration > 0 ? Duration : DefaultDuration;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ClassMapper.Add(PrefixCls);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            _needsMount = _hasMounted;
            await base.SetParametersAsync(parameters);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender || _needsMount)
            {
                _needsMount = false;

                try
                {
                    var info = await JsInvokeAsync<BorderInfo>(
                        JSInteropConstants.BorderBeamHelper.Mount, Ref);

                    if (info != null)
                    {
                        _borderInfo = info;
                        if (!_hasMounted)
                        {
                            _hasMounted = true;
                            await InvokeStateHasChangedAsync();
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    // JS interop is unavailable during prerendering.
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private string GetBeamStyle(int index)
        {
            var style = new StringBuilder();
            var gradient = GetGradient();
            var duration = MergedDuration;
            var inset = Outset != null ? GetInset(Outset) : GetDefaultInset();

            style.Append("--ant-border-beam-inset-offset:").Append(inset).Append(';');
            style.Append("--ant-border-beam-border-radius:").Append(_borderInfo.BorderRadius ?? "0px").Append(';');

            if (gradient != null)
                style.Append("--ant-border-beam-beam-gradient:").Append(gradient).Append(';');
            if (IsFinite(Duration) && Duration > 0)
                style.Append("--ant-border-beam-duration:").Append(ToCssNumber(Duration)).Append("s;");
            if (LineWidth != null)
                style.Append("--ant-border-beam-line-width:").Append(GetUnit(LineWidth)).Append(';');
            if (Size != null)
                style.Append("--ant-border-beam-size:").Append(GetUnit(Size)).Append(';');
            if (index > 0)
                style.Append("--ant-border-beam-delay:").Append(ToCssNumber(-duration * index / MergedCount)).Append("s;");

            return style.ToString();
        }

        private string GetDefaultInset()
        {
            var widths = _borderInfo.BorderWidth ?? new[] { 0d, 0d, 0d, 0d };
            return string.Join(" ", widths.Select(x => GetInset((object)x)));
        }

        private static string GetInset(object value)
        {
            if (IsNumber(value))
                return "-" + ToCssNumber(Convert.ToDouble(value, CultureInfo.InvariantCulture)) + "px";

            var text = Convert.ToString(value, CultureInfo.InvariantCulture);
            return string.IsNullOrWhiteSpace(text) ? "0px" : $"calc(-1 * {text})";
        }

        private static string GetUnit(object value)
        {
            if (IsNumber(value))
                return ToCssNumber(Convert.ToDouble(value, CultureInfo.InvariantCulture)) + "px";

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        private string GetGradient()
        {
            var stops = GetStops();
            if (stops.Count == 0)
                return null;

            if (stops[stops.Count - 1].Percent != 100)
            {
                var last = stops[stops.Count - 1];
                stops.Add(new BorderBeamGradient { Color = last.Color, Percent = 100 });
            }

            var mapped = stops.Select(x => $"{x.Color} {ToCssNumber(Math.Min(Math.Max(x.Percent, 0), 100) / 100 * MaxColorStopPercent)}%");
            return $"linear-gradient(to left, {string.Join(", ", mapped)}, transparent)";
        }

        private List<BorderBeamGradient> GetStops()
        {
            if (Color.IsT0)
            {
                var text = Color.AsT0;
                return string.IsNullOrWhiteSpace(text)
                    ? new List<BorderBeamGradient>()
                    : new List<BorderBeamGradient> { new BorderBeamGradient { Color = text, Percent = 0 } };
            }

            return Color.AsT1?.ToList() ?? new List<BorderBeamGradient>();
        }

        private static bool IsNumber(object value) => value is sbyte || value is byte || value is short || value is ushort
            || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal;

        private static bool IsFinite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);

        private static string ToCssNumber(double value) => value.ToString("0.##", CultureInfo.InvariantCulture);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    /// <summary>A color stop used by <see cref="BorderBeam"/>.</summary>
    public sealed class BorderBeamGradient
    {
        /// <summary>CSS color value for this gradient stop.</summary>
        public string Color { get; set; }

        /// <summary>Position of this gradient stop, from 0 to 100 percent.</summary>
        public double Percent { get; set; }
    }

    internal sealed class BorderInfo
    {
        public double[] BorderWidth { get; set; } = new[] { 0d, 0d, 0d, 0d };
        public string BorderRadius { get; set; } = "0px";
    }
}
