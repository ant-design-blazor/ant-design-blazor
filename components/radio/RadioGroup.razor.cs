using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class RadioGroup<TValue> : AntInputComponentBase<TValue>
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string ButtonStyle { get; set; } = "outline";

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public TValue DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                _hasDefaultValue = true;
            }
        }

        [Parameter]
        public EventCallback<TValue> OnChange { get; set; }

        private List<Radio<TValue>> _radioItems = new List<Radio<TValue>>();

        private TValue _defaultValue;

        private bool _hasDefaultValue;
        private bool _defaultValueSetted;

        protected override void OnInitialized()
        {
            string prefixCls = "ant-radio-group";
            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-large", () => Size == "large")
                .If($"{prefixCls}-small", () => Size == "small")
                .GetIf(() => $"{prefixCls}-{ButtonStyle}", () => ButtonStyle.IsIn("outline", "solid"))
                .If($"{prefixCls}-rtl", () => RTL)
                ;

            base.OnInitialized();

            if (_hasDefaultValue && !_defaultValueSetted)
            {
                CurrentValue = _defaultValue;
                _defaultValueSetted = true;
            }
        }

        internal async Task AddRadio(Radio<TValue> radio)
        {
            if (this.Name != null)
            {
                radio.SetName(Name);
            }
            _radioItems.Add(radio);
            if (EqualsValue(this.CurrentValue, radio.Value))
            {
                await radio.Select();
                StateHasChanged();
            }
        }

        internal void RemoveRadio(Radio<TValue> radio)
        {
            _radioItems.Remove(radio);
        }

        protected override async Task OnParametersSetAsync()
        {
            foreach (var radio in _radioItems)
            {
                if (EqualsValue(this.CurrentValue, radio.Value))
                {
                    await radio.Select();
                }
                else
                {
                    await radio.UnSelect();
                }
            }
            await base.OnParametersSetAsync();
        }

        internal async Task OnRadioChange(TValue value)
        {
            if (!EqualsValue(this.CurrentValue, value))
            {
                this.CurrentValue = value;

                await this.ValueChanged.InvokeAsync(value);

                if (this.OnChange.HasDelegate)
                {
                    await this.OnChange.InvokeAsync(value);
                }
            }
        }

        private static bool EqualsValue(TValue left, TValue right)
        {
            if (left != null) return left.Equals(right);
            if (right != null) return right.Equals(left);
            if (left == null && right == null) return true;
            return false;
        }
    }
}
