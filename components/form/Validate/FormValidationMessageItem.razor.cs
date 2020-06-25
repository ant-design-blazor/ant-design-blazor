using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class FormValidationMessageItem
    {
        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }
    }
}
