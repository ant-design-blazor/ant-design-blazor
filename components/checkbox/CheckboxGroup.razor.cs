using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class CheckboxGroup : AntInputComponentBase<string[]>
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public OneOf<CheckboxOption[], string[]> Options
        {
            get { return _options; }
            set
            {
                _options = value;
                _isOptionDefined = true;
            }
        }

        [Parameter]
        public CheckboxGroupMixedMode MixedMode
        {
            get { return _mixedMode; }
            set {
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

        [Parameter]
        public EventCallback<string[]> OnChange { get; set; }

        private string[] _selectedValues;
        private Func<string[]> _currentValue;
        private IList<Checkbox> _checkboxItems;
        private OneOf<CheckboxOption[], string[]> _options;
        private OneOf<CheckboxOption[], string[]> _constructedOptions;
        private bool _isOptionDefined;
        private bool _afterFirstRender;
        private int _indexConstructedOptionsOffset = -1;
        private int _indexSetOptionsOffset = -1;
        private CheckboxGroupMixedMode _mixedMode = CheckboxGroupMixedMode.ChildContentFirst;

        [Parameter]
        public bool Disabled { get; set; }

        public CheckboxGroup()
        {
            ClassMapper
                .Add("ant-checkbox-group")
                .If("ant-checkbox-group-rtl", () => RTL);
        }

        internal void AddItem(Checkbox checkbox)
        {
            this._checkboxItems ??= new List<Checkbox>();
            this._checkboxItems?.Add(checkbox);

            checkbox.IsFromOptions = IsCheckboxFromOptions(checkbox);
            if (!checkbox.IsFromOptions)
            {
                checkbox.SetValue(_selectedValues.Contains(checkbox.Label));
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
                        arr => arr.Contains(checkbox.Label));
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

        protected override void OnInitialized()
        {
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

            _selectedValues ??= Array.Empty<string>();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (ChildContent is not null && _checkboxItems.Count > 0)
                {
                    _constructedOptions = CreateConstructedOptions();
                }
                _currentValue = GetCurrentValueFunc();
                _afterFirstRender = true;
            }
            base.OnAfterRender(firstRender);
        }

        private OneOf<CheckboxOption[], string[]> CreateConstructedOptions()
        {
            if (Options.IsT0)
            {
                return _checkboxItems
                    .Where(c => !c.IsFromOptions)
                    .Select(c => new CheckboxOption { Label = c.Label, Value = c.Label, Checked = c.Value })
                    .ToArray();
            }
            return _checkboxItems
                .Where(c => !c.IsFromOptions)
                .Select(c => c.Label).ToArray();
        }

        private Func<string[]> GetCurrentValueFunc()
        {
            if (ChildContent is not null && _isOptionDefined)
            {
                return Options.Match<Func<string[]>>(
                    opt => () => opt.Where(x => x.Checked).Select(x => x.Value)
                                                .Union(_constructedOptions.AsT0.Where(x => x.Checked).Select(x => x.Value))
                                                .ToArray(),
                    arr => () => _selectedValues);
            }
            var workWith = (_isOptionDefined ? Options : _constructedOptions);
            return workWith.Match<Func<string[]>>(
                opt => () => opt.Where(x => x.Checked).Select(x => x.Value).ToArray(),
                arr => () => _selectedValues);

        }

        /// <summary>
        /// Called when [checkbox change].
        /// </summary>
        /// <param name="checkbox">The checkbox.</param>
        /// <returns></returns>
        internal void OnCheckboxChange(Checkbox checkbox)
        {
            var index = _checkboxItems.IndexOf(checkbox);
            int indexOffset;
            OneOf<CheckboxOption[], string[]> workWith;
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

            CurrentValue = _currentValue();
            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(CurrentValue);
            }

            StateHasChanged();
        }
    }
}
