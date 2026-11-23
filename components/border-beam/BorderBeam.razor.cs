using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// Animated border decoration using the v6 BorderBeam styles. Render directly
    /// inside a positioned host, for example in <see cref="Card.Decoration"/>.
    /// Respects the stylesheet's reduced-motion and browser-support fallbacks.
    /// </summary>
    public partial class BorderBeam : AntDomComponentBase
    {
        /// <summary>Number of evenly spaced beams.</summary>
        [Parameter] public int Count { get; set; } = 1;
        /// <summary>Seconds for one circuit. Defaults to six.</summary>
        [Parameter] public double? Duration { get; set; }
        /// <summary>Border thickness, such as 2px.</summary>
        [Parameter] public string LineWidth { get; set; }
        /// <summary>Beam size, such as 100px.</summary>
        [Parameter] public string Size { get; set; }
        /// <summary>Outward offset. Defaults to the host's computed border widths.</summary>
        [Parameter] public string Outset { get; set; }
        /// <summary>A solid beam color or gradient color stops. The trailing segment fades to transparent.</summary>
        [Parameter] public OneOf<string, IReadOnlyList<BorderBeamGradientStop>> Color { get; set; }

        private string GetGradient()
        {
            IReadOnlyList<BorderBeamGradientStop> stops;
            if (Color.IsT0)
            {
                var solidColor = Color.AsT0;
                stops = string.IsNullOrWhiteSpace(solidColor)
                    ? Array.Empty<BorderBeamGradientStop>()
                    : new[] { new BorderBeamGradientStop { Color = solidColor, Percent = 0 } };
            }
            else if (Color.IsT1)
            {
                stops = Color.AsT1 ?? Array.Empty<BorderBeamGradientStop>();
            }
            else
            {
                stops = Array.Empty<BorderBeamGradientStop>();
            }

            var validStops = stops
                .Where(stop => stop != null && !string.IsNullOrWhiteSpace(stop.Color))
                .Select(stop => new BorderBeamGradientStop
                {
                    Color = stop.Color,
                    Percent = Math.Clamp(double.IsFinite(stop.Percent) ? stop.Percent : 0, 0, 100)
                })
                .ToList();
            if (validStops.Count == 0) return null;

            if (validStops[^1].Percent < 100)
            {
                validStops.Add(new BorderBeamGradientStop { Color = validStops[^1].Color, Percent = 100 });
            }

            var gradientStops = validStops.Select(stop =>
            {
                var percent = (stop.Percent * 0.7).ToString("0.##", CultureInfo.InvariantCulture);
                return $"{stop.Color} {percent}%";
            });
            return $"linear-gradient(to left,{string.Join(", ", gradientStops)}, transparent)";
        }

        private string GetBeamStyle(int index)
        {
            var values = new List<string>();
            if (!string.IsNullOrWhiteSpace(Style)) values.Add(Style.TrimEnd(';'));
            if (!string.IsNullOrWhiteSpace(LineWidth)) values.Add($"--ant-border-beam-line-width:{LineWidth}");
            if (!string.IsNullOrWhiteSpace(Size)) values.Add($"--ant-border-beam-size:{Size}");
            var gradient = GetGradient();
            if (!string.IsNullOrWhiteSpace(gradient)) values.Add($"--ant-border-beam-beam-gradient:{gradient}");
            var duration = Duration.HasValue && Duration.Value > 0 && !double.IsInfinity(Duration.Value) ? Duration.Value : 6;
            if (Duration.HasValue && Duration.Value > 0 && !double.IsInfinity(Duration.Value))
                values.Add($"--ant-border-beam-duration:{duration.ToString(CultureInfo.InvariantCulture)}s");
            if (index > 0) values.Add($"--ant-border-beam-delay:{(-duration * index / Math.Max(1, Count)).ToString(CultureInfo.InvariantCulture)}s");
            if (!string.IsNullOrWhiteSpace(Outset)) values.Add($"--ant-border-beam-inset-offset:calc(-1 * {Outset})");
            return string.Join(";", values);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await JsInvokeAsync("AntDesign.interop.borderBeamHelper.update", Ref);
        }
    }
}
