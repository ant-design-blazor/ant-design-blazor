using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public abstract class SelectBase<TItemValue, TItem> : AntInputComponentBase<TItemValue>
    {
        [Parameter] public bool AllowClear { get; set; }
        [Parameter] public bool AutoClearSearchValue { get; set; } = true;
        [Parameter] public bool Disabled { get; set; }

        [Parameter] public string Mode { get; set; } = "default";

        [Parameter] public bool EnableSearch { get; set; }
        [Parameter] public bool Loading { get; set; }
        [Parameter] public bool Open { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public Action OnFocus { get; set; }
        [Parameter] public SortDirection SortByGroup { get; set; } = SortDirection.None;
        [Parameter] public SortDirection SortByLabel { get; set; } = SortDirection.None;
        [Parameter] public RenderFragment SuffixIcon { get; set; }
        [Parameter] public bool HideSelected { get; set; }
        [Parameter] public override EventCallback<TItemValue> ValueChanged { get; set; }
        [Parameter] public EventCallback<IEnumerable<TItemValue>> ValuesChanged { get; set; }

        internal HashSet<SelectOptionItem<TItemValue, TItem>> SelectOptionItems { get; } = new HashSet<SelectOptionItem<TItemValue, TItem>>();

        internal SelectMode SelectMode => Mode.ToSelectMode();

        /// <summary>
        /// Determines if SelectOptions has any selected items
        /// </summary>
        /// <returns>true if SelectOptions has any selected Items, otherwise false</returns>
        internal bool HasValue => SelectOptionItems.Where(x => x.IsSelected).Any();

        /// <summary>
        /// Returns the value of EnableSearch parameter
        /// </summary>
        /// <returns>true if search is enabled</returns>
        internal bool IsSearchEnabled => EnableSearch;

        /// <summary>
        /// Sorted list of SelectOptionItems
        /// </summary>
        protected internal IEnumerable<SelectOptionItem<TItemValue, TItem>> SortedSelectOptionItems
        {
            get
            {
                var selectOption = SelectOptionItems;

                if (SortByGroup == SortDirection.Ascending && SortByLabel == SortDirection.None)
                {
                    return selectOption.OrderBy(g => g.GroupName);
                }
                else if (SortByGroup == SortDirection.Descending && SortByLabel == SortDirection.None)
                {
                    return selectOption.OrderByDescending(g => g.GroupName);
                }
                else if (SortByGroup == SortDirection.None && SortByLabel == SortDirection.Ascending)
                {
                    return selectOption.OrderBy(l => l.Label);
                }
                else if (SortByGroup == SortDirection.None && SortByLabel == SortDirection.Descending)
                {
                    return selectOption.OrderByDescending(l => l.Label);
                }
                else if (SortByGroup == SortDirection.Ascending && SortByLabel == SortDirection.Ascending)
                {
                    return selectOption.OrderBy(g => g.GroupName).ThenBy(l => l.Label);
                }
                else if (SortByGroup == SortDirection.Ascending && SortByLabel == SortDirection.Descending)
                {
                    return selectOption.OrderBy(g => g.GroupName).OrderByDescending(l => l.Label);
                }
                else if (SortByGroup == SortDirection.Descending && SortByLabel == SortDirection.Ascending)
                {
                    return selectOption.OrderByDescending(g => g.GroupName).ThenBy(l => l.Label);
                }
                else if (SortByGroup == SortDirection.Descending && SortByLabel == SortDirection.Descending)
                {
                    return selectOption.OrderByDescending(g => g.GroupName).OrderByDescending(l => l.Label);
                }
                else
                {
                    return selectOption;
                }
            }
        }

        internal ElementReference _inputRef;
        internal bool Focused { get; set; }
        protected string _searchValue = string.Empty;
        protected OverlayTrigger _dropDown;
        protected bool _isInitialized;
        protected const string DefaultWidth = "width: 100%;";

        /// <summary>
        /// Returns a true/false if the placeholder should be displayed or not.
        /// </summary>
        /// <returns>true if SelectOptions has no values and the searchValue is empty; otherwise false </returns>
        protected bool ShowPlaceholder
        {
            get
            {
                return !HasValue && string.IsNullOrEmpty(_searchValue);
            }
        }

        protected override void OnInitialized()
        {
            SetClassMap();

            if (string.IsNullOrWhiteSpace(Style))
                Style = DefaultWidth;

            _isInitialized = true;

            base.OnInitialized();
        }

        protected void OnOverlayHide()
        {
            if (!IsSearchEnabled)
                return;

            if (!AutoClearSearchValue)
                return;

            if (string.IsNullOrWhiteSpace(_searchValue))
                return;

            _searchValue = string.Empty;

            if (SelectMode != SelectMode.Default && HideSelected)
            {
                SelectOptionItems.Where(x => !x.IsSelected && x.IsHidden)
                    .ForEach(i => i.IsHidden = false);
            }
            else
            {
                SelectOptionItems.Where(x => x.IsHidden)
                    .ForEach(i => i.IsHidden = false);
            }
        }

        /// <summary>
        /// The method is called every time if the user select/de-select a item by mouse or keyboard.
        /// Don't change the IsSelected property outside of this function.
        /// </summary>
        protected internal async Task SetValueAsync(SelectOptionItem<TItemValue, TItem> selectOption)
        {
            if (selectOption == null) throw new ArgumentNullException(nameof(selectOption));

            if (SelectMode == SelectMode.Default)
            {
                SelectOptionItems.Where(x => x.IsSelected)
                    .ForEach(i => i.IsSelected = false);

                selectOption.IsSelected = true;

                await ValueChanged.InvokeAsync(selectOption.Value);
            }
            else
            {
                selectOption.IsSelected = !selectOption.IsSelected;

                if (selectOption.IsSelected)
                {
                    if (HideSelected && !selectOption.IsHidden)
                        selectOption.IsHidden = true;

                    if (IsSearchEnabled)
                    {
                        if (!string.IsNullOrWhiteSpace(_searchValue))
                        {
                            ClearSearch();

                            await SetInputFocusAsync();
                        }
                    }
                }
                else
                {
                    if (selectOption.IsHidden)
                        selectOption.IsHidden = false;
                }

                await InvokeValuesChanged();

                await UpdateOverlayPositionAsync();
            }
        }

        protected async Task InvokeValuesChanged()
        {
            var newSelectedValues = new List<TItemValue>();

            SelectOptionItems.Where(x => x.IsSelected)
                .ForEach(i =>
                {
                    newSelectedValues.Add(i.Value);
                });

            await ValuesChanged.InvokeAsync(newSelectedValues);
        }

        protected void ClearSearch()
        {
            if (SelectMode != SelectMode.Default)
            {
                if (HideSelected)
                {
                    SelectOptionItems.Where(x => x.IsHidden && !x.IsSelected)
                        .ForEach(i => i.IsHidden = false);
                }
                else
                {
                    SelectOptionItems.Where(x => x.IsHidden)
                        .ForEach(i => i.IsHidden = false);
                }

                SelectOptionItems.Where(x => x.IsActive)
                        .ForEach(i => i.IsActive = false);
            }

            _searchValue = string.Empty;
        }

        /// <summary>
        /// Check if Focused property is False; Set the Focused property to true, change the
        /// style and set the Focus on the Input element via DOM. It also invoke the OnFocus Action.
        /// </summary>
        protected async Task SetInputFocusAsync()
        {
            if (!Focused)
            {
                Focused = true;

                SetClassMap();

                await JsInvokeAsync(JSInteropConstants.Focus, _inputRef);

                OnFocus?.Invoke();
            }
        }

        /// <summary>
        /// Inform the Overlay to update the position.
        /// </summary>
        internal async Task UpdateOverlayPositionAsync()
        {
            await _dropDown.GetOverlayComponent().UpdatePosition();
        }

        protected abstract void SetClassMap();
    }
}
