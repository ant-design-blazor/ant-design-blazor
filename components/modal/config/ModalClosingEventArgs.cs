using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class ModalClosingEventArgs<TResult> : ModalClosingEventArgs
    {
        public TResult Result { get; set; }
        public ModalClosingEventArgs(TResult result, bool cancel)
        {
            Result = result;
            Cancel = cancel;
        }

    }

    public class ModalClosingEventArgs
    {

        public ModalClosingEventArgs() { }

        public ModalClosingEventArgs(bool cancel)
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
