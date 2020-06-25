using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class RadioGroup<TValue> : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string ButtonStyle { get; set; } = "outline";

        [Parameter]
        public string Size { get; set; } = "default";

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public EventCallback<TValue> OnChange { get; set; }

        private TValue _value;
        [Parameter]
        public TValue Value
        {
            get { return _value; }
            set
            {
                _value = value;
                foreach (var radio in RadioItems)
                {
                    if (EqualsValue(this.CurrentValue, radio.Value))
                    {
                        InvokeAsync(radio.Select);
                    }
                    else
                    {
                        InvokeAsync(radio.UnSelect);
                    }
                }
                StateHasChanged();
            }
        }

        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        internal List<Radio<TValue>> RadioItems { get; set; } = new List<Radio<TValue>>();

        TValue CurrentValue => Value;

        protected override async Task OnInitializedAsync()
        {
            string prefixCls = "ant-radio-group";
            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-large", () => Size == "large")
                .If($"{prefixCls}-small", () => Size == "small")
                .If($"{prefixCls}-solid", () => ButtonStyle == "solid");


            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {

            //foreach (var radio in RadioItems)
            //{
            //    if (EqualsValue(this.CurrentValue, radio.Value))
            //    {
            //        await radio.Select();
            //    }
            //    else
            //    {
            //        await radio.UnSelect();
            //    }
            //}
            //StateHasChanged();
            await base.OnParametersSetAsync();
        }

        internal async Task AddRadio(Radio<TValue> radio)
        {
            if (this.Name != null)
            {
                radio._name = Name;
            }
            RadioItems.Add(radio);
            if (EqualsValue(this.CurrentValue, radio.Value))
            {
                await radio.Select();
            }
            StateHasChanged();
        }

        internal async Task OnRadioChange(TValue value)
        {
            if (!EqualsValue(this.CurrentValue, value))
            {
                this.Value = value;
                await this.ValueChanged.InvokeAsync(value);

                foreach (var radio in RadioItems)
                {
                    if (!EqualsValue(this.CurrentValue, radio.Value))
                    {
                        await radio.UnSelect();
                    }
                }

                if (this.OnChange.HasDelegate)
                {
                    await this.OnChange.InvokeAsync(value);
                }
            }
            StateHasChanged();

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
