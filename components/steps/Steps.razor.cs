﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Steps : AntDomComponentBase
    {
        private bool _showProgressDot;
        private RenderFragment _progressDot;
        public EventHandler Handler { get; }

        internal List<Step> _children;

        [Parameter] public int Current { get; set; }

        [Parameter] public double? Percent { get; set; }

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

        [Parameter]
        public bool ShowProgressDot
        {
            get => _showProgressDot;
            set => _showProgressDot = value;
        }

        [Parameter] public string Direction { get; set; } = "horizontal";

        [Parameter] public string LabelPlacement { get; set; } = "horizontal";

        [Parameter] public string Type { get; set; } = "default";

        [Parameter] public string Size { get; set; } = "default";

        [Parameter] public int StartIndex { get; set; } = 0;

        [Parameter] public string Status { get; set; } = "process";

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public EventCallback<int> OnChange { get; set; }

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
                .If($"{prefixName}-label-horizontal", () => Direction == "horizontal")
                .If($"{prefixName}-label-vertical", () => (_showProgressDot || LabelPlacement == "vertical") && Direction == "horizontal")
                .If($"{prefixName}-dot", () => _showProgressDot)
                .If($"{prefixName}-small", () => Size == "small")
                .If($"{prefixName}-navigation", () => Type == "navigation")
                .If($"{prefixName}-with-progress", () => Percent != null)
                .If($"{prefixName}-rtl", () => RTL)
                ;
        }
    }
}
