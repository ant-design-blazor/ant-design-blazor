// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    /// <summary>
    /// 
    /// <para>
    /// Add specific text or patterns to the page.
    /// </para>
    /// 
    /// <h2>When To Use</h2>
    /// 
    /// <list type="bullet">
    /// <item>Use when the page needs to be watermarked to identify the copyright.</item>
    /// <item>Suitable for preventing information theft.</item>
    /// </list>
    /// </summary>
    [Documentation(DocumentationCategory.Components, DocumentationType.Feedback, "https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*wr1ISY50SyYAAAAAAAAAAAAADrJ8AQ/original", Columns = 1)]
    public partial class Watermark : AntDomComponentBase
    {
        /// <summary>
        /// The width of the watermark, the default value of <c>content</c> is its own width
        /// </summary>
        /// <default value="120"/>
        [Parameter] public int Width { get; set; } = 120;

        /// <summary>
        /// The height of the watermark, the default value of <c>content</c> is its own height
        /// </summary>
        /// <default value="64"/>
        [Parameter] public int Height { get; set; } = 64;

        /// <summary>
        /// When the watermark is drawn, the rotation Angle, unit <c>°</c>
        /// </summary>
        /// <default value="-22"/>
        [Parameter] public int Rotate { get; set; } = -22;

        /// <summary>
        /// transparency, value range [0-1]
        /// </summary>
        /// <default value="1f"/>
        [Parameter] public float Alpha { get; set; } = 1f;

        /// <summary>
        /// font size
        /// </summary>
        /// <default value="16"/>
        [Parameter] public int FontSize { get; set; } = 16;

        /// <summary>
        /// font color 
        /// </summary>
        /// <default value="rgba(0,0,0,.15)"/>
        [Parameter] public string FontColor { get; set; } = "rgba(0,0,0,.15)";

        /// <summary>
        /// font family
        /// </summary>
        /// <default value="sans-serif"/>
        [Parameter] public string FontFamily { get; set; } = "sans-serif";

        /// <summary>
        /// font weight. The value can be <c>normal</c>, <c>light</c>, <c>weight</c> and number. 
        /// </summary>
        /// <default value="normal"/>
        [Parameter] public string FontWeight { get; set; } = "normal";

        /// <summary>
        /// font style. The value can be <c>none</c>, <c>normal</c>, <c>italic</c> and <c>oblique</c>.
        /// </summary>
        /// <default value="normal"/>
        [Parameter] public string FontStyle { get; set; } = "normal";

        /// <summary>
        /// Watermark text content
        /// </summary>
        [Parameter] public string Content { get; set; }

        /// <summary>
        /// Multiple-line watermark text contents
        /// </summary>
        [Parameter] public string[] Contents { get; set; }

        /// <summary>
        /// The z-index of the appended watermark element.
        /// </summary>
        /// <default value="9"/>
        [Parameter] public int ZIndex { get; set; } = 9;

        /// <summary>
        /// The spacing between watermarks.
        /// <para>X: the x-axis spacing between watermarks</para>
        /// <para>Y: the y-axis spacing between watermarks</para>
        /// </summary>
        /// <default value="(100, 100)"/>
        [Parameter] public (int X, int Y) Gap { get; set; } = (100, 100);

        /// <summary>
        /// Image source, it is recommended to export 2x or 3x image, high priority (support base64 format)
        /// </summary>
        [Parameter] public string Image { get; set; }

        /// <summary>
        /// The offset of the watermark from the upper left corner of the container. The default is <c>gap/2</c>.
        /// </summary>
        /// <default value="(gap[0]/2, gap[1]/2)"/>
        [Parameter] public (int X, int Y) Offset { get; set; }

        /// <summary>
        /// The line spacing, only applies when there are multiple lines (content is configured as an array)
        /// </summary>
        /// <default value="16"/>
        [Parameter] public int LineSpace { get; set; } = 16;

        /// <summary>
        /// Enable infinite scrolling text effect
        /// </summary>
        [Parameter] public bool Scrolling { get; set; }

        /// <summary>
        /// The interval of motion displacement, in milliseconds.
        /// </summary>
        /// <default value="3000"/>
        [Parameter] public int ScrollingSpeed { get; set; } = 3000;

        /// <summary>
        /// set or get whether to repeat the watermark
        /// </summary>
        /// <default value="true"/>
        [Parameter] public bool Repeat { get; set; } = true;

        /// <summary>
        /// Whether the picture needs gray scale display (color to gray) 
        /// </summary>
        /// <default value="false"/>
        [Parameter] public bool Grayscale { get; set; } = false;

        /// <summary>
        /// The child Content
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        private string _backgroundImage;
        private ElementReference _watermarkContentRef;

        private string BackgroundRepeat => Scrolling ? "no-repeat" : Repeat ? "repeat" : "no-repeat";

        private bool _haveRrender;

        private async Task GenerateBase64Url()
        {
            if (!_haveRrender)
            {
                return;
            }

            var options = new
            {
                alpha = Alpha,
                width = Width,
                height = Height,
                rotate = Scrolling ? 0 : Rotate,
                lineSpace = LineSpace,
                gapX = Scrolling ? 0 : Gap.X,
                gapY = Scrolling ? 0 : Gap.Y,
                offsetLeft = Scrolling ? 0 : Math.Max(Offset.X, Gap.X / 2m),
                offsetTop = Scrolling ? 0 : Math.Max(Offset.Y, Gap.Y / 2m),
                watermarkContent = !string.IsNullOrEmpty(Image) ?
                (object)new
                {
                    url = Image,
                    isGrayscale = Grayscale,
                } :
                Contents is { Length: > 0 } ?
                Contents.Select(text => new
                {
                    fontColor = FontColor,
                    fontSize = FontSize,
                    fontWeight = FontWeight,
                    fontStyle = FontStyle,
                    text = text,
                    fontFamily = FontFamily,
                }) :
                new
                {
                    fontColor = FontColor,
                    fontSize = FontSize,
                    fontWeight = FontWeight,
                    text = Content,
                    fontFamily = FontFamily,
                }
            };

            await Js.InvokeAsync<string>(JSInteropConstants.WatermarkHelper.GenerateBase64Url, options, DotNetObjectReference.Create(this), _watermarkContentRef, Ref);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var contentChanged = parameters.IsParameterChanged(nameof(Content), Content)
                || parameters.IsParameterChanged(nameof(Contents), Contents)
                || parameters.IsParameterChanged(nameof(Image), Image);

            await base.SetParametersAsync(parameters);

            if (contentChanged)
            {
                await GenerateBase64Url();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _haveRrender = true;
                await GenerateBase64Url();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private static string RandomMovingStyle()
        {
            var random = new Random();
            var align = Math.Floor(random.NextDouble() * 4);
            var p1 = Math.Floor(random.NextDouble() * 70) + 30;
            var leftTopLimit = 0;
            var bottomLimit = 95;
            var rightLimit = 90;
            var keyframesStyle = $$$"""
                @keyframes watermark {
                    0% {
                        left: {{{(align == 1 ? rightLimit : align == 3 ? leftTopLimit : p1)}}}%;
                        top: {{{(align == 0 ? leftTopLimit : align == 2 ? bottomLimit : p1)}}}%;
                    }
                    25% {
                        left: {{{(align == 0 ? rightLimit : align == 2 ? leftTopLimit : 100 - p1)}}}%;
                        top: {{{(align == 1 ? bottomLimit : align == 3 ? leftTopLimit : p1)}}}%;
                    }
                    50% {
                        left: {{{(align == 1 ? leftTopLimit : align == 3 ? rightLimit : 100 - p1)}}}%;
                        top: {{{(align == 0 ? bottomLimit : align == 2 ? leftTopLimit : 100 - p1)}}}%;
                    }
                    75% {
                        left: {{{(align == 0 ? leftTopLimit : align == 2 ? rightLimit : p1)}}}%;
                        top: {{{(align == 1 ? leftTopLimit : align == 3 ? bottomLimit : 100 - p1)}}}%;
                    }
                    100% {
                        left: {{{(align == 1 ? rightLimit : align == 3 ? leftTopLimit : p1)}}}%;
                        top: {{{(align == 0 ? leftTopLimit : align == 2 ? bottomLimit : p1)}}}%;
                    }
                }
                """;
            return keyframesStyle;
        }

        [JSInvokable("load")]
        public void Load(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                _backgroundImage = url;
                InvokeStateHasChanged();
            }
        }
    }
}
