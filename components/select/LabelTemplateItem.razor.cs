using Microsoft.AspNetCore.Components;
using AntDesign.Select.Internal;

#pragma warning disable 1591 // Disable missing XML comment

namespace AntDesign.Select
{
    public partial class LabelTemplateItem<TItemValue, TItem>
    {
        [CascadingParameter(Name = "SelectContent")] private SelectContent<TItemValue, TItem> ParentSelectContent { get; set; }
        [CascadingParameter(Name = "SelectOption")] private SelectOptionItem<TItemValue, TItem> SelectOption { get; set; }
        [Parameter] public RenderFragment<TItem> LabelTemplateItemContent { get; set; }
        [Parameter] public string Style { get; set; }
        [Parameter] public string Class { get; set; }
        [Parameter] public string ContentStyle { get; set; }
        [Parameter] public string ContentClass { get; set; }
        [Parameter] public string RemoveIconStyle { get; set; }
        [Parameter] public string RemoveIconClass { get; set; }
        [Parameter] public ForwardRef RefBack { get; set; } = new ForwardRef();

        private ElementReference _ref;

        /// <summary>
        /// Returned ElementRef reference for DOM element.
        /// </summary>
        public virtual ElementReference Ref
        {
            get => _ref;
            set
            {
                _ref = value;
                RefBack?.Set(value);
            }
        }

        protected override void OnInitialized()
        {
            if (ParentSelectContent.ParentSelect.SelectMode == SelectMode.Default
                && string.IsNullOrWhiteSpace(Class)
                && string.IsNullOrWhiteSpace(ContentStyle))
            {
                Class = "ant-select-selection-item";
                ContentStyle = "ant-select-selection-item-content";
            }
        }
    }
}
