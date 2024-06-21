using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Divider : AntDomComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string Text { get; set; }

        [Parameter] public bool Plain { get; set; } = false;

        /// <summary>
        ///  'horizontal' | 'vertical'
        /// </summary>
        [Parameter] public DirectionVHType Type { get; set; } = DirectionVHType.Horizontal;

        /// <summary>
        /// 'left' | 'right' | 'center'
        /// </summary>
        [Parameter] public string Orientation { get; set; } = "center";

        [Parameter] public bool Dashed { get; set; } = false;

        private void SetClass()
        {
            var prefixCls = "ant-divider";
            var hashId = UseStyle(prefixCls);
            ClassMapper.Clear()
                .Add(prefixCls)
                .Add(hashId)
                .If(prefixCls, () => RTL)
                .Get(() => $"{prefixCls}-{this.Type.Name.ToLowerInvariant()}")
                .If($"{prefixCls}-with-text", () => Text != null || ChildContent != null)
                .GetIf(() => $"{prefixCls}-with-text-{this.Orientation.ToLowerInvariant()}", () => Text != null || ChildContent != null)
                .If($"{prefixCls}-plain", () => Plain && (Text != null || ChildContent != null))
                .If($"{prefixCls}-dashed", () => Dashed)
                ;
        }

        protected override void OnInitialized()
        {
            SetClass();
            base.OnInitialized();
        }
    }
}
