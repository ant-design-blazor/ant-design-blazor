// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class FloatButton : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment Icon { get; set; }

        [Parameter]
        public RenderFragment Description { get; set; }

        [Parameter]
        public RenderFragment Tooltip { get; set; }

        [Parameter]
        public string Type { get; set; } = "default";

        [Parameter]
        public string Shape { get; set; } = "circle";

        [Parameter]
        public EventCallback OnClick { get; set; }

        [Parameter]
        public string Href { get; set; }

        [Parameter]
        public string Target { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var prefixCls = "ant-float-btn";

            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-default", () => Type == "default")
                .If($"{prefixCls}-primary", () => Type == "primary")
                .If($"{prefixCls}-circle", () => Shape == "circle")
                .If($"{prefixCls}-square", () => Shape == "square");
        }

        private void HandleClick(MouseEventArgs args)
        {

        }
    }
}
