using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class MessageItem : AntDomComponentBase
    {
        [Parameter]
        public MessageConfig Config { get; set; }

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
    }
}
