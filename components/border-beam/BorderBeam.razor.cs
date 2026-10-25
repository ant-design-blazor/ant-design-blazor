using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

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
        /// <summary>Beam color. The trailing segment fades to transparent.</summary>
        [Parameter] public string Color { get; set; }

        private string GetBeamStyle(int index)
        {
            var values = new List<string>();
            if (!string.IsNullOrWhiteSpace(Style)) values.Add(Style.TrimEnd(';'));
            if (!string.IsNullOrWhiteSpace(LineWidth)) values.Add($"--ant-border-beam-line-width:{LineWidth}");
            if (!string.IsNullOrWhiteSpace(Size)) values.Add($"--ant-border-beam-size:{Size}");
            if (!string.IsNullOrWhiteSpace(Color)) values.Add($"--ant-border-beam-beam-gradient:linear-gradient(to left,{Color} 0%,{Color} 70%,transparent)");
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
            if (string.IsNullOrWhiteSpace(Outset))
                await JsInvokeAsync("AntDesign.interop.borderBeamHelper.update", Ref);
        }
    }
}
