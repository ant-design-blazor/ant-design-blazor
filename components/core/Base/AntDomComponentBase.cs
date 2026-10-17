// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public abstract class AntDomComponentBase : AntComponentBase
    {
        /// <summary>
        /// Additional HTML attributes for the component's DOM target. See the arbitrary
        /// attributes documentation for composite components and attribute precedence.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        // Existing named attribute dictionaries remain supported. Their values take
        // precedence over captured attributes, just like dedicated component parameters.
        internal IReadOnlyDictionary<string, object> GetAdditionalAttributes(IReadOnlyDictionary<string, object> attributes)
        {
            var additionalAttributes = AdditionalAttributes;
            if (additionalAttributes == null || additionalAttributes.Count == 0) return attributes;
            if (attributes == null || attributes.Count == 0) return additionalAttributes;

            // Start with the larger input to avoid repeated growth without reserving
            // twice the space when most or all attribute names overlap.
            var capacity = System.Math.Max(additionalAttributes.Count, attributes.Count);
            var merged = new Dictionary<string, object>(capacity, System.StringComparer.OrdinalIgnoreCase);
            foreach (var attribute in additionalAttributes)
            {
                merged[attribute.Key] = attribute.Value;
            }
            foreach (var attribute in attributes)
            {
                merged[attribute.Key] = attribute.Value;
            }
            return merged;
        }

        [Inject]
        private IComponentIdGenerator ComponentIdGenerator { get; set; }

        /// <summary>
        /// ID for the component's HTML
        /// </summary>
        /// <default value="Uniquely Generated ID" />
        [Parameter]
        public string Id { get; set; }

        [CascadingParameter]
        public ConfigProvider ConfigProvider { get; set; }

        protected bool RTL => ConfigProvider?.Direction == "RTL";

        private ElementReference _ref;

        /// <summary>
        /// Returned ElementRef reference for DOM element.
        /// It would set the value into <see cref="AntComponentBase.RefBack"/> that user can get the <see cref="ElementReference"/> outside.
        /// </summary>
        internal protected virtual ElementReference Ref
        {
            get => _ref;
            set
            {
                _ref = value;
                RefBack?.Set(value);
            }
        }

        protected ClassMapper ClassMapper { get; } = new ClassMapper();

        protected AntDomComponentBase()
        {
        }

        protected override void OnInitialized()
        {
            Id ??= ComponentIdGenerator.Generate(this);
            base.OnInitialized();
        }

        /// <summary>
        /// Specifies one or more class names for an DOM element.
        /// </summary>
        [Parameter]
        public string Class
        {
            get => _class;
            set
            {
                _class = value;
                ClassMapper.OriginalClass = value;
            }
        }

        /// <summary>
        /// Specifies an inline style for a DOM element.
        /// </summary>
        [Parameter]
        public string Style
        {
            get => _style;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _style = string.Empty;
                }
                else
                {
                    var trimmed = value.Trim();
                    _style = trimmed.EndsWith(';') ? trimmed : trimmed + ";";
                }
            }
        }

        protected virtual string GenerateStyle()
        {
            return Style;
        }

        private string _class;
        private string _style;
    }
}
