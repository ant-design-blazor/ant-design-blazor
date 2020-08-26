using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class ConfirmService
    {
        internal event Func<ModalRef, Task> OnOpenEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="confirmButtons"></param>
        /// <param name="confirmIcon"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<ConfirmResult> Show(
            OneOf<string, RenderFragment> content,
            OneOf<string, RenderFragment> title,
            ConfirmButtons confirmButtons,
            ConfirmIcon confirmIcon,
            ConfirmButtonOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            ConfirmOptions confirmOptions = new ConfirmOptions()
            {
                Title = title,
                Content = content,
                ConfirmButtons = confirmButtons,
                ConfirmIcon = confirmIcon,
            };

            #region config button default properties

            if (options.Button1Props != null)
            {
                confirmOptions.Button1Props = options.Button1Props;
            }
            if (options.Button2Props != null)
            {
                confirmOptions.Button2Props = options.Button2Props;
            }
            if (options.Button3Props != null)
            {
                confirmOptions.Button3Props = options.Button3Props;
            }

            #endregion

            var modalRef = new ModalRef(confirmOptions);
            modalRef.TaskCompletionSource = new TaskCompletionSource<ConfirmResult>(); ;
            await OnOpenEvent?.Invoke(modalRef);
            return await modalRef.TaskCompletionSource.Task;
        }

        public Task<ConfirmResult> Show
            (OneOf<string, RenderFragment> content,
            OneOf<string, RenderFragment> title,
            ConfirmButtons confirmButtons = ConfirmButtons.OKCancel,
            ConfirmIcon confirmIcon = ConfirmIcon.Info)
        {
            return Show(content, title, confirmButtons, confirmIcon, new ConfirmButtonOptions());
        }
    }
}
