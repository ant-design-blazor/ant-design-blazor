// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
        <para>A content area which can be collapsed and expanded.</para>

        <h2>When To Use</h2>

        <list type="bullet">
            <item>Can be used to group or hide complex regions to keep the page clean.</item>
            <item><c>Accordion</c> is a special kind of <c>Collapse</c>, which allows only one panel to be expanded at a time.</item>
        </list>
    </summary>
    <seealso cref="Panel"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/IxH16B9RD/Collapse.svg", Columns = 1, Title = "Collapse", SubTitle = "折叠面板")]
    public partial class Collapse : AntDomComponentBase
    {
        #region Parameter

        /// <summary>
        /// Enable/disable accordion mode. When true, only one panel can be open at once. When opening another the rest collapse.
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Accordion { get; set; }

        /// <summary>
        /// Enable/disable border
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool Bordered { get; set; } = true;

        /// <summary>Whether to render the Collapse without its container background and border.</summary>
        [Parameter]
        public bool Ghost { get; set; }

        /// <summary>Set the size of the Collapse headers.</summary>
        [Parameter]
        public CollapseSize Size { get; set; } = CollapseSize.Middle;

        /// <summary>Controls which part of each panel header is clickable. Null makes the whole header interactive; Header limits interaction to title and icon.</summary>
        [Parameter]
        public CollapseCollapsible? Collapsible { get; set; }

        /// <summary>Semantic classes for the root, header, title, body and icon slots.</summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> ClassNames { get; set; }

        /// <summary>Semantic inline styles for the root, header, title, body and icon slots.</summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> Styles { get; set; }

        /// <summary>
        /// Expand icon position
        /// </summary>
        /// <default value="CollapseExpandIconPosition.Left"/>
        [Parameter]
        public CollapseExpandIconPosition ExpandIconPosition { get; set; } = CollapseExpandIconPosition.Left;

        /// <summary>
        /// Default <see cref="Panel"/> element's <see cref="Panel.Key"/>
        /// </summary>
        [Parameter]
        public string[] DefaultActiveKey { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Currently active panel keys with two-way binding support
        /// </summary>
        [Parameter]
        public string[] ActiveKeys { get; set; }

        /// <summary>
        /// Two-way binding parameter callback for ActiveKeys
        /// </summary>
        [Parameter]
        public EventCallback<string[]> ActiveKeysChanged { get; set; }

        /// <summary>
        /// Callback executed when open panels change
        /// </summary>
        [Parameter]
        public EventCallback<string[]> OnChange { get; set; }

        /// <summary>
        /// Icon to display in <see cref="ExpandIconPosition"/>
        /// </summary>
        /// <default value="right"/>
        [Parameter]
        public string ExpandIcon { get; set; } = IconType.Outline.Right;

        /// <summary>
        /// Expand icon content to display in <see cref="ExpandIconPosition"/>. Takes priority over <see cref="ExpandIcon"/>
        /// </summary>
        [Parameter]
        public RenderFragment<bool> ExpandIconTemplate { get; set; }


        /// <summary>
        /// Whether enable the expand/collapse animation
        /// </summary>
        [Parameter]
        public bool Animation { get; set; }


        #endregion Parameter

        /// <summary>
        /// Content of the collapse. Typically contains <see cref="Panel"/> elements
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private IList<Panel> Items { get; } = new List<Panel>();

        private string[] _internalActiveKeys;
        private DotNetObjectReference<Collapse> _dotNetReference;
        private bool _clientMotionAttached;
        internal string ClientActiveKeysJson => JsonSerializer.Serialize(EffectiveActiveKeys ?? Array.Empty<string>());

        /// <summary>
        /// Get the effective active keys, either from external binding or internal state
        /// </summary>
        private string[] EffectiveActiveKeys => ActiveKeys ?? _internalActiveKeys;

        private void SetClassMap()
        {
            ClassMapper
                .Add("ant-collapse")
                .If("ant-collapse-icon-placement-start", () => ExpandIconPosition == CollapseExpandIconPosition.Left)
                .If("ant-collapse-icon-placement-end", () => ExpandIconPosition == CollapseExpandIconPosition.Right)
                .If("ant-collapse-borderless", () => !this.Bordered)
                .If("ant-collapse-ghost", () => Ghost)
                .If("ant-collapse-small", () => Size == CollapseSize.Small)
                .If("ant-collapse-large", () => Size == CollapseSize.Large)
                .If("ant-collapse-rtl", () => RTL);
        }

        internal string SemanticClass(string slot, string baseClass)
        {
            var customClass = ClassNames != null && ClassNames.TryGetValue(slot, out var value) ? value : null;
            return string.IsNullOrWhiteSpace(customClass) ? baseClass : $"{baseClass} {customClass}";
        }

        internal string SemanticStyle(string slot, string baseStyle = null)
        {
            var customStyle = Styles != null && Styles.TryGetValue(slot, out var value) ? value : null;
            if (string.IsNullOrWhiteSpace(customStyle)) return baseStyle;
            if (string.IsNullOrWhiteSpace(baseStyle)) return customStyle;
            return $"{baseStyle.Trim().TrimEnd(';')}; {customStyle}";
        }

        protected override async Task OnInitializedAsync()
        {
            SetClassMap();
            _internalActiveKeys = DefaultActiveKey;
            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (ActiveKeys != null)
            {
                foreach (var item in Items)
                {
                    var shouldBeActive = item.Key.IsIn(ActiveKeys);
                    if (item.Active != shouldBeActive)
                    {
                        item.SetActiveInt(shouldBeActive);
                    }
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (Animation && !_clientMotionAttached && !IsDisposed)
            {
                _dotNetReference ??= DotNetObjectReference.Create(this);
                await JsInvokeAsync(JSInteropConstants.CollapseHelper.Attach, _dotNetReference, Ref);
                _clientMotionAttached = true;
            }
            else if (!Animation && _clientMotionAttached)
            {
                await DetachClientMotionAsync();
            }
        }

        [JSInvokable]
        public async Task ApplyClientActiveKeys(string[] activeKeys, bool deferInitialContent = false)
        {
            activeKeys ??= Array.Empty<string>();
            var selectedKeys = Items.Where(item => activeKeys.Contains(item.Key)).Select(item => item.Key).ToArray();
            foreach (var item in Items)
            {
                await item.SetActiveFromClientAsync(selectedKeys.Contains(item.Key), deferInitialContent);
            }

            if (ActiveKeys == null)
            {
                _internalActiveKeys = selectedKeys;
            }
            else if (ActiveKeysChanged.HasDelegate)
            {
                await ActiveKeysChanged.InvokeAsync(selectedKeys);
            }

            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(selectedKeys);
            }
        }

        [JSInvokable]
        public Task SetPanelClientHidden(string key, bool hidden)
        {
            var panel = Items.FirstOrDefault(item => item.Key == key);
            // A close transition can finish after the same panel has already been reopened.
            if (panel == null || (hidden && panel.Active)) return Task.CompletedTask;
            panel.SetClientHidden(hidden);
            return Task.CompletedTask;
        }

        internal void AddPanel(Panel panel)
        {
            this.Items.Add(panel);
            if (panel.Key.IsIn(EffectiveActiveKeys))
            {
                panel.SetActiveInt(true);
            }

            StateHasChanged();
        }

        internal void RemovePanel(Panel panel)
        {
            this.Items.Remove(panel);
        }

        internal void TogglePanelState(Panel panel)
        {
            if (panel == null)
            {
                return;
            }

            var newActiveKeys = new List<string>(EffectiveActiveKeys);

            if (this.Accordion && !panel.Active)
            {
                this.Items.Where(item => item != panel && item.Active)
                    .ForEach(item => item.SetActiveInt(false));
                newActiveKeys.Clear();
            }

            var isActive = !panel.Active;
            panel.SetActiveInt(isActive);

            if (isActive)
            {
                if (!newActiveKeys.Contains(panel.Key))
                {
                    newActiveKeys.Add(panel.Key);
                }
            }
            else
            {
                newActiveKeys.Remove(panel.Key);
            }

            var selectedKeys = newActiveKeys.ToArray();

            if (ActiveKeys != null)
            {
                if (ActiveKeysChanged.HasDelegate)
                {
                    ActiveKeysChanged.InvokeAsync(selectedKeys);
                }
            }
            else
            {
                _internalActiveKeys = selectedKeys;
            }

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(selectedKeys);
            }

            if (panel.OnActiveChange.HasDelegate)
            {
                panel.OnActiveChange.InvokeAsync(panel.Active);
            }
        }

        /// <summary>
        /// Activate the specified panels
        /// </summary>
        /// <param name="activeKeys"></param>
        public void Activate(params string[] activeKeys)
            => Activate(activeKeys.AsSpan());

        private void Activate(params ReadOnlySpan<string> activeKeys)
        {
            var newActiveKeys = new List<string>();

            foreach (var item in Items)
            {
                if (item.Key.IsIn(activeKeys))
                {
                    newActiveKeys.Add(item.Key);
                    item.SetActiveInt(true);
                }
                else if (this.Accordion)
                {
                    item.SetActiveInt(false);
                }
            }

            var selectedKeys = newActiveKeys.ToArray();

            if (ActiveKeys != null)
            {
                if (ActiveKeysChanged.HasDelegate)
                {
                    ActiveKeysChanged.InvokeAsync(selectedKeys);
                }
            }
            else
            {
                _internalActiveKeys = selectedKeys;
            }

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(selectedKeys);
            }
        }

        /// <summary>
        /// Deactivate the specified panels
        /// </summary>
        /// <param name="inactiveKeys"></param>
        public void Deactivate(params string[] inactiveKeys)
            => Deactivate(inactiveKeys.AsSpan());

        private void Deactivate(params ReadOnlySpan<string> inactiveKeys)
        {
            var newActiveKeys = new List<string>(EffectiveActiveKeys);

            foreach (var item in Items)
            {
                if (item.Key.IsIn(inactiveKeys))
                {
                    item.SetActiveInt(false);
                    newActiveKeys.Remove(item.Key);
                }
            }

            var selectedKeys = newActiveKeys.ToArray();

            if (ActiveKeys != null)
            {
                if (ActiveKeysChanged.HasDelegate)
                {
                    ActiveKeysChanged.InvokeAsync(selectedKeys);
                }
            }
            else
            {
                _internalActiveKeys = selectedKeys;
            }

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(selectedKeys);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_clientMotionAttached)
            {
                _ = DetachClientMotionAsync();
            }
            else
            {
                _dotNetReference?.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task DetachClientMotionAsync()
        {
            try
            {
                await JsInvokeAsync(JSInteropConstants.CollapseHelper.Detach, Ref);
            }
            catch (JSDisconnectedException) { }
            catch (TaskCanceledException) { }
            catch (ObjectDisposedException) { }
            finally
            {
                _clientMotionAttached = false;
                _dotNetReference?.Dispose();
                _dotNetReference = null;
            }
        }
    }
}
