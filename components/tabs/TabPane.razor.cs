﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class TabPane : AntDomComponentBase
    {
        [CascadingParameter(Name = "IsTab")]
        internal bool IsTab { get; set; }

        [CascadingParameter(Name = "IsPane")]
        internal bool IsPane { get; set; }

        [CascadingParameter]
        private Tabs Parent { get; set; }

        /// <summary>
        /// Forced render of content in tabs, not lazy render after clicking on tabs
        /// </summary>
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

        [Parameter]
        public RenderFragment TabTemplate { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment TabContextMenu { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Closable { get; set; } = true;
        internal bool IsActive => _isActive;

        private bool HasTabTitle => Tab != null || TabTemplate != null;

        internal ElementReference TabRef => _tabRef;

        private const string PrefixCls = "ant-tabs-tab";

        private ElementReference _tabRef;
        private bool _isActive;
        private bool _shouldRender;
        private bool _shouldTabRender;
        private bool _hasClosed;
        private bool _hasRendered;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.SetClass();

            Parent?.AddTabPane(this);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (IsTab && HasTabTitle)
            {
                _hasRendered = true;
            }

            _shouldRender = false;
            _shouldTabRender = false;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            _shouldRender = true;
            _shouldTabRender = true;
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            // Avoid changes in tab as we modify the properties used for display when drag and drop occurs
            if (IsTab && _hasRendered)
            {
                return;
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
        }

        internal void SetKey(string key)
        {
            Key = key;
        }

        internal void SetActive(bool isActive)
        {
            if (_isActive != isActive)
            {
                _isActive = isActive;
                _shouldTabRender = true;
                InvokeAsync(StateHasChanged);
            }
        }

        internal void Close()
        {
            _hasClosed = true;

            _shouldTabRender = true;
            _shouldRender = true;

            Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            Parent?.RemovePane(this);

            base.Dispose(disposing);
        }

        protected override bool ShouldRender()
        {
            if (IsTab)
            {
                return _shouldTabRender;
            }

            return _shouldRender;
        }

        internal void ExchangeWith(TabPane other)
        {
            var temp = other.Clone();
            other.SetPane(this);
            this.SetPane(temp);
        }

        private TabPane Clone()
        {
            return new TabPane
            {
                Key = Key,
                Tab = this.Tab,
                TabTemplate = this.TabTemplate,
                Disabled = this.Disabled,
                Closable = this.Closable,
            };
        }

        private void SetPane(TabPane tabPane)
        {
            Key = tabPane.Key;
            Tab = tabPane.Tab;
            TabTemplate = tabPane.TabTemplate;
            Disabled = tabPane.Disabled;
            Closable = tabPane.Closable;

            _shouldTabRender = true;
            StateHasChanged();
        }
    }
}
