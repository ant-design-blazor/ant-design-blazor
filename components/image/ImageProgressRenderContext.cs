using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>Context passed to <see cref="Image.ProgressRender"/>.</summary>
    public sealed class ImageProgressRenderContext
    {
        /// <summary>The normalized progress percentage, rounded and clamped to 0-100.</summary>
        public int Percent { get; set; }

        /// <summary>Whether a determinate percentage was supplied.</summary>
        public bool HasPercent { get; set; }

        /// <summary>The default progress rail and percentage indicator.</summary>
        public RenderFragment DefaultProgress { get; set; }
    }
}
