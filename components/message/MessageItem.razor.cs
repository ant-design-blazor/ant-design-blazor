using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class MessageItem
    {
        [Parameter] public MessageConfig Config { get; set; }

        [Parameter] public bool IsRtl { get; set; }

        private CultureInfo _cultureInfo = new CultureInfo("en-us", false);

        private string GetClassName()
        {
            var className = $"{PrefixCls}-{Config.Type.ToString().ToLower(_cultureInfo)}";

            if (IsRtl)
            {
                className += " ant-message-rtl";
            }

            return className;
        }
    }
}
