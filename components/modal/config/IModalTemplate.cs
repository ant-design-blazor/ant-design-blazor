using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AntDesign
{
    public interface IModalTemplate
    {
        /// <summary>
        /// Call back when OK button is triggered
        /// 点击确定按钮时调用，可以重写它来放入自己的逻辑
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please replace it with OnFeedbackOkAsync")]
        Task OkAsync(ModalClosingEventArgs args);

        /// <summary>
        /// Call back when Cancel button is triggered
        /// 点击取消按钮时调用，可以重写它来放入自己的逻辑
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please replace it with OnFeedbackCancelAsync")]
        Task CancelAsync(ModalClosingEventArgs args);

        /// <summary>
        /// Call back when OK button is triggered
        /// 击确定按钮时调用，可以重写它来放入自己的逻辑
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task OnFeedbackOkAsync(ModalClosingEventArgs args);

        /// <summary>
        /// Call back when Cancel button is triggered
        /// 点击取消按钮时调用，可以重写它来放入自己的逻辑
        /// </summary>
        Task OnFeedbackCancelAsync(ModalClosingEventArgs args);
    }
}
