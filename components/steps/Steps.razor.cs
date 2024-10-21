// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

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
        private bool _showProgressDot;
        private RenderFragment _progressDot;

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
        /// <default value="horizontal" />
        [Parameter]
        public string Direction { get; set; } = StepsDirection.Horizontal;

        /// <summary>
        /// Place title and description horizontal or vertical
        /// </summary>
        /// <default value="horizontal" />
        [Parameter]
        public string LabelPlacement { get; set; } = StepsLabelPlacement.Horizontal;

        /// <summary>
        /// Type of steps. Possible Values: default, navigation
        /// </summary>
        /// <default value="default" />
        [Parameter]
        public string Type { get; set; } = StepsType.Default;

        /// <summary>
        /// Size of steps. Possible Values: default, small
        /// </summary>
        /// <default value="default" />
        [Parameter]
        public string Size { get; set; } = StepsSize.Default;

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
        public string Status { get; set; } = StepsStatus.Process;

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
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].Dispose();
            }

            _children.Clear();
            base.Dispose(disposing);
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
                _children[i].Direction = this.Direction;
                _children[i].Index = i + this.StartIndex;
                _children[i].GroupCurrentIndex = this.Current;
                _children[i].Percent = this.Percent;
                _children[i].Size = this.Size;
                _children[i].Last = _children.Count == i + 1;
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
                .GetIf(() => $"{prefixName}-{Direction}", () => !string.IsNullOrEmpty(Direction))
                .If($"{prefixName}-label-horizontal", () => Direction == StepsDirection.Horizontal)
                .If($"{prefixName}-label-vertical", () => (_showProgressDot || LabelPlacement == StepsLabelPlacement.Vertical) && Direction == StepsDirection.Horizontal)
                .If($"{prefixName}-dot", () => _showProgressDot)
                .If($"{prefixName}-small", () => Size == StepsSize.Small)
                .If($"{prefixName}-navigation", () => Type == StepsType.Navigation)
                .If($"{prefixName}-with-progress", () => Percent != null)
                .If($"{prefixName}-rtl", () => RTL);
        }
    }
}
