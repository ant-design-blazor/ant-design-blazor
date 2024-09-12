// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class TabPane : AntDomComponentBase
    {
        [CascadingParameter]
        private Tabs Parent { get; set; }

        /// <summary>
        /// Forced render of content in tabs, not lazy render after clicking on tabs
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool ForceRender { get; set; } = false;

        /// <summary>
        /// TabPane's key
        /// </summary>
        [Parameter]
        public string Key { get; set; }

        /// <summary>
        /// Show text in <see cref="TabPane"/>'s head
        /// </summary>
        [Parameter]
        public string Tab { get; set; }

        /// <summary>
        /// Template of TabPane's head
        /// </summary>
        [Parameter]
        public RenderFragment TabTemplate { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Template for customer context menu
        /// </summary>
        [Parameter]
        public RenderFragment TabContextMenu { get; set; }

        /// <summary>
        /// If the tab is disabled
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// If the tab is closable
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Closable { get; set; } = true;

        internal bool IsActive => _isActive;

        private bool HasTabTitle => Tab != null || TabTemplate != null;

        internal int TabIndex => _tabIndex;

        internal ElementReference TabRef => _tabRef;

        internal string TabId => $"rc-tabs-{Id}-tab-{Key}";

        internal ElementReference TabBtnRef => _tabBtnRef;

        private ClassMapper _tabPaneClassMapper = new();

        private const string PrefixCls = "ant-tabs-tab";
        private const string TabPanePrefixCls = "ant-tabs-tabpane";

        private ElementReference _tabRef;
        private ElementReference _tabBtnRef;
        private bool _isActive;

        private bool _hasClosed;

        private bool _hasRendered;
        private int _tabIndex;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.SetClass();

            Parent?.AddTabPane(this);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (HasTabTitle)
            {
                _hasRendered = true;
            }
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            if (parameters.IsParameterChanged(nameof(Tab), Tab))
            {
                Parent?.UpdateTabsPosition();
            }

            await base.SetParametersAsync(parameters);
        }

        private void SetClass()
        {
            ClassMapper
                .Add(PrefixCls)
                .If($"{PrefixCls}-active", () => _isActive)
                .If($"{PrefixCls}-with-remove", () => Closable)
                .If($"{PrefixCls}-disabled", () => Disabled);

            _tabPaneClassMapper
                .Add(TabPanePrefixCls)
                .If($"{TabPanePrefixCls}-active", () => _isActive)
                .If($"{TabPanePrefixCls}-hidden", () => !_isActive)
                ;
        }

        internal void SetActive(bool isActive)
        {
            if (_isActive != isActive)
            {
                _isActive = isActive;
                InvokeAsync(StateHasChanged);
            }
        }

        internal void SetIndex(int index)
        {
            _tabIndex = index;
        }

        internal void Close()
        {
            _hasClosed = true;

            Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            Parent?.RemovePane(this);

            base.Dispose(disposing);
        }

        internal void ExchangeWith(TabPane other)
        {
            (_tabIndex, other._tabIndex) = (other._tabIndex, _tabIndex);
        }

        private void HandleKeydown(KeyboardEventArgs e)
        {
            Parent?.HandleKeydown(e, this);
        }
    }
}
