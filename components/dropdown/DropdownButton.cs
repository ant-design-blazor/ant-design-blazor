using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DropdownButton : Dropdown
    {
        protected override void OnInitialized()
        {
            Placement = PlacementType.BottomRight;
            base.OnInitialized();
        }

        /// <summary>
        /// Force overlay trigger to be attached to wrapping element of 
        /// the right button. Right button has to be wrapped, 
        /// because overlay will be looking for first child
        /// element of the overlay trigger to calculate the overlay position.
        /// If the right button was the trigger, then its first child
        /// would be the icon/ellipsis and the overlay would have been 
        /// rendered too high.
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Ref = RefBack.Current;
                DomEventService.AddEventListener(Ref, "click", OnUnboundClick, true);
                DomEventService.AddEventListener(Ref, "mouseover", OnUnboundMouseEnter, true);
                DomEventService.AddEventListener(Ref, "mouseout", OnUnboundMouseLeave, true);
                DomEventService.AddEventListener(Ref, "focusin", OnUnboundFocusIn, true);
                DomEventService.AddEventListener(Ref, "focusout", OnUnboundFocusOut, true);
                DomEventService.AddEventListener(Ref, "contextmenu", OnContextMenu, true, true);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        protected override void Dispose(bool disposing)
        {
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "click", OnUnboundClick);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "mouseover", OnUnboundMouseEnter);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "mouseout", OnUnboundMouseLeave);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "focusin", OnUnboundFocusIn);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "focusout", OnUnboundFocusOut);
            DomEventService.RemoveEventListerner<JsonElement>(Ref, "contextmenu", OnContextMenu);

            base.Dispose(disposing);
        }

        internal override async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (!Loading)
            {
                await _overlay.Show(overlayLeft, overlayTop);
            }
        }

        [Parameter]
        public bool Loading
        {
            get => _loading;
            set
            {
                _loading = value;
                if (value)
                {
                    _preLoadingIcon = _icon;
                    Icon = "loading";
                }
                else
                {
                    Icon = _preLoadingIcon;
                }
            }
        }

        private string _preLoadingIcon;
        private string _icon = "ellipsis";
        [Parameter]
        public string Icon
        {
            get => _icon;
            set
            {
                if (!_loading || value == "loading")
                {
                    _icon = value;
                    ChangeRightButtonIcon(value);
                }
            }
        }

        private string _size = AntSizeLDSType.Default;
        [Parameter]
        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                ChangeButtonSize(value);
            }
        }

        private string _type = ButtonType.Default;
        private bool _loading;

        [Parameter]
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                ChangeButtonType(value);
            }
        }

        public DropdownButton() => IsButton = true;
    }
}
