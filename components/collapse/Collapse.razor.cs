using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Collapse : AntDomComponentBase
    {
        #region Parameter

        [Parameter]
        public bool Accordion { get; set; }

        [Parameter]
        public bool Bordered { get; set; } = true;

        [Parameter]
        public string ExpandIconPosition { get; set; } = CollapseExpandIconPosition.Left;

        [Parameter]
        public string[] DefaultActiveKey { get; set; } = Array.Empty<string>();

        [Parameter]
        public EventCallback<string[]> OnChange { get; set; }

        [Parameter]
        public string ExpandIcon { get; set; } = "right";

        [Parameter]
        public RenderFragment<bool> ExpandIconTemplate { get; set; }


        /// <summary>
        /// Whether enable the expand/collapse animation
        /// </summary>
        [Parameter]
        public bool Animation { get; set; }


        #endregion Parameter

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
