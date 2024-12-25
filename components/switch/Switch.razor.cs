// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /**
    <summary>
    <para>Switching Selector.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>If you need to represent the switching between two states or on-off state.</item>
        <item>The difference between `Switch` and `Checkbox` is that Switch will trigger a state change directly when you toggle it, while Checkbox is generally used for state marking, which should work in conjunction with submit operation.</item>
    </list>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/zNdJQMhfm/Switch.svg", Title = "Switch", SubTitle = "开关")]
    public partial class Switch : AntInputBoolComponentBase
    {
        /// <summary>
        /// Whether switch is loading or not. Will display spinner on handle when true.
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Loading { get; set; }

        /// <summary>
        /// String displayed inside switch when checked is true
        /// </summary>
        [Parameter]
        public string CheckedChildren { get; set; } = string.Empty;

        /// <summary>
        /// Content displayed inside switch when checked is true. Takes priority over <see cref="CheckedChildren"/>.
        /// </summary>
        [Parameter]
        public RenderFragment CheckedChildrenTemplate { get; set; }

        /// <summary>
        /// When true, the status of Switch no longer automatically changes the data based on the click event.
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Control { get; set; }

        /// <summary>
        /// Callback executed when the switch is clicked. When used in combination with <see cref="Control"/> it allows manually controlling the Switch.
        /// </summary>
        [Parameter]
        public EventCallback OnClick { get; set; }

        /// <summary>
        /// String displayed inside switch when checked is false.
        /// </summary>
        [Parameter]
        public string UnCheckedChildren { get; set; } = string.Empty;

        /// <summary>
        /// Content displayed inside switch when checked is false. Takes priority over <see cref="UnCheckedChildren"/>.
        /// </summary>
        [Parameter]
        public RenderFragment UnCheckedChildrenTemplate { get; set; }

        private bool _clickAnimating = false;

        private string _prefixCls = "ant-switch";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClass();
        }

        protected void SetClass()
        {
            var hashId = UseStyle(_prefixCls, SwitchStyle.UseComponentStyle);
            ClassMapper.Clear()
                .Add(_prefixCls)
                .Add(hashId)
                .If($"{_prefixCls}-checked", () => CurrentValue)
                .If($"{_prefixCls}-disabled", () => Disabled || Loading)
                .If($"{_prefixCls}-loading", () => Loading)
                .If($"{_prefixCls}-small", () => Size == InputSize.Small)
                .If($"{_prefixCls}-rtl", () => RTL);
        }

        private async Task HandleClick(MouseEventArgs e)
        {
            if (!Control)
                await base.ChangeValue(!CurrentValue);
            if (OnClick.HasDelegate)
                await OnClick.InvokeAsync(null);
        }

        private void HandleMouseOver(MouseEventArgs e) => _clickAnimating = true;

        private void HandleMouseOut(MouseEventArgs e) => _clickAnimating = false;
    }
}
