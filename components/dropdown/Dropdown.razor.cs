using System;
using System.Text.Json;
using System.Threading.Tasks;
using AntBlazor.Internal;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public partial class Dropdown : AntDomComponentBase
    {
        [CascadingParameter]
        public string PrefixCls { get; set; } = "ant-dropdown";

        [Parameter]
        public string PopupContainerSelector { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Visible { get; set; } = false;

        [Parameter]
        public bool IsButton { get; set; } = false;

        [Parameter]
        public DropdownTrigger[] Trigger { get; set; } = new DropdownTrigger[] { DropdownTrigger.Hover };

        [Parameter]
        public string OverlayClassName { get; set; }

        [Parameter]
        public string OverlayStyle { get; set; }

        [Parameter]
        public EventCallback<bool> OnVisibleChange { get; set; }

        [Parameter]
        public Func<RenderFragment, RenderFragment, RenderFragment> ButtonsRender { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        [Parameter]
        public RenderFragment Overlay { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public DropdownPlacement Placement { get; set; } = DropdownPlacement.BottomLeft;

        private DropdownOverlay _DropdownOverlay = null;

        private string _rightButtonIcon = "ellipsis";
        private string _buttonSize = AntSizeLDSType.Default;
        private string _buttonType = AntButtonType.Default;

        private bool _mouseInTrigger = false;
        private bool _mouseInOverlay = false;

        private RenderFragment _leftButton;
        private RenderFragment _rightButton;


        [Inject]
        private DomEventService DomEventService { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            DomEventService.AddEventListener("app", "mouseup", OnMouseUp);
        }

        protected override void OnParametersSet()
        {
            this.SetClass();

            base.OnParametersSet();
        }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
               ;
        }

        protected void ChangeRightButtonIcon(string icon)
        {
            _rightButtonIcon = icon;

            StateHasChanged();
        }

        protected void ChangeButtonSize(string size)
        {
            _buttonSize = size;

            StateHasChanged();
        }

        protected void ChangeButtonType(string type)
        {
            _buttonType = type;

            StateHasChanged();
        }

        private async Task OnTriggerMouseEnter()
        {
            _mouseInTrigger = true;

            if (_DropdownOverlay != null && IsContainTrigger(DropdownTrigger.Hover))
            {
                _DropdownOverlay.PreventHide(true);

                await _DropdownOverlay.Show();
            }
        }

        private async Task OnTriggerMouseLeave()
        {
            _mouseInTrigger = false;

            if (_DropdownOverlay != null && IsContainTrigger(DropdownTrigger.Hover))
            {
                _DropdownOverlay.PreventHide(_mouseInOverlay);

                await _DropdownOverlay.Hide();
            }
        }

        private void OnOverlayMouseEnter()
        {
            _mouseInOverlay = true;

            if (_DropdownOverlay != null && IsContainTrigger(DropdownTrigger.Hover))
            {
                _DropdownOverlay.PreventHide(true);
            }
        }

        private async Task OnOverlayMouseLeave()
        {
            _mouseInOverlay = false;

            if (_DropdownOverlay != null && IsContainTrigger(DropdownTrigger.Hover))
            {
                _DropdownOverlay.PreventHide(_mouseInTrigger);

                await _DropdownOverlay.Hide();
            }
        }

        private async Task OnClickDiv(MouseEventArgs args)
        {
            if (!IsButton)
            {
                await OnTriggerClick();
            }
            else
            {
                await OnClick.InvokeAsync(args);
            }
        }

        private async Task OnTriggerClick()
        {
            if (IsContainTrigger(DropdownTrigger.Click))
            {
                if (_DropdownOverlay.IsPopup())
                {
                    await _DropdownOverlay.Hide();
                }
                else
                {
                    await _DropdownOverlay.Show();
                }
            }
        }

        private async Task OnTriggerContextmenu(MouseEventArgs args)
        {
            if (IsContainTrigger(DropdownTrigger.ContextMenu))
            {
                // TODO：MouseEventArgs will support offsetX/offsetY in the future
                int offsetX = 10;
                int offsetY = 10;

                await _DropdownOverlay.Hide();
                await _DropdownOverlay.Show(offsetX, offsetY);
            }
        }

        private void OnMouseUp(JsonElement element)
        {
            if (_mouseInOverlay == false && _mouseInTrigger == false)
            {
                _DropdownOverlay.Hide();
            }
        }

        private bool IsContainTrigger(DropdownTrigger triggerType)
        {
            foreach (DropdownTrigger trigger in Trigger)
            {
                if (trigger == triggerType)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task Show()
        {
            await _DropdownOverlay.Show();
        }

        public async Task Hide()
        {
            await _DropdownOverlay.Hide();
        }

        public DropdownOverlay GetDropdownOverlay()
        {
            return _DropdownOverlay;
        }
    }
}
