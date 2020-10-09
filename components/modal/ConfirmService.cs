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
        /// show a confirm dialog like MessageBox of Windows
        /// </summary>
        /// <param name="content">the content of dialog</param>
        /// <param name="title">the title of dialog</param>
        /// <param name="confirmButtons">the buttons of dialog</param>
        /// <param name="confirmIcon">the icon of dialog</param>
        /// <param name="options">the configuration options for dialog</param>
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

            var modalRef = new ModalRef(confirmOptions)
            {
                TaskCompletionSource = new TaskCompletionSource<ConfirmResult>()
            };
            if (OnOpenEvent != null)
            {
                await OnOpenEvent.Invoke(modalRef);
            }
            return await modalRef.TaskCompletionSource.Task;
        }

        /// <summary>
        /// show a confirm dialog like MessageBox of Windows
        /// </summary>
        /// <param name="content">the content of dialog</param>
        /// <param name="title">the title of dialog</param>
        /// <param name="confirmButtons">the buttons of dialog</param>
        /// <param name="confirmIcon">the icon of dialog</param>
        /// <returns></returns>
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
