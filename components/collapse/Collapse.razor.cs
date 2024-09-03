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

        /// <summary>
        /// Expand icon position
        /// </summary>
        /// <default value="CollapseExpandIconPosition.Left"/>
        [Parameter]
        public string ExpandIconPosition { get; set; } = CollapseExpandIconPosition.Left;

        /// <summary>
        /// Default <see cref="Panel"/> element's <see cref="Panel.Key"/>
        /// </summary>
        [Parameter]
        public string[] DefaultActiveKey { get; set; } = Array.Empty<string>();

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
        public string ExpandIcon { get; set; } = "right";

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

        private void SetClassMap()
        {
            ClassMapper
                .Add("ant-collapse")
                .If("ant-collapse-icon-position-left", () => ExpandIconPosition == CollapseExpandIconPosition.Left)
                .If("ant-collapse-icon-position-right", () => ExpandIconPosition == CollapseExpandIconPosition.Right)
                .If("ant-collapse-borderless", () => !this.Bordered)
                .If("ant-collapse-rtl", () => RTL);
        }

        protected override async Task OnInitializedAsync()
        {
            SetClassMap();
            await base.OnInitializedAsync();
        }

        internal void AddPanel(Panel panel)
        {
            this.Items.Add(panel);
            if (panel.Key.IsIn(DefaultActiveKey))
            {
                panel.SetActiveInt(true);
            }

            StateHasChanged();
        }

        internal void RemovePanel(Panel panel)
        {
            this.Items.Remove(panel);
        }

        internal void Click(Panel panel)
        {
            if (panel == null)
            {
                return;
            }

            if (this.Accordion && !panel.Active)
            {
                this.Items.Where(item => item != panel && item.Active)
                    .ForEach(item => item.SetActiveInt(false));
            }

            panel.SetActiveInt(!panel.Active);

            var selectedKeys = this.Items.Where(x => x.Active).Select(x => x.Key).ToArray();
            OnChange.InvokeAsync(selectedKeys);

            panel.OnActiveChange.InvokeAsync(panel.Active);
        }

        /// <summary>
        /// Activate the specified panels
        /// </summary>
        /// <param name="activeKeys"></param>
        public void Activate(params string[] activeKeys)
        {
            var selectedKeys = new List<string>(activeKeys.Length);

            foreach (var item in Items)
            {
                if (item.Key.IsIn(activeKeys))
                {
                    selectedKeys.Add(item.Key);
                    item.SetActiveInt(true);
                }
                else if (this.Accordion)
                {
                    item.SetActiveInt(false);
                }
            }

            OnChange.InvokeAsync(selectedKeys.ToArray());
        }

        /// <summary>
        /// Deactivate the specified panels
        /// </summary>
        /// <param name="inactiveKeys"></param>
        public void Deactivate(params string[] inactiveKeys)
        {
            var selectedKeys = new List<string>();

            foreach (var item in Items)
            {
                if (item.Key.IsIn(inactiveKeys))
                {
                    item.SetActiveInt(false);
                }
                else if (item.Active)
                {
                    selectedKeys.Add(item.Key);
                }
            }

            OnChange.InvokeAsync(selectedKeys.ToArray());
        }
    }
}
