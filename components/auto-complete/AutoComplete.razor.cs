using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using System.Data;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
using OneOf;
#pragma warning disable IDE1006 // 命名样式
namespace AntDesign
{
    public partial class AutoComplete : AntDomComponentBase
    {
        #region Parameters


        [Parameter]
        public bool DefaultActiveFirstOption { get; set; } = true;
        [Parameter]
        public bool Backfill { get; set; } = false;

        [Parameter]
        public OneOf<IList<AutocompleteDataSourceItem>, IList<string>, IList<int>> DataSource { get; set; }
        [Parameter]
        public EventCallback<AutoCompleteOption> OnSelectionChange { get; set; }
        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment<List<AutoCompleteOption>> AutoCompleteOptions { get; set; }

        #endregion Parameters

        internal object SelectedValue;
        /// <summary>
        /// 选择的项
        /// </summary>
        [Parameter]
        public AutoCompleteOption SelectedItem { get; set; }

        /// <summary>
        /// 高亮的项目
        /// </summary>
        internal AutoCompleteOption ActiveItem { get; set; }

        [Parameter]
        public bool ShowPanel { get; set; } = false;

        private IAutoCompleteInput InputComponent;

        public void SetInputComponent(IAutoCompleteInput input)
        {
            InputComponent = input;
        }


        #region 子控件触发事件
        public async Task InputFocus(FocusEventArgs e)
        {
            this.OpenPanel();
            StateHasChanged();
        }

        public async Task InputBlur(FocusEventArgs e)
        {
            this.ShowPanel = false;
        }

        public async Task InputInput(ChangeEventArgs args)
        {
            SelectedValue = args.Value;
            if (OnInput.HasDelegate) await OnInput.InvokeAsync(args);
            StateHasChanged();
        }

        public async Task InputKeyDown(KeyboardEventArgs args)
        {
            var key = args.Key;

            if (this.ShowPanel)
            {
                if (key == "Escape" || key == "Tab")
                {
                    this.ClosePanel();
                }
                else if (key == "Enter" && this.ActiveItem != null)
                {
                    await SetSelectedItem(this.ActiveItem);
                }
            }
            else
            {
                this.OpenPanel();
            }
            if (key == "ArrowUp")
            {
                this.SetPreviousItemActive();
            }
            else if (key == "ArrowDown")
            {
                this.SetNextItemActive();
            }
        }

        #endregion

        /// <summary>
        /// 对象集合
        /// </summary>
        public List<AutoCompleteOption> Options = new List<AutoCompleteOption>();

        public void AddOption(AutoCompleteOption option)
        {
            Options.Add(option);
        }

        public void RemoveOption(AutoCompleteOption option)
        {
            if (Options?.Contains(option) == true)
                Options?.Remove(option);
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        public void OpenPanel()
        {
            if (this.ShowPanel == false)
            {
                this.ShowPanel = true;

            }
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void ClosePanel()
        {
            if (this.ShowPanel == true)
            {
                this.ShowPanel = false;
            }
        }

        //设置高亮的对象
        public void SetActiveItem(AutoCompleteOption item)
        {
            this.ActiveItem = item;
            StateHasChanged();
        }

        //设置下一个激活
        public void SetNextItemActive()
        {
            var nextItem = Options.IndexOf(ActiveItem);
            if (nextItem == -1 || nextItem == Options.Count - 1)
                SetActiveItem(Options.FirstOrDefault());
            else
                SetActiveItem(Options[nextItem + 1]);

            StateHasChanged();
        }

        //设置上一个激活
        public void SetPreviousItemActive()
        {
            var nextItem = Options.IndexOf(ActiveItem);
            if (nextItem == -1 || nextItem == 0)
                SetActiveItem(Options.LastOrDefault());
            else
                SetActiveItem(Options[nextItem - 1]);
        }

        private void ResetActiveItem()
        {
            this.ActiveItem = null;
        }

        public async Task SetSelectedItem(AutoCompleteOption item)
        {
            this.SelectedValue = item?.Value;
            this.SelectedItem = item;
            InputComponent?.SetValue(this.SelectedValue);

            if (OnSelectionChange.HasDelegate) await OnSelectionChange.InvokeAsync(this.SelectedItem);
            this.ClosePanel();
        }
    }

    public class AutocompleteDataSourceItem
    {
        public AutocompleteDataSourceItem() { }

        public AutocompleteDataSourceItem(string value, string label)
        {
            Value = value;
            Label = label;
        }

        public string Value { get; set; }
        public string Label { get; set; }
    }
}
#pragma warning restore IDE1006 // 命名样式
