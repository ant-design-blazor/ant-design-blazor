﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class TabPane : AntDomComponentBase
    {
        private const string PrefixCls = "ant-tabs-tab";

        internal bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                }
            }
        }

        internal bool HasRendered { get; set; }
        internal ElementReference TabBar { get; set; }

        [CascadingParameter(Name = "IsEmpty")]
        private bool IsEmpty { get; set; }

        [CascadingParameter(Name = "IsTab")]
        private bool IsTab { get; set; }

        [CascadingParameter(Name = "IsPane")]
        private bool IsPane { get; set; }

        [CascadingParameter]
        private Tabs Parent { get; set; }

        [CascadingParameter(Name = "Pane")]
        private TabPane Pane { get; set; }

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
        public bool Disabled { get; set; }

        [Parameter]
        public bool Closable { get; set; } = true;

        private bool _hasTab;
        private bool _hasContent;
        private bool _isActive;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (IsEmpty)
            {
                Parent?.AddTabPane(this);
            }

            if (IsTab && !Pane._hasTab)
            {
                Pane.SetTab();

                Pane.ClassMapper
                 .Add(PrefixCls)
                 .If($"{PrefixCls}-active", () => Pane?.IsActive == true)
                 .If($"{PrefixCls}-with-remove", () => Pane?.Closable == true)
                 .If($"{PrefixCls}-disabled", () => Pane?.Disabled == true);

                if (Parent?.Card != null)
                {
                    Parent.Complete(Pane);
                }
            }

            if (IsPane && !Pane._hasContent)
            {
                Pane.SetContent();
                Parent.Complete(Pane);
            }
        }

        private void SetTab()
        {
            _hasTab = true;
        }

        private void SetContent()
        {
            _hasContent = true;
        }

        internal bool IsComplete() => _hasTab && (Parent?.Card != null || _hasContent);

        protected override bool ShouldRender() => Pane?.Key == Key;

        protected override void Dispose(bool disposing)
        {
            if (IsEmpty)
            {
                Parent?._panes.Remove(this);
            }
            else
            {
                Pane = null;
            }
            base.Dispose(disposing);
        }
    }
}
