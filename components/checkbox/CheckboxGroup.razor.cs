// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// Display a group of related checkboxes
    /// </summary>
#if NET6_0_OR_GREATER
    [CascadingTypeParameter(nameof(TValue))]
#endif

    public partial class CheckboxGroup<TValue> : AntInputComponentBase<TValue[]>, ICheckboxGroup
    {
        /// <summary>
        /// Display content in the group. Use <see cref="MixedMode"/> to specify where this should render if using with <see cref="Options"/>
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Options for checkboxes
        /// </summary>
        [Parameter]
        public OneOf<CheckboxOption<TValue>[], TValue[]> Options
        {
            get { return _options; }
            set
            {
                _options = value;
                _isOptionDefined = true;
                if (_afterFirstRender)
                {
                    _currentValue = GetCurrentValueFunc();
                }
            }
        }

        /// <summary>
        /// Button style for the group
        /// </summary>
        /// <default value="CheckboxButtonStyle.Outline" />
        [Parameter]
        public CheckboxButtonStyle? ButtonStyle { get; set; }

        /// <summary>
        /// When both <see cref="ChildContent"/> and <see cref="Options"/> are used this specifies which should render first.
        /// </summary>
        /// <default value="CheckboxGroupMixedMode.ChildContentFirst"/>
        [Parameter]
        public CheckboxGroupMixedMode MixedMode
        {
            get { return _mixedMode; }
            set
            {
                bool isChanged = _afterFirstRender && _mixedMode != value;
                _mixedMode = value;
                if (isChanged)
                {
                    //were changed by RemoveItem
                    _indexConstructedOptionsOffset = -1; //force recalculation
                    _indexSetOptionsOffset = 0;
                }
            }
        }

        /// <summary>
        /// Callback executed when the checked options change
        /// </summary>
        [Parameter]
        public EventCallback<TValue[]> OnChange { get; set; }

        /// <summary>
        /// Disable all checkboxes in the group
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Disabled { get; set; }

        bool ICheckboxGroup.Disabled => Disabled;

        string ICheckboxGroup.NameAttributeValue => NameAttributeValue;

        private TValue[] _selectedValues;
        private Func<TValue[]> _currentValue;
        private IList<Checkbox> _checkboxItems;
        private OneOf<CheckboxOption<TValue>[], TValue[]> _options;
        private OneOf<CheckboxOption<TValue>[], TValue[]> _constructedOptions;
        private bool _isOptionDefined;
        private bool _afterFirstRender;
        private int _indexConstructedOptionsOffset = -1;
        private int _indexSetOptionsOffset = -1;
        private CheckboxGroupMixedMode _mixedMode = CheckboxGroupMixedMode.ChildContentFirst;

        private static readonly Dictionary<CheckboxButtonStyle, string> _buttonStyleDics = new()
        {
            [CheckboxButtonStyle.Outline] = "outline",
            [CheckboxButtonStyle.Solid] = "solid",
        };

        protected override void OnInitialized()
        {
            string prefixCls = "ant-radio-group";
            ClassMapper
                .Add(prefixCls)
                .If($"{prefixCls}-large", () => Size == ButtonSize.Large)
                .If($"{prefixCls}-small", () => Size == ButtonSize.Small)
                .GetIf(() => $"{prefixCls}-{_buttonStyleDics[ButtonStyle.Value]}", () => ButtonStyle.HasValue && ButtonStyle.IsIn(CheckboxButtonStyle.Outline, CheckboxButtonStyle.Solid))
                .If($"{prefixCls}-rtl", () => RTL);

            if (ChildContent is null && MixedMode == CheckboxGroupMixedMode.ChildContentFirst)
                MixedMode = CheckboxGroupMixedMode.OptionsFirst;

            base.OnInitialized();

            if (Value != null)
            {
                _selectedValues = Value;
                if (Options.IsT0)
                {
                    Options.AsT0.ForEach(opt => opt.Checked = opt.Value.IsIn(_selectedValues));
                }
            }

            _selectedValues ??= Array.Empty<TValue>();
        }

        internal void AddItem(Checkbox checkbox)
        {
            this._checkboxItems ??= new List<Checkbox>();
            this._checkboxItems?.Add(checkbox);

            checkbox.IsFromOptions = IsCheckboxFromOptions(checkbox);
            if (!checkbox.IsFromOptions)
            {
                checkbox.SetValue(_selectedValues.Any(x => x.ToString() == checkbox.Label));
                checkbox.SetItemValue(checkbox.Label);

                if (_indexConstructedOptionsOffset == -1)
                    _indexConstructedOptionsOffset = _checkboxItems.Count - 1;
            }
            else if (checkbox.IsFromOptions && _indexSetOptionsOffset == -1)
                _indexSetOptionsOffset = _checkboxItems.Count - 1;
        }

        private bool IsCheckboxFromOptions(Checkbox checkbox)
        {
            if (Options.Value is not null)
            {
                if (ChildContent is not null)
                {
                    return Options.Match(
                        opt => opt.Any(o => o.Label.Equals(checkbox.Label)),
                        arr => arr.Contains((TValue)checkbox.ItemValue));
                }
                return true;
            }
            return false;
        }

        internal void RemoveItem(Checkbox checkbox)
        {
            this._checkboxItems?.Remove(checkbox);
            if (!checkbox.IsFromOptions && _indexConstructedOptionsOffset >= 0)
                _indexConstructedOptionsOffset--;
            else if (checkbox.IsFromOptions && _indexSetOptionsOffset >= 0)
                _indexSetOptionsOffset--;
        }

        protected override void OnValueChange(TValue[] value)
        {
            base.OnValueChange(value);

            if (Disabled)
            {
                return;
            }

            _selectedValues = value;

            if (Options.Value != null && Options.IsT0)
            {
                Options.AsT0.ForEach(x =>
                {
                    if (!x.Disabled)
                    {
                        x.Checked = x.Value.IsIn(_selectedValues);
                    }
                });
            }
            else
            {
                _checkboxItems.ForEach(x =>
                {
                    var checkBoxValue = ((TValue)x.ItemValue).IsIn(value);
                    x.SetValue(checkBoxValue);
                });
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (ChildContent is not null && _checkboxItems?.Count > 0)
                {
                    _constructedOptions = CreateConstructedOptions();
                }
                _currentValue = GetCurrentValueFunc();
                _afterFirstRender = true;
            }
            base.OnAfterRender(firstRender);
        }

        private OneOf<CheckboxOption<TValue>[], TValue[]> CreateConstructedOptions()
        {
            if (Options.Value is not null && Options.IsT0)
            {
                return _checkboxItems
                    .Where(c => !c.IsFromOptions)
                    .Select(c => new CheckboxOption<TValue> { Label = c.Label, Value = (TValue)c.ItemValue, Checked = c.Value })
                    .ToArray();
            }
            return _checkboxItems
                .Where(c => !c.IsFromOptions)
                .Select(c => c.ItemValue ?? c.Label).Cast<TValue>().ToArray();
        }

        private Func<TValue[]> GetCurrentValueFunc()
        {
            if (ChildContent is not null && _isOptionDefined)
            {
                return Options.Match<Func<TValue[]>>(
                    opt => () => opt.Where(x => x.Checked).Select(x => x.Value)
                                                .Union(_constructedOptions.AsT0.Where(x => x.Checked).Select(x => x.Value))
                                                .ToArray(),
                    arr => () => _selectedValues);
            }
            var workWith = (_isOptionDefined ? Options : _constructedOptions);
            return workWith.Match<Func<TValue[]>>(
                opt => () => opt.Where(x => x.Checked).Select(x => x.Value).ToArray(),
                arr => () => _selectedValues);
        }

        /// <summary>
        /// Called when [checkbox change].
        /// </summary>
        /// <param name="checkbox">The checkbox.</param>
        /// <param name="invokeOnChange">Flag for whether or not to depart for a change event.</param>
        /// <returns></returns>
        internal void OnCheckboxChange(Checkbox checkbox, bool invokeOnChange = true)
        {
            var index = _checkboxItems.IndexOf(checkbox);
            int indexOffset;
            OneOf<CheckboxOption<TValue>[], TValue[]> workWith;
            if (checkbox.IsFromOptions)
            {
                indexOffset = _indexSetOptionsOffset;
                workWith = Options;
            }
            else
            {
                indexOffset = _indexConstructedOptionsOffset;
                workWith = _constructedOptions;
            }

            workWith.Switch(opts =>
            {
                if (opts[index] != null)
                {
                    opts[index].Checked = checkbox.Checked;
                }
            }, opts =>
            {
                if (checkbox.Checked && !opts[index - indexOffset].IsIn(_selectedValues))
                {
                    _selectedValues = _selectedValues.Append(opts[index - indexOffset]);
                }
                else
                {
                    _selectedValues = _selectedValues.Except(new[] { opts[index - indexOffset] }).ToArray();
                }
            });

            if (invokeOnChange)
            {
                CurrentValue = _currentValue();

                InvokeValueChange();
            }
        }

        private void InvokeValueChange()
        {
            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(CurrentValue);
            }

            StateHasChanged();
        }

        void ICheckboxGroup.AddItem(Checkbox checkbox) => AddItem(checkbox);

        void ICheckboxGroup.OnCheckboxChange(Checkbox checkbox) => OnCheckboxChange(checkbox);

        void ICheckboxGroup.RemoveItem(Checkbox checkbox) => RemoveItem(checkbox);
    }
}
