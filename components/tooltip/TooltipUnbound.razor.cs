using Microsoft.AspNetCore.Components;
using AntDesign.Internal;
using OneOf;
using System.Threading.Tasks;
using System.Linq;
using AntDesign.JsInterop;
using System.Text.Json;
using System.Diagnostics;

namespace AntDesign
{
    public partial class TooltipUnbound : OverlayTriggerUnbound
    {
        [Parameter]
        public OneOf<string, RenderFragment, MarkupString> Title { get; set; } = string.Empty;

        [Parameter]
        public bool ArrowPointAtCenter { get; set; } = false;

        [Parameter]
        public double MouseEnterDelay { get; set; } = 0.1;

        [Parameter]
        public double MouseLeaveDelay { get; set; } = 0.1;

        //Added to transfer events from wrapping div to JS Callbacks
        [Inject]
        private DomEventService DomEventService { get; set; }

        public TooltipUnbound()
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

        #region All code added to handle div wrapper removal
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            //add to js
            Debug.WriteLine($"tooltip Title: {Title}, ref id: {RefBack.Current.Id}");
            if (firstRender)
            {
                Ref = RefBack.Current;
                DomEventService.AddEventListener(RefBack.Current, "mouseover", OnMouseEnter, false);
                DomEventService.AddEventListener(RefBack.Current, "mouseout", OnMouseLeave, false);
                DomEventService.AddEventListener(RefBack.Current, "focusin", OnFocusIn, false);
                DomEventService.AddEventListener(RefBack.Current, "focusout", OnFocusOut, false);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        private async void OnMouseEnter(JsonElement jsonElement)
        {
            await OnTriggerMouseEnter();
        }

        private async void OnMouseLeave(JsonElement jsonElement)
        {
            await OnTriggerMouseLeave();
        }

        private async void OnFocusIn(JsonElement jsonElement)
        {
            await OnTriggerFocusIn();
        }

        private async void OnFocusOut(JsonElement jsonElement)
        {
            await OnTriggerFocusOut();
        }

        private async void OnWindowResize(JsonElement obj)
        {
            await GetOverlayComponent().UpdatePosition();
        }
        #endregion
    }
}
