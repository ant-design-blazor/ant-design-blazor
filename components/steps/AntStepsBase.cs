using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntBlazor
{
    public class AntStepsBase:AntDomComponentBase
    {
        private bool _showProgressDot;
        private RenderFragment _progressDot;
        public EventHandler Handler { get; }

        internal List<AntStepBase> Children = new List<AntStepBase>();
        [Parameter] public int Current { get; set; }
        [Parameter] public RenderFragment ProgressDot {
            get => _progressDot;
            set {
                _progressDot = value;
                _showProgressDot = value!=null;
                ResetChildrenSteps();
            }    
        }
        [Parameter] public string Direction { get; set; } = "horizontal";
        [Parameter] public string LabelPlacement { get; set; } = "horizontal";
        [Parameter] public string Type { get; set; } = "default";
        [Parameter] public string Size { get; set; } = "default";
        [Parameter] public int StartIndex { get; set; } = 0;
        [Parameter] public string Status { get; set; } = "process";
        [Parameter] public RenderFragment ChildContent { get; set; }

        public override void Dispose()
        {
            foreach(AntStepBase step in Children)
            {
                step.Dispose();
            }
            Children.Clear();
            base.Dispose();
        }

        internal void ResetChildrenSteps()
        {
            for(int i = 0; i< Children.Count; i++)
            {
                Children[i].GroupStatus = this.Status;
                Children[i].ShowProcessDot = this._showProgressDot;
                if (this.ProgressDot !=null )
                {
                    Children[i].ProgressDot = this.ProgressDot;
                }
                Children[i].Clickable = this.Children.Count > 0; //TODO: Develop event emitter
                Children[i].Direction = this.Direction;
                Children[i].Index = i + this.StartIndex;
                Children[i].GroupCurrentIndex = this.Current;
                Children[i].Last = Children.Count == i + 1;
                Children[i].MarkForCheck();
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
                .If($"{prefixName}-{Direction}", () => !string.IsNullOrEmpty(Direction))
                .If($"{prefixName}-label-horizontal", ()=>Direction=="horizontal")
                .If($"{prefixName}-label-vertical", () => (_showProgressDot || LabelPlacement=="vertical") && Direction=="horizontal" )
                .If($"{prefixName}-dot", () => _showProgressDot)
                .If($"{prefixName}-small", () => Size == "small")
                .If($"{prefixName}-navigation", () => Direction == "navigation")
                ;
        }
    }
}
