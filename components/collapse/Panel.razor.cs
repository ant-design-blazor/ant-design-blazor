// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Panel : AntDomComponentBase
    {
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

        /// <summary>
        /// Content for the panel.
        /// </summary>
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
                this.Collapse?.TogglePanelState(this);
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
                        StateHasChanged();
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
                        StateHasChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Set the active state of the panel
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active)
        {
            if (!active || this.Collapse is null)
            {
                this.SetActiveInt(active);
            }
            else
            {
                this.Collapse.TogglePanelState(this);
            }
        }

        /// <summary>
        /// Toggle the active state of the panel
        /// </summary>
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
