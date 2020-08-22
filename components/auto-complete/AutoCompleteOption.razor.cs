using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

#pragma warning disable IDE1006 // 命名样式
namespace AntDesign
{
    public partial class AutoCompleteOption : AntDomComponentBase
    {
        #region Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public object Value { get; set; }
        [Parameter]
        public string Label { get; set; }
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
            await AutoComplete.SetSelectedItem(this);
        }

        /// <summary>
        /// 获得标题
        /// </summary>
        /// <returns></returns>
        public string GetLabel()
        {
            return this.Label ?? this.Value?.ToString();
        }

        /// <summary>
        /// 计算当前计算选择状态
        /// </summary>
        /// <returns></returns>
        private bool CalcSelected()
        {
            return AutoComplete?.SelectedValue?.ToString() == Value.ToString();
        }

        private bool CalcActive()
        {
            return AutoComplete?.ActiveItem == this;
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
#pragma warning restore IDE1006 // 命名样式
