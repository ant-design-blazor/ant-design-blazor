// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign.Core.Extensions
{
    public static class RenderTreeBuilderExtensions
    {
        /// <summary>
        /// Creates an element with attributes and content, automatically handling the closing.
        /// </summary>
        public static void CreateElement(this RenderTreeBuilder builder, string elementName, Action<ElementBuilder> buildElement, int sequence)
        {
            builder.OpenElement(sequence, elementName);
            buildElement?.Invoke(new ElementBuilder(builder, sequence + 1));
            builder.CloseElement();
        }

        /// <summary>
        /// Creates an element conditionally with attributes and content, automatically handling the closing.
        /// </summary>
        public static void CreateElement(this RenderTreeBuilder builder, bool condition, string elementName, Action<ElementBuilder> buildElement, int sequence)
        {
            if (condition)
            {
                builder.CreateElement(elementName, buildElement, sequence);
            }
        }

        /// <summary>
        /// Wraps content with another element conditionally, with support for child content building.
        /// </summary>
        public static RenderTreeBuilder WrapElement2(this RenderTreeBuilder builder, bool condition, string elementName, Action<RenderTreeBuilder, RenderFragment> configureWrapper, Action<RenderTreeBuilder> buildChild, int sequence)
        {
            if (condition)
            {
                builder.OpenElement(sequence++, elementName);
                
                // Create child content as a render fragment
                RenderFragment childFragment = childBuilder =>
                {
                    buildChild(childBuilder);
                };

                // Configure the wrapper and add child content
                configureWrapper(builder, childFragment);

                builder.CloseElement();
            }
            else
            {
                buildChild(builder);
            }

            return builder;
        }

        /// <summary>
        /// Helper class to build element attributes and content
        /// </summary>
        public class ElementBuilder
        {
            private readonly RenderTreeBuilder _builder;
            private readonly int _sequence;

            public ElementBuilder(RenderTreeBuilder builder, int sequence)
            {
                _builder = builder;
                _sequence = sequence;
            }

            public ElementBuilder AddAttribute(string name, object value)
            {
                _builder.AddAttribute(_sequence, name, value);
                return this;
            }

            public ElementBuilder AddAttributes(IReadOnlyDictionary<string, object> attributes)
            {
                if (attributes != null)
                {
                    foreach (var attr in attributes)
                    {
                        _builder.AddAttribute(_sequence, attr.Key, attr.Value);
                    }
                }
                return this;
            }

            public ElementBuilder AddContent(object content)
            {
                _builder.AddContent(_sequence, content);
                return this;
            }

            public ElementBuilder AddContent(RenderFragment content)
            {
                _builder.AddContent(_sequence, content);
                return this;
            }

            public ElementBuilder AddMarkupContent(string markupContent)
            {
                _builder.AddMarkupContent(_sequence, markupContent);
                return this;
            }

            public ElementBuilder AddChildElement(string elementName, Action<ElementBuilder> buildElement)
            {
                _builder.CreateElement(elementName, buildElement, _sequence);
                return this;
            }

            public ElementBuilder AddChildElement(bool condition, string elementName, Action<ElementBuilder> buildElement)
            {
                if (condition)
                {
                    _builder.CreateElement(elementName, buildElement, _sequence);
                }
                return this;
            }

            public ElementBuilder AddElementReferenceCapture(Action<ElementReference> elementReferenceCaptureAction)
            {
                _builder.AddElementReferenceCapture(_sequence, elementReferenceCaptureAction);
                return this;
            }

            public ElementBuilder AddEventStopPropagation(string eventName, bool stopPropagation)
            {
                _builder.AddEventStopPropagationAttribute(_sequence, eventName, stopPropagation);
                return this;
            }

            public ElementBuilder OpenElement(string elementName)
            {
                _builder.OpenElement(_sequence, elementName);
                return this;
            }

            public ElementBuilder CloseElement()
            {
                _builder.CloseElement();
                return this;
            }
        }

        /// <summary>
        /// Adds an element with conditional rendering and automatic closing.
        /// </summary>
        /// <param name="builder">The RenderTreeBuilder instance</param>
        /// <param name="condition">Condition to determine if the element should be rendered</param>
        /// <param name="elementName">The name of the HTML element</param>
        /// <param name="buildContent">Action to build the element's content</param>
        /// <param name="sequence">The sequence number for the frame</param>
        public static void AddElement(this RenderTreeBuilder builder, bool condition, string elementName, Action<RenderTreeBuilder> buildContent, int? sequence = null)
        {
            if (condition)
            {
                var seq = sequence ?? builder.GetHashCode();
                builder.CreateElement(elementName, b => 
                {
                    buildContent?.Invoke(builder);
                }, seq);
            }
        }

        /// <summary>
        /// Adds an element with conditional rendering, attributes, and automatic closing.
        /// </summary>
        /// <param name="builder">The RenderTreeBuilder instance</param>
        /// <param name="condition">Condition to determine if the element should be rendered</param>
        /// <param name="elementName">The name of the HTML element</param>
        /// <param name="attributes">Dictionary of attributes to add to the element</param>
        /// <param name="buildContent">Action to build the element's content</param>
        /// <param name="sequence">The sequence number for the frame</param>
        public static void AddElement(this RenderTreeBuilder builder, bool condition, string elementName, Dictionary<string, object> attributes, Action<RenderTreeBuilder> buildContent, int? sequence = null)
        {
            if (condition)
            {
                var seq = sequence ?? builder.GetHashCode();
                builder.CreateElement(elementName, b =>
                {
                    b.AddAttributes(attributes);
                    buildContent?.Invoke(builder);
                }, seq);
            }
        }

        /// <summary>
        /// Adds an element with conditional rendering based on a function.
        /// </summary>
        /// <param name="builder">The RenderTreeBuilder instance</param>
        /// <param name="conditionFunc">Function that returns whether the element should be rendered</param>
        /// <param name="elementName">The name of the HTML element</param>
        /// <param name="buildContent">Action to build the element's content</param>
        /// <param name="sequence">The sequence number for the frame</param>
        public static void AddElement(this RenderTreeBuilder builder, Func<bool> conditionFunc, string elementName, Action<RenderTreeBuilder> buildContent, int? sequence = null)
        {
            AddElement(builder, conditionFunc(), elementName, buildContent, sequence);
        }

        /// <summary>
        /// Adds content conditionally without wrapping element.
        /// </summary>
        /// <param name="builder">The RenderTreeBuilder instance</param>
        /// <param name="condition">Condition to determine if the content should be rendered</param>
        /// <param name="buildContent">Action to build the content</param>
        /// <param name="sequence">The sequence number for the frame</param>
        public static void AddContent(this RenderTreeBuilder builder, bool condition, Action<RenderTreeBuilder> buildContent, int? sequence = null)
        {
            if (condition)
            {
                buildContent?.Invoke(builder);
            }
        }

        /// <summary>
        /// Adds a component with conditional rendering.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component</typeparam>
        /// <param name="builder">The RenderTreeBuilder instance</param>
        /// <param name="condition">Condition to determine if the component should be rendered</param>
        /// <param name="parameters">Dictionary of parameters to pass to the component</param>
        /// <param name="sequence">The sequence number for the frame</param>
        public static void AddComponent<TComponent>(this RenderTreeBuilder builder, bool condition, Dictionary<string, object> parameters = null, int? sequence = null) where TComponent : IComponent
        {
            if (condition)
            {
                var seq = sequence ?? builder.GetHashCode();
                builder.OpenComponent<TComponent>(seq);

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        builder.AddAttribute(seq + 1, param.Key, param.Value);
                    }
                }

                builder.CloseComponent();
            }
        }

        /// <summary>
        /// Adds a markup string with conditional rendering.
        /// </summary>
        /// <param name="builder">The RenderTreeBuilder instance</param>
        /// <param name="condition">Condition to determine if the markup should be rendered</param>
        /// <param name="markup">The markup string to render</param>
        /// <param name="sequence">The sequence number for the frame</param>
        public static void AddMarkup(this RenderTreeBuilder builder, bool condition, string markup, int? sequence = null)
        {
            if (condition)
            {
                builder.AddMarkupContent(sequence ?? builder.GetHashCode(), markup);
            }
        }

        /// <summary>
        /// Wraps content with another element conditionally.
        /// </summary>
        /// <param name="builder">The RenderTreeBuilder instance</param>
        /// <param name="condition">Condition to determine if the wrapping should be applied</param>
        /// <param name="wrapperElementName">The name of the wrapper HTML element</param>
        /// <param name="configureWrapper">Action to configure the wrapper element and build content</param>
        /// <param name="buildContent">Action to build the content when wrapper is not needed</param>
        /// <param name="sequence">The sequence number for the frame</param>
        public static void WrapElement(this RenderTreeBuilder builder, bool condition, string wrapperElementName, Action<ElementBuilder> configureWrapper, Action<ElementBuilder> buildContent, int sequence)
        {
            if (condition)
            {
                builder.CreateElement(wrapperElementName, configureWrapper, sequence);
            }
            else
            {
                builder.CreateElement("div", buildContent, sequence);
            }
        }
    }
}
