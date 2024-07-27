using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /// <summary>
    /// DOT NOT use Confirm Directly,
    /// please using ModalService or ConfirmService to create a Confirm dialog
    /// </summary>
    public partial class Confirm
    {
        #region Parameters

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public ConfirmOptions Config { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public ConfirmRef ConfirmRef { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<ConfirmRef> OnRemove { get; set; }

        #endregion

        private string _okBtnId = "ant-blazor-" + Guid.NewGuid();
        private string _cancelBtnId = "ant-blazor-" + Guid.NewGuid();

        DialogOptions _dialogOptions;
        private DialogOptions BuildDialogOptions(ConfirmOptions confirmOptions)
        {
            DialogOptions config = new DialogOptions()
            {
                Title = confirmOptions.Title,
                TitleTemplate = confirmOptions.TitleTemplate,
                OkButtonProps = confirmOptions.OkButtonProps,
                CancelButtonProps = confirmOptions.CancelButtonProps,
                Width = confirmOptions.Width,
                Centered = confirmOptions.Centered,
                Mask = confirmOptions.Mask,
                MaskClosable = confirmOptions.MaskClosable,
                MaskStyle = confirmOptions.MaskStyle,
                OkType = confirmOptions.OkType,
                CloseIcon = confirmOptions.Icon,
                ZIndex = confirmOptions.ZIndex,
                Keyboard = confirmOptions.Keyboard,
                GetContainer = confirmOptions.GetContainer,
                Footer = null,

                ClassName = confirmOptions.ClassName,
                Closable = false,
                CreateByService = confirmOptions.CreateByService,
                DestroyOnClose = true
            };

            config.ClassName = "ant-modal-confirm ant-modal-confirm-" + confirmOptions.ConfirmType;
            config.Title = null;
            config.CloseIcon = null;
            config.AfterClose = Close;
            config.OnCancel = ConfirmRef.Config.CreateByService ? e => HandleCancel(e, ConfirmResult.Cancel) : new Func<MouseEventArgs, Task>(async (e) => await Close());
            return config;
        }

        #region override

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            _dialogOptions = BuildDialogOptions(Config);
            if (ConfirmRef.OnOpen != null)
                await ConfirmRef.OnOpen.Invoke();
            await base.OnInitializedAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (Config.Visible && Config.AutoFocusButton != ConfirmAutoFocusButton.Null)
            {
                var element = Config.AutoFocusButton == ConfirmAutoFocusButton.Cancel
                    ? _cancelBtnId
                    : _okBtnId;
                if (element != null)
                {
                    await JsInvokeAsync(JSInteropConstants.FocusDialog, $"#{element}");
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        #endregion

        internal async Task Close()
        {
            if (Config.Visible)
            {
                Config.Visible = false;
                await InvokeAsync(StateHasChanged);
                await Task.Delay(250);
                if (OnRemove.HasDelegate)
                {
                    await OnRemove.InvokeAsync(ConfirmRef);
                }

                ConfirmRef.OnClose?.Invoke();
            }
        }

        private async Task HandleOk(MouseEventArgs e, ConfirmResult confirmResult)
        {
            var args = new ModalClosingEventArgs(e, false);

            var modalTemplate = (ConfirmRef as IFeedbackRef)?.ModalTemplate;
            if (modalTemplate != null)
                await modalTemplate.OnFeedbackOkAsync(args);

            if (!args.Cancel && Config.OnOk != null)
            {
                Config.OkButtonProps.Loading = true;
                await InvokeAsync(StateHasChanged);
                await Config.OnOk.Invoke(args);
            }

            if (args.Cancel)
            {
                Config.OkButtonProps.Loading = false;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await Close();
                ConfirmRef.TaskCompletionSource?.TrySetResult(confirmResult);
            }
        }

        private async Task HandleCancel(MouseEventArgs e, ConfirmResult confirmResult)
        {
            var args = new ModalClosingEventArgs(e, false);

            var modalTemplate = (ConfirmRef as IFeedbackRef)?.ModalTemplate;
            if (modalTemplate != null)
                await modalTemplate.OnFeedbackCancelAsync(args);

            if (!args.Cancel && Config.OnCancel != null)
            {
                Config.CancelButtonProps.Loading = true;
                await InvokeAsync(StateHasChanged);
                await Config.OnCancel.Invoke(args);
            }

            if (args.Cancel)
            {
                Config.CancelButtonProps.Loading = false;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await Close();
                ConfirmRef.TaskCompletionSource?.TrySetResult(confirmResult);
            }
        }

        private async Task HandleBtn1Click(MouseEventArgs e, ConfirmResult confirmResult)
        {
            if (ConfirmRef.Config.CreateByService)
            {
                await HandleOk(e, confirmResult);
            }
            else
            {
                Config.Button1Props.Loading = false;
                await InvokeAsync(StateHasChanged);
                await Close();
                ConfirmRef.TaskCompletionSource?.TrySetResult(confirmResult);
            }
        }

        private async Task HandleBtn2Click(MouseEventArgs e, ConfirmResult confirmResult)
        {
            if (ConfirmRef.Config.CreateByService)
            {
                await HandleCancel(e, confirmResult);
            }
            else
            {
                Config.Button2Props.Loading = false;
                await InvokeAsync(StateHasChanged);
                await Close();
                ConfirmRef.TaskCompletionSource?.TrySetResult(confirmResult);
            }
        }

        private async Task HandleBtn3Click(MouseEventArgs _, ConfirmResult confirmResult)
        {
            Config.Button3Props.Loading = false;
            await InvokeAsync(StateHasChanged);
            await Close();
            ConfirmRef.TaskCompletionSource?.TrySetResult(confirmResult);
        }

    }
}
