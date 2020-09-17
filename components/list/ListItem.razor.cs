using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ListItem : AntDomComponentBase
    {
        public string PrefixName { get; set; } = "ant-list-item";

        [Parameter] public string Content { get; set; }

        [Parameter] public RenderFragment Extra { get; set; }

        [Parameter] public RenderFragment[] Actions { get; set; }

        [Parameter] public AntDirectionVHType ItemLayout { get; set; }

        [Parameter] public ListGridType Grid { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string ColStyle { get; set; }

        [Parameter] public int ItemCount { get; set; }

        [Parameter] public EventCallback OnClick { get; set; }

        [Parameter] public bool NoFlex { get; set; }

        [CascadingParameter(Name = "ItemClick")] public Action ItemClick { get; set; }

        [Inject]
        public DomEventService DomEventService { get; set; }

        public bool IsVerticalAndExtra()
        {
            return this.ItemLayout == AntDirectionVHType.Vertical && this.Extra != null;
        }

        protected override async Task OnInitializedAsync()
        {
            SetClassMap();

            if (Grid != null)
            {
                await this.SetGutterStyle();
                DomEventService.AddEventListener<object>("window", "resize", OnResize, false);
            }

            await base.OnInitializedAsync();
        }

        private async void OnResize(object o)
        {
            await SetGutterStyle();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
        }

        private static Hashtable _gridResponsiveMap = new Hashtable()
        {
            [nameof(BreakpointEnum.xs)] = "(max-width: 575px)",
            [nameof(BreakpointEnum.sm)] = "(max-width: 576px)",
            [nameof(BreakpointEnum.md)] = "(max-width: 768px)",
            [nameof(BreakpointEnum.lg)] = "(max-width: 992px)",
            [nameof(BreakpointEnum.xl)] = "(max-width: 1200px)",
            [nameof(BreakpointEnum.xxl)] = "(max-width: 1600px)",
        };

        private async Task SetGutterStyle()
        {
            string breakPoint = null;

            await typeof(BreakpointEnum).GetEnumNames().ForEachAsync(async bp =>
            {
                if (await JsInvokeAsync<bool>(JSInteropConstants.MatchMedia, _gridResponsiveMap[bp]))
                {
                    Console.WriteLine(bp);
                    breakPoint = bp;
                }
            });

            var column = GetColumn(breakPoint);

            int columnCount = column > 0 ? column : (Grid?.Column ?? 0);
            if (Grid != null && columnCount > 0)
            {
                ColStyle = $"width:{100 / columnCount}%;max-width:{100 / columnCount}%";
            }

            InvokeStateHasChanged();
        }

        private int GetColumn(string breakPoint)
        {
            var column = 0;
            if (Grid != null && !string.IsNullOrEmpty(breakPoint))
            {
                var value = GetModelValue(breakPoint, Grid);
                int.TryParse(value, out column);
                return column;
            }
            return column;
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add(PrefixName)
                .If($"{PrefixName}-no-flex", () => NoFlex);
        }

        private bool IsFlexMode()
        {
            if (ItemLayout == AntDirectionVHType.Vertical)
            {
                return Extra != null;
            }

            return (Actions != null || Grid != null) && ItemCount > 1;
        }

        private string GetModelValue(string fieldName, object obj)
        {
            try
            {
                if (obj == null || string.IsNullOrEmpty(fieldName)) return null;
                var o = obj.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance).GetValue(obj, null);
                var value = o?.ToString() ?? null;
                if (string.IsNullOrEmpty(value)) return null;
                return value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void HandleClick()
        {
            if (OnClick.HasDelegate)
            {
                OnClick.InvokeAsync(this);
            }

            ItemClick?.Invoke();

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            DomEventService.RemoveEventListerner<object>("window", "resize", OnResize);
        }
    }
}
