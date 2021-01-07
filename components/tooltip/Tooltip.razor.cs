using Microsoft.AspNetCore.Components;
using AntDesign.Internal;
using OneOf;
using System.Threading.Tasks;
using System.Linq;
using AntDesign.JsInterop;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Tooltip : OverlayTrigger
    {
        [Parameter]
        public OneOf<string, RenderFragment, MarkupString> Title { get; set; } = string.Empty;

        [Parameter]
        public bool ArrowPointAtCenter { get; set; } = false;

        [Parameter]
        public double MouseEnterDelay { get; set; } = 0.1;

        [Parameter]
        public double MouseLeaveDelay { get; set; } = 0.1;

        [Inject]
        private DomEventService DomEventService { get; set; }

        public Tooltip()
        {
            PrefixCls = "ant-tooltip";
            Placement = PlacementType.Top;
        }

        internal override string GetOverlayEnterClass()
        {
            return "zoom-big-fast-enter zoom-big-fast-enter-active zoom-big-fast";
        }

        internal override string GetOverlayLeaveClass()
        {
            return "zoom-big-fast-leave zoom-big-fast-leave-active zoom-big-fast";
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
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && Unbound != null)
            {
                Ref = RefBack.Current;
                DomEventService.AddEventListener(Ref, "mouseover", OnTooltipMouseEnter, true);
                DomEventService.AddEventListener(Ref, "mouseout", OnTooltipMouseLeave, true);
                DomEventService.AddEventListener(Ref, "focusin", OnTooltipFocusIn, true);
                DomEventService.AddEventListener(Ref, "focusout", OnTooltipFocusOut, true);
                DomEventService.AddEventListener(Ref, "contextmenu", OnContextMenu, true, true);

            }
            return base.OnAfterRenderAsync(firstRender);
        }

        private void OnTooltipMouseEnter(JsonElement jsonElement) => OnTriggerMouseEnter();
        private void OnTooltipMouseLeave(JsonElement jsonElement) => OnTriggerMouseLeave();
        private void OnTooltipFocusIn(JsonElement jsonElement) => OnTriggerFocusIn();
        private void OnTooltipFocusOut(JsonElement jsonElement) => OnTriggerFocusOut();

        protected override void Dispose(bool disposing)
        {
            if (Unbound != null)
            {
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "mouseover", OnTooltipMouseEnter);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "mouseout", OnTooltipMouseLeave);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "focusin", OnTooltipFocusIn);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "focusout", OnTooltipFocusOut);
                DomEventService.RemoveEventListerner<JsonElement>(Ref, "contextmenu", OnContextMenu);
            }
            base.Dispose(disposing);
        }

        private async void OnContextMenu(JsonElement jsonElement)
        {
            var eventArgs = JsonSerializer.Deserialize<MouseEventArgs>(jsonElement.ToString(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await base.OnTriggerContextmenu(eventArgs);
        }
    }
}
