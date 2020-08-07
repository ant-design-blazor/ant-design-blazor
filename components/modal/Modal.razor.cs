using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using OneOf;

namespace AntDesign
{
    public partial class Modal
    {
        private const string PrefixCls = "ant-modal";

        #region Parameter

        [Parameter] public Func<Task> AfterClose { get; set; }

        [Parameter] public string BodyStyle { get; set; }

        [Parameter] public OneOf<string, RenderFragment> CancelText { get; set; } = "Cancel";

        [Parameter] public bool Centered { get; set; }

        [Parameter] public bool Closable { get; set; } = true;

        private static readonly RenderFragment _defaultCloseIcon = (builder) =>
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "Type", "close");
            builder.AddAttribute(2, "Theme", "outline");
            builder.CloseComponent();
        };

        [Parameter] public RenderFragment CloseIcon { get; set; } = _defaultCloseIcon;

        [Parameter] public bool ConfirmLoading { get; set; }

        [Parameter] public bool DestroyOnClose { get; set; }

        private static readonly RenderFragment _defaultFooter = (builder) =>
        {
            builder.OpenComponent<ModalFooter>(0);
            builder.CloseComponent();
        };

        [Parameter] public OneOf<string, RenderFragment>? Footer { get; set; } = _defaultFooter;

        [Parameter] public bool ForceRender { get; set; }

        [Parameter] public ElementReference? GetContainer { get; set; } = null;

        [Parameter] public bool Keyboard { get; set; } = true;
        [Parameter] public bool Mask { get; set; } = true;
        [Parameter] public bool MaskClosable { get; set; } = true;

        [Parameter] public string MaskStyle { get; set; }

        [Parameter] public OneOf<string, RenderFragment> OkText { get; set; } = "OK";

        [Parameter] public string OkType { get; set; } = ButtonType.Primary;

        [Parameter] public OneOf<string, RenderFragment> Title { get; set; }

        [Parameter] public bool Visible { get; set; }

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

        [Parameter] public string WrapClassName { get; set; }

        [Parameter] public int ZIndex { get; set; } = 1000;

        [Parameter] public EventCallback<MouseEventArgs> OnCancel { get; set; }

        [Parameter] public EventCallback<MouseEventArgs> OnOk { get; set; }

        [Parameter] public ButtonProps OkButtonProps { get; set; }

        [Parameter] public ButtonProps CancelButtonProps { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [CascadingParameter] public bool Rtl { get; set; } = false;

        private string GetWrapClassNameExtended()
        {
            var classNameArray = new List<string>();
            if (Centered)
            {
                classNameArray.Add($"{PrefixCls}-centered");
            }

            if (Rtl)
            {
                classNameArray.Add($"{PrefixCls}-wrap-rtl");
            }

            return string.Join(' ', classNameArray);
        }

        #endregion Parameter

        private Dialog _dialog;

        private DialogOptions BuildDialogOptions()
        {
            DialogOptions options = new DialogOptions()
            {
                OnClosed = AfterClose,
                BodyStyle = BodyStyle,
                CancelText = CancelText,
                Centered = Centered,
                Closable = Closable,
                CloseIcon = CloseIcon,
                ConfirmLoading = ConfirmLoading,
                DestroyOnClose = DestroyOnClose,
                Footer = Footer,
                ForceRender = ForceRender,
                GetContainer = GetContainer,
                Keyboard = Keyboard,
                Mask = Mask,
                MaskClosable = MaskClosable,
                MaskStyle = MaskStyle,
                OkText = OkText,
                OkType = OkType,
                Title = Title,
                Width = Width,
                WrapClassName = WrapClassName,
                ZIndex = ZIndex,
                OnCancel = async (e) =>
                {
                    if (OnCancel.HasDelegate)
                    {
                        await OnCancel.InvokeAsync(e);
                    }
                },
                OnOk = async (e) =>
                {
                    if (OnOk.HasDelegate)
                    {
                        await OnOk.InvokeAsync(e);
                    }
                },
                OkButtonProps = OkButtonProps,
                CancelButtonProps = CancelButtonProps,
                Rtl = Rtl
            };
            return options;
        }

        private bool _hasAdd = false;
        private bool _hasFocus = false;

        protected override async Task OnParametersSetAsync()
        {
            if (Visible && !_hasAdd)
            {
                _hasAdd = true;
            }

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool isFirst)
        {
            if (Visible)
            {
                if (!_hasFocus)
                {
                    await JsInvokeAsync(JSInteropConstants.focusDialog, $"#{_dialog.SentinelStart}");
                    _hasFocus = true;
                }
            }
            else
            {
                if (_hasAdd && DestroyOnClose)
                {
                    if (_dialog != null)
                    {
                        await _dialog.Hide();
                    }

                    _hasAdd = false;
                    _hasFocus = false;
                    await InvokeAsync(StateHasChanged);
                }
            }

            await base.OnAfterRenderAsync(isFirst);
        }
    }
}
