// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class MessageItem : AntDomComponentBase
    {
        /// <summary>
        /// The message config
        /// </summary>
        [Parameter]
        public MessageConfig Config { get; set; }

        [Parameter]
        public EventCallback<MessageItem> OnMouseEnter { get; set; }

        [Parameter]
        public EventCallback<MessageItem> OnMouseLeave { get; set; }

        protected const string PrefixCls = "ant-message";

        private string GetClassName()
        {
            var className = $"{PrefixCls}-{Config.Type.ToString().ToLower(CultureInfo.InvariantCulture)}";

            if (RTL)
            {
                className += " ant-message-rtl";
            }
            return className;
        }

        private void HandleMouseEnter()
        {
            OnMouseEnter.InvokeAsync(this);
        }

        private void HandleMouseLeave()
        {
            OnMouseLeave.InvokeAsync(this);
        }
    }
}
