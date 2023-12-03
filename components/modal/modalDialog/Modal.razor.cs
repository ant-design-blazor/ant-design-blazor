using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// Modal Dialog
    /// </summary>
    public partial class Modal
    {
        #region Parameter

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public ModalRef ModalRef { get; set; }

        /// <summary>
        /// Specify a function that will be called when modal is closed
        /// </summary>
        [Parameter]
        public Func<Task> AfterClose { get; set; }

        /// <summary>
        /// Body style for modal body element. Such as height, padding etc
        /// </summary>
        [Parameter]
        public string BodyStyle { get; set; }

        /// <summary>
        /// Text or RenderFragment of the Cancel button, it will override the ModalLocale
        /// </summary>
        [Parameter]
        public OneOf<string, RenderFragment>? CancelText { get; set; } = null;

        /// <summary>
        /// centered
        /// </summary>
        [Parameter]
        public bool Centered { get; set; }

        /// <summary>
        /// Whether a close (x) button is visible on top right of the modal dialog or not
        /// </summary>
        [Parameter]
        public bool Closable { get; set; } = true;

        /// <summary>
        /// Whether the modal dialog box be dragged
        /// </summary>
        [Parameter]
        public bool Draggable { get; set; }

        /// <summary>
        /// Drag and drop only within the Viewport
        /// </summary>
        [Parameter]
        public bool DragInViewport { get; set; } = true;

        /// <summary>
        /// closer icon RenderFragment, the default is a "X"
        /// </summary>
        [Parameter]
        public RenderFragment CloseIcon { get; set; } = DialogOptions.DefaultCloseIcon;

        /// <summary>
        /// Whether to apply loading visual effect for OK button or not
        /// </summary>
        [Parameter]
        public bool ConfirmLoading { get; set; }

        /// <summary>
        /// Whether to unmount child components on onClose, default is false
        /// </summary>
        [Parameter]
        public bool DestroyOnClose { get; set; }

        /// <summary>
        /// Footer content, set as Footer=null when you don't need default buttons
        /// </summary>
        [Parameter]
        public OneOf<string, RenderFragment>? Footer { get; set; } = DialogOptions.DefaultFooter;

        /// <summary>
        /// get or set the modal parent DOM, default is null: which is specifying document.body
        /// </summary>
        [Parameter]
        public ElementReference? GetContainer { get; set; } = null;

        /// <summary>
        /// Whether support press esc to close
        /// </summary>
        [Parameter]
        public bool Keyboard { get; set; } = true;

        /// <summary>
        /// Whether show mask or not
        /// </summary>
        [Parameter]
        public bool Mask { get; set; } = true;

        /// <summary>
        /// Whether to close the modal dialog when the mask (area outside the modal) is clicked
        /// </summary>
        [Parameter]
        public bool MaskClosable { get; set; } = true;

        /// <summary>
        /// Style for modal's mask element
        /// </summary>
        [Parameter]
        public string MaskStyle { get; set; }

        /// <summary>
        /// Text of RenderFragment of the OK button, it will override the ModalLocale
        /// </summary>
        [Parameter]
        public OneOf<string, RenderFragment>? OkText { get; set; } = null;

        /// <summary>
        /// Button type of the OK button
        /// </summary>
        [Parameter]
        public string OkType { get; set; } = ButtonType.Primary;

        #region title

        /// <summary>
        /// The modal dialog's title. If <param>TitleTemplate</param>!= null, <param>Title</param> will not take effect
        /// </summary>
        [Parameter]
        public string Title { get; set; } = null;

        /// <summary>
        /// The modal dialog's title
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; } = null;

        #endregion title

        /// <summary>
        /// Whether the modal dialog is visible or not
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }


        /// <summary>
        /// Specify a function invoke when the modal dialog is visible or not
        /// </summary>
        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

        /// <summary>
        /// Width of the modal dialog, the default value is 520
        /// </summary>
        [Parameter] public OneOf<string, double> Width { get; set; } = 520;

        private string GetWidth()
        {
            if (Width.IsT0)
            {
                return Width.AsT0;
            }
            else
            {
                return $"{Width.AsT1}px";
            }
        }

        /// <summary>
        /// The class name of the container of the modal dialog
        /// </summary>
        [Parameter]
        public string WrapClassName { get; set; }

        /// <summary>
        /// The z-index of the Modal
        /// </summary>
        [Parameter]
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// Specify a function that will be called when a user clicks mask, close button on top right or Cancel button
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnCancel { get; set; }

        /// <summary>
        /// Specify a function that will be called when a user clicks the OK button
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnOk { get; set; }

        /// <summary>
        /// The OK button props
        /// </summary>
        [Parameter]
        public ButtonProps OkButtonProps { get; set; }

        /// <summary>
        /// The Cancel button props
        /// </summary>
        [Parameter]
        public ButtonProps CancelButtonProps { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Is RTL
        /// </summary>
        public bool Rtl => base.RTL;

        /// <summary>
        /// Modal Locale
        /// </summary>
        [Parameter]
        public ModalLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Modal;

        /// <summary>
        /// max modal content body height
        /// </summary>
        [Parameter]
        public string MaxBodyHeight { get; set; } = null;

        /// <summary>
        /// show modal maximize button
        /// </summary>
        [Parameter]
        public bool Maximizable { get; set; } = false;

        /// <summary>
        /// The icon of the maximize button when the modal is in normal state
        /// </summary>
        [Parameter]
        public RenderFragment MaximizeBtnIcon { get; set; } = DialogOptions.DefaultMaximizeIcon;

        /// <summary>
        /// The icon of the maximize button when the modal is maximized
        /// </summary>
        [Parameter]
        public RenderFragment RestoreBtnIcon { get; set; } = DialogOptions.DefaultRestoreIcon;

        /// <summary>
        /// Maximize the Modal during component initialization, and it will ignore the Maximizable value.
        /// </summary>
        [Parameter]
        public bool DefaultMaximized { get; set; } = false;

        /// <summary>
        /// Resizable (Horizontal direction only)
        /// </summary>
        [Parameter]
        public bool Resizable { get; set; } = false;

        #endregion Parameter

#pragma warning disable 649
        private DialogWrapper _dialogWrapper;
#pragma warning restore 649

        private DialogOptions BuildDialogOptions()
        {
            DialogOptions options = new DialogOptions()
            {
                OnClosed = AfterClose,
                BodyStyle = BodyStyle,
                CancelText = CancelText ?? Locale.CancelText,
                Centered = Centered,
                Closable = Closable,
                Draggable = Draggable,
                DragInViewport = DragInViewport,
                CloseIcon = CloseIcon,
                ConfirmLoading = ConfirmLoading,
                Footer = Footer,

                GetContainer = GetContainer,
                Keyboard = Keyboard,
                Mask = Mask,
                MaskClosable = MaskClosable,
                MaskStyle = MaskStyle,

                OkText = OkText ?? Locale.OkText,
                OkType = OkType,
                Title = Title,
                TitleTemplate = TitleTemplate,
                Width = Width,
                WrapClassName = WrapClassName,
                ZIndex = ZIndex,
                OnCancel = async (e) =>
                {
                    var args = new ModalClosingEventArgs(e, false);

                    var modalTemplate = (ModalRef as IFeedbackRef)?.ModalTemplate;
                    if (modalTemplate != null)
                        await modalTemplate.OnFeedbackCancelAsync(args);
                    if (!args.Cancel)
                    {
                        await (ModalRef?.OnCancel?.Invoke() ?? Task.CompletedTask);

                        if (VisibleChanged.HasDelegate)
                        {
                            await VisibleChanged.InvokeAsync(false);
                        }

                        if (OnCancel.HasDelegate)
                        {
                            await OnCancel.InvokeAsync(e);
                        }
                    }
                },
                OnOk = async (e) =>
                {
                    var args = new ModalClosingEventArgs(e, false);

                    var modalTemplate = (ModalRef as IFeedbackRef)?.ModalTemplate;
                    if (modalTemplate != null)
                        await modalTemplate.OnFeedbackOkAsync(args);
                    if (!args.Cancel)
                    {
                        await (ModalRef?.OnOk?.Invoke() ?? Task.CompletedTask);

                        if (VisibleChanged.HasDelegate)
                        {
                            await VisibleChanged.InvokeAsync(false);
                        }

                        if (OnOk.HasDelegate)
                        {
                            await OnOk.InvokeAsync(e);
                        }
                    }
                    else
                    {
                        ConfirmLoading = false;
                        await InvokeStateHasChangedAsync();
                    }
                },
                OkButtonProps = OkButtonProps,

                CancelButtonProps = CancelButtonProps,
                Rtl = Rtl,
                MaxBodyHeight = MaxBodyHeight,
                Maximizable = Maximizable,
                MaximizeBtnIcon = MaximizeBtnIcon,
                RestoreBtnIcon = RestoreBtnIcon,
                DefaultMaximized = DefaultMaximized,
                Resizable = Resizable,
            };

            return options;
        }

        #region Sustainable Dialog

        private bool _hasFocus = false;

        private async Task OnAfterDialogShow()
        {
            if (!_hasFocus)
            {
                await JsInvokeAsync(JSInteropConstants.FocusDialog, $"#{_dialogWrapper.Dialog.SentinelStart}");
                _hasFocus = true;
                if (ModalRef?.OnOpen != null)
                {
                    await ModalRef.OnOpen();
                }
            }
        }

        private async Task OnAfterHide()
        {
            _hasFocus = false;
            if (ModalRef?.OnClose != null)
            {
                await ModalRef.OnClose();
            }
        }

        private async Task OnBeforeDialogWrapperDestroy()
        {
            await InvokeAsync(StateHasChanged);
        }

        #endregion Sustainable Dialog
    }
}
