// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /// <summary>
    /// A step in a Steps component
    /// </summary>
    public partial class Step : AntDomComponentBase
    {
        private StepsStatus? _status = StepsStatus.Wait;
        private bool _isCustomStatus;
        private int _groupCurrent;

        private readonly Dictionary<string, object> _containerAttributes = new Dictionary<string, object>();

        internal bool Clickable { get; set; }

        internal bool Last { get; set; }

        internal bool ShowProcessDot { get; set; }

        internal StepsStatus? GroupStatus { get; set; }

        internal int GroupCurrentIndex
        {
            get => _groupCurrent;
            set
            {
                _groupCurrent = value;
                if (!_isCustomStatus)
                {
                    this._status = value > this.Index ? StepsStatus.Finish : value == this.Index ? GroupStatus ?? null : StepsStatus.Wait;
                }
                InvokeStateHasChanged();
            }
        }

        internal int Index { get; set; }
        internal double? Percent { get; set; }
        internal StepsSize Size { get; set; } = StepsSize.Default;
        internal RenderFragment ProgressDot { get; set; }
        internal StepsDirection Direction { get; set; } = StepsDirection.Horizontal;

        [CascadingParameter]
        public Steps Parent { get; set; }

        /// <summary>
        /// Icon of the step
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        /// To specify the status. It will be automatically set by current of Steps if not configured. Possible Values: wait, process, finish, error
        /// </summary>
        [Parameter]
        public StepsStatus Status
        {
            get => _status.GetValueOrDefault();
            set
            {
                if (_status != value)
                {
                    _status = value;
                    _isCustomStatus = true;
                    InvokeStateHasChanged();
                }
            }
        }

        /// <summary>
        /// Title of the step
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Title of the step
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Subtitle of the step
        /// </summary>
        [Parameter]
        public string Subtitle { get; set; } = string.Empty;

        /// <summary>
        /// Subtitle of the step
        /// </summary>
        [Parameter]
        public RenderFragment SubtitleTemplate { get; set; }

        /// <summary>
        /// Description of the step
        /// </summary>
        /// <default value="string.Empty" />
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Description of the step
        /// </summary>
        [Parameter]
        public RenderFragment DescriptionTemplate { get; set; }

        /// <summary>
        /// Callback executed when clicking step
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Disable click
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        private static readonly Dictionary<StepsStatus, string> _statusMap = new()
        {
            [StepsStatus.Wait] = "wait",
            [StepsStatus.Process] = "process",
            [StepsStatus.Finish] = "finish",
            [StepsStatus.Error] = "error",
        };

        protected override void OnInitialized()
        {
            Parent?.AddStep(this);

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

        internal int? GetTabIndex() => !Disabled && Clickable ? 0 : null;

        protected void SetClassMap()
        {
            const string PrefixName = "ant-steps-item";
            ClassMapper.Clear()
                .Add(PrefixName)
                .Get(() => $"{PrefixName}-{_statusMap[Status]}")
                .If($"{PrefixName}-active", () => Parent.Current == Index)
                .If($"{PrefixName}-disabled", () => Disabled)
                .If($"{PrefixName}-custom", () => !string.IsNullOrEmpty(Icon))
                .If($"ant-steps-next-error", () => GroupStatus == StepsStatus.Error && Parent.Current == Index + 1)
                .If($"{PrefixName}-rtl", () => RTL)
                ;
        }

        private void HandleClick(MouseEventArgs args)
        {
            if (Clickable && !Disabled)
            {
                Parent.NavigateTo(Index);
                if (OnClick.HasDelegate)
                {
                    OnClick.InvokeAsync(args);
                }
            }
        }
    }
}
