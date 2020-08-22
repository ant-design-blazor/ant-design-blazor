using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public interface IAutoCompleteInput
    {
        //public AutoComplete AutoComplete { get; set; }

        public void SetValue(object value);
    }
}
