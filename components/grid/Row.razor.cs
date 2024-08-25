﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /*
     * Possible values and meaning
     * int                                                  - horizontal gutter
     * Dictionary<string, int>                              - horizontal gutters for different screen sizes
     * (int, int)                                           - horizontal gutter, vertical gutter
     * (Dictionary<string, int>, int)                       - horizontal gutters for different screen sizes, vertical gutter
     * (int, Dictionary<string, int>)                       - horizontal gutter, vertical gutter for different screen sizes
     * (Dictionary<string, int>, Dictionary<string, int>)   - horizontal gutters for different screen sizes, vertical gutter for different screen sizes
     */
    using GutterType = OneOf<int, Dictionary<string, int>, (int, int), (Dictionary<string, int>, int), (int, Dictionary<string, int>), (Dictionary<string, int>, Dictionary<string, int>)>;

    public partial class Row : AntDomComponentBase
    {
        /// <summary>
        /// Content of the row, generally contains <see cref="Col"/> elements.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Currently unused
        /// </summary>
        [Parameter]
        public string Type { get; set; }

        /// <summary>
        /// Vertical alignment for the flex layout: 'top' | 'middle' | 'bottom'
        /// </summary>
        [Parameter]
        public string Align { get; set; }

        /// <summary>
        /// Hotizontal alignment for the flex layout: 'start' | 'end' | 'center' | 'space-around' | 'space-between'
        /// </summary>
        [Parameter]
        public string Justify { get; set; }

        /// <summary>
        /// Allow the row's content to wrap or not
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Wrap { get; set; } = true;

        /// <summary>
        /// Spacing between grids, could be a number or a dictionary like 
        /// <c>{ xs: 8, sm: 16, md: 24 }</c>, an array to make horizontal and vertical spacing work at the same time <c>[horizontal, vertical]</c>
        /// </summary>
        [Parameter]
        public GutterType Gutter
        {
            get => _gutter;
            set
            {
                _gutter = value.Match<GutterType>(
                    @int => @int,
                    dict => new Dictionary<string, int>(dict, StringComparer.OrdinalIgnoreCase),
                    tuple => tuple,
                    tupleDictInt => (new Dictionary<string, int>(tupleDictInt.Item1, StringComparer.OrdinalIgnoreCase), tupleDictInt.Item2),
                    tupleIntDict => (tupleIntDict.Item1, new Dictionary<string, int>(tupleIntDict.Item2, StringComparer.OrdinalIgnoreCase)),
                    tupleDictDict => (new Dictionary<string, int>(tupleDictDict.Item1, StringComparer.OrdinalIgnoreCase), new Dictionary<string, int>(tupleDictDict.Item2, StringComparer.OrdinalIgnoreCase))
                    );

                if (_currentBreakPoint != null)
                {
                    SetGutterStyle(_currentBreakPoint);
                }
            }
        }

        /// <summary>
        /// Callback executed when a screen size breakpoint is triggered
        /// </summary>
        [Parameter]
        public EventCallback<BreakpointType> OnBreakpoint { get; set; }

        /// <summary>
        /// Default screen size breakpoint. Used to set gutter during pre-rendering
        /// </summary>
        [Parameter]
        public BreakpointType? DefaultBreakpoint { get; set; } = BreakpointType.Xxl;

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        private string _gutterStyle;
        private BreakpointType? _currentBreakPoint;
        private GutterType _gutter;

        private IList<Col> _cols = new List<Col>();

        private static BreakpointType[] _breakpoints = new[] {
            BreakpointType.Xs,
            BreakpointType.Sm,
            BreakpointType.Md,
            BreakpointType.Lg,
            BreakpointType.Xl,
            BreakpointType.Xxl
        };

        protected override async Task OnInitializedAsync()
        {
            var prefixCls = "ant-row";
            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-top", () => Align == "top")
                .If($"{prefixCls}-middle", () => Align == "middle")
                .If($"{prefixCls}-bottom", () => Align == "bottom")
                .If($"{prefixCls}-start", () => Justify == "start")
                .If($"{prefixCls}-end", () => Justify == "end")
                .If($"{prefixCls}-center", () => Justify == "center")
                .If($"{prefixCls}-space-around", () => Justify == "space-around")
                .If($"{prefixCls}-space-between", () => Justify == "space-between")
                .If($"{prefixCls}-no-wrap", () => !Wrap)
                .If($"{prefixCls}-rtl", () => RTL)
                ;

            if (DefaultBreakpoint != null)
            {
                SetGutterStyle(DefaultBreakpoint);
            }

            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var dimensions = await JsInvokeAsync<Window>(JSInteropConstants.GetWindow);
                DomEventListener.AddShared<Window>("window", "resize", OnResize);
                OptimizeSize(dimensions.InnerWidth);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        internal void AddCol(Col col)
        {
            this._cols.Add(col);
            var gutter = this.GetGutter(_currentBreakPoint ?? DefaultBreakpoint);
            col.RowGutterChanged(gutter);
        }

        internal void RemoveCol(Col col)
        {
            this._cols.Remove(col);
        }

        private void OnResize(Window window)
        {
            OptimizeSize(window.InnerWidth);
        }

        private void OptimizeSize(decimal windowWidth)
        {
            BreakpointType actualBreakpoint = _breakpoints[_breakpoints.Length - 1];
            for (int i = 0; i < _breakpoints.Length; i++)
            {
                if (windowWidth <= (int)_breakpoints[i] && (windowWidth >= (i > 0 ? (int)_breakpoints[i - 1] : 0)))
                {
                    actualBreakpoint = _breakpoints[i];
                }
            }

            this._currentBreakPoint = actualBreakpoint;

            SetGutterStyle(actualBreakpoint);

            if (OnBreakpoint.HasDelegate)
            {
                OnBreakpoint.InvokeAsync(actualBreakpoint);
            }

            StateHasChanged();
        }

        private void SetGutterStyle(BreakpointType? breakPoint)
        {
            var gutter = this.GetGutter(breakPoint);
            _cols.ForEach(x => x.RowGutterChanged(gutter));

            _gutterStyle = "";
            if (gutter.horizontalGutter > 0)
            {
                _gutterStyle = $"margin-left: -{gutter.horizontalGutter / 2}px; margin-right: -{gutter.horizontalGutter / 2}px; ";
            }
            _gutterStyle += $"row-gap: {gutter.verticalGutter}px; ";

            StateHasChanged();
        }

        private (int horizontalGutter, int verticalGutter) GetGutter(BreakpointType? breakPoint)
        {
            var breakPointName = Enum.GetName(typeof(BreakpointType), breakPoint);

            return _gutter.Match(
                num => (num, 0),
                dic => dic.ContainsKey(breakPointName) ? (dic[breakPointName], 0) : (0, 0),
                tuple => tuple,
                tupleDicInt => (tupleDicInt.Item1.ContainsKey(breakPointName) ? tupleDicInt.Item1[breakPointName] : 0, tupleDicInt.Item2),
                tupleIntDic => (tupleIntDic.Item1, tupleIntDic.Item2.ContainsKey(breakPointName) ? tupleIntDic.Item2[breakPointName] : 0),
                tupleDicDic => (tupleDicDic.Item1.ContainsKey(breakPointName) ? tupleDicDic.Item1[breakPointName] : 0, tupleDicDic.Item2.ContainsKey(breakPointName) ? tupleDicDic.Item2[breakPointName] : 0)
            );
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener?.Dispose();
            base.Dispose(disposing);
        }
    }
}
