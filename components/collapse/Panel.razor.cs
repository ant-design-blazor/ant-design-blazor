using System;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Panel : AntDomComponentBase
    {
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

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        private Collapse Collapse { get; set; }

        private ElementReference _warpperRef;
        private decimal _warpperHight;
        private string _warpperStyle;

        private bool _isActive = false;
        private bool _isInactive = true;
        private bool _isHidden = true;
        private bool _isCollapseEnterPrepare;
        private bool _isCollapseEnterStart;
        private bool _isCollapseEnterActive;

        private bool _isCollapseLeavePrepare;
        private bool _isCollapseLeaveStart;
        private bool _isCollapseLeaveActive;

        private ClassMapper _contentClassMapper = new ClassMapper();

        private void SetClassMap()
        {
            ClassMapper
                .Add("ant-collapse-item")
                .If("ant-collapse-no-arrow", () => !this.ShowArrow)
                .If("ant-collapse-item-active", () => this.Active)
                .If("ant-collapse-item-disabled", () => this.Disabled);

            _contentClassMapper
                .Add("ant-collapse-content")
                .If("ant-collapse-content-active", () => _isActive)
                .If("ant-collapse-content-inactive", () => _isInactive)
                .If("ant-collapse-content-hidden", () => _isHidden)
                .If("ant-motion-collapse-enter ant-motion-collapse-enter-prepare ant-motion-collapse", () => _isCollapseEnterPrepare)
                .If("ant-motion-collapse-enter ant-motion-collapse-enter-start ant-motion-collapse", () => _isCollapseEnterStart)
                .If("ant-motion-collapse-enter ant-motion-collapse-enter-active ant-motion-collapse", () => _isCollapseEnterActive)
                .If("ant-motion-collapse-leave ant-motion-collapse-leave-prepare ant-motion-collapse", () => _isCollapseLeavePrepare)
                .If("ant-motion-collapse-leave ant-motion-collapse-leave-start ant-motion-collapse", () => _isCollapseLeaveStart)
                .If("ant-motion-collapse-leave ant-motion-collapse-leave-active ant-motion-collapse", () => _isCollapseLeaveActive)
                ;
        }

        protected override async Task OnInitializedAsync()
        {
            this.Collapse?.AddPanel(this);
            SetClassMap();
            await base.OnInitializedAsync();
            if (Active)
            {
                _isActive = true;
                _isHidden = false;
                _isInactive = false;
            }
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

                if (Active)
                {
                    if (Collapse.Animation)
                    {
                        HandleExpand();
                    }
                    else
                    {
                        _isActive = true;
                        _isInactive = false;
                        _isHidden = false;
                    }
                }
                else
                {
                    if (Collapse.Animation)
                    {
                        HandleCollapse();
                    }
                    else
                    {
                        _isActive = false;
                        _isInactive = true;
                        _isHidden = true;
                    }

                }
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

        private async Task UpdateHeight()
        {
            var rect = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _warpperRef);
            _warpperHight = rect.ScrollHeight;
        }

        private void HandleExpand()
        {
            _isActive = true;
            _isInactive = false;
            _isHidden = false;
            _isCollapseEnterPrepare = true;

            CallAfterRender(async () =>
            {
                await UpdateHeight();

                _isCollapseEnterPrepare = false;
                _isCollapseEnterStart = true;
                _warpperStyle = "height: 0px; opacity: 0;";

                CallAfterRender(async () =>
                {
                    _isCollapseEnterStart = false;
                    _isCollapseEnterActive = true;
                    StateHasChanged();
                    await Task.Delay(100);

                    _warpperStyle = $"height: {_warpperHight}px; opacity: 1;";
                    StateHasChanged();
                    await Task.Delay(450);

                    _isCollapseEnterActive = false;
                    _warpperStyle = "";
                    StateHasChanged();
                });

                StateHasChanged();
            });
        }

        private void HandleCollapse()
        {
            _isActive = false;
            _isInactive = true;
            _isCollapseLeavePrepare = true;

            CallAfterRender(async () =>
            {
                _isCollapseLeavePrepare = false;
                _isCollapseLeaveStart = true;
                _warpperStyle = $"height: {_warpperHight}px;";

                CallAfterRender(async () =>
                {
                    await Task.Delay(100);
                    _isCollapseLeaveStart = false;
                    _isCollapseLeaveActive = true;

                    _warpperStyle = "height: 0px; opacity: 0;";//still active
                    StateHasChanged();

                    await Task.Delay(450);
                    _isHidden = true; // still height 0
                    _warpperStyle = "";
                    _isCollapseLeaveActive = false;
                    StateHasChanged();
                });

                StateHasChanged();
                await Task.Yield();
            });

            StateHasChanged();
        }
    }
}
