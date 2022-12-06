// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Double column transfer choice box.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>It is a select control essentially which can be use for selecting multiple items.</item>
        <item>Transfer can display more information for items and take up more space.</item>
    </list>

    <para>Transfer the elements between two columns in an intuitive and efficient way.</para>

    <para>
        One or more elements can be selected from either column, one click on the proper `direction` button, and the transfer is done. 
        The left column is considered the `source` and the right column is considered the `target`. 
        As you can see in the API description, these names are reflected in.
    </para>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/QAXskNI4G/Transfer.svg", Columns = 1)]
    public partial class Transfer : AntDomComponentBase
    {
        private const string PrefixName = "ant-transfer";
        private const string DisabledClass = "ant-transfer-list-content-item-disabled";
        private const string FooterClass = "ant-transfer-list-with-footer";

        /// <summary>
        /// Used for setting the source data. 
        /// The elements that are part of this array will be present jn the left column, except for the elements whose keys are included in <see cref="TargetKeys"/>.
        /// </summary>
        [Parameter]
        public IList<TransferItem> DataSource { get; set; }

        /// <summary>
        /// Titles for the columns left to right. Must be an array with 2 elements.
        /// </summary>
        [Parameter]
        public string[] Titles { get; set; } = new string[2];

        /// <summary>
        /// Labels to display next to the icons for the operations, from top to bottom. Must be an array with 2 elements.
        /// </summary>
        [Parameter]
        public string[] Operations { get; set; } = new string[2];

        /// <summary>
        /// Whether the component is disabled or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// Whether to show search bars in the columns or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool ShowSearch { get; set; } = false;

        /// <summary>
        /// Whether to show select all buttons in the columns or not
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool ShowSelectAll { get; set; } = true;

        /// <summary>
        /// Keys of elements that are listed in the right column
        /// </summary>
        [Parameter]
        public string[] TargetKeys { get; set; }

        /// <summary>
        /// Currently selected items
        /// </summary>
        [Parameter]
        public string[] SelectedKeys { get; set; }

        /// <summary>
        /// Callback executed when an item moves columns
        /// </summary>
        [Parameter]
        public EventCallback<TransferChangeArgs> OnChange { get; set; }

        /// <summary>
        /// Callback executed when a column is scrolled
        /// </summary>
        [Parameter]
        public EventCallback<TransferScrollArgs> OnScroll { get; set; }

        /// <summary>
        /// Callback executed when a column is searched
        /// </summary>
        [Parameter]
        public EventCallback<TransferSearchArgs> OnSearch { get; set; }

        /// <summary>
        /// Callbac executed when the selected items change in either column
        /// </summary>
        [Parameter]
        public EventCallback<TransferSelectChangeArgs> OnSelectChange { get; set; }

        /// <summary>
        /// Custom render for the items in the columns
        /// </summary>
        [Parameter]
        public Func<TransferItem, OneOf<string, RenderFragment>> Render { get; set; }

        /// <summary>
        /// Locacle used for localization of text
        /// </summary>
        [Parameter]
        public TransferLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Transfer;

        /// <summary>
        /// Footer string displayed in the footer of each column
        /// </summary>
        [Parameter]
        public string Footer { get; set; } = string.Empty;

        /// <summary>
        /// Footer content displayed in the footer of each column
        /// </summary>
        [Parameter]
        public RenderFragment FooterTemplate { get; set; }

        private List<string> _targetKeys;

        private bool _leftCheckAllState = false;
        private bool _leftCheckAllIndeterminate = false;
        private bool _rightCheckAllState = false;
        private bool _rightCheckAllIndeterminate = false;

        private string _leftCountText = string.Empty;
        private string _rightCountText = string.Empty;

        private bool _leftButtonDisabled = true;
        private bool _rightButtonDisabled = true;

        private IEnumerable<TransferItem> _leftDataSource;
        private IEnumerable<TransferItem> _rightDataSource;

        private List<string> _sourceSelectedKeys;
        private List<string> _targetSelectedKeys;

        private string _leftFilterValue = string.Empty;
        private string _rightFilterValue = string.Empty;

        protected override void OnInitialized()
        {
            ClassMapper
                .Add(PrefixName)
                .If($"{PrefixName}-rtl", () => RTL);

            _targetKeys = TargetKeys.ToList();
            var selectedKeys = SelectedKeys.ToList();
            _sourceSelectedKeys = selectedKeys.Where(key => !_targetKeys.Contains(key)).ToList();
            _targetSelectedKeys = selectedKeys.Where(key => _targetKeys.Contains(key)).ToList();
            var count = _sourceSelectedKeys.Count;

            InitData();

            MathTitleCount();
        }

        private void InitData()
        {
            _leftDataSource = DataSource.Where(a => !_targetKeys.Contains(a.Key));
            _rightDataSource = DataSource.Where(a => _targetKeys.Contains(a.Key));
        }

        private void MathTitleCount()
        {
            _rightButtonDisabled = _sourceSelectedKeys.Count == 0;
            _leftButtonDisabled = _targetSelectedKeys.Count == 0;

            var leftSuffix = _leftDataSource.Count() == 1 ? Locale.ItemUnit : Locale.ItemsUnit;
            var rightSuffix = _rightDataSource.Count() == 1 ? Locale.ItemUnit : Locale.ItemsUnit;

            var leftCount = _sourceSelectedKeys.Count == 0 ? $"{_leftDataSource.Count()}" : $"{_sourceSelectedKeys.Count}/{_leftDataSource.Count()}";
            var rightCount = _targetSelectedKeys.Count == 0 ? $"{_rightDataSource.Count()}" : $"{_targetSelectedKeys.Count}/{_rightDataSource.Count()}";

            _leftCountText = $"{leftCount} {leftSuffix}";
            _rightCountText = $"{rightCount} {rightSuffix}";

            CheckAllState();
        }

        private async Task SelectItem(bool isCheck, string direction, string key)
        {
            var holder = direction == TransferDirection.Left ? _sourceSelectedKeys : _targetSelectedKeys;
            var index = Array.IndexOf(holder.ToArray(), key);

            if (index > -1)
            {
                holder.RemoveAt(index);
            }
            if (isCheck)
                holder.Add(key);

            HandleSelect(direction, holder);

            MathTitleCount();

            if (OnSelectChange.HasDelegate)
            {
                await OnSelectChange.InvokeAsync(new TransferSelectChangeArgs(_sourceSelectedKeys.ToArray(), _targetSelectedKeys.ToArray()));
            }
        }

        private async Task SelectAll(bool isCheck, string direction)
        {
            var list = _leftDataSource;
            if (direction == TransferDirection.Right)
            {
                list = _rightDataSource;
            }

            var holder = isCheck ? list.Where(a => !a.Disabled).Select(a => a.Key).ToList() : new List<string>(list.Count());

            HandleSelect(direction, holder);

            MathTitleCount();

            if (OnSelectChange.HasDelegate)
            {
                await OnSelectChange.InvokeAsync(new TransferSelectChangeArgs(_sourceSelectedKeys.ToArray(), _targetSelectedKeys.ToArray()));
            }
        }

        private void HandleSelect(string direction, List<string> keys)
        {
            if (direction == TransferDirection.Left)
            {
                _sourceSelectedKeys = keys;
            }
            else
            {
                _targetSelectedKeys = keys;
            }
        }

        private async Task MoveItem(MouseEventArgs e, string direction)
        {
            var moveKeys = direction == TransferDirection.Right ? _sourceSelectedKeys : _targetSelectedKeys;

            if (direction == TransferDirection.Left)
            {
                _targetKeys.RemoveAll(key => moveKeys.Contains(key));
            }
            else
            {
                _targetKeys.AddRange(moveKeys);
            }

            InitData();

            var oppositeDirection = direction == TransferDirection.Right ? TransferDirection.Left : TransferDirection.Right;

            HandleSelect(oppositeDirection, new List<string>());

            if (!string.IsNullOrEmpty(_leftFilterValue))
            {
                await HandleSearch(new ChangeEventArgs() { Value = _leftFilterValue }, TransferDirection.Left, false);
            }

            if (!string.IsNullOrEmpty(_rightFilterValue))
            {
                await HandleSearch(new ChangeEventArgs() { Value = _rightFilterValue }, TransferDirection.Right, false);
            }

            MathTitleCount();

            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(new TransferChangeArgs(_targetKeys.ToArray(), direction, moveKeys.ToArray()));
            }
        }

        private void CheckAllState()
        {
            if (_leftDataSource.Any(a => !a.Disabled))
            {
                _leftCheckAllState = _sourceSelectedKeys.Count == _leftDataSource.Count(a => !a.Disabled);
            }
            else
            {
                _leftCheckAllState = false;
            }

            _leftCheckAllIndeterminate = !_leftCheckAllState && _sourceSelectedKeys.Count > 0;

            if (_rightDataSource.Any(a => !a.Disabled))
            {
                _rightCheckAllState = _targetSelectedKeys.Count == _rightDataSource.Count(a => !a.Disabled);
            }
            else
            {
                _rightCheckAllState = false;
            }

            _rightCheckAllIndeterminate = !_rightCheckAllState && _targetSelectedKeys.Count > 0;
        }

        private async Task HandleScroll(string direction, EventArgs e)
        {
            if (OnScroll.HasDelegate)
            {
                await OnScroll.InvokeAsync(new TransferScrollArgs(direction, e));
            }
        }

        private async Task HandleSearch(ChangeEventArgs e, string direction, bool mathTileCount = true)
        {
            if (direction == TransferDirection.Left)
            {
                _leftFilterValue = e.Value.ToString();
                _leftDataSource = DataSource.Where(a => !_targetKeys.Contains(a.Key) && a.Title.Contains(_leftFilterValue, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            else
            {
                _rightFilterValue = e.Value.ToString();
                _rightDataSource = DataSource.Where(a => _targetKeys.Contains(a.Key) && a.Title.Contains(_rightFilterValue, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (mathTileCount)
                MathTitleCount();

            if (OnSearch.HasDelegate)
            {
                await OnSearch.InvokeAsync(new TransferSearchArgs(direction, e.Value.ToString()));
            }
        }

        private async Task ClearFilterValueAsync(string direction)
        {
            if (direction == TransferDirection.Left)
            {
                _leftFilterValue = string.Empty;
                await HandleSearch(new ChangeEventArgs() { Value = string.Empty }, direction);
            }
            else
            {
                _rightFilterValue = string.Empty;
                await HandleSearch(new ChangeEventArgs() { Value = string.Empty }, direction);
            }
        }
    }
}
