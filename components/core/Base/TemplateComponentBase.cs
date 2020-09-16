using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public abstract class TemplateComponentBase<TComponentOptions> : AntComponentBase
    {
        /// <summary>
        /// The options that allow you to pass in templates from the outside
        /// </summary>
        [Parameter]
        public TComponentOptions Options { get; set; }
    }
}
