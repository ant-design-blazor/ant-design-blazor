using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

#pragma warning disable 1591
#pragma warning disable CA1716

namespace AntDesign
{
    public partial class SelectOption : AntDomComponentBase
    {
        #region Private
        private string _label = null;
        private bool _isActive = false;
        private const string ClassPrefix = "ant-select-item-option";
        private ElementReference _contentRef;
        private bool IsSelected => SelectParent.OptionIsSelected(Value);

        #endregion Private

        #region Protected

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add("ant-select-item")
                .Add(ClassPrefix)
                .If($"{ClassPrefix}-disabled", () => Disabled)
                .If($"{ClassPrefix}-selected", () => IsSelected)
                .If($"{ClassPrefix}-active", () => _isActive)
                .If($"{ClassPrefix}-grouped", () => SelectOptGroupParent != null)
                .If(ClassName, () => !string.IsNullOrWhiteSpace(ClassName));
        }

        #region Properties
        protected string InnerStyle
        {
            get
            {
                if (SelectParent.IsShowOption(this))
                {
                    return Style;
                }
                else
                {
                    return Style + ";display:none";
                }
            }
        }
        #endregion

        #region Events
        protected override void OnInitialized()
        {
            SetClassMap();
            SelectParent?.AddOption(this);
            SelectOptGroupParent?.AddOption(this);
            base.OnParametersSet();
        }

        protected async override Task OnFirstAfterRenderAsync()
        {
            if (string.IsNullOrEmpty(Children))
            {
                Children = await JsInvokeAsync<string>(JSInteropConstants.getInnerText, _contentRef);
                await InvokeAsync(StateHasChanged);
            }
        }

        protected async Task OnSelectOptionClick(EventArgs _)
        {
            if (!Disabled)
            {
                SelectParent.IsPreventPenetration = true;
                await SelectParent.ToggleOrSetValue(Value);
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

        #endregion Events

        #endregion Protected

        #region Public

        #region Properties
        #region Generals
        public string Children { get; private set; } = string.Empty;
        #endregion

        #region Paramters
        [Parameter] public bool IsTagOption { get; set; } = false;

        [Parameter] public string Title { get; set; }

        [Parameter] public string Value { get; set; }

        [Parameter] public string ClassName { get; set; }

        [Parameter] public bool Disabled { get; set; } = false;

        [Parameter] public RenderFragment ChildContent { get; set; }

        [CascadingParameter] public Select SelectParent { get; set; }

        [CascadingParameter] public SelectOptGroup SelectOptGroupParent { get; set; }

        [Parameter] public string Label { get => _label ?? Children; set => _label = value; }
        #endregion
        #endregion
        #endregion
    }
}
