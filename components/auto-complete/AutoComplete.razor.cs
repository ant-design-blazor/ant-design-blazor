using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class AutoComplete : AntInputComponentBase<string>, IAutoCompleteRef
    {
        #region Parameters

        [Parameter]
        public string Placeholder { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool DefaultActiveFirstOption { get; set; } = true;
        [Parameter]
        public bool Backfill { get; set; } = false;

        [Parameter]
        public OneOf<IList<AutoCompleteDataItem>, IList<string>, IList<int>> Options { get; set; }
        [Parameter]
        public EventCallback<AutoCompleteOption> OnSelectionChange { get; set; }
        [Parameter]
        public EventCallback<AutoCompleteOption> OnActiveChange { get; set; }

        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        [Parameter]
        public EventCallback<bool> OnPanelVisibleChange { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment AutoCompleteOptions { get; set; }

        [Parameter]
        public Func<object, object, bool> CompareWith { get; set; } = (o1, o2) => o1?.ToString() == o2?.ToString();

        [Parameter]
        public Func<AutoCompleteDataItem, bool> FilterOption { get; set; }

        [Parameter]
        public OneOf<int?, string> Width { get; set; }

        [Parameter]
        public string OverlayClassName { get; set; }

        [Parameter]
        public string OverlayStyle { get; set; }

        [Parameter]
        public string PopupContainerSelector { get; set; } = "body";

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

        private bool _isOptionsZero = true;

        private IAutoCompleteInput _inputComponent;

        public void SetInputComponent(IAutoCompleteInput input)
        {
            _inputComponent = input;
        }

        #region 子控件触发事件
        public async Task InputFocus(FocusEventArgs e)
        {
            this.OpenPanel();
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
        public List<AutoCompleteOption> _options = new List<AutoCompleteOption>();

        public void AddOption(AutoCompleteOption option)
        {
            _options.Add(option);
        }

        public void RemoveOption(AutoCompleteOption option)
        {
            if (_options?.Contains(option) == true)
                _options?.Remove(option);
        }

        public IList<AutoCompleteDataItem> GetOptionItems()
        {
            if (Options.Value != null)
            {
                var opts = Options.Match<IList<AutoCompleteDataItem>>(
                                          f0 => f0,
                                          f1 => f1.Select(x => new AutoCompleteDataItem(x, x)).ToList(),
                                          f2 => f2.Select(x => new AutoCompleteDataItem(x, x.ToString())).ToList());
                if (FilterOption != null)
                    return opts.Where(FilterOption).ToList();
                else
                    return opts;
            }
            else if (_options.Count > 0)
            {
                var opts = _options.Select(x => new AutoCompleteDataItem(x.Value, x.Label)).ToList();
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
            return _options.FirstOrDefault(x => CompareWith(x.Value, this.ActiveValue));
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
            var opts = _options.Where(x => x.Disabled == false).ToList();
            var nextItem = opts.IndexOf(GetActiveItem());
            if (nextItem == -1 || nextItem == opts.Count - 1)
                SetActiveItem(opts.FirstOrDefault());
            else
                SetActiveItem(opts[nextItem + 1]);

            if (Backfill)
                _inputComponent.SetValue(this.ActiveValue);
        }

        //设置上一个激活
        public void SetPreviousItemActive()
        {
            var opts = _options.Where(x => x.Disabled == false).ToList();
            var nextItem = opts.IndexOf(GetActiveItem());
            if (nextItem == -1 || nextItem == 0)
                SetActiveItem(opts.LastOrDefault());
            else
                SetActiveItem(opts[nextItem - 1]);

            if (Backfill)
                _inputComponent.SetValue(this.ActiveValue);
        }

        private void ResetActiveItem()
        {
            var items = GetOptionItems();
            _isOptionsZero = items.Count == 0 && Options.Value != null;
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
                _inputComponent?.SetValue(this.SelectedItem.Label);

                if (OnSelectionChange.HasDelegate) await OnSelectionChange.InvokeAsync(this.SelectedItem);
            }
            this.ClosePanel();
        }

        bool _parPanelVisible = false;

        private async void OnOverlayTriggerVisibleChange(bool visible)
        {
            if (OnPanelVisibleChange.HasDelegate && _parPanelVisible != visible)
            {
                _parPanelVisible = visible;
                await OnPanelVisibleChange.InvokeAsync(visible);
            }

            if (this.ShowPanel != visible)
            {
                this.ShowPanel = visible;
            }
        }

        private string _minWidth = "";
        private async Task SetOverlayWidth()
        {
            string newWidth;
            if (Width.Value != null)
            {
                var w = Width.Match<string>(f0 => $"{f0}px", f1 => f1);
                newWidth = $"min-width:{w}";
            }
            else
            {
                Element element = await JsInvokeAsync<Element>(JSInteropConstants.GetDomInfo, _divRef);
                newWidth = $"min-width:{element.clientWidth}px";
            }
            if (newWidth != _minWidth) _minWidth = newWidth;
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

        public bool IsDisabled { get; set; }
    }
}

