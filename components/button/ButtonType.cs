using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AntDesign
{
    public enum ButtonType
    {
        [Description("default")]
        Default,
        [Description("primary")]
        Primary,
        [Description("dashed")]
        Dashed,
        [Description("link")]
        Link,
        [Description("ghost")]
        Ghost,
        [Description("text")]
        Text,
    }
}
