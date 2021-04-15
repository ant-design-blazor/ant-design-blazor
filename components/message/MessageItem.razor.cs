using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class MessageItem : AntDomComponentBase
    {
        [Parameter]
        public MessageConfig Config { get; set; }

        protected const string PrefixCls = "ant-message";

        private CultureInfo _cultureInfo = new CultureInfo("en-us", false);

        private string GetClassName()
        {
            var className = $"{PrefixCls}-{Config.Type.ToString().ToLower(_cultureInfo)}";

            if (RTL)
            {
                className += " ant-message-rtl";
            }
            return className;
        }
    }
}
