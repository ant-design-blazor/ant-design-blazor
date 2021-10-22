// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class MultiRangeGroup : AntDomComponentBase
    {
        private const string PreFixCls = "ant-multi-range-group";
        private List<MultiRangeSlider> _items = new();
        List<string> _keys = new();
        internal double _markSize = 0;

        /// <summary>
        /// Used for rendering select options manually.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Height is only used when <see cref="Vertical"/>. 
        /// Examples: "100px", "45%", "21vh"
        /// </summary>
        [Parameter]
        public string Height { get; set; }

        /// <summary>
        /// Tick mark of the `MultiRangeSlider`, type of key must 
        /// be number, and must in closed interval [min, max], 
        /// each mark can declare its own style.
        /// </summary>
        [Parameter]
        public RangeItemMark[] Marks { get; set; }

        /// <summary>
        /// Render the sliders with scale starting form left side 
        /// to right or from bottom towards top.
        /// </summary>
        [Parameter]
        public bool Reverse { get; set; }

        /// <summary>
        /// If true, the slider will be vertical.
        /// </summary>
        [Parameter]
        public bool Vertical { get; set; }

        internal bool IsFirst(MultiRangeSlider item)
        {
            if (_items.Count == 0)
            {
                return false;
            }
            return item.Id == _items[0].Id;
        }

        protected override void OnParametersSet()
        {
            ClassMapper.Clear()
                .Add(PreFixCls)
                .If($"{PreFixCls}-vertical", () => Vertical);

            base.OnParametersSet();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && _items.Count > 0)
            {
                var firstTrackDom = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _items.First()._railRef);
                var lastTrackDom = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _items.Last()._railRef);
                if (Vertical)
                {
                    _markSize = (lastTrackDom.AbsoluteLeft + lastTrackDom.ClientWidth) - firstTrackDom.AbsoluteLeft;
                }
                else
                {
                    _markSize = (lastTrackDom.AbsoluteTop + lastTrackDom.ClientHeight) - firstTrackDom.AbsoluteTop;
                }
                StateHasChanged();

            }
            await base.OnAfterRenderAsync(firstRender);
        }

        internal string GetMarkSizeStyle()
        {
            if (Vertical)
            {
                return $"width: {_markSize}px;";
            }
            return $"height: {_markSize}px;";
        }

        private string GetHeight()
        {
            if (Vertical && !string.IsNullOrWhiteSpace(Height))
            {
                return $"height: {Height};";
            }
            return "";
        }

        internal bool IsLast(MultiRangeSlider item)
        {
            if (_items.Count == 0)
            {
                return false;
            }
            return item.Id == _items.Last().Id;
        }

        internal void AddMultiRangeSliderItem(MultiRangeSlider item)
        {
            if (Marks is not null && Marks.Any())
            {
                item.SetMarksFromParent(Marks);
            }
            item.SetReverseFromParent(Reverse);
            item.SetVerticalFromParent(Vertical);

            _items.Add(item);
            if (_keys.Count < _items.Count)
            {
                _keys.Add(Guid.NewGuid().ToString());
            }
        }

        internal void RemoveMultiRangeSliderItem(MultiRangeSlider item)
        {
            int index = _items.IndexOf(item);
            if (index >= 0)
            {
                _items.RemoveAt(index);
                _keys.RemoveAt(index);
            }
        }

        private string GetOrAddKey(int index)
        {
            if (_keys.Count <= index)
            {
                string newKey = Guid.NewGuid().ToString();
                _keys.Add(newKey);
                return newKey;
            }
            return _keys[index];
        }
    }
}
