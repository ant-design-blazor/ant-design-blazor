using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public abstract class TemplateComponentBase<TConfig> : AntComponentBase
    {
        /// <summary>
        /// The component Parameter object
        /// </summary>
        [Parameter]
        public TConfig Config { get; set; }
    }
}
