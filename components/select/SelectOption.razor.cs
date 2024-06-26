// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AntDesign.Select.Internal;
using System.Globalization;
using System.Collections.Generic;

namespace AntDesign
{
    public partial class SelectOption<TItemValue, TItem>
    {
        private const string ClassPrefix = "ant-select-item-option";

        # region Parameters
        [CascadingParameter(Name = "InternalId")] internal Guid InternalId { get; set; }
        [CascadingParameter(Name = "ItemTemplate")] internal RenderFragment<TItem> ItemTemplate { get; set; }
        [CascadingParameter(Name = "Model")] internal SelectOptionItem<TItemValue, TItem> Model { get; set; }
        [CascadingParameter] internal Select<TItemValue, TItem> SelectParent { get; set; }

        /// <summary>
        /// Disable this option
        /// The parameter should only be used if the SelectOption was created directly.
        /// </summary>
        [Parameter]
        public bool Disabled
        {
            get => _disabled;
            set
            {
                if (_disabled != value)
                {
                    _disabled = value;

                    if (Model != null)
                        Model.IsDisabled = value;
                }
            }
        }

        /// <summary>
        /// Label of Select after selecting this Option
        /// The parameter should only be used if the SelectOption was created directly.
        /// </summary>
        [Parameter]
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

        /// <summary>
        /// Value of Select after selecting this Option
        /// The parameter should only be used if the SelectOption was created directly.
        /// </summary>
        [Parameter] public TItemValue Value { get; set; }
        /// <summary>
        /// Item of the SelectOption
        /// The parameter should only be used if the SelectOption was created directly.
        /// </summary>

        private bool _itemSet;
        private TItem _item;

        [Parameter]
        public TItem Item
        {
            get => _item;
            set
            {
                _item = value;
                _itemSet = true;
            }
        }
        #endregion

        # region Properties
        private string _label = string.Empty;
        private bool _disabled;

        private bool _isSelected;

        internal bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    StateHasChanged();
                }
            }
        }

        private bool _isDisabled;

        internal bool IsDisabled
        {
            get => _isDisabled;
            set
            {
                if (_isDisabled != value)
                {
                    _isDisabled = value;
                    StateHasChanged();
                }
            }
        }

        private bool _isHidden;

        internal bool IsHidden
        {
            get => _isHidden;
            set
            {
                if (_isHidden != value)
                {
                    _isHidden = value;
                    StateHasChanged();
                }
            }
        }

        private bool _isActive;

        internal bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    StateHasChanged();
                }
            }
        }

        private string _internalLabel = string.Empty;

        internal string InternalLabel
        {
            get => _internalLabel;
            set
            {
                if (_internalLabel != value)
                {
                    _internalLabel = value;
                    StateHasChanged();
                }
            }
        }

        private string _groupName = string.Empty;

        internal string GroupName
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
            bool isAlreadySelected = false;
            if (SelectParent.SelectOptions == null)
            {
                // The SelectOptionItem was already created, now only the SelectOption has to be
                // bound to the SelectOptionItem.
                var item = SelectParent.SelectOptionItems.First(x => x.InternalId == InternalId);
                item.ChildComponent = this;
            }
            else if (Model is not null)
            {
                InternalId = Model.InternalId;
                Label = Model.Label;
                IsDisabled = Model.IsDisabled;
                GroupName = Model.GroupName;
                Value = Model.Value;
                Model.ChildComponent = this;
                isAlreadySelected = IsAlreadySelected(Model);
            }
            else
            {
                // The SelectOption was not created by using a DataSource, a SelectOptionItem must be created.
                InternalId = Guid.NewGuid();

                var newSelectOptionItem = new SelectOptionItem<TItemValue, TItem>()
                {
                    InternalId = InternalId,
                    Label = Label,
                    IsDisabled = Disabled,
                    GroupName = _groupName,
                    Value = Value,
                    Item = _itemSet ? Item : THelper.ChangeType<TItem>(Value, CultureInfo.CurrentCulture),
                    ChildComponent = this
                };

                SelectParent.SelectOptionItems.Add(newSelectOptionItem);
                SelectParent.AddEqualityToNoValue(newSelectOptionItem);
                isAlreadySelected = IsAlreadySelected(newSelectOptionItem);
            }

            SetClassMap();
            await base.OnInitializedAsync();
            if (isAlreadySelected)
            {
                await SelectParent.ProcessSelectedSelectOptions();
            }
        }

        private bool IsAlreadySelected(SelectOptionItem<TItemValue, TItem> selectOption)
        {
            if (SelectParent.Mode == "default")
            {
                return EqualityComparer<TItemValue>.Default.Equals(selectOption.Value, SelectParent.Value) ||
                    EqualityComparer<TItemValue>.Default.Equals(selectOption.Value, SelectParent.LastValueBeforeReset);
            }
            else
            {
                return SelectParent.Values is null || SelectParent.Values.Contains(selectOption.Value);
            }
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
            SelectParent.ActiveOption = Model;
        }

        protected override void Dispose(bool disposing)
        {
            if (SelectParent?.SelectOptions != null)
            {
                // The SelectOptionItem must be explicitly removed if the SelectOption was not created using the DataSource .
                var selectOptionItem = SelectParent.SelectOptionItems
                    .FirstOrDefault(x => x.InternalId == InternalId && !x.IsSelected);
                if (selectOptionItem is not null)
                {
                    SelectParent.SelectOptionItems.Remove(selectOptionItem);
                    SelectParent.RemoveEqualityToNoValue(selectOptionItem);
                }
            }

            base.Dispose(disposing);
        }
    }
}
