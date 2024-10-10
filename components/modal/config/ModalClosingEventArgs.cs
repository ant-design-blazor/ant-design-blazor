// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        /// Whether the closing should be cancelled.
        /// Setting true if the closing should be cancelled; default is false.
        /// 是否应取消关闭Modal。
        /// 如果应取消关闭，请设置为 true; 默认为 false。
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Reject to close the modal.
        /// </summary>
        public void Reject()
        {
            Cancel = true;
        }
    }
}
