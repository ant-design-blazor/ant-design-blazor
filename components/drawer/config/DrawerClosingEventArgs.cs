using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class DrawerClosingEventArgs
    {

        public DrawerClosingEventArgs() { }

        public DrawerClosingEventArgs(bool cancel)
        {
            Cancel = cancel;
         }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应取消事件。
        /// 返回结果: true 如果应取消事件;否则为 false。
        /// </summary>
        public bool Cancel { get; set; }
    }
}
