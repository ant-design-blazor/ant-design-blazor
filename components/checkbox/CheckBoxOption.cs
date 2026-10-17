// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class CheckboxOption<TValue>
    {
        private bool _disabled;

        public string Label { get; set; }

        public TValue Value { get; set; }

        public bool Checked { get; set; }

        /// <summary>Whether this option is disabled.</summary>
        public bool Disabled
        {
            get => _disabled;
            set
            {
                _disabled = value;
                HasDisabledOverride = true;
            }
        }

        /// <summary>Indicates whether Disabled was explicitly set, including to false.</summary>
        internal bool HasDisabledOverride { get; private set; }

        /// <summary>CSS class applied to the checkbox wrapper.</summary>
        public string ClassName { get; set; }

        /// <summary>Inline styles applied to the checkbox wrapper.</summary>
        public string Style { get; set; }

        /// <summary>Tooltip text applied to the checkbox input.</summary>
        public string Title { get; set; }

        /// <summary>Identifier applied to the checkbox input and its label.</summary>
        public string Id { get; set; }

        /// <summary>Whether the checkbox is required.</summary>
        public bool Required { get; set; }
    }
}
