// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
            ClassMapper.Clear()
                .Add("ant-divider")
                .If("ant-divider", () => RTL)
                .Get(() => $"ant-divider-{this.Type.Name.ToLowerInvariant()}")
                .If("ant-divider-with-text", () => Text != null || ChildContent != null)
                .GetIf(() => $"ant-divider-with-text-{this.Orientation.ToLowerInvariant()}", () => Text != null || ChildContent != null)
                .If($"ant-divider-plain", () => Plain && (Text != null || ChildContent != null))
                .If("ant-divider-dashed", () => Dashed)
                ;
        }

        protected override void OnInitialized()
        {
            SetClass();
            base.OnInitialized();
        }
    }
}
