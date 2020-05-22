using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

#pragma warning disable 1591
#pragma warning disable CA1716
// ReSharper disable once CheckNamespace

namespace AntBlazor
{
    public partial class SelectOption : AntDomComponentBase
    {
        #region Private
        private bool _isActive = false;
        private const string ClassPrefix = "ant-select-item";

        private bool IsSelected => SelectParent.OptionIsSelected(Value);
        #endregion

        #region Protected
        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add(ClassPrefix)
                .Add($"{ClassPrefix}-option")
                .If($"{ClassPrefix}-option-disabled", () => Disabled)
                .If($"{ClassPrefix}-option-selected", () => IsSelected)
                .If($"{ClassPrefix}-option-active", () => _isActive)
                .If(ClassName, () => !string.IsNullOrWhiteSpace(ClassName));
        }

        protected override void OnInitialized()
        {
            SetClassMap();
            SelectParent?.AddOption(this);
            SelectOptGroupParent?.AddOption(this);
            base.OnParametersSet();
        }

        #region Events
        protected async Task OnSelectOptionClick(EventArgs _)
        {
            if (!Disabled)
            {
                await SelectParent.AddOrSetValue(Value);
                await InvokeAsync(StateHasChanged);
            }
        }

        protected virtual async Task OnSelectOptionMouseEnter()
        {
            _isActive = true;
            SetClassMap();
            await InvokeAsync(StateHasChanged);
        }

        protected virtual async Task OnSelectOptionMouseLeave()
        {
            _isActive = false;
            SetClassMap();
            await InvokeAsync(StateHasChanged);
        }
        #endregion
        #endregion

        #region Public
        #region Properties
        #region Paramters
        [Parameter] public string Title { get; set; }

        [Parameter] public string Value { get; set; }

        [Parameter] public string ClassName { get; set; }

        [Parameter] public bool Disabled { get; set; } = false;


        [Parameter] public RenderFragment ChildContent { get; set; }


        [CascadingParameter] public Select SelectParent { get; set; }
        [CascadingParameter] public SelectOptGroup SelectOptGroupParent { get; set; }
        #endregion
        #endregion
        #endregion
    }
}
