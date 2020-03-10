using AntBlazor.typography;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public abstract class AntTypographyBase : AntDomComponentBase
    {
        [Parameter]
        public bool copyable { get; set; } = false;
        [Parameter]
        public TypographyCopyableConfig copyConfig { get; set; }
        [Parameter]
        public bool delete { get; set; } = false;
        [Parameter]
        public bool disabled { get; set; } = false;
        [Parameter]
        public bool editable { get; set; } = false;
        [Parameter]
        public TypographyEditableConfig editConfig { get; set; }
        [Parameter]
        public bool ellipsis { get; set; } = false;
        [Parameter]
        public TypographyEllipsisConfig ellipsisConfig {get;set;}
        [Parameter]
        public bool mark { get; set; } = false;
        [Parameter]
        public bool underline { get; set; } = false;
        [Parameter]
        public bool strong { get; set; } = false;
        [Parameter]
        public Action onChange { get; set; }
        
        [Parameter]
        public string type { get; set; } = string.Empty;
    }

    public class TypographyCopyableConfig
    {
        public bool copyable { get; set; } = false;
        public Action onCopy { get; set; } = null;
    }

    public class TypographyEditableConfig
    {
        public bool editable { get; set; } = false;
        public Action onStart { get; set; }
        public Action<string> onChange { get; set; }
    }

    public class TypographyEllipsisConfig
    {
        public bool expandable { get; set; }
        public string suffix { get; set; } = "...";
        public int rows { get; set; }
        public Action onExpand { get; set; }
    }
}
