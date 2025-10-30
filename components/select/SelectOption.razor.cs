// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class SelectOption<TItemValue, TItem>
    {
        private System.Reflection.MethodInfo _previousChildContentMethod;
        private const string ClassPrefix = "ant-select-item-option";

        # region Parameters
        [CascadingParameter(Name = "InternalId")] internal Guid InternalId { get; set; }
        [CascadingParameter(Name = "ItemTemplate")] internal RenderFragment<TItem> ItemTemplate { get; set; }
        [CascadingParameter(Name = "Model")] internal SelectOptionItem<TItemValue, TItem> Model { get; set; }
        [CascadingParameter] internal SelectBase<TItemValue, TItem> SelectParent { get; set; }

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

        /// <summary>
        /// Optional child content of the SelectOption, used as label template for the option.
        /// If provided it will be used for rendering the option content (when ItemTemplate is not present)
        /// and it will be assigned to the underlying SelectOptionItem.LabelTemplate so the Select's
        /// selected-item display can reuse it when Select.LabelTemplate is not set.
        /// </summary>
        [PublicApi("1.5.0")]
        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

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
                    // ChildContent is a template for the item itself (TItem). Build a null-safe label template
                    // that falls back to the explicit Label or Value when Item is null.
                    item.LabelTemplate = BuildLabelTemplateFromChildContent(item);
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
                    Model.LabelTemplate = BuildLabelTemplateFromChildContent(Model);
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
                    // Don't forcibly convert Value -> TItem. If Item was explicitly set, use it;
                    // otherwise leave Item as default to avoid invalid conversions (e.g. string -> complex type).
                    Item = _itemSet ? Item : (Value is TItem v ? v : default),
                    ChildComponent = this
                };

                if (ChildContent != null)
                {
                    _optionItem.LabelTemplate = BuildLabelTemplateFromChildContent(_optionItem);
                }
                else if (string.IsNullOrEmpty(_optionItem.Label))
                {
                    // Fallback label when no Item is available
                    _optionItem.Label = Value?.ToString() ?? string.Empty;
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
            // Compare the incoming ChildContent by its MethodInfo (stable across re-renders)
            // rather than by delegate instance equality which can change on each render
            // and cause an infinite refresh loop when we request parent refresh.
            var currentMethod = ((Delegate)ChildContent)?.Method;

            if (!Equals(_previousChildContentMethod, currentMethod))
            {
                _previousChildContentMethod = currentMethod;

                if (ChildContent != null)
                {
                    // Update existing model if present
                    if (Model != null)
                    {
                        Model.LabelTemplate = BuildLabelTemplateFromChildContent(Model);
                    }

                    // If this SelectOption created its own option item, update it too
                    if (_optionItem != null)
                    {
                        _optionItem.LabelTemplate = BuildLabelTemplateFromChildContent(_optionItem);
                    }

                    // Only request parent to refresh selected display when needed.
                    // Requesting refresh unconditionally on each render can cause
                    // repeated re-renders if the parent recreates the ChildContent delegate
                    // even though the template method hasn't changed.
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

        /// <summary>
        /// Build a RenderFragment to be used as the SelectOptionItem.LabelTemplate based on the
        /// ChildContent template. If the option's Item is null, fall back to Label or Value.ToString().
        /// </summary>
        private RenderFragment BuildLabelTemplateFromChildContent(SelectOptionItem<TItemValue, TItem> option)
        {
            if (option is null)
            {
                return null;
            }

            // If there is no child template provided, nothing to build here.
            if (ChildContent == null)
            {
                return null;
            }

            // If the Item is null (reference type or nullable), return a simple text fragment
            // that uses the explicit Label if present, otherwise the Value's ToString().
            if (option.Item is null)
            {
                var text = !string.IsNullOrEmpty(option.Label) ? option.Label : (option.Value?.ToString() ?? string.Empty);
                return builder => builder.AddContent(0, text);
            }

            // Otherwise, invoke the ChildContent template with the actual item.
            return ChildContent(option.Item);
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
