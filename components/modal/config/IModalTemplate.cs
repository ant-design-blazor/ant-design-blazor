using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public interface IModalTemplate
    {
        /// <summary>
        /// 点击确定按钮时调用，可以重写它来放入自己的逻辑
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please replace it with OnFeedbackOkAsync")]
        Task OkAsync(ModalClosingEventArgs args);

        /// <summary>
        /// 点击确定按钮时调用，可以重写它来放入自己的逻辑
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please replace it with OnFeedbackCancelAsync")]
        Task CancelAsync(ModalClosingEventArgs args);

        /// <summary>
        /// 击确定按钮时调用，可以重写它来放入自己的逻辑
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task OnFeedbackOkAsync(ModalClosingEventArgs args);

        /// <summary>
        /// 点击取消按钮时调用，可以重写它来放入自己的逻辑
        /// </summary>
        Task OnFeedbackCancelAsync(ModalClosingEventArgs args);
    }
}
