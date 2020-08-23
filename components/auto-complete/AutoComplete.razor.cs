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
        public OneOf<IList<AutoCompleteDataItem>, IList<string>, IList<int>> DataSource { get; set; }
        [Parameter]
        public EventCallback<AutoCompleteOption> OnSelectionChange { get; set; }
        [Parameter]
        public EventCallback<AutoCompleteOption> OnActiveChange { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment AutoCompleteOptions { get; set; }

        [Parameter]
        public Func<object, object, bool> CompareWith { get; set; } = (o1, o2) => o1?.ToString() == o2?.ToString();

        [Parameter]
        public Func<AutoCompleteDataItem, bool> FilterOption { get; set; }

        #endregion Parameters

        private ElementReference _divRef;

        public object SelectedValue { get; internal set; }
        /// <summary>
        /// 选择的项
        /// </summary>
        [Parameter]
        public AutoCompleteOption SelectedItem { get; set; }

        /// <summary>
        /// 高亮的项目
        /// </summary>
        public object ActiveValue { get; internal set; }


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
        }

        public async Task InputBlur(FocusEventArgs e)
        {
            this.ClosePanel();
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
                else if (key == "Enter" && this.ActiveValue != null)
                {
                    await SetSelectedItem(GetActiveItem());
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

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            ResetActiveItem();
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            await SetOverlayWidth();
            await base.OnFirstAfterRenderAsync();
        }

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

        public IList<AutoCompleteDataItem> GetOptionItems()
        {
            if (DataSource.Value != null)
            {
                var opts = DataSource.Match<IList<AutoCompleteDataItem>>(
                                          f0 => f0,
                                          f1 => f1.Select(x => new AutoCompleteDataItem(x, x)).ToList(),
                                          f2 => f2.Select(x => new AutoCompleteDataItem(x, x.ToString())).ToList());
                if (FilterOption != null)
                    return opts.Where(FilterOption).ToList();
                else
                    return opts;
            }
            else if (Options.Count > 0)
            {
                var opts = Options.Select(x => new AutoCompleteDataItem(x.Value, x.Label)).ToList();
                return opts;
            }
            else
            {
                return new List<AutoCompleteDataItem>();
            }
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        public void OpenPanel()
        {
            if (this.ShowPanel == false)
            {
                this.ShowPanel = true;
                ResetActiveItem();
                StateHasChanged();
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
                StateHasChanged();
            }
        }

        public AutoCompleteOption GetActiveItem()
        {
            return Options.FirstOrDefault(x => CompareWith(x.Value, this.ActiveValue));
        }

        //设置高亮的对象
        public void SetActiveItem(AutoCompleteOption item)
        {
            this.ActiveValue = item?.Value;
            if (OnActiveChange.HasDelegate) OnActiveChange.InvokeAsync(item);
            StateHasChanged();
        }

        //设置下一个激活
        public void SetNextItemActive()
        {
            var nextItem = Options.IndexOf(GetActiveItem());
            if (nextItem == -1 || nextItem == Options.Count - 1)
                SetActiveItem(Options.FirstOrDefault());
            else
                SetActiveItem(Options[nextItem + 1]);

            StateHasChanged();
        }

        //设置上一个激活
        public void SetPreviousItemActive()
        {
            var nextItem = Options.IndexOf(GetActiveItem());
            if (nextItem == -1 || nextItem == 0)
                SetActiveItem(Options.LastOrDefault());
            else
                SetActiveItem(Options[nextItem - 1]);
        }

        private void ResetActiveItem()
        {
            var items = GetOptionItems();

            if (items.Any(x => CompareWith(x.Value, this.ActiveValue)) == false)
            {//如果当前激活项找在列表中不存在，那么我们需要做一些处理
                if (items.Any(x => CompareWith(x.Value, this.SelectedValue)))
                {
                    this.ActiveValue = this.SelectedValue;
                }
                else if (DefaultActiveFirstOption == true && items.Count > 0)
                {
                    this.ActiveValue = items.FirstOrDefault()?.Value;
                }
                else
                {
                    this.ActiveValue = null;
                }
            }
        }

        public async Task SetSelectedItem(AutoCompleteOption item)
        {
            if (item != null)
            {
                this.SelectedValue = item?.Value;
                this.SelectedItem = item;
                InputComponent?.SetValue(this.SelectedItem.Label);

                if (OnSelectionChange.HasDelegate) await OnSelectionChange.InvokeAsync(this.SelectedItem);
            }
            this.ClosePanel();
        }

        private string minWidth = "";
        private async Task SetOverlayWidth()
        {
            Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _divRef);
            var newWidth = $"min-width:{element.clientWidth}px";
            if (newWidth != minWidth) minWidth = newWidth;
        }
    }

    public class AutoCompleteDataItem
    {
        public AutoCompleteDataItem() { }

        public AutoCompleteDataItem(object value, string label)
        {
            Value = value;
            Label = label;
        }

        public object Value { get; set; }
        public string Label { get; set; }
    }
}
#pragma warning restore IDE1006 // 命名样式
