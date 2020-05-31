using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;

namespace AntBlazor
{
    public partial class AntCascader : AntDomComponentBase
    {
        [Parameter] public bool Readonly { get; set; } = true;

        [Parameter] public string Value { get; set; }

        /// <summary>
        /// 是否支持清除
        /// </summary>
        [Parameter] public bool AllowClear { get; set; } = true;

        /// <summary>
        /// 是否显示关闭图标
        /// </summary>
        private bool ShowClearIcon { get; set; }

        /// <summary>
        /// 当此项为 true 时，点选每级菜单选项值都会发生变化
        /// </summary>
        [Parameter] public bool ChangeOnSelect { get; set; }

        /// <summary>
        /// 默认的选中项
        /// </summary>
        [Parameter] public string DefaultValue { get; set; }

        /// <summary>
        /// 次级菜单的展开方式，可选 'click' 和 'hover'
        /// </summary>
        [Parameter] public string ExpandTrigger { get; set; }

        /// <summary>
        /// 当下拉列表为空时显示的内容
        /// </summary>
        [Parameter] public string NotFoundContent { get; set; } = "Not Found";

        /// <summary>
        /// 输入框占位文本
        /// </summary>
        [Parameter] public string PlaceHolder { get; set; } = "Please Select";

        /// <summary>
        /// 在选择框中显示搜索框
        /// </summary>
        [Parameter] public bool ShowSearch { get; set; }

        /// <summary>
        /// 输入框大小，可选 'large', 'middle', 'small'
        /// </summary>
        [Parameter] public string Size { get; set; } = "large";

        /// <summary>
        /// 选择完成后的回调(参数为选中的节点集合及选中值)
        /// </summary>
        [Parameter] public Action<List<AntCascaderNode>, string> OnChange { get; set; }

        [Parameter]
        public IReadOnlyCollection<AntCascaderNode> Nodes
        {
            get
            {
                if (_nodelist != null)
                    return _nodelist;
                return Array.Empty<AntCascaderNode>();
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    _nodelist = null;
                    return;
                }
                if (_nodelist == null) _nodelist = new List<AntCascaderNode>();
                else if (_nodelist.Count != 0) _nodelist.Clear();
                _nodelist.AddRange(value);
            }
        }

        private List<AntCascaderNode> _nodelist;

        /// <summary>
        /// 选中节点集合
        /// </summary>
        internal List<AntCascaderNode> _selectedNodes = new List<AntCascaderNode>();

        internal List<AntCascaderNode> _hoverSelectedNodes = new List<AntCascaderNode>();

        /// <summary>
        /// 展开/折叠状态
        /// </summary>
        private bool ToggleState { get; set; }

        private bool IsOnCascader { get; set; }

        private SelectedTypeEnum SelectedType { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitCascaderNodeState(_nodelist, null, 0);
            SetDefaultValue(DefaultValue);
        }

        /// <summary>
        /// 渲染组件 dom
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null) return;

            builder.OpenElement(1, "div");
            builder.AddAttribute(2, "style", Style);
            //输入框DOM
            builder.OpenElement(1, "span");
            string pickerSizeClass = string.Empty;
            if (Size == "large") pickerSizeClass = "ant-cascader-picker-large"; else if (Size == "small") pickerSizeClass = "ant-cascader-picker-small";
            builder.AddAttribute(2, "class", $"ant-cascader-picker {pickerSizeClass}");
            builder.AddAttribute(3, "tabindex", "0");
            builder.AddAttribute(4, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, async (me) =>
            {
                await Task.Delay(1);
                SelectedType = SelectedTypeEnum.Click;
                _hoverSelectedNodes.Clear();
                ToggleState = !ToggleState;
            }));
            builder.AddAttribute(5, "onblur", EventCallback.Factory.Create<FocusEventArgs>(this, async (me) =>
            {
                await Task.Delay(1);
                if (!IsOnCascader) ToggleState = false;
            }));
            if (AllowClear)
            {
                builder.AddAttribute(6, "onmouseover", EventCallback.Factory.Create<MouseEventArgs>(this, async (me) =>
                {
                    await Task.Delay(1);
                    ShowClearIcon = true;
                }));
                builder.AddAttribute(7, "onmouseout", EventCallback.Factory.Create<MouseEventArgs>(this, async (me) =>
                {
                    await Task.Delay(1);
                    ShowClearIcon = false;
                }));
            }

            builder.OpenElement(1, "span");
            builder.AddAttribute(2, "class", "ant-cascader-picker-label");
            builder.CloseElement();

            builder.OpenElement(1, "input");
            builder.AddAttribute(2, "autocomplete", "off");
            builder.AddAttribute(3, "tabindex", "-1");
            string inputSizeClass = string.Empty;
            if (Size == "large") inputSizeClass = "ant-input-lg"; else if (Size == "small") inputSizeClass = "ant-input-sm";
            builder.AddAttribute(7, "class", $"ant-input ant-cascader-input {inputSizeClass}");
            builder.AddAttribute(8, "placeholder", PlaceHolder);
            builder.AddAttribute(9, "readonly", Readonly);
            builder.AddAttribute(11, "type", "text");
            builder.AddAttribute(13, "value", Value);
            builder.CloseElement();

            if (string.IsNullOrWhiteSpace(Value) || !ShowClearIcon)
            {
                string spanExpandClass = ToggleState ? "ant-cascader-picker-arrow-expand" : string.Empty;
                string html = $"<span role='img' aria-label='down' class='anticon anticon-down ant-cascader-picker-arrow {spanExpandClass}'><svg viewBox='64 64 896 896' focusable='false' class='' data-icon='down' width='1em' height='1em' fill='currentColor' aria-hidden='true'><path d='M884 256h-75c-5.1 0-9.9 2.5-12.9 6.6L512 654.2 227.9 262.6c-3-4.1-7.8-6.6-12.9-6.6h-75c-6.5 0-10.3 7.4-6.5 12.7l352.6 486.1c12.8 17.6 39 17.6 51.7 0l352.6-486.1c3.9-5.3.1-12.7-6.4-12.7z'></path></svg></span>";
                builder.AddContent(14, (MarkupString)html);
            }
            else
            {
                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "role", "img");
                builder.AddAttribute(3, "aria-label", "close-circle");
                builder.AddAttribute(4, "tabindex", "-1");
                builder.AddAttribute(5, "class", "anticon anticon-close-circle ant-cascader-picker-clear");
                builder.AddAttribute(6, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, async (me) =>
                {
                    await Task.Delay(1);
                    _selectedNodes.Clear();
                    _hoverSelectedNodes.Clear();
                    Value = string.Empty;
                }));
                builder.AddEventStopPropagationAttribute(7, "onclick", true);
                string html = "<svg viewBox='64 64 896 896' focusable='false' class='' data-icon='close-circle' width='1em' height='1em' fill='currentColor' aria-hidden='true'><path d='M512 64C264.6 64 64 264.6 64 512s200.6 448 448 448 448-200.6 448-448S759.4 64 512 64zm165.4 618.2l-66-.3L512 563.4l-99.3 118.4-66.1.3c-4.4 0-8-3.5-8-8 0-1.9.7-3.7 1.9-5.2l130.1-155L340.5 359a8.32 8.32 0 01-1.9-5.2c0-4.4 3.6-8 8-8l66.1.3L512 464.6l99.3-118.4 66-.3c4.4 0 8 3.5 8 8 0 1.9-.7 3.7-1.9 5.2L553.5 514l130 155c1.2 1.5 1.9 3.3 1.9 5.2 0 4.4-3.6 8-8 8z'></path></svg>";
                builder.AddContent(14, (MarkupString)html);
                builder.CloseElement();
            }

            builder.CloseElement();
            builder.CloseElement();

            //下拉选项DOM
            if (ToggleState)
            {
                builder.OpenElement(1, "div");
                builder.AddAttribute(2, "class", "ant-cascader-menus ant-cascader-menus-placement-bottomLeft");

                builder.OpenElement(3, "div");
                builder.AddAttribute(4, "onmouseover", EventCallback.Factory.Create<MouseEventArgs>(this, async (me) =>
                {
                    await Task.Delay(1);
                    IsOnCascader = true;
                }));
                builder.AddAttribute(5, "onmouseout", EventCallback.Factory.Create<MouseEventArgs>(this, async (me) =>
                {
                    await Task.Delay(1);
                    IsOnCascader = false;
                }));
                RenderCascader(builder);
                builder.CloseElement();

                builder.CloseElement();
            }
        }

        internal void RenderCascader(RenderTreeBuilder builder)
        {
            //一级节点
            builder.OpenElement(1, "ul");
            builder.AddAttribute(2, "class", "ant-cascader-menu");

            foreach (AntCascaderNode node in _nodelist)
                node?.RenderRecursive(builder, this);

            builder.CloseElement();

            List<AntCascaderNode> list = new List<AntCascaderNode>();
            if (SelectedType == SelectedTypeEnum.Click)
                list = _selectedNodes;
            else
                list = _hoverSelectedNodes;
            //其他等级节点
            list.Sort((x, y) => x.Level.CompareTo(y.Level));  //Level 升序排序
            foreach (AntCascaderNode node in list)
            {
                if (node == null || !node.HasChildren) return;

                builder.OpenElement(1, "ul");
                builder.AddAttribute(2, "class", "ant-cascader-menu");

                foreach (AntCascaderNode nd in node.Children)
                    nd?.RenderRecursive(builder, this);

                builder.CloseElement();
            }
        }

        internal async Task SetSelectedNode(AntCascaderNode cascaderNode, SelectedTypeEnum selectedType)
        {
            await Task.Delay(1);
            if (cascaderNode == null) return;

            SelectedType = selectedType;
            if (selectedType == SelectedTypeEnum.Click)
            {
                //清空并设置新的选中节点
                _selectedNodes.Clear();
                SetSelectedNodeWithParent(cascaderNode, ref _selectedNodes);

                if (ChangeOnSelect || !cascaderNode.HasChildren)
                    SetValue(cascaderNode.Value);
            }
            else
            {
                _hoverSelectedNodes.Clear();
                SetSelectedNodeWithParent(cascaderNode, ref _hoverSelectedNodes);
            }

            if (!cascaderNode.HasChildren)
            {
                ToggleState = false;
                IsOnCascader = false;
            }

            StateHasChanged();
        }

        private void SetSelectedNodeWithParent(AntCascaderNode node, ref List<AntCascaderNode> list)
        {
            if (node == null) return;

            list.Add(node);
            SetSelectedNodeWithParent(node.ParentCascaderNode, ref list);
        }

        private void InitCascaderNodeState(List<AntCascaderNode> list, AntCascaderNode parentNode, int level)
        {
            if (list == null) return;

            foreach (var node in list)
            {
                node.Level = level;
                node.ParentCascaderNode = parentNode;

                if (node.HasChildren)
                    InitCascaderNodeState(node.Children.ToList(), node, level + 1);
            }
        }

        private void SetDefaultValue(string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(defaultValue))
                return;

            var node = GetNodeByValue(_nodelist, defaultValue);
            SetSelectedNodeWithParent(node, ref _selectedNodes);
            SetValue(node.Value);
        }

        private void SetValue(string value)
        {
            _selectedNodes.Sort((x, y) => x.Level.CompareTo(y.Level));  //Level 升序排序
            string v = string.Empty; int count = 0;
            foreach (var node in _selectedNodes)
            {
                if (node == null) continue;

                if (count < _selectedNodes.Count - 1)
                    v += node.Label + " / ";
                else
                    v += node.Label;
                count++;
            }
            Value = v;

            if (OnChange != null)
                OnChange(_selectedNodes, value);
        }

        private AntCascaderNode GetNodeByValue(List<AntCascaderNode> list, string value)
        {
            if (list == null) return null;
            AntCascaderNode result = null;

            foreach (var node in list)
            {
                if (node.Value == value)
                    return node;

                if (node.HasChildren)
                {
                    var nd = GetNodeByValue(node.Children.ToList(), value);
                    if (nd != null)
                        result = nd;
                }
            }
            return result;
        }
    }

    internal enum SelectedTypeEnum
    {
        Click,
        Hover
    }
}
