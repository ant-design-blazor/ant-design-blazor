using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Popconfirm : OverlayTrigger
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        [Parameter]
        public string CancelText { get; set; } = "Cancel";

        [Parameter]
        public string OkText { get; set; } = "OK";

        [Parameter]
        public string OkType { get; set; } = "primary";

        [Parameter]
        public ButtonProps OkButtonProps { get; set; }

        [Parameter]
        public ButtonProps CancelButtonProps { get; set; }

        [Parameter]
        public string Icon { get; set; } = "exclamation-circle";

        [Parameter]
        public RenderFragment IconTemplate { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnCancel { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnConfirm { get; set; }

        [Parameter]
        public bool ArrowPointAtCenter { get; set; } = false;

        [Parameter]
        public double MouseEnterDelay { get; set; } = 0.1;

        [Parameter]
        public double MouseLeaveDelay { get; set; } = 0.1;

        [Inject]
        private DomEventService DomEventService { get; set; }
        public Popconfirm()
        {
            PrefixCls = "ant-popover";
            Placement = PlacementType.Top;
            Trigger = new[] { TriggerType.Click };
        }

        internal override async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (Trigger.Contains(TriggerType.Hover))
            {
                await Task.Delay((int)(MouseEnterDelay * 1000));
            }

            await base.Show(overlayLeft, overlayTop);
        }

        internal override async Task Hide(bool force = false)
        {
            if (Trigger.Contains(TriggerType.Hover))
            {
                await Task.Delay((int)(MouseLeaveDelay * 1000));
            }

            await base.Hide(force);
        }

        private async Task Cancel(MouseEventArgs args)
        {
            if (OnCancel.HasDelegate)
            {
                await OnCancel.InvokeAsync(args);
            }
            await base.Hide();
        }

        private async Task Confirm(MouseEventArgs args)
        {
            if (OnConfirm.HasDelegate)
            {
                await OnConfirm.InvokeAsync(args);
            }
            await base.Hide();
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
