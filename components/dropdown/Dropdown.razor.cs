using System;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Dropdown : OverlayTrigger
    {
        [Parameter]
        public Func<RenderFragment, RenderFragment, RenderFragment> ButtonsRender { get; set; }

        private string _rightButtonIcon = "ellipsis";
        private string _buttonSize = AntSizeLDSType.Default;
        private string _buttonType = ButtonType.Default;

        private RenderFragment _leftButton;
        private RenderFragment _rightButton;

        [Inject]
        private DomEventService DomEventService { get; set; }

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

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && Unbound != null)
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

        private void OnUnboundMouseEnter(JsonElement jsonElement) => OnTriggerMouseEnter();
        private void OnUnboundMouseLeave(JsonElement jsonElement) => OnTriggerMouseLeave();
        private void OnUnboundFocusIn(JsonElement jsonElement) => OnTriggerFocusIn();
        private void OnUnboundFocusOut(JsonElement jsonElement) => OnTriggerFocusOut();

        protected override void Dispose(bool disposing)
        {
            if (Unbound != null)
            {
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "click", OnUnboundClick);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "mouseover", OnUnboundMouseEnter);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "mouseout", OnUnboundMouseLeave);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "focusin", OnUnboundFocusIn);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "focusout", OnUnboundFocusOut);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "contextmenu", OnContextMenu);
            }
            base.Dispose(disposing);
        }

        private async void OnUnboundClick(JsonElement jsonElement)
        {
            var eventArgs = JsonSerializer.Deserialize<MouseEventArgs>(jsonElement.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await base.OnClickDiv(eventArgs);
        }
        private async void OnContextMenu(JsonElement jsonElement)
        {
            var eventArgs = JsonSerializer.Deserialize<MouseEventArgs>(jsonElement.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await base.OnTriggerContextmenu(eventArgs);
        }

    }
}
