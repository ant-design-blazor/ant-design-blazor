using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntSteps : AntDomComponentBase
    {
        private bool _showProgressDot;
        private RenderFragment _progressDot;
        public EventHandler Handler { get; }

        internal List<AntStep> _children = new List<AntStep>();
        [Parameter] public int Current { get; set; }

        [Parameter]
        public RenderFragment ProgressDot
        {
            get => _progressDot;
            set
            {
                _progressDot = value;
                _showProgressDot = value != null;
                ResetChildrenSteps();
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
        [Parameter] public string ClassName { get; set; }
        [Parameter] public Action<int> OnChange { get; set; }

        protected override void Dispose(bool disposing)
        {
            foreach (AntStep step in _children)
            {
                step.Dispose();
            }
            _children.Clear();
            base.Dispose(disposing);
        }

        internal void ResetChildrenSteps()
        {
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].GroupStatus = this.Status;
                _children[i].ShowProcessDot = this._showProgressDot;
                //if (this.ProgressDot !=null )
                //{
                //    Children[i].ProgressDot = this.ProgressDot;
                //}
                _children[i].Clickable = OnChange != null; //TODO: Develop event emitter
                _children[i].Direction = this.Direction;
                _children[i].Index = i + this.StartIndex;
                _children[i].GroupCurrentIndex = this.Current;
                _children[i].Last = _children.Count == i + 1;
                AntStep.MarkForCheck();
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
            ResetChildrenSteps();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            SetClassMap();
            await Task.Run(() =>
            {
                ResetChildrenSteps();
            });
        }

        protected void SetClassMap()
        {
            string prefixName = "ant-steps";
            ClassMapper.Clear()
                .Add(prefixName)
                .Add(ClassName)
                .If($"{prefixName}-{Direction}", () => !string.IsNullOrEmpty(Direction))
                .If($"{prefixName}-label-horizontal", () => Direction == "horizontal")
                .If($"{prefixName}-label-vertical", () => (_showProgressDot || LabelPlacement == "vertical") && Direction == "horizontal")
                .If($"{prefixName}-dot", () => _showProgressDot)
                .If($"{prefixName}-small", () => Size == "small")
                .If($"{prefixName}-navigation", () => Type == "navigation")
                ;
        }

        internal void MarkForCheck()
        {
        }
    }
}
