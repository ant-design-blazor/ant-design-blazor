using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public interface IAutoCompleteRef
    {
        void SetInputComponent(IAutoCompleteInput input);

        Task InputFocus(FocusEventArgs e);

        Task InputInput(ChangeEventArgs args);

        Task InputKeyDown(KeyboardEventArgs args);
    }
}
