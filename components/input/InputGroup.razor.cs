using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class InputGroup : AntDomComponentBase
    {
        protected const string PrefixCls = "ant-input-group";
        private bool _compact;
        private string _compactStyleOverride;

        /// <summary>
        /// Content wrapped by InputGroup.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Whether use compact style
        /// </summary>
        [Parameter]
        public bool Compact
        {
            get { return _compact; }
            set 
            { 
                _compact = value;
                if (_compact)
                    _compactStyleOverride = "display: flex;";
                else
                    _compactStyleOverride = "";

            }
        }

        /// <summary>
        /// The size of InputGroup specifies the size of the included Input fields. 
        /// Available: large default small
        /// </summary>
        [Parameter]
        public string Size { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-lg", () => Size == InputSize.Large)
                .If($"{PrefixCls}-sm", () => Size == InputSize.Small)
                .If($"{PrefixCls}-compact", () => Compact)
                .If($"{PrefixCls}-rtl", () => RTL);
        }
    }
}
