using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>Content and owner of a Menu submenu popup.</summary>
    public sealed class MenuPopupContext
    {
        public SubMenu Item { get; }
        public IReadOnlyList<string> Keys { get; }
        public RenderFragment Content { get; }

        internal MenuPopupContext(SubMenu item, IReadOnlyList<string> keys, RenderFragment content)
        {
            Item = item;
            Keys = keys;
            Content = content;
        }
    }
}
