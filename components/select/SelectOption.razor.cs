using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

#pragma warning disable 1591 // Disable missing XML comment

namespace AntDesign
{
    public partial class SelectOption<TItemValue, TItem>
    {
        private const string ClassPrefix = "ant-select-item-option";

        # region Parameters
        [CascadingParameter(Name = "ItemTemplate")] internal RenderFragment<TItem> ItemTemplate { get; set; }
        [CascadingParameter] public Select<TItemValue, TItem> SelectParent { get; set; }
        [Parameter] public Guid InternalId { get; set; }
        #endregion

        # region Properties
        public SelectOptionItem<TItemValue, TItem> Model { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    SetClassMap();
                }
            }
        }

        private bool _isDisabled;
        public bool IsDisabled
        {
            get => _isDisabled;
            set
            {
                if (_isDisabled != value)
                {
                    _isDisabled = value;
                    SetClassMap();
                }
            }
        }

        private bool _isHidden;
        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                if (_isHidden != value)
                {
                    _isHidden = value;
                    SetClassMap();
                }
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    SetClassMap();
                }
            }
        }

        private string _label = string.Empty;
        public string Label
        {
            get => _label;
            set
            {
                if (_label != value)
                {
                    _label = value;
                    StateHasChanged();
                }
            }
        }

        private string _groupName = string.Empty;
        public string GroupName
        {
            get => _groupName;
            set
            {
                if (_groupName != value)
                {
                    _groupName = value;
                    StateHasChanged();
                }
            }
        }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            var item = SelectParent.SelectOptions.First(x => x.InternalId == InternalId);
            item.ChildComponent = this;

            SetClassMap();

            await base.OnInitializedAsync();
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add("ant-select-item")
                .Add(ClassPrefix)
                .If($"{ClassPrefix}-disabled", () => IsDisabled)
                .If($"{ClassPrefix}-selected", () => IsSelected)
                .If($"{ClassPrefix}-active", () => IsActive)
                .If($"{ClassPrefix}-grouped", () => SelectParent.IsGroupingEnabled);
            StateHasChanged();
        }

        protected string InnerStyle
        {
            get
            {
                if (!IsHidden)
                    return Style;

                return Style + ";display:none";
            }
        }

        protected async Task OnClick(EventArgs _)
        {
            if (!IsDisabled)
            {
                await SelectParent.SetValueAsync(Model);

                if (SelectParent.SelectMode == SelectMode.Default)
                {
                    await SelectParent.CloseAsync();
                }
                else
                {
                    await SelectParent.UpdateOverlayPositionAsync();
                }
            }
        }

        protected void OnMouseEnter()
        {
            // Workaround to prevent double active items if the actual active item was set by keyboard
            SelectParent.SelectOptions.Where(x => x.IsActive)
                .ForEach(i => i.IsActive = false);

            Model.IsActive = true;
        }

        protected void OnMouseLeave()
        {
            Model.IsActive = false;
        }
    }
}
