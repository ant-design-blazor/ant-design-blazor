using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
using System.Collections.Generic;

namespace AntDesign
{
    public partial class Step : AntDomComponentBase
    {
        private string _status = "wait";
        private bool _isCustomStatus;
        private int _groupCurrent;

        private readonly Dictionary<string, object> _containerAttributes = new Dictionary<string, object>();

        internal bool Clickable { get; set; }
        internal bool Last { get; set; }
        internal bool ShowProcessDot { get; set; }
        internal string GroupStatus { get; set; } = string.Empty;

        internal int GroupCurrentIndex
        {
            get => _groupCurrent;
            set
            {
                _groupCurrent = value;
                if (!_isCustomStatus)
                {
                    this._status = value > this.Index ? "finish" :
                        value == this.Index ? GroupStatus ?? string.Empty : "wait";
                }

                SetClassMap();
            }
        }

        internal int Index { get; set; }
        internal RenderFragment ProgressDot { get; set; }
        internal string Direction { get; set; } = "horizontal";

        [CascadingParameter] public Steps Parent { get; set; }

        [Parameter] public string Icon { get; set; }

        [Parameter]
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                _isCustomStatus = true;
                SetClassMap();
            }
        }

        [Parameter] public OneOf<string, RenderFragment> Title { get; set; }
        [Parameter] public OneOf<string, RenderFragment> Subtitle { get; set; }
        [Parameter] public OneOf<string, RenderFragment> Description { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter] public bool Disabled { get; set; }

        protected override void OnInitialized()
        {
            Parent._children.Add(this);
            this.Index = Parent._children.Count - 1;
            SetClassMap();
            if (Clickable && !Disabled)
            {
                _containerAttributes["role"] = "button";
            }
        }

        protected override void Dispose(bool disposing)
        {
            Parent._children.Remove(this);
            Parent.ResetChildrenSteps();
            base.Dispose(disposing);
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
                .If($"{prefixName}-active", () => Parent.Current == Index)
                .If($"{prefixName}-disabled", () => Disabled)
                .If($"{prefixName}-custom", () => !string.IsNullOrEmpty(Icon))
                .If($"ant-steps-next-error", () => GroupStatus == "error" && Parent.Current == Index + 1)
                ;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
        }

        internal static void MarkForCheck()
        {
        }
    }
}
