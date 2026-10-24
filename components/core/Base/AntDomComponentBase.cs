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
            if (additionalAttributes == null) return attributes;
            if (attributes == null) return additionalAttributes;
            return MergeAdditionalAttributes(additionalAttributes, attributes);
        }

        private static IReadOnlyDictionary<string, object> MergeAdditionalAttributes(
            IReadOnlyDictionary<string, object> additionalAttributes, IReadOnlyDictionary<string, object> attributes)
        {
            var additionalCount = additionalAttributes.Count;
            if (additionalCount == 0) return attributes;
            var attributeCount = attributes.Count;
            if (attributeCount == 0) return additionalAttributes;

            // Reserve for the union instead of growing for disjoint inputs or
            // reserving their sum when most keys overlap. A source comparer can
            // differ from the output comparer; the estimate only affects capacity.
            var capacity = additionalCount + attributeCount;
            if (attributes is Dictionary<string, object> dictionary)
            {
                foreach (var attribute in dictionary)
                {
                    if (additionalAttributes.ContainsKey(attribute.Key)) capacity--;
                }
            }
            else
            {
                foreach (var attribute in attributes)
                {
                    if (additionalAttributes.ContainsKey(attribute.Key)) capacity--;
                }
            }
            var merged = new Dictionary<string, object>(capacity, System.StringComparer.OrdinalIgnoreCase);
            CopyAdditionalAttributes(merged, additionalAttributes);
            CopyAdditionalAttributes(merged, attributes);
            return merged;
        }

        private static void CopyAdditionalAttributes(Dictionary<string, object> destination, IReadOnlyDictionary<string, object> source)
        {
            // The common Dictionary path uses its struct enumerator rather than
            // allocating an interface enumerator on every copy.
            if (source is Dictionary<string, object> dictionary)
            {
                foreach (var attribute in dictionary)
                {
                    destination[attribute.Key] = attribute.Value;
                }
            }
            else
            {
                foreach (var attribute in source)
                {
                    destination[attribute.Key] = attribute.Value;
                }
            }
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
