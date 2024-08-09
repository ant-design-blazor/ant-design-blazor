using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Core.JsInterop.ObservableApi;
using AntDesign.Internal;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class AutoComplete<TOption> : AntInputComponentBase<string>, IAutoCompleteRef
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
        public int DebounceMilliseconds { get; set; } = 250;

        /// <summary>
        /// 列表对象集合
        /// List object collection
        /// </summary>
        private List<AutoCompleteOption> AutoCompleteOptions { get; set; } = new List<AutoCompleteOption>();

        /// <summary>
        /// 列表数据集合
        /// List data collection
        /// </summary>
        private List<AutoCompleteDataItem<TOption>> _optionDataItems = new List<AutoCompleteDataItem<TOption>>();

        /// <summary>
        /// 列表绑定数据源集合
        /// List bound data source collection
        /// </summary>
        private IEnumerable<TOption> _options;

        [Parameter]
        public IEnumerable<TOption> Options
        {
            get
            {
                return _options;
            }
            set
            {
                _options = value;
                _optionDataItems = _options?.Select(x => new AutoCompleteDataItem<TOption>(x, x.ToString())).ToList() ?? new List<AutoCompleteDataItem<TOption>>();
            }
        }

        /// <summary>
        /// 绑定列表数据项格式的数据源
        /// Bind the data source of the list data item format
        /// </summary>
        [Parameter]
        public IEnumerable<AutoCompleteDataItem<TOption>> OptionDataItems
        {
            get
            {
                return _optionDataItems;
            }
            set
            {
                _optionDataItems = value.ToList();
            }
        }

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

        /// <summary>
        /// 选项模板
        /// Option template
        /// </summary>
        [Parameter]
        public RenderFragment<AutoCompleteDataItem<TOption>> OptionTemplate { get; set; }

        /// <summary>
        /// 格式化选项，可以自定义显示格式
        /// Formatting options, you can customize the display format
        /// </summary>
        [Parameter]
        public Func<AutoCompleteDataItem<TOption>, string> OptionFormat { get; set; }

        /// <summary>
        /// 所有选项模板
        /// All option templates
        /// </summary>
        [Parameter]
        public RenderFragment OverlayTemplate { get; set; }

        /// <summary>
        /// 对比，用于两个对象比较是否相同
        /// Contrast, used to compare whether two objects are the same
        /// </summary>
        [Parameter]
        public Func<object, object, bool> CompareWith { get; set; } = (o1, o2) => o1?.ToString() == o2?.ToString();

        /// <summary>
        /// 过滤表达式
        /// Filter expression
        /// </summary>
        [Parameter]
        public Func<AutoCompleteDataItem<TOption>, string, bool> FilterExpression { get; set; } = (option, value) => string.IsNullOrEmpty(value) ? false : option.Label.Contains(value, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// 允许过滤
        /// Allow filtering
        /// </summary>
        [Parameter]
        public bool AllowFilter { get; set; } = true;

        [Parameter]
        public OneOf<int?, string> Width { get; set; }

        [Parameter]
        public string OverlayClassName { get; set; }

        [Parameter]
        public string OverlayStyle { get; set; }

        [Parameter]
        public string PopupContainerSelector { get; set; } = "body";

        #endregion Parameters

        //private ElementReference _divRef;
        private OverlayTrigger _overlayTrigger;

        public object SelectedValue { get; set; }

        /// <summary>
        /// 选择的项
        /// </summary>
        [Parameter]
        public AutoCompleteOption SelectedItem { get; set; }

        /// <summary>
        /// 高亮的项目
        /// </summary>
        public object ActiveValue { get; set; }

        /// <summary>
        /// Overlay adjustment strategy (when for example browser resize is happening). Check
        /// enum for details.
        /// </summary>
        [Parameter]
        public TriggerBoundaryAdjustMode BoundaryAdjustMode { get; set; } = TriggerBoundaryAdjustMode.InView;

        [Parameter]
        public bool ShowPanel { get; set; } = false;

        [Inject] private IDomEventListener DomEventListener { get; set; }

        private bool _isOptionsZero = true;

        private IAutoCompleteInput _inputComponent;

        private IList<AutoCompleteDataItem<TOption>> _filteredOptions;

        private string _minWidth = "";
        private bool _parPanelVisible = false;

        public void SetInputComponent(IAutoCompleteInput input)
        {
            _inputComponent = input;
        }

        #region 子控件触发事件 / Child controls trigger events

        public Task InputFocus(FocusEventArgs e)
        {
            if (!_isOptionsZero)
            {
                this.OpenPanel();
            }

            return Task.CompletedTask;
        }

        public async Task InputInput(ChangeEventArgs args)
        {
            if (OnInput.HasDelegate) await OnInput.InvokeAsync(args);
        }

        public async Task InputValueChange(string value)
        {
            SelectedValue = value;
            UpdateFilteredOptions();
            await ResetActiveItem();
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
            else if (!_isOptionsZero)
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

        #endregion 子控件触发事件 / Child controls trigger events

        protected override void OnInitialized()
        {
            _isOptionsZero = Options?.Any() != true;
            base.OnInitialized();
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            await SetOverlayWidth();
            await DomEventListener.AddResizeObserver(_overlayTrigger.RefBack.Current, UpdateWidth);
            await base.OnFirstAfterRenderAsync();
        }

        public void AddOption(AutoCompleteOption option)
        {
            AutoCompleteOptions.Add(option);
        }

        public void RemoveOption(AutoCompleteOption option)
        {
            if (AutoCompleteOptions?.Contains(option) == true)
                AutoCompleteOptions?.Remove(option);
        }

        public IList<AutoCompleteDataItem<TOption>> GetOptionItems()
        {
            if (_optionDataItems != null)
            {
                if (FilterExpression != null && AllowFilter == true && SelectedValue != null)
                    return _optionDataItems.Where(x => FilterExpression(x, SelectedValue?.ToString())).ToList();
                else
                    return _optionDataItems;
            }
            else
            {
                return new List<AutoCompleteDataItem<TOption>>();
            }
        }

        /// <summary>
        /// 打开面板
        /// Open panel
        /// </summary>
        public void OpenPanel()
        {
            if (this.ShowPanel == false)
            {
                this.ShowPanel = true;

                _overlayTrigger.Show();

                ResetActiveItem();
                StateHasChanged();
            }
        }

        /// <summary>
        /// 关闭面板
        /// Close panel
        /// </summary>
        public void ClosePanel()
        {
            if (this.ShowPanel == true)
            {
                this.ShowPanel = false;

                _overlayTrigger.Close();

                StateHasChanged();
            }
        }

        public AutoCompleteOption GetActiveItem()
        {
            return AutoCompleteOptions.FirstOrDefault(x => CompareWith(x.Value, this.ActiveValue));
        }

        //设置高亮的对象
        //Set the highlighted object
        public void SetActiveItem(AutoCompleteOption item)
        {
            this.ActiveValue = item == null ? default(TOption) : item.Value;
            if (OnActiveChange.HasDelegate) OnActiveChange.InvokeAsync(item);
            StateHasChanged();
        }

        //设置下一个激活
        //Set the next activation
        public void SetNextItemActive()
        {
            var opts = AutoCompleteOptions.Where(x => x.Disabled == false).ToList();
            var nextItem = opts.IndexOf(GetActiveItem());
            if (nextItem == -1 || nextItem == opts.Count - 1)
                SetActiveItem(opts.FirstOrDefault());
            else
                SetActiveItem(opts[nextItem + 1]);

            if (Backfill)
                _inputComponent.SetValue(this.ActiveValue);
        }

        //设置上一个激活
        //Set last activation
        public void SetPreviousItemActive()
        {
            var opts = AutoCompleteOptions.Where(x => x.Disabled == false).ToList();
            var nextItem = opts.IndexOf(GetActiveItem());
            if (nextItem == -1 || nextItem == 0)
                SetActiveItem(opts.LastOrDefault());
            else
                SetActiveItem(opts[nextItem - 1]);

            if (Backfill)
                _inputComponent.SetValue(this.ActiveValue);
        }

        public void UpdateFilteredOptions()
        {
            _filteredOptions = GetOptionItems();
            _isOptionsZero = _filteredOptions.Count == 0 && Options != null;
        }

        private async Task ResetActiveItem()
        {
            var items = _filteredOptions;

            if (items.Any(x => CompareWith(x.Value, this.ActiveValue)) == false)
            {
                // 如果当前激活项找在列表中不存在，那么我们需要做一些处理
                // If the current activation item does not exist in the list, then we need to do some processing
                if (items.Any(x => CompareWith(x.Value, this.SelectedValue)))
                {
                    this.ActiveValue = this.SelectedValue;
                }
                else if (DefaultActiveFirstOption == true && items.Count > 0)
                {
                    this.ActiveValue = items.FirstOrDefault()?.Value.ToString();
                }
                else
                {
                    this.ActiveValue = null;
                }
            }

            if (_overlayTrigger != null)
            {
                // if options count == 0 then close overlay
                if (_isOptionsZero && _overlayTrigger.IsOverlayShow())
                {
                    await _overlayTrigger.Close();
                }
                // if options count > 0 then open overlay
                else if (!_isOptionsZero && !_overlayTrigger.IsOverlayShow())
                {
                    await _overlayTrigger.Show();
                }
            }
        }

        public async Task SetSelectedItem(AutoCompleteOption item)
        {
            if (item != null)
            {
                this.SelectedValue = item.Value;
                this.SelectedItem = item;
                _inputComponent?.SetValue(this.SelectedItem.Label);

                if (OnSelectionChange.HasDelegate) await OnSelectionChange.InvokeAsync(this.SelectedItem);
            }
            this.ClosePanel();
        }

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

        private async void UpdateWidth(List<ResizeObserverEntry> entry)
        {
            await SetOverlayWidth();
            InvokeStateHasChanged();
        }

        private async Task SetOverlayWidth()
        {
            HtmlElement element = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _overlayTrigger.RefBack.Current);
            _minWidth = $"min-width:{(CssSizeLength)element.ClientWidth}";
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener.RemoveResizeObserver(_overlayTrigger.RefBack.Current, UpdateWidth);
            base.Dispose(disposing);
        }
    }

    public class AutoCompleteDataItem<TOption>
    {
        public AutoCompleteDataItem()
        {
        }

        public AutoCompleteDataItem(TOption value, string label)
        {
            Value = value;
            Label = label;
        }

        public TOption Value { get; set; }
        public string Label { get; set; }

        public bool IsDisabled { get; set; }
    }
}
