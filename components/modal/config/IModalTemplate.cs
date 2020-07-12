using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public interface IModalTemplate
    {
        /// <summary>
        /// 点击确定按钮时调用，可以重写它来放入自己的逻辑
        /// </summary>
        Task OkAsync(ModalClosingEventArgs args);

        /// <summary>
        /// 点击确定按钮时调用，可以重写它来放入自己的逻辑
        /// </summary>
        Task CancelAsync(ModalClosingEventArgs args);

    }
}
