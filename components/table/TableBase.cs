// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class TableBase : AntDomComponentBase
    {
        [Parameter]
        public TableSize Size { get; set; }

        [Parameter]
        public bool Responsive { get; set; }

        [Parameter]
        public bool Bordered { get; set; }

        //[Parameter]
        //public bool FixedHeader { get; set; }

        [Parameter]
        public bool Loading { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected readonly ClassMapper WrapperClassMapper = new ClassMapper();

        protected override void OnInitialized()
        {
            string prefixCls = "ant-table";
            ClassMapper.Add(prefixCls)
                .Add($"{prefixCls}-simple")
                //.If($"{prefixCls}-fixed-header", () => FixedHeader)
                .If($"{prefixCls}-bordered", () => Bordered)
                .If($"{prefixCls}-small", () => Size == TableSize.Small)
                .If($"{prefixCls}-middle", () => Size == TableSize.Middle)
                .If($"{prefixCls}-rtl", () => RTL)
                ;

            WrapperClassMapper
                .Add($"{prefixCls}-wrapper")
                .If($"{prefixCls}-responsive", () => Responsive) // Not implemented in ant design
                .If($"{prefixCls}-wrapper-rtl", () => RTL);

            base.OnInitialized();
        }
    }
}
