using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public abstract class AntDomComponentBase : AntComponentBase
    {
        [Inject]
        private IComponentIdGenerator ComponentIdGenerator { get; set; }

        [Parameter]
        public string Id { get; set; }

        [CascadingParameter]
        public ConfigProvider ConfigProvider { get; set; }

        protected bool RTL => ConfigProvider?.Direction == "RTL";

        //[Parameter(CaptureUnmatchedValues = true)]
        //public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

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
            ClassMapper
                .Get(() => this.Class);
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
                this.StateHasChanged();
            }
        }

        protected virtual string GenerateStyle()
        {
            return Style;
        }

        private string _class;
        private string _style;

        public virtual Dictionary<string, object> CascadingAttributes { get; set; }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            ApplyCascadingAttributes(parameters);
            return base.SetParametersAsync(parameters);
        }

        protected void ApplyCascadingAttributes(ParameterView parameters)
        {
            if (parameters.TryGetValue("CascadingAttributes", out Dictionary<string, object> cascadingAttributes) && cascadingAttributes?.Any() == true)
            {
                var parametersDictionary = parameters.ToDictionary();
                var additionalParametersDictionary = new Dictionary<string, object>();
                foreach (var attribute in cascadingAttributes)
                {
                    if (!parametersDictionary.ContainsKey(attribute.Key))
                    {
                        additionalParametersDictionary.Add(attribute.Key, attribute.Value);
                    }
                }
                ParameterView.FromDictionary(additionalParametersDictionary).SetParameterProperties(this);
            }
        }
    }
}
