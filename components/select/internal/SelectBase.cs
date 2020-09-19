using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign.Internal
{
    public class SelectBase : AntInputComponentBase<string>
    {
        public SelectMode SelectMode => Mode.ToSelectMode();
        protected string[] _tokenSeparators;
        protected OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>>? _value;

        [CascadingParameter(Name = "ModalCompleteShow")]
        public bool ModalCompleteShow { get; set; }

        #region Boolean(15)
        [Parameter] public bool AutoFocus { get; set; } = false;

        [Parameter] public bool AllowClear { get; set; } = false;

        [Parameter] public bool AutoClearSearchValue { get; set; } = true;

        [Parameter] public bool DefaultActiveFirstOption { get; set; } = true;

        [Parameter] public bool Disabled { get; set; } = false;

        [Parameter] public bool LabelInValue { get; set; } = false;

        [Parameter] public bool ShowArrow { get; set; } = true;

        [Parameter] public bool ShowSearch { get; set; } = false;

        [Parameter] public bool Virtual { get; set; } = true;

        [Parameter] public bool Loading { get; set; } = false;

        [Parameter] public bool Bordered { get; set; } = true;

        [Parameter] public bool Open { get; set; } = false;

        [Parameter] public bool DefaultOpen { get; set; } = false;

        [Parameter] public bool HideSelected { get; set; } = false;
        #endregion

        #region String(7)
        [Parameter] public string Mode { get; set; }

        [Parameter] public string Placeholder { get; set; }

        [Parameter] public string DropdownStyle { get; set; }

        [Parameter] public string OptionLabelProp { get; set; }

        [Parameter] public string DropdownClassName { get; set; }

        [Parameter] public string OptionFilterProp { get; set; } = "value";

        [Parameter]
        public string PopupContainerSelector { get; set; } = "body";

        #endregion

        #region Number(3)
        [Parameter] public int? MaxTagCount { get; set; }

        [Parameter] public int? MaxTagTextLength { get; set; }

        [Parameter] public int ListHeight { get; set; } = 256;
        #endregion

        #region Array(2)
        [Parameter] public IEnumerable<LabeledValue> Options { get; set; }

        [Parameter]
        public IEnumerable<string> TokenSeparators
        {
            get => _tokenSeparators;
            set
            {
                _tokenSeparators = value.ToArray();
            }
        }
        #endregion

        #region Complex(5)
        [Parameter] public OneOf<bool, int>? DropdownMatchSelectWidth { get; set; }

        [Parameter]
        public OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>>? SelectedValues
        {
            get => _value;
            set
            {
                if (value.HasValue)
                {
                    if (SelectMode == SelectMode.Default)
                    {
                        if (LabelInValue)
                        {
                            if (value.Value.AsT2 != null)
                            {
                                _value = value;
                            }
                        }
                        else if (value.Value.AsT0 != null)
                        {
                            _value = value;
                        }
                    }
                    else
                    {
                        if (LabelInValue)
                        {
                            if (value.Value.AsT3 != null)
                            {
                                _value = value;
                            }
                        }
                        else if (value.Value.AsT1 != null)
                        {
                            _value = value;
                        }
                    }
                }
            }
        }

        [Parameter] public OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>>? DefaultValue { get; set; }

        [Parameter] public OneOf<bool, Func<string, SelectOption, bool>> FilterOption { get; set; } = true;

        [Parameter] public OneOf<RenderFragment, Func<string[], RenderFragment>>? MaxTagPlaceholder { get; set; }
        #endregion

        #region RenderFragment(6)
        [Parameter] public RenderFragment ClearIcon { get; set; }

        [Parameter] public RenderFragment RemoveIcon { get; set; }

        [Parameter] public RenderFragment SuffixIcon { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public RenderFragment NotFoundContent { get; set; }

        [Parameter] public RenderFragment MenuItemSelectedIcon { get; set; }
        #endregion

        #region Function(14)
        [Parameter] public Action OnBlur { get; set; }

        [Parameter] public Action OnFocus { get; set; }

        [Parameter] public Action OnMouseEnter { get; set; }

        [Parameter] public Action OnMouseLeave { get; set; }

        [Parameter] public Action OnPopupScroll { get; set; }

        [Parameter] public Action OnInputKeyDown { get; set; }

        [Parameter] public Action<string> OnSearch { get; set; }

        [Parameter] public Action<bool> OnDropdownVisibleChange { get; set; }

        [Parameter] public Action<OneOf<string, LabeledValue>> OnDeselect { get; set; }

        [Parameter] public Action<OneOf<string, LabeledValue>, SelectOption> OnSelect { get; set; }

        [Parameter] public Func<Properties, RenderFragment> TagRender { get; set; }

        [Parameter] public Func<RenderFragment, Properties, RenderFragment> DropdownRender { get; set; }

        [Parameter] public Action<OneOf<string, IEnumerable<string>, LabeledValue, IEnumerable<LabeledValue>>, OneOf<SelectOption, IEnumerable<SelectOption>>> OnChange { get; set; }
        #endregion
    }
}
