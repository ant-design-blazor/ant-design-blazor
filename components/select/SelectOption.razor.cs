using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

#pragma warning disable 1591
#pragma warning disable CA1716

namespace AntDesign
{
    public partial class SelectOption : AntDomComponentBase
    {

        #region Paramters
        [Parameter] public bool IsTag { get; set; } = false;

        [Parameter] public bool IsSearch { get; set; } = false;

        [Parameter] public string Title { get; set; }

        [Parameter] public string Value { get; set; }

        [Parameter] public string ClassName { get; set; }

        [Parameter] public bool Disabled { get; set; } = false;

        [Parameter] public RenderFragment ChildContent { get; set; }

        [CascadingParameter] public Select SelectParent { get; set; }

        [CascadingParameter] public SelectOptGroup SelectOptGroupParent { get; set; }

        [Parameter] public string Label { get => _label ?? Children; set => _label = value; }
        #endregion

        #region Private
        private string _label = null;
        private bool _isActive = false;
        private const string ClassPrefix = "ant-select-item-option";
        private ElementReference _contentRef;
        private bool IsSelected => IsTag || (!IsSearch && SelectParent.OptionIsSelected(Value));

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
                if (IsSearch)
                {
                    return Style;
                }

                if (SelectParent.IsShowOption(this))
                {
                    return Style;
                }

                return Style + ";display:none";
            }
        }
        #endregion

        #region Events
        protected override void Dispose(bool disposing)
        {
            SelectParent?.RemoveOption(this);
            SelectOptGroupParent?.RemoveOption(this);
            base.Dispose(disposing);
        }

        protected override async Task OnInitializedAsync()
        {
            SetClassMap();

            SelectParent?.AddOption(this);
            SelectOptGroupParent?.AddOption(this);

            await base.OnInitializedAsync();
        }

        protected override void OnInitialized()
        {
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            if (string.IsNullOrEmpty(Children))
            {
                await Task.Delay(1);
                Children = await JsInvokeAsync<string>(JSInteropConstants.GetInnerText, _contentRef);
            }
        }

        protected async Task OnSelectOptionClick(EventArgs _)
        {
            if (!Disabled)
            {
                await Task.Delay(1);
                await SelectParent.ToggleOrSetValue(Value);

                if (SelectParent.SelectMode == SelectMode.Default)
                {
                    await SelectParent.Close();
                }
            }
        }

        protected virtual async Task OnSelectOptionMouseEnter()
        {
            _isActive = true;
            SetClassMap();
        }

        protected virtual async Task OnSelectOptionMouseLeave()
        {
            _isActive = false;
            SetClassMap();
        }
        #endregion Events

        #region Methods
        internal void SearchToTag()
        {
            if (IsSearch)
            {
                IsTag = true;
                IsSearch = false;
            }
        }
        #endregion
        #endregion Protected

        #region Public
        #region Properties
        #region Generals
        public string Children { get; private set; } = string.Empty;
        #endregion

        #endregion
        #endregion

        public override bool Equals(object obj)
        {
            SelectOption compareObj = obj as SelectOption;

            if (compareObj == null)
            {
                return false;
            }

            return this.Value == compareObj.Value;
        }
    }
}
