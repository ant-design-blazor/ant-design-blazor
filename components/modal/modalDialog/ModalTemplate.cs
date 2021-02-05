using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// ModalTemplate
    /// </summary>
    /// <typeparam name="TComponentOptions"></typeparam>
    public class ModalTemplate<TComponentOptions> : TemplateComponentBase<TComponentOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public ModalRef ModalRef { get; set; }
    }
}
