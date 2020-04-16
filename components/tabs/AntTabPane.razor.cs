using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntTabPane : AntDomComponentBase
    {
        private const string PrefixCls = "ant-tabs-tab";
        private AntTabs _parent;

        internal ClassMapper ClassMapper = new ClassMapper();
        internal bool IsActive { get; set; }

        [CascadingParameter]
        internal AntTabs Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (_parent == null)
                {
                    _parent = value;
                    _parent.AddTabPane(this);
                }
            }
        }

        /// <summary>
        /// Forced render of content in tabs, not lazy render after clicking on tabs
        /// </summary>
        [Parameter]
        public bool ForceRender { get; set; } = false;

        /// <summary>
        /// TabPane's key
        /// </summary>
        [Parameter]
        public string Key { get; set; }

        /// <summary>
        /// Show text in <see cref="AntTabPane"/>'s head
        /// </summary>
        [Parameter]
        public RenderFragment Tab { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Closable { get; set; } = true;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            ClassMapper.Clear().
                Add(PrefixCls)
                .If($"{PrefixCls}-active", () => IsActive)
                .If($"{PrefixCls}-disabled", () => Disabled);
        }
    }
}