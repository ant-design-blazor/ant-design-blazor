// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Empty : AntDomComponentBase
    {
        [Parameter]
        public string PrefixCls { get; set; } = "ant-empty";

        [Parameter]
        public string ImageStyle { get; set; }

        [Parameter]
        public bool Small { get; set; }

        [Parameter]
        public bool Simple { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public OneOf<string, bool?> Description { get; set; } = LocaleProvider.CurrentLocale.Empty.Description;

        [Parameter]
        public RenderFragment DescriptionTemplate { get; set; }

        [Parameter]
        public string Image { get; set; }

        [Parameter]
        public RenderFragment ImageTemplate { get; set; }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-normal", () => Simple)
                .If($"{PrefixCls}-small", () => Small)
                .If($"{PrefixCls}-rtl", () => RTL)
                ;
        }

        protected override void OnInitialized()
        {
            this.SetClass();
        }
    }
}
