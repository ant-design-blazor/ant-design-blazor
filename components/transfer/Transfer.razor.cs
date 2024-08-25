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
    public partial class Transfer : AntDomComponentBase
    {
        private const string PrefixName = "ant-transfer";
        private const string DisabledClass = "ant-transfer-list-content-item-disabled";
        private const string FooterClass = "ant-transfer-list-with-footer";

        [Parameter]
        public IEnumerable<TransferItem> DataSource { get; set; }

        [Parameter]
        public string[] Titles { get; set; } = new string[2];

        [Parameter]
        public string[] Operations { get; set; } = new string[2];

        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public bool ShowSearch { get; set; } = false;

        [Parameter]
        public bool ShowSelectAll { get; set; } = true;

        [Parameter]
        public IEnumerable<string> TargetKeys { get; set; }

        [Parameter]
        public IEnumerable<string> SelectedKeys { get; set; }

        [Parameter]
        public EventCallback<TransferChangeArgs> OnChange { get; set; }

        [Parameter]
        public EventCallback<TransferScrollArgs> OnScroll { get; set; }

        [Parameter]
        public EventCallback<TransferSearchArgs> OnSearch { get; set; }

        [Parameter]
        public EventCallback<TransferSelectChangeArgs> OnSelectChange { get; set; }

        [Parameter]
        public Func<TransferItem, OneOf<string, RenderFragment>> Render { get; set; }

        [Parameter]
        public TransferLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Transfer;

        [Parameter]
        public string Footer { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment FooterTemplate { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// A custom CSS style used for rendering the transfer columns
        /// </summary>
        [Parameter]
        public Func<TransferDirection, string> ListStyle { get; set; } = _ => string.Empty;

        private List<string> _targetKeys = new List<string>();
        private List<string> _selectedKeys = new List<string>();

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

        private bool _initialized = false;

        protected override void OnInitialized()
        {
            ClassMapper
                .Add(PrefixName)
                .If($"{PrefixName}-rtl", () => RTL);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var needRefreshTargetKeys = false;
            var needRefreshDataSource = false;
            var needRefreshSelectedKeys = false;
            if (parameters.TryGetValue(nameof(TargetKeys), out IEnumerable<string> newTargetKeys))
            {
                needRefreshTargetKeys |= TargetKeys != newTargetKeys;
            }

            if (parameters.TryGetValue(nameof(SelectedKeys), out IEnumerable<string> newSelectedKeys))
            {
                needRefreshSelectedKeys |= SelectedKeys != newSelectedKeys;
            }

            if (parameters.TryGetValue(nameof(DataSource), out IEnumerable<TransferItem> dataSource))
            {
                needRefreshDataSource |= DataSource == null || dataSource == null || !DataSource.SequenceEqual(dataSource);
            }

            await base.SetParametersAsync(parameters);

            if (needRefreshDataSource)
            {
                RefreshDataSource();

                if (_initialized)
                {
                    MathTitleCount();
                }

                _initialized = true;
            }

            if (needRefreshTargetKeys)
            {
                _targetKeys = TargetKeys.ToList();
                _selectedKeys.Clear();
                RefreshSelectedKeys();
            }

            if (needRefreshSelectedKeys)
            {
                _selectedKeys = SelectedKeys.ToList();
                RefreshSelectedKeys();
            }
        }

        private void RefreshSelectedKeys()
        {
            var removeKeys = new List<string>();
            foreach (var key in _selectedKeys)
            {
                if (DataSource.Any(x => x.Key == key && x.Disabled))
                {
                    removeKeys.Add(key);
                }
            }

            removeKeys.ForEach(k => _selectedKeys.Remove(k));

            _sourceSelectedKeys = _selectedKeys.Where(key => !_targetKeys.Contains(key)).ToList();
            _targetSelectedKeys = _selectedKeys.Where(key => _targetKeys.Contains(key)).ToList();

            MathTitleCount();
        }

        private void RefreshDataSource()
        {
            _leftDataSource = DataSource.Where(a => !_targetKeys.Contains(a.Key));
            _rightDataSource = DataSource.Where(a => _targetKeys.Contains(a.Key));
        }

        private void MathTitleCount()
        {
            _rightButtonDisabled = _sourceSelectedKeys?.Count == 0;
            _leftButtonDisabled = _targetSelectedKeys?.Count == 0;

            var leftDataSourceCount = _leftDataSource?.Count() ?? 0;
            var rightDataSourceCount = _rightDataSource?.Count() ?? 0;

            var leftSuffix = leftDataSourceCount == 1 ? Locale.ItemUnit : Locale.ItemsUnit;
            var rightSuffix = rightDataSourceCount == 1 ? Locale.ItemUnit : Locale.ItemsUnit;

            var leftCount = _sourceSelectedKeys?.Count == 0 ? $"{leftDataSourceCount}" : $"{_sourceSelectedKeys.Count}/{leftDataSourceCount}";
            var rightCount = _targetSelectedKeys?.Count == 0 ? $"{rightDataSourceCount}" : $"{_targetSelectedKeys.Count}/{rightDataSourceCount}";

            _leftCountText = $"{leftCount} {leftSuffix}";
            _rightCountText = $"{rightCount} {rightSuffix}";

            CheckAllState();
        }

        private async Task SelectItem(bool isCheck, TransferDirection direction, string key)
        {
            var holder = direction == TransferDirection.Left ? _sourceSelectedKeys : _targetSelectedKeys;
            var index = Array.IndexOf(holder.ToArray(), key);

            if (index > -1)
            {
                holder.RemoveAt(index);
            }
            if (isCheck)
                holder.Add(key);

            _selectedKeys = _sourceSelectedKeys.Union(_targetSelectedKeys).ToList();

            HandleSelect(direction, holder);

            MathTitleCount();

            if (OnSelectChange.HasDelegate)
            {
                await OnSelectChange.InvokeAsync(new TransferSelectChangeArgs(_sourceSelectedKeys.ToArray(), _targetSelectedKeys.ToArray()));
            }
        }

        private async Task SelectAll(bool isCheck, TransferDirection direction)
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

        private void HandleSelect(TransferDirection direction, List<string> keys)
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

        private async Task MoveItem(MouseEventArgs e, TransferDirection direction)
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

            RefreshDataSource();

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

        private async Task HandleScroll(TransferDirection direction, EventArgs e)
        {
            if (OnScroll.HasDelegate)
            {
                await OnScroll.InvokeAsync(new TransferScrollArgs(direction, e));
            }
        }

        private async Task HandleSearch(ChangeEventArgs e, TransferDirection direction, bool mathTileCount = true)
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

        private async Task ClearFilterValueAsync(TransferDirection direction)
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
