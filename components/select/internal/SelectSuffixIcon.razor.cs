using System;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign.Select.Internal
{
    public partial class SelectSuffixIcon<TItemValue, TItem>
    {
        [Parameter] public bool IsOverlayShow { get; set; }

        /// <summary>
        /// Whether show search input in single mode.
        /// </summary>
        [Parameter]
        public bool ShowSearchIcon { get; set; }

        [Parameter]
        public bool ShowArrowIcon { get; set; } 

        [Parameter]
        public EventCallback<MouseEventArgs> OnClearClick { get; set; } 
        
        [CascadingParameter(Name = "ParentSelect")] internal SelectBase<TItemValue, TItem> ParentSelect { get; set; }

        [Inject] private IDomEventListener DomEventListener { get; set; }

        private string _id = "";
        private ElementReference _clearRef;
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _id = _clearRef.Id;
                if (!ParentSelect.Disabled && ParentSelect.AllowClear && ParentSelect.HasValue)
                {
                    DomEventListener.AddExclusive<JsonElement>(_clearRef, "click", OnClear, true, true);
                }
            }

            if (_clearRef.Id != _id)
            {
                _id = _clearRef.Id;
                if (!ParentSelect.Disabled && ParentSelect.AllowClear && ParentSelect.HasValue)
                {
                    DomEventListener.AddExclusive<JsonElement>(_clearRef, "click", OnClear, true, true);
                }
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        private async void OnClear(JsonElement jsonElement)
        {
            if (OnClearClick.HasDelegate)
            {
                await OnClearClick.InvokeAsync(null);
            }
        }
    }
}
