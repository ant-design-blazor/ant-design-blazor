using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Panel : AntDomComponentBase
    {
        #region Parameter

        /// <summary>
        /// If the panel is active or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Active { get; set; }

        /// <summary>
        /// Unique identifier for the panel
        /// </summary>
        [Parameter]
        public string Key { get; set; }

        /// <summary>
        /// If true, the panel cannot be opened or closed.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Display an arrow or not for the panel
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool ShowArrow { get; set; } = true;

        /// <summary>
        /// Extra string for the corner of the panel
        /// </summary>
        [Parameter]
        public string Extra { get; set; }

        /// <summary>
        /// Extra content for the corner of the panel. Takes priority over <see cref="Extra"/>
        /// </summary>
        [Parameter]
        public RenderFragment ExtraTemplate { get; set; }

        /// <summary>
        /// Header string for the panel
        /// </summary>
        [Parameter]
        public string Header { get; set; }

        /// <summary>
        /// Header content for the panel. Takes priority over <see cref="Header"/>
        /// </summary>
        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }

        /// <summary>
        /// Callback executed when this panel's active status changes
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnActiveChange { get; set; }

        #endregion Parameter

        /// <summary>
        /// Content for the panel.
        /// </summary>
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
