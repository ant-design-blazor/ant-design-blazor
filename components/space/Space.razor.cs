// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Set components spacing.</para>

    <h2>When To Use</h2>

    <para>Avoid components clinging together and set a unified space.</para>
    </summary>
    <seealso cref="SpaceItem"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Layout, "https://gw.alipayobjects.com/zos/antfincdn/wc6%263gJ0Y8/Space.svg", Columns = 1, Title = "Space", SubTitle = "间距")]
    public partial class Space : AntDomComponentBase
    {
        /// <summary>
        /// Alignment of items - start | end | center | baseline
        /// </summary>
        [Parameter]
        public SpaceAlign Align { get; set; }

        /// <summary>
        /// Item flow direction
        /// </summary>
        /// <default value="SpaceDirection.Horizontal" />
        [Parameter]
        public SpaceDirection Direction { get; set; } = SpaceDirection.Horizontal;

        /// <summary>
        /// Size of space between items
        /// </summary>
        /// <default value="small" />
        [Parameter]
        public OneOf<SpaceSize, string, (string, string)> Size
        {
            get { return _size; }
            set
            {
                _size = value;
            }
        }

        /// <summary>
        /// Wrap items to multiple lines or not. Ignored when Direction is vertical.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Wrap { get; set; }

        /// <summary>
        /// Content displayed in the space between items
        /// </summary>
        [Parameter]
        public RenderFragment Split { get; set; }

        /// <summary>
        /// Content of space - should contain SpaceItem elements
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal int SpaceItemCount { get; set; }

        private readonly IList<SpaceItem> _items = new List<SpaceItem>();

        private bool HasAlign => Align.IsIn(SpaceAlign.Start, SpaceAlign.End, SpaceAlign.Center, SpaceAlign.Baseline);

        private const string PrefixCls = "ant-space";

        private OneOf<SpaceSize, string, (string, string)> _size = SpaceSize.Small;

        private string InnerStyle => Wrap && Direction == SpaceDirection.Horizontal ? "flex-wrap: wrap;" : "";

        private readonly static Dictionary<SpaceAlign, string> _alignMap = new()
        {
            [SpaceAlign.Start] = "start",
            [SpaceAlign.End] = "end",
            [SpaceAlign.Center] = "center",
            [SpaceAlign.Baseline] = "baseline"
        };

        private void SetClass()
        {
            ClassMapper
                .Add(PrefixCls)
                .Get(() => $"{PrefixCls}-{Direction.ToString().ToLowerInvariant()}")
                .GetIf(() => $"{PrefixCls}-align-{_alignMap[Align]}", () => HasAlign)
                .If($"{PrefixCls}-align-center", () => !HasAlign && Direction == SpaceDirection.Horizontal)
                .If($"{PrefixCls}-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            SetClass();
            base.OnInitialized();
        }
    }
}
