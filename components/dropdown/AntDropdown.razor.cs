using System;
using System.Text.Json;
using System.Threading.Tasks;
using AntBlazor.Internal;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public partial class AntDropdown : AntDomComponentBase
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
        public AntDropdownTrigger[] Trigger { get; set; } = new AntDropdownTrigger[] { AntDropdownTrigger.Hover };

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
        public AntDropdownPlacement Placement { get; set; } = AntDropdownPlacement.BottomLeft;

        private AntDropdownOverlay _antDropdownOverlay = null;

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

            if (IsContainTrigger(AntDropdownTrigger.Hover))
            {
                _antDropdownOverlay.PreventHide(true);

                await _antDropdownOverlay.Show();
            }
        }

        private async Task OnTriggerMouseLeave()
        {
            _mouseInTrigger = false;

            if (IsContainTrigger(AntDropdownTrigger.Hover))
            {
                _antDropdownOverlay.PreventHide(_mouseInOverlay);

                await _antDropdownOverlay.Hide();
            }
        }

        private void OnOverlayMouseEnter()
        {
            _mouseInOverlay = true;

            if (IsContainTrigger(AntDropdownTrigger.Hover))
            {
                _antDropdownOverlay.PreventHide(true);
            }
        }

        private async Task OnOverlayMouseLeave()
        {
            _mouseInOverlay = false;

            if (IsContainTrigger(AntDropdownTrigger.Hover))
            {
                _antDropdownOverlay.PreventHide(_mouseInTrigger);

                await _antDropdownOverlay.Hide();
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
            if (IsContainTrigger(AntDropdownTrigger.Click))
            {
                if (_antDropdownOverlay.IsPopup())
                {
                    await _antDropdownOverlay.Hide();
                }
                else
                {
                    await _antDropdownOverlay.Show();
                }
            }
        }

        private async Task OnTriggerContextmenu(MouseEventArgs args)
        {
            if (IsContainTrigger(AntDropdownTrigger.ContextMenu))
            {
                // TODO：MouseEventArgs will support offsetX/offsetY in the future
                int offsetX = 10;
                int offsetY = 10;

                await _antDropdownOverlay.Hide();
                await _antDropdownOverlay.Show(offsetX, offsetY);
            }
        }

        private void OnMouseUp(JsonElement element)
        {
            if (_mouseInOverlay == false && _mouseInTrigger == false)
            {
                _antDropdownOverlay.Hide();
            }
        }

        private bool IsContainTrigger(AntDropdownTrigger triggerType)
        {
            foreach (AntDropdownTrigger trigger in Trigger)
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
            await _antDropdownOverlay.Show();
        }

        public async Task Hide()
        {
            await _antDropdownOverlay.Hide();
        }
    }
}
