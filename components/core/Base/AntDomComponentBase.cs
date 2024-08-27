// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public abstract class AntDomComponentBase : AntComponentBase
    {
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
        /// </summary>
        public virtual ElementReference Ref
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
                _style = value;
                if (!string.IsNullOrWhiteSpace(_style) && !_style.EndsWith(";"))
                {
                    _style += ";";
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
