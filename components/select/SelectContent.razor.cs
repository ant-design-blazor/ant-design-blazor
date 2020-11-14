using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

#pragma warning disable 1591 // Disable missing XML comment

namespace AntDesign
{
    public partial class SelectContent<TItemValue, TItem>
    {
        [CascadingParameter(Name = "ParentSelect")] public Select<TItemValue, TItem> ParentSelect { get; set; }
        [CascadingParameter(Name = "ParentLabelTemplate")] internal RenderFragment<TItem> ParentLabelTemplate { get; set; }
        [CascadingParameter(Name = "ShowSearchIcon")] private bool ShowSearchIcon { get; set; }
        [CascadingParameter(Name = "ShowArrowIcon")] private bool ShowArrowIcon { get; set; }

        [Parameter] public string Prefix { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public bool IsOverlayShow { get; set; }
        [Parameter] public bool ShowPlaceholder { get; set; }
        [Parameter] public EventCallback<ChangeEventArgs> OnInput { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnBlur { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClearClick { get; set; }
        [Parameter] public EventCallback<SelectOptionItem<TItemValue, TItem>> OnRemoveSelected { get; set; }
        [Parameter] public string SearchValue { get; set; }

        private string _inputStyle = string.Empty;
        private string _inputWidth;
        private bool _suppressInput;

        protected override void OnInitialized()
        {
            SetSuppressInput();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            SetSuppressInput();

            return base.OnAfterRenderAsync(firstRender);
        }

        protected override Task OnParametersSetAsync()
        {
            if (ParentSelect.SelectMode != SelectMode.Default) // ToDo Fix class
                SetInputWidth();

            return base.OnParametersSetAsync();
        }

        private void SetInputWidth()
        {
            if (!string.IsNullOrWhiteSpace(SearchValue))
            {
                _inputWidth = $"width: {4 + SearchValue.Length * 8}px;";
            }
            else
            {
                if (ParentSelect.HasValue)
                {
                    _inputWidth = "width: 4px;"; //ToDo fix class 
                }
                else
                {
                    _inputWidth = "width: 4px; margin-left: 6.5px;"; //ToDo fix class     
                }
            }
        }

        private void SetSuppressInput()
        {
            if (!ParentSelect.IsSearchEnabled)
            {
                if (!_suppressInput)
                {
                    _suppressInput = true;
                    _inputStyle = "caret-color: transparent;";
                }
            }
            else
            {
                if (_suppressInput)
                {
                    _suppressInput = false;
                    _inputStyle = string.Empty;
                }
            }
        }

        protected void OnKeyPressEventHandler(KeyboardEventArgs _)
        {
            if (!ParentSelect.IsSearchEnabled)
                SearchValue = string.Empty;
        }

        private Dictionary<string, object> AdditonalAttributes()
        {
            var dict = new Dictionary<string, object>();

            if (ParentSelect.Disabled)
                dict.Add("tabindex", "-1");

            return dict;
        }
    }
}
