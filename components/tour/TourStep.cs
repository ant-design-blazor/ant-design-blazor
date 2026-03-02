// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /// <summary>
    /// Tour step configuration
    /// </summary>
    public class TourStep
    {
        /// <summary>
        /// Title of the step
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description content of the step
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Custom render fragment for description
        /// </summary>
        public RenderFragment DescriptionTemplate { get; set; }

        /// <summary>
        /// Cover image or content
        /// </summary>
        public RenderFragment Cover { get; set; }

        /// <summary>
        /// Target element ref function. Use null for center placement.
        /// </summary>
        public Func<ElementReference?> Target { get; set; }

        /// <summary>
        /// Target element CSS selector string. Takes priority over <see cref="Target"/> if set.
        /// </summary>
        public string TargetSelector { get; set; }

        /// <summary>
        /// Placement of the tour panel relative to target
        /// </summary>
        public Placement Placement { get; set; } = Placement.Bottom;

        /// <summary>
        /// Whether to show arrow pointing to target
        /// </summary>
        public bool Arrow { get; set; } = true;

        /// <summary>
        /// Whether to show mask (overlay)
        /// </summary>
        public bool? Mask { get; set; }

        /// <summary>
        /// Custom mask style
        /// </summary>
        public string MaskStyle { get; set; }

        /// <summary>
        /// Type of the step (default or primary)
        /// </summary>
        public TourType Type { get; set; } = TourType.Default;

        /// <summary>
        /// Whether the step is closable
        /// </summary>
        public bool Closable { get; set; } = true;

        /// <summary>
        /// Custom properties for Previous button
        /// </summary>
        public TourButtonProps PrevButtonProps { get; set; }

        /// <summary>
        /// Custom properties for Next button
        /// </summary>
        public TourButtonProps NextButtonProps { get; set; }
    }

    /// <summary>
    /// Button properties for tour actions
    /// </summary>
    public class TourButtonProps
    {
        /// <summary>
        /// Button text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Button click handler
        /// </summary>
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Button disabled state
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Custom CSS class
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Custom inline style
        /// </summary>
        public string Style { get; set; }
    }

    /// <summary>
    /// Tour type enumeration
    /// </summary>
    public enum TourType
    {
        /// <summary>
        /// Default style
        /// </summary>
        Default,

        /// <summary>
        /// Primary style with highlighted background
        /// </summary>
        Primary
    }
}
