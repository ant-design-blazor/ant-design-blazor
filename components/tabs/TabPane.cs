using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class TabPane : AntDomComponentBase
    {
        private const string PrefixCls = "ant-tabs-tab";
        private Tabs _parent;

        internal ClassMapper _classMapper = new ClassMapper();

        internal bool IsActive { get; set; }
        internal bool HasRendered { get; set; }
        internal ElementReference TabBar { get; set; }

        public TabPane()
        {
        }

        public TabPane(string key, RenderFragment tab, RenderFragment childContent)
        {
            this.Key = key;
            this.Tab = tab;
            this.ChildContent = childContent;
        }

        [CascadingParameter]
        internal Tabs Parent
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
        /// Show text in <see cref="TabPane"/>'s head
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

            _classMapper.Clear().
                Add(PrefixCls)
                .If($"{PrefixCls}-active", () => IsActive)
                .If($"{PrefixCls}-with-remove", () => Closable)
                .If($"{PrefixCls}-disabled", () => Disabled);
        }

        protected override void Dispose(bool disposing)
        {
            _parent?._panes.Remove(this);
            base.Dispose(disposing);
        }
    }
}
