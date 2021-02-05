using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Panel : AntDomComponentBase
    {
        #region Parameter

        [Parameter]
        public bool Active { get; set; }

        [Parameter]
        public string Key { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool ShowArrow { get; set; } = true;

        [Parameter]
        public string Extra { get; set; }

        [Parameter]
        public RenderFragment ExtraTemplate { get; set; }

        [Parameter]
        public string Header { get; set; }

        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }

        [Parameter]
        public EventCallback<bool> OnActiveChange { get; set; }

        #endregion Parameter

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        public Collapse Collapse { get; set; }

        private void SetClassMap()
        {
            ClassMapper.Clear()
                .Add("ant-collapse-item")
                .If("ant-collapse-no-arrow", () => !this.ShowArrow)
                .If("ant-collapse-item-active", () => this.Active)
                .If("ant-collapse-item-disabled", () => this.Disabled);
        }

        protected override async Task OnInitializedAsync()
        {
            this.Collapse?.AddPanel(this);
            SetClassMap();
            await base.OnInitializedAsync();
        }

        private void OnHeaderClick()
        {
            if (!this.Disabled)
            {
                this.Collapse?.Click(this);
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.Collapse?.RemovePanel(this);
            base.Dispose(disposing);
        }

        internal void SetActiveInt(bool active)
        {
            if (this.Active != active)
            {
                this.Active = active;
                this.OnActiveChange.InvokeAsync(active);
                StateHasChanged();
            }
        }

        public void SetActive(bool active)
        {
            if (!active || this.Collapse is null)
            {
                this.SetActiveInt(active);
            }
            else
            {
                this.Collapse.Click(this);
            }
        }

        public void Toggle() => SetActive(!this.Active);
    }
}
