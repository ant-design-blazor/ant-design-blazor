// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /**
    <summary>
        <para>A simple and compact confirmation dialog of an action.</para>

        <h2>When To Use</h2>

        <para>A simple and compact dialog used for asking for user confirmation.</para>
        <para>The difference with the <c>confirm</c> modal dialog is that it's more lightweight than the static popped full-screen confirm modal.</para>

        <h2>Two types</h2>

        <para>There are 2 rendering approaches for <c>Popconfirm</c>:</para>  

        <list type="number">
            <item>Wraps child element (content of the <c>Popconfirm</c>) with a <c>div</c> tag (default approach).</item>
            <item>Child element is not wrapped with anything. This approach requires usage of <c>Unbound</c> tag inside <c>Popconfirm</c> and depending on the child element type (please refer to the first example):
                <list type="bullet">
                    <item>html tag: has to have its <c>@ref</c> set to <c>@context.Current</c></item>
                    <item><c>Ant Design Blazor</c> component: has to have its <c>RefBack</c> attribute set to <c>@context</c>.</item>
                </list>       
            </item>
        </list>

        <para>Please ensure that the child node of <c>Popconfirm</c> accepts <c>onMouseEnter</c>, <c>onMouseLeave</c>, <c>onFocus</c>, <c>onClick</c> events.</para>
    </summary>
    <inheritdoc/>
    <seealso cref="ButtonProps" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Feedback, "https://gw.alipayobjects.com/zos/alicdn/fjMCD9xRq/Popconfirm.svg", Title = "Popconfirm", SubTitle = "气泡确认框")]
    public partial class Popconfirm : OverlayTrigger
    {
        /// <summary>
        /// Title
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Title. Takes priority over Title.
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Cancel button text
        /// </summary>
        /// <default value="Cancel" />
        [Parameter]
        public string CancelText { get; set; }

        /// <summary>
        /// Okay button text
        /// </summary>
        /// <default value="OK" />
        [Parameter]
        public string OkText { get; set; }

        /// <summary>
        /// Okay button type
        /// </summary>
        /// <default value="primary" />
        [Parameter]
        public string OkType { get; set; } = "primary";

        /// <summary>
        /// Properties to pass through to the okay button
        /// </summary>
        [Parameter]
        public PopconfirmLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Popconfirm;

        /// <summary>
        /// Properties to pass through to the okay button
        /// </summary>
        [Parameter]
        public ButtonProps OkButtonProps { get; set; } = new();

        /// <summary>
        /// Properties to pass through to the cancel button
        /// </summary>
        [Parameter]
        public ButtonProps CancelButtonProps { get; set; } = new();

        /// <summary>
        /// Icon displayed by text
        /// </summary>
        /// <default value="exclamation-circle" />
        [Parameter]
        public string Icon { get; set; } = "exclamation-circle";

        /// <summary>
        /// Icon displayed by text. Takes priority over Icon.
        /// </summary>
        [Parameter]
        public RenderFragment IconTemplate { get; set; }

        /// <summary>
        /// Callback executed when clicking cancel button
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnCancel { get; set; }

        /// <summary>
        /// Callback executed when clicking okay button
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnConfirm { get; set; }

        /// <summary>
        /// Point the tooltip arrow at the center
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool ArrowPointAtCenter { get; set; } = false;

        /// <summary>
        /// Delay (in seconds) before showing popconfirm when trigger is Hover and mouse enters
        /// </summary>
        /// <default value="0.1" />
        [Parameter]
        public double MouseEnterDelay { get; set; } = 0.1;

        /// <summary>
        /// Delay (in seconds) before hiding popconfirm when trigger is Hover and mouse leaves
        /// </summary>
        /// <default value="0.1" />
        [Parameter]
        public double MouseLeaveDelay { get; set; } = 0.1;

        public Popconfirm()
        {
            PrefixCls = "ant-popover";
            Placement = Placement.Top;
            Trigger = new[] { AntDesign.Trigger.Click };
        }

        protected override void OnInitialized()
        {
            OkText ??= Locale.OkText ?? "Ok";
            CancelText ??= Locale.CancelText ?? "Cancel";
        }

        internal override async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (Trigger.Contains(AntDesign.Trigger.Hover))
            {
                await Task.Delay((int)(MouseEnterDelay * 1000));
            }

            await base.Show(overlayLeft, overlayTop);
        }

        internal override async Task Hide(bool force = false)
        {
            if (Trigger.Contains(AntDesign.Trigger.Hover))
            {
                await Task.Delay((int)(MouseLeaveDelay * 1000));
            }

            await base.Hide(force);
        }

        private async Task Cancel(MouseEventArgs args)
        {
            if (OnCancel.HasDelegate)
            {
                await OnCancel.InvokeAsync(args);
            }
            await base.Hide();
        }

        private async Task Confirm(MouseEventArgs args)
        {
            if (OnConfirm.HasDelegate)
            {
                await OnConfirm.InvokeAsync(args);
            }
            await base.Hide();
        }

        internal override string GetOverlayEnterClass()
        {
            return "ant-zoom-big-enter ant-zoom-big-enter-active ant-zoom-big";
        }

        internal override string GetOverlayLeaveClass()
        {
            return "ant-zoom-big-leave ant-zoom-big-leave-active ant-zoom-big";
        }
    }
}
