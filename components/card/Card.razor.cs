// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Card : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Obsolete("Use ChildContent instead")]
        [Parameter]
        public RenderFragment Body { get; set; }

        [Parameter]
        public RenderFragment ActionTemplate { get; set; }

        [Parameter]
        public bool Bordered { get; set; } = true;

        [Parameter]
        public bool Hoverable { get; set; } = false;

        [Parameter]
        public bool Loading { get; set; } = false;

        [Parameter]
        public string BodyStyle { get; set; }

        [Parameter]
        public RenderFragment Cover { get; set; }

        [Parameter]
        public IList<RenderFragment> Actions { get; set; } = new List<RenderFragment>();

        [Parameter]
        public string Type { get; set; }

        [Parameter]
        public string Size { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        [Parameter]
        public RenderFragment Extra { get; set; }

        [Parameter]
        public RenderFragment CardTabs { get; set; }

        private bool _hasGrids;
        internal IList<CardAction> _cardActions;

        protected void SetClassMap()
        {
            this.ClassMapper
                .Add("ant-card")
                .If("ant-card-loading", () => Loading)
                .If("ant-card-bordered", () => Bordered)
                .If("ant-card-hoverable", () => Hoverable)
                .If("ant-card-small", () => Size == "small")
                .If("ant-card-contain-grid", () => _hasGrids)
                .If("ant-card-type-inner", () => Type == "inner")
                .If("ant-card-contain-tabs", () => CardTabs != null)
                ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        internal void MarkHasGrid()
        {
            _hasGrids = true;
            StateHasChanged();
        }

        internal void SetBody(RenderFragment body)
        {
            this.Body = body;
            InvokeAsync(StateHasChanged);
        }

        internal void AddCardAction(CardAction action)
        {
            _cardActions ??= new List<CardAction>();
            _cardActions.Add(action);
        }
    }
}
