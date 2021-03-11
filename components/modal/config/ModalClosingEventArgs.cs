using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class ModalClosingEventArgs
    {

        public ModalClosingEventArgs() { }

        public ModalClosingEventArgs(MouseEventArgs mouseEvent, bool cancel)
        {
            Cancel = cancel;
            MouseEvent = mouseEvent;
        }

        public MouseEventArgs MouseEvent { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应取消事件。
        /// 返回结果: true 如果应取消事件;否则为 false。
        /// Gets or sets a value indicating whether the event should be cancelled.
        /// Return result: true if the event should be cancelled; otherwise false.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
