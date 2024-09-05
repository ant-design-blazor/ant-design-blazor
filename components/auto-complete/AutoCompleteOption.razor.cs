﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class AutoCompleteOption : AntDomComponentBase
    {
        #region Parameters

        /// <summary>
        /// Label for the option. Takes priority over Label
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Value for the option
        /// </summary>
        [Parameter]
        public object Value { get; set; }

        private string _label;
        
        /// <summary>
        /// Label for the option
        /// </summary>
        /// <default value="Value.ToString()" />
        [Parameter]
        public string Label
        {
            get { return _label ?? Value?.ToString(); }
            set { _label = value; }
        }

        /// <summary>
        /// If option is disabled or not
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Disabled { get; set; } = false;

        [CascadingParameter]
        internal IAutoCompleteRef AutoComplete { get; set; }

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

        private void OnMouseEnter()
        {
            AutoComplete.SetActiveItem(this);
        }

        private async Task OnClick()
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
            return AutoComplete.CompareWith(AutoComplete.SelectedValue, Value);
        }

        private bool CalcActive()
        {
            return AutoComplete.CompareWith(AutoComplete.ActiveValue, Value);
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
