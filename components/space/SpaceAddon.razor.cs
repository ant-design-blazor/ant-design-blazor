// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>Displays a styled addon alongside compact controls.</summary>
    public partial class SpaceAddon : AntDomComponentBase
    {
        [CascadingParameter]
        internal SpaceCompactItemContext CompactItemContext { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public string Variant { get; set; } = "outlined";
        [Parameter] public string Status { get; set; }
        [Parameter] public InputSize? Size { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter(CaptureUnmatchedValues = true)]
        public System.Collections.Generic.IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClass();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClass();
        }

        private void SetClass()
        {
            var effectiveSize = Size ?? CompactItemContext?.Size;
            ClassMapper.Clear()
                .Add("ant-space-addon")
                .Add($"ant-space-addon-variant-{Variant}")
                .If("ant-space-addon-disabled", () => Disabled)
                .If("ant-space-addon-large", () => effectiveSize == InputSize.Large)
                .If("ant-space-addon-small", () => effectiveSize == InputSize.Small)
                .GetIf(() => $"ant-space-addon-status-{Status}", () => !string.IsNullOrWhiteSpace(Status))
                .If("ant-space-addon-compact-item", () => CompactItemContext != null)
                .If("ant-space-addon-compact-first-item", () => CompactItemContext?.Position is SpaceCompactItemPosition.First or SpaceCompactItemPosition.Only)
                .If("ant-space-addon-compact-last-item", () => CompactItemContext?.Position is SpaceCompactItemPosition.Last or SpaceCompactItemPosition.Only);
        }
    }
}
