// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class SelectOption<TItemValue, TItem>
    {
        private RenderFragment<TItem> _previousChildContent;
        private const string ClassPrefix = "ant-select-item-option";

        # region Parameters
        [CascadingParameter(Name = "InternalId")] internal Guid InternalId { get; set; }
        [CascadingParameter(Name = "ItemTemplate")] internal RenderFragment<TItem> ItemTemplate { get; set; }
        [CascadingParameter(Name = "Model")] internal SelectOptionItem<TItemValue, TItem> Model { get; set; }
        [CascadingParameter] internal SelectBase<TItemValue, TItem> SelectParent { get; set; }
        
    /// <summary>
    /// Optional child content of the SelectOption, used as label template for the option.
    /// If provided it will be used for rendering the option content (when ItemTemplate is not present)
    /// and it will be assigned to the underlying SelectOptionItem.LabelTemplate so the Select's
    /// selected-item display can reuse it when Select.LabelTemplate is not set.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem> ChildContent { get; set; }

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

        private SelectOptionItem<TItemValue, TItem> _optionItem;

        protected override async Task OnInitializedAsync()
        {
            bool isAlreadySelected = false;
            if (SelectParent?.HasSelectOptions != true)
            {
                // The SelectOptionItem was already created, now only the SelectOption has to be
                // bound to the SelectOptionItem.
                var item = SelectParent.SelectOptionItems.First(x => x.InternalId == InternalId);
                item.ChildComponent = this;
                // If this SelectOption provides a ChildContent label template, assign it to the underlying
                // SelectOptionItem so the Select can reuse it for the selected-item display when needed.
                if (ChildContent != null)
                {
                    item.LabelTemplate = ChildContent(item.Item);
                }
            }
            else if (Model is not null)
            {
                InternalId = Model.InternalId;
                Label = Model.Label;
                IsDisabled = Model.IsDisabled;
                GroupName = Model.GroupName;
                Value = Model.Value;
                Model.ChildComponent = this;
                if (ChildContent != null)
                {
                    Model.LabelTemplate = ChildContent(Model.Item);
                }
                isAlreadySelected = IsAlreadySelected(Model);
            }
            else
            {
                // The SelectOption was not created by using a DataSource, a SelectOptionItem must be created.
                InternalId = Guid.NewGuid();

                _optionItem = new SelectOptionItem<TItemValue, TItem>()
                {
                    InternalId = InternalId,
                    Label = Label,
                    IsDisabled = Disabled,
                    GroupName = _groupName,
                    Value = Value,
                    Item = _itemSet ? Item : THelper.ChangeType<TItem>(Value, CultureInfo.CurrentCulture),
                    ChildComponent = this
                };

                if (ChildContent != null)
                {
                    _optionItem.LabelTemplate = ChildContent(_optionItem.Item);
                }

                SelectParent.AddOptionItem(_optionItem);
                SelectParent.AddEqualityToNoValue(_optionItem);
                isAlreadySelected = IsAlreadySelected(_optionItem);
            }

            SetClassMap();
            await base.OnInitializedAsync();
            if (isAlreadySelected)
            {
                await SelectParent.ProcessSelectedSelectOptions();
            }
        }

        protected override Task OnParametersSetAsync()
        {
            // If ChildContent changed at runtime, update the underlying model's LabelTemplate
            if (!EqualityComparer<RenderFragment<TItem>>.Default.Equals(_previousChildContent, ChildContent))
            {
                _previousChildContent = ChildContent;

                if (ChildContent != null)
                {
                    // Update existing model if present
                    if (Model != null)
                    {
                        Model.LabelTemplate = ChildContent(Model.Item);
                    }

                    // If this SelectOption created its own option item, update it too
                    if (_optionItem != null)
                    {
                        _optionItem.LabelTemplate = ChildContent(_optionItem.Item);
                    }

                    // Notify parent to re-render selected display if necessary.
                    // StateHasChanged is protected on ComponentBase, so invoke it via reflection on the parent component.
                    // Ask the parent Select to refresh its selected display using an internal method
                    SelectParent?.RequestSelectedDisplayRefresh();
                }
            }

            return base.OnParametersSetAsync();
        }

        private bool IsAlreadySelected(SelectOptionItem<TItemValue, TItem> selectOption)
        {
            if (SelectParent.Mode == SelectMode.Default)
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
                .If($"{ClassPrefix}-grouped", () => (SelectParent as Select<TItemValue, TItem>)?.IsGroupingEnabled ?? false);

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

                if (SelectParent.Mode == SelectMode.Default)
                {
                    await SelectParent.CloseAsync();
                }
                else
                {
                    await SelectParent.UpdateOverlayPositionAsync();

                    SelectParent.FocusIfInSearch();
                }
            }
        }

        protected void OnMouseEnter()
        {
            SelectParent.ActiveOption = Model;
        }

        protected override void Dispose(bool disposing)
        {
            if (SelectParent?.HasSelectOptions == true)
            {
                // The SelectOptionItem must be explicitly removed if the SelectOption was not created using the DataSource .
                var selectOptionItem = SelectParent.SelectOptionItems
                    .FirstOrDefault(x => x.InternalId == InternalId && !x.IsSelected);
                if (selectOptionItem is not null)
                {
                    SelectParent.RemoveOptionItem(selectOptionItem);
                    SelectParent.RemoveEqualityToNoValue(selectOptionItem);
                }
            }

            base.Dispose(disposing);
        }
    }
}
