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
        <para>Simple rectangular container.</para>

        <h2>When To Use</h2>
        
        <list type="bullet">
            <item>A card can be used to display content related to a single subject. The content can consist of multiple elements of varying types and sizes.</item>
        </list>
    </summary>
    <seealso cref="CardGrid"/>
    <seealso cref="CardMeta"/>
     */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/antfincdn/NqXt8DJhky/Card.svg", Columns = 1)]
    public partial class Card : AntDomComponentBase
    {
        /// <summary>
        /// Content for the card's body. Shown below <see cref="Body"/>
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Content for the card's body. Shown above <see cref="ChildContent"/>
        /// </summary>
        [Obsolete("Use ChildContent instead")]
        [Parameter]
        public RenderFragment Body { get; set; }

        /// <summary>
        /// Content to put in the actions section of the card. Takes priority over <see cref="Actions"/>
        /// </summary>
        [Parameter]
        public RenderFragment ActionTemplate { get; set; }

        /// <summary>
        /// Toggles rendering of the border around the card
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Bordered { get; set; } = true;

        /// <summary>
        /// Make card hoverable
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Hoverable { get; set; } = false;

        /// <summary>
        /// Shows a loading indicator while the contents of the card are being fetched
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Loading { get; set; } = false;

        /// <summary>
        /// Style string for body section
        /// </summary>
        [Parameter]
        public string BodyStyle { get; set; }

        /// <summary>
        /// Cover content for card. Displayed below header and above body
        /// </summary>
        [Parameter]
        public RenderFragment Cover { get; set; }

        /// <summary>
        /// Actions for the card
        /// </summary>
        [Parameter]
        public IList<RenderFragment> Actions { get; set; } = new List<RenderFragment>();

        /// <summary>
        /// Card style type, can be set to inner or not set
        /// </summary>
        [Parameter]
        public string Type { get; set; }

        /// <summary>
        /// Size of the card
        /// </summary>
        [Parameter]
        public string Size { get; set; }

        /// <summary>
        /// Title string for header
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Title content for header. Takes priority over <see cref="Title"/>
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Content to render in the top-right corner of the card
        /// </summary>
        [Parameter]
        public RenderFragment Extra { get; set; }

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

        internal void SetTabs(RenderFragment tabs)
        {
            this.CardTabs = tabs;
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
