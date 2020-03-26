using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace AntBlazor
{
    public class AntStepBase : AntDomComponentBase
    {
        private string _status = "wait";
        private bool _isCustomStatus;
        private int _groupCurrent;

        protected Dictionary<string, object> ContainerAttributes = new Dictionary<string, object>();

        internal bool Clickable { get; set; }
        internal bool Last { get; set; }
        internal bool ShowProcessDot { get; set; }
        internal string GroupStatus { get; set; } = string.Empty;
        internal int GroupCurrentIndex {
            get => _groupCurrent;
            set {
                _groupCurrent = value;
                if (!_isCustomStatus)
                {
                    this._status = value > this.Index ? "finish" : value == this.Index ? GroupStatus ?? string.Empty : "wait";
                }
                SetClassMap();
            }
        }
        internal int Index { get; set; }
        internal RenderFragment? ProgressDot { get; set; }
        internal string Direction { get; set; } = "horizontal";

        [CascadingParameter] AntStepsBase Parent { get; set; }
        
        [Parameter] public string Icon { get; set; }
        [Parameter] public string Status {
            get => _status;
            set {
                _status = value;
                _isCustomStatus = true;
                SetClassMap();
            }
        }
        [Parameter] public string Title { get; set; } = string.Empty;
        [Parameter] public string Subtitle { get; set; } = string.Empty;
        [Parameter] public string Description { get; set; } = string.Empty;
        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter] public bool Disabled { get; set; }
        

        protected override void OnInitialized()
        {
            Parent.Children.Add(this);
            this.Index = Parent.Children.Count - 1;
            SetClassMap();
            if (Clickable && !Disabled)
            {
                ContainerAttributes["role"] = "button";
            }
        }

        public override void Dispose()
        {
            Parent.Children.Remove(this);
            Parent.ResetChildrenSteps();
            base.Dispose();
        }

        internal int? GetTabIndex()
        {
            if (!Disabled && Clickable) return 0;
            else return null;
        }

        protected void SetClassMap()
        {
            string prefixName = "ant-steps-item";
            ClassMapper.Clear()
                .Add(prefixName)
                .If($"{prefixName}-{Status}", () => !string.IsNullOrEmpty(Status))
                .If($"{prefixName}-active", () => Parent.Current==Index)
                .If($"{prefixName}-disabled", () => Disabled)
                .If($"{prefixName}-custom", () => !string.IsNullOrEmpty(Icon))
                .If($"ant-steps-next-error", () => GroupStatus=="error" && Parent.Current==Index+1)
                ;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
        }

        internal void MarkForCheck()
        {

        }
    }
}
