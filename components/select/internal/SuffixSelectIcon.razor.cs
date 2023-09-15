using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign.Select.Internal
{
    public partial class SuffixSelectIcon<TItemValue, TItem>
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
    }
}
