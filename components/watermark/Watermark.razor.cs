// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class Watermark : AntDomComponentBase
    {
        [Parameter] public int Width { get; set; } = 120;

        [Parameter] public int Height { get; set; } = 64;

        [Parameter] public int Rotate { get; set; } = -22;

        [Parameter] public float Alpha { get; set; } = 1f;

        [Parameter] public int FontSize { get; set; } = 14;

        [Parameter] public string FontColor { get; set; } = "rgba(0,0,0,.15)";

        [Parameter] public string FontFamily { get; set; } = "sans-serif";

        [Parameter] public string FontWeight { get; set; } = "normal";

        [Parameter] public string FontStyle { get; set; } = "normal";

        [Parameter] public string Content { get; set; }

        [Parameter] public string[] Contents { get; set; }

        [Parameter] public int ZIndex { get; set; } = 9;

        [Parameter] public (int X, int Y) Gap { get; set; } = (100, 100);

        [Parameter] public string Image { get; set; }

        [Parameter] public (int X, int Y) Offset { get; set; }

        [Parameter] public int LineSpace { get; set; } = 16;

        [Parameter] public bool Scrolling { get; set; }

        [Parameter] public int ScrollingSpeed { get; set; } = 3000;

        [Parameter] public bool Repeat { get; set; } = true;

        [Parameter] public bool Grayscale { get; set; } = false;

        [Parameter] public RenderFragment ChildContent { get; set; }

        private string _backgroundImage;
        private ElementReference _watermarkContentRef;

        private string BackgroundRepeat => Scrolling ? "no-repeat" : Repeat ? "repeat" : "no-repeat";

        private async Task GenerateBase64Url()
        {
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
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
