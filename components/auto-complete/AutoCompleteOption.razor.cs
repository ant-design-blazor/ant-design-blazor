using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class AutoCompleteOption : AntDomComponentBase
    {
        #region Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public object Value { get; set; }

        private string _label;
        [Parameter]
        public string Label
        {
            get { return _label ?? Value?.ToString(); }
            set { _label = value; }
        }

        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]

        [CascadingParameter]
        public AutoComplete AutoComplete { get; set; }

        #endregion Parameters

        protected override void OnInitialized()
        {
            SetClassMap();
            AutoComplete?.AddOption(this);
            base.OnInitialized();
        }

        protected override void Dispose(bool disposing)
        {
            AutoComplete?.RemoveOption(this);
            base.Dispose(disposing);
        }

        private void SetClassMap()
        {
            ClassMapper.Clear().Add("ant-select-item").Add("ant-select-item-option")
                //.If("ant-select-item-option-grouped", () => this.AutocompleteOptgroup)
                .If("ant-select-item-option-selected", () => CalcSelected())
                .If("ant-select-item-option-active", () => CalcActive())
                .If("ant-select-item-option-disabled", () => this.Disabled);
        }

        public void OnMouseEnter()
        {
            AutoComplete.SetActiveItem(this);
        }

        public async Task OnClick()
        {
            if (Disabled) return;
            await AutoComplete.SetSelectedItem(this);
        }

        /// <summary>
        /// 计算当前计算选择状态
        /// </summary>
        /// <returns></returns>
        private bool CalcSelected()
        {
            return AutoComplete.CompareWith(AutoComplete?.SelectedValue, Value);
        }

        private bool CalcActive()
        {
            return AutoComplete.CompareWith(AutoComplete?.ActiveValue, Value);
        }
    }


    public class OptionSelectionChange
    {
        public AutoCompleteOption Source { get; set; }

        public bool IsUserInput { get; set; }

        public OptionSelectionChange(AutoCompleteOption source, bool isUserInput = false)
        {
            Source = source;
            IsUserInput = isUserInput;
        }
    }

}
