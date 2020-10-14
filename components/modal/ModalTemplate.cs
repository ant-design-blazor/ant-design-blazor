using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ModalTemplate<TComponentOptions> : TemplateComponentBase<TComponentOptions>
    {
        [Parameter]
        public ModalRef ModalRef { get; set; }


    }
}
