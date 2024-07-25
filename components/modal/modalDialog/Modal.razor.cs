﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// Modal Dialog
    /// </summary>
    public partial class Modal: AntDomComponentBase
    {
        #region Parameter

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
        /// Header content
        /// </summary>
        [Parameter]
        public RenderFragment Header { get; set; } = DialogOptions.DefaultHeader;

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

        public ModalRef ModalRef => _modalRef;

        #endregion Parameter

        [Inject] private ModalService ModalService { get; set; }

        private ModalOptions BuildDialogOptions()
        {
            ModalOptions options = new ModalOptions()
            {
                AfterClose = OnAfterHide,
                AfterOpen = OnAfterDialogShow,
                BodyStyle = BodyStyle,
                CancelText = CancelText ?? Locale.CancelText,
                Centered = Centered,
                Closable = Closable,
                Content = ChildContent,
                Draggable = Draggable,
                DragInViewport = DragInViewport,
                DestroyOnClose = DestroyOnClose,
                CloseIcon = CloseIcon,
                Header = Header,
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
                OnCancel = HandleOnCancel,
                OnOk = HandleOnOk,
                OkButtonProps = OkButtonProps,
                CancelButtonProps = CancelButtonProps,
                Rtl = base.RTL,
                MaxBodyHeight = MaxBodyHeight,
                Maximizable = Maximizable,
                MaximizeBtnIcon = MaximizeBtnIcon,
                RestoreBtnIcon = RestoreBtnIcon,
                DefaultMaximized = DefaultMaximized,
                Resizable = Resizable,
                CreateByService = false,
                Visible = Visible,
                Style = Style,
                ClassName = ClassMapper.Class,
            };

            options.ConfirmLoading = ConfirmLoading;

            return options;
        }

        #region Sustainable Dialog

        private bool _hasFocus = false;

        private bool _firstShow = true;

        private ModalRef _modalRef;

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var visibleChanged = parameters.IsParameterChanged(nameof(Visible), Visible, out var newVisible);

            parameters.SetParameterProperties(this);

            if (_modalRef == null)
            {
                var options = BuildDialogOptions();
                _modalRef = new ModalRef(options, ModalService);
            }
            else
            {
                _modalRef.UpdateConfig(BuildDialogOptions());
            }

            if (visibleChanged)
            {
                if (newVisible)
                {
                    await _modalRef?.OpenAsync();
                }
                else
                {
                    await _modalRef?.CloseAsync();
                }
            }
        }

        private async Task OnAfterDialogShow()
        {
            if (!_hasFocus)
            {
                await JsInvokeAsync(JSInteropConstants.FocusDialog, $"#{_modalRef.Dialog.SentinelStart}");
                _hasFocus = true;
                if (_modalRef?.OnOpen != null)
                {
                    await _modalRef.OnOpen();
                }
            }
        }

        private async Task OnAfterHide()
        {
            _hasFocus = false;
            if (_modalRef?.OnClose != null)
            {
                await _modalRef.OnClose();
            }
            if (AfterClose != null)
            {
                await AfterClose.Invoke();
            }
        }

        private async Task HandleOnCancel(MouseEventArgs e)
        {
            var args = new ModalClosingEventArgs(e, false);

            var modalTemplate = (_modalRef as IFeedbackRef)?.ModalTemplate;
            if (modalTemplate != null)
                await modalTemplate.OnFeedbackCancelAsync(args);
            if (!args.Cancel)
            {
                await (_modalRef?.OnCancel?.Invoke() ?? Task.CompletedTask);

                await _modalRef.CloseAsync();

                Visible = false;
                if (VisibleChanged.HasDelegate)
                {
                    await VisibleChanged.InvokeAsync(false);
                }

                if (OnCancel.HasDelegate)
                {
                    await OnCancel.InvokeAsync(e);
                }
            }
        }

        private async Task HandleOnOk(MouseEventArgs e)
        {
            var args = new ModalClosingEventArgs(e, false);

            var modalTemplate = (_modalRef as IFeedbackRef)?.ModalTemplate;
            if (modalTemplate != null)
                await modalTemplate.OnFeedbackOkAsync(args);
            if (!args.Cancel)
            {
                await (_modalRef?.OnOk?.Invoke() ?? Task.CompletedTask);

                if (OnOk.HasDelegate)
                {
                    await OnOk.InvokeAsync(e);
                }

                await _modalRef.CloseAsync();
                if (VisibleChanged.HasDelegate)
                {
                    await VisibleChanged.InvokeAsync(false);
                }
            }
            else
            {
                ConfirmLoading = false;
                await InvokeStateHasChangedAsync();
            }
        }

        private async Task OnBeforeDialogWrapperDestroy()
        {
            await InvokeAsync(StateHasChanged);
        }

        #endregion Sustainable Dialog
    }
}
