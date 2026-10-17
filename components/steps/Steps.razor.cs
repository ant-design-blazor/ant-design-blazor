// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    /**
    <summary>
    <para>Navigation bar that guides users through the steps of a task.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>When a given task is complicated or has a certain sequence in the series of subtasks, we can decompose it into several steps to make things easier.</item>
    </list>
    </summary>
    <seealso cref="Step"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Navigation, "https://gw.alipayobjects.com/zos/antfincdn/UZYqMizXHaj/Steps.svg", Columns = 1, Title = "Steps", SubTitle = "步骤条")]
    public partial class Steps : AntDomComponentBase
    {
        private const decimal ResponsiveBreakpoint = 575m;

        private bool _showProgressDot;
        private RenderFragment _progressDot;
        private decimal? _windowWidth;
        private bool _isResponsiveVertical;

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        internal List<Step> _children;

        /// <summary>
        /// Current step index, zero based
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public int Current { get; set; }

        /// <summary>
        /// Percent display for current step
        /// </summary>
        [Parameter]
        public double? Percent { get; set; }

        /// <summary>
        /// Custom rendering for progress dot. Will also set ShowProgressDot to true if set.
        /// </summary>
        [Parameter]
        public RenderFragment ProgressDot
        {
            get => _progressDot;
            set
            {
                if (_progressDot != value)
                {
                    _progressDot = value;
                    _showProgressDot = value != null;
                    ResetChildrenSteps();
                }
            }
        }

        /// <summary>
        /// Show progress dot as opposed to the title, description, icon, etc
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool ShowProgressDot
        {
            get => _showProgressDot;
            set => _showProgressDot = value;
        }

        /// <summary>
        /// Direction of the step bar
        /// </summary>
        /// <default value="StepsDirection.Horizontal" />
        [Parameter]
        public StepsDirection Direction { get; set; } = StepsDirection.Horizontal;

        /// <summary>
        /// Automatically switches horizontal steps to vertical on narrow screens.
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Responsive { get; set; } = true;

        private StepsDirection EffectiveDirection => Responsive && Direction == StepsDirection.Horizontal && _isResponsiveVertical
            ? StepsDirection.Vertical
            : Direction;

        /// <summary>
        /// Place title and description horizontal or vertical
        /// </summary>
        /// <default value="StepsLabelPlacement.Horizontal" />
        [Parameter]
        public StepsLabelPlacement LabelPlacement { get; set; } = StepsLabelPlacement.Horizontal;

        /// <summary>
        /// Truncate step titles and descriptions when they exceed the available space.
        /// </summary>
        [Parameter]
        public bool Ellipsis { get; set; }

        /// <summary>
        /// Type of steps. Possible Values: default, navigation
        /// </summary>
        /// <default value="StepsType.Default" />
        [Parameter]
        public StepsType Type { get; set; } = StepsType.Default;

        /// <summary>
        /// Render steps with outlined styling.
        /// </summary>
        [Parameter]
        public bool Outlined { get; set; }

        /// <summary>
        /// Size of steps. Possible Values: default, small
        /// </summary>
        /// <default value="StepsSize.Default" />
        [Parameter]
        public StepsSize Size { get; set; } = StepsSize.Default;

        /// <summary>
        /// Starting step index
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public int StartIndex { get; set; } = 0;

        /// <summary>
        /// Status of the current step. Possible Values: wait, process, finish, error
        /// </summary>
        /// <default value="process" />
        [Parameter]
        public StepsStatus Status { get; set; } = StepsStatus.Process;

        /// <summary>
        /// Child content should contain Step elements
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Callback executed when step changes. Received the index of the step changing to
        /// </summary>
        [Parameter]
        public EventCallback<int> OnChange { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClassMap();
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener?.Dispose();

            for (int i = 0; _children != null && i < _children.Count; i++)
            {
                _children[i].Dispose();
            }

            _children?.Clear();
            base.Dispose(disposing);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                DomEventListener.AddShared<JsonElement>("window", "resize", OnWindowResize);
                bool? isNarrowScreen = null;
                try
                {
                    isNarrowScreen = await JsInvokeAsync<bool>(JSInteropConstants.MatchMedia, $"(max-width: {ResponsiveBreakpoint}px)");
                }
                catch (Exception exception) when (exception is JSException || exception is InvalidOperationException)
                {
                    // JS interop is unavailable during prerendering and in static renderers.
                }

                if (isNarrowScreen.HasValue)
                {
                    _windowWidth = isNarrowScreen.Value ? ResponsiveBreakpoint : ResponsiveBreakpoint + 1;
                    if (UpdateResponsiveDirection())
                    {
                        await InvokeAsync(StateHasChanged);
                    }
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task OnWindowResize(JsonElement element)
        {
            if (element.TryGetProperty("innerWidth", out var width))
            {
                _windowWidth = width.GetDecimal();
                if (UpdateResponsiveDirection())
                {
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        private bool UpdateResponsiveDirection()
        {
            var isResponsiveVertical = Responsive
                && Direction == StepsDirection.Horizontal
                && _windowWidth.HasValue
                && _windowWidth.Value <= ResponsiveBreakpoint;

            if (_isResponsiveVertical == isResponsiveVertical)
            {
                return false;
            }

            _isResponsiveVertical = isResponsiveVertical;
            SetClassMap();
            ResetChildrenSteps();
            return true;
        }

        internal void ResetChildrenSteps()
        {
            if (_children == null)
            {
                return;
            }

            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].GroupStatus = this.Status;
                _children[i].ShowProcessDot = this._showProgressDot;
                if (this._progressDot != null)
                {
                    _children[i].ProgressDot = this._progressDot;
                }
                _children[i].Clickable = OnChange.HasDelegate; //TODO: Develop event emitter
                _children[i].Direction = EffectiveDirection;
                _children[i].Index = i + this.StartIndex;
                _children[i].GroupCurrentIndex = this.Current;
                _children[i].Percent = this.Percent;
                _children[i].Size = this.Size;
                _children[i].Last = _children.Count == i + 1;
            }

            // In React Steps the rail uses the following item's status by default.
            for (int i = 0; i < _children.Count - 1; i++)
            {
                _children[i].RailStatus = _children[i + 1].Status;
            }
        }

        internal void AddStep(Step step)
        {
            this._children ??= new List<Step>();
            this._children.Add(step);
            step.Index = _children.Count - 1;

            ResetChildrenSteps();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            UpdateResponsiveDirection();
            SetClassMap();
            ResetChildrenSteps();
        }

        internal void NavigateTo(int current)
        {
            this.Current = current;
            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(current);
            }
        }

        private void SetClassMap()
        {
            string prefixName = "ant-steps";
            ClassMapper.Clear()
                .Add(prefixName)
                .If($"{prefixName}-horizontal", () => EffectiveDirection == StepsDirection.Horizontal)
                .If($"{prefixName}-vertical", () => EffectiveDirection == StepsDirection.Vertical)
                // v6 styles key title layout from title-placement classes. The legacy
                // label-placement class names don't activate the corresponding layout rules.
                .If($"{prefixName}-title-horizontal", () => !(_showProgressDot || LabelPlacement == StepsLabelPlacement.Vertical) || EffectiveDirection == StepsDirection.Vertical)
                .If($"{prefixName}-title-vertical", () => (_showProgressDot || LabelPlacement == StepsLabelPlacement.Vertical) && EffectiveDirection == StepsDirection.Horizontal)
                .If($"{prefixName}-dot", () => _showProgressDot)
                .If($"{prefixName}-small", () => Size == StepsSize.Small)
                .If($"{prefixName}-navigation", () => Type == StepsType.Navigation)
                .If($"{prefixName}-inline", () => Type == StepsType.Inline)
                .If($"{prefixName}-panel", () => Type == StepsType.Panel)
                // React Steps defaults to the filled variant. Without this class, v6 leaves
                // the icon state variables unset and the browser falls back to black icons.
                .If($"{prefixName}-filled", () => !Outlined)
                .If($"{prefixName}-outlined", () => Outlined)
                .If($"{prefixName}-ellipsis", () => Ellipsis)
                .If($"{prefixName}-with-progress", () => Percent != null)
                .If($"{prefixName}-rtl", () => RTL);
        }
    }
}
